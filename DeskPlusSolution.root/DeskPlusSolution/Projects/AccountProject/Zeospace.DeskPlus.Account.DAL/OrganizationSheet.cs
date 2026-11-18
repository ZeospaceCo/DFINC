using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class OrganizationSheet : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public OrganizationSheetRecord Get(int companyID)
        {
            if (companyID <= 0) throw new ArgumentOutOfRangeException("companyID");
            const string query = @"SELECT LayoutJson, Revision, ModifyUserID, ModifyDate
FROM dbo.DP_ACC_OrganizationSheet WHERE CompanyID = @CompanyID";
            using (var connection = new SqlConnection(ConnSolution(companyID)))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) return new OrganizationSheetRecord { StorageReady = true };
                        return new OrganizationSheetRecord
                        {
                            StorageReady = true,
                            LayoutJson = reader.GetString(0),
                            Revision = reader.GetInt32(1),
                            ModifyUserID = reader.GetInt32(2),
                            ModifyDate = reader.GetDateTime(3)
                        };
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number != 208) throw;
                    return new OrganizationSheetRecord { StorageReady = false };
                }
            }
        }

        public OrganizationSheetRecord Save(int companyID, int userID, int expectedRevision, string json)
        {
            if (companyID <= 0 || userID <= 0) throw new ArgumentOutOfRangeException("companyID");
            using (var connection = new SqlConnection(ConnSolution(companyID)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string select = @"SELECT Revision FROM dbo.DP_ACC_OrganizationSheet WITH (UPDLOCK, HOLDLOCK)
WHERE CompanyID = @CompanyID";
                    using (var command = new SqlCommand(select, connection, transaction))
                    {
                        command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                        object current = command.ExecuteScalar();
                        int revision = current == null ? 0 : Convert.ToInt32(current);
                        if (revision != expectedRevision) throw new DBConcurrencyException("조직도가 다른 관리자에 의해 변경되었습니다. 새로고침 후 다시 편집해 주세요.");
                        var modified = DateTime.UtcNow;
                        command.CommandText = current == null
                            ? @"INSERT dbo.DP_ACC_OrganizationSheet (CompanyID, LayoutJson, Revision, ModifyUserID, ModifyDate)
VALUES (@CompanyID, @LayoutJson, @Revision, @UserID, @ModifyDate)"
                            : @"UPDATE dbo.DP_ACC_OrganizationSheet SET LayoutJson = @LayoutJson, Revision = @Revision,
ModifyUserID = @UserID, ModifyDate = @ModifyDate WHERE CompanyID = @CompanyID";
                        command.Parameters.Add("@LayoutJson", SqlDbType.NVarChar, -1).Value = json;
                        command.Parameters.Add("@Revision", SqlDbType.Int).Value = revision + 1;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                        command.Parameters.Add("@ModifyDate", SqlDbType.DateTime2).Value = modified;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        return new OrganizationSheetRecord { StorageReady = true, Revision = revision + 1, ModifyUserID = userID, ModifyDate = modified };
                    }
                }
            }
        }
    }
}
