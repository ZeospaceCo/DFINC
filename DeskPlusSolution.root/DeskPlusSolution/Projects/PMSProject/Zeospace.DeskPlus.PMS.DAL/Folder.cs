using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Folder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private FolderInfo GetFolder(IDataReader _reader)
        {
            return new FolderInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                ParentFolderID = (_reader["ParentFolderID"] == DBNull.Value) ? -1 : _reader["ParentFolderID"].ToString().ToNumber<int>(),
                FolderBase = _reader["FolderBase"].ToString().ToNull(),
                FolderKo = _reader["FolderKo"].ToString().ToNull(),
                FolderEn = _reader["FolderEn"].ToString().ToNull(),
                FolderJa = _reader["FolderJa"].ToString().ToNull(),
                FolderFr = _reader["FolderFr"].ToString().ToNull(),
                FolderZhcn = _reader["FolderZhcn"].ToString().ToNull(),
                FolderZhtw = _reader["FolderZhtw"].ToString().ToNull(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<FolderInfo> GetFolderList(IDataReader reader)
        {
            List<FolderInfo> list = new List<FolderInfo>();
            while (reader.Read())
                list.Add(GetFolder(reader));
            return list;
        }

        #endregion DataReader

        #region 추가, 수정, 삭제

        /// <summary>
        /// 폴더 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(FolderInfo _info)
        {
            try
            {
                int folderID = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Folder_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentFolderID", SqlDbType.Int).Value = _info.ParentFolderID.ToNullInt();
                    cmd.Parameters.Add("@FolderBase", SqlDbType.NVarChar, 100).Value = _info.FolderBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderKo", SqlDbType.NVarChar, 20).Value = _info.FolderKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderEn", SqlDbType.NVarChar, 100).Value = _info.FolderEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderJa", SqlDbType.NVarChar, 100).Value = _info.FolderJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderFr", SqlDbType.NVarChar, 100).Value = _info.FolderFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderZhcn", SqlDbType.NVarChar, 100).Value = _info.FolderZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderZhtw", SqlDbType.NVarChar, 100).Value = _info.FolderZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    folderID = ExecuteScalar(cmd).ToString().ToNumber<int>();

                    if (folderID < 1)
                    {
                        return -1;
                    }
                }
                return folderID;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 폴더 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(FolderInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Folder_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@FolderBase", SqlDbType.NVarChar, 100).Value = _info.FolderBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderKo", SqlDbType.NVarChar, 20).Value = _info.FolderKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderEn", SqlDbType.NVarChar, 100).Value = _info.FolderEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderJa", SqlDbType.NVarChar, 100).Value = _info.FolderJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderFr", SqlDbType.NVarChar, 100).Value = _info.FolderFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderZhcn", SqlDbType.NVarChar, 100).Value = _info.FolderZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@FolderZhtw", SqlDbType.NVarChar, 100).Value = _info.FolderZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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
        /// 폴더 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_folderID">The _folder ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _folderID, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Folder_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

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

        #endregion 추가, 수정, 삭제

        #region 가져오기

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 나의 프로젝트 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-13
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentFolderID">The _parent folder ID.</param>
        /// <returns></returns>
        public DataSet GetFolder(int _companyID, int _parentFolderID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Folder_SelByPFolderID");
            query = (_parentFolderID == 0) ? string.Format(query, "is null") : string.Format(query, "=" + _parentFolderID);

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();
                return ExecuteDataSet(cmd, "GetFolder");
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 나의 프로젝트 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-13
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_folderID">The _folder ID.</param>
        /// <returns></returns>
        public DataTableReader GetFolderByID(int _companyID, int _folderID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Folder_SelByFolderID");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 가져오기
    }
}