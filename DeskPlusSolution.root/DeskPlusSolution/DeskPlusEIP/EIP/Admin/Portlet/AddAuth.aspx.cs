using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class AddAuth : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        public int PortletID;
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string MaxRowMax = "0";
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddAuth).Assembly;	//어셈블리 선언
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
            LoadAuthority();

            UCRoleTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCRoleTreeScript1.UseIcon = true;
            UCRoleTreeScript1.UseCheckBox = true;

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
                cbtnClose.Text = resMng.GetString("Closed");
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnAdd.Text = resMng.GetString("Add");
            }

        }


        /// <summary>
        /// 작성자 : 심재운
        /// 권한 설정하기
        /// </summary>
        private void LoadAuthority()
        {
            StringBuilder sb = new StringBuilder();
            Zeospace.DeskPlus.EIP.DAL.PortletRole dal = new DAL.PortletRole();
            List<BE.PortletRoleInfo> al = dal.GetPortletRole(CurrentUserInfo.CompanyID, PortletID);

            int i = 1;

            foreach (BE.PortletRoleInfo info in al)
            {
                sb.Append(
                    info.RoleID
                    + "$" + info.RoleName
                    + ";");

                AuthTable += CreateTableAuth(
                    i,
                    info.RoleID.ToString(),
                    info.RoleName);

                i += 1;
            }

            MaxRowMax = i.ToString();
            AuthArrayList = sb.ToString().TrimEnd(';');

        }


        /// <summary>
        /// 카테고리별 권한 테이블 그리기
        /// </summary>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_keyID">The _key ID.</param>
        /// <param name="_name">The _name.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, string _keyID, string _name)
        {
            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'>"
            + "<select name='ddlListType' id='ddlListType" + _idx + "'>"
            + "	<option selected='selected' value='" + _keyID + "'>" + resMng.GetString("Apply") + "</option>"
            + "</select></td>"
            + @"<td class='center'><a onclick=""AddAuth.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
            + "</tr>";

            return tempStr;
        }

        #endregion
    }
}