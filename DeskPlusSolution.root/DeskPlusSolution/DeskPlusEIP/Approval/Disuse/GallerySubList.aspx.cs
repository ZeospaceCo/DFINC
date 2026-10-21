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

namespace Zeospace.DeskPlus.Approval.Web.Disuse
{
    public partial class GallerySubList : Base.BasePageApprovalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ChildDocID = 0;
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Disuse.GallerySubList).Assembly;	//어셈블리 선언
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
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            DAL.AttachFile dal = new DAL.AttachFile();

             // 부모 문서번호들 가져오기
            DAL.Document dalDoc = new DAL.Document();
            DAL.DocumentProcess dalDocProcess = new DAL.DocumentProcess();

            int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
            string upPathID = dalDoc.GetUpPathID(tmpCompanyID, ChildDocID);   

            upPathID = upPathID.TrimStart(',');
            string[] arrUpPathID = upPathID.Split(',');

            StringBuilder sb = new StringBuilder();
            // 부모문서 갯수만큼 반복    
            for (int i = 0; i < arrUpPathID.Length; i++)
            {
                List<BE.AttachFileInfo> al = dal.GetAttachImageLists(CurrentUserInfo.CompanyID, arrUpPathID[i].ToNumber<int>());

                string filePath = "";
                string fileGUID = "";
                string fileRealName = ""; //guid.fileType
                string thumnails = "";

                string encThumParams = "";
                string encParams = "";

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
                            byte[] imgByte = GetImageByteArr(new Bitmap(Server.MapPath(filePath + fileGUID)));
                            MemoryStream memoryStream = new MemoryStream();
                            memoryStream.Write(imgByte, 0, imgByte.Length);
                            System.Drawing.Image imagen = System.Drawing.Image.FromStream(memoryStream);

                            System.Drawing.Image thumb = imagen.GetThumbnailImage(150, 150,
                            delegate() { return false; }, (IntPtr)0);

                            thumb.Save(Server.MapPath(thumnails));
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
                                                       

                            sb.Append(string.Format(@"<img src='{0}' alt='{1}' title='{2}' />", encThumParams, encParams, info.FileName));
                        }
                        else
                        {
                            sb.Append(string.Format(@"<img src='{0}' alt='{1}' title='{2}' />", filePath + "Thum_" + fileGUID, filePath + fileGUID, info.FileName));
                        }
                    }
                }
               
            }
            ImageHtml = sb.ToString();
        }

        /// <summary>
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

        #endregion

        #region (함수)



        /// <summary>
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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ChildDocID = GetDecryptParam("childDocID", "0").ToNumber<int>();
        }
        

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}