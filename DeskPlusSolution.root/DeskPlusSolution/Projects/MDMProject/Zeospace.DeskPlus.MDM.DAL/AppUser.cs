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
    public class AppUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-03
        /// 작성목적 : App 등록 사용자 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <param name="_isApp"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _sendID, string _isApp)
        {
            int rtCnt = 0;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_AppUser_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;
                cmd.Parameters.Add("@IsApp", SqlDbType.Char,1).Value = _isApp;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-03
        /// 작성목적 : App 등록 사용자 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <param name="_isApp"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _sendID, string _isApp, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_AppUser_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;
                cmd.Parameters.Add("@IsApp", SqlDbType.Char, 1).Value = _isApp;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}
