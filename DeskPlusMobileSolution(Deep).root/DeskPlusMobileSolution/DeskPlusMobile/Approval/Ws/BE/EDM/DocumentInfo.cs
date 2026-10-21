using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE.EDM
{
    public class DocumentInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int FolderID { get; set; }
        public int VersionID { get; set; }
        public int ClassID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Version { get; set; }
        public string ACLID { get; set; }
        public string LifeTimeCode { get; set; }
        public DateTime LifeTimeStart { get; set; }
        public DateTime LifeTimeEnd { get; set; }
        public decimal ReadCount { get; set; }
        public int TotalPoint { get; set; }
        public string Recycle { get; set; }
        public int RecycleUserID { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int CheckOutUserID { get; set; }
        public string CheckOutUserInfo { get; set; }
        public string CheckOutName { get; set; }
        public string CheckOutDescription { get; set; }
        public string CheckOutStatus { get; set; }
        public string CheckOutPath { get; set; }
        public DateTime CheckInPlanDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public int CheckInUserID { get; set; }
        public string CheckInUserInfo { get; set; }
        public string CheckInDescription { get; set; }
        public string BaseDocPath { get; set; }
        public string HistoryEvent { get; set; }
        public DateTime WriteDate { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserInfo { get; set; }
        public string WriteName { get; set; }
        public DateTime ModifyDate { get; set; }
        public int ModifyUserID { get; set; }
        public string ModifyUserInfo { get; set; }
        public int OwnerID { get; set; }
        public string Etc1 { get; set; }
        public string Etc2 { get; set; }
        public string Etc3 { get; set; }
        public string Etc4 { get; set; }
        public string Etc5 { get; set; }
        public string Etc6 { get; set; }
        public string Etc7 { get; set; }
    }
}