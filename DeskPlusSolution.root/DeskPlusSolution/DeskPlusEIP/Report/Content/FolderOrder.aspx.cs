using System;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class FolderOrder : Base.BasePageReport
    {
        #region 변수설정

        public string FolderList = "";        

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.FolderOrder).Assembly;	//어셈블리 선언

            if (!IsPostBack)
            {
                LoadParm();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)



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

            DAL.Folder dal = new DAL.Folder();
            var dtr = dal.GetParentFolder(CurrentUserInfo.CompanyID, FolderID, false);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        sb.Append("<li id='");
                        sb.Append(dtr["FolderID"].ToString());
                        sb.Append("'>");
                        sb.Append(this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS());
                        sb.Append("</li>");
                    }
                }
            }

            FolderList = sb.ToString();
        }

        #endregion (함수)
    }
}