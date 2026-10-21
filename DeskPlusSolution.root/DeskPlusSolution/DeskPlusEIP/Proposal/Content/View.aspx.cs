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

namespace Zeospace.DeskPlus.Proposal.Web.Content
{
    public partial class View : Base.BasePageProposal
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ProposalNo = 0;
        public string IsAttach = "";
        public string MenuID = "";
        public string Status = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.View).Assembly;	//어셈블리 선언
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
            cbtnMod.Visible = false;
            cbtnDel.Visible = false;

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

            // 본인이 작성 한 글이고, 심사 전일 경우
            if (CurrentUserInfo.UserID == writeUserID && statusID == 2 && MenuID == "MyList")
            {
                cbtnMod.Visible = true;
                cbtnDel.Visible = true;
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
                        coWorkInfo += string.Format(@"<a onclick=""View.AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>,",
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
        #endregion
    }
}