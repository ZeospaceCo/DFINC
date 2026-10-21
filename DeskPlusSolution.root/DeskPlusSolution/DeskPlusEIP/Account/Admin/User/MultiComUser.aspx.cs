using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class MultiComUser : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID;
        public int MultiCompanyID;
        public int UserID;
        public string GroupGUID;//타회사그룹명

        public string AuthArrayList = "";
        public string UserDeptTable = "";               
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.MultiComUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            Zeospace.DeskPlus.Common.DAL.Company biz = new DeskPlus.Common.DAL.Company();
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = biz.GetCompany(true);
            ddlMultiCompany.Items.Clear();
            ddlMultiCompany.Items.Add(new ListItem("-----", "0"));
            ddlMultiCompany.AppendDataBoundItems = true;

            int firstCompany = 0;
            string companyName = "";
            foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
            {
                //타회사리스트만 출력한다.
                if (info.CompanyID != CompanyID)
                {
                    if(firstCompany == 0) firstCompany = info.CompanyID;

                    companyName = UtilHelper.GetLangName(
                        info.NameBase,
                        info.NameKo, info.NameEn, info.NameJa,
                        info.NameFr, info.NameZhcn,
                        info.NameZhtw, PreLang).ToReverseXSS();

                    ddlMultiCompany.Items.Add(new ListItem(companyName, info.CompanyID.ToString()));
                }
            }
            //타회사 선택값이 없으면 처음 회사를 선택해 준다.
            if (MultiCompanyID == 0) MultiCompanyID = firstCompany;
            ddlMultiCompany.SelectedValue = MultiCompanyID.ToString();

            //처음에 없다가.. 타회사 추가하고나서 새로고침하므로 다시 groupguid 를 가져와서 셋팅힌다.
            if (GroupGUID.Length == 0)
            {
                DAL.UserCompany dal = new DAL.UserCompany();
                List<BE.UserCompanyInfo> alList = dal.GetUserCompany(CompanyID, UserID);
                if (alList.Count > 0) GroupGUID = alList[0].GroupGUID;
            }
            
            LoadUserDeptList();

            DeptUserTreeScript1.CompanyID = MultiCompanyID;
            DeptUserTreeScript1.DeptID = 0;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxGroup = false;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;

        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            MultiCompanyID = GetDecryptParam("multiCompanyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            GroupGUID = GetDecryptParam("groupGUID", "");
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
                //cbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                cbtnSave.Text = GetResource("BtnSave");
                cbtnClose.Text = GetResource("Cancel");
                cbtnAdd.Text = GetResource("Admin_UserList_cbsUserAdd");
            }
        }

        /// <summary>
        /// 용도 : 부서,직급,직책
        /// 작성자 : 심재운
        /// </summary>
        private void LoadUserDeptList()
        {
            Zeospace.DeskPlus.Account.DAL.User userBiz = new DAL.User();
            Zeospace.DeskPlus.Account.DAL.UserCompany userComBiz = new DAL.UserCompany();
            List<BE.UserCompanyInfo> alUser = userComBiz.GetUserCompanyByGUID(MultiCompanyID, GroupGUID);

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (UserCompanyInfo info in alUser)
            {
                List<UserInfo> alUserList = userBiz.GetUserInfo(info.CompanyID, info.UserID, true, true);

                if (alUserList.Count == 0) continue;

                string name = this.GetLangName(
                    alUserList[0].NameBase,
                    alUserList[0].NameKo,
                    alUserList[0].NameEn,
                    alUserList[0].NameJa,
                    alUserList[0].NameFr,
                    alUserList[0].NameZhCn,
                    alUserList[0].NameZhTw, PreLang);

                string deptName = this.GetLangName(
                    alUserList[0].DeptBase,
                    alUserList[0].DeptKo,
                    alUserList[0].DeptEn,
                    alUserList[0].DeptJa,
                    alUserList[0].DeptFr,
                    alUserList[0].DeptZhCn,
                    alUserList[0].DeptZhTw, PreLang);


                //키는 deptid
                sb.Append(alUserList[0].UserID
                    + "$" + name
                    + "$" + alUserList[0].DeptID
                    + "$" + deptName
                    + ";");

                UserDeptTable += CreateTableAuth(i,
                    alUserList[0].UserID,
                    name,
                    alUserList[0].DeptID,
                    deptName);

                i += 1;

                AuthArrayList = sb.ToString().TrimEnd(';');
            }
        }

        /// <summary>
        /// 부서 및 직급,직책 할당하기
        /// </summary>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_keyID">The _key ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleID">The _title ID.</param>
        /// <param name="_dutyID">The _duty ID.</param>
        /// <param name="_baseDept">The _base dept.</param>
        /// <param name="_titleDs">The _title ds.</param>
        /// <param name="_dutyDs">The _duty ds.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, int _userID, string _name, int _deptID, string _deptName)
        {            
            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _deptName
            + "</td>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + @"<td class='center'><a class='btn' onclick=""MultiComUser.DelRow(" + _idx + ",'" + _userID + @"')"">" + GetResource("Admin_CompanyManager_btnDel") + "</a></td>"
            //+ @"<td class='center'><a onclick=""MultiComUser.DelRow(" + _idx + ",'" + _userID + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
            + "</tr>";

            return tempStr;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)



        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}