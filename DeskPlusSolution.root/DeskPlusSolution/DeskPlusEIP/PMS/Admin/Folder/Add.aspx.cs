using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Admin.Folder
{
    public partial class Add : Base.BasePagePMSAdmin
    {
        #region Declaration (컨트롤 선언)
        private string eventArgs = String.Empty;
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Admin.Folder.Add).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   :  데이터가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-16
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;	//회사 id
            DeptID = CurrentUserInfo.DeptID;	//회사 id
            #endregion
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                CbtnDel.Text = resMng.GetString("Unit_Register_Delete");
                CbtnMod.Text = resMng.GetString("Mod");
                CbtnAdd.Text = resMng.GetString("Add");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get
            {
                return GetViewStateInt("CompanyID", "0");
            }
            set
            {
                ViewState["CompanyID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get
            {
                return GetViewStateInt("UserID", "0");
            }
            set
            {
                ViewState["UserID"] = value;
            }
        }


        /// <summary>
        /// 나의 부서아이디
        /// </summary>
        /// <value>The dept ID.</value>
        public int DeptID
        {
            get
            {
                return GetViewStateInt("DeptID", "0");
            }
            set
            {
                ViewState["DeptID"] = value;
            }
        }
        #endregion


        #region Event Handler (이벤트 핸들러)
        /// <summary>
        /// Handles the Click event of the lbtnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                BE.FolderInfo be = new Zeospace.DeskPlus.PMS.BE.FolderInfo();
                be.CompanyID = CompanyID;
                be.FolderID = 0;
                be.ParentFolderID = tbxFolderID.Text.Trim().ToNumber<int>();
                //be.Title = tbxFolderName.Text.Trim();

                be.FolderBase = tbxFolderName.Text;
                MultiName = this.CTBMultiName.Text.Trim();
                if (MultiName != "" && MultiName.IndexOf(",") != -1)
                {
                    be.FolderKo = MultiName.Split(',')[0];
                    be.FolderEn = MultiName.Split(',')[1];
                    be.FolderJa = MultiName.Split(',')[2];
                    be.FolderFr = MultiName.Split(',')[3];
                    be.FolderZhcn = MultiName.Split(',')[4];
                    be.FolderZhtw = MultiName.Split(',')[5];
                }

                be.WriteUserID = UserID;

                DAL.Folder bll = new Zeospace.DeskPlus.PMS.DAL.Folder();

                string args = eventArgs;
                switch (args)
                {
                    case "add":  //추가
                        be.Orders = 1;
                        bll.Add(be);
                        break;

                    case "edit":  //수정
                        be.FolderID = tbxFolderID.Text.ToNumber<int>();
                        bll.Mod(be);
                        break;

                    case "del": //삭제

                        DAL.Project list = new Zeospace.DeskPlus.PMS.DAL.Project();
                        int cnt = list.GetProjectCountByFolderID(CompanyID, tbxFolderID.Text.ToNumber<int>());
                        if (cnt == 0)
                        {
                            bll.Del(CompanyID, tbxFolderID.Text.ToNumber<int>(), UserID);
                        }
                        else
                        {
                            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.ClientMessage.Alert(this.GetResource("Project_Register_CountProjectList"));
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("Project_Register_CountProjectList"), "");
                            return;
                        }
                        break;
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click : ", ex);
                throw;
            }

            //string scripts = @"<script type=""text/javascript"">location = '" + Request.Url.ToString() + ";'<" + "/script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);

            Response.Redirect(Request.Url.ToString(), true);
        }



        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다. 이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}
