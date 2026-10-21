using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class AddParam : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        public int PortletID;
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddParam).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region Method
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 이현수
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "ENCRIPTION");
            ddlEncript.Items.Clear();
            ddlEncript.AppendDataBoundItems = true;
            ddlEncript.Items.Add(new ListItem(resMng.GetString("NotUsed"), "")); //사용안함

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlEncript.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            dv = null;

            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "LDAP");
            ddlLDAP.Items.Clear();
            ddlLDAP.AppendDataBoundItems = true;
            ddlLDAP.Items.Add(new ListItem(resMng.GetString("SearchTimeFromTo"), "")); //직접입력

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlLDAP.Items.Add(new ListItem(dv[i]["CodeID"].ToString(), dv[i]["CodeID"].ToString()));
            }

            dv = null;

            //파라미터 연결 대상( U : UI / D : Data / L : Link)
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TargetType");
            ddlTargetType.Items.Clear();           

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlTargetType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }

            dv = null;

            //연결대상이 Data일때 (R : RSS / S : SQL)
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TargetDataType");
            ddlTargetDataType.Items.Clear();

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlTargetDataType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
        }


        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            PortletID = GetDecryptParam("portletID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Closed");
            }
        }
        #endregion
    }
}