using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListDay : Base.BaseWSPim
    {
        #region Get

        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 쪽지 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            string nowDate = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string order = string.Empty;

            SetDecryptString(p);
            nowDate = GetDecryptParam("nowDate", "");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            order = GetDecryptParam("order", "");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.ShortReply biz = new DAL.ShortReply();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                setPageingStr[0], setPageingStr[1], nowDate, nowDate, "", "", order, CurrentUserInfo.UserID);

            if(dtr != null)
            {
                if(dtr.HasRows)
                {
                    string rname = "";

                    while(dtr.Read())
                    {
                        BE.ShortReplyInfo info = new ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.RUserID = dtr["RUserID"].ToString().ToNumber<int>(); // 추가
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>(); // 추가

                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.UserName = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        if ((info.GroupID == info.ShortReplyID) && (info.RUserID != CurrentUserInfo.UserID))
                        {
                            rname = this.GetLangName(
                                dtr["RNameBase"].ToString(),
                                dtr["RNameKo"].ToString(), dtr["RNameEn"].ToString(), dtr["RNameJa"].ToString(),
                                dtr["RNameFr"].ToString(), dtr["RNameZhCn"].ToString(), dtr["RNameZhTw"].ToString(),
                                PreLang);

                            if (!String.IsNullOrEmpty(rname))
                            {
                                info.RUserName = "[" + rname + "]";
                            }
                            else
                            {
                                info.RUserName = "";
                            }
                        }
                        else
                        {
                            info.RUserName = "";
                        }

                        if (info.GroupID != info.ShortReplyID)
                        {
                            info.Depth = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";
                        }
                        else
                        {
                            info.Depth = "";
                        }

                        listobj.Add(info);
                    }
                }
            }
            
            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 쪽지 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);
            string nowDate = GetDecryptParam("nowDate", "");

            DAL.ShortReply biz = new DAL.ShortReply();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                nowDate, nowDate, "", "", CurrentUserInfo.UserID);
        }

        #endregion

        #region Set
        /// <summary>
        /// 사용자별 가장 마지막에 선택한 일정분류 저장
        /// </summary>
        /// <param name="p"></param>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCheckMenu(string p)
        {
            bool rtn = true;

            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userId", "").ToNumber<int>();
                string checkState = GetDecryptParam("checkState", "");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModPimCheckMenu(companyID, userID, checkState);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModCheckSchedule: ", ex);
                rtn = false;
            }

            return rtn;
        }
        #endregion
    }
}
