using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Content.Admin
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class EstimateItem : Base.BasePageProposal
    {
        #region 변수설정
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Admin.EstimateItem).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-24
        /// 작성목적 : Loads the data.
        /// </summary>
        /// <remarks></remarks>
        private void LoadData()
        {
            LoadParam();
            GetEstimateInfo();
        }

        #endregion

        #region 함수

        /// <summary>
        /// 작성자 : shne
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 다국어
        /// </summary>
        /// <remarks></remarks>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            
        }

        private void GetEstimateInfo()
        {
            
            DAL.Estimate dal = new DAL.Estimate();
            var itemList = dal.GetEstimateList(CurrentUserInfo.CompanyID);

            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    CTBName1.Text = item.Item1.ToString();
                    CTBScore1.Text = item.Score1.ToString();
                    CTBUnit1.Text = item.Sc_type1.ToString();
                    CTBExplain1.Text = item.Comment1.ToString();

                    CTBName2.Text = item.Item2.ToString();
                    CTBScore2.Text = item.Score2.ToString();
                    CTBUnit2.Text = item.Sc_type2.ToString();
                    CTBExplain2.Text = item.Comment2.ToString();

                    CTBName3.Text = item.Item3.ToString();
                    CTBScore3.Text = item.Score3.ToString();
                    CTBUnit3.Text = item.Sc_type3.ToString();
                    CTBExplain3.Text = item.Comment3.ToString();

                    CTBName4.Text = item.Item4.ToString();
                    CTBScore4.Text = item.Score4.ToString();
                    CTBUnit4.Text = item.Sc_type4.ToString();
                    CTBExplain4.Text = item.Comment4.ToString();

                    CTBName5.Text = item.Item5.ToString();
                    CTBScore5.Text = item.Score5.ToString();
                    CTBUnit5.Text = item.Sc_type5.ToString();
                    CTBExplain5.Text = item.Comment5.ToString();

                    CTBName6.Text = item.Item6.ToString();
                    CTBScore6.Text = item.Score6.ToString();
                    CTBUnit6.Text = item.Sc_type6.ToString();
                    CTBExplain6.Text = item.Comment6.ToString();

                    CTBName7.Text = item.Item7.ToString();
                    CTBScore7.Text = item.Score7.ToString();
                    CTBUnit7.Text = item.Sc_type7.ToString();
                    CTBExplain7.Text = item.Comment7.ToString();

                    CTBName8.Text = item.Item8.ToString();
                    CTBScore8.Text = item.Score8.ToString();
                    CTBUnit8.Text = item.Sc_type8.ToString();
                    CTBExplain8.Text = item.Comment8.ToString();

                    CTBName9.Text = item.Item9.ToString();
                    CTBScore9.Text = item.Score9.ToString();
                    CTBUnit9.Text = item.Sc_type9.ToString();
                    CTBExplain9.Text = item.Comment9.ToString();

                    CTBName10.Text = item.Item10.ToString();
                    CTBScore10.Text = item.Score10.ToString();
                    CTBUnit10.Text = item.Sc_type10.ToString();
                    CTBExplain10.Text = item.Comment10.ToString();
                    CTBExplain10.Text = item.Comment10.ToString();

                }
            }
        }

        /// <summary>
        /// 평가항목 저장
        /// </summary>
        private void Register()
        {
            if (Add())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", "보기 페이지로 이동합니다.");
                paraDic.Add("goURL", "/DeskPlusEIP/Proposal/Content/Admin/EstimateItem.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 평가항목 추가
        /// </summary>
        /// <returns></returns>
        private bool Add()
        {
            DAL.Estimate dal = new DAL.Estimate();
            BE.EstimateInfo info = new BE.EstimateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;

            info.Item1 = CTBName1.Text;
            info.Score1 = CTBScore1.Text;
            info.Sc_type1 = CTBUnit1.Text;
            info.Comment1 = CTBExplain1.Text;

            info.Item2 = CTBName2.Text;
            info.Score2 = CTBScore2.Text;
            info.Sc_type2 = CTBUnit2.Text;
            info.Comment2 = CTBExplain2.Text;

            info.Item3 = CTBName3.Text;
            info.Score3 = CTBScore3.Text;
            info.Sc_type3 = CTBUnit3.Text;
            info.Comment3 = CTBExplain3.Text;

            info.Item4 = CTBName4.Text;
            info.Score4 = CTBScore4.Text;
            info.Sc_type4 = CTBUnit4.Text;
            info.Comment4 = CTBExplain4.Text;

            info.Item5 = CTBName5.Text;
            info.Score5 = CTBScore5.Text;
            info.Sc_type5 = CTBUnit5.Text;
            info.Comment5 = CTBExplain5.Text;

            info.Item6 = CTBName6.Text;
            info.Score6 = CTBScore6.Text;
            info.Sc_type6 = CTBUnit6.Text;
            info.Comment6 = CTBExplain6.Text;

            info.Item7 = CTBName7.Text;
            info.Score7 = CTBScore7.Text;
            info.Sc_type7 = CTBUnit7.Text;
            info.Comment7 = CTBExplain7.Text;

            info.Item8 = CTBName8.Text;
            info.Score8 = CTBScore8.Text;
            info.Sc_type8 = CTBUnit8.Text;
            info.Comment8 = CTBExplain8.Text;

            info.Item9 = CTBName9.Text;
            info.Score9 = CTBScore9.Text;
            info.Sc_type9 = CTBUnit9.Text;
            info.Comment9 = CTBExplain9.Text;

            info.Item10 = CTBName10.Text;
            info.Score10 = CTBScore10.Text;
            info.Sc_type10 = CTBUnit10.Text;
            info.Comment10 = CTBExplain10.Text;

            return dal.Add(info);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 평가항목 저장
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;

                switch(args)
                {
                    case "Add":     // 평가항목 추가
                        Register();
                        break;
                    case "Del":     // 평가항목 가져오기
                        GetEstimateInfo();
                        break;

                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSave_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", "처리시 문제가 발생하였습니다.", "");
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