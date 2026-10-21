<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Statistic.Chart" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link type="text/css" href="/DeskPlusEIP/Board/Chart/css/base.css" rel="stylesheet" />
    <link type="text/css" href="/DeskPlusEIP/Board/Chart/css/AreaChart.css" rel="stylesheet" />    
</head>
<body onload="init()" >
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%=GetResource("NoticeCheck")%></h1>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><span><%=GetResource("RegDate")%></span></li>
                <li>
                    <div id="startEndDate">
                        <cc3:ClientSmartTextBox ID="startDate" Required="true"  Width="100px" runat="server" ReadOnly></cc3:ClientSmartTextBox>
                        <%=GetResource("From")%>
                        <cc3:ClientSmartTextBox ID="endDate" Required="true"  Width="100px" runat="server" ReadOnly></cc3:ClientSmartTextBox>
                        <%=GetResource("To")%>
                    </div>
                </li>
                <li><input type="radio" class="searchradio" checked="true" name="searchradio" value="0" id = "rdoAsc" runat = "server" /><%=GetResource("Up")%><input type="radio" class="searchradio" name="searchradio" value="1" id = "rdoDesc"  runat = "server" /><%=GetResource("Down")%></li>
                <li>
                    <asp:DropDownList ID="DDLSearchType" runat="server" CssClass="searchselect">
                        <asp:ListItem Text="10개" Value="10"></asp:ListItem>
                        <asp:ListItem Text="50개" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100개" Value="100"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li><cc3:ClientSmartButton ID="CbtnSearch" runat="server" Text="검색" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul> 
        </div>

        <div id="layout_content">
            <div id="center-container">
                <div id="infovis"></div>
            </div>

            <h3><span id="pageInfo"></span></h3>
        </div>
    
    </div>
</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Chart/js/jit.js"></script>
    <!--[if IE]><script language="javascript" type="text/javascript" src="/DeskPlusEIP/Board/Chart/js/excanvas.js"></script><![endif]-->
     <script type="text/javascript">
         var StartDate = ""; //검색분류
         var EndDate = ""; //검색어
         var CurrentPage = 1; //현재페이지
         var Order = " COUNT(WriteUserID)   "; //정렬
         var TotalItemCount = "<%=TotalCount %>"; //게시물수
         var RecordPerPage = 10; //보여줄게시물수
         var CategoryID = ""; //카테고리
         var ImagePath = "<%=BaseImgPath%>";
         var PreLang = "<%=PreLang%>";
     </script>
     <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Statistic/Chart.js"></script>
</body>
</html>
