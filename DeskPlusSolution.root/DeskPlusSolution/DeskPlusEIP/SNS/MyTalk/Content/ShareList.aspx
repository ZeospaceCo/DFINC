<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareList.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.MyTalk.Content.ShareList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
   <form id="form1" runat="server">
<div class="fixedlayout" id="layout_body">
    <div class="layout_full">
    
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><span id="lblMyInfo" runat="server"></span> <%=GetResource("ShareTalkTitle")%></h1>  
            <!-- 버튼 //-->         
            <ul class="btn_right">            
            <li><cc3:ClientSmartButton ID="cbtnMy" runat="server"/></li>
            <li><cc3:ClientSmartButton ID="CbtnReload" runat="server"/></li>
            <li><cc3:ClientSmartButton ID="cbtnOpen" runat="server" /></li> 
            <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false" /></li> 
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg"></ul>
            <ul class="btn_searchbox">
                <li>
                    <div id="loading"></div>
                </li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server"  class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>


        <div id="layout_content">
            <div class="row">
            <div class="col-9 content-area-body-left"><!-- width 100%(col-12) -->

                <!-- 테이블 시작 //-->
                <div id="phList" style="clear: both;"></div>     
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
            </div>

            <div class="col-3 content-area-body-right"><!-- width 100%(col-12) -->
                <ul>
                    <li><span class="subtextH1"><%=GetResource("TalkDes")%></span></li>
                    <li>
                        <!-- 글 작성영역 -->
                        <div id="MyTalkAdd">
                            <div>
                            (<span class="charsRemaining"></span>)
                            <textarea name="textarea" id="textarea" class="textarea" style="width: 99%; height: 55px; border: 1px solid rgb(210, 210, 210);" maxlength="200"></textarea>
                            <div class="pt-1 float-right ">
                                <asp:CheckBox ID="chkOpen" runat="server" /> <%=GetResource("Open")%>
                                <cc3:ClientSmartButton ID="cbtnAddFile" runat="server" />                   
                                <cc3:ClientSmartButton ID="cbtnShortReplyAdd" runat="server"  class="btn btn-secondary" />                   
                            </div>
                         </div>

                            <!-- 첨부 파일 리스트 -->     
                            <div id="phFile" style="clear: both;"></div>
                            <!-- 첨부 파일 리스트 -->   

                        </div>
                        <!-- 글 작성영역 -->
                    </li>
                </ul>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
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
        var RUserID = "<%=RUserID %>";       
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬              
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var MyTalkCheck = "1";
        var TmpGroup = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.bmp;*.tif;*.pjpeg;*.pjpg;*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.bmp *.tif *.pjpeg *.pjpg *.jpg *.jpeg *.png *.gif)";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_NoAuthListData = "<%=GetResource("NoAuthListData") %>";        
        var R_ReallyApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
        var R_NeedContents = "<%=GetResource("NeedContents") %>"; 
        var R_ReplyAdd = "<%=GetResource("ReplyAdd") %>"; 
        var R_ReplyOpen = "<%=GetResource("ReplyOpen") %>"; 
        var R_Save = "<%=GetResource("Save") %>"; 
        var R_AddShortReply = "<%=GetResource("AddShortReply") %>"; 
        var R_ShortReply = "<%=GetResource("ShortReply") %>"; 
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_FileDel = "<%=GetResource("FileDel") %>";
        var R_MsgDel = "<%=GetResource("MsgDel") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/MyTalk/Content/ShareList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/SNS/Js/MyTalk/Content/ShareList.js");
    </script>
</body>
</html>
