using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;


namespace Zeospace.DeskPlus.Base.Web.Util.PDF
{
    public partial class HtmlToPdf : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.tbInput.Text = basicData();
            }
           
        }

        protected void btnHtmlToPdf_Click(object sender, EventArgs e)
        {
            string htmlText = this.tbInput.Text;

            //HTMLToPdf3(htmlText, "ApprovalDoc001.pdf");
            //HtmlToImage(htmlText, "ApprovalDoc001.png");

            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            string filePath = Server.MapPath("/DeskPlusEIP/Util/PDF/");
            string fileName = "기안서_" + DateTime.Now.Millisecond;
            Zeospace.DeskPlus.Component.Converter.HtmlToPDF browser = new Component.Converter.HtmlToPDF(htmlText, filePath, fileName, "V", resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });
            
            // 이미지 생성시까지 대기 
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string  result = browser.HtmlResult; // 완료 이벤트 

            hlFilePath.Text = result;
            hlFilePath.NavigateUrl = "/DeskPlusEIP/Util/PDF/" + fileName + ".pdf";
            hlFilePath.Target = "_blank";

            browser.Dispose(); // 이미지 생성 쓰레드 Kill

        }


        protected void btnHtmlToImage_Click(object sender, EventArgs e)
        {
            string htmlText = this.tbInput.Text;

            //HTMLToPdf3(htmlText, "ApprovalDoc001.pdf");
            //HtmlToImage(htmlText, "ApprovalDoc001.png");

            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            string filePath = Server.MapPath("/DeskPlusEIP/Util/PDF/");
            string fileName = "기안서_" + DateTime.Now.Millisecond;
            Zeospace.DeskPlus.Component.Converter.HtmlToImage browser = new Component.Converter.HtmlToImage(htmlText, filePath, fileName, resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기 
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트 

            hlFilePath.Text = result;
            hlFilePath.NavigateUrl = "/DeskPlusEIP/Util/PDF/" + fileName + ".jpg";
            hlFilePath.Target = "_blank";

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }


        //public void HtmlToImage(string HTML, string FilePath)
        //{
        //    Bitmap m_Bitmap = new Bitmap(1024, 640);
        //    PointF point = new PointF(0, 0);
        //    SizeF maxSize = new System.Drawing.SizeF(1024, 1024);
        //    HtmlRenderer.HtmlRender.Render(Graphics.FromImage(m_Bitmap),
        //                                            HTML,
        //                                             point, maxSize);

        //    m_Bitmap.Save(Server.MapPath("/DeskPlusEIP/Util/PDF/" + FilePath), ImageFormat.Png);
        //}

     
        //protected void HTMLToPdf2(string HTML, string FilePath)
        //{

        //        /*
        //        var writer = PdfWriter.GetInstance(document, pdfOutput);
        //        var worker = XMLWorkerHelper.GetInstance();

        //        document.Open();
        //        worker.ParseXHtml(writer, document, htmlInput, null, Encoding.UTF8, new UnicodeFontFactory());

        //        document.Close();
        //         **/

        //        try
        //        {
        //            var bytes = Encoding.UTF8.GetBytes(HTML);
        //            using (var input = new MemoryStream(bytes))
        //            {
        //                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
        //                var writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("/DeskPlusEIP/Util/PDF/ApprovalDoc001.pdf"), FileMode.Create));
        //                writer.CloseStream = false;
        //                document.Open();

        //                var xmlWorker = XMLWorkerHelper.GetInstance();
        //                xmlWorker.ParseXHtml(writer, document, input, null, Encoding.UTF8, new UnicodeFontFactory());
        //                document.Close();
        //            }

        //            //ShowPdf("ApprovalDoc001.pdf");
        //        }
        //        catch (Exception ex)
        //        {
        //            string msg = ex.Message;
        //        }

        //        //ShowPdf("ApprovalDoc001.pdf");

        //}

        //protected void HTMLToPdf3(string HTML, string FilePath)
        //{
        //    string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");
        //    FontFactory.Register(FontPath);
        //    StyleSheet style = new StyleSheet();
        //    style.LoadTagStyle("body", "face", "Arial Unicode MS");
        //    style.LoadTagStyle("table", "font-size", "9px");
        //    style.LoadTagStyle("td", "font-size", "9px");
        //    style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
        //    using (Document document = new Document(PageSize.A4, 30, 30, 30, 30))
        //    {
        //        PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("/DeskPlusEIP/Util/PDF/ApprovalDoc001.pdf"), FileMode.Create));
        //        document.Open();
        //        foreach (IElement element in HTMLWorker.ParseToList(
        //            new StringReader(HTML.ToString()), style))
        //        {
        //            document.Add(element);
        //        }
        //    }

            /*
            using (var document = new Document(PageSize.A4, 30, 30, 30, 30))
            {
                /*
                var writer = PdfWriter.GetInstance(document, pdfOutput);
                var worker = XMLWorkerHelper.GetInstance();

                document.Open();
                worker.ParseXHtml(writer, document, htmlInput, null, Encoding.UTF8, new UnicodeFontFactory());

                document.Close();
                 **/

            //    try
            //    {
            //        PdfWriter.GetInstance(document, new FileStream(Server.MapPath("/DeskPlusEIP/Util/PDF/ApprovalDoc001.pdf"), FileMode.Create));

            //        string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),"arialuni.ttf");
            //        BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            //        document.Open();

            //        HTMLWorker hw = new HTMLWorker(document);
            //        hw.Parse(new StringReader(HTML));


            //        document.Close();

            //        ShowPdf("ApprovalDoc001.pdf");
            //    }
            //    catch (Exception ex)
            //    {
            //        string msg = ex.Message;
            //    }

            //    //ShowPdf("ApprovalDoc001.pdf");
            //}

        //}


//        public void HTMLToPdf(string HTML, string FilePath)
//        {
//            Document document = new Document();
//            //BaseFont bf = BaseFont.CreateFont("gulim.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

//            PdfWriter.GetInstance(document, new FileStream(Server.MapPath("/DeskPlusEIP/Util/PDF/ApprovalDoc001.pdf"), FileMode.Create));
//            document.Open();

//            //string GulimFont = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\gulim.ttc";
//            //FontFactory.Register(GulimFont);

//            string arialuniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
//            //Register the font with iTextSharp
//            iTextSharp.text.FontFactory.Register(arialuniTff);
            
///*
//            Image pdfImage = Image.GetInstance(Server.MapPath("/DeskPlusEIP/Util/PDF/logo.gif"));

//            pdfImage.ScaleToFit(193, 45);

//            pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; 
//            pdfImage.SetAbsolutePosition(180, 760);

//            document.Add(pdfImage);
// */
//            StyleSheet style = new iTextSharp.text.html.simpleparser.StyleSheet();
//            style.LoadTagStyle("body", "FONT-FAMILY", "Gulim");
//            style.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);

//            //HTMLWorker hw = new HTMLWorker(document);

//            foreach (IElement element in HTMLWorker.ParseToList(new StringReader(HTML.ToString()), style))
//            {
//                document.Add(element);
//            }
            
//            //hw.Parse(new StringReader(HTML));
//            document.Close();
//            ShowPdf("ApprovalDoc001.pdf");
//        }
//        private void ShowPdf(string s)
//        {
//            Response.ClearContent();
//            Response.ClearHeaders();
//            Response.AddHeader("Content-Disposition", "inline;filename=" + s);
//            Response.ContentType = "application/pdf";
//            Response.WriteFile(s);
//            Response.Flush();
//            Response.Clear();
//        }

        private string basicData()
        {
            string DocTest = "<table style=\"FONT-SIZE: 9pt\" cellspacing=\"0\" cellpadding=\"3\" width=\"648\" border=\"0\">";
            DocTest += "<tbody>";
            DocTest += "<tr height=\"25\">";
            DocTest += "<td style=\"BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 0px solid\" height=\"27\" width=\"479\" colspan=\"6\"><strong><span style=\"FONT-FAMILY: 맑은 고딕\">문서번호</span> : </strong><input id=\"NO\" value=\"기술본부-201309-휴가원-1\" class=\"formfieldinput\" contenteditable=\"false\" style=\"FONT-FAMILY: 맑은 고딕; WIDTH: 50%\" readonly=\"readonly\" unselectable=\"on\"><strong>　</strong>　</td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 0px solid\" width=\"182\" colspan=\"2\">";
            DocTest += "<p style=\"MARGIN-BOTTOM: 0pt; TEXT-ALIGN: right; MARGIN-TOP: 0pt; LINE-HEIGHT: 1\"><strong><span style=\"FONT-FAMILY: 맑은 고딕\">기안일 </span>: </strong><input id=\"DATE\" class=\"formfieldinput\" contenteditable=\"false\" style=\"HEIGHT: 0%; FONT-FAMILY: 맑은 고딕; TEXT-ALIGN: right; WIDTH: 50%\" readonly=\"readonly\" value=\"2013-09-04\" unselectable=\"on\"></p></td></tr>";
            DocTest += "<tr height=\"25\">";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; TEXT-ALIGN: center; BORDER-LEFT: #000000 1px solid\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"58\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong>소속부서</strong></span></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" height=\"28\" width=\"152\"><input id=\"UPDPT\" class=\"formfieldinput\" contenteditable=\"false\" style=\"FONT-FAMILY: 맑은 고딕; WIDTH: 95%\" readonly=\"readonly\" value=\"기술본부\" unselectable=\"on\">　</td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; FONT-WEIGHT: bolder; BORDER-LEFT: #000000 1px solid\" height=\"117\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" rowspan=\"4\" width=\"26\">";
            DocTest += "<p style=\"FONT-FAMILY: ; TEXT-ALIGN: center\" align=\"center\"><strong>승</strong></p>";
            DocTest += "<p style=\"FONT-FAMILY: ; TEXT-ALIGN: center\" align=\"center\">　</p>";
            DocTest += "<p style=\"FONT-FAMILY: ; TEXT-ALIGN: center\" align=\"center\"><strong>인</strong></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; BORDER-LEFT: #000000 1px solid\" height=\"28\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"71\">";
            DocTest += "<p style=\"FONT-FAMILY: ; TEXT-ALIGN: center\" align=\"center\"><strong>담당</strong></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; BORDER-LEFT: #000000 1px solid\" height=\"28\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"71\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong>";
            DocTest += "<p style=\"FONT-FAMILY: \" align=\"center\"><span id=\"ZD1_INFO9\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span></p></strong></span></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; BORDER-LEFT: #000000 1px solid\" height=\"28\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"71\" align=\"center\">";
            DocTest += "<p style=\"FONT-FAMILY: \" align=\"center\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong><span id=\"ZD2_INFO9\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span></strong></span></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; BORDER-LEFT: #000000 1px solid\" height=\"28\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"71\" align=\"center\">";
            DocTest += "<p style=\"FONT-FAMILY: \" align=\"center\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong><span id=\"ZD3_INFO9\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span></strong></span></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 1px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; TEXT-ALIGN: center; BORDER-LEFT: #000000 1px solid; WIDTH: 72px\" height=\"28\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" width=\"71\"><span id=\"ZD4_INFO9\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">대표이사</span>　</td></tr>";
            DocTest += "<tr height=\"25\">";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; TEXT-ALIGN: center; BORDER-LEFT: #000000 1px solid\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong>보 고 자</strong></span></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\"><input id=\"UPNM\" class=\"formfieldinput\" contenteditable=\"false\" style=\"FONT-FAMILY: 맑은 고딕; WIDTH: 30%\" readonly=\"readonly\" value=\"이현수\" unselectable=\"on\"></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" rowspan=\"2\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span id=\"ZD0\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\" isapp=\"true\" key=\"1_ZD0\" subtree=\"0\" isgroup=\"M\" dutyname=\"본부장\" dutyid=\"69\" titlename=\"이사\" titleid=\"9\" deptname=\"기술본부\" deptid=\"12\" userid=\"11\" isasign=\"true\" defaulttext=\"담당\"><img style=\"HEIGHT: 50px; WIDTH: 50px\" src=\"http://ep.zeospace.com/DeskPlusEIP/FileData/1/Approval/Document/201309/4475/ZD_sealZD0.gif\"></span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" rowspan=\"2\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span id=\"ZD1\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">/</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" rowspan=\"2\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span id=\"ZD2\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">/</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" rowspan=\"2\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span id=\"ZD3\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">/</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 1px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid\" rowspan=\"2\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span id=\"ZD4\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\" isapp=\"true\" key=\"5_ZD4\" subtree=\"0\" isgroup=\"M\" dutyname=\"대표이사\" dutyid=\"66\" titlename=\"사장\" titleid=\"5\" deptname=\"임원\" deptid=\"9\" userid=\"2\" isasign=\"true\" defaulttext=\"결재4\"><img style=\"HEIGHT: 50px; WIDTH: 50px\" src=\"http://ep.zeospace.com/DeskPlusEIP/FileData/1/Approval/Document/201309/4475/ZD_sealZD4.gif\"></span>　</p></td></tr>";
            DocTest += "<tr height=\"25\">";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; FONT-WEIGHT: bolder; TEXT-ALIGN: center; BORDER-LEFT: #000000 1px solid\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\" align=\"center\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span style=\"FONT-FAMILY: 맑은 고딕\"><strong>수신부서</strong></span></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid; WIDTH: 181px\" width=\"150\"><span id=\"ZP\" class=\"ZP\" contenteditable=\"false\" unselectable=\"on\"></span>　</td></tr>";
            DocTest += "<tr height=\"25\">";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; FONT-WEIGHT: bolder; TEXT-ALIGN: center; BORDER-LEFT: #000000 1px solid\" background=\"http://ep.zeospace.com/DeskPlusEIP/Design/C/img/Etc/background.gif\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><strong>참조부서</strong></p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; FONT-FAMILY: 맑은 고딕; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid; WIDTH: 181px\"><span id=\"ZC\" class=\"ZC\" contenteditable=\"false\" unselectable=\"on\" defaulttext=\"참조\"><span id=\"ZC01\" class=\"ZC\" key=\"7_ZC01\" subtree=\"0\" isgroup=\"M\" dutyname=\"실장\" dutyid=\"25\" titlename=\"실장\" titleid=\"13\" deptname=\"총무팀\" deptid=\"70\" userid=\"27\" isasign=\"true\" idx=\"1\">총무팀/실장/박지원,</span></span></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span id=\"ZD0_INFO14\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">09-04</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span id=\"ZD1_INFO14\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\" align=\"center\"><span id=\"ZD2_INFO14\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span id=\"ZD3_INFO14\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">&nbsp;</span>　</p></td>";
            DocTest += "<td style=\"BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid\">";
            DocTest += "<p style=\"TEXT-ALIGN: center\"><span id=\"ZD4_INFO14\" class=\"ZD\" contenteditable=\"false\" unselectable=\"on\">09-04</span>　</p></td></tr>";
            DocTest += "<tr>";
            DocTest += "<td height=\"2\" width=\"648\" colspan=\"7\"></td></tr></tbody></table>";

            return DocTest;
        }


    }

    //public class UnicodeFontFactory : FontFactoryImp
    //{
        //private static readonly string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
        //  "arialuni.ttf");

        //private readonly BaseFont _baseFont;

        //public UnicodeFontFactory()
        //{
        //    _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        //}

        //public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
        //  bool cached)
        //{
        //    return new Font(_baseFont, size, style, color);
        //}
    //}
}