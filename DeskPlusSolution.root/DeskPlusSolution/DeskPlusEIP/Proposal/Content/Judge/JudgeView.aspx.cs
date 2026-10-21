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
using System.Text;

namespace Zeospace.DeskPlus.Proposal.Web.Content.Judge
{
    public partial class JudgeView : Base.BasePageProposal
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ProposalNo = 0;
        public string IsAttach = "";
        public string MenuID = "";
        public string Status = "";
        private string HeaderName = "";     // 항목명
        public int Count = 0;          // 항목 수
        private string Scope = "";      // 점수 범위
        private string Score = "";      // 항목의 총합
        public string CodeName = "";   // 항목 코드
        public string JudgeList = "";   // 1차 심사자
        public int ItemNo = 0;          // 평가ID
        string[] ArrCodeName = null;
        string[] ArrScope = null;
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Judge.JudgeView).Assembly;	//어셈블리 선언
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
            LoadJudgeList();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ProposalNo = GetDecryptParam("proposalNo", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "");
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
            int writeUserID = 0;
            int statusID = 0;

            DAL.List biz = new DAL.List();

            DataTableReader dtr = biz.GetList(CurrentUserInfo.CompanyID, ProposalNo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        writeUserID = dtr["UserID"].ToString().ToNumber<int>();
                        statusID = dtr["Status"].ToString().ToNumber<int>();

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
                        // 이름
                        string name = GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                            dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                            dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                            dtr["NameZhtw"].ToString().ToNull(), PreLang);

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
                        ltlWriteName.Text = GetWriteUserName(name, writeUserID);
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
                        // 진행상황
                        Status = GetStatusName(statusID);
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
                        coWorkInfo += string.Format(@"<a onclick=""JudgeView.AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>,",
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
        /// 용도 : 작성자 정보 가져오기
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _name, int _userID)
        {
            string WriteUserInfo = "";

            if (_userID < 1)
                return _name;

            WriteUserInfo = string.Format(@"<a onclick=""AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>",
                            _name, _userID);

            return WriteUserInfo;
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
        /// 용도 : 평가항목 리스트 가져오기
        /// </summary>
        private void LoadExamList()
        {
            GetEstimateInfo();

            ArrCodeName = CodeName.Split('^');
            ArrScope = Scope.Split('^');

            StringBuilder sb = new StringBuilder();

            sb.Append("<table class='table_typeA'>");
            sb.Append("<colgroup>");
            sb.Append("<col width='80px' />");

            for (int i = 0; i < Count; i++)
            {
                sb.Append("<col  />");
            }

            sb.Append("<col width='100px' />");
            sb.Append("</colgroup>");

            sb.Append("<tr>");
            sb.Append("<th class='center'>평가</th>");

            foreach (var item in HeaderName.Split('^'))
            {
                sb.Append("<th class='center'>" + item + "</th>");
            }

            sb.Append("<th class='center'>총합보기</th>");
            sb.Append("</tr>");
            
            sb.Append("<tr>");
            sb.Append("<td class='center'>1차</td>");

            for (int i = 0; i < Count; i++)
            {
                sb.Append("<td class='center'><select CssClass='searchselect' runat='server' id='ddl" + ArrCodeName[i] + "' name='ddl" + ArrCodeName[i] + "' ");
                sb.Append(" onchange=\"JudgeView.ChangeScore()\" >");

                foreach (string item in ArrScope[i].Split(','))
                {
                    sb.Append("<option value='" + item + "'>" + item + "</option>");
                }

                sb.Append("</select> / " + Score.Split('^')[i]);
                sb.Append("</td>");
            }

            sb.Append("<td class='center'><input type='text' id='tbSum' class='text required' readOnly='readOnly' size='10' value=0 /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            ltlExamList.Text = sb.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 1차 심사자 리스트 가져오기
        /// </summary>
        private void LoadJudgeList()
        {
            DAL.JudgeUser judge = new DAL.JudgeUser();
            DataTableReader dtr = judge.GetJudgeUserInfo(CurrentUserInfo.CompanyID, ProposalNo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        JudgeList += dtr["UserID"].ToString() + ",";
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 평가항목 가져오기
        /// </summary>
        private void GetEstimateInfo()
        {
            DAL.Estimate estimate = new DAL.Estimate();

            var estmateInfo = estimate.GetEstimateList(CurrentUserInfo.CompanyID);

            if (estimate != null)
            {
                foreach (var info in estmateInfo)
                {
                    ItemNo = info.EstimateID;

                    if (info.Item1 != "" && info.Score1 != "")
                    {
                        HeaderName = info.Item1 + "^";
                        CodeName = "Item1" + "^";
                        Scope = info.Sc_type1 + "^";
                        Score = info.Score1 + "^";
                        Count++;
                    }

                    if (info.Item2 != "" && info.Score2 != "")
                    {
                        HeaderName += info.Item2 + "^";
                        CodeName += "Item2" + "^";
                        Scope += info.Sc_type2 + "^";
                        Score += info.Score2 + "^";
                        Count++;
                    }

                    if (info.Item3 != "" && info.Score3 != "")
                    {
                        HeaderName += info.Item3 + "^";
                        CodeName += "Item3" + "^";
                        Scope += info.Sc_type3 + "^";
                        Score += info.Score3 + "^";
                        Count++;
                    }

                    if (info.Item4 != "" && info.Score4 != "")
                    {
                        HeaderName += info.Item4 + "^";
                        CodeName += "Item4" + "^";
                        Scope += info.Sc_type4 + "^";
                        Score += info.Score4 + "^";
                        Count++;
                    }

                    if (info.Item5 != "" && info.Score5 != "")
                    {
                        HeaderName += info.Item5 + "^";
                        CodeName += "Item5" + "^";
                        Scope += info.Sc_type5 + "^";
                        Score += info.Score5 + "^";
                        Count++;
                    }

                    if (info.Item6 != "" && info.Score6 != "")
                    {
                        HeaderName += info.Item6 + "^";
                        CodeName += "Item6" + "^";
                        Scope += info.Sc_type6 + "^";
                        Score += info.Score6 + "^";
                        Count++;
                    }

                    if (info.Item7 != "" && info.Score7 != "")
                    {
                        HeaderName += info.Item7 + "^";
                        CodeName += "Item7" + "^";
                        Scope += info.Sc_type7 + "^";
                        Score += info.Score7 + "^";
                        Count++;
                    }

                    if (info.Item8 != "" && info.Score8 != "")
                    {
                        HeaderName += info.Item8 + "^";
                        CodeName += "Item8" + "^";
                        Scope += info.Sc_type8 + "^";
                        Score += info.Score8 + "^";
                        Count++;
                    }

                    if (info.Item9 != "" && info.Score9 != "")
                    {
                        HeaderName += info.Item9 + "^";
                        CodeName += "Item9" + "^";
                        Scope += info.Sc_type9 + "^";
                        Score += info.Score9 + "^";
                        Count++;
                    }

                    if (info.Item10 != "" && info.Score10 != "")
                    {
                        HeaderName += info.Item10 + "^";
                        CodeName += "Item10" + "^";
                        Scope += info.Sc_type10 + "^";
                        Score += info.Score10 + "^";
                        Count++;
                    }
                }
            }

            HeaderName = HeaderName.TrimEnd('^');
            CodeName = CodeName.TrimEnd('^');
            Scope = Scope.TrimEnd('^');
            Score = Score.TrimEnd('^');
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

        /// <summary>
        /// 진행상황 값 가져오기
        /// </summary>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        private string GetStatusName(int _status)
        {
            string status = "";

            switch (_status)
            {
                case 1:
                    status = "제안 신청중";
                    break;
                case 2:
                    status = "1차 심사중";
                    break;
                case 3:
                    status = "1차 평가완료";
                    break;
                case 4:
                    status = "1차 등급처리";
                    break;
                case 9:
                    status = "제안 기각";
                    break;
                default:
                    status = "";
                    break;
            }

            return status;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        protected void lbtnMod_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;

                switch (args)
                {
                    case "Del":  // 다시 평가하기
                        {
                            LoadExamList();
                            LoadJudgeList();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}