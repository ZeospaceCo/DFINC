using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Signature
{
    /// <summary>
    /// SignImage의 요약 설명입니다.
    /// </summary>
    public class GetImage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetExpires(DateTime.Now);
            context.Response.Cache.SetValidUntilExpires(false);

            string signPath = context.Request["p"];
            signPath = context.Server.UrlDecode(Utility.DecryptString(signPath));

            if (File.Exists(context.Server.MapPath(signPath)))
            {
                Bitmap sealBitmap;

                sealBitmap = new Bitmap(context.Server.MapPath(signPath));

                using (MemoryStream ms = new MemoryStream())
                {
                    sealBitmap.Save(ms, ImageFormat.Png);
                    ms.Flush();
                    context.Response.BinaryWrite(ms.GetBuffer());
                }
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