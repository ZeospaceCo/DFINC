using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Proposal.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content.Admin
{
    /// <summary>
    /// FirstProcess의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FirstProcess : Base.BaseWSProposal
    {
        DataSet dsJudgeUser = null;

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
        /// 1차 심사 중 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            SetDecryptString(p);

            string judgeGubun = GetDecryptParam("judgeGubun", "0");

            DAL.List biz = new DAL.List();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, judgeGubun);
        }

        /// <summary>
        /// 작성자 : shine
        /// 전체 제안 보기 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.Admin.ProcessPageList> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizePageNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                string judgeGubun = GetDecryptParam("judgeGubun", "0");

                int companyID = CurrentUserInfo.CompanyID;


                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List biz = new DAL.List();
                DAL.JudgeUser User = new DAL.JudgeUser();
                List<Ws.Content.Admin.ProcessPageList> alList = new List<ProcessPageList>();

                DataTableReader dtr = biz.GetPageList(companyID, setPageingStr[0], setPageingStr[1],judgeGubun);
                dsJudgeUser = User.GetJudgeUserInfo(companyID);
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.Admin.ProcessPageList info = new ProcessPageList();

                            string years = string.Empty;

                            info.YearMonth = dtr["YYYYMM"].ToString();
                            years = info.YearMonth.Substring(2, 4);

                            info.ReceiptNo = dtr["ReceiptNo"].ToString();
                            info.No = "IP" + years + "-" + info.ReceiptNo.PadLeft(3, '0');
                            info.ParentDeptName = dtr["ParentDeptName"].ToString();
                            info.DeptName = dtr["DeptName"].ToString();
                            info.Name = dtr["Name"].ToString();
                            info.Title = dtr["Title"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.ProposalNo = dtr["ProposalNo"].ToString().ToNumber<int>();
                            info.PersonCnt = dtr["PersonCnt"].ToString().ToNumber<int>();
                            info.JudgeCnt = dtr["JudgeCnt"].ToString().ToNumber<int>();
                            string judegInfo = GetJudgeUserInfo(info.ProposalNo);
                            info.JudgeUserID = judegInfo.Split('^')[0].ToNull();
                            info.JudgeName = judegInfo.Split('^')[1].ToNull();
                            info.ItemNo = dtr["ItemNo"].ToString().ToNull();

                            #region 등록일
                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateYMD = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateYMD = string.Empty;
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 1차 심사 중, 완료 승인자 정보 가져오기
        /// </summary>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        private string GetJudgeUserInfo(int _proposalNo)
        {
            string judgeUserName = string.Empty;
            string judgeUserID = string.Empty;

            DataView dvJudgeUser = dsJudgeUser.Tables[0].DefaultView;
            dvJudgeUser.RowFilter = "ProposalNo = '" + _proposalNo + "'";

            for (int i = 0; i < dvJudgeUser.Count; i++)
            {
                judgeUserID += dvJudgeUser[i].ToString() + ",";

                string name = this.GetLangName(
                    dvJudgeUser[i]["NameBase"].ToString().ToNull(),
                    dvJudgeUser[i]["NameKo"].ToString().ToNull(),
                    dvJudgeUser[i]["NameEn"].ToString().ToNull(), dvJudgeUser[i]["NameJa"].ToString().ToNull(),
                    dvJudgeUser[i]["NameFr"].ToString().ToNull(), dvJudgeUser[i]["NameZhCn"].ToString().ToNull(),
                    dvJudgeUser[i]["NameZhTw"].ToString().ToNull(), PreLang);

                judgeUserName += name + ",";
            }

            judgeUserID = judgeUserID.TrimEnd(',');
            judgeUserName = judgeUserName.TrimEnd(',');

            return judgeUserID + "^" + judgeUserName;
        }
    }

    [Serializable]
    public class ProcessPageList
    {
        public ProcessPageList() { }

        public string Row { get; set; }
        public string ReceiptNo { get; set; }
        public string No { get; set; }
        public string WriteDateYMD { get; set; }
        public string ParentDeptName { get; set; }
        public string DeptName { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Process { get; set; }
        public string Grade { get; set; }
        public string YearMonth { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; }
        public int ProposalNo { get; set; }
        public int PersonCnt { get; set; }
        public int JudgeCnt { get; set; }
        public string ShowNew { get; set; }
        public string JudgeUserID { get; set; }
        public string JudgeName { get; set; }
        public string ItemNo { get; set; }
    }
}
