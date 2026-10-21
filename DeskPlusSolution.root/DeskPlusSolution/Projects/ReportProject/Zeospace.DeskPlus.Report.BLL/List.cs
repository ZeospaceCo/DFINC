using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.BLL
{
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// Adds the specified _info.
        /// </summary>
        /// <param name="_listinfo">The _info.</param>
        /// <param name="_msg">The _MSG.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Add(BE.ListInfo _listinfo, string _msg, string _preLang)
        {
            if (_listinfo == null) return -1;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();
                _listinfo.IsLinked = "0"; //0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                _listinfo.ListID = dalList.Add(_listinfo);

                //조회테이블에 추가 및 수정자
                DAL.ListViewer dal = new DAL.ListViewer();
                dal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.WriteUserID, _listinfo.DeptID);

                #region 첨부파일

                if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));

                    if (di.Exists)
                    {
                        BLL.Attach attachDal = new Attach();
                        attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.HiddenSpoolPath, "");
                    }
                }

                #endregion 첨부파일

                SendAlarmConfirmUser(_listinfo, _msg);

                SendAlarmFolderUser(_listinfo, _msg);

                ts.Complete();
                return _listinfo.ListID;
            }
        }

        #region 선택이관, 선택복사 (사용자)

        /// <summary>
        /// 작성자 : 심재운
        /// 선택이관(사용자) : 타 사용자에게 이관하면, 이름과 부서명이 변경된다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <param name="_afterUserID">The _after user ID.</param>
        /// <returns></returns>
        public bool MoveUserSelList(int _companyID, string _list, int _afterUserID)
        {
            if (_list.Length == 0 || _afterUserID == 0) return false;

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetList(_companyID, _list);

            Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
            var uList = dalUser.GetUserInfo(_companyID, _afterUserID, true, true);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                //소유주를 변경한다.
                info.WriteUserID = _afterUserID;
                info.IsLinked = "1";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)

                foreach (var uinfo in uList)
                {
                    info.DeptID = uinfo.DeptID;//이관자 부서아이디 할당
                }

                dalList.Mod(info);
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 선택이관(부서) - 작성자는 그대로고, 부서만 변경된다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <param name="_afterDeptID">The _after dept ID.</param>
        /// <returns></returns>
        public bool MoveDeptSelList(int _companyID, string _list, int _afterDeptID)
        {
            if (_list.Length == 0 || _afterDeptID == 0) return false;

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetList(_companyID, _list);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                //소유자의 부서를 변경한다.
                info.DeptID = _afterDeptID;
                info.IsLinked = "3";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                dalList.Mod(info);
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 선택복사(사용자) - 타 사용자에게 이관하면, 이름과 부서명이 변경된다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <param name="_afterUserID">The _after user ID.</param>
        /// <returns></returns>
        public bool CopyUserSelList(int _companyID, string _list, int _afterUserID)
        {
            if (_list.Length == 0 || _afterUserID == 0) return false;

            DAL.List dalList = new DAL.List();
            DAL.ShortReply dalSr = new DAL.ShortReply();
            DAL.Attach dalAtt = new DAL.Attach();

            Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
            var uList = dalUser.GetUserInfo(_companyID, _afterUserID, true, true);

            var alList = dalList.GetList(_companyID, _list);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                info.WriteUserID = _afterUserID;
                info.IsLinked = "2";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)

                foreach (var uinfo in uList)
                {
                    info.DeptID = uinfo.DeptID;//이관자 부서아이디 할당
                }

                int rtnListID = dalList.Add(info);

                //댓글 저장하기
                var dtrSr = dalSr.GetShortReplyList(_companyID, info.ListID);

                if (dtrSr != null)
                {
                    if (dtrSr.HasRows)
                    {
                        while (dtrSr.Read())
                        {
                            var srInfo = new ShortReplyInfo();

                            srInfo.CompanyID = _companyID;
                            srInfo.ListID = rtnListID;
                            srInfo.UserID = dtrSr["UserID"].ToString().ToNumber<int>();
                            srInfo.Content = dtrSr["Content"].ToString();

                            dalSr.Add(srInfo);
                        }
                    }
                }

                //첨부파일 저장하기
                var attList = dalAtt.GetAttachLists(_companyID, info.ListID);

                foreach (var attInfo in attList)
                {
                    attInfo.CompanyID = _companyID;
                    attInfo.ListID = rtnListID;
                    dalAtt.Add(attInfo);
                }
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 선택복사(부서)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <param name="_afterDeptID">The _after dept ID.</param>
        /// <returns></returns>
        public bool CopyDeptSelList(int _companyID, string _list, int _afterDeptID)
        {
            if (_list.Length == 0 || _afterDeptID == 0) return false;

            DAL.List dalList = new DAL.List();
            DAL.ShortReply dalSr = new DAL.ShortReply();
            DAL.Attach dalAtt = new DAL.Attach();

            var alList = dalList.GetList(_companyID, _list);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                info.DeptID = _afterDeptID;
                info.IsLinked = "4";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                int rtnListID = dalList.Add(info);

                //댓글 저장하기
                var dtrSr = dalSr.GetShortReplyList(_companyID, info.ListID);

                if (dtrSr != null)
                {
                    if (dtrSr.HasRows)
                    {
                        while (dtrSr.Read())
                        {
                            var srInfo = new ShortReplyInfo();

                            srInfo.CompanyID = _companyID;
                            srInfo.ListID = rtnListID;
                            srInfo.UserID = dtrSr["UserID"].ToString().ToNumber<int>();
                            srInfo.Content = dtrSr["Content"].ToString();

                            dalSr.Add(srInfo);
                        }
                    }
                }

                //첨부파일 저장하기
                var attList = dalAtt.GetAttachLists(_companyID, info.ListID);

                foreach (var attInfo in attList)
                {
                    attInfo.CompanyID = _companyID;
                    attInfo.ListID = rtnListID;
                    dalAtt.Add(attInfo);
                }
            }

            return true;
        }

        #endregion 선택이관, 선택복사 (사용자)

        #region 전체이관, 전체복사 (사용자,부서)

        /// <summary>
        /// 작성자 : 심재운
        /// 전체이관(사용자)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_beforeUserID">The _before user ID.</param>
        /// <param name="_afterUserID">The _after user ID.</param>
        /// <returns></returns>
        public bool MoveUserAllList(int _companyID, int _beforeUserID, int _afterUserID)
        {
            if (_beforeUserID == 0 || _afterUserID == 0) return false;

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetListByUserID(_companyID, _beforeUserID);

            Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
            var uList = dalUser.GetUserInfo(_companyID, _afterUserID, true, true);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                //소유자 변경하기
                info.WriteUserID = _afterUserID;
                info.IsLinked = "1";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)

                foreach (var uinfo in uList)
                {
                    info.DeptID = uinfo.DeptID;//이관자 부서아이디 할당
                }

                dalList.Mod(info);
            }

            return true;
        }

        /// <summary>
        /// 전체복사(사용자)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_beforeUserID">The _before user ID.</param>
        /// <param name="_afterUserID">The _after user ID.</param>
        /// <returns></returns>
        public bool CopyUserAllList(int _companyID, int _beforeUserID, int _afterUserID)
        {
            if (_beforeUserID == 0 || _afterUserID == 0) return false;

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetListByUserID(_companyID, _beforeUserID);
            DAL.ShortReply dalSr = new DAL.ShortReply();
            DAL.Attach dalAtt = new DAL.Attach();

            Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
            var uList = dalUser.GetUserInfo(_companyID, _afterUserID, true, true);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                info.IsLinked = "2";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                info.WriteUserID = _afterUserID;

                foreach (var uinfo in uList)
                {
                    info.DeptID = uinfo.DeptID;//이관자 부서아이디 할당
                }

                int rtnListID = dalList.Add(info);

                //댓글 저장하기
                var dtrSr = dalSr.GetShortReplyList(info.CompanyID, info.ListID);

                if (dtrSr != null)
                {
                    if (dtrSr.HasRows)
                    {
                        while (dtrSr.Read())
                        {
                            var srInfo = new ShortReplyInfo();

                            srInfo.CompanyID = _companyID;
                            srInfo.ListID = rtnListID;
                            srInfo.UserID = dtrSr["UserID"].ToString().ToNumber<int>();
                            srInfo.Content = dtrSr["Content"].ToString();

                            dalSr.Add(srInfo);
                        }
                    }
                }

                //첨부파일 저장하기
                var attList = dalAtt.GetAttachLists(_companyID, info.ListID);

                foreach (var attInfo in attList)
                {
                    attInfo.CompanyID = _companyID;
                    attInfo.ListID = rtnListID;
                    dalAtt.Add(attInfo);
                }
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전체이관(부서)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_beforeDeptID">The _before dept ID.</param>
        /// <param name="_afterDeptID">The _after dept ID.</param>
        /// <returns></returns>
        public bool MoveDeptAllList(int _companyID, int _beforeDeptID, int _afterDeptID)
        {
            if (_beforeDeptID == 0 || _afterDeptID == 0) return false;

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetListByDeptID(_companyID, _beforeDeptID);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                //소유자의 부서를 변경한다.
                info.DeptID = _afterDeptID;
                info.IsLinked = "3";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                dalList.Mod(info);
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전체복사(부서)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_beforeDeptID">The _before dept ID.</param>
        /// <param name="_afterDeptID">The _after dept ID.</param>
        /// <returns></returns>
        public bool CopyDeptAllList(int _companyID, int _beforeDeptID, int _afterDeptID)
        {
            if (_beforeDeptID == 0 || _afterDeptID == 0) return false;

            DAL.ShortReply dalSr = new DAL.ShortReply();
            DAL.Attach dalAtt = new DAL.Attach();

            DAL.List dalList = new DAL.List();
            var alList = dalList.GetListByDeptID(_companyID, _beforeDeptID);

            foreach (var info in alList)
            {
                info.CompanyID = _companyID;
                info.IsLinked = "4";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                info.DeptID = _afterDeptID;
                int rtnListID = dalList.Add(info);

                //댓글 저장하기
                var dtrSr = dalSr.GetShortReplyList(info.CompanyID, info.ListID);

                if (dtrSr != null)
                {
                    if (dtrSr.HasRows)
                    {
                        while (dtrSr.Read())
                        {
                            var srInfo = new ShortReplyInfo();

                            srInfo.CompanyID = _companyID;
                            srInfo.ListID = rtnListID;
                            srInfo.UserID = dtrSr["UserID"].ToString().ToNumber<int>();
                            srInfo.Content = dtrSr["Content"].ToString();

                            dalSr.Add(srInfo);
                        }
                    }
                }

                //첨부파일 저장하기
                var attList = dalAtt.GetAttachLists(_companyID, info.ListID);

                foreach (var attInfo in attList)
                {
                    attInfo.CompanyID = _companyID;
                    attInfo.ListID = rtnListID;
                    dalAtt.Add(attInfo);
                }
            }

            return true;
        }

        #endregion 전체이관, 전체복사 (사용자,부서)

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 글수정
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_msg">The _MSG.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _listinfo, string _msg, string _preLang)
        {
            if (_listinfo != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.List dalList = new DAL.List();
                    dalList.Mod(_listinfo);

                    //조회테이블에 추가 및 수정자
                    DAL.ListViewer dal = new DAL.ListViewer();
                    dal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.ModifyUserID, _listinfo.DeptID);

                    #region 첨부파일 추가

                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.ListID,
                                _listinfo.HiddenSpoolPath, _listinfo.WriteDateDT);
                        }
                    }

                    #endregion 첨부파일 추가

                    SendAlarmConfirmUser(_listinfo, _msg);

                    SendAlarmFolderUser(_listinfo, _msg);

                    ts.Complete();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 승인자 처리하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_compUserID">The _comp user ID.</param>
        /// <param name="_compName">Name of the _comp.</param>
        /// <param name="_flag">The _flag.</param>
        /// <param name="_msg">The _MSG.</param>
        /// <returns></returns>
        public bool ConfirmMod(int _companyID, int _listID, int _userID, int _compUserID, string _compName, string _flag, string _msg)
        {
            DAL.List biz = new DAL.List();
            biz.ConfirmMod(_companyID, _listID, _compUserID, _flag);

            if (_msg.Length > 0)
            {
                BE.ListInfo _listinfo = new ListInfo();
                _listinfo.CompanyID = _companyID;
                _listinfo.Title = _msg;
                _listinfo.ListID = _listID;

                _listinfo.WriteUserID = _compUserID; //보내는 사용자 ID
                _listinfo.WriteUserName = _compName; //보내는 사람

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("list.cs -ConfirmMod() 알람시도 : " + _listinfo.ListID + "|" + _userID, AppSettingType.REPORT);

                if (_listinfo.ListID > 0 && _userID > 0)
                {
                    SendAlarm(_listinfo, _userID);
                }
            }

            return true;
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Del(ListInfo _listInfo)
        {
            try
            {
                if (_listInfo == null)
                {
                    return false;
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        //1.내용 삭제
                        DAL.List dalList = new DAL.List();
                        dalList.Del(_listInfo.CompanyID, _listInfo.ListID);

                        //2. 짧은 댓글 삭제
                        DAL.ShortReply dalShort = new DAL.ShortReply();
                        dalShort.Del(_listInfo.CompanyID, _listInfo.ListID);

                        //3. 첨부파일 실제 파일 삭제하기
                        DAL.Attach dalAttach = new DAL.Attach();
                        List<BE.AttachInfo> alAttach = dalAttach.GetAttachLists(_listInfo.CompanyID, _listInfo.ListID);

                        //5.1. 첨부파일 삭제
                        bool response = dalAttach.Del(_listInfo.CompanyID, _listInfo.ListID);

                        //이관 및 복사를 하므로 파일을 하나 가지고 쓰므로 , 실제파일은 삭제하지 않는다.
                        //if (response)
                        //{
                        //    foreach (BE.AttachInfo info in alAttach)
                        //    {
                        //        Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(info.FilePath, false);
                        //    }
                        //}

                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Report.List[Del] : " + ex);
                //ContextUtil.SetAbort();
                return false;
            }
        }

        #endregion DEL()

        #region 알람

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람 보내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public void SendAlarm(BE.ListInfo _listInfo, int _receiverID)
        {
            #region 로직
            /*
             */
            #endregion

            #region 소스
            
            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_listInfo.CompanyID, "AlarmWork");

            string AlarmWork = string.Empty;
            string AlarmMail = string.Empty;
            string AlarmMessenger = string.Empty;
            string AlarmMobile = string.Empty;
            string AlarmSMS = string.Empty;

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_listInfo.CompanyID, _receiverID);

            if (listUser != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                {
                    AlarmWork = infoUser.AlarmWork;
                }
            }

            if (AlarmWork.IndexOf("1-").Equals(0))
            {
                AlarmMail = (AlarmWork.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                AlarmMessenger = (AlarmWork.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                AlarmMobile = (AlarmWork.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                AlarmSMS = (AlarmWork.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
            }
            else
            {
                AlarmMessenger = "0";
                AlarmMail = "0";
                AlarmMobile = "0";
                AlarmSMS = "0";
            }

            foreach (DataRowView dr in dvAlarm)
            {
                string method = dr["CodeID"].ToString().ToUpper();
                switch (method)
                {
                    case "MESSENGER":
                        if (AlarmMessenger.Equals("1"))
                        {
                            SendMessenger(_listInfo, _receiverID);
                        }                        
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                SendMobile(_listInfo, _receiverID);                
            }

            #endregion            
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_listInfo"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ListInfo _listInfo, int _receiverID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _listInfo.CompanyID;
            alarmInfo.SenderID = _listInfo.WriteUserID;
            alarmInfo.SenderName = _listInfo.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _listInfo.Title.ToReverseXSS();
            alarmInfo.Content = _listInfo.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.REPORT, "AlarmTargetURL", "");
            if (_listInfo.Flag == "4") alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.REPORT, "AlarmTargetURL_PT", "");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listInfo.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Report";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 모바일
        /// </summary>
        /// <param name="_listInfo"></param>
        /// <param name="_receiverID"></param>
        /// <returns></returns>
        private bool SendMobile(BE.ListInfo _listInfo, int _receiverID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _listInfo.CompanyID;
            alarmInfo.SenderID = _listInfo.WriteUserID;
            alarmInfo.SenderName = _listInfo.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _listInfo.Title.ToReverseXSS();
            alarmInfo.Content = _listInfo.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.REPORT, "AlarmTargetURL", "");
            if (_listInfo.Flag == "4") alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.REPORT, "AlarmTargetURL_PT", "");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listInfo.ListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Report";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 보고자에게 알람 발송
        /// 심재운
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_msg">The _MSG.</param>
        public void SendAlarmConfirmUser(BE.ListInfo _listinfo, string _msg)
        {
            //상태(0:-,1:저장;2:요청;3:확인)
            if (_listinfo.Flag == "2" && _msg.Length > 0)
            {
                DAL.ShareUser biz = new DAL.ShareUser();
                DataTableReader dtr = biz.GetShareUserTargerUserInfo(_listinfo.CompanyID, _listinfo.WriteUserID);

                int targetUserID = 0;
                _listinfo.Title = _msg;
                _listinfo.AlarmContent = _msg;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            targetUserID = dtr["TargetUserID"].ToString().ToNumber<int>();

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("list.cs -Add() 알람시도 : " + _listinfo.ListID + "|" + targetUserID, AppSettingType.REPORT);

                            if (_listinfo.ListID > 0 && targetUserID > 0)
                            {
                                SendAlarm(_listinfo, targetUserID);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 분류 자에게 알람 발송
        /// 심재운
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_msg">The _MSG.</param>
        public void SendAlarmFolderUser(BE.ListInfo _listinfo, string _msg)
        {
            //상태(0:-,1:저장;2:요청;3:확인;4:보고용)
            if (_listinfo.Flag == "4" && _msg.Length > 0)
            {
                BLL.FolderAuth biz = new FolderAuth();
                string sendUserIDs = biz.GetFolderAuthUser(_listinfo.CompanyID, _listinfo.FolderID, true);

                if (!string.IsNullOrEmpty(sendUserIDs))
                {
                    _listinfo.Title = _msg;
                    _listinfo.AlarmContent = _msg;

                    //전체 알람 발송
                    if (sendUserIDs == "all")
                    {
                        Account.DAL.User userBiz = new Account.DAL.User();
                        List<int> al = userBiz.GetUserIDList(_listinfo.CompanyID, true); //전체 사용자 가져오기

                        foreach (int targetUserID in al)
                        {
                            //(본인제외)
                            if (_listinfo.WriteUserID != targetUserID) SendAlarm(_listinfo, targetUserID);
                        }
                    }
                    else //부서 및 사용자 알람 발송
                    {
                        sendUserIDs = sendUserIDs.TrimEnd(',');

                        List<string> userList = sendUserIDs.Split(',').ToList<string>();
                        IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                        foreach (string targetUserID in userDis)
                        {
                            //부서 및 사용자 알람 발송 (본인제외)
                            if (_listinfo.WriteUserID != targetUserID.ToNumber<int>()) SendAlarm(_listinfo, targetUserID.ToNumber<int>());
                        }
                    }
                }
            }
        }

        #endregion 알람
    }
}