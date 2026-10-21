using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Search.BE.Address;

namespace Zeospace.DeskPlus.Search.DAL
{
    public class Address : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #region 주소록

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private ListInfo Get(IDataReader _reader)
        {
            return new ListInfo
            {
                //====================================================== 추가 필드
                ID = _reader["CompanyID"].ToString() + "_" + _reader["CategoryID"].ToString() + "_" + _reader["GroupID"].ToString() + "_" + _reader["ObjectID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                GroupID = _reader["GroupID"].ToString().ToNull(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ObjectID = (_reader["ObjectID"] == DBNull.Value) ? -1 : _reader["ObjectID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                MobilePhone = _reader["MobilePhone"].ToString().ToNull(),
                OfficePhone = _reader["OfficePhone"].ToString().ToNull(),
                Title = _reader["Title"].ToString().ToNull(),
                Duty = _reader["Duty"].ToString().ToNull(),
                Memo = _reader["Memo"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                DeptBase = _reader["DeptBase"].ToString().ToNull(),

                //====================================================== 검색 필드
                Name = _reader["Name"].ToString().ToNull(),
                Mail = _reader["Mail"].ToString().ToNull(),
                DeptName = _reader["DeptName"].ToString().ToNull(),
                Company = _reader["Company"].ToString().ToNull()                
            };
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ListInfo> GetList(IDataReader reader)
        {
            List<ListInfo> list = new List<ListInfo>();
            while (reader.Read())
                list.Add(Get(reader));
            return list;
        }

        #endregion 주소록

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <returns></returns>
        public List<ListInfo> GetListAll()
        {
            using (var cn = new SqlConnection(ConnSolution()))
            {
                //string query = AppSettingHelper.GetDynamicQuery(AppSettingType.Board, "QRY_APP_Doc_Sel");
                string query = "SELECT * FROM VW_Search_ADD_GetAddressList";
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetID(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = "SELECT CompanyID, CategoryID, GroupID, ObjectID FROM VW_Search_ADD_GetAddressList WHERE UserID = @UserID or UserID = 0";
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ListInfo GetList(int ID)
        {
            return GetListAll().SingleOrDefault(x => x.ID.Equals(ID));
        }

        #endregion GET()
    }
}
