using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Folder
{
    public partial class ModSeq : Base.BasePageApprovalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string FolderList = "";
        public int FolderID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Folder.ModSeq).Assembly;	//어셈블리 선언
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
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
        }
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = this.resMng.GetString("Confirm");
                cbtnClose.Text = this.resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.Folder fDal = new DAL.Folder();
            List<BE.FolderInfo> fsList = fDal.GetSubFolderList(CurrentUserInfo.CompanyID, FolderID, false);

            if (fsList != null)
            {
                foreach (BE.FolderInfo info in fsList)
                {
                    sb.Append("<li id='");
                    sb.Append(info.FolderID.ToString());
                    sb.Append("'>");
                    sb.Append(GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang));
                    sb.Append("</li>");
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