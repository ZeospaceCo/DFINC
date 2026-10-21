using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Main
{
    public partial class Add : Base.BasePageCommunity
    {
        #region 변수설정
  
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Main.Add).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-03
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            DAL.Category dal = new DAL.Category();
            DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID);

            ddlParent.Items.Clear();
            ddlParent.Items.Add(new ListItem(resMng.GetString("SelParentID"), "0"));

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                                        dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ddlParent.Items.Add(new ListItem(name, dtr["CategoryID"].ToString()));
                    }
                }
            }

            if (ddlParent.Items.Count > 1)
            {
                ddlParent.SelectedIndex = 1;
            }
            else
            {
                ddlParent.SelectedIndex = 0;
            }

            int tmpCategoryID = ddlParent.SelectedValue.ToNumber<int>();

            DataTableReader dtr1 = dal.GetCategory(CurrentUserInfo.CompanyID, tmpCategoryID);

            ddlChild.Items.Clear();
            ddlChild.Items.Add(new ListItem(resMng.GetString("SelChildID"), "0"));

            if (dtr1 != null)
            {
                if (dtr1.HasRows)
                {
                    while (dtr1.Read())
                    {
                        string name = this.GetLangName(dtr1["NameBase"].ToString(), dtr1["NameKo"].ToString(), dtr1["NameEn"].ToString(), dtr1["NameJa"].ToString(),
                                                        dtr1["NameFr"].ToString(), dtr1["NameZhcn"].ToString(), dtr1["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ddlChild.Items.Add(new ListItem(name, dtr1["CategoryID"].ToString()));
                    }
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-03
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";
                cbtnCancel.Text = resMng.GetString("Cancel");         //"취소";

                rblType.Items.Clear();
                rblType.Items.Add(new ListItem(resMng.GetString("Type1"), "1"));
                rblType.Items.Add(new ListItem(resMng.GetString("Type2") + "<span class='cm_subtxt'> * " + resMng.GetString("Type2Des") + "</span>", "2"));
                rblType.Items.Add(new ListItem(resMng.GetString("Type3") + "<span class='cm_subtxt'> * " + resMng.GetString("Type3Des") + "</span>", "3"));

                rblType.SelectedValue = "1";

                rblJoinType.Items.Clear();
                rblJoinType.Items.Add(new ListItem(resMng.GetString("JoinType1"), "1"));
                rblJoinType.Items.Add(new ListItem(resMng.GetString("JoinType2"), "2"));

                rblJoinType.SelectedValue = "1";


            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 등록
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {   
            int makeOption = 0;
         
            DataView dvMake = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "CMNMake");

            for (int i = 0; i < dvMake.Count; i++)
            {
                if (dvMake[i]["ActiveCode"].ToString() == "1")
                {
                    if (dvMake[i]["CodeID"].ToString() == "MakeOption1")
                    {
                        makeOption = 0;
                    }
                    else if (dvMake[i]["CodeID"].ToString() == "MakeOption2")
                    {
                        makeOption = 1;
                    }
                }
            }

            string usermail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

            int tmpType = rblType.SelectedValue.ToNumber<int>();
            int tmpJoinType = 2;

            if (tmpType == 1)
            {
                tmpJoinType = rblJoinType.SelectedValue.ToNumber<int>();
            }
            else if (tmpType == 2)
            {
                tmpJoinType = 2;
            }
            else if (tmpType == 3)
            {
                tmpJoinType = 3;
            }
            
            if (makeOption == 0)
            {
                BE.ComInfo info = new BE.ComInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.Name = ctbName.Text;
                info.Description = ctbDesc.Text;
                info.Question = ctbQuestion.Text;
                info.Answer = ctbAnswer.Text;
                info.ParentID = ddlParent.SelectedValue.ToNumber<int>();
                info.ChildID = tbChild.Text.ToNumber<int>();//ddlChild.SelectedValue.ToNumber<int>();
                info.Type = tmpType;
                info.JoinType = tmpJoinType;
                info.Status = 1;
                info.GubunGUID = System.Guid.NewGuid().ToString();
                info.WriteUserID = CurrentUserInfo.UserID;
                info.WriteUserName = CurrentUserInfo.Name;

                BLL.Com bll = new BLL.Com();  
                return bll.Add(info);
            }
            else
            {
                BE.ComApplyInfo info = new BE.ComApplyInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.Name = ctbName.Text;
                info.Description = ctbDesc.Text;
                info.Question = ctbQuestion.Text;
                info.Answer = ctbAnswer.Text;
                info.ParentID = ddlParent.SelectedValue.ToNumber<int>();
                info.ChildID = tbChild.Text.ToNumber<int>();
                info.Type = tmpType;
                info.JoinType = tmpJoinType;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.WriteUserName = CurrentUserInfo.Name;

                BLL.ComApply bll = new BLL.ComApply();
                return bll.Add(info, PreLang, CurrentUserInfo.LogInID, usermail);
            }  
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 등록
        /// </summary>
        private void Save()
        {
            int rtnID = Addition();

            if (rtnID > 0)
            {
                /*
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoMain"));
                paraDic.Add("goURL", "/DeskPlusEIP/Community/Main/Content.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
                */

                string sc = "Add.GoMain();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
            
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            Save();
        }

        #endregion
    }
}