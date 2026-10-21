<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavoriteMove.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.FavoriteMove" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="WebHardTree" Src="../Common/WebHardTree.ascx" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("FavoriteFolderMove")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="80px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FavoriteFolderName")%></th>
                    <td><cc3:ClientSmartTextBox ID="folderName" ReadOnly="true" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>
            <div class="layout_scroll" style="min-height:285px;height:285px;">
            <uc1:WebHardTree ID="WebHardTree" runat="server" UseCategory="F" ></uc1:WebHardTree>
            </div>
        </div>
    </div>
</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var FavoriteID = "<%=FavoriteID %>";
        var ParentFavoriteID = "<%=ParentFavoriteID %>";
        var TmpFavoriteID = 0;
        var Types = "";
        var ChildCnt = "<%=ChildCnt %>";
        var R_SelectFolder = "<%=GetResource("SelectFolder") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_SelectFavoriteFolder = "<%=GetResource("SelectFavoriteFolder") %>";
        var R_SelectFavoriteFolderMove = "<%=GetResource("SelectFavoriteFolderMove") %>";
        var R_MoveSameFavorite = "<%=GetResource("MoveSameFavorite") %>";
        var R_MoveSubFavorite = "<%=GetResource("MoveSubFavorite") %>";
        var R_MoveLocationFavorite = "<%=GetResource("MoveLocationFavorite") %>";
        var R_FavoriteFolder = "<%=GetResource("FavoriteFolder") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/FavoriteMove.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Content/FavoriteMove.js");
    </script>
</body>
</html>
