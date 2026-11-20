<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.View" EnableViewStateMac="true" EnableEventValidation="true" ValidateRequest="true" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <style type="text/css">
        .education-attachment-list { box-sizing:border-box; min-height:24px; }
        .education-attachment-list > div > ul { margin-top:0; margin-bottom:0; }
        .education-attachment-list > div > ul > li:empty { display:none; }
        .edu-user-status-section { margin: 16px 0; }
        .edu-user-status-section .edu-user-status-title { margin: 0 0 8px; color: #4a5668; font-size: 13px; font-weight: 600; line-height: 20px; }
        #eduConfirmSaveStatus { display:inline-block; margin-left:8px; }
        #placeholder_certificate .certificate-file-size,
        #placeholder_certificate_pending .certificate-file-size { display:inline-block; margin-left:8px; }
        #placeholder_certificate .certificate-saved-file-delete,
        #placeholder_certificate_pending .certificate-file-delete { display:inline-block; margin-left:10px; color:#dc3545; text-decoration:none; }
        #placeholder_certificate .certificate-saved-file-delete i,
        #placeholder_certificate_pending .certificate-file-delete i { color:inherit; }
        #placeholder_certificate .certificate-saved-file-delete:hover,
        #placeholder_certificate .certificate-saved-file-delete:focus,
        #placeholder_certificate_pending .certificate-file-delete:hover,
        #placeholder_certificate_pending .certificate-file-delete:focus { color:#b02a37; }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1>보기</h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnTargetList" runat="server" BeginEndTag="li" Text="대상자리스트" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnSendAlarm" runat="server" BeginEndTag="li" Text="알람발송" Class="btn" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnSendAllAlarm" runat="server" BeginEndTag="li" Text="미완료알람발송" Class="btn" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnSendDelayedAlarm" runat="server" BeginEndTag="li" Text="지연알람발송" Class="btn" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnSendIncompleteMail" runat="server" BeginEndTag="li" Text="미완료메일발송" Class="btn" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnSendDelayedMail" runat="server" BeginEndTag="li" Text="지연메일발송" Class="btn" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnComplete" runat="server" BeginEndTag="li" Text="완료" Class="btn btn-secondary" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnCancelComplete" runat="server" BeginEndTag="li" Text="완료 취소" Class="btn btn-danger" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnMod" runat="server" BeginEndTag="li" Text="수정" Class="btn btn-secondary" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnDelete" runat="server" BeginEndTag="li" Text="삭제" Class="btn btn-danger" Visible="false" />
                <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
            </ul>
        </div>

        <div id="layout_content">           
            <div class="row">
				<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                        <div id="toPrint">                 
                        <!--기본정보-->
                        <table class="table_typeB">
                            <caption>List</caption>
		                    <colgroup>
			                    <col width="90px" />
                                <col />
                                <col width="90px" />
                                <col />
		                    </colgroup>
		                    <tbody>
                                <tr>
                                    <th>분류</th>
                                    <td colspan="3"><asp:Literal ID="ltlItem" runat="server"></asp:Literal></td>                                    
                                </tr>
                                <tr>
                                    <th>기간</th>
                                    <td><asp:Literal ID="ltlDate" runat="server"></asp:Literal></td>
                                    <th>첨부파일필수</th>
                                    <td><asp:Literal ID="ltlFileYN" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>대상자</th>
                                    <td colspan="3"><asp:Literal ID="ltlReceive" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>팝업설정</th>
                                    <td><asp:Literal ID="ltlPopUpYN" runat="server"></asp:Literal></td>
                                    <th>알람</th>
                                    <td><asp:Literal ID="ltlAlarmDay" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>등록일</th>
                                    <td><asp:Literal ID="ltlWriteDate" runat="server"></asp:Literal></td>
                                    <th>등록자</th>
                                    <td><asp:Literal ID="ltlWriteUserName" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>과정명</th>
                                    <td colspan="3"><asp:Label title="" ID="lbTitle" runat="server"></asp:Label></td>
                                </tr>

                            </tbody>
                         </table>
                         <!--기본정보-->

                         <asp:PlaceHolder ID="phUserStatus" runat="server" Visible="false">
                             <div class="edu-user-status-section" role="region" aria-labelledby="eduUserStatusTitle">
                                 <h2 id="eduUserStatusTitle" class="edu-user-status-title">사용자 수강정보</h2>
                                 <table class="table_typeB">
                                     <caption>사용자 수강정보</caption>
                                     <colgroup>
                                         <col width="80px" />
                                         <col />
                                         <col width="80px" />
                                         <col />
                                     </colgroup>
                                     <tbody>
                                         <tr>
                                             <th>사용자</th>
                                             <td colspan="3"><asp:Literal ID="ltlTargetUser" runat="server" Mode="Encode" /></td>
                                         </tr>
                                         <tr>
                                             <th>완료여부</th>
                                             <td><asp:Literal ID="ltlComplete" runat="server" Mode="Encode" /></td>
                                             <th>진행상태</th>
                                             <td><asp:Literal ID="ltlStatus" runat="server" Mode="Encode" /></td>
                                         </tr>
                                         <tr>
                                             <th>완료자</th>
                                             <td><asp:Literal ID="ltlCompleteUserName" runat="server" /></td>
                                             <th>완료일</th>
                                             <td><asp:Literal ID="ltlCompleteDate" runat="server" Mode="Encode" /></td>
                                         </tr>
                                         <tr>
                                             <th><label for="chkAdminConfirm">확인여부</label></th>
                                             <td colspan="3">
                                                 <input type="checkbox" id="chkAdminConfirm" disabled="disabled" aria-describedby="eduConfirmSaveStatus" />
                                                 <span id="eduConfirmSaveStatus" role="status" aria-live="polite"></span>
                                             </td>
                                         </tr>
                                         <tr>
                                             <th>확인자</th>
                                             <td><span id="eduConfirmUserName"></span></td>
                                             <th>확인일</th>
                                             <td><span id="eduConfirmDate"></span></td>
                                         </tr>
                                     </tbody>
                                 </table>
                             </div>
                         </asp:PlaceHolder>
            
                         <!--본문정보-->
                         <div class="content_box"  style="overflow:auto;">
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
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  > 

                    <ul> 
                        <!--공지첨부-->
                        <li class="title">공지첨부</li>  
                        <li class="education-attachment-list"><div id="placeholder_file" style="clear: both;"></div></li>
                        <!--공지첨부-->
                     </ul>
                    <asp:PlaceHolder ID="pnlCertificates" runat="server" Visible="false">
                        <ul>
                            <li class="title">첨부파일
                                <asp:PlaceHolder ID="phCertificateUpload" runat="server" Visible="false">
                                    <a href="#" id="cbtnSwfFileAdd" title="파일추가" aria-label="첨부파일 추가"><i class="fas fa-plus-circle text-secondary"></i></a>
                                </asp:PlaceHolder>
                            </li>
                            <li class="education-attachment-list">
                                <div id="placeholder_certificate" style="clear: both;"><asp:Literal ID="ltlCertificateFile" runat="server"></asp:Literal></div>
                                <div id="placeholder_certificate_pending" style="clear:both;"></div>
                                <asp:Label ID="lblCompleteMessage" runat="server" ForeColor="Red" role="alert" Visible="false"></asp:Label>
                            </li>
                        </ul>
                    </asp:PlaceHolder>
                                       
	            </div><!-- end col content-area-body-right --> 
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->
    <div style="display:none;">
        <asp:LinkButton ID="lbtnComplete" runat="server" CausesValidation="false" OnClick="lbtnComplete_Click">완료</asp:LinkButton>
        <asp:LinkButton ID="lbtnSaveAttachments" runat="server" CausesValidation="false" OnClick="lbtnSaveAttachments_Click">첨부파일 등록</asp:LinkButton>
        <asp:HiddenField ID="hfDeleteAttachmentGuid" runat="server" />
        <asp:LinkButton ID="lbtnDeleteAttachment" runat="server" CausesValidation="false" OnClick="lbtnDeleteAttachment_Click">첨부파일 삭제</asp:LinkButton>
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
       
        var HasEducation = <%= HasEducation ? "true" : "false" %>;
        var ListID = "<%= ListID %>";
        var TargetUserID = <%= TargetUserID %>;
        var AdminConfirmation = <%=AdminConfirmationJson%>;
        var IsComplete = <%= IsComplete ? "true" : "false" %>;
        var IsAdminConfirmed = <%=IsAdminConfirmed ? "true" : "false" %>;
        var IsWaitingForCompletion = <%= IsWaitingForCompletion ? "true" : "false" %>;
        var CertificateRequired = <%= CertificateRequired ? "true" : "false" %>;
        var SavedCertificateCount = <%=SavedCertificateCount %>;
        var SwfUploadGUID = "<%=HttpUtility.JavaScriptStringEncode(SwfUploadGUID) %>";
        var CompletePostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnComplete.UniqueID) %>";
        var SaveAttachmentsPostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnSaveAttachments.UniqueID) %>";
        var DeleteAttachmentFieldID = "<%=HttpUtility.JavaScriptStringEncode(hfDeleteAttachmentGuid.ClientID) %>";
        var DeleteAttachmentPostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnDeleteAttachment.UniqueID) %>";
        var SearchDateValue = "<%=HttpUtility.JavaScriptStringEncode(SearchDateValue) %>";
        var SearchTarget = "<%=HttpUtility.JavaScriptStringEncode(SearchTarget) %>";
        var SearchItem = "<%=HttpUtility.JavaScriptStringEncode(SearchItem) %>";
        var ReturnListUrl = "<%=HttpUtility.JavaScriptStringEncode(ReturnListUrl ?? "") %>";
        var ReturnAfterDeleteUrl = "<%=HttpUtility.JavaScriptStringEncode(ReturnAfterDeleteUrl ?? "") %>";
        var ModifyUrl = "<%=HttpUtility.JavaScriptStringEncode(ModifyUrl ?? "") %>";
     
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/View.js?v=5"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/InlineCompletion.js?v=6"></script>
</body>
</html>
