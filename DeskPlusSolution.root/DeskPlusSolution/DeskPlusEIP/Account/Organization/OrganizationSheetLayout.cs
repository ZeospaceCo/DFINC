using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using Zeospace.DeskPlus.Account.BE;
using AuthUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo;

namespace Zeospace.DeskPlus.Account.Web.Organization
{
    public class OrganizationSheetState
    {
        public OrganizationSheetDocument document { get; set; }
        public int revision { get; set; }
        public bool storageReady { get; set; }
        public bool isTemplate { get; set; }
        public string modifiedAt { get; set; }
    }

    public class OrganizationSheetTemplate
    {
        public int companyID { get; set; }
        public OrganizationSheetDocument document { get; set; }
    }

    public static class OrganizationSheetLayout
    {
        public const int MaxJsonLength = 4000000;
        private const string TokenKey = "Account.OrganizationSheet.Csrf";

        public static JavaScriptSerializer Serializer()
        {
            return new JavaScriptSerializer { MaxJsonLength = MaxJsonLength, RecursionLimit = 40 };
        }

        public static bool CanEdit(AuthUser user)
        {
            return user != null && user.CompanyID > 0 && user.UserID > 0 &&
                (user.IsSysAdmin || user.IsInRole("admin") || user.IsInRole("account") ||
                 user.IsInRole("accountExp") || user.IsInRole("Employee"));
        }

        public static string GetToken(HttpContext context)
        {
            var token = context.Session[TokenKey] as string;
            if (String.IsNullOrEmpty(token))
            {
                var bytes = new byte[32];
                using (var random = RandomNumberGenerator.Create()) random.GetBytes(bytes);
                token = Convert.ToBase64String(bytes);
                context.Session[TokenKey] = token;
            }
            return token;
        }

        public static void DemandEdit(AuthUser user, HttpContext context, string token)
        {
            if (!CanEdit(user)) throw new HttpException(403, "조직도를 업로드할 관리자 권한이 없습니다.");
            var expected = context.Session[TokenKey] as string;
            if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(expected) || token.Length != expected.Length)
                throw new HttpException(403, "업로드 세션이 만료되었습니다. 새로고침 후 다시 시도해 주세요.");
            int difference = 0;
            for (int i = 0; i < expected.Length; i++) difference |= expected[i] ^ token[i];
            if (difference != 0) throw new HttpException(403, "업로드 세션을 확인할 수 없습니다.");
        }

        public static OrganizationSheetState Load(int companyID, string templatePath)
        {
            var record = new DAL.OrganizationSheet().Get(companyID);
            OrganizationSheetDocument document = null;
            bool template = false;
            if (!String.IsNullOrEmpty(record.LayoutJson)) document = Serializer().Deserialize<OrganizationSheetDocument>(record.LayoutJson);
            else if (File.Exists(templatePath))
            {
                var initial = Serializer().Deserialize<OrganizationSheetTemplate>(File.ReadAllText(templatePath));
                // 원본에 포함된 이름/사진은 지정된 회사에만 제공한다.
                if (initial.companyID == companyID) { document = initial.document; template = true; }
            }
            if (document == null) document = EmptyDocument();
            Validate(document);
            return new OrganizationSheetState
            {
                document = document, revision = record.Revision, storageReady = record.StorageReady, isTemplate = template,
                modifiedAt = record.ModifyDate == DateTime.MinValue ? "" : DateTime.SpecifyKind(record.ModifyDate, DateTimeKind.Utc).ToString("o")
            };
        }

