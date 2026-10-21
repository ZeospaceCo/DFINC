using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE.SmartPlusEDM
{
    public class FileInfo
    {
        #region 생성자
        public FileInfo() { }

        public FileInfo(int _companyID, int _documentID, int _versionID, int _fileID, string _filePath, string _fileName, string _fileType, long _fileLength,
            string _fileGUID, string _checkOutStatus)
        {
            this.CompanyID = _companyID;
            this.DocumentID = _documentID;
            this.VersionID = _versionID;
            this.FileID = _fileID;
            this.FilePath = _filePath;
            this.FileName = _fileName;
            this.FileType = _fileType;
            this.FileLength = _fileLength;
            this.FileGUID = _fileGUID;
            this.CheckOutStatus = _checkOutStatus;
        }
        #endregion

        #region 필드/프로퍼티 (초기화)
        /// < summary>
        /// 회사 ID
        /// </summary>
        private int companyID = 0;
        public int CompanyID { get { return companyID; } set { companyID = value; } }

        /// < summary>
        /// 문서ID
        /// </summary>
        private int documentID = 0;
        public int DocumentID { get { return documentID; } set { documentID = value; } }

        /// < summary>
        /// 버전ID
        /// </summary>
        private int versionID = 0;
        public int VersionID { get { return versionID; } set { versionID = value; } }

        /// < summary>
        /// 파일ID
        /// </summary>
        private int fileID = 0;
        public int FileID { get { return fileID; } set { fileID = value; } }

        /// < summary>
        /// 파일경로
        /// </summary>
        private string filePath = String.Empty;
        public string FilePath { get { return filePath; } set { filePath = value; } }

        /// < summary>
        /// 파일명
        /// </summary>
        private string fileName = String.Empty;
        public string FileName { get { return fileName; } set { fileName = value; } }

        /// < summary>
        /// 파일형식
        /// </summary>
        private string fileType = String.Empty;
        public string FileType { get { return fileType; } set { fileType = value; } }

        /// < summary>
        /// 파일크기
        /// </summary>
        private long fileLength = 0;
        public long FileLength { get { return fileLength; } set { fileLength = value; } }

        /// < summary>
        /// 파일고유명
        /// </summary>
        private string fileGUID = String.Empty;
        public string FileGUID { get { return fileGUID; } set { fileGUID = value; } }

        /// <summary>
        /// 체크아웃 여부
        /// </summary>
        private string checkOutStatus = String.Empty;
        public string CheckOutStatus { get { return checkOutStatus; } set { checkOutStatus = value; } }
        #endregion 프로퍼티
    }
}
