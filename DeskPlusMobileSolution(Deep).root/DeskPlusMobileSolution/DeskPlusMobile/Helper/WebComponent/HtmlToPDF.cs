using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class HtmlToPDF : System.Windows.Forms.ApplicationContext
    {
        AutoResetEvent resultEvent; //리턴 이벤트
        string htmlResult;          //결과 값
        string filePath;            //생성되는 파일 Full 경로명
        string fileName;            //파일명
        string rotate;              //가로표시
        WebBrowser webBrowser;
        Thread thrd;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_source">HTML source</param>
        /// <param name="_filePath">파일 경로 d:\deskpluseip\filedata\</param>
        /// <param name="_fileName">파일명 ImageName(확장자 제외 jpg 로생성함)</param>
        /// <param name="_rotate">가로(V)/세로(H)</param>
        /// <param name="_resultEvent"></param>
        public HtmlToPDF(string _source, string _filePath, string _fileName, string _rotate, AutoResetEvent _resultEvent)
        {
            try
            {
                this.resultEvent = _resultEvent;
                htmlResult = null;
                filePath = _filePath;
                fileName = _fileName;
                rotate = _rotate;

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
                Helper.AppException.WriteException("WordToText : " + _source + " ==> " + _filePath + _fileName + " ==> " + ex.Message);
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

                string pdfName = filePath + fileName + ".pdf"; // c:\imsi\imageName.jpg
                Bitmap bitmap = new Bitmap(webBrowser.Document.Body.ScrollRectangle.Width, webBrowser.Document.Body.ScrollRectangle.Height);


                webBrowser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));

                iTextSharp.text.Rectangle rt;

                if (rotate.ToUpper() == "H")
                {
                    rt = PageSize.A4.Rotate();
                }
                else
                {
                    rt = PageSize.A4;
                }

                Document pdfDoc = new Document(rt, 10f, 10f, 10f, 0f);

                PdfWriter.GetInstance(pdfDoc, new FileStream(pdfName, FileMode.Create));

                pdfDoc.Open();

                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(bitmap, System.Drawing.Imaging.ImageFormat.Bmp);

                //pdfImage.ScaleToFit(400, 45);
                //pdfImage.ScalePercent(70f); 

                //pdfImage.ScaleToFit(193, 45);

                //pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
                //pdfImage.SetAbsolutePosition(180, 760);

                float lm = 10f;
                float rm = 10f;
                float tm = 10f;
                float bm = 0f;

                float pageWidth = pdfDoc.PageSize.Width - (lm + rm);
                float pageHeight = pdfDoc.PageSize.Height - (bm + tm);

                pdfImage.SetAbsolutePosition(lm, tm);
                pdfImage.ScaleToFit(pageWidth, pageHeight);
                pdfImage.Alignment = iTextSharp.text.Image.ALIGN_TOP;

                pdfDoc.Add(pdfImage);
                pdfDoc.Close();

                //bitmap.Save(imageName, System.Drawing.Imaging.ImageFormat.Jpeg);


                htmlResult = pdfName; // 성공시 생성된 이미지 경로 제공

                this.resultEvent.Set();
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException(ex.Message);
                htmlResult = "FAIL"; // 실패시 리턴

                this.resultEvent.Set();
            }
            finally
            {

            }
        }

        public string HtmlResult
        {
            get { return htmlResult; }
        }
    }
}