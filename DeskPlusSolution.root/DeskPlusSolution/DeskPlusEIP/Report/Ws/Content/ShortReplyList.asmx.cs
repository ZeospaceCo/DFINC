using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// ShortReplyList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyList : Base.BaseWSReport
    {
        #region 리스트

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
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
        /// 작업일지 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.ShortReplyPageList> GetPageList(string p)
        {
            try
            {
                List<Ws.Content.ShortReplyPageList> alList = new List<ShortReplyPageList>();

                string listID = string.Empty;
                string totalCount = string.Empty;
                string pageSize = string.Empty;
                string pageSizePageNo = string.Empty;

                SetDecryptString(p);
                listID = GetDecryptParam("listID", "0");
                totalCount = GetDecryptParam("totalCount", "0");
                pageSize = GetDecryptParam("pageSize", "0");
                pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.ShortReply biz = new DAL.ShortReply();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, listID.ToNumber<int>(), setPageingStr[0], setPageingStr[1]);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string name = "";

                        while (dtr.Read())
                        {
                            Ws.Content.ShortReplyPageList info = new ShortReplyPageList();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                            info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                            info.UserName = name;
                            info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                            DateTime dt;

                            if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                                info.InputDateDT = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.InputDateDT = string.Empty;

                            #region 날짜 비교(New Img)

                            if (dtr["InputDate"].ToString() != "")
                            {
                                DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                                DateTime todayDate = System.DateTime.Now;
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }
                            else
                                info.ShowNew = "N";

                            #endregion 날짜 비교(New Img)

                            alList.Add(info);
                        }
                    }
                }

                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 댓글 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "0");

            DAL.ShortReply biz = new DAL.ShortReply();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, listID.ToNumber<int>());
        }

        #endregion 리스트
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ShortReplyPageList : Zeospace.DeskPlus.Common.BE.DeptCodeInfo
    {
        public ShortReplyPageList()
        {
        }

        public int CompanyID { get; set; }

        public int ListID { get; set; }

        public int ShortReplyID { get; set; }

        public string Content { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public DateTime InputDate { get; set; }

        public string UserName { get; set; }

        public string IsAuth { get; set; }

        public string StrInputDate { get; set; }

        public string ShowNew { get; set; }

        public string InputDateDT { get; set; }
    }
}