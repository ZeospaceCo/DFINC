<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPopupImpo.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.ViewPopupImpo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
<form id="form1" runat="server">
    <div class="layer_popup view">
        <div class="inside">
            <div class="popup_header">
                <h1><%= CategoryTitle%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnRead" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnRate" runat="server" BeginEndTag="li" />

                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />                
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnMail" runat="server" BeginEndTag="li" Visible="false" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine pb-0 mb-0"></div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->

                <div class="row">
                    <div class="col-12 col-lg-8 content-area-body-left" ><!-- width 12,9,8/12 -->

                        <div id="toPrint">
                        <!--기본정보-->
                        <table class="table_typeB">
                        <caption>List</caption>
                        <colgroup>
                        <col width="80px" />
                        <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("Title") %></th>
                            <td class="wrap"><asp:Label title="" ID="lbTitle" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="last-child"><%=GetResource("View_RegInfo") %></th>
                            <td class="last-child">
                                <%=GetResource("Register")%>
                                :
                                    <asp:Literal ID="lbWriteInfo" runat="server"></asp:Literal>
                                    &nbsp;/&nbsp;
                                    <%=GetResource("RegDate")%>
                                    :
                                    <asp:Label title="" ID="lbWriteDate" runat="server"></asp:Label>&nbsp;/&nbsp;
                                    <%=GetResource("ViewCount")%>
                                    :
                                    <asp:Label title="" ID="lbReadCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        </tbody>
                        </table>
                        <!--기본정보-->
            
                        <!--본문정보-->
                        <div class="content_box"  style="overflow-y:hidden">
                        <table cellspacing="0" style="min-height:300px;width:100%;">
                            <tr>
                                <td valign="top">
                                    <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        </div>
                        <!--본문정보-->
                        </div>
                   </div><!-- end col content-area-body-left -->            
                   <div class="col-12 col-lg-4 content-area-body-right"  ><!-- width 12,3,4/12 -->
                     <ul> 
                        <!--첨부파일-->
                        <li class="download">
                            <ul>
                                <li class="clearfix"><cc3:ClientSmartButton ID="cbtnShowImage" runat="server" BeginEndTag="li" class="btn btn-sm  bg-white" /></li>  
                            </ul>  
                            <ul id="msgDown">
                                <li><p id="DownLoadMessage" class="text-danger clearfix clear-left" style="display:blcok !important;" ></p></li>
                            </ul>           
                        </li>
                        <li class="title"><%=GetResource("AttachName")%></li> 
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        
                        <li class="title" ID="liReply" runat="server"><%=GetResource("ShortReply")%>
							<cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                        </li>
                        <li ID="liReplyList" runat="server" class="reply"><div id="placeholder" style="clear: both;"></div></li>

                     </ul>                             
                 </div><!-- end col content-area-body-right -->                        
              </div><!-- end row  -->
            </div><!-- end layout_content  -->            
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
<div style="display: none;">
    <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
    <input type="hidden" id="hdShortReplyID" />
    <input type="hidden" id="hdGood" />
</div>
</form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank" title=""></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.resize.js"></script>
    <script type="text/javascript">
        var CatID = "<%=CatID %>"; //트리에서 넘어온 카테고리
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%= ListID %>";
        var SearchGroup = "<%=SearchGroup %>"; //분류
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var Popup = "<%=Popup %>"; //popup
        var OptionListType = "<%=OptionListType %>";
        var GoPage = "<%=GoPage %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var UserID = "<%=CurrentUserInfo.UserID %>";
        var OptionShortReplyGood = "<%=OptionShortReplyGood %>";
        var OptionShortReplyTop = "<%=OptionShortReplyTop %>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_ApplyDelete = "<%=GetResource("ImportantDel") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var Option_Reple = "<%=liReply.Visible %>"; //리플 사용여부
        var OptionShowViewer = ""; //조회자사용여부
        var WindowObjectReference; //새창용   
        var OptionDownload = "<%=OptionDownload %>"; //외부다운로드 체크
        var R_DownLoadArea = "<%=GetResource("DownLoadArea") %>"; //이 게시물의 첨부파일은 외부 다운로드가 불가능합니다.
        var R_AddShortReply = "<%=GetResource("AddShortReply") %>";
        var R_ReplyAdd = "<%=GetResource("ReplyAdd") %>";
        var R_Save = "<%=GetResource("Submit") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
        var R_DelReply = "<%=GetResource("DelReply") %>";
        var R_MyComment = "<%=GetResource("MyComment") %>";
        var R_AlreadyComment = "<%=GetResource("AlreadyComment") %>";
        var R_Recommend = "<%=GetResource("Recommend") %>";
        var R_Decommendation = "<%=GetResource("Decommendation") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/ViewImpo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/ViewImpo.js");
    </script>
</body>
</html>
