using System;
using System.Web;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// DelFileHandler에 대한 요약 설명입니다.
	/// </summary>
	public class DelFileHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			try
			{
				if (_context.Request["fileName"] != null)
				{
					string where = _context.Request["where"].ToString();
					string fileName = _context.Request["fileName"].ToString();
					string companyID = _context.Request["companyID"].ToString();
					string userID = _context.Request["userID"].ToString();

                    if (!where.EndsWith("/")) where += "/";
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string targetPath = baseFileDirectory + companyID + "/users/" + userID + "/spool/" + where + fileName; 
					targetPath = _context.Server.MapPath(targetPath);

					if (File.Exists(targetPath))
					{
						File.Delete(targetPath);
					}
				}
			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelFileHandler[ProcessRequest] : " + ex.Message);
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
