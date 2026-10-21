using System;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.SMS
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISMS
    {
        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 
		/// 작성자     : 컴포넌트 용
		/// 최초작성일 : 2008-03-14
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        bool Transfer(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo);
    }
}
