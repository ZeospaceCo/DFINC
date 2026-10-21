using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Component.Grids
{

    /// <summary>
    /// 체크박스를 가지는 템플릿 컬럼
    /// </summary>
    public class ClientCheckedBoxColumnTemplate : ITemplate
    {
        /// <summary>
        /// 클래스에서 구현될 때, 자식 컨트롤 및 템플릿이 속해 있는 <see cref="T:System.Web.UI.Control"/> 개체를 정의합니다. 그러면 이 자식 컨트롤이 인라인 템플릿에 정의됩니다.
        /// </summary>
        /// <param name="container">인라인 템플릿에서 인스턴스화된 컨트롤이 들어 있는 <see langword="Control"/> 개체입니다.</param>
        public void InstantiateIn(Control container)
        {
            System.Web.UI.HtmlControls.HtmlInputCheckBox cb = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
            container.Controls.Add(cb);
        }
    }


    /// <summary>
    /// ServerCheckedBoxColumnTemplate 클래스
    /// </summary>
    public class ServerCheckedBoxColumnTemplate : ITemplate
    {
        /// <summary>
        /// 클래스에서 구현될 때, 자식 컨트롤 및 템플릿이 속해 있는 <see cref="T:System.Web.UI.Control"/> 개체를 정의합니다. 그러면 이 자식 컨트롤이 인라인 템플릿에 정의됩니다.
        /// </summary>
        /// <param name="container">인라인 템플릿에서 인스턴스화된 컨트롤이 들어 있는 <see langword="Control"/> 개체입니다.</param>
        public void InstantiateIn(Control container)
        {
            CheckBox cb = new CheckBox();
            container.Controls.Add(cb);
        }
    }



    /// <summary>
    /// MultiSelectGrid 클래스
    /// </summary>
    public class MultiSelectGrid : DataGrid
    {
        private System.Web.UI.WebControls.ImageButton imgBtn;

        #region  전역변수
        /// <summary>
        /// 
        /// </summary>
        public String PagerCurrentPageCssClass = "";

        /// <summary>
        /// 
        /// </summary>
        public String PagerLinksCssClass = "";

        /// <summary>
        /// 
        /// </summary>
        public bool AllowMultiSelect = false;				// 멀티 선택

        //private bool ExistItem = false; // 아이템 존재 유무 (아이템이 존재하지 않으면 페이징을 않보이게 함)
        #endregion

        #region 생성자

        /// <summary>
        /// MultiSelectGrid 생성자
        /// </summary>
        public MultiSelectGrid()
        {
            // 페이지 스타일
            this.CssClass = "grid_typeA";
            HeaderStyle.CssClass = "grid_thTR";
            PagerStyle.CssClass = "grid_pager";
            //ItemStyle.CssClass = "grid_Item";
            //AlternatingItemStyle.CssClass = "grid_altItem";
            //SelectedItemStyle.CssClass = "grid_selItem";
            //FooterStyle.CssClass = "grid_footer";
            this.GridLines = GridLines.None;
            ShowFooter = true;

            //BorderWidth = Unit.Pixel(0);
            PagerStyle.Mode = PagerMode.NumericPages;
            PagerStyle.PageButtonCount = 15;
            PagerStyle.HorizontalAlign = HorizontalAlign.Center;

            // 페이징 설정
            AllowPaging = true;			// 페이지 기본 허용
            PageSize = 15;				// 페이지 사이즈 

            // 정렬 설정
            AllowSorting = true;
            ViewState["SortedAscending"] = "yes";

            // 필터
            ViewState["RowSelectFilter"] = "";
            //Style["filter"] = "progid:DXImageTransform.Microsoft.dropshadow(OffX=2, OffY=2, Color='gray', Positive='true')";

            // 이벤트 헨들러
            Init += new EventHandler(OnInit);
            this.ItemDataBound += new DataGridItemEventHandler(OnItemDataBound);
            ItemCreated += new DataGridItemEventHandler(OnItemCreated);
            SortCommand += new DataGridSortCommandEventHandler(OnSortCommand);
            PageIndexChanged += new DataGridPageChangedEventHandler(OnPageIndexChanged);
            this.PreRender += new EventHandler(MultiSelectGrid_PreRender);
        }
        #endregion

        #region 이벤트

        /// <summary>
        /// EventHandler
        /// </summary>
        public event EventHandler UpdateView;

        /// <summary>
        /// 데이터 다시 로드
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdateView(EventArgs e)
        {
            if (UpdateView != null)
                UpdateView(this, e);
        }

        private void MultiSelectGrid_PreRender(object sender, System.EventArgs e)
        {
            //javascirpt 호출하여 textbox 에 넣기
            //체크한 박스 ID 가져오기 (row 구분)  (필드구분)

            if (this.IsClientCheckBox != null && IsClientCheckBox != "")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<SCRIPT language=\"javascript\">\n");
                sb.Append("function SelectedItems(){");
                sb.Append("var msgrid = " + this.ClientID + ".all.tags(\"INPUT\");");
                sb.Append("var chkList = \"\";var tmpID = \"\";var idx = \"\";var fileName = \"\";");
                sb.Append("for (i=0; i<msgrid.length; i++){if (msgrid[i].type.toLowerCase() == \"checkbox\"){if (msgrid[i].checked){tmpID = msgrid[i].value;");
                sb.Append("chkList += tmpID + \"\";}}}");
                sb.Append("if (chkList.length > 0)chkList.substring(0, chkList.length -1);return chkList;}");

                sb.Append("function SelectedItemsByID(mgID){");
                sb.Append("var msgrid = document.all(mgID).all.tags(\"INPUT\");");
                sb.Append("var chkList = \"\";var tmpID = \"\";var idx = \"\";var fileName = \"\";");
                sb.Append("for (i=0; i<msgrid.length; i++){if (msgrid[i].type.toLowerCase() == \"checkbox\"){if (msgrid[i].checked){tmpID = msgrid[i].value;");
                sb.Append("chkList += tmpID + \"\";}}}");
                sb.Append("if (chkList.length > 0)chkList.substring(0, chkList.length -1);return chkList;}");

                sb.Append("</SCRIPT>\n");

                if (!Page.ClientScript.IsClientScriptBlockRegistered("MultiSelectGridScript")) Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MultiSelectGridScript", sb.ToString());

                StringBuilder sbChecked = new StringBuilder();
                sbChecked.Append("<SCRIPT language=\"javascript\">\n");
                sbChecked.Append("function " + this.ClientID + "_DpMsgUnChecked(){");
                sbChecked.Append("var msgrid = " + this.ClientID + ".all.tags(\"INPUT\");");
                sbChecked.Append("for (i=0; i<msgrid.length; i++){if (msgrid[i].type.toLowerCase() == \"checkbox\"){msgrid[i].checked = !msgrid[i].checked;}}}");
                sbChecked.Append("</SCRIPT>\n");

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.ClientID + "_MultiSelectGridScript")) Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.ClientID + "_MultiSelectGridScript", sbChecked.ToString());
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered("DpMsgCssScript")) Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "DpMsgScript", "<SCRIPT>if (document.all(\"" + this.ClientID + "\"))document.all(\"" + this.ClientID + "\").style.borderCollapse = \"separate\";</SCRIPT>");

        }

        /// <summary>
        /// Init 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>		
        public void OnInit(Object sender, EventArgs e)
        {
            if (AllowMultiSelect)			// 멀티 선택시
                AddSelectColumn();

            if (SortExpression != null && SortExpression != "")
            {
                ProcessSortExpression(SortExpression);
                PrepareSortExpression();
            }
        }


        /// <summary>
        /// 전체 취소 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnUnselect(Object sender, ImageClickEventArgs e)
        {
            if (ViewState["CheckedAll"] == null || ViewState["CheckedAll"].ToString() == "")
            {
                foreach (DataGridItem dgi in Items)
                {
                    CheckBox cb = (CheckBox)dgi.Cells[0].Controls[0];
                    cb.Checked = true;
                }

                ViewState["CheckedAll"] = "1";

                if (ImgPath != "")
                {
                    imgBtn.ImageUrl = ImgPath + "/Component/Grid/grid_cb_true.gif";
                }
                else
                {
                    imgBtn.ImageUrl = "/DeskPlusEIP/Design/C/Img/Component/Grid/grid_cb_true.gif";
                }

            }
            else
            {
                foreach (DataGridItem dgi in Items)
                {
                    CheckBox cb = (CheckBox)dgi.Cells[0].Controls[0];
                    cb.Checked = false;
                }

                ViewState["CheckedAll"] = "";

                if (ImgPath != "")
                {
                    imgBtn.ImageUrl = ImgPath + "/Component/Grid/grid_cb_false.gif";
                }
                else
                {
                    imgBtn.ImageUrl = "/DeskPlusEIP/Design/C/Img/Component/Grid/grid_cb_false.gif";
                }
            }

        }

        /// <summary>
        /// Called when [item data bound].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        public void OnItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;
            if (AllowMultiSelect)					// 멀티선택시  전체 선택/취소 체크박스 이미지 버튼 넣기
            {

                if (this.IsClientCheckBox != null && IsClientCheckBox != "")
                {

                    if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)	// 아이템/반전아이템
                    {
                        string items = "";

                        for (int i = 1; i < Columns.Count; i++)
                        {
                            items += e.Item.Cells[i].Text + "";
                        }

                        items.TrimEnd('');

                        ((System.Web.UI.HtmlControls.HtmlInputCheckBox)e.Item.Cells[0].Controls[0]).Value = items;
                    }
                }
            }


            //if (itemType == ListItemType.Footer)
            //{
            //    e.Item.Cells[0].Controls.Add(new LiteralControl("<span class='NoListData'></span>"));
            //}


            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)	// 아이템/반전아이템
            {
                if (IsCustomViewLine == null || IsCustomViewLine == "")
                {
                    string rowLine = String.Empty;
                    int colSpanCount = 0;
                    int viewLineColumnCount = 0;

                    if (ViewLineColumnCount == null || ViewLineColumnCount == "")
                    {
                        viewLineColumnCount = Columns.Count - 1;
                    }
                    else
                    {

                        viewLineColumnCount = Convert.ToInt32(ViewLineColumnCount) - 1; // 배열이여서 1을 빼준다
                    }

                    if (ViewLineColumnColSpan == null || ViewLineColumnColSpan == "")
                    {
                        for (int i = 0; i < Columns.Count - 1; i++)
                        {
                            e.Item.Cells[i].CssClass = "grid_tdTD";
                            if (e.Item.Cells[i].Visible) colSpanCount += 1;
                        }
                    }
                    else
                    {
                        colSpanCount = Convert.ToInt32(ViewLineColumnColSpan);
                    }



                    //rowLine = "</td></tr><tr><td colspan=\"" + colSpanCount + "\" class=\"grid_tdTD\" align=\"center\" valign=\"middle\">";

                    //rowLine = e.Item.Cells[viewLineColumnCount].Text + rowLine;
                    //e.Item.Cells[viewLineColumnCount].Controls.Add(new LiteralControl(rowLine));
                }



                //ExistItem = true;
            }
        }

        /// <summary>
        /// 데이터 Create 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>	
        public void OnItemCreated(Object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;

            string rowLine = String.Empty;
            //int colSpanCount = 0;
            //int viewLineColumnCount = 0;

            if (itemType == ListItemType.Header)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    e.Item.Cells[i].CssClass = "grid_thTD";
                    e.Item.Cells[i].Wrap = false;
                }
            }

            #region Item/AlterantingItem
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)	// 아이템/반전아이템
            {
                for (int i = 0; i < Columns.Count; i++)		// 정렬 시 현재 정렬 형식(asc/desc)형태를 보여주는 삼각형 표
                {
                    e.Item.Cells[i].CssClass = "grid_tdTD";
                    TableCell cell = e.Item.Cells[i];
                    if (e.Item.Cells[i].Width.ToString() == "100%" || e.Item.Cells[i].Width.ToString() == "")
                    {
                        //e.Item.Cells[i].Wrap = false;
                    }

                    if (e.Item.Cells[i].Controls.Count > 0)
                    {
                        if (e.Item.Cells[i].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DataGridLinkButton")
                        {
                            ((LinkButton)e.Item.Cells[i].Controls[0]).CssClass = "ALinkByHTC";
                        }
                    }
                }
            }


            #endregion


            #region pager
            if (itemType == ListItemType.Pager)												// 페이징
            {
                TableCell pager = (TableCell)e.Item.Controls[0];
                //if (ExistItem)
                //{
                pager.Visible = true;
                pager.CssClass = "grid_pagerTD";

                for (int i = 0; i < pager.Controls.Count; i += 2)
                {
                    Object o = pager.Controls[i];
                    if (o is LinkButton)
                    {
                        LinkButton h = (LinkButton)o;
                        h.Text = "[" + h.Text + "]";
                        h.CssClass = "grid_thTDText";
                    }
                    else
                    {
                        Label l = (Label)o;
                        l.Text = "<b>" + l.Text + "</b>";
                        l.CssClass = "grid_thTDText";
                    }
                }
                Literal lt = new Literal();
                lt.Text = "&nbsp;&nbsp;&nbsp;";	// 페이지 디자인
                pager.Controls.AddAt(pager.Controls.Count, lt);
                //}
                //else
                //{
                //    pager.Visible = false;

                //}
            }
            #endregion


            #region Header
            if (itemType == ListItemType.Header)										// 헤더 만들기
            {
                int j = 0;
                if (AllowMultiSelect)					// 멀티선택시  전체 선택/취소 체크박스 이미지 버튼 넣기
                {
                    if (IsClientCheckBox != null && IsClientCheckBox != "")
                    {
                        System.Web.UI.HtmlControls.HtmlInputCheckBox cb = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                        cb.Attributes.Add("onClick", this.ClientID + "_DpMsgUnChecked()");
                        e.Item.Cells[0].Controls.Add(cb);
                        e.Item.Cells[0].Attributes.Add("align", "center");
                        e.Item.Cells[0].ControlStyle.Width = Unit.Pixel(25);
                        e.Item.Cells[0].CssClass = "grid_thTR";

                        j = 1;
                    }
                    else
                    {
                        imgBtn = new ImageButton();

                        if (ViewState["CheckedAll"] == null || ViewState["CheckedAll"].ToString() == "")
                        {
                            if (ImgPath != "")
                            {
                                imgBtn.ImageUrl = ImgPath + "/Component/Grid/grid_cb_false.gif";
                            }
                            else
                            {
                                imgBtn.ImageUrl = "/DeskPlusEIP/Design/C/Img/Component/Grid/grid_cb_false.gif";
                            }
                        }
                        else
                        {
                            if (this.SelectedItems.Count > 0)
                            {
                                if (ImgPath != "")
                                {
                                    imgBtn.ImageUrl = ImgPath + "/Component/Grid/grid_cb_true.gif";
                                }
                                else
                                {
                                    imgBtn.ImageUrl = "/DeskPlusApp/Design/C/Img/Component/Grid/grid_cb_true.gif";
                                }
                            }
                            else
                            {
                                if (ImgPath != "")
                                {
                                    imgBtn.ImageUrl = ImgPath + "/Component/Grid/grid_cb_false.gif";
                                }
                                else
                                {
                                    imgBtn.ImageUrl = "/DeskPlusEIP/Design/C/Img/Component/Grid/grid_cb_false.gif";
                                }
                            }
                        }

                        imgBtn.ID = "lnkUnSelect";
                        imgBtn.Click += new ImageClickEventHandler(OnUnselect);
                        e.Item.Cells[0].Controls.Add(imgBtn);
                        e.Item.Cells[0].Attributes.Add("align", "center");
                        e.Item.Cells[0].ControlStyle.Width = Unit.Pixel(25);
                        e.Item.Cells[0].CssClass = "grid_thTR";

                        j = 1;
                    }
                }


                for (int i = j; i < Columns.Count; i++)		// 정렬 시 현재 정렬 형식(asc/desc)형태를 보여주는 삼각형 표
                {
                    TableCell cell = e.Item.Cells[i];

                    if (IsSortedByThisField(Columns[i].SortExpression))
                    {
                        Label lblSorted = new Label();
                        lblSorted.Font.Name = "webdings";
                        lblSorted.Font.Size = FontUnit.XSmall;
                        lblSorted.Text = GetOrderSymbol();
                        lblSorted.CssClass = "grid_thTDText";
                        cell.Controls.Add(lblSorted);
                    }
                    //cell.CssClass = "lblSorted";

                    for (int k = 0; k < cell.Controls.Count; k++)
                    {
                        if (cell.Controls[k] is LinkButton)
                        {
                            ((LinkButton)cell.Controls[k]).CssClass = "grid_thTDText";
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 라인 디자인 HTML 리턴
        /// </summary>
        /// <param name="_columnCount"></param>
        /// <returns></returns>
        public String GetLineCssText(int _columnCount)
        {
            return "</td></tr><tr><td colspan=\"" + _columnCount + "\" class=\"grid_tdTD\" align=\"center\" valign=\"middle\">";
        }


        /// <summary>
        /// 정렬 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSortCommand(Object sender, DataGridSortCommandEventArgs e)
        {
            ProcessSortExpression(e.SortExpression);
            PrepareSortExpression();

            OnUpdateView(EventArgs.Empty);
        }


        /// <summary>
        /// 페이지 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPageIndexChanged(Object sender, DataGridPageChangedEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
            OnUpdateView(EventArgs.Empty);
        }

        #endregion

        #region 멀티선택/열클릭 관련 함수
        /// <summary>
        /// 멀티 선택시 체크박스 Cell 추가
        /// </summary>
        private void AddSelectColumn()
        {
            TemplateColumn tc = new TemplateColumn();

            if (IsClientCheckBox != null && IsClientCheckBox != "")
            {
                tc.ItemTemplate = new ClientCheckedBoxColumnTemplate();
            }
            else
            {
                tc.ItemTemplate = new ServerCheckedBoxColumnTemplate();
            }

            tc.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            tc.ItemStyle.Width = Unit.Pixel(25);

            Columns.AddAt(0, tc);
        }

        #endregion

        #region 정렬관련 함수
        /// <summary>
        /// 정렬 함수들 
        /// </summary>
        /// <param name="strNewSortExpr"></param>
        private void ProcessSortExpression(String strNewSortExpr)
        {
            // Internal-use constants
            const String ORDER_DESC = " desc";
            const String ORDER_ASC = " asc";
            const String CONCAT_ORDER_DESC = "desc,";
            const String CONCAT_ORDER_ASC = "asc,";

            // Read current settings for fields and orders (DESC/ASC)
            String strSortExpressions = (String)ViewState["SortingFields"];
            String strSortOrders = (String)ViewState["SortingOrders"];

            // Parse the new sorting string. This string can only be one of 
            // the strings read from the SortExpression attribute for datagrid's
            // columns. It has any of the forms: 
            // "FieldName", "FieldName DESC", "FieldName ASC",
            // "Field1,Field2 DESC", "Field1 ASC,Field2 DESC", "Field1,Field2".
            // The function splits on the comma, trims all strings and 
            // builds up the two string to store in the Attributes repository.

            // Separate field names from the information regarding the order.
            // Use strNewSortExpr to build the order-less string of fields 
            Char[] aSplitterChar = new Char[] { ',' };
            String[] aSortExpressions = strNewSortExpr.Split(aSplitterChar);

            // Needed in case you want to show a glyph on the caption. In this 
            // case you need to know about the original expression 
            ViewState["ColumnSortExpression"] = strNewSortExpr;

            // Separates fields from orders and builds up two distinct, comma-sep strings
            // i.e. INPUT  --> lastname,firstname DESC
            // i.e. OUTPUT --> lastname,firstname AND asc,desc

            String strNewSortExpressions = "";
            String strNewSortOrders = "";
            for (int i = 0; i < aSortExpressions.Length; i++)
            {
                String tmp1 = aSortExpressions[i].ToLower().Trim();

                int nPos;
                if (tmp1.EndsWith(ORDER_DESC))
                {
                    strNewSortOrders += CONCAT_ORDER_DESC;
                    nPos = tmp1.LastIndexOf(ORDER_DESC);
                    strNewSortExpressions += tmp1.Substring(0, nPos).Trim() + ",";
                }
                else		// ASC or nothing specified, as ASC is the default
                {
                    strNewSortOrders += CONCAT_ORDER_ASC;
                    try
                    {
                        nPos = tmp1.LastIndexOf(ORDER_ASC);
                        strNewSortExpressions += tmp1.Substring(0, nPos).Trim() + ",";
                    }
                    catch
                    {
                        strNewSortExpressions += tmp1 + ",";
                    }
                }
            }

            // Cuts the final "," down
            strNewSortExpressions = strNewSortExpressions.Trim(aSplitterChar);
            strNewSortOrders = strNewSortOrders.Trim(aSplitterChar);

            // Compare current sorting fields with the new one. If it is 
            // the same, and we have sorted already on this fields, 
            // then ASC and DESC are inverted in strNewSortOrders.
            if (strSortExpressions == strNewSortExpressions && strSortOrders != "")
            {
                String[] aSortOrders;
                aSortOrders = strSortOrders.Split(aSplitterChar);

                strNewSortOrders = "";
                for (int i = 0; i < aSortOrders.Length; i++)
                {
                    if (aSortOrders[i] == "desc")
                        strNewSortOrders += CONCAT_ORDER_ASC;
                    else
                        strNewSortOrders += CONCAT_ORDER_DESC;
                }

                strNewSortOrders = strNewSortOrders.Trim(aSplitterChar);
            }

            // Stores the sorting settings to the Attributes repository.
            // This information will be retrieved later when the datagrid rebinds
            ViewState["SortingFields"] = strNewSortExpressions;
            ViewState["SortingOrders"] = strNewSortOrders;
        }

        private void PrepareSortExpression()
        {
            // i.e. INPUT from Attributes  --> lastname,firstname AND asc,desc
            // i.e. OUTPUT to the DataView --> lastname asc,firstname desc

            String strSortInfo = "";

            // Read current settings for fields and orders (DESC/ASC)
            String strSortExpressions = (String)ViewState["SortingFields"];
            String strSortOrders = (String)ViewState["SortingOrders"];

            // Build the string to sort the datagrid by
            try
            {
                Char[] aSplitterChar = new Char[] { ',' };
                String[] aSortExpressions = strSortExpressions.Split(aSplitterChar);
                String[] aSortOrders = strSortOrders.Split(aSplitterChar);

                for (int i = 0; i < aSortExpressions.Length; i++)
                    strSortInfo += aSortExpressions[i] + " " + aSortOrders[i] + ",";
                strSortInfo = strSortInfo.Trim(aSplitterChar);
            }
            catch { }

            // Persists the CURRENT sort expression in Attributes
            ViewState["CurrentSortExpression"] = strSortInfo;
        }

        private bool IsSortedByThisField(String strSortExpression)
        {
            // Retrieves the sort expression associated with the clicked column
            String strRequestedSortExpression = (String)ViewState["ColumnSortExpression"];

            // Compares it to the sort expression of the column whose header is being processed
            return (strSortExpression == strRequestedSortExpression);
        }

        private String GetOrderSymbol()
        {
            // Retrieves the string with order information
            String strSortOrders = (String)ViewState["SortingOrders"];

            // Gets the webding corresponding to ASC/DESC for the first (primary) column
            // in the sort expression. The glyph is decided based on the order of the 
            // primary column in the sort expression.
            bool bDescending = strSortOrders.StartsWith("desc");

            // Webdings #6 and #5 evaluate to down and up triangles
            return (bDescending ? " 6" : " 5");
        }

        #endregion

        #region 프로퍼티
        /// <summary>
        /// 체크 박스로 선택된 아이템들
        /// </summary>
        public ArrayList SelectedItems
        {
            get
            {
                if (!AllowMultiSelect) return null;

                ArrayList a = new ArrayList();
                foreach (DataGridItem dgi in Items)
                {

                    if (IsClientCheckBox != null && IsClientCheckBox != "")
                    {
                        System.Web.UI.HtmlControls.HtmlInputCheckBox cb = (System.Web.UI.HtmlControls.HtmlInputCheckBox)dgi.Cells[0].Controls[0];

                        if (cb.Checked)
                            a.Add(dgi);
                    }
                    else
                    {
                        CheckBox cb = (CheckBox)dgi.Cells[0].Controls[0];
                        if (cb.Checked)
                            a.Add(dgi);
                    }

                }
                return a;
            }
        }


        /// <summary>
        /// 클라이언트 체크박스
        /// </summary>
        public String IsClientCheckBox
        {
            get { return (String)ViewState["IsClientCheckBox"]; }
            set { ViewState["IsClientCheckBox"] = value; }
        }


        /// <summary>
        /// 현재 정렬
        /// </summary>
        public String SortExpression
        {
            get { return (String)ViewState["CurrentSortExpression"]; }
            set { ViewState["CurrentSortExpression"] = value; }
        }


        /// <summary>
        /// 필터
        /// </summary>
        public String RowSelectFilter
        {
            get { return (String)ViewState["RowSelectFilter"]; }
            set { ViewState["RowSelectFilter"] = value; }
        }


        /// <summary>
        /// 이미지 위치
        /// </summary>
        public String ImgPath
        {
            get { return (String)ViewState["ImgPath"]; }
            set { ViewState["ImgPath"] = value; }
        }



        /// <summary>
        /// GridCell 여부
        /// </summary>
        public String IsCustomViewLine
        {
            get { return (String)ViewState["IsCustomViewLine"]; }
            set { ViewState["IsCustomViewLine"] = value; }
        }

        /// <summary>
        /// 각 Row 에 라인 디자인을 할 경우 마지막 View TD의 위치를 정해 줍니다 보이는 TD의 갯수를 넣으세요  
        /// </summary>
        public String ViewLineColumnCount
        {
            get { return (String)ViewState["ViewLineColumnCount"]; }
            set { ViewState["ViewLineColumnCount"] = value; }
        }

        /// <summary>
        /// 각 Row 에 라인 디자인을 할 경우 정확한 TD(Column) 갯수를 넣어줘야 정확한 라인이 생성됩니다.  ColSpan
        /// </summary>
        public String ViewLineColumnColSpan
        {
            get { return (String)ViewState["ViewLineColumnColSpan"]; }
            set { ViewState["ViewLineColumnColSpan"] = value; }
        }

        /// <summary>
        /// 데이터 원본
        /// </summary>
        public override object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;
            }
        }


        #endregion
    }
}
