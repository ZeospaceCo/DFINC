using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Education.BE;

namespace Zeospace.DeskPlus.Education.BLL
{
    public class TOAttach : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        public bool Add(int companyID, int listID, int userID, string spoolPath)
        {
            var dal = new DAL.TOAttach();

            string spoolFolder = FileHelper.GetServerMapPath(spoolPath);
            int fileCount = Directory.Exists(spoolFolder) ? Directory.GetFiles(spoolFolder).Length : 0;
            if (fileCount == 0) throw new ArgumentException("수료증 파일을 첨부해 주세요.");

            string yearMonth = dal.GetYearMonth(companyID, listID);
            if (String.IsNullOrEmpty(yearMonth)) return false;
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrEmpty(basePath))
                throw new InvalidOperationException("첨부파일 저장 경로가 설정되지 않았습니다.");
            string targetPath = basePath.TrimEnd('/') + "/" + companyID + "/Education/" + yearMonth + "/" + listID + "/" + userID;
            ArrayList fileList = null;
            bool completed = false;
            try
            {
                fileList = FileHelper.GetUploadFile(spoolPath, targetPath, FolderType.None, listID, false, yearMonth);
                if (fileList == null || fileList.Count != fileCount) return false;

                var certificates = new List<TOAttachInfo>();
                foreach (UploadFileInfo file in fileList)
                {
                    if (file.fileLength <= 0) throw new ArgumentException("수료증 파일이 비어 있습니다.");
                    certificates.Add(new TOAttachInfo
                    {
                        CompanyID = companyID,
                        ListID = listID,
                        UserID = userID,
                        FilePath = file.filePath,
                        FileName = file.fileName,
                        FileType = file.fileType,
                        FileLength = file.fileLength,
                        FileGUID = file.fileGUID
                    });
                }
                completed = dal.Add(certificates);
                return completed;
            }
            finally
            {
                // 실패한 이번 등록 요청의 복사본만 정리하고 임시 업로드 파일은 유지합니다.
                if (!completed && fileList != null)
                {
                    foreach (UploadFileInfo file in fileList)
                        FileHelper.DelFile(file.filePath + file.fileGUID, false);
                }
            }
        }
    }
}
