using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.PMS.Web.Unit
{
    public partial class MoveUpdown : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정                
        public string FolderList = "";        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Unit.MoveUpdown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Apply");
                cbtnClose.Text = resMng.GetString("Close");
                ltlTitle.Text = resMng.GetString("ChangeOrder");
                
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();
            
            Zeospace.DeskPlus.PMS.DAL.Unit dal = new DAL.Unit();
            DataTableReader dtr = dal.GetUnit(CurrentUserInfo.CompanyID, ProjectID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<li id='");
                        sb.Append(dtr["UnitID"].ToString());
                        sb.Append("'>");
                        sb.Append(dtr["Subjects"].ToString().ToReverseXSS());
                        sb.Append("</li>");
                    }
                }
            }

            FolderList = sb.ToString();
        }

        #endregion

        #region (속성)

        #endregion
    }
}