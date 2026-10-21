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
    public class HolidayR : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region Add
        public void Add(BE.HolidayRInfo _info)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.HolidayR dal = new DAL.HolidayR();
                dal.Add(_info);
                ts.Complete();
            }
        }
        #endregion

        #region Mod
        public void Mod(BE.HolidayRInfo _info)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.HolidayR dal = new DAL.HolidayR();
                dal.Mod(_info);
                ts.Complete();
            }
        }
        #endregion

        #region Set

        /// <summary>
        /// 사용자 접근 가능한 메뉴 정보 캐쉬 초기화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        public void ClearCacheHolidayList(int _companyID)
        {
            string CacheName = "HolidayList_";

            HttpContext context = HttpContext.Current;
            if (context.Cache[CacheName + _companyID] != null) Cache.Remove(CacheName + _companyID);
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {            
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    //공휴일정보
                    Pim.DAL.HolidayR dal = new Pim.DAL.HolidayR();

                    DataTableReader dtr = dal.GetHoliDayByYear(_companyID, _UserSelectedYear.ToNumber<int>());

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                BE.HolidayRInfo _data = new BE.HolidayRInfo();

                                _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                                _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                                _data.Title = dtr["TITLE"].ToString();                                         //제목

                                _data.NameKo = dtr["NameKo"].ToString();
                                _data.NameEn = dtr["NameEn"].ToString();
                                _data.NameJa = dtr["NameJa"].ToString();
                                _data.NameFr = dtr["NameFr"].ToString();
                                _data.NameZhcn = dtr["NameZhcn"].ToString();
                                _data.NameZhtw = dtr["NameZhtw"].ToString();

                                _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                                _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                                _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                                _data.IsReplaceDay = 0;                                                        //대체휴가일자
                                _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                                listobj.Add(_data);
                            }
                        }
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                    //dicMenuList = (Dictionary<int, string>)context.Cache[CacheNameMenuListUser + _companyID];
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }
    }
    #endregion
}