using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class ScheduleInfo 
    {
        public ScheduleInfo(
            int _companyID, int _scheduleID, int _toDoID,
            string _Type, string _Title, 
            string _content,
            DateTime _startTime, DateTime _endTime,
            string _repeat,
            string _alert, string _alertType, int _alertTime,
            string _alertTargetAddress, string _isOpen,
            int _userID, int _deptID,
            DateTime _writeDate,
            string  _openType, 
            int _reservationID, 
            string _lunar,
            string _UserName
            )
        {
            this.CompanyID = _companyID;
            this.ScheduleID = _scheduleID;
            this.ToDoID = _toDoID;
            this.Type = _Type;
            this.Title = _Title;
            this.Content = _content;
            this.StartTime = _startTime;
            this.EndTime = _endTime;
            this.Repeat = _repeat;
            this.Alert = _alert;
            this.AlertType = _alertType;
            this.AlertTime = _alertTime;
            this.AlertTargetAddress = _alertTargetAddress;
            this.IsOpen = _isOpen;
            this.UserID = _userID;
            this.DeptID = _deptID;
            this.WriteDate = _writeDate;
            this.OpenType = _openType;
            this.ReservationID = _reservationID;
            this.Lunar = _lunar;
            this.UserName = _UserName; 
        }

        public ScheduleInfo()
        {
        }

        public int DisplayID { get; set; }

        public string strScheduleID { get; set; }
        public string UserIDList { get; set; }
        public string KindDesk { get; set; }
        public string UserName { get; set; }
        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }
        public string AlarmContent { get; set; }
        

        public int ToDoID { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Repeat { get; set; }

        public string Alert { get; set; }

        public string AlertType { get; set; }

        public int AlertTime { get; set; }

        public string AlertTargetAddress { get; set; }

        public string IsOpen { get; set; }

        public int UserID { get; set; }

        public int DeptID { get; set; }

        public DateTime WriteDate { get; set; }

        public string OpenType { get; set; }

        public int ReservationID { get; set; }

        public string Lunar { get; set; }

        public string WhereQuery { get; set; }

        public string stringStartTime { get; set; }
        public string stringENDTime { get; set; }

        /// <summary>
        /// 상태값
        /// </summary>
        public int StatusID { get; set; }

        /// <summary>
        /// 분류 색깔
        /// </summary>
        /// <value>
        /// 분류 색깔
        /// </value>
        public string StatusColors { get; set; }
        
        /// <summary>
        /// 테두리 칼라
        /// </summary>
        public string BorderColor { get; set; }

        /// <summary>
        /// 댓글 카운트
        /// </summary>
        public int ReplyCnt { get; set; }

        /// <summary>
        /// backgroundColor
        /// </summary>
        /// <value>
        /// backgroundColor
        /// </value>
        public string Color { get; set; }

        public string SearchKey { get; set; }
        public string SearchValue { get; set; }
        public int Pagesize { get; set; }
        public int PageSizePageNo { get; set; }
        public string Order { get; set; }
        public string Country { get; set; }
        public string HiddenSpoolPath { get; set; }

        public string Parameterstring  { get; set; }


        //머지시 필요한 컬럼
        public int Row { get; set; }
        public bool allDay { get; set; }

        //알람 메일
        public string SenderMail { get; set; }
        public string CCRecvMail { get; set; }
        public string RecvMail { get; set; }
        public string RecvName { get; set; }
    }
}
