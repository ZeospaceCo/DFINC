using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Aspose.Cells.Rendering;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class AsposeExcel
    {
        /*
         D:\01_프로젝트\01_진행중인 프로젝트\01_DeskPlus40\00.팩키징\PDF컴포넌트
         * \Aspose_Cells_NET-master\Aspose_Cells_NET-master\ProgrammersGuide\AsposeCells\WorkingWithFiles\UtilityFeatures
         */


        /// <summary>
        /// Excel => PDF
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public bool ExcelToPDF(string _originfile, string _targetfile, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetfile.Equals(""))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ExcelToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return false;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetfile = (_isrealpath) ? _targetfile : context.Server.MapPath(_targetfile);

            try
            {
                Workbook wb = new Workbook(originfile);
                wb.Save(targetfile, SaveFormat.Pdf);

                //PdfSaveOptions opt = new PdfSaveOptions();
                //opt.Compliance = PdfCompliance.PdfA1b;
                //wb.Save(targetfile, opt);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ExcelToPDF(" + _originfile + "," + _targetfile + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Excel => JPG
        /// </summary>
        /// <param name="_originpath"></param>
        /// <param name="_targetpath"></param>
        /// <returns></returns>
        public ArrayList ExcelToJPG(string _originfile, string _targetpath, string _targetfilenm, bool _isrealpath)
        {
            if (_originfile.Equals("") || _targetpath.Equals("") || _targetfilenm.Equals(""))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ExcelToJPG(" + _originfile + "," + _targetpath + "," + _targetfilenm + "," + _isrealpath.ToString() + ") : 입력값 에러");
                return null;
            }

            HttpContext context = HttpContext.Current;
            string originfile = (_isrealpath) ? _originfile : context.Server.MapPath(_originfile);
            string targetpath = (_isrealpath) ? _targetpath : context.Server.MapPath(_targetpath);
            targetpath = targetpath.Length.Equals(targetpath.LastIndexOf("\\") + 1) ? targetpath : targetpath + "\\";
            
            try
            {
                ArrayList arrval = new ArrayList();

                ImageOrPrintOptions imgOptions = new ImageOrPrintOptions();             //Define ImageOrPrintOptions
                imgOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;       //Specify the image format
                imgOptions.OnePagePerSheet = true;                                      //Only one page for the whole sheet would be rendered

                string ext = _targetfilenm.Split('.')[_targetfilenm.Split('.').Length - 1];
                string nm = _targetfilenm.Replace("." + ext, "");

                int cnt = 0;
                Workbook wb = new Workbook(originfile);                                 //Create a new Workbook object and open a template Excel file.
                foreach (Worksheet sheet in wb.Worksheets)
                {
                    string filenm = (cnt.Equals(0)) ? nm : nm + "_" + cnt.ToString().PadLeft(2, '0');
                    filenm += "." + ext;

                    arrval.Add(targetpath + filenm);

                    SheetRender sr = new SheetRender(sheet, imgOptions);                //Render the sheet with respect to specified image/print options
                    Bitmap bitmap = sr.ToImage(0);                                      //Render the image for the sheet

                    bitmap.Save(targetpath + filenm);                                   //Save the image file specifying its image format.

                    cnt++;
                }

                return arrval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ExcelToJPG(" + _originfile + "," + _targetpath + "," + _targetfilenm + "," + _isrealpath.ToString() + ") : " + ex.Message);
                return null;
            }
        }
    }
}