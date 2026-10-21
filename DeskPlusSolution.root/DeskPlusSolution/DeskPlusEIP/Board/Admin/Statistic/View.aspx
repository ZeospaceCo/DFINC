<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Statistic.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_header">
        <ul class="btn_left">
            <li>
                <cc3:ClientSmartButton ID="cbtnRead" runat="server"></cc3:ClientSmartButton>
            </li>
            <li>
                <cc3:ClientSmartButton ID="cbtnRate" runat="server"></cc3:ClientSmartButton>
            </li>
        </ul>
        <ul class="btn_right">            
            <li>
                <cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton>
            </li>
        </ul>
    </div>
    <div id="layout_info">
        <!-- 기본정보 -->
        <h2>
            <%=GetResource("View_Info") %></h2>
        <div class="table_typeBtop_left">
            <p>
            </p>
        </div>
        <table cellspacing="0" class="table_typeB">
            <tr>
                <th>
                    <%=GetResource("Title") %>
                </th>
                <td>
                    <asp:Label ID="lbTitle" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="last-child">
                    <%=GetResource("View_RegInfo") %>
                </th>
                <td class="last-child">
                    <%=GetResource("Register")%>
                    :
                    <% if (WriteUserID > 0)
                       {%>
                    <a href="#" onclick="javascript:AccountInfo('<%=WriteUserID%>')" style="cursor:pointer">
                    <% }
                       else
                       { %><a href="#"><%} %>
                    <asp:Literal ID="lbWriteInfo" runat="server"></asp:Literal>
                    </a>
                    &nbsp;/&nbsp;
                    <%=GetResource("RegDate")%>
                    :
                    <asp:Label ID="lbWriteDate" runat="server"></asp:Label>&nbsp;/&nbsp;
                    <%=GetResource("ViewCount")%>
                    :
                    <asp:Label ID="lbReadCount" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div class="table_typeBbottom_left">
            <p>
            </p>
        </div>
        <!-- 기본정보 -->
        <!-- 본문정보 -->
        <h2>
        </h2>
        <div class="content_box"  style="overflow:auto;">
        <table cellspacing="0" style="min-height:300px;width:100%;">
            <tr>
                <td>
                    <asp:Label ID="lbContent" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        </div>
        <!-- 본문정보 -->
        <!-- 첨부파일 -->
        <h2><%=GetResource("AttachName")%></h2>
        <div id="placeholder_file" style="clear: both;"></div>
        <asp:Panel ID="pnReple" runat="server">
            <div class="layout_box">
                <h2>
                    <%=GetResource("ShortReply")%></h2>
                <ul class="btn_right">
                    <li>                        
                    </li>
                </ul>
            </div>
            <div id="placeholder" style="clear: both;"></div>
        </asp:Panel>
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CatID = "<%=CatID %>"; //트리에서 넘어온 카테고리
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%= ListID %>";
        var SearchGroup = "<%=SearchGroup %>"; //분류
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var Option_Reple = "<%=pnReple.Visible %>"; //리플 사용여부
        var OptionShowViewer = ""; //조회자사용여부
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Statistic/View.js"></script>
</body>
</html>
