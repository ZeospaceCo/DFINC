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
    public class Bar : Base.BaseWSPortlet
    {
        #region Get
        /// <summary>
        /// CPU/Memory 정보 가져오기
        /// </summary>
        /// <param name="_machineName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> Get(string _dataURL, string _fName, string _fValue, int _pCount)
        {
            List<object> chartData = new List<object>();
            Stack<object> yTempData = new Stack<object>();
            List<string> xAxesData = new List<string>();
            List<string> yAxesData;

            string[] fValueArray = _fValue.Split('|');

            //CPU

            DAL.Chart dal = new DAL.Chart();

            DataSet ds = dal.GetBar(_dataURL, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            DataTable dt, dtTemp;
            dt = ds.Tables[0];

            string xTemp;
            foreach (string fValue in fValueArray)
            {
                
                yAxesData = new List<string>();

                DataView dv = dt.DefaultView;
                dv.RowFilter = _fName + " = " + fValue;
                dv.Sort = "ROW DESC";

                //DataTable dt = dv.ToTable();

                if (_pCount > 0)
                {
                    dtTemp = dv.ToTable().AsEnumerable().Take(_pCount).CopyToDataTable();
                }
                else
                {
                    dtTemp = dv.ToTable();
                }

                foreach (DataRow dr in dtTemp.Rows)
                {
                    xTemp = dr["xAxes"].ToString();
                    if (!xAxesData.Exists(x => x == xTemp))
                    {
                        xAxesData.Add(dr["xAxes"].ToString());
                    }
                    yAxesData.Add(Convert.ToDouble(dr["yAxes"]).ToString());
                }

                yTempData.Push(yAxesData);
            }

            chartData.Add(xAxesData);
            int tmpCount = yTempData.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                chartData.Add(yTempData.Pop());
            }

            return chartData;
        }
        #endregion
    }
}
