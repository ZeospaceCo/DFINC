using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class Env : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// Gets the env.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.EnvInfo GetEnv(IDataReader _reader)
        {
            return new EnvInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                Gubun = _reader["Gubun"].ToString().ToNull(),
                KeyID = _reader["KeyID"].ToString().ToNull(),
                KeyValue = _reader["KeyValue"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        /// <summary>
        /// Gets the company list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<EnvInfo> GetEnvList(IDataReader reader)
        {
            List<EnvInfo> list = new List<EnvInfo>();
            while (reader.Read())
                list.Add(GetEnv(reader));
            return list;
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : 심재운
        /// 환경정보 수정
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(BE.EnvInfo _listinfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Env_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 100).Value = _listinfo.Gubun.ToNull().ToXSS();
                    cmd.Parameters.Add("@KeyID", SqlDbType.VarChar, 100).Value = _listinfo.KeyID.ToNull().ToXSS();
                    cmd.Parameters.Add("@KeyValue", SqlDbType.VarChar, 100).Value = _listinfo.KeyValue.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _listinfo.WriteUserID;

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

        #endregion ADD()
    }
}