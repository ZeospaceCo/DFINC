using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Board.Web.Admin.Template
{
    public partial class List : Zeospace.DeskPlus.Board.Web.Base.BasePageBoardAdmin
    {
        #region Declaration (컨트롤 선언)        
        public string TotalCount = "0";
        #endregion

        #region 변수설정
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
            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Template.List).Assembly;	//어셈블리 선언
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
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        { 
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                DDLSearchType.Items.Clear();
                DDLSearchType.Items.Add(new ListItem(resMng.GetString("Title"), "Title"));
                DDLSearchType.Items.Add(new ListItem(resMng.GetString("Contents"), "Content"));

                CbtnAdd.Text = resMng.GetString("Add");
                CbtnSearch.Text = resMng.GetString("Search");
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        /// <returns></returns>
        private void GetListCount()
        {
            DAL.Templates biz = new DAL.Templates();
            TotalCount = biz.GetTemplatePageListCount(CurrentUserInfo.CompanyID, "", "").ToString();
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        [ComponentArt.Web.UI.ComponentArtCallbackMethod]
        public string AjaxSelectAttachFiles(int _categoryID, int _listID)
        {
            string loginID = HttpContext.Current.User.Identity.Name;
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo
                userInfo = Authenticator.GetInstance().AuthenticatorInfo(loginID);

            string response = "0";

            if (userInfo != null)
            {
                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(userInfo.CompanyID, _categoryID, _listID);

                string totalFileInfo = "";

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    if (_attachInfo.FileGUID != "")
                    {
                        totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileGUID + "|"; // Key < 파일경로파일명
                    }
                    else
                    {
                        totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileName + "|"; // Key < 파일경로파일명
                    }
                }

                totalFileInfo = totalFileInfo.TrimEnd('|');

                if (totalFileInfo.Length > 0)
                {
                    response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
                }
            }

            return response;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)





        #endregion
    }
}