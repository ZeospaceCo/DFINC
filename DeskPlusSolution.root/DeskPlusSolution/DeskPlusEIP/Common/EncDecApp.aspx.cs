using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web
{
    public partial class EncDecApp : Base.BasePageCommon
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 문자값을 암호화 처리하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ctnEnc_Click(object sender, EventArgs e)
        {
            string encVal = tbLeftEnc.Text.Trim();
            if (encVal.Length > 0)
            {
                tbRightDec.Text = GetEncryptParam(encVal);
            }
        }

        /// <summary>
        /// 암호화 문자열을 복고화 하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDec_Click(object sender, EventArgs e)
        {
            string decVal = tbLeftDec.Text.Trim();
            if (decVal.Length > 0)
            {
                decVal = Server.UrlDecode(decVal);//디코딩
                decVal = Utility.DecryptString(decVal);
                decVal = Server.UrlDecode(decVal);//디코딩

                tbRightEnc.Text = decVal;
            }
        }        
    }
}