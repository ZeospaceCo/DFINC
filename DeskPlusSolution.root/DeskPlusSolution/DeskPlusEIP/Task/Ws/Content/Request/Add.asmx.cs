using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Task.Web.Ws.Content.Request
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSTask
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-13
        /// 작성목적 : 대분류에 맞는 소분류 가져오기
        /// </summary>
        /// <param name="p"></param>
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
                    BE.ItemSubInfo Info = new ItemSubInfo();
                    Info.ItemSubID = be.ItemSubID;
                    Info.ItemID = be.ItemID;
                    Info.TempCnt = be.TempCnt;

                    #region 이름 다국어 설정

                    Info.NameBase = this.GetLangName(
                            be.NameBase.ToString().ToNull(), be.NameKo.ToString().ToNull(),
                            be.NameEn.ToString().ToNull(), be.NameJa.ToString().ToNull(),
                            be.NameFr.ToString().ToNull(), be.NameZhcn.ToString().ToNull(),
                            be.NameZhtw.ToString().ToNull(), PreLang);

                    #endregion 이름 다국어 설정

                    alList.Add(Info);
                }
            }

            return alList;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-13
        /// 작성목적 : 소분류에 지정되어 있는 사용자, 부서정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetItemSubUser(string p)
        {
            SetDecryptString(p);

            int itemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>();
            if (itemSubID == 0) return "";

            string itemSubUser = "";
            
            DAL.Specifier dalSpecifier = new DAL.Specifier();            
            var dtrSpecifier = dalSpecifier.GetSpecifier(CurrentUserInfo.CompanyID, itemSubID, CurrentUserInfo.UserID);

            if (dtrSpecifier != null)
            {
                if (dtrSpecifier.HasRows)
                {
                    string userID = "";
                    string deptID = "";
                    string name = "";
                    string isGroup = "";
                    string isType = "";

                    while (dtrSpecifier.Read())
                    {
                        userID += dtrSpecifier["UserID"].ToString() + ",";
                        deptID += dtrSpecifier["RDeptID"].ToString() + ",";
                        isGroup += dtrSpecifier["IsGroup"].ToString() + ",";
                        isType += dtrSpecifier["IsType"].ToString() + ",";

                        if (dtrSpecifier["IsGroup"].ToString() == "G")
                        {
                            name += this.GetLangName(
                                    dtrSpecifier["NameBase"].ToString().ToNull(), dtrSpecifier["NameKo"].ToString().ToNull(),
                                    dtrSpecifier["NameEn"].ToString().ToNull(), dtrSpecifier["NameJa"].ToString().ToNull(),
                                    dtrSpecifier["NameFr"].ToString().ToNull(), dtrSpecifier["NameZhcn"].ToString().ToNull(),
                                    dtrSpecifier["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                        }
                        else
                        {
                            string tmpName = this.GetLangName(
                                    dtrSpecifier["NameBase"].ToString().ToNull(), dtrSpecifier["NameKo"].ToString().ToNull(),
                                    dtrSpecifier["NameEn"].ToString().ToNull(), dtrSpecifier["NameJa"].ToString().ToNull(),
                                    dtrSpecifier["NameFr"].ToString().ToNull(), dtrSpecifier["NameZhcn"].ToString().ToNull(),
                                    dtrSpecifier["NameZhtw"].ToString().ToNull(), PreLang);

                            string tmpDept = this.GetLangName(
                                    dtrSpecifier["DeptBase"].ToString().ToNull(), dtrSpecifier["DeptKo"].ToString().ToNull(),
                                    dtrSpecifier["DeptEn"].ToString().ToNull(), dtrSpecifier["DeptJa"].ToString().ToNull(),
                                    dtrSpecifier["DeptFr"].ToString().ToNull(), dtrSpecifier["DeptZhcn"].ToString().ToNull(),
                                    dtrSpecifier["DeptZhtw"].ToString().ToNull(), PreLang);

                            string tmpTitle = this.GetLangName(
                                    dtrSpecifier["TitleBase"].ToString().ToNull(), dtrSpecifier["TitleKo"].ToString().ToNull(),
                                    dtrSpecifier["TitleEn"].ToString().ToNull(), dtrSpecifier["TitleJa"].ToString().ToNull(),
                                    dtrSpecifier["TitleFr"].ToString().ToNull(), dtrSpecifier["TitleZhcn"].ToString().ToNull(),
                                    dtrSpecifier["TitleZhtw"].ToString().ToNull(), PreLang);

                            name += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                        }
                    }

                    itemSubUser = isGroup + "‡" + userID + "‡" + deptID + "‡" + name + "‡" + isType;
                }
            }

            if (itemSubUser == "")
            {
                DAL.ItemSubUser dal = new DAL.ItemSubUser();
                var dtr = dal.GetList(CurrentUserInfo.CompanyID, itemSubID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string userID = "";
                        string deptID = "";
                        string name = "";
                        string isGroup = "";
                        string isType = "";

                        while (dtr.Read())
                        {
                            userID += dtr["UserID"].ToString() + ",";
                            deptID += dtr["RDeptID"].ToString() + ",";
                            isGroup += dtr["IsGroup"].ToString() + ",";
                            isType += dtr["IsType"].ToString() + ",";

                            if (dtr["IsGroup"].ToString() == "G")
                            {
                                name += this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                            }
                            else
                            {
                                string tmpName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                string tmpDept = this.GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                                string tmpTitle = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                name += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                            }
                        }

                        itemSubUser = isGroup + "‡" + userID + "‡" + deptID + "‡" + name + "‡" + isType;
                    }
                }
            }          


            return itemSubUser;
        }

        #endregion 

        #region ADD()
                /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-19
        /// 작성목적 : 지정자 저장하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSpecifier(string p)
        {
            SetDecryptString(p);

            int companyID = CurrentUserInfo.CompanyID;
            int specifierUserID = CurrentUserInfo.UserID;
            int itemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>();
            string toUser = GetDecryptParam("toUser", "0").ToString();
            string ccUser = GetDecryptParam("ccUser", "0").ToString();
            int toDelFlag = 1;
            int ccDelFlag = 1;
            

            DAL.Specifier dal = new DAL.Specifier();

            
             foreach (var user in toUser.Split(','))
             {
                 if (toDelFlag == 1)
                 {
                     dal.DelSpecifier(companyID, specifierUserID, itemSubID, "TO");
                     if (user != "0")
                     {
                         BE.Specifier be = new Specifier();

                         be.CompanyID = companyID;
                         be.SpecifierUserID = specifierUserID;
                         be.ItemSubID = itemSubID;
                         be.IsGroup = user.Split('_')[0].ToString();
                         be.UserID = user.Split('_')[1].ToString().ToNumber<int>();
                         be.DeptID = user.Split('_')[2].ToString().ToNumber<int>();
                         be.SpecifierOrder = toDelFlag;
                         be.IsType = "TO";

                         dal.AddSpecifier(be);  
                     }

                 }
                 else
                 {
                     BE.Specifier be = new Specifier();

                     be.CompanyID = companyID;
                     be.SpecifierUserID = specifierUserID;
                     be.ItemSubID = itemSubID;
                     be.IsGroup = user.Split('_')[0].ToString();
                     be.UserID = user.Split('_')[1].ToString().ToNumber<int>();
                     be.DeptID = user.Split('_')[2].ToString().ToNumber<int>();
                     be.SpecifierOrder = toDelFlag;
                     be.IsType = "TO";

                     dal.AddSpecifier(be); 
                 }
                 toDelFlag++;

             }


             foreach (var user in ccUser.Split(','))
             {
                 if (ccDelFlag == 1)
                 {
                     dal.DelSpecifier(companyID, specifierUserID, itemSubID, "CC");
                     if (user != "0")
                     {
                         BE.Specifier be = new Specifier();

                         be.CompanyID = companyID;
                         be.SpecifierUserID = specifierUserID;
                         be.ItemSubID = itemSubID;
                         be.IsGroup = user.Split('_')[0].ToString();
                         be.UserID = user.Split('_')[1].ToString().ToNumber<int>();
                         be.DeptID = user.Split('_')[2].ToString().ToNumber<int>();
                         be.SpecifierOrder = ccDelFlag;
                         be.IsType = "CC";

                         dal.AddSpecifier(be);
                     }

                 }
                 else
                 {
                     BE.Specifier be = new Specifier();

                     be.CompanyID = companyID;
                     be.SpecifierUserID = specifierUserID;
                     be.ItemSubID = itemSubID;
                     be.IsGroup = user.Split('_')[0].ToString();
                     be.UserID = user.Split('_')[1].ToString().ToNumber<int>();
                     be.DeptID = user.Split('_')[2].ToString().ToNumber<int>();
                     be.SpecifierOrder = ccDelFlag;
                     be.IsType = "CC";

                     dal.AddSpecifier(be);
                 }
                 ccDelFlag++;

             }            
          
            return true;
        }
        #endregion
    }
}
