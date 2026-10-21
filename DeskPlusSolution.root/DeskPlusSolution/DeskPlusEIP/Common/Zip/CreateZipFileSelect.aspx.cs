using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web.Zip
{
    public partial class CreateZipFileSelect : Base.BasePageCommon
    {
        #region 변수설정

        private string Module;
        private string ListID; //고유키값
        private string FileID; //체크된 FileID

        //중국어 찾기
        private Regex RegexChinese = new Regex(@"\p{IsCJKUnifiedIdeographs}");

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.Zip.CreateZipFileSelect).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (Module.ToUpper() == "APPROVAL" && ListID.ToNumber<int>() > 0)
            {
                ZipFileAppDoc();
            }
            else if (Module.ToUpper() == "APPROVALDISUSE" && ListID.ToNumber<int>() > 0)
            {
                ZipFileAppDisUseDoc();
            }
            else if (Module.ToUpper() == "APPROVALCIR" && ListID.ToNumber<int>() > 0)
            {
                ZipFileAppCirDoc();
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 중국어 정규화
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsChinese(char s)
        {
            return RegexChinese.IsMatch(s.ToString());
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            Module = GetDecryptParam("module", "");
            ListID = GetDecryptParam("listID", "");
            FileID = GetDecryptParam("fileID", "");

            //POST 방식으로 암호화처리 하지 않고 넘김(결재->게시판이관용)
            if (Module == "" && ListID == "" && FileID == "")
            {
                Module = ((string)(Request.Params["module"] ?? "") == "") ? "" : (string)(Request.Params["module"] ?? "");
                ListID = ((string)(Request.Params["listID"] ?? "") == "") ? "" : (string)(Request.Params["listID"] ?? "");
                FileID = ((string)(Request.Params["fileID"] ?? "") == "") ? "" : (string)(Request.Params["fileID"] ?? "");
            }
        }

        #region 결재 문서 ZIP 하기

        /// <summary>
        /// 결재 문서 : zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileAppDoc()
        {
            string subject = "";

            Zeospace.DeskPlus.Approval.DAL.DocumentProcess dal = new Approval.DAL.DocumentProcess();
            List<DocumentProcessInfo> al = dal.GetDocumentUnion(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            foreach (var docInfo in al)
            {
                subject = StripFileName(docInfo.Subject);
            }

            string shortDateStr = DateHelper.GetCultureDate(DateTime.Now.ToShortDateString(), "yyyyMMdd", "ko");           
            
            //string fileName = subject + "_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";
            string fileName = subject + "_" + shortDateStr + ".zip";
            
            string BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            //\DeskPlus40\FileData\1\Users\1\Spool\subject_20170313
            string zipFolderPath = BaseFileDirectory + "{0}/Users/{1}/Spool/" + subject + "_" + shortDateStr;
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true);

            FileHelper.CreateSubFolder(zipFolderPath);

            Zeospace.DeskPlus.Approval.DAL.AttachFile att = new AttachFile();
            var attList = att.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string oriAttachFileID = "";
            string realFileName = "";

            string[] arrFileID = FileID.Split(',');

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명
                oriAttachFileID = attInfo.AttachFileID.ToString();

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                foreach (string id in arrFileID)
                {
                    if (id == oriAttachFileID)
                    {
                        System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
                    }
                }
            }

            //5.zip 하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            String header = Request.Headers["User-Agent"];
            
            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else if(header.IndexOf("Chrome") > -1)                        
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }


            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 결재 문서 ZIP 하기

        #region 결재 폐기문서 ZIP 하기

        /// <summary>
        /// 결재 폐기문서 : zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileAppDisUseDoc()
        {
            string subject = "";

            Zeospace.DeskPlus.Approval.DAL.DocumentProcess dal = new Approval.DAL.DocumentProcess();
            List<DocumentProcessInfo> al = dal.GetDocumentUnion((CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>(), ListID.ToNumber<int>());

            foreach (var docInfo in al)
            {
                subject = StripFileName(docInfo.Subject);
            }

            string shortDateStr = DateHelper.GetCultureDate(DateTime.Now.ToShortDateString(), "yyyyMMdd", "ko");

            //string fileName = subject + "_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";
            string fileName = subject + "_" + shortDateStr + ".zip";

            string BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            //\DeskPlus40\FileData\1\Users\1\Spool\subject_20170313
            string zipFolderPath = BaseFileDirectory + "{0}/Users/{1}/Spool/" + subject + "_" + shortDateStr;
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true);

            FileHelper.CreateSubFolder(zipFolderPath);

            Zeospace.DeskPlus.Approval.DAL.AttachFile att = new AttachFile();
            var attList = att.GetDisuseAttachFileListsByPath(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string oriAttachFileID = "";
            string realFileName = "";

            string[] arrFileID = FileID.Split(',');

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명
                oriAttachFileID = attInfo.AttachFileID.ToString();

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                foreach (string id in arrFileID)
                {
                    if (id == oriAttachFileID)
                    {
                        System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
                    }
                }
            }

            //5.zip 하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 결재 폐기문서 ZIP 하기

        #region 결재 회람문서 ZIP 하기

        /// <summary>
        /// 결재 회람문서 : zip 하기
        /// </summary>
        /// <param name="docInfo"></param>
        private void ZipFileAppCirDoc()
        {
            string subject = "";

            Zeospace.DeskPlus.Approval.DAL.CirculationDocument dal = new Approval.DAL.CirculationDocument();
            List<CirculationDocumentInfo> al = dal.GetDocList(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            foreach (var cirDocInfo in al)
            {
                subject = StripFileName(cirDocInfo.Subject);
            }

            string shortDateStr = DateHelper.GetCultureDate(DateTime.Now.ToShortDateString(), "yyyyMMdd", "ko");

            //string fileName = subject + "_" + DateTime.Now.ToShortDateString().Replace("-", "") + ".zip";
            string fileName = subject + "_" + shortDateStr + ".zip";

            string BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            //\DeskPlus40\FileData\1\Users\1\Spool\subject_20170313
            string zipFolderPath = BaseFileDirectory + "{0}/Users/{1}/Spool/" + subject + "_" + shortDateStr;
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true);

            FileHelper.CreateSubFolder(zipFolderPath);
            
            Zeospace.DeskPlus.Approval.DAL.CirculationAttachFile att = new CirculationAttachFile();
            List<CirculationAttachFileInfo> attList = att.GetAttachFileLists(CurrentUserInfo.CompanyID, ListID.ToNumber<int>());

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string oriAttachFileID = "";
            string realFileName = "";

            string[] arrFileID = FileID.Split(',');

            foreach (var attInfo in attList)
            {
                oriFilePath = attInfo.FilePath; // dr["FilePath"].ToString();    //파일 경로
                oriFileGUID = attInfo.FileGUID;// dr["FileGUID"].ToString();    //암호화된 파일명
                oriFileName = attInfo.FileName;// dr["FileName"].ToString();    //원본 파일명
                oriAttachFileID = attInfo.CirculationAttachFileID.ToString();

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                foreach (string id in arrFileID)
                {
                    if (id == oriAttachFileID)
                    {
                        System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(zipFolderPath + "/" + realFileName));
                    }
                }
            }

            //5.zip 하기
            ZipFile.ZipCompress(
                Server.MapPath(zipFolderPath),
                Server.MapPath(zipFolderPath + "/" + fileName),
                "");

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            Response.ContentType = "multipart/form-data";
            // 파일 다운로드 시키기
            Response.TransmitFile(Server.MapPath(zipFolderPath + "/" + fileName));
            Response.Flush();
        }

        #endregion 결재 회람문서 ZIP 하기

        #region 파일, 폴더명이 될 수 없는 특수문자 제거

        private string StripFileName(string name)
        {

            name = System.Text.RegularExpressions.Regex.Replace(name, "[\\\\/:*?\"<>|]", "_");

            foreach (var ch in System.IO.Path.GetInvalidFileNameChars())
            {
                name = name.Replace(ch, '_');
            }

            return name;
        }

        #endregion 파일, 폴더명이 될 수 없는 특수문자 제거

        #region zip 모듈

        private void Compress(string source, string destination)
        {
            using (System.IO.FileStream sourceStream = System.IO.File.OpenRead(source))
            {
                using (System.IO.FileStream destinationStream = System.IO.File.OpenWrite(destination))
                {
                    using (System.IO.Compression.GZipStream compressedStream = new System.IO.Compression.GZipStream(destinationStream, System.IO.Compression.CompressionMode.Compress))
                    {
                        byte[] data = new byte[sourceStream.Length];

                        sourceStream.Read(data, 0, data.Length);
                        compressedStream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public class ZipFile
        {
            public ZipFile()
            {
                //
                // TODO: 여기에 생성자 논리를 추가합니다.
                //
                //string startFolder = @"C:\inetpub\wwwroot\conversion";
                //string outputZipPath = @"C:\inetpub\wwwroot";
                //string password = "1111";
                //Zeospace.DeskPlus.Component.WebComponent.ZipFile.ZipIt(startFolder,outputZipPath + "\\" + DateTime.Now.ToString("yyyyddmmhhmm")	+ ".exe", password);
            }

            private static Hashtable excludeDirs = new Hashtable();

            //압축하기
            public static void ZipCompress(string Path, string outPathAndZipFile, string password)
            {
                string OutPath = outPathAndZipFile;
                ArrayList ar = GenerateFileList(Path); // generate file list
                // find number of chars to remove from orginal file path
                int TrimLength = (Directory.GetParent(Path)).ToString().Length;
                TrimLength += 1; //remove '\'

                byte[] obuffer;

                using (ZipOutputStream oZipStream = new ZipOutputStream(File.Create(OutPath)))
                {
                    if (password != String.Empty) oZipStream.Password = password;
                    oZipStream.SetLevel(7); // 9 = maximum compression level;7level 안전
                    ZipEntry oZipEntry;

                    foreach (string Fil in ar) // for each file, generate a zipentry
                    {
                        if (Fil.ToUpper().IndexOf("THUMBS.DB") == -1)
                        {
                            oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                            //oZipEntry.Flags = (int)ICSharpCode.SharpZipLib.Zip.GeneralBitFlags.UnicodeText;

                            oZipStream.PutNextEntry(oZipEntry);

                            if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                            {
                                //ostream = File.OpenRead(Fil);
                                //obuffer = new byte[ostream.Length]; // byte buffer
                                //ostream.Read(obuffer, 0, obuffer.Length);
                                //oZipStream.Write(obuffer, 0, obuffer.Length);
                                //ostream.Close();
                                //Console.Write(".");

                                using (FileStream ostream = File.OpenRead(Fil))
                                {
                                    obuffer = new byte[ostream.Length];
                                    ostream.Read(obuffer, 0, obuffer.Length);
                                    oZipStream.Write(obuffer, 0, obuffer.Length);
                                }
                            }
                        }
                    }
                }
            }

            private static ArrayList GenerateFileList(string Dir)
            {
                ArrayList mid = new ArrayList();
                bool Empty = true;
                foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
                {
                    mid.Add(file);
                    Empty = false;
                }

                if (Empty)
                {
                    if (Directory.GetDirectories(Dir).Length == 0) // if directory is completely empty, add it
                    {
                        mid.Add(Dir + @"/");
                    }
                }

                foreach (string dirs in Directory.GetDirectories(Dir)) // do this recursively
                {
                    // set up the excludeDir test
                    string testDir = dirs.Substring(dirs.LastIndexOf(@"\") + 1).ToUpper();
                    if (ZipFile.excludeDirs.Contains(testDir))
                        continue;
                    foreach (object obj in GenerateFileList(dirs))
                    {
                        mid.Add(obj);
                    }
                }
                return mid; // return file list
            }

            /// <summary>
            /// 압축풀기
            /// </summary>
            /// <param name="file"></param>
            /// <param name="dir"></param>
            /// <returns></returns>
            public static bool ZipUnpack(string file, string dir)
            {
                try
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (directoryName != String.Empty)
                            Directory.CreateDirectory(dir + "\\" + directoryName);

                        if (fileName != String.Empty)
                        {
                            FileStream streamWriter = File.Create(dir + "\\" + theEntry.Name);
                            int size = 2048;
                            byte[] data = new byte[2048];

                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            streamWriter.Close();
                        }
                    }

                    s.Close();

                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion zip 모듈

        #endregion (함수)
    }
}