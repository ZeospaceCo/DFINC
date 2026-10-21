/*
 * jQuery File Upload Plugin C# Example 1.0
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * PHP Version: Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 * 
 * Translated to C# by Shannon Whitley 2012
 * http://whitleymedia.com
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */
/*
 * Notes:  Form action must reference the full url, including default.aspx.
 *         Orientation and Resumable Downloads are not current supported.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.Web.Upload.JSUpload
{
    public partial class Default : Base.BasePage
    {
        protected global::System.Web.UI.WebControls.HiddenField forgeryToken;

        UploadHandler upload_handler = null;

        public string csrf_token = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            CsrfHandler.Validate(this.Page, forgeryToken);

            Guid antiforgeryToken = Guid.NewGuid();
            csrf_token = antiforgeryToken.ToString();

            string filePath = GetDecryptParam("filePath", "");
            string blockExt = GetDecryptParam("blockExt", "");
            int max_file_size = Convert.ToInt32(GetDecryptParam("max_file_size", "0"));
            if (String.IsNullOrEmpty(filePath))
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = baseFileDirectory + "{0}/Users/{1}/Spool/";
                filePath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }

            if (!Directory.Exists(Server.MapPath(filePath))) Directory.CreateDirectory(Server.MapPath(filePath));

            upload_handler = new UploadHandler(Server.MapPath(filePath), FullUrlGet(), blockExt, max_file_size);
            Response.Clear();
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Cache-Control", "no-store, no-cache, must-revalidate");
            Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            Response.AddHeader("X-Content-Type-Options", "nosniff");
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            Response.AddHeader("Access-Control-Allow-Methods", "OPTIONS, HEAD, GET, POST, PUT, DELETE");
            Response.AddHeader("Access-Control-Allow-Headers", "X-File-Name, X-File-Type, X-File-Size");

            switch (Request.HttpMethod)
            {
                case "OPTIONS":
                    break;
                case "HEAD":
                case "GET":
                    Get();
                    break;
                case "POST":
                    if (Request["_method"] != null && Request["_method"] == "DELETE")
                    {
                        Delete();
                    }
                    else
                    {
                        Post();
                    }
                    break;
                case "DELETE":
                    Delete();
                    break;
                default:
                    Response.Status = "Method Not Allowed";
                    Response.StatusCode = 405;
                    Response.End();
                    break;
            }
        }


        private void Get()
        {
            string file_name = null;
            string json = "";

            if (Request["file"] != null)
            {
                file_name = Path.GetFileName(Request["file"]);
            }

            Response.AddHeader("Content-type", "application/json");

            if (!String.IsNullOrEmpty(file_name))
            {
                json = Json.Serialize<UploadHandler.UploadFileInfo>(upload_handler.FileObjectGet(file_name));
            }
            else
            {
                json = Json.Serialize<List<UploadHandler.UploadFileInfo>>(upload_handler.FileObjectsGet());
            }
            Response.Write(json);
            Response.End();
        }


        private void Post()
        {
            if (Request["_method"] != null && Request["_method"] == "DELETE")
            {
                Delete();
            }

            List<UploadHandler.UploadFileInfo> fileInfoList = new List<UploadHandler.UploadFileInfo>();
            HttpFileCollection upload = Request.Files;

            for (int i = 0; i < upload.Count; i++)
            {
                UploadHandler.UploadFileInfo fileInfo = new UploadHandler.UploadFileInfo();
                HttpPostedFile file = upload[i];
                fileInfo.type = Path.GetExtension(file.FileName).ToLower();
                fileInfo.name = Path.GetFileName(file.FileName);
                fileInfo.size = file.InputStream.Length;
                if (Request.Headers["X-File-Size"] != null)
                {
                    fileInfo.size = long.Parse(Request.Headers["X-File-Size"].ToString());
                }

                fileInfo = upload_handler.FileUploadHandle(file, fileInfo.name, fileInfo.size, fileInfo.type, fileInfo.error, i);
                fileInfoList.Add(fileInfo);
            }
            Response.Clear();
            Response.AddHeader("Vary", "Accept");
            string json = Json.Serialize<List<UploadHandler.UploadFileInfo>>(fileInfoList);
            string redirect = null;
            if (Request["redirect"] != null)
            {
                redirect = Request["Redirect"];
            }
            if (redirect != null)
            {
                Response.AddHeader("Location,", String.Format(redirect, Server.UrlEncode(json)));
                Response.End();
            }
            if (Request.ServerVariables["HTTP_ACCEPT"] != null && Request.ServerVariables["HTTP_ACCEPT"].ToString().IndexOf("application/json") >= 0)
            {
                Response.AddHeader("Content-type", "application/json");
            }
            else
            {
                Response.AddHeader("Content-type", "text/plain");
            }

            Response.Write(json);
            Response.End();

        }

        private void Delete()
        {
            string file_name = null;
            if (Request["file"] != null)
            {
                file_name = Request["file"];
            }
            string file_path = upload_handler.upload_dir + file_name;
            UploadHandler.UploadFileInfo file = upload_handler.FileObjectGet(file_name);

            bool success = File.Exists(file_path) && file_name.Length > 0 && file_name.Substring(0, 1) != ".";
            if (success)
            {
                success = false;
                File.Delete(file_path);
                success = true;
            }
            if (success)
            {
                //Delete other file versions.
                foreach (string version in file.image_versions.Keys)
                {
                    if (File.Exists(file.image_versions[version].dir + file_name))
                    {
                        File.Delete(file.image_versions[version].dir + file_name);
                    }
                }
            }
            Response.AddHeader("Content-type", "application/json");
            Response.Write(Json.Serialize<bool>(success));
            Response.End();
        }

        private string FullUrlGet()
        {
            string url = Request.Url.AbsoluteUri;
            if (url.LastIndexOf("/") > 1)
            {
                url = url.Substring(0, url.LastIndexOf("/"));
            }


            return url;
        }
    }

    public class CsrfHandler
    {
        public static void Validate(Page page, HiddenField forgeryToken)
        {
            if (!page.IsPostBack)
            {
                Guid antiforgeryToken = Guid.NewGuid();
                page.Session["AntiforgeryToken"] = antiforgeryToken;
                forgeryToken.Value = antiforgeryToken.ToString();
            }
            else
            {
                Guid stored = (Guid)page.Session["AntiforgeryToken"];
                Guid sent = new Guid(forgeryToken.Value);
                if (sent != stored)
                {
                    // you can throw an exception, in my case I'm just logging the user out
                    page.Session.Abandon();
                    page.Response.Redirect("/DeskPlusEIP/Site/Auth/LogOut.aspx");
                }
            }
        }
    }
}