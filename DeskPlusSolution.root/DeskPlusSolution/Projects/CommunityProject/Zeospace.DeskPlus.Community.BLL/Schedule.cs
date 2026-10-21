using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Community.BE;
using System.Transactions;
using System.Data;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class Schedule : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 등록 (반복일정 0)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Add(ScheduleInfo _info, List<BE.ScheduleEtcInfo> _al, RepeatInfo _repInfo)
        {
            try
            {
                int tmpVal = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (BE.ScheduleEtcInfo info in _al)
                    {
                        // 1.Schedule 테이블 처리
                        DAL.Schedule dal = new DAL.Schedule();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentScheduleID = tmpVal;

                        int scheduleID = dal.Add(_info);

                        if (_info.ParentScheduleID == 0)
                        {
                            tmpVal = scheduleID;
                        }

                        if (scheduleID < 1)
                        {
                            return -1;
                        }
                    }

                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ScheduleID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, tmpVal);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(Repeat)", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 등록 (반복일정 X)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ScheduleInfo _info)
        {
            try
            {
                int scheduleID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    scheduleID = dal.Add(_info);

                    if (scheduleID < 1)
                    {
                        return -1;
                    }      
                                        
                    ts.Complete();
                }

                SendAlarm(_info, scheduleID);

                return scheduleID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 0) - 이번일정만 / 반복일정 미 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentScheduleID"></param>
        /// <returns></returns>
        public bool Mod(ScheduleInfo _info, int _parentScheduleID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    int ParentScheduleID = dal.Mod(_info);

                    if (ParentScheduleID < 0)
                    {
                        return false;
                    }                  

                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentScheduleID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우 
                        // 3-1.반복 테이블 삭제 
                        if (!repdal.Del(_info.CompanyID, _info.CommunityID, _info.ScheduleID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 이미 지정되어 있는 다른 반복일정이 있을 경우                              
                        if (_parentScheduleID != ParentScheduleID)
                        {
                            // 3-2.반복 테이블 업데이트
                            if (!repdal.Upd(_info.CompanyID, _info.CommunityID, _parentScheduleID, ParentScheduleID))
                            {
                                return false;
                            }                            
                        }                       
                    }                    

                    ts.Complete();
                }

                SendAlarm(_info, _info.ScheduleID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 0) - 이번일정만 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(ScheduleInfo _info, List<BE.ScheduleEtcInfo> _al, RepeatInfo _repInfo, int _parentScheduleID)
        {
            try
            {
                int tmpVal = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Schedule /  Repeat
                    int ParentScheduleID = ModRepeat(_info.CompanyID, _info.CommunityID, _info.ScheduleID, _info.ParentScheduleID);
                    if (ParentScheduleID < 0)
                    {
                        return -1;
                    }

                    foreach (BE.ScheduleEtcInfo info in _al)
                    {
                        // 2. Schedule 테이블 처리
                        DAL.Schedule dal = new DAL.Schedule();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentScheduleID = tmpVal;

                        int scheduleID = dal.Add(_info);
                        if (_info.ParentScheduleID == 0)
                        {
                            tmpVal = scheduleID;
                        }

                        if (scheduleID < 1)
                        {
                            return -1;                            
                        }
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ScheduleID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }                  

                    ts.Complete();
                }

                SendAlarm(_info, tmpVal);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod2", ex);
                return -1;
            }
        }

        /// <summary>        
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 0) - 향후 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_parentScheduleID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModFuture(ScheduleInfo _info, List<BE.ScheduleEtcInfo> _al, RepeatInfo _repInfo, int _parentScheduleID, string _startDate)
        {
            try
            {
                int tmpVal = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Schedule / Repeat 
                    int ParentScheduleID = ModRepeat(_info.CompanyID, _info.CommunityID, _info.ParentScheduleID, _startDate);
                    if (ParentScheduleID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ScheduleEtcInfo info in _al)
                    {
                        // 2. Schedule 테이블 처리
                        DAL.Schedule dal = new DAL.Schedule();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentScheduleID = tmpVal;

                        int scheduleID = dal.Add(_info);
                        if (_info.ParentScheduleID == 0)
                        {
                            tmpVal = scheduleID;
                        }

                        if (scheduleID < 1)
                        {
                            return -1;
                        }          
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ScheduleID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, tmpVal);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFuture", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 0) - 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_parentScheduleID"></param>
        /// <returns></returns>
        public int ModAll(ScheduleInfo _info, List<BE.ScheduleEtcInfo> _al, RepeatInfo _repInfo, int _parentScheduleID)
        {
            try
            {
                int tmpVal = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Schedule / Repeat 삭제
                    DAL.Schedule dal = new DAL.Schedule();
                    if (!dal.DelAll(_info.CompanyID, _info.CommunityID, _info.ParentScheduleID))
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ScheduleEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentScheduleID = tmpVal;

                        int scheduleID = dal.Add(_info);
                        if (_info.ParentScheduleID == 0)
                        {
                            tmpVal = scheduleID;
                        }

                        if (scheduleID < 1)
                        {
                            return -1;
                        }
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ScheduleID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, tmpVal);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModAll", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 0) - 반복이 없었다가 생긴 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(ScheduleInfo _info, List<BE.ScheduleEtcInfo> _al, RepeatInfo _repInfo)
        {
            try
            {
                int tmpVal = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Schedule  삭제
                    DAL.Schedule dal = new DAL.Schedule();
                    if (!dal.Del(_info.CompanyID, _info.CommunityID, _info.ScheduleID))
                    {
                        return -1;
                    }          

                    // 새로운 일정 처리
                    foreach (BE.ScheduleEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentScheduleID = tmpVal;

                        int scheduleID = dal.Add(_info);
                        if (_info.ParentScheduleID == 0)
                        {
                            tmpVal = scheduleID;
                        }

                        if (scheduleID < 1)
                        {
                            return -1;
                        }
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ScheduleID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, tmpVal);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod4", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 X) - 선택해서 들어간 날짜 이후로의 반복 일정은 삭제, 새로운 일정 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentScheduleID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModFuture(ScheduleInfo _info, int _parentScheduleID, string _startDate)
        {
            try
            {
                int scheduleID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. ScheduleID /  Repeat 
                    int ParentScheduleID = ModRepeat(_info.CompanyID, _info.CommunityID, _parentScheduleID, _startDate);
                    if (ParentScheduleID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    // 2. Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    scheduleID = dal.Add(_info);

                    if (scheduleID < 1)
                    {                        
                        return -1;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, scheduleID);

                return scheduleID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFuture2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 수정  (반복일정 X) - 반복설정이 첨부터 없었을 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool Mod(ScheduleInfo _info)
        {
            try
            {
                int parentScheduleID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    parentScheduleID = dal.Mod(_info);

                    if (parentScheduleID < 0)
                    {
                        return false;
                    }

                    ts.Complete();
                }

                SendAlarm(_info, parentScheduleID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod3", ex);
                return false;
            }
        }

        #endregion 

        #region 알람발송
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 알람발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ScheduleInfo _info, int _scheduleID)
        {
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS

            BLL.Alarm bll = new Alarm();

            //알람 정보 가져오기
            if (_info.Alert == "Y")
            {
                DAL.User userdal = new DAL.User();
                DataTableReader dtr = userdal.GetUserAuth(_info.CompanyID, _info.CommunityID, _info.MenuID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string AlarmCommunity = dtr["AlarmCommunity"].ToString();
                            string AlarmMail = string.Empty;
                            string AlarmMessenger = string.Empty;
                            string AlarmMobile = string.Empty;
                            string AlarmSMS = string.Empty;

                            if (AlarmCommunity.IndexOf("1-").Equals(0))
                            {
                                AlarmMail = (AlarmCommunity.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                AlarmMessenger = (AlarmCommunity.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                AlarmMobile = (AlarmCommunity.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                AlarmSMS = (AlarmCommunity.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                AlarmMessenger = "0";
                                AlarmMail = "0";
                                AlarmMobile = "0";
                                AlarmSMS = "0";
                            }

                            if (AlarmMessenger.Equals("1"))
                            {
                                userIDMESSENGER += dtr["UserID"].ToString() + ",";
                            }

                            userIDMOBILE += dtr["UserID"].ToString() + ",";                                                                                

                            if (AlarmMail.Equals("1"))
                            {
                                userIDMAIL += dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString() + ",";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                userIDSMS += dtr["MobilePhone"].ToString() + ",";
                            }
                        }
                    }
                }
            }

            foreach (DataRowView dr in dvAlarm)
            {
                string method = dr["CodeID"].ToString().ToUpper();
                switch (method)
                {
                    case "MAIL":
                        if (_info.Alert == "Y")
                        {
                            if (_info.AlertType == "Mail")
                            {
                                if (userIDMAIL.Length > 0)
                                {
                                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailbiz = new Common.BLL.MailZettaFactory();
                                    mailbiz.CreateMHT(_info.AlarmMail, _info.WriteUserName, userIDMAIL, "", "", _info.Title, _info.Content, null, "", false);
                                }
                            }
                        }
                        break;

                    case "MESSENGER":
                        if ((_info.Alert == "Y") && (_info.AlertType == "Messenger") && (userIDMESSENGER.Length > 0))
                        {
                            foreach (string userID in userIDMESSENGER.Split(','))
                            {
                                if (userID != "")
                                {
                                    bll.SendAlarm(_info, userID.ToNumber<int>(), _scheduleID);
                                }
                            }
                        }
                        break;

                    case "SMS":
                        if (_info.Alert == "Y")
                        {
                            if (_info.AlertType == "SMS")
                            {
                                if (userIDSMS.Length > 0)
                                {
                                    foreach (string mobile in userIDSMS.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                            smsbll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, _info.Title, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력   
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                if ((_info.Alert == "Y") && (_info.AlertType == "Messenger") && (userIDMOBILE.Length > 0))
                {
                    foreach (string userID in userIDMOBILE.Split(','))
                    {
                        if (userID != "")
                        {
                            bll.SendMobile(_info, userID.ToNumber<int>(), _scheduleID);
                        }
                    }
                }
            }
        }
        #endregion
        
        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _scheduleID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    if (!dal.Del(_companyID, _communityID, _scheduleID))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 삭제 (반복일정이 있었을 경우 - 이번 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_parentScheduleID"></param>
        /// <param name="_writeYM"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _scheduleID, int _parentScheduleID, string _writeYM)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    int ParentScheduleID = dal.DelUpd(_companyID, _communityID, _scheduleID, _parentScheduleID);

                    if (ParentScheduleID < 0)
                    {
                        return false;
                    }


                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentScheduleID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우 
                        // 2-1.반복 테이블 삭제 
                        if (!repdal.Del(_companyID, _communityID, _scheduleID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 이미 지정되어 있는 다른 반복일정이 있을 경우                              
                        if (_parentScheduleID != ParentScheduleID)
                        {
                            // 2-2.반복 테이블 업데이트
                            if (!repdal.Upd(_companyID, _communityID, _parentScheduleID, ParentScheduleID))
                            {
                                return false;
                            }                            
                        }
                    }
                    
                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del2", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 삭제 (반복일정이 있었을 경우 - 향후 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_parentScheduleID"></param>
        /// <param name="_writeYM"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _scheduleID, int _parentScheduleID, string _writeYM, string _startDate)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Schedule / Repeat 테이블 처리
                    int ParentScheduleID = ModRepeat(_companyID, _communityID, _parentScheduleID, _startDate);
                    if (ParentScheduleID < 0)
                    {
                        return false;
                    }                    

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del3", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 삭제 (반복일정이 있었을 경우 - 모든 일정 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_parentScheduleID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _communityID, int _parentScheduleID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.ScheduleID , Repeat 테이블 삭제
                    DAL.Schedule dal = new DAL.Schedule();
                    if (!dal.DelAll(_companyID, _communityID, _parentScheduleID))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelAll", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 변경 (반복일정에서 삭제) 향후 일정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_parentScheduleID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _communityID, int _parentScheduleID, string _startDate)
        {
            try
            {
                int ParentReservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Schedule /테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    ParentReservationID = dal.Del(_companyID, _communityID, _parentScheduleID, _startDate);

                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리 
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제 
                        if (!repdal.Del(_companyID, _communityID, _parentScheduleID))
                        {
                            return -1;
                        }
                    }

                    // ParentReservationID 값이 0보다 클때 굳이 값을 변경할 필요가 없음

                    ts.Complete();
                }

                return ParentReservationID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 스케줄 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_parentScheduleID"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _communityID, int _scheduleID, int _parentScheduleID)
        {
            try
            {
                int ParentScheduleID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Schedule 테이블 처리
                    DAL.Schedule dal = new DAL.Schedule();
                    ParentScheduleID = dal.DelUpd(_companyID, _communityID, _scheduleID, _parentScheduleID);

                    if (ParentScheduleID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리 
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentScheduleID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제 
                        if (!repdal.Del(_companyID, _communityID, _scheduleID))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (_parentScheduleID != ParentScheduleID)
                        {
                            // 2-2. Repeat 테이블 업데이트
                            if (!repdal.Upd(_companyID, _communityID, _parentScheduleID, ParentScheduleID))
                            {
                                return -1;
                            }                            
                        }
                    }

                    ts.Complete();
                }

                return ParentScheduleID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat", ex);
                return -1;
            }
        }
        #endregion 
    }
}
