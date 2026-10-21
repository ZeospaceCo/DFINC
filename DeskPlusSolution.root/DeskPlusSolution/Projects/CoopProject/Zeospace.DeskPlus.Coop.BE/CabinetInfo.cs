using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Coop.BE
{
    /// <summary>
    /// CabinetInfo
    /// </summary>
    [Serializable]
    public class CabinetInfo
    {
        public CabinetInfo() { }

        public int CompanyID {get;set;}
        public string CabinetID {get;set;}
        public string ParentCabinetID {get;set;}        
        public int CabinetOrder {get;set;}
        public string NameBase {get;set;}
        public string NameKo {get;set;}
        public string NameEn {get;set;}
        public string NameJa {get;set;}
        public string NameFr {get;set;}
        public string NameZhcn {get;set;}
        public string NameZhtw {get;set;}
        public string TargetPage {get;set;}
        public string ActiveCabinet {get;set;}
        public int WriteUserID {get;set;}
        public DateTime WriteDate {get;set;}
        public int ModifyUserID {get;set;}
        public DateTime ModifyDate {get;set;}

    }
}
