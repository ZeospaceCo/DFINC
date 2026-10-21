using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Zeospace.DeskPlus.Approval.BLL
{
    internal class Transfer
    {
        /// <summary>
        /// 전자결재 --> EDMS 이관
        /// 전자결재상에 있는 트렌젝션에 속해 있는 엑션임, 별도의 트렌젝션 필요 없음
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <returns></returns>
        public int ApprovalToEDM(BE.DocumentProcessInfo _dpInfo)
        {
            try
            {
                #region 이관사용여부 - 무조건 호출 하고 여기서 체크해서 이관이 필요 없는 경우 리턴 한다.

                string IsLinked = string.Empty;
                string LinkedEtcForm = string.Empty;
                // 양식정보
                DAL.Form fDal = new DAL.Form();
                List<BE.FormInfo> list = fDal.GetForm(_dpInfo.CompanyID, _dpInfo.FormID);

                foreach (BE.FormInfo info in list)
                {
                    IsLinked = info.IsLinked.ToString();
                    LinkedEtcForm = info.IsLinkedEtc.ToString();
                }

                #endregion 이관사용여부 - 무조건 호출 하고 여기서 체크해서 이관이 필요 없는 경우 리턴 한다.

                // 양식이관 설정이 1 이고 양식 또는 문서상 이관 폴더 설정이 있을 경우만 실행
                if (IsLinked == "1" && (LinkedEtcForm != "" || _dpInfo.IsLinkedEtc != ""))
                {
                    HttpContext context = HttpContext.Current;

                    #region 경로설정

                    // EDM 임시경로로 저장한다.
                    string spoolPath = "/DeskPlusEDM/FileData/" + _dpInfo.CompanyID.ToString() + "/Users/" + _dpInfo.UserID.ToString() + "/ApprovalDoc/" + _dpInfo.DocumentID.ToString() + "/";
                    //mht 경로 및 파일명 지정하기

                    string phisicalPath = context.Server.MapPath(spoolPath);

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EDM Log : 4");

                    // Spool Folder 기존 삭제 로직에서 예외로 남아 있는 것을 방지 하기 위해 spool 을 한번 삭제 하고 시작한다.
                    if (!Directory.Exists(phisicalPath))
                    {
                        Directory.CreateDirectory(phisicalPath);
                    }
                    //else
                    //{
                    //    // 이미 있다면 Spool Folder 기존 삭제 로직에서 예외로 남아 있는 것을 방지 하기 위해 spool 안에 파일을 한번 삭제 하고 시작 한다.
                    //    File.Delete(phisicalPath);
                    //}

                    #endregion 경로설정

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EDM Log : 3");

                    #region ***************************** MHT 파일 만들기 및 첨부파일 처리*****************************

                    string documentPath = phisicalPath + _dpInfo.DocNumber + ".html";
                    //if (File.Exists(documentPath)) File.Delete(documentPath); // 타 첨부파일을 삭제하지 않음으로 상단에서 예외 처리 했씀

                    //mht 만들기 (documentPath 는 실제로 저장될 경로) // 현재는 MHT(html) 만.. PDF 나 DOC 구현 하면 좋을 텐데..
                    if (SaveHtmToEDMSpool(_dpInfo.ContentHTML, documentPath))
                    {
                        string attachFullPath = String.Empty;

                        Zeospace.DeskPlus.Approval.DAL.AttachFile att = new DAL.AttachFile();
                        var attList = att.GetAttachFileListsByPath(_dpInfo.CompanyID, _dpInfo.DocumentID);

                        string oriFilePath = "";
                        string oriFileName = "";
                        string oriFileGUID = "";

                        foreach (var attInfo in attList)
                        {
                            oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                            oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                            oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명
                            System.IO.File.Copy(context.Server.MapPath(oriFilePath + oriFileGUID), phisicalPath + oriFileName, true);
                        }
                    }

                    #endregion ***************************** MHT 파일 만들기 및 첨부파일 처리*****************************

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EDM Log : 2");

                    #region document

                    #region 상신자 정보 가져오기
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> orilist = null;
                    if (_dpInfo.AccountTag.IndexOf("ZQ") == 0)
                    {
                        //발신문서 상신자 정보
                        orilist = dpDal.GetDocumentUnion(_dpInfo.CompanyID, _dpInfo.GroupID);
                    }
                    else
                    {
                        orilist = dpDal.GetDocumentUnion(_dpInfo.CompanyID, _dpInfo.DocumentID);
                    }

                    foreach (BE.DocumentProcessInfo info in orilist)
                    {
                        _dpInfo.UserID = info.UserID;
                        _dpInfo.NameBase = info.NameBase;
                        _dpInfo.DeptBase = info.DeptBase;
                        _dpInfo.TitleBase = info.TitleBase;
                        _dpInfo.DutyBase = info.DutyBase;
                    }
                    #endregion

                    string[] EdmFolderInfo = (_dpInfo.IsLinkedEtc != "") ? _dpInfo.IsLinkedEtc.Split('$') : LinkedEtcForm.Split('$');

                    BE.SmartPlusEDM.DocumentInfo docInfo = new BE.SmartPlusEDM.DocumentInfo();
                    docInfo.CompanyID = _dpInfo.CompanyID;
                    docInfo.FolderID = Int32.Parse(EdmFolderInfo[0]);
                    docInfo.VersionID = 1;
                    docInfo.ClassID = 0;
                    docInfo.Name = _dpInfo.Subject;
                    docInfo.Description = "전자결재문서";
                    docInfo.Keyword = _dpInfo.Subject + ";" + _dpInfo.DocNumber; // 더 머 넣어야 되지;;
                    docInfo.Version = "1";
                    docInfo.ACLID = ""; // 기존 폴더 권한 상속

                    docInfo.LifeTimeCode = EdmFolderInfo[1];
                    docInfo.LifeTimeStart = DateTime.Now;

                    if (EdmFolderInfo[1] == "0")
                    {
                        docInfo.LifeTimeEnd = DateTime.MaxValue;
                    }
                    else if (Convert.ToInt32(EdmFolderInfo[1]) > 0)
                    {
                        docInfo.LifeTimeEnd = DateTime.Now.AddYears(Convert.ToInt32(EdmFolderInfo[1]));
                    }
                    else
                    {
                        // 이 경우는 별다른 액션 없음 - 그냥 안 넣으면 됨
                    }

                    docInfo.ReadCount = 0;
                    docInfo.TotalPoint = 0;
                    docInfo.Recycle = "N"; //처음 추가시 기본값 세팅
                    docInfo.CheckOutDate = DateTime.MaxValue;
                    docInfo.CheckOutUserID = 0;
                    docInfo.CheckOutUserInfo = "";
                    docInfo.CheckOutName = "";
                    docInfo.CheckOutDescription = "";
                    docInfo.CheckOutStatus = "N";
                    docInfo.CheckOutPath = "";
                    docInfo.CheckInPlanDate = DateTime.MaxValue;
                    docInfo.CheckInDate = DateTime.Now;
                    docInfo.CheckInUserID = _dpInfo.UserID;
                    docInfo.CheckInUserInfo = _dpInfo.DeptBase + "" + _dpInfo.TitleBase + "" + _dpInfo.NameBase;
                    docInfo.CheckInDescription = "";
                    docInfo.BaseDocPath = "/DeskPlusEDM/FileData/" + docInfo.CompanyID.ToString() + "/Document/" + EdmFolderInfo[0] + "/" + _dpInfo.Number2 + "/";
                    docInfo.HistoryEvent = "CAAFDF";
                    docInfo.WriteDate = DateTime.Now;
                    docInfo.WriteUserID = _dpInfo.UserID;
                    docInfo.WriteUserInfo = _dpInfo.DeptBase + "" + _dpInfo.TitleBase + "" + _dpInfo.NameBase;
                    docInfo.WriteName = _dpInfo.NameBase;
                    docInfo.ModifyDate = DateTime.Now;
                    docInfo.ModifyUserID = _dpInfo.UserID;
                    docInfo.ModifyUserInfo = _dpInfo.DeptBase + "" + _dpInfo.TitleBase + "" + _dpInfo.NameBase;
                    docInfo.OwnerID = _dpInfo.UserID;

                    int documentID = 0;

                    DAL.Transfer dt = new DAL.Transfer();

                    documentID = dt.AddDocument(docInfo);

                    if (documentID < 1)
                    {
                        return -1;
                    }

                    #endregion document

                    #region Version Table 처리

                    //Version Table 처리
                    BE.SmartPlusEDM.VersionInfo versionInfo = new BE.SmartPlusEDM.VersionInfo();
                    versionInfo.CompanyID = docInfo.CompanyID;
                    versionInfo.DocumentID = documentID;
                    versionInfo.VersionID = docInfo.VersionID;
                    versionInfo.Version = docInfo.Version;
                    versionInfo.VersionType = "1"; //처음 추가된 문서는 배포버전이다
                    versionInfo.VersionDescription = "";
                    versionInfo.Name = docInfo.Name;
                    versionInfo.Description = docInfo.Description;
                    versionInfo.Keyword = docInfo.Keyword;
                    versionInfo.LifeTimeCode = docInfo.LifeTimeCode;
                    versionInfo.LifeTimeStart = docInfo.LifeTimeStart;
                    versionInfo.LifeTimeEnd = docInfo.LifeTimeEnd;
                    versionInfo.HistoryEvent = docInfo.HistoryEvent;
                    versionInfo.CheckOutDate = docInfo.CheckOutDate;
                    versionInfo.CheckOutUserID = docInfo.CheckOutUserID;
                    versionInfo.CheckOutUserInfo = docInfo.CheckOutUserInfo;
                    versionInfo.CheckOutName = docInfo.CheckOutName;
                    versionInfo.CheckOutDescription = docInfo.CheckOutDescription;
                    versionInfo.CheckOutPath = docInfo.CheckOutPath;
                    versionInfo.CheckOutStatus = docInfo.CheckOutStatus;
                    versionInfo.CheckInPlanDate = docInfo.CheckInPlanDate;
                    versionInfo.CheckInDate = docInfo.CheckInDate;
                    versionInfo.CheckInUserID = docInfo.CheckInUserID;
                    versionInfo.CheckInUserInfo = docInfo.CheckInUserInfo;
                    versionInfo.CheckInDescription = docInfo.CheckInDescription;
                    versionInfo.WriteDate = docInfo.WriteDate;
                    versionInfo.WriteUserID = docInfo.WriteUserID;
                    versionInfo.WriteUserInfo = docInfo.WriteUserInfo;

                    if (dt.AddVersion(versionInfo) != 0)
                    {
                        return -1;
                    }

                    #endregion Version Table 처리

                    #region 공동작업자

                    BE.SmartPlusEDM.CoworkerInfo coworkerInfo = new BE.SmartPlusEDM.CoworkerInfo();
                    coworkerInfo.CompanyID = docInfo.CompanyID;
                    coworkerInfo.DocumentID = documentID;
                    coworkerInfo.VersionID = docInfo.VersionID;
                    coworkerInfo.OwnerType = "M";
                    coworkerInfo.OwnerID = docInfo.WriteUserID;
                    coworkerInfo.Rating = "100";

                    if (dt.AddCoworker(coworkerInfo) < 0)
                    {
                        return -1;
                    }

                    #endregion 공동작업자

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EDM Log : 1");

                    #region 첨부파일 처리 Process

                    List<BE.SmartPlusEDM.FileInfo> arAdd = new List<BE.SmartPlusEDM.FileInfo>();

                    //첨부파일이 이동할 폴더 /FileData/CompanyID/Document/FolderID/YYYYMM/DocumentID/
                    string targetWebPath = docInfo.BaseDocPath + documentID + "/" + docInfo.VersionID + "/";
                    string targetPath = "/DeskPlusEDM/FileData/" + docInfo.CompanyID.ToString() + "/Document/" + EdmFolderInfo[0] + "/" + _dpInfo.Number2 + "/" + documentID + "/" + docInfo.VersionID + "/";// HttpContext.Current.Server.MapPath(targetWebPath); //물리적 경로

                    if (Directory.Exists(phisicalPath))
                    {
                        DirectoryInfo newDir = Directory.CreateDirectory(context.Server.MapPath(targetPath));

                        string[] fileArr = Directory.GetFiles(phisicalPath);
                        string tmpFileName = "";
                        string tmpFile = "";
                        string sourceFile = "";
                        string destinationFile = "";
                        string fileGUID = "";
                        int tmpfileIdx = -1;

                        foreach (string fileName in Directory.GetFiles(phisicalPath))
                        {
                            fileGUID = System.Guid.NewGuid().ToString() + ".zeo";	//파일을 GUID형식으로저장하기 위하여 생성

                            tmpFile = fileName;
                            tmpfileIdx = tmpFile.LastIndexOf("\\");
                            tmpFileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                            sourceFile = phisicalPath + tmpFileName;

                            //destinationFile = NewFileName(targetPath, fileName);
                            if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                            destinationFile = context.Server.MapPath(targetPath) + fileGUID;

                            //File 상세 정보 얻기
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(sourceFile);

                            BE.SmartPlusEDM.FileInfo befileInfo = new BE.SmartPlusEDM.FileInfo();
                            befileInfo.CompanyID = docInfo.CompanyID;
                            befileInfo.DocumentID = documentID;
                            befileInfo.VersionID = docInfo.VersionID;
                            befileInfo.FilePath = targetWebPath;
                            befileInfo.FileName = fileInfo.Name;
                            befileInfo.FileType = fileInfo.Extension;
                            befileInfo.FileLength = fileInfo.Length;
                            befileInfo.FileGUID = fileGUID;
                            befileInfo.CheckOutStatus = "N";

                            arAdd.Add(befileInfo);

                            //Spool 폴더에서 실제 폴더로 이동
                            System.IO.File.Move(sourceFile, destinationFile);
                        }

                        // Spool Folder 삭제
                        //Directory.Delete(phisicalPath, true);
                    }

                    if (arAdd.Count > 0)
                    {
                        if (dt.AddFile(arAdd) < 1)
                        {
                            return -1;
                        }
                    }

                    // 검색엔진 처리
                    dt.SearchProcess(1, "G", documentID, docInfo.WriteUserID, "C");

                    #endregion 첨부파일 처리 Process
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Approval.BLL.Transfer.ApprovalToEDM 이관연동중 에러 :" + ex.Message);
                throw;
            }

            return 0;
        }

        /// <summary>
        /// Spool 에 html 만들기
        /// </summary>
        /// <param name="_contents"></param>
        /// <param name="_phisicalPath"></param>
        /// <returns></returns>
        public bool SaveHtmToEDMSpool(string _contents, string _phisicalPath)
        {
            try
            {
                string savePath = _phisicalPath;

                using (StreamWriter sr = File.CreateText(savePath))
                {
                    sr.WriteLine("<html>");
                    sr.WriteLine("<head>");
                    sr.WriteLine("<title></title>");
                    sr.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />");
                    sr.WriteLine("</head>");
                    sr.WriteLine("<body>");
                    sr.WriteLine(_contents);
                    sr.WriteLine("</body>");
                    sr.WriteLine("</html>");
                    sr.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Approval.BLL.Transfer.ApprovalToEDM.SaveHtmToEDMSpool 내용(mht) 파일생성 에러 :" + ex.Message);
                throw;
            }
        }
    }
}