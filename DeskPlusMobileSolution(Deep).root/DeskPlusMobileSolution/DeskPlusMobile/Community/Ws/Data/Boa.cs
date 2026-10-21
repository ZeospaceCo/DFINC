using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class Boa
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-10
        /// 작성목적 : 게시물 내용가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_boardID">The _board ID.</param>
        /// <returns></returns>
        public DataSet GetBoardList(int _companyID, int _communityID, int _menuID, int _boardID)
        {
            try
            {
                if (_companyID < 0 || _communityID < 0 || _menuID < 0 || _boardID < 0) return null;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_BOACONTENT");
                proc = string.Format(proc, _companyID, _communityID, _menuID, _boardID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetBoardList", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardList: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 이전, 이후 listid 값 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_boardID"></param>
        /// <param name="_gubun"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public string GetBoardIDBeforeNext(int _companyID, int _communityID, int _menuID, int _boardID, string _gubun, string _searchValue)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_PROC_CMNBOA_LIST_TOPDOWN");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CommunityID", SqlDbType.Int),
                                                            new SqlParameter("@MenuID", SqlDbType.Int),
                                                            new SqlParameter("@BoardID", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.VarChar,10),
                                                            new SqlParameter("@SearchValue", SqlDbType.NVarChar, 100),
                    };

                parms[0].Value = _companyID;
                parms[1].Value = _communityID;
                parms[2].Value = _menuID;
                parms[3].Value = _boardID;
                parms[4].Value = _gubun;
                parms[5].Value = _searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBoardIDBeforeNext", parms);
                return (ds != null) ? ds.Tables[0].Rows[0][0].ToString() : "0";
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBoardIDBeforeNext: " + errMsg);
                throw;
            }
        }
    }
}

