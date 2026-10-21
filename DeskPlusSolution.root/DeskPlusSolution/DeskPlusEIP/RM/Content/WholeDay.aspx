<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WholeDay.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.WholeDay" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
     <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <!-- ############# start of contextMenu ############## -->
    <ul id="aMenu" class="contextMenu">
        <li class="accept"><a href="#accept"><%=GetResource("Approved") %></a></li>
        <li class="unaccept"><a href="#unaccept"><%=GetResource("Unapproved") %></a></li>
        <li class="view"><a href="#view"><%=GetResource("ViewContent")%></a></li>
    </ul>
    <ul id="bMenu" class="contextMenu">        
        <li class="unaccept"><a href="#unaccept"><%=GetResource("Unapproved") %></a></li>
        <li class="view"><a href="#view"><%=GetResource("ViewContent")%></a></li>
    </ul>
    <ul id="cMenu" class="contextMenu">
        <li class="accept"><a href="#accept"><%=GetResource("Approved") %></a></li>        
        <li class="view"><a href="#view"><%=GetResource("ViewContent")%></a></li>
    </ul>
    <!-- ############# end of contextMenu ############## -->
    <div id="layout_info" style="min-height:400px;">
        <div class="list_add_style1 wholeday">
            <ul class="cw_src">
                <li><asp:CheckBoxList ID="cbxUsable" RepeatDirection="Horizontal" runat="server" CssClass="sel_stat"></asp:CheckBoxList></li>
            </ul>
            <div class="date titleDate"> <a href="" class="b_prev" id="pvDay"><i class="fas fa-angle-left"></i></a> 
            <span id="timeList"></span>
            <a href="" class="b_next" id="nxDay"><i class="fas fa-angle-right"></i></a> </div>
        </div>

        <!-- 테이블 시작 -->    
        <div id="toPrint">
        <%=DayTable %>
        </div>
        <!-- 테이블 끝 -->
        <!-- //layout_info -->
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/iframeResizer.contentWindow.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID%>";
        var TmpDate = "<%=TmpDate%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchUsable = "<%=SearchUsable %>"; 
        var TmpReID = "";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";
        var R_Subscriber = "<%=GetResource("Subscriber") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_Time = "<%=GetResource("Time") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_SendAlarm = "<%=GetResource("SendAlarm") %>";
        var R_ConfirmSend = "<%=GetResource("ConfirmSend") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/WholeDay.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Content/WholeDay.js");
    </script>
</body>
</html>
