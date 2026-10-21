using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Task.DAL;

namespace Zeospace.DeskPlus.Task.Web.Admin.Source
{
    public partial class Updown : Base.BasePageTaskAdmin
    {

        #region 전역변수
        public string SourceList = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Source.Updown).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Data Load (데이터를 컨트롤에 바인딩 하는 메서드)
        public void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.Source dal = new DAL.Source();

            List<BE.SourceInfo> source = dal.GetSource(CurrentUserInfo.CompanyID, false);

            foreach (var info in source)
            {
                string name = GetLangName(info.NameBase.ToNull(), info.NameKo.ToNull(),
                    info.NameEn.ToNull(), info.NameJa.ToNull(),
                    info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                    info.NameZhtw.ToNull(), PreLang);

                sb.Append("<li id='");
                sb.Append(info.SourceID.ToString());
                sb.Append("'>");
                sb.Append(name);
                sb.Append("</li>");
            }

            SourceList = sb.ToString();
        }
        #endregion

        #region 함수
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("SourceSetting");
            }
        }
        #endregion

    }
}