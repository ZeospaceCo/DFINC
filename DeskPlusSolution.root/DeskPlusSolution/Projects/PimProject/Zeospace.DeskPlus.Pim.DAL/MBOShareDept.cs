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
    public class MBOShareDept : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
        cmd.Parameters.Add("@TypeTargetNum", SqlDbType.Int).Value = _info.TypeTargetNum;
        cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
        cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar,50).Value = _info.DeptName.ToNull().ToXSS();

        */

        /// <summary>
        /// Gets the MBO share dept.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.MBOShareDeptInfo GetMBOShareDept(IDataReader _reader)
        {
            return new BE.MBOShareDeptInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["MBOID"] == DBNull.Value) ? -1 : (int)_reader["MBOID"],
                (_reader["TypeTargetNum"] == DBNull.Value) ? -1 : (int)_reader["TypeTargetNum"],
                (_reader["DeptID"] == DBNull.Value) ? -1 : (int)_reader["DeptID"],
                _reader["DeptName"].ToString()
            );
        }

        /// <summary>
        /// Gets the MBO share dept list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<MBOShareDeptInfo> GetMBOShareDeptList(IDataReader reader)
        {
            List<MBOShareDeptInfo> list = new List<MBOShareDeptInfo>();
            while (reader.Read())
                list.Add(GetMBOShareDept(reader));
            return list;
        }


        #region GET()

        public List<BE.MBOShareDeptInfo> GetMBOFile(BE.MBOShareDeptInfo _info)
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
                    return GetMBOShareDeptList(reader);
                }

            }
        }


        public DataTableReader GetMBOShareDeptID(int _CompanyID, int _MBOID)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_MBOShareDept_Sel_ID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;

                return ExecuteDataTableReader(cmd);
            }
        }


        #endregion

        #region ADD()

        public void AddMBOShareDeft(BE.MBOShareDeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOShareDept_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
                    cmd.Parameters.Add("@TypeTargetNum", SqlDbType.Int).Value = _info.TypeTargetNum;

                    cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 100).Value = _info.DeptID;
                    cmd.Parameters.Add("@DeptName", SqlDbType.NVarChar, 100).Value = _info.DeptName;


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
        public void DelMBOShareDept(int _CompanyID, int _MBOID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOShareDept_DEL", cn);
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
