using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Board.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class ShortReplyGood : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region GET()

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_shortreplyinfo"></param>
        /// <returns></returns>
        public int GetGoodCount(ShortReplyGoodInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReplyGood_SelCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _shortreplyinfo.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _shortreplyinfo.ListID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortreplyinfo.ShortReplyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;

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

        #region 추가/수정/삭제

        /// <summary>
        /// Adds the reple.
        /// </summary>
        /// <param name="_shortreplyinfo">The _shortreplyinfo.</param>
        /// <returns></returns>
        public int Add(ShortReplyGoodInfo _shortreplyinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_shortreplyinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ShortReplyGood_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _shortreplyinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _shortreplyinfo.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _shortreplyinfo.ListID;
                    cmd.Parameters.Add("@ShortReplyID", SqlDbType.Int).Value = _shortreplyinfo.ShortReplyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _shortreplyinfo.UserID;
                    cmd.Parameters.Add("@Good", SqlDbType.Int).Value = _shortreplyinfo.Good;
                    cmd.Parameters.Add("@Bad", SqlDbType.Int).Value = _shortreplyinfo.Bad;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
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
