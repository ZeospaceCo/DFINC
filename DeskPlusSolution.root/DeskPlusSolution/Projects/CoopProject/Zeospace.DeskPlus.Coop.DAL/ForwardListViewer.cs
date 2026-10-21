using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.DAL
{
    public class ForwardListViewer : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {


        /// <summary>
        /// 총카운트, 읽은 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <returns></returns>
        public DataTableReader GetForwardListCnt(int _companyID, int _forwardUserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_ForwardList_Cnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardUserID", SqlDbType.Int).Value = _forwardUserID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        #region 추가/수정/삭제


        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 조회 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _forwardListID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ForwardListViewer_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        #endregion
    }
}
