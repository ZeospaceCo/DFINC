using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Education.BE;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class TOAttach : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 첨부파일은 유지하고 대상자의 완료 정보만 취소합니다.
        /// </summary>
        public CompletionCancelResult CancelComplete(int companyID, int listID, int userID)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0)
                throw new ArgumentException("완료 취소할 교육 대상자 정보가 올바르지 않습니다.");

            return new CompletionCancelResult
            {
                Cancelled = new DAL.TOUser().CancelComplete(companyID, listID, userID)
            };
        }

        /// <summary>미완료 교육에서 해당 사용자의 첨부 등록만 삭제합니다.</summary>
        public bool DeleteAttachment(int companyID, int listID, int userID, string fileGuid)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0)
                throw new ArgumentException("첨부파일을 삭제할 교육 대상자 정보가 올바르지 않습니다.");
            if (String.IsNullOrWhiteSpace(fileGuid) || fileGuid.Length > 200 || fileGuid != fileGuid.Trim()
                || fileGuid == "." || fileGuid == ".." || fileGuid.EndsWith(".", StringComparison.Ordinal)
                || fileGuid.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || fileGuid.IndexOf('/') >= 0 || fileGuid.IndexOf('\\') >= 0
                || !String.Equals(Path.GetFileName(fileGuid), fileGuid, StringComparison.Ordinal))
                throw new ArgumentException("삭제할 첨부파일 정보가 올바르지 않습니다.");

            var dal = new DAL.TOAttach();
            string yearMonth = dal.GetYearMonth(companyID, listID);
            List<TOAttachInfo> deletedFiles;
            // DAL이 완료 처리와 같은 잠금 안에서 완료 여부를 다시 검사한 후 삭제합니다.
            if (!dal.DeleteAttachment(companyID, listID, userID, fileGuid, out deletedFiles)) return false;

            string expectedFolder = null;
            foreach (TOAttachInfo file in deletedFiles)
            {
                try
                {
                    if (file == null)
                        throw new InvalidOperationException("삭제한 첨부파일의 저장 정보를 확인할 수 없습니다.");
                    // DB에서 다른 첨부도 참조하는 실제 파일은 유지합니다.
                    if (file.SharedFile) continue;
                    if (expectedFolder == null)
                        expectedFolder = GetCertificateFolder(companyID, listID, userID, yearMonth);
                    DeleteCertificateFile(GetSafeCertificatePath(expectedFolder,
                        FileHelper.GetServerMapPath(file.FilePath + file.FileGUID), file.FileGUID));
                }
                catch (Exception ex)
                {
                    // 등록 정보는 이미 커밋되었습니다. 다른 파일을 지우지 않고 정리 오류만 기록합니다.
                    try
                    {
                        Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                            String.Format(CultureInfo.InvariantCulture, "Education.DeleteAttachment file cleanup ({0}, {1}, {2})",
                                companyID, listID, userID), ex);
                    }
                    catch (Exception) { }
                }
            }
            return true;
        }

        private static string GetCertificateFolder(int companyID, int listID, int userID, string yearMonth)
        {
            DateTime month;
            if (String.IsNullOrEmpty(yearMonth) || yearMonth.Length != 6
                || !DateTime.TryParseExact(yearMonth, "yyyyMM", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out month))
                throw new InvalidOperationException("교육의 수료증 저장 월을 확인할 수 없습니다.");
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrWhiteSpace(basePath))
                throw new InvalidOperationException("첨부파일 저장 경로가 설정되지 않았습니다.");
            string targetPath = String.Format(CultureInfo.InvariantCulture, "{0}/{1}/Education/{2}/{3}/{4}/",
                basePath.TrimEnd('/'), companyID, yearMonth, listID, userID);
            return Path.GetFullPath(FileHelper.GetServerMapPath(targetPath))
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string GetSafeCertificatePath(string expectedFolder, string mappedPath, string fileGuid)
        {
            if (String.IsNullOrWhiteSpace(fileGuid) || fileGuid != fileGuid.Trim()
                || fileGuid == "." || fileGuid == ".." || fileGuid.EndsWith(".", StringComparison.Ordinal)
                || fileGuid.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || fileGuid.IndexOf('/') >= 0 || fileGuid.IndexOf('\\') >= 0
                || !String.Equals(Path.GetFileName(fileGuid), fileGuid, StringComparison.Ordinal))
                throw new InvalidOperationException("수료증 파일명이 안전한 단일 파일명이 아닙니다.");
            string deviceName = fileGuid.Split('.')[0].ToUpperInvariant();
            if (deviceName == "CON" || deviceName == "PRN" || deviceName == "AUX" || deviceName == "NUL"
                || (deviceName.Length == 4 && (deviceName.StartsWith("COM", StringComparison.Ordinal)
                    || deviceName.StartsWith("LPT", StringComparison.Ordinal))
                    && deviceName[3] >= '1' && deviceName[3] <= '9'))
                throw new InvalidOperationException("수료증 파일명이 예약된 장치명입니다.");

            string fullPath = Path.GetFullPath(mappedPath);
            string folder = Path.GetDirectoryName(fullPath);
            if (!String.Equals(folder, expectedFolder, StringComparison.OrdinalIgnoreCase)
                || !String.Equals(Path.GetFileName(fullPath), fileGuid, StringComparison.Ordinal))
                throw new InvalidOperationException("수료증 파일이 해당 교육 대상자의 저장 폴더 밖에 있습니다.");

            // 연결된 디렉터리나 파일을 통해 다른 경로로 삭제가 이어지지 않도록 상위 경로도 확인합니다.
            for (string current = fullPath; !String.IsNullOrEmpty(current); current = Path.GetDirectoryName(current))
            {
                FileAttributes attributes;
                try { attributes = File.GetAttributes(current); }
                catch (FileNotFoundException) { continue; }
                catch (DirectoryNotFoundException) { continue; }
                if ((attributes & FileAttributes.ReparsePoint) != 0)
                    throw new InvalidOperationException("연결된 수료증 파일 또는 폴더는 삭제하지 않습니다.");
                if (current == fullPath && (attributes & FileAttributes.Directory) != 0)
                    throw new InvalidOperationException("수료증 파일 대신 폴더가 지정되어 있습니다.");
            }
            return fullPath;
        }

        private static void DeleteCertificateFile(string filePath)
        {
            try
            {
                // File.Exists는 접근 거부도 false로 반환하므로 실제 속성 조회로 없는 파일만 구분합니다.
                FileAttributes attributes = File.GetAttributes(filePath);
                if ((attributes & (FileAttributes.Directory | FileAttributes.ReparsePoint)) != 0)
                    throw new InvalidOperationException("수료증 파일의 종류가 올바르지 않습니다.");
                File.Delete(filePath);
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }
        }

        public bool CompleteSaved(int companyID, int listID, int userID, int completeUserID)
        {
            // 완료에서는 파일을 조회/복사/재등록하지 않고 DB의 필수 첨부 여부만 검사합니다.
            return new DAL.TOAttach().CompleteSaved(companyID, listID, userID, completeUserID);
        }

        public bool SaveAttachments(int companyID, int listID, int userID, int writeUserID, string spoolPath)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0 || writeUserID <= 0)
                throw new ArgumentException("첨부파일을 등록할 교육 대상자 정보가 올바르지 않습니다.");
            var dal = new DAL.TOAttach();
            dal.EnsureCanSaveAttachments(companyID, listID, userID);
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrWhiteSpace(basePath))
                throw new InvalidOperationException("첨부파일 저장 경로가 설정되지 않았습니다.");
            string spoolRoot = String.Format(CultureInfo.InvariantCulture, "{0}/{1}/Users/{2}/Spool/",
                basePath.TrimEnd('/'), companyID, writeUserID);
            string trimmedSpoolPath = (spoolPath ?? String.Empty).TrimEnd('/');
            string uploadID = trimmedSpoolPath.Substring(trimmedSpoolPath.LastIndexOf('/') + 1);
            Guid uploadGuid;
            if (!Guid.TryParseExact(uploadID, "D", out uploadGuid) || uploadGuid == Guid.Empty
                || !String.Equals(spoolPath, spoolRoot + uploadID + "/", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("첨부파일의 임시 업로드 경로가 올바르지 않습니다.");
            string spoolFolder = Path.GetFullPath(FileHelper.GetServerMapPath(spoolPath))
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!Directory.Exists(spoolFolder)) return false;
            // 같은 요청의 중복 적용은 파일 복사와 임시 파일 정리가 교차되지 않게 막습니다.
            string lockPath = GetSafeCertificatePath(Path.GetDirectoryName(spoolFolder),
                spoolFolder + ".saving", uploadID + ".saving");
            try
            {
                using (var uploadLock = new FileStream(lockPath, FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None))
                {
                    return SaveAttachmentsFromSpool(dal, companyID, listID, userID, writeUserID,
                        basePath, spoolFolder, uploadGuid);
                }
            }
            finally
            {
                try { DeleteCertificateFile(lockPath); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        private static bool SaveAttachmentsFromSpool(DAL.TOAttach dal, int companyID, int listID,
            int userID, int writeUserID, string basePath, string spoolFolder, Guid uploadGuid)
        {
            if (!Directory.Exists(spoolFolder)) return false;
            string[] sourceFiles = Directory.GetFiles(spoolFolder);
            if (sourceFiles.Length == 0) return false;
            string yearMonth = dal.GetYearMonth(companyID, listID);
            string targetFolder = GetCertificateFolder(companyID, listID, userID, yearMonth);
            string targetPath = String.Format(CultureInfo.InvariantCulture, "{0}/{1}/Education/{2}/{3}/{4}/",
                basePath.TrimEnd('/'), companyID, yearMonth, listID, userID);
            GetSafeCertificatePath(targetFolder, Path.Combine(targetFolder, "path-check"), "path-check");
            Directory.CreateDirectory(targetFolder);
            var certificates = new List<TOAttachInfo>();
            var createdFiles = new List<TOAttachInfo>();
            bool saved = false;
            try
            {
                foreach (string sourceFile in sourceFiles)
                {
                    string fileName = Path.GetFileName(sourceFile);
                    string extension = Path.GetExtension(fileName);
                    if (fileName.Length > 500 || extension.Length > 50 || targetPath.Length > 1000)
                        throw new ArgumentException("첨부파일 이름 또는 경로가 너무 깁니다.");
                    string safeSource = GetSafeCertificatePath(spoolFolder, sourceFile, fileName);
                    using (var source = new FileStream(safeSource, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (source.Length <= 0) throw new ArgumentException("첨부파일이 비어 있습니다.");
                        // 같은 요청/파일명/내용이면 같은 GUID를 사용하여 재시도 중복 등록을 막습니다.
                        byte[] digest = GetUploadDigest(source, uploadGuid, fileName);
                        var guidBytes = new byte[16];
                        Array.Copy(digest, guidBytes, guidBytes.Length);
                        string fileGuid = new Guid(guidBytes).ToString("D") + extension;
                        string destination = GetSafeCertificatePath(targetFolder,
                            Path.Combine(targetFolder, fileGuid), fileGuid);
                        var certificate = new TOAttachInfo
                        {
                            CompanyID = companyID, ListID = listID, UserID = userID,
                            WriteUserID = writeUserID, FilePath = targetPath,
                            FileName = fileName, FileType = extension, FileLength = source.Length, FileGUID = fileGuid
                        };
                        if (File.Exists(destination))
                        {
                            using (var existing = new FileStream(destination, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                if (!String.Equals(Convert.ToBase64String(digest),
                                    Convert.ToBase64String(GetUploadDigest(existing, uploadGuid, fileName)), StringComparison.Ordinal))
                                    throw new InvalidOperationException("이미 저장된 첨부파일의 내용이 일치하지 않습니다.");
                            }
                        }
                        else
                        {
                            using (var destinationStream = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                            {
                                createdFiles.Add(certificate);
                                source.Position = 0;
                                source.CopyTo(destinationStream);
                            }
                        }
                        certificates.Add(certificate);
                    }
                }
                saved = dal.SaveAttachments(companyID, listID, userID, certificates, writeUserID);
                if (saved)
                {
                    // DB 저장이 성공한 후에만 이번 임시 업로드를 정리합니다.
                    try
                    {
                        foreach (string sourceFile in sourceFiles)
                            DeleteCertificateFile(GetSafeCertificatePath(spoolFolder, sourceFile, Path.GetFileName(sourceFile)));
                        if (Directory.Exists(spoolFolder) && Directory.GetFileSystemEntries(spoolFolder).Length == 0)
                            Directory.Delete(spoolFolder, false);
                    }
                    catch (Exception ex) { LogAttachmentCleanup(ex); }
                }
                return saved;
            }
            finally
            {
                if (!saved && createdFiles.Count > 0)
                {
                    // DB 응답이 끊긴 경우도 이미 등록된 파일은 유지합니다. 조회 실패 시에도 보존합니다.
                    try
                    {
                        List<TOAttachInfo> savedFiles = dal.GetList(companyID, listID, userID);
                        foreach (TOAttachInfo file in createdFiles)
                        {
                            if (!savedFiles.Exists(existing => existing.FileGUID == file.FileGUID))
                                DeleteCertificateFile(GetSafeCertificatePath(targetFolder,
                                    Path.Combine(targetFolder, file.FileGUID), file.FileGUID));
                        }
                    }
                    catch (Exception ex) { LogAttachmentCleanup(ex); }
                }
            }
        }

        private static void LogAttachmentCleanup(Exception exception)
        {
            try
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(
                    "Education.SaveAttachments file cleanup", exception);
            }
            catch (Exception) { }
        }

        private static byte[] GetUploadDigest(Stream source, Guid uploadGuid, string fileName)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] prefix = Encoding.UTF8.GetBytes(uploadGuid.ToString("D") + "\n" + fileName.ToUpperInvariant() + "\n");
                hash.TransformBlock(prefix, 0, prefix.Length, prefix, 0);
                byte[] buffer = new byte[81920];
                int length;
                while ((length = source.Read(buffer, 0, buffer.Length)) > 0)
                    hash.TransformBlock(buffer, 0, length, buffer, 0);
                hash.TransformFinalBlock(new byte[0], 0, 0);
                return hash.Hash;
            }
        }
    }
}
