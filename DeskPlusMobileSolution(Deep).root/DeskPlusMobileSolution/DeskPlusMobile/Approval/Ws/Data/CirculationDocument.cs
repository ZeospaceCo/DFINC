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
    public class CirculationDocument
    {
        #region GET()

        /// <summary>
        /// 회람 문서 정보 documentid 로 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.CirculationDocumentInfo> GetDocListByDocID(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_CirDocByDID");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

                List<BE.CirculationDocumentInfo> list = new List<BE.CirculationDocumentInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.CirculationDocumentInfo info = new BE.CirculationDocumentInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.CirculationDocumentID = dr["CirculationDocumentID"].ToString().ToNumber<int>();
                    info.Number2 = dr["Number2"].ToString();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocListByDocID: " + errMsg);
                throw;
            }
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 문서삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_CirDoc_DelByDocID", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }
        }

        #endregion
    }
}