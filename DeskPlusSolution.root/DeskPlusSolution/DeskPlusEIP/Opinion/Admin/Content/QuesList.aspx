<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuesList.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
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
        var MenuID      = "<%=MenuID%>";    //메뉴
        var Section     = "<%=Section%>";    //메뉴
        var TotCnt      = "<%=TotCnt%>";    //총 게시물수
        var PageSize    = "<%=PageSize%>";  //보여질 게시물수
        var PageNo      = "<%=PageNo %>";   //현재페이지
        var SKey        = "<%=SKey %>";     //검색키워드
        var SVal        = "<%=SVal %>";     //검색어
        var SPeriod     = "<%=SPeriod%>";   //기간검색
        var SPeriodVal  = "<%=SPeriodVal%>"; //기간검색어
        var Order       = "<%=Order %>";    //정렬
        var PageUrl = "<%=Request.Url.ToString()%>";
        var R_Msg_ErrDate       = "<%=GetResource("Msg_ErrDate") %>";
        var R_Msg_ErrSEDate     = "<%=GetResource("Msg_ErrSEDate") %>";
        var R_Msg_ErrDelItem    = "<%=GetResource("Msg_ErrDelItem") %>";
        var R_Msg_Del           = "<%=GetResource("Msg_Del") %>";
        var R_Msg_ErrActive     = "<%=GetResource("Msg_ErrActive") %>";
        var R_Msg_Active        = "<%=GetResource("Msg_Active") %>";
        var R_JTF1 = "<%=GetResource("JTF1") %>";
        var R_JTF2 = "<%=GetResource("JTF2") %>";
        var R_JTF3 = "<%=GetResource("JTF3") %>";
        var R_JTF4 = "<%=GetResource("JTF4") %>";
        var R_JTF5 = "<%=GetResource("JTF5") %>";
        var R_JTF6 = "<%=GetResource("JTF6") %>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";
        var R_Result = "<%=GetResource("TitF16") %>";
        var ImagePath   = "<%=BaseImgPath%>";        
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";             
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/QuesList.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/QuesList.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=PageTitle%></h1>
            <!-- 버튼 //-->

            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnActive" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li>
                    <div id="loading"></div>
                </li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlSchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </div>
                    <div id="divPeriod" class="searchterm">
                        <input type="text" id="sPerDate" size="10" class="clsDate searchtext  form-control" />-<input type="text" id="ePerDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li>
                    <asp:DropDownList ID="ddlSrh" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </li>
                <li>
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSrh" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    <div id="divDate"><input type="text" id="sDate" size="10" class="clsDate searchtext  form-control" />-<input type="text" id="eDate" size="10" class="clsDate searchtext  form-control" /></div>
                </li>
                <li>
                    <cc3:ClientSmartButton ID="cbtnSrh" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton>
                </li>
            </ul>
        </div>

                    
        <div id="layout_content"><!-- 컨텐츠 하단영역 -->
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->           

                    <!-- 테이블 시작 //-->
                    <div id="placeholder" style="clear: both;"></div>

                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1">
                        <h3><span id="pageInfo"></span></h3>
                        <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->        

                </div><!-- end col content-area-body -->
            </div><!-- end row  -->
        </div><!-- end layout_content  -->
    
     </div><!-- end layout_full  -->
</div><!-- end layout_body  -->

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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </form>
</body>
</html>
