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
    public class ScheShareList : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public ScheShareListInfo GetScheShare(IDataReader _reader)
        {
            try
            {
                return new ScheShareListInfo(
                  (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                    (_reader["ScheduleID"] == DBNull.Value) ? -1 : (int)_reader["ScheduleID"],
                    (_reader["ScheShareListID"] == DBNull.Value) ? -1 : (int)_reader["ScheShareListID"],
                    _reader["UserName"].ToString() ,
                    _reader["UserMail"].ToString() ,
                    (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                    (_reader["DeptID"] == DBNull.Value) ? -1 : (int)_reader["DeptID"]
                    
                );
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetScheShare", exc);
                throw exc;
            }
        }

        public ScheShareList()
        {
        }


        /// <summary>
        /// Gets the schedule list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ScheShareListInfo> GetScheShareList(IDataReader reader)
        {
            List<ScheShareListInfo> list = new List<ScheShareListInfo>();
            while (reader.Read())
                list.Add(GetScheShare(reader));

            return list;
        }


        #region Get

        /// <summary>
        /// 공유리스트 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<BE.ScheShareListInfo> GetScheShareList(int companyid, int scheduleID,int userid)
        {

            using (var cn = new SqlConnection(ConnSolution(companyid)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ScheShareListSelect");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyid;
                cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = scheduleID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;
                
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetScheShareList(reader);
                }
            }
        }





        
        /// <summary>
        /// 공유리스트 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetScheShareListTable(int companyid, int scheduleID, int userid)
        {

            using (var cn = new SqlConnection(ConnSolution(companyid)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ScheShareListSelect");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyid;
                cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = scheduleID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userid;

                cn.Open();

                return  ExecuteDataTableReader(cmd);
            }
        }



        public DataTableReader GetShareListInfoUser(int companyid, int userid)
        {
            using (var cn = new SqlConnection(ConnSolution(companyid)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_NOTI_USER");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyid;
                cmd.Parameters.Add("@USERID  ", SqlDbType.Int).Value = userid;
                
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public DataTableReader GetShareListInfoDept(int companyid, int deptid)
        {
            using (var cn = new SqlConnection(ConnSolution(companyid)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_NOTI_DEPT");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyid;
                
                cmd.Parameters.Add("@DeptID   ", SqlDbType.Int).Value = deptid;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        #endregion

        #region ADD
        /// <summary>
        /// Adds the list.
        /// </summary>
        /// <param name="_listinfo">The  .</param>
        /// <returns></returns>
        public void AddScheShareLis(ScheShareListInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ScheShareList_Ins", cn);


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar,100).Value = _listinfo.UserName;
                    cmd.Parameters.Add("@UserMail", SqlDbType.NVarChar, 100).Value = _listinfo.UserMail;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _listinfo.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _listinfo.DeptID;
                    //cmd.Parameters.Add("@ShareDeptID", SqlDbType.Int).Value = _listinfo.ShareDeptID;
                    cn.Open();

                    ExecuteScalar(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                
            }

        }
        



        #endregion

        #region Del

        public bool DelScheShareLis(ScheShareListInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ScheShareList_Del", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                     
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
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
        #endregion

    }
}
