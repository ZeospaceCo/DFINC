using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Temp
{
    public partial class View : Board.Web.Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정                
        public int UserID = 0;
        public int ListID = 0;
        public string Contents = string.Empty; //내용        
        public string AttachLimitSize = string.Empty; //첨부파일 제한용량
        private string eventArgs = String.Empty;        
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Temp.View).Assembly;	//어셈블리 선언
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
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {                
                cbtnAdd.Text = this.resMng.GetString("List_Register");
                cbtnDel.Text = this.resMng.GetString("Del");
                cbtnMod.Text = this.resMng.GetString("Modify");
                cbtnPrint.Text = this.resMng.GetString("Print");
                cbtnCancel.Text = this.resMng.GetString("List");
                cbtnMove.Text = this.resMng.GetString("List_Move");//이동하기
            }
        }

        /// <summary>
        /// 파라미터 가져오기
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            if (ListID == 0) return;

            #region 내용 보이기

            DAL.MyTemplates dal = new DAL.MyTemplates();
            List<BE.MyTemplateInfo> al = dal.GetMyTemplateLists(CurrentUserInfo.CompanyID, CategoryID, ListID, CurrentUserInfo.UserID);

            if (al.Count > 0)
            {
                foreach (BE.MyTemplateInfo info in al)
                {
                    DAL.MyTemplateEtc dalEtc = new DAL.MyTemplateEtc();
                    List<BE.MyTemplateEtcInfo> alEtc = dalEtc.GetList(CurrentUserInfo.CompanyID, info.CategoryID, info.MyTemplateID);

                    if (al.Count > 0)
                    {
                        foreach (BE.MyTemplateEtcInfo infoEtc in alEtc)
                        {
                            if (infoEtc.Etc2.ToNull().Length > 0)
                            {
                                string[] etcStr = infoEtc.Etc2.Split(',');
                                StringBuilder sb = new StringBuilder();
                                if (etcStr[2].ToString() != "0") sb.Append("<b>");
                                if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                                if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                                sb.Append(info.Title);
                                if (etcStr[0].ToString() != "") sb.Append("</font>");
                                if (etcStr[1].ToString() != "") sb.Append("</font>");
                                if (etcStr[2].ToString() != "0") sb.Append("</b>");

                                lbTitle.Text = sb.ToString();
                            }
                        }
                    }
                    else
                    {
                        lbTitle.Text = info.Title;
                    }

                    lbWriteInfo.Text = info.WriteUserName;
                    lbWriteDate.Text = info.WriteDate.ToShortDateString();
                    lbContent.Text = info.Content;
                }
            }
            else
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }

            #endregion
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 페이지 이동
        /// </summary>
        public string GoPage
        {
            get
            {
                BLL.Lists biz = new BLL.Lists();
                return "/DeskplusEIP/Board/Temp/" + biz.GoListPageTemp(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        private void Del()
        {
            BE.MyTemplateInfo info = new BE.MyTemplateInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID;
            info.MyTemplateID = ListID;
            info.WriteUserID = CurrentUserInfo.UserID;

            BLL.MyTemplate biz = new BLL.MyTemplate();
            biz.Del(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("categoryID", CategoryID.ToString());
            paraDic.Add("listID", ListID.ToString());
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", this.GetResource("GoToList"));
            
            paraDic.Add("goURL", GoPage);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);        
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "DEL":  //수정
                        Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"),"");
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

        #region (ETC)
        #endregion
    }
}