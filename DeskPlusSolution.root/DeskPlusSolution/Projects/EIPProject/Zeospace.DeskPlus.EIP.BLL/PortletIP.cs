using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;
using Zeospace.DeskPlus.Account.BE;
using System.Web;
using System.Data;


namespace Zeospace.DeskPlus.EIP.BLL
{
    public class PortletIP : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// MENU 캐쉬명
        /// </summary>
        private string CacheName = "PortletIP_";

        #region 추가/수정/삭제
        #endregion

        #region 가져오기

        /// <summary>
        /// 메뉴아이피 캐쉬 초기화 하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        public void ClearCachePortletIP(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Cache[CacheName + _companyID] != null) Cache.Remove(CacheName + _companyID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 PortletIP 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.PortletIPInfo> SetCachePortletIP(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            List<BE.PortletIPInfo> alPortletIP = new List<BE.PortletIPInfo>();

            if (context.Cache[CacheName + _companyID] == null)
            {
                //모든 사용자 가져오기(활성화 및 겸직 제외)
                DAL.PortletIP dalPortletIP = new DAL.PortletIP();
                alPortletIP = dalPortletIP.GetPortletIP(_companyID);
                HttpContext ctx = HttpContext.Current;
                ctx.Cache.Insert(CacheName + _companyID, alPortletIP, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else if (context.Cache[CacheName + _companyID] != null)
            {
                alPortletIP = (List<BE.PortletIPInfo>)context.Cache[CacheName + _companyID];
            }

            return alPortletIP;
        }
        #endregion
    }
}
