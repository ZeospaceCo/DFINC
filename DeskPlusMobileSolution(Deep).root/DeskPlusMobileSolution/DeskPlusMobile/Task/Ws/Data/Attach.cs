using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Task.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws.Data
{
    public class Attach
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _ListID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_Attach");
                sql = string.Format(sql, _companyID, _ListID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAttach", null);

                List<BE.AttachInfo> list = new List<AttachInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AttachInfo info = new AttachInfo();
                    info.AttachID = dr["AttachID"].ToString().ToNumber<int>();
                    info.FilePath = dr["FilePath"].ToString().ToNull();
                    info.FileName = dr["FileName"].ToString().ToNull();
                    info.FileType = dr["FileType"].ToString().ToNull();
                    info.FileLength = dr["FileLength"].ToString().ToNumber<int>();
                    info.FileGUID = dr["FileGUID"].ToString().ToNull();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetAttach: " + errMsg);
                throw;
            }

        }
        
        /// <summary>
        /// 문서 스트리밍 파일 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// /// <param name="_attachType">The _attachType ID.</param>
        /// <returns></returns>
        public DataSet GetDSFiles(int _companyID, int _listID, string _attachType)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int),
                                               new SqlParameter("@AttachType", SqlDbType.NVarChar, 50),
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _listID;
            parms[2].Value = _attachType;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_TAS_DS_Sel", "GetDSFiles", parms);
        }
    }
}