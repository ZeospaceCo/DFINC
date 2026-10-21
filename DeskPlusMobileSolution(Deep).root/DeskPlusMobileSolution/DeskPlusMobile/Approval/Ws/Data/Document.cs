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
    public class Document
    {
        #region GET()

        /// <summary>
        /// 문서정보 가져오기 (특정문서의 전체정보)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.DocInfo> GetDocument(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocInfo");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

                List<BE.DocInfo> list = new List<BE.DocInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DocInfo info = new BE.DocInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                    info.Number2 = dr["Number2"].ToString();
                    info.Number3 = dr["Number3"].ToString();
                    info.LifeTime = dr["LifeTime"].ToString();
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
                    info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                    info.Etc1 = dr["Etc1"].ToString();
                    info.Etc2 = dr["Etc2"].ToString();
                    info.Etc3 = dr["Etc3"].ToString();
                    info.Etc4 = dr["Etc4"].ToString();
                    info.Etc5 = dr["Etc5"].ToString();
                    info.Etc6 = dr["Etc6"].ToString();
                    info.Etc7 = dr["Etc7"].ToString();
                    info.Etc8 = dr["Etc8"].ToString();
                    info.Etc9 = dr["Etc9"].ToString();
                    info.Etc10 = dr["Etc10"].ToString();

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
    }
}