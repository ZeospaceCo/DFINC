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
    public class OpenUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@OpenUserID", SqlDbType.Int).Value = _info.OpenUserID;
        cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
        cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;

        */


        #region datatable
        public OpenUser()
        {
        }


        private OpenUserInfo GetOpenUserList(IDataReader _reader)
        {
            return new OpenUserInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["OpenUserID"] == DBNull.Value) ? -1 : (int)_reader["OpenUserID"],
                (_reader["GroupID"] == DBNull.Value) ? -1 : (int)_reader["GroupID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                _reader["Name"].ToString()
                );
        }

        protected List<OpenUserInfo> GetOpenUserInfoList(IDataReader reader)
        {
            List<OpenUserInfo> list = new List<OpenUserInfo>();
            while (reader.Read())
                list.Add(GetOpenUserList(reader));
            return list;
        }
        #endregion 

        

        #region GET()
        /// <summary>
        /// 작성자   : 김광일
        /// 작성일자 : 2011-06-10
        /// 작성목적 : 공유대상
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_StartDate"></param>
        /// <returns></returns>
        public DataTableReader GetOpenUser(int _companyID, int _UserID )
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_OpenUser_Select");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                
                cn.Open();
                return ExecuteDataTableReader(cmd);
                
            }
        }


        public DataSet GetOpenUserDs(int _companyID, int _UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_OpenUser_Select");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();
                return ExecuteDataSet(cmd,"SHARE");

            }
        }



        public DataTableReader GetOpenUserCombo(int _companyID, int _UserID, int _DeptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_OpenUserCombo_Select");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                //cmd.Parameters.Add("@DeptID ", SqlDbType.Int).Value = _DeptID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public DataTableReader GetOpenUserAll(int _companyID, int _UserID, int _DeptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_OpenUserAllUser_Select");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@DeptID ", SqlDbType.Int).Value = _DeptID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        



        /// <summary>
        /// 스케줄의 공유된 유저 정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_DeptID"></param>
        /// <returns></returns>
        public DataTableReader GetShareListInfo(int _Scheduleid, int _UserID, int _CompanyId)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyId)))
            {

                var cmd = new SqlCommand("DP_PIM_SCHEDULE_ShareInfo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                 
                
                cmd.Parameters.Add("@Scheduleid", SqlDbType.Int).Value = _Scheduleid;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@CompanyID ", SqlDbType.Int).Value = _CompanyId;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        public DataTableReader GetAdminShareListInfo(int _pagesize,int _PagesizePageNo,int _companyID,string _NAME ,string _kind)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {

                var cmd = new SqlCommand("DP_PIM_OpenUser_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                 
                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pagesize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _PagesizePageNo;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar,100).Value = _NAME;
                cmd.Parameters.Add("@KIND ", SqlDbType.Char,1).Value = _kind;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public int GetAdminShareListInfoCnt(int _companyID,string _NAME ,string _kind)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PIM_OpenUser_Sel_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar,100).Value = _NAME;
                cmd.Parameters.Add("@KIND ", SqlDbType.Char,1).Value = _kind;

                cn.Open();
                return int.Parse(ExecuteScalar(cmd).ToString());
            }
        }


        public DataTableReader GetOpenUserListPop(int _companyID, int _UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {

                var cmd = new SqlCommand("DP_PIM_OPENUSER_ADMIN", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add("@USERID", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@COMPANYID", SqlDbType.Int).Value = _companyID;
                 
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        #endregion

        #region ADD()


        public int OpenUserAdd(OpenUserInfo _info)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_OpenUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@OpenUserID", SqlDbType.Int).Value = _info.OpenUserID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@kind", SqlDbType.Char).Value = _info.kind;

                    cn.Open();

                    rtnVal = ExecuteNonQuery(cmd);
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
        public int OpenUserDel(OpenUserInfo _info)
        {
            try
            {
                
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_OpenUser_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                   

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }



        public void OpenUserDelUserid(int _companyID, int _UserID,int _OpenUserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_OpenUser_UserID_del");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@USERID ", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@OpenUserID   ", SqlDbType.Int).Value = _OpenUserID;
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }

        public void OpenUserAdminDelUserid(int _companyID, int _UserID, int _OpenUserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_OpenUser_UserID_del");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@USERID ", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@OpenUserID   ", SqlDbType.Int).Value = _OpenUserID;
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }

        public void OpenUserAdminDelDeptid(int _companyID, int _UserID, int _GroupID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_OpenUser_DeptID_del");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@USERID ", SqlDbType.Int).Value = _UserID;
                cmd.Parameters.Add("@GroupID     ", SqlDbType.Int).Value = _GroupID;
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }
        
        public void OpenUserAdminDelAllid(int _companyID, int _UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_OpenUser_UserIDAll_del");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@USERID ", SqlDbType.Int).Value = _UserID;
                
                cn.Open();
                ExecuteNonQuery(cmd);
            }
        }



        #endregion
    }
}
