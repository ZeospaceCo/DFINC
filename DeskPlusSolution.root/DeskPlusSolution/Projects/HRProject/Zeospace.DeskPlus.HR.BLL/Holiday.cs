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

namespace Zeospace.DeskPlus.HR.BLL
{
    public class Holiday : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 작성자 : kkzz52
        /// 공휴일저장 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheAddHolidayList(int _companyID, Dictionary<string, string> _holidayDic)
        {
            try
            {
                string CacheName = "AddHolidayList_";

                HttpContext context = HttpContext.Current;
                
                if (context.Cache[CacheName + _companyID] == null)
                {
                    DAL.Holiday holiDal = new DAL.Holiday();

                    holiDal.Del(_companyID);

                    foreach (KeyValuePair<string, string> kv in _holidayDic)
                    {
                        holiDal.Add(_companyID, kv.Key);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, "1", null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetCacheAddHolidayList : " + ex);
                throw;
            }
        }
    }
}
