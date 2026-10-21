using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class CirculationAttachFile : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.CirculationAttachFileInfo GetCirculationAttachFileInfo(IDataReader _reader)
        {
            return new CirculationAttachFileInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                CirculationDocumentID = (_reader["CirculationDocumentID"] == DBNull.Value) ? -1 : _reader["CirculationDocumentID"].ToString().ToNumber<int>(),
                CirculationAttachFileID = (_reader["CirculationAttachFileID"] == DBNull.Value) ? -1 : _reader["CirculationAttachFileID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CirculationAttachFileInfo> GetCirculationAttachFileInfoList(IDataReader reader)
        {
            List<CirculationAttachFileInfo> list = new List<CirculationAttachFileInfo>();
            while (reader.Read())
                list.Add(GetCirculationAttachFileInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 첨부 파일 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirDocID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.CirculationAttachFileInfo> GetAttachFileLists(int _companyID, int _cirDocID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirAttachFile_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CirculationDocumentID", SqlDbType.Int).Value = _cirDocID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCirculationAttachFileInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 심재운
        /// 목적 : ID 별 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirDocID">The _document ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public List<BE.CirculationAttachFileInfo> GetAttachFileLists(int _companyID, int _cirDocID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirAttachFile_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CirculationDocumentID", SqlDbType.Int).Value = _cirDocID;
                cmd.Parameters.Add("@CirculationAttachFileID", SqlDbType.Int).Value = _attachFileID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCirculationAttachFileInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첨부파일 중에 이미지만 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_cirDocID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.CirculationAttachFileInfo> GetAttachImageLists(int _companyID, int _cirDocID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirAttachFile_SelGetImage");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CirculationDocumentID", SqlDbType.Int).Value = _cirDocID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCirculationAttachFileInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첨부파일 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_cirDocID">The _ list ID.</param>
        /// <returns></returns>
        public int GetAttachCnt(int _companyID, int _cirDocID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirAttachFile_SelCnt");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CirculationDocumentID", SqlDbType.Int).Value = _cirDocID;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion GET()

        #region ADD()

        #endregion ADD()

        #region MOD()

        #endregion MOD()

        #region DEL()

        #endregion DEL()
    }
}