<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavoriteCopy.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.FavoriteCopy" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../Common/BoardTreeScript.ascx" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript">
        var Dcparm = "<%= Dcparm %>";
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>";
        var FavoriteID = 0;
        var Types = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_Complete = "<%=GetResource("Processing") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_SelectFavoriteFolder = "<%=GetResource("SelectFavoriteFolder") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/FavoriteCopy.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/FavoriteCopy.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">

        <div class="popup_header">
            <h1><%=GetResource("FavoriteFolderAdd")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:300px;">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="70px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FavoriteFolderName")%></th>
                    <td><input type="text" id="folderName" style="width:95%" /></td>
                </tr>
            </tbody>
            </table>
            <div class="layout_scroll" style="min-height:285px;height:285px;">
                <uc1:BoardTreeScript ID="UCBoardTreeScript" runat="server" UseIcon="true" UseWriteAuthority="false" UseReadAuthority="true" 
                UseFolder="false" UseMyTemplate="false" UseMyFavorite="true" ShowTitleNoAuth="true"
                SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:BoardTreeScript>
            </div>
        </div>
    </div>
</div>
</form>
</body>
</html>
