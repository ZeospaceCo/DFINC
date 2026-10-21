<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDay.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.ListDay" %>
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
        <div class="list_add_style1 listday">
            <div class="date titleDate"> <a href="" class="b_prev" id="pvDay"><i class="fas fa-angle-left"></i></a> 
            <span id="timeList"></span>
            <a href="" class="b_next" id="nxDay"><i class="fas fa-angle-right"></i></a> </div>
        </div>

        <div id="toPrint">
        <div id="phListDay" style="clear: both;"></div>     
        </div>       
        <!-- 페이징 시작 //-->
        <div id="Pager1" name="Pager1" class="paging_num_s1"> 
            <h3><span id="pageInfo"></span></h3>
	        <span id="pager"></span>
            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                <asp:ListItem Text="30" Value="30"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <!-- 페이징 끝 //-->    
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
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ROW = "<%=GetResource("ROW") %>";
        var R_VisitType = "<%=GetResource("VisitType") %>";
        var R_StartDate = "<%=GetResource("StartDate") %>";
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_Subscriber = "<%=GetResource("Subscriber") %>";
        var R_WriteDate = "<%=GetResource("WriteDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/ListDay.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Content/ListDay.js");
    </script>    
</body>
</html>
