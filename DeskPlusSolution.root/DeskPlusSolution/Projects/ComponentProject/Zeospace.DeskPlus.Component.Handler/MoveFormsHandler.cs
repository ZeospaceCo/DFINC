using System;
using System.Web;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// MoveFormsHandler에 대한 요약 설명입니다.
	/// </summary>
	public class MoveFormsHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			//			try
			//			{
			//				string CompanyID = _context.Request["CompanyID"].ToString();
			//				string spoolPath = _context.Server.MapPath(_context.Request["spoolPath"].ToString());
			//
			//				if (Convert.ToInt32(CompanyID) > 0 && spoolPath.Length > 0)
			//				{
			//					string sourcePath = spoolPath + @"\" + fileName;
			//					string targetPath = "/DeskPlusEIP/FileData/" + CompanyID + "/forms/"; 
			//					targetPath = _context.Server.MapPath(targetPath);
			//
			//					if (File.Exists(sourcePath))
			//					{
			//						File.Copy(sourcePath, targetPath, true);
			//
			//						Directory.Delete(spoolPath, true);
			//
			//						_context.Response.Write("<script>parent.location.reload();</script>");
			//					}
			//				}
			//			}
			//			catch(Exception ex)
			//			{
			//				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
			//			}
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
