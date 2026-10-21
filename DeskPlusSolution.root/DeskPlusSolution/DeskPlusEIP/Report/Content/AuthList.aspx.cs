using System;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class AuthList : Base.BasePageReport
    {
        public string AuthArrayList = "";
        public string AuthTable = "";
        public string MaxRowMax = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.AuthList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

            #region 이미지 처리

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";

            #endregion 이미지 처리
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
                CbtnAdd.Text = resMng.GetString("Add");
                CbtnMod.Text = resMng.GetString("Save");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 설정하기
        /// </summary>
        private void LoadAuthority()
        {
            Zeospace.DeskPlus.Report.DAL.ShareUser dal = new DAL.ShareUser();
            DataTableReader dtr = dal.GetShareUserList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
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
                            dtr["TargetNameBase"].ToString(),
                            dtr["TargetNameKo"].ToString(),
                            dtr["TargetNameEn"].ToString(),
                            dtr["TargetNameJa"].ToString(),
                            dtr["TargetNameFr"].ToString(),
                            dtr["TargetNameZhCn"].ToString(),
                            dtr["TargetNameZhTw"].ToString(), PreLang);

                        #endregion 다국어 이름 설정

                        sb.Append(dtr["TargetUserID"].ToString()
                            + "$" + dtr["UseComplete"].ToString()
                            + "$" + name
                            + ";");

                        AuthTable += CreateTableAuth(i,
                            dtr["TargetUserID"].ToString().ToNumber<int>(),
                            dtr["UseComplete"].ToString(), name);

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
        /// <param name="_useComplete">The _use complete.</param>
        /// <param name="_name">The _name.</param>
        /// <returns></returns>
        private string CreateTableAuth(int _idx, int _targetUserID, string _useComplete, string _name)
        {
            string selected1 = "";
            string selected2 = "";

            switch (_useComplete)
            {
                case "0":
                    selected1 = "selected='selected'";
                    break;

                case "1":
                    selected2 = "selected='selected'";
                    break;
            }

            string tempStr = "<tr id='boardAuth" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'>"
            + "<select name='ddlListType' id='ddlListType" + _idx + "' class='searchselect form-control'>"
            + "	<option " + selected1 + " value='" + _targetUserID + "$0'>" + GetResource("ShareUser") + "</option>"
            + "	<option " + selected2 + " value='" + _targetUserID + "$1'>" + GetResource("ReportUser") + "</option>"
            + "</select></td>"
            + @"<td class='center'><a class='btn' onclick=""AuthList.DelAuthRow(" + _idx + ",'" + _targetUserID + "$" + _useComplete + @"')"">" + resMng.GetString("Del") + "</a></td>"
        + "</tr>";

            return tempStr;
        }
    }
}