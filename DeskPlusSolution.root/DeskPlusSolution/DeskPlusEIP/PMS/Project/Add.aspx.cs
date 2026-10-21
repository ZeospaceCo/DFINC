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
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class Add : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.Add).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
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
            GetUserInfo();
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성자   : 심재운
        /// </summary>
        private void GetUserInfo()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id				
            DeptID = CurrentUserInfo.DeptID;		//부서 id
            UserName = CurrentUserInfo.Name;		//사용자 이름
            DeptName = CurrentUserInfo.DeptName;	//부서명
            TitleName = CurrentUserInfo.TitleName;	//직급명
            DutyName = CurrentUserInfo.DutyName;	//직책명
            #endregion

            #region 유저 부서

            string userDepts = "";
            string[] userDeptArr = CurrentUserInfo.UserDepts;	//arr 유저의 부서
            for (int i = 0; i < userDeptArr.Length; i++)
            {
                if (userDepts.Length > 0) userDepts += ",";
                userDepts += userDeptArr[i];
            }
            UserDepts = userDepts;	//유저의 부서 id(,구분)

            #endregion            
        }        


        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                ltlDate.Text = resMng.GetString("List_PlanDate");
                ltlFolder.Text = resMng.GetString("List_ltlFolder");
                ltlOpen.Text = resMng.GetString("List_IsOpen");
                ltlTitle.Text = resMng.GetString("List_ProjectName");

                CbtnConfirm.Text = resMng.GetString("Request"); // 신청
                CbtnFolder.Text = resMng.GetString("Module_Folder_Node1"); //폴더관리
            }
        }


        #endregion

        #region Property (프로퍼티)


        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 회사아이디
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return (int)(ViewState["CompanyID"] ?? 0); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 사용자아이디
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return (int)(ViewState["UserID"] ?? 0); }
            set { ViewState["UserID"] = value; }
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 부서아이디
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The dept ID.</value>
        public int DeptID
        {
            get { return (int)(ViewState["DeptID"] ?? 1); }
            set { ViewState["DeptID"] = value; }
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 이름
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return (string)(ViewState["UserName"] ?? ""); }
            set { ViewState["UserName"] = value; }
        }

        /// <summary>
        /// 사용자 부서
        /// </summary>
        public string UserDepts
        {
            get { return this.GetViewStateString("UserDepts", ""); }
            set { ViewState["UserDepts"] = value; }
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 부서명
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The name of the dept.</value>
        public string DeptName
        {
            get { return (string)(ViewState["DeptName"] ?? ""); }
            set { ViewState["DeptName"] = value; }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 직급명
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The name of the dept.</value>
        public string TitleName
        {
            get { return (string)(ViewState["TitleName"] ?? ""); }
            set { ViewState["TitleName"] = value; }
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 직책명
        /// 작성자   : 심재운
        /// </summary>
        /// <value>The name of the duty.</value>
        public string DutyName
        {
            get { return (string)(ViewState["DutyName"] ?? ""); }
            set { ViewState["DutyName"] = value; }
        }  


        

        #endregion


        #region Event Handler (이벤트 핸들러)





        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 추가, 답변 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private BE.ProjectInfo WriteMode()
        {
            string title = tbxTitle.Text.Trim();
            string content = tbxContent.Text;

            BE.ProjectInfo info = new Zeospace.DeskPlus.PMS.BE.ProjectInfo();
            info.CompanyID = CompanyID;
            info.ProjectID = 0;
            info.FolderID = tbxFolderID.Text.ToNumber<int>();
            info.ChargeDeptID = "";

            //다국어 개발 필요
            info.SubjectBase = tbxTitle.Text;

            info.Contents = tbxContent.Text;
            //비공개 0
            info.IsOpen = (cbxIsOpen.Checked) ? 0 : 1;

            DateTime dtStartTime = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper.GetCultureDateConvertString(tbxPlanStartDate.Text, "", PreLangReal);
            DateTime dtEndTime = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper.GetCultureDateConvertString(tbxPlanEndDate.Text, "", PreLangReal);

            info.PlanStartDate = dtStartTime;
            info.PlanEndDate = dtEndTime;            

            info.Process = 0;
            //(1:대기;2:진행;3:완료;4:지연;5:보류)
            info.Status = 1;
            info.IsApproval = 0;
            info.Opinion = "";
            //(0:일반;1:폐쇄신청;2:폐쇄완료;3:폐쇄거절)
            info.IsClosed = "0";
            info.WriteUserID = UserID;


            return info;
        }


        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가글
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void lbtnWrite_Click(object sender, System.EventArgs e)
        {
            Zeospace.DeskPlus.PMS.BLL.Project bll = new Zeospace.DeskPlus.PMS.BLL.Project();

            string xmlPath = Server.MapPath("/DeskPlusEIP/PMS/App_Data/" + PreLang + "/ProjectMenuType.xml");
            List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

            int rtnListID = bll.Add(WriteMode(), al);

            if (rtnListID > 0)
            {
                Response.Redirect("JList.aspx", false);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }
        }

        #endregion
    }
}
