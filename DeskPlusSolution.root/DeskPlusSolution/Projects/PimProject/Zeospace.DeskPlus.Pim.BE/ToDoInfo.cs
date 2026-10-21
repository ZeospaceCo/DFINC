using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class ToDoInfo
    {
        public ToDoInfo(
            int _companyID, int _toDoID, string _type, string _status, string _priority, string _title, string _content, DateTime _startDate,
            DateTime _endDate, string _scheduleYN, string _isOpen, int _userID, DateTime _writeDate, int _reservationID, int _OpenType
            )
        {
            this.CompanyID = _companyID;
            this.ToDoID = _toDoID;
            this.Type = _type;
            this.Status = _status;
            this.Priority = _priority;
            this.Title = _title;
            this.Content = _content;
            this.StartDate = _startDate;
            this.EndDate = _endDate;
            this.ScheduleYN = _scheduleYN;
            this.IsOpen = _isOpen;
            this.UserID = _userID;
            this.WriteDate = _writeDate;
            this.ReservationID = _reservationID;
            this.OpenType = _OpenType;
        }

        public ToDoInfo()
        {
        }



        public int CompanyID { get; set; }

        public int ToDoID { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Priority { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ScheduleYN { get; set; }

        public string IsOpen { get; set; }

        public int UserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ReservationID { get; set; }

        public int OpenType { get; set; }
        public int DeptID { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 댓글 카운트
        /// </summary>
        public int ReplyCnt { get; set; }
    }
}
