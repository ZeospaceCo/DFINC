using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ListInfo
    {
        public int CompanyID { get; set; }      //회사ID
        public int CategoryID { get; set; }
        public string Title { get; set; }       //자원명
        public int ListID { get; set; }
        public int ReservationID { get; set; }
        public int ParentReservationID { get; set; }
        public string ReserveDate { get; set; } //예약기간

        public string Subject { get; set; } //제목
        public int ModifyUserID { get; set; }       //수정자ID
        public string ModifyUserName { get; set; }  //수정자명
        public string ModifyUserInfo { get; set; }  //수정자정보(부서 / 이름 / 직급)
        public DateTime ModifyDate { get; set; }    //수정일
        public string ModifyDateStr { get; set; }   //수정일str
        public string Usable { get; set; }          //승인여부
        public string UsableStr { get; set; }          //승인여부
        public string IsGuard { get; set; }         //구분        
        public int Auth { get; set; }               //권한
        public string ReplyCount { get; set; } //댓글카운트
    }
}