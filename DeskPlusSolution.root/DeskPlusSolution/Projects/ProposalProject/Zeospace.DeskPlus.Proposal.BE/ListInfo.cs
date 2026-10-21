using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.BE
{
    [Serializable]
    public class ListInfo : PersonInfo
    {
        public ListInfo() { }

        
        public int ReceiptNo { get; set; }
        public string Title { get; set; }
        public string Gubun { get; set; }
        public string Status { get; set; }
        public string BeforeCont { get; set; }
        public string AfterCont { get; set; }
        public string BeforeEffect { get; set; }
        public string AfterEffect { get; set; }
        public string Grade1 { get; set; }
        public string Score1 { get; set; }
        public string Grade2 { get; set; }
        public string score2 { get; set; }
        public string LastGrade { get; set; }
        public string ChargerView { get; set; }
        public string Etc1 { get; set; }
        public string Etc2 { get; set; }
        public DateTime WriteDate { get; set; }
        public int Bul_Select { get; set; }
        public string YYYYMM { get; set; }

        /// <summary>
        /// 공동제안자UserID
        /// </summary>
        public string CoWorkUserIDList { get; set; }
        /// <summary>
        /// 공동제안자 이름
        /// </summary>
        public string CoWorkNameList { get; set; }
        /// <summary>
        /// 1차 심사자UserID
        /// </summary>
        public string JudgeUserIDList { get; set; }
        /// <summary>
        /// 1차 심사자이름
        /// </summary>
        public string JudgeNameList { get; set; }
        /// <summary>
        /// 첨부파일
        /// </summary>
        public string HiddenSpoolPath { get; set; }
    }
}
