using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.PagerControl
{
    /// <summary>
	/// Pager 컨트롤의 PageIndexChanged 이벤트에 데이터를 제공합니다.
	/// </summary>
    public sealed class PageIndexChangedEventArgs : EventArgs 
    {
		private int _newPageIndex;

		/// <summary>
		/// 생성자 함수
		/// </summary>
		/// <param name="newPageIndex">새 페이지 인덱스</param>
		public PageIndexChangedEventArgs(int newPageIndex) {
			this._newPageIndex = newPageIndex;
		}

		/// <summary>
		/// 새 페이지 인덱스를 가져옵니다.
		/// </summary>
		public int NewPageIndex {
			get { return this._newPageIndex; }
		}
	}
}
