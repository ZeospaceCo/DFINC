using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web.Notify
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CallNotify : Base.BasePageCommon
    {
        string Alert_Success = "";
        string Alert_Register = "";
        string Alert_Modify = "";
        string Alert_Delete = "";
        string Alert_Error = "";
        string AlertType = "";
        string AlertMsg = "";
        string GoURL = "";

        public string MSG { get; set; }
        public string GoPage { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Notify.CallNotify).Assembly;	//어셈블리 선언
            #endregion            

            if (!IsPostBack)
            {
                this.GetResourceManager();

                if (!this.resMng.Equals(null))
                {
                    Alert_Success = this.resMng.GetString("Alert_Success");
                    Alert_Register = this.resMng.GetString("Alert_Register");
                    Alert_Modify = this.resMng.GetString("Alert_Modify");
                    Alert_Delete = this.resMng.GetString("Alert_Delete");
                    Alert_Error = this.resMng.GetString("Alert_Error");
                }

                AlertType = GetDecryptParam("alertType", "0");                
                AlertMsg = GetDecryptParam("alertMsg", "0");
                GoURL = GetDecryptParam("goURL", "0");
                
                if (AlertType.Length > 0)
                {
                    switch (AlertType)
                    {
                        case "Alert_Success":
                            MSG = Alert_Success;
                            break;
                        case "Alert_Register":
                            MSG = Alert_Register;
                            break;
                        case "Alert_Modify":
                            MSG = Alert_Modify;
                            break;
                        case "Alert_Delete":
                            MSG = Alert_Delete;
                            break;
                        case "Alert_Error":
                            MSG = Alert_Error;
                            break;
                        default:
                            break;
                    }
                }

                if (AlertMsg.Length > 0) MSG = MSG + " " + AlertMsg;
                //string url = @"location='{0}';";
                //url = string.Format(url, GoURL + "?p=" + GetParm("p", ""));
                //Utility.ShowMessageBox(Page, "success", message, url);

                GoPage = GoURL + "?p=" + GetParm("p", "");
            }
        }
    }
}