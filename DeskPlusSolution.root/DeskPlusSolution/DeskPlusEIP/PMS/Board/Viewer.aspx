<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Viewer.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Board.Viewer" %>
<%@ Register TagPrefix="cc2" Namespace="Zeospace.DeskPlus.Component.Grids" Assembly="Zeospace.DeskPlus.Component.Grids" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
<form id="form1" method="post" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div id="popup_header">
            <h1><%=GetResource("Board_List_Viewer")%></h1>
            <ul class="btn_right">
                <li><div id="loading"></div></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll"  style="min-height:290px;height:290px;"> 
            
                <cc2:MultiSelectGrid ID="msgListViewer" runat="server" PageSize="15" DataKeyField="BoardViewerID"
                    AutoGenerateColumns="False" AllowCustomPaging="True" Width="100%" BaseImgPath="<%#BaseImgPath%>"
                    ShowFooter="false" OnPageIndexChanged="msgListViewer_PageIndexChanged" ViewLineColumnCount="4"
                    ViewLineColumnColSpan="4">
                    <PagerStyle HorizontalAlign="Right" PageButtonCount="10" CssClass="GridPager" Mode="NumericPages">
                    </PagerStyle>
                    <AlternatingItemStyle CssClass="GridAltItem"></AlternatingItemStyle>
                    <SelectedItemStyle CssClass="GridSelItem"></SelectedItemStyle>
                    <ItemStyle CssClass="GridItem"></ItemStyle>
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
                            <itemtemplate>
						<%# RecDeptName(DataBinder.Eval(Container.DataItem, "DeptName").ToString())%>
					</itemtemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
                            <itemtemplate>
						<%# RecTitleName(DataBinder.Eval(Container.DataItem, "TitleName").ToString())%>
					</itemtemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
                            <itemtemplate>
						<%# RecUserName(DataBinder.Eval(Container.DataItem, "UserName").ToString())%>
					</itemtemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="CRUDDate">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                        </asp:BoundColumn>
                    </Columns>
                </cc2:MultiSelectGrid>
                
            </div>
            <asp:literal id="ltlReceiveManSelect" runat="server" Visible="false"></asp:literal>
        </div>                
    </div>
</div>

        <!-- ================================== Hidden Input ===================================== -->
        <!-- ==================================================================================== -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Board/Viewer.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Board/Viewer.js");
    </script>
</body>
</html>
