using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class QuesUnitInfo : UnitInfo
    {
        public QuesUnitInfo()
        {
        }

        #region 필드
        /// <summary>
        /// 회사내부ID
        /// </summary>
        //public int CompanyID { get; set; }
        /// <summary>
        /// 설문질문ID
        /// </summary>
        public int QuesUnitID { get; set; }
        /// <summary>
        /// 설문ID
        /// </summary>
        public int QuesID { get; set; }
        /// <summary>
        /// 질문ID
        /// </summary>
        //public int UnitID { get; set; }
        /// <summary>
        /// 질문순서
        /// </summary>
        public int Seq { get; set; }
        #endregion

        #region 그외 기타
        public int RowNum { get; set; }
        public int UnitItemID { get; set; }
        public string ItemType { get; set; }
        public string ItemCont { get; set; }
        public string ItemImgUrl { get; set; }
        public int VoteCnt { get; set; }
        public int VoteTotCnt { get; set; }
        #endregion
    }
}
