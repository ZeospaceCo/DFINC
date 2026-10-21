using System;
using System.Collections;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class MailZettaFactory
    {
        private string MailProduct = string.Empty, ServerUrl = string.Empty, IsUserDel = string.Empty;

        public MailZettaFactory()
        {
            //메일 업체
            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            ServerUrl = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://zetta.zeospace.com:4040");
            IsUserDel = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_IsUserDel", "0");     //사용자삭제시, 영구삭제여부 (정지-0/삭제-1/휴면-2)
        }

        #region ZETTA Mail

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 계정 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _id.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_email">The _email.</param>
        /// <returns></returns>
        public string AddUser(int _companyID, string _mailID, string _pw, string _name, string _email, int _isEncrypt)
        {
            string response = "";

            if (_mailID.Length == 0 || _pw.Length == 0 || _name.Length == 0 || _email.Length == 0) return response;

            if (MailProduct.ToUpper() == "ZETTA65")
            {
                MailZetta65 biz = new MailZetta65();
                response = biz.AddUser(_companyID, _mailID, _pw, _name, _email, _isEncrypt);
            }

            return response;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 계정 삭제
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mail">The _id.</param>
        /// <returns></returns>
        public string DelUser(string _mail)
        {
            string response = "";

            if (_mail.Length == 0 || _mail == "@") return response;

            if (MailProduct.ToUpper() == "ZETTA65")
            {
                MailZetta65 biz = new MailZetta65();
                response = biz.DelUser(_mail);
            }

            return response;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 패스워드 변경
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <returns></returns>
        public string ChangePwdUser(int _companyID, string _mailID, string _newPw)
        {
            string response = "";

            if (_mailID.Length == 0 || _newPw.Length == 0) return response;

            if (MailProduct.ToUpper() == "ZETTA65")
            {
                MailZetta65 biz = new MailZetta65();
                response = biz.ChangePwdUser(_mailID, _newPw);
            }

            return response;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : 메일 사용자 부서 변경
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_id">The _id.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <returns></returns>
        public bool ChangeDeptUser(string _mailID, int _deptID)
        {
            bool response = false;

            if (_mailID.Length == 0 || _deptID == 0) return response;

            if (MailProduct.ToUpper() == "ZETTA65")
            {
                MailZetta65 biz = new MailZetta65();
                response = biz.ChangeDeptUser(_mailID, _deptID);
            }

            return response;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// MHT 생성하기 (다중값은 ; 으로 할당)
        /// </summary>
        /// <param name="_sendMail">The _send mail.</param>
        /// <param name="_sendName">Name of the _send.</param>
        /// <param name="_to">The _to.</param>
        /// <param name="_cc">The _CC.</param>
        /// <param name="_bcc">The _BCC.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_body">The _body.</param>
        /// <param name="_attach">The _attach.</param>
        /// <param name="_empSpoolPath">The _emp spool path.</param>
        /// <param name="_xExtention">The _x extention.</param>
        /// <returns></returns>
        public string CreateMHT(string _sendMail, string _sendName, string _to, string _cc, string _bcc, string _subject, string _body, Hashtable _attach, string _empSpoolPath, bool _xExtention)
        {
            MailDataInfo mailDInfo = new MailDataInfo();

            //key : 보여줄 첨부파일, value : 첨부할 실제 파일 경로
            mailDInfo.Attach = _attach;

            //보내는 사람
            mailDInfo.From = _sendName + "<" + _sendMail + ">";

            //받는 사람
            foreach (string tmp in _to.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailDInfo.To.Contains(tmp)) mailDInfo.To.Add(tmp, tmp);
                }
            }

            //참조
            foreach (string tmp in _cc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailDInfo.Cc.Contains(tmp)) mailDInfo.Cc.Add(tmp, tmp);
                }
            }

            //숨은 참조
            foreach (string tmp in _bcc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailDInfo.Bcc.Contains(tmp)) mailDInfo.Bcc.Add(tmp, tmp);
                }
            }

            bool addAttach = false;
            if (_attach != null)
            {
                addAttach = (_attach.Count > 0) ? true : false;
            }

            mailDInfo.Subject = _subject;
            mailDInfo.Contents = _body;

            string xExtentionString = "";

            if (_xExtention)
            {
                string msg = SendMail.GetEmail(mailDInfo.From) + ";no_save_attach=0;alert_message=";
                xExtentionString = "X-Zmail-Savemail: =?utf-8?B?" + Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(msg)) + "?=\r\n";
            }

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mail:SendMail" + _to + "," + _sendMail + "," + _subject, AppSettingType.MAIL);

            SendMail biz = new SendMail();
            var saveFullFileName = biz.SaveEml(mailDInfo, _empSpoolPath, addAttach, xExtentionString);

            return saveFullFileName;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 :
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mURL">Post Url</param>
        /// <returns></returns>
        public string SendPost(string _mURL)
        {
            string getValue = "";
            if (_mURL.Length == 0) return "";

            _mURL = ServerUrl + _mURL;
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_mURL : " + _mURL, AppSettingType.MAIL);
            // Send to server
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_mURL);
            httpWebRequest.Method = "get";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Get response
            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream))
                        {
                            StringBuilder response = new StringBuilder();

                            char[] read = new Char[256];
                            int count = responseReader.Read(read, 0, 256);

                            while (count > 0)
                            {
                                response.Append(read, 0, count);
                                count = responseReader.Read(read, 0, 256);
                            }
                            responseReader.Close();
                            getValue = response.ToString();
                        }
                    }
                }
            }

            getValue = getValue.Replace("\r\n", "|");
            return getValue;
        }

        /// <summary>
        /// 포틀릿 메일 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_mailID"></param>
        /// <param name="_mailDomain"></param>
        /// <param name="_listCount"></param>
        /// <returns></returns>
        public DataTableReader GetMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetMailList(_companyID, _mailID, _mailDomain, _listCount);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
                BLL.MailZetta52 biz = new MailZetta52();
                dtr = biz.GetMailList(_companyID, _mailID, _mailDomain, _listCount);
            }

            return dtr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 메일주소로 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mail">The _mail.</param>
        /// <returns></returns>
        public DataTableReader GetUserInfo(int _companyID, string _mail)
        {
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetUserInfo(_companyID, _mail);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
            }

            return dtr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 보낸 메일 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_user_sn">The _member_sn.</param>
        /// <param name="_mailbox_code">The _mailbox_code.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserSendMailList(int _companyID, string _mailID, string _mailDomain, string _user_sn, string _mailbox_code, int _pageNo, int _pageSize)
        {
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetUserSendMailList(_companyID, _user_sn, _mailbox_code, _pageNo, _pageSize);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
                BLL.MailZetta52 biz = new MailZetta52();
                dtr = biz.GetUserSendMailList(_companyID, _mailID, _mailDomain, _pageSize);
            }

            return dtr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxList(int _companyID, string _mailID, string _mailDomain, string _user_sn)
        {
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetUserMailBoxList(_companyID, _user_sn);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
                BLL.MailZetta52 biz = new MailZetta52();
                dtr = biz.GetUserMailBoxList(_companyID, _mailID, _mailDomain);
            }

            return dtr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 특정 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <param name="_mailboxcode">The _mailboxcode.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxListByCode(int _companyID, string _mailID, string _mailDomain, string _user_sn, string _mailboxcode, int _pageNo, int _pageSize)
        {
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetUserMailBoxListByCode(_companyID, _user_sn, _mailboxcode, _pageNo, _pageSize);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
                BLL.MailZetta52 biz = new MailZetta52();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetUserMailBoxListByCode - _mailID : " + _mailID, AppSettingType.MAIL);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetUserMailBoxListByCode - _mailDomain : " + _mailDomain, AppSettingType.MAIL);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetUserMailBoxListByCode - _mailboxcode : " + _mailboxcode, AppSettingType.MAIL);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("GetUserMailBoxListByCode - _pageSize : " + _pageSize, AppSettingType.MAIL);

                dtr = biz.GetUserMailBoxListByCode(_companyID, _mailID, _mailDomain, _mailboxcode, _pageSize);
            }

            return dtr;
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
            DataTableReader dtr = null;
            if (MailProduct.ToUpper() == "ZETTA65")
            {
                BLL.MailZetta65 biz = new MailZetta65();
                dtr = biz.GetUserMailConfirmList(_companyID, _mail, _pageSize);
            }
            else if (MailProduct.ToUpper() == "ZETTA52")
            {
                BLL.MailZetta52 biz = new MailZetta52();
                dtr = biz.GetUserMailConfirmList(_companyID, _mail, _pageSize);
            }

            return dtr;
        }

        #endregion ZETTA Mail
    }
}