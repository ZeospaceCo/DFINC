using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Organization
{
    /// <summary>Imports a displayed worksheet from an XLSX package without running Excel or macros.</summary>
    public sealed class OrganizationSheetExcelImporter
    {
        public const int MaxUploadBytes = 10 * 1024 * 1024;
        private static readonly XNamespace Main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private static readonly XNamespace Drawing = "http://schemas.openxmlformats.org/drawingml/2006/main";
        private static readonly XNamespace SheetDrawing = "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing";
        private static readonly XNamespace Relationship = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        private readonly Dictionary<string, ZipArchiveEntry> entries;
        private readonly List<string> theme = new List<string> { "#ffffff", "#000000", "#eeece1", "#1f497d", "#4f81bd", "#c0504d", "#9bbb59", "#8064a2", "#4bacc6", "#f79646" };
        private readonly List<string> indexed = new List<string> { "#000000", "#ffffff", "#ff0000", "#00ff00", "#0000ff", "#ffff00", "#ff00ff", "#00ffff" };
        private List<XElement> fonts, fills, borders, formats;
        private readonly Dictionary<int, string> numberFormats = new Dictionary<int, string>();
        private List<string> strings;
        private bool date1904;

        private OrganizationSheetExcelImporter(ZipArchive archive)
        {
            Require(archive.Entries.Count <= 2000 && archive.Entries.Sum(e => e.Length) <= 64L * 1024 * 1024, "엑셀 파일의 압축 해제 용량이 너무 큽니다.");
            entries = new Dictionary<string, ZipArchiveEntry>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.EndsWith("/", StringComparison.Ordinal)) continue;
                string name = Resolve("", entry.FullName);
                Require(!entries.ContainsKey(name), "엑셀 파일 안에 중복된 항목이 있습니다.");
                entries.Add(name, entry);
            }
        }

        public static OrganizationSheetDocument Import(Stream input, string fileName)
        {
            Require(String.Equals(Path.GetExtension(fileName), ".xlsx", StringComparison.OrdinalIgnoreCase), "엑셀에서 .xlsx 형식으로 저장한 파일을 선택해 주세요.");
            Require(input != null && input.CanRead, "업로드한 파일을 읽을 수 없습니다.");
            using (var buffer = new MemoryStream())
            {
                var bytes = new byte[81920];
                int count;
                while ((count = input.Read(bytes, 0, bytes.Length)) > 0)
                {
                    Require(buffer.Length + count <= MaxUploadBytes, "엑셀 파일은 10MB 이내로 업로드해 주세요.");
                    buffer.Write(bytes, 0, count);
                }
                Require(buffer.Length > 0, "비어 있는 파일은 업로드할 수 없습니다.");
                buffer.Position = 0;
                try
                {
                    using (var archive = new ZipArchive(buffer, ZipArchiveMode.Read, true))
                        return new OrganizationSheetExcelImporter(archive).Read(Path.GetFileName(fileName));
                }
                catch (InvalidDataException) { throw new ArgumentException("파일이 올바른 .xlsx 형식이 아니거나 손상되었습니다."); }
                catch (XmlException) { throw new ArgumentException("엑셀 파일의 내용을 읽을 수 없습니다. 엑셀에서 다시 저장한 후 업로드해 주세요."); }
                catch (FormatException) { throw new ArgumentException("엑셀의 셀 값이나 서식 정보가 올바르지 않습니다."); }
                catch (OverflowException) { throw new ArgumentException("엑셀의 셀 범위나 숫자가 너무 큽니다."); }
            }
        }

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new ArgumentException(message);
        }

        private static string Resolve(string owner, string target)
        {
            Require(!String.IsNullOrEmpty(target) && !target.Contains(":") && !target.Contains("\\"), "외부 파일을 참조하는 엑셀 항목은 사용할 수 없습니다.");
            target = Uri.UnescapeDataString(target);
            var parts = new List<string>();
            if (!target.StartsWith("/", StringComparison.Ordinal) && owner.Contains("/")) parts.AddRange(owner.Substring(0, owner.LastIndexOf('/')).Split('/'));
            foreach (string part in target.TrimStart('/').Split('/'))
            {
                if (part == "" || part == ".") continue;
                if (part == "..") { Require(parts.Count > 0, "엑셀 파일의 경로가 올바르지 않습니다."); parts.RemoveAt(parts.Count - 1); }
                else parts.Add(part);
            }
            return String.Join("/", parts);
        }

        private XDocument Xml(string path, bool optional = false)
        {
            ZipArchiveEntry entry;
            if (!entries.TryGetValue(path, out entry)) { Require(optional, "엑셀 파일에 필요한 항목이 없습니다: " + path); return null; }
            Require(entry.Length <= 20 * 1024 * 1024, "엑셀 시트의 데이터가 너무 큽니다.");
            using (var stream = entry.Open())
            using (var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 20 * 1024 * 1024 }))
                return XDocument.Load(reader);
        }

        private Dictionary<string, string> Relationships(string owner)
        {
            string path = owner.Substring(0, owner.LastIndexOf('/') + 1) + "_rels/" + Path.GetFileName(owner) + ".rels";
            var xml = Xml(path, true);
            var result = new Dictionary<string, string>();
            if (xml != null) foreach (var relation in xml.Root.Elements())
                if ((string)relation.Attribute("TargetMode") != "External") result[(string)relation.Attribute("Id")] = Resolve(owner, (string)relation.Attribute("Target"));
            return result;
        }

        private static int Integer(XElement element, string attribute, int fallback = 0)
        {
            return element == null || element.Attribute(attribute) == null ? fallback : Int32.Parse((string)element.Attribute(attribute), CultureInfo.InvariantCulture);
        }
        private static double Number(XElement element, string attribute, double fallback = 0)
        {
            return element == null || element.Attribute(attribute) == null ? fallback : Double.Parse((string)element.Attribute(attribute), CultureInfo.InvariantCulture);
        }
        private static string Text(XElement element)
        {
            if (element == null) return "";
            return String.Concat(element.Elements().SelectMany(child => child.Name == Main + "t" ? new[] { child.Value } : child.Name == Main + "r" ? child.Elements(Main + "t").Select(t => t.Value) : Enumerable.Empty<string>()));
        }
        private static string Key(int row, int column) { return row + ":" + column; }
        private static int[] Address(string value)
        {
            var match = Regex.Match(value ?? "", @"^\$?([A-Z]+)\$?([0-9]+)$");
            Require(match.Success, "엑셀의 셀 주소가 올바르지 않습니다.");
            int column = 0;
            foreach (char letter in match.Groups[1].Value) column = checked(column * 26 + letter - 'A' + 1);
            return new[] { Int32.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) - 1, column - 1 };
        }

        private string Color(XElement element, string fallback)
        {
            if (element == null) return fallback;
            string result = fallback;
            string rgb = (string)element.Attribute("rgb");
            if (!String.IsNullOrEmpty(rgb) && Regex.IsMatch(rgb, "^[0-9a-fA-F]{6}([0-9a-fA-F]{2})?$")) result = "#" + rgb.Substring(rgb.Length - 6);
            else if (element.Attribute("theme") != null && Integer(element, "theme") < theme.Count) result = theme[Integer(element, "theme")];
            else if (element.Attribute("indexed") != null)
            {
                int index = Integer(element, "indexed");
                result = index < indexed.Count ? indexed[index] : index == 65 ? "#ffffff" : index == 64 ? "#000000" : fallback;
            }
            double tint = Number(element, "tint");
            if (tint != 0)
            {
                var channels = Enumerable.Range(0, 3).Select(i => Int32.Parse(result.Substring(1 + i * 2, 2), NumberStyles.HexNumber)).Select(c => Math.Max(0, Math.Min(255, (int)Math.Round(tint < 0 ? c * (1 + tint) : c * (1 - tint) + 255 * tint))));
                result = "#" + String.Concat(channels.Select(c => c.ToString("x2")));
            }
            return result;
        }

        private OrganizationSheetStyle Style(XElement format)
        {
            int fontIndex = Integer(format, "fontId"), fillIndex = Integer(format, "fillId"), borderIndex = Integer(format, "borderId");
            Require(fontIndex >= 0 && fontIndex < fonts.Count && fillIndex >= 0 && fillIndex < fills.Count && borderIndex >= 0 && borderIndex < borders.Count, "엑셀의 서식 참조가 올바르지 않습니다.");
            var font = fonts[fontIndex];
            var fill = fills[fillIndex].Element(Main + "patternFill");
            var alignment = format.Element(Main + "alignment");
            var style = new OrganizationSheetStyle
            {
                fontSize = Math.Round(Number(font.Element(Main + "sz"), "val", 11) * 4 / 3, 2),
                bold = font.Element(Main + "b") != null && (string)font.Element(Main + "b").Attribute("val") != "0",
                color = Color(font.Element(Main + "color"), "#000000"),
                fill = fill != null && (string)fill.Attribute("patternType") == "solid" ? Color(fill.Element(Main + "fgColor"), "#ffffff") : "#ffffff",
                align = (string)(alignment == null ? null : alignment.Attribute("horizontal")) ?? "center",
                vertical = (string)(alignment == null ? null : alignment.Attribute("vertical")) ?? "center"
            };
            if (!new[] { "left", "center", "right" }.Contains(style.align)) style.align = "center";
            if (!new[] { "top", "center", "bottom" }.Contains(style.vertical)) style.vertical = "center";
            Func<string, OrganizationSheetBorder> edge = side =>
            {
                var border = borders[borderIndex].Element(Main + side);
                string kind = border == null ? null : (string)border.Attribute("style");
                return new OrganizationSheetBorder { width = String.IsNullOrEmpty(kind) ? 0 : new[] { "medium", "thick", "double" }.Contains(kind) ? 2 : 1, color = Color(border == null ? null : border.Element(Main + "color"), "#000000") };
            };
            style.top = edge("top"); style.right = edge("right"); style.bottom = edge("bottom"); style.left = edge("left");
            return style;
        }

        private string Display(XElement cell, int styleIndex)
        {
            if (cell == null) return "";
            string type = (string)cell.Attribute("t");
            var value = cell.Element(Main + "v");
            if (cell.Element(Main + "f") != null) Require(value != null && (value.Value != "" || type == "str"), "계산값이 없는 수식이 있습니다. 엑셀에서 계산 후 저장한 파일을 다시 업로드해 주세요.");
            Require(type != "e", "조직도 시트에 수식 오류가 있습니다. 엑셀에서 오류를 수정한 후 업로드해 주세요.");
            if (type == "inlineStr") return Text(cell.Element(Main + "is"));
            if (type == "s") { int index = value == null ? -1 : Int32.Parse(value.Value, CultureInfo.InvariantCulture); Require(index >= 0 && index < strings.Count, "엑셀의 문자 참조가 올바르지 않습니다."); return strings[index]; }
            if (type == "b") return value != null && value.Value == "1" ? "TRUE" : "FALSE";
            if (value == null || value.Value == "") return "";
            if (type == "str") return value.Value;
            double numeric;
            if (!Double.TryParse(value.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out numeric)) return value.Value;
            int formatId = Integer(formats[styleIndex], "numFmtId");
            string format;
            numberFormats.TryGetValue(formatId, out format);
            format = format ?? "";
            string unquoted = Regex.Replace(format, "\"[^\"]*\"", "");
            if (formatId >= 14 && formatId <= 22 || formatId >= 27 && formatId <= 36 || Regex.IsMatch(unquoted, "[ymd]", RegexOptions.IgnoreCase))
                return (date1904 ? new DateTime(1904, 1, 1).AddDays(numeric) : DateTime.FromOADate(numeric)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string section = format.Split(';')[0];
            var number = Regex.Match(section, "[0#?][0#?,.]*");
            if (!number.Success) return numeric.ToString("G15", CultureInfo.InvariantCulture);
            string before = section.Substring(0, number.Index), after = section.Substring(number.Index + number.Length);
            Func<string, string> literal = part => Regex.Replace(Regex.Replace(part, @"_(.)|\*(.)", ""), "\"([^\"]*)\"|\\\\(.)", match => match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value);
            int dot = number.Value.IndexOf('.');
            int decimals = dot < 0 ? 0 : number.Value.Substring(dot + 1).Count(c => c == '0' || c == '#');
            bool percentage = after.Contains("%");
            string numericFormat = number.Value.Contains(",") ? "N" : "F";
            string displayed = (percentage ? numeric * 100 : numeric).ToString(numericFormat + Math.Min(decimals, 8), CultureInfo.InvariantCulture);
            return literal(before) + displayed + literal(after);
        }

        private OrganizationSheetDocument Read(string fileName)
        {
            var workbook = Xml("xl/workbook.xml");
            var sheets = workbook.Root.Element(Main + "sheets").Elements(Main + "sheet").ToList();
            var visible = sheets.Where(s => (string)s.Attribute("state") != "hidden" && (string)s.Attribute("state") != "veryHidden").ToList();
            Require(visible.Count > 0, "표시할 수 있는 시트가 없습니다.");
            var selected = visible.FirstOrDefault(s => (string)s.Attribute("name") == "조직도");
            int active = Integer(workbook.Root.Element(Main + "bookViews") == null ? null : workbook.Root.Element(Main + "bookViews").Element(Main + "workbookView"), "activeTab");
            if (selected == null) selected = active >= 0 && active < sheets.Count && visible.Contains(sheets[active]) ? sheets[active] : visible[0];
            string sheetName = (string)selected.Attribute("name"), sheetPath;
            Require(Relationships("xl/workbook.xml").TryGetValue((string)selected.Attribute(Relationship + "id"), out sheetPath), "엑셀 시트의 위치를 확인할 수 없습니다.");
            var sheet = Xml(sheetPath);
            var printArea = workbook.Root.Element(Main + "definedNames") == null ? null : workbook.Root.Element(Main + "definedNames").Elements(Main + "definedName").FirstOrDefault(n => (string)n.Attribute("name") == "_xlnm.Print_Area" && Integer(n, "localSheetId", -1) == sheets.IndexOf(selected));
            string range;
            if (printArea == null)
            {
                var dimension = sheet.Root.Element(Main + "dimension");
                Require(dimension != null && dimension.Attribute("ref") != null, "조직도 시트의 범위를 확인할 수 없습니다. 엑셀에서 인쇄 영역을 지정해 주세요.");
                range = (string)dimension.Attribute("ref");
            }
            else
            {
                var area = Regex.Match(printArea.Value, @"^(?:'(?:[^']|'')+'|[^!,]+)!(?<range>\$?[A-Z]+\$?\d+(?::\$?[A-Z]+\$?\d+)?)$");
                Require(area.Success, "조직도 시트의 인쇄 영역을 하나의 사각형 범위로 지정해 주세요.");
                range = area.Groups["range"].Value;
            }
            Require(!range.Contains(","), "조직도 시트의 인쇄 영역을 하나의 사각형 범위로 지정해 주세요.");
            string[] endpoints = range.Split(':');
            var start = Address(endpoints[0]); var end = Address(endpoints.Length == 1 ? endpoints[0] : endpoints[1]);
            int rowCount = end[0] - start[0] + 1, columnCount = end[1] - start[1] + 1;
            Require(start[0] >= 0 && start[1] >= 0 && rowCount >= 1 && rowCount <= 200 && columnCount >= 1 && columnCount <= 100, "조직도 범위는 200행·100열 이내여야 합니다. 엑셀의 인쇄 영역을 조직도 부분으로 지정해 주세요.");
            var styleXml = Xml("xl/styles.xml");
            fonts = styleXml.Root.Element(Main + "fonts").Elements().ToList(); fills = styleXml.Root.Element(Main + "fills").Elements().ToList(); borders = styleXml.Root.Element(Main + "borders").Elements().ToList(); formats = styleXml.Root.Element(Main + "cellXfs").Elements().ToList();
            Require(formats.Count > 0 && formats.Count <= 4096, "엑셀의 서식 종류가 너무 많습니다.");
            var customFormats = styleXml.Root.Element(Main + "numFmts");
            if (customFormats != null) foreach (var format in customFormats.Elements()) numberFormats[Integer(format, "numFmtId")] = (string)format.Attribute("formatCode");
            var themeXml = Xml("xl/theme/theme1.xml", true);
            if (themeXml != null)
            {
                var palette = themeXml.Descendants(Drawing + "clrScheme").FirstOrDefault();
                if (palette != null)
                {
                    var themeColors = palette.Elements().Select(e => e.Elements().First()).Select(c => "#" + ((string)c.Attribute("lastClr") ?? (string)c.Attribute("val"))).ToList();
                    if (themeColors.Count >= 4) { string swap = themeColors[0]; themeColors[0] = themeColors[1]; themeColors[1] = swap; swap = themeColors[2]; themeColors[2] = themeColors[3]; themeColors[3] = swap; theme.Clear(); theme.AddRange(themeColors); }
                }
            }
            var indexedColors = styleXml.Descendants(Main + "indexedColors").FirstOrDefault();
            if (indexedColors != null) { indexed.Clear(); indexed.AddRange(indexedColors.Elements().Select(c => "#" + ((string)c.Attribute("rgb")).Substring(2))); }
            var shared = Xml("xl/sharedStrings.xml", true);
            strings = shared == null ? new List<string>() : shared.Root.Elements(Main + "si").Select(Text).ToList();
            date1904 = (string)(workbook.Root.Element(Main + "workbookPr") == null ? null : workbook.Root.Element(Main + "workbookPr").Attribute("date1904")) == "1";
            var rowDefinitions = sheet.Descendants(Main + "row").ToDictionary(r => Integer(r, "r") - 1);
            var columnDefinitions = sheet.Descendants(Main + "col").ToList();
            var sourceCells = new Dictionary<string, XElement>();
            foreach (var cell in sheet.Descendants(Main + "c")) { var address = Address((string)cell.Attribute("r")); sourceCells[Key(address[0], address[1])] = cell; Require(sourceCells.Count <= 100000, "시트에 데이터가 너무 많습니다. 조직도만 별도 시트에 저장해 주세요."); }
            var document = new OrganizationSheetDocument { schemaVersion = 1, title = sheetName, sourceFileName = fileName, sourceSheetName = sheetName, columns = new List<double>(), rows = new List<double>(), styles = formats.Select(Style).ToList(), cells = new List<OrganizationSheetCell>(), images = new List<OrganizationSheetImage>(), lines = new List<OrganizationSheetLine>() };
            for (int c = start[1]; c <= end[1]; c++)
            {
                var definition = columnDefinitions.LastOrDefault(col => Integer(col, "min") <= c + 1 && Integer(col, "max") >= c + 1);
                Require(definition == null || (string)definition.Attribute("hidden") != "1", "조직도 인쇄 영역 안에 숨긴 열이 있습니다. 표시할 열만 남겨 저장한 후 업로드해 주세요.");
                document.columns.Add(Math.Floor(Number(definition, "width", 8.43) * 7 + 5));
            }
            for (int r = start[0]; r <= end[0]; r++)
            {
                XElement definition; rowDefinitions.TryGetValue(r, out definition);
                Require(definition == null || (string)definition.Attribute("hidden") != "1", "조직도 인쇄 영역 안에 숨긴 행이 있습니다. 표시할 행만 남겨 저장한 후 업로드해 주세요.");
                document.rows.Add(Math.Round(Number(definition, "ht", 15) * 4 / 3, 2));
            }
            var merged = new Dictionary<string, int[]>(); var covered = new HashSet<string>();
            foreach (var merge in sheet.Descendants(Main + "mergeCell"))
            {
                var bounds = ((string)merge.Attribute("ref")).Split(':'); var first = Address(bounds[0]); var last = Address(bounds[1]);
                if (last[0] < start[0] || first[0] > end[0] || last[1] < start[1] || first[1] > end[1]) continue;
                Require(first[0] >= start[0] && first[1] >= start[1] && last[0] <= end[0] && last[1] <= end[1], "인쇄 영역의 경계에 걸친 병합 셀이 있습니다. 병합 범위를 포함해 인쇄 영역을 지정해 주세요.");
                merged[Key(first[0], first[1])] = new[] { last[0] - first[0] + 1, last[1] - first[1] + 1 };
                for (int r = first[0]; r <= last[0]; r++) for (int c = first[1]; c <= last[1]; c++) if (r != first[0] || c != first[1]) covered.Add(Key(r, c));
            }
            Func<int, int, int> styleAt = (r, c) =>
            {
                XElement cell; sourceCells.TryGetValue(Key(r, c), out cell);
                var col = columnDefinitions.LastOrDefault(def => Integer(def, "min") <= c + 1 && Integer(def, "max") >= c + 1);
                XElement row; rowDefinitions.TryGetValue(r, out row);
                int index = Integer(cell, "s", Integer(row, "s", Integer(col, "style")));
                Require(index >= 0 && index < formats.Count, "셀의 서식 참조가 올바르지 않습니다.");
                return index;
            };
            for (int r = start[0]; r <= end[0]; r++) for (int c = start[1]; c <= end[1]; c++)
            {
                if (covered.Contains(Key(r, c))) continue;
                XElement cell; sourceCells.TryGetValue(Key(r, c), out cell);
                int originalStyle = styleAt(r, c), styleIndex = originalStyle; int[] span;
                if (!merged.TryGetValue(Key(r, c), out span)) span = new[] { 1, 1 };
                if (span[0] > 1 || span[1] > 1)
                {
                    var style = Style(formats[originalStyle]);
                    for (int rr = r; rr < r + span[0]; rr++) { var edge = document.styles[styleAt(rr, c + span[1] - 1)].right; if (edge.width > style.right.width) style.right = edge; }
                    for (int cc = c; cc < c + span[1]; cc++) { var edge = document.styles[styleAt(r + span[0] - 1, cc)].bottom; if (edge.width > style.bottom.width) style.bottom = edge; }
                    styleIndex = document.styles.Count; document.styles.Add(style);
                }
                document.cells.Add(new OrganizationSheetCell { r = r - start[0], c = c - start[1], rs = span[0], cs = span[1], text = Display(cell, originalStyle), formula = "", prefix = "", suffix = "", style = styleIndex });
            }
            ReadDrawings(sheet, sheetPath, start, end, document);
            Require(document.cells.Any(cell => !String.IsNullOrWhiteSpace(cell.text)) || document.images.Count > 0, "선택된 시트에 표시할 내용이 없습니다. 조직도 시트를 확인한 후 업로드해 주세요.");
            OrganizationSheetLayout.Validate(document);
            Require(OrganizationSheetLayout.Serializer().Serialize(document).Length <= OrganizationSheetLayout.MaxJsonLength, "변환한 조직도의 용량이 너무 큽니다. 이미지 용량을 줄인 후 업로드해 주세요.");
            return document;
        }

        private void ReadDrawings(XDocument sheet, string sheetPath, int[] start, int[] end, OrganizationSheetDocument document)
        {
            var drawingReference = sheet.Descendants(Main + "drawing").FirstOrDefault();
            if (drawingReference == null) return;
            string path;
            Require(Relationships(sheetPath).TryGetValue((string)drawingReference.Attribute(Relationship + "id"), out path), "조직도 이미지의 위치를 확인할 수 없습니다.");
            var relationships = Relationships(path); int index = 0;
            Func<XElement, OrganizationSheetAnchor> anchor = point => new OrganizationSheetAnchor
            {
                r = Int32.Parse(point.Element(SheetDrawing + "row").Value, CultureInfo.InvariantCulture) - start[0],
                c = Int32.Parse(point.Element(SheetDrawing + "col").Value, CultureInfo.InvariantCulture) - start[1],
                dx = Math.Round(Double.Parse(point.Element(SheetDrawing + "colOff").Value, CultureInfo.InvariantCulture) / 9525, 2),
                dy = Math.Round(Double.Parse(point.Element(SheetDrawing + "rowOff").Value, CultureInfo.InvariantCulture) / 9525, 2)
            };
            Func<OrganizationSheetAnchor, bool> inside = point => point.r >= 0 && point.c >= 0 && point.r <= document.rows.Count && point.c <= document.columns.Count;
            foreach (var item in Xml(path).Root.Elements())
            {
                index++;
                var origin = item.Element(SheetDrawing + "from");
                Require(origin != null, "절대 위치 도형은 지원하지 않습니다. 셀에 연결된 이미지와 직선 연결선으로 저장해 주세요.");
                var from = anchor(origin);
                if (!inside(from)) continue;
                var picture = item.Element(SheetDrawing + "pic");
                if (picture != null)
                {
                    var blip = picture.Descendants(Drawing + "blip").FirstOrDefault(); string imagePath;
                    Require(blip != null && blip.Attribute(Relationship + "embed") != null && relationships.TryGetValue((string)blip.Attribute(Relationship + "embed"), out imagePath), "외부 링크 이미지는 사용할 수 없습니다. 엑셀 파일 안에 이미지를 삽입해 주세요.");
                    imagePath = relationships[(string)blip.Attribute(Relationship + "embed")];
                    ZipArchiveEntry entry;
                    Require(entries.TryGetValue(imagePath, out entry) && entry.Length <= 1000000, "이미지 한 개의 용량은 1MB 이내여야 합니다.");
                    Require(imagePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || imagePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || imagePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase), "조직도 이미지는 PNG 또는 JPEG 형식으로 삽입해 주세요.");
                    double width, height; var extent = item.Element(SheetDrawing + "ext");
                    if (extent == null) extent = picture.Descendants(Drawing + "xfrm").Elements(Drawing + "ext").FirstOrDefault();
                    Require(extent != null, "이미지 크기를 확인할 수 없습니다.");
                    width = Number(extent, "cx") / 9525; height = Number(extent, "cy") / 9525;
                    using (var data = new MemoryStream()) using (var stream = entry.Open())
                    {
                        var buffer = new byte[81920]; int count;
                        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            Require(data.Length + count <= 1000000, "이미지 한 개의 용량은 1MB 이내여야 합니다.");
                            data.Write(buffer, 0, count);
                        }
                        document.images.Add(new OrganizationSheetImage { id = "image-" + index, anchor = from, width = Math.Round(width, 2), height = Math.Round(height, 2), src = "data:image/" + (imagePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? "png" : "jpeg") + ";base64," + Convert.ToBase64String(data.ToArray()) });
                    }
                }
                else if (item.Element(SheetDrawing + "cxnSp") != null)
                {
                    var connector = item.Element(SheetDrawing + "cxnSp"); var target = item.Element(SheetDrawing + "to");
                    var geometry = connector.Descendants(Drawing + "prstGeom").FirstOrDefault();
                    Require(target != null && geometry != null && (string)geometry.Attribute("prst") == "line", "직선이 아닌 연결 도형은 지원하지 않습니다. 직선 연결선으로 저장해 주세요.");
                    var to = anchor(target); if (!inside(to)) continue;
                    var transform = connector.Descendants(Drawing + "xfrm").FirstOrDefault();
                    if (transform != null && (string)transform.Attribute("flipV") == "1") { int r = from.r; double dy = from.dy; from.r = to.r; from.dy = to.dy; to.r = r; to.dy = dy; }
                    if (transform != null && (string)transform.Attribute("flipH") == "1") { int c = from.c; double dx = from.dx; from.c = to.c; from.dx = to.dx; to.c = c; to.dx = dx; }
                    var line = connector.Descendants(Drawing + "ln").FirstOrDefault();
                    document.lines.Add(new OrganizationSheetLine { id = "line-" + index, start = from, end = to, width = Math.Round(Number(line, "w", 12700) / 9525, 2), color = "#000000" });
                }
                else throw new ArgumentException("현재 조직도에는 셀·이미지·직선 연결선만 사용할 수 있습니다. 텍스트 상자나 차트는 셀 형태로 바꿔 저장해 주세요.");
            }
        }
    }
}
