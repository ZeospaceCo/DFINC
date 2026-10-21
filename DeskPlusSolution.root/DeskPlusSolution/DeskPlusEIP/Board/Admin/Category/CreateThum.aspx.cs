using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Category
{
    public partial class CreateThum : Base.BasePageBoardAdmin
    {
        #region Declaration (컨트롤 선언)

        private int ListID = 0;
        private string Filepath = "";
        private string Filename = "";
        private string Filetype = "";
        private string Fileguid = "";

        #endregion Declaration (컨트롤 선언)



        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Category.CreateThum).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
            }
        }

        #endregion Page Process (페이지 처리)



        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Closed");
            }
        }

        /// <summary>
        /// 스크린샷 이미지 추가 하기
        /// </summary>
        private void Add()
        {
            try
            {
                DAL.AttachFiles dal = new DAL.AttachFiles();
                DataTableReader dtr = dal.GetAttachImageOne(CurrentUserInfo.CompanyID, CategoryID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ListID = dtr["listID"].ToString().ToNumber<int>();
                            Filename = dtr["FileName"].ToString();
                            Filepath = dtr["filePath"].ToString();
                            Filetype = dtr["filetype"].ToString();
                            Fileguid = dtr["fileguid"].ToString();

                            CreateThumImage();
                        }
                    }
                }

                Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("Processing"), "parent.disablePopup();");
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
            }
        }

        private void CreateThumImage()
        {
            string fromfile = "";
            string thum = "";

            if (Fileguid.ToLower().IndexOf(".zeo") != -1)
            {
                string tofile = "";

                fromfile = Filepath + Fileguid;
                tofile = Filepath + Fileguid.Replace(".zeo", Filetype);
                thum = Filepath + "Thum_" + Fileguid.Replace(".zeo", Filetype);

                System.IO.File.Copy(Server.MapPath(fromfile), Server.MapPath(tofile), true);

                if (System.IO.File.Exists(Server.MapPath(tofile)))
                {
                    string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(tofile), 160, 160);

                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(tofile));
                    System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                        delegate() { return false; }, (IntPtr)0);

                    thumb.Save(Server.MapPath(thum));
                }
            }
            else
            {
                fromfile = Filepath + Fileguid;
                thum = Filepath + "Thum_" + Fileguid;

                if (System.IO.File.Exists(Server.MapPath(fromfile)))
                {
                    string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(fromfile), 160, 160);
                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(fromfile));
                    System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                    delegate() { return false; }, (IntPtr)0);

                    thumb.Save(Server.MapPath(thum));
                }
            }

            DAL.Lists dal = new DAL.Lists();
            dal.ModByScreenShot(CurrentUserInfo.CompanyID, CategoryID, ListID, thum);
        }

        #endregion (함수)

        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            Add();
        }
    }
}