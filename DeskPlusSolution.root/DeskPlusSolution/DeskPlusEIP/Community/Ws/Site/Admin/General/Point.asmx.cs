using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.General
{
    /// <summary>
    /// Point의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Point : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : 포인트 관리 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                string order = GetDecryptParam("order", "");

                DAL.PointConfig dal = new DAL.PointConfig();
                DataTableReader dtr  = dal.GetPointList(CurrentUserInfo.CompanyID, communityID, order);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PointConfigInfo info = new PointConfigInfo();

                            info.Type = dtr["Type"].ToString().ToNumber<int>();
                            info.Point = dtr["Point"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            #region 분류명
                            info.CategoryName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 등록자
                            info.WriteUserName = this.GetLangName(
                                    dtr["UserNameBase"].ToString().ToNull(), dtr["UserNameKo"].ToString().ToNull(),
                                    dtr["UserNameEn"].ToString().ToNull(), dtr["UserNameJa"].ToString().ToNull(),
                                    dtr["UserNameFr"].ToString().ToNull(), dtr["UserNameZhcn"].ToString().ToNull(),
                                    dtr["UserNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 등록일

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #endregion

                            info.ROW = dtr["ROW"].ToString().ToNumber<int>();


                            listobj.Add(info);
                        }
                    }
                }
                return listobj;
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
