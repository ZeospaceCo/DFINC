<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>

<script language="C#" runat="server">
    private void Page_Load(object sender, EventArgs e)
    {
        using (DataSet ds = GetUserInfo())
        {
            foreach (DataRow user in ds.Tables["Users"].Rows)
            {
                int userID = Convert.ToInt32(user["UserID"]);
                string nameBase = user["NameBase"].ToString();

                CopyUserImages(userID, nameBase);
            }
        }
    }

    private DataSet GetUserInfo()
    {
        try
        {
            DataSet users = new DataSet();
            string query = "SELECT UserID, NameBase FROM DP_ACC_User WHERE CompanyID=1 AND UserID>1";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(users, "Users");
            }

            return users;
        }
        catch (SqlException ex)
        {
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            throw;
        }
    }

    private void UpdateUserImages(int userID, bool sealCopied, bool photoCopied)
    {
        if (!sealCopied && !photoCopied)
        {
            return;
        }

        string query = "UPDATE DP_ACC_User SET ";
        query += "SealImage = CASE WHEN @SealCopied=1 THEN 'seal.jpg' ELSE SealImage END, ";
        query += "PhotoImage = CASE WHEN @PhotoCopied=1 THEN 'photo.jpg' ELSE PhotoImage END ";
        query += "WHERE CompanyID=1 AND UserID=@UserID";

        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@UserID", SqlDbType.Int) { Value = userID },
            new SqlParameter("@SealCopied", SqlDbType.Bit) { Value = sealCopied },
            new SqlParameter("@PhotoCopied", SqlDbType.Bit) { Value = photoCopied }
        };

        SQLHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString,
            CommandType.Text, query, parameters);
    }

    private void CopyUserImages(int userID, string nameBase)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nameBase))
            {
                Response.Write(userID + ": skipped (NameBase is empty).<br />");
                return;
            }

            string sourceFileName = nameBase + ".jpg";
            string photoSourcePath = Path.Combine(Server.MapPath("/DeskPlusEIP/FileData/1/Users/Photo"), sourceFileName);
            string sealSourcePath = Path.Combine(Server.MapPath("/DeskPlusEIP/FileData/1/Users/Sign"), sourceFileName);
            string picturePath = Server.MapPath("/DeskPlusEIP/FileData/1/Users/" + userID + "/Picture");

            bool photoCopied = CopyUserImage(photoSourcePath, Path.Combine(picturePath, "photo.jpg"));
            bool sealCopied = CopyUserImage(sealSourcePath, Path.Combine(picturePath, "seal.jpg"));

            UpdateUserImages(userID, sealCopied, photoCopied);

            Response.Write(userID + " (" + Server.HtmlEncode(nameBase) + "): photo="
                + (photoCopied ? "copied" : "source missing") + ", seal="
                + (sealCopied ? "copied" : "source missing") + "<br />");
        }
        catch (Exception ex)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                "CopyUserImages (UserID=" + userID + ") : " + ex.ToString());
            throw;
        }
    }

    private bool CopyUserImage(string sourcePath, string destinationPath)
    {
        if (!File.Exists(sourcePath))
        {
            return false;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
        File.Copy(sourcePath, destinationPath, true);
        return true;
    }
</script>
