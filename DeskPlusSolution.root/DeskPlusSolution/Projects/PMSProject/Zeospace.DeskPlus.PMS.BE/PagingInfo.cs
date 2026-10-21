using System;

namespace Zeospace.DeskPlus.PMS.BE
{
    /// <summary>
    /// PagingInfo
    /// </summary>
    [Serializable]
    public class PagingInfo
    {
        public PagingInfo()
        {
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page size page no.
        /// </summary>
        /// <value>The page size page no.</value>
        public int PageSizePageNo { get; set; }

        /// <summary>
        /// Gets or sets the search field.
        /// </summary>
        /// <value>The search field.</value>
        public string SearchField { get; set; }

        /// <summary>
        /// Gets or sets the search value.
        /// </summary>
        /// <value>The search value.</value>
        public string SearchValue { get; set; }

        /// <summary>
        /// 시작일자
        /// </summary>
        public string SearchStartDate { get; set; }

        /// <summary>
        /// 종료일자
        /// </summary>
        public string SearchEndDate { get; set; }
    }
}