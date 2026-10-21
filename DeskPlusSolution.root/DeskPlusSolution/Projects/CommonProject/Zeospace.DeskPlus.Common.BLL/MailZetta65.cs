using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.BLL
{
    /// <summary>
    /// 작성자 : 심재운
    /// </summary>
    public class MailZetta65
    {
        private string MailServerIP = "";
        private string Zetta65_AddUser = "";
        private string Zetta65_DelUser = "";
        private string Zetta65_ModPasswd = "";
        private string Zetta65_ModDeptUser = "";
        private string Zetta65_RetiredUser = "";
        private string Zetta65_SuspendUser = "";
        private string Zetta65_ApprovalUser = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepBiz"/> class.
        /// </summary>
        public MailZetta65()
        {
            MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://zetta.zeospace.com:4040");
            Zetta65_AddUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_AddUser", "");
            Zetta65_ModPasswd = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_ModPasswd", "");
            Zetta65_ModDeptUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_ModDeptUser", "");
            Zetta65_DelUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_DelUser", "");
            Zetta65_RetiredUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_RetiredUser", "");
            Zetta65_SuspendUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_Suspend_user", "");
            Zetta65_ApprovalUser = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_Approval_user", "");
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : zetta 6 메일 사용자 추가(심재운 (2010-10-01)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_email">The _email.</param>
        /// <returns></returns>
        public string AddUser(int _companyID, string _mailID, string _pw, string _name, string _email, int _isencrypt)
        {
            try
            {
                DAL.MailZetta65 dal = new DAL.MailZetta65();
                bool existUser = dal.ExitUser(_companyID, _email);

                //메일 사용자가 존재하지 않으면 추가한다.
                if (!existUser)
                {
                    //zmail/api/make_user.nvd?user_account={0}&amp;pass={1}&amp;email={2}&amp;name={3}&amp;isencrypt={4}
                    Zetta65_AddUser = string.Format(Zetta65_AddUser, _mailID, _pw, _email, _name, _isencrypt);

                    string sUrl = MailServerIP + Zetta65_AddUser;
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("AddUser url : " + sUrl, AppSettingType.MAIL);

                    //제타 사용자 생성
                    //여기서 제타 API 호출 SendPost
                    string response = Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("AddUser response : " + response, AppSettingType.MAIL);

                    return response;
                }

                return "";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddUser : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 계정 삭제(심재운 (2010-10-01)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mail">The _mail.</param>
        /// <returns></returns>
        public string DelUser(string _mail)
        {
            try
            {
                //zmail/api/delete_user.nvd?user_account={0}
                Zetta65_DelUser = string.Format(Zetta65_DelUser, _mail);

                string sUrl = MailServerIP + Zetta65_DelUser;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta DelUser url : " + sUrl, AppSettingType.MAIL);

                string response = Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta DelUser response : " + response, AppSettingType.MAIL);

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelUser : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 패스워드 변경
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <returns></returns>
        public string ChangePwdUser(string _mail, string _newPw)
        {
            try
            {
                //zmail/api/change_pass.nvd?user_account={0}&amp;new_pass={1}&amp;is_encrypt={2}
                //암호화 is_encrypt 값은 1
                Zetta65_ModPasswd = string.Format(Zetta65_ModPasswd, _mail, _newPw, 1);

                string sUrl = MailServerIP + Zetta65_ModPasswd;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta ChangePwdUser url : " + sUrl, AppSettingType.MAIL);

                string response = Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta ChangePwdUser response : " + response, AppSettingType.MAIL);

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ChangePwdUser : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 부서 변경
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mailID">The _mail.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <returns></returns>
        public bool ChangeDeptUser(string _mailID, int _deptID)
        {
            try
            {
                Zetta65_ModDeptUser = string.Format(Zetta65_ModDeptUser, _mailID, _deptID);
                string sUrl = MailServerIP + Zetta65_ModDeptUser;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta ChangePwdUser url : " + sUrl, AppSettingType.MAIL);

                // Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("sUrl : " + sUrl.ToString());
                ///한번 호출 후 재 호출을 해 줘야지만 정상 동작 (신규 사용자 자동완성기능 + 회사주소록 나타나는 소스)
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);
                string response = Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("zetta ChangePwdUser response : " + response, AppSettingType.MAIL);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ChangeDeptUser : " + ex.Message);
                throw;
            }
        }

        public bool RetiredUser(string _email)
        {
            try
            {
                if (_email != "")
                {
                    Zetta65_RetiredUser = string.Format(Zetta65_RetiredUser, _email);
                    string sUrl = MailServerIP + Zetta65_RetiredUser;
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Zetta65_RetiredUser : " + sUrl, AppSettingType.MAIL);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RetiredUser : " + ex.Message);
                throw;
            }
        }

        public string RetireUserEmail(int _userID)
        {
            string email = "";
            try
            {
                DAL.MailZetta65 dal = new DAL.MailZetta65();
                DataSet ds = dal.RetiredUser(_userID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    email = ds.Tables[0].Rows[0]["mailID"].ToString() + "@" + ds.Tables[0].Rows[0]["MailDomain"].ToString();
                    //modify 윤석원 2015.01.13 (불필요하다고 함)
                    //Zetta65_RetiredUser = string.Format(Zetta65_RetiredUser, email);
                    //string sUrl = MailServerIP + Zetta65_RetiredUser;
                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);
                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Zetta65_RetiredUser : " + sUrl, AppSettingType.MAIL);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RetireUserEmail  : " + ex);
            }
            return email;
        }

        /// <summary>
        /// 사용자 비활성화
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>
        public string SuspendUser(string _email)
        {
            try
            {
                Zetta65_SuspendUser = String.Format(Zetta65_SuspendUser, _email);

                string sUrl = MailServerIP + Zetta65_SuspendUser;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("제타사용자 비활성화 : " + sUrl, AppSettingType.MAIL);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("제타사용자 비활성화 오류  : " + ex);
                throw;
            }

            return _email;
        }

        /// <summary>
        /// 사용자 활성화
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>
        public string ApprovalUser(string _email)
        {
            try
            {
                Zetta65_ApprovalUser = String.Format(Zetta65_ApprovalUser, _email);

                string sUrl = MailServerIP + Zetta65_ApprovalUser;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.SendPost(sUrl);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("제타사용자 활성화 : " + sUrl, AppSettingType.MAIL);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("제타사용자 활성화 오류  : " + ex);
                throw;
            }
            return _email;
        }

        /// <summary>
        /// 포틀릿 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_mailID"></param>
        /// <param name="_mailDomain"></param>
        /// <param name="_listCount"></param>
        /// <returns></returns>
        public DataTableReader GetMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            Zeospace.DeskPlus.Common.DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetMailList(_companyID, _mailID, _mailDomain, _listCount);
        }

        /// <summary>
        /// 메일주소로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mail">The _mail.</param>
        /// <returns></returns>
        public DataTableReader GetUserInfo(int _companyID, string _mail)
        {
            DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetUserInfo(_companyID, _mail);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 보낸 메일 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <param name="_mailbox_code">The _mailbox_code.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserSendMailList(int _companyID, string _user_sn, string _mailbox_code, int _pageNo, int _pageSize)
        {
            DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetUserSendMailList(_companyID, _user_sn, _mailbox_code, _pageNo, _pageSize);
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
            DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetUserMailBoxList(_companyID, _user_sn);
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
            DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetUserMailBoxListByCode(_companyID, _user_sn, _mailboxcode, _pageNo, _pageSize);
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
            DAL.MailZetta65 dal = new DAL.MailZetta65();
            return dal.GetUserMailConfirmList(_companyID, _mail, _pageSize);
        }
    }
}