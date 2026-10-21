using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class ListOption : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListOptionInfo GetListOption(IDataReader _reader)
        {
            return new ListOptionInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ItemID = (_reader["ItemID"] == DBNull.Value) ? -1 : _reader["ItemID"].ToString().ToNumber<int>(),
                ItemSubID = (_reader["ItemSubID"] == DBNull.Value) ? -1 : _reader["ItemSubID"].ToString().ToNumber<int>(),
                Status = _reader["Status"].ToString().ToNull(),
                StartDate = _reader["StartDate"].ToString().ToNull(),
                EndDate = _reader["EndDate"].ToString().ToNull(),                                
                RequestDate = _reader["RequestDate"].ToString().ToNull(),
                CompleteRequestDate = _reader["CompleteRequestDate"].ToString().ToNull(),
                CompleteDate = _reader["CompleteDate"].ToString().ToNull(),
                ExcessDay = _reader["ExcessDay"].ToString().ToNull(),  
                IsClose = _reader["IsClose"].ToString().ToNull(),
                IsGubun = _reader["IsGubun"].ToString().ToNull(),             
                SourceID = (_reader["SourceID"] == DBNull.Value) ? -1 : _reader["SourceID"].ToString().ToNumber<int>(),
                TaskNumber = _reader["TaskNumber"].ToString().ToNull(),
                TaskSeq = (_reader["TaskSeq"] == DBNull.Value) ? -1 : _reader["TaskSeq"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<ListOptionInfo> GetListOptionList(IDataReader reader)
        {
            List<ListOptionInfo> list = new List<ListOptionInfo>();
            while (reader.Read())
                list.Add(GetListOption(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 옵셥 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public List<BE.ListOptionInfo> GetListOption(int _companyID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ListOption_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListOptionList(reader);
                }
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-15
        /// 작성목적 : 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();                                        
                    cmd.Parameters.Add("@RequestDate", SqlDbType.Char, 8).Value = _info.RequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.Char, 8).Value = _info.CompleteDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
                    cmd.Parameters.Add("@IsGubun", SqlDbType.Char, 1).Value = _info.IsGubun.ToNull();
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;
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
                throw;
            }
        }

       
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 완료취소하기 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModCompCancel(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdCompCancel", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;                    
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();                    
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 수정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
                    cmd.Parameters.Add("@IsGubun", SqlDbType.Char, 1).Value = _info.IsGubun.ToNull();
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-20
        /// 작성목적 : 접수하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAccept(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdAccept", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();                   
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-20
        /// 작성목적 : 완료하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModComplete(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdComplete", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.Char, 8).Value = _info.CompleteDate.ToNull();                    
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 재검토요청하기 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModReject(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdReject", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();                    
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 재요청하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModRecall(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdRecall", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
                    cmd.Parameters.Add("@RequestDate", SqlDbType.Char, 8).Value = _info.RequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
                    cmd.Parameters.Add("@IsGubun", SqlDbType.Char, 1).Value = _info.IsGubun.ToNull();
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-28
        /// 작성목적 : 변경저장하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModIngDetail(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListOption_UpdIngDetail", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
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

        #endregion 
    }
}
