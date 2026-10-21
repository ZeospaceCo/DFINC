using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Text;
using System.Xml;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class ShortReply : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        public string PageNo;
        public string PageListUrl;
        public int UnitID;
        public int TemplateID;        
        public string Status = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.ShortReply).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

            //cbsShortReply.Icon = BaseImgPath + "/Board/Button/but_write_short.gif";
            //cbsCancel.Icon = BaseImgPath + "/Board/Button/but_close.gif";
        }
        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 데이터 로드
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-09
        /// </summary>
        private void LoadData()
        {

        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            PageNo = GetDecryptParam("pageNo", "0");
            PageListUrl = GetDecryptParam("pageListUrl", "List.aspx");
            UnitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            TemplateID = GetDecryptParam("templateID", "0").ToNumber<int>();            
            Status = GetDecryptParam("status", "0");
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
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        #endregion

        #region Property (프로퍼티)


        #endregion

        #region EVENT HANDLER (이벤트핸들러)


        #endregion
    }
}
