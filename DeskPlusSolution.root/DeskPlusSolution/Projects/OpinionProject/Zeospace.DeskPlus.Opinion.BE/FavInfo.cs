using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class FavInfo
    {
        public FavInfo()
        {
        }

        #region 필드
        /// <summary>
        /// 회사내부ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 중요표시ID
        /// </summary>
        public int FavID { get; set; }
        /// <summary>
        /// 관리자ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 설문ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        public DateTime WriteDate { get; set; }

        #endregion

        #region 그외 기타
        #endregion
    }
}
