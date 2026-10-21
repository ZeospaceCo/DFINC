using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Visit.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.RM.BE;
using Zeospace.DeskPlus.RM.BLL;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;

namespace Zeospace.DeskPlus.Visit.BLL
{
    public class Reserve : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 방문 예약  (반복일정 X)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_alOpen"></param>
        /// <returns></returns>
        public int Add(BE.ReserveInfo _info, Zeospace.DeskPlus.RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen)
        {
            try
            {
                int reserveID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    reserveID = dal.Add(_info);

                    if (reserveID > 0)
                    {
                        // 2. 공유설정
                        foreach (BE.OpenListInfo info in _alOpen)
                        {
                            DAL.OpenList dalOpen = new DAL.OpenList();
                            info.ReserveGroupID = reserveID;
                            if (!dalOpen.Add(info))
                            {
                                return -1;
                            }
                        }

                        // 3. 자원처리
                        if (_rmInfo.ArrListID != null)
                        {
                            string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                            string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                            string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                            Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                            for (int i = 0; i < arrListID.Length; i++)
                            {
                                _rmInfo.ListID = arrListID[i].ToNumber<int>();
                                _rmInfo.Qty = arrQty[i].ToNumber<int>();
                                _rmInfo.Usable = arrUsable[i].ToString();

                                // 3-1. 자원관리 처리
                                int tmpReservationID = rmBll.Add(_rmInfo, null);
                                if (tmpReservationID <= 0)
                                {
                                    return -1;
                                }

                                // 3-2. DP_VIS_RmList 처리
                                DAL.RmList dalRM = new DAL.RmList();
                                if (!dalRM.Add(_info.CompanyID, reserveID, tmpReservationID))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    // 4. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, reserveID))
                            {
                                return -1;
                            }
                        }
                    }


                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, reserveID);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }



                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

                return reserveID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 방문 예약 (반복일정 0)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_alOpen"></param>
        /// <param name="_al"></param>
        /// <param name="_rmal"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_rmrepInfo"></param>
        /// <returns></returns>
        public int Add(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen,
            List<BE.ReserveEtcInfo> _al, List<RM.BE.ReserveEtcInfo> _rmal, BE.RepeatInfo _repInfo, RM.BE.RepeatInfo _rmrepInfo)
        {
            try
            {
                int tmpVal = 0;
                string tmpReserveID = String.Empty;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 1.Reserve 테이블 처리
                        DAL.Reserve dal = new DAL.Reserve();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.ReserveGroupID = tmpVal;

                        int reserveID = dal.Add(_info);

                        tmpReserveID += reserveID + ",";

                        if (_info.ReserveGroupID == 0)
                        {
                            tmpVal = reserveID;
                        }

                        if (reserveID < 1)
                        {
                            return -1;
                        }
                    }

                    // 2. 공유설정
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        DAL.OpenList dalOpen = new DAL.OpenList();
                        info.ReserveGroupID = tmpVal;
                        if (!dalOpen.Add(info))
                        {
                            return -1;
                        }
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReserveGroupID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    // 4. 자원처리
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 2-1. 자원관리 처리
                            string tmpReservationID = rmBll.Add(_rmInfo, _rmal, _rmrepInfo, true, null);

                            if (tmpReservationID == "-1")
                            {
                                return -1;
                            }

                            // 2-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            string[] arrReserveID = tmpReserveID.TrimEnd(',').Split(',');
                            string[] arrReservationID = tmpReservationID.TrimEnd(',').Split(',');

                            for (int j = 0; j < arrReserveID.Length; j++)
                            {
                                if (!dalRM.Add(_info.CompanyID, arrReserveID[j].ToNumber<int>(), arrReservationID[j].ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal))
                            {
                                return -1;
                            }
                        }
                    }

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, tmpVal);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }



                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(Repeat)", ex);
                return -1;
            }
        }


        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 방문예약 수정  (반복일정 X) - 반복설정이 첨부터 없었을 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_alOpen"></param>
        /// <returns></returns>
        public bool Mod(BE.ReserveInfo _info, Zeospace.DeskPlus.RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    int reserveGroupID = dal.Mod(_info);

                    if (reserveGroupID < 0)
                    {
                        return false;
                    }

                    // 2. 공유설정
                    // 2-1. 공유자 삭제
                    DAL.OpenList dalOpen = new DAL.OpenList();
                    if (!dalOpen.Del(_info.CompanyID, _info.ReserveGroupID))
                    {
                        return false;
                    }

                    // 2-2. 공유자 추가
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        info.ReserveGroupID = _info.ReserveGroupID;
                        if (!dalOpen.Add(info))
                        {
                            return false;
                        }
                    }

                    // 3. 자원처리
                    // 3-1. 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                    DAL.RmList rmListDal = new DAL.RmList();
                    DataTableReader dtr = rmListDal.GetRmList(_info.CompanyID, _info.ReserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                // 3-2. 전에 등록되어 있는 자원 삭제 
                                if (!rmBll.DelVisit(_info.CompanyID, dtr["ReservationID"].ToString().ToNumber<int>()))
                                {
                                    return false;
                                }
                            }

                            // 3-3. RmList 테이블 삭제 
                            if (!rmListDal.DelAll(_info.CompanyID, _info.ReserveID))
                            {
                                return false;
                            }
                        }
                    }

                    // 3-4. 자원관리 테이블 처리
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 3-1. 자원관리 처리
                            int tmpReservationID = rmBll.Add(_rmInfo, null);
                            if (tmpReservationID <= 0)
                            {
                                return false;
                            }

                            // 3-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            if (!dalRM.Add(_info.CompanyID, _info.ReserveID, tmpReservationID))
                            {
                                return false;
                            }
                        }
                    }

                    // 4. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, _info.ReserveID))
                            {
                                return false;
                            }
                        }
                    }

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, _info.ReserveGroupID);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }



                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod3", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-14
        /// 작성목적 : 방문예약 수정  (반복일정 X) - 이번일정만 / 반복일정 미 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_alOpen"></param>
        /// <returns></returns>
        public bool Mod(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, int _reserveGroupID, List<BE.OpenListInfo> _alOpen)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    int reserveGroupID = dal.Mod(_info);

                    if (reserveGroupID < 0)
                    {
                        return false;
                    }

                    DAL.OpenList dalOpen = new DAL.OpenList();
                    DAL.Repeat repdal = new DAL.Repeat();
                    DAL.RmList rmListDal = new DAL.RmList();

                    if (reserveGroupID == 0)
                    {
                        // * 이미 지정되어 있는 다른 반복일정이 없을 경우 
                        // 2-1. 공유자 삭제                        
                        if (!dalOpen.Del(_info.CompanyID, _reserveGroupID))
                        {
                            return false;
                        }

                        // 2-2. 공유자 추가
                        foreach (BE.OpenListInfo info in _alOpen)
                        {
                            info.ReserveGroupID = _info.ReserveGroupID;
                            if (!dalOpen.Add(info))
                            {
                                return false;
                            }
                        }

                        // 3-1. 반복 테이블 삭제 
                        if (!repdal.Del(_info.CompanyID, _reserveGroupID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // * 이미 지정되어 있는 다른 반복일정이 있을 경우                              
                        if (_reserveGroupID != reserveGroupID)
                        {
                            // 2-1. 공유자 업데이트
                            if (!dalOpen.Upd(_info.CompanyID, _reserveGroupID, reserveGroupID))
                            {
                                return false;
                            }

                            // 2-2. 공유자 추가
                            foreach (BE.OpenListInfo info in _alOpen)
                            {
                                info.ReserveGroupID = _info.ReserveGroupID;
                                if (!dalOpen.Add(info))
                                {
                                    return false;
                                }
                            }

                            // 3-1.반복 테이블 업데이트
                            if (!repdal.Upd(_info.CompanyID, _reserveGroupID, reserveGroupID))
                            {
                                return false;
                            }

                            // 5-1. Attach 테이블 처리 
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.Mod(_info.CompanyID, _reserveGroupID, reserveGroupID, _info.WriteDateYM))
                            {
                                return false;
                            }
                        }
                    }

                    // 5. 자원처리
                    // 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                    DataTableReader dtr = rmListDal.GetRmList(_info.CompanyID, _info.ReserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                // 자원 삭제 
                                int delID = dtr["ReservationID"].ToString().ToNumber<int>();
                                if (!rmBll.Del(dtr["CompanyID"].ToString().ToNumber<int>(), delID, dtr["ParentReservationID"].ToString().ToNumber<int>(), _info.WriteDateYM))
                                {
                                    return false;
                                }

                                // RmList 테이블 삭제 
                                if (!rmListDal.Del(_info.CompanyID, delID))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // 자원관리 테이블 처리
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ParentReservationID = 0;
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 3-1. 자원관리 처리
                            int tmpReservationID = rmBll.Add(_rmInfo, null);
                            if (tmpReservationID <= 0)
                            {
                                return false;
                            }

                            // 3-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            if (!dalRM.Add(_info.CompanyID, _info.ReserveID, tmpReservationID))
                            {
                                return false;
                            }
                        }
                    }

                    /*
                    // 4. 자원처리
                    // 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();

                    string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                    string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                    string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');
                    string totalListID = String.Empty;

                    DataTableReader dtr = rmListDal.GetRmList(_info.CompanyID, _info.ReserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {   
                                string tmpListID = dtr["ListID"].ToString();
                                string tmpUse1 = "N";
                                int tmpRtnUse = 0;

                                for (int i = 0; i < arrListID.Length; i++)
                                {
                                    if (tmpListID == arrListID[i].ToString())
                                    {
                                        tmpUse1 = "Y";
                                        tmpRtnUse = i;
                                    }
                                }

                                if (tmpUse1 == "Y")
                                {
                                    _rmInfo.ParentReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                                    _rmInfo.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                                    _rmInfo.ListID = arrListID[tmpRtnUse].ToNumber<int>();
                                    _rmInfo.Qty = arrQty[tmpRtnUse].ToNumber<int>();
                                    _rmInfo.Usable = arrUsable[tmpRtnUse].ToString();

                                    // 자원 수정 
                                    if (!rmBll.Mod(_rmInfo, dtr["ParentReservationID"].ToString().ToNumber<int>()))
                                    {
                                        return false;
                                    }

                                    totalListID += arrListID[tmpRtnUse].ToNumber<int>() + ",";
                                }
                                else
                                {
                                    // 자원 삭제 
                                    int delID = dtr["ReservationID"].ToString().ToNumber<int>();
                                    if (!rmBll.Del(dtr["CompanyID"].ToString().ToNumber<int>(), delID, dtr["ParentReservationID"].ToString().ToNumber<int>(), _info.WriteDateYM))
                                    {
                                        return false;
                                    }

                                    // RmList 테이블 삭제 
                                    if (!rmListDal.Del(_info.CompanyID, delID))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    // 자원관리 테이블 처리
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrTotalListID = totalListID.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            string tmpUser2 = "N";

                            for (int j = 0; j < arrTotalListID.Length; j++)
                            {                                
                                if (arrTotalListID[j].ToString() == arrListID[i].ToString())
                                {
                                    tmpUser2 = "Y";
                                }
                            }

                            // 자원 등록 (새롭게 추가된 자원 추가)
                            if (tmpUser2 == "N")
                            {
                                _rmInfo.ParentReservationID = 0;
                                _rmInfo.ListID = arrListID[i].ToNumber<int>();
                                _rmInfo.Qty = arrQty[i].ToNumber<int>();
                                _rmInfo.Usable = arrUsable[i].ToString();

                                // 자원관리 처리
                                int tmpReservationID = rmBll.Add(_rmInfo);
                                if (tmpReservationID <= 0)
                                {
                                    return false;
                                }

                                // DP_VIS_RmList 처리
                                DAL.RmList dalRM = new DAL.RmList();
                                if (!dalRM.Add(_info.CompanyID, _info.ReserveID, tmpReservationID))
                                {
                                    return false;
                                }
                            }
                        }
                    }      
                    */

                    // 5-2. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, _info.ReserveGroupID))
                            {
                                return false;
                            }
                        }
                    }

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, _info.ReserveGroupID);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 수정  (반복일정 X) - 선택해서 들어간 날짜 이후로의 반복 일정은 삭제, 새로운 일정 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_alOpen"></param>
        /// <returns></returns>
        public int ModFuture(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, int _reserveGroupID, string _startDate, List<BE.OpenListInfo> _alOpen)
        {
            try
            {
                int reserveID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve / Repeat / OpenList / RMList / DP_RM..
                    int ReserveGroupID = ModRepeat(_info.CompanyID, _reserveGroupID, _startDate);
                    if (ReserveGroupID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    // 2. Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    reserveID = dal.Add(_info);

                    if (reserveID > 0)
                    {
                        // 2. 공유설정
                        foreach (BE.OpenListInfo info in _alOpen)
                        {
                            DAL.OpenList dalOpen = new DAL.OpenList();
                            info.ReserveGroupID = reserveID;
                            if (!dalOpen.Add(info))
                            {
                                return -1;
                            }
                        }

                        // 3. 자원처리
                        if (_rmInfo.ArrListID != null)
                        {
                            string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                            string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                            string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                            Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                            for (int i = 0; i < arrListID.Length; i++)
                            {
                                _rmInfo.ListID = arrListID[i].ToNumber<int>();
                                _rmInfo.Qty = arrQty[i].ToNumber<int>();
                                _rmInfo.Usable = arrUsable[i].ToString();

                                // 3-1. 자원관리 처리
                                int tmpReservationID = rmBll.Add(_rmInfo, null);
                                if (tmpReservationID <= 0)
                                {
                                    return -1;
                                }

                                // 3-2. DP_VIS_RmList 처리
                                DAL.RmList dalRM = new DAL.RmList();
                                if (!dalRM.Add(_info.CompanyID, reserveID, tmpReservationID))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    // 4. 이전 첨부파일 처리 
                    if (ReserveGroupID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우 
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _reserveGroupID, reserveID, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ReserveGroupID, reserveID, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, reserveID))
                            {
                                return -1;
                            }
                        }
                    }                   

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, reserveID);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion


                    ts.Complete();
                }

                return reserveID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFuture2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 수정  (반복일정 0) - 반복이 없었다가 생긴 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen,
            List<BE.ReserveEtcInfo> _al, List<RM.BE.ReserveEtcInfo> _rmal, BE.RepeatInfo _repInfo, RM.BE.RepeatInfo _rmrepInfo)
        {
            try
            {
                int tmpVal = 0;
                string tmpReserveID = String.Empty;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve  삭제
                    DAL.Reserve dal = new DAL.Reserve();
                    if (!dal.Del(_info.CompanyID, _info.ReserveID))
                    {
                        return -1;
                    }

                    // 2. 공유자 삭제
                    DAL.OpenList dalOpen = new DAL.OpenList();
                    if (!dalOpen.Del(_info.CompanyID, _info.ReserveGroupID))
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 3. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.ReserveGroupID = tmpVal;

                        int reserveID = dal.Add(_info);

                        tmpReserveID += reserveID + ",";
                        if (_info.ReserveGroupID == 0)
                        {
                            tmpVal = reserveID;
                        }

                        if (reserveID < 1)
                        {
                            return -1;
                        }
                    }

                    // 4. 공유설정
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        info.ReserveGroupID = tmpVal;
                        if (!dalOpen.Add(info))
                        {
                            return -1;
                        }
                    }

                    // 5. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReserveGroupID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    // 6. 자원처리
                    // 6-1. 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                    DAL.RmList rmListDal = new DAL.RmList();
                    DataTableReader dtr = rmListDal.GetRmList(_info.CompanyID, _info.ReserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                // 6-2. 전에 등록되어 있는 자원 삭제 
                                if (!rmBll.DelVisit(_info.CompanyID, dtr["ReservationID"].ToString().ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }

                            // 6-3. RmList 테이블 삭제 
                            if (!rmListDal.DelAll(_info.CompanyID, _info.ReserveID))
                            {
                                return -1;
                            }
                        }
                    }

                    // 6-4. 자원처리
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 2-1. 자원관리 처리
                            string tmpReservationID = rmBll.Add(_rmInfo, _rmal, _rmrepInfo, true, null);

                            if (tmpReservationID == "-1")
                            {
                                return -1;
                            }

                            // 2-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            string[] arrReserveID = tmpReserveID.TrimEnd(',').Split(',');
                            string[] arrReservationID = tmpReservationID.TrimEnd(',').Split(',');

                            for (int j = 0; j < arrReserveID.Length; j++)
                            {
                                if (!dalRM.Add(_info.CompanyID, arrReserveID[j].ToNumber<int>(), arrReservationID[j].ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 7. 이전 첨부파일 처리                     
                    BLL.Attach attbll = new BLL.Attach();
                    if (!attbll.ModRepeat(_info.CompanyID, _info.ReserveID, tmpVal, _info.WriteDateYM))
                    {
                        return -1;
                    }

                    // 8. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal))
                            {
                                return -1;
                            }
                        }
                    }                   

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, tmpVal);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

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
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 수정  (반복일정 0) - 이번일정만 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen, List<BE.ReserveEtcInfo> _al
            , List<RM.BE.ReserveEtcInfo> _rmal, BE.RepeatInfo _repInfo, RM.BE.RepeatInfo _rmrepInfo, int _reserveGroupID)
        {
            try
            {
                int tmpVal = 0;
                string tmpReserveID = String.Empty;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / Repeat / Attach 테이블 변경 , 첨부파일 이동
                    int ReserveGroupID = ModRepeat(_info.CompanyID, _info.ReserveID, _info.ReserveGroupID, _info.WriteDateYM);
                    if (ReserveGroupID < 0)
                    {
                        return -1;
                    }

                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        DAL.Reserve dal = new DAL.Reserve();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.ReserveGroupID = tmpVal;

                        int reserveID = dal.Add(_info);

                        tmpReserveID += reserveID + ",";

                        if (_info.ReserveGroupID == 0)
                        {
                            tmpVal = reserveID;
                        }

                        if (reserveID < 1)
                        {
                            return -1;
                        }
                    }

                    DAL.OpenList dalOpen = new DAL.OpenList();
                    DAL.Repeat repdal = new DAL.Repeat();
                    DAL.RmList rmListDal = new DAL.RmList();

                    // 3. 공유자 추가
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        info.ReserveGroupID = tmpVal;
                        if (!dalOpen.Add(info))
                        {
                            return -1;
                        }
                    }

                    // 4. Repeat 테이블 처리
                    _repInfo.ReserveGroupID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    // 5. 자원처리
                    // 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();

                    string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                    string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                    string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                    DataTableReader dtr = rmListDal.GetRmList(_info.CompanyID, _info.ReserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                // 자원 삭제 
                                int delID = dtr["ReservationID"].ToString().ToNumber<int>();
                                if (!rmBll.Del(dtr["CompanyID"].ToString().ToNumber<int>(), delID, dtr["ParentReservationID"].ToString().ToNumber<int>(), _info.WriteDateYM))
                                {
                                    return -1;
                                }

                                // RmList 테이블 삭제 
                                if (!rmListDal.Del(_info.CompanyID, delID))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 자원관리 테이블 처리
                    if (_rmInfo.ArrListID != null)
                    {
                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ParentReservationID = 0;
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 자원관리 처리
                            string tmpReservationID = rmBll.Add(_rmInfo, _rmal, _rmrepInfo, true, null);

                            if (tmpReservationID == "-1")
                            {
                                return -1;
                            }

                            // DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            string[] arrReserveID = tmpReserveID.TrimEnd(',').Split(',');
                            string[] arrReservationID = tmpReservationID.TrimEnd(',').Split(',');

                            for (int j = 0; j < arrReserveID.Length; j++)
                            {
                                if (!dalRM.Add(_info.CompanyID, arrReserveID[j].ToNumber<int>(), arrReservationID[j].ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리 
                    if (ReserveGroupID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우 
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _reserveGroupID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ReserveGroupID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }


                    // 7. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal))
                            {
                                return -1;
                            }
                        }
                    }                   


                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, tmpVal);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 수정  (반복일정 0) - 향후 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_alOpen"></param>
        /// <param name="_al"></param>
        /// <param name="_rmal"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_rmrepInfo"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModFuture(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen, List<BE.ReserveEtcInfo> _al
            , List<RM.BE.ReserveEtcInfo> _rmal, BE.RepeatInfo _repInfo, RM.BE.RepeatInfo _rmrepInfo, int _reserveGroupID, string _startDate)
        {
            try
            {
                int tmpVal = 0;
                string tmpReserveID = String.Empty;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve / Repeat / OpenList / RMList / DP_RM..
                    int ReserveGroupID = ModRepeat(_info.CompanyID, _info.ReserveGroupID, _startDate);
                    if (ReserveGroupID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        DAL.Reserve dal = new DAL.Reserve();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.ReserveGroupID = tmpVal;

                        int reserveID = dal.Add(_info);

                        tmpReserveID += reserveID + ",";

                        if (_info.ReserveGroupID == 0)
                        {
                            tmpVal = reserveID;
                        }

                        if (reserveID < 1)
                        {
                            return -1;
                        }
                    }

                    // 3. 공유설정
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        DAL.OpenList dalOpen = new DAL.OpenList();
                        info.ReserveGroupID = tmpVal;
                        if (!dalOpen.Add(info))
                        {
                            return -1;
                        }
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReserveGroupID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    // 5. 자원처리
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 2-1. 자원관리 처리
                            string tmpReservationID = rmBll.Add(_rmInfo, _rmal, _rmrepInfo, true, null);

                            if (tmpReservationID == "-1")
                            {
                                return -1;
                            }

                            // 2-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            string[] arrReserveID = tmpReserveID.TrimEnd(',').Split(',');
                            string[] arrReservationID = tmpReservationID.TrimEnd(',').Split(',');

                            for (int j = 0; j < arrReserveID.Length; j++)
                            {
                                if (!dalRM.Add(_info.CompanyID, arrReserveID[j].ToNumber<int>(), arrReservationID[j].ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리 
                    if (ReserveGroupID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우 
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _reserveGroupID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ReserveGroupID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }


                    // 7. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal))
                            {
                                return -1;
                            }
                        }
                    }                    

                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, tmpVal);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 수정  (반복일정 0) - 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_rmInfo"></param>
        /// <param name="_alOpen"></param>
        /// <param name="_al"></param>
        /// <param name="_rmal"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_rmrepInfo"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public int ModAll(BE.ReserveInfo _info, RM.BE.ReserveInfo _rmInfo, List<BE.OpenListInfo> _alOpen, List<BE.ReserveEtcInfo> _al
            , List<RM.BE.ReserveEtcInfo> _rmal, BE.RepeatInfo _repInfo, RM.BE.RepeatInfo _rmrepInfo, int _reserveGroupID)
        {
            try
            {
                int tmpVal = 0;
                string tmpReserveID = String.Empty;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 0. RM 처리
                    DAL.RmList rmListDal = new DAL.RmList();
                    RM.DAL.Reverse rmDal = new RM.DAL.Reverse();

                    DataTableReader dtr = rmListDal.GetRmAllList(_info.CompanyID, _reserveGroupID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (!rmDal.DelAll(_info.CompanyID, dtr["ParentReservationID"].ToString().ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // RMList 테이블 처리 
                    if (!rmListDal.DelAllGroupID(_info.CompanyID, _reserveGroupID))
                    {
                        return -1;
                    }

                    // 1. Reserve / Repeat / OpenList 
                    DAL.Reserve dal = new DAL.Reserve();
                    if (!dal.DelAll(_info.CompanyID, _info.ReserveGroupID))
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.ReserveGroupID = tmpVal;

                        int reserveID = dal.Add(_info);

                        tmpReserveID += reserveID + ",";

                        if (_info.ReserveGroupID == 0)
                        {
                            tmpVal = reserveID;
                        }

                        if (reserveID < 1)
                        {
                            return -1;
                        }
                    }

                    // 3. 공유설정
                    foreach (BE.OpenListInfo info in _alOpen)
                    {
                        DAL.OpenList dalOpen = new DAL.OpenList();
                        info.ReserveGroupID = tmpVal;
                        if (!dalOpen.Add(info))
                        {
                            return -1;
                        }
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReserveGroupID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    // 5. 자원처리         
                    RM.BLL.Reserve rmBll = new RM.BLL.Reserve();
                    if (_rmInfo.ArrListID != null)
                    {
                        string[] arrListID = _rmInfo.ArrListID.Trim().TrimEnd(',').Split(',');
                        string[] arrQty = _rmInfo.ArrQty.Trim().TrimEnd(',').Split(',');
                        string[] arrUsable = _rmInfo.ArrUsable.Trim().TrimEnd(',').Split(',');

                        for (int i = 0; i < arrListID.Length; i++)
                        {
                            _rmInfo.ListID = arrListID[i].ToNumber<int>();
                            _rmInfo.Qty = arrQty[i].ToNumber<int>();
                            _rmInfo.Usable = arrUsable[i].ToString();

                            // 5-1. 자원관리 처리
                            string tmpReservationID = rmBll.Add(_rmInfo, _rmal, _rmrepInfo, true, null);

                            if (tmpReservationID == "-1")
                            {
                                return -1;
                            }

                            // 5-2. DP_VIS_RmList 처리
                            DAL.RmList dalRM = new DAL.RmList();
                            string[] arrReserveID = tmpReserveID.TrimEnd(',').Split(',');
                            string[] arrReservationID = tmpReservationID.TrimEnd(',').Split(',');

                            for (int j = 0; j < arrReserveID.Length; j++)
                            {
                                if (!dalRM.Add(_info.CompanyID, arrReserveID[j].ToNumber<int>(), arrReservationID[j].ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리                     
                    BLL.Attach attbll = new BLL.Attach();
                    if (!attbll.ModRepeat(_info.CompanyID, _reserveGroupID, tmpVal, _info.WriteDateYM))
                    {
                        return -1;
                    }


                    // 7. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal))
                            {
                                return -1;
                            }
                        }
                    }                    


                    #region 알람
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarm(_info.CompanyID, _info.WriteUserID, tmpVal);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }


                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(_info, userID.ToNumber<int>(), "0");
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(_info, mailReceiver, "0");
                                }
                            }

                            // SMS 알람
                            if (alarm == "SMS")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    string smsContent = AlarmSMSTemplate(_info.WriteUserName, _info.Subject, _info.PreLang, "0");

                                    foreach (string mobile in mobileReceiver.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS bll = new Common.BLL.SMS();
                                            bll.SendSMS(_info.CompanyID, _info.WriteUserID, _info.SMSLoginID, smsContent, false, mobile);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    ts.Complete();
                }

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModAll", ex);
                return -1;
            }
        }

        #endregion

        #region DEL()


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _reserveID, int _reserveGroupID, string _writeDateYM)
        {
            try
            {
                int ReserveGroupID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve  테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    ReserveGroupID = dal.DelUpd(_companyID, _reserveID, _reserveGroupID);

                    if (ReserveGroupID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리 
                    DAL.OpenList opendal = new DAL.OpenList();
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ReserveGroupID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제 
                        if (!repdal.Del(_companyID, _reserveGroupID))
                        {
                            return -1;
                        }

                        // 3. OpenList 테이블 처리 
                        if (!opendal.Del(_companyID, _reserveGroupID))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (_reserveGroupID != ReserveGroupID)
                        {
                            // 2-2. Repeat 테이블 업데이트
                            if (!repdal.Upd(_companyID, _reserveGroupID, ReserveGroupID))
                            {
                                return -1;
                            }

                            // 3. OpenList 테이블 업데이트
                            if (!opendal.Upd(_companyID, _reserveGroupID, ReserveGroupID))
                            {
                                return -1;
                            }

                            // 4. 첨부파일 처리
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.ModRepeat(_companyID, _reserveGroupID, ReserveGroupID, _writeDateYM))
                            {
                                return -1;
                            }
                        }
                    }

                    ts.Complete();
                }

                return ReserveGroupID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 방문 예약 변경 (반복일정에서 삭제) 향후 일정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _reserveGroupID, string _startDate)
        {
            try
            {
                int ReserveGroupID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 0. 자원 처리
                    DAL.RmList rmListDal = new DAL.RmList();
                    RM.BLL.Reserve rmBll = new RM.BLL.Reserve();

                    DataTableReader dtr = rmListDal.GetRmReserveList(_companyID, _reserveGroupID, _startDate);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                int ParentReservationID = rmBll.ModRepeat(_companyID, dtr["ParentReservationID"].ToString().ToNumber<int>(), _startDate);
                                if (ParentReservationID < 0)
                                {
                                    return -1;
                                }

                                // RMList 테이블 처리 
                                if (!rmListDal.Del(_companyID, dtr["ReservationID"].ToString().ToNumber<int>()))
                                {
                                    return -1;
                                }
                            }
                        }
                    }

                    // 1. Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    ReserveGroupID = dal.Del(_companyID, _reserveGroupID, _startDate);

                    if (ReserveGroupID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리 
                    DAL.Repeat repdal = new DAL.Repeat();
                    DAL.OpenList opendal = new DAL.OpenList();

                    if (ReserveGroupID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제 
                        if (!repdal.Del(_companyID, _reserveGroupID))
                        {
                            return -1;
                        }

                        // 3. OpenList 테이블 처리 
                        if (!opendal.Del(_companyID, _reserveGroupID))
                        {
                            return -1;
                        }
                    }

                    // ParentReservationID 값이 0보다 클때 굳이 값을 변경할 필요가 없음

                    ts.Complete();
                }

                return ReserveGroupID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    if (!dal.Del(_companyID, _reserveID))
                    {
                        return false;
                    }

                    // 2.OpenList 테이블 처리
                    DAL.OpenList opendal = new DAL.OpenList();
                    if (!opendal.Del(_companyID, _reserveID))
                    {
                        return false;
                    }

                    // 3. 자원 처리 
                    RM.BLL.Reserve rmBll = new RM.BLL.Reserve();
                    DAL.RmList rmListDal = new DAL.RmList();
                    DataTableReader dtr = rmListDal.GetRmListByReserveID(_companyID, _reserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (!rmBll.Del(_companyID, dtr["ReservationID"].ToString().ToNumber<int>()))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // 4. rmList 테이블 처리 
                    if (!rmListDal.DelAll(_companyID, _reserveID))
                    {
                        return false;
                    }

                    // 5. 첨부파일 처리    
                    BLL.Attach bll = new BLL.Attach();
                    if (!bll.Del(_companyID, _reserveID))
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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제 (반복일정이 있었을 경우 - 이번 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_writeYM"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveID, int _reserveGroupID, string _writeYM)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 0. 자원 처리
                    // 전에 등록되어 있는 자원 정보 RmList 테이블에서 가져오기 
                    DAL.RmList rmListDal = new DAL.RmList();
                    Zeospace.DeskPlus.RM.BLL.Reserve rmBll = new Zeospace.DeskPlus.RM.BLL.Reserve();
                    DataTableReader dtr = rmListDal.GetRmList(_companyID, _reserveID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                // 자원 삭제 
                                int delID = dtr["ReservationID"].ToString().ToNumber<int>();
                                if (!rmBll.Del(dtr["CompanyID"].ToString().ToNumber<int>(), delID, dtr["ParentReservationID"].ToString().ToNumber<int>(), _writeYM))
                                {
                                    return false;
                                }

                                // RmList 테이블 삭제 
                                if (!rmListDal.Del(_companyID, delID))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // 1.Reserve 테이블 처리
                    DAL.Reserve dal = new DAL.Reserve();
                    int ReserveGroupID = dal.DelUpd(_companyID, _reserveID, _reserveGroupID);

                    if (ReserveGroupID < 0)
                    {
                        return false;
                    }


                    // 2.  Repeat 테이블 처리
                    DAL.OpenList opendal = new DAL.OpenList();
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ReserveGroupID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우 
                        // 3-1.반복 테이블 삭제 
                        if (!repdal.Del(_companyID, _reserveID))
                        {
                            return false;
                        }

                        // 3. OpenList 테이블 처리 
                        if (!opendal.Del(_companyID, _reserveGroupID))
                        {
                            return false;
                        }

                        // 4. 첨부파일 처리    
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Del(_companyID, _reserveID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 이미 지정되어 있는 다른 반복일정이 있을 경우                              
                        if (_reserveGroupID != ReserveGroupID)
                        {
                            // 3-2.반복 테이블 업데이트
                            if (!repdal.Upd(_companyID, _reserveGroupID, ReserveGroupID))
                            {
                                return false;
                            }

                            // 4. OpenList 테이블 업데이트
                            if (!opendal.Upd(_companyID, _reserveGroupID, ReserveGroupID))
                            {
                                return false;
                            }

                            // 5. Attach 테이블 처리 
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.ModRepeat(_companyID, _reserveGroupID, ReserveGroupID, _writeYM))
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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제 (반복일정이 있었을 경우 - 향후 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <param name="_writeYM"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reserveID, int _reserveGroupID, string _writeYM, string _startDate)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / OpenList / Repeat 테이블 처리
                    int ReserveGroupID = ModRepeat(_companyID, _reserveGroupID, _startDate);
                    if (ReserveGroupID < 0)
                    {
                        return false;
                    }

                    // 2. 첨부파일 처리    
                    if (ReserveGroupID == 0)
                    {
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Del(_companyID, _reserveID))
                        {
                            return false;
                        }
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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제 (반복일정이 있었을 경우 - 모든 일정 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reserveGroupID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _reserveGroupID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 0. RM 처리
                    DAL.RmList rmListDal = new DAL.RmList();
                    RM.DAL.Reverse rmDal = new RM.DAL.Reverse();

                    DataTableReader dtr = rmListDal.GetRmAllList(_companyID, _reserveGroupID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (!rmDal.DelAll(_companyID, dtr["ParentReservationID"].ToString().ToNumber<int>()))
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    // RMList 테이블 처리 
                    if (!rmListDal.DelAllGroupID(_companyID, _reserveGroupID))
                    {
                        return false;
                    }

                    // 1.Reserve , OpenList,  Repeat 테이블 삭제
                    DAL.Reserve dal = new DAL.Reserve();
                    if (!dal.DelAll(_companyID, _reserveGroupID))
                    {
                        return false;
                    }

                    // 2. 첨부파일 처리
                    BLL.Attach bll = new BLL.Attach();
                    if (!bll.Del(_companyID, _reserveGroupID))
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

        #endregion


        #region 알람

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_visitType"></param>
        /// <returns></returns>
        public bool SendAlarm(BE.ReserveInfo _info, int _receiverID, string _visitType)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_info, _info.PreLang, "MESSENGER", _visitType);
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.VISIT, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Visit"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 메일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_mailReceiver"></param>
        /// <returns></returns>
        public bool SendMail(BE.ReserveInfo _info, string _mailReceiver, string _visitType)
        {
            // 받는 사람
            Hashtable htMailTo = new Hashtable();

            _mailReceiver = _mailReceiver.TrimEnd(',');

            foreach (string mail in _mailReceiver.Split(','))
            {
                htMailTo.Add(mail, mail);
            }

            MailDataInfo mailInfo = new MailDataInfo();
            mailInfo.To = htMailTo;
            mailInfo.From = _info.WriteUserName + "<" + _info.Mail + ">";

            string[] arrAlarm = AlarmTemplate(_info, _info.PreLang, "MAIL", _visitType);
            if (arrAlarm == null) return false;

            mailInfo.Subject = arrAlarm[0];
            mailInfo.Contents = arrAlarm[1];

            SendMail sendMail = new SendMail();
            return sendMail.Send(mailInfo);
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 알람 내용 템플릿
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preLang"></param>
        /// <param name="_gubun"></param>
        /// <param name="_visitType"></param>
        /// <returns></returns>
        private string[] AlarmTemplate(BE.ReserveInfo _info, string _preLang, string _gubun, string _visitType)
        {
            try
            {
                XmlDocument confirmXml = null;
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Visit/App_Data/" + _preLang + ".xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                string[] arrRtnval = new string[2];
                arrRtnval[0] = tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/SUBJ" + _visitType).InnerText + _info.Subject.ToReverseXSS();
                arrRtnval[1] = (tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML" + _visitType) != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML" + _visitType).InnerText) : "";
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", _info.Subject.ToReverseXSS());          //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$Name$", _info.WriteUserName);          //예약자
                arrRtnval[1] = arrRtnval[1].Replace("$StartDate$", _info.StartDate.ToString("yyyy-MM-dd")); //시작년월일
                arrRtnval[1] = arrRtnval[1].Replace("$EndDate$", _info.EndDate.ToString("yyyy-MM-dd")); //종료시분초
                arrRtnval[1] = arrRtnval[1].Replace("$StartTime$", _info.StartDate.ToString("HH:mm")); //시작년월일
                arrRtnval[1] = arrRtnval[1].Replace("$EndTime$", _info.EndDate.ToString("HH:mm")); //종료시분초

                return arrRtnval;   // [0] : Subj / [1] : Content

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Visit.AlarmTemplate : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : SMS알람 내용 템플릿
        /// </summary>
        /// <param name="_writeUserName"></param>
        /// <param name="_subject"></param>
        /// <param name="_preLang"></param>
        /// <param name="_visitType"></param>
        /// <returns></returns>
        private string AlarmSMSTemplate(string _writeUserName, string _subject, string _preLang, string _visitType)
        {
            string contHtml = "";

            try
            {
                XmlDocument confirmXml = null;
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Visit/App_Data/" + _preLang + ".xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                contHtml = (tmpXmlNode.SelectSingleNode("/ALARM/SMS/ContentHTML" + _visitType) != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ALARM/SMS/ContentHTML" + _visitType).InnerText) : "";

                contHtml = contHtml.Replace("$Name$", _writeUserName);
                contHtml = contHtml.Replace("$VName$", _subject);

            }
            catch { }

            return contHtml;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 상태값 변경시에 알람처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_reserveID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool AlarmVisitType(int _companyID, int _userID, int _reserveID, string _visitType, string _preLang)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Reserve dal = new DAL.Reserve();
                    DataTableReader dtr = dal.GetList(_companyID, _reserveID);

                    BE.ReserveInfo info = new BE.ReserveInfo();

                    info.PreLang = _preLang;

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                                info.Subject = dtr["Subject"].ToString();
                                info.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                                info.EndDate = DateTime.Parse(dtr["EndDate"].ToString());
                                info.WriteUserName = dtr["WriteUserName"].ToString();
                                info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            }
                        }
                    }

                    DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "VisitAlarm");

                    string tmAlarm = "";
                    for (int i = 0; i < dvAlarm.Count; i++)
                    {
                        if (dvAlarm[i]["ActiveCode"].ToString() == "1")
                        {
                            tmAlarm += dvAlarm[i]["CodeID"].ToString() + ",";
                        }
                    }

                    string userIDReceiver = ""; //알람받을대상자의 UserID                    
                    string mailReceiver = "";        //알람받을대상자의 메일                    
                    string mobileReceiver = "";      //알람받을핸드폰번호
                    ArrayList Receiver = new ArrayList();

                    // 알람 설정이 있을 경우
                    if (tmAlarm != "")
                    {
                        DAL.Share sharedal = new DAL.Share();
                        DataTableReader dtrUser = sharedal.GetAlarmVisitType(info.CompanyID, _userID, _reserveID);

                        if (dtrUser != null)
                        {
                            if (dtrUser.HasRows)
                            {
                                while (dtrUser.Read())
                                {
                                    if (!Receiver.Contains(dtrUser["UserID"].ToString()))
                                    {
                                        userIDReceiver += dtrUser["UserID"].ToString() + ",";
                                        mailReceiver += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        mobileReceiver += dtrUser["MobilePhone"].ToString() + ",";
                                    }
                                }
                            }
                        }



                        foreach (string alarm in tmAlarm.Split(','))
                        {
                            // 메신저 알람 
                            if (alarm == "Messenger")
                            {
                                if (userIDReceiver.Length > 0)
                                {
                                    foreach (string userID in userIDReceiver.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            SendAlarm(info, userID.ToNumber<int>(), _visitType);
                                        }
                                    }
                                }
                            }

                            // 메일
                            if (alarm == "Mail")
                            {
                                if (mailReceiver.Length > 0)
                                {
                                    SendMail(info, mailReceiver, _visitType);
                                }
                            }

                            // SMS 알람 생략

                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod3", ex);
                return false;
            }
        }

        #endregion
    }
}
