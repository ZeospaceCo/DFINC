using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class UnitAttach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the unit attach.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UnitAttachInfo GetUnitAttach(IDataReader _reader)
        {
            return new UnitAttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProjectID = (_reader["ProjectID"] == DBNull.Value) ? -1 : _reader["ProjectID"].ToString().ToNumber<int>(),
                UnitID = (_reader["UnitID"] == DBNull.Value) ? -1 : _reader["UnitID"].ToString().ToNumber<int>(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the unit attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<UnitAttachInfo> GetUnitAttachList(IDataReader reader)
        {
            List<UnitAttachInfo> list = new List<UnitAttachInfo>();
            while (reader.Read())
                list.Add(GetUnitAttach(reader));
            return list;
        }

        #endregion DataReader

        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 첨부파일 추가
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-01
        /// </summary>
        /// <param name="_attachFileInfo">The _attach file info.</param>
        /// <returns></returns>
        public int Add(UnitAttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_UnitAttach_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _info.UnitID;
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
                throw;
            }
        }

        #endregion ADD()

        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 첨부파일 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-03
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _unitID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_UnitAttach_DelByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 첨부파일 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-03
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _unitID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_UnitAttach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()

        #region GET()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 파일 정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-03
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <returns></returns>
        public List<BE.UnitAttachInfo> GetAttachFile(int _companyID, int _projectID, int _unitID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_UnitAttach_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUnitAttachList(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 파일 정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-03
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public List<BE.UnitAttachInfo> GetAttachFile(int _companyID, int _projectID, int _unitID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_UnitAttach_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUnitAttachList(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion GET()
    }
}