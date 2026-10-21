using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class TargetInfo : Common.BE.DeptCodeInfo
    {
        public TargetInfo()
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
        /// 대상ID
        /// </summary>
        public int TargetID { get; set; }
        /// <summary>
        /// 대상자ID:UserID/DeptID
        /// </summary>
        public int OwnerID { get; set; }
        /// <summary>
        /// 형태:개인(M)/그룹(G)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 부서,직급,직책고유키코드(히스토리용)
        /// </summary>
        public int DeptCodeID { get; set; }

        #endregion

        #region 그외 기타
        /// <summary>
        /// 형태(string)
        /// </summary>
        public string TypeStr { get; set; }
        #endregion
    }
}
