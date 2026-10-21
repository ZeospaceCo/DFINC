using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using System.Drawing;
using System.Text;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class GallerySubList : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
        public string ImageHtml = "";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.GallerySubList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.BoaAttach dal = new DAL.BoaAttach();
            List<BE.BoaAttachInfo> al = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID);

            string filePath = "";
            string fileGUID = "";
            string fileRealName = ""; //guid.fileType
            string thumnails = "";

            string encThumParams = "";
            string encParams = "";

            StringBuilder sb = new StringBuilder();
            foreach (BE.BoaAttachInfo info in al)
            {
                if (GetCheckImage(info.FileType))
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
                            byte[] imgByte = GetImageByteArr(new Bitmap(Server.MapPath(filePath + fileGUID)));
                            MemoryStream memoryStream = new MemoryStream();
                            memoryStream.Write(imgByte, 0, imgByte.Length);
                            System.Drawing.Image imagen = System.Drawing.Image.FromStream(memoryStream);

                            System.Drawing.Image thumb = imagen.GetThumbnailImage(150, 150,
                            delegate() { return false; }, (IntPtr)0);

                            thumb.Save(Server.MapPath(thumnails));
                        }

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

                        sb.Append(string.Format(@"<img src='{0}' alt='{1}' title='{2}' />", encThumParams, encParams, fileGUID));
                    }
                }
            }

            ImageHtml = sb.ToString();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-09-10
        /// 작성목적 : 이미지 파일인지 구분
        /// </summary>
        /// <returns></returns>
        private bool GetCheckImage(string _fileType)
        {
            bool fileOK = false;

            string fileExtension = _fileType.ToLower();
            string[] allowedExtensions = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ImgFileExtension", "").Split(',');

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : bitmap 이미지를 byte 배열로 반환하기
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

        #endregion

        #region (함수)


        /// <summary>
        /// 작성자  : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자  : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
        }




        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)





        #endregion
    }
}