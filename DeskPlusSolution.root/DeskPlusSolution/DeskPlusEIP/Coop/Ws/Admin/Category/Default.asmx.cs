using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Coop.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Admin.Category
{
    /// <summary>
    /// Default의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Default : Base.BaseWSCoop
    {
        #region GET
        /// <summary>
        /// 작성자 : shine
        /// 대분류 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Admin.Category.ItemList> GetPageList(string p)
        {
            SetDecryptString(p);

            DAL.Item biz = new DAL.Item();
            List<Ws.Admin.Category.ItemList> listobj = new List<ItemList>();

            var itemList = biz.GetItem(CurrentUserInfo.CompanyID, false);

            foreach (var item in itemList)
            {
                Ws.Admin.Category.ItemList info = new ItemList();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);
                info.ActiveGroup = item.ActiveGroup;

                listobj.Add(info);
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 선택한 대분류 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Admin.Category.ItemList> GetItem(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "0");

            DAL.Item biz = new DAL.Item();
            List<Ws.Admin.Category.ItemList> listobj = new List<ItemList>();

            var ItemList = biz.GetItem(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), false);

            foreach (var item in ItemList)
            {
                Ws.Admin.Category.ItemList info = new ItemList();

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

                info.ActiveGroup = item.ActiveGroup;

                listobj.Add(info);
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 소분류 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Admin.Category.ItemList> GetPageSubList(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "0");

            DAL.ItemSub biz = new DAL.ItemSub();
            List<Ws.Admin.Category.ItemList> listobj = new List<ItemList>();

            if (itemID == "0") return listobj;

            var itemSubList = biz.GetItemSub(CurrentUserInfo.CompanyID,itemID.ToNumber<int>(), false);

            foreach (var item in itemSubList)
            {
                Ws.Admin.Category.ItemList info = new ItemList();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemSubID = item.ItemSubID;

                info.ItemSubName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);

                info.SubActiveGroup = item.ActiveGroup;
                info.TempCnt = item.TempCnt;

                listobj.Add(info);
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 선택한 소분류 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Admin.Category.ItemList> GetSubItem(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "0");
            string itemSubID = GetDecryptParam("itemSubID", "0");

            DAL.ItemSub biz = new DAL.ItemSub();
            List<Ws.Admin.Category.ItemList> listobj = new List<ItemList>();

            if (itemID == "0" || itemSubID == "0") return listobj;

            var ItemSubList = biz.GetItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), itemSubID.ToNumber<int>(), false);

            foreach (var item in ItemSubList)
            {
                Ws.Admin.Category.ItemList info = new ItemList();

                info.CompanyID = item.CompanyID;
                info.ItemID = item.ItemID;
                info.ItemSubID = item.ItemSubID;

                info.ItemSubName = this.GetLangName(
                                item.NameBase.ToNull(), item.NameKo.ToNull(),
                                item.NameEn.ToNull(), item.NameJa.ToNull(),
                                item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                                item.NameZhtw.ToNull(), PreLang);

                info.SubNameBase = item.NameBase.ToNull();
                info.SubNameKo = item.NameKo.ToNull();
                info.SubNameEn = item.NameEn.ToNull();
                info.SubNameJa = item.NameJa.ToNull();
                info.SubNameFr = item.NameFr.ToNull();
                info.SubNameZhcn = item.NameZhcn.ToNull();
                info.SubNameZhtw = item.NameZhtw.ToNull();

                info.SubActiveGroup = item.ActiveGroup;

                listobj.Add(info);
            }
            return listobj;
        }

        #endregion

        #region ADD

        /// <summary>
        /// 작성자 : shine
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

                info.ActiveGroup = active;
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
        /// 작성자 : shine
        /// 소분류 항목 추가
        /// </summary>
        /// <param name="p">The p.</param>
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
                info.ItemID = itemID.ToNumber<int>();

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

                info.ActiveGroup = active;
                info.WriteUserID = CurrentUserInfo.UserID;

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
        /// 작성자 : shine
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
                info.ItemID = itemID.ToNumber<int>();

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.ActiveGroup = active;
                info.ModifyUserID = CurrentUserInfo.UserID;

                DAL.Item dal = new DAL.Item();

                return dal.ModItem(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
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
                string itemID = GetDecryptParam("itemID", "0");
                string itemSubID = GetDecryptParam("itemSubID", "0");

                BE.ItemSubInfo info = new BE.ItemSubInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;
                info.ItemID = itemID.ToNumber<int>();
                info.ItemSubID = itemSubID.ToNumber<int>();

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.ActiveGroup = active;
                info.ModifyUserID = CurrentUserInfo.UserID;

                DAL.ItemSub dal = new DAL.ItemSub();

                return dal.ModItemSub(info);
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
        /// 작성자 : shine
        /// 대분류 항목 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelItem(string p)
        {
            try
            {
                SetDecryptString(p);

                string itemID = GetDecryptParam("itemID", "0");

                DAL.Item dal = new DAL.Item();

                return dal.DelItem(CurrentUserInfo.CompanyID, itemID.ToNumber<int>());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 소분류 항목 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelItemSub(string p)
        {
            try
            {
                SetDecryptString(p);

                string itemID = GetDecryptParam("itemID", "0");
                string itemSubID = GetDecryptParam("itemSubID", "0");

                DAL.ItemSub dal = new DAL.ItemSub();

                return dal.DelItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), itemSubID.ToNumber<int>());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// 분류관리
    /// </summary>
    [Serializable]
    public class ItemList
    {
        public ItemList(){}

        #region 대분류
        public int CompanyID { get; set; }
        public int ItemID {get; set;}
        
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ActiveGroup { get; set; }
        public int WriteUserID { get; set; }
        public int ModifyUserID { get; set; }

        public string ItemName { get; set; }

        #endregion

        #region 소분류
        public int ItemSubID { get; set; }

        public string SubNameBase { get; set; }
        public string SubNameKo { get; set; }
        public string SubNameEn { get; set; }
        public string SubNameJa { get; set; }
        public string SubNameFr { get; set; }
        public string SubNameZhcn { get; set; }
        public string SubNameZhtw { get; set; }
        public string SubActiveGroup { get; set; }
        public int SubWriteUserID { get; set; }
        public int SubModifyUserID { get; set; }

        public string ItemSubName { get; set; }
        public int TempCnt { get; set; }
        
        #endregion

        #region 템플릿
        public int TemplateID { get; set; }

        public string Title { get; set; }
        public string Contents { get; set; }
        public int TempWriteUserID { get; set; }
        public int TempModifyUserID { get; set; }
        #endregion
    }
}
