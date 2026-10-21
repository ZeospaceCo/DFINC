<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AccessGrade")%></h1>
            <!-- 버튼 //-->
            <b><font color="red"> * <%=GetResource("AccessGradeDeptN")%><%=GetResource("TitleName2")%> (<span id="spnAccessGradeDeptN" runat="server"></span>)</font></b>     
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnActive" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnSeq" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnBase" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
            </ul>            
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                <li>
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder=""></cc3:ClientSmartTextBox>
                    <input type="text" id="dummy" style="display: none;" />
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
            
                    <div id="placeholder" style="clear: both;"></div>
            
                    <!-- 테이블 끝 //-->

                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->
                </div><!-- end col content-area-body-left -->
            </div><!-- end row  -->
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
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
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
        var SearchField     = "<%=SearchField %>";      //검색키워드
        var SearchValue     = "<%=SearchValue %>";      //검색어
        var CurrentPage     = "<%=CurrentPage %>";      //현재페이지 (PageNo)
        var TotalItemCount  = "<%=TotalItemCount %>";   //게시물수 (전체)
        var RecordPerPage   = "<%=RecordPerPage %>";    //보여줄게시물수 (PageSize)
        var Order           = "<%=Order %>";            //정렬
            
        var ImagePath       = "<%=BaseImgPath%>";  
        var PreLang         = "<%=PreLang%>";
        var PreLangReal     = "<%=PreLangReal %>"; //ja     

        var R_AccessGradeName = "<%=GetResource("AccessGradeName") %>";     //열람등급명
        var R_AccessGradeDept = "<%=GetResource("AccessGradeDept") %>";     //열람지정
        var R_IsActiveTit = "<%=GetResource("IsActiveTit") %>";             //사용여부
        var R_IsBase = "<%=GetResource("IsBase") %>";                       //기본값여부
        var R_NoContent = "<%=GetResource("NoContent") %>";                 //데이터 없음        
                
        var R_MSG_SelItem = "<%=GetResource("MSG_SelItem") %>";             //"항목을 선택하세요.";
        var R_MSG_DoActive = "<%=GetResource("MSG_DoActive") %>";           //"활성/비활성화 하시겠습니까?";
        var R_Msg_Del = "<%=GetResource("Msg_Del") %>";                     //"삭제하시겠습니까?";
        var R_DelAccessGradeCheck = "<%=GetResource("DelAccessGradeCheck") %>";
                
        var R_Previous  = "<%=GetResource("Previous") %>";
        var R_Next      = "<%=GetResource("Next") %>";
        
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";

        var R_MsgAccessGrade1 =  "<%=GetResource("MsgAccessGrade1")%>";
        var R_MsgAccessGrade2 =  "<%=GetResource("MsgAccessGrade2")%>";
        var R_MsgAccessGrade3 =  "<%=GetResource("MsgAccessGrade3")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/AccessGrade/List.js"></script>       
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/AccessGrade/List.js");
    </script>

</body>
</html>
