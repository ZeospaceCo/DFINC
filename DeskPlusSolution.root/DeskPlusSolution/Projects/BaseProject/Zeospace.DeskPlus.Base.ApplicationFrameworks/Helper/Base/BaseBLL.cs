using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base
{
    public class BaseBLL
    {
        protected static Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2010-12-03
        /// 작성목적 : Cache 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        protected static void CacheData(string key, object data)
        {
            if (EnableCaching == "Y" && data != null)
            {
                Cache.Insert(key, data, null,
                   DateTime.Now.AddSeconds(CacheDuration), TimeSpan.Zero);
            }
        }

        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2010-12-03
        /// 작성목적 : Cache 삭제
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="prefix"></param>
        protected static void RemoveCacheItems(string prefix)
        {
            prefix = prefix.ToLower();
            List<string> itemsToRemove = new List<string>();

            IDictionaryEnumerator enumerator = Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower().StartsWith(prefix))
                    itemsToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string itemToRemove in itemsToRemove)
                Cache.Remove(itemToRemove);
        }

        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2010-12-03
        /// 작성목적 : Cache 사용 유무
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        protected static string EnableCaching
        {
            get
            {
                return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EnableCaching", "Y");
            }
        }

        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2010-12-03
        /// 작성목적 : Cache 유지 시간
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        protected static double CacheDuration
        {
            get
            {
                return Convert.ToDouble(AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "CacheDuration", "600"));
            }
        }
    }
}