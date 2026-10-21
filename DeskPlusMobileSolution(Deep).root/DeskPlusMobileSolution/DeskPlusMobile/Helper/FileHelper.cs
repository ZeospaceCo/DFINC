using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Security;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 디렉토리 파일 옮기기
        /// </summary>
        /// <param name="_fromWebPath">현재경로</param>
        /// <param name="_toWebPath">이동경로</param>
        /// <param name="_deleteFromPath">삭제여부</param>
        /// <returns></returns>
        public static bool MoveFolder(string _fromWebPath, string _toWebPath, bool _deleteFromPath)
        {
            if (_fromWebPath == "" || _fromWebPath == null)
            {
                return false;
            }

            string fromPath = "";
            string toPath = "";
            string newFolder = "";

            try
            {
                HttpContext context = HttpContext.Current;
                fromPath = context.Server.MapPath(_fromWebPath);		// 절대 경로로 변경
                toPath = context.Server.MapPath(_toWebPath);		// 절대 경로로 변경

                if (Directory.Exists(fromPath))
                {
                    CopyFiles(_fromWebPath, _toWebPath);

                    string[] subDir = Directory.GetDirectories(fromPath);

                    for (int i = 0; i < subDir.Length; i++)
                    {
                        newFolder = subDir[i].Replace(fromPath, "").Replace("\\", "/");
                        CopyFiles(_fromWebPath + newFolder, _toWebPath + newFolder);
                    }

                    if (_deleteFromPath)
                    {
                        Directory.Delete(fromPath, true);
                    }

                    return true;
                }
                else
                {
                    Helper.AppException.WriteException("Deleted FromWebPath : " + _fromWebPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 파일 복사
        /// </summary>
        /// <param name="_fromWebPath">현재파일경로</param>
        /// <param name="_toWebPath">복사될 파일경로</param>
        public static void CopyFiles(string _fromWebPath, string _toWebPath)
        {
            if (_fromWebPath == "" || _fromWebPath == null)
            {
                return;
            }

            HttpContext context = HttpContext.Current;
            string fromPath = context.Server.MapPath(_fromWebPath);		// 절대 경로로 변경
            string toPath = context.Server.MapPath(_toWebPath);		// 절대 경로로 변경

            int tmpfileIdx;
            string tmpFile;
            string fileName;
            string sourceFile;
            string destinationFile;
            string newFolder = "";

            if (Directory.Exists(fromPath))
            {
                if (!Directory.Exists(toPath))
                {
                    DirectoryInfo targetDir = Directory.CreateDirectory(toPath);
                }
                else
                {
                    DirectoryInfo targetDir = new DirectoryInfo(toPath);
                }


                string[] fileArr = Directory.GetFiles(fromPath);

                for (int j = 0; j < fileArr.Length; j++)
                {
                    tmpFile = fileArr[j];
                    tmpfileIdx = tmpFile.LastIndexOf("\\");
                    fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                    sourceFile = fromPath + fileName;
                    destinationFile = toPath + fileName;
                    File.Copy(fileArr[j], destinationFile, true);
                }

                string[] subDir = Directory.GetDirectories(fromPath);

                for (int i = 0; i < subDir.Length; i++)
                {
                    newFolder = subDir[i].Replace(fromPath, "").Replace("\\", "/");
                    CopyFiles(_fromWebPath + newFolder, _toWebPath + newFolder);
                }
            }
        }

        /// <summary>
        /// 폴더 사이즈
        /// </summary>
        /// <param name="_path">The _path.</param>
        /// <returns></returns>
        public static long GetFolderSize(string _path)
        {
            long dirSize = 0;
            HttpContext context = HttpContext.Current;

            string serverPath = context.Server.MapPath(_path);

            if (Directory.Exists(serverPath))
            {
                DirectoryInfo dir = new DirectoryInfo(serverPath);

                foreach (FileInfo file in dir.GetFiles())
                    dirSize += file.Length;

                foreach (DirectoryInfo subdir in dir.GetDirectories())
                    dirSize += GetFolderSize(_path + "/" + subdir.Name);
            }

            return dirSize;
        }

        /// <summary>
        /// 경로를 실제파일경로로 치환 메소드
        /// </summary>
        /// <param name="_webPath">경로</param>
        /// <returns></returns>
        public static string GetServerMapPath(string _webPath)
        {
            HttpContext context = HttpContext.Current;
            return context.Server.MapPath(_webPath);
        }

        /// <summary>
        /// 현재 년월 가져오기 [200601]
        /// </summary>
        /// <returns></returns>
        public static string GetYearMonth()
        {
            string nowYearMon = "";
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                nowYearMon = DateTime.Now.Year.ToString() + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                nowYearMon = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            }
            return nowYearMon;
        }
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 프로젝트별 파일경로 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-31
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_nowYearMon">The _now year mon.</param>
        /// <param name="_objectID">The _object ID.</param>
        /// <returns></returns>
        private static string GetFolderTargetPath(FolderType _type, string _nowYearMon, string _objectID)
        {
            string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH") + "/";
            string targetPath = "";

            #region 인증자인지 확인(미승인자 차단)

            HttpContext context = HttpContext.Current;
            string loginID = HttpContext.Current.User.Identity.Name;
            FormsAuthenticationTicket ticket = null;

            string useSession = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USESESSION");
            if (useSession == "Y")
            {
                ticket = FormsAuthentication.Decrypt(context.Session["DeskPlusMobile_" + loginID].ToString());
            }
            else
            {
                string cookieName = "DeskPlusMobile_" + loginID;

                if (context.Request.Cookies[cookieName] != null && context.Request.Cookies[cookieName].Value != "")
                {
                    ticket = FormsAuthentication.Decrypt(context.Request.Cookies[cookieName].Value);
                }
            }

            if (ticket == null) context.Response.Write("error");

            String[] userInfoStr = ticket.UserData.Split(new char[] { '┐' });
            if (userInfoStr[1].Trim() == "") context.Response.Write("error"); //사용자아이디 없을 경우
            int companyID = userInfoStr[0].ToNumber<int>();
            int userID = userInfoStr[1].ToNumber<int>();

            #endregion 인증자인지 확인(미승인자 차단)

            switch (_type)
            {
                case FolderType.Approval:
                    targetPath = baseFileDirectory + companyID + "/Approval/Document/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Board:
                    targetPath = baseFileDirectory + companyID + "/Board/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.ADDRESS:
                    targetPath = baseFileDirectory + companyID + "/Address/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Opinion:
                    targetPath = baseFileDirectory + companyID + "/Opinion/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Community:
                    targetPath = baseFileDirectory + companyID + "/Community/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.ResourceManager:
                    targetPath = baseFileDirectory + companyID + "/ResourceManager/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Work:
                    targetPath = baseFileDirectory + companyID + "/Work/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Proposal:
                    targetPath = baseFileDirectory + companyID + "/Etc/Proposal/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Report:
                    targetPath = baseFileDirectory + companyID + "/Etc/Report/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Pims:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Pims/" + _objectID;
                    break;

                case FolderType.MBO:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/MBO/" + _objectID;
                    break;

                case FolderType.MyBoard:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Board/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.MYCOOPERATION:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/MyCooperation/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Draft:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Approval/Draft/" + _objectID;
                    break;

                case FolderType.UserComplete:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Approval/Complete/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.Picture:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Picture/";
                    break;

                case FolderType.PROJECTBOARD:
                    targetPath = baseFileDirectory + companyID + "/PMS/Board/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.PROJECTUNIT:
                    targetPath = baseFileDirectory + companyID + "/PMS/Unit/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.SNS:
                    targetPath = baseFileDirectory + companyID + "/MyTalk/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.TASK:
                    targetPath = baseFileDirectory + companyID + "/Task/" + _nowYearMon + "/" + _objectID;
                    break;

                case FolderType.TaskDraft:
                    targetPath = baseFileDirectory + companyID + "/Users/" + userID + "/Task/" + _objectID;
                    break;

                case FolderType.VISIT:
                    targetPath = baseFileDirectory + companyID + "/Visit/" + _nowYearMon + "/" + _objectID;
                    break;
            }

            return targetPath;
        }

        /// <summary>
        /// Copy 할 서버 파일리스트 가져오기
        /// </summary>
        /// <param name="_tbHidenFileList">The _TB hiden file list.</param>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_objectID">The _object ID.</param>
        /// <returns></returns>
        public static ArrayList GetCopyFileList(string _tbHidenFileList, string _targetPath, FolderType _type, int _objectID)
        {
            if (_tbHidenFileList == "" || _tbHidenFileList == null)
            {
                return null;
            }

            //S : 저장된 파일(수정모드), L : 로컬 파일, W : 웹폴더, A : 전자결재, B : 지식맵, M : 메일
            //S>AttachID>/DeskPlusEIP/FileData/1/Board/200607/>aaa.txt>GUID|W>AttachID>/DeskPlusEIP/FileData/1/Users/WebHard/1/>bbb.txt>GUID|...
            _tbHidenFileList = _tbHidenFileList.TrimEnd('|');     // 불필요한 구분자 삭제
            string[] hdFileListArr = _tbHidenFileList.Split('|'); // String 형식을 배열로 변환

            if (hdFileListArr.Length == 0) return null;  // 비교할 값이 없다면 Return

            string targetPath = String.Empty;         // 파일을 복사할 서버 경로
            string nowYearMon = GetYearMonth();       // 년월일 폴더를 만들기위한 정보
            ArrayList serverFile = new ArrayList();   // Return할 파일 리스트

            try
            {
                HttpContext context = HttpContext.Current;

                if (_targetPath == "")
                {
                    targetPath = GetFolderTargetPath(_type, nowYearMon, _objectID.ToString());
                }
                else
                {
                    targetPath = _targetPath;
                }

                UploadFileInfo upFileInfo;
                string fileGUID = String.Empty;  // 새롭게 생성할 파일명 GUID
                string fromPath = String.Empty; // 원본 물리적 파일 경로
                string toPath = String.Empty;   // 복사할 물리적 파일 경로
                string tmpExt = String.Empty; // 복사할 파일의 실제 확장자
                string tmpFileName = String.Empty; // 복사할 파일의 실제 파일명
                string tmpFileList = String.Empty; // 복사할 파일들의 임시 저장소 (String으로 받아 온것을 배열형식으로 변환후 사용)
                string[] tmpFileListArr;           // 복사할 파일들의 임시 저장소 (배열형식)
                string[] fileInfo = null;          // 실제 파일 정보를 알기 위한 임시 저장소

                for (int i = 0; i < hdFileListArr.Length; i++)
                {
                    tmpFileList = hdFileListArr[i];
                    if (tmpFileList.Trim() != "")
                    {
                        tmpFileListArr = tmpFileList.Split('>');

                        //저장된 파일과 로컬 파일이 아닌 서버에 있는 파일 리스트
                        //W : 웹폴더, A : 전자결재, B : 지식맵, M : 메일일 경우
                        //S : 저장된 파일(수정모드), L : 로컬 파일, W : 웹폴더, A : 전자결재, B : 지식맵, M : 메일, T:업무협업
                        //S>AttachID>/DeskPlusEIP/FileData/1/Board/200607/>aaa.txt>GUID|W>AttachID>/DeskPlusEIP/FileData/1/Users/WebHard/1/>bbb.txt>GUID|...
                        if (tmpFileListArr[0] != "S" && tmpFileListArr[0] != "L")
                        {
                            upFileInfo = new UploadFileInfo();

                            // 서버 파일 경로
                            fromPath = tmpFileListArr[2] + tmpFileListArr[4];
                            fromPath = context.Server.MapPath(fromPath);

                            tmpFileName = tmpFileListArr[3];    //실제 파일명 정보를 가져오기 위하여 작업
                            fileInfo = tmpFileName.Split('.');  //실제 파일 정보 배열

                            if (fileInfo.Length > 1)
                            {
                                // 파일명에 '.'가 들어간 경우, 예외처리
                                tmpExt = fileInfo[fileInfo.Length - 1]; //실질 파일 확장자 가져오기
                                //tmpExt = fileInfo[1];
                            }

                            if (File.Exists(fromPath))
                            {
                                // 대상 파일 경로 (파일명은 GUID 로 한다.)
                                fileGUID = System.Guid.NewGuid().ToString() + "." + tmpExt;	//파일을 GUID형식으로저장하기 위하여 생성
                                if (!targetPath.EndsWith("/")) targetPath += "/";       //마지막에 / 로 끝나는지 확인
                                //서버 물리적 경로로 변환

                                toPath = context.Server.MapPath(targetPath);     //서버의 물리적 경로로 변경
                                //+ fileGUID; // 후 파일명을 GUID 형식으로 추가

                                if (!Directory.Exists(toPath)) Directory.CreateDirectory(toPath); // 대상 디렉토리가 존재하지 않으면 생성

                                toPath += fileGUID;                     // 생성된 경로에 파일명을 GUID 형식으로 추가

                                File.Copy(fromPath, toPath);

                                FileInfo file = new FileInfo(fromPath);

                                upFileInfo.fileLength = file.Length;    // 파일 사이즈
                                upFileInfo.fileName = tmpFileName;     // 실제 파일명
                                upFileInfo.filePath = targetPath;      // 서버 경로
                                upFileInfo.fileType = "." + tmpExt;          // 실제 파일 확장자
                                upFileInfo.fileGUID = fileGUID;         // 저장될 GUID 명

                                serverFile.Add(upFileInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("FileHelper[GetCopyFileList] : ", ex);
                return null;
            }

            return serverFile;
        }

        /// <summary>
        /// 업로드된 파일정보 가져오기
        /// </summary>
        /// <param name="_spoolpath">The _spoolpath.</param>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_objectID">The _object ID.</param>
        /// <param name="_deleteSpoolPath">if set to <c>true</c> [_delete spool path].</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public static ArrayList GetUploadFile(string _spoolpath, string _targetPath, FolderType _type,
            int _objectID, bool _deleteSpoolPath, string _yearMonth)
        {
            if (_spoolpath == "" || _spoolpath == null)
            {
                return null;
            }

            string spoolPath = "";
            string targetPath = "";
            //신규 글 추가라면 현재 년월이지만, 수정시의 추가라면 기존의 년도로 넣어야 할듯 함.
            string nowYearMon = (_yearMonth.Length > 0) ? _yearMonth : GetYearMonth();
            ArrayList fileList;

            try
            {
                HttpContext context = HttpContext.Current;
                /*
                UserInfoHelper userInfo = new UserInfoHelper(context.User.Identity.Name);

                int companyID = userInfo.CompanyID;	//회사고유값
                int userID = userInfo.UserID;
                */

                spoolPath = context.Server.MapPath(_spoolpath);

                if (_targetPath == "")
                {
                    targetPath = GetFolderTargetPath(_type, nowYearMon, _objectID.ToString());
                }
                else
                {
                    targetPath = _targetPath;
                }

                string targetWebPath = targetPath + "/";

                if (targetPath.IndexOf("/") > -1)
                {
                    targetPath = context.Server.MapPath(targetPath);		// 절대 경로로 변경
                }

                if (Directory.Exists(spoolPath))
                {
                    DirectoryInfo newDir = Directory.CreateDirectory(targetPath);
                    fileList = new ArrayList();

                    string[] fileArr = Directory.GetFiles(spoolPath);
                    string fileName = "";
                    string tmpFile = "";
                    string sourceFile = "";
                    string destinationFile = "";
                    string fileGUID = "";
                    int tmpfileIdx = -1;

                    UploadFileInfo upFileInfo;
                    for (int i = 0; i < fileArr.Length; i++)
                    {
                        tmpFile = fileArr[i];
                        tmpfileIdx = tmpFile.LastIndexOf("\\");
                        fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                        sourceFile = spoolPath + fileName;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);

                        //파일명은 GUID 이며, 확장자는 원본과 동일하다.(.ZEO 사용안함)
                        fileGUID = System.Guid.NewGuid().ToString() + sourceFileInfo.Extension;
                        //destinationFile = NewFileName(targetPath, fileName);
                        if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                        destinationFile = targetPath + fileGUID;

                        File.Copy(sourceFile, destinationFile, true);

                        upFileInfo = new UploadFileInfo();
                        upFileInfo.fileLength = sourceFileInfo.Length;
                        upFileInfo.fileName = sourceFileInfo.Name;
                        upFileInfo.filePath = targetWebPath;
                        upFileInfo.fileType = sourceFileInfo.Extension;
                        upFileInfo.fileGUID = fileGUID;
                        upFileInfo.fileLastWriteTime = sourceFileInfo.CreationTime;

                        fileList.Add(upFileInfo);
                    }

                    if (_deleteSpoolPath)
                    {
                        Directory.Delete(spoolPath, true);
                    }

                    return fileList;
                }
                else
                {
                    Helper.AppException.WriteException("GetUploadFile : " + spoolPath);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("FileHelper[GetUploadFile] : ", ex);
                return null;
            }
        }

        /// <summary>
        /// 폴더 삭제
        /// </summary>
        /// <param name="_folderPath">폴더경로</param>
        /// <param name="_isRealPath">절대경로여부</param>
        /// <returns></returns>
        public static bool DelFolder(string _folderPath, bool _isRealPath)
        {
            string folderPath;

            if (_folderPath == "" || _folderPath == null)
            {
                return false;
            }

            if (_isRealPath)
            {
                folderPath = _folderPath;
            }
            else
            {
                HttpContext context = HttpContext.Current;
                folderPath = context.Server.MapPath(_folderPath);		// 절대 경로로 변경
            }

            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);

                    return true;
                }
                else
                {
                    Helper.AppException.WriteException("Not Found Path : " + folderPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelFolder : ", ex);
                return false;
            }
        }

        /// <summary>
        /// 파일 복사
        /// </summary>
        /// <param name="_fromWebFilePath">현재파일경로</param>
        /// <param name="_toWebFilePath">복사될 파일경로</param>
        public static void CopyFile(string _fromWebFilePath, string _toWebFilePath)
        {
            if (_fromWebFilePath == "" || _fromWebFilePath == null)
            {
                return;
            }

            HttpContext context = HttpContext.Current;
            string fromPath = context.Server.MapPath(_fromWebFilePath);		// 절대 경로로 변경
            string toPath = context.Server.MapPath(_toWebFilePath);		// 절대 경로로 변경

            if (File.Exists(fromPath))
            {
                if (!Directory.Exists(toPath.Substring(0, toPath.LastIndexOf("\\"))))
                {
                    Directory.CreateDirectory(toPath.Substring(0, toPath.LastIndexOf("\\")));
                }

                File.Copy(fromPath, toPath, true);
            }
        }

        /// <summary>
        /// 자동적으로 경로에 맞게 폴더 자동 생성해 주기
        /// </summary>
        /// <param name="_createFolderPath"></param>
        public static void CreateSubFolder(string _createFolderPath)
        {
            if (_createFolderPath.Length == 0) return;

            HttpContext context = HttpContext.Current;
            string[] folderStr = _createFolderPath.Split('/');
            string folderName = "";
            foreach (string folder in folderStr)
            {
                if (folder.Length > 0)
                {
                    folderName += "/" + folder;
                    if (!Directory.Exists(context.Server.MapPath(folderName)))
                    {
                        Directory.CreateDirectory(context.Server.MapPath(folderName));
                    }
                }
            }
        }


        #region 파일 리스트 정렬

        public enum SortDirection
        {
            Asc,
            Desc
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 파일 리스트 정렬(UploadFileInfo 용)
        /// </summary>
        public class FileSortComparer : IComparer
        {
            private SortDirection m_direction = SortDirection.Asc;

            public FileSortComparer()
                : base()
            {
            }

            public FileSortComparer(SortDirection direction)
            {
                this.m_direction = direction;
            }

            int IComparer.Compare(object x, object y)
            {
                UploadFileInfo fileX = (UploadFileInfo)x;
                UploadFileInfo fileY = (UploadFileInfo)y;

                if (fileX == null && fileY == null)
                {
                    return 0;
                }
                else if (fileX == null && fileY != null)
                {
                    return (this.m_direction == SortDirection.Asc) ? -1 : 1;
                }
                else if (fileX != null && fileY == null)
                {
                    return (this.m_direction == SortDirection.Asc) ? 1 : -1;
                }
                else
                {
                    return (this.m_direction == SortDirection.Asc) ?
                        fileX.fileLastWriteTime.CompareTo(fileY.fileLastWriteTime) :
                        fileY.fileLastWriteTime.CompareTo(fileX.fileLastWriteTime);
                }
            }
        }

        #endregion 파일 리스트 정렬
    }

    #region Enum

    /// <summary>
    /// ENUM : ApprovalComplete = 0, Progress = 1, Draft = 2,UserComplete = 3, Board = 4, Poll = 5,Picture = 6, Pims = 10
    /// </summary>
    public enum FolderType
    {
        /// <summary>
        /// ApprovalComplete
        /// </summary>
        Approval = 0,

        /// <summary>
        /// Progress
        /// </summary>
        //Progress = 1,

        /// <summary>
        /// Draft
        /// </summary>
        Draft = 2,

        /// <summary>
        /// UserComplete
        /// </summary>
        UserComplete = 3,

        /// <summary>
        /// Board
        /// </summary>
        Board = 4,

        /// <summary>
        /// Poll
        /// </summary>
        Poll = 5,

        /// <summary>
        /// Picture
        /// </summary>
        Picture = 6,

        /// <summary>
        /// Opinion
        /// </summary>
        Opinion = 7,

        /// <summary>
        /// Community
        /// </summary>
        Community = 8,

        /// <summary>
        /// Resource
        /// </summary>
        ResourceManager = 9,

        /// <summary>
        /// Pims
        /// </summary>
        Pims = 10,

        /// <summary>
        /// Work
        /// </summary>
        Work = 11,

        /// <summary>
        /// Mail
        /// </summary>
        Mail = 12,

        /// <summary>
        /// None
        /// </summary>
        None = 13,

        /// <summary>
        /// MyBoard
        /// </summary>
        MyBoard = 14,

        /// <summary>
        /// 목표관리 (MBO)
        /// </summary>
        MBO = 15,

        /// <summary>
        /// 제안제도
        /// </summary>
        Proposal = 16,

        /// <summary>
        /// 업무일지
        /// </summary>
        Report = 17,

        /// <summary>
        /// 주소록
        /// </summary>
        ADDRESS = 18,

        /// <summary>
        /// 업무협업
        /// </summary>
        COOP = 19,

        /// <summary>
        /// 나의 업무협업
        /// </summary>
        MYCOOPERATION = 20,

        /// <summary>
        /// 프로젝트 board
        /// </summary>
        PROJECTBOARD = 21,

        /// <summary>
        /// 프로젝트 단위
        /// </summary>
        PROJECTUNIT = 22,

        /// <summary>
        /// 방문예약
        /// </summary>
        VISIT = 23,

        /// <summary>
        /// SNS
        /// </summary>
        SNS = 24,

        /// <summary>
        /// TASK
        /// </summary>
        TASK = 25,

        /// <summary>
        /// TaskDraft
        /// </summary>
        TaskDraft = 26,
    };

    #endregion Enum

    #region UpladFile Class

    /// <summary>
    /// 업로드할 클래스 설정
    /// </summary>
    [Serializable]
    public class UploadFileInfo
    {
        /// <summary>
        /// filePath
        /// </summary>
        public string filePath;

        /// <summary>
        /// fileName
        /// </summary>
        public string fileName;

        /// <summary>
        /// fileType
        /// </summary>
        public string fileType;

        /// <summary>
        /// fileLength
        /// </summary>
        public long fileLength;

        /// <summary>
        /// Save FileName
        /// </summary>
        public string fileGUID;

        /// <summary>
        /// 파일생성일자
        /// </summary>
        public DateTime fileLastWriteTime;
    }

    #endregion UpladFile Class
}
