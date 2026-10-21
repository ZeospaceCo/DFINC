<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminList.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.AdminList" %>

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
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>
                    <%=GetResource("AllList")%>
                    <div style="display: none">
                        <%=GetResource("WorkFolder")%>
                        <%=GetResource("DocumentTransfer")%>
                        <span class="app_captit">*
                            <%=GetResource("BeforeInfo")%>
                            : <strong><span id="sendInfo"></span></strong>->
                            <%=GetResource("AfterInfo")%>
                            : <strong><span id="receiveInfo"></span></strong></span>
                    </div>
                </h1>
                <!-- 버튼 //-->
                <div style="float: left; display: none">
                    <asp:DropDownList ID="ddlFolder" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </div>
                <!--ul class="btn_left" style="display: none">

                </ul-->
                <ul class="btn_right">
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnBeforeDept" runat="server"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnBeforeDeptGarbage" runat="server"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnBeforeUser" runat="server"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnAfterDept" runat="server"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnAfterUser" runat="server"></cc3:ClientSmartButton>
                    </li>
                    
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnTransfer" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnTransferAll" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnCopy" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton>
                    </li>
                    <li style="display: none">
                        <cc3:ClientSmartButton ID="cbtnCopyAll" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton>
                    </li>
                    <li>
                        <cc3:ClientSmartButton ID="cbtnDel" runat="server"  class="btn btn-danger"></cc3:ClientSmartButton>
                    </li>
                </ul>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <!-- 테이블 시작 //-->
                        <div id="placeholderAdminList" style="clear: both;"></div>
                        <!-- 테이블 끝 //-->
                        <!-- 페이징 시작 //-->
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
                        <!-- 페이징 끝 //-->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var Years = "<%=Years %>";
        var Months = "<%=Months %>";
        var Days = "<%=Days %>";
        var Weeks = "<%=Weeks %>";
        var BeforeAfter = ""; //변경전,후 구분
        var TargetUserID = "0"; //변경전 사용자 아이디
        var TargetDeptID = "0"; //변경전 부서 아이디
        var AfterUserID = "0"; //변경후 사용자 아이디
        var AfterDeptID = "0"; //변경후 부서 아이디
        var Flag = "<%=Flag %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var ListType = "<%=ListType %>";
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "";
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
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_SelectMoveList = "<%=GetResource("SelectMoveList") %>";
        var R_SelectDeptAfterDept = "<%=GetResource("SelectDeptAfterDept") %>";
        var R_SelectUserAfterUser = "<%=GetResource("SelectUserAfterUser") %>";
        var R_SelectDeptBeforeDept = "<%=GetResource("SelectDeptBeforeDept") %>";
        var R_SelectUserBeforeUser = "<%=GetResource("SelectUserBeforeUser") %>";
        var R_SelectCopyList = "<%=GetResource("SelectCopyList") %>";
        var R_SelectDeleteList = "<%=GetResource("SelectDeleteList") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_SelectUser = "<%=GetResource("SelectUser") %>";
        var R_Linked = "<%=GetResource("Linked") %>";
        var R_MoveUser = "<%=GetResource("LinkedMoveUser") %>";
        var R_CopyUser = "<%=GetResource("LinkedCopyUser") %>";
        var R_MoveDept = "<%=GetResource("LinkedMoveDept") %>";
        var R_CopyDept = "<%=GetResource("LinkedCopyDept") %>";
        var R_Report = "<%=GetResource("Work") %>";
        var R_Inform = "<%=GetResource("Inform") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/AdminList.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Report/Js/Content/AdminList.js");
    </script>
</body>
</html>