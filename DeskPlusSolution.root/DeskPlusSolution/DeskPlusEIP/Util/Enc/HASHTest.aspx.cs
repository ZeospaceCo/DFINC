using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Base.Web.Util.Enc
{
    public partial class HASHTest : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSHA1_Click(object sender, EventArgs e)
        {
            string plainText = tbInput.Text.Trim();

            try
            {
                tbOut.Text = HashManager.GetSHA1Hash(plainText);
            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }

        protected void btnSHA2_Click(object sender, EventArgs e)
        {
            string plainText = tbInput.Text.Trim();

            try
            {
                tbOut.Text = HashManager.GetSHA256Hash(plainText);
            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }

        protected void btnSHA5_Click(object sender, EventArgs e)
        {
            string plainText = tbInput.Text.Trim();

            try
            {
                tbOut.Text = HashManager.GetSHA512Hash(plainText);
            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }


        protected void btnSMD5_Click(object sender, EventArgs e)
        {
            string plainText = tbInput.Text.Trim();

            try
            {
                tbOut.Text = HashManager.GetMd5Hash(plainText);
            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }
    }
}