using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class ForwardUserInfo
    {
        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int ForwardDocumentID { get; set; }
        public int ReceiveUserID { get; set; }
        public DateTime ConfirmDate { get; set; }


    }
}
