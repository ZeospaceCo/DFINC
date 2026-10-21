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
    public class UploadBoardHandler : IHttpHandler
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
                    string uploadPath = _context.Server.MapPath("/DeskPlusMobile/FileData/" + companyID + "/Users/" + userID + "/Board/Mobile/" + swfUploadGUID + "/");

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
                            string filePath = NewFileName(uploadPath, fileName);  //서버에는 같은 파일명이 있을수 있으니 중복 확인을 해서 유니크한 파일명을 얻어온다
                            MyFile.SaveAs(filePath);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("UploadAttachHandler[ProcessRequest] : " + ex.Message);
            }
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
    }
}