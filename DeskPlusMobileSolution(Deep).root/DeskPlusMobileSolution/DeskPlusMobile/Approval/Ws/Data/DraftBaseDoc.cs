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
    public class DraftBaseDoc
    {
        #region GET()

        /// <summary>
        /// 근거문서리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.DraftBaseDocInfo> GetBaseDocLists(int _companyID, int _draftID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@DraftID", SqlDbType.Int)
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftBaseDoc_Sel", "dsBaseDoc", parms);

                List<BE.DraftBaseDocInfo> list = new List<BE.DraftBaseDocInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DraftBaseDocInfo info = new BE.DraftBaseDocInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.BaseID = dr["BaseID"].ToString().ToNumber<int>();
                    info.DraftID = dr["DraftID"].ToString().ToNumber<int>();
                    info.ParentDocumentID = dr["ParentDocumentID"].ToString().ToNumber<int>();
                    info.Name = dr["Name"].ToString().ToNull();
                    info.Subject = dr["Subject"].ToString().ToNull().ToReverseXSS();
                    info.IsLegacy = dr["IsLegacy"].ToString().ToNull();
                    info.Path = dr["Path"].ToString().ToNull();
                    info.BaseOrder = dr["BaseOrder"].ToString().ToNumber<int>();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBaseDocLists: " + errMsg);
                throw;
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 임시저장 - 근거문서추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.DraftBaseDocInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DraftID", SqlDbType.Int),
                                    new SqlParameter("@ParentDocumentID", SqlDbType.Int),
                                    new SqlParameter("@Name", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@IsLegacy", SqlDbType.Char,1),
                                    new SqlParameter("@Path", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@BaseOrder", SqlDbType.Int)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DraftID;
                parms[2].Value = _info.ParentDocumentID;
                parms[3].Value = _info.Name.ToNull();
                parms[4].Value = _info.IsLegacy.ToNull();
                parms[5].Value = _info.Path.ToNull();
                parms[6].Value = _info.BaseOrder;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftBaseDoc_Ins", parms);

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
    }
}