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

namespace Zeospace.DeskPlusMobile.Web.Common.Ws.Data
{
    public class CreatePDF
    {
        #region GET()

        public DataSet GetPDFLog(int _companyID, string _module, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_PDFLog");
                sql = string.Format(sql, _companyID.ToString(), _module, _listID);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsPDF", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetPDFLog: " + errMsg);
                throw;
            }
        }

        #endregion GET

        #region ADD()

        /// <summary>
        /// PDF생성로그 등록
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_module"></param>
        /// <param name="_listID"></param>
        /// <param name="_status"></param>
        /// <param name="_errorLog"></param>
        /// <param name="_fileName"></param>
        /// <param name="_filePath"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, string _module, int _listID, string _status, string _errorLog, string _fileName, string _filePath, int _userID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@Module", SqlDbType.VarChar,100),
                                    new SqlParameter("@ListID", SqlDbType.Int),
                                    new SqlParameter("@Status", SqlDbType.Char, 1),
                                    new SqlParameter("@ErrorLog", SqlDbType.VarChar,1000),
                                    new SqlParameter("@FileName", SqlDbType.VarChar,500),
                                    new SqlParameter("@FilePath", SqlDbType.VarChar,500),
                                    new SqlParameter("@UserID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _module;
                parms[2].Value = _listID;
                parms[3].Value = _status;
                parms[4].Value = _errorLog;
                parms[5].Value = _fileName;
                parms[6].Value = _filePath;
                parms[7].Value = _userID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_COM_PDFLog_Ins", parms);

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