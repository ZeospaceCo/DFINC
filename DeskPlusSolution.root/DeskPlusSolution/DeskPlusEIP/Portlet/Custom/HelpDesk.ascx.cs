using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.BLL;
using Zeospace.DeskPlus.Board.DAL;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Configuration;

namespace Zeospace.DeskPlus.Portlet.Web.Custom
{
    public partial class HelpDesk : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public int TotalCount;        
        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Custom.HelpDesk).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {            
            GetListCount();
        }
        #endregion

        #region (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 미사용
        /// 작성자     : 
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetListCount()
        {
            try
            {
                string dbConn = "Data Source=help.deskplus.co.kr;Initial Catalog=CRM;Persist Security Info=True;User ID=deskpluseip;Password=deskplus";
                using (var cn = new SqlConnection(dbConn))
                {

                    string query = @"SELECT COUNT(*) FROM (
	                                    SELECT 
		                                    (select name from user_info where user_id= x1.processor) as manager 
	                                    FROM help_cust_qna x1, cust_basic x2, help_product x3, help_product_sub x4, help_gubun x5
	                                    WHERE 
		                                    x1.catagory_code = x4.code and x1.cust_no = x2.cust_no and x1.gubun = x5.gubun_code
		                                    and x1.product_no = x4.product_no and x1.product_no = x3.product_no
		                                    and (x1.gubun='31' or x1.gubun='30' )
                                    ) AS X
                                    WHERE X.manager = '{0}'";

                    query = string.Format(query, CurrentUserInfo.Name);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cn.Open();

                    TotalCount = cmd.ExecuteScalar().ToString().ToNumber<int>();
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                TotalCount = 0;
                //throw;
            }
        }


        #endregion

        #region (프로퍼티)

        #endregion
    }
}