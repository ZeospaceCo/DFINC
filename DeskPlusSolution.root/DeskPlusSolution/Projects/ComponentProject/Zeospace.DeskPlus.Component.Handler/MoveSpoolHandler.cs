using System;
using System.Web;
using System.IO;
using System.Text;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// MoveSpoolHandler에 대한 요약 설명입니다.
	/// </summary>
	public class MoveSpoolHandler: IHttpHandler
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
				string targetPath = _context.Server.MapPath(_context.Request["TargetPath"].ToString());


				if (Directory.Exists(spoolPath))
				{
					DirectoryInfo newDir = Directory.CreateDirectory(targetPath);
					string[] fileArr = Directory.GetFiles(spoolPath);

					string fileName = "";
					string tmpFile = "";
					string sourceFile = "";
					string destinationFile = "";
					int tmpfileIdx = -1;

					for (int i=0; i<fileArr.Length; i++)
					{
						tmpFile = fileArr[i];
						tmpfileIdx = tmpFile.LastIndexOf("\\");
						fileName = tmpFile.Substring(tmpfileIdx + 1, tmpFile.Length - tmpfileIdx - 1);

						sourceFile = spoolPath + "\\" + fileName;
						destinationFile = NewFileName(targetPath, fileName);
														

						File.Copy(sourceFile, destinationFile, true);
					}

					Directory.Delete(spoolPath, true);
					Directory.CreateDirectory(spoolPath);
					_context.Response.Write("<script>parent.CompleteMove();</script>");
				}
				else
				{
					Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Deleted SpoolPath : " + spoolPath);
				}
				
			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveSpoolHandler[ProcessRequest] : " + ex.Message);
			}
		}

		private string NewFileName(string _targetPath, string _fileName)
		{
			bool isEqual = true;

			string newFileName = _targetPath + "\\" + _fileName;

			while(isEqual)
			{
				FileInfo fi = new FileInfo(newFileName);
				if (fi.Exists)
				{
					newFileName = _targetPath + "\\" + "사본 - " + fi.Name;
					isEqual = true;
				}
				else
				{
					isEqual = false;
				}
			}

			return newFileName;
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
