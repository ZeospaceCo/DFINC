<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPopup.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ViewPopup" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup view">
        <div class="layout_full">
            <div class="popup_header">
                <h1><%=GetResource("WriteView")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnListView" runat="server" BeginEndTag="li" />
                    <li class="sec"></li>
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine pb-0 mb-0"></div>
            <div id="layout_content">
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
                                    <th class="last-child">
                                        <%=GetResource("View_RegInfo")%>
                                    </th>
                                    <td>
                                        <%=GetResource("Registrant")%>:
									    <asp:Literal ID="lbWriteInfo" runat="server"></asp:Literal>
									    &nbsp;/&nbsp;
                                            <%=GetResource("RegDate")%>:
                                            <asp:Label title="" ID="lbWriteDate" runat="server"></asp:Label>&nbsp;/&nbsp;
                                            <%=GetResource("ViewCount")%>:
                                            <asp:Label title="" ID="lbReadCount" runat="server"></asp:Label>
                                            <%=GetResource("RecomCount")%>:
                                            <asp:Label ID="lbRecomCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <!--기본정보-->
                        <!--본문정보-->
                        <div class="content_box"  style="overflow-y:hidden">
                        <table cellspacing="0" style="min-height: 300px;width:100%;">
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
                        </li>
                        <li class="title"><%=GetResource("AttachFile")%></li>                      
        
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        <!--첨부파일-->
                        
                        <li class="title"><%=GetResource("ShortReply")%>
							<cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                        </li>
                        <li class="reply"><div id="placeholder_shortReple" style="clear: both;"></div></li>

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
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
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
        var CommunityID = "<%=CommunityID %>";
        var MenuID = "<%=MenuID %>";
        var BoardID = "<%=BoardID %>";
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var Order = "<%=Order %>"; //정렬
        var Popup = "1";
        var ListType = "<%=ListType %>";
        var Grade = "<%=Grade %>"; 
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel ="<%=GetResource("FileDel") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_Mod = "<%=GetResource("Mod") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileDown = "<%=GetResource("FileDown") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmIssue = "<%=GetResource("ConfirmIssue") %>";
        var R_ConfirmRecommend = "<%=GetResource("ConfirmRecommend") %>";
        var R_UserName = "<%=GetResource("UserName") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Apps/Board/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Apps/Board/View.js");
    </script>
</body>
</html>
