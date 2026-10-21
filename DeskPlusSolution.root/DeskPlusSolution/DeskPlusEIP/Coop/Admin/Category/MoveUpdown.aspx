<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoveUpdown.aspx.cs" Inherits="Zeospace.DeskPlus.Coop.Web.Admin.Category.MoveUpdown" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Coop.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   

    <script type="text/javascript">

        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_ChangeOrders = "<%=GetResource("ChangeOrders") %>";

    </script>     
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Coop/Js/Admin/Category/MoveUpdown.js");
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="top-left"></div>
        <div class="top-right"></div>
        <div class="inside">
            <div id="popup_header">
                <h5><asp:literal ID="ltlTitle" runat="server"></asp:literal></h5>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" BeginEndTagClass="btn_blue"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">        	    
			    <div class="layout_scroll" style="min-height:325px;height:325px;">
                    <div class="DD">
                        <ul id="ulWidgets">
                            <%=ItemList %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>    

    </form>
</body>
</html>
