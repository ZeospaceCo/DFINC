using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Proposal.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.Web.Content.Admin
{
    public partial class ResultView : Base.BasePageProposalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ProposalNo = 0;
        public string MenuID = "";
        public string IsAttach = "";
        public int ItemNo = 0;
        private string Headers = string.Empty;
        private int Count = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Admin.ResultView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadList();
            LoadAttach();
            LoadExamList();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ProposalNo = GetDecryptParam("proposalNo", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "Result");
            ItemNo = GetDecryptParam("itemNo", "0").ToNumber<int>();
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 게시물 내용
        /// </summary>
        private void LoadList()
        {

            DAL.List biz = new DAL.List();

            DataTableReader dtr = biz.GetList(CurrentUserInfo.CompanyID, ProposalNo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = string.Empty;
                    while (dtr.Read())
                    {
                        // 상위부서
                        string pDeptName = GetLangName(dtr["PNameBase"].ToString().ToNull(), dtr["PNameKo"].ToString().ToNull(),
                            dtr["PNameEn"].ToString().ToNull(), dtr["PNameJa"].ToString().ToNull(),
                            dtr["PNameFr"].ToString().ToNull(), dtr["PNameZhcn"].ToString().ToNull(),
                            dtr["PNameZhtw"].ToString().ToNull(), PreLang);
                        // 부서
                        string deptName = GetLangName(dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                        // 소속
                        ltlDeptName.Text = pDeptName + " " + deptName;
                        // 접수일자
                        ltlWriteDate.Text = Convert.ToDateTime(dtr["WriteDate"].ToString()).ToShortDateString();
                        // 직위
                        ltlTitleName.Text = GetLangName(dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                            dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                            dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                            dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        // 사번
                        ltlEmpNo.Text = dtr["EmployeeNumber"].ToString().ToNull();
                        // 년월
                        string yyyymm = dtr["YYYYMM"].ToString().Substring(2, 4);
                        // 번호
                        string no = dtr["ReceiptNo"].ToString().PadLeft(3, '0');
                        // 접수번호
                        ltlNo.Text = "IP" + yyyymm + "-" + no;
                        // 작성자
                        name = GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                            dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                            dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                            dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        ltlWriteName.Text = string.Format(@"<a onclick=""AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>",
                             name, dtr["UserID"].ToString().ToNumber<int>());
                         
                        // 제안구분
                        ltlGubun.Text = dtr["Gubun"].ToString();
                        // 공동제안자
                        ltlCoWork.Text = GetCoWorkName();
                        // 제안제목
                        ltlTitle.Text = dtr["Title"].ToString();
                        // 팀분류
                        ltlTeamGubun.Text = GetTeamName(dtr["Bul_Select"].ToString());

                        // 1. 현행 및 문제점
                        ltlProblem.Text = dtr["BeforeCont"].ToString().Replace("\r\n", "<br />");
                        // 2. 개선안
                        ltlSuggestion.Text = dtr["AfterCont"].ToString().Replace("\r\n", "<br />");
                        // 3. 제안의 예상효과
                        ltlEffect.Text = dtr["BeforeEffect"].ToString().Replace("\r\n", "<br />");
                        // 비고
                        ltlEtc.Text = dtr["Etc1"].ToString().Replace("\r\n", "<br />");
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 공동 제안자 정보 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetCoWorkName()
        {
            DAL.Person biz = new DAL.Person();
            string coWorkInfo = "";

            DataTableReader dtr = biz.GetPersonInfo(CurrentUserInfo.CompanyID, ProposalNo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        coWorkInfo += string.Format(@"<a onclick=""AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>,",
                            dtr["Name"].ToString(), dtr["UserID"].ToString().ToNumber<int>());
                    }
                }
                else
                {
                    coWorkInfo = "공동 제안자 없음";
                }
            }

            coWorkInfo = coWorkInfo.TrimEnd(',');
            return coWorkInfo;
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 첨부파일 리스트 가져오기
        /// </summary>
        private void LoadAttach()
        {
            DAL.Attach biz = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = biz.GetAttachFileList(CurrentUserInfo.CompanyID, ProposalNo);

            if (attachInfo.Count == 0)
            {
                ltlAttachFile.Text = "";
                IsAttach = @"style='display:none'";
            }
            else
            {
                string attHtml = "";

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"
                        <tr>
                            <td>
                                <a onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a>
                            </td>
                        </tr>";

                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID);
                }

                ltlAttachFile.Text = string.Format(@"
                    <table border='0' class='att_css'>
                        {0}
                    </table>", attHtml);
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 평가표 리스트
        /// </summary>
        private void LoadExamList()
        {
            // 평가아이템(헤더)
            GetEstimateInfo();

            double totalScoreSum = 0;           // 심사위원 총합
            string lastGrade = string.Empty;    // 등급

            DAL.Exam biz = new DAL.Exam();
            StringBuilder sb = new StringBuilder();

            DataTableReader dtr = biz.GetStatusList(CurrentUserInfo.CompanyID, ProposalNo);

            sb.Append("<table class='table_typeA'>");
            sb.Append("<colgroup>");
            sb.Append("<col width='120px' />");

            for(int i = 0; i < Count; i++)
            {
                sb.Append("<col  />");
            }

            sb.Append("<col width='80px' />");
            sb.Append("</colgroup>");
            
            sb.Append("<tr>");
            sb.Append("<th class='center'>심사위원</th>");
            
            foreach (var item in Headers.Split('^'))
            {
                sb.Append("<th class='center'>" + item + "</th>");
            }

            sb.Append("<th class='center'>총합</th>");
            sb.Append("<th class='center'>심사의견</th>");
            sb.Append("</tr>");
            
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        // 심사위원 이름
                        string name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);

                        // 심사위원 UserID
                        string userID = dtr["JUserID"].ToString();
                        // 인별 총합
                        string totalScore = dtr["TotalScore"].ToString().ToNull();
                        // 심사위원 인원수
                        int personCnt = dtr["PersonCnt"].ToString().ToNumber<int>();
                        // 심사위원 총합
                        totalScoreSum = dtr["TotalScoreSum"].ToString().ToDouble<double>();
                        // 등급
                        lastGrade = dtr["LastGrade"].ToString().ToNull();
                        // 심사위원 총합의 평균점수
                        totalScoreSum = totalScoreSum / personCnt;

                        sb.Append("<tr>");
                        sb.Append("<td class='center'>");
                        sb.Append(@"<a href=""#"" onclick=""javascript:AccountInfo('" + userID + @"')"" style='cursor:pointer'>" + name + "</a></td>");

                        // 평가 점수
                        for (int i = 0; i < Count; i++)
                        {
                            sb.Append("<td class='center'>" + dtr["Score" + (i + 1)].ToString() + "</td>");
                        }

                        sb.Append("<td class='center'>&nbsp;" + totalScore + "</td>");
                        sb.Append("<td class='center'>");
                        sb.Append(@"<a href=""#"" onclick=""javascript:View.Opinion('" + ProposalNo + "','" + userID + @"')"" style='cursor:pointer'>보기</a></td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan=" + (Count + 2) + ">&nbsp;</td>");
                    sb.Append("</tr>");
                }
            }

            sb.Append("</table>");

            // 평균점수
            ltltotalScore.Text = totalScoreSum.ToString();
            //등급
            ctbGrade.Text = lastGrade;
            // 평가표
            ltlExamList.Text = sb.ToString();           
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 평가표 정보 가져오기
        /// </summary>
        private void GetEstimateInfo()
        {
            DAL.Estimate biz = new DAL.Estimate();
            var itemList = biz.GetEstimateList(CurrentUserInfo.CompanyID, ItemNo);

            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    if (item.Item1 != "" && item.Score1 != "")
                    {
                        Headers = item.Item1 + "^";
                        Count++;
                    }

                    if (item.Item2 != "" && item.Score2 != "")
                    {
                        Headers += item.Item2 + "^";
                        Count++;
                    }

                    if (item.Item3 != "" && item.Score3 != "")
                    {
                        Headers += item.Item3 + "^";
                        Count++;
                    }

                    if (item.Item4 != "" && item.Score4 != "")
                    {
                        Headers += item.Item4 + "^";
                        Count++;
                    }

                    if (item.Item5 != "" && item.Score5 != "")
                    {
                        Headers += item.Item5 + "^";
                        Count++;
                    }

                    if (item.Item6 != "" && item.Score6 != "")
                    {
                        Headers += item.Item6 + "^";
                        Count++;
                    }

                    if (item.Item7 != "" && item.Score7 != "")
                    {
                        Headers += item.Item7 + "^";
                        Count++;
                    }

                    if (item.Item8 != "" && item.Score8 != "")
                    {
                        Headers += item.Item8 + "^";
                        Count++;
                    }

                    if (item.Item9 != "" && item.Score9 != "")
                    {
                        Headers += item.Item9 + "^";
                        Count++;
                    }

                    if (item.Item10 != "" && item.Score10 != "")
                    {
                        Headers += item.Item10 + "^";
                        Count++;
                    }
                }
            }

            Headers = Headers.TrimEnd('^');
        }

        /// <summary>
        /// 팀분류 이름 가져오기
        /// </summary>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        private string GetTeamName(string _gubun)
        {
            string teamName = string.Empty;

            switch (_gubun)
            {
                case "1":
                    teamName = "경영본부";
                    break;
                case "2":
                    teamName = "반도체본부";
                    break;
                case "3":
                    teamName = "시스템본부";
                    break;
                case "4":
                    teamName = "마이크로본부";
                    break;
                default:
                    teamName = "";
                    break;
            }

            return teamName;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}