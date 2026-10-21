using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// ClientTextBox
    /// </summary>
    [
        DefaultProperty("Title"),
        ToolboxData("<{0}:ClientTextBox runat=server></{0}:ClientTextBox>")
    ]
    public class ClientTextBox : System.Web.UI.WebControls.TextBox
    {
        #region 속성분류

        /// <summary>
        /// EnumInputMode
        /// </summary>
        public enum EnumInputMode
        {
            /// <summary>
            /// 기본
            /// </summary>
            None,

            /// <summary>
            ///  엔터쳤을때 함수 호출 textbox
            /// </summary>
            Default,

            /// <summary>
            ///
            /// </summary>
            NumericComma,   // 숫자만 입력, 3자리마다 컴마,  엔터쳤을때 함수 호출 textbox

            /// <summary>
            ///
            /// </summary>
            Numeric          // 숫자만 입력
        }

        /// <summary>
        ///
        /// </summary>
        public enum EnumAlign
        {
            /// <summary>
            ///
            /// </summary>
            Left,

            /// <summary>
            ///
            /// </summary>
            Right,

            /// <summary>
            ///
            /// </summary>
            Center
        }

        #endregion 속성분류

        #region 프로퍼티

        /// <summary>
        /// 타이틀
        /// </summary>
        private string _title = "";

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("타이틀을 입력합니다")]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        /// <summary>
        /// 타이틀 너비
        /// </summary>
        private string _titleWidth = "130";

        /// <summary>
        /// Gets or sets the width of the title.
        /// </summary>
        /// <value>The width of the title.</value>
        [Description("타이틀의 너비를 지정합니다")]
        public string TitleWidth
        {
            get
            {
                return _titleWidth;
            }
            set
            {
                _titleWidth = value;
            }
        }

        /// <summary>
        /// 타이틀 정렬
        /// </summary>
        private EnumAlign _titleAlign;

        /// <summary>
        /// Gets or sets the title align.
        /// </summary>
        /// <value>The title align.</value>
        [Description("타이틀의 정렬방식을 지정합니다")]
        public EnumAlign TitleAlign
        {
            get
            {
                return _titleAlign;
            }
            set
            {
                _titleAlign = value;
            }
        }

        /// <summary>
        /// 타이틀 정렬
        /// </summary>
        private EnumAlign _textAlign;

        /// <summary>
        /// Gets or sets the text align.
        /// </summary>
        /// <value>The text align.</value>
        [Description("텍스트의 정렬방식을 지정합니다")]
        public EnumAlign TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            {
                _textAlign = value;
            }
        }

        private string _scriptSubmitFunName = "None()";

        /// <summary>
        /// Gets or sets the width of the title.
        /// </summary>
        /// <value>The width of the title.</value>
        [Description("submit 의 js 함수 호출")]
        public string ScriptSubmitFunName
        {
            get
            {
                return _scriptSubmitFunName;
            }
            set
            {
                _scriptSubmitFunName = value;
            }
        }

        /// <summary>
        /// 필수항목 여부
        /// </summary>
        private bool _required;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ClientTextBox"/> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        [Description("필수항목 여부를 지정합니다. Attribute 만 생성하므로 필수항목 체크는 별도의 코딩작업이 필요합니다")]
        public bool Required
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
            }
        }

        /// <summary>
        /// 일반/ 숫자/ 컴마가 찍히는 숫자 프로퍼티
        /// </summary>
        private EnumInputMode _inputMode = EnumInputMode.None;

        /// <summary>
        /// Gets or sets the input mode.
        /// </summary>
        /// <value>The input mode.</value>
        [Description("[일반],[컴마가들어간숫자],[숫자] 중 선택합니다")]
        public EnumInputMode InputMode
        {
            get
            {
                return _inputMode;
            }
            set
            {
                _inputMode = value;
            }
        }

        #endregion 프로퍼티

        #region Rendering

        /// <summary>
        /// 	<see cref="T:System.Web.UI.WebControls.TextBox"/> 컨트롤을 지정된 <see cref="T:System.Web.UI.HtmlTextWriter"/> 개체에 렌더링합니다.
        /// </summary>
        /// <param name="writer">렌더링된 출력을 받는 <see cref="T:System.Web.UI.HtmlTextWriter"/>입니다.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// 지정된 <see cref="T:System.Web.UI.HtmlTextWriter"/> 인스턴스에 렌더링되어야 하는 HTML 특성 및 스타일을 추가합니다.
        /// </summary>
        /// <param name="writer">클라이언트에서 HTML 콘텐츠를 렌더링하는 데 사용되는 출력 스트림을 나타내는 <see cref="T:System.Web.UI.HtmlTextWriter"/>입니다.</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            // 필수항목 구분 사용자 Attribute 추가
            writer.AddAttribute("Required", Required.ToString());
            writer.AddStyleAttribute("vertical-align", "middle");

            // CssClass 정의
            this.CssClass = "DefaultTextBox";

            // 타이틀이 존재하면 타이틀 부분을 만들어 준다
            if (Title != "")
            {
                writer.Write("<span style=height:100%;width:" + TitleWidth + ";text-align:" + TitleAlign + " class='TitleTextBoxTitle'>" + Title + "</span>&nbsp;");
            }

            if (this.TextMode == TextBoxMode.MultiLine)
            {
                this.CssClass = "DefaultMultiLineTextBox";
            }
            else
            {
                // 필수항목이라면 배경에 필수항목 그림을 넣는다
                if (Required)
                {
                    this.CssClass = "RequiredTextBox";
                }
                else if (this.ReadOnly) // 읽기 전용 이라면 배경색을 바꾸어준다
                {
                    this.CssClass = "ReadOnlyTextBox";
                }
            }

            writer.AddAttribute("class", CssClass);

            // InputMode 에 따라서 세가지로 나뉜다
            // 1. None : 기본 textbox
            // 2. Default : 엔터쳤을때 함수 호출 textbox
            // 3. Numeric : 숫자만 입력가능, 엔터쳤을때 함수 호출 textbox
            // 4. Numeric_comma : 숫자만 입력가능하고, 입력시 3자리 마다 컴마가 찍힌다
            switch (InputMode)
            {
                case EnumInputMode.None:
                    writer.AddAttribute("onBlur", "DeskPlusEIP.Component.TextBox.f_onTrim()");
                    break;

                case EnumInputMode.Default:
                    writer.AddAttribute("onkeydown", "DeskPlusEIP.Component.TextBox.f_key()");
                    writer.AddAttribute("onBlur", "DeskPlusEIP.Component.TextBox.f_onTrim()");
                    break;

                case EnumInputMode.NumericComma:
                    //if (this.Text.Trim() == "") writer.AddAttribute("value", "0");
                    writer.AddStyleAttribute("ime-mode", "disabled");
                    writer.AddStyleAttribute("text-align", TextAlign.ToString());
                    writer.AddAttribute("onkeydown", "DeskPlusEIP.Component.TextBox.f_keyIsNum()");
                    writer.AddAttribute("onkeyup", "DeskPlusEIP.Component.TextBox.f_onKeyManage()");
                    writer.AddAttribute("onBlur", "DeskPlusEIP.Component.TextBox.f_setZero()");
                    writer.AddAttribute("onFocus", "DeskPlusEIP.Component.TextBox.f_onFocusManage()");
                    break;

                case EnumInputMode.Numeric:
                    //if (this.Text.Trim() == "") writer.AddAttribute("value", "0");
                    writer.AddStyleAttribute("ime-mode", "disabled");
                    writer.AddStyleAttribute("text-align", TextAlign.ToString());
                    writer.AddAttribute("onkeydown", "DeskPlusEIP.Component.TextBox.f_keyIsNum()");
                    writer.AddAttribute("onBlur", "DeskPlusEIP.Component.TextBox.f_setZero()");
                    writer.AddAttribute("onFocus", "DeskPlusEIP.Component.TextBox.f_onFocusManage()");
                    break;
            }

            //엔터키로 함수 호츨하는 input 박스 일경우, onkeypress 함수를 사용한다.
            if (ScriptSubmitFunName != "None()")
            {
                //엔터로 함수를 호출 할경우, postback 발생때문에 꽁수!!
                writer.AddAttribute("onkeypress", "return checkAdd(event,this.form)");
            }
        }

        /// <summary>
        /// 	<see cref="P:System.Web.UI.WebControls.TextBox.AutoPostBack"/>이 true인 경우 클라이언트에서 렌더링하기 전에 포스트백 이벤트를 생성하기 위한 클라이언트 스크립트를 등록합니다.
        /// </summary>
        /// <param name="e">이벤트 데이터가 들어 있는 <see cref="T:System.EventArgs"/>입니다.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("titleTextBoxScript"))
            {
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script type='text/javascript' src='/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js'></script>");

                string keyCheck = " <script language='vbscript'>Sub keyCheck() If window.event.KeyCode = 13 Then {0} End If End Sub </script>";
                string onkeyPress = " <script language='javascript'> function checkAdd(evt,theForm){ if(evt.keyCode == 13) return false;} function None(){} </script>";

                keyCheck = string.Format(keyCheck, ScriptSubmitFunName);

                //엔터키로 함수 호츨하는 input 박스 일경우, onkeypress 함수를 사용한다.
                keyCheck = (ScriptSubmitFunName != "None()") ? keyCheck + onkeyPress : keyCheck;

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "titleTextBoxScript", strScript.ToString() + keyCheck);
            }
        }

        #endregion Rendering
    }
}