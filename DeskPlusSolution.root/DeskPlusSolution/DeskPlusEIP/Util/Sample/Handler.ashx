<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
        {
            try
            {
                //System.Threading.Thread.Sleep(5000) 'Slow down for testing only!
                HttpPostedFile myFile = context.Request.Files[0];
                string RootPath = context.Server.UrlDecode("/DeskPlusEIP/FileData/1/Users/1/Spool/");
                if (System.IO.File.Exists(context.Server.MapPath(RootPath + myFile.FileName)))
                {
                    //중복파일 있음
                    context.Response.Write("multi");
                }
                else
                {
                    myFile.SaveAs(context.Server.MapPath(RootPath + myFile.FileName));
                    context.Response.Write("ok");
                }
            }
            catch
            {
                context.Response.Write("error");
            }
        }

   
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}