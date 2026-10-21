using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class UnitInfo
    {
        public UnitInfo()
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
        /// 구분: 질문(0)/예시(1)
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 형태 : 단일선택(A)/다중선택(B)/단일가점(C)/다중가점(D)/주관식(E)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 필수여부 : 필수(1)/비필수(0)
        /// </summary>
        public string IsReq { get; set; }
        /// <summary>
        /// 최대점수
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// 활성화여부 : 활성화(1)/비활성화(0)
        /// </summary>
        public string IsActive { get; set; }
        /// <summary>
        /// Display여부(1:가로,0:세로)
        /// </summary>
        public string IsHorizon { get; set; }
        /// <summary>
        /// 등록일
        /// </summary>
        public DateTime WriteDate { get; set; }
        public int WriteUserID { get; set; }
        #endregion

        #region 그외 기타
        /// <summary>
        /// 입력일(String)
        /// </summary>
        public string WriteDateStr { get; set; }
        /// <summary>
        /// 단위항목수
        /// </summary>
        public string ItemCnt { get; set; }
        /// <summary>
        /// 사용설문수
        /// </summary>
        public string QuesUnitCnt { get; set; }

        /// <summary>
        /// 구분(string)
        /// </summary>
        public string SectionStr { get; set; }
        /// <summary>
        /// 형태(string)
        /// </summary>
        public string TypeStr { get; set; }
        /// <summary>
        /// 필수여부(string)
        /// </summary>
        public string IsReqStr { get; set; }
        /// <summary>
        /// 활성화유무(string)
        /// </summary>
        public string IsActiveStr { get; set; }
        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }
        /// <summary>
        /// 파일 삭제 다국어
        /// </summary>
        public string FileDel { get; set; }
        #endregion

    }
}
