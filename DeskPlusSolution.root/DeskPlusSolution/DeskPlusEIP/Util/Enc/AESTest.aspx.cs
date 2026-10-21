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
    public partial class AESTest : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnc_Click(object sender, EventArgs e)
        {
            string plainText = Server.UrlDecode(tbInput.Text);
            try
            {
                switch (ddlType.SelectedValue)
                {
                    case "AES" :
                        tbOut.Text = AESManager.Encrypt(plainText);
                        break;
                    case "TripleDES" :
                        TripleDESManager tds = new TripleDESManager();
                        tbOut.Text = tds.Encrypt(plainText.Trim(), System.Text.Encoding.UTF8);
                        break;
                    case "Base64" :
                        tbOut.Text = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
                        break;

                }
            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }

        protected void btnDec_Click(object sender, EventArgs e)
        {
            string cipherText = tbInput.Text;

            try{
                switch (ddlType.SelectedValue)
                {
                    case "AES":
                        tbOut.Text = AESManager.Decrypt(cipherText);
                        break;
                    case "TripleDES":
                        TripleDESManager tds = new TripleDESManager();
                        string data = Server.UrlDecode(cipherText.Trim());
                        data = tds.Decrypt(data, System.Text.Encoding.UTF8);
                        tbOut.Text = data.TrimEnd('\0');
                        break;
                    case "Base64":
                        tbOut.Text = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(cipherText.Replace(" ","+")));
                        break;
                }

            }
            catch (Exception ex)
            {
                lbDebug.Text = ex.Message;
            }
        }
    }
}