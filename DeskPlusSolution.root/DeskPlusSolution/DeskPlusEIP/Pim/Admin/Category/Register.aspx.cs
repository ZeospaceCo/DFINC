using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.Admin.Category
{
    public partial class Register : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)
        public int StatusID;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PIM.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Category.Register).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지

            #endregion
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            SetColor();

            if (StatusID > 0) LoadList();
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
                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnDel.Text = resMng.GetString("Del");

                ddlActive.Items.Clear();
                ddlActive.Items.Add(new ListItem(resMng.GetString("Active"), "1"));
                ddlActive.Items.Add(new ListItem(resMng.GetString("NonActive"), "0"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            StatusID = GetDecryptParam("statusID", "0").ToNumber<int>();
        }


        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
        private void LoadList()
        {
            DAL.Status dal = new DAL.Status();
            List<StatusInfo> al = dal.GetList(CurrentUserInfo.CompanyID, StatusID);

            foreach (StatusInfo be in al)
            {
                ctbName.Text = be.NameBase.ToReverseXSS();
                
                ctbMuliLang.Text = GetMuiltiLang(
                        be.NameKo.ToReverseXSS(), be.NameEn.ToReverseXSS(), be.NameJa.ToReverseXSS(), be.NameFr.ToReverseXSS(), be.NameZhcn.ToReverseXSS(), be.NameZhtw.ToReverseXSS());

                ddlColor.Value = be.Colors;
                ddlActive.SelectedValue = be.UseActive;
                ctbStatusCode.Text = be.StatusCode;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 폰트 칼라
        /// </summary>
        private void SetColor()
        {
            ddlColor.Items.Clear();
            string xmlPath = Server.MapPath("/DeskplusEIP/Common/App_Data/" + PreLang + "/Common_FontColor.xml");
            List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

            foreach (XmlDataInfo be in al)
            {
                ListItem tmpItem = new ListItem(be.KeyValue, be.KeyID);
                tmpItem.Attributes.Add("style", "color:" + be.KeyID);
                this.ddlColor.Items.Add(tmpItem);
            }

            ddlColor.Items[0].Text = "----";
        }

        #endregion

        #region Property (프로퍼티)


        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}