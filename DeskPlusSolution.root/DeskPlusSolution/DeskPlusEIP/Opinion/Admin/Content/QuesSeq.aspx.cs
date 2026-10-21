using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.Web.Admin.Content
{
    public partial class QuesSeq : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string FolderList = "";
        public int QuesID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesSeq).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            //파라미터
            QuesID = GetDecryptParam("quesID", "0").ToNumber<int>();
        }
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");       //"적용";
                cbtnClose.Text = resMng.GetString("BtnClose");         //"닫기";
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.QuesUnit quDal = new DAL.QuesUnit();
            DataTableReader dtr = quDal.GetQuesUnitList(CurrentUserInfo.CompanyID, QuesID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<li id='");
                        sb.Append(dtr["QuesUnitID"].ToString());
                        sb.Append("'>");
                        sb.Append(dtr["Title"].ToString());
                        sb.Append("</li>");
                    }
                }
            }

            FolderList = sb.ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion        
    }
}