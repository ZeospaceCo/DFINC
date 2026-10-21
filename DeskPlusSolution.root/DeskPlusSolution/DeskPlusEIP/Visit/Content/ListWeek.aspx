<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListWeek.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.ListWeek" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <!-- ############# start of contextMenu ############## -->
    <ul id="basicMenu" class="contextMenu">
        <li class="accept"><a href="#accept"><%=GetResource("Type1")%></a></li>
        <li class="unaccept"><a href="#unaccept"><%=GetResource("Type2")%></a></li>   
    </ul>
    <!-- ############# end of contextMenu ############## -->
    <div id="layout_info">
        <div class="list_add_style1 listweek">
            <div class="date titleDate"> <a href="" class="b_prev" id="pvWeek"><i class="fas fa-angle-left"></i></a> 
            <span id="timeList" runat="server"></span> <span>  ~  </span> <span id="EtimeList" runat="server"></span>
            <a href="" class="b_next" id="nxWeek"><i class="fas fa-angle-right"></i></a> </div>
        </div>

         <!-- 테이블 시작 -->    
        <div id="toPrint">           
        <div id="phListWeek" style="clear: both;"></div>
        </div> 
        <!-- 테이블 끝 -->    
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var TmpDate = "<%=TmpDate%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_WeekName = "<%=GetResource("WeekName") %>";
        var R_VisitType = "<%=GetResource("VisitType") %>";
        var R_StartDate = "<%=GetResource("StartDate") %>";
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_Subscriber = "<%=GetResource("Subscriber") %>";
        var R_WriteDate = "<%=GetResource("WriteDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/ListWeek.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Content/ListWeek.js");
    </script>    
</body>
</html>
