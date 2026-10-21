using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class UnitItemInfo
    {
        public UnitItemInfo()
        {
        }

        #region 필드
        /// <summary>
        /// 회사내부ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 질문ID
        /// </summary>
        public int UnitID { get; set; }
        /// <summary>
        /// 질문항목ID
        /// </summary>
        public int UnitItemID { get; set; }
        /// <summary>
        /// 항목형태:주관식(1)/미지정(0)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 항목내용
        /// </summary>
        public string Cont { get; set; }
        /// <summary>
        /// 이미지경로
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 이미지명
        /// </summary>
        public string ImgName { get; set; }
        /// <summary>
        /// 순서
        /// </summary>
        public int Seq { get; set; }
        #endregion

        #region 그외 기타
        /// <summary>
        /// 항목형태(string)
        /// </summary>
        public string TypeStr { get; set; }
        #endregion
    }
}
