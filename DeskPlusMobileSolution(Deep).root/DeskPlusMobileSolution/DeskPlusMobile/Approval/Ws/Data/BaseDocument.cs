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
    public class BaseDocument
    {
        #region GET()

        /// <summary>
        /// 근거문서 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.BaseDocumentInfo> GetBaseDocLists(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_BaseDoc");
                sql = string.Format(sql, _companyID.ToString(), _documentID.ToString());
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsBaseDoc", null);

                List<BE.BaseDocumentInfo> list = new List<BE.BaseDocumentInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.BaseDocumentInfo info = new BE.BaseDocumentInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ParentDocumentID = dr["ParentDocumentID"].ToString().ToNumber<int>();
                    info.Name = dr["Name"].ToString().ToNull();

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

        #endregion

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.BaseDocumentInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@ParentDocumentID", SqlDbType.Int),
                                    new SqlParameter("@Name", SqlDbType.NVarChar, 500),
                                    new SqlParameter("@IsLegacy", SqlDbType.Char,1),
                                    new SqlParameter("@Path", SqlDbType.NVarChar,500),
                                    new SqlParameter("@BaseOrder", SqlDbType.Int)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.ParentDocumentID;
                parms[3].Value = _info.Name.ToNull();
                parms[4].Value = _info.IsLegacy.ToNull();
                parms[5].Value = _info.Path.ToNull();
                parms[6].Value = _info.BaseOrder;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_BaseDoc_Ins", parms);

                return true;
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