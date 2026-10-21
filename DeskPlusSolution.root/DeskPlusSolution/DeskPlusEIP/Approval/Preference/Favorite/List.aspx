<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Favorite.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
      
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("FavoriteApprovalUser") %></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><a href="#" id="ModSeq" class="btn"><span ><%=GetResource("ChangeOrder3")%></span></a></li>
                <li id="modifyInfo" style="display:none"><a href="#" id="ModList" class="btn"><span ><%=GetResource("ModifyList")%></span></a></li>
                <li><a href="#" id="AddDept" class="btn btn-secondary"><span><%=GetResource("FavAddDept")%></span></a></li>
                <li><a href="#" id="Add" class="btn btn-secondary"><span><%=GetResource("FavAddUser")%></span></a></li>
                <li><a href="#" id="Del" class="btn btn-danger"><span><%=GetResource("Del") %></span></a></li>                
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                
                <div class="col-12 content-area-body">
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
                    <span class="app_subtxt"><%=GetResource("FavScription")%></span>
                </div><!-- end col-3 content-area-body-right -->
                
            </div><!-- end row  -->
        </div><!-- end layout_content  -->
        
    </div><!-- end layout_full  -->
</div><!-- end ifm_layout_body  -->

    <div style="display:none;">
    <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbtnAddDept" runat="server" OnClick="lbtnAddDept_Click"></asp:LinkButton>
    <asp:TextBox ID="tbxHiddenValue" runat="Server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbxHdApprovalDeptID" runat="Server" Width="0"></asp:TextBox>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>           
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬      
        var ModifyCount = "<%=ModifyCount %>"; //부서변경된 사용자 수

        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>"; 
        var R_ConfirmDeleteFavoritePerson = "<%=GetResource("ConfirmDeleteFavoritePerson") %>"; 
        var R_CheckDeleteFavoritePerson = "<%=GetResource("CheckDeleteFavoritePerson") %>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_DutyName = "<%=GetResource("DutyName") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_AddDate = "<%=GetResource("AddDate") %>"; 
        var R_NoData = "<%=GetResource("NoData") %>"; 
        var R_PlaceholderText = "<%=GetResource("PlaceholderText") %>"; 
        var R_IsModfy = "<%=GetResource("IsModfy") %>"

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja    
          
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Favorite/List.js?v=1"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Favorite/List.js");
    </script>

</body>
</html>
