using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlusMobile.Web.Html.Util.Enc
{
    public partial class AESTest : System.Web.UI.Page
    {
        public string skey = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                skey = Guid.NewGuid().ToString();
                KeyBox.Text = skey;
                //KeyBox.Text = "2b97726c-2d37-4ade-b659-4927df98bfc2";
            }
        }

        protected void btnEncryptNew_Click(object sender, EventArgs e)
        {
            Helper.Security.AES aes = new Helper.Security.AES();
            string result = aes.Encrypt(DataTextBox.Text, KeyBox.Text, 256);
            ResultTextBoxNew.Text = result;
        }

        protected void btnDecryptNewServer_Click(object sender, EventArgs e)
        {
            Helper.Security.AES aes = new Helper.Security.AES();
            string result = aes.Decrypt(ResultTextBoxNew.Text, KeyBox.Text, 256);
            DecryptTextBoxNewServer.Text = result;
        }
    }
}