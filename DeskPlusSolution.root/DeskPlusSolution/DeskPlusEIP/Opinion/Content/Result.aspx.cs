using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Opinion.BE;

namespace Zeospace.DeskPlus.Opinion.Web.Content
{
    public partial class Result : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinion
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string Section = string.Empty;   //메뉴(진행:0 or 마감:1)
        public string EventArgs = string.Empty;
        public int CompanyID = 0;
        public int UserID = 0;
        public int QuesID = 0;
        public int IsAdmin = 0;
        public string Contents = string.Empty; //내용
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Content.Result).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            LoadParm();     //파라미터처리
            SetLanguage();  //다국어처리

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            this.LoadQuesInfo(this.QuesID);
            this.LoadTargetList(this.QuesID);
            this.LoadAdminList(this.QuesID);
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            this.Section = (MenuID.ToUpper().IndexOf("CLOSE").Equals(0)) ? "1" : "0";      //구분 (진행:0 or 마감:1)
            this.QuesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            this.cbtnVoteUser.Visible = (GetIsAdmin(QuesID) > 0) ? true : false;
            IsAdmin = (GetIsAdmin(QuesID) > 0) ? 1 : 0;
        }

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //Button
                this.cbtnPrint.Text = this.GetResource("BtnPrint");
                this.cbtnBoard.Text = this.GetResource("BtnBoard");
                this.cbtnExcel.Text = this.GetResource("BtnExcel");
                this.cbtnVoteUser.Text = this.GetResource("BtnVoteUser");
                this.cbtnCancel.Text = this.GetResource("BtnList");
                this.cbtnReplyAdd.Text = this.GetResource("BtnReply");

                //checkbox
                this.cbIsMain.Text = this.GetResource("TitIsMain");
                this.cbIsMain.ToolTip = this.GetResource("TitF5");
                this.cbIsReply.Text = this.GetResource("TitReply");
                this.cbIsReply.ToolTip = GetResource("TitF6");
                this.cbIsOpen.Text = this.GetResource("TitIsOpen");
                this.cbIsOpen.ToolTip = GetResource("TitF7");
                this.cbIsVoteUser.Text = this.GetResource("VoteUser");
                this.cbIsVoteUser.ToolTip = GetResource("TitF8");
                this.cbIsSecret.Text = this.GetResource("Secret");
                this.cbIsSecret.ToolTip = GetResource("TitSecret");

                this.lbSub.Text = (MenuID.ToUpper().IndexOf("CLOSE").Equals(0)) ? this.GetResource("TitSubM2") : this.GetResource("TitSubM1");
            }
        }

        /// <summary>
        /// 설문정보 가져오기
        /// </summary>
        /// <param name="_quesID"></param>
        private void LoadQuesInfo(int _quesID)
        {
            if (_quesID.Equals(0)) return;

            DAL.Ques qDAL = new DAL.Ques();
            List<QuesInfo> qList = qDAL.GetQues(CurrentUserInfo.CompanyID, _quesID);

            foreach (QuesInfo qInfo in qList)
            {
                this.ltTitle.Text = qInfo.Title;
                this.ltCont.Text = qInfo.Cont;
                this.ltSDate.Text = qInfo.StartDate.ToShortDateString();
                this.ltEDate.Text = qInfo.EndDate.ToShortDateString();
                this.ltDeptNm.Text = GetLangName(qInfo.SuperDeptNmBase, qInfo.SuperDeptNmKo, qInfo.SuperDeptNmEn, qInfo.SuperDeptNmJa, qInfo.SuperDeptNmFr, qInfo.SuperDeptNmZhcn, qInfo.SuperDeptNmZhtw, PreLang);
                this.cbIsMain.Checked = (qInfo.IsMain.Equals("1")) ? true : false;
                this.cbIsReply.Checked = (qInfo.IsReply.Equals("1")) ? true : false;
                this.cbIsVoteUser.Checked = (qInfo.IsVoteUser.Equals("1")) ? true : false;
                this.cbIsOpen.Checked = (qInfo.IsOpen.Equals("1")) ? true : false;
                this.cbIsSecret.Checked = (qInfo.IsSecret.Equals("1")) ? true : false;
                this.ltIsActive.Text = (qInfo.IsActive.Equals("1")) ? this.GetResource("Unit_Active") : this.GetResource("Unit_ActiveNon");

                if (this.cbIsSecret.Checked)
                {
                    //익명
                    this.cbtnVoteUser.Visible = false;
                }
                else
                {
                    this.cbtnVoteUser.Visible = (qInfo.IsVoteUser.Equals("1")) ? true : this.cbtnVoteUser.Visible;
                }
            }
        }
        /// <summary>
        /// 대상자리스트
        /// </summary>
        /// <param name="_quesID"></param>
        private void LoadTargetList(int _quesID)
        {
            this.ltTarget.Text = "";

            if (_quesID.Equals(0)) return;

            DAL.Target tDAL = new DAL.Target();
            DataTableReader dtr = tDAL.GetTargetList(CurrentUserInfo.CompanyID, _quesID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        this.ltTarget.Text += GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);
                        this.ltTarget.Text += " ";
                        this.ltTarget.Text += GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);
                        this.ltTarget.Text += "/";
                        this.ltTarget.Text += GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                        this.ltTarget.Text += " ";
                        this.ltTarget.Text += GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        this.ltTarget.Text += "<br>";
                    }
                }
            }
        }
        /// <summary>
        /// 관리자리스트
        /// </summary>
        /// <param name="_quesID"></param>
        private void LoadAdminList(int _quesID)
        {
            this.ltAdmin.Text = "";

            if (_quesID.Equals(0)) return;

            DAL.Admin aDAL = new DAL.Admin();
            DataTableReader dtr = aDAL.GetAdminList(CurrentUserInfo.CompanyID, _quesID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        this.ltAdmin.Text += GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);
                        this.ltAdmin.Text += " ";
                        this.ltAdmin.Text += GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);
                        this.ltAdmin.Text += "/";
                        this.ltAdmin.Text += GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                        this.ltAdmin.Text += " ";
                        this.ltAdmin.Text += GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        this.ltAdmin.Text += "<br>";
                    }
                }
            }
        }

        /// <summary>
        /// 특정설문의 관리자여부
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int GetIsAdmin(int _quesID)
        {
            DAL.Admin aDAL = new DAL.Admin();
            return aDAL.GetIsAdmin(CurrentUserInfo.CompanyID, _quesID, CurrentUserInfo.UserID);
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParm(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}