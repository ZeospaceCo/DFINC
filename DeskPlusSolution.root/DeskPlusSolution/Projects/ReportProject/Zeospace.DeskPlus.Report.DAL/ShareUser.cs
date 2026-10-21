using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Report.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.DAL
{
    public class ShareUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ShareUserInfo GetShareUser(IDataReader _reader)
        {
            return new ShareUserInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),                
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                TargetUserID = (_reader["TargetUserID"] == DBNull.Value) ? -1 : _reader["TargetUserID"].ToString().ToNumber<int>(),
                UseComplete = _reader["UseComplete"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],

                #region #### Target  부서,직급,직책 다국어 시작 #######
                TargetNameBase = _reader["NameBase"].ToString().ToNull(),
                TargetNameKo = _reader["NameKo"].ToString().ToNull(),
                TargetNameEn = _reader["NameEn"].ToString().ToNull(),
                TargetNameJa = _reader["NameJa"].ToString().ToNull(),
                TargetNameFr = _reader["NameFr"].ToString().ToNull(),
                TargetNameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                TargetNameZhtw = _reader["NameZhtw"].ToString().ToNull(),

                TargetDeptBase = _reader["DeptBase"].ToString().ToNull(),
                TargetDeptKo = _reader["DeptKo"].ToString().ToNull(),
                TargetDeptEn = _reader["DeptEn"].ToString().ToNull(),
                TargetDeptJa = _reader["DeptJa"].ToString().ToNull(),
                TargetDeptFr = _reader["DeptFr"].ToString().ToNull(),
                TargetDeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                TargetDeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TargetTitleBase = _reader["TitleBase"].ToString().ToNull(),
                TargetTitleKo = _reader["TitleKo"].ToString().ToNull(),
                TargetTitleEn = _reader["TitleEn"].ToString().ToNull(),
                TargetTitleJa = _reader["TitleJa"].ToString().ToNull(),
                TargetTitleFr = _reader["TitleFr"].ToString().ToNull(),
                TargetTitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TargetTitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                TargetDutyBase = _reader["DutyBase"].ToString().ToNull(),
                TargetDutyKo = _reader["DutyKo"].ToString().ToNull(),
                TargetDutyEn = _reader["DutyEn"].ToString().ToNull(),
                TargetDutyJa = _reader["DutyJa"].ToString().ToNull(),
                TargetDutyFr = _reader["DutyFr"].ToString().ToNull(),
                TargetDutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                TargetDutyZhtw = _reader["DutyZhtw"].ToString().ToNull()
                #endregion
            };
        }

        protected List<ShareUserInfo> GetShareUserList(IDataReader reader)
        {
            List<ShareUserInfo> list = new List<ShareUserInfo>();
            while (reader.Read())
                list.Add(GetShareUser(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 보고대상자, 공유자 로 지정한 사용자 아이디 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_useComplete">The _use complete.</param>
        /// <returns></returns>
        public DataSet GetShareUserListTargetUserID(int _companyID, int _userID, string _useComplete)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_ShareUser_SelByTargetUserID");            
            
            if (_useComplete == "")
            {
                //보고자,대상자 전부 가져오기
                query = string.Format(query, "");
            }
            else
            {
                //공유자 또는 보고대상자(1:보고대상자;0:공유자)
                query = string.Format(query, " AND UseComplete =" + _useComplete.ToInject());
            }

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@TargetUserID", SqlDbType.Int).Value = _userID;//나를 공유한 사용자 정보가져오기
                cn.Open();

                return ExecuteDataSet(cmd, "GetShareUserListTargetUserID");
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 비공개 작업일지의 관리자, 공유자 권한 여부
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_targetUserID">The _target user ID.</param>
        /// <param name="_useComplete">if set to <c>true</c> [_use complete].</param>
        /// <returns></returns>
        public int GetShareUserCntTargetUserID(int _companyID, int _userID, int _targetUserID, string _useComplete)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_ShareUser_SelCountByUserID");

            if (_useComplete == "")
            {
                // 관리자, 공유자
                query = string.Format(query, "");
            }
            else
            {
                query = string.Format(query, " AND UseComplete = " + _useComplete.ToInject());
            }

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@TargetUserID", SqlDbType.Int).Value = _targetUserID;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 나의 공유 테이블 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetShareUserList(int _companyID, int _userID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_ShareUser_SelByUserID");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

            
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 나의 보고받는 사람 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetShareUserTargerUserInfo(int _companyID, int _userID)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_ShareUser_SelByUseComplete");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }


        }

        #endregion

        #region ADD()


        /// <summary>
        /// 추가하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_targetUserID">The _target user ID.</param>
        /// <param name="_useComplete">The _use complete.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, int _targetUserID, string _useComplete)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_ShareUser_Ins", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@TargetUserID", SqlDbType.Int).Value = _targetUserID;
                cmd.Parameters.Add("@UseComplete", SqlDbType.Char, 1).Value = _useComplete;

                cn.Open();

                ExecuteNonQuery(cmd);

                return true;
            }
        }

        #endregion

        #region MOD()
        #endregion

        #region DEL()


        /// <summary>
        /// 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_REPORT_ShareUser_Del", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                ExecuteNonQuery(cmd);

                return true;
            }
        }

        #endregion

    }
}
