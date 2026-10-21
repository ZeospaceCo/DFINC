<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Proxy.List" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("SetProxy") %></h1>
                <!-- 버튼 //-->
                <span class="app_caption"><%=GetResource("ProxyAdminSubTxt")%></span>
                <ul class="btn_right">
                    <li><a id="Add" class="btn btn-secondary"><%=GetResource("AddProxy") %></a></li>
                    <li><a id="Del" class="btn btn-danger"><%=GetResource("DelProxy") %></a></li>
                </ul>
            </div>
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
                    <div id="startEndDate" class="searchterm"><input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" /></div>
                    </li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                        class="searchtext" placeholder=""></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" BeginEndTag="li" Class="searchbtn btn-secondary" /></li>         
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body pt-0 mt-4" ><!-- width 100%(col-12) -->
                        <!-- 테이블 시작 //-->
                        <div id="placeholder" style="clear: both;"></div>
                        <!-- 테이블 끝 //-->

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                            <h3><span id="pageInfo"></span></h3>
	                        <span id="pager"></span>
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10 " Value="10" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <!-- 페이징 끝 //-->
                
                        <div class="app_subtxt"><%=GetResource("ProxyText6")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText3")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText4")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText5")%></div>
                    </div><!-- end col content-area-body-right -->
                </div><!-- end row  -->
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:TextBox ID="tbxHiddenValue" runat="Server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbxHiddenDeptValue" runat="Server" Width="0"></asp:TextBox>
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
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값 
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어

        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>"; 
        var R_ConfirmDeleteProxyPerson = "<%=GetResource("ConfirmDeleteProxyPerson") %>";
        var R_CheckDeleteProxyPerson = "<%=GetResource("CheckDeleteProxyPerson") %>";

        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";

        var R_Photo = "<%=GetResource("Photo") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_Mail = "<%=GetResource("Mail") %>";
        var R_Mobile = "<%=GetResource("Mobile") %>";
        var R_Cphone = "<%=GetResource("Cphone") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_NoData = "<%=GetResource("NoDataProxy") %>"; 
        var R_Msg_Resource04 = "<%=GetResource("Msg_Resource04") %>"; 
        var R_Msg_Resource05 = "<%=GetResource("Msg_Resource05") %>"; 
        var R_Msg_Resource06 = "<%=GetResource("Msg_Resource06") %>";
        var R_Msg_Resource07  = "<%=GetResource("Msg_Resource07") %>";
        var R_Msg_Resource08  = "<%=GetResource("Msg_Resource08") %>";
        var R_Msg_Resource09  = "<%=GetResource("Msg_Resource09") %>";
        var R_StateInfo = "<%=GetResource("EnforceStateInfo") %>"; 
        var R_IsModfy = "<%=GetResource("IsModfy") %>"
        var R_TargetUser = "<%=GetResource("TargetUser") %>";

        var R_ProxyDate = "<%=GetResource("AbsencePeriod")%>";
        var R_ProxyType = "<%=GetResource("TypeAbsence")%>";
        var R_AllDay = "<%=GetResource("AllDay")%>";

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   

        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
       
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Proxy/List.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Proxy/List.js");
    </script>

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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
</body>
</html>
