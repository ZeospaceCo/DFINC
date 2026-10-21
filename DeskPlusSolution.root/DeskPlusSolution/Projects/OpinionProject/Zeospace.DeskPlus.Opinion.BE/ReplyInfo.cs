using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class ReplyInfo : Common.BE.DeptCodeInfo
    {
        public ReplyInfo()
        {
        }

        #region 필드

        /// <summary>
        /// 회사내부ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 설문ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 답글ID
        /// </summary>
        public int ReplyID { get; set; }
        /// <summary>
        /// 작성자ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string Cont { get; set; }
        /// <summary>
        /// 입력일
        /// </summary>
        public DateTime WriteDate { get; set; }
        /// <summary>
        /// 부서,직급,직책고유키코드(히스토리용)
        /// </summary>
        public int DeptCodeID { get; set; }

        #endregion

        #region 그외 기타

        /// <summary>
        /// 등록일(string)
        /// </summary>
        public string WriteDateStr { get; set; }
        /// <summary>
        /// 수정/삭제 권한
        /// </summary>
        public string IsEdit { get; set; }
        #endregion
    }
}
