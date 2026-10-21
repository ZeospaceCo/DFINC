using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class DeptList : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 1;
        public string searchValue = "";
        public int targetDeptID = 0;
        public int targetTitleID = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.DeptList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "0");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            targetDeptID = GetDecryptParam("TargetDeptID", UserInfo.DeptID.ToString()).ToNumber<int>();
            targetTitleID = GetDecryptParam("TargetTitleID", UserInfo.CodeTitleID.ToString()).ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-22
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadData()
        {
            LoadParam();

            tbSearch.Value = searchValue;

            //사용자 부서 정보 가져오기
            ddlDept.Items.Clear();

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTDEPT");
            proc = string.Format(proc, CompanyID, UserID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserDept", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string DepName = this.GetLangName(
                               dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                               dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                               dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                               dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    if (DepName.Length > 15)
                    {
                        DepName = DepName.Substring(0, 13) + "..";
                    }

                    ddlDept.Items.Add(new ListItem(DepName, dr["DeptID"].ToString() + "^" + dr["CodeTitleID"].ToString()));
                }
            }

            ddlDept.SelectedValue = targetDeptID + "^" + targetTitleID;

            GetListCount(targetDeptID, targetTitleID);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount(int _deptID, int _titleID)
        {
            Ws.Data.List dal = new Ws.Data.List();
            int Total = dal.GetPublicListCount(CompanyID, UserID, _deptID, _titleID, searchValue);

            int recsInLastPage = Total % 10;
            int TotalPage = 0;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
            {
                TotalPage = (Total / 10);
            }
            else
            {
                TotalPage = (Total / 10) + 1;
            }

            if (TotalPage < pageNo)
            {
                pageNo = 1;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = pageNo.ToString();
        }
    }
}