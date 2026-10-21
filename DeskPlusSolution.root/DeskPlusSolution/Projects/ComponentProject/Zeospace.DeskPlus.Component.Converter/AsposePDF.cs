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

namespace Zeospace.DeskPlus.Component.Converter
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MergePDF(" + _arrorginalfiles.Length + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값에러");
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MergePDF(" + _arrorginalfiles.Length + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// pdf파일 내, file embeded
        /// </summary>
        /// <param name="_arrpdf"></param>
        /// <param name="_targetpath"></param>
        /// <returns></returns>
        public bool EmbeddedFilePDF(string[] _arrorginalfiles, string _targetfile, bool _isrealpath)
        {
            if (_arrorginalfiles.Length.Equals(0) || _targetfile.Equals("")) return false;

            try
            {
                HttpContext context = HttpContext.Current;
                string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

                Document pdf = new Document(targetfile);
                //pdf.Direction = Direction.L2R;

                string filepath = string.Empty;
                foreach (string s in _arrorginalfiles)
                {
                    filepath = (_isrealpath) ? s : context.Server.MapPath(s);

                    FileSpecification attfile = new FileSpecification(filepath);
                    pdf.EmbeddedFiles.Add(attfile);
                }

                pdf.Save(targetfile);
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EmbeddedFilePDF(" + _arrorginalfiles.Length + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// PDF -> JPG
        /// </summary>
        /// <param name="_originpath"></param>
        /// <param name="_targetfolder"></param>
        /// <param name="_flagnm"></param>
        /// <returns></returns>
        public ArrayList PDFToJPG(string _originfile, string _targetpath, string _targetfilenm, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetpath.Equals("") || _targetfilenm.Equals(""))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PDFToJPG(" + _originfile + "," + _targetpath + "," + _targetfilenm + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return null;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetpath = (_isrealpath) ? _targetpath : context.Server.MapPath(_targetpath);
            targetpath = targetpath.Length.Equals(targetpath.LastIndexOf("\\") + 1) ? targetpath : targetpath + "\\";

            try
            {
                ArrayList arrval = new ArrayList();

                string ext = _targetfilenm.Split('.')[_targetfilenm.Split('.').Length - 1];
                string nm = _targetfilenm.Replace("." + ext, "");

                int cnt = 1;
                Document pdf = new Document(originfile);
                for (int i = 0; i < pdf.Pages.Count; i++)
                {
                    string filenm = (cnt.Equals(1)) ? nm : nm + "_" + (cnt - 1).ToString().PadLeft(2, '0');
                    filenm += ".jpg";

                    arrval.Add(targetpath + filenm);

                    using (FileStream imageStream = new FileStream(targetpath + filenm, FileMode.Create))
                    {
                        //create JPEG device with specified attributes
                        //Width, Height, Resolution, Quality
                        //Quality [0-100], 100 is Maximum
                        //create Resolution object
                        Resolution resolution = new Resolution(300);

                        //JpegDevice jpegDevice = new JpegDevice(500, 700, resolution, 100);
                        JpegDevice jpegDevice = new JpegDevice(resolution, 100);

                        //convert a particular page and save the image to stream
                        jpegDevice.Process(pdf.Pages[cnt], imageStream);

                        //close stream
                        imageStream.Close();
                    }

                    cnt++;
                }

                return arrval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PDFToJPG(" + _originfile + "," + _targetpath + "," + _targetfilenm + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ImageFiles(ex.Tiff,JPG,BMP 등) -> PDF
        /// </summary>
        /// <param name="_originpath"></param>
        /// <param name="_targetfile"></param>
        /// <param name="_isrealpath"></param>
        /// <returns></returns>
        public bool ImagesToPDF(string _originpath, string _targetfile, bool _isrealpath)
        {
            if (_originpath.Equals("") || _targetfile.Equals(""))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImagesToPDF(" + _originpath + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            try
            {
                HttpContext context = HttpContext.Current;
                string originpath = (_isrealpath) ? _originpath : context.Server.MapPath(_originpath);
                string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

                if (!Directory.Exists(originpath))
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImagesToPDF(" + _originpath + "," + _targetfile + "," + _isrealpath.ToString() + ") : 폴더없음 에러");
                    return false;
                }

                // create a PDF object
                Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf();

                string[] arrfiles = Directory.GetFiles(originpath);
                foreach (string path in arrfiles)
                {
                    // create a section and add it to pdf document
                    Aspose.Pdf.Generator.Section mainsec = pdf1.Sections.Add();

                    //Add the radio form field to the paragraphs collection of the section
                    // create an image object
                    Aspose.Pdf.Generator.Image img = new Aspose.Pdf.Generator.Image();

                    // specify the image file path information
                    img.ImageInfo.File = path;

                    // specify the image file type
                    img.ImageInfo.ImageFileType = Aspose.Pdf.Generator.ImageFileType.Bmp;

                    // create bitmap image object to load image information
                    Bitmap myimage = new Bitmap(path);

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
                }

                pdf1.Save(targetfile);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImagesToPDF(" + _originpath + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            try
            {
                HttpContext context = HttpContext.Current;
                string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
                string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

                if (!File.Exists(originfile))
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 파일없음 에러");
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ImageToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Text -> PDF
        /// </summary>
        /// <param name="_originfile"></param>
        /// <param name="_targetfile"></param>
        /// <param name="_isrealpath"></param>
        /// <returns></returns>
        public bool TextToPDF(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TextToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                //read the source text file
                System.IO.TextReader tr = new StreamReader(originfile);

                //Instantiate Pdf pbject by calling its empty constructor
                Aspose.Pdf.Generator.Pdf pdf1 = new Aspose.Pdf.Generator.Pdf();

                //Create a new section in the Pdf object
                Aspose.Pdf.Generator.Section sec1 = pdf1.Sections.Add();

                //Create a new text paragraph and pass the text to its constructor as argument
                Aspose.Pdf.Generator.Text t2 = new Aspose.Pdf.Generator.Text(tr.ReadToEnd());
                sec1.Paragraphs.Add(t2);

                pdf1.Save(targetfile);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TextToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }
    }
}