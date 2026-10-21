<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Admin.Content.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
   <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=SiteMap%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnMoveSubFolder" runat="server" BeginEndTag="li" />  
                <cc3:ClientSmartButton ID="CbtnMoveList" runat="server" BeginEndTag="li" />   
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
            </ul>
        </div>

       <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                    <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="12" readonly class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" readonly class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary" /></li>         
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <!-- 테이블 시작 //-->            
                    <div id="phList" style="clear: both;"></div>            
                    <!-- 테이블 끝 //-->

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
                    <!-- 페이징 끝 //-->
                </div>
            </div>
        </div>
    </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTerm = "<%=SearchTerm %>";   //날짜검색키워드
        var SearchTermValue = "<%=SearchTermValue %>";   //기간검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬       
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";
        var R_Qty = "<%=GetResource("Qty") %>";
        var R_ResourcesUser = "<%=GetResource("ResourcesUser") %>";
        var R_WriteDate = "<%=GetResource("WriteDate") %>";
        var R_WriteUserName = "<%=GetResource("WriteUserName") %>";
        var R_DelList = "<%=GetResource("DelList") %>";
        var R_SelectDel = "<%=GetResource("SelectDel") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja    

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
        //var R_ListOrder = "<%=GetResource("ListOrder")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Admin/Content/List.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
//        JsCall("/DeskPlusEIP/RM/Js/Admin/Content/List.js");
    </script>
</body>
</html>
