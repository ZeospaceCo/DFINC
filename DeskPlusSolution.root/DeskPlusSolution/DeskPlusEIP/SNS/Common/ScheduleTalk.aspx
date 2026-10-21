<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleTalk.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.Common.ScheduleTalk" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css"/>        
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><span><%=GetResource("WorkTalk")%></span></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="CbtnReload" runat="server"/></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div id="layout_srh" style="display:none;">
        <ul class="btn_searchbox">
            <li><div id="loading"></div></li>
            <li>
            <div class="searchterm">
            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
            <asp:ListItem Value=""></asp:ListItem>
            <asp:ListItem Value="week"></asp:ListItem>
            <asp:ListItem Value="month"></asp:ListItem>
            <asp:ListItem Value="period"></asp:ListItem>
            </asp:DropDownList></div>
            <div id="startEndDate" class="searchterm">
                <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
            </div>
            </li> 
            <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
            <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                        class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
            </li>
            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton></li>  
        </ul>            
        </div>
        <div class="popup_layout_style3" style="padding-top:5px;height:310px;min-height:310px; border:0px solid #f00;">
            
            <div id="placeholder_SRelpyList" style="clear: both;"></div>
            
            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                    <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                    <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="layout_subbox" style="padding:0 20px 0 20px">
            <h1><%=GetResource("ShortReply")%></h1>
            (<span class="charsRemaining"></span>)
            <ul class="btn_right">
            </ul>
            <span style="float:right;padding-right:10px;"><asp:CheckBox ID="chkOpen" runat="server" /> <%=GetResource("ReplyOpen")%></span>
            <textarea name="textarea" id="textarea"  class="textarea" style="width:97%;height:55px;" maxlength="200"></textarea>
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
        var ScheduleID = "<%=ScheduleID %>";
        var DisplayID = "<%=DisplayID %>"; 
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
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Processing = "<%=GetResource("MSG_SUCCESS") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("NeedContents") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_MsgError = "<%= GetResource("ProcessingError")%>";
        var R_ApplyRegDate = "<%= GetResource("InputSearchDay")%>";
        var R_SecurityMsg = "<%= GetResource("NoAuthListData")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/Common/ScheduleTalk.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/SNS/Js/Common/ScheduleTalk.js");
    </script>
</body>
</html>
