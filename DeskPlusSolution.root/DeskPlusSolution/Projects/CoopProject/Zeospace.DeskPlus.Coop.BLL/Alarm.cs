using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.BLL
{
    public class Alarm : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 알람

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람 보내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_senderUserID">The _sender user ID.</param>
        /// <param name="_senderName">Name of the _sender.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_contents">The _contents.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <returns></returns>
        public bool SendAlarm(int _companyID, int _senderUserID, string _senderName, string _subject, 
            string _contents, int _listID, int _groupID, int _receiverID, int _forwardListID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderUserID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject.ToReverseXSS();
            alarmInfo.Content = _contents.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COOP, "AlarmTargetURL", "{0}{1}/DeskPlusEIP/Coop/Content/Total/ViewPopup.aspx");
            if (_forwardListID > 0) alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COOP, "AlarmForwardTargetURL", "{0}{1}/DeskPlusEIP/Coop/Content/Forward/ViewPopup.aspx");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listID.ToString());
            paraDic.Add("groupID", _groupID.ToString());
            if (_forwardListID > 0) paraDic.Add("forwardListID", _forwardListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Coop";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// SMS 발송하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_receiveUsers">The _sender users.</param>
        /// <param name="_contents">The _contents.</param>
        public void SendSMSUser(int _companyID, int _userID, string _loginID, string _receiveUsers, string _contents)
        {
            if (_receiveUsers.Length > 0)
            {
                Common.BLL.SMS bll = new Common.BLL.SMS();

                Account.DAL.User userDal = new Account.DAL.User();
                
                foreach (string receiverMobile in userDal.GetMobileByUsers(_companyID, _receiveUsers, true))
                {
                    //부서 및 사용자 알람 발송
                    bll.SendSMS(_companyID, _userID, _loginID, _contents, false, receiverMobile);
                }
            }
        }

        #endregion

        #region 요청알람

        /// <summary>
        /// 요청 = 수신자 + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_loginID">The _login identifier.</param>
        /// <param name="_writeUsername">The _write username.</param>
        /// <param name="_toContent">Content of the _to.</param>
        /// <param name="_ccContent">Content of the _CC.</param>
        /// <returns></returns>
        public bool AlarmRequest(int _companyID, int _listID, int _writeUserID, string _loginID, string _writeUsername, string _toContent, string _ccContent)
        {
            if (_writeUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmRequest", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("TO") != -1)
            {
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    if (userID.Length > 0)
                    {
                        //업무협업 | "요청자" 로부터 "협업제목" 수신 되었습니다.|요청자
                        SendAlarm(_companyID, _writeUserID, _writeUsername,
                            _toContent, _toContent, _listID, _listID, userID.ToNumber<int>(), 0);

                        //수신자에게만 SMS 문자 발송함.
                        SendSMSUser(_companyID, _writeUserID, _loginID, userID.ToString(), _toContent);
                    }
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        //업무협업 | "요청자" 로부터 "협업제목" 참조 되었습니다.|요청자
                        SendAlarm(_companyID, _writeUserID, _writeUsername,
                            _ccContent, _ccContent, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 접수알람

        /// <summary>
        /// 접수 = 요청자 + 수신자(접수자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_acceptUserID">The _accept user ID.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmAccept(int _companyID, int _listID, int _writeUserID, int _acceptUserID, string _acceptUserName, string _content)
        {
            if (_writeUserID == 0 || _acceptUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmAccept", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("FROM") != -1)
            {
                //요청자
                SendAlarm(_companyID, _acceptUserID, _acceptUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            }
            
            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    //접수자는 제외한다.
                    if (_acceptUserID != userID.ToNumber<int>() && userID.Length > 0)
                    {
                        SendAlarm(_companyID, _acceptUserID, _acceptUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {
                //참조자
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        SendAlarm(_companyID, _acceptUserID, _acceptUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 완료알람

        /// <summary>
        /// 완료 = 요청자 + 수신자(완료자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_acceptUserID">The _accept user ID.</param>
        /// <param name="_acceptUserName">Name of the _accept user.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmComplete(int _companyID, int _listID, int _writeUserID, int _completeUserID, string _completeUserName, string _content)
        {
            if (_writeUserID == 0 || _completeUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmComplete", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("FROM") != -1)
            {
                //요청자
                SendAlarm(_companyID, _completeUserID, _completeUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            }

            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    //완료자는 제외한다.
                    if (_completeUserID != userID.ToNumber<int>() && userID.Length > 0)
                    {
                        SendAlarm(_companyID, _completeUserID, _completeUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        SendAlarm(_companyID, _completeUserID, _completeUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 반려알람

        /// <summary>
        /// 반려 =  요청자 + 수신자(반려자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_rejectUserID">The _reject user ID.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmReject(int _companyID, int _listID, int _writeUserID, int _rejectUserID, string _rejectUserName, string _content)
        {
            if (_writeUserID == 0 || _rejectUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmReject", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("FROM") != -1)
            {
                //요청자
                SendAlarm(_companyID, _rejectUserID, _rejectUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            }

            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    //반려자는 제외한다.
                    if (_rejectUserID != userID.ToNumber<int>() && userID.Length > 0)
                    {
                        SendAlarm(_companyID, _rejectUserID, _rejectUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {
                //참조자
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        SendAlarm(_companyID, _rejectUserID, _rejectUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 답글알람

        /// <summary>
        /// 요청자 답글 = [수신자 + 참조자]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmReplyRequest(int _companyID, int _listID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_replyUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmReplyRequest", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {
                //참조자
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        SendAlarm(_companyID, _replyUserID, _replyUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 참조자 답글 = [요청자 + 수신자 + 참조자(작성자제외)]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmReplyCC(int _companyID, int _listID, int _writeUserID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_writeUserID == 0 || _replyUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmReplyCC", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("FROM") != -1)
            {
                //요청자
                SendAlarm(_companyID, _replyUserID, _replyUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            }

            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            //참조자
            DAL.ListCC ccDal = new DAL.ListCC();
            var ccList = ccDal.GetListCC(_companyID, _listID);
            string tmpCCUser = "";
            foreach (var info in ccList) tmpCCUser += info.UserID + ",";
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                //참조자 중, 답변 작성자는 제외한다.
                if (_replyUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            return true;

        }

        /// <summary>
        /// 수신자 답글 = [요청자 + 수신자(답글작성자제외) + 참조자]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmReplyTO(int _companyID, int _listID, int _writeUserID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_writeUserID == 0 || _replyUserID == 0) return false;

            var alarmType = AppSettingHelper.GetSysCode(_companyID, "AlarmReplyTO", "CoopAlarm", "ko");
            if (alarmType.Length == 0) alarmType = "FROM,TO,CC";//발신,수신,참조 전부 발송

            if (alarmType.IndexOf("FROM") != -1)
            {
                //요청자
                SendAlarm(_companyID, _replyUserID, _replyUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            }

            if (alarmType.IndexOf("TO") != -1)
            {
                //수신자
                DAL.ListTO toDal = new DAL.ListTO();
                var toList = toDal.GetListTo(_companyID, _listID);
                string tmpTOUser = "";
                foreach (var info in toList) tmpTOUser += info.UserID + ",";
                tmpTOUser = tmpTOUser.TrimEnd(',');

                List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
                IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisTO)
                {
                    //답글작성자제외
                    if (_replyUserID != userID.ToNumber<int>() && userID.Length > 0)
                    {
                        SendAlarm(_companyID, _replyUserID, _replyUserName,
                                _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            if (alarmType.IndexOf("CC") != -1)
            {

                //참조자
                DAL.ListCC ccDal = new DAL.ListCC();
                var ccList = ccDal.GetListCC(_companyID, _listID);
                string tmpCCUser = "";
                foreach (var info in ccList) tmpCCUser += info.UserID + ",";
                tmpCCUser = tmpCCUser.TrimEnd(',');

                List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
                IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

                foreach (string userID in userDisCC)
                {
                    if (userID.Length > 0)
                    {
                        SendAlarm(_companyID, _replyUserID, _replyUserName,
                                _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                    }
                }
            }

            return true;
        }

        #endregion

        #region 전달알람

        /// <summary>
        /// 전달 = 전달받을자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_writeUsername">The _write username.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmForward(int _companyID, int _listID, int _writeUserID, string _writeUsername, string _forwardUserID, int _forwardListID, string _content)
        {
            if (string.IsNullOrEmpty(_forwardUserID) || _forwardUserID.Length == 0) return false;

            List<string> fList = _forwardUserID.Split(',').ToList<string>();
            IEnumerable<string> uInfo = fList.Distinct(); //사용자 중복제거

            foreach (string userID in uInfo)
            {
                //gubun 은 forward : 전달받은협업 경로 팝업은 다르다.
                SendAlarm(_companyID, _writeUserID, _writeUsername,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), _forwardListID);
            }

            return true;
        }

        #endregion
    }
}
