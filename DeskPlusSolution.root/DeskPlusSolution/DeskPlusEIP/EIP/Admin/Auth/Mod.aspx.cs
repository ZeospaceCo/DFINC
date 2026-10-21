using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Auth
{
    public partial class Mod : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string RecordPerPage = "5"; //게시판 리스트 개수        
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어        
        public string Order = string.Empty; //sort        

        public int RoleID;
        public string Gubun;
        public string AuthArrayList = "";
        public string AuthTable = "";
        private string MultiName = String.Empty;
        public string MaxRowMax = "0";

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
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Auth.Mod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            SetLanguage();

            LoadAuthority();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "codeorder asc" : Order;

            RoleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "0");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
                cbtnConfirm.Text = resMng.GetString("Save"); //확인
                cbtnList.Text = resMng.GetString("Cancel");  //목록
                cbtnAdd.Text = resMng.GetString("Add"); 
            }

            //cbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 설정하기
        /// </summary>
        private void LoadAuthority()
        {
            Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();
            DataTableReader dtr = dal.GetUserRoleList(CurrentUserInfo.CompanyID, RoleID);

            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        #region 다국어 이름 설정
                        string name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);
                        #endregion

                        string types = (dtr["Types"].ToString().Trim() == "GM") ? "M" : "G";

                        //(G$1$영업부,영업부)
                        sb.Append(
                            types
                            + "$" + dtr["UserID"].ToString()
                            + "$" + name
                            + ";");

                        AuthTable += CreateTableAuth(
                            i,
                            types + "$" + dtr["UserID"].ToString(),
                            name,
                            types);

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
        /// <param name="_keyID">The _key ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, string _keyID, string _name, string _types)
        {
            string gubun = (_types == "M") ? resMng.GetString("Users") : resMng.GetString("Dept");

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'>"
            + gubun
            + "</td>"
            + "<td class='center'>"
            + "<select name='ddlListType' id='ddlListType" + _idx + "' class='searchselect form-control'>"
            + "	<option selected='selected' value='" + _keyID + "'>" + resMng.GetString("Apply") + "</option>"
            + "</select></td>"
            + @"<td class='center'><a class='btn' onclick=""Mod.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"
            + "</tr>";

            return tempStr;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 수정
        /// </summary>
        private void Modify()
        {
            List<BE.UserRoleInfo> al = new List<BE.UserRoleInfo>();

            string authValue = tbAuthValue.Text.Trim();
            if (authValue.Length > 0)
            {
                foreach (string _authValue in authValue.Split(','))
                {
                    //권한값$구분$키값$이름
                    string[] authValStr = _authValue.Split('$');                    

                    BE.UserRoleInfo authInfo = new BE.UserRoleInfo();
                    authInfo.CompanyID = CurrentUserInfo.CompanyID;
                    authInfo.RoleID = RoleID;
                    authInfo.UserID = authValStr[1].ToNumber<int>();
                    authInfo.Types = (authValStr[0] == "G") ? "GG" : "GM";

                    al.Add(authInfo);
                }
            }

            BLL.UserRole biz = new BLL.UserRole();
            biz.AddTx(al);
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            LoadParam();
            Modify();

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("searchField", SearchField.ToString());
            paraDic.Add("searchValue", SearchValue.ToString());
            paraDic.Add("order", Order.ToString());
            paraDic.Add("roleID", RoleID.ToString());
            paraDic.Add("gubun", Gubun.ToString());

            string url = @"location='{0}';";
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

            if (Gubun == "HRDept")
            {
                url = string.Format(url, "/DeskPlusEIP/HR/Admin/Content/RoleAdmin.aspx?p=" + encParams);
            }
            else if (Gubun == "HRAttendance")
            {
                url = string.Format(url, "/DeskPlusEIP/HR/Admin/Content/AttendanceRoleAdmin.aspx?p=" + encParams);
            }
            else
            {
                url = string.Format(url, "/DeskPlusEIP/EIP/Admin/Auth/List.aspx?p=" + encParams);
            }

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "SUCCESS", GetResource("SUCCESS"), url);            
        }

        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}