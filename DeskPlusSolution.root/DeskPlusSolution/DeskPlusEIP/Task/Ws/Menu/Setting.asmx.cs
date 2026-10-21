using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Ws.Menu
{
    /// <summary>
    /// Setting의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Setting : Base.BaseWSTask
    {
        #region GET
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-07
        /// 작성목적 : 대분류 전체 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemInfo> GetPageList()
        {
            DAL.Item item = new DAL.Item();

            List<BE.ItemInfo> lst = new List<ItemInfo>();

            List<BE.ItemInfo> listinfo = item.GetItem(CurrentUserInfo.CompanyID, true);

            foreach (var itemInfo in listinfo)
            {
                BE.ItemInfo info = new ItemInfo();

                info.ItemID = itemInfo.ItemID;

                info.ItemName = GetLangName(itemInfo.NameBase.ToNull(), itemInfo.NameKo.ToNull(),
                    itemInfo.NameEn.ToNull(), itemInfo.NameJa.ToNull(), itemInfo.NameFr.ToNull(),
                    itemInfo.NameZhcn.ToNull(), itemInfo.NameZhtw.ToNull(), PreLang);
                lst.Add(info);
            }

            return lst;
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-07
        /// 작성목적 : 각 구분 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetIsTypeList(string p)
        {
            SetDecryptString(p);           

            string IsType = GetDecryptParam("isType", "RE");

            string UserItemList = "";
            DAL.Menu dal = new DAL.Menu();
            DataTableReader dtr = dal.GetMenuSetting(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, IsType);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        UserItemList += dtr["ItemID"].ToString() + ',';
                    }
                }
            }
            return UserItemList;
        }

        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-07
        /// 작성목적 : 보이지 않길 원하는 리스트 저장하기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSetting(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "");
                string IsType = GetDecryptParam("isType", "RE");
                
                
                 DAL.Menu dal = new DAL.Menu();

                 if (dal.DelMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, IsType))  //삭제 후 Insert
                 {
                     if (listID.Length > 0)
                     {
                         foreach (var list in listID.Split(','))
                         {
                             BE.Menu info = new BE.Menu();

                             info.CompanyID = CurrentUserInfo.CompanyID;
                             info.UserID = CurrentUserInfo.UserID;
                             info.ItemID = list.ToString().ToNumber<int>();
                             info.IsType = IsType;

                             dal.AddMenu(info);


                         }  
                     }
                 }
               

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        } 
        #endregion

    }

}
