using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Component.Converter;
using System.Data;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class CreatePDF
    {
        private string FileSpool = string.Empty;
        private string FileName = "Result.pdf";

        public bool PDFProcess(string _module, int _listID, string _number2, string _isCreate, int _companyID, int _userID, List<BE.PDFInfo> _list)
        {

            // 1. 임시폴더 설정            
            FileSpool = "/DeskPlusEIP/FileData/" + _companyID.ToString() + "/Users/" + _userID.ToString() + "/Spool/PrintBulk/";

            // 2. 임시폴더 초기화
            HttpContext context = HttpContext.Current;
            if (Directory.Exists(context.Server.MapPath(FileSpool)))
            {
                FileHelper.DelFolder(FileSpool, false);
            }
            FileHelper.CreateSubFolder(FileSpool);

            // 3. 데이타처리
            DAL.CreatePDF dal = new DAL.CreatePDF();
            dal.GetPDFLog(_companyID, _module, _listID);

            string rtnpath = "";
            bool IsCreate = (_isCreate.Equals("1")) ? true : false;

            string beforePath = string.Empty;

            //이전 생성된 pdf파일 경로 가져오기
            DataSet ds = dal.GetPDFLog(_companyID, _module, _listID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                beforePath = ds.Tables[0].Rows[0]["FilePath"].ToString();
            }

            if (_module.ToUpper() == "APPROVAL" && _listID > 0 && !_number2.Equals("")) //전자결재
            {
                rtnpath = ConvertApproval(_companyID, _userID, _listID, _number2, IsCreate, _list);
            }

            if (_module.ToUpper() == "BOARD" && _listID > 0) //게시판
            {
                rtnpath = ConvertBoard(_companyID, _userID, _listID, _number2, IsCreate, _list, beforePath);
            }



            //pdf로그
            if (rtnpath == "")
            {
                //rtnpath가 없을경우 error
                dal.Add(_companyID, _module, _listID, "N", "", "", "", _userID);
            }
            else
            {
                dal.Add(_companyID, _module, _listID, "Y", "", FileName, rtnpath, _userID);
            }

            return true;
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
        private string ConvertApproval(int _companyID, int _userID, int _documentID, string _number2, bool _iscreate, List<BE.PDFInfo> _list)
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

            if (_companyID.Equals(0) || _userID.Equals(0) || _documentID.Equals(0) || _number2.Equals("")) return "";

            try
            {
                HttpContext context = HttpContext.Current;
                string targetfolder = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetfolder += "{0}/Approval/Document/{1}/{2}/PrintBulk/";
                targetfolder = string.Format(targetfolder, _companyID, _number2, _documentID);


                string targetfile = targetfolder + FileName;
                string targetsvr = context.Server.MapPath(targetfile);

                #region 0. 기존파일 처리

                // 신규로 생성코자 할때, 기존 파일 삭제.
                if (_iscreate)
                {
                    if (Directory.Exists(targetfolder))
                    {
                        FileHelper.DelFolder(targetfolder, false);
                    }

                    FileHelper.CreateSubFolder(targetfolder);
                }

                // 기존 파일을 그대로 사용 시, 해당 파일경로 리턴.
                if (File.Exists(targetsvr) && !_iscreate)
                {
                    return targetfile;
                }

                #endregion

                #region 1. 첨부파일 리스트 가져오기

                // 임시폴더에 pdf파일 생성
                string[] arr = new string[_list.Count];
                int i = 0;
                foreach (BE.PDFInfo info in _list)
                {
                    arr[i] = ConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf");
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
        /// 전자결재 - 일괄출력
        /// </summary>
        /// <param name="_companyid"></param>
        /// <param name="_userid"></param>
        /// <param name="_docid"></param>
        /// <param name="_num2"></param>
        /// <param name="_iscreate"></param>
        /// <returns></returns>
        private string ConvertBoard(int _companyID, int _userID, int _listID, string _yearMonth, bool _iscreate, List<BE.PDFInfo> _list, string _beforePath)
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

            if (_companyID.Equals(0) || _userID.Equals(0) || _listID.Equals(0) || _yearMonth.Equals("")) return "";

            try
            {
                HttpContext context = HttpContext.Current;
                string targetfolder = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                targetfolder += "{0}/Board/{1}/{2}/PrintBulk/";
                targetfolder = string.Format(targetfolder, _companyID, _yearMonth, _listID);

                string targetfile = targetfolder + FileName;
                string targetsvr = context.Server.MapPath(targetfile);

                #region 0. 기존파일 처리

                // 신규로 생성코자 할때, 기존 파일 삭제.
                if (_iscreate)
                {
                    //게시판의 경우 수정시에 해당 월이 지날경우 파일의 경로가 바뀔 수 있기 때문에 DB에서 가져온 이전 데이타 경로를 삭제                    
                    string delFolder = (_beforePath == "") ? targetfolder : _beforePath;
                    if (Directory.Exists(delFolder))
                    {
                        FileHelper.DelFolder(delFolder, false);
                    }

                    FileHelper.CreateSubFolder(targetfolder);
                }

                // 기존 파일을 그대로 사용 시, 해당 파일경로 리턴.
                if (File.Exists(targetsvr) && !_iscreate)
                {
                    return _beforePath;
                }

                #endregion

                #region 1. 첨부파일 리스트 가져오기

                // 임시폴더에 pdf파일 생성
                string[] arr = new string[_list.Count];
                int i = 0;
                foreach (BE.PDFInfo info in _list)
                {
                    arr[i] = ConvertFile(info, FileSpool + "ORG_" + i.ToString().PadLeft(3, '0') + ".pdf");
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
        private string ConvertFile(BE.PDFInfo _info, string _taretfile)
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
                    HttpContext context = HttpContext.Current;
                    System.IO.TextReader tr = new StreamReader(context.Server.MapPath(_info.FilePath + _info.FileGUID));
                    string cont = tr.ReadToEnd();
                    string path = _taretfile.Substring(0, _taretfile.LastIndexOf("/") + 1);
                    string nm = _taretfile.Replace(path, "");

                    string filePath = context.Server.MapPath(_info.FilePath);
                    HtmlToPDF(cont, path, nm.ToUpper().Replace(".PDF", ""), "V", false);        // V or H
                    //HtmlToPDF(cont, path, nm.ToUpper().Replace(".PDF", ""), "V", false);        // V or H
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

        /// <summary>
        /// PDF파일삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_module"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public bool DelPdf(int _companyID, string _module, int _listID)
        {
            try
            {
                string strFilePath = string.Empty;
                string strFileName = string.Empty;

                DAL.CreatePDF dal = new DAL.CreatePDF();

                //이전 생성된 pdf파일 경로 가져오기
                DataSet ds = dal.GetPDFLog(_companyID, _module, _listID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strFilePath = ds.Tables[0].Rows[0]["FilePath"].ToString();
                }

                //실제 PDF삭제
                HttpContext context = HttpContext.Current;
                if (Directory.Exists(context.Server.MapPath(strFilePath)))
                {
                    FileHelper.DelFolder(strFilePath, false);
                }

                //PDFLog 삭제
                dal.Del(_companyID, _module, _listID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MergePDF Error", ex);
                return false;
            }

        }

    }
}
