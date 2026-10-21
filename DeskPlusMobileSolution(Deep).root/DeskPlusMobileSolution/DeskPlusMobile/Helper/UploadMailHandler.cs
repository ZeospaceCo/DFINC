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
    public class UploadMailHandler : IHttpHandler
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
                string swfUploadGUID = _context.Request.Form["swfUploadGUID"];

                if (_context.Request.Files["upload"] != null)
                {                    
                    //string uploadPath = _context.Server.MapPath("/DeskPlusMobile/FileData/" + companyID + "/Users/" + userID + "/MailMobile/" + swfUploadGUID + "/");
                    string uploadPath = _context.Server.MapPath("/DeskPlusMobile/MailAttach/" + companyID + "/Users/" + userID + "/" + swfUploadGUID + "/");

                    // Spool 폴더 생성                                    
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    HttpFileCollection uploadFiles = _context.Request.Files;
                    for (int i = 0; i < uploadFiles.Count; i++)
                    {
                        HttpPostedFile MyFile = uploadFiles[i];

                        if (MyFile.ContentLength > 0)
                        {
                            string fileName = MyFile.FileName;

                            if (fileName.IndexOf("\\") > 0)
                            {
                                fileName = Path.GetFileName(fileName);
                            }

                            int FileSize = MyFile.ContentLength;
                            byte[] FileByteArray = new byte[FileSize];
                            MyFile.InputStream.Read(FileByteArray, 0, FileSize);
                            MyFile.SaveAs(uploadPath + fileName);
                        }
                    }                    
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("UploadMailHandler[ProcessRequest] : " + ex.Message);
            }
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