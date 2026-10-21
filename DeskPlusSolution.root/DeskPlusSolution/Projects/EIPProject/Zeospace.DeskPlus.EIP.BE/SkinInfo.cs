using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.EIP.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SkinInfo
    {
        public SkinInfo() { }

        public int CompanyID { get; set; }
        public int SkinID { get; set; }
        public int TabID { get; set; }
        public int UserID { get; set; }
        public string SkinColor { get; set; }
    }
}
