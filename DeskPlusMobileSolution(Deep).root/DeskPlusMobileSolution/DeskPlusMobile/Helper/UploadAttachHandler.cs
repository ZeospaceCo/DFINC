using System;
using System.Web;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public class UploadAttachHandler : IHttpHandler
    {

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="_context">The _context.</param>
        public void ProcessRequest(HttpContext _context)
        {
            try
            {   
                string companyID = _context.Request.Form["companyID"];
                string userID = _context.Request.Form["userID"];
                string folderID = _context.Request.Form["folderID"];
                string publicGubun = _context.Request.Form["publicGubun"];
                int fileID = 0;
                
                if (_context.Request.Files["upload"] != null)
                {
                    string uploadPath = _context.Server.MapPath("/DeskPlusMobile/FileData/" + companyID + "/Users/" + userID + "/WebHard/Mobile/");

                    string targetPath = string.Empty;

                    if (publicGubun == "Y")
                    {
                        targetPath = _context.Server.MapPath("/DeskPlusMobile/FileData/" + companyID + "/WebHard/" + folderID + "/");
                    }
                    else
                    {
                        targetPath = _context.Server.MapPath("/DeskPlusMobile/FileData/" + companyID + "/Users/" + userID + "/WebHard/" + folderID + "/");
                    }

                    // Spool 폴더 생성
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
                    HttpFileCollection uploadFiles = _context.Request.Files;
                    for (int i = 0; i < uploadFiles.Count; i++)
                    {
                        HttpPostedFile MyFile = uploadFiles[i];

                        if (MyFile.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(MyFile.FileName);                            
                            int FileSize = MyFile.ContentLength;
                            byte[] FileByteArray = new byte[FileSize];
                            MyFile.InputStream.Read(FileByteArray, 0, FileSize);
                            MyFile.SaveAs(uploadPath + fileName);

                            // 웹폴더 처리
                            fileID = SetWebFolder(companyID, userID, folderID, uploadPath, targetPath, fileName, publicGubun);

                            var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "DocStreamYN");

                            if (docStreamYN == "Y")
                            {
                                DocStream ds = new DocStream();
                                try
                                {
                                    ds.AddDocStream(Convert.ToInt32(companyID), fileID, "WHD_FILE");
                                    ds.UpdateFileID(Convert.ToInt32(companyID), fileID, "WHD_FILE");
                                }
                                catch (Exception ex)
                                {
                                    ds.DelDocStream(Convert.ToInt32(companyID), fileID, "WHD_FILE");
                                    AppException.WriteException("UploadAttachHandler[WHD_FILE_Addition_DS] : " + ex.Message);
                                }
                            }
                        }
                    }
                }

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("FolderID", folderID.ToString());
                //paraDic.Add("FileID", fileID.ToString());
                paraDic.Add("PublicGubun", publicGubun);
                string encParams = Utility.EncryptString(_context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));

                string url = "/DeskPlusMobile/WebHard/List.aspx?p=" + encParams;
                HttpContext ctx = HttpContext.Current;
                ctx.Response.Clear();
                ctx.Response.Redirect(url, false);
                ctx.Response.Flush();

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("UploadAttachHandler[ProcessRequest] : " + ex.Message);
            }
        }

        private int SetWebFolder(string _companyID, string _userID, string _folderID, string _uploadPath, string _path, string _fileName, string _publicGubun)
        {
            // DB 처리
            FileInfo fInfo = new FileInfo(_uploadPath + _fileName);

            Zeospace.DeskPlusMobile.Web.WebHard.Ws.BE.FileInfo info = new WebHard.Ws.BE.FileInfo();

            info.CompanyID = _companyID.ToNumber<int>();
            info.UserID = _userID.ToNumber<int>();
            info.FolderID = _folderID.ToNumber<int>();

            if (_publicGubun == "Y")
            {
                info.FilePath = "/DeskPlusEIP/FileData/" + _companyID + "/WebHard/" + _folderID + "/";
            }
            else
            {
                info.FilePath = "/DeskPlusEIP/FileData/" + _companyID + "/Users/" + _userID + "/WebHard/" + _folderID + "/";
            }

            info.FileName = _fileName;
            info.FileType = fInfo.Extension;
            info.FileLength2 = fInfo.Length;
            info.FileGUID = System.Guid.NewGuid().ToString() + fInfo.Extension;
            info.DeleteYN = "N";

            if (HasScreenShot(info.FileType))
            {
                info.ScreenShot = "Y";
            }
            else
            {
                info.ScreenShot = "N";
            }

            Zeospace.DeskPlusMobile.Web.WebHard.Ws.DAL.File dal = new WebHard.Ws.DAL.File();

            var rtnval = dal.Add(info);

            // File 처리
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            File.Copy(_uploadPath + _fileName, _path + info.FileGUID, true);
            File.Delete(_uploadPath + _fileName);

            if (info.ScreenShot == "Y")
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(_path + info.FileGUID);

                System.Drawing.Image thumb = image.GetThumbnailImage(150, 150,
                    delegate() { return false; }, (IntPtr)0);

                //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                thumb.Save(_path + "Thum_" + info.FileGUID);

                image.Dispose();
            }

            return rtnval;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-02-25
        /// 작성목적 : 이미지 파일인지 구분
        /// </summary>
        /// <returns></returns>
        private bool HasScreenShot(string _fileType)
        {
            bool fileOK = false;

            string[] allowedExtensions = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ImgFileExtension").Split(',');

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (_fileType.ToLower() == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
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
    }
}