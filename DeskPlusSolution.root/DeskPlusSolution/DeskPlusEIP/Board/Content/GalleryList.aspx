<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GalleryList.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.GalleryList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>
                    <%= CategoryTitle%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnOrder" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnInterested" runat="server" BeginEndTag="li" />   
                    <cc3:ClientSmartButton ID="cbtnFavor" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnPortlet" runat="server" BeginEndTag="li" />    
                    <cc3:ClientSmartButton ID="cbtnInfo" runat="server" BeginEndTag="li" />     

                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" />
                </ul>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchImg">
                    <li><a id="listType1" href="#" class="board" title="<%=GetResource("TooltiplistType1") %>"><i class="fas fa-bars"></i></a></li>
                    <li><a id="listType2" href="#" class="blog" title="<%=GetResource("TooltiplistType2") %>"><i class="fas fa-list-ul"></i></a></li>
                    <li class="on"><a id="listType3" href="#" class="album" title="<%=GetResource("TooltiplistType3") %>"><i class="fas fa-th-large "></i></a></li>
                </ul>
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li><asp:DropDownList ID="ddlSearchGroup" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li>
                        <div class="searchterm"><asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="12" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" class="clsDate searchtext form-control" />
                        </div>
                    </li>
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <!-- 테이블 시작 //-->
                        <div class="table_typeLtop"><p></p></div>
                        <div id="placeholder" style="clear: both;"></div>
                        <div class="table_typeLbottom"><p></p></div>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
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
        var Dcparm = "<%=Dcparm %>";
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchGroup = "<%=SearchGroup %>"; //분류
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬        
        var OptionUseDept = "<%=OptionUseDept %>";
        var OptionUseView = "<%=OptionUseView %>";
        var OptionListType = "<%=OptionListType %>";
        var FavoriteID = "<%=FavoriteID %>";
        var Types = "<%=Types %>";
        var CatTitle = "<%=CatTitle %>"; 
        var BoaComID = "<%=BoaComID %>";
        var PGubun = "<%=PGubun%>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_Register = "<%=GetResource("Register") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_CategoryName = "<%=GetResource("CategoryName") %>";
        var R_AttachName = "<%=GetResource("AttachName") %>";
        var R_ViewCount = "<%=GetResource("ViewCount") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Notice = "<%=GetResource("Notice") %>";
        var R_Common = "<%=GetResource("Common") %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
        var R_LengthOver = "<%=GetResource("LengthOver") %>";
        var R_SearchTermAL = "<%=GetResource("SearchTermAL") %>";
        var R_SearchTermPE = "<%=GetResource("SearchTermPE") %>";

        var Kind = "<%=Kind %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/GalleryList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("../Js/Content/GalleryList.js");
    </script>
</body>
</html>
