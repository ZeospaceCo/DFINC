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

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Menu
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSTask
    {
        #region GET
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 전체리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Task.BE.ItemInfo> GetPageList(string p)
        {
            DAL.Item biz = new DAL.Item();

            List<Task.BE.ItemInfo> listobj = new List<Task.BE.ItemInfo>();

            var ItemList = biz.GetItem(CurrentUserInfo.CompanyID, false);

            foreach (var item in ItemList)
            {
                BE.ItemInfo info = new ItemInfo();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);

                info.NameBase = item.NameBase.ToNull();
                info.NameKo = item.NameKo.ToNull();
                info.NameEn = item.NameEn.ToNull();
                info.NameJa = item.NameJa.ToNull();
                info.NameFr = item.NameFr.ToNull();
                info.NameZhcn = item.NameZhcn.ToNull();
                info.NameZhtw = item.NameZhtw.ToNull();

                info.IsActive = item.IsActive;

                listobj.Add(info);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 선택 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemInfo> GetItem(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "1");

            DAL.Item biz = new DAL.Item();
            List<BE.ItemInfo> listobj = new List<ItemInfo>();

            var ItemList = biz.GetItemID(CurrentUserInfo.CompanyID, itemID.ToNumber<int>());

            foreach (var item in ItemList)
            {
                BE.ItemInfo info = new ItemInfo();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;

                info.ItemName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);

                info.NameBase = item.NameBase.ToNull();
                info.NameKo = item.NameKo.ToNull();
                info.NameEn = item.NameEn.ToNull();
                info.NameJa = item.NameJa.ToNull();
                info.NameFr = item.NameFr.ToNull();
                info.NameZhcn = item.NameZhcn.ToNull();
                info.NameZhtw = item.NameZhtw.ToNull();

                info.IsActive = item.IsActive;

                listobj.Add(info);
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 대분류 선택시 소분류 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemSubInfo> GetItemSubList(string p)
        {
            SetDecryptString(p);
            int itemID = GetDecryptParam("itemID", "0").ToString().ToNumber<int>();           

            List<BE.ItemSubInfo> list = new List<ItemSubInfo>();

            DAL.ItemSub itemSub = new DAL.ItemSub();

            List<BE.ItemSubInfo> itemSubList = itemSub.GetItemSub(CurrentUserInfo.CompanyID, itemID, false);

            if (itemID == 0) return itemSubList;

            foreach (var item in itemSubList)
            {
                BE.ItemSubInfo info = new ItemSubInfo();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemSubID = item.ItemSubID;
                info.ManyName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);
                info.IsActive = item.IsActive;

                info.TempCnt = item.TempCnt;

                list.Add(info);
            }

            return list;
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 소분류 선택시 이름, 활성화정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemSubInfo> GetItemSub(string p)
        {
            SetDecryptString(p);
            int ItemID = GetDecryptParam("itemID", "").ToString().ToNumber<int>();
            int ItemSubID = GetDecryptParam("itemSubID", "").ToString().ToNumber<int>();

            DAL.ItemSub dalItemSub = new DAL.ItemSub();
            List<BE.ItemSubInfo> itemSubID = new List<ItemSubInfo>();

            List<BE.ItemSubInfo> itemSubIDList = dalItemSub.GetItemSubID(CurrentUserInfo.CompanyID, ItemID, ItemSubID, false);

            foreach (var item in itemSubIDList)
            {
                BE.ItemSubInfo info = new ItemSubInfo();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemSubID = item.ItemSubID;
                info.NameBase = item.NameBase;
                info.ManyName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);

                info.NameKo = item.NameKo;
                info.NameEn = item.NameEn;
                info.NameJa = item.NameJa;
                info.NameFr = item.NameJa;
                info.NameZhcn = item.NameZhcn;
                info.NameZhtw = item.NameZhtw;

                info.IsActive = item.IsActive;

                info.TempCnt = item.TempCnt;

                itemSubID.Add(info);
            }
            
            return itemSubID;

        }

        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 항목 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddItem(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");

                BE.ItemInfo info = new BE.ItemInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.WriteUserID = CurrentUserInfo.UserID;

                DAL.Item dal = new DAL.Item();

                return dal.AddItem(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-22
        /// 소분류 항목 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddItemSub(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");
                string itemID = GetDecryptParam("itemID", "0");

                BE.ItemSubInfo info = new BE.ItemSubInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.ItemID = itemID.ToString().ToNumber<int>();

                DAL.ItemSub dal = new DAL.ItemSub();

                return dal.AddItemSub(info);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region MOD

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 소분류 항목 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModItemSub(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");
                string itemSubID = GetDecryptParam("itemSubID", "0");

                BE.ItemSubInfo info = new BE.ItemSubInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ItemSubID = itemSubID.ToNumber<int>();

                DAL.ItemSub dal = new DAL.ItemSub();

                return dal.ModItemSub(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 항목 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModItem(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");
                string itemID = GetDecryptParam("itemID", "0");

                BE.ItemInfo info = new BE.ItemInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ItemID = itemID.ToNumber<int>();

                DAL.Item dal = new DAL.Item();

                return dal.ModItem(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region DEL
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 항목 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelItem(string p)
        {
            SetDecryptString(p);
            int itemID =  GetDecryptParam("itemID", "").ToString().ToNumber<int>(); //대분류 아이디값            

            DAL.Item item = new DAL.Item();
            return item.DelItem(CurrentUserInfo.CompanyID, itemID);

        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 소분류 항목 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSubItem(string p)
        {
            SetDecryptString(p);
            int itemSubID = GetDecryptParam("itemSubID", "").ToString().ToNumber<int>(); //소분류 아이디값            

            DAL.ItemSub ItemSub = new DAL.ItemSub();
            return ItemSub.DelItemSub(CurrentUserInfo.CompanyID, itemSubID);

        }
        #endregion

    }

}
