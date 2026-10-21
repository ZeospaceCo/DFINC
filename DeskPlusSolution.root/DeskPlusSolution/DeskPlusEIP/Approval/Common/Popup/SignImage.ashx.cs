using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    /// <summary>
    /// SignImage의 요약 설명입니다.
    /// </summary>
    public class SignImage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetExpires(DateTime.Now);
            context.Response.Cache.SetValidUntilExpires(false);

            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)context.Session["SignImage"];

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Flush();
                context.Response.BinaryWrite(ms.GetBuffer());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}