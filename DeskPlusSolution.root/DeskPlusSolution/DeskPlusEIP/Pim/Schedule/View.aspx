<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.View" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">        
        var DisplayID = '<%=DisplayID%>';
        var ScheduleID = '<%=ScheduleID%>';
        var CompanyID = '<%=CompanyID%>';
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = '<%=GetResource("FileName") %>';
        var R_FileType = '<%=GetResource("FileType") %>';
        var R_FileLen = '<%=GetResource("FileLen") %>';
        var R_FileDown = '<%=GetResource("FileDownload") %>';
        var R_Name = '<%=GetResource("Name") %>';
        var R_Contents = '<%=GetResource("Contents") %>';
        var R_RegDate = '<%=GetResource("RegDate") %>';
        var R_ApplyDelete = '<%=GetResource("ApplyDelete") %>';
        var R_ReallyApplyDelete = '<%=GetResource("ReallyApplyDelete") %>';
        var R_TextEmpty = '<%=GetResource("TextEmpty") %>';
        var Dateinfo = '<%=Date%>';
        var PreLang = "<%=PreLang%>"; 
        var PageUrl = '<%=PageUrl%>';
        var s_Distance = '<%=s_Distance %>';
        var s_inquryPerson = '<%=s_inquryPerson %>';
        var s_DatePeriod = '<%=s_DatePeriod %>';
        var s_SearchKind = '<%=s_SearchKind %>';
        var s_SerachValue = '<%=s_SerachValue %>';
        var s_StartDate = '<%=s_StartDate %>';
        var s_EndDate = '<%=s_EndDate %>';
        var s_UserId = '<%=s_UserID %>';
        var s_PageSize = '<%=s_PageSize %>';
        var s_CurrentPage = '<%=s_CurrentPage %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_MsgError = "<%= GetResource("MsgError")%>";
        var R_SecurityMsg = "<%= GetResource("SecurityMsg")%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        function RepeatMod(arg) {
            $("#hdRepeatSel").val(arg);
            View.Delete();
        }
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/View.js");             
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <!-- layout_body -->
        <div class="layout_full">
            <!-- layout_full -->
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Schedule")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="CbtnPrint" runat="server" BeginEndTag="li" />

                    <cc3:ClientSmartButton ID="CbtnEdit" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="CbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
				    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->                  
                        <div id="toPrint">
                            <table cellspacing="0" class="table_typeB">
                                <caption>
                                    <%=GetResource("Schedule")%></caption>
                                <colgroup>
                                    <col width="80px" />
                                    <col width="300px"/>
                                    <col width="80px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <th><%=GetResource("Title")%></th>
                                    <td colspan="3"><asp:Label ID="lblTitle" runat="server"></asp:Label></td>  
                                </tr>
                                <tr>
                                    <th><%=GetResource("WRITER")%></th>
                                    <td><asp:Label ID="lblWriter" runat="server"></asp:Label></td>
                                    <th><%=GetResource("ViewLimit")%></th>
                                    <td><asp:Label ID="lblshare" runat="server"></asp:Label></td>    
                                </tr>
                                <tr id="tdDate" style="vertical-align: top;">
                                    <th><%=GetResource("DateView")%></th>
                                    <td><asp:Label ID="lblDateStr" runat="server"></asp:Label></td>                                                            
                                    <th><%=GetResource("CategoryName")%></th>
                                    <td><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                                </tr>
                                <tr style="display: none">
                                    <th><%=GetResource("ResourceReservation")%></th>
                                    <td colspan="3"></td>
                                </tr>
                            </table>
                            <div class="content_box" style="overflow:auto;">
                            <table cellspacing="0" style="min-height:300px;width:100%;">
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblContent" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                          </div>
                    </div><!-- end col content-area-body-right -->
                    <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"> 
                        <ul> 
                            <!-- 공유대상 리스트 -->    
                            <li class="title">
                                <%=GetResource("ShareSetting")%>
                            </li>
                            <li><asp:Label ID="lblshareuser" runat="server"></asp:Label></li>
                        </ul> 
                        <ul> 
                            <!-- 첨부파일 -->
                            <li class="title">
                                <%=GetResource("AttachName")%>
                            </li>
                            <li><div id="placeholder_file" style="clear: both;"></div></li>
                        </ul> 
                        <ul>                     
                            <li class="title"><%=GetResource("ShortReply")%>
							    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-sm btn-secondary " />
                            </li>
                            <li class="reply"><div id="placeholder_shortReply" style="clear: both;"></div></li>
                        </ul>
                     </div>
                </div>				
            </div>
        </div>                
    </div>
    <!-- //layout_info -->
    <div style="display: none;">
        <asp:TextBox ID="hdRepeatSel" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeatChk" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:LinkButton ID="lbtnEdit" runat="server"></asp:LinkButton>
        <asp:LinkButton ID="lbtnCancel" runat="server"></asp:LinkButton>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>
</body>
</html>
