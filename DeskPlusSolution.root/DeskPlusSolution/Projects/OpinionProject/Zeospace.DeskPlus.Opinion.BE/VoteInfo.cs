using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class VoteInfo : Common.BE.DeptCodeInfo
    {
        public VoteInfo()
        {
        }

        #region 필드
        /// <summary>
        /// 회사내부ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 투표ID
        /// </summary>
        public int VoteID { get; set; }
        /// <summary>
        /// 설문ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 투표자ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 질문ID
        /// </summary>
        public int UnitID { get; set; }
        /// <summary>
        /// 질문항목ID
        /// </summary>
        public int UnitItemID { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string UnitItemCont { get; set; }
        /// <summary>
        /// 투표일
        /// </summary>
        public DateTime WriteDate { get; set; }
        /// <summary>
        /// 부서,직급,직책고유키코드(히스토리용)
        /// </summary>
        public int DeptCodeID { get; set; }
        /// <summary>
        /// 점수(가점형점수)
        /// </summary>
        public int Point { get; set; }
        #endregion

        #region 그외 기타
        public int RowNum { get; set; }
        public int VoteTotCnt { get; set; }
        public string WriteDateStr { get; set; }
        #endregion
    }
}
