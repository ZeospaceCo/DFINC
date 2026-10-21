<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserListByDept.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Common.UserListByDept" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var DeptID = "<%=DeptID %>";        

        var SearchGubun = "<%=SearchGubun %>"; //검색구분자  
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬

        var Callback = "<%=Callback %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PageUrl = "<%= Request.Url.ToString() %>";

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        
        var R_Admin_UserList_MSG_HTName = "<%=GetResource("Admin_UserList_MSG_HTName") %>";        
        var R_Private_AccountInfo_ltMobile = "<%=GetResource("Private_AccountInfo_ltMobile") %>";
        var R_Private_AccountInfo_ltUserDept = "<%=GetResource("Private_AccountInfo_ltUserDept") %>";
        var R_Private_AccountInfo_ltTitle = "<%=GetResource("Private_AccountInfo_ltTitle") %>";
        var R_Private_AccountInfo_ltDuty = "<%=GetResource("Private_AccountInfo_ltDuty") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

        var R_RegisterDeptName = "<%=GetResource("RegisterDeptName")%>";
        var R_DeleteDept = "<%=GetResource("DeleteDept")%>";
        var R_CompleteOrder = "<%=GetResource("CompleteOrder")%>";
        var R_Admin_DeptManager_cbActive = "<%=GetResource("Admin_DeptManager_cbActive")%>";
        
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Account/Js/Common/UserListByDept.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">    
        <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("CheckMember")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li><asp:DropDownList ID="ddlSearchGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div class="popup_layout_style3" style="overflow:hidden;"> 
                <br />
                <div class="layout_scroll" style="min-height:450px;height:450px;">
                    <!-- 테이블 시작 //-->
                    <div id="placeholder" style="clear: both;"></div>
                    <!-- 테이블 끝 //-->
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3>
                            <span id="pageInfo"></span>
                        </h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                            <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                            <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>             
            </div>
        </div>
    </div> 
    </form>
</body>
</html>
