<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.View" %>

<!DOCTYPE html>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup view">
        <div class="layout_full">
            <div class="popup_header">
                <h1><%=GetResource("ReserveResource") %></h1>

                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" BeginEndTag="li" runat="server" />

                    <cc3:ClientSmartButton ID="cbtnEnd" BeginEndTag="li" Class="btn btn-secondary" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnMod" BeginEndTag="li" Class="btn btn-secondary" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" Class="btn btn-danger" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnList" BeginEndTag="li" runat="server" />
                </ul>
            </div>
            <div class="dotLine pb-0 mb-0"></div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 col-lg-8 content-area-body-left" ><!-- width 12,9,8/12 -->

                        <div id="toPrint">
                            <!--기본정보-->
                            <table class="table_typeB">
                                <caption>
                                    View</caption>
                                <colgroup>
                                    <col width="80px" />
                                    <col width="300px" />
                                    <col width="80px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th><%=GetResource("Subject") %></th>
                                        <td colspan="3"><asp:Label title="" ID="lbSubject" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("Period") %></th>
                                        <td><asp:Label title="" ID="lbDate" runat="server"></asp:Label></td>
                                        <th><%=GetResource("Subscriber")%></th>
                                        <td><asp:Label title="" ID="lbWriteName" runat="server"></asp:Label></td>                                       
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("ModifyDate")%></th>
                                        <td colspan="3"><asp:Label title="" ID="lbModifyDate" runat="server"></asp:Label></td>
                                        <!--<th><%=GetResource("Gubun")%></th>
                                        <td><span id="Pattern" runat="server"></span></td>-->
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("SecuritySetting")%></th>
                                        <td><asp:Label title="" ID="lbGuard" runat="server"></asp:Label></td>
                                        <th><%=GetResource("UsableStr")%></th>
                                        <td><asp:Label title="" ID="lbUsable" runat="server"></asp:Label></td>                                    
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("Repeat")%></th>
                                        <td colspan="3"><asp:Label title="" ID="lbRepeat" runat="server"></asp:Label></td>
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
                            <!-- 거부사유 -->
                            <li class="title" id="dvReject" style="display:none">
                                 <%=GetResource("RejectStr")%>
                            </li>        
                            <li id="dvRejectList" style="display:none"><div id="phReject" style="clear: both;"></div></li>
                            <!-- 거부사유 -->         

                            <!-- 자원 선택 -->
                            <li class="title">
                                    <%=GetResource("ReserveResource")%>
                            </li>
                            <li>
                                <ul class="table_typeT ul_typeT">   
                                    <li>
                                        <span ID="spTitle" runat="server"></span>
                                    </li>
                                </ul>
                            </li>   
                            <!-- 자원 선택 -->

                            <!-- 공유자 선택 -->
                            <li class="title">
                                 <%=GetResource("ShareUser")%>
                            </li>
                            <li><ul id="phShareList" class="table_typeT ul_typeT"></ul></li>    
                            <!-- 공유자 선택 -->  

                             <!-- 첨부 파일 -->
                            <li class="title">
                                <%=GetResource("Attach")%>
                            </li>
                            <li><div id="phFile" style="clear: both;"></div></li>   
                            <!-- 첨부 파일 -->

                            <li class="title"><%=GetResource("ShortReply")%>
							    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                            </li>
                            <li class="reply"><div id="placeholder" style="clear: both;"></div></li>
                        </ul>     

                     </div>
                </div>
            </div>            
         </div>
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbtnConfirmEnd" runat="server" OnClick="lbtnConfirmEnd_Click"></asp:LinkButton>
        <asp:TextBox ID="hdRepeatSel" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeatChk" Style="display: none" runat="server" Width="0px"></asp:TextBox>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">        
        var CategoryID = "<%=CategoryID%>";
        var ReservationID = "<%=ReservationID%>";
        var ParentReservationID = "<%=ParentReservationID%>";
        var TagGubun = "<%=TagGubun %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var TmpDate = "<%=TmpDate%>";
        var GroupGubun = "<%=GroupGubun %>";
        var DeptPageNo = "<%=DeptPageNo%>";
        var MainPageNo = "<%=MainPageNo%>";
        var Usable = "<%=Usable%>";
        var CntYN = "<%=CntYN%>";
        var RmCnt = "<%=RmCnt%>";
        var PageName = "<%= System.IO.Path.GetFileName(Request.Path) %>";
        var ImagePath = "<%=BaseImgPath%>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Download = "<%=GetResource("Download") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DelResources = "<%=GetResource("DelResources") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_WriteDate = "<%=GetResource("WriteDate") %>";
        var R_Mod  = "<%=GetResource("Mod") %>";
        var R_Del  = "<%=GetResource("Del") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_AttendeeType1 = "<%=GetResource("AttendeeType1") %>"; //부서
        var R_AttendeeType2 = "<%=GetResource("AttendeeType2") %>"; //사용자
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/View.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/RM/Js/Content/View.js");
    </script>
</body>
</html>
