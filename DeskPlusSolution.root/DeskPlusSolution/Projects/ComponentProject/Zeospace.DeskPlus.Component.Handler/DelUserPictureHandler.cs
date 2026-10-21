using System;
using System.Web;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// DelUserPictureHandler에 대한 요약 설명입니다.
	/// </summary>
	public class DelUserPictureHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			try
			{
				string companyID = _context.Request["companyID"].ToString();
				string userID = _context.Request["userID"].ToString();
				string fileName = _context.Request["fileName"].ToString();
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string targetPath = baseFileDirectory + companyID + "/users/" + userID + "/picture/" + fileName; 

				if (Convert.ToInt32(companyID) > 0 && Convert.ToInt32(userID) > 0 && fileName.Length > 0 && targetPath.Length > 0)
				{
					targetPath = _context.Server.MapPath(targetPath);

					if (File.Exists(targetPath))
					{
						File.Delete(targetPath);

						_context.Response.Write("<script>parent.location.reload();</script>");
					}
				}
				
			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelUserPictureHandler[ProcessRequest] : " + ex.Message);
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
