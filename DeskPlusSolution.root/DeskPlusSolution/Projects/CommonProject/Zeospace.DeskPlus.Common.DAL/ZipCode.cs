using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class ZipCode : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 우편번호 검색(1: 지번검색, 2: 도로명)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_siName">Name of the _si.</param>
        /// <param name="_guName">Name of the _gu.</param>
        /// <param name="_dongName">Name of the _dong.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataTableReader GetZipCode(int _companyID, string _siName, string _guName, string _dongName, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COM_ZipCodeNew_Sel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SiName", SqlDbType.VarChar, 50).Value = _siName;
                    cmd.Parameters.Add("@GuName", SqlDbType.VarChar, 50).Value = _guName;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.VarChar, 80).Value = _dongName;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Int).Value = _gubun.ToNumber<int>();

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

        /// <summary>
        /// 시 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataTableReader GetSiCode(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_ZipCode_SelBySi");
                var cmd = new SqlCommand(query, cn);

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 선택한 시에 따른 구 가져오기
        /// </summary>
        /// <param name="_compnayID">The _compnay ID.</param>
        /// <param name="_siName">Name of the _si.</param>
        /// <param name="_bugun">The _bugun.</param>
        /// <returns></returns>
        public DataTableReader GetGuCode(int _companyID, string _siName, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COM_ZipCodeNew_SelbyGu", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SearchValue", SqlDbType.VarChar, 80).Value = _siName;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Int).Value = _gubun.ToNumber<int>();

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

        /// <summary>
        /// 작성자 : shine
        /// 도로명주소에서 ID 값으로 주소 가져오기(시,구,동..)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="Code">The code.</param>
        /// <returns></returns>
        public DataTableReader GetNewAddress(int _companyID, string _code)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_ZipCode_SelByNum_bldg");
                var cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@Code", SqlDbType.Char, 25).Value = _code;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}