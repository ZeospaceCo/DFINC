using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Task.BLL
{
    public class Item : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 사이트맵
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="ItemID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public string GetSitemapName(int _companyID, int ItemID, string _preLang)
        {
            string siteMap = "";

            DAL.Item Dal = new DAL.Item();
            var ItemList = Dal.GetItemID(_companyID, ItemID);

            foreach (var item in ItemList)
            {
                siteMap = UtilHelper.GetLangName(item.NameBase, item.NameKo, item.NameEn,
                            item.NameJa, item.NameFr, item.NameZhcn, item.NameZhtw, _preLang);              
            }             

            return siteMap;
        }

    }
}
