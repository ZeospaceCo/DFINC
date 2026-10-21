using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Component.Repeater.UI.Controls.RepeaterExControl
{
    /// <summary>
	/// DataSource가 비어있는 경우 사용될 EmptyDataTemplate 템플릿이 추가된 Repeater 컨트롤입니다.
	/// </summary>
	[ToolboxDataAttribute("<{0}:RepeaterEx runat=\"server\"></{0}:RepeaterEx>")]
	[DesignerAttribute("System.Web.UI.Design.WebControls.RepeaterDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class RepeaterEx : System.Web.UI.WebControls.Repeater {
		/// <summary>
		/// 목록에 표시된 각 항목에 대해 지정된 템플릿을 반복 사용하여 레이아웃을 사용자 지정할 수 있는 데이터 바인딩된 List 컨트롤입니다
		/// </summary>
		private ITemplate _emptyDataTemplate;

		/// <summary>
		/// RepeaterEx 클래스를 초기화 합니다.
		/// </summary>
		public RepeaterEx()
			: base() {
		}

		/// <summary>
		/// <see cref="RepeaterEx">Repeater</see> 컨트롤에서 빈 항목을 표시하는 방법을 정의하는 <see cref="System.Web.UI.ITemplate">System.Web.UI.ITemplate</see>을 가져오거나 설정합니다. 
		/// </summary>
		[PersistenceModeAttribute(PersistenceMode.InnerProperty)]
		[BrowsableAttribute(false)]
		[TemplateContainer(typeof(RepeaterItem))]
		public virtual ITemplate EmptyDataTemplate {
			get { return this._emptyDataTemplate; }
			set { this._emptyDataTemplate = value; }
		}

		/// <summary>
		/// DataBinding 이벤트를 발생시킵니다.
		/// </summary>
		/// <param name="e">이벤트 데이터가 들어있는 <see cref="EventArgs">EventArgs</see>입니다.</param>
		protected override void OnDataBinding(EventArgs e) {
			base.OnDataBinding(e);
			if (this.Items.Count == 0) {
				this.Controls.Clear();

				if (this.HeaderTemplate != null) {
					this.HeaderTemplate.InstantiateIn(this);
				}

				if (this.EmptyDataTemplate != null) {
					this.EmptyDataTemplate.InstantiateIn(this);
				}

				if (this.FooterTemplate != null) {
					this.FooterTemplate.InstantiateIn(this);
				}
			}
		}
	}//end of class RepeaterEx
}//end of namespace Daniel.Web.UI.Controls
