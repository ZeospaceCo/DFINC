using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Community.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws.Data
{
    public class BoaShortReply
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 선택한 게시물의 댓글가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_boardID"></param>
        /// <returns></returns>
        public DataSet GetShortReply(int _companyID, int _communityID, int _menuID, int _boardID)
        {
            if (_companyID < 0 || _communityID < 0 || _menuID < 0 || _boardID < 0) return null;

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_BOASHORTREPLY");
            proc = string.Format(proc, _companyID, _communityID, _menuID, _boardID);

            return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetShortReply", null);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-12
        /// 작성목적 : 댓글, 포인트 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BoaShortReplyInfo _info)
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "INS_PROC_BOASHORTREPLYPOINT");

            SqlParameter[] parms = new SqlParameter[] {
                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                            new SqlParameter("@CommunityID", SqlDbType.Int),
                                            new SqlParameter("@MenuID", SqlDbType.Int),
                                            new SqlParameter("@BoardID", SqlDbType.Int),                                            
                                            new SqlParameter("@UserID", SqlDbType.Int),
                                            new SqlParameter("@Content", SqlDbType.NVarChar,200),                   
                                            new SqlParameter("@UserName", SqlDbType.NVarChar,50)
                    
			};

            parms[0].Value = _info.CompanyID;
            parms[1].Value = _info.CommunityID;
            parms[2].Value = _info.MenuID;
            parms[3].Value = _info.BoardID;  
            parms[4].Value = _info.UserID;
            parms[5].Value = _info.Content.ToXSS();
            parms[6].Value = _info.UserName;            
 
            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return false;
            }
        }
    }
}