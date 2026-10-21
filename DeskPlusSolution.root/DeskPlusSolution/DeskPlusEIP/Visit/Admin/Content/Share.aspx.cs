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

namespace Zeospace.DeskPlus.Visit.Web.Admin.Content
{
    public partial class Share : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisitAdmin
    {
        #region 변수처리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string MaxRowMax = "0";
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Admin.Content.Share).Assembly;	//어셈블리 선언
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
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;

            LoadShare();
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnSave.Text = resMng.GetString("Save");     
                CbtnAdd.Text = resMng.GetString("Add");       
            }

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 권한 설정하기
        /// </summary>
        private void LoadShare()
        {
            DAL.Share bll = new DAL.Share();
            DataTableReader dtr = bll.GetShare(CurrentUserInfo.CompanyID);
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
                            dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(),
                            dtr["NameZhCn"].ToString(),
                            dtr["NameZhTw"].ToString(), PreLang);
                        #endregion

                        //(G$1$영업부)
                        sb.Append(dtr["Type"].ToString()
                            + "$" + dtr["OwnerID"].ToString()
                            + "$" + name
                            + ";");

                        AuthTable += CreateTableAuth(i, dtr["Type"].ToString() + "$" + dtr["OwnerID"].ToString(), name);

                        i += 1;
                    }

                    MaxRowMax = i.ToString();
                }
            }

            AuthArrayList = sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 테이블 그리기
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_keyID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, string _keyID, string _name)
        {

            string tempStr = "<tr id='visitShare" + _idx + "'>"
			+ "<td class='center'>"
			+ _name
			+ "</td>"
            + @"<td class='center'><a class='btn' onclick=""Share.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"
            //+ @"<td class='center'><a onclick=""Share.DelAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
		    + "</tr>";

            return tempStr;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 기본공유자 설정
        /// </summary>
        private void Register()
        {          
            List<BE.ShareInfo> alAuth = new List<BE.ShareInfo>();
            
            string shareValue = tbShareValue.Text.Trim();
            if (shareValue.Length > 0)
            {
                foreach (string _shareValue in shareValue.Split(','))
                {
                    //구분$키값$이름
                    string[] authValStr = _shareValue.Split('$');

                    BE.ShareInfo shareInfo = new BE.ShareInfo();
                    shareInfo.CompanyID = CurrentUserInfo.CompanyID;                    
                    shareInfo.Type = authValStr[0];
                    shareInfo.OwnerID = authValStr[1].ToNumber<int>();                    
                    shareInfo.WriteUserID = CurrentUserInfo.UserID;
                    shareInfo.WriteUserName = CurrentUserInfo.Name;

                    alAuth.Add(shareInfo);
                }
            }        
            
            BLL.Share bll = new BLL.Share();
            bll.Add(alAuth, CurrentUserInfo.CompanyID);

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteConfirm"), "location='" + Request.Url.ToString() + "';");

        }

        #endregion


        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {

            string args = eventArgs;
            switch (args)
            {
                case "Register":  
                    Register();
                    break;
            }
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
    }
}