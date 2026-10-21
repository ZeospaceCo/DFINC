using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class MailZetta65 : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 메일 사용자 존재여부
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_email">The _email.</param>
        /// <returns></returns>
        public bool ExitUser(int _companyID, string _email)
        {
            string query = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_ExistUser", "");
            if (string.IsNullOrEmpty(query) || query == "") return true;
            query = string.Format(query, _email);

            using (SqlConnection cn = new SqlConnection(ConnSolution(1)))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                if (ExecuteScalar(cmd).ToString().ToNumber<int>() > 0)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 그룹웨어에서 사용자 삭제시 제타메일은 퇴사자쪽으로 그룹 이동
        /// </summary>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet RetiredUser(int _userID)
        {
            try
            {
                string query = @"Select userID, mailID, mailDomain from Dp_Acc_User where userID= @userID";
                DataSet ds = new System.Data.DataSet();
                query = string.Format(query, _userID);
                using (SqlConnection cn = new SqlConnection(ConnSolution(1)))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 100).Value = _userID;

                    cn.Open();

                    if (ExecuteScalar(cmd).ToString().ToNumber<int>() > 0)
                    {
                        ds = ExecuteDataSet(cmd, "retired");
                        return ds;
                    }
                }
                return ds;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ex", ex);
                throw;
            }
        }

        /// <summary>
        /// 제타쪽에서 주소록 추가시 그룹웨어 개인주소록 카테고리에 저장
        /// </summary>
        /// <param name="_group_sn"></param>
        /// <param name="_userName"></param>
        /// <param name="_email"></param>
        /// <param name="_mobilePhone"></param>
        /// <param name="_user_sn"></param>
        /// <returns></returns>
        public bool InsertAddress(int _companyID, string _group_sn, string _userName, string _email, string _mobilePhone, int _user_sn)
        {
            try
            {
                //제타메일의 user_sn 값을 가지고 그룹웨어 userid 가져오기
                string userid = EipUserID(_user_sn);

                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    int uid = userid.ToNumber<int>();
                    //그룹웨어 userid를 가져왔다면 해당 카테고리에 insert 시키기
                    //Insert into DeskPlusEIP.dbo.DP_ADD_Private(CompanyID,UserID,CategoryID,LastName,WholeName,Mail,MobilePhone,ISReceive,SendUserID) Values (1,@UserID,@CategoryID,@LastName,@WholeName,@Mail,@MobilePhone,0,null)

                    string qry = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "ZettaAddressAdd", "");
                    qry = string.Format(qry, _companyID, uid, _group_sn.ToNumber<int>(), _userName, _userName, _userName, _userName, _userName, _userName, _userName, _userName, _userName, _email, _mobilePhone);

                    SqlCommand cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ZettaAddressAdd : " + qry, AppSettingType.MAIL);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("InsertAddress : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 제타 user_sn 가지고 그룹웨어 userid 값 가져오기
        /// </summary>
        /// <param name="_user_sn"></param>
        /// <returns></returns>
        public string EipUserID(int _user_sn)
        {
            try
            {
                string qry = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Gw_UserID", "");
                qry = string.Format(qry, _user_sn);
                string userid = "";

                using (SqlConnection cn = new SqlConnection(ConnSolution(1)))
                {
                    SqlCommand cmd = new SqlCommand(qry, cn);

                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    DataSet ds = new System.Data.DataSet();
                    ds = ExecuteDataSet(cmd, "userid");

                    userid = ds.Tables[0].Rows[0]["userid"].ToString();

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Gw_UserID : " + qry, AppSettingType.MAIL);
                    return userid;
                }
            }
            catch (System.Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EipUserID : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 제타 user_sn 가지고 그룹웨어 companyID 값 가져오기
        /// </summary>
        /// <param name="_user_sn"></param>
        /// <returns></returns>
        public string EipCompanyID(int _user_sn)
        {
            try
            {
                string qry = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Gw_UserID", "");
                qry = string.Format(qry, _user_sn);
                string companyID = "";

                using (SqlConnection cn = new SqlConnection(ConnSolution(1)))
                {
                    SqlCommand cmd = new SqlCommand(qry, cn);

                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    DataSet ds = new System.Data.DataSet();
                    ds = ExecuteDataSet(cmd, "userid");

                    companyID = ds.Tables[0].Rows[0]["CompanyID"].ToString();

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Gw_UserID : " + qry, AppSettingType.MAIL);
                    return companyID;
                }
            }
            catch (System.Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EipUserID : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : freeman
        /// 작성일자 : 2012-05-14
        /// 작성목적 : 제타메일 메일 최신 건수 가져오기 5건 가져오기
        /// 수정일자 :
        /// 입력값 변경해야 함
        /// --- zetta5.0 ---
        /// select top {0} message_type as state_code, mail_sn as user_mail_sn, left(mail_subject,11)+'..' as mail_subject, from_header as from_name, from_header as from_email, date_header as receive_time from zt_message where domain_idx=(select domain_idx from zt_domain where domain_name=@MailDomain) and member_sn=(select member_sn from zt_userinfo where domain_idx=(select domain_idx from zt_domain where domain_name=@MailDomain) and member_id=@MailID) and (mailbox_code='1100' or mailbox_code&gt;='1300') and delete_flag=0 order by date_header desc
        /// --- zetta6.5 ---
        /// SELECT TOP {0} CASE a.read_flag  WHEN '1' THEN '12' ELSE a.state_code  END as state_code, a.user_mail_sn, b.mail_subject as mail_subject, b.from_name, b.from_email, b.receive_time FROM zt_user_mailinfo a,zt_mail_message b WHERE a.mail_sn=b.mail_sn AND a.user_sn=(SELECT user_sn FROM zt_user_info WHERE user_account=@MailID + '@' + @MailDomain) AND a.state_code IN (10,11,12) AND a.trash_flag=0 AND a.mailbox_code='1100' ORDER BY a.create_time DESC, a.user_mail_sn DESC
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_listCount">The _list count.</param>
        /// <returns></returns>
        public DataTableReader GetMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SelZettaMail");
                    query = string.Format(query, _listCount);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar).Value = _mailID;
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar).Value = _mailDomain;

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
        /// 메일주소로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mail">The _mail.</param>
        /// <returns></returns>
        public DataTableReader GetUserInfo(int _companyID, string _mail)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(1)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Zetta65_UserInfo");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@user_account", SqlDbType.VarChar, 500).Value = _mail;

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
        /// 작성자 : 심재운
        /// 보낸편지함 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <param name="_mailbox_code">The _code.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserSendMailList(int _companyID, string _user_sn, string _mailbox_code, int _pageNo, int _pageSize)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(1)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SendList");
                    //string query = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_SendList", "");
                    query = String.Format(query, _user_sn, _mailbox_code, _pageNo, _pageSize);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

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
        /// 작성자 : 심재운
        /// 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxList(int _companyID, string _user_sn)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(1)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_MailBoxList");
                    //string query = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_MailBoxList", "");
                    query = String.Format(query, _user_sn);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

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
        /// 작성자 : 심재운
        /// 특정 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <param name="_mailboxcode">The _mailboxcode.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxListByCode(int _companyID, string _user_sn, string _mailboxcode, int _pageNo, int _pageSize)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(1)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_MailBoxListByCode");
                    //string query = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_MailBoxListByCode", "");
                    query = String.Format(query, _user_sn, _mailboxcode, _pageNo, _pageSize);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

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
        /// 승인 요청확인 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailConfirmList(int _companyID, string _mail, int _pageSize)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SelListConfirm");
                    query = String.Format(query, _pageSize);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Mail", SqlDbType.VarChar, 500).Value = _mail;

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
    }
}