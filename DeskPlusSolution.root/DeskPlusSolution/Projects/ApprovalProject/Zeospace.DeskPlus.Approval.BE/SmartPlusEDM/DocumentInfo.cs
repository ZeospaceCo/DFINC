using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE.SmartPlusEDM
{
    public class DocumentInfo
    {
        #region 생성자
        public DocumentInfo() { }

        public DocumentInfo(int _companyID, int _documentID, int _folderID, int _versionID, int _classID, string _name, string _description, string _keyword, string _version,
            string _aclID, string _lifeTimeCode, DateTime _lifeTimeStart, DateTime _lifeTimeEnd, decimal _readCount, int _totalPoint, string _recycle, int _recycleUserID,
            DateTime _checkOutDate, int _checkOutUserID, string _checkOutUserInfo, string _checkOutName, string _checkOutDescription, string _checkOutStatus, string _checkOutPath,
            DateTime _checkInPlanDate, DateTime _checkInDate, int _checkInUserID, string _checkInUserInfo, string _checkInDescriptioin, string _baseDocPath, string _historyEvent, DateTime _writeDate,
            int _writeUserID, string _writeUserInfo, string _writeName, DateTime _modifyDate, int _modifyUserID, string _modifyUserInfo, int _ownerID,
            string _etc1, string _etc2, string _etc3, string _etc4, string _etc5, string _etc6, string _etc7)
        {
            this.CompanyID = _companyID;
            this.DocumentID = _documentID;
            this.FolderID = _folderID;
            this.VersionID = _versionID;
            this.ClassID = _classID;
            this.Name = _name;
            this.Description = _description;
            this.Keyword = _keyword;
            this.Version = _version;
            this.ACLID = _aclID;
            this.LifeTimeCode = _lifeTimeCode;
            this.LifeTimeStart = _lifeTimeStart;
            this.LifeTimeEnd = _lifeTimeEnd;
            this.ReadCount = _readCount;
            this.TotalPoint = _totalPoint;
            this.Recycle = _recycle;
            this.RecycleUserID = _recycleUserID;
            this.CheckOutDate = _checkOutDate;
            this.CheckOutUserID = _checkOutUserID;
            this.CheckOutUserInfo = _checkOutUserInfo;
            this.CheckOutName = _checkOutName;
            this.CheckOutDescription = _checkOutDescription;
            this.CheckOutStatus = _checkOutStatus;
            this.CheckOutPath = _checkOutPath;
            this.CheckInPlanDate = _checkInPlanDate;
            this.CheckInDate = _checkInDate;
            this.CheckInUserID = _checkInUserID;
            this.CheckInUserInfo = _checkInUserInfo;
            this.CheckInDescription = _checkInDescriptioin;
            this.BaseDocPath = _baseDocPath;
            this.HistoryEvent = _historyEvent;
            this.WriteDate = _writeDate;
            this.WriteUserID = _writeUserID;
            this.WriteUserInfo = _writeUserInfo;
            this.WriteName = _writeName;
            this.ModifyDate = _modifyDate;
            this.ModifyUserID = _modifyUserID;
            this.ModifyUserInfo = _modifyUserInfo;
            this.OwnerID = _ownerID;
            this.Etc1 = _etc1;
            this.Etc2 = _etc2;
            this.Etc3 = _etc3;
            this.Etc4 = _etc4;
            this.Etc5 = _etc5;
            this.Etc6 = _etc6;
            this.Etc7 = _etc7;
        }
        #endregion

        #region 필드/프로퍼티 (초기화)
        /// <summary>
        /// 회사 ID
        /// </summary>
        private int companyID = 0;
        public int CompanyID { get { return companyID; } set { companyID = value; } }

        /// <summary>
        /// 문서 ID
        /// </summary>
        private int documentID = 0;
        public int DocumentID { get { return documentID; } set { documentID = value; } }

        /// <summary>
        /// 폴더 ID
        /// </summary>
        private int folderID = 0;
        public int FolderID { get { return folderID; } set { folderID = value; } }

        /// <summary>
        /// 버전 ID
        /// </summary>
        private int versionID = 0;
        public int VersionID { get { return versionID; } set { versionID = value; } }

        /// <summary>
        /// Class ID
        /// </summary>
        private int classID = 0;
        public int ClassID { get { return classID; } set { classID = value; } }

        /// <summary>
        /// 문서제목
        /// </summary>
        private string name = String.Empty;
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// 문서설명
        /// </summary>
        private string description = String.Empty;
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// 키워드
        /// </summary>
        private string keyword = String.Empty;
        public string Keyword { get { return keyword; } set { keyword = value; } }

        /// <summary>
        /// 버전 구분(배포버전/임시버전)
        /// </summary>
        private string version = String.Empty;
        public string Version { get { return version; } set { version = value; } }

        /// <summary>
        /// 문서형식
        /// </summary>
        private string aCLID = String.Empty;
        public string ACLID { get { return aCLID; } set { aCLID = value; } }

        /// <summary>
        /// 문서 보존 기간코드
        /// </summary>
        private string lifeTimeCode = String.Empty;
        public string LifeTimeCode { get { return lifeTimeCode; } set { lifeTimeCode = value; } }

        /// <summary>
        /// 문서 보존 시작일
        /// </summary>
        private DateTime lifeTimeStart = DateTime.Now;
        public DateTime LifeTimeStart { get { return lifeTimeStart; } set { lifeTimeStart = value; } }

        /// <summary>
        /// 문서 보존 만기일
        /// </summary>
        private DateTime lifeTimeEnd = DateTime.Now;
        public DateTime LifeTimeEnd { get { return lifeTimeEnd; } set { lifeTimeEnd = value; } }

        /// <summary>
        /// 조회수
        /// </summary>
        private decimal readCount = 0;
        public decimal ReadCount { get { return readCount; } set { readCount = value; } }

        /// <summary>
        /// 문서 총 점수
        /// </summary>
        private int totalPoint = 0;
        public int TotalPoint { get { return totalPoint; } set { totalPoint = value; } }

        /// <summary>
        /// 휴지통 ( 삭제시 'Y' , 기본 'N')
        /// </summary>
        private string recycle = String.Empty;
        public string Recycle { get { return recycle; } set { recycle = value; } }

        /// <summary>
        /// 삭제한 사용자 ID
        /// </summary>
        private int recycleUserID = 0;
        public int RecycleUserID { get { return recycleUserID; } set { recycleUserID = value; } }

        /// <summary>
        /// 체크아웃 일시
        /// </summary>
        private DateTime checkOutDate = DateTime.Now;
        public DateTime CheckOutDate { get { return checkOutDate; } set { checkOutDate = value; } }

        /// <summary>
        /// 체크아웃 사용자 ID
        /// </summary>
        private int checkOutUserID = 0;
        public int CheckOutUserID { get { return checkOutUserID; } set { checkOutUserID = value; } }

        /// <summary>
        /// 체크아웃 사용자 정보 
        /// </summary>
        private string checkOutUserInfo = String.Empty;
        public string CheckOutUserInfo { get { return checkOutUserInfo; } set { checkOutUserInfo = value; } }

        /// <summary>
        /// 체크아웃 사용자 정보 
        /// </summary>
        private string checkOutName = String.Empty;
        public string CheckOutName { get { return checkOutName; } set { checkOutName = value; } }

        /// <summary>
        /// 체크아웃 사유
        /// </summary>
        private string checkOutDescription = String.Empty;
        public string CheckOutDescription { get { return checkOutDescription; } set { checkOutDescription = value; } }

        /// <summary>
        /// 체크아웃 상태(Y/N)
        /// </summary>
        private string checkOutStatus = String.Empty;
        public string CheckOutStatus { get { return checkOutStatus; } set { checkOutStatus = value; } }

        /// <summary>
        /// 체크아웃 경로
        /// </summary>
        private string checkOutPath = String.Empty;
        public string CheckOutPath { get { return checkOutPath; } set { checkOutPath = value; } }

        /// <summary>
        /// 체크인 예정일
        /// </summary>
        private DateTime checkInPlanDate = DateTime.Now;
        public DateTime CheckInPlanDate { get { return checkInPlanDate; } set { checkInPlanDate = value; } }

        /// <summary>
        /// 체크인 일시
        /// </summary>
        private DateTime checkInDate = DateTime.Now;
        public DateTime CheckInDate { get { return checkInDate; } set { checkInDate = value; } }

        /// <summary>
        /// 체크인 사용자 ID
        /// </summary>
        private int checkInUserID = 0;
        public int CheckInUserID { get { return checkInUserID; } set { checkInUserID = value; } }

        /// <summary>
        /// 체크인 사용자 정보 
        /// </summary>
        private string checkInUserInfo = String.Empty;
        public string CheckInUserInfo { get { return checkInUserInfo; } set { checkInUserInfo = value; } }

        /// <summary>
        /// 체크인 사유
        /// </summary>
        private string checkInDescription = String.Empty;
        public string CheckInDescription { get { return checkInDescription; } set { checkInDescription = value; } }

        /// <summary>
        /// 서버 저장 위치
        /// </summary>
        private string baseDocPath = String.Empty;
        public string BaseDocPath { get { return baseDocPath; } set { baseDocPath = value; } }

        /// <summary>
        /// 문서 이력 조회 이벤트 리스트 
        /// 1|1|1|1|1|1|1|1|1|1|1|1|
        /// 속성변경|휴지통으로삭제|완전삭제|복원|이동|복사|문서보기|체크인|체크아웃|파일수정|버전수정|메일보내기|
        /// </summary>
        private string historyEvent = String.Empty;
        public string HistoryEvent { get { return historyEvent; } set { historyEvent = value; } }

        /// <summary>
        /// 등록일
        /// </summary>
        private DateTime writeDate = DateTime.Now;
        public DateTime WriteDate { get { return writeDate; } set { writeDate = value; } }

        /// <summary>
        /// 등록자 ID
        /// </summary>
        private int writeUserID = 0;
        public int WriteUserID { get { return writeUserID; } set { writeUserID = value; } }

        /// <summary>
        /// 등록자 정보
        /// </summary>
        private string writeUserInfo = String.Empty;
        public string WriteUserInfo { get { return writeUserInfo; } set { writeUserInfo = value; } }

        /// <summary>
        /// 등록자 이름
        /// </summary>
        private string writeName = String.Empty;
        public string WriteName { get { return writeName; } set { writeName = value; } }

        /// <summary>
        /// 변경 일자
        /// </summary>
        private DateTime modifyDate = DateTime.Now;
        public DateTime ModifyDate { get { return modifyDate; } set { modifyDate = value; } }

        /// <summary>
        /// 변경자 ID
        /// </summary>
        private int modifyUserID = 0;
        public int ModifyUserID { get { return modifyUserID; } set { modifyUserID = value; } }

        /// <summary>
        /// 변경자 정보
        /// </summary>
        private string modifyUserInfo = String.Empty;
        public string ModifyUserInfo { get { return modifyUserInfo; } set { modifyUserInfo = value; } }

        /// <summary>
        /// 소유자 ID
        /// </summary>
        private int ownerID = 0;
        public int OwnerID { get { return ownerID; } set { ownerID = value; } }

        /// <summary>
        /// ETC1
        /// </summary>
        private string etc1 = String.Empty;
        public string Etc1 { get { return etc1; } set { etc1 = value; } }

        /// <summary>
        /// ETC2
        /// </summary>
        private string etc2 = String.Empty;
        public string Etc2 { get { return etc2; } set { etc2 = value; } }

        /// <summary>
        /// ETC3
        /// </summary>
        private string etc3 = String.Empty;
        public string Etc3 { get { return etc3; } set { etc3 = value; } }

        /// <summary>
        /// ETC4
        /// </summary>
        private string etc4 = String.Empty;
        public string Etc4 { get { return etc4; } set { etc4 = value; } }

        /// <summary>
        /// ETC5
        /// </summary>
        private string etc5 = String.Empty;
        public string Etc5 { get { return etc5; } set { etc5 = value; } }

        /// <summary>
        /// ETC6
        /// </summary>
        private string etc6 = String.Empty;
        public string Etc6 { get { return etc6; } set { etc6 = value; } }

        /// <summary>
        /// ETC7
        /// </summary>
        private string etc7 = String.Empty;
        public string Etc7 { get { return etc7; } set { etc7 = value; } }

        #endregion 프로퍼티
    }
}
