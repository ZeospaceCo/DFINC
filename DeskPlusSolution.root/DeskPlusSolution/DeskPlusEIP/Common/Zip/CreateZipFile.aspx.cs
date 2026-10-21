using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;

namespace Zeospace.DeskPlus.Common.Web.Zip
{
    public partial class CreateZipFile : Base.BasePageCommon
    {
        #region 변수설정

        private string Module;
        private string ListID;//고유키값
        private string CirDocID;//회람용 id
        private string Contents;
        private string BaseFileDirectory;

        //중국어 찾기
        private Regex RegexChinese = new Regex(@"\p{IsCJKUnifiedIdeographs}");

        //mht파일명
        private const string MhtFileName = "Contents.mht";

        private string FileSpool = "/DeskPlusEIP/FileDataSpool/";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Zip.CreateZipFile).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (!System.IO.Directory.Exists(Server.MapPath(FileSpool))) Directory.CreateDirectory(Server.MapPath(FileSpool));

            if (Module.ToUpper() == "APPROVAL" && ListID.ToNumber<int>() > 0) //완결문서
            {
                CreateMHTAppDoc();
            }
            else if (Module.ToUpper() == "APPROVAL" && CirDocID.ToNumber<int>() > 0) //완결회람문서
            {
                CreateMHTAppCir();
            }
            else if (Module.ToUpper() == "APPROVALPROCESS" && ListID.ToNumber<int>() > 0) //진행문서
            {
                CreateMHTAppDocProcess();
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 중국어 정규화
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsChinese(char s)
        {
            return RegexChinese.IsMatch(s.ToString());
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
            }
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Module = GetDecryptParam("module", "");
            ListID = GetDecryptParam("listID", "");
            CirDocID = GetDecryptParam("cirDocID", ""); //회람용 id
            Contents = GetDecryptParam("contents", ""); //내용 DB의 내용이 잘못되어 있을 경우 파라미터로 처리

            //POST 방식으로 암호화처리 하지 않고 넘김(결재->게시판이관용)
            if (Module == "" && ListID == "" && CirDocID == "" && Contents == "")
            {
                Module = ((string)(Request.Params["module"] ?? "") == "") ? "" : (string)(Request.Params["module"] ?? "");
                ListID = ((string)(Request.Params["listID"] ?? "") == "") ? "" : (string)(Request.Params["listID"] ?? "");
                CirDocID = ((string)(Request.Params["cirDocID"] ?? "") == "") ? "" : (string)(Request.Params["cirDocID"] ?? "");
                Contents = ((string)(Request.Params["contents"] ?? "") == "") ? "" : (string)(Request.Params["contents"] ?? "");
            }
        }

        #region 완결결재 문서 MHT, ZIP 하기

