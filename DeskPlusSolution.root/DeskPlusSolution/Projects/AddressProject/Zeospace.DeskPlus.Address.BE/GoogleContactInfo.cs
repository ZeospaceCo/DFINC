using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Address.BE
{
    [Serializable]
    public class GoogleContactInfo
    {
        /// <summary>
        /// 전체이름
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// 성
        /// </summary>
        public string FamilyName { get; set; }
        /// <summary>
        /// 회사이메일
        /// </summary>
        public string OfficeEmail { get; set; }
        /// <summary>
        /// 개인이메일
        /// </summary>
        public string HomeEmail { get; set; }
        /// <summary>
        /// 휴대폰
        /// </summary>
        public string MPhone { get; set; }
        /// <summary>
        /// 회사전화
        /// </summary>
        public string CPhone { get; set; }
        /// <summary>
        /// 집전화
        /// </summary>
        public string HPhone { get; set; }
        /// <summary>
        /// 집주소
        /// </summary>
        public string HomeAddress { get; set; }
        /// <summary>
        /// 회사주소
        /// </summary>
        public string OfficeAddress { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// 키값
        /// </summary>
        public string SelfUrl { get; set; }
        /// <summary>
        /// 국가
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 길가
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 시
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 지역
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 우편번호
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// true 사용하시길
        /// 참고 : The feedUri value is typically www.google.com/calendar/feeds/default/private/full 
        /// for working with your primary calendar. To use a non-primary calendar, find the address 
        /// in the Google Calendar UI by clicking the arrow next to the calendar, choosing calendar 
        /// settings and copy and paste the XML URL for the 'Calendar Address' seen on the settings tab.
        /// </summary>
        public bool Primary { get; set; }
        /// <summary>
        /// 갱신일자
        /// </summary>
        public DateTime UpDated { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string CompanyName { get; set; }
    }
}
