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
    public class ToDo: Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ToDoInfo GetTodo(IDataReader _reader)
        {
            return new ToDoInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["ToDoID"] == DBNull.Value) ? -1 : (int)_reader["ToDoID"],
                _reader["Type"].ToString() ,
                _reader["Status"].ToString() ,
                _reader["Priority"].ToString() ,
                _reader["Title"].ToString() ,
                _reader["Content"].ToString() ,
                (_reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartDate"],
                (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"],
                _reader["ScheduleYN"].ToString() ,
                _reader["IsOpen"].ToString() ,
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["ReservationID"] == DBNull.Value) ? -1 : (int)_reader["ReservationID"],
                (_reader["OpenType"] == DBNull.Value) ? -1 : (int)_reader["OpenType"]
                );
        }

        protected List<ToDoInfo> GetToDoInfoList(IDataReader reader)
        {
            List<ToDoInfo> list = new List<ToDoInfo>();
            while (reader.Read())
                list.Add(GetTodo(reader));
            return list;
        }

        #endregion 


        #region GET()

        /// <summary>
        /// 작성자   : 김광일
        /// 작성일자 : 2011-06-10
        /// 작성목적 : 할일가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_StartDate"></param>
        /// <returns></returns>
        public  List<BE.ToDoInfo>  GetTodoList(BE.ToDoInfo info)
        {
            using (var cn = new SqlConnection(ConnSolution(info.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDo_Select");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = info.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = info.StartDate;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetToDoInfoList(reader);
                }
            }
        }


        public DataTableReader GetTodoID(int _CompanyID,int _ToDoID)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDo_SelectID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _ToDoID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 할일 리스트 갖고오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_toDoID">The _to do ID.</param>
        /// <returns></returns>
        public List<BE.ToDoInfo> GetTodoListInfo(int _companyID, int _toDoID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ToDo_SelectID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _toDoID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetToDoInfoList(reader);
                }
            }
        }



        #endregion

        #region ADD()

        public int TodoAdd(ToDoInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDo_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _info.Type;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;
                    cmd.Parameters.Add("@Priority", SqlDbType.VarChar, 2).Value = _info.Priority;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 300).Value = _info.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@ScheduleYN", SqlDbType.Char, 1).Value = _info.ScheduleYN;
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _info.IsOpen;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _info.ReservationID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    
                    cmd.Parameters.Add("@OpenType", SqlDbType.Int).Value = _info.OpenType;
                    
                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                    //return ExecuteNonQuery(cmd);
                }
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
        /// <summary>
        /// 할일수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int ModTodo(ToDoInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDo_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _info.ToDoID;

                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _info.Type;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;
                    cmd.Parameters.Add("@Priority", SqlDbType.VarChar, 2).Value = _info.Priority;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 300).Value = _info.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@ScheduleYN", SqlDbType.Char, 1).Value = _info.ScheduleYN;
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _info.IsOpen;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _info.ReservationID;
                    cmd.Parameters.Add("@OpenType", SqlDbType.Int).Value = _info.OpenType;
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

        #endregion

        #region DEL()
        public bool DelTodo(int _companyId,int _todoID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyId)))
                {
                    var cmd = new SqlCommand("DP_PIM_ToDo_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyId;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _todoID;
 
                    cn.Open();

                    ExecuteNonQuery(cmd);
                    return true;
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion
        
        #region 할일 통계 일간
        //월별 건수

        /// <summary>
        /// 나의 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_MineCount(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_MINE_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 개인 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_PersonCount(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_PERSON_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 부서 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_DeptCount(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_DEPT_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공유 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_ShareCount(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_SHARE_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 전체 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_AllCount(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_ALL_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                //cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        /////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 나의 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_Mine(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_MINE", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 개인 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_Person(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_PERSON", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        

        /// <summary>
        /// 부서 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_Dept(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_DEPT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공유 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_Share(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_SHARE", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 전체 
        /// </summary>
        /// <param name="_be"></param>
        /// <returns></returns>
        public DataTableReader GetTodo_All(BE.ToDoInfo _be)
        {
            using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TODO_SEL_ALL", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                //cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = _be.DeptID;
                cmd.Parameters.Add("@STARTDATE", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        #endregion


       


    }



  

}
