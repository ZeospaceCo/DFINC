using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class CharListInfo
    {
        public CharListInfo()
        {
        }

        public int CompanyID { get; set; }

        public int SMSNoticeTemplateID { get; set; }

        public string Gubun { get; set; }

        public string Content { get; set; }

        public string ContentDecode { get; set; }

        public int WriteUserID { get; set; }
    }
}