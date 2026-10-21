using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Configuration;
using Zeospace.DeskPlusMobile.Web.Messenger.BE;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Messenger.BLL
{
    public class Alarm
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-13
        /// 작성목적 : 알람 발송
        /// </summary>
        /// <param name="_alarmInfo"></param>
        /// <param name="_alarmType"></param>
        /// <returns></returns>
        public bool Add(AlarmInfo _alarmInfo)
        {
            if (_alarmInfo == null)
            {
                return false;
            }
            else
            {
                string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                string mobileDOMAIN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");

                Data.Alarm ialarm = new Zeospace.DeskPlusMobile.Web.Messenger.Data.Alarm();
                string tmpInfo = ialarm.GetLoginIDPwd(_alarmInfo.CompanyID, _alarmInfo.ReceiverID);

                string receiverLoginID = (tmpInfo.Split('^')[1] != null) ? tmpInfo.Split('^')[0] : "";
                string receiverPwd = (tmpInfo.Split('^')[0] != null) ? tmpInfo.Split('^')[1] : "";

                _alarmInfo.TargetUrl = httpType + fullDomain + "/DeskPlusEIP/Site/Auth/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                    + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&tURL=" + _alarmInfo.TargetUrl;

                _alarmInfo.RandomStr = System.Guid.NewGuid().ToString().Substring(0, 7);

                if (!String.IsNullOrEmpty(_alarmInfo.TargetMobileUrl))
                {
                    _alarmInfo.TargetMobileUrl = httpType + mobileDOMAIN + "/DeskPlusMobile/Portal/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                       + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&ran=" + _alarmInfo.RandomStr + "&tURL=" + _alarmInfo.TargetMobileUrl;
                }

                _alarmInfo.AlarmUrl = "";

                if (String.IsNullOrEmpty(_alarmInfo.MsgSendID.ToString()))
                {
                    _alarmInfo.MsgSendID = 0;
                }

                if (String.IsNullOrEmpty(_alarmInfo.ScheduleID.ToString()))
                {
                    _alarmInfo.ScheduleID = 0;
                }

                if (String.IsNullOrEmpty(_alarmInfo.DisplayID.ToString()))
                {
                    _alarmInfo.DisplayID = 0;
                }

                return ialarm.Add(_alarmInfo);
            }
        }
    }
}