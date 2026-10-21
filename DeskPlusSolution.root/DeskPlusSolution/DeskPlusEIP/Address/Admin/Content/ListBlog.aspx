<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListBlog.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Admin.Content.ListBlog" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=SiteMap%></h1>                
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnCopy" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnMove" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div class="roundtable_l_b">
                <!--라운드테이블입니다.-좌우,상하 유동적입니다.-->
                <div class="roundtable_l_t">
                    <div class="roundtable_r_b">
                        <div class="roundtable_r_t">
                            <ul class="listAtoZ">
                                <li class="first-child"><a href="#" onclick="javascript:List.SearchFilter('')"><span id="index0"><%=GetResource("Whole")%></span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㄱ')"><span id="index1">ㄱ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㄴ')"><span id="index2">ㄴ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㄷ')"><span id="index3">ㄷ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㄹ')"><span id="index4">ㄹ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅁ')"><span id="index5">ㅁ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅂ')"><span id="index6">ㅂ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅅ')"><span id="index7">ㅅ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅇ')"><span id="index8">ㅇ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅈ')"><span id="index9">ㅈ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅊ')"><span id="index10">ㅊ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅋ')"><span id="index11">ㅋ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅌ')"><span id="index12">ㅌ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅍ')"><span id="index13">ㅍ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('ㅎ')"><span id="index14">ㅎ</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('A')"><span id="index15">A~Z</span></a></li>
                                <li><a href="#" onclick="javascript:List.SearchFilter('1')"><span id="index16">0~9</span></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchImg">                    				
				<li class="on"><a id="listType2" href="#" class="blog" title="<%=GetResource("TooltiplistType2") %>"><i class="fas fa-list-ul"></i></a></li>
                </ul>
                <ul class="btn_searchbox">
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control" />                    </li>
                    <li><input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox ID="ctbSearch" runat="server" InputMode="None" Width="200px" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                     <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div id="simpleAdd" class="list_add_style2">
                <div class="list_add_style4">
                    <input id="ctbName" class="searchtext form-control" type="text" size="20" placeholder="<%=GetResource("FirstName") %>" value="" />
                    <input id="ctbMail" class="searchtext form-control" type="text" size="40" placeholder="<%=GetResource("EmailEx") %>" value="" />
                    <input id="ctbMobilePhone" class="searchtext form-control" type="text" size="30" placeholder="<%=GetResource("CellPhoneEx") %>" value="" />
                    <cc3:ClientSmartButton ID="cbtnSimpleAdd" runat="server" Class="btn-sm btn-secondary" />                
                </div>                
            </div>
		    <div id="layout_content"><!-- 컨텐츠 하단영역 -->
			    <div class="row">
				    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->                
                
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
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID %>";
        var Types = "<%=Types %>";        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var SearchKey = "<%=SearchKey %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchFilter = "<%=SearchFilter %>"; //검색ㄱㄴㄷㄹ
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_Both = "<%=GetResource("Both") %>";
        var R_CompanyAddress = "<%=GetResource("CompanyAddress") %>";
        var R_CommonAddress = "<%=GetResource("CommonAddress") %>";
        var R_PrivateAddress = "<%=GetResource("PrivateAddress") %>";
        var R_ForwardAddress = "<%=GetResource("ForwardAddress") %>";
        var R_TitleAddress = "<%=GetResource("TitleAddress") %>";
        var R_DutyAddress = "<%=GetResource("DutyAddress") %>";
        var R_InputFitstName = "<%=GetResource("InputFitstName") %>";
        var R_NotCorrectEmailFormat = "<%=GetResource("NotCorrectEmailFormat") %>";
        var R_CellPhoneFormat = "<%=GetResource("CellPhoneFormat") %>";
        var R_AfterProcessingError = "<%=GetResource("AfterProcessingError") %>";
        var R_SelectCopyAddress = "<%=GetResource("SelectCopyAddress")%>";
        var R_SelectMoveAddress = "<%=GetResource("SelectMoveAddress")%>";
        var R_SelectDeleteAddress = "<%=GetResource("SelectDeleteAddress")%>";
        var R_CompleteDeleteAddress = "<%=GetResource("CompleteDeleteAddress")%>";
        var R_SelectSendAddress = "<%=GetResource("SelectSendAddress")%>";
        var R_ReallyDel = "<%=GetResource("ReallyDel")%>";
        var R_Complete = "<%=GetResource("Complete")%>";
        var R_Picture = "<%=GetResource("Picture") %>";
        var R_FirstName = "<%=GetResource("FirstName") %>";
        var R_Mail = "<%=GetResource("E-Mail") %>";
        var R_CellPhone = "<%=GetResource("CellPhone") %>";
        var R_CompanyTel = "<%=GetResource("CompanyTel") %>";
        var R_Dept = "<%=GetResource("Dept") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>

    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Admin/Content/ListBlog.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Admin/Content/ListBlog.js");
    </script>
</body>
</html>
