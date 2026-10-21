using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    [Serializable]
    public class ItemFavUserInfo : Common.BE.DeptCodeInfo
    {
        public int CompanyID { get; set; }
        public int ItemFavUserID {get;set;}
        public int ItemSubID {get;set;}
        public int UserID {get;set;}
        public int Orders {get;set;}
        public int WriteUserID {get;set;}

    }
}
