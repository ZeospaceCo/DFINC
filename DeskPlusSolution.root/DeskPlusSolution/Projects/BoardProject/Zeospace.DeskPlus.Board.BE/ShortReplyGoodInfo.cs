using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Board.BE
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ShortReplyGoodInfo : Common.BE.DeptCodeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortReplyInfo"/> class.
        /// </summary>
        public ShortReplyGoodInfo()
        {
        }

        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public int ShortReplyID { get; set; }
        public int UserID { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
    }
}
