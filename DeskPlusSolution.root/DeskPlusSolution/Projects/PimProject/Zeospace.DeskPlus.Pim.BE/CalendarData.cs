using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class CalendarData
    {
        public CalendarData()
        {
        }
        
        public string description { get; set; }
        public string id { get; set; }
        public string title { get; set; }        
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public bool allDay { get; set; }
        public bool editable { get; set; }

        /// <summary>
        /// 내부 박스 안에 색깔을 입힌다.
        /// </summary>
        /// <value>
        /// 내부 박스 안에 색깔을 입힌다.
        /// </value>
        public string backgroundColor { get; set; }
        /// <summary>
        /// 상태값(이는 테두리 칼라로 처리한다.)
        /// </summary>
        /// <value>
        /// 상태값(이는 테두리 칼라로 처리한다.)
        /// </value>
        public string borderColor { get; set; }

        public string rcnt { get; set; }
    }
}
