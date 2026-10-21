<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Auth.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=HeaderTitle %></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />            
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                </ul>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control" /></li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox ID="ctbSearch" runat="server" InputMode="None" Width="200px" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <!-- 테이블 시작 //-->
                    <div id="placeholder" style="clear: both;"></div>
                    <!-- 테이블 끝 -->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3>
                            <span id="pageInfo"></span>
                        </h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var RoleID = "<%=RoleID %>";
        var Gubun = "<%=Gubun %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_AuthCode = "<%=GetResource("AuthCode") %>";
        var R_AuthName = "<%=GetResource("AuthName") %>";
        var R_UserNameDeptName = "<%=GetResource("UserNameDeptName") %>";
        var R_Section = "<%=GetResource("Section") %>";
        var R_CheckDeleteList = "<%=GetResource("CheckDeleteList") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
        var R_Users = "<%=GetResource("Users") %>";
        var R_Dept = "<%=GetResource("Dept") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Auth/List.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Auth/List.js");
    </script>
</body>
</html>
