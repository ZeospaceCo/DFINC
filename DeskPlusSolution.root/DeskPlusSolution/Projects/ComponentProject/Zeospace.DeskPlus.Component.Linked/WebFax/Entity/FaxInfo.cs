using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.Component.Linked.WebFax.Entity
{
    [Serializable]
    public class FaxInfo
    {
        public FaxInfo() { }

        /// <summary>
        /// 내부회사아이디
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 키 : guid 이며 하이픈(-) 제거
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 수신자 수 1~9999
        /// </summary>
        public int RecvCount { get; set; }

        /// <summary>
        /// 파일개수 1~5
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        /// 첨부파일명 리스트 (확장자 포함 36자) - 특수문자 불가
        /// </summary>
        public List<string> FileNameList { get; set; }

        /// <summary>
        /// 예약시간 200711161450 (12자리)
        /// </summary>
        public string ResTime { get; set; }

        /// <summary>
        /// LDCC 가 지정한 회사번호 (4자리)
        /// </summary>
        public string ClientNo { get; set; }

        /// <summary>
        /// 송신자 부서명 
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 송신자 이름 (30자)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 송신자 이메일 (50자)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 송신자 로긴 아이디
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }
    }
}
