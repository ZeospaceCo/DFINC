using System;
using System.Web;
using System.IO;
using System.Text;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// UploadHandler에 대한 요약 설명입니다.
	/// </summary>
	public class DelSpoolHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			try
			{
				string spoolPath = _context.Server.MapPath(_context.Request["SpoolPath"].ToString());


				if (Directory.Exists(spoolPath))
				{
					Directory.Delete(spoolPath, true);

					_context.Response.Write("Deleted Folder : " + spoolPath);
				}
				
			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSpoolHandler[ProcessRequest] : " + ex.Message);
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
