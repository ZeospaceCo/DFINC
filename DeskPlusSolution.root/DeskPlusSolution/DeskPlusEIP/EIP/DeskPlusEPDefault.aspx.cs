using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Configuration;

namespace Zeospace.DeskPlus.EIP.Web
{   
    public partial class DeskPlusEPDefault : Base.BasePageEIP
    {
        public SortedDictionary<string, string> PopupContent;
        public string MainUrl = "about:blank";

        protected void Page_Load(object sender, EventArgs e)
        {
            string tmpDcparm = this.GetParm("dcparm", "");

            if (String.IsNullOrEmpty(tmpDcparm))
            {
                MainUrl = "Package/NMain.aspx";
            }

            PopupContent = new SortedDictionary<string, string>();

            GetPopUp();
        }

        /// <summary>
        /// 팝업
        /// </summary>
        private void GetPopUp()
        {
            string ct = string.Empty;
            string mID = string.Empty;
            int h, w;

            DataSet ds = GetPopUpManager();

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    h = dr["Height"].ToString().ToNumber<int>();
                    w = dr["Width"].ToString().ToNumber<int>();

                    mID = dr["ManagerID"].ToString();

                    ct = "window.open('/DeskPlusEIP/Preview.aspx?managerID=" + mID + "', 'pop" + mID + "', 'menubar=no, scrollbars=yes, status=no, toolbar=no, resizable=no, width=" + w + ", height=" + h + ", top=0, left=0');";
                    PopupContent.Add(mID, ct);
                }
            }
        }

        private DataSet GetPopUpManager()
        {
            string sqlQuery = string.Empty;
            string nowDate = string.Empty;

            nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            sqlQuery = "SELECT * FROM DP_POP_Manager "
                     + " WHERE StartDate <= '" + nowDate + "'"
                     + " AND EndDate >= '" + nowDate + "' AND LoginYN=1";

            return SQLHelper.ExecuteDataSet(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString, CommandType.Text, sqlQuery, "GetPopUpManager(Login)", null);

        }
    }
}