using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class CreatePDF : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        public DataSet GetPDFLog(int _companyID, string _module, int _listID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PDFLog_Sel");

                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Module", SqlDbType.VarChar, 100).Value = _module;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPDFLog");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion GET

        #region ADD

        /// <summary>
        /// PDF생성로그 등록
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(int _companyID, string _module, int _listID, string _status, string _errorLog, string _fileName, string _filePath, int _userID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_PDFLog_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Module", SqlDbType.VarChar, 100).Value = _module;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _status;
                    cmd.Parameters.Add("@ErrorLog", SqlDbType.VarChar, 1000).Value = _errorLog;
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 500).Value = _fileName;
                    cmd.Parameters.Add("@FilePath", SqlDbType.VarChar, 500).Value = _filePath;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion ADD

        #region MOD

        #endregion MOD

        #region DEL

        /// <summary>
        /// PDF생성로그 삭제
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _module, int _listID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution()))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_PDFLog_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@Module", SqlDbType.VarChar, 100).Value = _module;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion DEL
    }
}
