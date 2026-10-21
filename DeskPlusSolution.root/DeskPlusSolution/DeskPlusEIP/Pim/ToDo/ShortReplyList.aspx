<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReplyList.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.ToDo.ShortReplyList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>        
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <a onclick="ShortReplyList.Search()">
            <h5><%=GetResource("ShortReply")%></h5>
            <img src="/DeskPlusEIP/Design/C/Img/Main/ico_ref.gif" alt="Reload" />
            </a>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div id="layout_srh">
        <ul class="btn_searchbox">
            <li><div id="loading"></div></li>
            <li>
            <div class="searchterm">
            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect">
            <asp:ListItem Value=""></asp:ListItem>
            <asp:ListItem Value="week"></asp:ListItem>
            <asp:ListItem Value="month"></asp:ListItem>
            <asp:ListItem Value="period"></asp:ListItem>
            </asp:DropDownList></div>
            <div id="startEndDate" class="searchterm">
                <input type="text" id="startDate" size="12" class="clsDate" />-<input type="text" id="endDate" size="12" class="clsDate" />
            </div>
            </li> 
            <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect"></asp:DropDownList></li>   
            <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                        class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
            </li>
            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton></li>  
        </ul>            
        </div>
        <div class="popup_layout_style3" style="padding-top:5px;height:368px;min-height:368px; border:0px solid #f00;">
            <div id="scrollDiv" >
            
            <div class="table_typeLtop"><p></p></div>
            <div id="placeholder_TRelpyList" style="clear: both;"></div>
            <div class="table_typeLbottom"><p></p></div>
            
            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                    <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                    <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            </div>


            <div class="layout_subbox">
                <h2><%=GetResource("ShortReply")%></h2>
                (<span class="charsRemaining"></span>) 
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" />
                </ul>
                <span style="float:right;padding-right:10px;"><asp:CheckBox ID="chkOpen" runat="server" /> <%=GetResource("ReplyOpen")%></span>
            </div>
            <textarea name="textarea" id="textarea"  class="textarea" style="width:98%;height:55px;" maxlength="200"></textarea>

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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ToDoID = "<%=ToDoID %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수                
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";  
        var PreLangReal = "<%=PreLangReal %>"; //ja              
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>";        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("TextEmpty") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Processing = "<%=GetResource("ACCOUNT_SUCCESS") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_MsgError = "<%= GetResource("MsgError")%>";
        var R_ApplyRegDate = "<%= GetResource("InputSearchDay")%>";
        var R_SecurityMsg = "<%= GetResource("SecurityMsg")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/ToDo/ShortReplyList.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/ToDo/ShortReplyList.js");
    </script>
</body>
</html>
