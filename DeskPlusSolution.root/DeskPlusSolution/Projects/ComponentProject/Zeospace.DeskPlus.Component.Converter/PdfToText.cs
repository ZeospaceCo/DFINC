using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.IO;
using System.Text;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class PdfToText
    {
        /// <summary>
        /// pdf 파일을 Txt로 변환
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public static bool GetText(string _source, string _target)
        {
            try
            {
                Document pdfDocument = new Document(_source);
                //create TextAbsorber object to extract text
                TextAbsorber textAbsorber = new TextAbsorber();
                //accept the absorber for all the pages
                pdfDocument.Pages.Accept(textAbsorber);
                //get the extracted text
                string extractedText = textAbsorber.Text;
                // create a writer and open the file
                TextWriter tw = new StreamWriter(_target, true, Encoding.UTF8);
                // write a line of text to the file
                tw.WriteLine(extractedText);
                // close the stream
                tw.Close();

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PdfToText : " + _source + " ==> " + _target + " ==> " + ex.Message);

                return false;
            }
        }
    }
}