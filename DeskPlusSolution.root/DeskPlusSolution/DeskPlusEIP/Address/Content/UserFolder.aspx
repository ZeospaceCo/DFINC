<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserFolder.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.UserFolder" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>   
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">        
        <div class="inside">
            <div id="popup_header">
                <h1><asp:literal ID="ltlTitle" runat="server"></asp:literal></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
			    <div class="layout_scroll" style="min-height:330px;height:330px;">
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
        var CategoryID = "<%=CategoryID %>";
        var Types = "<%=Types %>"; //PG
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_ChangeCategoryOrder = "<%=GetResource("ChangeCategoryOrder") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/UserFolder.js"></script>
</body>
</html>