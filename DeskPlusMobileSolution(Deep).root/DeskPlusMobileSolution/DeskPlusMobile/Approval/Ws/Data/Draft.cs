using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class Draft
    {
        #region GET()

        /// <summary>
        ///  임시저장정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public List<BE.DraftInfo> GetDraft(int _companyID, int _draftID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DraftID", SqlDbType.Int)
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Draft_Sel", "dsDoc", parms);

                List<BE.DraftInfo> list = new List<BE.DraftInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DraftInfo info = new BE.DraftInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DraftID = dr["DraftID"].ToString().ToNumber<int>();
                    info.AccessTitleID = dr["AccessTitleID"].ToString().ToNumber<int>();
                    info.Subject = dr["Subject"].ToString();
                    info.ContentHTML = dr["ContentHTML"].ToString();
                    info.ContentTEXT = dr["ContentTEXT"].ToString();
                    info.IsLegacy = dr["IsLegacy"].ToString();
                    info.CustomFileName = dr["CustomFileName"].ToString();
                    info.IsAfterSubmitCall = dr["IsAfterSubmitCall"].ToString();
                    info.IsGuard = dr["IsGuard"].ToString();
                    info.IsLinkedEtc = dr["IsLinkedEtc"].ToString();
                    info.IsUseFlow = dr["IsUseFlow"].ToString();
                    info.CarbonView = dr["CarbonView"].ToString();
                    info.FormID = dr["FormID"].ToString().ToNumber<int>();
                    info.FormBase = dr["FormBase"].ToString().ToNull();
                    info.FormKo = dr["FormKo"].ToString().ToNull();
                    info.FormEn = dr["FormEn"].ToString().ToNull();
                    info.FormJa = dr["FormJa"].ToString().ToNull();
                    info.FormFr = dr["FormFr"].ToString().ToNull();
                    info.FormZhcn = dr["FormZhcn"].ToString().ToNull();
                    info.FormZhtw = dr["FormZhtw"].ToString().ToNull();
                    info.WriteTimeYMD = dr["WriteTimeYMD"].ToString().ToNull();
                    info.IsLinked = dr["IsLinked"].ToString().ToNull();
                    info.IsLinkedType = dr["IsLinkedType"].ToString().ToNull();


                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocument: " + errMsg);
                throw;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 임시저장 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.DraftInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@FormID", SqlDbType.Int),
                                    new SqlParameter("@Subject", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@ContentHTML", SqlDbType.NText),
                                    new SqlParameter("@ContentTEXT", SqlDbType.NVarChar, 4000),
                                    new SqlParameter("@AccessTitleID", SqlDbType.Int),
                                    new SqlParameter("@IsGuard", SqlDbType.Char,1),
                                    new SqlParameter("@IsLinkedEtc", SqlDbType.NVarChar,500),
                                    new SqlParameter("@IsUseFlow", SqlDbType.NVarChar,50),
                                    new SqlParameter("@CarbonView", SqlDbType.Char,1),
                                    new SqlParameter("@IsMobile", SqlDbType.Char,1)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.UserID;
                parms[2].Value = _info.FormID;
                parms[3].Value = _info.Subject.ToXSS();
                parms[4].Value = _info.ContentHTML.ToEditorXSS();
                parms[5].Value = _info.ContentTEXT;
                parms[6].Value = _info.AccessTitleID;
                parms[7].Value = _info.IsGuard;
                parms[8].Value = _info.IsLinkedEtc;
                parms[9].Value = _info.IsUseFlow;
                parms[10].Value = _info.CarbonView;
                parms[11].Value = _info.IsMobile;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Draft_Ins", parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }

        #endregion ADD()

        #region DEL()

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DraftID", SqlDbType.Int),
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _draftID;


                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Draft_Del", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                return false;
            }

        }

        #endregion DEL()
    }
}