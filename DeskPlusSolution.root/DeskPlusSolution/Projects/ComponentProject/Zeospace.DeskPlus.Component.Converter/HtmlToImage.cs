using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Zeospace.DeskPlus.Component.Converter
{
    public class HtmlToImage : System.Windows.Forms.ApplicationContext
    {
        AutoResetEvent resultEvent; //리턴 이벤트
        string htmlResult;          //결과 값
        string filePath;            //생성되는 파일 Full 경로명
        string fileName;            //파일명
        WebBrowser webBrowser;
        Thread thrd;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_source">HTML source</param>
        /// <param name="_filePath">파일 경로 d:\deskpluseip\filedata\</param>
        /// <param name="_fileName">파일명 ImageName(확장자 제외 jpg 로생성함)</param>
        /// <param name="_resultEvent"></param>
        public HtmlToImage(string _source, string _filePath, string _fileName, AutoResetEvent _resultEvent)
        {
            try
            {
                this.resultEvent = _resultEvent;
                htmlResult = null;
                filePath = _filePath;
                fileName = _fileName;

                thrd = new Thread(new ThreadStart(
                    delegate
                    {
                        Init(_source);
                        System.Windows.Forms.Application.Run(this);
                    }));
                // set thread to STA state before starting
                thrd.SetApartmentState(ApartmentState.STA);
                thrd.Start();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WordToText : " + _source + " ==> " + _filePath + _fileName + " ==> " + ex.Message);   
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="source"></param>
        private void Init(string source)
        {
            // 웹브라우져 생성
            webBrowser = new WebBrowser();

            //문서 완료 이벤트
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);

            //HTML 추가
            webBrowser.DocumentText = source;
        }

        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (thrd != null)
            {
                thrd.Abort();
                thrd = null;
                return;
            }

            System.Runtime.InteropServices.Marshal.Release(webBrowser.Handle);
            webBrowser.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// 문서 생성 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var webBrowser = (WebBrowser)sender;

            try
            {
                webBrowser.Width = webBrowser.Document.Body.ScrollRectangle.Width + 50; // html 문서의 넓이
                webBrowser.Height = webBrowser.Document.Body.ScrollRectangle.Height + 50; // html 문서의 높이 

                string imageName = filePath + fileName + ".jpg"; // c:\imsi\imageName.jpg
                using (Bitmap bitmap = new Bitmap(webBrowser.Document.Body.ScrollRectangle.Width, webBrowser.Document.Body.ScrollRectangle.Height))
                {
                    webBrowser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));



                    bitmap.Save(imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                htmlResult = imageName; // 성공시 생성된 이미지 경로 제공

                this.resultEvent.Set();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                htmlResult = "FAIL"; // 실패시 리턴

                this.resultEvent.Set();
            }
        }

        public string HtmlResult
        {
            get { return htmlResult; }
        }
    }
}