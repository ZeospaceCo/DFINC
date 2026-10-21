using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class ZipCodeNew : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public DataTableReader GetZipCode(string _dongName)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_ZipCodeNew_Sel");
            query = string.Format(query, _dongName.ToInject());

            using (SqlConnection cn = new SqlConnection(ConnSolution(1)))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}