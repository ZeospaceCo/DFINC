using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class AsposeLicense
    {
        public AsposeLicense()
        {
            Aspose.BarCode.License licBarCode = new Aspose.BarCode.License();
            licBarCode.SetLicense("Aspose.Total.lic");

            Aspose.Cells.License licCells = new Aspose.Cells.License();
            licCells.SetLicense("Aspose.Total.lic");

            Aspose.Diagram.License licDiagram = new Aspose.Diagram.License();
            licDiagram.SetLicense("Aspose.Total.lic");

            Aspose.Email.License licEmail = new Aspose.Email.License();
            licEmail.SetLicense("Aspose.Total.lic");

            Aspose.Imaging.License licImaging = new Aspose.Imaging.License();
            licImaging.SetLicense("Aspose.Total.lic");

            Aspose.Note.License licNote = new Aspose.Note.License();
            licNote.SetLicense("Aspose.Total.lic");

            Aspose.OCR.License licOCR = new Aspose.OCR.License();
            licOCR.SetLicense("Aspose.Total.lic");

            Aspose.Pdf.License licPdf = new Aspose.Pdf.License();
            licPdf.SetLicense("Aspose.Total.lic");

            Aspose.Slides.License licSlides = new Aspose.Slides.License();
            licSlides.SetLicense("Aspose.Total.lic");

            Aspose.Tasks.License licTasks = new Aspose.Tasks.License();
            licTasks.SetLicense("Aspose.Total.lic");

            Aspose.Words.License licWords = new Aspose.Words.License();
            licWords.SetLicense("Aspose.Total.lic");
        }
    }
}