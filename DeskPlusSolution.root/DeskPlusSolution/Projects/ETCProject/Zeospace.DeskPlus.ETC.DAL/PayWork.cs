using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.ETC.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class PayWork : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기

        /// <summary>
        /// 작성자   : 박수현
        /// 작성일자 : 
        /// 작성목적 : 페이징 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PAY_Work_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 박수현
        /// 작성일자 : 
        /// 작성목적 : 특근내역 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_workID"></param>
        /// <returns></returns>
        public DataTableReader GetPageWorkList(int _companyID, int _workID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PAY_Work_SelPageWorkList", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@WorkID", SqlDbType.Int).Value = _workID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : 박수현
        /// 작성일자 : 
        /// 작성목적 : 카운트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetPageCnt(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PAY_Work_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                object obj = ExecuteScalar(cmd);
                return obj.ToString().ToNumber<int>();
            }
        }


        #endregion
    }
}
