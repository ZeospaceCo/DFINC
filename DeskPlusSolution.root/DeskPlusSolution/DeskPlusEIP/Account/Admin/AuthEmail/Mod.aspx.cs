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
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Admin.AuthEmail
{
    public partial class Mod : Base.BasePageAccountAdmin
    {

        #region 변수설정
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string MaxRowMax = "0";
        public string NowManagerID = "";
        private string eventArgs = String.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.AuthEmail.Mod).Assembly;	//어셈블리 선언
            #endregion
            cbtnSubmit.Text = GetResource("Admin_UserRegister_Save");            

            if (!IsPostBack)
            {
                
                SetLanguage();
                LoadData();
                
            }
        }

        private void LoadData()
        {

            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnAdd.Text = GetResource("Admin_UserList_cbsUserAdd");
                cbtnSubmit.Text = resMng.GetString("BtnSave");         //"확인";              
            }

            CbtnAdd.Text = GetResource("Admin_UserList_cbsUserAdd");

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;

            LoadManager();

        }

        private void LoadManager()
        {

            Zeospace.DeskPlus.Account.DAL.PasswordManager dal = new DAL.PasswordManager();
            DataTableReader dtr = dal.GetManager(CurrentUserInfo.CompanyID);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 0;
                    while (dtr.Read())
                    {
                        #region 다국어 이름 설정
                        NowManagerID = dtr["UserID"].ToString(); ///현재 관리자가 있는지 확인

                        string name = dtr["UserName"].ToString();

                        #endregion 다국어 이름 설정

                        //(G$1$영업부,영업부)
                        sb.Append("M"
                            + "$" + dtr["UserID"].ToString()
                            + "$" + name
                            + ";");

                        AuthTable += CreateTableAuth(i, "M"
                            + "$" + dtr["UserID"].ToString(),
                            name, dtr["TitleBase"].ToString(), dtr["mail"].ToString(), dtr["DeptBase"].ToString());

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
        /// <param name="_auth">The _auth.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, string _keyID, string _name,string _title, string _email, string _dept )
        {
            string tempStr = "<tr id='UserAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name + " / " + _dept + " / " + _title + " / " + _email
            + "</td>"
            + "<td class='center'><a class='btn' onclick=\"Mod.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + "')\">" + GetResource("Admin_CompanyManager_btnDel") + "</a></td>"
            //+ "<td class='center'><a onclick=\"Mod.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + "')\"><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
            + "</tr>";

            return tempStr;
        }


        #region (이벤트핸들러)
        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
           // GetParam();

            string args = eventArgs;
            switch (args)
            {
                case "Modify":  //수정
                    Modify();
                    break;

                case "Register":  //추가
                    Register();
                    break;
                case "Delete":  //삭제
                    Delete();
                    break;
            }

            //게시판 카테고리 정보 전역캐쉬 삭제
         
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

        #region 함수
        /// <summary>
        /// 저장
        /// </summary>
        private void Register()
        {
            try
            {
                Zeospace.DeskPlus.Account.DAL.User dal = new DAL.User();
                Zeospace.DeskPlus.Account.DAL.PasswordManager dalManager = new DAL.PasswordManager();

                string managerID = tbAuthValue.Text.ToString().Split('^')[0].ToString();
                string managerName = "";

                DataTableReader dtr = dal.GetActiveUser(CurrentUserInfo.CompanyID, Convert.ToInt32(managerID));

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            managerName = dtr["NameBase"].ToString();
                        }
                    }
                }

                dalManager.Add(CurrentUserInfo.CompanyID, Convert.ToInt32(managerID), managerName, CurrentUserInfo.UserID);

                string url = @"location.href='{0}'";
                url = string.Format(url, "/DeskPlusEIP/Account/Admin/AuthEmail/Mod.aspx");
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteSave"), url);
            }
            catch (Exception ex)
            {
                 Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);             
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        private void Modify()
        {

            try
            {
                Zeospace.DeskPlus.Account.DAL.User dal = new DAL.User();
                Zeospace.DeskPlus.Account.DAL.PasswordManager dalManager = new DAL.PasswordManager();

                string managerID = tbAuthValue.Text.ToString().Split('^')[0].ToString();
                string managerName = "";

                DataTableReader dtr = dal.GetActiveUser(CurrentUserInfo.CompanyID, Convert.ToInt32(managerID));

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            managerName = dtr["NameBase"].ToString();
                        }
                    }
                }

                dalManager.ModManager(CurrentUserInfo.CompanyID, Convert.ToInt32(managerID), managerName, CurrentUserInfo.UserID);

                string url = @"location.href='{0}'";
                url = string.Format(url, "/DeskPlusEIP/Account/Admin/AuthEmail/Mod.aspx");
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteModify"), url);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private void Delete()
        {
            try
            {
                Zeospace.DeskPlus.Account.DAL.PasswordManager dal = new DAL.PasswordManager();
                dal.DelManager(CurrentUserInfo.CompanyID);

                string url = @"location.href='{0}'";
                url = string.Format(url, "/DeskPlusEIP/Account/Admin/AuthEmail/Mod.aspx");
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteSave"), url);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        } 
        #endregion

        #region 프로퍼티
       
        #endregion
    }
}