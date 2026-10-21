using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class ViewFaxImage : Base.BasePageCommon
    {
        #region 변수설정
        public string FaxImage = String.Empty;
        public string PageCnt = String.Empty;
        private string FaxFileData = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.ViewFaxImage).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();
            string htmlFormat = @"<img src='{0}' alt='{0}' title='{0}' />";

            
            
            //fax 파일 데이터
            if (FaxImage.Length == 0) return;
            
            for (int i = 0; i < PageCnt.ToNumber<int>(); i++)
            {
                if (FaxImage.IndexOf("ReceiveData") != -1) //수신함
                {
                    /*
                        faxnum + RecvSqe 8자리(앞에 0) + pageCnt 3자리(앞에 0) + "_view.gif
                        3261210100000001.tif (pc 다운용)
                        3261210100000001001_icon.jpg (섬네일용)
                        3261210100000001001_print.jpg (인쇄용)
                        3261210100000001001_view.jpg (보기용)
                    */

                    sb.Append(String.Format(htmlFormat, FaxImage + (i + 1).ToString().PadLeft(3, '0') + "_view.gif"));
                }
                else if (FaxImage.IndexOf("BackUpData") != -1) //송신함
                {
                    /*
                        GUID + 점 + pageCnt 3자리(앞에 0) + 점 + 용도별.gif
                        3261210100000001.tif (pc 다운용)
                        3261210100000001.001.icon.jpg (섬네일용)
                        3261210100000001.001.print.jpg (인쇄용)
                        3261210100000001.001.view.jpg (보기용)
                    */

                    sb.Append(String.Format(htmlFormat, FaxImage + "." + (i + 1).ToString().PadLeft(3, '0') + ".view.gif"));
                }

                if ((i + 1) != PageCnt.ToNumber<int>()) sb.Append("<br />");
            }

            ltlFaxImage.Text = sb.ToString();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            FaxImage = GetDecryptParam("faxImage", "");
            PageCnt = GetDecryptParam("pageCnt", "0");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnDel.Text = resMng.GetString("Del");
                cbtnPrint.Text = resMng.GetString("PrintMove");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion


    }
}