using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Configuration;
using Zeospace.DeskPlusMobile.Web.MDM.WS.BE;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.MDM.WS.BLL
{
    public class Notification
    {
        /// <summary>
        /// </summary>
        /// <param name="_alarmInfo"></param>
        /// <param name="_alarmType"></param>
        /// <returns></returns>
        public bool Add(NotificationInfo _notificationInfo)
        {
            if (_notificationInfo == null)
            {
                return false;
            }
            else
            {
                string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");

                Data.Notification inotification = new Zeospace.DeskPlusMobile.Web.MDM.WS.Data.Notification();

                switch (_notificationInfo.KindSubject)
                {
                    case "Approval":
                        _notificationInfo.TargetUrl = httpType + fullDomain + _notificationInfo.TargetUrl;
                        break;
                    default:
                        _notificationInfo.TargetUrl = "";
                        break;
                }

                return inotification.NotificationAdd(_notificationInfo);
            }
        }
    }
}