using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class FolderStatus : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.FolderStatusInfo GetFolderStatusInfo(IDataReader _reader)
        {
            return new FolderStatusInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderStatusID = _reader["FolderStatusID"].ToString().ToNull(),
                ParentFolderStatusID = _reader["ParentFolderStatusID"].ToString().ToNull(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                TargetPage = _reader["TargetPage"].ToString().ToNull(),
                ActiveFolder = _reader["ActiveFolder"].ToString().ToNull(),
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
        protected List<FolderStatusInfo> GetFolderStatusInfoList(IDataReader reader)
        {
            List<FolderStatusInfo> list = new List<FolderStatusInfo>();
            while (reader.Read())
                list.Add(GetFolderStatusInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자 : 심재운
        /// 결재상황조회 폴더 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCabinet">if set to <c>true</c> [_active cabinet].</param>
        /// <returns></returns>
        public DataSet GetFolderTree(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_FolderStatus_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetFolderTree");
            }
        }

        /// <summary>
        /// 작성자 : 이문호
        /// 결재상황조회 폴더 이름 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cabinetID">The _cabinet ID.</param>
        /// <param name="_activeCabinet">if set to <c>true</c> [_active cabinet].</param>
        /// <returns></returns>
        public List<BE.FolderStatusInfo> GetFolderStatusName(int _companyID, string _FolderStatusID, bool _activeFolder)
        {
            string activeFolder = (_activeFolder) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.HR, "QRY_HR_FolderStatus_SelByID");
                query = string.Format(query, activeFolder);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FolderStatusID", SqlDbType.VarChar, 500).Value = _FolderStatusID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetFolderStatusInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        #endregion ADD()

        #region MOD()

        #endregion MOD()

        #region DEL()

        #endregion DEL()
    }
}
