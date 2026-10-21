<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReplyList.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.ShortReply.ShortReplyList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>        
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <style type="text/css">
    .ali li{position:relative;width:100%;border-bottom:1px solid #dfdfdf;background:#FFF;}
    .ali .mct{display:block;position:relative;padding:0.2em 10px 0.2em;border:0px solid red;}
    .ali .mct font{display:inline;position:relative;padding:0 5px 0 0; text-align:left;border:0px solid red;}
    .ali a.mct{padding-right:31px;background:url(../Img/bu_lk.gif) no-repeat 100% 50%;border:0px solid red;}
    .ali .lis{display:inline;color:#6B77AC;font-size:0.9em;margin:0 5px 0 0;letter-spacing:0px; border:0px solid red;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <a onclick="ShortReplyList.Search()">
                <h1><%=GetResource("MyTalk")%></h1>
                <img src="/DeskPlusEIP/Design/C/Img/Main/ico_ref.gif" alt="Reload" />
            </a>
            &nbsp;<label id="lblMyInfo" runat="server"></label>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnShortReplyShare" runat="server" /></li> 
                <li class="sec"></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" /></li> 
            </ul>
        </div>
        <div class="dotLine"></div>
        <div id="layout_srh">
        <ul class="btn_searchbox">            
            <li><div id="loading"></div></li>
            <li>
                <div class="searchterm">
                    <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="week"></asp:ListItem>
                    <asp:ListItem Value="month"></asp:ListItem>
                    <asp:ListItem Value="period"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div id="startEndDate" class="searchterm">
                    <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                </div>
            </li> 
            <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
            <li>
                <input type="text" id="dummy" style="display: none;" />
                <cc3:ClientSmartTextBox ID="ctbSearch" runat="server" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
            </li>
            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>  
        </ul>           
        </div>
        <div class="popup_layout_style3" style="padding-top:5px;height:368px;min-height:368px; border:0px solid #f00;">
            <div id="scrollDiv" >
            
            <div id="placeholder_ShortRelpyList" style="clear: both;"></div>
            
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

            <div class="layout_subbox"">
                <h2><%=GetResource("WallPost")%></h2>
                (<span class="charsRemaining"></span>) 
                <ul class="btn_right">
                    <li><input type="text" id="jobDate" size="10" class="clsDate" style="display:none"/></li>
                    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                </ul>
                <span style="float:right;padding-right:10px;"><asp:CheckBox ID="chkOpen" runat="server" /> <%=GetResource("Open")%></span>
            </div>
            <textarea name="textarea" id="textarea"  class="textarea" style="width:98%;height:55px;" maxlength="200"></textarea>
        </div>
    </div>
</div>
    </form>
    <!-- ############# start of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var RUserID = "<%=RUserID %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수                
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
        var R_Add_InputDate = "<%=GetResource("Add_InputDate") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_MsgError = "<%= GetResource("MsgError")%>";
        var R_ApplyRegDate = "<%= GetResource("InputSearchDay")%>";
        var R_SecurityMsg = "<%= GetResource("SecurityMsg")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js" ></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/ShortReply/ShortReplyList.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/ShortReply/ShortReplyList.js");
    </script>
</body>
</html>
