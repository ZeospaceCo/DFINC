using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class MBOShareUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
        cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
        cmd.Parameters.Add("@TypeTargetNum", SqlDbType.Int).Value = _info.TypeTargetNum;
        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar,50).Value = _info.UserName.ToNull().ToXSS();
        cmd.Parameters.Add("@UserMail", SqlDbType.NVarChar,50).Value = _info.UserMail.ToNull().ToXSS();

        */


        /// <summary>
        /// Gets the MBO share user.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.MBOShareUserInfo GetMBOShareUser(IDataReader _reader)
        {
            return new BE.MBOShareUserInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["MBOID"] == DBNull.Value) ? -1 : (int)_reader["MBOID"],
                (_reader["TypeTargetNum"] == DBNull.Value) ? -1 : (int)_reader["TypeTargetNum"],
                _reader["UserName"].ToString(),
                _reader["UserMail"].ToString(),
                (_reader["TargetUserID"] == DBNull.Value) ? -1 : (int)_reader["TargetUserID"]
            );
        }

        /// <summary>
        /// Gets the MBO share user list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<MBOShareUserInfo> GetMBOShareUserList(IDataReader reader)
        {
            List<MBOShareUserInfo> list = new List<MBOShareUserInfo>();
            while (reader.Read())
                list.Add(GetMBOShareUser(reader));
            return list;
        }

        #region GET()

        public List<BE.MBOShareUserInfo> GetMBOFile(BE.MBOShareUserInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_MBO_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                //cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetMBOShareUserList(reader);
                }

            }
        }


        public DataTableReader GetMBOShareUserID(int _CompanyID, int _MBOID)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_MBOShareUser_Sel_ID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region ADD()

        public void AddMBOShareUser(BE.MBOShareUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOShareUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
                    cmd.Parameters.Add("@TypeTargetNum", SqlDbType.Int).Value = _info.TypeTargetNum;

                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = _info.UserName;
                    cmd.Parameters.Add("@UserMail", SqlDbType.NVarChar, 100).Value = _info.UserMail;
                    cmd.Parameters.Add("@TargetUserID", SqlDbType.Int).Value = _info.TargetUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);

            }
        }


        #endregion

        #region MOD()
        #endregion

        #region DEL()
        public void DelMBOShareUser(int _CompanyID, int _MBOID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOShareUser_DEL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;
                    

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);

            }
        }

        #endregion 
    }
}
