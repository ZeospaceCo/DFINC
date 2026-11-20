using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Education.BE;
using System.Web;
using System.Transactions;
using System.IO;
using System.Globalization;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class Attach : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가하기 

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_yearMonth"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, int _groupID, string _spoolPath, string _yearMonth)
        {
            string targetPath = GetAttachPath(_companyID, _listID, _yearMonth);
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.None, _listID, false, _yearMonth);
            DAL.Attach dalAttach = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;                    
                    fileInfo.ListID = _listID;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    dalAttach.Add(fileInfo);                    
                }
            }

            return true;
        }

        #endregion 

        #region 수정 첨부 추가

        /// <summary>
        /// 수정 화면에서 새로 업로드한 파일만 추가하고 기존 파일은 복사하거나 삭제하지 않습니다.
        /// 업로드 파일 저장에 실패하면 수정 트랜잭션을 완료하지 않도록 false를 반환합니다.
        /// </summary>
        public bool AddForModify(int _companyID, int _listID, string _spoolPath)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || _companyID <= 0 || _listID <= 0 || String.IsNullOrEmpty(_spoolPath)) return false;

            string spoolPath = context.Server.MapPath(_spoolPath);
            // 파일을 올리지 않으면 업로드 디렉터리가 생성되지 않으므로 정상적인 무첨부 수정입니다.
            if (!Directory.Exists(spoolPath)) return true;
            int expectedCount = Directory.GetFiles(spoolPath).Length;
            if (expectedCount == 0) return true;

            string targetPath = GetAttachPath(_companyID, _listID, "");
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.None, _listID, false, "");
            if (fileList == null || fileList.Count != expectedCount) return false;

            fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
            DAL.Attach dalAttach = new DAL.Attach();
            foreach (UploadFileInfo upload in fileList)
            {
                if (String.IsNullOrEmpty(upload.fileName) || upload.fileName.Length > 500
                    || String.IsNullOrEmpty(upload.filePath) || upload.filePath.Length > 1000
                    || (upload.fileType ?? "").Length > 5
                    || String.IsNullOrEmpty(upload.fileGUID) || upload.fileGUID.Length > 200
                    || upload.fileLength < 0)
                    return false;

                string savedPath = context.Server.MapPath(upload.filePath + upload.fileGUID);
                if (!File.Exists(savedPath) || new FileInfo(savedPath).Length != upload.fileLength) return false;

                BE.AttachInfo fileInfo = new BE.AttachInfo();
                fileInfo.CompanyID = _companyID;
                fileInfo.ListID = _listID;
                fileInfo.FilePath = upload.filePath;
                fileInfo.FileName = upload.fileName;
                fileInfo.FileType = upload.fileType;
                fileInfo.FileLength = upload.fileLength;
                fileInfo.FileGUID = upload.fileGUID;
                if (!dalAttach.Add(fileInfo)) return false;
            }
            return true;
        }

        #endregion

        #region MOD

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_spoolPath"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _listID, int _groupID, string _spoolPath)
        {
            string targetPath = GetAttachPath(_companyID, _listID, "");
            ArrayList fileList = FileHelper.GetCopyFileList(_spoolPath, targetPath, FolderType.None, _listID);
            DAL.Attach dalAttach = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;
                    fileInfo.ListID = _listID;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    dalAttach.Add(fileInfo);
                }
            }

            return true;
        }
     
        #endregion

        private string GetAttachPath(int companyID, int listID, string yearMonth)
        {
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrEmpty(yearMonth)) yearMonth = FileHelper.GetYearMonth();
            return basePath.TrimEnd('/') + "/" + companyID + "/Education/" + yearMonth + "/" + listID;
        }

        #region 삭제하기

        /// <summary>
        /// 수정 가능한 교육의 선택한 일반 첨부 DB 기록과 실제 파일을 함께 삭제합니다.
        /// </summary>
        public bool DelForModify(int companyID, int listID, int attachID)
        {
            if (companyID <= 0 || listID <= 0 || attachID <= 0 || HttpContext.Current == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 교육 존재 여부와 첨부 정보를 같은 트랜잭션 안에서 다시 확인합니다.
                    if (new DAL.List().GetModifyState(companyID, listID) == 0) return false;

                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> attachments = dalAttach.GetAttachList(companyID, listID);
                    BE.AttachInfo selected = null;
                    foreach (BE.AttachInfo attachment in attachments)
                    {
                        if (attachment == null || attachment.AttachID != attachID) continue;
                        if (selected != null || attachment.CompanyID != companyID || attachment.ListID != listID)
                            return false;
                        selected = attachment;
                    }
                    if (selected == null) return false;

                    // 경로는 클라이언트 값이 아니라 현재 회사와 교육에서 조회한 DB 정보만 사용합니다.
                    string storedFileName = String.IsNullOrEmpty(selected.FileGUID) ? selected.FileName : selected.FileGUID;
                    string filePath = GetSafeModifyAttachPath(companyID, listID, selected.FilePath, storedFileName);
                    if (dalAttach.IsSharedFile(companyID, listID, attachID, selected.FilePath, storedFileName))
                        return false;

                    if (!dalAttach.Del(companyID, listID, attachID)) return false;
                    // 없는 파일만 정상으로 처리하고, 접근 거부 등 삭제 실패는 DB 삭제도 완료하지 않습니다.
                    DeleteModifyAttachFile(filePath);
                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                        String.Format(CultureInfo.InvariantCulture, "Education.DelForModify ({0}, {1}, {2})",
                            companyID, listID, attachID), ex);
                }
                catch (Exception) { }
                return false;
            }
        }

        private static string GetSafeModifyAttachPath(int companyID, int listID, string fileFolder, string storedFileName)
        {
            if (String.IsNullOrWhiteSpace(storedFileName) || storedFileName != storedFileName.Trim()
                || storedFileName == "." || storedFileName == ".." || storedFileName.EndsWith(".", StringComparison.Ordinal)
                || storedFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || storedFileName.IndexOf('/') >= 0 || storedFileName.IndexOf('\\') >= 0
                || !String.Equals(Path.GetFileName(storedFileName), storedFileName, StringComparison.Ordinal))
                throw new InvalidOperationException("첨부파일명이 안전한 단일 파일명이 아닙니다.");
            string deviceName = storedFileName.Split('.')[0].TrimEnd(' ').ToUpperInvariant();
            if (deviceName == "CON" || deviceName == "PRN" || deviceName == "AUX" || deviceName == "NUL"
                || (deviceName.Length == 4 && (deviceName.StartsWith("COM", StringComparison.Ordinal)
                    || deviceName.StartsWith("LPT", StringComparison.Ordinal))
                    && ((deviceName[3] >= '1' && deviceName[3] <= '9')
                        || deviceName[3] == '\u00b9' || deviceName[3] == '\u00b2' || deviceName[3] == '\u00b3')))
                throw new InvalidOperationException("첨부파일명이 예약된 장치명입니다.");

            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrWhiteSpace(basePath) || String.IsNullOrWhiteSpace(fileFolder))
                throw new InvalidOperationException("첨부파일 저장 경로를 확인할 수 없습니다.");
            string educationFolder = String.Format(CultureInfo.InvariantCulture, "{0}/{1}/Education/",
                basePath.TrimEnd('/'), companyID);
            if (!fileFolder.StartsWith(educationFolder, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("첨부파일이 현재 회사의 교육 저장 폴더 밖에 있습니다.");

            // 수정 시 추가한 파일은 교육 등록 월과 다른 월 폴더에도 저장될 수 있습니다.
            string[] folders = fileFolder.Substring(educationFolder.Length).Split('/');
            DateTime month;
            if (folders.Length != 3 || folders[0].Length != 6
                || !DateTime.TryParseExact(folders[0], "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out month)
                || folders[1] != listID.ToString(CultureInfo.InvariantCulture) || folders[2].Length != 0)
                throw new InvalidOperationException("첨부파일이 해당 교육의 저장 폴더 밖에 있습니다.");

            string expectedFolder = Path.GetFullPath(FileHelper.GetServerMapPath(educationFolder + folders[0] + "/"
                + listID.ToString(CultureInfo.InvariantCulture) + "/"))
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fullPath = Path.GetFullPath(FileHelper.GetServerMapPath(fileFolder + storedFileName));
            if (!String.Equals(Path.GetDirectoryName(fullPath), expectedFolder, StringComparison.OrdinalIgnoreCase)
                || !String.Equals(Path.GetFileName(fullPath), storedFileName, StringComparison.Ordinal))
                throw new InvalidOperationException("첨부파일의 실제 경로가 해당 교육의 저장 폴더 밖에 있습니다.");

            ValidateModifyAttachPath(fullPath);
            return fullPath;
        }

        private static void ValidateModifyAttachPath(string filePath)
        {
            for (string current = filePath; !String.IsNullOrEmpty(current); current = Path.GetDirectoryName(current))
            {
                FileAttributes attributes;
                try { attributes = File.GetAttributes(current); }
                catch (FileNotFoundException) { continue; }
                catch (DirectoryNotFoundException) { continue; }
                if ((attributes & FileAttributes.ReparsePoint) != 0)
                    throw new InvalidOperationException("연결된 첨부파일 또는 폴더는 삭제하지 않습니다.");
                if (current == filePath && (attributes & FileAttributes.Directory) != 0)
                    throw new InvalidOperationException("첨부파일 대신 폴더가 지정되어 있습니다.");
            }
        }

        private static void DeleteModifyAttachFile(string filePath)
        {
            // DB 삭제 직후에도 경로를 다시 확인하여 연결된 파일이나 폴더를 거부합니다.
            ValidateModifyAttachPath(filePath);
            try
            {
                FileAttributes attributes = File.GetAttributes(filePath);
                if ((attributes & (FileAttributes.Directory | FileAttributes.ReparsePoint)) != 0)
                    throw new InvalidOperationException("첨부파일의 종류가 올바르지 않습니다.");
                File.Delete(filePath);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }
        }

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _attachID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttach(_companyID, _listID, _attachID);

                    // 관련 DB 삭제
                    DAL.Attach dal = new DAL.Attach();
                    bool response = dal.Del(_companyID, _listID, _attachID);

                    HttpContext context = HttpContext.Current;

                    if (response)
                    {
                        foreach (BE.AttachInfo be in alAttach)
                        {
                            string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }

        #endregion 

    }
}
