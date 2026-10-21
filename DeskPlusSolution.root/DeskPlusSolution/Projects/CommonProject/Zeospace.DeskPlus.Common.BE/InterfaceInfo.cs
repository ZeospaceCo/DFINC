using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class InterfaceInfo
    {
        public InterfaceInfo()
        {
        }

        /// <summary>
        /// 회사아이디
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 로그아이디
        /// </summary>
        public int InterfaceID { get; set; }

        /// <summary>
        /// 발송모듈
        /// </summary>
        /// <value></value>
        public string ModuleSend { get; set; }

        /// <summary>
        /// 수신모듈
        /// </summary>
        /// <value></value>
        public string SendTarget { get; set; }

        /// <summary>
        /// 발송아이디
        /// </summary>
        public int SendID { get; set; }

        /// <summary>
        /// 수신아이디
        /// </summary>
        public int TargetID { get; set; }

        /// <summary>
        /// 작성자아이디
        /// </summary>
        public int WriteUserID { get; set; }

        /// <summary>
        /// 작성자명
        /// </summary>
        /// <value></value>
        public string WriteUserName { get; set; }

        /// <summary>
        /// 상태
        /// </summary>
        /// <value></value>
        public string StatusCode { get; set; }

        /// <summary>
        /// 상태메세지
        /// </summary>
        /// <value></value>
        public string StatusMessage { get; set; }        
    }
}
