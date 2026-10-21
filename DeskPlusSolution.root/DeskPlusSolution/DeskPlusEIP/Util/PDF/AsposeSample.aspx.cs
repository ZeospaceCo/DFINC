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
using System.IO;

namespace Zeospace.DeskPlus.Base.Web.Util.PDF
{
    public partial class AsposeSample : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExcelToJpg_Click(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Component.Converter.AsposeExcel xls = new Component.Converter.AsposeExcel();

            string org = tbOrginPath.Text;
            string path = tbTargetPath.Text;
            string filenm = tbTargetName.Text;

            ArrayList ar = xls.ExcelToJPG(org, path, filenm, true);
            if (ar != null)
            {
                for (int i = 0; i < ar.Count; i++)
                {
                    lbResult.Text += ar[i].ToString() + "</br>";
                }
            }
            else {
                lbResult.Text = "-1";
            }
        }

        protected void btnPdfToJpg_Click(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Component.Converter.AsposePDF pdf = new Component.Converter.AsposePDF();

            string org = tbOrginPath.Text;
            string path = tbTargetPath.Text;
            string filenm = tbTargetName.Text;

            ArrayList ar = pdf.PDFToJPG(org, path, filenm, true);
            if (ar != null)
            {
                for (int i = 0; i < ar.Count; i++)
                {
                    lbResult.Text += ar[i].ToString() + "</br>";
                }
            }
            else {
                lbResult.Text = "-1";
            }
        }

        protected void btnDocToTiff_Click(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Component.Converter.AsposeWord word = new Component.Converter.AsposeWord();

            string org = tbOrginPath.Text;
            string path = tbTargetPath.Text;
            string filenm = tbTargetName.Text;

            bool rtnval = word.DocToTiff(org, path+filenm, true);
            lbResult.Text = rtnval.ToString();
        }

        protected void btnPPTToTiff_Click(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Component.Converter.AsposePPT ppt = new Component.Converter.AsposePPT();

            string org = tbOrginPath.Text;
            string path = tbTargetPath.Text;
            string filenm = tbTargetName.Text;

            //bool rtnval = ppt.PPTToTiff(org, path+filenm, true);
            bool rtnval = false;
            lbResult.Text = rtnval.ToString();
        }

        protected void btnPptxToTiff_Click(object sender, EventArgs e)
        {
            Zeospace.DeskPlus.Component.Converter.AsposePPT ppt = new Component.Converter.AsposePPT();

            string org = tbOrginPath.Text;
            string path = tbTargetPath.Text;
            string filenm = tbTargetName.Text;

            //bool rtnval = ppt.PPTXToTiff(org, path + filenm, true);
            bool rtnval = false;
            lbResult.Text = rtnval.ToString();
        }

        protected void btnImgPDF_Click(object sender, EventArgs e)
        {
            string folderpath = tbOriginalFolder.Text;
            string targetfile = tbTargetFile.Text;

            Component.Converter.AsposePDF pdf = new Component.Converter.AsposePDF();
            bool rtnval = pdf.ImagesToPDF(folderpath, targetfile, true);
            lbResult2.Text = rtnval.ToString();
        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            string folderpath = tbOriginalFolder.Text;
            string targetfile = tbTargetFile.Text;

            if (Directory.Exists(folderpath))
            {
                string[] arrfiles = Directory.GetFiles(folderpath);

                foreach (string s in arrfiles)
                {
                    
                }

            }
        }

        
        
    }
}