using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class AdminInfo : Common.BE.DeptCodeInfo
    {
        public AdminInfo()
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
        /// 관리자 자동증가ID
        /// </summary>
        public int AdminID { get; set; }
        /// <summary>
        /// 관리자ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 부서,직급,직책고유키코드(히스토리용)
        /// </summary>
        public int DeptCodeID { get; set; }
        #endregion

        #region 그외 기타
        #endregion
    }
}
