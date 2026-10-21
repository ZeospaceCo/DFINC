using System;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Text;
using System.Data;                                         
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// LoadMenuHandler에 대한 요약 설명입니다.
	/// </summary>
	public class LoadMenuHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			string loginID = _context.Request["uID"].ToString();

			if (loginID != "")
			{
				SqlParameter[] parms = new SqlParameter[]{
															 new SqlParameter("@LoginID", SqlDbType.NVarChar, 50)
														 };	
		
				parms[0].Value = loginID;

				string menuName = "";
				string directCall = "";
				string targetURL = "";
				string serverName = "";

				try
				{
                    serverName = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
				}
				catch
				{
					serverName = _context.Request.ServerVariables["SERVER_NAME"].ToString();
				}

				StringBuilder sb = new StringBuilder();
				//sb.Append("<?xml version='1.0' encoding='utf-8' ?>\n");
				//sb.Append("<Menus>");
				using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_DP_EIP_Menus_SelectByLoginID", parms)) 
				{
					while(sdr.Read())
					{
						if (sdr["DirectCall"].ToString() != "" && sdr["DirectCall"] != System.DBNull.Value)
						{
							menuName = sdr["MenuName"].ToString();
							directCall = sdr["DirectCall"].ToString();
					
							targetURL = "http://" + serverName + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=" + directCall;

							//sb.Append("<Menu><Name>" + menuName + "</Name><Value>" + targetURL + "</Value></Menu>");
							sb.Append(menuName + "," + targetURL + "|");
						}
					}
				}

				string menuString =  sb.ToString().TrimEnd('|');
	
				//string encoding = Encoding.UTF8.GetString(System.Convert..FromBase64String(menuString));

				//string encoding = HttpUtility.UrlEncode(menuString, System.Text.Encoding.UTF8);
				//sb.Append("</Menus>");
				_context.Response.ContentType = "text/xml";
				_context.Response.Charset = "utf-8";

				//_context.Response.Write(sb.ToString());
				
				_context.Response.Write(menuString);
				//_context.Response.Write(sb.ToString().TrimEnd('|'));
			}
		}

		/// <summary>
		/// 다른 요청에서 <see cref="T:System.Web.IHttpHandler"/> 인스턴스를 사용할 수 있는지 여부를 나타내는 값을 가져옵니다.
		/// </summary>
		/// <value></value>
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