        public static OrganizationSheetDocument EmptyDocument()
        {
            var border = new OrganizationSheetBorder { width = 0, color = "#000000" };
            var cells = new List<OrganizationSheetCell> { new OrganizationSheetCell { r = 0, c = 0, rs = 1, cs = 5, text = "조직도", formula = "", prefix = "", suffix = "", style = 0 } };
            for (int r = 1; r < 8; r++) for (int c = 0; c < 5; c++) cells.Add(new OrganizationSheetCell { r = r, c = c, rs = 1, cs = 1, text = "", formula = "", prefix = "", suffix = "", style = 0 });
            return new OrganizationSheetDocument
            {
                schemaVersion = 1, title = "조직도", columns = new List<double> { 140, 140, 140, 140, 140 },
                rows = new List<double> { 60, 50, 50, 50, 50, 50, 50, 50 },
                styles = new List<OrganizationSheetStyle> { new OrganizationSheetStyle { fontSize = 20, color = "#000000", fill = "#ffffff", align = "center", vertical = "center", top = border, right = border, bottom = border, left = border } },
                cells = cells,
                images = new List<OrganizationSheetImage>(), lines = new List<OrganizationSheetLine>()
            };
        }

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new ArgumentException(message);
        }

        private static bool Between(double value, double minimum, double maximum)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value) && value >= minimum && value <= maximum;
        }

        private static void Color(string color)
        {
            Require(color != null && Regex.IsMatch(color, "^#[0-9a-fA-F]{6}$"), "색상 값이 올바르지 않습니다.");
        }

        private static void Anchor(OrganizationSheetAnchor anchor, OrganizationSheetDocument document)
        {
            Require(anchor != null && anchor.r >= 0 && anchor.r <= document.rows.Count && anchor.c >= 0 && anchor.c <= document.columns.Count &&
                Between(anchor.dx, -2000, 2000) && Between(anchor.dy, -2000, 2000), "연결선 또는 이미지 위치가 올바르지 않습니다.");
        }

        public static void Validate(OrganizationSheetDocument document)
        {
            Require(document != null && document.schemaVersion == 1, "지원하지 않는 조직도 양식입니다.");
            Require(document.title != null && document.title.Length <= 200, "제목은 200자 이내로 입력해 주세요.");
            Require((document.sourceFileName ?? "").Length <= 200 && (document.sourceSheetName ?? "").Length <= 100, "업로드 파일명이나 시트명이 너무 깁니다.");
            Require(document.columns != null && document.columns.Count >= 1 && document.columns.Count <= 100 && document.columns.All(n => Between(n, 8, 600)), "열 너비가 올바르지 않습니다.");
            Require(document.rows != null && document.rows.Count >= 1 && document.rows.Count <= 200 && document.rows.All(n => Between(n, 8, 400)), "행 높이가 올바르지 않습니다.");
            Require(document.styles != null && document.styles.Count >= 1 && document.styles.Count <= 4096, "서식 수가 올바르지 않습니다.");
            foreach (var style in document.styles)
            {
                Require(style != null && Between(style.fontSize, 8, 72), "글자 크기는 8~72px 범위로 지정해 주세요.");
                Color(style.color); Color(style.fill);
                Require(new[] { "left", "center", "right" }.Contains(style.align) && new[] { "top", "center", "bottom" }.Contains(style.vertical), "정렬 값이 올바르지 않습니다.");
                foreach (var border in new[] { style.top, style.right, style.bottom, style.left })
                {
                    Require(border != null && Between(border.width, 0, 4), "테두리가 올바르지 않습니다.");
                    Color(border.color);
                }
            }
            Require(document.cells != null && document.cells.Count <= 20000, "항목 수가 너무 많습니다.");
            var occupied = new HashSet<int>();
            foreach (var cell in document.cells)
            {
                Require(cell != null && cell.r >= 0 && cell.c >= 0 && cell.rs >= 1 && cell.rs <= document.rows.Count && cell.cs >= 1 && cell.cs <= document.columns.Count &&
                    cell.r + cell.rs <= document.rows.Count && cell.c + cell.cs <= document.columns.Count && cell.style >= 0 && cell.style < document.styles.Count, "항목 위치 또는 병합 범위가 올바르지 않습니다.");
                Require((cell.text ?? "").Length <= 2000 && (cell.prefix ?? "").Length <= 100 && (cell.suffix ?? "").Length <= 100, "항목 내용이 너무 깁니다.");
                if (!String.IsNullOrEmpty(cell.formula))
                {
                    Require(cell.formula.Length <= 1000 && Regex.IsMatch(cell.formula, @"^[A-Z0-9$():,+.\s\-]+$"), "인원 집계 수식이 올바르지 않습니다.");
                    foreach (Match name in Regex.Matches(cell.formula, @"[A-Z]+(?=\s*\()")) Require(name.Value == "SUM" || name.Value == "COUNTA", "집계에는 SUM과 COUNTA만 사용할 수 있습니다.");
                    foreach (Match reference in Regex.Matches(cell.formula, @"\$?([A-Z]+)\$?([0-9]+)"))
                    {
                        int column = 0, row;
                        foreach (char letter in reference.Groups[1].Value) { if (column > 100) break; column = column * 26 + letter - 'A' + 1; }
                        Require(Int32.TryParse(reference.Groups[2].Value, out row) && row >= 1 && row <= document.rows.Count && column >= 1 && column <= document.columns.Count, "인원 집계 수식이 조직도 범위를 벗어났습니다.");
                    }
                }
                for (int r = cell.r; r < cell.r + cell.rs; r++) for (int c = cell.c; c < cell.c + cell.cs; c++)
                    Require(occupied.Add(r * 100 + c), "항목의 병합 영역이 서로 겹칩니다.");
            }
            Require(document.images != null && document.images.Count <= 50 && document.lines != null && document.lines.Count <= 500, "이미지 또는 연결선 수가 너무 많습니다.");
            int imageBytes = 0;
            var ids = new HashSet<string>();
            foreach (var image in document.images)
            {
                Require(image != null && !String.IsNullOrEmpty(image.id) && image.id.Length <= 80 && ids.Add(image.id), "이미지 식별자가 올바르지 않습니다.");
                Anchor(image.anchor, document);
                Require(Between(image.width, 8, 2000) && Between(image.height, 8, 2000), "이미지 크기가 올바르지 않습니다.");
                string source = image.src ?? "";
                bool png = source.StartsWith("data:image/png;base64,", StringComparison.Ordinal);
                bool jpeg = source.StartsWith("data:image/jpeg;base64,", StringComparison.Ordinal);
                Require((png || jpeg) && source.Length <= 1500000, "사진은 1MB 이하 PNG 또는 JPEG 파일만 사용할 수 있습니다.");
                byte[] bytes;
                try { bytes = Convert.FromBase64String(source.Substring(source.IndexOf(',') + 1)); }
                catch (FormatException) { throw new ArgumentException("사진 데이터가 올바르지 않습니다."); }
                Require(bytes.Length >= 8 && (png ? bytes[0] == 137 && bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71 : bytes[0] == 255 && bytes[1] == 216 && bytes[2] == 255), "사진 형식이 올바르지 않습니다.");
                imageBytes += bytes.Length;
            }
            Require(imageBytes <= 2000000, "이미지 전체 용량은 2MB 이내로 줄여 주세요.");
            ids.Clear();
            foreach (var line in document.lines)
            {
                Require(line != null && !String.IsNullOrEmpty(line.id) && line.id.Length <= 80 && ids.Add(line.id) && Between(line.width, .1, 5), "연결선 정보가 올바르지 않습니다.");
                Anchor(line.start, document); Anchor(line.end, document); Color(line.color);
            }
        }

        public static string SafeJson(object value)
        {
            return Serializer().Serialize(value).Replace("&", "\\u0026").Replace("<", "\\u003c").Replace(">", "\\u003e")
                .Replace("\u2028", "\\u2028").Replace("\u2029", "\\u2029");
        }
    }
}
