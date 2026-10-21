using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE.SmartPlusEDM
{
    public class VersionInfo
    {
        #region 생성자
        public VersionInfo() { }

        public VersionInfo(int _companyID, int _documentID, int _versionID, string _version, string _versionType, string _versionDescription, string _name,
            string _description, string _keyword, string _lifeTimeCode, DateTime _lifeTimeStart, DateTime _lifeTimeEnd, string _historyEvent, DateTime _checkOutDate, int _checkOutUserID,
            string _checkOutUserInfo, string _checkOutName, string _checkOutDescription, string _checkOutStatus, string _checkOutPath, DateTime _checkInPlanDate, DateTime _checkInDate, int _checkInUserID, string _checkInUserInfo, string _checkInDescription,
            DateTime _writeDate, int _writeUserID, string _writeUserInfo)
        {
            this.CompanyID = _companyID;
            this.DocumentID = _documentID;
            this.VersionID = _versionID;
            this.Version = _version;
            this.VersionType = _versionType;
            this.VersionDescription = _versionDescription;
            this.Name = _name;
            this.Description = _description;
            this.Keyword = _keyword;
            this.LifeTimeCode = _lifeTimeCode;
            this.LifeTimeStart = _lifeTimeStart;
            this.LifeTimeEnd = _lifeTimeEnd;
            this.HistoryEvent = _historyEvent;
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
            this.CheckInDescription = _checkInDescription;
            this.WriteDate = _writeDate;
            this.WriteUserID = _writeUserID;
            this.WriteUserInfo = _writeUserInfo;
        }
        #endregion

        #region 필드/프로퍼티 (초기화)
        /// < summary>
        /// 회사ID
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
        ///
        /// </summary>
        private string version = String.Empty;
        public string Version { get { return version; } set { version = value; } }

        /// < summary>
        /// 버전구분
        /// </summary>
        private string versionType = String.Empty;
        public string VersionType { get { return versionType; } set { versionType = value; } }

        /// < summary>
        /// 버전설명
        /// </summary>
        private string versionDescription = String.Empty;
        public string VersionDescription { get { return versionDescription; } set { versionDescription = value; } }

        /// < summary>
        /// 문서버전
        /// </summary>
        private string name = String.Empty;
        public string Name { get { return name; } set { name = value; } }

        /// < summary>
        /// 문서설명
        /// </summary>
        private string description = String.Empty;
        public string Description { get { return description; } set { description = value; } }

        /// < summary>
        /// 키워드
        /// </summary>
        private string keyword = String.Empty;
        public string Keyword { get { return keyword; } set { keyword = value; } }

        /// < summary>
        /// 문서 보존 기간코드
        /// </summary>
        private string filePath = String.Empty;
        public string LifeTimeCode { get { return filePath; } set { filePath = value; } }

        /// < summary>
        /// 문서 보존 시작일
        /// </summary>
        private DateTime lifeTimeStart = DateTime.Now;
        public DateTime LifeTimeStart { get { return lifeTimeStart; } set { lifeTimeStart = value; } }

        /// < summary>
        /// 문서 보존 만기일
        /// </summary>
        private DateTime lifeTimeEnd = DateTime.Now;
        public DateTime LifeTimeEnd { get { return lifeTimeEnd; } set { lifeTimeEnd = value; } }

        /// < summary>
        /// 문서 이력 이벤트
        /// </summary>
        private string historyEvent = String.Empty;
        public string HistoryEvent { get { return historyEvent; } set { historyEvent = value; } }

        /// < summary>
        /// 체크인일시
        /// </summary>
        private DateTime checkOutDate = DateTime.Now;
        public DateTime CheckOutDate { get { return checkOutDate; } set { checkOutDate = value; } }

        /// < summary>
        /// 체크아웃사용자ID
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

        /// < summary>
        /// 체크아웃 사유
        /// </summary>
        private string checkOutDescription = String.Empty;
        public string CheckOutDescription { get { return checkOutDescription; } set { checkOutDescription = value; } }

        /// < summary>
        /// 체크아웃사유
        /// </summary>
        private string checkOutStatus = String.Empty;
        public string CheckOutStatus { get { return checkOutStatus; } set { checkOutStatus = value; } }

        /// < summary>
        ///
        /// </summary>
        private string checkOutPath = String.Empty;
        public string CheckOutPath { get { return checkOutPath; } set { checkOutPath = value; } }

        /// < summary>
        ///
        /// </summary>
        private DateTime checkInPlanDate = DateTime.Now;
        public DateTime CheckInPlanDate { get { return checkInPlanDate; } set { checkInPlanDate = value; } }

        /// < summary>
        /// 체크인일시
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

        /// < summary>
        /// 체크인 사유
        /// </summary>
        private string checkInDescription = String.Empty;
        public string CheckInDescription { get { return checkInDescription; } set { checkInDescription = value; } }

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


        #endregion 프로퍼티
    }
}
