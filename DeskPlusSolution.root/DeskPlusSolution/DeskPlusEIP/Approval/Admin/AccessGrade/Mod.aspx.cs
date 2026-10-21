using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade
{
    public partial class Mod : Base.BasePageApprovalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string EventArgs = string.Empty;
        public string MultiLang = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade.Mod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            CompanyID = CurrentUserInfo.CompanyID.ToString();
            AccessID = GetDecryptParam("accessID", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnMod.Text = this.resMng.GetString("Btn_Edit");               //"수정";
                cbtnAdd.Text = this.resMng.GetString("Add");                    //"추가";
                cbtnDel.Text = this.resMng.GetString("Btn_Del");                //"삭제";
                cbtnClose.Text = this.resMng.GetString("List");                 //"목록";
            }

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++) MultiLang += dv[i]["CodeID"].ToString() + ",";
            MultiLang = MultiLang.TrimEnd(',');
    
            #region 직급추가
            ddlAccessTitle.Items.Clear();

            this.ddlAccessTitle.Items.Add(new ListItem(resMng.GetString("PleaseSelectPerson"), ""));

            Zeospace.DeskPlus.Common.DAL.Code cDal = new Zeospace.DeskPlus.Common.DAL.Code();
            DataSet ds = cDal.GetCodeByScope(CurrentUserInfo.CompanyID, "T", true);

            int color = 1;
            int n = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAccessTitle.Items.Add(new ListItem(GetLangName(dr["NameBase"].ToString().ToReverseXSS(), dr["NameKo"].ToString().ToReverseXSS(), dr["NameEn"].ToString().ToReverseXSS(), dr["NameJa"].ToString().ToReverseXSS(), dr["NameFr"].ToString().ToReverseXSS(), dr["NameZhcn"].ToString().ToReverseXSS(), dr["NameZhtw"].ToString().ToReverseXSS(), PreLang)
                                                      , dr["CodeID"].ToString()));

                ddlAccessTitleIs.Items.Add(new ListItem(dr["AccessID"].ToString(), dr["CodeID"].ToString()));

                if (dr["AccessID"].ToString() == "" || dr["AccessID"].ToString() == "0")
                {
                    ddlAccessTitle.Items[color].Attributes.Add("style", "color:red;bold:true");
                    n = n + 1;
                }

                color = color + 1;
            }            
            #endregion

            spnAccessGradeDeptN.InnerText = n.ToString();

            rblIsAccess.Items.Clear();
            rblIsAccess.Items.Add(new ListItem(resMng.GetString("IsActiveTitY"), "1"));
            rblIsAccess.Items.Add(new ListItem(resMng.GetString("IsActiveTitN"), "0"));
            rblIsAccess.SelectedValue = "1";
        }

        private void LoadData()
        {
            DAL.AccessGrade biz = new DAL.AccessGrade();

            DataTableReader dtr = biz.GetDataByAccessID(CurrentUserInfo.CompanyID, AccessID.ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbAccessGrade.Text = dtr["AccessGrade"].ToString();

                        string NameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(),
                                dtr["NameZhTw"].ToString(), PreLang);

                        ctbMultiLang.Text = GetMuiltiLang(
                        dtr["NameKo"].ToString().ToReverseXSS(),
                        dtr["NameEn"].ToString().ToReverseXSS(),
                        dtr["NameJa"].ToString().ToReverseXSS(),
                        dtr["NameFr"].ToString().ToReverseXSS(),
                        dtr["NameZhcn"].ToString().ToReverseXSS(),
                        dtr["NameZhtw"].ToString().ToReverseXSS());

                        ctbAccessGradeName.Text = NameBase;

                        rblIsAccess.SelectedValue = dtr["IsAccess"].ToString();

                    }
                }
            }
        }

        private bool Addition()
        {
            BE.AccessGradeInfo info = new AccessGradeInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.AccessID = AccessID.ToString().ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            info.NameBase = ctbAccessGradeName.Text.Trim().ToXSS();

            string multiName = ctbMultiLang.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }

            info.IsAccess = rblIsAccess.SelectedValue;

            DAL.AccessGrade fDal = new DAL.AccessGrade();
            return fDal.Mod(info);
        }
         #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// [확인][삭제]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = Addition();

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Mod.Close();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "Mod.Close();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// 
        /// </summary>
        public string CompanyID
        {
            get { return GetViewStateString("CompanyID", ""); }
            set { ViewState["CompanyID"] = value; }
        }

        public string AccessID
        {
            get { return GetViewStateString("AccessID", ""); }
            set { ViewState["AccessID"] = value; }
        }

        #endregion
    }
}