using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Search.BE.Coop;

namespace Zeospace.DeskPlus.Search.DAL
{
    public class Coop : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #region 업무협업

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
                ID = _reader["CompanyID"].ToString() + "_" + _reader["GroupID"].ToString() + "_" + _reader["ListID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),
                ItemNameBase = _reader["ItemNameBase"].ToString().ToNull(),
                ItemManyName = _reader["ItemManyName"].ToString().ToNull(),
                ItemSubNameBase = _reader["ItemSubNameBase"].ToString().ToNull(),
                ItemSubManyName = _reader["ItemSubManyName"].ToString().ToNull(),
                CoopNumber = _reader["CoopNumber"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),              
                FilePath = "",
                FileGUID = "",
                //====================================================== 검색 필드
                Title = _reader["Title"].ToString().ToNull(),
                FileName = _reader["Title"].ToString().ToNull(),
                Content = _reader["MobileContent"].ToString().ToNull(),

                //====================================================== ETC 필드
                ListOrder = (_reader["ListOrder"] == DBNull.Value) ? -1 : _reader["ListOrder"].ToString().ToNumber<int>(),
                Depth = (_reader["Depth"] == DBNull.Value) ? -1 : _reader["Depth"].ToString().ToNumber<int>(),
                Status = _reader["Status"].ToString().ToNull(),
                IsAlarm = _reader["IsAlarm"].ToString().ToNull(),
                DeptName = _reader["DeptName"].ToString().ToNull(),
                TitleName = _reader["TitleName"].ToString().ToNull(),
                DutyName = _reader["DutyName"].ToString().ToNull(),
                StartDate = _reader["StartDate"].ToString().ToNull(),
                IsClose = _reader["IsClose"].ToString().ToNull(),
                EndDate = _reader["EndDate"].ToString().ToNull(),
                ExcessDay = _reader["ExcessDay"].ToString().ToNull(),
                Difficultys = _reader["Difficultys"].ToString().ToNull(),
                RequestDate = _reader["RequestDate"].ToString().ToNull(),
                CompleteRequestDate = _reader["CompleteRequestDate"].ToString().ToNull(),
                CompleteDate = _reader["CompleteDate"].ToString().ToNull(),
                StatusLast = _reader["StatusLast"].ToString().ToNull()

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

        #endregion 업무협업

        #region 첨부파일

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private AttachFileInfo GetAttach(IDataReader _reader)
        {
            return new AttachFileInfo
            {
                //====================================================== 추가 필드
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : (long)_reader["FileLength"],
                ID = _reader["CompanyID"].ToString() + "_" + _reader["GroupID"].ToString() + "_" + _reader["ListID"].ToString() + "_" + _reader["AttachFileID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ItemNameBase = _reader["ItemNameBase"].ToString().ToNull(),
                ItemManyName = _reader["ItemManyName"].ToString().ToNull(),
                ItemSubNameBase = _reader["ItemSubNameBase"].ToString().ToNull(),
                ItemSubManyName = _reader["ItemSubManyName"].ToString().ToNull(),
                CoopNumber = _reader["CoopNumber"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileGUID = _reader["FileGUID"].ToString().ToNull(),

                //====================================================== 검색 필드
                Title = _reader["Title"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull()
            };
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<AttachFileInfo> GetAttachList(IDataReader reader)
        {
            List<AttachFileInfo> list = new List<AttachFileInfo>();
            while (reader.Read())
                list.Add(GetAttach(reader));
            return list;
        }

        #endregion 첨부파일

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
                string query = "SELECT * FROM VW_Search_COOP_GetCoopList";
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
        public List<AttachFileInfo> GetAttachAll()
        {
            using (var cn = new SqlConnection(ConnSolution()))
            {
                //string query = AppSettingHelper.GetDynamicQuery(AppSettingType.Board, "QRY_APP_Doc_Sel");
                string query = "SELECT * FROM VW_Search_COOP_GetAttachList";
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 :
        /// 작성목적 : Prototype Test
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetListID(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = "SELECT DISTINCT ListID FROM VW_Search_COOP_GetListIDByUserID WHERE TargetID = @UserID or UserID = @UserID";
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

        public AttachFileInfo GetAttach(int ID)
        {
            return GetAttachAll().SingleOrDefault(x => x.ID.Equals(ID));
        }

        #endregion GET()
    }
}