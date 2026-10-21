using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.Design;
using Zeospace.DeskPlus.Component.Repeater.UI.Controls.PagerControl;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.Designers.PagerControl
{
    public class PagerDesigner : System.Web.UI.Design.ControlDesigner
    {
        /// <summary>
		/// 디자인 타임에 보여질 HTML 코드를 가져옵니다.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml() {
			Pager pager = this.Component as Pager;
			if (pager == null) {
				return this.GetErrorDesignTimeHtml(new Exception());
			}

			string html = String.Empty;
			switch (pager.Mode) {
				case PagerMode.DropDown:
					html = this.RenderAsDropDown(pager);
					break;
				case PagerMode.Numeric:
					html = this.RenderAsNumeric(pager);
					break;
			}

			return html;
		}

		private string RenderAsDropDown(Pager pager) {
			StringBuilder objBuilder = new StringBuilder();
			objBuilder.AppendLine("<select style=\"");

			CssStyleCollection objCol = pager.DropDownStyle.GetStyleAttributes(pager.Page);
			IEnumerator objEnum = objCol.Keys.GetEnumerator();

			while (objEnum.MoveNext()) {
				string key = (string)objEnum.Current;
				objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
			}

			objBuilder.Append("\"><option>");
			if (String.IsNullOrEmpty(pager.DropDownNumericFormatString)) {
				objBuilder.Append(pager.CurrentPageIndex + 1);
			}
			else {
				objBuilder.AppendFormat(pager.DropDownNumericFormatString, pager.CurrentPageIndex + 1);
			}
			objBuilder.AppendLine("</option>");
			objBuilder.AppendLine("</select>");
			return objBuilder.ToString();
		}

		private string RenderAsNumeric(Pager pager) {
			StringBuilder objBuilder = new StringBuilder();

			int currentNavigateButtons = (int)pager.NavigateButton;
			if ((currentNavigateButtons & 2) > 0) {
				//처음 버튼 출력
				if (pager.CurrentPageIndex > 0 || pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
					objBuilder.Append("<a href=\"#\" style=\"");

					CssStyleCollection objCol = pager.NavigateButtonStyle.GetStyleAttributes(pager.Page);
					IEnumerator objEnum = objCol.Keys.GetEnumerator();

					while (objEnum.MoveNext()) {
						string key = (string)objEnum.Current;
						objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
					}

					if (!String.IsNullOrEmpty(pager.FirstPageImageUrl)) {
						objBuilder.AppendFormat("\"><img src=\"{0}\" border=\"0\"></a>", pager.FirstPageImageUrl);
					}
					else {
						objBuilder.AppendFormat("\">{0}</a>", pager.FirstPageText);
					}
					objBuilder.Append(pager.NavigateButtonDelimiter);
				}
			}

			if ((currentNavigateButtons & 1) > 0) {
				//이전 버튼 출력
				bool printPrev = false;

				if (pager.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
					if (pager.CurrentPageIndex > 0 || pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
						printPrev = true;
					}
				}
				else {
					if (pager.CurrentPageIndex >= pager.NumericButtonCount) {
						printPrev = true;
					}
				}

				if (!printPrev && pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
					printPrev = true;
				}

				if (printPrev) {
					objBuilder.Append("<a href=\"#\" style=\"");

					CssStyleCollection objCol = pager.NavigateButtonStyle.GetStyleAttributes(pager.Page);
					IEnumerator objEnum = objCol.Keys.GetEnumerator();

					while (objEnum.MoveNext()) {
						string key = (string)objEnum.Current;
						objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
					}

					if (!String.IsNullOrEmpty(pager.PreviousPageImageUrl)) {
						objBuilder.AppendFormat("\"><img src=\"{0}\" border=\"0\"></a>", pager.PreviousPageImageUrl);
					}
					else {
						objBuilder.AppendFormat("\">{0}</a>", pager.PreviousPageText);
					}
					objBuilder.Append(pager.NavigateNumericButtonDelimiter);
				}
			}

			int firstIndex = 0;
			int lastIndex = 0;

			PagingHelper.GetPagerIndexRange(pager, out firstIndex, out lastIndex);

			//숫자 버튼 출력
			for (int i = firstIndex; i <= lastIndex; i++) {
				objBuilder.Append("<a href=\"#\" style=\"");

				CssStyleCollection objCol = null;

				if (i == pager.CurrentPageIndex) {
					objCol = pager.SelectedNumericItemStyle.GetStyleAttributes(pager.Page);
				}
				else {
					objCol = pager.NumericItemStyle.GetStyleAttributes(pager.Page);
				}

				IEnumerator objEnum = objCol.Keys.GetEnumerator();
				while (objEnum.MoveNext()) {
					string key = (string)objEnum.Current;
					objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
				}
				
				objBuilder.Append("\">");
				if (String.IsNullOrEmpty(pager.NumericButtonFormatString)) {
					objBuilder.Append(i + 1);
				}
				else {
					objBuilder.AppendFormat(pager.NumericButtonFormatString, i + 1);
				}
				objBuilder.Append("</a>");
				
				if (i < lastIndex) {
					objBuilder.AppendFormat(pager.NumericButtonDelimiter);
				}
			}

			//다음 버튼 출력

			if ((currentNavigateButtons & 1) > 0) {
				//이전 버튼 출력
				bool printNext = false;

				if (pager.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
					if (lastIndex < (pager.PageCount - 1) || pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
						printNext = true;
					}
				}
				else {
					if (lastIndex < (pager.PageCount - 1)) {
						printNext = true;
					}
				}

				if (!printNext && pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
					printNext = true;
				}

				if (printNext) {
					objBuilder.Append(pager.NavigateNumericButtonDelimiter);
					objBuilder.Append("<a href=\"#\" style=\"");

					CssStyleCollection objCol = pager.NavigateButtonStyle.GetStyleAttributes(pager.Page);
					IEnumerator objEnum = objCol.Keys.GetEnumerator();

					while (objEnum.MoveNext()) {
						string key = (string)objEnum.Current;
						objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
					}

					if (!String.IsNullOrEmpty(pager.NextPageImageUrl)) {
						objBuilder.AppendFormat("\"><img src=\"{0}\" border=\"0\"></a>", pager.NextPageImageUrl);
					}
					else {
						objBuilder.AppendFormat("\">{0}</a>", pager.NextPageText);
					}
				}
			}

			if ((currentNavigateButtons & 2) > 0) {
				//마지막 버튼 출력
				if ((lastIndex < pager.PageCount - 1) || pager.UnreachablePageBehavior != UnreachablePageBehaviors.Hide) {
					objBuilder.Append(pager.NavigateButtonDelimiter);
					objBuilder.Append("<a href=\"#\" style=\"");

					CssStyleCollection objCol = pager.NavigateButtonStyle.GetStyleAttributes(pager.Page);
					IEnumerator objEnum = objCol.Keys.GetEnumerator();

					while (objEnum.MoveNext()) {
						string key = (string)objEnum.Current;
						objBuilder.Append(String.Format("{0}:{1};", key, objCol[key]));
					}

					if (!String.IsNullOrEmpty(pager.FirstPageImageUrl)) {
						objBuilder.AppendFormat("\"><img src=\"{0}\" border=\"0\"></a>", pager.LastPageImageUrl);
					}
					else {
						objBuilder.AppendFormat("\">{0}</a>", pager.LastPageText);
					}
				}
			}

			return objBuilder.ToString();
		}
    }
}
