using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Statistics.BE
{
    [Serializable]
    public class LoginTotalInfo 
    {
        public LoginTotalInfo() { }

        public int CompanyID { get; set; }
        public string TargetYear { get; set; }

        public int LoginTotalCount { get; set; }
        public int LoginFailTotalCount { get; set; }
        public int MLoginTotalCount { get; set; }
        public int MLoginFailTotalCount { get; set; }



    }
}
