using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Text;

using Aspose.Email;
using Aspose.Email.Mail;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class AsposeEmail
    {
        /// <summary>
        /// EML => PDF
        /// </summary>
        /// <param name="_originfile"></param>
        /// <param name="_targetfile"></param>
        /// <param name="_isrealpath"></param>
        /// <returns></returns>
        public bool EmlToPdf(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("EmlToPdf(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                Aspose.Words.Document doc;

                MailMessage mailMsg = MailMessage.Load(originfile, MailMessageLoadOptions.DefaultEml);
                mailMsg.PreferredTextEncoding = Encoding.UTF8;
                using (var mhtStream = new MemoryStream())
                {
                    mailMsg.Save(mhtStream, SaveOptions.DefaultMhtml); // MailMessageSaveType.MHtmlFormat);
                    doc = new Aspose.Words.Document(mhtStream);
                }

                doc.Save(targetfile, Aspose.Words.SaveFormat.Pdf);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("EmlToPdf(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }
    }
}