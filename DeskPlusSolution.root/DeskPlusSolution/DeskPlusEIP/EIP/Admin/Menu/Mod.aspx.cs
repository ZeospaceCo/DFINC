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
    public partial class Mod : Base.BasePageEIPAdmin
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Menu.Mod).Assembly;	//어셈블리 선언
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

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "OpenURL");
            ddlOpenURL.Items.Clear();
            string name = "";
            for (int i = 0; i < dv.Count; i++)
            {
                name = this.GetLangName(
                    dv[i]["NameBase"].ToString().ToReverseXSS(),
                    dv[i]["NameKo"].ToString().ToReverseXSS(),
                    dv[i]["NameEn"].ToString().ToReverseXSS(), 
                    dv[i]["NameJa"].ToString().ToReverseXSS(),
                    dv[i]["NameFr"].ToString().ToReverseXSS(), 
                    dv[i]["NameZhCn"].ToString().ToReverseXSS(),
                    dv[i]["NameZhTw"].ToString().ToReverseXSS(), PreLang);

                ddlOpenURL.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            GetMenu();
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
                cbtnChagneParent.Text = resMng.GetString("ChangeParentMenu");
                cbtnMove.Text = resMng.GetString("ModSubMenuOrder");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Save"); //내용 수정
                cbtnMove.Text = resMng.GetString("Orders");

                cbtnADDParam.Text = resMng.GetString("AddParameter");
                cbtnADDAuth.Text = resMng.GetString("AddAuth");
                cbtnADDIP.Text = resMng.GetString("AddIP"); 

              
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 내용 가져오기
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
                           dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                           dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

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

                        ctbMenuName.Text = dr["NameBase"].ToString().ToReverseXSS();

                        ctbMultiName.Text = this.GetMuiltiLang(
                            dr["NameKo"].ToString().ToReverseXSS(),
                            dr["NameEn"].ToString().ToReverseXSS(),
                            dr["NameJa"].ToString().ToReverseXSS(),
                            dr["NameFr"].ToString().ToReverseXSS(),
                            dr["NameZhcn"].ToString().ToReverseXSS(),
                            dr["NameZhtw"].ToString().ToReverseXSS());

                        ctbDesc.Text = dr["MenuContent"].ToString().ToReverseXSS();
                        ctbDirectCall.Text = dr["DirectCall"].ToString().ToReverseXSS();
                        ctbTargetURL.Text = dr["TargetURL"].ToString();
                        ddlOpenURL.SelectedValue = dr["OpenURL"].ToString().ToNull();
                        ddlSendMethod.SelectedValue = (dr["SendMethod"].ToString().ToNull() == "0") ? "0" : "1";
                        rbClassName.SelectedValue = dr["ClassName"].ToString().ToNull();

                        GetMenuParent(dr["ParentMenuID"].ToString().ToNumber<int>());
                    }
                }
            }

            ltlHeader.Text = name;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 상위메뉴명 가져오기
        /// </summary>
        /// <param name="_parentMenuID">The _parent menu ID.</param>
        private void GetMenuParent(int _parentMenuID)
        {
            if (_parentMenuID > 0)
            {
                DAL.Menu dal = new DAL.Menu();

                DataSet ds = dal.GetMenu(CurrentUserInfo.CompanyID, _parentMenuID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
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


                        ltlParentName.Text = name.Substring(1, name.Length - 1);
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            BE.MenuInfo info = new BE.MenuInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.MenuID = MenuID;
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


            info.MenuContent = ctbDesc.Text;
            info.DirectCall = ctbDirectCall.Text.Trim();
            info.TargetURL = ctbTargetURL.Text.Trim();
            info.OpenURL = ddlOpenURL.SelectedValue;
            info.SendMethod = ddlSendMethod.SelectedValue;
            info.ClassName = rbClassName.SelectedValue;

            DAL.Menu dal = new DAL.Menu();
            return dal.Mod(info);
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
            
            if (Modify())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("menuID", MenuID.ToString());
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