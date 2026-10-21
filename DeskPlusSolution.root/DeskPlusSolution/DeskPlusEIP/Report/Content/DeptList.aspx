<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptList.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.DeptList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("DeptFolder")%></h1>
                <!-- 버튼 //-->
                <!--ul class="btn_left">
                </ul-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Visible="false" BeginEndTag="li" class="btn btn-secondary" />
                </ul>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchImg">
                    <li style="font-weight:600">
                        <%=CurrentUserInfo.DeptName %>
                    </li>
                </ul>
                <ul class="btn_searchbox">
                    <li>
                        <div id="loading">
                        </div>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect  form-control">
                            <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                            <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlWeek" runat="server" CssClass="searchselect form-control"  >
                        </asp:DropDownList>
                    </li>
                    <li>
                        <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control">
                        </asp:DropDownList>
                    </li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                            class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li>
                        <cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton>
                    </li>
                </ul>
            </div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    
                    
                <!-- 테이블 시작 //-->
                <div id="placeholderDeptList" style="clear: both;"></div>
                <!-- 테이블 끝 //-->
                        
                        
                    </div><!-- end col content-area-body-right -->
                </div><!-- end row  -->
            </div><!-- end layout_content  -->
    
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var Years = "<%=Years %>";
        var Months = "<%=Months %>";
        var Days = "<%=Days %>";
        var TargetUserID = "<%=TargetUserID %>";
        var TargetDeptID = "<%=TargetDeptID %>";
        var Weeks = "<%=Weeks %>";
        var Flag = "<%=Flag %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var ListType = "<%=ListType %>";
        var Order = "";
        var Check = "";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Check = "<%=GetResource("Check") %>";
        var R_Write = "<%=GetResource("Write") %>";
        var R_Term = "<%=GetResource("Term") %>";
        var R_Writer = "<%=GetResource("Writer") %>";
        var R_Confirm = "<%=GetResource("Confirm") %>";
        var R_Attach = "<%=GetResource("Attach") %>";
        var R_Short = "<%=GetResource("Short") %>";
        var R_Save = "<%=GetResource("Save") %>";
        var R_Request = "<%=GetResource("Request") %>";
        var R_ConfirmCheck = "<%=GetResource("ConfirmCheck") %>";
        var R_CloseYN = "<%=GetResource("CloseYN") %>";
        var R_IsClose = "<%=GetResource("IsClose") %>";
        var R_IsOpen = "<%=GetResource("IsOpen") %>";
        var R_CheckAdmin = "<%=GetResource("CheckAdmin") %>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_Linked = "<%=GetResource("Linked") %>";
        var R_MoveUser = "<%=GetResource("LinkedMoveUser") %>";
        var R_CopyUser = "<%=GetResource("LinkedCopyUser") %>";
        var R_MoveDept = "<%=GetResource("LinkedMoveDept") %>";
        var R_CopyDept = "<%=GetResource("LinkedCopyDept") %>";
        var R_Report = "<%=GetResource("Work") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/DeptList.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Report/Js/Content/DeptList.js");
    </script>
</body>
</html>