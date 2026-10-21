using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// 小 액션메뉴 버튼
    /// 클라이언트 화면상에서 데이터 Submit하지 않는 범위내의 액션에 대해 사용
    /// 사용자 측면에서 직관적으로 클릭해도 이상이 없는 정도의 액션에 사용한다
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:ClientAButton runat=server></{0}:ClientAButton>")]
    public class ClientSmartButtonOld : WebControl, IPostBackEventHandler, INamingContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAButton"/> class.
        /// </summary>
        public ClientSmartButtonOld()
            : base()
        {
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
        /// 서버사이트 이벤트 호출 전에 클라이언트 Confirm 메시지를 받을때 사용
        /// </summary>
        public string ConfirmMessage
        {
            get { return Convert.ToString(ViewState["ConfirmMessage"]); }
            set { ViewState["ConfirmMessage"] = value; }
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
        /// 비활성화 여부
        /// </summary>
        public string IsDisabled
        {
            get { return Convert.ToString(ViewState["IsDisabled"]); }
            set { ViewState["IsDisabled"] = value; }
        }

        /// <summary>
        /// a 태그 첫끝에 추가할 태그
        /// </summary>
        public string BeginEndTag
        {
            get { return Convert.ToString(ViewState["BeginEndTag"]); }
            set { ViewState["BeginEndTag"] = value; }
        }

        /// <summary>
        /// BeginEndTag 의 Class
        /// </summary>
        public string BeginEndTagClass
        {
            get { return Convert.ToString(ViewState["BeginEndTagClass"]); }
            set { ViewState["BeginEndTagClass"] = value; }
        }

        /// <summary>
        /// 버튼 스타일
        /// </summary>
        public string Class
        {
            get { return Convert.ToString(ViewState["Class"]); }
            set { ViewState["Class"] = value; }
        }

        /// <summary>
        /// 이미지 경로
        /// </summary>
        public string ImagePath
        {
            get { return Convert.ToString(ViewState["ImagePath"]); }
            set { ViewState["ImagePath"] = value; }
        }

        #endregion Property

        /// <summary>
        /// Renders the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            if (this.IsDisabled == "1")
            {
                output.Write("<span>" + this.Text + "</span>");
            }
            else
            {
                if (BeginEndTag.Length > 0 && !string.IsNullOrEmpty(BeginEndTag))
                {
                    output.WriteBeginTag(BeginEndTag);
                    if (BeginEndTagClass.Length > 0 && !string.IsNullOrEmpty(BeginEndTagClass))
                    {
                        output.WriteAttribute("class", BeginEndTagClass);
                    }
                    output.Write(HtmlTextWriter.TagRightChar);
                }

                output.WriteBeginTag("a");

                if (this.ConfirmMessage.Trim() != null && this.ConfirmMessage.Trim() != "")
                {
                    string scriptStr = "javascript: if (confirm('" + ConfirmMessage + "')){";
                    scriptStr += Page.ClientScript.GetPostBackEventReference(this, "");
                    scriptStr += ";}else{return false;}";

                    output.WriteAttribute("href", "#");
                    output.WriteAttribute("onclick", scriptStr);
                }
                else
                {
                    output.WriteAttribute("href", "#");
                    //output.WriteAttribute("onclick", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, ""));
                }

                if (!String.IsNullOrEmpty(Class) && Class != "NOPTAG")
                {
                    output.WriteAttribute("class", Class);
                }

                output.WriteAttribute("id", this.ID);

                output.WriteAttribute("title", this.Text);
                output.Write(HtmlTextWriter.TagRightChar);

                if (!String.IsNullOrEmpty(ImagePath))
                {
                    output.Write("<img src=\"" + ImagePath + "\" />");
                }
                else
                {
                    if (!String.IsNullOrEmpty(Class))
                    {
                        output.Write(this.Text);
                    }
                    else
                    {
                        output.Write("<span>" + this.Text + "</span>");
                    }
                }
                output.WriteEndTag("a");

                if (BeginEndTag.Length > 0) output.WriteEndTag(BeginEndTag);
            }
        }
    }
}