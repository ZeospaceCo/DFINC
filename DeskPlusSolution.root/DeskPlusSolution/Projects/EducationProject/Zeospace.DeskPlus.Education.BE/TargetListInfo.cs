using System;

namespace Zeospace.DeskPlus.Education.BE
{
    [Serializable]
    public class TargetListInfo
    {
        public string WorkplaceName { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
        public string UserName { get; set; }
        public string StatusStr { get; set; }
        public string CompleteDateStr { get; set; }
    }
}
