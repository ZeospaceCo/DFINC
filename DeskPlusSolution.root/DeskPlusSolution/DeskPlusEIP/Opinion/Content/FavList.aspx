<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FavList.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Content.FavList" %>

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
        var MenuID = "<%=MenuID%>";         //메뉴
        var TotCnt = "<%=TotCnt%>";         //총 게시물수
        var PageSize = "<%=PageSize%>";     //보여질 게시물수
        var PageNo = "<%=PageNo %>";        //현재페이지
        var SKey = "<%=SKey %>";            //검색키워드
        var SVal = "<%=SVal %>";            //검색어
        var SPeriod = "<%=SPeriod%>";       //기간검색
        var SPeriodVal = "<%=SPeriodVal%>"; //기간검색어
        var Order = "<%=Order %>";          //정렬
        var ListType = "<%=ListType %>";    //리스트 형식 일반리스트(1) / 블로그(2) / 앨범(3)
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PageUrl = "<%=Request.Url.ToString()%>";
        var R_Msg_ErrDate = "<%=GetResource("Msg_ErrDate") %>";
        var R_Msg_ErrSEDate = "<%=GetResource("Msg_ErrSEDate") %>";
        var R_TitF1 = "<%=GetResource("TitF1") %>";
        var R_TitF2 = "<%=GetResource("JTF2") %>";
        var R_TitF3 = "<%=GetResource("JTF4") %>";
        var R_TitF4 = "<%=GetResource("TitIsOpen") %>";
        var R_TitF5 = "<%=GetResource("TitReply") %>";
        var R_TitF6 = "<%=GetResource("VoteUser") %>";
        var R_TitF7 = "<%=GetResource("JTF25") %>";
        var R_TitF8 = "<%=GetResource("JTF26") %>";
        var R_TitF9 = "<%=GetResource("TitF25") %>";
        var R_TitF10 = "<%=GetResource("Tit_Person") %>";
        var R_Secret = "<%=GetResource("Secret") %>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Content/FavList.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Content/FavList.js");
    </script>
</head>
<body>

<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MenuF1")%> <%=GetResource("TitF22")%></h1>
            <!-- 버튼 //-->
        </div>


        <div id="layout_srh">
            <ul id="TypeGubun" class="btn_searchImg">
                <li id="board"><a id="listType1" href="#" class="board" title="<%=GetResource("ListType")%>"><i class="fas fa-bars"></i></a></li>
                <li id="bolg"><a id="listType2" href="#" class="blog" title="<%=GetResource("BlogListType")%>"><i class="fas fa-list-ul"></i></a></li>
                <!-- li class="album"><a href="#" class="album" title="앨범형리스트"><i class="fas fa-th-large "></i></a></li-->
                
            </ul>
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm"><asp:DropDownList ID="ddlSchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div>
                    <div id="divPeriod" class="searchterm"><input type="text" id="sPerDate" size="10" class="clsDate searchtext  form-control" />-<input type="text" id="ePerDate" size="10"  class="clsDate searchtext  form-control" /></div>
                </li>
                <li><asp:DropDownList ID="ddlSrh" runat="server" CssClass="searchselect  form-control"></asp:DropDownList></li>
                <li>
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSrh" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    <div id="divDate"><input type="text" id="sDate" size="10" class="clsDate searchtext  form-control" />-<input type="text" id="eDate" size="10" class="clsDate searchtext  form-control" /></div>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSrh" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
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
