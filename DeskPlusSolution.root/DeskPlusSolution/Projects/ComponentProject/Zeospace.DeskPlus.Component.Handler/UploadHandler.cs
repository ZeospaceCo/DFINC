using System;
using System.Web;
using System.IO;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// UploadHandler에 대한 요약 설명입니다.
	/// </summary>
	public class UploadHandler : IHttpHandler
	{
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="_context">The _context.</param>
		public void ProcessRequest(HttpContext _context)
		{
			try
			{				
				string spoolPath = _context.Request["sp"];

                //경로값이 없을 경우
                if (spoolPath.Length == 0) return;

                //잘못된 경로로 파일을 업로드 할 경우 차단.
                if (spoolPath.Length > 0)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    if (spoolPath.ToLower().IndexOf(baseFileDirectory.ToLower()) == -1)
                    {
                        HttpContext context = HttpContext.Current;
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                            "file path is problem : UploadHandler", 
                            "/DeskPlusEIP/Data/Logs/FileUpload/",
                            "");

                        return;
                    }
                }

				spoolPath = _context.Server.MapPath(spoolPath);
				string targetPath = "";
				string fileName = "";
				string overWrite = "";
				
				if (!spoolPath.EndsWith("\\")) spoolPath += "\\";

				if (_context.Request["tp"] != null)
				{
					targetPath = _context.Server.MapPath(_context.Request["tp"].ToString());
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
				if (!Directory.Exists(spoolPath)) Directory.CreateDirectory(spoolPath);		

				// 파일 생성
                fileName = CreateFile(_context, spoolPath, fileName, overWrite);

                //파일명이 없다는 것은, 잘못된 확장자를 업로드 경우 차단함
                if (fileName.Length == 0) return;

				// 원하는 위치로 이동
				if (targetPath != "")
				{
					MoveFile(spoolPath, targetPath, fileName, overWrite);
					//_context.Response.Write("<script>parent.CompleteMove();</script>");
				}
				// Spool폴더 삭제

			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[ProcessRequest] : " + ex.Message);
			}
		}


        /// <summary>
        /// 파일 만들기
        /// </summary>
        /// <param name="_context">The _context.</param>
        /// <param name="_path">The _path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <param name="_overWrite">The _over write.</param>
        /// <returns></returns>
        public string CreateFile(HttpContext _context, string _path, string _fileName, string _overWrite)
        {
            #region 파일 만들기
            string filePath = "";
            string fileName = "";

            string blockExt = Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(
                Base.ApplicationFrameworks.Helper.AppSettingType.COMMON, "BlockExt", "");

            string[] blockExtStr = null;
            if (blockExt.Length > 0) blockExtStr = blockExt.Split(',');

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

                    fileName = _context.Server.UrlDecode(postedFile.FileName);//넘겨주는 실제 파일명

                    if (_overWrite == "")
                    {
                        filePath = NewFileName(_path, fileName);  //서버에는 같은 파일명이 있을수 있으니 중복 확인을 해서 유니크한 파일명을 얻어온다
                        fileName = Path.GetFileName(filePath); //유니크파일명을 새롭게 가져온다
                    }
                    else
                    {
                        filePath = _path + fileName;
                    }
                }

                bool response = false;
                if (blockExtStr != null)
                {
                    foreach (string ext in blockExtStr)
                    {
                        int pos = fileName.LastIndexOf(".");
                        if (pos > -1)
                        {
                            string fileExt = fileName.Substring(fileName.LastIndexOf("."), fileName.Length - pos);

                            if (fileExt.ToLower() == "." + ext.ToLower())
                            {
                                response = true;
                                break;
                            }
                        }
                    }
                }

                if (!response)
                {
                    fileCollection[i].SaveAs(filePath);
                }
                else
                {
                    HttpContext context = HttpContext.Current;
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                        "file ext is problem. (UploadHandler) : " + fileName, 
                        "/DeskPlusEIP/Data/Logs/FileDownload/",
                        "");

                    //잘못된 파일확장자 일경우 초기화
                    fileName = "";
                }                
            }

            return fileName;
            #endregion
        }


        /// <summary>
        /// 파일 만들기
        /// </summary>
        /// <param name="_context">The _context.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <returns></returns>
		public string MakeFile(HttpContext _context, string _spoolPath, string _fileName)
		{
			#region 파일 만들기
			FileStream fs = null;
			BinaryWriter bw = null;
			Stream dataStream = null;

			string filePath = "";
			string fileName = "";
			string fileSize = "";

			string nameSChar = "fn=\"";
			string nameEChar = "\"";
			string sizeSChar = " fs=\"";
			string sizeEChar = "\"\r\n";

			try
			{
				//기존 파일 스트림
				byte[] fileInfo = new byte[1024];

				

				dataStream = _context.Request.InputStream;
				dataStream.Read(fileInfo, 0, 1024);
				
				string[] fileInfoArr = Encoding.Default.GetString(fileInfo, 0, 1024).Split(';');

				fileName = fileInfoArr[0].TrimStart(nameSChar.ToCharArray()).TrimEnd(nameEChar.ToCharArray()).Trim();
				fileSize = fileInfoArr[1].TrimStart(sizeSChar.ToCharArray()).TrimEnd(sizeEChar.ToCharArray()).Trim();

				//새로만들 파일 스트림
				if (_fileName != "")
				{
					filePath = _spoolPath + _fileName;		//지정된 파일명
				}
				else
				{
					filePath = _spoolPath + fileName;		//기존 파일명
				}


				fs = new FileStream(filePath, FileMode.Create);
				bw = new BinaryWriter(fs, Encoding.UTF8);

				//기존 byte 크기
				long nBytesToRead = dataStream.Length;

				int nByte = 0;
				for (long i=1024; i<nBytesToRead; i++) 
				{
					nByte = dataStream.ReadByte();
					bw.Write((byte)nByte);
				}
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UploadHandler[UploadFile]", ex);
			}
			finally
			{
				if (bw != null)	bw.Close();
				if (fs != null)	fs.Close();
				if (dataStream != null)	dataStream.Close();
			}

			return fileName;
			#endregion
				
			#region 기존 컴포넌트 이용
			//				ZEOUPLOADLib.ZeoUploadCtrl uploadObj = new ZEOUPLOADLib.ZeoUploadCtrlClass();
			//			
			//				if (_overWrite != -1)
			//				{
			//					uploadObj.SetUploadUpdate = _overWrite;
			//				}
			//
			//				if (_spoolPath != "")
			//				{
			//					uploadObj.m_sLocalSpoolPath = _spoolPath;
			//				}
			//
			//				if (_uploadPath != "")
			//				{
			//					uploadObj.m_sLocalSavePath = _uploadPath;
			//				}
			//
			//				if (_fileName != null && _fileName != "")
			//				{
			//					uploadObj.m_sLocalSaveFileName = _fileName;
			//				}
			//
			//				uploadObj.m_sLogFile = LogFilePath(_context.Server.MapPath("/DeskPlusEIP/Data/Logs/"));
			//				uploadObj.RunUpload();
			//				string err = uploadObj.m_sErrorString;
			//
			//				string savePath = uploadObj.m_sMoveFilePath;
			#endregion
		}


//		/// <summary>
//		/// 날짜별 로그파일 쓰기 : 현재 사용하지 않음
//		/// </summary>
//		/// <param name="_logDir"></param>
//		/// <returns></returns>
//		private string LogFilePath(string _logDir)
//		{
//			if (Directory.Exists(_logDir))
//			{
//				string nowYear = DateTime.Now.Year.ToString();
//				string nowMon = String.Empty;
//				string nowDay = String.Empty;
//
//				if (DateTime.Now.Month.ToString().Length == 1)
//				{
//					nowMon = "0" + DateTime.Now.Month.ToString();
//				}
//				else
//				{
//					nowMon = DateTime.Now.Month.ToString();
//				}
//
//				if (DateTime.Now.Day.ToString().Length == 1)
//				{
//					nowDay = "0" + DateTime.Now.Day.ToString();
//				}
//				else
//				{
//					nowDay = DateTime.Now.Day.ToString();
//				}
//
//				return _logDir + "fileUpload_" + nowYear + nowMon + nowDay + ".txt";
//			}
//			else
//			{
//				return _logDir + "fileUpload.txt";
//			}
//		}

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
        /// 새로운 파일명 - 중복되지 않음
        /// </summary>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_fileName">Name of the _file.</param>
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
