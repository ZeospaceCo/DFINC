using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class CounterPartUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.CounterPartUserInfo GetCounterPartUserInfo(IDataReader _reader)
        {
            return new CounterPartUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                Sequence = (_reader["Sequence"] == DBNull.Value) ? -1 : _reader["Sequence"].ToString().ToNumber<int>(),
                ApprovalTag = _reader["ApprovalTag"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                MyTurn = _reader["MyTurn"].ToString().ToNull(),
                ApprovedDocumentID = (_reader["ApprovedDocumentID"] == DBNull.Value) ? -1 : _reader["ApprovedDocumentID"].ToString().ToNumber<int>(),
                ApprovalDate = (_reader["ApprovalDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ApprovalDate"],
                ApprovalDateYMD = _reader["ApprovalDateYMD"].ToString().ToNull(),
                ApprovalType = _reader["ApprovalType"].ToString().ToNull(),
                IsComplete = _reader["IsComplete"].ToString().ToNull(),
                Viewing = _reader["Viewing"].ToString().ToNull(),
                ViewingUserID = (_reader["ViewingUserID"] == DBNull.Value) ? -1 : _reader["ViewingUserID"].ToString().ToNumber<int>(),
                ViewingTime = (_reader["ViewingTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ViewingTime"],
                ViewUserID = (_reader["ViewUserID"] == DBNull.Value) ? -1 : _reader["ViewUserID"].ToString().ToNumber<int>(),
                ViewNameBase = _reader["ViewNameBase"].ToString().ToNull(),
                ViewNameKo = _reader["ViewNameKo"].ToString().ToNull(),
                ViewNameEn = _reader["ViewNameEn"].ToString().ToNull(),
                ViewNameJa = _reader["ViewNameJa"].ToString().ToNull(),
                ViewNameFr = _reader["ViewNameFr"].ToString().ToNull(),
                ViewNameZhcn = _reader["ViewNameZhcn"].ToString().ToNull(),
                ViewNameZhtw = _reader["ViewNameZhtw"].ToString().ToNull(),
                ViewManyName = _reader["ViewManyName"].ToString().ToNull(),
                ViewDeptCodeID = (_reader["ViewDeptCodeID"] == DBNull.Value) ? -1 : _reader["ViewDeptCodeID"].ToString().ToNumber<int>(),
                ViewDate = (_reader["ViewDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ViewDate"],
                Opinion = _reader["Opinion"].ToString().ToNull(),
                Proxy = _reader["Proxy"].ToString().ToNull(),
                ProxyUserID = (_reader["ProxyUserID"] == DBNull.Value) ? -1 : _reader["ProxyUserID"].ToString().ToNumber<int>(),
                ProxyNameBase = _reader["ProxyNameBase"].ToString().ToNull(),
                ProxyNameKo = _reader["ProxyNameKo"].ToString().ToNull(),
                ProxyNameEn = _reader["ProxyNameEn"].ToString().ToNull(),
                ProxyNameJa = _reader["ProxyNameJa"].ToString().ToNull(),
                ProxyNameFr = _reader["ProxyNameFr"].ToString().ToNull(),
                ProxyNameZhcn = _reader["ProxyNameZhcn"].ToString().ToNull(),
                ProxyNameZhtw = _reader["ProxyNameZhtw"].ToString().ToNull(),
                ProxyManyName = _reader["ProxyManyName"].ToString().ToNull(),
                ProxyDeptCodeID = (_reader["ProxyDeptCodeID"] == DBNull.Value) ? -1 : _reader["ProxyDeptCodeID"].ToString().ToNumber<int>(),

                #region ####  부서,직급,직책 다국어 시작 #######

                //DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleID = (_reader["TitleID"] == DBNull.Value) ? -1 : _reader["TitleID"].ToString().ToNumber<int>(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyID = (_reader["DutyID"] == DBNull.Value) ? -1 : _reader["DutyID"].ToString().ToNumber<int>(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull()

                #endregion ####  부서,직급,직책 다국어 시작 #######
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CounterPartUserInfo> GetCounterPartUserInfoList(IDataReader reader)
        {
            List<CounterPartUserInfo> list = new List<CounterPartUserInfo>();
            while (reader.Read())
                list.Add(GetCounterPartUserInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        /// <summary>
        /// 문서정보 가져오기 (특정문서의 전체정보)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<CounterPartUserInfo> GetCounterPartUsers(int _companyID, int _documentID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CounterPartUser_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCounterPartUserInfoList(reader);
                }
            }
        }

        /// <summary>
        /// 수신문서 상신했는 지 체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public DataTableReader GetCounterPartUsers(int _companyID, int _documentID, string _approvalTag)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.APPROVAL, "QRY_APP_CounterPartUser_SelByApprovalTag");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.NVarChar, 20).Value = _approvalTag;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 현재 결재여부 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public string GetIsMyTurn(int _companyID, int _documentID, int _sequence, string _approvalTag, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_CounterPartUser_SelMyTurn", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _sequence;
                cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _approvalTag;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteScalar(cmd).ToString();
            }
        }

        ///// <summary>
        ///// 최종문서ID가져오기
        ///// </summary>
        ///// <param name="_info"></param>
        ///// <returns></returns>
        //public int GetChildDocID(int _companyID, int _documentID, string _isComplete)
        //{
        //    try
        //    {
        //        using (var cn = new SqlConnection(ConnSolution(_companyID)))
        //        {
        //            var cmd = new SqlCommand("DP_APP_CounterPartUser_SelChildDocID", cn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

        //            #region 파라미터 처리
        //            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
        //            cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
        //            cmd.Parameters.Add("@IsComplete", SqlDbType.Char, 1).Value = _isComplete.ToNull();
        //            #endregion

        //            cn.Open();
        //            return ExecuteScalar(cmd).ToString().ToNumber<int>();
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
        //        Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
        //        return _documentID;
        //    }
        //}

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.CounterPartUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.NVarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@MyTurn", SqlDbType.Char, 1).Value = (_info.MyTurn.ToNull().Equals("")) ? "N" : _info.MyTurn.ToNull();

                    #endregion 파라미터 처리

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

        /// <summary>
        /// 다음 수신부서 처리 (다음 수신자정보 리턴 : ApprovalTag-UserID-DeptCodeID)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public string ModNextZQ(int _companyID, int _groupID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_UpdDecideZQ", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;

                    #endregion 파라미터 처리

                    cn.Open();

                    return ExecuteScalar(cmd).ToString();
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
        /// 수신문서 상신(결재)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModDecide(CounterPartUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_UpdDecide", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@ApprovedDocumentID", SqlDbType.Int).Value = _info.ApprovedDocumentID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ApprovalType", SqlDbType.Char, 1).Value = _info.ApprovalType.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 1000).Value = _info.Opinion.ToNull();
                    cmd.Parameters.Add("@Proxy", SqlDbType.Char, 1).Value = _info.Proxy.ToNull();
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;

                    #endregion 파라미터 처리

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
        /// 수신완료 (ZP만 가능)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModComplete(CounterPartUserInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_UpdComplete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ApprovalType", SqlDbType.Char, 1).Value = _info.ApprovalType.ToNull();
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 1000).Value = _info.Opinion.ToNull();
                    cmd.Parameters.Add("@Proxy", SqlDbType.Char, 1).Value = _info.Proxy.ToNull();
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;

                    #endregion 파라미터 처리

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
        /// 담당자변경
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModReplace(CounterPartUserInfo _info, string _contentHTML, string _contentTEXT)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_UpdReplace", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _contentHTML.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentTEXT", SqlDbType.NVarChar, 4000).Value = _contentTEXT.ToNull();

                    #endregion 파라미터 처리

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
        /// 수신회수
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModCancel(CounterPartUserInfo _info, string _contentHTML)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_CounterPartUser_UpdCancel", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    #region 파라미터 처리

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = _info.Sequence;
                    cmd.Parameters.Add("@ApprovalTag", SqlDbType.VarChar, 20).Value = _info.ApprovalTag.ToNull();
                    cmd.Parameters.Add("@ContentHTML", SqlDbType.NText).Value = _contentHTML.ToNull().ToEditorXSS();

                    #endregion 파라미터 처리

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

        #endregion DEL()
    }
}