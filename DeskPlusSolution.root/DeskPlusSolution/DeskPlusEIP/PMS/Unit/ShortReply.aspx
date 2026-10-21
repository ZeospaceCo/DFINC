<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReply.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Unit.ShortReply" %>
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
<form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("Board_List_ShortReply")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll" style="min-height:120px;height:120px;">
            <textarea name="textarea" id="textarea" style="width:97%;"></textarea>
            </div>
        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
		var projectID           = "<%=ProjectID%>";
		var menuID              = "<%=MenuID%>";
		var menuType = "<%=MenuType %>";
		var rqProjectID = "<%=RqProjectID %>";
        var UnitID       = "<%=UnitID %>";
        var pageListUrl         = "<%=PageListUrl %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값  
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Status = "<%=Status %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_ReplyInputContent = "<%=GetResource("Private_BoardAdd_DPEditorTEXT") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Unit/ShortReply.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Unit/ShortReply.js");
    </script>
</body>
</html>
