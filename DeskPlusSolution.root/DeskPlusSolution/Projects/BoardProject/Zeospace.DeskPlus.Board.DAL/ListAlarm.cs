using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class ListAlarm : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the list etc.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private ListAlarmInfo GetListAlarm(IDataReader _reader)
        {
            return new ListAlarmInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                AlarmID = (_reader["AlarmID"] == DBNull.Value) ? -1 : _reader["AlarmID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                IsGroup = _reader["IsGroup"].ToString().ToNull(),
                IsLowRank = _reader["IsLowRank"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),

                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull()                
            };
        }

        protected List<ListAlarmInfo> GetListAlarmList(IDataReader reader)
        {
            List<ListAlarmInfo> list = new List<ListAlarmInfo>();
            while (reader.Read())
                list.Add(GetListAlarm(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-8
        /// 작성목적 : 추가
        /// <returns></returns>
        public bool Add(int _companyID, int _categoryID, int _listID, string _list, int _writeUserID)
        {
            try
            {
                if (_list != null && _list.Length > 0)
                {
                    string[] _listStr = _list.Split(':');
                    string[] _listStr2 = _listStr[0].Split(',');

                    string _isLowRank = _listStr[1];

                    if (_listStr[0].Length > 0)
                    {
                        using (var cn = new SqlConnection(ConnSolution(_companyID)))
                        {
                            cn.Open();

                            for (int i = 0; i < _listStr2.Length; i++)
                            {
                                string[] _str = _listStr2[i].Split('_');

                                var cmd = new SqlCommand("DP_BOA_ListAlarm_Ins", cn);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _str[1].ToNumber<int>();
                                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _str[2].ToNumber<int>();
                                cmd.Parameters.Add("@IsGroup", SqlDbType.Char, 1).Value = _str[0];
                                cmd.Parameters.Add("@IsLowRank", SqlDbType.Char, 1).Value = _isLowRank;
                                cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = (i + 1);
                                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

                                ExecuteNonQuery(cmd);
                            }
                        }
                    }
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
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-8
        /// 작성목적 : 삭제
        /// <returns></returns>
        public int Del(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ListAlarm_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

                    cn.Open();
                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 리스트알람 가져오기
        /// </summary>
        /// <returns></returns>
        public List<BE.ListAlarmInfo> GetList(int _companyID, int _categoryID, int _ListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListAlarm_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;

                    cn.Open();

                    return GetListAlarmList(ExecuteDataTableReader(cmd));
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 전체 사용자 아이디 가져오기(basedept = 1 자동임)
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetUserIDList(int _companyID)
        {
            Dictionary<int, string> userList = new Dictionary<int, string>();

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_ListAlarm_SelByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString().ToNumber<int>(), reader["AlarmBoard"].ToString());
                    }
                }
                return userList;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 전체 사용자 SMS 가져오기(basedept = 1 자동임)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetUserSMSList(int _companyID)
        {
            Dictionary<string, string> userList = new Dictionary<string, string>();

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_ListAlarmSMS_SelByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["MobilePhone"].ToString().ToString(), reader["AlarmBoard"].ToString());
                    }
                }
                return userList;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 알람대상자 가져오기
        public Dictionary<int, string> GetAlarm(int _listcompanyID, int _companyID, int _categoryID, int _ListID)
        {
            Dictionary<int, string> userList = new Dictionary<int, string>();

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_ListAlarm_SelByID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ListCompanyID", SqlDbType.Int).Value = _listcompanyID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString().ToNumber<int>(), reader["AlarmBoard"].ToString());
                    }
                }
                return userList;
            }
        }
        #endregion
    }
}