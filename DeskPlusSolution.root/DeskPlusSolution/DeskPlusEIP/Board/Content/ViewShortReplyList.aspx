<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewShortReplyList.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.ViewShortReplyList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
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
    <div style="display: none;">
        <input type="hidden" id="hdShortReplyID" />
        <input type="hidden" id="hdGood" />
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.resize.js"></script>
    <script type="text/javascript">
        var CatID = "<%=CatID %>"; //트리에서 넘어온 카테고리
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%= ListID %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var OptionShortReplyGood = "<%=OptionShortReplyGood %>";
        var OptionShortReplyTop = "<%=OptionShortReplyTop %>";
        var Option_Reple = "<%=liReplyList.Visible %>"; //리플 사용여부
        var WindowObjectReference; //새창용  
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>"; 
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_AddShortReply = "<%=GetResource("AddShortReply") %>";
        var R_ReplyAdd = "<%=GetResource("ReplyAdd") %>";
        var R_Save = "<%=GetResource("Submit") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
        var R_DelReply = "<%=GetResource("DelReply") %>";
        var R_MyComment = "<%=GetResource("MyComment") %>";
        var R_AlreadyComment = "<%=GetResource("AlreadyComment") %>";
        var R_Recommend = "<%=GetResource("Recommend") %>";
        var R_Decommendation = "<%=GetResource("Decommendation") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/ViewShortReplyList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/ViewShortReplyList.js");
    </script>
</body>
</html>