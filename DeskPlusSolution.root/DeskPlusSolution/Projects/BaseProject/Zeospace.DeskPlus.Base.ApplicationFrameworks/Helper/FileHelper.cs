using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    #region FileHelper Class

    /// <summary>
    /// FileHelper에 대한 요약 설명입니다.
    /// </summary>
    public abstract class FileHelper
    {
        #region 회사 관련 폴더 추가/수정/삭제

        /// <summary>
        /// 회사 폴더 경로 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        public static string GetCompanyFolder(int _companyID)
        {
            HttpContext context = HttpContext.Current;

            string folderName = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            folderName = context.Server.MapPath(folderName);
            if (folderName.EndsWith("\\"))
            {
                folderName += _companyID;
            }
            else
            {
                folderName += @"\" + _companyID;
            }

            return folderName;
        }

        /// <summary>
        /// 회사 폴더 만들기
        /// </summary>
        /// <param name="_companyID">화사 아이디</param>
        /// <returns></returns>
        public static bool AddCompanyFolder(int _companyID)
        {
            try
            {
                if (_companyID == 0) return false;

                //사용자 디렉토리 확인
                DirectoryInfo di = new DirectoryInfo(@GetCompanyFolder(_companyID));
                if (di.Exists == false)
                {
                    di.Create();
                    DirectoryInfo dis = di.CreateSubdirectory("Approval");
                    DirectoryInfo dis1 = dis.CreateSubdirectory("Complete");
                    dis1 = dis.CreateSubdirectory("Progress");
                    dis = di.CreateSubdirectory("Forms");

                    dis = di.CreateSubdirectory("Board");
                    dis = di.CreateSubdirectory("Community");
                    dis = di.CreateSubdirectory("Etc");
                    dis = di.CreateSubdirectory("Opinion");
                    dis = di.CreateSubdirectory("Pims");
                    dis = di.CreateSubdirectory("Report");
                    dis = di.CreateSubdirectory("Coop");
                    dis = di.CreateSubdirectory("ResourceManager");
                    dis = di.CreateSubdirectory("Users");
                    dis = di.CreateSubdirectory("Address");

                    DirectoryInfo dirProj = di.CreateSubdirectory("PMS");
                    DirectoryInfo dirSubProj = dirProj.CreateSubdirectory("Board");
                    dirSubProj = dirProj.CreateSubdirectory("Unit");
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_companyID + " Company Folder Create Error : [" + ex.Message + "]");
                return false;
            }
        }

        #endregion 회사 관련 폴더 추가/수정/삭제

        #region 사용자 관련 폴더 추가/수정/삭제

        /// <summary>
        /// 사용자 폴더 경로 가져오기
        /// </summary>
        /// <param name="_companyID">회사 아이디</param>
        /// <param name="_userID">사용자 아이디</param>
        public static string GetUserFolder(int _companyID, int _userID)
        {
            HttpContext context = HttpContext.Current;

            string folderName = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            folderName = context.Server.MapPath(folderName);
            if (folderName.EndsWith("\\"))
            {
                folderName += _companyID + @"\users\" + _userID;
            }
            else
            {
                folderName += @"\" + _companyID + @"\users\" + _userID;
            }

            return folderName;
        }

        /// <summary>
        /// 사용자 폴더 생성하기
        /// </summary>
        /// <param name="_companyID">회사 아이디</param>
        /// <param name="_userID">사용자 아이디</param>
        /// <returns></returns>
        public static bool AddUserFolder(int _companyID, int _userID)
        {
            try
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                //사용자 디렉토리 확인
                DirectoryInfo di = new DirectoryInfo(@GetUserFolder(_companyID, _userID));
                if (di.Exists == false)
                {
                    di.Create();
                    DirectoryInfo dis = di.CreateSubdirectory("Approval");
                    DirectoryInfo dis1 = dis.CreateSubdirectory("Complete");
                    dis1 = dis.CreateSubdirectory("Draft");

                    dis = di.CreateSubdirectory("Board");
                    dis = di.CreateSubdirectory("Mail");
                    dis = di.CreateSubdirectory("Picture");
                    dis = di.CreateSubdirectory("Pims");
                    dis = di.CreateSubdirectory("Spool");
                    dis = di.CreateSubdirectory("WebHard");
                    dis = di.CreateSubdirectory("Address");
                    dis = di.CreateSubdirectory("Visit");

                    try
                    {
                        //picture 폴더 생성후, 기본 이미지 인장 추가 (전자결재 땜시~~)
                        if (AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "AddNoSignImage", "") == "Y")
                        {
                            HttpContext context = HttpContext.Current;
                            File.Copy(context.Server.MapPath(baseImageDirectory + "/C/Img/Approval/app_ico_NoSign.jpg"),
                                context.Server.MapPath(baseFileDirectory + _companyID + "/users/" + _userID + "/picture/seal.jpg"), true);
                        }
                    }
                    catch // 기존 사이트에 accont.xml 에 정보가 세팅되어 있지 않을 경우를 대비
                    {
                        HttpContext context = HttpContext.Current;
                        File.Copy(context.Server.MapPath(baseImageDirectory + "/C/Img/Approval/app_ico_NoSign.jpg"),
                            context.Server.MapPath(baseFileDirectory + _companyID + "/users/" + _userID + "/picture/seal.jpg"), true);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_userID + " User Folder Create Error : [" + ex.Message + "]");
                return false;
            }
        }

        /// <summary>
        /// 회사 폴더 삭제
        /// </summary>
        /// <param name="_companyID">회사 아이디</param>
        /// <returns></returns>
        public static bool DelCompanyFolder(int _companyID)
        {
            try
            {
                //사용자 디렉토리 확인
                DirectoryInfo di = new DirectoryInfo(@GetCompanyFolder(_companyID));
                if (di.Exists)
                {
                    di.Delete(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_companyID + " Company Folder Delete Error : [" + ex.Message + "]");
                return false;
            }
        }

        /// <summary>
        /// 사용자 폴더 삭제
        /// </summary>
        /// <param name="_companyID">회사 아이디</param>
        /// <param name="_userID">사용자 아이디</param>
        /// <returns></returns>
        public static bool DelUserFolder(int _companyID, int _userID)
        {
            try
            {
                //사용자 디렉토리 확인
                //				DirectoryInfo di = new DirectoryInfo(@GetUserFolder(_companyID, _userID));
                //				if (di.Exists)
                //				{
                //					di.Delete(true);
                //				}
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_userID + " Company Folder Delete Error : [" + ex.Message + "]");
                return false;
            }
        }

        #endregion 사용자 관련 폴더 추가/수정/삭제

        #region 업로드 파일 정보 가져오기

        /// <summary>
        /// 수정시 삭제된 파일리스트 가져오기
        /// </summary>
        /// <param name="_tbHidenFileList">The _TB hiden file list.</param>
        /// <param name="_tbHiddenSaveFileList">The _TB hidden save file list.</param>
        /// <returns></returns>
        public static ArrayList GetDelFileList(string _tbHidenFileList, string _tbHiddenSaveFileList)
        {
            #region 수정시 삭제된 파일리스트 가져오기

            //_tbHidenFileList (타입>서버 파일 경로|) ==> S>/DeskPlusEIP/FileData/1/Borad/200607/3/>a.txt>GUID|W>/DeskPlusEIP/FileData/1/Users/WebHard/1/bbb.txt|
            //_tbHiddenOldFileList (파일ID<서버파일경로|파일ID<서버 파일 경로) ==> S>1>/DeskPlusEIP/FileData/1/Borad/200607/3/a.txt|S>2>/DeskPlusEIP/FileData/1/Borad/200607/4/bb.txt
            _tbHidenFileList = _tbHidenFileList.TrimEnd('|');           //불필요한 구분자 삭제
            _tbHiddenSaveFileList = _tbHiddenSaveFileList.TrimEnd('|'); //불필요한 구분자 삭제

            string[] saveFileListArr = _tbHiddenSaveFileList.Split('|'); //String 형식을 배열로 변환

            ArrayList fileList = GetServerFileList(_tbHidenFileList); // S Type(저장된 파일) 파일 리스트 가져오기
            ArrayList delFileList = new ArrayList();

            string attachFileID = String.Empty;
            string[] tmpFileListArr;

            for (int j = 0; j < saveFileListArr.Length; j++)
            {
                tmpFileListArr = saveFileListArr[j].Split('>');

                if (tmpFileListArr[0] == "S")
                {
                    attachFileID = tmpFileListArr[1].ToString();
                    if (!fileList.Contains(saveFileListArr[j]))
                    {
                        delFileList.Add(attachFileID + "|" + saveFileListArr[j]);	// 원본 삭제 파일들의 AttachFileID | FileFullName 값을 Array에 저장
                    }
                }
            }

            return delFileList;

            #endregion 수정시 삭제된 파일리스트 가져오기
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
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string targetPath = "";

            #region 인증자인지 확인(미승인자 차단)

            HttpContext context = HttpContext.Current;
            string loginID = HttpContext.Current.User.Identity.Name;
            FormsAuthenticationTicket ticket = null;

            string useSession = Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseSession", "N");
            if (useSession == "Y")
            {
                ticket = FormsAuthentication.Decrypt(context.Session["DeskPlusEIP"].ToString());
            }
            else
            {
                string cookieName = "DeskPlusEIP" + loginID;

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

                case FolderType.COOP:
                    targetPath = baseFileDirectory + companyID + "/Coop/" + _nowYearMon + "/" + _objectID;
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FileHelper[GetCopyFileList] : " + ex.Message);
                return null;
            }

            return serverFile;
        }

        /// <summary>
        /// 서버에 저장된 파일 리스트를 가져온다
        /// </summary>
        /// <param name="_tbHidenFileList">The _TB hiden file list.</param>
        /// <returns></returns>
        public static ArrayList GetServerFileList(string _tbHidenFileList)
        {
            //S : 저장된 파일(수정모드) GetDelFileList, L : 로컬 파일(GetUploadFile), W : 웹폴더, A : 전자결재, B : 지식맵, M : 메일 (GetCopyFileList)
            //S>파일패스>파일명>GUID|W>/DeskPlusEIP/FileData/1/Users/WebHard/1/bbb.txt|...
            //S>/DeskPlusEIP/Test/2/>test.exe>GUID|W>/DeskPlusEIP/FileData/1/Users/WebHard/1/bbb.txt|...
            _tbHidenFileList = _tbHidenFileList.TrimEnd('|');
            string[] hdFileListArr = _tbHidenFileList.Split('|');
            string tmpFileList = String.Empty;
            string[] tmpFileListArr;
            ArrayList serverFile = new ArrayList();
            for (int i = 0; i < hdFileListArr.Length; i++)
            {
                tmpFileList = hdFileListArr[i];
                tmpFileListArr = tmpFileList.Split('>');

                //저장된 파일과 로컬 파일이 아닌 서버에 있는 파일 리스트
                if (tmpFileListArr[0] == "S")
                {
                    serverFile.Add(hdFileListArr[i]);
                }
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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Deleted SpoolPath : " + spoolPath);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FileHelper[GetUploadFile] : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 새로운 파일이름 가져오기
        /// </summary>
        /// <param name="_targetPath">The _target path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <returns></returns>
        private static string NewFileName(string _targetPath, string _fileName)
        {
            int i = 1;
            string targetPath = _targetPath + RemoveSpecialChar(_fileName);
            string fileName = "";
            int fileIdx = -1;

            while (File.Exists(targetPath))
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
        /// 파일 사이즈
        /// </summary>
        /// <param name="_size">The _size.</param>
        /// <returns></returns>
        public static string GetFileSize(string _size)
        {
            return String.Format("{0:n1}", Convert.ToDecimal(_size) / 1024) + " KB";
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
        /// 작성자 : 심재운
        /// 파일 리스트 정렬 (최신 작성일자 ASC) - fileInfo[] 용
        /// </summary>
        public class FileListSort : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = (FileInfo)x;
                FileInfo yInfo = (FileInfo)y;
                //return yInfo.LastWriteTime.CompareTo(xInfo.LastWriteTime); //desc 오름차순
                return xInfo.LastWriteTime.CompareTo(yInfo.LastWriteTime); //asc 내림차순
            }

            #endregion IComparer Members
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

        #endregion 업로드 파일 정보 가져오기

        #region Method

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

        #endregion Method

        #region 서버파일 처리

        /// <summary>
        /// 폴더 생성 하기
        /// </summary>
        /// <param name="_tagetFolder_physicalPath">폴더 경로</param>
        /// <param name="_newFolderName">폴더명</param>
        /// <returns></returns>
        public static bool AddFolder(string _tagetFolder_physicalPath, string _newFolderName)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(_tagetFolder_physicalPath);
                if (di.Exists == true)
                {
                    di.CreateSubdirectory(_newFolderName);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CreateFolder Folder Create Error : [" + ex.Message + "]");
                return false;
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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Not Found Path : " + folderPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 하위폴더 삭제(하위폴더들 중 파일 없는 하위폴더삭제) ex : DelNoFileSubFolder("/DeskPlusEIP/상위폴더")
        /// </summary>
        /// <param name="_absoluteRootPath">상위폴더의 절대경로</param>
        /// <returns>bool</returns>
        public static bool DelSubFolderNoFile(string _absoluteRootPath)
        {
            try
            {
                //절대경로 -> 파일경로
                string filePath = GetServerMapPath(_absoluteRootPath);
                DirectoryInfo di = new DirectoryInfo(@filePath);

                FileSystemInfo[] dirs = di.GetDirectories("*");

                foreach (DirectoryInfo diNext in dirs)
                {
                    //if dic have no file~
                    if (diNext.GetFileSystemInfos("*").Length == 0)
                    {
                        DirectoryInfo noDatadi = new DirectoryInfo(@filePath + "\\" + diNext);

                        if (noDatadi.Exists == true)
                        {
                            noDatadi.Delete(true);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

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
            //string newFolder = "";

            try
            {
                HttpContext context = HttpContext.Current;
                fromPath = context.Server.MapPath(_fromWebPath);		// 절대 경로로 변경
                toPath = context.Server.MapPath(_toWebPath);		// 절대 경로로 변경

                if (Directory.Exists(fromPath))
                {
                    CopyFiles(_fromWebPath, _toWebPath);

                    // 2015-10-08 (withsuki) CopyFiles에서 처리. 중복코드 제거.
                    //string[] subDir = Directory.GetDirectories(fromPath);

                    //for (int i = 0; i < subDir.Length; i++)
                    //{
                    //    newFolder = subDir[i].Replace(fromPath, "").Replace("\\", "/");
                    //    CopyFiles(_fromWebPath + newFolder, _toWebPath + newFolder);
                    //}

                    if (_deleteFromPath)
                    {
                        //Directory.Delete(fromPath, true);

                        if (!DeleteDirectory(fromPath))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Deleted FromWebPath : " + _fromWebPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 디렉토리 안의 파일과 하위디렉토리 삭제
        /// </summary>
        /// <param name="target_dir"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(string target_dir)
        {
            bool result = false;
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            try
            {
                Directory.Delete(target_dir, false);
            }
            catch (IOException)
            {
                Thread.Sleep(0);
                Directory.Delete(target_dir, false);
            }

            return result;
        }

        /// <summary>
        /// 파일 생성하기
        /// </summary>
        /// <param name="_tagetFolder_physicalPath">폴더</param>
        /// <param name="_newFileName">파일명</param>
        /// <param name="_content">내용</param>
        public static void AddFile(string _tagetFolder_physicalPath, string _newFileName, string _content)
        {
            if (Directory.Exists(_tagetFolder_physicalPath))
            {
                //string nowYear = DateTime.Now.Year.ToString();
                //string nowMon = DateTime.Now.ToString("MM");
                //string nowDay = DateTime.Now.ToString("dd");
                //string nowHour = DateTime.Now.ToString("HH");

                StreamWriter stream = null;
                try
                {
                    stream = new StreamWriter(_tagetFolder_physicalPath + "\\" + _newFileName, true, Encoding.Default);
                    stream.Write(_content);
                }
                catch { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
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
        /// 폴더내의 파일을 카피한다
        /// </summary>
        /// <param name="_fromWebPath">원본 폴더</param>
        /// <param name="_toWebPath">대상 폴더</param>
        /// <param name="_subDir">하위 디렉토리까지 적용</param>
        /// <param name="_extension">카피 할 파일 확장자를 지정</param>
        public static void CopyFiles(string _fromWebPath, string _toWebPath, bool _subDir, string _extension)
        {
            if (_fromWebPath == "" || _fromWebPath == null)
            {
                return;
            }

            HttpContext context = HttpContext.Current;
            string fromPath = context.Server.MapPath(_fromWebPath);		// 물리적 경로로 변경
            string toPath = context.Server.MapPath(_toWebPath);		// 물리적 경로로 변경

            int tmpfileIdx = 0;
            string tmpFile = String.Empty;
            string fileName = String.Empty;
            string sourceFile = String.Empty;
            string destinationFile = String.Empty;
            string newFolder = String.Empty;

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
                string fileExtention = "";

                for (int j = 0; j < fileArr.Length; j++)
                {
                    tmpFile = fileArr[j];
                    tmpfileIdx = tmpFile.LastIndexOf("\\");
                    fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                    fileExtention = Path.GetExtension(fileName);

                    if (_extension.Trim() != "")
                    {
                        if (fileExtention.ToUpper() == _extension.ToUpper())
                        {
                            sourceFile = fromPath + fileName;
                            destinationFile = toPath + fileName;
                            File.Copy(fileArr[j], destinationFile, true);
                        }
                    }
                    else
                    {
                        sourceFile = fromPath + fileName;
                        destinationFile = toPath + fileName;
                        File.Copy(fileArr[j], destinationFile, true);
                    }
                }

                if (_subDir)  // 원본 폴더의 모든 파일을 카피하기
                {
                    string[] subDir = Directory.GetDirectories(fromPath);

                    for (int i = 0; i < subDir.Length; i++)
                    {
                        newFolder = subDir[i].Replace(fromPath, "").Replace("\\", "/");
                        CopyFiles(_fromWebPath + newFolder, _toWebPath + newFolder);
                    }
                }
            }
        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="_filePath">파일 경로</param>
        /// <param name="_isRealPath">절대경로여부</param>
        /// <returns></returns>
        public static bool DelFile(string _filePath, bool _isRealPath)
        {
            string filePath;

            if (_filePath == "" || _filePath == null)
            {
                return false;
            }

            //절대경로일경우
            if (_isRealPath)
            {
                filePath = _filePath;
            }
            else
            {
                HttpContext context = HttpContext.Current;
                filePath = context.Server.MapPath(_filePath);		// 절대 경로로 변경
            }

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    return true;
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Not Found File : " + filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 파일명 특수문자 제거
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-01
        /// </summary>
        /// <param name="_fileName">Name of the _file.</param>
        /// <returns></returns>
        public static string RemoveSpecialChar(string _fileName)
        {
            if (_fileName.Length == 0) return "";

            _fileName.Replace("\\", "");
            _fileName.Replace("/", "");
            _fileName.Replace(":", "");
            _fileName.Replace("*", "");
            _fileName.Replace("?", "");
            _fileName.Replace("\"", "");
            _fileName.Replace("<", "");
            _fileName.Replace(">", "");
            _fileName.Replace("|", "");

            return _fileName;
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

        #endregion 서버파일 처리
    }

    #endregion FileHelper Class

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