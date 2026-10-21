using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Menu
{
    public partial class AdminMenu : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        public string AMenuImage;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Menu.AdminMenu).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadImage(CurrentUserInfo.CompanyID);
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-03-15
        /// 작성목적 : Loads the Image
        /// </summary>
        /// <param name="_companyID"></param>
        private void LoadImage(int _companyID)
        {
            string AMenuImageCnt = "";
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<DeskPlus.Common.BE.CompanyInfo>();
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            al = dal.GetCompany(CurrentUserInfo.CompanyID, false);

            if (al.Count > 0)
            {
                foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
                {                   
                    AMenuImageCnt = info.Alogo;
                }
            }

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");

            //기본 이미지 
            AMenuImage = this.BaseImgPath + "/Portal/Alogo.gif";

            //회사별 이미지
            string ciAMenuImage = baseFileDirectory + _companyID + "/Alogo.gif";

            #region Login / Menu 이미지
            if (System.IO.File.Exists(Server.MapPath(ciAMenuImage)))
            {
                AMenuImage = ciAMenuImage + "?v=" + AMenuImageCnt;
            }

            /*
            Dictionary<string, string> paraDic = new Dictionary<string, string>(); paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", AMenuImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            AMenuImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            */
            #endregion
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}