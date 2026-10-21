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

namespace Zeospace.DeskPlus.Account.Web.Admin.User.UserDept
{
    public partial class List : Base.BasePageAccountAdmin
    {

        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string AuthArrayList = "";
        public string UserDeptTable = "";
        public string TitleTable = "";
        public string DutyTable = "";        
        public int UserID;
        public int DeptID;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.List).Assembly;	//어셈블리 선언
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
            LoadUserDeptList();

            UCDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptTreeScript1.DeptID = 0;
            UCDeptTreeScript1.UseActiveDept = true;
            UCDeptTreeScript1.UseCheckBox = true;
            UCDeptTreeScript1.UseIcon = true;

        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();            
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
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
                //CbtnAdd.Text = "추가";
                //cbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                cbtnAdd.Text = GetResource("Admin_UserList_cbsUserAdd");
                cbtnSubmit.Text = resMng.GetString("BtnSave");         //"확인";
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");                    //"기본정보";
                cbtnExtend.Text = resMng.GetString("AddBaseInfo");                              //"기본추가정보";
                cbtnDept.Text = resMng.GetString("DeptInfo");                                   //"부서정보";
                cbtnList.Text = resMng.GetString("List");                                       //"목록";
            }
        }

        /// <summary>
        /// 용도 : 부서,직급,직책
        /// 작성자 : 심재운
        /// </summary>
        private void LoadUserDeptList()
        {            
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            DataSet titleDs = codeDal.GetCodeByScope(CurrentUserInfo.CompanyID, "T", true);
            DataSet dutyDs = codeDal.GetCodeByScope(CurrentUserInfo.CompanyID, "D", true);

            foreach (DataRow _dr in titleDs.Tables[0].Rows)
            {
                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                        _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                TitleTable += "<option value='" + _dr["CodeID"].ToString() + "'>" + name + "</option>";
            }

            foreach (DataRow _dr in dutyDs.Tables[0].Rows)
            {
                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                        _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                DutyTable += "<option value='" + _dr["CodeID"].ToString() + "'>" + name + "</option>";
            }

            Zeospace.DeskPlus.Account.DAL.User userBiz = new DAL.User();
            List<UserInfo> alUser = userBiz.GetUserInfo(CurrentUserInfo.CompanyID, UserID, false, false); 
            
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (UserInfo info in alUser)
            {
                //부서값이 존재할 경우
                if (info.DeptID > 0)
                {
                    #region 다국어 이름 설정
                    string deptName = this.GetLangName(
                        info.DeptBase,
                        info.DeptKo, info.DeptEn, info.DeptJa,
                        info.NameFr, info.NameZhCn, info.NameZhTw, PreLang);
                    #endregion


                    //키는 deptid
                    sb.Append(info.DeptID
                        + "$" + deptName
                        + ";");

                    UserDeptTable += CreateTableAuth(i,
                        info.DeptID.ToString(),
                        info.DeptID.ToString(), deptName,
                        info.CodeTitleID.ToString(), info.CodeDutyID.ToString(),
                        info.BaseDept,
                        titleDs, dutyDs);

                    i += 1;

                    AuthArrayList = sb.ToString().TrimEnd(';');
                }
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
        private string CreateTableAuth(int _idx, string _keyID, string _deptID, string _deptName, 
            string _titleID, string _dutyID, string _baseDept, DataSet _titleDs, DataSet _dutyDs)
        {
            string sbTitle = "";
            foreach (DataRow _dr in _titleDs.Tables[0].Rows)
            {
                string titleName = this.GetLangName(
                    _dr["NameBase"].ToString(),
                    _dr["NameKo"].ToString(), 
                    _dr["NameEn"].ToString(), 
                    _dr["NameJa"].ToString(),
                    _dr["NameFr"].ToString(), 
                    _dr["NameZhCn"].ToString(), 
                    _dr["NameZhTw"].ToString(), PreLang);

                string titleFormat = "<option {0} value='" + _dr["CodeID"].ToString() + "'>" + titleName + "</option>";
                if (_titleID == _dr["CodeID"].ToString())
                {
                    titleFormat = string.Format(titleFormat, "selected='selected'");
                }
                else
                {
                    titleFormat = string.Format(titleFormat, "");
                }

                sbTitle += titleFormat;
            }

            string sbDuty = "";
            foreach (DataRow _dr in _dutyDs.Tables[0].Rows)
            {
                string dutyName = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                        _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                string dutyFormat = "<option {0} value='" + _dr["CodeID"].ToString() + "'>" + dutyName + "</option>";
                if (_dutyID == _dr["CodeID"].ToString())
                {
                    dutyFormat = string.Format(dutyFormat, "selected='selected'");
                }
                else
                {
                    dutyFormat = string.Format(dutyFormat, "");
                }

                sbDuty += dutyFormat;
            }

            string selBaseDept = (_baseDept == "1") ? "selected='selected'" : "";
            string selBaseEtcDept = (_baseDept == "0") ? "selected='selected'" : "";

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _deptName
            + "</td>"
            + "<td class='center'>"
            + " <select name='ddlTitle' id='ddlTitle" + _idx + "' class='searchselect form-control'>" + sbTitle + "</select>"
            + "</td>"
            + "<td class='center'>"
            + " <select name='ddlDuty' id='ddlDuty" + _idx + "' class='searchselect form-control'>" + sbDuty + "</select>"
            + "</td>"
            + "<td class='center'>"
            + " <select name='ddlBaseDept' id='ddlBaseDept" + _idx + "' class='searchselect form-control'>"
            + " <option value='1' " + selBaseDept + ">" + resMng.GetString("Basic") + "</option>"
            + " <option value='0' " + selBaseEtcDept + ">" + resMng.GetString("Both") + "</option>"
            + " </select>"
            + "</td>"
            + @"<td class='center'><a class='btn' onclick=""List.DelRow(" + _idx + ",'" + _keyID + "','" + _deptName + @"')"">" + GetResource("Admin_CompanyManager_btnDel") + "</a></td>"
            //+ @"<td class='center'><a onclick=""List.DelRow(" + _idx + ",'" + _keyID + "','" + _deptName + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
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