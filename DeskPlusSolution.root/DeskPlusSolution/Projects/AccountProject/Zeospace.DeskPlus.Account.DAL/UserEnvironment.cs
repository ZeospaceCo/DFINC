using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserEnvironment : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@PreferredLanguage", SqlDbType.VarChar,10).Value = _info.PreferredLanguage.ToNull().ToXSS();
        cmd.Parameters.Add("@DesignStyle", SqlDbType.VarChar,20).Value = _info.DesignStyle.ToNull().ToXSS();
        cmd.Parameters.Add("@BaseMenu", SqlDbType.NVarChar,500).Value = _info.BaseMenu.ToNull().ToXSS();
        cmd.Parameters.Add("@ApprovalStatusMenu", SqlDbType.NVarChar,50).Value = _info.ApprovalStatusMenu.ToNull().ToXSS();
        cmd.Parameters.Add("@ApprovalPublicMenu", SqlDbType.NVarChar,50).Value = _info.ApprovalPublicMenu.ToNull().ToXSS();
        cmd.Parameters.Add("@BaseFuncMenu", SqlDbType.NVarChar,500).Value = _info.BaseFuncMenu.ToNull().ToXSS();
        cmd.Parameters.Add("@ListCnt", SqlDbType.NVarChar,500).Value = _info.ListCnt.ToNull().ToXSS();
        cmd.Parameters.Add("@MobilePortalDesign", SqlDbType.VarChar,50).Value = _info.MobilePortalDesign.ToNull().ToXSS();
        cmd.Parameters.Add("@MobileRemember", SqlDbType.VarChar,1).Value = _info.MobileRemember.ToNull().ToXSS();
        cmd.Parameters.Add("@MobileActionMenu", SqlDbType.VarChar,100).Value = _info.MobileActionMenu.ToNull().ToXSS();
        cmd.Parameters.Add("@MobilePims", SqlDbType.VarChar,100).Value = _info.MobilePims.ToNull().ToXSS();

        */

        #region DataReader

        private UserEnvironmentInfo GetUserEnvironment(IDataReader _reader)
        {
            return new UserEnvironmentInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                PreferredLanguage = _reader["PreferredLanguage"].ToString().ToNull(),
                DesignStyle = _reader["DesignStyle"].ToString().ToNull(),
                BaseMenu = _reader["BaseMenu"].ToString().ToNull(),
                ApprovalStatusMenu = _reader["ApprovalStatusMenu"].ToString().ToNull(),
                ApprovalPublicMenu = _reader["ApprovalPublicMenu"].ToString().ToNull(),
                BaseFuncMenu = _reader["BaseFuncMenu"].ToString().ToNull(),
                ListCnt = _reader["ListCnt"].ToString().ToNull(),
                BaseTab = (_reader["BaseTab"] == DBNull.Value) ? -1 : _reader["BaseTab"].ToString().ToNumber<int>(),
                MobilePims = _reader["MobilePims"].ToString().ToNull(),
                AlarmMail = _reader["AlarmMail"].ToString().ToNull(),
                AlarmMessenger = _reader["AlarmMessenger"].ToString().ToNull(),
                AlarmSMS = _reader["AlarmSMS"].ToString().ToNull(),
                ApprovalHomeMenu = _reader["ApprovalHomeMenu"].ToString().ToNull(),
                TaskHomeMenu = _reader["TaskHomeMenu"].ToString().ToNull(),
                RMHomeMenu = _reader["RMHomeMenu"].ToString().ToNull(),
                IsApprovePwd = (_reader["IsApprovePwd"].ToString().ToNull().Equals("")) ? "N" : _reader["IsApprovePwd"].ToString().ToNull(),
                ApprovalStatusMenuType = GetApprovalStatusMenuType(GetReaderValue(_reader, "ApprovalStatusMenuType")),
                IsTodo = (_reader["IsTodo"].ToString().ToNull().Equals("")) ? "N" : _reader["IsTodo"].ToString().ToNull(),
                TodoMenu = _reader["TodoMenu"].ToString().ToNull(),

                AlarmApproval = (_reader["AlarmApproval"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmApproval"].ToString().ToNull(),
                AlarmBoard = (_reader["AlarmBoard"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmBoard"].ToString().ToNull(),
                AlarmPims = (_reader["AlarmPims"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmPims"].ToString().ToNull(),
                AlarmTask = (_reader["AlarmTask"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmTask"].ToString().ToNull(),
                AlarmWork = (_reader["AlarmWork"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmWork"].ToString().ToNull(),
                AlarmCommunity = (_reader["AlarmCommunity"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmCommunity"].ToString().ToNull(),
                AlarmRM = (_reader["AlarmRM"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmRM"].ToString().ToNull(),
                AlarmSNS = (_reader["AlarmSNS"].ToString().ToNull().Equals("")) ? "0-" : _reader["AlarmSNS"].ToString().ToNull(),

                ImageYN = (_reader["ImageYN"].ToString().ToNull().Equals("")) ? "N" : _reader["ImageYN"].ToString().ToNull(),
            };
        }

        private string GetReaderValue(IDataReader _reader, string _columnName)
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                if (_reader.GetName(i).Equals(_columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return (_reader[i] == DBNull.Value) ? "" : _reader[i].ToString();
                }
            }

            return "";
        }

        private string GetApprovalStatusMenuType(string _approvalStatusMenuType)
        {
            string menuType = _approvalStatusMenuType.ToNull();
            return (menuType.Equals("NEW") || menuType.Equals("N")) ? "NEW" : "OLD";
        }

        protected List<UserEnvironmentInfo> GetUserEnvironmentList(IDataReader reader)
        {
            List<UserEnvironmentInfo> list = new List<UserEnvironmentInfo>();
            while (reader.Read())
                list.Add(GetUserEnvironment(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 개인별 시작탭 설정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabID"></param>
        /// <returns></returns>
        public bool ModBaseTab(int _companyID, int _userID, int _tabID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdBaseTab", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@BaseTab", SqlDbType.Int).Value = _tabID;

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
        /// 개인별 결재 알람 설정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabID"></param>
        /// <returns></returns>
        public bool ModMyinfo(BE.UserEnvironmentInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdMyInfo", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    //cmd.Parameters.Add("@AlarmMail", SqlDbType.Int).Value = _info.AlarmMail;
                    //cmd.Parameters.Add("@AlarmMessenger", SqlDbType.Int).Value = _info.AlarmMessenger;
                    //cmd.Parameters.Add("@AlarmSMS", SqlDbType.Int).Value = _info.AlarmSMS;
                    cmd.Parameters.Add("@IsTodo", SqlDbType.Char, 1).Value = (_info.IsTodo.Equals("")) ? "N" : _info.IsTodo;

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
        /// 작성자 : 이문호
        /// 용도 : 결재상황조회 초기메뉴 설정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ApprovalStatusMenu(int _companyID, int _userID, string _folderStatusID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdAppStatusMenu", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@FolderStatusID", SqlDbType.VarChar, 20).Value = _folderStatusID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        // 문서함 초기메뉴 설정
        public bool ApprovalPublicMenu(int _companyID, int _userID, string _cabinetID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdAppPublicMenu", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@CabinetID", SqlDbType.VarChar, 20).Value = _cabinetID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        // 전자결재 - 홈화면 초기메뉴 설정
        public bool ApprovalHomeMenu(int _companyID, int _userID, string _tagID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdAppHomeMenu", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@TabID", SqlDbType.VarChar, 20).Value = _tagID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        // 자원관리 - 홈화면 초기메뉴 설정
        public bool RMHomeMenu(int _companyID, int _userID, string _tagID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdRMHomeMenu", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@TabID", SqlDbType.VarChar, 20).Value = _tagID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        // 업무협업 - 홈화면 초기메뉴 설정
        public bool TaskHomeMenu(int _companyID, int _userID, string _tagID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdTaskHomeMenu", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@TabID", SqlDbType.VarChar, 20).Value = _tagID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 일정관리 사용자 최종 선택 탭 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabType"></param>
        public bool ModPimTabMenu(int _companyID, int _userID, string _tabType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdPimTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = _tabType;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }

        /// <summary>
        /// 자원관리 사용자 최종 선택 탭 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabType"></param>
        public bool ModRMTabMenu(int _companyID, int _userID, string _tabType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdRMTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = _tabType;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }

        /// <summary>
        /// 자원관리 사용자 최종 선택Type 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabType"></param>
        public bool ModRMTypeMenu(int _companyID, int _userID, string _type)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdRMTypeMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = _type;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }


        /// <summary>
        /// 방문예약 사용자 최종 선택 탭 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_tabType"></param>
        public bool ModVisitTabMenu(int _companyID, int _userID, string _tabType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdVisitTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = _tabType;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }

        /// <summary>
        /// 전자결재 : 승인 시, 비밀번호 사용유무 설정하기 (N/Y)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_isApprovePwd"></param>
        /// <returns></returns>
        public bool ModIsApprovePwd(int _companyID, int _userID, string _isApprovePwd)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdApprovePwd", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@IsApprovePwd", SqlDbType.Char, 1).Value = _isApprovePwd;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }

        /// <summary>
        /// 전자결재 : 결재함 메뉴 유형 설정하기 (OLD=구메뉴, NEW=신메뉴)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_approvalStatusMenuType"></param>
        /// <returns></returns>
        public bool ModApprovalStatusMenuType(int _companyID, int _userID, string _approvalStatusMenuType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdApprovalStatusMenuType", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ApprovalStatusMenuType", SqlDbType.VarChar, 3).Value = GetApprovalStatusMenuType(_approvalStatusMenuType);

                cn.Open();
                ExecuteNonQuery(cmd);
            }

            return true;
        }

        /// <summary>
        /// 일정관리 사용자 최종 체크한 일정분류 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_checkState"></param>
        public bool ModPimCheckMenu(int _companyID, int _userID, string _checkState)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdPimCheckMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@CheckState", SqlDbType.VarChar, 100).Value = _checkState;

                cn.Open();
                ExecuteNonQuery(cmd);

            }
            return true;
        }

        public bool ModTodoMenu(int _companyID, int _userID, string _todoMenu)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_UpdTodoMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@TodoMenu", SqlDbType.NVarChar, 100).Value = _todoMenu;

                cn.Open();
                ExecuteNonQuery(cmd);
            }
            return true;
        }

        /// <summary>
        /// 개인별 결재 알람 설정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAlarm(BE.UserEnvironmentInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdAlarm", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@AlarmApproval", SqlDbType.NVarChar, 50).Value = (_info.AlarmApproval.ToNull().Equals("")) ? "0-" : _info.AlarmApproval;
                    cmd.Parameters.Add("@AlarmBoard", SqlDbType.NVarChar, 50).Value = (_info.AlarmBoard.ToNull().Equals("")) ? "0-" : _info.AlarmBoard;
                    cmd.Parameters.Add("@AlarmPims", SqlDbType.NVarChar, 50).Value = (_info.AlarmPims.ToNull().Equals("")) ? "0-" : _info.AlarmPims;
                    cmd.Parameters.Add("@AlarmTask", SqlDbType.NVarChar, 50).Value = (_info.AlarmTask.ToNull().Equals("")) ? "0-" : _info.AlarmTask;
                    cmd.Parameters.Add("@AlarmWork", SqlDbType.NVarChar, 50).Value = (_info.AlarmWork.ToNull().Equals("")) ? "0-" : _info.AlarmWork;
                    cmd.Parameters.Add("@AlarmCommunity", SqlDbType.NVarChar, 50).Value = (_info.AlarmCommunity.ToNull().Equals("")) ? "0-" : _info.AlarmCommunity;
                    cmd.Parameters.Add("@AlarmRM", SqlDbType.NVarChar, 50).Value = (_info.AlarmRM.ToNull().Equals("")) ? "0-" : _info.AlarmRM;
                    cmd.Parameters.Add("@AlarmSNS", SqlDbType.NVarChar, 50).Value = (_info.AlarmSNS.ToNull().Equals("")) ? "0-" : _info.AlarmSNS;

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
        /// 이미지 영역 감추기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_imageYN"></param>
        /// <returns></returns>
        public bool ModImage(int _companyID, int _userID, string _imageYN)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEnv_UpdImage", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ImageYN", SqlDbType.Char, 1).Value = _imageYN;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }
        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// USERID 로 사용자 환경 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<UserEnvironmentInfo> GetUserEnv(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserEnv_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUserEnvironmentList(reader);
                }
            }
        }

        /// <summary>
        /// 일정관리 사용자 일간,주간,월간 중 최종으로 선택한 탭 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetPimTabMenu(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_SelPimTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 자원관리 사용자 일간,주간,월간 중 최종으로 선택한 탭 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetRMTabMenu(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_SelRMTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 자원관리 Type(list,blog) 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetRMTypeMenu(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_SelRMTypeMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 방문예약 사용자 일간,주간,월간 중 최종으로 선택한 탭 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetVisitTabMenu(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_SelVisitTabMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 일정관리 사용자 최종 체크한 일정분류 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetPimCheckMenu(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserEnv_SelPimCheckMenu", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 가져오기
    }
}
