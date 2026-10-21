using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Task.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class Source : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private SourceInfo GetSource(IDataReader _reader)
        {
            return new SourceInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                SourceID = (_reader["SourceID"] == DBNull.Value) ? -1 : _reader["SourceID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                IsActive = _reader["IsActive"].ToString().ToNull(),
                SourceOrder = (_reader["SourceOrder"] == DBNull.Value) ? -1 : _reader["SourceOrder"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<SourceInfo> GetSourceList(IDataReader reader)
        {
            List<SourceInfo> list = new List<SourceInfo>();
            while (reader.Read())
                list.Add(GetSource(reader));
            return list;
        }

        #endregion

        #region GET             

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-14
        /// 작성목적 : 출처 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.SourceInfo> GetSource(int _companyID, bool _isActive)
        {
            string isActive = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Source_Sel");
                query = string.Format(query, isActive);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSourceList(reader);
                }
            }
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 작성목적 : 출처ID 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.SourceInfo> GetSourceID(int _companyID,int _sourceID)
        {            
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Source_SelSourceID");                

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _sourceID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSourceList(reader);
                }
            }
        }

        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 작성목적 : 출처 저장하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddSource(BE.SourceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Source_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _info.IsActive;
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
        #endregion

        #region Mod
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 작성목적 : 출처 수정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModSource(BE.SourceInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Source_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
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
                return false;
            }
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 출처 순서 수정하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sourceList"></param>
        /// <returns></returns>
        public bool SourceOrder(int _companyID, string _sourceList)
        {
            try
            {
                if (_sourceList.Length > 0)
                {
                    string[] _sourceListStr = _sourceList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _sourceListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_TAS_Source_Order", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _sourceListStr[i];
                            cmd.Parameters.Add("@SourceOrder", SqlDbType.Int).Value = (i + 1);

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
                return false;
            }
        }
        #endregion

        #region DEL
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 작성목적 : 출처ID 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sourceID"></param>
        /// <returns></returns>
        public bool DelSource(int _companyID, int _sourceID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Source_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _sourceID;

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
