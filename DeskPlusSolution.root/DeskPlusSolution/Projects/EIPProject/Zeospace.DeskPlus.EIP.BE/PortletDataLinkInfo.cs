using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class PortletDataLinkInfo
    {
        public PortletDataLinkInfo() { }

        public int CompanyID { get; set; }
        public int PortletDataLinkID { get; set; }
        public int PortletID { get; set; }
        public string EncriptType { get; set; }
        public string EncriptValue { get; set; }
        /// <summary>
        /// 파라미터 연결 대상( U : UI / D : Data / L : Link)
        /// </summary>
        public string TargetType { get; set; }
        /// <summary>
        /// 연결대상이 Data일때 (E : 없음 / R : RSS / S : SQL)
        /// </summary>
        public string TargetDataType { get; set; }
        public string LinkName { get; set; }
        public string LinkValue { get; set; }
        /// <summary>
        /// OU : 사용자입력값; OL : LDAP 값
        /// </summary>
        public string LinkType { get; set; }
        public string Description { get; set; }


    }
}
