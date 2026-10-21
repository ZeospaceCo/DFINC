using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class SuremOracle
    {
        public bool Transfer(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsInfo)
        {
            return TransferProc(_smsInfo);
        }

        private bool TransferProc(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsComInfo)
        {
            bool rtn = false;
            return rtn;
        }
    }
}