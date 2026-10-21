<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WriteCount.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Statistic.WriteCount" %>
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
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        
        <div id="layout_header">
            <h1><%=GetResource("WriteContents")%></h1>
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
                        <asp:ListItem Text="" Value="10"></asp:ListItem>
                        <asp:ListItem Text="" Value="50"></asp:ListItem>
                        <asp:ListItem Text="" Value="100"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li><cc3:ClientSmartButton ID="CbtnSearch" runat="server" Text="검색" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            
            <div id="placeholder" style="clear: both;"></div>
            

            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->
        </div>
    
    </div>
</div>
</form>
            
<form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList"/>
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no"  src="about:blank" ></iframe>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" type="text/javascript"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
     <script type="text/javascript">
         /**********************************************************************************************
         전역 변수들
         **********************************************************************************************/
         var StartDate = ""; //검색분류
         var EndDate = ""; //검색어
         var CurrentPage = 1; //현재페이지
         var Order = " COUNT(WriteUserID)   "; //정렬
         var TotalItemCount = "<%=TotalCount %>"; //게시물수
         var RecordPerPage = 10; //보여줄게시물수
         var CategoryID = ""; //카테고리

         var ImagePath = "<%=BaseImgPath%>";
         var PreLang = "<%=PreLang%>";
         var AttachFileInfoList = null;

         var R_Rank = "<%=GetResource("Rank") %>";    //순위
         var R_Deft = "<%=GetResource("Department") %>";    //부서
         var R_User = "<%=GetResource("UserName") %>";    //사용자명
         var R_WriteCnt = "<%=GetResource("WriteCnt") %>";    //등록수
         var R_Previous = "<%=GetResource("Previous") %>";
         var R_Next = "<%=GetResource("Next") %>";
         var R_NotExistList = "<%=GetResource("NotExistList") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Statistic/WriteCount.js"></script>
</body>
</html>

