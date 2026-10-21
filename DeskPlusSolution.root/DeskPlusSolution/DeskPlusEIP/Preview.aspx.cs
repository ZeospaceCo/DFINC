using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;

namespace Zeospace.DeskPlus.Base.Web
{
    public partial class Preview : System.Web.UI.Page
    {
        #region 변수설정
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }

        public string Contents = string.Empty;
        public string PopupTitle = string.Empty;
        public int Height = 0;
        private int ManagerID;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Preview).Assembly;	//어셈블리 선언

            GetResourceManager();

            #endregion
            if (!IsPostBack)
            {
                if (GetParm("managerID", "") != "")
                    LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            try
            {
                ManagerID = GetParm("managerID", "0").ToNumber<int>();

                DataSet ds = GetContents(ManagerID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PopupTitle = dr["Subject"].ToString();
                        Contents = dr["Content"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
                        Height = dr["Height"].ToString().ToNumber<int>() - 30;
                    }
                }
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Preview.aspx", ex);
            }
        }

        #endregion

        #region 함수

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

        private DataSet GetContents(int _managerID)
        {
            string sqlQuery = string.Empty;
            sqlQuery = "SELECT * FROM DP_POP_Manager WHERE ManagerID = " + _managerID;

            return SQLHelper.ExecuteDataSet(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString, CommandType.Text, sqlQuery, "GetContents", null);
        }

        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}