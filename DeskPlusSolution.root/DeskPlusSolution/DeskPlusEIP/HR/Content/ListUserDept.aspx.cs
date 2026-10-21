using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Content
{
    public partial class ListUserDept : Base.BasePageHR
    {
        #region 변수설정
        public string UserDeptTable = "";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.ListCompany).Assembly;	    //어셈블리 선언
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
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 권한 체크

            cbtnAdmin.Visible = (AccessAuth) ? true : false;

            if (!AccessAuth)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            #endregion

            LoadParam();

            LoadUserDeptList();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자               
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            
        }

        private void LoadUserDeptList()
        {
            Zeospace.DeskPlus.Account.DAL.User userBiz = new Account.DAL.User();            

            DAL.UserDept dal = new DAL.UserDept();
            List<UserDeptInfo> alUser = dal.GetUserDept(CurrentUserInfo.CompanyID, 0);

            int i = 0;
            foreach (UserDeptInfo info in alUser)
            {
                // 겸직부서 가져오기
                List<Zeospace.DeskPlus.Account.BE.UserInfo> subUser = userBiz.GetUserInfo(CurrentUserInfo.CompanyID, info.UserID, true, false);

                string deptFormat = string.Empty;
                foreach (Zeospace.DeskPlus.Account.BE.UserInfo subinfo in subUser)
                {
                    deptFormat += "<option {0} value='" + subinfo.DeptID.ToString() + "'>" + subinfo.DeptBase + "</option>";
                    if (info.DeptID == subinfo.DeptID)
                    {
                        deptFormat = string.Format(deptFormat, "selected='selected'");
                    }
                    else
                    {
                        deptFormat = string.Format(deptFormat, "");
                    }
                }
                
                UserDeptTable += CreateTableAuth(i, 
                        info.UserID,
                        info.NameBase,
                        info.DeptID,
                        info.DeptBase,
                        deptFormat);

                i += 1;
            }

            if (UserDeptTable == "")
            {
                UserDeptTable = "<tr>";
                UserDeptTable += "<td class='center' colspan='5'>";
                UserDeptTable += GetResource("NoContent");
                UserDeptTable += "</td>";
                UserDeptTable += "</tr>";
            }
        }

        /// <summary>
        /// 겸직자 기본부서 할당하기
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_userID"></param>
        /// <param name="_userName"></param>
        /// <param name="_deptID"></param>
        /// <param name="_deptName"></param>
        /// <param name="_deptFormat"></param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, int _userID, string _userName, int _deptID, string _deptName, string _deptFormat)
        {
            string tempStr = "<tr>";
            tempStr += "<td class='center'>";
            if (_deptID > 0)
            {
                tempStr += GetResource("SetUp");
            }
            else
            {
                tempStr += "<span style='color: red'>" + GetResource("NoSetUp") + "</span>";
            }
            tempStr += "</td>";

            tempStr += "<td class='center' style='font-weight: bold'>";
            if (_deptName != "")
            {
                tempStr += _deptName;
            }
            else
            {
                tempStr += "&nbsp;";
            }
            tempStr += "</td>";

            tempStr += "<td class='center'>";
            tempStr += _userName;
            tempStr += "</td>";

            tempStr += "<td class='center'>";
            tempStr += "<select name='ddlBaseDept' id='ddlBaseDept" + _idx + "' class='searchselect form-control'>";
            tempStr += _deptFormat;
            tempStr += " </select>";
            tempStr += "</td>";

            tempStr += @"<td class='center'><a href='#' onclick=""ListUserDept.Add(" + _idx + ",'" + _userID + @"')"" class='btn'>" + GetResource("Select") + "</a></td>";
            
            tempStr += "</tr>";

            return tempStr;
        }

        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)       

        #endregion
    }
}