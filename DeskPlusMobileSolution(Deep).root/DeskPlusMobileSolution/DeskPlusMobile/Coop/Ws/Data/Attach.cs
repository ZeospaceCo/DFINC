using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Coop.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws.Data
{
    public class Attach
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _ListID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_Attach");
                sql = string.Format(sql, _companyID, _ListID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAttach", null);

                List<BE.AttachInfo> list = new List<AttachInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AttachInfo info = new AttachInfo();
                    info.AttachFileID = dr["AttachFileID"].ToString().ToNumber<int>();
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
    }
}