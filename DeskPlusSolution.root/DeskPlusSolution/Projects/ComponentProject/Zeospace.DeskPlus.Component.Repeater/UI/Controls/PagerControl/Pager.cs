using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.PagerControl
{
    /// <summary>
	/// 페이징 기능을 제공하는 웹 서버 컨트롤입니다.
	/// </summary>
	[ToolboxDataAttribute("<{0}:Pager runat=\"server\"></{0}:Pager>")]
	[DesignerAttribute(typeof(Zeospace.DeskPlus.Component.Repeater.UI.Controls.Designers.PagerControl.PagerDesigner))]
	[DefaultPropertyAttribute("CurrentPageIndex")]
	[DefaultBindingPropertyAttribute("CurrentPageIndex")]
	public class Pager : WebControl, IPostBackDataHandler, IPostBackEventHandler, INamingContainer {
		private Style _selectedNumericItemStyle;
		private Style _numericItemStyle;
		private Style _navigateButtonStyle;
		private Style _dropDownStyle;
		private int _flags = 0;

		/// <summary>
		/// 페이징 컨트롤의 현재 페이지가 변경되면 발생합니다.
		/// </summary>
		public event PageIndexChangedEventHandler PageIndexChanged {
			add {
				base.Events.AddHandler("PageIndexChanged", value);
			}
			remove {
				base.Events.RemoveHandler("PageIndexChanged", value);
			}
		}

		/// <summary>
		/// Pager 클래스의 인스턴스를 생성합니다.
		/// </summary>
		public Pager()
			: base() {
		}

		/// <summary>
		/// 현재 페이지의 인덱스를 지정하거나 가져옵니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DefaultValueAttribute(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Data")]
		[Description("현재 페이지의 인덱스를 가져오거나 지정합니다.")]
		public int CurrentPageIndex {
			get {
				if (this.ViewState["_currentPageIndex"] != null) {
					return (int)this.ViewState["_currentPageIndex"];
				}
				return 0;
			}
			set {
				if ((int)value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["_currentPageIndex"] = value;

				if ((int)value > this.PageCount - 1) {
					this.ViewState["_currentPageIndex"] = 0;
				}
				else {
					this.ViewState["_currentPageIndex"] = value;
				}
			}
		}

		/// <summary>
		/// 컨트롤의 렌더링 방식을 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute(typeof(PagerMode), "Numeric")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("컨트롤의 렌더링 방식을 가져오거나 지정합니다.")]
		public PagerMode Mode {
			get {
				if (this.ViewState["_mode"] != null) {
					return (PagerMode)this.ViewState["_mode"];
				}
				return PagerMode.Numeric;
			}
			set {
				this.ViewState["_mode"] = value;
			}
		}

		/// <summary>
		/// 전체 페이지 개수가 1일 경우 페이징 컨트롤의 렌더링 여부를 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute(false)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("전체 페이지 개수가 1일 경우 페이징 컨트롤의 렌더링 여부를 가져오거나 지정합니다.")]
		public bool HideOnSingleView {
			get {
				if (this.ViewState["_hideOnSingleView"] != null) {
					return (bool)this.ViewState["_hideOnSingleView"];
				}
				return false;
			}
			set {
				this.ViewState["_hideOnSingleView"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 각 숫자 버튼 사이의 구분 문자를 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("&nbsp;")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 각 숫자 버튼 사이의 구분 문자를 가져오거나 지정합니다.")]
		public string NumericButtonDelimiter {
			get {
				if (this.ViewState["_numericButtonDelimiter"] != null) {
					return this.ViewState["_numericButtonDelimiter"].ToString();
				}
				return "&nbsp;";
			}
			set {
				this.ViewState["_numericButtonDelimiter"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 각 이동 버튼 사이의 구분 문자를 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("&nbsp;")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 각 이동 버튼 사이의 구분 문자를 가져오거나 지정합니다.")]
		public string NavigateButtonDelimiter {
			get {
				if (this.ViewState["_navigateButtonDelimiter"] != null) {
					return this.ViewState["_navigateButtonDelimiter"].ToString();
				}
				return "&nbsp;";
			}
			set {
				this.ViewState["_navigateButtonDelimiter"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 이동 버튼과 숫자 버튼 사이의 구분 문자를 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("&nbsp;")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 이동 버튼과 숫자 버튼 사이의 구분 문자를 가져오거나 지정합니다.")]
		public string NavigateNumericButtonDelimiter {
			get {
				if (this.ViewState["_navigateNumericButtonDelimiter"] != null) {
					return this.ViewState["_navigateNumericButtonDelimiter"].ToString();
				}
				return "&nbsp;";
			}
			set {
				this.ViewState["_navigateNumericButtonDelimiter"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 선택된 숫자의 스타일을 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[CategoryAttribute("Style")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 선택된 숫자의 스타일을 지정합니다.")]
		public Style SelectedNumericItemStyle {
			get {
				if (this._selectedNumericItemStyle == null) {
					this._selectedNumericItemStyle = new Style();
				}

				return this._selectedNumericItemStyle;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 선택되지 않은 숫자의 스타일을 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[CategoryAttribute("Style")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 선택되지 않은 숫자의 스타일을 지정합니다.")]
		public Style NumericItemStyle {
			get {
				if (this._numericItemStyle == null) {
					this._numericItemStyle = new Style();
				}

				return this._numericItemStyle;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 네비게이션 버튼의 스타일을 지정합니다. 이 속성은 Mode 속성이 DropDown일 경우 무시됩니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[CategoryAttribute("Style")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 선택된 숫자의 스타일을 지정합니다.")]
		public Style NavigateButtonStyle {
			get {
				if (this._navigateButtonStyle == null) {
					this._navigateButtonStyle = new Style();
				}

				return this._navigateButtonStyle;
			}
		}

		/// <summary>
		/// Mode속성이 DropDown일 경우 드롭다운 리스트의 스타일을 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
		[CategoryAttribute("Style")]
		[DescriptionAttribute("Mode속성이 DropDown일 경우 드롭다운 리스트의 스타일을 지정합니다.")]
		public Style DropDownStyle {
			get {
				if (this._dropDownStyle == null) {
					this._dropDownStyle = new Style();
				}

				return this._dropDownStyle;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 숫자 버튼의 캡션 형식을 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("{0}")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 숫자 버튼의 캡션 형식을 지정합니다.")]
		public string NumericButtonFormatString {
			get {
				if (this.ViewState["_numericButtonFormatString"] != null) {
					return this.ViewState["_numericButtonFormatString"].ToString();
				}
				return "{0}";
			}
			set {
				this.ViewState["_numericButtonFormatString"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 DropDown일 경우 페이지 번호의 형식을 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("{0} Page")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 DropDown일 경우 페이지 번호의 형식을 지정합니다.")]
		public string DropDownNumericFormatString {
			get {
				if (this.ViewState["_dropDownNumericFormatString"] != null) {
					return this.ViewState["_dropDownNumericFormatString"].ToString();
				}
				return "{0} Page";
			}
			set {
				this.ViewState["_dropDownNumericFormatString"] = value;
			}
		}

		/// <summary>
		/// 전체 레코드 개수를 지정하거나 가져옵니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[BindableAttribute(true, BindingDirection.TwoWay)]
		[DefaultValueAttribute(0)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Data")]
		[DescriptionAttribute("전체 레코드 개수를 지정하거나 가져옵니다.")]
		public int RecordCount {
			get {
				if (this.ViewState["_recordCount"] != null) {
					return (int)this.ViewState["_recordCount"];
				}
				return 0;
			}
			set {
				if ((int)value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.ViewState["_recordCount"] = value;
				this.PageCount = PagingHelper.GetPageCount(this);
			}
		}

		/// <summary>
		/// 한 페이지 당 페이지의 개수를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[BindableAttribute(true, BindingDirection.TwoWay)]
		[DefaultValueAttribute(10)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Data")]
		[DescriptionAttribute("한 페이지 당 페이지의 개수를 가져옵니다.")]
		public int PageSize {
			get {
				if (this.ViewState["_pageSize"] != null) {
					return (int)this.ViewState["_pageSize"];
				}
				return 10;
			}
			set {
				if ((int)value <= 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.ViewState["_pageSize"] = value;
				this.PageCount = PagingHelper.GetPageCount(this);
			}
		}

		/// <summary>
		/// 전체 페이지의 개수를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(false)]
		[BindableAttribute(BindableSupport.No)]
		[DefaultValueAttribute(1)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[CategoryAttribute("Data")]
		[DescriptionAttribute("전체 페이지의 개수를 가져옵니다.")]
		public int PageCount {
			get {
				if (this.ViewState["_pageCount"] != null) {
					return (int)this.ViewState["_pageCount"];
				}
				return 1;
			}
			internal set {
				if ((int)value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.ViewState["_pageCount"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 보여질 페이징 버튼의 개수를 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute(10)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 보여질 페이징 버튼의 개수를 가져오거나 지정합니다.")]
		public int NumericButtonCount {
			get {
				if (this.ViewState["_numericButtonCount"] != null) {
					return (int)this.ViewState["_numericButtonCount"];
				}
				return 10;
			}
			set {
				if ((int)value <= 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.ViewState["_numericButtonCount"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 처음 버튼의 텍스트를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("[First]")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 처음 버튼의 텍스트를 지정합니다.")]
		public string FirstPageText {
			get {
				if (this.ViewState["_firstPageText"] != null) {
					return this.ViewState["_firstPageText"].ToString();
				}

				return "[First]";
			}
			set {
				if (String.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["_firstPageText"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 이전 버튼의 텍스트를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("[Prev]")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 이전 버튼의 텍스트를 지정합니다.")]
		public string PreviousPageText {
			get {
				if (this.ViewState["_prevPageText"] != null) {
					return this.ViewState["_prevPageText"].ToString();
				}

				return "[Prev]";
			}
			set {
				if (String.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["_prevPageText"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 다음 버튼의 텍스트를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("[Next]")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 다음 버튼의 텍스트를 지정합니다.")]
		public string NextPageText {
			get {
				if (this.ViewState["_nextPageText"] != null) {
					return this.ViewState["_nextPageText"].ToString();
				}

				return "[Next]";
			}
			set {
				if (String.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["_nextPageText"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 마지막 버튼의 텍스트를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("[Last]")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 마지막 버튼의 텍스트를 지정합니다.")]
		public string LastPageText {
			get {
				if (this.ViewState["_lastPageText"] != null) {
					return this.ViewState["_lastPageText"].ToString();
				}

				return "[Last]";
			}
			set {
				if (String.IsNullOrEmpty(value)) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["_lastPageText"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 처음 버튼의 이미지 경로를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 처음 버튼의 이미지 경로를 지정합니다.")]
		public string FirstPageImageUrl {
			get {
				if (this.ViewState["_firstPageImgUrl"] != null) {
					return this.ViewState["_firstPageImgUrl"].ToString();
				}

				return String.Empty;
			}
			set {
				this.ViewState["_firstPageImgUrl"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 이전 버튼의 이미지 경로를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 이전 버튼의 이미지 경로를 지정합니다.")]
		public string PreviousPageImageUrl {
			get {
				if (this.ViewState["_prevPageImgUrl"] != null) {
					return this.ViewState["_prevPageImgUrl"].ToString();
				}

				return String.Empty;
			}
			set {
				this.ViewState["_prevPageImgUrl"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 다음 버튼의 이미지 경로를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 다음 버튼의 이미지 경로를 지정합니다.")]
		public string NextPageImageUrl {
			get {
				if (this.ViewState["_nextPageImgUrl"] != null) {
					return this.ViewState["_nextPageImgUrl"].ToString();
				}

				return String.Empty;
			}
			set {
				this.ViewState["_nextPageImgUrl"] = value;
			}
		}

		/// <summary>
		/// Mode속성이 Numeric일 경우 마지막 버튼의 이미지 경로를 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute("")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Appearance")]
		[DescriptionAttribute("Mode속성이 Numeric일 경우 마지막 버튼의 이미지 경로를 지정합니다.")]
		public string LastPageImageUrl {
			get {
				if (this.ViewState["_lastPageImgUrl"] != null) {
					return this.ViewState["_lastPageImgUrl"].ToString();
				}

				return String.Empty;
			}
			set {
				this.ViewState["_lastPageImgUrl"] = value;
			}
		}

		/// <summary>
		/// 이동할 수 없는 페이지 버튼에 대한 행동 양식을 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute(typeof(UnreachablePageBehaviors), "NotSet")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Behavior")]
		[DescriptionAttribute("이동할 수 없는 페이지 버튼에 대한 행동 양식을 가져오거나 지정합니다.")]
		public UnreachablePageBehaviors UnreachablePageBehavior {
			get {
				if (this.ViewState["_unreachablePageBehavior"] != null) {
					return (UnreachablePageBehaviors)this.ViewState["_unreachablePageBehavior"];
				}
				return UnreachablePageBehaviors.NotSet;
			}
			set {
				this.ViewState["_unreachablePageBehavior"] = value;
			}
		}

		/// <summary>
		/// 네비게이션 버튼의 형식을 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Behavior")]
		[DescriptionAttribute("네비게이션 버튼의 형식을 가져오거나 지정합니다.")]
		public NavigateButtons NavigateButton {
			get {
				if (this.ViewState["_navigationButtons"] != null) {
					return (NavigateButtons)this.ViewState["_navigationButtons"];
				}

				return NavigateButtons.FirstPrevNextLast;
			}
			set {
				if (!Enum.IsDefined(typeof(NavigateButtons), value) ||
					this.Mode == PagerMode.DropDown) {
					this.ViewState["_navigationButtons"] = NavigateButtons.None;
				}
				else {
					this.ViewState["_navigationButtons"] = value;
				}
			}
		}

		/// <summary>
		/// 이전, 다음 버튼의 행동 양식을 가져오거나 지정합니다.
		/// </summary>
		[BrowsableAttribute(true)]
		[DefaultValueAttribute(typeof(PrevNextButtonBehaviors), "PagingBlock")]
		[PersistenceModeAttribute(PersistenceMode.Attribute)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
		[CategoryAttribute("Behavior")]
		[DescriptionAttribute("네비게이션 버튼의 형식을 가져오거나 지정합니다.")]
		public PrevNextButtonBehaviors PrevNextButtonBehavior {
			get {
				if (this.ViewState["_prevNextButtonBehavior"] != null) {
					return (PrevNextButtonBehaviors)this.ViewState["_prevNextButtonBehavior"];
				}
				return PrevNextButtonBehaviors.PagingBlock;
			}
			set {
				this.ViewState["_prevNextButtonBehavior"] = value;
			}
		}

		/// <summary>
		/// 처음 페이지로 이동 가능 여부를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(false)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[BindableAttribute(false)]
		[ThemeableAttribute(false)]
		public bool CanMoveToFirst {
			get {
				if (this.PageCount == 0) {
					return false;
				}

				return (this.CurrentPageIndex > 0);
			}
		}

		/// <summary>
		/// 이전 페이지로 이동 가능 여부를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(false)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[BindableAttribute(false)]
		[ThemeableAttribute(false)]
		public bool CanMoveToPrevious {
			get {
				if (this.PageCount == 0) {
					return false;
				}

				if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.PagingBlock) {
					int first = 0;
					int last = 0;
					PagingHelper.GetPagerIndexRange(this, out first, out last);

					return (first >= this.NumericButtonCount);
				}
				else {
					return (this.CurrentPageIndex > 0);
				}
			}
		}

		/// <summary>
		/// 다음 페이지로 이동 가능 여부를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(false)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[BindableAttribute(false)]
		[ThemeableAttribute(false)]
		public bool CanMoveToNext {
			get {
				if (this.PageCount == 0) {
					return false;
				}

				if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.PagingBlock) {
					int first = 0;
					int last = 0;
					PagingHelper.GetPagerIndexRange(this, out first, out last);

					return (last < this.PageCount - 1);
				}
				else {
					return (this.CurrentPageIndex < this.PageCount - 1);
				}
			}
		}

		/// <summary>
		/// 마지막 페이지로 이동 가능 여부를 가져옵니다.
		/// </summary>
		[BrowsableAttribute(false)]
		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		[BindableAttribute(false)]
		[ThemeableAttribute(false)]
		public bool CanMoveToLast {
			get {
				if (this.PageCount == 0) {
					return false;
				}

				return (this.CurrentPageIndex < this.PageCount - 1);
			}
		}

		/// <summary>
		/// 컨트롤을 렌더링 합니다.
		/// </summary>
		/// <param name="writer">컨트롤을 렌더링할 HtmlTextWriter 객체</param>
		protected override void Render(HtmlTextWriter writer) {
			if (this.Page != null) {
				this.Page.VerifyRenderingInServerForm(this);

				if (this.PageCount == 1 && this.HideOnSingleView) {
					return;
				}
				else {
					switch (this.Mode) {
						case PagerMode.DropDown:
							RenderAsDropDown(writer);
							break;
						case PagerMode.Numeric:
							RenderAsNumeric(writer);
							break;
					}
				}
			}
		}//end of method Render

		/// <summary>
		/// PageIndexChanged 이벤트를 실행합니다.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPageIndexChanged(PageIndexChangedEventArgs e) {
			PageIndexChangedEventHandler handler = (PageIndexChangedEventHandler)base.Events["PageIndexChanged"];
			if (handler != null) {
				handler(this, e);
			}
		}

		/// <summary>
		/// 포스트 백 이벤트를 발생시킵니다.
		/// </summary>
		/// <param name="eventArgument">포스트 백 이벤트의 파라미터</param>
		public void RaisePostBackEvent(string eventArgument) {
			int newPage = 0;

			if (this.Mode == PagerMode.DropDown) {
				Int32.TryParse(this.Context.Request.Form[this.UniqueID], out newPage);
			}
			else {
				Int32.TryParse(eventArgument, out newPage);
			}

			this.CurrentPageIndex = newPage;
			this.OnPageIndexChanged(new PageIndexChangedEventArgs(newPage));
		}

		/// <summary>
		/// 포스트 백 데이터를 가져옵니다. 이 메서드는 컨트롤 내부에서 사용됩니다.
		/// </summary>
		/// <param name="postDataKey">포스트 백 데이터 키</param>
		/// <param name="postCollection">포스트 백 데이터 컬렉션</param>
		/// <returns>포스트 백 이후 값이 변경되면 true, 그렇지 않으면 false</returns>
		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			int currentPage = this.CurrentPageIndex;
			int newPage = 0;
			Int32.TryParse(postCollection[postDataKey], out newPage);

			if (currentPage != newPage) {
				this.CurrentPageIndex = newPage;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 포스트 백 이벤트를 발생시킵니다.
		/// </summary>
		public void RaisePostDataChangedEvent() {
			this.OnPageIndexChanged(new PageIndexChangedEventArgs(this.CurrentPageIndex));
		}

		private void RenderAsDropDown(HtmlTextWriter writer) {
			//id 및 name 특성 지정
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);

			//드롭다운 리스트의 스타일 특성을 지정
			if (this._dropDownStyle != null) {
				CssStyleCollection dropDownStyle = this._dropDownStyle.GetStyleAttributes(this.Page);
				IEnumerator objKeys = dropDownStyle.Keys.GetEnumerator();

				while (objKeys.MoveNext()) {
					string key = (String)objKeys.Current;
					writer.AddStyleAttribute(key, dropDownStyle[key]);
				}
			}

			//__doPostBack 함수 호출 연결
			string postBackScript = this.Page.ClientScript.GetPostBackEventReference(this, String.Empty);
			writer.AddAttribute("onchange", postBackScript);

			//<SELECT>태그 출력
			writer.RenderBeginTag(HtmlTextWriterTag.Select);

			//페이징 번호 출력
			int firstPageIndex = 0;
			int lastPageIndex = 0;
			PagingHelper.GetPagerIndexRange(this, out firstPageIndex, out lastPageIndex);

			for (int i = firstPageIndex; i <= lastPageIndex; i++) {
				writer.AddAttribute(HtmlTextWriterAttribute.Value, i.ToString());
				if (i == this.CurrentPageIndex) {
					writer.AddAttribute(HtmlTextWriterAttribute.Selected, "Selected");
				}

				writer.RenderBeginTag(HtmlTextWriterTag.Option);
				writer.Write(String.Format(this.DropDownNumericFormatString, (i + 1)));
				writer.RenderEndTag();
			}

			//</SELECT>태그 출력
			writer.RenderEndTag();
		}

		private void RenderAsNumeric(HtmlTextWriter writer) {
			string postBackScript = String.Empty;
			int currentNavigationButtons = (int)this.NavigateButton;

			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			writer.RenderBeginTag(HtmlTextWriterTag.Span);

			//처음으로 버튼 출력
			if ((currentNavigationButtons & 2) > 0) {
				this.RenderFirstButton(writer);
				this._flags |= 0x0001;
			}
			
			//이전 버튼 출력
			if ((currentNavigationButtons & 1) > 0) {
				if ((this._flags & 0x0001) > 0) writer.Write(this.NavigateButtonDelimiter);
				this._flags |= 0x0002;
				this.RenderPreviousButton(writer);
			}

			//구분 문자 출력
			if ((this._flags & 0x0003) > 0) {
				writer.Write(this.NavigateNumericButtonDelimiter);
			}

			int firstPageIndex = 0;
			int lastPageIndex = 0;
			PagingHelper.GetPagerIndexRange(this, out firstPageIndex, out lastPageIndex);

			//숫자 버튼 출력
			for (int i = firstPageIndex; i <= lastPageIndex; i++) {
				this.RenderNumericButton(writer, i);
				if (i < lastPageIndex) {
					writer.Write(this.NumericButtonDelimiter);
				}
			}

			//구분 문자 출력
			if ((this._flags & 0x0003) > 0) {
				writer.Write(this.NavigateNumericButtonDelimiter);
			}

			//다음 버튼 출력
			if ((currentNavigationButtons & 1) > 0) {
				this._flags |= 0x0004;
				this.RenderNextButton(writer);
			}

			//마지막으로 버튼 출력
			if ((currentNavigationButtons & 2) > 0) {
				//구분 문자 출력
				if ((this._flags & 0x0004) > 0) {
					writer.Write(this.NavigateButtonDelimiter);
				}

				this.RenderLastButton(writer);
			}

			writer.RenderEndTag();
		}

		/// <summary>
		/// 네비게이션의 처음으로 버튼을 생성
		/// </summary>
		/// <param name="writer">HtmlTextWriter 객체</param>
		private void RenderFirstButton(HtmlTextWriter writer) {
			string postBackScript = this.Page.ClientScript.GetPostBackClientHyperlink(this, "0", true);
			
			//하이퍼 링크 연결
			if (this.CurrentPageIndex > 0) {
				//현재 페이지 인덱스가 0보다 크면
				//__doPostBack 함수 호출을 연결
				writer.AddAttribute("href", postBackScript);
			}
			else {
				//현재 페이지 인덱스가 0일 때

				//이용 불가 버튼 숨기기 옵션이 설정된 경우
				if (this.UnreachablePageBehavior == UnreachablePageBehaviors.Hide) {
					return;
				}
				else if (this.UnreachablePageBehavior == UnreachablePageBehaviors.ShowAlert) {
					//경고 메시지를 출력하는 스크립트 작성
					postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoFirst);
					writer.AddAttribute("href", postBackScript);
				}
			}

			//스타일 정보를 적용
			if (this.NavigateButtonStyle != null) {
				CssStyleCollection objCol = this.NavigateButtonStyle.GetStyleAttributes(this.Page);
				IEnumerator objKeys = objCol.Keys.GetEnumerator();

				while (objKeys.MoveNext()) {
					string key = (string)objKeys.Current;
					writer.AddStyleAttribute(key, objCol[key]);
				}
			}
			writer.RenderBeginTag(HtmlTextWriterTag.A);

			//이미지 경로가 지정된 경우 <IMG>태그 출력
			if (!String.IsNullOrEmpty(this.FirstPageImageUrl)) {
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ResolveUrl(this.FirstPageImageUrl));
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.RenderBeginTag("img");
				writer.RenderEndTag();
			}
			else {
				//이미지 경로가 없으면 텍스트 출력
				writer.Write(this.FirstPageText);
			}
			writer.RenderEndTag();
		}

		/// <summary>
		/// 이전 버튼을 생성
		/// </summary>
		/// <param name="writer">HtmlTextWriter 객체</param>
		private void RenderPreviousButton(HtmlTextWriter writer) {
			int prevPageIndex = 0;

			if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
				prevPageIndex = this.CurrentPageIndex - 1;
			}
			else {
				int first = 0;
				int last = 0;
				PagingHelper.GetPagerIndexRange(this, out first, out last);

				prevPageIndex = first - this.NumericButtonCount;
			}

			string postBackScript = this.Page.ClientScript.GetPostBackClientHyperlink(this, prevPageIndex.ToString(), true);

			//하이퍼 링크 연결
			if (prevPageIndex >= 0) {
				//현재 페이지 인덱스가 0보다 크면
				//__doPostBack 함수 호출을 연결
				writer.AddAttribute("href", postBackScript);
			}
			else {
				//현재 페이지 인덱스가 0일 때

				//이용 불가 버튼 숨기기 옵션이 설정된 경우
				if (this.UnreachablePageBehavior == UnreachablePageBehaviors.Hide) {
					return;
				}
				else if (this.UnreachablePageBehavior == UnreachablePageBehaviors.ShowAlert) {
					//경고 메시지를 출력하는 스크립트 작성
					if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
						postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoPrevious);
					}
					else {
						postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoPreviousBlock);
					}
					writer.AddAttribute("href", postBackScript);
				}
			}

			//스타일 정보를 적용
			if (this.NavigateButtonStyle != null) {
				CssStyleCollection objCol = this.NavigateButtonStyle.GetStyleAttributes(this.Page);
				IEnumerator objKeys = objCol.Keys.GetEnumerator();

				while (objKeys.MoveNext()) {
					string key = (string)objKeys.Current;
					writer.AddStyleAttribute(key, objCol[key]);
				}
			}

			writer.RenderBeginTag(HtmlTextWriterTag.A);

			//이미지 경로가 지정된 경우 <IMG>태그 출력
			if (!String.IsNullOrEmpty(this.PreviousPageImageUrl)) {
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ResolveUrl(this.PreviousPageImageUrl));
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.RenderBeginTag("img");
				writer.RenderEndTag();
			}
			else {
				//이미지 경로가 없으면 텍스트 출력
				writer.Write(this.PreviousPageText);
			}
			writer.RenderEndTag();
		}

		/// <summary>
		/// 다음 버튼을 생성
		/// </summary>
		/// <param name="writer">HtmlTextWriter 객체</param>
		private void RenderNextButton(HtmlTextWriter writer) {
			int nextPageIndex = 0;

			if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
				nextPageIndex = this.CurrentPageIndex + 1;
			}
			else {
				int first = 0;
				int last = 0;
				PagingHelper.GetPagerIndexRange(this, out first, out last);

				nextPageIndex = last + 1;
			}

			string postBackScript = this.Page.ClientScript.GetPostBackClientHyperlink(this, nextPageIndex.ToString(), true);

			//하이퍼 링크 연결
			if (nextPageIndex < this.PageCount) {
				//현재 페이지 인덱스가 마지막 페이지 인덱스보다 작으면
				//__doPostBack 함수 호출을 연결
				writer.AddAttribute("href", postBackScript);
			}
			else {
				//현재 페이지가 마지막 인덱스일 때

				//이용 불가 버튼 숨기기 옵션이 설정된 경우
				if (this.UnreachablePageBehavior == UnreachablePageBehaviors.Hide) {
					return;
				}

				if (this.UnreachablePageBehavior == UnreachablePageBehaviors.ShowAlert) {
					//경고 메시지를 출력하는 스크립트 작성
					if (this.PrevNextButtonBehavior == PrevNextButtonBehaviors.SinglePage) {
						postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoNext);
					}
					else {
						postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoNextBlock);
					}
					writer.AddAttribute("href", postBackScript);
				}
			}

			//스타일 정보를 적용
			if (this.NavigateButtonStyle != null) {
				CssStyleCollection objCol = this.NavigateButtonStyle.GetStyleAttributes(this.Page);
				IEnumerator objKeys = objCol.Keys.GetEnumerator();

				while (objKeys.MoveNext()) {
					string key = (string)objKeys.Current;
					writer.AddStyleAttribute(key, objCol[key]);
				}
			}
			writer.RenderBeginTag(HtmlTextWriterTag.A);

			//이미지 경로가 지정된 경우 <IMG>태그 출력
			if (!String.IsNullOrEmpty(this.NextPageImageUrl)) {
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ResolveUrl(this.NextPageImageUrl));
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.RenderBeginTag("img");
				writer.RenderEndTag();
			}
			else {
				//이미지 경로가 없으면 텍스트 출력
				writer.Write(this.NextPageText);
			}
			writer.RenderEndTag();
		}

		/// <summary>
		/// 숫자 버튼을 생성
		/// </summary>
		/// <param name="writer">HtmlTextWriter 객체</param>
		/// <param name="pageIndex">페이지 인덱스</param>
		private void RenderNumericButton(HtmlTextWriter writer, int pageIndex) {
			string postBackScript = this.Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString(), true);

			//하이퍼 링크 연결
			if (pageIndex != this.CurrentPageIndex) {
				//현재 페이지가 아니면
				//스타일 정보를 적용
				if (this.NumericItemStyle != null) {
					CssStyleCollection objCol = this.NumericItemStyle.GetStyleAttributes(this.Page);
					IEnumerator objKeys = objCol.Keys.GetEnumerator();

					while (objKeys.MoveNext()) {
						string key = (string)objKeys.Current;
						writer.AddStyleAttribute(key, objCol[key]);
					}
				}
				
				//__doPostBack 함수 호출을 연결
				writer.AddAttribute("href", postBackScript);
			}
			else {
				//현재 페이지이면

				//선택된 아이템 스타일 정보를 적용
				if (this.SelectedNumericItemStyle != null) {
					CssStyleCollection objCol = this.SelectedNumericItemStyle.GetStyleAttributes(this.Page);
					IEnumerator objKeys = objCol.Keys.GetEnumerator();

					while (objKeys.MoveNext()) {
						string key = (string)objKeys.Current;
						writer.AddStyleAttribute(key, objCol[key]);
					}
				}
			}

			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write(String.Format(this.NumericButtonFormatString, pageIndex + 1));
			writer.RenderEndTag();
		}

		/// <summary>
		/// 마지막 버튼을 생성
		/// </summary>
		/// <param name="writer">HtmlTextWriter 객체</param>
		private void RenderLastButton(HtmlTextWriter writer) {
			int lastPageIndex = (this.PageCount <= 0) ? 0 : (this.PageCount - 1);
			string postBackScript = this.Page.ClientScript.GetPostBackClientHyperlink(this, lastPageIndex.ToString(), true);

			//하이퍼 링크 연결
			if (this.CurrentPageIndex != lastPageIndex) {
				//__doPostBack 함수 호출을 연결
				writer.AddAttribute("href", postBackScript);
			}
			else {
				//현재 페이지 인덱스가 0일 때

				//이용 불가 버튼 숨기기 옵션이 설정된 경우
				if (this.UnreachablePageBehavior == UnreachablePageBehaviors.Hide) {
					return;
				}
				else if (this.UnreachablePageBehavior == UnreachablePageBehaviors.ShowAlert) {
					//경고 메시지를 출력하는 스크립트 작성
					postBackScript = String.Format("javascript:alert('{0}')", Resources.Controls.PagerResource.CanNotGotoLast);
					writer.AddAttribute("href", postBackScript);
				}
			}

			//스타일 정보를 적용
			if (this.NavigateButtonStyle != null) {
				CssStyleCollection objCol = this.NavigateButtonStyle.GetStyleAttributes(this.Page);
				IEnumerator objKeys = objCol.Keys.GetEnumerator();

				while (objKeys.MoveNext()) {
					string key = (string)objKeys.Current;
					writer.AddStyleAttribute(key, objCol[key]);
				}
			}
			writer.RenderBeginTag(HtmlTextWriterTag.A);

			//이미지 경로가 지정된 경우 <IMG>태그 출력
			if (!String.IsNullOrEmpty(this.LastPageImageUrl)) {
				writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Page.ResolveUrl(this.LastPageImageUrl));
				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Align, "absmiddle");
				writer.RenderBeginTag("img");
				writer.RenderEndTag();
			}
			else {
				//이미지 경로가 없으면 텍스트 출력
				writer.Write(this.LastPageText);
			}
			writer.RenderEndTag();
		}
	}//end of class Pager
}//end of namespace Daniel.Web.UI.Controls