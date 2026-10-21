using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class BirthDay : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기

        public DataTableReader GetBirthDayList(int _listCount, int _companyID, int _activeUser, int _activeCompany, int _baseDept,
                                 string _anniversaryStartDate, string _anniversaryEndDate, string _searchVal,
                                 string _lunarStartDate, string _lunarEndDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Anniversary_Sel");
                    query = string.Format(query, _listCount, _searchVal);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ActiveUser", SqlDbType.Int).Value = _activeUser;
                    cmd.Parameters.Add("@ActiveCompany", SqlDbType.Int).Value = _activeCompany;
                    cmd.Parameters.Add("@BaseDept", SqlDbType.Int).Value = _baseDept;
                    cmd.Parameters.Add("@AnniversaryStartDate", SqlDbType.Char, 8).Value = _anniversaryStartDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@AnniversaryEndDate", SqlDbType.Char, 8).Value = _anniversaryEndDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@LunarStartDate", SqlDbType.Char, 8).Value = _lunarStartDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@LunarEndDate", SqlDbType.Char, 8).Value = _lunarEndDate.ToNull().ToXSS();

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}