using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Pdf;
using System.IO;
using Aspose.Pdf.Devices;
using System.Threading;
using System.Collections;
using System.Drawing;

using Aspose.Pdf.Structure;
using Aspose.Pdf.Text;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class AsposePDF
    {
        /// <summary>
        /// pdf파일 merge
        /// </summary>
        /// <param name="_arrpdf"></param>
        /// <param name="_targetpath"></param>
        /// <returns></returns>
        public bool MergePDF(string[] _arrorginalfiles, string _targetfile, bool _isrealpath)
        {
            if (_arrorginalfiles.Length.Equals(0) || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("MergePDF(" + _arrorginalfiles.Length + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값에러");
                return false;
            }

            try
            {
                HttpContext context = HttpContext.Current;
                string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

                Document pdf = new Document();
                pdf.Save(targetfile);
                //pdf.Direction = Direction.L2R;

                string filepath = string.Empty;
                foreach (string s in _arrorginalfiles)
                {
                    if (!s.Equals(""))
                    {
                        filepath = (_isrealpath) ? s : context.Server.MapPath(s);

                        Document sub = new Document(filepath);
                        pdf.Pages.Add(sub.Pages);
                    }
                }

                pdf.Save(targetfile);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("MergePDF(" + _arrorginalfiles.Length + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ImageFiles(ex.Tiff,JPG,BMP 등) -> PDF
        /// </summary>
        /// <param name="_originpath"></param>
        /// <param name="_targetfile"></param>
        /// <param name="_isrealpath"></param>
        /// <returns></returns>
        public bool ImageToPDF(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            try
            {
                HttpContext context = HttpContext.Current;
                string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
                string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

                if (!File.Exists(originfile))
                {
                    Helper.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 파일없음 에러");
                    return false;
                }

                // create a PDF object
                Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf();

                // create a section and add it to pdf document
                Aspose.Pdf.Generator.Section mainsec = pdf1.Sections.Add();

                //Add the radio form field to the paragraphs collection of the section
                // create an image object
                Aspose.Pdf.Generator.Image img = new Aspose.Pdf.Generator.Image();

                // specify the image file path information
                img.ImageInfo.File = originfile;

                // specify the image file type
                img.ImageInfo.ImageFileType = Aspose.Pdf.Generator.ImageFileType.Bmp;

                // create bitmap image object to load image information
                Bitmap myimage = new Bitmap(originfile);

                // Set margins so image will fit, etc.
                mainsec.PageInfo.Margin.Top = 5;
                mainsec.PageInfo.Margin.Bottom = 5;
                mainsec.PageInfo.Margin.Left = 5;
                mainsec.PageInfo.Margin.Right = 5;


                mainsec.PageInfo.PageWidth = (myimage.Width / myimage.HorizontalResolution) * 72;
                mainsec.PageInfo.PageHeight = (myimage.Height / myimage.VerticalResolution) * 72;

                #region 가로세로 체크불필요 - why. PDF인쇄 시, 자동처리
                // 이미지 가로세로 여부 - 가로일때, 로테이션
                //if (myimage.Width > myimage.Height)
                //{
                //    img.RotatingAngle = 90;
                //}

                //// specify the image width information equal to page width & Height
                //if (myimage.Width > (mainsec.PageInfo.PageWidth - 10))
                //{
                //    img.ImageInfo.FixWidth = mainsec.PageInfo.PageWidth - mainsec.PageInfo.Margin.Left - mainsec.PageInfo.Margin.Right;
                //}
                //if (myimage.Height > (mainsec.PageInfo.PageHeight - 10))
                //{
                //    img.ImageInfo.FixHeight = mainsec.PageInfo.PageHeight - mainsec.PageInfo.Margin.Top - mainsec.PageInfo.Margin.Bottom;
                //}

                //// check if the width of the image file is greater than Page width or not
                //// mainsec.IsLandscape = (myimage.Width > mainsec.PageInfo.PageWidth) ? true : false;
                #endregion

                mainsec.Paragraphs.Add(img);

                pdf1.Save(targetfile);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }
    }
}