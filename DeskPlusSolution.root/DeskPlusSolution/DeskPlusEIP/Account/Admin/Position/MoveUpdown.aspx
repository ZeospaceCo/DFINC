<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoveUpdown.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.Position.MoveUpdown" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">        
        <div class="inside">
            <div id="popup_header">
                <h1><asp:literal ID="ltlTitle" runat="server"></asp:literal></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
        	    <div class="layout_scroll" style="min-height:325px;height:325px;">
                    <div class="DD">
                        <ul id="ulWidgets">
                            <%=FolderList %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>        
    </div>    

    </form>
    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>

    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_SelectFolder = "<%=GetResource("SelectTitle") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_CompleteOrder = "<%=GetResource("CompleteOrder") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Position/MoveUpdown.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Position/MoveUpdown.js");
    </script>
</body>
</html>