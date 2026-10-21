using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ShortReply : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정


        public int ScheduleID = 0;

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ShortReply).Assembly;	//어셈블리 선언
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
            ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }


        private void LoadData()
        {
            LoadParam();
            /*
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

            string optionUseDept = "";
            string optionUseShortReply = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    optionUseDept = dr["OptionUseDept"].ToString();
                    optionUseShortReply = dr["OptionUseShortReply"].ToString();
                }
            }

            //댓글 활성화 여부
            if (optionUseShortReply == "1")
            {
                BLL.Category bizCat = new BLL.Category();
                NameValueCollection nvcAuth = new NameValueCollection();

                //댓글 쓰기(3,4) 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    CurrentUserInfo.DeptID, CategoryID, optionUseDept, false, "3,4", true, false);

                if (nvcAuth.AllKeys.Length == 0)
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }
            else
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }*/
        }
        #endregion

        #region (함수)

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}