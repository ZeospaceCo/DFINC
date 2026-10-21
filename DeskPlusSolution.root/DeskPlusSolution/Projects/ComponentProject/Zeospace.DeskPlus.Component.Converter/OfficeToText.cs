using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class OfficeToText
    {
        public static void GetDirToText(string _dir, string _target)
        {
            string targetFileName = String.Empty;
            string[] files = Directory.GetFiles(_dir);
            string[] dirs = Directory.GetDirectories(_dir);
            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);
                targetFileName = f.DirectoryName.Replace('\\', '_') + "_" + f.Name + ".txt";

                if (f.Exists)
                {
                    switch (f.Extension.ToLower())
                    {
                        case ".doc":
                            WordToText.GetText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("doc : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".docx":
                            WordToText.GetText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("docx : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".xls":
                            CellToText.GetText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("xls : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".xlsx":
                            CellToText.GetText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("xlsx : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".ppt":
                            SlideToText.GetPptToText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ppt : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".pptx":
                            SlideToText.GetPptxToText(file, _target + targetFileName);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("pptx : " + targetFileName, Base.ApplicationFrameworks.Helper.AppSettingType.SEARCH);
                            break;
                        case ".pdf":
                            PdfToText.GetText(file, _target + targetFileName);
                            break;
                        //case ".eml":

                        //    break;
                    }
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetDirToText : The file does not exist " + file);
                }


            }
            foreach (string dir in dirs)
            {
                GetDirToText(dir, _target);
            }
        }
    }
}