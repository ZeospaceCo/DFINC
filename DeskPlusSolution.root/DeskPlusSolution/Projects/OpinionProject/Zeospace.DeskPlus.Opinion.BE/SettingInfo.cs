using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class SettingInfo
    {
        public SettingInfo()
        {
        }

        #region 필드
        /// <summary>
        /// 회사내부ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 사용자ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 카테고리(트리)정보
        /// </summary>
        public string MenuInfo { get; set; }
        /// <summary>
        /// 조회형태
        /// </summary>
        public string ListType { get; set; }
        /// <summary>
        /// 조회수
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        public DateTime WriteDate { get; set; }
        #endregion

        #region 그외 기타
        #endregion

    }
}
