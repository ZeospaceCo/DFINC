using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;

using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class Schedule : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        public Schedule()
        { }


        #region Add
        public void ScheduleInsert(Pim.BE.ScheduleInfo _info, BE.RepeatScheduleInfo _rinfo,string _shareinfo)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                //스케줄 내용
                Pim.DAL.Schedule bll = new DAL.Schedule();
                int scheduleid = bll.Add(_info);

                //첨부파일
                DAL.Attach attachdal = new DAL.Attach();
                attachdal.AddList(_info.CompanyID, scheduleid, _info.HiddenSpoolPath);  
                
                //공유설정
                if (_shareinfo.Length > 0)
                {
                    BE.ScheShareListInfo sdata = new BE.ScheShareListInfo();
                    DAL.ScheShareList biz = new DAL.ScheShareList();
                    sdata.CompanyID = _info.CompanyID;
                    sdata.ScheduleID = scheduleid;
                    sdata.UserID = 0;
                    sdata.DeptID = 0;
                    sdata.UserMail = string.Empty;
                    sdata.UserName = string.Empty;

                    string strTemp = _shareinfo;
                    if (strTemp.Length > 0)
                    {
                        string[] arVal = strTemp.Split(',');

                        foreach (string _be in arVal)
                        {


                            string[] arSource = _be.Split('$');
                            if (arSource.Length > 1)
                            {
                                if (arSource[0].Equals("G"))
                                {
                                    //그룹
                                    sdata.UserID = 0;
                                    sdata.DeptID = int.Parse(arSource[1]);
                                    sdata.UserName = arSource[2];
                                }
                                else if (arSource[0].Equals("M"))
                                {
                                    //유저공유
                                    sdata.UserID = int.Parse(arSource[1]);
                                    sdata.DeptID = 0;
                                    sdata.UserName = arSource[2];
                                }


                                biz.AddScheShareLis(sdata);
                            }
                        }
                    }
                }

                //반복설정
                #region 반복
                if (_rinfo.RepeatType.Length > 0)
                {
                    BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                    BE.RepeatScheduleInfo RepInfo = new BE.RepeatScheduleInfo();
                    DAL.Display bizDis = new DAL.Display();
                    DAL.RepeatSchedule bizRep = new DAL.RepeatSchedule();
                    
                    BLL.RepeatScheduleProc bizRepeatBiz = new BLL.RepeatScheduleProc(_rinfo);

                    switch (_rinfo.RepeatType)
                    {
                        case "D":
                            bizRepeatBiz.RepeatDate();
                            break;
                        case "W":
                            bizRepeatBiz.CalcWeekDayCount();
                            break;
                        case "M":
                            // this.Option2 = 0~5   날, 평일,주말
                            //this.Option1 =1~5   첫째 둘째 
                            if (_rinfo.MaxReptCnt > 0)
                            {
                                /// 매달 특정일에 회수 제한 
                                if (_rinfo.SelectType == "1")
                                {
                                    bizRepeatBiz.calcMonth1RepetitionByCount();
                                }
                                else
                                {
                                    //매월 반복 
                                    bizRepeatBiz.calcMonth2RepetitionByCount();
                                }
                            }
                            else
                            {
                                if (_rinfo.SelectType == "1")
                                {

                                    /// 매월 반복 - 특정일까지
                                    bizRepeatBiz.calcMonth1RepetitionByTillDate();
                                }
                                else
                                {
                                    //매월 반복 - 특정일까지
                                    bizRepeatBiz.calcMonth2RepetitionByTillDate();
                                }
                            }
                            break;
                    }

                    List<BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();
                    foreach (BE.DisplayInfo _be in listobj)
                    {
                        _be.CompanyID = _info.CompanyID;
                        _be.ScheduleID = scheduleid;
                        _be.UserID = _info.UserID;
                        _be.EndDate = _be.EndDate;
                        if (_be.StartDate == _be.EndDate)
                        {
                            _be.EndDate = _be.EndDate.AddHours(1);
                        }

                        bizDis.AddDisplay(_be);
                    }
                    _rinfo.EndDate = _rinfo.R_EndDate;//DateTime.Parse(strE1);
                    if (_rinfo.StartDate == _rinfo.EndDate)
                    {
                        _rinfo.EndDate = _rinfo.EndDate.AddHours(1);
                    }
                    bizRep.AddRepeatSchedule(_rinfo);
                }
                else
                {
                    BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                    DAL.Display bizDis = new DAL.Display();
                    DisInfo.CompanyID = _info.CompanyID;
                    DisInfo.UserID = _info.UserID;
                    DisInfo.StartDate = _info.StartTime;
                    DisInfo.EndDate = _info.EndTime;
                    if (DisInfo.StartDate == DisInfo.EndDate)
                    {
                        DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                    }
                    DisInfo.ScheduleID = scheduleid;
                    //DisInfo.
                    bizDis.AddDisplay(DisInfo);
                }
                #endregion
            }

        }


        #endregion 

        #region MOD
        #endregion

        #region Del
        #endregion

        #region alarm

        public void SendAlarm(BE.ScheduleInfo _info, int _scheduleID, int _receiverID, Hashtable attach, int _displayID)
        {
            #region 로직
            #endregion

            #region 소스

            if (_info.AlertType == "M") //메신저     
            {
                string AlarmPims = string.Empty;
                string AlarmMessenger = string.Empty;

                if (_receiverID > 0)
                {
                    Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                    List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_info.CompanyID, _receiverID);

                    if (listUser != null)
                    {
                        foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                        {
                            AlarmPims = infoUser.AlarmPims;
                        }
                    }

                    if (AlarmPims.IndexOf("1-").Equals(0))
                    {
                        AlarmMessenger = (AlarmPims.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                    }
                    else
                    {
                        AlarmMessenger = "0";
                    }
                }

                if (AlarmMessenger.Equals("1"))
                {
                    SendMessenger(_info, _receiverID, _scheduleID, _displayID);
                }
            }
            else if (_info.AlertType == "E") //메일        
            {
                SendMail(_info, attach);
            }
            else if (_info.AlertType == "B") //모바일    
            {
                // 모바일사용체크
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    SendMobile(_info, _receiverID, _scheduleID, _displayID);
                }                
            }

            #endregion
       
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ScheduleInfo _info, int _receiverID, int _scheduleID, int _displayID)
        {
            Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = string.Empty;//_info.AlertTargetAddress; email 아님
            alarmInfo.SenderPhone = string.Empty; //_info.AlertTargetAddress;sms개발중
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = _info.UserName;
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _info.StartTime.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _info.StartTime + "~" + _info.EndTime + " " + _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);
            alarmTargetURL = alarmTargetURL + "?" + "P=" + _info.Parameterstring;
            alarmInfo.TargetUrl = alarmTargetURL;            

            alarmInfo.StartDate = _info.StartTime.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _info.StartTime.AddMinutes(-_info.AlertTime);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;
            alarmInfo.ScheduleID = _scheduleID;
            alarmInfo.DisplayID = _displayID;

            return biz.Add(alarmInfo, AlarmEnum.Cool.ToString());
        }

        /// <summary>
        /// 모바일
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        /// <returns></returns>
        private bool SendMobile(BE.ScheduleInfo _info, int _receiverID, int _scheduleID, int _displayID)
        {
            Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = string.Empty;//_info.AlertTargetAddress; email 아님
            alarmInfo.SenderPhone = string.Empty; //_info.AlertTargetAddress;sms개발중
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = _info.UserName;
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _info.StartTime.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _info.StartTime + "~" + _info.EndTime + " " + _info.Title.ToReverseXSS();
            alarmInfo.Content = "";

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);
            alarmTargetURL = alarmTargetURL + "?" + "P=" + _info.Parameterstring;
            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Pims/AlarmView.aspx?p=" + _info.Parameterstring;

            alarmInfo.StartDate = _info.StartTime.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _info.StartTime.AddMinutes(-_info.AlertTime);

            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;
            alarmInfo.ScheduleID = _scheduleID;
            alarmInfo.DisplayID = _displayID;

            return biz.Add(alarmInfo, AlarmEnum.Cool.ToString());
        }

        /// <summary>
        /// E-MAIL
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="attach"></param>
        private void SendMail(BE.ScheduleInfo _info, Hashtable attach)
        {
            Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailbiz = new Common.BLL.MailZettaFactory();

            mailbiz.CreateMHT(_info.SenderMail, _info.UserName, _info.AlertTargetAddress, "", "", _info.Title, _info.Content, attach, "", false);
        }
        #endregion

    }
}
