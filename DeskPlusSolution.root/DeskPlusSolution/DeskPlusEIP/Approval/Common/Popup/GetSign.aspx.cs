using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    public partial class GetSign : Base.BasePageApproval
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.GetSign).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {

        }


        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnClear.Text = resMng.GetString("SignatureClear");
                cbtnSubmit.Text = resMng.GetString("Confirm");
                cbtnCancel.Text = resMng.GetString("Close");

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(200, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.Flush();
            }

            Session["SignImage"] = bmp;
        }

        protected void Page_Disposed(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)Session["SignImage"];
            bmp.Dispose();
        }
    }
}