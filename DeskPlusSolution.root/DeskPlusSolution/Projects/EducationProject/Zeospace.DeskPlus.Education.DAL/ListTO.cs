using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Education.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class ListTO : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListTOInfo GetListTO(IDataReader _reader)
        {
            return new ListTOInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ListTOID = (_reader["ListTOID"] == DBNull.Value) ? -1 : _reader["ListTOID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                IsGroup = _reader["IsGroup"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                TOOrders = (_reader["TOOrders"] == DBNull.Value) ? -1 : _reader["TOOrders"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDateYMD = _reader["WriteDateYMD"].ToString().ToNull(),

                #region ####  부서,직급,직책 다국어 시작 #######
                TitleID = (_reader["TitleID"] == DBNull.Value) ? -1 : _reader["TitleID"].ToString().ToNumber<int>(),

                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),

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
                #endregion

            };
        }

        protected List<ListTOInfo> GetListTOList(IDataReader reader)
        {
            List<ListTOInfo> list = new List<ListTOInfo>();
            while (reader.Read())
                list.Add(GetListTO(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public List<BE.ListTOInfo> GetListTo(int _companyID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"SELECT List.*, DC.TitleID, DC.DeptBase, DC.DeptKo, DC.DeptEn, DC.DeptJa, DC.DeptFr, DC.DeptZhcn, DC.DeptZhtw, DC.DeptManyName,     
DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa, DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw, DC.TitleManyName,   
DC.DutyBase, DC.DutyKo, DC.DutyEn, DC.DutyJa, DC.DutyFr, DC.DutyZhcn, DC.DutyZhtw, DC.DutyManyName  
FROM DP_EDU_ListTO List  LEFT OUTER JOIN  DP_COM_DeptCode AS DC   ON List.CompanyID = DC.CompanyID AND List.DeptCodeID = DC.DeptCodeID   
WHERE List.CompanyID = @CompanyID AND List.ListID = @ListID";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListTOList(reader);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetListAlarm(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"SELECT Target.UserID, 'TO' AS IsType
FROM DP_EDU_TOUser AS Target
WHERE Target.CompanyID = @CompanyID AND Target.ListID = @ListID
  AND EXISTS (
      SELECT 1
      FROM VW_UserInfo AS U
      WHERE U.CompanyID = Target.CompanyID AND U.UserID = Target.UserID
        AND U.ActiveCompany = 1 AND U.ActiveUser = 1
  )
ORDER BY Target.UserID";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_list"></param>
        /// <param name="_writeUserID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, string _list, int _writeUserID)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] _listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            string[] _str = _listStr[i].Split('_');

                            var cmd = new SqlCommand("DP_EDU_ListTO_Ins", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _str[1].ToNumber<int>();
                            cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _str[2].ToNumber<int>();
                            cmd.Parameters.Add("@IsGroup", SqlDbType.Char, 1).Value = _str[0];
                            cmd.Parameters.Add("@TOOrders", SqlDbType.Int).Value = (i + 1);
                            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

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
                throw;
            }

        }      

        /// <summary>
        /// </summary>
        public int AddAll(int _companyID, int _listID, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_ListTO_InsAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }    

        #endregion
    }
}
