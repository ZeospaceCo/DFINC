using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Slides;
using Aspose.Slides.Util;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class SlideToText
    {
        /// <summary>
        /// PPT(97~2003) 파일을 Txt로 변환
        /// </summary>
        /// <param name="_source">원본파일 전체 경로</param>
        /// <param name="_target">대상파일 전체 경로</param>
        public static bool GetPptToText(string _source, string _target)
        {
            return GetPptxToText(_source, _target);
        }

        /// <summary>
        /// PPTX(2007~) 파일을 Txt로 변환
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_target"></param>
        public static bool GetPptxToText(string _source, string _target)
        {
            StreamWriter stream = null;
            try
            {
                //txt 파일 스트림 생성
                stream = new StreamWriter(_target, true, Encoding.UTF8);
                
                using (Presentation pptxPresentation = new Presentation(_source))
                {
                    //Get an Array of TextFrameEx objects from all slides in the PPTX
                    ITextFrame[] textFramesPPTX = Aspose.Slides.Util.SlideUtil.GetAllTextFrames(pptxPresentation, true);

                    //Loop through the Array of TextFrames
                    for (int i = 0; i < textFramesPPTX.Length; i++)
                    {
                        //Loop through paragraphs in current TextFrame
                        foreach (IParagraph para in textFramesPPTX[i].Paragraphs)
                        {

                            //Loop through portions in the current Paragraph
                            foreach (IPortion port in para.Portions)
                            {
                                stream.WriteLine(port.Text);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPptxToText : " + _source + " ==> " + _target + " ==> " + ex.Message);

                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }	
        }
    }
}