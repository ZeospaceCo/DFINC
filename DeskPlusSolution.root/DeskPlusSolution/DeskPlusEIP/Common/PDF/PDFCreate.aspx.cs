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
using Zeospace.DeskPlus.Component.Converter;
using System.Web;

namespace Zeospace.DeskPlus.Common.Web.PDF
{
    public partial class PDFCreate : Base.BasePageCommon
    {
        #region 변수설정

        private string FileSpool = string.Empty;

        private string Module = string.Empty;
        private int ListID = 0;//고유키값
        private int CategoryID = 0; //게시판catID
        private string Number2 = string.Empty;
        private bool IsCreate;
        private string FileName = "Result.pdf";

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.PDF.PDFCreate).Assembly;	//어셈블리 선언

            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                if (CurrentUserInfo.CompanyID.Equals(0) || CurrentUserInfo.UserID.Equals(0) || ListID.Equals(0)) return;

                // 1. 임시폴더 설정
                FileSpool = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Spool/PrintBulk/";

                // 2. 임시폴더 초기화
                if (Directory.Exists(Server.MapPath(FileSpool)))
                {
                    FileHelper.DelFolder(FileSpool, false);
                }
                FileHelper.CreateSubFolder(FileSpool);

                // 3. 데이타처리
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            string rtnpath = "";

            if (Module.ToUpper() == "APPROVAL" && ListID > 0 && !Number2.Equals("")) //완결문서
            {
                rtnpath = ConvertApproval(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ListID, Number2, IsCreate);
            }

            if (Module.ToUpper() == "BOARD" && ListID > 0) //게시물
            {
                rtnpath = ConvertBoard(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ListID, CategoryID, IsCreate);
            }

            DAL.CreatePDF dal = new DAL.CreatePDF();

            //pdf로그
            if (rtnpath.Equals(""))
            {
                dal.Add(CurrentUserInfo.CompanyID, Module, ListID, "N", "", "", "", CurrentUserInfo.UserID);
            }
            else
            {
                dal.Add(CurrentUserInfo.CompanyID, Module, ListID, "Y", "", FileName, rtnpath, CurrentUserInfo.UserID);
            }


