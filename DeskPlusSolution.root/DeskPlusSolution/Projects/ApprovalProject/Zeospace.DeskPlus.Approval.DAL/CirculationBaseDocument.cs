using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class CirculationBaseDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.CirculationBaseDocumentInfo GetCirculationBaseDocumentInfo(IDataReader _reader)
        {
            return new CirculationBaseDocumentInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                CirculationDocumentID = (_reader["CirculationDocumentID"] == DBNull.Value) ? -1 : _reader["CirculationDocumentID"].ToString().ToNumber<int>(),
                BaseDocumentID = (_reader["BaseDocumentID"] == DBNull.Value) ? -1 : _reader["BaseDocumentID"].ToString().ToNumber<int>(),
                Name = _reader["Name"].ToString().ToNull(),
                Subject = _reader["Subject"].ToString().ToNull(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                Path = _reader["Path"].ToString().ToNull(),
                BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CirculationBaseDocumentInfo> GetCirculationBaseDocumentInfoList(IDataReader reader)
        {
            List<CirculationBaseDocumentInfo> list = new List<CirculationBaseDocumentInfo>();
            while (reader.Read())
                list.Add(GetCirculationBaseDocumentInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 근거문서 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirDocID">The _cir doc ID.</param>
        /// <returns></returns>
        public List<BE.CirculationBaseDocumentInfo> GetBaseDocLists(int _companyID, int _cirDocID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirBaseDoc_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CirculationDocumentID", SqlDbType.Int).Value = _cirDocID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCirculationBaseDocumentInfoList(reader);
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
        public int GetBaseDocCnt(int _companyID, int _cirDocID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CirBaseDoc_SelCnt");
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