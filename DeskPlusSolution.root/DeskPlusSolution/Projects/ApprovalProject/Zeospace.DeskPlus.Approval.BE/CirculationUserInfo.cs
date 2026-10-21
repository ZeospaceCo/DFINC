using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class CirculationUserInfo : DeptCodeInfo
    {

        public CirculationUserInfo() { }

        public int CompanyID { get; set; }
        public int DocumentID { get; set; }
        public int CirculationDocumentID { get; set; }
        public int CirculationUserID { get; set; }
        public int DeptCodeID { get; set; }
        public int UserID { get; set; }        
        public DateTime ConfirmDate { get; set; }
        public string ConfirmDateYMD { get; set; }
        public string IsDelete { get; set; }        

        /*그외 필드*/
        public string ConfirmDateStr { get; set; }
    }
}
