using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Component.Linked.WebFax.Entity
{
    [Serializable]
    public class FaxRecvInfo
    {
        /// <summary>
        /// 수신자 이름 (30자)
        /// </summary>
        public string RecvName { get; set; }

        /// <summary>
        /// 수신자 회사명 (30자)
        /// </summary>
        public string RecvComp { get; set; }

        /// <summary>
        /// 수신자 전화번호 (20자)
        /// </summary>
        public string RecvPhone { get; set; }

        /// <summary>
        /// 수신자 핸드폰 (20자)
        /// </summary>
        public string RecvMobile { get; set; }

        /// <summary>
        /// 수신자 팩스 (20자) - 숫자만
        /// </summary>
        public string RecvFax { get; set; }

        /// <summary>
        /// 수신자 고유키 (32자)
        /// </summary>
        public string Key2 { get; set; }
    }
}
