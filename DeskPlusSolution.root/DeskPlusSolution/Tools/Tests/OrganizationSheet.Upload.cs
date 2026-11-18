using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.Web.Organization;

// Standalone regression runner; --render also produces a read-only browser-preview fixture.
public static class OrganizationSheetUploadTests
{
    static readonly XNamespace MainNs = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    static void Check(bool value, string message) { if (!value) throw new Exception(message); }
    static OrganizationSheetDocument Read(byte[] bytes, string name = "organization.xlsx")
    {
        using (var stream = new MemoryStream(bytes)) return OrganizationSheetExcelImporter.Import(stream, name);
    }
    static byte[] Change(byte[] bytes, string path, Action<XDocument> update)
    {
        using (var stream = new MemoryStream())
        {
            stream.Write(bytes, 0, bytes.Length); stream.Position = 0;
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Update, true))
            {
                var entry = zip.GetEntry(path); XDocument xml;
                using (var input = entry.Open()) xml = XDocument.Load(input);
                update(xml); entry.Delete();
                using (var output = zip.CreateEntry(path).Open()) xml.Save(output);
            }
            return stream.ToArray();
        }
    }
    static void Reject(byte[] bytes, string name = "organization.xlsx")
    {
        try { Read(bytes, name); }
        catch (ArgumentException) { return; }
        throw new Exception("Invalid workbook was accepted.");
    }
    static string Cell(OrganizationSheetDocument doc, int row, int column)
    {
        return doc.cells.Single(c => c.r == row - 1 && c.c == column - 1).text;
    }
    public static void Main(string[] args)
    {
        try
        {
            bool render = args[0] == "--render";
            string source = render ? args[1] : args[0];
            byte[] bytes;
            using (var input = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var buffer = new MemoryStream()) { input.CopyTo(buffer); bytes = buffer.ToArray(); }
            var document = Read(bytes, Path.GetFileName(source));
            if (render)
            {
                File.WriteAllText(args[2], OrganizationSheetLayout.SafeJson(document), new UTF8Encoding(false));
                return;
            }
            Check(document.rows.Count == 31 && document.columns.Count == 50, "Print area mismatch");
            Check(document.images.Count == 1 && document.lines.Count == 20, "Visible logo/connectors mismatch");
            Check(document.cells.Any(c => c.cs > 1 || c.rs > 1), "Merged cells missing");
            Check(document.sourceSheetName == "조직도" && document.sourceFileName == Path.GetFileName(source), "Source metadata missing");
            Check(Cell(document, 3, 47).Trim() == "총 101명", "Cached total/unit formatting mismatch: " + Cell(document, 3, 47));
            Check(Cell(document, 11, 4) == "10명", "Department formatting mismatch");
            Check(Cell(document, 1, 45) == "2026-11-02", "Date formatting mismatch: " + Cell(document, 1, 45));
            Check(document.cells.All(c => String.IsNullOrEmpty(c.formula)), "Uploaded formulas must use saved display values");
            Check(document.styles.All(s => s.color != "#windowText" && s.fill != "#window"), "Invalid theme colors");
            var edited = Change(bytes, "xl/worksheets/sheet2.xml", xml => {
                var cell = xml.Descendants(MainNs + "c").Single(c => (string)c.Attribute("r") == "AU3");
                cell.Element(MainNs + "v").Value = "102";
            });
            Check(Cell(Read(edited), 3, 47).Trim() == "총 102명", "Updated workbook did not update the display");
            Reject(Change(bytes, "xl/worksheets/sheet2.xml", xml => xml.Descendants(MainNs + "c").Single(c => (string)c.Attribute("r") == "AU3").Element(MainNs + "v").Remove()));
            Reject(Change(bytes, "xl/workbook.xml", xml => xml.Descendants(MainNs + "definedName").Single(c => (string)c.Attribute("name") == "_xlnm.Print_Area" && (string)c.Attribute("localSheetId") == "1").Value = "조직도!$A$1:$AX$31,조직도!$A$35:$AX$40"));
            Reject(Change(bytes, "xl/workbook.xml", xml => xml.Descendants(MainNs + "definedName").Single(c => (string)c.Attribute("name") == "_xlnm.Print_Area" && (string)c.Attribute("localSheetId") == "1").Value = "조직도!$A$1:$ZZ$300"));
            Reject(Change(bytes, "xl/workbook.xml", xml => { foreach (var sheet in xml.Descendants(MainNs + "sheet")) sheet.SetAttributeValue("state", "hidden"); }));
            var renamed = Change(bytes, "xl/workbook.xml", xml => {
                xml.Descendants(MainNs + "sheet").Single(s => (string)s.Attribute("name") == "조직도").SetAttributeValue("name", "현황");
                foreach (var area in xml.Descendants(MainNs + "definedName")) area.Value = area.Value.Replace("조직도!", "현황!").Replace("'조직도'!", "'현황'!");
            });
            Check(Read(renamed).sourceSheetName == "현황", "Active visible sheet fallback failed");
            Reject(new byte[] { 1, 2, 3 }); Reject(new byte[0]); Reject(bytes, "bad.xls"); Reject(new byte[OrganizationSheetExcelImporter.MaxUploadBytes + 1]);
            Console.WriteLine("XLSX print range, merges, theme, cached formulas, date/units, edited values, logo/connectors, visible-sheet selection and invalid-upload checks passed.");
        }
        catch (Exception ex) { Console.WriteLine(ex.GetType().FullName + ": " + ex.Message); Environment.ExitCode = 1; }
    }
}
