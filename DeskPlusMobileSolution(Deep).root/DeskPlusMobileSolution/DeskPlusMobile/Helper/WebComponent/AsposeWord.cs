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

using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Rendering;
using Aspose.Words.Saving;
using Aspose.Words.Tables;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class AsposeWord
    {
        // <summary>
        /// Word => PDF (Img로 변환하지 않아도, 호환성 좋음)
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public bool DocToPDF(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("DocToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                LoadOptions lopt = new LoadOptions();
                lopt.Encoding = Encoding.Default;

                Document doc = new Document(originfile, lopt);

                doc.Save(targetfile, SaveFormat.Pdf);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DocToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Tiff (Img) -> PDF
        /// </summary>
        /// <param name="_originfile"></param>
        /// <param name="_targetfile"></param>
        /// <param name="_isrealpath"></param>
        /// <returns></returns>
        public bool ImageToPdf(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Helper.AppException.WriteException("ImageToPdf(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                // Create Aspose.Words.Document and DocumentBuilder.
                // The builder makes it simple to add content to the document.
                Document doc = new Document();
                DocumentBuilder builder = new DocumentBuilder(doc);

                // Read the image from file, ensure it is disposed.
                using (Image image = Image.FromFile(originfile))
                {
                    // Get the number of frames in the image.
                    int frmcnt = image.GetFrameCount(FrameDimension.Page);

                    // Loop through all frames.
                    for (int idx = 0; idx < frmcnt; idx++)
                    {
                        // Insert a section break before each new page, in case of a multi-frame TIFF.
                        if (idx != 0)
                            builder.InsertBreak(BreakType.SectionBreakNewPage);

                        // Select active frame.
                        image.SelectActiveFrame(FrameDimension.Page, idx);

                        // Max page size
                        const double maxPageHeight = 1584;
                        const double maxPageWidth = 1584;

                        double currentImageHeight = ConvertUtil.PixelToPoint(image.Height, image.VerticalResolution);
                        double currentImageWidth = ConvertUtil.PixelToPoint(image.Width, image.HorizontalResolution);

                        if (currentImageWidth >= maxPageWidth || currentImageHeight >= maxPageHeight)
                        {
                            // Get max image size.
                            CalculateImageSize(image, maxPageHeight, maxPageWidth, out currentImageHeight, out currentImageWidth);
                        }

                        // We want the size of the page to be the same as the size of the image.
                        // Convert pixels to points to size the page to the actual image size.
                        PageSetup ps = builder.PageSetup;
                        ps.PageWidth = currentImageWidth;
                        ps.PageHeight = currentImageHeight;

                        // Insert the image into the document and position it at the top left corner of the page.
                        builder.InsertImage(
                            image,
                            RelativeHorizontalPosition.Page,
                            0,
                            RelativeVerticalPosition.Page,
                            0,
                            ps.PageWidth,
                            ps.PageHeight,
                            WrapType.None);
                    }
                }

                // Save the document to PDF.
                doc.Save(targetfile);


                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ImageToPdf(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// Calculates size of Image
        /// </summary>
        /// <param name="img">Original image</param>
        /// <param name="containerHeight">Height of container where image should be inserted.</param>
        /// <param name="containerWidth">Width of container where image should be inserted.</param>
        /// <param name="targetHeight">Height of the image</param>
        /// <param name="targetWidth">Width of the image</param>
        public void CalculateImageSize(Image img, double containerHeight, double containerWidth, out double targetHeight, out double targetWidth)
        {
            //Calculate width and height
            targetHeight = containerHeight;
            targetWidth = containerWidth;

            //Get size of an image
            double imgHeight = ConvertUtil.PixelToPoint(img.Height);
            double imgWidth = ConvertUtil.PixelToPoint(img.Width);

            if (imgHeight < targetHeight && imgWidth < targetWidth)
            {
                targetHeight = imgHeight;
                targetWidth = imgWidth;
            }
            else
            {
                //Calculate size of an image in the document
                double ratioWidth = imgWidth / targetWidth;
                double ratioHeight = imgHeight / targetHeight;
                if (ratioWidth > ratioHeight)
                    targetHeight = (targetHeight * (ratioHeight / ratioWidth));
                else
                    targetWidth = (targetWidth * (ratioWidth / ratioHeight));
            }
        }
    }
}