using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class PublicFolder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.PublicFolderInfo GetPublicFolderInfo(IDataReader _reader)
        {
            return new PublicFolderInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                ParentFolderID = (_reader["ParentFolderID"] == DBNull.Value) ? -1 : _reader["ParentFolderID"].ToString().ToNumber<int>(),
                FolderOrder = (_reader["FolderOrder"] == DBNull.Value) ? -1 : _reader["FolderOrder"].ToString().ToNumber<int>(),
                FolderBase = _reader["FolderBase"].ToString().ToNull(),
                FolderKo = _reader["FolderKo"].ToString().ToNull(),
                FolderEn = _reader["FolderEn"].ToString().ToNull(),
                FolderJa = _reader["FolderJa"].ToString().ToNull(),
                FolderFr = _reader["FolderFr"].ToString().ToNull(),
                FolderZhcn = _reader["FolderZhcn"].ToString().ToNull(),
                FolderZhtw = _reader["FolderZhtw"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameBase = _reader["PathNameBase"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
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
        protected List<PublicFolderInfo> GetPublicFolderInfoList(IDataReader reader)
        {
            List<PublicFolderInfo> list = new List<PublicFolderInfo>();
            while (reader.Read())
                list.Add(GetPublicFolderInfo(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(PublicFolderInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicFolder_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ParentFolderID", SqlDbType.Int).Value = _info.ParentFolderID;
                    cmd.Parameters.Add("@FolderBase", SqlDbType.NVarChar, 100).Value = _info.FolderBase.ToNull();
                    cmd.Parameters.Add("@FolderKo", SqlDbType.NVarChar, 100).Value = _info.FolderKo.ToNull();
                    cmd.Parameters.Add("@FolderEn", SqlDbType.NVarChar, 100).Value = _info.FolderEn.ToNull();
                    cmd.Parameters.Add("@FolderJa", SqlDbType.NVarChar, 100).Value = _info.FolderJa.ToNull();
                    cmd.Parameters.Add("@FolderFr", SqlDbType.NVarChar, 100).Value = _info.FolderFr.ToNull();
                    cmd.Parameters.Add("@FolderZhcn", SqlDbType.NVarChar, 100).Value = _info.FolderZhcn.ToNull();
                    cmd.Parameters.Add("@FolderZhtw", SqlDbType.NVarChar, 100).Value = _info.FolderZhtw.ToNull();
                    cmd.Parameters.Add("@ActiveFolder", SqlDbType.Char, 1).Value = _info.ActiveFolder.ToNull();
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

        /// <summary>
        /// 심재운
        /// 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(PublicFolderInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicFolder_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@ParentFolderID", SqlDbType.Int).Value = _info.ParentFolderID;
                    cmd.Parameters.Add("@FolderBase", SqlDbType.NVarChar, 100).Value = _info.FolderBase.ToNull();
                    cmd.Parameters.Add("@FolderKo", SqlDbType.NVarChar, 100).Value = _info.FolderKo.ToNull();
                    cmd.Parameters.Add("@FolderEn", SqlDbType.NVarChar, 100).Value = _info.FolderEn.ToNull();
                    cmd.Parameters.Add("@FolderJa", SqlDbType.NVarChar, 100).Value = _info.FolderJa.ToNull();
                    cmd.Parameters.Add("@FolderFr", SqlDbType.NVarChar, 100).Value = _info.FolderFr.ToNull();
                    cmd.Parameters.Add("@FolderZhcn", SqlDbType.NVarChar, 100).Value = _info.FolderZhcn.ToNull();
                    cmd.Parameters.Add("@FolderZhtw", SqlDbType.NVarChar, 100).Value = _info.FolderZhtw.ToNull();
                    cmd.Parameters.Add("@ActiveFolder", SqlDbType.Char, 1).Value = _info.ActiveFolder.ToNull();
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
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_folderID">The _folder ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _deptID, int _folderID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicFolder_DEL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, int _deptID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] _listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_APP_PublicFolder_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                            cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _listStr[i];
                            cmd.Parameters.Add("@FolderOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 폴더 아이디로  정보 가져오기 (0 이면 최상위 메뉴들 가져옴)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_folderID">The _folder ID.</param>
        /// <returns></returns>
        public List<BE.PublicFolderInfo> GetFolder(int _companyID, int _deptID, int _folderID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_PublicFolder_SelByID");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;
                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetPublicFolderInfoList(reader);
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
        /// 작성자 : 심재운
        /// 부서별 전체 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public DataSet GetFolder(int _companyID, int _deptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_PublicFolder_SelGetTree", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetFolder");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}