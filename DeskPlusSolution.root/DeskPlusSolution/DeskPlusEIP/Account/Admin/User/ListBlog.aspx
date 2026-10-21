<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListBlog.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.ListBlog" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
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
                <cc3:ClientSmartButton ID="cbtnActive" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnModDept" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnMod" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />   
            </ul>
        </div>

       <div id="layout_srh">
            <ul class="btn_searchImg">
                <li><a id="listType1" href="#" class="board" title=""><i class="fas fa-bars"></i></a></li>
                <li class="on"><a id="listType2" href="#" class="blog" title=""><i class="fas fa-list-ul"></i></a></li>
            </ul>

            <ul class="btn_searchbox">
                <li><%=GetResource("TotalUsers")%> : <span id="spUserCnt""><%=UserCnt%></span> , <%=GetResource("License")%> : <span id="licenseCnt""></span>&nbsp;&nbsp;</li>
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm"><asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li><asp:DropDownList ID="ddlSearchGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary" /></li>         
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->            
                    <div id="placeholder" style="clear: both;"></div>            
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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var DeptID = "<%=DeptID %>"; //카테고리     

        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값 
        var SearchGubun = "<%=SearchGubun %>"; //검색구분자  
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬  

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var PageUrl = "<%= Request.Url.ToString() %>";

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_LoginID = "<%=GetResource("LoginID") %>";
        var R_Admin_UserList_MSG_HTName = "<%=GetResource("Admin_UserList_MSG_HTName") %>";
        var R_MailAddress = "<%=GetResource("MailAddress") %>";
        var R_Private_AccountInfo_ltMobile = "<%=GetResource("Private_AccountInfo_ltMobile") %>";
        var R_Private_AccountInfo_ltUserDept = "<%=GetResource("Private_AccountInfo_ltUserDept") %>";
        var R_Private_AccountInfo_ltTitle = "<%=GetResource("Private_AccountInfo_ltTitle") %>";
        var R_Private_AccountInfo_ltDuty = "<%=GetResource("Private_AccountInfo_ltDuty") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_CheckDeleteList = "<%=GetResource("CheckDeleteList") %>";
        var R_DeleteInfo = "<%=GetResource("DeleteInfo") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_CheckList = "<%=GetResource("CheckList") %>";
        var R_CheckChangeDeptUserList = "<%=GetResource("CheckChangeDeptUserList") %>";
        var R_MyPhoto = "<%=GetResource("MyPhoto") %>";
        var R_Admin_DeptManager_cbActive = "<%=GetResource("Admin_DeptManager_cbActive") %>";
        var R_Admin_UserManager_MSG_NotSelectRootNode = "<%=GetResource("Admin_UserManager_MSG_NotSelectRootNode") %>";        
        var R_Both = "<%=GetResource("Both") %>";
        var R_SUCCESS = "<%=GetResource("SUCCESS") %>"; 
        var R_LicenseError = "<%=GetResource("License_Error")%>";
        var R_UserDeptOrder  = "<%=GetResource("Admin_UserManager_GridHeader5")%>";

        var UserCnt = "<%=UserCnt%>";
        var LicenseCnt = 0;

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="../../Js/Admin/User/ListBlog.js"></script>
     <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
         //JsCall("../../Js/Admin/User/ListBlog.js");         
    </script>
</body>
</html>
