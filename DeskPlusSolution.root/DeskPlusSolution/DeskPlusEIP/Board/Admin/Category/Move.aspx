<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Move.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Category.Move" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../../Common/BoardTreeScript.ascx" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
    
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("FolderMove") %></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:320px;">
                <table class="table_typeB">
			    <caption></caption>
			    <colgroup>
                    <col width="60px" />
                    <col />
			    </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("FolderTitle")%></th>
                        <td><input type="text" id="folderName" readonly="readonly" size="30"/></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:285px;height:285px;">
                <uc1:BoardTreeScript ID="UCBoardTreeScript" runat="server" 
                UseAdmin = "true"
                ShowTitleNoAuth="true"
                SetNodeSelect="onNodeSelect" 
                SetCallbackComplete="onCallbackComplete"></uc1:BoardTreeScript>
                </div>
            </div>
        </div>

    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var FolderID = "0";
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>";        

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_CompleteFolderMoved = "<%=GetResource("CompleteFolderMoved")%>";
        var R_NoMove = "<%=GetResource("NoMove")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Category/Move.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Category/Move.js");
    </script>
</body>
</html>

