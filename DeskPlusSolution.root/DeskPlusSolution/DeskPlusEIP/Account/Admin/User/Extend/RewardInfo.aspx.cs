using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.Admin.User.Extend
{
    public partial class RewardInfo : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.Extend.RewardInfo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
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
            DataView dv = null;
            #region 관계
            ddlGubun.Items.Clear();
            ddlGubun.AppendDataBoundItems = true;
            ddlGubun.Items.Add(new ListItem("---", ""));
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "C00000");

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlGubun.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
            #endregion
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
                //cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");         //"기본정보";
                cbtnDept.Text = resMng.GetString("DeptInfo");                        //"부서정보";
                cbtnExtend.Text = resMng.GetString("AddBaseInfo");                   //"기본추가정보";
                cbtnOrder.Text = resMng.GetString("ChangeOrders");                   //"순서변경";
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}