<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewShortReplyList.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.ToDo.ViewShortReplyList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css"/>        
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("ShortReply")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
             </ul>
        </div>
        <div class="dotLine"></div>       
        <div class="layout_scroll" style="min-height: 610px;height:610px;padding:20px;" >
            <ul> 
                <li ID="liReplyList" runat="server" class="replyPop">
                    <div id="placeholder" style="clear: both;"></div>
                </li>
            </ul>  
        </div>
    </div>
</div>
    </form>
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var TodoID = "<%=TodoID%>";  
        var ImagePath = "<%=BaseImgPath%>";        
     
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_SecurityMsg = "<%= GetResource("SecurityMsg")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/ToDo/ViewShortReplyList.js" ></script>         
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/ToDo/ViewShortReplyList.js");
    </script>
</body>
</html>
