using System;

namespace Zeospace.DeskPlus.Account.BE
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class UserEnvironmentInfo
    {
        public UserEnvironmentInfo()
        {
        }

        /// <summary>
        /// CompanyID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// PreferredLanguage
        /// </summary>
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// DesignStyle
        /// </summary>
        public string DesignStyle { get; set; }

        /// <summary>
        /// BaseMenu
        /// </summary>
        public string BaseMenu { get; set; }

        /// <summary>
        /// ApprovalStatusMenu
        /// </summary>
        public string ApprovalStatusMenu { get; set; }

        /// <summary>
        /// ApprovalPublicMenu
        /// </summary>
        public string ApprovalPublicMenu { get; set; }

        /// <summary>
        /// BaseFuncMenu
        /// </summary>
        public string BaseFuncMenu { get; set; }

        /// <summary>
        /// ListCnt
        /// </summary>
        public string ListCnt { get; set; }

        /// <summary>
        /// MobilePims
        /// </summary>
        public string MobilePims { get; set; }

        /// <summary>
        /// basetab
        /// </summary>
        public int BaseTab { get; set; }

        public string AlarmMail { get; set; }

        public string AlarmMessenger { get; set; }

        public string AlarmSMS { get; set; }

        public string ApprovalHomeMenu { get; set; }

        public string TaskHomeMenu { get; set; }

        public string RMHomeMenu { get; set; }

        // N or Y
        public string IsApprovePwd { get; set; }
        public string ApprovalStatusMenuType { get; set; }
        public string IsTodo { get; set; }
        public string TodoMenu { get; set; }
        public string ImageYN { get; set; }

        // 개인알람설정관련 (사용유무-옵션정보, ex) 1-Mail,Messenger,Mobile)
        public string AlarmApproval { get; set; }
        public string AlarmBoard { get; set; }
        public string AlarmPims { get; set; }
        public string AlarmTask { get; set; }
        public string AlarmWork { get; set; }
        public string AlarmCommunity { get; set; }
        public string AlarmRM { get; set; }
        public string AlarmSNS { get; set; }
    }
}