            #region 파일 다운로드
            if (!rtnpath.Equals(""))
            {
                if (Response.IsClientConnected)
                {
                    String header = Request.Headers["User-Agent"];

                    Response.AddHeader("Content-Disposition", "attachment;filename=PrintAll_" + DateTime.Today.ToShortDateString() + ".pdf");
                    Response.ContentType = "application/pdf";
                    // 파일 다운로드 시키기
                    Response.TransmitFile(Server.MapPath(rtnpath));
                    Response.Flush();
                    //Response.Close();
                }
                else
                {
                    // If the browser is not connected
                    // stop all response processing.                    
                    Response.End();
                }

            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "alert('" + this.GetResource("MSG_PDFError") + "');");
            }
            #endregion
        }

        /// <summary>
        /// 전자결재 - 일괄출력
        /// </summary>
        /// <param name="_companyid"></param>
        /// <param name="_userid"></param>
        /// <param name="_docid"></param>
        /// <param name="_num2"></param>
        /// <param name="_iscreate"></param>
        /// <returns></returns>
        private string ConvertApproval(int _companyid, int _userid, int _docid, string _num2, bool _iscreate)
        {
            /*
             * 0. 기존 파일이 있는지 확인
             * 
             * 1. 임시폴더 초기화 : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/
             * 
             * 2. 첨부파일 리스트 가져오기
             * 
             * 3. PDF변환 : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/
                        .xls | .xlsx
                        .doc | .docx
                        .ppt | .pptx
                        .html | .htm | .mht
                        .txt
                        .eml | .msg
                        .jpg | .gif | .bmp | .tiff | .png
                
             * 4. PDF Merge : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/Result/Result.pdf
             * 
             * /FileData/[CompanyID]/Approval/Document/[Number2]/[DocumentID]/PrintBulk/Result.pdf
             * 
             * 5. Result.pdf 파일다운로드.
             * 
             */

            if (_companyid.Equals(0) || _userid.Equals(0) || _docid.Equals(0) || _num2.Equals("")) return "";

            try
            {
                string targetfolder = "/DeskPlusEIP/FileData/" + _companyid.ToString() + "/Approval/Document/" + _num2 + "/" + _docid.ToString() + "/PrintBulk/";
                string targetfile = targetfolder + FileName;
                string targetsvr = Server.MapPath(targetfile);

                #region 0. 기존파일 처리

                // 신규로 생성코자 할때, 기존 파일 삭제.
                if (_iscreate)
                {
                    FileHelper.DelFolder(targetfolder, false);
                    FileHelper.CreateSubFolder(targetfolder);
                }

                // 기존 파일을 그대로 사용 시, 해당 파일경로 리턴.
                if (File.Exists(targetsvr) && !_iscreate)
                {
                    return targetfile;
                }

                #endregion

                #region 1. 첨부파일 리스트 가져오기

                Zeospace.DeskPlus.Approval.DAL.AttachFile dal = new AttachFile();
                //List<Zeospace.DeskPlus.Approval.BE.AttachFileInfo> list = dal.GetAttachFileLists(_companyid, _docid);
                List<Zeospace.DeskPlus.Approval.BE.AttachFileInfo> list = dal.GetAttachFileListsByPath(_companyid, _docid);

                // 임시폴더에 pdf파일 생성
                string[] arr = new string[list.Count];
                int i = 0;
                foreach (Approval.BE.AttachFileInfo info in list)
                {
                    //arr[i] = (!ConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf")) ? "" : FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf";
                    arr[i] = ApprovalConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf");
                    i++;
                }

                #endregion

                #region 3. pdf 파일 merge

                AsposePDF pdf = new AsposePDF();
                string rtnval = (pdf.MergePDF(arr, targetfile, false)) ? targetfile : "";

                #endregion

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MergePDF Error", ex);
                return "";
            }
        }

        /// <summary>
        /// 게시판 - 일괄출력
        /// </summary>
        /// <param name="_companyid"></param>
        /// <param name="_userid"></param>
        /// <param name="_docid"></param>
        /// <param name="_num2"></param>
        /// <param name="_iscreate"></param>
        /// <returns></returns>
        private string ConvertBoard(int _companyid, int _userid, int _listid, int _catid, bool _iscreate)
        {
            /*
             * 0. 기존 파일이 있는지 확인
             * 
             * 1. 임시폴더 초기화 : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/
             * 
             * 2. 첨부파일 리스트 가져오기
             * 
             * 3. PDF변환 : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/
                        .xls | .xlsx
                        .doc | .docx
                        .ppt | .pptx
                        .html | .htm | .mht
                        .txt
                        .eml | .msg
                        .jpg | .gif | .bmp | .tiff | .png
                
             * 4. PDF Merge : /DeskPlusEIP/FileDataSpool/[UserID]/PrintBulk/Result/Result.pdf
             * 
             * /FileData/[CompanyID]/Approval/Document/[Number2]/[DocumentID]/PrintBulk/Result.pdf
             * 
             * 5. Result.pdf 파일다운로드.
             * 
             */

            if (_companyid.Equals(0) || _userid.Equals(0) || _listid.Equals(0) || _catid.Equals(0)) return "";

            try
            {
                string yearMonth = FileHelper.GetYearMonth();
                string targetfolder = "/DeskPlusEIP/FileData/" + _companyid.ToString() + "/Board/" + yearMonth + "/" + _listid.ToString() + "/PrintBulk/";
                string targetfile = targetfolder + FileName;
                string targetsvr = Server.MapPath(targetfile);

                #region 0. 기존파일 처리

                // 신규로 생성코자 할때, 기존 파일 삭제.
                if (_iscreate)
                {
                    FileHelper.DelFolder(targetfolder, false);
                    FileHelper.CreateSubFolder(targetfolder);
                }

                // 기존 파일을 그대로 사용 시, 해당 파일경로 리턴.
                if (File.Exists(targetsvr) && !_iscreate)
                {
                    return targetfile;
                }

                #endregion

                #region 1. 첨부파일 리스트 가져오기

                //Zeospace.DeskPlus.Approval.DAL.AttachFile dal = new AttachFile();

                //List<Zeospace.DeskPlus.Approval.BE.AttachFileInfo> list = dal.GetAttachFileListsByPath(_companyid, _docid);

                Zeospace.DeskPlus.Board.DAL.AttachFiles dal = new Board.DAL.AttachFiles();
                List<Zeospace.DeskPlus.Board.BE.AttachFileInfo> list = dal.GetAttachFileLists(_companyid, _catid, _listid);


                // 임시폴더에 pdf파일 생성
                string[] arr = new string[list.Count];
                int i = 0;
                foreach (Board.BE.AttachFileInfo info in list)
                {
                    //arr[i] = (!ConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf")) ? "" : FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf";
                    arr[i] = BoardConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf");
                    i++;
                }

                #endregion

                #region 3. pdf 파일 merge

                AsposePDF pdf = new AsposePDF();
                string rtnval = (pdf.MergePDF(arr, targetfile, false)) ? targetfile : "";

                #endregion

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MergePDF Error", ex);
                return "";
            }
        }

        /// <summary>
        /// 파일변환 (*.* => *.pdf)
        /// </summary>
        /// <param name="_ext"></param>
        /// <param name="_info"></param>
        /// <param name="_taretfile"></param>
        /// <returns></returns>
        public string ApprovalConvertFile(Approval.BE.AttachFileInfo _info, string _taretfile)
        {
            if (_info == null || _taretfile.Equals("")) return "";

            try
            {
                string rtnval = "";

                string ext = _info.FileType.ToLower();

                // .xls / .xlsx
                if (ext.IndexOf("xls") > 0)
                {
                    AsposeExcel excel = new AsposeExcel();
                    if (excel.ExcelToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .doc / .docx
                if (ext.IndexOf("doc") > 0 || ext.IndexOf("txt") > 0)
                {
                    AsposeWord word = new AsposeWord();
                    if (word.DocToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .ppt / .pptx
                if (ext.IndexOf("ppt") > 0)
                {
                    AsposePPT ppt = new AsposePPT();
                    if (ppt.PPTToPDF(_info.FilePath + _info.FileGUID, _taretfile, false, _info.FileType.ToUpper().Replace(".", "")))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .htm / .html
                if (ext.IndexOf("htm") > 0)
                {
                    System.IO.TextReader tr = new StreamReader(Server.MapPath(_info.FilePath + _info.FileGUID));
                    string cont = tr.ReadToEnd();
                    string path = _taretfile.Substring(0, _taretfile.LastIndexOf("/") + 1);
                    string nm = _taretfile.Replace(path, "");
                    HtmlToPDF(cont, path, nm.ToUpper().Replace(".PDF", ""), "V", false);        // V or H
                    //HtmlToImage(cont, tarpath, tarname, false);
                    rtnval = _taretfile;
                }
                // .eml / .msg
                if (ext.IndexOf("eml") > 0 || ext.IndexOf("msg") > 0)
                {
                    AsposeEmail mail = new AsposeEmail();
                    if (mail.EmlToPdf(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .tiff
                if (ext.IndexOf("tiff") > 0)
                {
                    AsposeWord wordimg = new AsposeWord();
                    if (wordimg.ImageToPdf(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .jpg / .png / .gif / .bmp / .emf
                if (ext.IndexOf("jpg") > 0 || ext.IndexOf("png") > 0 || ext.IndexOf("gif") > 0 || ext.IndexOf("bmp") > 0 || ext.IndexOf("emf") > 0)
                {
                    AsposePDF pdf = new AsposePDF();
                    if (pdf.ImageToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .pdf
                if (ext.IndexOf("pdf") > 0)
                {
                    rtnval = _info.FilePath + _info.FileGUID;
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ConvertFile(" + _taretfile + ")", ex);
                return "";
            }
        }

        /// <summary>
        /// 파일변환 (*.* => *.pdf)
        /// </summary>
        /// <param name="_ext"></param>
        /// <param name="_info"></param>
        /// <param name="_taretfile"></param>
        /// <returns></returns>
        public string BoardConvertFile(Board.BE.AttachFileInfo _info, string _taretfile)
        {
            if (_info == null || _taretfile.Equals("")) return "";

            try
            {
                string rtnval = "";

                string ext = _info.FileType.ToLower();

                // .xls / .xlsx
                if (ext.IndexOf("xls") > 0)
                {
                    AsposeExcel excel = new AsposeExcel();
                    if (excel.ExcelToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .doc / .docx
                if (ext.IndexOf("doc") > 0 || ext.IndexOf("txt") > 0)
                {
                    AsposeWord word = new AsposeWord();
                    if (word.DocToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .ppt / .pptx
                if (ext.IndexOf("ppt") > 0)
                {
                    AsposePPT ppt = new AsposePPT();
                    if (ppt.PPTToPDF(_info.FilePath + _info.FileGUID, _taretfile, false, _info.FileType.ToUpper().Replace(".", "")))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .htm / .html
                if (ext.IndexOf("htm") > 0)
                {
                    System.IO.TextReader tr = new StreamReader(Server.MapPath(_info.FilePath + _info.FileGUID));
                    string cont = tr.ReadToEnd();
                    string path = _taretfile.Substring(0, _taretfile.LastIndexOf("/") + 1);
                    string nm = _taretfile.Replace(path, "");
                    HtmlToPDF(cont, path, nm.ToUpper().Replace(".PDF", ""), "V", false);        // V or H
                    //HtmlToImage(cont, tarpath, tarname, false);
                    rtnval = _taretfile;
                }
                // .eml / .msg
                if (ext.IndexOf("eml") > 0 || ext.IndexOf("msg") > 0)
                {
                    AsposeEmail mail = new AsposeEmail();
                    if (mail.EmlToPdf(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .tiff
                if (ext.IndexOf("tiff") > 0)
                {
                    AsposeWord wordimg = new AsposeWord();
                    if (wordimg.ImageToPdf(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .jpg / .png / .gif / .bmp / .emf
                if (ext.IndexOf("jpg") > 0 || ext.IndexOf("png") > 0 || ext.IndexOf("gif") > 0 || ext.IndexOf("bmp") > 0 || ext.IndexOf("emf") > 0)
                {
                    AsposePDF pdf = new AsposePDF();
                    if (pdf.ImageToPDF(_info.FilePath + _info.FileGUID, _taretfile, false))
                    {
                        rtnval = _taretfile;
                    }
                }
                // .pdf
                if (ext.IndexOf("pdf") > 0)
                {
                    rtnval = _info.FilePath + _info.FileGUID;
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ConvertFile(" + _taretfile + ")", ex);
                return "";
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Module = GetDecryptParam("module", "");
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();

            //POST 방식으로 암호화처리 하지 않고 넘김(결재->게시판이관용)
            if (Module == "" && ListID.Equals(0))
            {
                Module = ((string)(Request.Params["module"] ?? "") == "") ? "" : (string)(Request.Params["module"] ?? "");
                ListID = ((string)(Request.Params["listID"] ?? "") == "") ? 0 : (Request.Params["listID"]).ToNumber<int>();
            }

            Number2 = GetDecryptParam("num2", "");
            IsCreate = (GetDecryptParam("iscreate", "0").Equals("1")) ? true : false;

            if (Number2.Equals(""))
            {
                Number2 = ((string)(Request.Params["num2"] ?? "") == "") ? "" : (string)(Request.Params["num2"] ?? "");
            }

            CategoryID = GetDecryptParam("catID", "0").ToNumber<int>();

            if (CategoryID.Equals(0))
            {
                CategoryID = ((string)(Request.Params["catID"] ?? "") == "") ? 0 : (Request.Params["catID"]).ToNumber<int>();
            }
        }

        #region HtmlToImage

        protected void HtmlToImage(string _content, string _filePath, string _fileName, bool _isrealpath)
        {
            HttpContext context = HttpContext.Current;
            string filePath = (_isrealpath) ? _filePath : context.Server.MapPath(_filePath);

            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToImage browser = new Component.Converter.HtmlToImage(_content, filePath, _fileName, resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }

        #endregion HtmlToImage

        #region HtmlToPDF

        private void HtmlToPDF(string _content, string _filePath, string _fileName, string _rotate, bool _isrealpath)
        {
            HttpContext context = HttpContext.Current;
            string filePath = (_isrealpath) ? _filePath : context.Server.MapPath(_filePath);

            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToPDF browser = new Component.Converter.HtmlToPDF(_content, filePath, _fileName, _rotate, resultEvent);

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