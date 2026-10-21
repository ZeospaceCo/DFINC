using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Common.Web.WebFax.Admin
{
    public partial class Register : Base.BasePageCommon
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string RecordPerPage = "";
        public string SearchKey = "";
        public string SearchValue = "";
        public string Order = "";
        public string Gubun = "";
        public int FaxID = 0;

        private string eventArgs = String.Empty;
        

        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Admin.Register).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
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

            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin
            if (!isAdmin)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            //지회정보
            this.ddlBranch.Items.Clear();
            ddlBranch.AppendDataBoundItems = true;
            ddlBranch.Items.Add(new ListItem("-------", ""));
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            DataSet jDs = codeDal.GetCodeByScope(CurrentUserInfo.CompanyID, "J", true);
            string branchName = "";
            foreach (DataRow _dr in jDs.Tables[0].Rows)
            {
                branchName = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                        _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                this.ddlBranch.Items.Add(new ListItem(branchName, _dr["CodeID"].ToString()));
            }

            if (FaxID > 0)
            {
                DAL.FaxManager dal = new DAL.FaxManager();
                var dtr = dal.GetList(CurrentUserInfo.CompanyID, FaxID);
                BE.FaxManagerInfo info = new BE.FaxManagerInfo();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FaxID = dtr["FaxID"].ToString().ToNumber<int>();
                            info.NameBase = dtr["NameBase"].ToString();
                            info.NameKo = dtr["NameKo"].ToString();
                            info.NameEn = dtr["NameEn"].ToString();
                            info.NameJa = dtr["NameJa"].ToString();
                            info.NameFr = dtr["NameFr"].ToString();
                            info.NameZhcn = dtr["NameZhcn"].ToString();
                            info.NameZhtw = dtr["NameZhtw"].ToString();                                                        
                            info.FaxNumber = dtr["FaxNumber"].ToString();
                            info.BranchID = dtr["BranchID"].ToString().ToNumber<int>();
                            info.Gubun = dtr["Gubun"].ToString();
                            info.IsActive = dtr["IsActive"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        }
                    }
                }

                info.NameBase = this.GetLangName(
                                    info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                ctbName.Text = info.NameBase;
                ctbFaxNumber.Text = info.FaxNumber;

                ctbMuliLang.Text = GetMuiltiLang(
                        info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(), info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());

                ddlActive.SelectedValue = info.IsActive.ToString();
                ddlBranch.SelectedValue = info.BranchID.ToString();

            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Gubun = GetDecryptParam("gubun", "");
            FaxID = GetDecryptParam("faxID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Alert_Ok");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnList.Text = resMng.GetString("FaxList");

                ddlActive.Items.Clear();
                ddlActive.Items.Add(new ListItem(resMng.GetString("Alert_Yes"), "1"));
                ddlActive.Items.Add(new ListItem(resMng.GetString("Alert_No"), "0"));
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        private void Add()
        {
            BE.FaxManagerInfo be = new BE.FaxManagerInfo();

            string multiName = ctbMuliLang.Text.Trim();

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.NameKo = multiName.Split(',')[0];
                be.NameEn = multiName.Split(',')[1];
                be.NameJa = multiName.Split(',')[2];
                be.NameFr = multiName.Split(',')[3];
                be.NameZhcn = multiName.Split(',')[4];
                be.NameZhtw = multiName.Split(',')[5];
            }
            be.NameBase = ctbName.Text.Trim();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.BranchID = ddlBranch.SelectedValue.ToNumber<int>();            
            be.FaxNumber = ctbFaxNumber.Text.ToString().Trim();
            be.Gubun = Gubun;
            be.IsActive = ddlActive.SelectedValue.ToNumber<int>();
            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.FaxManager dal = new DAL.FaxManager();
            bool response = dal.Add(be);

            if (response)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("recordPerPage", RecordPerPage.ToString());
                paraDic.Add("searchKey", Server.UrlEncode(SearchKey));
                paraDic.Add("searchValue", Server.UrlEncode(SearchValue));
                paraDic.Add("order", Order);
                paraDic.Add("gubun", Gubun);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Common/WebFax/Admin/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        private void Mod()
        {
            BE.FaxManagerInfo be = new BE.FaxManagerInfo();

            string multiName = ctbMuliLang.Text.Trim();

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.NameKo = multiName.Split(',')[0];
                be.NameEn = multiName.Split(',')[1];
                be.NameJa = multiName.Split(',')[2];
                be.NameFr = multiName.Split(',')[3];
                be.NameZhcn = multiName.Split(',')[4];
                be.NameZhtw = multiName.Split(',')[5];
            }
            be.NameBase = ctbName.Text.Trim();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.BranchID = ddlBranch.SelectedValue.ToNumber<int>();
            be.FaxID = FaxID;
            be.FaxNumber = ctbFaxNumber.Text.ToString().Trim();
            be.Gubun = Gubun;
            be.IsActive = ddlActive.SelectedValue.ToNumber<int>();
            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.FaxManager dal = new DAL.FaxManager();
            bool response = dal.Mod(be);

            if (response)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("recordPerPage", RecordPerPage.ToString());
                paraDic.Add("searchKey", Server.UrlEncode(SearchKey));
                paraDic.Add("searchValue", Server.UrlEncode(SearchValue));
                paraDic.Add("order", Order);
                paraDic.Add("gubun", Gubun);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Common/WebFax/Admin/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            

        }

        /// <summary>
        /// 삭제
        /// </summary>
        private void Del()
        {
            DAL.FaxManager dal = new DAL.FaxManager();
            bool response = dal.Del(CurrentUserInfo.CompanyID, FaxID);

            if (response)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();                
                paraDic.Add("RecordPerPage", RecordPerPage.ToString());
                paraDic.Add("searchKey", Server.UrlEncode(SearchKey));
                paraDic.Add("searchValue", Server.UrlEncode(SearchValue));
                paraDic.Add("order", Order);
                paraDic.Add("gubun", Gubun);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Common/WebFax/Admin/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)


        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            LoadParam();

            string args = eventArgs;

            switch (args)
            {
                case "":
                    if (FaxID > 0)
                    {
                        Mod();
                    }
                    else
                    {
                        Add();
                    }

                    break;
                case "del":
                    Del();
                    break;
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

        #region Property (프로퍼티)

        #endregion
    }
}