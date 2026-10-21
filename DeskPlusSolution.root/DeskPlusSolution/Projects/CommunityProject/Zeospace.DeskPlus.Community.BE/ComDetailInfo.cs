using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Community.BE
{
    [Serializable]
    public class ComDetailInfo
    {      
        public int CompanyID { get; set; }
        public int CommunityID { get; set; }
        public string TitleImage { get; set; }
        public string TitleTextColor { get; set; }
        public string MenuColor1 { get; set; }
        public string MenuColor2 { get; set; }
        public string MenuColor3 { get; set; }
        public string MenuBlock1TextClass { get; set; }
        public string MenuBlock2TextClass { get; set; }
        public string AdminDescription { get; set; }
    }
}
