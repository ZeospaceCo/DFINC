using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Proposal.Web.Content.Admin
{
    public partial class AuthList : Base.BasePageProposalAdmin
    {
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string MaxRowMax = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Admin.AuthList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

            #region 이미지 처리
            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
            #endregion
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadAuthority();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {

        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                
            }
        }

        /// <summary>
        /// 작성목적   : 권한
        /// 작성자     : shine
        /// </summary>
        private void LoadAuthority()
        {
            DAL.Judge biz = new DAL.Judge();
            DataTableReader dtr = biz.GetJudgeList(CurrentUserInfo.CompanyID, 1);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        sb.Append(dtr["UserID"].ToString() 
                            + "$" + dtr["Name"].ToString()
                            + ";");

                        AuthTable += CreateTableAuth(i,
                            dtr["UserID"].ToString().ToNumber<int>(),
                            dtr["Name"].ToString());

                        i += 1;
                    }

                    MaxRowMax = i.ToString();
                }
            }

            AuthArrayList = sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 카테고리별 권한 테이블 그리기
        /// </summary>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_targetUserID">The _target user ID.</param>
        /// <param name="_name">The _name.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, int _userID, string _name)
        {

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td id='" + _userID + "'>"
            + _name
            + "</td>"
            + @"<td class='center'><a class='btn' onclick=""AuthList.DelAuthRow(" + _idx + ",'" + _userID + @"')"">삭제</a></td>"
            //+ @"<td><a onclick=""AuthList.DelAuthRow(" + _idx + ",'" + _userID + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
            + "</tr>";

            return tempStr;
        }
    }
}