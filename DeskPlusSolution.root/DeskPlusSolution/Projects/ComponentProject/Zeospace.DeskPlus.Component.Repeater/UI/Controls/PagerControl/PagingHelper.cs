using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.PagerControl
{
    internal static class PagingHelper {
		internal static int GetPageCount(Pager pager) {
			if (pager == null) {
				throw new ArgumentNullException("pager");
			}

			int recordCount = pager.RecordCount;
			int pageSize = pager.PageSize;
			int pageCount = 0;

			if ((recordCount == 0) || (pageSize == 0)) {
				return 0;
			}

			pageCount = recordCount / pageSize;
			if (recordCount % pageSize > 0) pageCount++;

			return pageCount;
		}

		internal static void GetPagerIndexRange(Pager pager, out int firstIndex, out int lastIndex) {
			if (pager == null) {
				throw new ArgumentNullException("pager");
			}

			if (pager.Mode == PagerMode.DropDown) {
				firstIndex = 0;
				lastIndex = pager.PageCount - 1;
			}
			else {
				if (pager.NavigateButton == NavigateButtons.None || pager.NavigateButton == NavigateButtons.FirstLast) {
					firstIndex = pager.CurrentPageIndex - 2;
					if (firstIndex < 0) {
						firstIndex = 0;
					}

					lastIndex = firstIndex + (pager.NumericButtonCount - 1);
					if (lastIndex > pager.PageCount - 1) {
						lastIndex = pager.PageCount - 1;
					}
				}
				else {
					firstIndex = (pager.CurrentPageIndex / pager.NumericButtonCount) * pager.NumericButtonCount;
					lastIndex = (firstIndex + pager.NumericButtonCount) - 1;

					if (lastIndex >= pager.PageCount) lastIndex = pager.PageCount - 1;
				}
			}

			if (pager.PageCount == 0) lastIndex = 0;
		}
	}//end of class PagingHelper
}