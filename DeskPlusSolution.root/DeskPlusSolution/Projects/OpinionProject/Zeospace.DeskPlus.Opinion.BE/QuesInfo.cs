using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class QuesInfo : Common.BE.DeptCodeInfo
    {
        public QuesInfo()
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
        /// 제목
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 상세내용
        /// </summary>
        public string Cont { get; set; }
        /// <summary>
        /// 시작일
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 종료일
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 말머리명
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 메인지정유무	: 메인(1)/비메인(0)
        /// </summary>
        public string IsMain { get; set; }
        /// <summary>
        /// 답글사용유무 : 답글사용(1)/답글미사용(0)
        /// </summary>
        public string IsReply { get; set; }
        /// <summary>
        /// 결과공개유무	: 결과공개(1)/결과비공개(0)
        /// </summary>
        public string IsOpen { get; set; }
        /// <summary>
        /// 응답자공개유무 : 응답자공개(1)/응답자비공개(0)
        /// </summary>
        public string IsVoteUser { get; set; }
        /// <summary>
        /// 무기명사용유무 : 사용(1)/미사용(0)
        /// </summary>
        public string IsSecret { get; set; }
        /// <summary>
        /// 활성화여부 : 활성화(1)/비활성화(0)
        /// </summary>
        public string IsActive { get; set; }
        /// <summary>
        /// 주관부서ID : 주관부서ID
        /// </summary>
        public int SuperDeptID { get; set; }
        /// <summary>
        /// 등록일	    
        /// </summary>
        public DateTime WriteDate { get; set; }
        /// <summary>
        /// 등록자ID	    
        /// </summary>
        public int WriteUserID { get; set; }
        /// <summary>
        /// 정보기준 : 부서,직급,직책고유키코드(히스토리용)
        /// </summary>
        public int DeptCodeID { get; set; }
        #endregion

        #region 그외 기타
        /// <summary>
        /// 투표자수
        /// </summary>
        public int VoteCnt { get; set; }
        /// <summary>
        /// 질문수
        /// </summary>
        public int UnitCnt { get; set; }
        /// <summary>
        /// 중요표시수
        /// </summary>
        public int ImpoCnt { get; set; }

        /// <summary>
        /// 주관부서명
        /// </summary>
        public string SuperDeptNmBase { get; set; }
        public string SuperDeptNmKo { get; set; }
        public string SuperDeptNmEn { get; set; }
        public string SuperDeptNmJa { get; set; }
        public string SuperDeptNmFr { get; set; }
        public string SuperDeptNmZhcn { get; set; }
        public string SuperDeptNmZhtw { get; set; }

        /// <summary>
        /// 시작일(String)
        /// </summary>
        public string StartDateStr { get; set; }
        /// <summary>
        /// 종료일(String)
        /// </summary>
        public string EndDateStr { get; set; }
        /// <summary>
        /// 메인지정유무(String)
        /// </summary>
        public string IsMainStr { get; set; }
        /// <summary>
        /// 답글사용유무(String)
        /// </summary>
        public string IsReplyStr { get; set; }
        /// <summary>
        /// 결과공개유무(String)
        /// </summary>
        public string IsOpenStr { get; set; }
        /// <summary>
        /// 응답자공개유무(String)
        /// </summary>
        public string IsVoteUserStr { get; set; }
        /// <summary>
        /// 익명사용유무(String)
        /// </summary>
        public string IsSecretStr { get; set; }
        /// <summary>
        /// 활성화유무(String)
        /// </summary>
        public string IsActiveStr { get; set; }
        /// <summary>
        /// 첨부파일경로
        /// </summary>
        public string HiddenSpoolPath { get; set; }
        #endregion
    }
}
