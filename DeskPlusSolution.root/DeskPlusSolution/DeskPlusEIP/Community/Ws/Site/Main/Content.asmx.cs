using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main
{
    /// <summary>
    /// Content의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Content : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-13
        /// 작성목적 : 이슈 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetIssueList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int grade = GetDecryptParam("grade", "10").ToNumber<int>();    

                DAL.BoaIssue dal = new DAL.BoaIssue();

                DataTableReader dtr = dal.GetIssueList(CurrentUserInfo.CompanyID, communityID, grade);

                int row = 1;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoaInfo info = new BE.BoaInfo();

                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                            
                            info.Title = dtr["Title"].ToString();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString();

                            #region 이름 다국어 설정
                            info.WriteUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            if (info.WriteUserName.Trim() == "")
                            {
                                info.WriteUserName = dtr["WriteUserName"].ToString();
                            }
                            #endregion

                            info.ROW = row.ToString();

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            #region 등록일
                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            #region 답글일 경우 제목에 화살표 추가

                            string space = "";
                            for (int i = 0; i < info.Depth; i++) space += "&nbsp;&nbsp;";

                            if (info.Depth > 0) info.Title = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />" + " " + info.Title;
                            if (space.Length > 0) info.Title = space + info.Title;

                            #endregion

                            // 메뉴 이름 
                            info.MenuName = this.GetLangName(
                                    dtr["CNameBase"].ToString().ToNull(), dtr["CNameKo"].ToString().ToNull(),
                                    dtr["CNameEn"].ToString().ToNull(), dtr["CNameJa"].ToString().ToNull(),
                                    dtr["CNameFr"].ToString().ToNull(), dtr["CNameZhcn"].ToString().ToNull(),
                                    dtr["CNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.Auth = grade;

                            row++;

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

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 덧글 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReplyList(string p)
        {
            try 
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int grade = GetDecryptParam("grade", "10").ToNumber<int>();    

                DAL.BoaShortReply reply = new DAL.BoaShortReply();
                DataTableReader dtr = reply.GetShortReplyList(CurrentUserInfo.CompanyID, communityID, grade);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoaShortReplyInfo info = new BE.BoaShortReplyInfo();

                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();

                            info.shortReplyCount = dtr["shortReplyCount"].ToString();
                            info.ReadCount = dtr["ReadCount"].ToString();
                            info.Title = dtr["Title"].ToString();

                            #region 메뉴명 다국어 설정
                            info.MenuName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

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

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-14
        /// 작성목적 : 조회수 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetViewList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int grade = GetDecryptParam("grade", "10").ToNumber<int>();    

                DAL.BoaView view = new DAL.BoaView();
                DataTableReader dtr = view.GetViewList(CurrentUserInfo.CompanyID, communityID, grade);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoaViewInfo info = new BE.BoaViewInfo();

                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();

                            info.ViewListCount = dtr["ViewListCount"].ToString();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCount = dtr["ReplyCount"].ToString();

                            #region 메뉴명 다국어 설정
                            info.MenuName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-13
        /// 작성목적 : 이슈 지정 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelIssue(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                DAL.BoaIssue dal = new DAL.BoaIssue();
                return dal.Del(CurrentUserInfo.CompanyID, communityID, menuID, boardID);
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
