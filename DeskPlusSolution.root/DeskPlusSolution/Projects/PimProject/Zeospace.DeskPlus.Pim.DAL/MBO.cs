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
    public class MBO : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
        cmd.Parameters.Add("@ParentMBOID", SqlDbType.Int).Value = _info.ParentMBOID;
        cmd.Parameters.Add("@MBOGroup", SqlDbType.Int).Value = _info.MBOGroup;
        cmd.Parameters.Add("@MBOOrder", SqlDbType.Int).Value = _info.MBOOrder;
        cmd.Parameters.Add("@Dept", SqlDbType.Int).Value = _info.Dept;
        cmd.Parameters.Add("@Title", SqlDbType.NVarChar,60).Value = _info.Title.ToNull().ToXSS();
        cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
        cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
        cmd.Parameters.Add("@ToDoListYN", SqlDbType.Char,1).Value = _info.ToDoListYN.ToNull().ToXSS();
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
        cmd.Parameters.Add("@DeptShared", SqlDbType.Int).Value = _info.DeptShared;

        */


        /// <summary>
        /// Gets the MBO.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.MBOInfo GetMBO(IDataReader _reader)
        {
            return new BE.MBOInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["MBOID"] == DBNull.Value) ? -1 : (int)_reader["MBOID"],
                (_reader["ParentMBOID"] == DBNull.Value) ? -1 : (int)_reader["ParentMBOID"],
                (_reader["MBOGroup"] == DBNull.Value) ? -1 : (int)_reader["MBOGroup"],
                (_reader["MBOOrder"] == DBNull.Value) ? -1 : (int)_reader["MBOOrder"],
                (_reader["Dept"] == DBNull.Value) ? -1 : (int)_reader["Dept"],
                _reader["Title"].ToString() ,
                (_reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartDate"],
                (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"],
                _reader["ToDoListYN"].ToString() ,
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["DeptShared"] == DBNull.Value) ? -1 : (int)_reader["DeptShared"]
            );
        }

        /// <summary>
        /// Gets the MBO list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<MBOInfo> GetMBOList(IDataReader reader)
        {
            List<MBOInfo> list = new List<MBOInfo>();
            while (reader.Read())
                list.Add(GetMBO(reader));
            return list;
        }

        #region GET() 
        public DataTableReader GetMBOInfo(BE.MBOInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_MBO_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _be.StartDate;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _be.DeptID;
                
                cn.Open();
                
                return ExecuteDataTableReader(cmd);

            }
        }

        public DataTableReader GetMBOInfoID(int _CompanyID,int _MBOID,int _DeptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_MBO_Sel_ID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value =_CompanyID;
                cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;
                //cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;

                return ExecuteDataTableReader(cmd);
            }
        }


        public int GetMBOInfoIDCnt(int _CompanyID, int _MBOID, int _DeptID )
        {
            int rtnVal = 0;
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_MBO_DEPT_ID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;
                cn.Open();
                //ExecuteNonQuery(cmd);
                rtnVal = ExecuteScalar(cmd).ToString().ToNumber<int>();
                //return 0;
            }

            return rtnVal;
        }




        #endregion 
 
        #region ADD() 
        
        public int AddMBO(BE.MBOInfo _info)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBO_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentMBOID", SqlDbType.Int).Value = _info.ParentMBOID;
                    cmd.Parameters.Add("@MBOGroup", SqlDbType.Int).Value = _info.MBOGroup;
                    cmd.Parameters.Add("@MBOOrder", SqlDbType.Int).Value = _info.MBOOrder;
                    cmd.Parameters.Add("@Dept", SqlDbType.Int).Value = _info.Dept;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar,120).Value = _info.Title;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@ToDoListYN", SqlDbType.Char).Value = _info.ToDoListYN;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptShared", SqlDbType.Int).Value = _info.DeptShared;

                    cn.Open();

                    //ExecuteNonQuery(cmd);
                    rtnVal = ExecuteScalar(cmd).ToString().ToNumber<int>();
                }

                return rtnVal;
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }
            
            
        #endregion 
 
        #region MOD() 
        #endregion 
 
        #region DEL()
        public void DelMBO(int _CompanyID,int _MBOID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBO_DEL", cn);
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
