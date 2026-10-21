using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.MyTalk.Ws.Data
{
    public class List
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-28
        /// 작성목적 : 총 게시물 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, string _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_PROC_MyTalkCnt");

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _userID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-10
        /// 작성목적 : 임시저장한 글 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftTalkID"></param>
        /// <returns></returns>
        public DataSet GetDraft(string _companyID, int _draftTalkID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Draft");
                sql = string.Format(sql, _companyID, _draftTalkID);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDraft: " + errMsg);
                throw;
            }
        }
    }
}

