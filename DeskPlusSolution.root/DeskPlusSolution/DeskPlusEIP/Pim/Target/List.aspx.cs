using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Pim.Web.Target
{
    public partial class List : Base.BasePagePim
    {        
        #region 변수선언
        public int CompanyID = 0;
        public int UserID = 0;
        public int DeptID = 0;
        public string SiteMap = String.Empty;//사이트맵
        #endregion

        #region Page Process(페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {         
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Target.List).Assembly;	//어셈블리 선언
            #endregion     

            if (!IsPostBack)
            {
                SetLanguage();
                SettingControl();
                LoadParam();
                GetSiteMap();  //사이트맵
            }            
        }

        #endregion 

        #region method(함수)
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnAdd.Text= GetResource("ADD");
                cbtnAdmin.Text = GetResource("ADMIN");
            }
        }


        private void SettingControl()
        {        
            int iYear = DateTime.Now.Year;
            iYear=iYear + 3;

            for (int i = 1; i < 4; i++)
            {
                ListItem ltYear = new ListItem(iYear.ToString(), iYear.ToString());
                iYear--;
                ddlSearchYear.Items.Add(ltYear);
            }
            //return;
            iYear = DateTime.Now.Year;

            for (int i = 0; i < 5; i++)
            {
                ListItem ltYear = new ListItem(iYear.ToString(), iYear.ToString());
                iYear--;
                ddlSearchYear.Items.Add(ltYear);
            }

            ddlSearchYear.SelectedValue = DateTime.Now.Year.ToString();
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            string value = GetDecryptParam("Value", "");
            if (value.Length > 0)
            {
                string [] arVal = value.Split('|');

                DeptID = arVal[0].ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            int tmpDeptID = DeptID;
            if (tmpDeptID == 0)
                tmpDeptID = CurrentUserInfo.DeptID;

            Account.DAL.Dept dal = new Account.DAL.Dept();
            List<Account.BE.DeptInfo> al = dal.GetDeptLists(CurrentUserInfo.CompanyID, tmpDeptID, false);
            foreach (Account.BE.DeptInfo info in al)
            {
                SiteMap = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                        info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);      
            }               
            

            if (SiteMap == "") SiteMap = resMng.GetString("MarkManage");
        }

        #endregion

        #region Event Handler(이벤트핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion



    }
}