using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class AccessGrade : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.AccessGradeInfo GetAccessGradeInfo(IDataReader _reader)
        {
            return new AccessGradeInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                AccessID = (_reader["AccessID"] == DBNull.Value) ? -1 : _reader["AccessID"].ToString().ToNumber<int>(),
                AccessGrade = (_reader["AccessGrade"] == DBNull.Value) ? -1 : _reader["AccessGrade"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                IsAccess = _reader["IsAccess"].ToString().ToNull(),
                AccessGradeUser = _reader["AccessGradeUser"].ToString().ToNull(),

                IsBase = (_reader["IsBase"] == DBNull.Value) ? "0" : _reader["IsBase"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<AccessGradeInfo> GetAccessGradeInfoList(IDataReader reader)
        {
            List<AccessGradeInfo> list = new List<AccessGradeInfo>();
            while (reader.Read())
                list.Add(GetAccessGradeInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-13
        /// 열람등급 갯수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <returns></returns>
        public int GetList(int _companyID, string _sKey, string _sVal)
        {
            int rtnCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_AccessGrade_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-13
        /// 열람등급 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_sPeriod"></param>
        /// <param name="_sPeriodVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _pageSize, int _pageNo, string _sKey, string _sVal, string _order)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DAL.AccessGrade[GetList]", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-13
        /// 작성목적 : 열람등급 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_proxyID"></param>
        /// <returns></returns>
        public DataTableReader GetDataByAccessID(int _companyID, int _accessID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_AccessGrade_SelByAccessID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-13
        /// 직급리스트 가져오기
        /// </summary>
        public DataTableReader GetTitleList(int _companyID, int _accessID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_SelByTitleAccessID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 작성자 : 이지원
        /// 목적 : 사용여부로 리스트 가져오기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public DataSet GetListIsAccess(int _companyID, string _isAccess)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_APP_AccessGrade_SelIsAccessList", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@IsAccess", SqlDbType.Char, 1).Value = _isAccess;

                cn.Open();

                return ExecuteDataSet(cmd, "GetAccessGrade");
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-12
        /// 작성목적 : 파일권한체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetChkList(int _companyID, string _tmpValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_SelCheck", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.VarChar, 4000).Value = _tmpValue;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return "0";
            }
        }
        #endregion GET()

        #region ADD()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 추가
        /// </summary>
        /// <returns></returns>
        public int Add(BE.AccessGradeInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 50).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 50).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 50).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 50).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 50).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 50).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsAccess", SqlDbType.Char, 1).Value = _info.IsAccess;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = _info.UserName;

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
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 수정
        /// </summary>
        /// <returns></returns>
        public bool Mod(BE.AccessGradeInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _info.AccessID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 50).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 50).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 50).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 50).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 50).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 50).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsAccess", SqlDbType.Char, 1).Value = _info.IsAccess;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100).Value = _info.UserName;

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

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_sequence"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _accessID, int _accessGrade)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_UpdOrder", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;
                    cmd.Parameters.Add("@AccessGrade", SqlDbType.Int).Value = _accessGrade;

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

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 활성화/비활성화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _accessID, string _isAccess)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_UpdIsActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;
                    cmd.Parameters.Add("@IsAccess", SqlDbType.Char, 1).Value = _isAccess;

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

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 작성목적 : 리스트에 직급 추가
        /// </summary>
        /// <returns></returns>
        public bool AddTitle(int _companyID, int _accessID, int _codeID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_UpdTitle", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;
                    cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _codeID;

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


        /// <summary>
        /// 기본값설정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_accessID"></param>
        /// <returns></returns>
        public bool UpdBase(int _companyID, int _accessID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_UpdBase", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;

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
        
        #endregion MOD()

        #region DEL()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 직급삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _accessID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _accessID;

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

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 작성목적 : 리스트에 직급 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelTitle(int _companyID, int _codeID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_AccessGrade_UpdDelTitle", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _codeID;

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
