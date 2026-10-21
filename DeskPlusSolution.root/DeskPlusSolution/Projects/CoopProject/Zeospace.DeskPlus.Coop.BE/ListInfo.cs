using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ListInfo : Common.BE.DeptCodeInfo
    {
        public ListInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }

        /// <summary>
        /// 1차분류아이디
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 2차분류아이디
        /// </summary>
        public int ItemSubID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public int Depth { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string MobileContent { get; set; }

        /// <summary>
        /// 요청일자
        /// </summary>
        public string RequestDate { get; set; }

        /// <summary>
        /// 완료요청일
        /// </summary>
        public string CompleteRequestDate { get; set; }

        /// <summary>
        /// 시작예정일자
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 종료예정일자
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 완료일자
        /// </summary>
        public string CompleteDate { get; set; }

        /// <summary>
        /// 진행하면서 상태값
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 0:글용;1:알람용(알람은 칼라색이 다름)
        /// </summary>
        public string IsAlarm { get; set; }

        /// <summary>
        /// 최종 상태값
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string StatusLast { get; set; }

        /// <summary>
        /// 공개 : 1; 비공개 : 2
        /// </summary>
        public string IsClose { get; set; }

        /// <summary>
        /// 난이도(상:1;중:2;하:3)
        /// </summary>
        public string Difficultys { get; set; }

        public int DeptCodeID { get; set; }        
        public string ManyName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public string HiddenSpoolPath { get; set; }
        public string AlarmContent { get; set; }
        public string SMSContent { get; set; }
        public string LoginID { get; set; }
        /// <summary>
        /// 수신자
        /// </summary>
        public string ToUserList { get; set; }
        /// <summary>
        /// 참조자
        /// </summary>
        public string CCUserList { get; set; }

    }
}
