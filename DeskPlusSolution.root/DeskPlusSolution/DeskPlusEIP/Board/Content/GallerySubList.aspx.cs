using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class GallerySubList : Base.BasePageBoard
    {
        #region 변수설정

        public int ListID = 0;
        public string ImageHtml = "";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.GallerySubList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            DAL.AttachFiles dal = new DAL.AttachFiles();
            List<BE.AttachFileInfo> al = dal.GetAttachImageLists(BoaComID, CategoryID, ListID);

            string filePath = "";
            string fileGUID = "";
            string fileRealName = ""; //guid.fileType
            string thumnails = "";

            string encThumParams = "";
            string encParams = "";

            StringBuilder sb = new StringBuilder();
            //sb.Append(@"<div id=""container"">");
            //sb.Append(@"<div class=""thumbnails yoxview"">");
            foreach (BE.AttachFileInfo info in al)
            {
                filePath = info.FilePath;
                fileGUID = info.FileGUID;

                fileRealName = fileGUID.Replace(".zeo", info.FileType);//guid.gif

                if (System.IO.File.Exists(Server.MapPath(filePath + fileRealName)) == false)
                {
                    if (fileGUID.IndexOf(".zeo") != -1)
                    {
                        if (System.IO.File.Exists(Server.MapPath(filePath + fileGUID)))
                        {
                            System.IO.File.Copy(
                                Server.MapPath(filePath + fileGUID),
                                Server.MapPath(filePath + fileRealName), true);
                        }
                    }
                }

                fileGUID = fileRealName;

                if (System.IO.File.Exists(Server.MapPath(filePath + fileGUID)))
                {
                    thumnails = filePath + "Thum_" + fileGUID;

                    if (!System.IO.File.Exists(Server.MapPath(thumnails)))
                    {
                        if (System.IO.File.Exists(Server.MapPath(filePath + fileGUID)))
                        {
                            string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(filePath + fileGUID), 160, 160);

                            byte[] imgByte = GetImageByteArr(new Bitmap(Server.MapPath(filePath + fileGUID)));
                            MemoryStream memoryStream = new MemoryStream();
                            memoryStream.Write(imgByte, 0, imgByte.Length);
                            System.Drawing.Image imagen = System.Drawing.Image.FromStream(memoryStream);

                            System.Drawing.Image thumb = imagen.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                            delegate() { return false; }, (IntPtr)0);

                            thumb.Save(Server.MapPath(thumnails));
                        }
                    }

                    //ie 전용
                    if (Request.Browser.Browser.Contains("IE"))
                    {
                        //이미지 경로 암호화
                        Dictionary<string, string> paraEncDic = new Dictionary<string, string>();
                        paraEncDic.Add("filePath", filePath + fileGUID);
                        encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraEncDic));
                        encParams = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                        //섬네일 이미지 경로 암호화
                        Dictionary<string, string> paraEncThumDic = new Dictionary<string, string>();
                        paraEncThumDic.Add("filePath", filePath + "Thum_" + fileGUID);
                        encThumParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraEncThumDic));
                        encThumParams = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encThumParams;

                        //<img src="../../Html/Js/SlidingPanelPhotowallGallery/thumbs/1.jpg" alt="../../Html/Js/SlidingPanelPhotowallGallery/images/1.jpg" title="Beatriz_Florido_01 © studio.es" />

                        sb.Append(string.Format(@"<img src='{0}' alt='{1}' title='{2}' />", encThumParams, encParams, fileGUID));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<img src='{0}' alt='{1}' title='{2}' />", filePath + "Thum_" + fileGUID, filePath + fileGUID, fileGUID));
                    }
                }
            }

            //sb.Append(@"</div>");
            //sb.Append(@"</div>");

            ImageHtml = sb.ToString();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// bitmap 이미지를 byte 배열로 반환하기
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private byte[] GetImageByteArr(System.Drawing.Image img)
        {
            byte[] ImgByte;
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                ImgByte = stream.ToArray();
            }
            return ImgByte;
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
        }

        #endregion (함수)
    }
}