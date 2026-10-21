using System;
using System.Web;
using System.IO;
using System.Text;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// UploadFileSaveHandler에 대한 요약 설명입니다.
	/// </summary>
	public class UploadFileSaveHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			try
			{
				string target = _context.Request["target"].ToString();
				string path = _context.Server.MapPath(_context.Request["path"].ToString());
				string targetPath = "";

				string fileName = "";
				string overWrite = "";
				
				if (!path.EndsWith("\\")) path += "\\";

				if (_context.Request["targetPath"] != null)
				{
					targetPath = _context.Server.MapPath(_context.Request["targetPath"].ToString());
					if (!targetPath.EndsWith("\\")) targetPath += "\\";
				}

				if (_context.Request["fileName"] != null)
				{
					fileName = _context.Request["fileName"].ToString();
				}

				if (_context.Request["overWrite"] != null)
				{
					overWrite = _context.Request["overWrite"].ToString();
				}

				// Spool 폴더 생성
				if (!Directory.Exists(path)) Directory.CreateDirectory(path);		

				// 파일 생성
				fileName = CreateFile(_context, path, fileName);

				// 원하는 위치로 이동
				if (targetPath != "")
				{
					MoveFile(path, targetPath, fileName, overWrite);
				}
			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[ProcessRequest] : " + ex.Message);
			}
		}

		/// <summary>
		/// Moves the file.
		/// </summary>
		/// <param name="_spoolPath">The _spool path.</param>
		/// <param name="_targetPath">The _target path.</param>
		/// <param name="_fileName">Name of the _file.</param>
		/// <param name="_overWrite">The _over write.</param>
		private void MoveFile(string _spoolPath, string _targetPath, string _fileName, string _overWrite)
		{
			string source = _spoolPath + _fileName;
			string target = "";
			if (File.Exists(source))
			{
				DirectoryInfo targetDir = Directory.CreateDirectory(_targetPath);

				if (_overWrite == "")
				{
					target = NewFileName(_targetPath, _fileName);
				}
				else
				{
					target = _targetPath + _fileName;
				}


				try
				{
					File.Copy(source, target, true);
					File.Delete(source);
				}
				catch(Exception ex)
				{
					Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[MoveFile]", ex);
				}
			}
			else
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[MoveFile] : Deleted SpoolPath : " + source);
			}
		}

		/// <summary>
		/// 파일 만들기
		/// </summary>
		/// <param name="_context"></param>
		/// <param name="_path"></param>
		/// <param name="_fileName"></param>
		/// <returns></returns>
		public string CreateFile(HttpContext _context, string _path, string _fileName)
		{
			#region 파일 만들기
			string filePath = "";
			string fileName = "";

			_context.Request.ContentEncoding = System.Text.Encoding.UTF8;

			HttpFileCollection fileCollection = _context.Request.Files;

			for (int i = 0; i < fileCollection.Count; i++)
			{
				if (_fileName != "")
				{
					fileName = _fileName;
					filePath = _path + fileName;		//지정된 파일명
				}
				else
				{
					HttpPostedFile postedFile = fileCollection.Get(0);

                    fileName = postedFile.FileName;
                    //fileName = fileName.Replace("%u7E", "~"); // UrlDecode 시 ~ 는 Decode를 못함 --; 다국어에서 %u7E 로 
                    fileName = fileName.Replace("%u7E%", "~%"); // ByFreeman 20130315
                    fileName = fileName.Replace("%u7E.", "~."); // 

                    fileName = _context.Server.UrlDecode(fileName);//넘겨주는 실제 파일명                    
					filePath = NewFileName(_path, fileName);
					fileName = Path.GetFileName(filePath);
				}

				fileCollection[i].SaveAs(filePath);
			}

			return fileName;
			#endregion
		}

		/// <summary>
		/// 새로운 파일명 - 중복되지 않음
		/// </summary>
		/// <param name="_targetPath"></param>
		/// <param name="_fileName"></param>
		/// <returns></returns>
		private string NewFileName(string _targetPath, string _fileName)
		{
			int i=1;
			string targetPath = _targetPath + _fileName;
			string fileName = "";
			int fileIdx = -1;

			while(File.Exists(targetPath))
			{
				fileName = targetPath.Substring(targetPath.LastIndexOf("\\") + 1, targetPath.Length - targetPath.LastIndexOf("\\") - 1);
				fileIdx = fileName.LastIndexOf(".");
				fileName = fileName.Substring(0, fileIdx) + "(" + i + ")" + fileName.Substring(fileIdx, fileName.Length - fileIdx);
				targetPath = targetPath.Substring(0, targetPath.LastIndexOf("\\") + 1) + fileName;
				i++;
			}

			return targetPath;
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
