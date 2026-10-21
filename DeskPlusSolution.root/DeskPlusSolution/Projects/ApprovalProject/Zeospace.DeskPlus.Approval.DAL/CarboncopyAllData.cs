using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class CarboncopyAllData : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public DataTableReader GetCarbonPageList(int companyID, int userID, int deptID, int formID, string pageSize, string pageSizePageNo,
            string searchStartDate, string searchEndDate, string searchField, string searchValue, string order, string confirmType)
        {
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListCarbonAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = deptID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = formID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = order;
                cmd.Parameters.Add("@ConfirmType", SqlDbType.VarChar, 20).Value = NormalizeConfirmType(confirmType);

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public int GetCarbonPageCount(int companyID, int userID, int deptID, int formID,
            string searchStartDate, string searchEndDate, string searchField, string searchValue, string confirmType)
        {
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntCarbonAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = deptID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = formID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = searchValue;
                cmd.Parameters.Add("@ConfirmType", SqlDbType.VarChar, 20).Value = NormalizeConfirmType(confirmType);

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        public DataTableReader GetForwardPageList(int companyID, int userID, int formID, string pageSize, string pageSizePageNo,
            string searchStartDate, string searchEndDate, string searchField, string searchValue, string order, string confirmType)
        {
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageListForwardAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = formID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = pageSizePageNo;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = searchValue;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 100).Value = order;
                cmd.Parameters.Add("@ConfirmType", SqlDbType.VarChar, 20).Value = NormalizeConfirmType(confirmType);

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public int GetForwardPageCount(int companyID, int userID, int formID,
            string searchStartDate, string searchEndDate, string searchField, string searchValue, string confirmType)
        {
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Doc_SelPageCntForwardAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@FormID", SqlDbType.Int).Value = formID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = searchValue;
                cmd.Parameters.Add("@ConfirmType", SqlDbType.VarChar, 20).Value = NormalizeConfirmType(confirmType);

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        private static string NormalizeConfirmType(string confirmType)
        {
            return confirmType.ToNull().Equals("Receive") ? "Receive" : "Complete";
        }
    }
}
