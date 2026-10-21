using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.RM.BE;

namespace Zeospace.DeskPlus.RM.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttachFile(IDataReader _reader)
        {
            return new AttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : (long)_reader["FileLength"],
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<AttachInfo> GetAttachFileList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttachFile(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-02
        /// 작성목적 : 파일 정보 가져오기 (자원 이미지)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _listID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Attach_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-02
        /// 작성목적 : 파일 정보 가져오기 (자원 이미지)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public DataSet GetQRCodeList(int _companyID, int _listID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Attach_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetList");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파일 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_ListID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _categoryID, int _ListID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Attach_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파일 정보 (예약자원)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _categoryID, int _reservationID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Attach_SelByReID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Int).Value = _gubun;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 파일 정보 (예약자원)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _categoryID, int _reservationID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.RESOURCEMANAGER, "QRY_RM_Attach_SelByReID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Int).Value = _gubun;
                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetAttachFileList(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 파일 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Attach_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull().ToXSS();
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _info.FileType.ToNull();
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 50).Value = _info.FileGUID.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion             
        
        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 파일 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Attach_DelByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 예약 자원 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _reservationID, int _attachFileID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Attach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _reservationID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 자원 예약 삭제 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, string _gubun)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_RM_Attach_DelByGubun", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char,1).Value = _gubun;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        #endregion
    }
}
