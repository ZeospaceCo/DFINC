using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Configuration;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Custom
{
    /// <summary>
    /// HelpDesk의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class HelpDesk : Base.BaseWSPortlet
    {

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-05-02
        /// 작성목적 : 메일, 일정, 결재 건수 가져오기 
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetHelpDesk(string p)
        {
            SetDecryptString(p);
            int pageSize = GetDecryptParam("pageSize", "10").ToNumber<int>();
            int pageSizeNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            //int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            try
            {
                DataSet ds = null;

                string dbConn = "Data Source=help.deskplus.co.kr;Initial Catalog=CRM;Persist Security Info=True;User ID=deskpluseip;Password=deskplus";
                using (var cn = new SqlConnection(dbConn))
                {

                    string query = @"WITH TmpTBL AS(     
	                                    SELECT Row_Number() over (order by X.request_date desc) AS RowNum, * FROM (
		                                    SELECT  
			                                    x2.cust_no, x2.cust_name, x1.product_no, x3.product_name,
			                                    x1.qna_no, x4.code_name, x5.gubun_code, x5.gubun_name, x1.gubun, x1.title,
			                                    x1.question, x1.request_date, x1.receip_date, x1.processor, x1.process_date,x1.answer, 
			                                    (select name from user_info where user_id= x1.processor) as manager 
		                                    FROM help_cust_qna x1, cust_basic x2, help_product x3, help_product_sub x4, help_gubun x5
		                                    WHERE 
			                                    x1.catagory_code = x4.code and x1.cust_no = x2.cust_no and x1.gubun = x5.gubun_code
			                                    and x1.product_no = x4.product_no and x1.product_no = x3.product_no
			                                    and (x1.gubun='31' or x1.gubun='30' )				
	                                    ) AS X
	                                    WHERE X.manager = '{0}'
                                    ) 
                                    SELECT * FROM TmpTBL WHERE RowNum Between (({1}-1) * {2}) + 1 and {1} * {2} ";

                    query = string.Format(query, CurrentUserInfo.Name, pageSizeNo, pageSize);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    adapter.Fill(ds, "helpdesk");
                }

                List<object> obj = new List<object>();

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.TemplateInfo info = new BE.TemplateInfo();
                        info.tmp1 = dr["cust_name"].ToString();                        
                        info.tmp3 = dr["title"].ToString();

                        DateTime dt;

                        if (DateTime.TryParse(dr["request_date"].ToString(), out dt))
                            info.tmp2 = DateHelper.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.tmp2 = string.Empty;

                        obj.Add(info);
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetHelpDesk(string p) : " + ex.Message);
                return new List<object>();
            }

        }
    }
}
