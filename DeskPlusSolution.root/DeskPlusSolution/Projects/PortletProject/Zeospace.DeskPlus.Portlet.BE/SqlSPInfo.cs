using System;

namespace Zeospace.DeskPlus.Portlet.BE
{
    [Serializable]
    public class SqlSPInfo
    {
        public SqlSPInfo()
        {
        }

        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 링크
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 작성일자
        /// </summary>
        public DateTime PublicationDate { get; set; }
    }
}