using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class AdminMenuInfo
    {
        public AdminMenuInfo(){}

        /// <summary>
        /// 메뉴 ID
        /// </summary>
        public string ID { get; set; }
        

        /// <summary>
        /// 메뉴 명
        /// </summary>
        public string Text { get; set; }
        

        /// <summary>
        /// 부모 메뉴 ID
        /// </summary>
        public string ParentID { get; set; }
        

        /// <summary>
        /// Action
        /// </summary>
        public string ClientSideCommand { get; set; }
    }



}
