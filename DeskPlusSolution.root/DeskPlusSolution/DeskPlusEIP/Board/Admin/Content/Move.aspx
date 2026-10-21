<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Move.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Content.Move" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="BoardTreeScript" Src="../../Common/BoardTreeScript.ascx" %>
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
                <h1><%=GetResource("FolderMove")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
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
                        <th><%=GetResource("FolderTitle")%></th>
                        <td><input type="text" id="folderName" readonly="readonly" size="width:95%"/></td>
                    </tr>
                </tbody>
                </table>
                <div class="layout_scroll" style="min-height:285px;height:285px;">
                <uc1:BoardTreeScript ID="UCBoardTreeScript" runat="server" UseIcon="true" UseWriteAuthority="true" UseAdmin="true" UseReadAuthority="true" UseMyTemplate="false" 
                ShowTitleNoAuth="true"
                SetNodeSelect="onNodeSelect" SetCallbackComplete="onCallbackComplete"></uc1:BoardTreeScript>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        var SelCatID = "";
        var ListID = "<%= ListID %>";
        var OptionUseView = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_MoveCategory = "<%=GetResource("MoveCategory") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Content/Move.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Content/Move.js");
    </script>
</body>
</html>
