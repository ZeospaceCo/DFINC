using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace Zeospace.DeskPlus.Component.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public class ThumbnailGenHandler : IHttpHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            int _width = 0;
            int _height = 0;
            string _path = String.Empty;
            const String NoThumbFile = "nothumb.jpg";

            if (context.Request["width"] != null)
            {
                _width = Int32.Parse(context.Request["width"]);
            }
            else
            {
                _width = 30;
            }

            if (context.Request["height"] != null)
            {
                _height = Int32.Parse(context.Request["height"]);
            }
            else
            {
                _height = 30;
            }

            String sNoThumbPath = context.Request.MapPath(context.Request.ApplicationPath.TrimEnd('/') + "/images/" + NoThumbFile);

            if (context.Request["ImgFilePath"] != null)
            {
                _path = context.Request.MapPath(context.Request["ImgFilePath"]);
            }
            else
            {
                _path = sNoThumbPath;
            }

            Bitmap thumbBitmap;

            thumbBitmap = new Bitmap(_path);

            if (thumbBitmap == null)
            {
                thumbBitmap = new Bitmap(sNoThumbPath);
            }

            if (context.Request["thumb"] != null && context.Request["thumb"] == "no")
            {
                thumbBitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                thumbBitmap = (Bitmap)thumbBitmap.GetThumbnailImage(_width, _height,
                new System.Drawing.Image.GetThumbnailImageAbort(ThumbCallback), IntPtr.Zero);
                context.Response.ContentType = "image/Jpeg";
                thumbBitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ThumbCallback() { return false; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

    }
}
