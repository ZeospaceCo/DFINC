using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Folder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FolderInfo GetFolderInfo(IDataReader _reader)
        {
            return new FolderInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                ParentID = (_reader["ParentID"] == DBNull.Value) ? -1 : _reader["ParentID"].ToString().ToNumber<int>(),
                TypeInfo = _reader["TypeInfo"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameBase = _reader["PathNameBase"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                FolderOrder = (_reader["FolderOrder"] == DBNull.Value) ? -1 : _reader["FolderOrder"].ToString().ToNumber<int>(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
                IsActive = _reader["IsActive"].ToString().ToNull(),
                ChildFolder = _reader["ChildFolder"].ToString(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<FolderInfo> GetFolderInfoList(IDataReader reader)
        {
            List<FolderInfo> list = new List<FolderInfo>();
            while (reader.Read())
                list.Add(GetFolderInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 타입별 폴더가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public DataSet GetFolderList(int _companyID, string _tableName, List<string> _arrType, bool _isActive)
        {
            string activeDept = (_isActive) ? "'1'" : "'0','1'";
            string type = "";
            foreach (string s in _arrType)
            {
                type += "'" + s + "',";
            }
            type = type.TrimEnd(',');

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_SelByType");
                query = string.Format(query, activeDept, type);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, _tableName);
            }
        }

        /// <summary>
        /// 폴더정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public List<BE.FolderInfo> GetFolder(int _companyID, int _folderID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 하위리스트가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public List<BE.FolderInfo> GetSubFolderList(int _companyID, int _folderID, bool _isActive)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string activeDept = (_isActive) ? "'1'" : "'0','1'";

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_SelSubList");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 특정폴더의 하위폴더리스트 가져오기
        /// </summary>
        /// <param name="_tableName"></param>
        /// <param name="_companyID"></param>
        /// <param name="_typeInfo"></param>
        /// <param name="_folderID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public DataSet GetSubFolderList(string _tableName, int _companyID, int _folderID, bool _isActive)
        {
            string activeDept = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_SelByParentID");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                return ExecuteDataSet(cmd, _tableName);
            }
        }

        /// <summary>
        /// 최상위 폴더정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_tableName"></param>
        /// <returns></returns>
        public List<BE.FolderInfo> GetFolderRoot(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_SelByParent");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 특정폴더의 하위폴더리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.FolderInfo> GetSubFolderTreeList(int _companyID, int _folderID, bool _isActive)
        {
            string activeDept = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_Folder_SelSubTreeList");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 폴더추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.FolderInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Folder_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentID", SqlDbType.Int).Value = _info.ParentID;
                    cmd.Parameters.Add("@TypeInfo", SqlDbType.Char, 1).Value = _info.TypeInfo.ToString().ToNull();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToString().ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _info.IsActive;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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

        #region MOD()

        /// <summary>
        /// 폴더수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.FolderInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Folder_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToString().ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _info.IsActive;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 폴더순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _folderID, int _sequence)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Folder_UpdOrder", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cmd.Parameters.Add("@FolderOrder", SqlDbType.Int).Value = _sequence;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 폴더삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _folderID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Folder_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()
    }
}