using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.Web.Organization;
using AuthUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo;

public static class OrganizationSheetIntegration
{
    static void Check(bool value, string message) { if (!value) throw new Exception(message); }
    static void Reject(Action action, Type expected)
    {
        try { action(); } catch (Exception ex) { Check(expected.IsInstanceOfType(ex), "Unexpected rejection: " + ex.GetType()); return; }
        throw new Exception("Expected rejection: " + expected.Name);
    }
    static HttpContext Context()
    {
        var context = new HttpContext(new HttpRequest("", "http://localhost/organization-test", ""), new HttpResponse(new StringWriter()));
        SessionStateUtility.AddHttpSessionStateToContext(context, new HttpSessionStateContainer(Guid.NewGuid().ToString(), new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20, true, HttpCookieMode.UseCookies, SessionStateMode.InProc, false));
        return context;
    }
    public static void Main(string[] args)
    {
        try { Run(args); }
        catch (Exception ex)
        {
            Console.WriteLine("Integration failure type: " + ex.GetType().FullName);
            Console.WriteLine("Integration failure message: " + ex.Message);
            Environment.ExitCode = 1;
        }
    }
    static void Run(string[] args)
    {
        var package = OrganizationSheetLayout.Serializer().Deserialize<OrganizationSheetTemplate>(File.ReadAllText(args[0]));
        var template = package.document;
        OrganizationSheetLayout.Validate(template);
        var serializer = OrganizationSheetLayout.Serializer();
        string json = serializer.Serialize(template);
        if (args.Length > 3)
        {
            using (var workbook = new FileStream(args[3], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                json = serializer.Serialize(OrganizationSheetExcelImporter.Import(workbook, Path.GetFileName(args[3])));
            template = serializer.Deserialize<OrganizationSheetDocument>(json);
        }
        Func<OrganizationSheetDocument> copy = () => serializer.Deserialize<OrganizationSheetDocument>(json);
        foreach (string role in new[] { "admin", "account", "accountExp", "Employee" })
            Check(OrganizationSheetLayout.CanEdit(new AuthUser { CompanyID = 1, UserID = 1, Roles = new[] { role } }), "Expected editor role: " + role);
        Check(OrganizationSheetLayout.CanEdit(new AuthUser { CompanyID = 1, UserID = 1, IsSysAdmin = true }), "System admin denied");
        var reader = new AuthUser { CompanyID = 1, UserID = 1, Roles = new string[0] };
        var admin = new AuthUser { CompanyID = 1, UserID = 2, Roles = new[] { "admin" } };
        Check(!OrganizationSheetLayout.CanEdit(reader) && !OrganizationSheetLayout.CanEdit(null), "Reader gained edit access");
        var context = Context();
        string token = OrganizationSheetLayout.GetToken(context);
        OrganizationSheetLayout.DemandEdit(admin, context, token);
        Reject(() => OrganizationSheetLayout.DemandEdit(reader, context, token), typeof(HttpException));
        Reject(() => OrganizationSheetLayout.DemandEdit(admin, context, "incorrect"), typeof(HttpException));
        Reject(() => OrganizationSheetLayout.DemandEdit(admin, Context(), token), typeof(HttpException));
        var invalid = copy(); invalid.cells.Add(invalid.cells[0]);
        Reject(() => OrganizationSheetLayout.Validate(invalid), typeof(ArgumentException));
        invalid = copy(); invalid.cells[0].rs = Int32.MaxValue;
        Reject(() => OrganizationSheetLayout.Validate(invalid), typeof(ArgumentException));
        invalid = copy(); invalid.images[0].src = "https://example.com/tracker.png";
        Reject(() => OrganizationSheetLayout.Validate(invalid), typeof(ArgumentException));
        invalid = copy(); invalid.styles[0].fill = "url(https://example.com)";
        Reject(() => OrganizationSheetLayout.Validate(invalid), typeof(ArgumentException));
        invalid = copy(); invalid.cells[0].formula = "SUM(A99999)";
        Reject(() => OrganizationSheetLayout.Validate(invalid), typeof(ArgumentException));
        Check(!OrganizationSheetLayout.SafeJson(new { text = "</script><script>alert(1)</script>" }).Contains("<"), "Unsafe bootstrap JSON");
        Console.WriteLine("Permissions, session-token isolation, layout validation and safe bootstrap checks passed.");

        var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString);
        string database = builder.InitialCatalog;
        Check(builder.DataSource.StartsWith(@"np:\\.\pipe\LOCALDB#", StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(database, "^CodexOrganizationSheetTests_[0-9a-f]{8}$"), "Integration SQL tests require a dedicated LocalDB database.");
        builder.InitialCatalog = "master";
        bool created = false;
        using (var master = new SqlConnection(builder.ToString()))
        {
            master.Open();
            try
            {
                string folder = Path.GetFullPath(args[2]);
                Directory.CreateDirectory(folder);
                using (var command = master.CreateCommand())
                {
                    command.CommandText = "CREATE DATABASE [" + database + "] ON PRIMARY (NAME=N'" + database + "',FILENAME=N'" + Path.Combine(folder, database + ".mdf").Replace("'", "''") + "') LOG ON (NAME=N'" + database + "_log',FILENAME=N'" + Path.Combine(folder, database + ".ldf").Replace("'", "''") + "')";
                    command.ExecuteNonQuery(); created = true;
                }
                var dal = new Zeospace.DeskPlus.Account.DAL.OrganizationSheet();
                Check(!dal.Get(1).StorageReady, "Missing storage must be reported");
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
                {
                    connection.Open();
                    foreach (string batch in Regex.Split(File.ReadAllText(args[1]), @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                        if (!String.IsNullOrWhiteSpace(batch)) using (var command = new SqlCommand(batch, connection)) command.ExecuteNonQuery();
                }
                Check(OrganizationSheetLayout.Load(1, args[0]).isTemplate, "Expected scoped template");
                Check(!OrganizationSheetLayout.Load(2, args[0]).isTemplate, "Template leaked to a different company");
                var first = dal.Save(1, 42, 0, json);
                Check(first.Revision == 1 && dal.Get(1).LayoutJson == json, "Save/load round trip failed");
                if (args.Length > 3)
                {
                    Reject(() => { using (var invalidUpload = new MemoryStream(new byte[] { 1, 2, 3 }))
                        dal.Save(1, 42, 1, serializer.Serialize(OrganizationSheetExcelImporter.Import(invalidUpload, "bad.xlsx"))); }, typeof(ArgumentException));
                    Check(dal.Get(1).Revision == 1 && dal.Get(1).LayoutJson == json, "Failed upload changed saved data");
                    Check(OrganizationSheetLayout.Load(1, args[0]).document.sourceFileName == Path.GetFileName(args[3]), "Uploaded workbook did not persist across reload");
                }
                Check(dal.Get(2).LayoutJson == null, "Company isolation failed");
                string empty = serializer.Serialize(OrganizationSheetLayout.EmptyDocument());
                dal.Save(2, 43, 0, empty);
                Check(dal.Get(1).LayoutJson == json, "Another company overwrote the layout");
                Reject(() => dal.Save(1, 42, 0, empty), typeof(DBConcurrencyException));
                Check(dal.Get(1).Revision == 1 && dal.Get(1).LayoutJson == json, "Stale write changed data");
                Check(dal.Save(1, 42, 1, empty).Revision == 2, "Update revision failed");
                var writers = new[] { 50, 51 }.Select(user => Task.Run(() => {
                    try { dal.Save(1, user, 2, json); return true; } catch (DBConcurrencyException) { return false; }
                })).ToArray();
                Task.WaitAll(writers);
                Check(writers.Count(task => task.Result) == 1 && dal.Get(1).Revision == 3, "Concurrent writes must have one winner");
                Reject(() => dal.Get(0), typeof(ArgumentOutOfRangeException));
                Reject(() => dal.Save(1, 0, 0, empty), typeof(ArgumentOutOfRangeException));
                Console.WriteLine("LocalDB install, template scoping, save/reload, company isolation and concurrent-write checks passed.");
            }
            finally
            {
                SqlConnection.ClearAllPools();
                if (created) using (var command = master.CreateCommand())
                {
                    command.CommandText = "ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + database + "]";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
