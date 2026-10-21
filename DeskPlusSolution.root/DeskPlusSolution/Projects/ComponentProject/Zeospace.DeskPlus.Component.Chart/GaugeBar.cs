using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace Zeospace.DeskPlus.Component.Chart
{
    /// <summary>
    /// GaugeBar
    /// </summary>
    [DefaultProperty("Value"),
    ToolboxData("<{0}:GaugeBar width=100 height=15 runat=server></{0}:GaugeBar>")]
    public class GaugeBar : WebControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeBar"/> class.
        /// </summary>
        public GaugeBar()
            : base()
        {
            ForeColor = Color.Orange;
            BackColor = Color.LightYellow;
            Width = 100;
            Height = 10;
            Maximum = 100;
            Value = 0;
            Segments = 4;
            //FormatString = "<b><font color=\"#737373\">{0}</font></b><font color=\"#333333\">/{1}</font>";
            FormatString = "<b><font color=\"#737373\">{0} %</font></b>";
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>The image path.</value>
        public string ImgPath
        {
            get { return Convert.ToString(ViewState["ImgPath"]); }
            set { ViewState["ImgPath"] = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value
        {
            get { return (float)ViewState["Value"]; }
            set { ViewState["Value"] = (value > Maximum ? Maximum : value); }
        }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public float Maximum
        {
            get { return (float)ViewState["Maximum"]; }
            set { ViewState["Maximum"] = value; }
        }

        /// <summary>
        /// Gets or sets the segments.
        /// </summary>
        /// <value>The segments.</value>
        public int Segments
        {
            get { return (int)ViewState["Segments"]; }
            set { ViewState["Segments"] = (value < 1 ? 1 : value); }
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString
        {
            get { return Convert.ToString(ViewState["FormatString"]); }
            set { ViewState["FormatString"] = value; }
        }

        /// <summary>
        /// 참여자 수
        /// </summary>
        public string ParticipantCnt
        {
            get { return Convert.ToString(ViewState["ParticipantCnt"]); }
            set { ViewState["ParticipantCnt"] = value; }
        }

        /// <summary>
        /// 참여자 수 타이틀
        /// </summary>
        public string ParticipantTitle
        {
            get { return Convert.ToString(ViewState["ParticipantTitle"]); }
            set { ViewState["ParticipantTitle"] = value; }
        }

        /// <summary>
        /// 참여자 수의 단위 (ex. ...명)
        /// </summary>
        public string ParticipantUnit
        {
            get { return Convert.ToString(ViewState["ParticipantUnit"]); }
            set { ViewState["ParticipantUnit"] = value; }
        }


        /// <summary>
        /// Renders the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            float valueToRepresent = 100f * Value / Maximum;

            // Begin the table
            output.RenderBeginTag("table");
            output.RenderBeginTag("tr");
            output.RenderBeginTag("td");


            // 막대 Chart 그리기 시작
            output.WriteBeginTag("table");
            output.WriteAttribute("border", "0");
            output.WriteAttribute("cellspacing", "0");
            output.WriteAttribute("cellpadding", "0");
            output.WriteAttribute("width", Width.ToString());
            output.WriteAttribute("height", Height.ToString());
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteFullBeginTag("tr");

            // 왼쪽 시작 선
            output.WriteBeginTag("td");
            output.WriteAttribute("width", "1");
            output.Write(HtmlTextWriter.TagRightChar);

            output.WriteBeginTag("img");
            output.WriteAttribute("src", ImgPath + "statebar_line.gif");
            output.WriteAttribute("width", "1");
            output.WriteAttribute("height", Height.ToString());
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEndTag("td");
            // 왼쪽 시작 선

            // TD 그리기
            DrawRuler(output, valueToRepresent);

            // 오른쪽 선
            output.WriteBeginTag("td");
            output.WriteAttribute("width", "1");
            output.Write(HtmlTextWriter.TagRightChar);

            output.WriteBeginTag("img");
            output.WriteAttribute("src", ImgPath + "statebar_line.gif");
            output.WriteAttribute("width", "1");
            output.WriteAttribute("height", Height.ToString());
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEndTag("td");
            // 오른쪽 선

            output.WriteEndTag("tr");
            output.WriteEndTag("table");
            // 막대 Chart 그리기 끝

            output.WriteEndTag("td");


            output.RenderBeginTag("td");

            output.Write(FormatString, Value);

            // (참여자수 : xx 명)
            //수정 : shimpark
            string partCntStr = this.ParticipantTitle + " " + this.ParticipantCnt + " " + this.ParticipantUnit;
            if (partCntStr.Trim().Length > 0) partCntStr = " (" + partCntStr + ")";

            output.Write(partCntStr);

            //			output.WriteBeginTag("font");
            //			output.WriteAttribute("color", "#737373");
            //			output.Write(HtmlTextWriter.TagRightChar);
            //			output.WriteBeginTag("b");
            //			output.Write(HtmlTextWriter.TagRightChar);
            //			output.Write(Value);
            //			output.WriteEndTag("b");
            //			output.WriteEndTag("font");
            //
            //
            //			output.WriteBeginTag("font");
            //			output.WriteAttribute("color", "#333333");
            //			output.Write(HtmlTextWriter.TagRightChar);
            //			output.Write("/" + Maximum);
            //			output.WriteEndTag("font");

            //output.Write(FormatString, Value, Maximum);
            output.WriteEndTag("td");

            output.WriteEndTag("tr");
            output.WriteEndTag("table");
        }

        void DrawRuler(HtmlTextWriter output, float valueToRepresent)
        {
            int SegmentWidth = 100 / Segments;
            bool finished = false;

            for (int i = 1; i <= Segments; i++)
            {
                if (valueToRepresent < i * SegmentWidth)
                {
                    if (finished)
                    {

                        output.WriteBeginTag("td");
                        output.WriteAttribute("background", ImgPath + "statebar_bg_normal.gif");
                        output.WriteAttribute("width", SegmentWidth.ToString() + "%");
                        output.Write(HtmlTextWriter.TagRightChar);
                        output.WriteEndTag("td");
                    }
                    else
                    {

                        output.WriteBeginTag("td");
                        output.WriteAttribute("width", SegmentWidth.ToString() + "%");
                        output.Write(HtmlTextWriter.TagRightChar);

                        output.WriteBeginTag("table");
                        output.WriteAttribute("border", "0");
                        output.WriteAttribute("cellspacing", "0");
                        output.WriteAttribute("cellpadding", "0");
                        output.WriteAttribute("width", "100%");
                        output.WriteAttribute("height", Height.ToString());
                        output.Write(HtmlTextWriter.TagRightChar);
                        output.WriteFullBeginTag("tr");

                        float fx = (100 * (valueToRepresent - SegmentWidth * (i - 1)) / SegmentWidth);
                        if (valueToRepresent > (i - 1) * SegmentWidth)
                        {
                            // 10:25=x:100

                            output.WriteBeginTag("td");
                            output.WriteAttribute("background", ImgPath + "statebar_bg_choice.gif");
                            output.WriteAttribute("width", fx.ToString() + "%");
                            output.Write(HtmlTextWriter.TagRightChar);
                            output.WriteEndTag("td");
                        }
                        output.WriteBeginTag("td");
                        output.WriteAttribute("background", ImgPath + "statebar_bg_normal.gif");
                        output.WriteAttribute("width", (100 - fx).ToString() + "%");
                        output.Write(HtmlTextWriter.TagRightChar);
                        output.WriteEndTag("td");

                        output.WriteEndTag("tr");
                        output.WriteEndTag("table");
                        output.WriteEndTag("td");
                        finished = true;
                    }
                }
                else
                {
                    output.WriteBeginTag("td");
                    output.WriteAttribute("background", ImgPath + "statebar_bg_choice.gif");
                    output.WriteAttribute("width", SegmentWidth.ToString() + "%");
                    output.Write(HtmlTextWriter.TagRightChar);
                    output.WriteEndTag("td");
                }

                // 구분 선
                output.WriteBeginTag("td");
                output.WriteAttribute("width", "1");
                output.Write(HtmlTextWriter.TagRightChar);
                output.WriteBeginTag("img");
                output.WriteAttribute("src", ImgPath + "statebar_line2.gif");
                output.WriteAttribute("width", "1");
                output.WriteAttribute("height", Height.ToString());
                output.Write(HtmlTextWriter.TagRightChar);
                output.WriteEndTag("td");

                // 구분 선
            }
        }
    }
}
