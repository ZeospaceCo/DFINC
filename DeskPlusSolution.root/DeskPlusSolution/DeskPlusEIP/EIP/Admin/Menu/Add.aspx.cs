using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Menu
{
    public partial class Add : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int MenuID;        
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
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Menu.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();
            GetMenu();

            rbClassName.Items[0].Selected = true;
            
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "OpenURL");
            ddlOpenURL.Items.Clear();
            string name = "";
            for (int i = 0; i < dv.Count; i++)
            {
                name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlOpenURL.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {                
                cbtnMove.Text = resMng.GetString("Orders");  
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnMove.Text = resMng.GetString("Orders");                
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 내용가져오기
        /// </summary>
        private void GetMenu()
        {
            string name = resMng.GetString("MenuManagement");
            DAL.Menu dal = new DAL.Menu();

            if (MenuID > 0)
            {
                DataSet ds = dal.GetMenu(CurrentUserInfo.CompanyID, MenuID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        name = this.GetLangName(
                           dr["NameBase"].ToString(),dr["NameKo"].ToString(),dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                           dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),dr["NameZhTw"].ToString(), PreLang);

                        /*
                        string name = this.GetLangName(
                           dr["PathNameBase"].ToString(),
                           dr["PathNameKo"].ToString(),
                           dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                           dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                           dr["PathNameZhTw"].ToString(), PreLang);

                        switch (PreLang)
                        {
                            case "ko":
                                name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "en":
                                name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "ja":
                                name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "fr":
                                name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhcn":
                                name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                            case "zhtw":
                                name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                break;
                        }


                        ltlHeader.Text = name.Substring(1, name.Length - 1);
                        */

                        ltlParentName.Text = name.Substring(1, name.Length - 1);                        
                    }
                }
            }

            ltlHeader.Text = name;
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            BE.MenuInfo info = new BE.MenuInfo();
                        
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.MenuID = 0;
            info.ParentMenuID = MenuID;            
            info.MenuOrder = 0;
            info.NameBase = ctbMenuName.Text.Trim();

            string multiName = ctbMultiName.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }


            info.MenuContent = ctbDesc.Text.Trim();
            info.DirectCall = ctbDirectCall.Text.Trim();
            info.TargetURL = ctbTargetURL.Text.Trim();
            info.OpenURL = ddlOpenURL.SelectedValue;
            info.ClassName = rbClassName.SelectedValue;

            DAL.Menu dal = new DAL.Menu();
            return dal.Add(info);
        }



        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            LoadParam();
            int menuID = Addition();

            if (menuID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("menuID", menuID.ToString());
                string url = @"parent.frmLeft.location='{0}';";
                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                url = string.Format(url, "/DeskplusEIP/EIP/Admin/Menu/Tree.aspx?p=" + encParams);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("SUCCESS"), url);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}