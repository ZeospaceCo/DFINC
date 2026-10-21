using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Zeospace.DeskPlusMobile.Web.Approval.Ws.BE.EDM;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.EDM
{
    public class Transfer
    {
        /// <summary>
        /// 작성자   : famo23 
        /// 작성목적 : EDM 이관 
        /// 작성일자 : 2013-01-21
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool ApprovalToEDM(int _companyID, int _documentID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    #region 이관사용여부 - 무조건 호출 하고 여기서 체크해서 이관이 필요 없는 경우 리턴 한다.
                    string IsLinked = string.Empty;
                    string LinkedEtcForm = string.Empty;

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocInfo");
                    sql = string.Format(sql, _companyID.ToString(), _documentID.ToString());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

                    AppDocInfo appDocInfo = new AppDocInfo();

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            appDocInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            appDocInfo.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                            appDocInfo.UserID = dr["UserID"].ToString().ToNumber<int>();
                            appDocInfo.Subject = dr["Subject"].ToString();
                            appDocInfo.DocNumber = dr["DocNumber"].ToString();
                            appDocInfo.Number2 = dr["Number2"].ToString();
                            appDocInfo.FormID = dr["FormID"].ToString().ToNumber<int>();
                            appDocInfo.DeptBase = dr["DeptBase"].ToString();
                            appDocInfo.TitleBase = dr["TitleBase"].ToString();
                            appDocInfo.NameBase = dr["NameBase"].ToString();
                            appDocInfo.ContentHTML = dr["ContentHTML"].ToString();
                            appDocInfo.IsLinkedEtc = dr["IsLinkedEtc"].ToString();
                        }
                    }

                    // 양식정보                
                    string sql1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_FormInfo");
                    sql1 = string.Format(sql1, _companyID.ToString(), appDocInfo.FormID);
                    DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql1, "dsFormInfo", null);

                    if (ds1 != null)
                    {
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            IsLinked = dr["IsLinked"].ToString();
                            LinkedEtcForm = dr["IsLinkedEtc"].ToString();
                        }
                    }
                    #endregion

                    // 양식이관 설정이 1 이고 양식 또는 문서상 이관 폴더 설정이 있을 경우만 실행
                    if (IsLinked == "1" && (LinkedEtcForm != "" || appDocInfo.IsLinkedEtc != ""))
                    {
                        HttpContext context = HttpContext.Current;
                        #region 경로설정
                        // EDM 임시경로로 저장한다.
                        string spoolPath = "/DeskPlusMobile/FileDataEDM/" + appDocInfo.CompanyID.ToString() + "/Users/" + appDocInfo.UserID.ToString() + "/Spool/Document/";
                        //mht 경로 및 파일명 지정하기

                        string phisicalPath = context.Server.MapPath(spoolPath);

                        // Spool Folder 기존 삭제 로직에서 예외로 남아 있는 것을 방지 하기 위해 spool 을 한번 삭제 하고 시작한다.
                        if (!Directory.Exists(phisicalPath))
                        {
                            Directory.CreateDirectory(phisicalPath);
                        }
                        else
                        {
                            // 이미 있다면 Spool Folder 기존 삭제 로직에서 예외로 남아 있는 것을 방지 하기 위해 spool 안에 파일을 한번 삭제 하고 시작 한다.
                            //File.Delete(phisicalPath);
                            //Directory.Delete(phisicalPath);
                            Directory.Delete(phisicalPath, true);
                            Directory.CreateDirectory(phisicalPath);
                        }
                        #endregion

                        #region ***************************** MHT 파일 만들기 및 첨부파일 처리*****************************
                        string documentPath = phisicalPath + appDocInfo.DocNumber + ".html";
                        if (File.Exists(documentPath)) File.Delete(documentPath); // 타 첨부파일을 삭제하지 않음으로 상단에서 예외 처리 했씀

                        //mht 만들기 (documentPath 는 실제로 저장될 경로) // 현재는 MHT(html) 만.. PDF 나 DOC 구현 하면 좋을 텐데..
                        if (SaveHtmToEDMSpool(appDocInfo.ContentHTML, documentPath))
                        {
                            string attachFullPath = String.Empty;

                            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectAttList");
                            SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                            };
                            parm[0].Value = appDocInfo.CompanyID;
                            parm[1].Value = appDocInfo.DocumentID;

                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAtt", parm);

                            string oriFilePath = "";
                            string oriFileName = "";
                            string oriFileGUID = "";

                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                oriFilePath = dr["FilePath"].ToString();    //파일 경로
                                oriFileGUID = dr["FileGUID"].ToString();    //암호화된 파일명
                                oriFileName = dr["FileName"].ToString();    //원본 파일명  

                                oriFilePath = oriFilePath.Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/");

                                System.IO.File.Copy(context.Server.MapPath(oriFilePath + oriFileGUID), phisicalPath + oriFileName);
                            }
                        }

                        #endregion

                        #region document

                        string HistoryEvent = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HistoryEvent");

                        string[] EdmFolderInfo = (appDocInfo.IsLinkedEtc != "") ? appDocInfo.IsLinkedEtc.Split('$') : LinkedEtcForm.Split('$');

                        DocumentInfo docInfo = new DocumentInfo();
                        docInfo.CompanyID = appDocInfo.CompanyID;
                        docInfo.FolderID = Int32.Parse(EdmFolderInfo[0]);
                        docInfo.VersionID = 1;
                        docInfo.ClassID = 0;
                        docInfo.Name = appDocInfo.Subject;
                        docInfo.Description = "전자결재문서";
                        docInfo.Keyword = appDocInfo.Subject + ";" + appDocInfo.DocNumber; // 더 머 넣어야 되지;;
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
                        docInfo.CheckInUserID = appDocInfo.UserID;
                        docInfo.CheckInUserInfo = appDocInfo.DeptBase + "" + appDocInfo.TitleBase + "" + appDocInfo.NameBase;
                        docInfo.CheckInDescription = "";
                        docInfo.BaseDocPath = "/DeskPlusEDM/FileData/" + docInfo.CompanyID.ToString() + "/Document/" + EdmFolderInfo[0] + "/" + appDocInfo.Number2 + "/";
                        docInfo.HistoryEvent = HistoryEvent.Replace(',', '');
                        docInfo.WriteDate = DateTime.Now;
                        docInfo.WriteUserID = appDocInfo.UserID;
                        docInfo.WriteUserInfo = appDocInfo.DeptBase + "" + appDocInfo.TitleBase + "" + appDocInfo.NameBase;
                        docInfo.WriteName = appDocInfo.NameBase;
                        docInfo.ModifyDate = DateTime.Now;
                        docInfo.ModifyUserID = appDocInfo.UserID;
                        docInfo.ModifyUserInfo = appDocInfo.DeptBase + "" + appDocInfo.TitleBase + "" + appDocInfo.NameBase;
                        docInfo.OwnerID = appDocInfo.UserID;
                        docInfo.Etc1 = "";
                        docInfo.Etc2 = "";
                        docInfo.Etc3 = "";
                        docInfo.Etc4 = "";
                        docInfo.Etc5 = "";
                        docInfo.Etc6 = "";
                        docInfo.Etc7 = "";

                        int documentID = 0;
                        documentID = AddDocument(docInfo);

                        if (documentID < 1)
                        {
                            return false;
                        }

                        #endregion

                        #region Version Table 처리
                        //Version Table 처리
                        VersionInfo versionInfo = new VersionInfo();
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

                        if (AddVersion(versionInfo) != 0)
                        {
                            return false;
                        }

                        #endregion

                        #region 공동작업자
                        CoworkerInfo coworkerInfo = new CoworkerInfo();
                        coworkerInfo.CompanyID = docInfo.CompanyID;
                        coworkerInfo.DocumentID = documentID;
                        coworkerInfo.VersionID = docInfo.VersionID;
                        coworkerInfo.OwnerType = "M";
                        coworkerInfo.OwnerID = docInfo.WriteUserID;
                        coworkerInfo.Rating = "100";

                        if (AddCoworker(coworkerInfo) < 0)
                        {
                            return false;
                        }

                        #endregion

                        #region 첨부파일 처리 Process
                        List<BE.EDM.FileInfo> arAdd = new List<BE.EDM.FileInfo>();


                        //첨부파일이 이동할 폴더 /FileData/CompanyID/Document/FolderID/YYYYMM/DocumentID/
                        string targetWebPath = docInfo.BaseDocPath + documentID + "/" + docInfo.VersionID + "/";
                        //string targetPath = "/DeskPlusEDM/FileData/" + docInfo.CompanyID.ToString() + "/Document/" + EdmFolderInfo[0] + "/" + appDocInfo.Number2 + "/" + documentID + "/" + docInfo.VersionID + "/";// HttpContext.Current.Server.MapPath(targetWebPath); //물리적 경로
                        string targetPath = "/DeskPlusMobile/FileDataEDM/" + docInfo.CompanyID.ToString() + "/Document/" + EdmFolderInfo[0] + "/" + appDocInfo.Number2 + "/" + documentID + "/" + docInfo.VersionID + "/";// HttpContext.Current.Server.MapPath(targetWebPath); //물리적 경로

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

                                BE.EDM.FileInfo befileInfo = new BE.EDM.FileInfo();
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
                            Directory.Delete(phisicalPath, true);
                        }

                        if (arAdd.Count > 0)
                        {
                            if (AddFile(arAdd) < 1)
                            {
                                return false;
                            }
                        }
                        #endregion

                        // 검색엔진 처리 
                        if (SearchProcess(docInfo.CompanyID, "G", documentID, 0, "C") != 0)
                        {
                            return false;
                        }

                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ApprovalToEDM 이관연동중 에러", ex);
                return false;
            }
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
                Helper.AppException.WriteException("ApprovalToEDM.SaveHtmToEDMSpool 내용(mht) 파일생성 에러", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Document 테이블 insert
        /// 작성일자 : 2013-01-22
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public int AddDocument(DocumentInfo _be)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                         new SqlParameter("@CompanyID", SqlDbType.Int)
                                        ,new SqlParameter("@FolderID", SqlDbType.Int)
                                        ,new SqlParameter("@VersionID", SqlDbType.Int)
                                        ,new SqlParameter("@ClassID", SqlDbType.Int)
                                        ,new SqlParameter("@Name", SqlDbType.NVarChar, 255)
                                        ,new SqlParameter("@Description", SqlDbType.NText)
                                        ,new SqlParameter("@Keyword", SqlDbType.NVarChar, 255)
                                        ,new SqlParameter("@Version", SqlDbType.VarChar, 20)
                                        ,new SqlParameter("@ACLID", SqlDbType.VarChar, 4000)
                                        ,new SqlParameter("@LifeTimeCode", SqlDbType.Char, 6)
                                        ,new SqlParameter("@LifeTimeStart", SqlDbType.DateTime)
                                        ,new SqlParameter("@LifeTimeEnd", SqlDbType.DateTime)
                                        ,new SqlParameter("@ReadCount", SqlDbType.Int)
                                        ,new SqlParameter("@TotalPoint", SqlDbType.Int)
                                        ,new SqlParameter("@Recycle", SqlDbType.Char, 1)
                                        ,new SqlParameter("@RecycleUserID", SqlDbType.Int)
                                        ,new SqlParameter("@CheckOutDate", SqlDbType.DateTime)
                                        ,new SqlParameter("@CheckOutUserID", SqlDbType.Int)
                                        ,new SqlParameter("@CheckOutUserInfo", SqlDbType.NVarChar, 200)
                                        ,new SqlParameter("@CheckOutName", SqlDbType.NVarChar, 100)
                                        ,new SqlParameter("@CheckOutDescription", SqlDbType.NText)
                                        ,new SqlParameter("@CheckOutStatus", SqlDbType.Char, 1)
                                        ,new SqlParameter("@CheckOutPath", SqlDbType.NVarChar, 255)
                                        ,new SqlParameter("@CheckInPlanDate", SqlDbType.DateTime)
                                        ,new SqlParameter("@CheckInDate", SqlDbType.DateTime)
                                        ,new SqlParameter("@CheckInUserID", SqlDbType.Int)
                                        ,new SqlParameter("@CheckInUserInfo", SqlDbType.NVarChar, 200)
                                        ,new SqlParameter("@CheckInDescription", SqlDbType.NText)
                                        ,new SqlParameter("@BaseDocPath", SqlDbType.VarChar, 100)
                                        ,new SqlParameter("@HistoryEvent", SqlDbType.VarChar, 100)
                                        ,new SqlParameter("@WriteDate", SqlDbType.DateTime)
                                        ,new SqlParameter("@WriteUserID", SqlDbType.Int)
                                        ,new SqlParameter("@WriteUserInfo", SqlDbType.NVarChar, 200)
                                        ,new SqlParameter("@WriteName", SqlDbType.NVarChar, 200)
                                        ,new SqlParameter("@ModifyDate", SqlDbType.DateTime)
                                        ,new SqlParameter("@ModifyUserID", SqlDbType.Int)
                                        ,new SqlParameter("@ModifyUserInfo", SqlDbType.NVarChar, 200)
                                        ,new SqlParameter("@OwnerID", SqlDbType.Int)
                                        ,new SqlParameter("@Etc1", SqlDbType.NVarChar, 500)
                                        ,new SqlParameter("@Etc2", SqlDbType.NVarChar, 500)
                                        ,new SqlParameter("@Etc3", SqlDbType.NVarChar, 500)
                                        ,new SqlParameter("@Etc4", SqlDbType.NVarChar, 500)
                                        ,new SqlParameter("@Etc5", SqlDbType.NVarChar, 500)
                                        ,new SqlParameter("@Etc6", SqlDbType.NText)
                                        ,new SqlParameter("@Etc7", SqlDbType.NText)

                };

                parms[0].Value = _be.CompanyID;
                parms[1].Value = _be.FolderID;
                parms[2].Value = _be.VersionID;
                parms[3].Value = _be.ClassID;
                parms[4].Value = _be.Name;
                parms[5].Value = _be.Description;
                parms[6].Value = _be.Keyword;
                parms[7].Value = _be.Version;
                parms[8].Value = _be.ACLID;
                parms[9].Value = _be.LifeTimeCode;
                parms[10].Value = _be.LifeTimeStart;
                parms[11].Value = _be.LifeTimeEnd;
                parms[12].Value = _be.ReadCount;
                parms[13].Value = _be.TotalPoint;
                parms[14].Value = _be.Recycle;
                parms[15].Value = _be.RecycleUserID;
                parms[16].Value = _be.CheckInDate;
                parms[17].Value = _be.CheckOutUserID;
                parms[18].Value = _be.CheckOutUserInfo;
                parms[19].Value = _be.CheckOutName;
                parms[20].Value = _be.CheckOutDescription;
                parms[21].Value = _be.CheckOutStatus;
                parms[22].Value = _be.CheckOutPath;
                parms[23].Value = _be.CheckInPlanDate;
                parms[24].Value = _be.CheckInDate;
                parms[25].Value = _be.CheckInUserID;
                parms[26].Value = _be.CheckInUserInfo;
                parms[27].Value = _be.CheckInDescription;
                parms[28].Value = _be.BaseDocPath;
                parms[29].Value = _be.HistoryEvent;
                parms[30].Value = _be.WriteDate;
                parms[31].Value = _be.WriteUserID;
                parms[32].Value = _be.WriteUserInfo;
                parms[33].Value = _be.WriteName;
                parms[34].Value = _be.ModifyDate;
                parms[35].Value = _be.ModifyUserID;
                parms[36].Value = _be.ModifyUserInfo;
                parms[37].Value = _be.OwnerID;
                parms[38].Value = _be.Etc1;
                parms[39].Value = _be.Etc2;
                parms[40].Value = _be.Etc3;
                parms[41].Value = _be.Etc4;
                parms[42].Value = _be.Etc5;
                parms[43].Value = _be.Etc6;
                parms[44].Value = _be.Etc7;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_EDMDocument");
                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_EDMS, CommandType.StoredProcedure, proc, parms);
                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddDocument(" + _be.CompanyID.ToString() + "," + _be.FolderID + ")", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Version 테이블 insert
        /// 작성일자 : 2013-01-22
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public int AddVersion(VersionInfo _be)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@VersionID", SqlDbType.Int)
                                            ,new SqlParameter("@Version", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@VersionType", SqlDbType.Char, 1)
                                            ,new SqlParameter("@VersionDescription", SqlDbType.NText)
                                            ,new SqlParameter("@Name", SqlDbType.NVarChar, 255)
                                            ,new SqlParameter("@Description", SqlDbType.NText)
                                            ,new SqlParameter("@Keyword", SqlDbType.NVarChar, 255)
                                            ,new SqlParameter("@LifeTimeCode", SqlDbType.Char, 6)
                                            ,new SqlParameter("@LifeTimeStart", SqlDbType.DateTime)
                                            ,new SqlParameter("@LifeTimeEnd", SqlDbType.DateTime)
                                            ,new SqlParameter("@HistoryEvent", SqlDbType.VarChar, 50)
                                            ,new SqlParameter("@CheckOutDate", SqlDbType.DateTime)
                                            ,new SqlParameter("@CheckOutUserID", SqlDbType.Int)
                                            ,new SqlParameter("@CheckOutUserInfo", SqlDbType.NVarChar, 200)
                                            ,new SqlParameter("@CheckOutName", SqlDbType.NVarChar, 100)
                                            ,new SqlParameter("@CheckOutDescription", SqlDbType.NText)
                                            ,new SqlParameter("@CheckOutStatus", SqlDbType.Char, 1)
                                            ,new SqlParameter("@CheckOutPath", SqlDbType.NVarChar, 255)
                                            ,new SqlParameter("@CheckInPlanDate", SqlDbType.DateTime)
                                            ,new SqlParameter("@CheckInDate", SqlDbType.DateTime)
                                            ,new SqlParameter("@CheckInUserID", SqlDbType.Int)
                                            ,new SqlParameter("@CheckInUserInfo", SqlDbType.NVarChar, 200)
                                            ,new SqlParameter("@CheckInDescription", SqlDbType.NText)
                                            ,new SqlParameter("@WriteDate", SqlDbType.DateTime)
                                            ,new SqlParameter("@WriteUserID", SqlDbType.Int)
                                            ,new SqlParameter("@WriteUserInfo", SqlDbType.NVarChar, 200)

                };

                parms[0].Value = _be.CompanyID;
                parms[1].Value = _be.DocumentID;
                parms[2].Value = _be.VersionID;
                parms[3].Value = _be.Version;
                parms[4].Value = _be.VersionType;
                parms[5].Value = _be.VersionDescription;
                parms[6].Value = _be.Name;
                parms[7].Value = _be.Description;
                parms[8].Value = _be.Keyword;
                parms[9].Value = _be.LifeTimeCode;
                parms[10].Value = _be.LifeTimeStart;
                parms[11].Value = _be.LifeTimeEnd;
                parms[12].Value = _be.HistoryEvent;
                parms[13].Value = _be.CheckOutDate;
                parms[14].Value = _be.CheckOutUserID;
                parms[15].Value = _be.CheckOutUserInfo;
                parms[16].Value = _be.CheckOutName;
                parms[17].Value = _be.CheckOutDescription;
                parms[18].Value = _be.CheckOutStatus;
                parms[19].Value = _be.CheckOutPath;
                parms[20].Value = _be.CheckInPlanDate;
                parms[21].Value = _be.CheckInDate;
                parms[22].Value = _be.CheckInUserID;
                parms[23].Value = _be.CheckInUserInfo;
                parms[24].Value = _be.CheckInDescription;
                parms[25].Value = _be.WriteDate;
                parms[26].Value = _be.WriteUserID;
                parms[27].Value = _be.WriteUserInfo;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_EDMVersion");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_EDMS, CommandType.StoredProcedure, proc, parms);

                return 0;

            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddVersion(" + _be.CompanyID.ToString() + "," + _be.DocumentID + ")", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Coworker 테이블 insert
        /// 작성일자 : 2013-01-22
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public int AddCoworker(CoworkerInfo _be)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@VersionID", SqlDbType.Int)
                                            ,new SqlParameter("@OwnerType", SqlDbType.Char, 1)
                                            ,new SqlParameter("@OwnerID", SqlDbType.Int)
                                            ,new SqlParameter("@Rating", SqlDbType.NVarChar,5)                                            
                                         
                };

                parms[0].Value = _be.CompanyID;
                parms[1].Value = _be.DocumentID;
                parms[2].Value = _be.VersionID;
                parms[3].Value = _be.OwnerType;
                parms[4].Value = _be.OwnerID;
                parms[5].Value = _be.Rating;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_EDMCoworker");
                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_EDMS, CommandType.StoredProcedure, proc, parms);
                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddCoworker(" + _be.CompanyID.ToString() + "," + _be.DocumentID + ")", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : File 테이블 insert
        /// 작성일자 : 2013-01-22
        /// </summary>
        /// <param name="_al"></param>
        /// <returns></returns>
        public int AddFile(List<BE.EDM.FileInfo> _al)
        {
            try
            {
                int result = 0;
                foreach (BE.EDM.FileInfo _be in _al)
                {
                    SqlParameter[] parms = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@VersionID", SqlDbType.Int)
                                            ,new SqlParameter("@FilePath", SqlDbType.NVarChar, 255)
                                            ,new SqlParameter("@FileName", SqlDbType.NVarChar, 255)
                                            ,new SqlParameter("@FileType", SqlDbType.Char,4)                                            
                                            ,new SqlParameter("@FileLength", SqlDbType.BigInt)                                            
                                            ,new SqlParameter("@FileGUID", SqlDbType.VarChar,40)                                            
                                            ,new SqlParameter("@CheckOutStatus", SqlDbType.Char,1)                                            
                                         
                    };

                    parms[0].Value = _be.CompanyID;
                    parms[1].Value = _be.DocumentID;
                    parms[2].Value = _be.VersionID;
                    parms[3].Value = _be.FilePath;
                    parms[4].Value = _be.FileName;
                    parms[5].Value = _be.FileType;
                    parms[6].Value = _be.FileLength;
                    parms[7].Value = _be.FileGUID;
                    parms[8].Value = _be.CheckOutStatus;

                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_EDMFile");
                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_EDMS, CommandType.StoredProcedure, proc, parms);

                    result = Convert.ToInt32(obj);

                    if (result < 1)
                    {
                        return -1;
                    }
                }

                return result;

            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddFile()", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Search 테이블 insert
        /// 작성일자 : 2013-01-22
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_docType"></param>
        /// <param name="_documentID"></param>
        /// <param name="_userID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public int SearchProcess(int _companyID, string _docType, int _documentID, int _userID, string _gubun)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocType", SqlDbType.Char,1)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.Char, 1)                                            
                                        
                };

                parms[0].Value = _companyID;
                parms[1].Value = _docType;
                parms[2].Value = _documentID;
                parms[3].Value = _userID;
                parms[4].Value = _gubun;


                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_EDMSearch");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_EDMS, CommandType.StoredProcedure, proc, parms);

                return 0;

            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("SearchProcess(" + _companyID + "," + _documentID + ")", ex);
                return -1;
            }

        }

    }
}