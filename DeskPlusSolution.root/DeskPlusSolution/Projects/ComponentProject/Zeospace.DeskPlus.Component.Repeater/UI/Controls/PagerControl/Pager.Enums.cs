using System;
using System.Collections.Generic;
using System.Text;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.PagerControl
{
    /// <summary>
	/// Pager 컨트롤의 렌더링 모드를 지정합니다.
	/// </summary>
	public enum PagerMode {
		/// <summary>
		/// 페이지 번호를 이용한 이동이 가능한 페이징 컨트롤을 렌더링합니다.
		/// </summary>
		Numeric,
		/// <summary>
		/// 드롭다운 형태로 이동이 가능한 페이징 컨트롤을 렌더링합니다.
		/// </summary>
		DropDown
	}//end of enum PagerMode

	/// <summary>
	/// Pager 컨트롤의 네비게이션 버튼의 형식을 지정합니다.
	/// </summary>
	public enum NavigateButtons : int {
		/// <summary>
		/// 네비게이션 버튼을 보여주지 않습니다.
		/// </summary>
		None = 0,
		/// <summary>
		/// 이전, 다음 버튼을 보여줍니다.
		/// </summary>
		PrevNext = 1,
		/// <summary>
		/// 처음, 마지막 버튼을 보여줍니다.
		/// </summary>
		FirstLast = 2,
		/// <summary>
		/// 처음, 이전, 다음, 마지막 버튼을 보여줍니다.
		/// </summary>
		FirstPrevNextLast = 3
	}//end of enum NavigateButtons

	/// <summary>
	/// Pager 컨트롤에서 이전 또는 다음 버튼을 클릭했을 때의 행동 양식을 지정합니다.
	/// </summary>
	public enum PrevNextButtonBehaviors {
		/// <summary>
		/// 이전 또는 다음 페이지로 이동합니다.
		/// </summary>
		SinglePage,
		/// <summary>
		/// 이전 또는 다음 페이징 블록으로 이동합니다.
		/// </summary>
		PagingBlock
	}//end of enum PrevNextButtonBehaviors

	/// <summary>
	/// Pager 컨트롤에서 이동할 페이지가 없을 경우 행동 양식을 지정합니다.
	/// </summary>
	public enum UnreachablePageBehaviors {
		/// <summary>
		/// 기본 동작을 수행합니다.
		/// </summary>
		NotSet,
		/// <summary>
		/// 이동할 수 없는 페이징 버튼을 숨깁니다.
		/// </summary>
		Hide,
		/// <summary>
		/// 이동할 수 없는 페이징 버튼을 클릭하면 안내 메시지를 보여줍니다.
		/// </summary>
		ShowAlert
	}//end of enum UnreachablePageBehaviors
}//end of namespace Daniel.Web.UI.Controls