using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class SSOInfo
    {
        public SSOInfo(
            int _UserID, int _CompanyID, string _DisplayName,
            string _EmployeeNumber, string _DepartmentNumber, string _Mail,
            string _UserPassword, string _SN, string _GivenName, string _Initials,
            string _Title, string _TelephoneNumber, string _FacsimileTelephoneNumber,
            string _Mobile, string _RoomNumber, string _CarLicense,
            string _O, string _OU, string _EmployeeType, string _PreferredLanguage,
            string _LabeledURI, string _UserCode, string _Description
            )
        {
            this.UserID = _UserID;
            this.CompanyID = _CompanyID;
            this.DisplayName = _DisplayName;
            this.EmployeeNumber = _EmployeeNumber;
            this.DepartmentNumber = _DepartmentNumber;
            this.Mail = _Mail;
            this.UserPassword = _UserPassword;
            this.SN = _SN;
            this.GivenName = _GivenName;
            this.Initials = _Initials;
            this.Title = _Title;
            this.TelephoneNumber = _TelephoneNumber;
            this.FacsimileTelephoneNumber = _FacsimileTelephoneNumber;
            this.Mobile = _Mobile;
            this.RoomNumber = _RoomNumber;
            this.CarLicense = _CarLicense;
            this.O = _O;
            this.OU = _OU;
            this.EmployeeType = _EmployeeType;
            this.PreferredLanguage = _PreferredLanguage;
            this.LabeledURI = _LabeledURI;
            this.UserCode = _UserCode;
            this.Description = _Description;
        }

        /// <summary>
        /// DeskPlus 사용자 내부코드
        /// </summary>
        public int UserID {get; set;}
        
        /// <summary>
        /// DeskPlus 사용자 내부코드
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 이름 - 이현수
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 사번 - 5
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 부서코드 - 15
        /// </summary>
        public string DepartmentNumber { get; set; }

        /// <summary>
        /// 메일 - freeman@zeospace.com
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 패스워드 - XXX
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// 성 - 이   (Etc로 사용가)
        /// </summary>
        public string SN { get; set; }


        /// <summary>
        /// 이름 - 현수 (Etc로 사용가)
        /// </summary>
        public string GivenName { get; set; }


        /// <summary>
        /// 이름 약자 - LHS (Etc로 사용가)
        /// </summary>
        public string Initials { get; set; }


        /// <summary>
        /// 직책 - 팀장(과장)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 전화번호 - 0221083079
        /// </summary>
        public string TelephoneNumber { get; set; }


        /// <summary>
        /// 팩스번호 - 0221083080
        /// </summary>
        public string FacsimileTelephoneNumber { get; set; }


        /// <summary>
        /// 휴대폰 - 017-XXX-XXXX
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// ETC 사용
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// 주민등록번호 - 721025-1XXXXXX
        /// </summary>
        public string CarLicense { get; set; }


        /// <summary>
        /// 회사명 - 제오스페이스
        /// </summary>
        public string O { get; set; }

        /// <summary>
        /// 부서명 - 솔루션개발
        /// </summary>
        public string OU { get; set; }

        /// <summary>
        /// 사용자 유형 정보로 사용 - ETC
        /// </summary>
        public string EmployeeType { get; set; }


        /// <summary>
        /// 사용자 환경 - kr, en
        /// </summary>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// ETC 사용
        /// </summary>
        public string LabeledURI { get; set; }

        /// <summary>
        /// 설명 - ^^
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 설명 - ^^
        /// </summary>
        public string Description { get; set; }

    }
}
