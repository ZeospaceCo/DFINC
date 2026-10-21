using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// 小 액션메뉴 버튼
    /// 클라이언트 화면상에서 데이터 Submit하지 않는 범위내의 액션에 대해 사용
    /// 사용자 측면에서 직관적으로 클릭해도 이상이 없는 정도의 액션에 사용한다
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:ClientButton width=100 height=10 runat=server></{0}:ClientButton>")]
    public class ClientButton : WebControl, IPostBackEventHandler, INamingContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientButton"/> class.
        /// </summary>
        public ClientButton()
            : base()
        {
            ForeColor = Color.Black;
            BackColor = Color.Silver;
            Width = 50;
            Height = 18;
        }

        #region 이벤트 헨들러

        /// <summary>
        /// Click
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        /// <summary>
        /// 클래스에서 구현될 때, 폼이 서버에 게시되면 발생되는 이벤트를 서버 컨트롤에서 처리할 수 있도록 합니다.
        /// </summary>
        /// <param name="eventArgument">이벤트 처리기에 전달되는 선택적 이벤트 인수를 나타내는 <see cref="T:System.String"/>입니다.</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            Page.Validate();
            OnClick(new EventArgs());
        }

        /// <summary>
        /// 지정된 <see cref="T:System.Web.UI.HtmlTextWriter" qualify="true"/>에 렌더링되어야 하는 HTML 특성 및 스타일을 추가합니다. 이 메서드는 주로 컨트롤 개발자에 의해 사용됩니다.
        /// </summary>
        /// <param name="writer">클라이언트에서 HTML 컨텐트를 렌더링하기 위한 출력 스트림을 나타내는 <see cref="T:System.Web.UI.HtmlTextWriter" qualify="true"/>입니다.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + Page.ClientScript.GetPostBackEventReference(this, ""));
        }

        #endregion 이벤트 헨들러

        #region Property

        /// <summary>
        /// 버튼의 길이, 픽셀단위로 입력
        /// </summary>
        public override Unit Width
        {
            get
            {
                object objWidth = this.ViewState["Width"];
                return ((objWidth == null) ? Unit.Pixel(50) : ((Unit)objWidth));
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// 버튼의 높이, 픽셀단위로 입력
        /// 추천값 : 18 pixel
        /// </summary>
        public override Unit Height
        {
            get
            {
                object objHeight = this.ViewState["Height"];
                return ((objHeight == null) ? Unit.Pixel(18) : ((Unit)objHeight));
            }
            set { ViewState["Height"] = value; }
        }

        /// <summary>
        /// 버튼을 click시 호출할 클라이언트 스크립트 함수명
        /// 입력하지 않으면 Server Control로 동작
        /// </summary>
        public string ScriptFunctionName
        {
            get { return Convert.ToString(ViewState["ScriptFunctionName"]); }
            set { ViewState["ScriptFunctionName"] = value; }
        }

        /// <summary>
        /// 서버사이트 이벤트 호출 전에 클라이언트 Confirm 메시지를 받을때 사용
        /// </summary>
        public string ConfirmMessage
        {
            get { return Convert.ToString(ViewState["ConfirmMessage"]); }
            set { ViewState["ConfirmMessage"] = value; }
        }

        /// <summary>
        /// onmouseover 이벤트가 발생할 경우 실행할 클라이언트 사이드 이벤트
        /// </summary>
        public string MouseOverClass
        {
            get { return Convert.ToString(ViewState["MouseOverClass"]); }
            set { ViewState["MouseOverClass"] = value; }
        }

        /// <summary>
        /// onmouseout 이벤트가 발생할 경우 실행할 클라이언트 사이드 이벤트
        /// </summary>
        public string MouseOutClass
        {
            get { return Convert.ToString(ViewState["MouseOutClass"]); }
            set { ViewState["MouseOutClass"] = value; }
        }

        /// <summary>
        /// 툴팁
        /// </summary>
        public string Title
        {
            get { return Convert.ToString(ViewState["Title"]); }
            set { ViewState["Title"] = value; }
        }

        /// <summary>
        /// 버튼에 보여질 Text
        /// </summary>
        public string Text
        {
            get { return Convert.ToString(ViewState["Text"]); }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// 풍선말
        /// </summary>
        public string ActivateToolTip
        {
            get { return Convert.ToString(ViewState["ActivateToolTip"]); }
            set { ViewState["ActivateToolTip"] = value; }
        }

        /// <summary>
        /// 풍선말 위치
        /// </summary>
        public string ActivateToolTipPos
        {
            get { return Convert.ToString(ViewState["ActivateToolTipPos"]); }
            set { ViewState["ActivateToolTipPos"] = value; }
        }

        /// <summary>
        /// 버튼에 보여질 Icon
        /// 웹경로로 참조 해야 함
        /// </summary>
        public string Icon
        {
            get
            {
                object objIcon = this.ViewState["Icon"];
                return ((objIcon == null) ? null : (string)objIcon);
            }
            set { ViewState["Icon"] = value; }
        }

        /// <summary>
        /// 비활성화 여부
        /// </summary>
        public string IsDisabled
        {
            get { return Convert.ToString(ViewState["IsDisabled"]); }
            set { ViewState["IsDisabled"] = value; }
        }

        /// <summary>
        /// 고정길이 여부 {false,true}
        /// </summary>
        public bool IsFixedWidth
        {
            get { return (ViewState["IsFixedWidth"] == null) ? false : Convert.ToBoolean(ViewState["IsFixedWidth"]); }
            set { ViewState["IsFixedWidth"] = value; }
        }

        #endregion Property

        /// <summary>
        /// Renders the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            output.WriteBeginTag("table");
            output.WriteAttribute("cellspacing", "0");
            output.WriteAttribute("cellpadding", "0");
            output.WriteAttribute("class", "button_main");
            output.WriteAttribute("border", "0");
            if (IsFixedWidth) output.WriteAttribute("width", this.Width.ToString());
            output.WriteAttribute("height", this.Height.ToString());
            output.Write(HtmlTextWriter.TagRightChar);

            output.WriteFullBeginTag("tr");
            output.WriteBeginTag("td");
            output.WriteAttribute("class", "button_left");
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEndTag("td");

            output.WriteBeginTag("td");
            if (this.IsDisabled == "1")
            {
                output.WriteAttribute("class", "button_disabled");
                output.Write("DISABLED");
            }
            else
            {
                output.WriteAttribute("class", "button_text");
            }

            if (this.ScriptFunctionName.Trim() != null && this.ScriptFunctionName.Trim() != "")
            {
                output.WriteAttribute("onclick", this.ScriptFunctionName);
            }
            else
            {
                if (this.ConfirmMessage.Trim() != null && this.ConfirmMessage.Trim() != "")
                {
                    string scriptStr = "javascript: if (confirm('" + ConfirmMessage + "')){";
                    scriptStr += Page.ClientScript.GetPostBackEventReference(this, "");
                    scriptStr += ";}else{return false;}";

                    output.WriteAttribute("onclick", scriptStr);
                }
                else
                {
                    output.WriteAttribute("onclick", Page.ClientScript.GetPostBackEventReference(this, ""));
                }
            }

            output.WriteAttribute("title", this.Title);
            output.WriteAttribute("valign", "bottom");
            output.Write(HtmlTextWriter.TagRightChar);

            if (this.Icon == null)
            {
                output.Write(this.Text);
            }
            else
            {
                output.WriteBeginTag("table");
                output.WriteAttribute("cellspacing", "0");
                output.WriteAttribute("cellpadding", "0");
                output.WriteAttribute("width", "100%");
                output.Write(HtmlTextWriter.TagRightChar);

                output.WriteFullBeginTag("tr");

                // Icon과 텍스트가 한 줄인 버튼
                output.WriteBeginTag("td");
                output.WriteAttribute("class", "PDBL");
                output.WriteAttribute("align", "center");
                output.WriteAttribute("style", "padding-right:5px");
                output.Write(HtmlTextWriter.TagRightChar);
                output.WriteBeginTag("img");
                output.WriteAttribute("src", this.Icon);
                output.Write(HtmlTextWriter.TagRightChar);
                output.WriteEndTag("td");

                output.WriteEndTag("tr");
                output.WriteEndTag("table");
            }

            output.WriteEndTag("td");
            output.WriteBeginTag("td");
            output.WriteAttribute("class", "button_right");
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEndTag("td");

            output.WriteEndTag("tr");

            output.WriteEndTag("table");
        }
    }
}