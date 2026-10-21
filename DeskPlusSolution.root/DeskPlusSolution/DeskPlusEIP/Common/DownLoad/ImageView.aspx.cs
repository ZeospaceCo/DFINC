using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Zeospace.DeskPlus.Common.Web.DownLoad
{
    public partial class ImageView : Base.BasePageCommon
    {
        #region 변수설정

        private string FilePath = ""; //최상위 나올 카테고리 트리
        private string thumnail = "N"; //썸네일 형식으로 보여줄지
        private string thumWidth = "0";
        private string thumHeight = "0";

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
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.DownLoad.ImageView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetStreamImage();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {            
            FilePath = GetDecryptParam("filePath", "");

            thumnail = GetDecryptParam("thumnail", "N").ToUpper();      //썸네일 사용여부
            thumWidth = GetDecryptParam("thumWidth", "0").ToUpper();    //썸네일 넓이 값
            thumHeight = GetDecryptParam("thumHeight", "0").ToUpper();  //썸네일 높이 값
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// Gets the stream image.
        /// </summary>
        private void GetStreamImage()
        {
            //System.Byte[] arrBytes = null; //파일을 바이트로 읽기 위한 변수를 선언            

            try
            {
                Response.ContentType = "image/jpeg"; //Content Type을 반드시 image/jpeg 혹은 image/gif 등의 image 유형으로 지정해야 한다. 
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (String.IsNullOrEmpty(FilePath) || FilePath.Length == 0) FilePath = this.BaseImgPath + "/Icon/ico_noimg2.gif";//이미지 경로값이 없을 경우,
                if (!System.IO.File.Exists(Server.MapPath(FilePath))) FilePath = this.BaseImgPath + "/Icon/ico_noimg2.gif"; //실제 파일이 존재하지 않을 경우


                /*

                arrBytes = new System.Byte[1000000]; //바이트를 넉넉하게 지정하자. 이 크기가 파일보다 작다면 에러가 발생한다.
                System.IO.FileStream fs = null; //파일을 읽기 위해 파일스트림 선언

                if (System.IO.File.Exists(Server.MapPath(FilePath)))
                {
                    fs = new System.IO.FileStream(Server.MapPath(FilePath), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                }

                fs.Read(arrBytes, 0, 1000000);
                fs.Flush();
                fs.Close(); //파일스트림객체는 사용한 후에 반드시 해제해야 한다. 

                Response.Clear();
                Response.OutputStream.Write(arrBytes, 0, arrBytes.Length); //웹 Response에 파일을 직접 쓴다. 이로써 이미지가 출력된다.

                */
                FileInfo file = new FileInfo(Server.MapPath(FilePath));

                if (file.Exists)
                {

                    Bitmap sealBitmap;

                    sealBitmap = new Bitmap(Server.MapPath(FilePath));

                    if (thumnail == "Y")
                    {
                        //이미지 용량을 줄이기 위하여 Thumbnail 적용 width를 100로 적용 ByFreeman 20130614
                        int realWidth = sealBitmap.Width;
                        int realHeight = sealBitmap.Height;

                        if (realWidth > Convert.ToInt32(thumWidth))
                        {
                            int convWidth = 100;
                            if (thumWidth != "0") convWidth = Convert.ToInt32(thumWidth);

                            int convHeight = Convert.ToInt32((realHeight * convWidth) / realWidth);
                            if (thumHeight != "0") convHeight = Convert.ToInt32(thumHeight);

                            sealBitmap = (Bitmap)sealBitmap.GetThumbnailImage(convWidth, convHeight,
                                            new System.Drawing.Image.GetThumbnailImageAbort(ThumbCallback), IntPtr.Zero);
                        }
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        switch (file.Extension.ToUpper())
                        {
                            case "JPG":
                                sealBitmap.Save(ms, ImageFormat.Jpeg);
                                break;
                            case "GIF":
                                sealBitmap.Save(ms, ImageFormat.Gif);
                                break;
                            default :
                                sealBitmap.Save(ms, ImageFormat.Png);
                                break;
                        }

                        
                        ms.Flush();
                        Response.BinaryWrite(ms.GetBuffer());
                        ms.Close();
                        ms.Dispose();
                    }
                    sealBitmap.Dispose();

                }


            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FilePath exception error : " + ex.Message);
                Response.Write(ex.ToString());
            }
        }

        public bool ThumbCallback() { return false; }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}