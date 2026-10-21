using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Search.BE.EDM;

namespace Zeospace.DeskPlus.Search.DAL
{
    public class EDM : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #region 문서관리

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
                ID = _reader["CompanyID"].ToString() + "_" + _reader["FolderID"].ToString() + "_" + _reader["DocumentID"].ToString() + "_" + _reader["VersionID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ObjectID = _reader["VersionID"].ToString().ToNull(),
                CategoryName = _reader["FolderName"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteName"].ToString().ToNull(),
                FilePath = "",
                FileGUID = "",
                //====================================================== 검색 필드
                Title = _reader["DocName"].ToString().ToNull(),
                FileName = _reader["Keyword"].ToString().ToNull(),
                Content = _reader["Description"].ToString().ToNull(),

                //====================================================== ETC 필드
                ClassName = _reader["ClassName"].ToString().ToNull(),
                DocName = _reader["DocName"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull(),
                Keyword = _reader["Keyword"].ToString().ToNull(),
                ACLID = _reader["ACLID"].ToString().ToNull(),
                //WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                //WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                //WriteUserInfo = _reader["WriteUserInfo"].ToString().ToNull(),
                WriteName = _reader["WriteName"].ToString().ToNull(),
                OwnerID = _reader["OwnerID"].ToString().ToNull(),
                FolderName = _reader["FolderName"].ToString().ToNull(),
                DocType = _reader["DocType"].ToString().ToNull(),
                PublishID = _reader["PublishID"].ToString().ToNull(),
                CheckOutName = _reader["CheckOutName"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                CheckOutStatus = _reader["CheckOutStatus"].ToString().ToNull(),
                FolderType = _reader["FolderType"].ToString().ToNull()
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

        #endregion 문서관리

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
                ID = _reader["CompanyID"].ToString() + "_" + _reader["FolderID"].ToString() + "_" + _reader["DocumentID"].ToString() + "_" + _reader["VersionID"].ToString() + "_" + _reader["AttachFileID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ObjectID = _reader["VersionID"].ToString().ToNull(),
                CategoryName = _reader["FolderName"].ToString().ToNull(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : (long)_reader["FileLength"],
                CheckOutStatus = _reader["CheckOutStatus"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteName"].ToString().ToNull(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileGUID = _reader["FileGUID"].ToString().ToNull(),

                //====================================================== 검색 필드
                Title = _reader["DocName"].ToString().ToNull(),
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
            using (var cn = new SqlConnection(ConnEDMS(0)))
            {
                //string query = AppSettingHelper.GetDynamicQuery(AppSettingType.Board, "QRY_APP_Doc_Sel");
                string query = "SELECT * FROM VW_Search_EDM_GetDocumentList";
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
            using (var cn = new SqlConnection(ConnEDMS(0)))
            {
                //string query = AppSettingHelper.GetDynamicQuery(AppSettingType.Board, "QRY_APP_Doc_Sel");
                string query = "SELECT * FROM VW_Search_EDM_GetAttachList";
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
        public DataTableReader GetDocumentID(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnEDMS(0)))
                {
                    string query = "SELECT * FROM VW_Search_EDM_GetDocumentByUserID WHERE userID = @UserID or userID = 0";
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