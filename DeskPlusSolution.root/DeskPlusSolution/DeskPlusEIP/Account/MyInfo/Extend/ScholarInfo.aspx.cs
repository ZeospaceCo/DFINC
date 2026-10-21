using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.MyInfo.Extend
{
    public partial class ScholarInfo : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Extend.ScholarInfo).Assembly;	//어셈블리 선언
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

            DataView dv = null;
            #region 학력사항
            ddlDegree.Items.Clear();
            ddlDegree.AppendDataBoundItems = true;
            ddlDegree.Items.Add(new ListItem("---", ""));
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "D00000");

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlDegree.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
            #endregion

            #region 졸업여부
            dv = null;
            ddlGraduateType.Items.Clear();
            ddlGraduateType.AppendDataBoundItems = true;
            ddlGraduateType.Items.Add(new ListItem("---", ""));
            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "D10000");

            for (int i = 0; i < dv.Count; i++)
            {
                string name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlGraduateType.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
            #endregion
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
                //cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");         //"기본정보";
                //cbtnDept.Text = resMng.GetString("DeptInfo");                        //"부서정보";
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