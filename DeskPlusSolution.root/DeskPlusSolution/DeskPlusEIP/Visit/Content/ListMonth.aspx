<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMonth.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.ListMonth" %>
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
        <li class="view"><a href="#view"><%=GetResource("ViewDetail")%></a></li>      
    </ul>
    <!-- ############# end of contextMenu ############## -->
    <div id="layout_info">
        <div class="list_add_style1 listmonth">  
            <ul class="cw_src"> 
                <li>  
                    <table class="sel_stat">
	                    <tr>
		                    <td><label><p class="unappT"><%=GetResource("Type0")%></p></label>&nbsp;</td>
                            <td><label><p class="appT"><%=GetResource("Type1")%></p></label>&nbsp;</td>
                            <td><label><p class="refuT"><%=GetResource("Type2")%></p></label>&nbsp;</td>
	                    </tr>
                    </table>
                </li>
            </ul>
            <div class="date titleDate">
                <a href="" class="b_prev" id="pvMonth"><i class="fas fa-angle-left"></i></a>
                <span id="timeList" runat="server"></span>
                <a href="" class="b_next" id="nxMonth"><i class="fas fa-angle-right"></i></a>
            </div>          

        </div>   
        <!-- 테이블 시작 -->
        <div id="toPrint">
        <div id="phListMonth" style="clear: both;"></div>       
        <div class="table_bt"></div>
        </div>
        
    </div><!-- //layout_info -->
    </form>
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
        var TmpDate = "<%=TmpDate%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var R_Sunday = "<%=GetResource("Sunday") %>";
        var R_Monday = "<%=GetResource("Monday") %>";
        var R_Tuesday = "<%=GetResource("Tuesday") %>";
        var R_Wednesday = "<%=GetResource("Wednesday") %>";
        var R_Thursday = "<%=GetResource("Thursday") %>";
        var R_Friday = "<%=GetResource("Friday") %>";
        var R_Saturday = "<%=GetResource("Saturday") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/ListMonth.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Content/ListMonth.js");
    </script>        
</body>
</html>
