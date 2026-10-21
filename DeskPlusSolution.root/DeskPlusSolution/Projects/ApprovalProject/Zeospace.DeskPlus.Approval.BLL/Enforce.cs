using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class Enforce : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 공문신청
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.EnforceInfo _info, UserInfo _sendInfo)
        {
            if (_info == null) return -1;

            int enforceID = 0;

            try
            {
                DAL.Enforce dal = new DAL.Enforce();
                enforceID = dal.Add(_info);

                if (enforceID > 0 && _info.AlarmSubject.ToNull() != "")
                {
                    #region 알람-메신저 (공문담당자한테 알람발송)

                    // 공문대상자 가져오기
                    DAL.EnforceAdmin daladmin = new DAL.EnforceAdmin();
                    DataTableReader dtr = daladmin.GetEnforceAdminList(_info.CompanyID, _info.EnforceStampID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Common.BLL.Alarm();
                            while (dtr.Read())
                            {
                                AlarmInfo alarmInfo = new AlarmInfo();
                                alarmInfo.CompanyID = _info.CompanyID;
                                alarmInfo.SenderID = _sendInfo.UserID;
                                alarmInfo.SenderName = _sendInfo.Name;
                                alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                                alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                                alarmInfo.ReceiverID = dtr["UserID"].ToString().ToNumber<int>();
                                alarmInfo.ReceiverName = "";
                                alarmInfo.ReceiverMail = "";
                                alarmInfo.ReceiverPhone = "";
                                alarmInfo.Status = "";
                                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.Subject = _info.AlarmSubject;
                                alarmInfo.Content = _info.AlarmContent;
                                alarmInfo.TargetUrl = "";
                                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.WriteDate = System.DateTime.Now;
                                alarmInfo.AlarmType = 1;
                                alarmInfo.KindSubject = "Approval";
                                alarmInfo.ViewOption = "01";
                                alarmInfo.Store = 1;
                                alarmInfo.UserState = 0;

                                alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                            }
                        }
                    }
                    #endregion
                }
                return enforceID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Enforce[Add] : " + ex);
                return -1;
            }
        }
        #endregion

        #region MOD
        /// <summary>
        /// 공문신청내용수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.EnforceInfo _info, bool _isReject, UserInfo _sendInfo)
        {
            if (_info == null) return false;

            try
            {
                DAL.Enforce dal = new DAL.Enforce();
                bool rtnval = dal.Mod(_info);

                if (rtnval && _isReject && _info.AlarmSubject.ToNull() != "")
                {
                    #region 알람-메신저 (공문담당자한테 알람발송)

                    // 공문대상자 가져오기
                    DAL.EnforceAdmin daladmin = new DAL.EnforceAdmin();
                    DataTableReader dtr = daladmin.GetEnforceAdminList(_info.CompanyID, _info.EnforceStampID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Common.BLL.Alarm();
                            while (dtr.Read())
                            {
                                AlarmInfo alarmInfo = new AlarmInfo();
                                alarmInfo.CompanyID = _info.CompanyID;
                                alarmInfo.SenderID = _sendInfo.UserID;
                                alarmInfo.SenderName = _sendInfo.Name;
                                alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                                alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                                alarmInfo.ReceiverID = dtr["UserID"].ToString().ToNumber<int>();
                                alarmInfo.ReceiverName = "";
                                alarmInfo.ReceiverMail = "";
                                alarmInfo.ReceiverPhone = "";
                                alarmInfo.Status = "";
                                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.Subject = _info.AlarmSubject;
                                alarmInfo.Content = _info.AlarmContent;
                                alarmInfo.TargetUrl = "";
                                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.WriteDate = System.DateTime.Now;
                                alarmInfo.AlarmType = 1;
                                alarmInfo.KindSubject = "Approval";
                                alarmInfo.ViewOption = "01";
                                alarmInfo.Store = 1;
                                alarmInfo.UserState = 0;

                                alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                            }
                        }
                    }
                    #endregion
                }
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Enforce[Mod] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 공문신청승인/반려
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Confirm(BE.EnforceInfo _info, UserInfo _sendInfo)
        {
            if (_info == null) return false;

            try
            {
                DAL.Enforce dal = new DAL.Enforce();
                bool rtnval = dal.ModConfirm(_info);

                if (rtnval && !_info.ReqUserID.Equals(0))
                {
                    #region 알람-메신저 (공문신청자한테 알람발송)

                    Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Common.BLL.Alarm();

                    AlarmInfo alarmInfo = new AlarmInfo();
                    alarmInfo.CompanyID = _info.CompanyID;
                    alarmInfo.SenderID = _sendInfo.UserID;
                    alarmInfo.SenderName = _sendInfo.Name;
                    alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                    alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                    alarmInfo.ReceiverID = _info.ReqUserID;
                    alarmInfo.ReceiverName = "";
                    alarmInfo.ReceiverMail = "";
                    alarmInfo.ReceiverPhone = "";
                    alarmInfo.Status = "";
                    alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.Subject = _info.AlarmSubject;
                    alarmInfo.Content = _info.AlarmContent;
                    alarmInfo.TargetUrl = "";
                    alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.WriteDate = System.DateTime.Now;
                    alarmInfo.AlarmType = 1;
                    alarmInfo.KindSubject = "Approval";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());

                    #endregion
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Enforce[Confirm] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL
        /// <summary>
        /// 공문삭제
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _enforceID, bool _isAdmin)
        {
            if (_enforceID.Equals(0)) return false;

            try
            {
                DAL.Enforce dal = new DAL.Enforce();
                bool rtnval = dal.Del(_companyID, _enforceID);

                if (rtnval && _isAdmin)
                {
                    // 공문신청자한테 알람발송
                }
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Enforce[Del] : " + ex);
                return false;
            }
        }
        #endregion

        
    }
}
