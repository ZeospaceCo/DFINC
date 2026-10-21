using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Words;
using System.IO;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class WordToText
    {
        /// <summary>
        /// 워드파일을 Txt로 변환
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public static bool GetText(string _source, string _target)
        {
            try
            {
                Document doc = new Document(_source);
                doc.Range.Replace(ControlChar.LineBreak, "\n", false, false);
                doc.Save(_target);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WordToText : " + _source + " ==> " + _target + " ==> " + ex.Message);
                
                return false;
            }
        }
    }
}