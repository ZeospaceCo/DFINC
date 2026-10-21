using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Zeospace.DeskPlus.Coop.BE
{
    /// <summary>
    /// ListOptionInfo
    /// </summary>
    [Serializable]
    public class ListOptionInfo
    {
        public ListOptionInfo() { }

        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int ItemID { get; set; }
        public int ItemSubID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        /// <summary>
        /// 난이도(상:1;중:2;하:3)
        /// </summary>
        public string Difficultys { get; set; }

        /// <summary>
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string Status { get; set; }
        public string RequestDate { get; set; }
        public string CompleteRequestDate { get; set; }
        public string CompleteDate { get; set; }
        /// <summary>
        /// 초과일수
        /// </summary>
        public string ExcessDay { get; set; }

        /// <summary>
        /// 공개 : 1; 비공개 : 2
        /// </summary>
        public string IsClose { get; set; }
        public int DeptCodeID { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string ManyName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }

    }

    /// <summary>
    /// 상태값
    /// 1:대기;2:접수;3:처리중;4:완료;5:반려
    /// </summary>
    public enum EnumStatus
    {        
        [Description("1")]
        STANDBY = 1,
        
        [Description("2")]
        ACCEPT = 2,

        [Description("3")]
        PROCESS = 3,

        [Description("4")]
        COMPLETE = 4,

        [Description("5")]
        REJECT = 5
    }
}
