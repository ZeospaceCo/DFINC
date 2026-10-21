using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class BaseDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.BaseDocumentInfo GetBaseDocumentInfo(IDataReader _reader)
        {
            BE.BaseDocumentInfo info = new BaseDocumentInfo();
            info.CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>();
            info.BaseDocumentID = (_reader["BaseDocumentID"] == DBNull.Value) ? -1 : _reader["BaseDocumentID"].ToString().ToNumber<int>();
            info.DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>();
            info.ParentDocumentID = (_reader["ParentDocumentID"] == DBNull.Value) ? -1 : _reader["ParentDocumentID"].ToString().ToNumber<int>();
            info.Name = _reader["Name"].ToString().ToNull();
            info.Subject = _reader["Subject"].ToString().ToNull().ToReverseXSS();
            info.IsLegacy = _reader["IsLegacy"].ToString().ToNull();
            info.Path = _reader["Path"].ToString().ToNull();
            info.BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>();
            return info;
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<BaseDocumentInfo> GetBaseDocumentInfoList(IDataReader reader)
        {
            List<BaseDocumentInfo> list = new List<BaseDocumentInfo>();
            while (reader.Read())
                list.Add(GetBaseDocumentInfo(reader));
            return list;
        }

        private BE.BaseDocumentInfo GetBaseDocumentInfoByPath(IDataReader _reader)
        {
            BE.BaseDocumentInfo info = new BaseDocumentInfo();
            info.CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>();
            info.BaseDocumentID = (_reader["BaseDocumentID"] == DBNull.Value) ? -1 : _reader["BaseDocumentID"].ToString().ToNumber<int>();
            info.DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>();
            info.ParentDocumentID = (_reader["ParentDocumentID"] == DBNull.Value) ? -1 : _reader["ParentDocumentID"].ToString().ToNumber<int>();
            info.Name = _reader["Name"].ToString().ToNull();
            info.Subject = _reader["Subject"].ToString().ToNull();
            info.IsLegacy = _reader["IsLegacy"].ToString().ToNull();
            info.Path = _reader["Path"].ToString().ToNull();
            info.BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>();
            if (_reader["ApprovalState"] != null)
            {
                info.ApprovalState = _reader["ApprovalState"].ToString().ToNull();
            }
            else
            {
                info.ApprovalState = "C";       // 수정인경우 'C'인 경우, 삭제불가
            }

            #region 근거문서 정보 (원 문서)

            info.DocNumber = _reader["DocNumber"].ToString().ToNull();
            info.Subject = _reader["Subject"].ToString().ToNull();

            info.NameBase = _reader["NameBase"].ToString().ToNull();
            info.NameKo = _reader["NameKo"].ToString().ToNull();
            info.NameEn = _reader["NameEn"].ToString().ToNull();
            info.NameJa = _reader["NameJa"].ToString().ToNull();
            info.NameFr = _reader["NameFr"].ToString().ToNull();
            info.NameZhcn = _reader["NameZhcn"].ToString().ToNull();
            info.NameZhtw = _reader["NameZhtw"].ToString().ToNull();
            info.FormBase = _reader["FormBase"].ToString().ToNull();
            info.FormKo = _reader["FormKo"].ToString().ToNull();
            info.FormEn = _reader["FormEn"].ToString().ToNull();
            info.FormJa = _reader["FormJa"].ToString().ToNull();
            info.FormFr = _reader["FormFr"].ToString().ToNull();
            info.FormZhcn = _reader["FormZhcn"].ToString().ToNull();
            info.FormZhtw = _reader["FormZhtw"].ToString().ToNull();
            info.DeptBase = _reader["DeptBase"].ToString().ToNull();
            info.DeptKo = _reader["DeptKo"].ToString().ToNull();
            info.DeptEn = _reader["DeptEn"].ToString().ToNull();
            info.DeptJa = _reader["DeptJa"].ToString().ToNull();
            info.DeptFr = _reader["DeptFr"].ToString().ToNull();
            info.DeptZhcn = _reader["DeptZhcn"].ToString().ToNull();
            info.DeptZhtw = _reader["DeptZhtw"].ToString().ToNull();
            info.TitleBase = _reader["TitleBase"].ToString().ToNull();
            info.TitleKo = _reader["TitleKo"].ToString().ToNull();
            info.TitleEn = _reader["TitleEn"].ToString().ToNull();
            info.TitleJa = _reader["TitleJa"].ToString().ToNull();
            info.TitleFr = _reader["TitleFr"].ToString().ToNull();
            info.TitleZhcn = _reader["TitleZhcn"].ToString().ToNull();
            info.TitleZhtw = _reader["TitleZhtw"].ToString().ToNull();
            info.DutyBase = _reader["DutyBase"].ToString().ToNull();
            info.DutyKo = _reader["DutyKo"].ToString().ToNull();
            info.DutyEn = _reader["DutyEn"].ToString().ToNull();
            info.DutyJa = _reader["DutyJa"].ToString().ToNull();
            info.DutyFr = _reader["DutyFr"].ToString().ToNull();
            info.DutyZhcn = _reader["DutyZhcn"].ToString().ToNull();
            info.DutyZhtw = _reader["DutyZhtw"].ToString().ToNull();

            #endregion 근거문서 정보 (원 문서)

            return info;
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<BaseDocumentInfo> GetBaseDocumentInfoListByPath(IDataReader reader)
        {
            List<BaseDocumentInfo> list = new List<BaseDocumentInfo>();
            while (reader.Read())
                list.Add(GetBaseDocumentInfoByPath(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 근거문서 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public List<BE.BaseDocumentInfo> GetBaseDocLists(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_BaseDoc_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetBaseDocumentInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 근거문서 카운트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public int GetBaseDocCnt(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_BaseDoc_SelCnt");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cn.Open();

                return cmd.ExecuteScalar().ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// UpPath로 근거문서리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public List<BE.BaseDocumentInfo> GetBaseDocListsByPath(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_BaseDoc_SelDocID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetBaseDocumentInfoListByPath(reader);
                }
            }
        }

        /// <summary>
        /// UpPath로 근거문서리스트 가져오기(건수)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_isComplete"></param>
        /// <returns></returns>
        public int GetBaseDocCntByPath(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_BaseDoc_SelDocIDCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cn.Open();

                return cmd.ExecuteScalar().ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-26
        /// UpPath로 근거문서리스트 가져오기 - 폐기문서함
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.BaseDocumentInfo> GetDisuseBaseDocListsByPath(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_BaseDoc_SelDisuse", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetBaseDocumentInfoListByPath(reader);
                    }
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
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-26
        /// UpPath로 근거문서리스트 가져오기(건수) - 폐기문서함
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public int GetDisuseBaseDocCntByPath(int _companyID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_BaseDoc_SelDisuseCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cn.Open();

                    return cmd.ExecuteScalar().ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BaseDocumentInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_BaseDoc_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@ParentDocumentID", SqlDbType.Int).Value = _info.ParentDocumentID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _info.Name.ToNull();
                    cmd.Parameters.Add("@IsLegacy", SqlDbType.Char, 1).Value = _info.IsLegacy.ToNull();
                    cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 500).Value = _info.Path.ToNull();
                    cmd.Parameters.Add("@BaseOrder", SqlDbType.Int).Value = _info.BaseOrder;

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

        #endregion ADD()

        #region MOD()

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 근거문서삭제 - ID 별 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_baseDocumentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID, int _baseDocumentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_BaseDoc_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                    cmd.Parameters.Add("@BaseDocumentID", SqlDbType.Int).Value = _baseDocumentID;

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

        #endregion DEL()
    }
}