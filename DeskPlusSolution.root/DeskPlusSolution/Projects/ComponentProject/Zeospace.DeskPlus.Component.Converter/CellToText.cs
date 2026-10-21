using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Cells;
using System.IO;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class CellToText
    {
        /// <summary>
        /// 엑셀 파일을 Txt로 변환
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public static bool GetText(string _source, string _target)
        {
            try
            {
                Workbook workbook = new Workbook(_source);

                //0-byte array
                byte[] workbookData = new byte[0];

                //Text save options. You can use any type of separator
                TxtSaveOptions opts = new TxtSaveOptions();
                opts.Separator = '\t';

                //Copy each worksheet data in text format inside workbook data array
                for (int idx = 0; idx < workbook.Worksheets.Count; idx++)
                {
                    //Save the active worksheet into text format
                    MemoryStream ms = new MemoryStream();
                    workbook.Worksheets.ActiveSheetIndex = idx;
                    workbook.Save(ms, opts);

                    //Save the worksheet data into sheet data array
                    ms.Position = 0;
                    byte[] sheetData = ms.ToArray();

                    //Combine this worksheet data into workbook data array
                    byte[] combinedArray = new byte[workbookData.Length + sheetData.Length];
                    Array.Copy(workbookData, 0, combinedArray, 0, workbookData.Length);
                    Array.Copy(sheetData, 0, combinedArray, workbookData.Length, sheetData.Length);

                    workbookData = combinedArray;
                }

                File.WriteAllBytes(_target, workbookData);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CellToText : " + _source + " ==> " + _target + " ==> " + ex.Message);

                return false;
            }
        }
    }
}