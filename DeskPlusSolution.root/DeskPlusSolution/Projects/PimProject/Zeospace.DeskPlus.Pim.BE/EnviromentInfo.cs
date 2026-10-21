using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    public class EnviromentInfo
    {
        public EnviromentInfo()
        {
        }

        // DP_PIM_Enviroment
        ///<summary>
        ///CompanyID
        ///</summary>
        public int CompanyID { get; set; }

        ///<summary>
        ///UserID
        ///</summary>
        public int UserID { get; set; }

        ///<summary>
        ///Kind
        ///</summary>
        public int Kind { get; set; }

        ///<summary>
        ///Etc1
        ///</summary>
        public string Etc1 { get; set; }

        ///<summary>
        ///Etc2
        ///</summary>
        public string Etc2 { get; set; }

        ///<summary>
        ///WriteDate
        ///</summary>
        public DateTime WriteDate { get; set; } 


    }
}
