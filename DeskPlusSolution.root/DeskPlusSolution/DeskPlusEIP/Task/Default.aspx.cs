using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web
{
    public partial class Default : Base.BasePageTask
    {
        #region 변수설정
        public string p_AccActive = "0"; //accordianActive
        public bool IsRoleTask = false;
        public string DefaultTab = "";
        public string HStatus = "";
        public string LeftURL = "";
        public string ContentURL = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            p_AccActive = GetDecryptParam("accActive", "0");
            HStatus = GetDecryptParam("hStatus", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetLeftURL();
            GetDefaultTab();            
            GetContentURL();
        }

        private void GetLeftURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("reLoad", "1");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + encParams;
        }

        private void GetDefaultTab()
        {
            IsRoleTask = CurrentUserInfo.IsInRole("taskall");//task admin

            if (IsRoleTask)
            {
                // 기본탭
                Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
                List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = envdal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (list != null)
                {
                    foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                    {
                        DefaultTab = info.TaskHomeMenu;
                    }
                }

                if (DefaultTab == "")
                {
                    DefaultTab = "AdminMain";
                }
            }
            else
            {
                DefaultTab = "Main";
            }
        }

        private void GetContentURL()
        {
            if (ReLoad == "") {

                if (IsRoleTask == false) {
                    ContentURL = "/DeskPlusEIP/Task/Home/Main.aspx";
                }
                else {
                    if (ItemID > 0) {
                        //차트에서 이동시
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("itemID", ItemID.ToString());
                        paraDic.Add("hStatus", HStatus.ToString());
                        paraDic.Add("searchField", SearchField.ToString());
                        paraDic.Add("searchDateValue", SearchDateValue.ToString());

                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        ContentURL = "/DeskPlusEIP/Task/Content/Total/List.aspx?p=" + encParams;
                    }
                    else {
                        if (HStatus == "") {
                            ContentURL = "/DeskPlusEIP/Task/Home/" + DefaultTab + ".aspx";
                        }
                        else {
                            //차트에서 이동시
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("hStatus", HStatus.ToString());
                            paraDic.Add("searchField", SearchField.ToString());
                            paraDic.Add("searchDateValue", SearchDateValue.ToString());

                            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                            ContentURL = "/DeskPlusEIP/Task/Content/Total/List.aspx?p=" + encParams;
                        }
                    }
                }
            }
        }
        #endregion
    }
}