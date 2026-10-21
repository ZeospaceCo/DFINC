using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class DraftBaseDoc : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.DraftBaseDocInfo GetDraftBaseDocInfo(IDataReader _reader)
        {
            return new DraftBaseDocInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                BaseID = (_reader["BaseID"] == DBNull.Value) ? -1 : _reader["BaseID"].ToString().ToNumber<int>(),
                DraftID = (_reader["DraftID"] == DBNull.Value) ? -1 : _reader["DraftID"].ToString().ToNumber<int>(),
                ParentDocumentID = (_reader["ParentDocumentID"] == DBNull.Value) ? -1 : _reader["ParentDocumentID"].ToString().ToNumber<int>(),
                Name = _reader["Name"].ToString().ToNull(),
                Subject = _reader["Subject"].ToString().ToNull().ToReverseXSS(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                Path = _reader["Path"].ToString().ToNull(),
                BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>(),
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<DraftBaseDocInfo> GetDraftInfoList(IDataReader reader)
        {
            List<DraftBaseDocInfo> list = new List<DraftBaseDocInfo>();
            while (reader.Read())
                list.Add(GetDraftBaseDocInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 근거문서리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.DraftBaseDocInfo> GetBaseDocLists(int _companyID, int _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_DraftBaseDoc_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDraftInfoList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 임시저장 - 근거문서추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.DraftBaseDocInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_DraftBaseDoc_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;
                    cmd.Parameters.Add("@ParentDocumentID", SqlDbType.Int).Value = _info.ParentDocumentID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _info.Name.ToNull();
                    cmd.Parameters.Add("@IsLegacy", SqlDbType.Char, 1).Value = _info.IsLegacy.ToNull();
                    cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 500).Value = _info.Path.ToNull();
                    cmd.Parameters.Add("@BaseOrder", SqlDbType.Int).Value = _info.BaseOrder;

                    #endregion 파라미터 처리

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

        #endregion ADD()

        #region MOD()

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _baseID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_DraftBaseDoc_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@BaseID", SqlDbType.Int).Value = _baseID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()
    }
}