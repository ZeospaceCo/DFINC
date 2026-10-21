using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ForwardListInfo : Common.BE.DeptCodeInfo
    {
        public ForwardListInfo() { }

        public int CompanyID { get; set; }
        public int ForwardListID { get; set; }
        public int ForwardGroupID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public int Depth { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string MobileContent { get; set; }
        public string Status { get; set; }
        public string IsAlarm { get; set; }
        public int DeptCodeID { get; set; }
        public string ManyName { get; set; }
        public int WriteUserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string WriteDateYMD { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
        public int SendUserID { get; set; }
        public int SendDeptCodeID { get; set; }
        public string SendNameBase { get; set; }
        public string SendNameKo { get; set; }
        public string SendNameEn { get; set; }
        public string SendNameJa { get; set; }
        public string SendNameFr { get; set; }
        public string SendNameZhcn { get; set; }
        public string SendNameZhtw { get; set; }
        public string SendManyName { get; set; }
        public DateTime SendWriteDate { get; set; }
        public int ForwardUserID { get; set; }

        /// <summary>
        /// 요청일자
        /// </summary>
        public string RequestDate { get; set; }

        /// <summary>
        /// 시작예정일자
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 종료예정일자
        /// </summary>
        public string EndDate { get; set; }
    }
}
