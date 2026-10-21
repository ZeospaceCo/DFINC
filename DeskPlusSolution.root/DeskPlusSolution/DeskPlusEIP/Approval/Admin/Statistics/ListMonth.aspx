<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMonth.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Statistics.ListMonth" %>
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
            <h1><%=GetResource("StatTitle")%></h1>
            <!-- 버튼 //-->
            <span class="app_subtxt" style="display:none"> <%=GetResource("StatSub")%></span>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnExcel" runat="server" Text="엑셀보기"></cc3:ClientSmartButton></li>
                <li style="display:none;"><cc3:ClientSmartButton ID="cbtnGraph" runat="server" Text="그래프보기"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        
        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="2013"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>
                <li>
                    <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </li>   
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder=""></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- Tab //-->
                    <div class="layout_srh clearfix">

                        <div class="float-left">
                            <ul class="tab">
                                <li style="border-bottom: 0px;"><a href="javascript:ListMonth.SetTab('Form');" class="tab_on" id="tabForm"><%=GetResource("StatMonthForm")%></a></li>
                                <li style="border-bottom: 0px;"><a href="javascript:ListMonth.SetTab('Dept');" id="tabDept"><%=GetResource("StatMonthDept")%></a></li>
                                <li style="border-bottom: 0px;" class="last"><a href="javascript:ListMonth.SetTab('User');" id="tabUser"><%=GetResource("StatMonthUser")%></a></li>
                            </ul>
                
                        </div>

                    </div>

                    <!-- Contents //-->
                    <div class="app_statistics_cont">

                        <div id="placeholder"></div>

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

                    </div>
                </div><!-- end col-3 content-area-body-right -->
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SearchField = "<%=SearchField %>";      //검색키워드
        var SearchValue = "<%=SearchValue %>";      //검색어
        var CurrentPage = "<%=CurrentPage %>";      //현재페이지 (PageNo)
        var TotalItemCount = "<%=TotalItemCount %>";    //게시물수 (전체)
        var RecordPerPage = "<%=RecordPerPage %>";      //보여줄게시물수 (PageSize)
        var TabMenu = "<%=TabMenu %>";                  //양식별(Form), 부서별(Dept), 개인별(User)
        var SelYears = "<%=SelYears %>";                  

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        
        // 다국어
        var R_Previous  = "<%=GetResource("Previous") %>";
        var R_Next      = "<%=GetResource("Next") %>";
        var R_StatMonth0 = "<%=GetResource("StatMonth0") %>";  //양식명/문서상태
        var R_StatMonth1 = "<%=GetResource("StatMonth1") %>";  //	1월
        var R_StatMonth2 = "<%=GetResource("StatMonth2") %>";  //	2월
        var R_StatMonth3 = "<%=GetResource("StatMonth3") %>";  //	3월
        var R_StatMonth4 = "<%=GetResource("StatMonth4") %>";  //	4월
        var R_StatMonth5      = "<%=GetResource("StatMonth5") %>";  //	5월
        var R_StatMonth6      = "<%=GetResource("StatMonth6") %>";  //	6월
        var R_StatMonth7      = "<%=GetResource("StatMonth7") %>";  //	7월
        var R_StatMonth8      = "<%=GetResource("StatMonth8") %>";  //	8월
        var R_StatMonth9      = "<%=GetResource("StatMonth9") %>";  //	9월
        var R_StatMonth10      = "<%=GetResource("StatMonth10") %>";  //	10월
        var R_StatMonth11      = "<%=GetResource("StatMonth11") %>";  //	11월
        var R_StatMonth12      = "<%=GetResource("StatMonth12") %>";  //	12월
        var R_StatTot      = "<%=GetResource("StatTot") %>";  //	총
        var R_StatUnit      = "<%=GetResource("StatUnit") %>";  //	건
        var R_StatC      = "<%=GetResource("StatC") %>";  //	완료
        var R_StatW      = "<%=GetResource("StatW") %>";  //	진행
        var R_StatR      = "<%=GetResource("StatR") %>";  //	반려
        var R_StatD      = "<%=GetResource("StatD") %>";  //	보류
        var R_StatSum      = "<%=GetResource("StatSum") %>";  //	합계
        var R_NoContent = "<%=GetResource("NoContent") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Statistics/ListMonth.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Statistics/ListMonth.js");
    </script>
</body>
</html>
