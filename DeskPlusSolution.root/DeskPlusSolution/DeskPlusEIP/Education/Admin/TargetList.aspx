<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TargetList.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.TargetList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["SolutionTitle"])%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
<form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1>대상자리스트</h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" Text="닫기" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <asp:Literal ID="ltlMessage" runat="server" Mode="Encode" />
                <asp:PlaceHolder ID="phTargetList" runat="server" Visible="false">
                    <div id="scrollDiv" style="overflow-y:auto;max-height:260px;">
                        <div id="placeholder" style="clear:both;"></div>
                    </div>
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" class="paging_num_s1">
                        <h3><span id="pageInfo"></span></h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="30" Value="30" />
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript">
        var HasEducation = <%=HasEducation ? "true" : "false"%>;
        var ListID = <%=ListID%>;
        var CurrentPage = 1;
        var TotalItemCount = <%=TotalCount%>;
        var RecordPerPage = <%=RecordPerPage%>;
        var R_Previous = "이전";
        var R_Next = "다음";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/TargetList.js?v=2"></script>
</body>
</html>
