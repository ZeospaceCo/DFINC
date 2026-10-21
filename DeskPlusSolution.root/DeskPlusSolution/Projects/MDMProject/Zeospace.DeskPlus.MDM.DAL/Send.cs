using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.MDM.BE;

namespace Zeospace.DeskPlus.MDM.DAL
{
    public class Send : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-02-25
        /// 작성목적 : 알림 발송 내역 카운트 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchField, string _searchValue, string _startDate, string _endDate)
        {
            int rtCnt = 0;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_Send_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate.ToNull().ToXSS();

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-02-25
        /// 작성목적 : 알림 발송 내역 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,string _searchField, string _searchValue, 
            string _startDate, string _endDate, string _order)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_Send_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order.ToNull().ToXSS(); ;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-03-02
        /// 작성목적 : 사용자, 부서 자동완성 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetAutoName(int _companyID, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_User_SelAuto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-02
        /// 작성목적 : 알림 내역 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <returns></returns>
        public DataTableReader GetSend(int _companyID, int _sendID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_Send_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
   

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-03-03
        /// 작성목적 : 알림 발송 내역 등록  
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(SendInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_Send_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@Header", SqlDbType.NVarChar,100).Value = _info.Header.ToNull().ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToXSS();
                    cmd.Parameters.Add("@TargetURL", SqlDbType.NVarChar, 500).Value = _info.TargetURL.ToNull();
                    cmd.Parameters.Add("@MobileTargetURL", SqlDbType.NVarChar, 500).Value = _info.MobileTargetURL.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;


                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 
    }
}