        /// <summary>
        /// 작성자  : 심재운
        /// 결재 문서 : MHT 파일 생성하기
        /// </summary>
        private void CreateMHTAppDoc()
        {
            BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            Zeospace.DeskPlus.Approval.DAL.Document dal = new Approval.DAL.Document();
            List<DocumentInfo> al = dal.GetDocList(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            foreach (var docInfo in al)
            {
                SendMail sendMail = new SendMail();
                MailDataInfo mailDataInfo = new MailDataInfo();

                string mailServerDomain = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerDomain", "");
                string mailAddress = "Approval_" + docInfo.DocumentID + "@" + mailServerDomain;
                mailDataInfo.From = mailAddress;
                mailDataInfo.To.Add(mailAddress, mailAddress);

                if (String.IsNullOrEmpty(Contents))
                {
                    mailDataInfo.Contents = docInfo.ContentHTML;
                }
                else
                {
                    mailDataInfo.Contents = Contents;
                }

                // 윈도우 파일명에는 \/:*?<>|" 특수문자를 넣을 수 없다. shimpark
                mailDataInfo.Subject = Regex.Replace(docInfo.Subject, @"[\\/:*?<>|""]{1}", "");

                //\DeskPlus40\FileData\1\Approval\Document\201205\1
                string zipFolderPath = BaseFileDirectory + "{0}/Approval/Document/{1}/{2}/ZipFile";
                zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.DocumentID);

                //string fileName = "Content.zip";

                docInfo.DocNumber = Regex.Replace(docInfo.DocNumber, @"[\\/:*?<>|""]{1}", "");
                string fileName = docInfo.DocNumber + ".zip";

                if (System.IO.File.Exists(Server.MapPath(zipFolderPath + "/" + fileName)))
                {
                    String header = Request.Headers["User-Agent"];

                    if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
                    {
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
                    }
                    else
                    {
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
                    }

                    //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                    Response.ContentType = "multipart/form-data";
                    // 파일 다운로드 시키기
                    Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
                    Response.Flush();
                }
                else
                {
                    if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true); // 가비지 파일이 있을수 있음
                    FileHelper.CreateSubFolder(zipFolderPath);

                    //if (System.IO.File.Exists(Server.MapPath(zipFolderPath + "/" + MhtFileName))) System.IO.File.Delete(Server.MapPath(zipFolderPath + "/" + MhtFileName));

                    string docType = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBackupFile", "MHT");

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("CreateMHTAppDoc : doctype - " + docType, AppSettingType.COMMON);

                    string tmpFileName = Guid.NewGuid().ToString();
                    string sourceFileName = String.Empty;
                    string destFileName = string.Empty;
                    switch (docType.ToUpper())
                    {
                        case "IMAGE":
                            sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".jpg";
                            destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".jpg";
                            //HtmlToImage(mailDataInfo.Contents, Server.MapPath(zipFolderPath + "/"), contentName);
                            HtmlToImage(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                            if (File.Exists(sourceFileName))
                            {
                                File.Move(sourceFileName, destFileName);
                            }
                            break;

                        case "PDF":
                            sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".pdf";
                            destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".pdf";

                            HtmlToPDF(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                            if (File.Exists(sourceFileName))
                            {
                                File.Move(sourceFileName, destFileName);
                            }
                            break;

                        default:
                            sendMail.SaveMht(mailDataInfo, Server.MapPath(zipFolderPath + "/" + mailDataInfo.Subject + ".mht"), false, "", false);
                            break;
                    }

                    ZipFileCompressAppDoc(docInfo);
                }
            }
        }

        /// <summary>
        /// 완결결재  zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileCompressAppDoc(DocumentInfo docInfo)
        {
            //string fileName = "Content.zip";
            docInfo.DocNumber = Regex.Replace(docInfo.DocNumber, @"[\\/:*?<>|""]{1}", "");
            string fileName = docInfo.DocNumber + ".zip";
            string zipFolderPath = BaseFileDirectory + "{0}/Approval/Document/{1}/{2}/ZipFile";
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.DocumentID);

            Zeospace.DeskPlus.Approval.DAL.AttachFile att = new AttachFile();
            var attList = att.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, docInfo.DocumentID);

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string realFileName = "";

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명
                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
            }

            //5.zip 파일 생성하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            //잔여 첨부파일 삭제하기 (용량을 위하~~~)
            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Delete(Server.MapPath(zipFolderPath + "/" + realFileName));
            }

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 완결결재 문서 MHT, ZIP 하기

        #region 완결회람문서 MHT, ZIP 하기

        /// <summary>
        /// 작성자  : 심재운
        /// 결재 문서 : 회람문서  MHT 파일 생성하기
        /// </summary>
        private void CreateMHTAppCir()
        {
            BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            Zeospace.DeskPlus.Approval.DAL.CirculationDocument dal = new Approval.DAL.CirculationDocument();
            var al = dal.GetDocList(CurrentUserInfo.CompanyID, CirDocID.ToNumber<int>());

            foreach (var docInfo in al)
            {
                SendMail sendMail = new SendMail();
                MailDataInfo mailDataInfo = new MailDataInfo();

                string mailServerDomain = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerDomain", "");
                string mailAddress = "Approval_" + docInfo.CirculationDocumentID + "@" + mailServerDomain;
                mailDataInfo.From = mailAddress;
                mailDataInfo.To.Add(mailAddress, mailAddress);

                if (String.IsNullOrEmpty(Contents))
                {
                    mailDataInfo.Contents = docInfo.ContentHTML;
                }
                else
                {
                    mailDataInfo.Contents = Contents;
                }
                //mailDataInfo.Contents = docInfo.ContentHTML;

                // 윈도우 파일명에는 \/:*?<>|" 특수문자를 넣을 수 없다. shimpark
                mailDataInfo.Subject = Regex.Replace(docInfo.Subject, @"[\\/:*?<>|""]{1}", "");

                //\DeskPlus40\FileData\1\Approval\Document\201205\1
                string zipFolderPath = BaseFileDirectory + "{0}/Approval/Circulation/{1}/{2}/ZipFile";
                zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.CirculationDocumentID);

                //string fileName = "Content.zip";
                //string fileName = docInfo.DocNumber + ".zip";                
                string fileName = "ApprovalCir_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";

                if (System.IO.File.Exists(Server.MapPath(zipFolderPath + "/" + fileName)))
                {
                    String header = Request.Headers["User-Agent"];

                    if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
                    {
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
                    }
                    else
                    {
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
                    }

                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
                    //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                    Response.ContentType = "multipart/form-data";
                    // 파일 다운로드 시키기
                    Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
                    Response.Flush();
                }
                else
                {
                    if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true); // 가비지 파일이 있을수 있음
                    FileHelper.CreateSubFolder(zipFolderPath);

                    //if (System.IO.File.Exists(Server.MapPath(zipFolderPath + "/" + MhtFileName))) System.IO.File.Delete(Server.MapPath(zipFolderPath + "/" + MhtFileName));

                    string docType = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBackupFile", "MHT");

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("CreateMHTAppCir : doctype - " + docType, AppSettingType.COMMON);

                    string tmpFileName = Guid.NewGuid().ToString();
                    string sourceFileName = String.Empty;
                    string destFileName = string.Empty;
                    switch (docType.ToUpper())
                    {
                        case "IMAGE":
                            sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".jpg";
                            destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".jpg";
                            //HtmlToImage(mailDataInfo.Contents, Server.MapPath(zipFolderPath + "/"), contentName);
                            HtmlToImage(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                            if (File.Exists(sourceFileName))
                            {
                                File.Move(sourceFileName, destFileName);
                            }
                            break;

                        case "PDF":
                            sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".pdf";
                            destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".pdf";

                            HtmlToPDF(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                            if (File.Exists(sourceFileName))
                            {
                                File.Move(sourceFileName, destFileName);
                            }
                            break;

                        default:
                            sendMail.SaveMht(mailDataInfo, Server.MapPath(zipFolderPath + "/" + mailDataInfo.Subject + ".mht"), false, "", false);
                            break;
                    }

                    ZipFileCompressAppCir(docInfo);
                }
            }
        }

        /// <summary>
        /// 결재 문서  zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileCompressAppCir(CirculationDocumentInfo docInfo)
        {
            //string fileName = "Content.zip";
            //string fileName = docInfo.DocNumber + ".zip";
            string fileName = "ApprovalCir_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";
            string zipFolderPath = BaseFileDirectory + "{0}/Approval/Circulation/{1}/{2}/ZipFile";
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.CirculationDocumentID);

            Zeospace.DeskPlus.Approval.DAL.CirculationAttachFile att = new CirculationAttachFile();
            var attList = att.GetAttachFileLists(CurrentUserInfo.CompanyID, docInfo.CirculationDocumentID);

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string realFileName = "";

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
            }

            //5.zip 하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            //잔여 첨부파일 삭제하기 (용량을 위하~~~)
            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Delete(Server.MapPath(zipFolderPath + "/" + realFileName));
            }

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 완결회람문서 MHT, ZIP 하기

        #region 진행결재 문서 MHT, ZIP 하기

        /// <summary>
        /// 작성자  : 심재운
        /// 결재 문서 : MHT 파일 생성하기
        /// </summary>
        private void CreateMHTAppDocProcess()
        {
            BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            Zeospace.DeskPlus.Approval.DAL.DocumentProcess dal = new Approval.DAL.DocumentProcess();
            List<DocumentProcessInfo> al = dal.GetDocument(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            foreach (var docInfo in al)
            {
                SendMail sendMail = new SendMail();
                MailDataInfo mailDataInfo = new MailDataInfo();

                string mailServerDomain = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerDomain", "");
                string mailAddress = "Approval_" + docInfo.DocumentID + "@" + mailServerDomain;
                mailDataInfo.From = mailAddress;
                mailDataInfo.To.Add(mailAddress, mailAddress);
                if (String.IsNullOrEmpty(Contents))
                {
                    mailDataInfo.Contents = docInfo.ContentHTML;
                }
                else
                {
                    mailDataInfo.Contents = Contents;
                }
                //mailDataInfo.Contents = docInfo.ContentHTML;

                // 윈도우 파일명에는 \/:*?<>|" 특수문자를 넣을 수 없다. shimpark
                mailDataInfo.Subject = Regex.Replace(docInfo.Subject, @"[\\/:*?<>|""]{1}", "");

                //\DeskPlus40\FileData\1\Approval\Document\201205\1
                string zipFolderPath = BaseFileDirectory + "{0}/Approval/Document/{1}/{2}/ZipFile";
                zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.DocumentID);

                if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true);

                FileHelper.CreateSubFolder(zipFolderPath);

                //if (System.IO.File.Exists(Server.MapPath(zipFolderPath + "/" + MhtFileName))) System.IO.File.Delete(Server.MapPath(zipFolderPath + "/" + MhtFileName));

                string docType = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBackupFile", "MHT");

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("CreateMHTAppDocProcess : doctype - " + docType, AppSettingType.COMMON);

                string tmpFileName = Guid.NewGuid().ToString();
                string sourceFileName = String.Empty;
                string destFileName = string.Empty;
                switch (docType.ToUpper())
                {
                    case "IMAGE":
                        sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".jpg";
                        destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".jpg";
                        //HtmlToImage(mailDataInfo.Contents, Server.MapPath(zipFolderPath + "/"), contentName);
                        HtmlToImage(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                        if (File.Exists(sourceFileName))
                        {
                            File.Move(sourceFileName, destFileName);
                        }
                        break;

                    case "PDF":
                        sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".pdf";
                        destFileName = Server.MapPath(zipFolderPath + "/") + mailDataInfo.Subject + ".pdf";

                        HtmlToPDF(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);
                        if (File.Exists(sourceFileName))
                        {
                            File.Move(sourceFileName, destFileName);
                        }
                        break;

                    default:
                        sendMail.SaveMht(mailDataInfo, Server.MapPath(zipFolderPath + "/" + mailDataInfo.Subject + ".mht"), false, "", false);
                        break;
                }

                ZipFileCompressAppDocProcess(docInfo);
            }
        }

        /// <summary>
        /// 진행중인결재  zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileCompressAppDocProcess(DocumentProcessInfo docInfo)
        {
            string fileName = "Approval_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";
            //string fileName = docInfo.Subject + ".zip"; //제목에 파일명으로 쓸수없는 문자가 들어갔을때 오류남(\ / < > | 등등), 진행문서는 문서번호 없음
            string zipFolderPath = BaseFileDirectory + "{0}/Approval/Document/{1}/{2}/ZipFile";
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, docInfo.Number2, docInfo.DocumentID);

            Zeospace.DeskPlus.Approval.DAL.AttachFile att = new AttachFile();
            var attList = att.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, docInfo.DocumentID);

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string realFileName = "";

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
            }

            //5.zip 하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 진행결재 문서 MHT, ZIP 하기

        #region zip 모듈

        private void Compress(string source, string destination)
        {
            using (System.IO.FileStream sourceStream = System.IO.File.OpenRead(source))
            {
                using (System.IO.FileStream destinationStream = System.IO.File.OpenWrite(destination))
                {
                    using (System.IO.Compression.GZipStream compressedStream = new System.IO.Compression.GZipStream(destinationStream, System.IO.Compression.CompressionMode.Compress))
                    {
                        byte[] data = new byte[sourceStream.Length];

                        sourceStream.Read(data, 0, data.Length);
                        compressedStream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public class ZipFile
        {
            public ZipFile()
            {
                //
                // TODO: 여기에 생성자 논리를 추가합니다.
                //
                //string startFolder = @"C:\inetpub\wwwroot\conversion";
                //string outputZipPath = @"C:\inetpub\wwwroot";
                //string password = "1111";
                //Zeospace.DeskPlus.Component.WebComponent.ZipFile.ZipIt(startFolder,outputZipPath + "\\" + DateTime.Now.ToString("yyyyddmmhhmm")	+ ".exe", password);
            }

            private static Hashtable excludeDirs = new Hashtable();

            //압축하기
            public static void ZipCompress(string Path, string outPathAndZipFile, string password)
            {
                string OutPath = outPathAndZipFile;
                ArrayList ar = GenerateFileList(Path); // generate file list
                // find number of chars to remove from orginal file path
                int TrimLength = (Directory.GetParent(Path)).ToString().Length;
                TrimLength += 1; //remove '\'

                byte[] obuffer;

                using (ZipOutputStream oZipStream = new ZipOutputStream(File.Create(OutPath)))
                {
                    if (password != String.Empty) oZipStream.Password = password;
                    oZipStream.SetLevel(7); // 9 = maximum compression level;7level 안전
                    ZipEntry oZipEntry;

                    foreach (string Fil in ar) // for each file, generate a zipentry
                    {
                        if (Fil.ToUpper().IndexOf("THUMBS.DB") == -1)
                        {
                            oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                            //oZipEntry.Flags = (int)ICSharpCode.SharpZipLib.Zip.GeneralBitFlags.UnicodeText;

                            oZipStream.PutNextEntry(oZipEntry);

                            if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                            {
                                //ostream = File.OpenRead(Fil);
                                //obuffer = new byte[ostream.Length]; // byte buffer
                                //ostream.Read(obuffer, 0, obuffer.Length);
                                //oZipStream.Write(obuffer, 0, obuffer.Length);
                                //ostream.Close();
                                //Console.Write(".");

                                using (FileStream ostream = File.OpenRead(Fil))
                                {
                                    obuffer = new byte[ostream.Length];
                                    ostream.Read(obuffer, 0, obuffer.Length);
                                    oZipStream.Write(obuffer, 0, obuffer.Length);
                                }
                            }
                        }
                    }
                }
            }

            private static ArrayList GenerateFileList(string Dir)
            {
                ArrayList mid = new ArrayList();
                bool Empty = true;
                foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
                {
                    mid.Add(file);
                    Empty = false;
                }

                if (Empty)
                {
                    if (Directory.GetDirectories(Dir).Length == 0) // if directory is completely empty, add it
                    {
                        mid.Add(Dir + @"/");
                    }
                }

                foreach (string dirs in Directory.GetDirectories(Dir)) // do this recursively
                {
                    // set up the excludeDir test
                    string testDir = dirs.Substring(dirs.LastIndexOf(@"\") + 1).ToUpper();
                    if (ZipFile.excludeDirs.Contains(testDir))
                        continue;
                    foreach (object obj in GenerateFileList(dirs))
                    {
                        mid.Add(obj);
                    }
                }
                return mid; // return file list
            }

            /// <summary>
            /// 압축풀기
            /// </summary>
            /// <param name="file"></param>
            /// <param name="dir"></param>
            /// <returns></returns>
            public static bool ZipUnpack(string file, string dir)
            {
                try
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (directoryName != String.Empty)
                            Directory.CreateDirectory(dir + "\\" + directoryName);

                        if (fileName != String.Empty)
                        {
                            FileStream streamWriter = File.Create(dir + "\\" + theEntry.Name);
                            int size = 2048;
                            byte[] data = new byte[2048];

                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            streamWriter.Close();
                        }
                    }

                    s.Close();

                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion zip 모듈

        #region HtmlToImage

        protected void HtmlToImage(string _content, string _filePath, string _fileName)
        {
            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToImage browser = new Component.Converter.HtmlToImage(_content, _filePath, _fileName, resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }

        #endregion HtmlToImage

        #region HtmlToPDF

        private void HtmlToPDF(string _content, string _filePath, string _fileName)
        {
            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToPDF browser = new Component.Converter.HtmlToPDF(_content, _filePath, _fileName, "V", resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }

        #endregion HtmlToPDF

        #endregion (함수)
    }
}