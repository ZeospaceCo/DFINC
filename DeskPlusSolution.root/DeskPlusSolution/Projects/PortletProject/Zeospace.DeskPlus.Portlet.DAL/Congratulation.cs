using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class Congratulation : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 작성일자 : 2014-03-01
        /// 작성목적 : 전체 사용자 생일 및 결혼기념일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetCongratulation(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Congratulation_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetCongratulation");
            }
        }

        #endregion 가져오기
    }
}