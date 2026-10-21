<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Statistics.Web.Content.Login.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Statistic.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Statistic.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">

<div id="layout_body" class="write">
    <div class="layout_one">

        <!-- 상단 = 타이블 + 버튼 //-->
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("LoginTitle")%></h1>
            <span class="app_subtxt"> * <%=GetResource("LoginDes")%></span>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnExcel" runat="server" Text="엑셀보기"></cc3:ClientSmartButton></li>
                <li style="display:none;"><a href="#" id="cbtnGraph" title="그래프보기"><span>그래프보기</span></a></li>
            </ul>        
                
            <div class="gap5"></div>
            <table class="table_typeB">
            <colgroup>
                <col width="130px" />
                <col />
                <col width="140px" />
                <col />
                <col width="130px" />
                <col />
                <col width="140px" />
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("LoginTotalCount")%></th>
                    <td class="txt_point"><asp:Label ID="lblLoginTotal"  runat="server"></asp:Label></td>
                    <th><%=GetResource("LoginFailTotalCount")%></th>
                    <td class="txt_point"><asp:Label ID="lblLoginFailTotal"  runat="server"></asp:Label></td>
                    <th><%=GetResource("MLoginTotalCount")%></th>
                    <td class="txt_point"><asp:Label ID="lblMLoginTotal"  runat="server"></asp:Label></td>
                    <th><%=GetResource("MLoginFailTotalCount")%></th>
                    <td class="txt_point"><asp:Label ID="lblMLoginFailTotal"  runat="server"></asp:Label></td>
                </tr>
            </tbody>
            </table>

        </div>
        <!-- 상단 = 타이블 + 버튼 //-->
        
        <!-- 검색 //-->
        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li><div class="searchterm"><asp:DropDownList ID="ddlSearchYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div></li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>    
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
            </ul>
        </div>
        <!-- 검색 //-->

        <div id="layout_content"  style="padding-right:30px">
        <br />
        <!-- 탭 //-->
        <div class="stat_tab">

            <div  class="layout_srh clearfix">
                <ul class="tab" style="margin-left:30px !important">
                    <li><a href="javascript:List.SetTab('LoginS');" class="tab_on" id="LoginS"><%=GetResource("LoginCountHis")%></a></li>
                    <li><a href="javascript:List.SetTab('LoginF');" id="LoginF"><%=GetResource("LoginFailCountHis")%></a></li>
                    <li><a href="javascript:List.SetTab('MLoginS');" id="MLoginS"><%=GetResource("MLoginCountHis")%></a></li>
                    <li class="last"><a href="javascript:List.SetTab('MLoginF');" id="MLoginF"><%=GetResource("MLoginFailCountHis")%></a></li>
                </ul>
            </div>

        </div>
        <!-- 탭 //-->

        <!-- Contents //-->
        <div class="stat_cont">

            <div id="plShareUser"></div>

            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                    <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                    <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->

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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount%>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var TabMenu = "<%=TabMenu %>"; 
        var SearchTerm = "";
        var SearchTermValue = "";
        var SearchField = "";
        var SearchField = "";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_UserName = "<%=GetResource("UserName") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_UserNM = "<%=GetResource("UserNM") %>";                     // 사용자명
        var R_TotalLoginCount = "<%=GetResource("TotalLoginCount") %>";   // 총 접속수
        var R_1Month = "<%=GetResource("1Month") %>";
        var R_2Month = "<%=GetResource("2Month") %>";
        var R_3Month = "<%=GetResource("3Month") %>";
        var R_4Month = "<%=GetResource("4Month") %>";
        var R_5Month = "<%=GetResource("5Month") %>";
        var R_6Month = "<%=GetResource("6Month") %>";
        var R_7Month = "<%=GetResource("7Month") %>";
        var R_8Month = "<%=GetResource("8Month") %>";
        var R_9Month = "<%=GetResource("9Month") %>";
        var R_10Month = "<%=GetResource("10Month") %>";
        var R_11Month = "<%=GetResource("11Month") %>";
        var R_12Month = "<%=GetResource("12Month") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Statistics/Js/Content/LogIn/List.js"></script>
</body>
</html>