using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Coop.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Content.Request
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSCoop
    {
        /// <summary>
        /// Gets the item sub.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemSubInfo> GetItemSub(string p)
        {
            SetDecryptString(p);

            string itemID = GetDecryptParam("itemID", "0");

            List<BE.ItemSubInfo> alList = new List<ItemSubInfo>();

            if (itemID.ToNumber<int>() > 0)
            {
                DAL.ItemSub dal = new DAL.ItemSub();
                var list = dal.GetItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), true);

                foreach (var be in list)
                {
                    BE.ItemSubInfo sInfo = new ItemSubInfo();
                    sInfo.ItemSubID = be.ItemSubID;
                    sInfo.ItemID = be.ItemID;
                    sInfo.NameBase = be.NameBase;
                    sInfo.NameKo = be.NameKo;
                    sInfo.NameEn = be.NameEn;
                    sInfo.NameJa = be.NameJa;
                    sInfo.NameFr = be.NameFr;
                    sInfo.NameZhcn = be.NameZhcn;
                    sInfo.NameZhtw = be.NameZhtw;

                    #region 이름 다국어 설정

                    sInfo.NameBase = this.GetLangName(
                            sInfo.NameBase.ToString().ToNull(), sInfo.NameKo.ToString().ToNull(),
                            sInfo.NameEn.ToString().ToNull(), sInfo.NameJa.ToString().ToNull(),
                            sInfo.NameFr.ToString().ToNull(), sInfo.NameZhcn.ToString().ToNull(),
                            sInfo.NameZhtw.ToString().ToNull(), PreLang);

                    #endregion 이름 다국어 설정

                    sInfo.TempCnt = be.TempCnt;

                    alList.Add(sInfo);
                }
            }

            return alList;
        }

        /// <summary>
        /// 근거문서 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.Request.ListBaseInfo> GetListBase(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "");

            List<Ws.Content.Request.ListBaseInfo> alList = new List<ListBaseInfo>();
            if (listID.Length == 0) return alList;

            DAL.List dal = new DAL.List();
            var dtr = dal.GetList(CurrentUserInfo.CompanyID, listID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Ws.Content.Request.ListBaseInfo info = new ListBaseInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                        #region 이름 다국어 설정

                        info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);

                        #endregion 이름 다국어 설정

                        #region 직급 다국어 설정

                        info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                        #endregion 직급 다국어 설정

                        info.ItemName = this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                        info.ItemSubName = this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        //1:대기;2:접수;3:처리중;4:완료;5:반려
                        info.Status = GetStatus(dtr["StatusLast"].ToString());

                        info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        info.StartDate = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        info.Title = dtr["Title"].ToString();

                        alList.Add(info);
                    }
                }
            }

            return alList;
        }

        /// <summary>
        /// 지정자 정보 가져오기
        /// 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetFavUser(string p)
        {
            SetDecryptString(p);

            int itemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>();
            if (itemSubID == 0) return "";

            string favorUser = "";

            DAL.ItemFavUser dal = new DAL.ItemFavUser();
            var dtr = dal.GetList(CurrentUserInfo.CompanyID, itemSubID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string uID = "";
                    string name = "";

                    while (dtr.Read())
                    {
                        uID += dtr["UserID"].ToString() + ",";

                        name += this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                    }

                    //var response = userID + "‡" + userName + "‡" + types;
                    favorUser = uID + "‡" + name + "‡" + "M";
                }
            }

            return favorUser;
        }
    }

    [Serializable]
    public class ListBaseInfo
    {
        public ListBaseInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ListID { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public int WriteUserID { get; set; }

        public string NameBase { get; set; }

        public string TitleBase { get; set; }

        public string ItemName { get; set; }

        public string ItemSubName { get; set; }

        public string RequestDate { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}