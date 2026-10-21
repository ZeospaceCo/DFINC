using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BE.EDM
{
    public class VersionInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int VersionID { get; set; }
        public string Version { get; set; }
        public string VersionType { get; set; }
        public string VersionDescription { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string LifeTimeCode { get; set; }
        public DateTime LifeTimeStart { get; set; }
        public DateTime LifeTimeEnd { get; set; }
        public string HistoryEvent { get; set; }
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
        public DateTime WriteDate { get; set; }
        public int WriteUserID { get; set; }
        public string WriteUserInfo { get; set; }

    }
}