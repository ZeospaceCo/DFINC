using System;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class TOAttachInfo : AttachInfo
    {
        public int UserID { get; set; }
        public DateTime WriteDate { get; set; }
        public string UserName { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
    }
}
