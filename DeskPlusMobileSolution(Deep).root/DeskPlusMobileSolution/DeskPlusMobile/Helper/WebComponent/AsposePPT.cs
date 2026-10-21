using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Slides;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class AsposePPT
    {
        /// <summary>
        /// PPT/PPTX => PDF
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public bool PPTToPDF(string _originfile, string _targetfile, bool _isrealpath, string _ext)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("PPTToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                if (_ext.ToUpper().Equals("PPTX"))
                {
                    Presentation pres1 = new Presentation(originfile);
                    pres1.Save(targetfile, Aspose.Slides.Export.SaveFormat.Pdf);

                }
                else
                {
                    Presentation pres2 = new Presentation(originfile);
                    pres2.Save(targetfile, Aspose.Slides.Export.SaveFormat.Pdf);
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("PPTToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }
    }
}