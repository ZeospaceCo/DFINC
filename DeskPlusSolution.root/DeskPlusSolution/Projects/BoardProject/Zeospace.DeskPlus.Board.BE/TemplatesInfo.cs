using System;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class TemplatesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatesInfo"/> class.
        /// </summary>
        public TemplatesInfo()
        {
        }

        public int CompanyID { get; set; }

        public int TemplateID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }

        public string WriteDateStr { get; set; }

        public string ROW { get; set; }
    }
}