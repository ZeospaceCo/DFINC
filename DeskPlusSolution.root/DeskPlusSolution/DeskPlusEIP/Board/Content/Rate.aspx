<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rate.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.Rate" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%= GetResource("Rate_Title") %></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnRatingList" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <div class="boa_rate_layleft">
                
                <!-- 추천 후 //-->
                <asp:Panel runat="server" ID="pnlVoteCast" style="min-height:325px;">
                    <span>▣ <%=GetResource("Rate_lblVoteCastMessage") %></span>
                </asp:Panel>

                <!-- 추천 전 //-->
                <asp:Panel runat="server" ID="pnlCastVote" style="min-height:325px;">
                    <span>▣ <%=GetResource("Rate_lblTopMessage") %></span>
                    <ul>
                        <li class="top"><span><%=GetResource("Rate_Outstanding")%></span></li>
                        <li><asp:RadioButton GroupName="rating" runat="server" ID="rating5"></asp:RadioButton> 5 : <img src="<%=BaseImgPath%>/Icon/ico_rate5.gif" alt="" title="" /></li>
                        <li><asp:RadioButton GroupName="rating" runat="server" ID="rating4"></asp:RadioButton> 4 : <img src="<%=BaseImgPath%>/Icon/ico_rate4.gif" alt="" title="" /></li>
                        <li><asp:RadioButton GroupName="rating" runat="server" ID="rating3" Checked="True"></asp:RadioButton> 3 : <img src="<%=BaseImgPath%>/Icon/ico_rate3.gif" alt="" title="" /></li>
                        <li><asp:RadioButton GroupName="rating" runat="server" ID="rating2"></asp:RadioButton> 2 : <img src="<%=BaseImgPath%>/Icon/ico_rate2.gif" alt="" title="" /></li>
                        <li><asp:RadioButton GroupName="rating" runat="server" ID="rating1"></asp:RadioButton> 1 : <img src="<%=BaseImgPath%>/Icon/ico_rate1.gif" alt="" title="" /></li>
                        <li class="bott"><span><%=GetResource("Rate_Poor")%></span></li>
                    </ul>
                    <span>▣ <%=GetResource("Rate_lblCommentMessage") %></span>
                        <ul>
                        <li><asp:TextBox ID="txtComments" runat="server" Width="90%" TextMode="MultiLine"></asp:TextBox></li>
                        </ul>
                </asp:Panel>

            </div>

            <div class="boa_rate_layright" style="height:370px;">
                <%=GetResource("Rate_lblAverageRatingMessage") %> : <span class="boa_orangeSqu"><asp:Label runat="server" ID="lblAverageRating"></asp:Label></span>
                <div class="stat">
                    <asp:Table ID="RatingsGraphTable" runat="server" align="center">
                    <asp:TableRow CssClass="tr1">
                        <asp:TableCell VerticalAlign="Bottom" Width="30px"></asp:TableCell>
                        <asp:TableCell VerticalAlign="Bottom" Width="30px"></asp:TableCell>
                        <asp:TableCell VerticalAlign="Bottom" Width="30px"></asp:TableCell>
                        <asp:TableCell VerticalAlign="Bottom" Width="30px"></asp:TableCell>
                        <asp:TableCell VerticalAlign="Bottom" Width="30px"></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="tr2">
                        <asp:TableCell>1</asp:TableCell>
                        <asp:TableCell>2</asp:TableCell>
                        <asp:TableCell>3</asp:TableCell>
                        <asp:TableCell>4</asp:TableCell>
                        <asp:TableCell>5</asp:TableCell>
                    </asp:TableRow>
                    </asp:Table>
                </div>
                <asp:Label runat="server" ID="lblRatingCount"></asp:Label>
            </div>

        </div>
    </div>

</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID %>";
        var ListID = "<%= ListID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var Rate_SelectVote = "<%= GetResource("Rate_SelectVote") %>";
        var Rate_Success = "<%= GetResource("Rate_Success") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/Rate.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/Rate.js");
    </script>
</body>
</html>

            


