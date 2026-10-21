using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Chart
{
    /// <summary>
    /// Bar의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Pie : Base.BaseWSPortlet
    {
        #region Get
        /// <summary>
        /// CPU/Memory 정보 가져오기
        /// </summary>
        /// <param name="_machineName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> Get(string _dataURL, int _pCount)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Chart.Pie).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            List<object> chartData = new List<object>();
            List<string> label = new List<string>();
            List<string> data = new List<string>();

            DAL.Chart dal = new DAL.Chart();

            DataSet ds = dal.GetBar(_dataURL, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            DataTable dt = ds.Tables[0];
            DataView dv = dt.DefaultView;
            dv.Sort = "ROW ASC";
            int i=0;
            string etcLabel = this.GetResource("ShortReplySec");
            double etcData = 0;
            foreach (DataRow dr in dv.ToTable().Rows)
            {
                if (_pCount > i)
                {
                    label.Add(dr["label"].ToString());
                    data.Add(dr["data"].ToString());
                }
                else
                {
                    etcData += Convert.ToDouble(dr["data"]);
                }

                i++;
            }

            label.Add(etcLabel);
            data.Add(Convert.ToDouble(etcData).ToString("f2"));

            chartData.Add(label);
            chartData.Add(data);

            return chartData;
        }
        #endregion
    }
}
