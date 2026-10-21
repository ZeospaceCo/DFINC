using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class ControlSendSMS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_smsInfo"></param>
        /// <returns></returns>
        public bool Transfer(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsInfo)
        {
            string linkedCompany = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "SMSCompany");
  
            bool rtn = false;

            //업체명
            switch (linkedCompany)
            {
                case "SMSNotice": 
                    Zeospace.DeskPlusMobile.Web.SMS.SMSNotice biz1 = new SMSNotice();
                    if (biz1.Transfer(_smsInfo))
                    {
                        rtn = true;
                    }

                    break;
                case "Surem":
                    Zeospace.DeskPlusMobile.Web.SMS.Surem biz2 = new Surem();
                    if (biz2.Transfer(_smsInfo))
                    {
                        rtn = true;
                    }

                    break;
                case "SuremOracle": 
                    Zeospace.DeskPlusMobile.Web.SMS.SuremOracle biz3 = new SuremOracle();
                    if (biz3.Transfer(_smsInfo))
                    {
                        rtn = true;
                    }                    

                    break;
                case "SuremDll":
                    Zeospace.DeskPlusMobile.Web.SMS.SuremDll biz4 = new SuremDll();
                    if (biz4.Transfer(_smsInfo))
                    {
                        rtn = true;
                    }
                    break;
                case "SMSLook":
                    Zeospace.DeskPlusMobile.Web.SMS.SMSLook biz5 = new SMSLook();
                    if (biz5.Transfer(_smsInfo))
                    {
                        rtn = true;
                    }
                    break;
            }                     


            return rtn;
        }
    }
}