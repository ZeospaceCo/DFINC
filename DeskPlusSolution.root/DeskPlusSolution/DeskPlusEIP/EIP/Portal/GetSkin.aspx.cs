using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class GetSkin : Base.BasePageEIP
    {
        #region Declaration (컨트롤 선언)
        
        #endregion

        #region 변수선언
        public int TabID = 0;   // 탭아이디
        public string MailProduct = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";     //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.GetSkin).Assembly;	    //어셈블리 선언
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
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 데이터 바인딩
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadData()
        {
            //LoadParam();

            SelSkin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

            //switch (skin.ToLower())
            //{
            //    case "blue":
            //        rbLSkin1.Checked = true;
            //        break;
            //    case "green":
            //        rbLSkin2.Checked = true;
            //        break;
            //    case "red":
            //        rbLSkin3.Checked = true;
            //        break;                
            //    default :
            //        rbLSkin1.Checked = true;
            //        break;
            //}


            //DAL.Skin biz = new DAL.Skin();
            //DataSet ds = biz.GetSkin(CurrentUserInfo.CompanyID, TabID, CurrentUserInfo.UserID);

            //if (ds != null)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        string setColor = ds.Tables[0].Rows[0]["SkinColor"].ToString();

            //        foreach(Control _be in form1.Controls)
            //        {
            //            if (_be.ID != null)
            //            {
            //                if (_be.ID.Length > 4)
            //                {
            //                    if (_be.ID.Substring(0, 4) == "skin")
            //                    {
            //                        if (((System.Web.UI.HtmlControls.HtmlInputRadioButton)_be).Value == setColor)
            //                        {
            //                            ((System.Web.UI.HtmlControls.HtmlInputRadioButton)_be).Checked = true;
            //                        }                                   
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 언어 설정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void SetLanguage()
        { 
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnMail.Text = this.resMng.GetString("BtnSkinMail");
                CbtnConfirm.Text = this.resMng.GetString("Confirm"); // "확인";
                CbtnClose.Text = this.resMng.GetString("Closed"); // "닫기";
            }

            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();
            cbtnMail.Visible = (MailProduct.ToUpper().IndexOf("DEEP").Equals(0)) ? true : false;
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            //TabID = (TabID == 0) ? 1 : TabID;
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 작성자  : freeman
        /// 작성일자 : 2012-07-14
        /// 작성목적 : 스킨 
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        public string SelSkin
        {
            get { return GetViewStateString("SelSkin", "C1"); }
            set { ViewState["SelSkin"] = value; }
        }
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}