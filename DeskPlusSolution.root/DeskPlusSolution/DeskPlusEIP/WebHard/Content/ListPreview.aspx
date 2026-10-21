<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListPreview.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.ListPreview" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=SiteMap%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnFavorMove" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnOrder" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnFavor" runat="server" BeginEndTag="li" />                
                <cc3:ClientSmartButton ID="CbtnShare" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnSend" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnMove" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnCopy" runat="server" BeginEndTag="li" />

                <li><div id="download" style="display:none"><cc3:ClientSmartButton ID="CbtnDownload" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></div></li>
                <li><cc3:ClientSmartButton ID="CbtnUpload" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <cc3:ClientSmartButton ID="CbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                <cc3:ClientSmartButton ID="CbtnAllDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
            </ul>
        </div>

        <div id="layout_srh">

            <ul class="btn_searchImg">
                <li><a id="listType1" href="#" class="board" title="<%=GetResource("ListType")%>"><i class="fas fa-bars"></i></a></li>
                <li><a id="listType2" href="#" class="blog" title="<%=GetResource("BlogListType")%>"><i class="fas fa-list-ul"></i></a></li>
                <li class="on"><a id="listType3" href="#" class="album" title="<%=GetResource("AlbumListType")%>"><i class="fas fa-th-large "></i></a></li>
            </ul>

            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm"><asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></div>
                    <div id="startEndDate" class="searchterm"><input type="text" id="startDate" size="10" class="searchtext form-control" />-<input type="text" id="endDate" size="10" class="searchtext form-control" /></div>
                </li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" CssClass="searchtext form-control" TitleAtt="검색" placeholder="검색어 입력"></cc3:ClientSmartTextBox> </li>
                <li><cc3:ClientSmartButton ID="CbtnSearch" runat="server" Class="searchbtn btn-secondary" /></li>            
            </ul>

        </div>

        <div id="layout_content">
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
                    <div class="table_typeLtop"><p></p></div>
                    <div class="table_typeLmiddle">
                    <div id="phListPre" style="clear: both;"></div>
                    </div>
                    <div class="table_typeLbottom"><p></p></div>
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
                 </div>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
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
        var FolderID = "<%=FolderID %>"; //선택한 트리 폴더
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTerm = "<%=SearchTerm %>";   //날짜검색키워드
        var SearchTermValue = "<%=SearchTermValue %>";   //기간검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬       
        var PrivateQuota = "<%=PrivateQuota %>"; // 개인에게 할당된 용량
        var ParentFolderID = "<%=ParentFolderID%>";
        var FavoriteID = "<%=FavoriteID%>";
        var ParentFavoriteID = "<%=ParentFavoriteID %>";
        var ChildCnt = "<%=ChildCnt%>";
        var Types = "<%=Types%>";
        var DownCheck = "<%=DownCheck%>";
        var UID = "<%=UID%>";
        var UseCount = "<%=UseCount%>";
        var FileLength = "<%=FileLength%>";    
        var PGubun = "<%=PGubun%>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_RegUser = "<%=GetResource("RegUser") %>";  
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_Download = "<%=GetResource("File") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_SelectSend = "<%=GetResource("SelectSend")%>";
        var R_SelectMove = "<%=GetResource("SelectMove")%>";
        var R_SelectCopy = "<%=GetResource("SelectCopy")%>";
        var R_SelectDel = "<%=GetResource("SelectDel")%>";
        var R_SelectDownload = "<%=GetResource("SelectDownload")%>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_DelTrashFile = "<%=GetResource("DelTrashFile")%>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_UploadCount = "<%=GetResource("UploadCount") %>";
        var R_PublicFolderCount = "<%=GetResource("PublicFolderCount") %>";     
        var R_DownloadAuthority = "<%=GetResource("DownloadAuthority") %>";
        var R_DelFile = "<%=GetResource("DelFile")%>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_DeleteAuthority = "<%=GetResource("DeleteAuthority") %>";
        var R_MoveAuthority = "<%=GetResource("MoveAuthority") %>";
        
        var AccessAuth = "<%=AccessAuth %>";
        var PAdmin = "<%=PAdmin %>";
        var ImagePath = "<%=BaseImgPath%>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/ListPreview.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/WebHard/Js/Content/ListPreview.js");
    </script>
</body>
</html>
