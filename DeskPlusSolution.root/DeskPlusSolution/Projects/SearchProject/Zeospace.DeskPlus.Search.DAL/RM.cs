using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Search.BE.RM;

namespace Zeospace.DeskPlus.Search.DAL
{
    public class RM : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #region 자원관리

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
                ID = _reader["CompanyID"].ToString() + "_" + _reader["CategoryID"].ToString() + "_" + _reader["ListID"].ToString() + "_" + _reader["ReservationID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                GroupID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ReservationID = (_reader["ReservationID"] == DBNull.Value) ? -1 : _reader["ReservationID"].ToString().ToNumber<int>(),
                Title = _reader["Title"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                FilePath = "",
                FileGUID = "",
                //====================================================== 검색 필드
                FileName = _reader["Title"].ToString().ToNull(),
                Subject = _reader["Subject"].ToString().ToNull(),
                Content = _reader["Content"].ToString().ToNull(),

                //====================================================== ETC 필드
                Authority = (_reader["Authority"] == DBNull.Value) ? -1 : _reader["Authority"].ToString().ToNumber<int>(),
                UsableQty = (_reader["UsableQty"] == DBNull.Value) ? -1 : _reader["UsableQty"].ToString().ToNumber<int>(),
                StartDate = _reader["StartDate"].ToString().ToNull(),
                EndDate = _reader["EndDate"].ToString().ToNull(),
                DifferMinite = _reader["DifferMinite"].ToString().ToNull(),
                UserImg = _reader["UserImg"].ToString().ToNull(),
                Usable = _reader["Usable"].ToString().ToNull(),
                Qty = _reader["Qty"].ToString().ToNull(),
                ManagerUserID = _reader["ManagerUserID"].ToString().ToNull(),
                MManyName = _reader["MManyName"].ToString().ToNull(),
                GroupStartDate = _reader["GroupStartDate"].ToString().ToNull()

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

        #endregion 자원관리

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
                ID = _reader["CompanyID"].ToString() + "_" + _reader["CategoryID"].ToString() + "_" + _reader["ListID"].ToString() + "_" + _reader["AttachFileID"].ToString(),
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                GroupID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ReservationID = (_reader["ReservationID"] == DBNull.Value) ? -1 : _reader["ReservationID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                Title = _reader["Title"].ToString().ToNull(),
                WriteDate = _reader["WriteDate"].ToString().ToNull(),
                WriteUserID = _reader["WriteUserID"].ToString().ToNull(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileGUID = _reader["FileGUID"].ToString().ToNull(),

                //====================================================== 검색 필드
                Subject = _reader["Subject"].ToString().ToNull(),
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
                string query = "SELECT * FROM VW_Search_RM_GetReserveList";
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
                string query = "SELECT * FROM VW_Search_RM_GetAttachList";
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
