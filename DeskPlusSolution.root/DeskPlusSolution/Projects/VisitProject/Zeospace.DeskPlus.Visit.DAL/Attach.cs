using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Visit.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttachFile(IDataReader _reader)
        {
            return new AttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ReserveGroupID = (_reader["ReserveGroupID"] == DBNull.Value) ? -1 : _reader["ReserveGroupID"].ToString().ToNumber<int>(),
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파일 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_Attach_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
                    
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파일 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _reserveGroupID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_Attach_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 파일 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.VISIT, "QRY_VIS_Attach_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
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
        /// 작성일자 : 2014-02-07
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
                    var cmd = new SqlCommand("DP_VIS_Attach_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _info.ReserveGroupID;                    
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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveGroupID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Attach_DelByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;
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
        /// 작성일자 : 2020-09-15
        /// 작성목적 : 파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_VIS_Attach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ReserveGroupID", SqlDbType.Int).Value = _reserveGroupID;

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
