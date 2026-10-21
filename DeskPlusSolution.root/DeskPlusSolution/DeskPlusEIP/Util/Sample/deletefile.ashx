<%@ WebHandler Language="C#" Class="deletefile" %>

using System;
using System.Web;

public class deletefile : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string RootPath = context.Server.UrlDecode("~/temp/");

        if (context.Request.QueryString["deletefile"] != null)
        {
            string fileName = context.Request.QueryString["deletefile"].ToString();
            if (System.IO.File.Exists(context.Server.MapPath(RootPath + fileName)))
            {
                System.IO.File.Delete(context.Server.MapPath(RootPath + fileName));
                context.Response.Write("file deleted");
            }
            else
                context.Response.Write("file does not exists");
        }
        else
        {
            context.Response.Write("no file passed!");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}