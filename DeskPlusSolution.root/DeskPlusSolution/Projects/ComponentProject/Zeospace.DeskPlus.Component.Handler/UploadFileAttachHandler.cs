using System;
using System.Web;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Component.Handler
{
    /// <summary>
    /// UploadFileAttachHandler에 대한 요약 설명입니다.
    /// </summary>
    public class UploadFileAttachHandler : IHttpHandler
    {

        private string decryString;

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="_context">The _context.</param>
        public void ProcessRequest(HttpContext _context)
        {
            try
            {
                string path = ""; //경로
                string fileName = "";
                string overWrite = "";
                string targetPath = "";
                bool unicode = true;

                if (_context.Request["p"] != null)
                {
                    SetDecryptString(_context);
                    string oriPath = GetDecryptParam("filePath", "");

                    //string oriPath = _context.Server.UrlDecode(_context.Request["p"].ToString());
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    //잘못된 경로로 파일을 업로드 할 경우 차단.
                    if (oriPath.Length > 0)
                    {
                        if (oriPath.ToLower().IndexOf(baseFileDirectory.ToLower()) == -1)
                        {
                            HttpContext context = HttpContext.Current;
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                                "file path is problem : UploadFileAttachHandler",
                                "/DeskPlusEIP/Data/Logs/FileUpload/", "");

                            return;
                        }

                        path = _context.Server.MapPath(oriPath);
                    }
                }

                if (path.Trim().Length == 0)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                            "file path is empty",
                            "/DeskPlusEIP/Data/Logs/FileDownload/", "");

                    return;
                }


                if (!path.EndsWith("\\")) path += "\\";

                //파일 경로가 없을 경우
                if (path.Length == 0) return;

                if (_context.Request["targetPath"] != null)
                {
                    targetPath = _context.Server.MapPath(_context.Request["targetPath"].ToString());
                    if (!targetPath.EndsWith("\\")) targetPath += "\\";
                }

                if (_context.Request["fileName"] != null) fileName = _context.Request["fileName"].ToString();
                if (_context.Request["overWrite"] != null) overWrite = _context.Request["overWrite"].ToString();
                if (_context.Request["unicode"] != null) unicode = false;

                // Spool 폴더 생성
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                // 파일 생성
                fileName = CreateFile(_context, path, fileName, unicode);

                //파일명이 없다는 것은, 잘못된 확장자를 업로드 경우 차단함
                if (fileName.Length == 0) return;

                // 원하는 위치로 이동
                if (targetPath != "")
                {
                    MoveFile(path, targetPath, fileName, overWrite);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[ProcessRequest] : " + ex.Message);
            }
        }

        /// <summary>
        /// Moves the file.
        /// </summary>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <param name="_overWrite">The _over write.</param>
        private void MoveFile(string _spoolPath, string _targetPath, string _fileName, string _overWrite)
        {
            string source = _spoolPath + _fileName;
            string target = "";
            if (File.Exists(source))
            {
                DirectoryInfo targetDir = Directory.CreateDirectory(_targetPath);

                if (_overWrite == "")
                {
                    target = NewFileName(_targetPath, _fileName);
                }
                else
                {
                    target = _targetPath + _fileName;
                }


                try
                {
                    File.Copy(source, target, true);
                    File.Delete(source);
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[MoveFile]", ex);
                }
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[MoveFile] : Deleted SpoolPath : " + source);
            }
        }

        /// <summary>
        /// 파일 만들기
        /// </summary>
        /// <param name="_context">The _context.</param>
        /// <param name="_path">The _path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <param name="_unicode">if set to <c>true</c> [_unicode].</param>
        /// <returns></returns>
        public string CreateFile(HttpContext _context, string _path, string _fileName, bool _unicode)
        {
            #region 파일 만들기
            string filePath = "";
            string fileName = "";

            if (_unicode) //utf8을 지원
            {
                _context.Request.ContentEncoding = System.Text.Encoding.UTF8;
            }
            else  //unicode를 지원하지 않는 98, me 버전용
            {
                _context.Request.ContentEncoding = System.Text.Encoding.Default;
            }

            string blockExt = Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(
                Base.ApplicationFrameworks.Helper.AppSettingType.COMMON, "BlockExt", "");

            string[] blockExtStr = null;
            if (blockExt.Length > 0) blockExtStr = blockExt.Split(',');

            HttpFileCollection fileCollection = _context.Request.Files;

            for (int i = 0; i < fileCollection.Count; i++)
            {
                if (_fileName != "")
                {
                    fileName = _fileName;
                    filePath = _path + fileName;		//지정된 파일명
                }
                else
                {
                    HttpPostedFile postedFile = fileCollection.Get(0);

                    fileName = postedFile.FileName;
                    //fileName = fileName.Replace("%u7E", "~"); // UrlDecode 시 ~ 는 Decode를 못함 --; 다국어에서 %u7E 로 
                    fileName = fileName.Replace("%u7E%", "~%"); // ByFreeman 20130315
                    fileName = fileName.Replace("%u7E.", "~."); // 
                    fileName = _context.Server.UrlDecode(fileName);//넘겨주는 실제 파일명                    
                    filePath = NewFileName(_path, fileName);  //서버에는 같은 파일명이 있을수 있으니 중복 확인을 해서 유니크한 파일명을 얻어온다
                    fileName = Path.GetFileName(filePath); //유니크파일명을 새롭게 가져온다
                }

                bool response = false;
                if (blockExtStr != null)
                {
                    foreach (string ext in blockExtStr)
                    {
                        int pos = fileName.LastIndexOf(".");
                        if (pos > -1)
                        {
                            string fileExt = fileName.Substring(fileName.LastIndexOf("."), fileName.Length - pos);

                            if (fileExt.ToLower() == "." + ext.ToLower())
                            {
                                response = true;
                                break;
                            }
                        }
                    }
                }

                if (!response)
                {
                    fileCollection[i].SaveAs(filePath);
                }
                else
                {
                    HttpContext context = HttpContext.Current;
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                        "file ext is problem. this ext is not use. (UploadFileAttachHandler) : " + fileName,
                        "/DeskPlusEIP/Data/Logs/FileUpload/", "");

                    //잘못된 파일확장자 일경우 초기화
                    fileName = "";
                }
            }

            return fileName;
            #endregion
        }

        /// <summary>
        /// 새로운 파일명 - 중복되지 않음
        /// </summary>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <returns></returns>
        private string NewFileName(string _targetPath, string _fileName)
        {
            int i = 1;
            string targetPath = _targetPath + _fileName;
            string fileName = "";
            int fileIdx = -1;

            while (File.Exists(targetPath))
            {
                fileName = targetPath.Substring(targetPath.LastIndexOf("\\") + 1, targetPath.Length - targetPath.LastIndexOf("\\") - 1);
                fileIdx = fileName.LastIndexOf(".");
                fileName = fileName.Substring(0, fileIdx) + "(" + i + ")" + fileName.Substring(fileIdx, fileName.Length - fileIdx);
                targetPath = targetPath.Substring(0, targetPath.LastIndexOf("\\") + 1) + fileName;
                i++;
            }

            return targetPath;
        }

        /// <summary>
        /// 다른 요청에서 <see cref="T:System.Web.IHttpHandler"/> 인스턴스를 사용할 수 있는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        /// <returns>
        ///   <see cref="T:System.Web.IHttpHandler"/> 인스턴스를 다시 사용할 수 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }


        #region 암호화 파라미터

        /// <summary>
        /// 
        /// </summary>
        private void SetDecryptString(HttpContext _context)
        {
            string param = _context.Request["p"];
            if (param != null)
            {
                param = _context.Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = _context.Server.UrlDecode(decryString);//디코딩
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        private string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        


        #endregion

    }
}
