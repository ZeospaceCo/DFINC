<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPopupTotal.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.ViewPopupTotal" EnableViewStateMac="true" EnableEventValidation="true" ValidateRequest="true" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title><%=HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["SolutionTitle"])%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <style type="text/css">
        .education-attachment-list { box-sizing:border-box; min-height:24px; }
        .education-attachment-list > div > ul { margin-top:0; margin-bottom:0; }
        .education-attachment-list > div > ul > li:empty { display:none; }
        html, body { margin: 0; padding: 0; min-width: 0; }
        .education-popup-page #layout_body { box-sizing: border-box; margin: 0; padding: 0; width: 100%; height: auto; min-width: 0; }
        .education-popup-page .layout_full { box-sizing: border-box; margin: 0; padding: 0; width: 100%; height: auto; min-width: 0; }
        .education-popup-page #layout_header { box-sizing: border-box; display: flex; flex-wrap: wrap; align-items: center; justify-content: space-between; gap: 10px; height: auto; padding: 14px 20px; }
        .education-popup-page #layout_header h1 { float: none; margin: 0; }
        .education-popup-page #layout_header .btn_right { position: static; float: none; display: flex; flex-wrap: wrap; gap: 4px; margin: 0; padding: 0; }
        .education-popup-page #layout_header .btn_right li { float: none; margin: 0; }
        .education-popup-page #layout_content { box-sizing: border-box; height: auto; padding: 16px 20px 20px; }
        .education-popup-layout { display: flex; align-items: stretch; gap: 20px; }
        .education-popup-main { flex: 1 1 0; min-width: 0; }
        .education-popup-page #layout_content .education-popup-files { box-sizing: border-box; flex: 0 0 260px; float: none; margin: 0; min-width: 0; min-height: 0; padding: 0; border-left: 1px solid rgba(0,0,0,0.1); }
        .education-popup-files > ul::after { content: ""; display: block; clear: both; }
        .education-popup-files > ul > li { padding: 7px 20px; }
        .education-popup-files a, .education-popup-main td { overflow-wrap: break-word; word-wrap: break-word; }
        .education-popup-page .table_typeB { width: 100%; table-layout: fixed; }
        .education-popup-page .content_box { margin-top: 10px; min-height: 220px; overflow: auto; }
        .education-popup-page .content_box img { max-width: 100%; height: auto; }
        .education-popup-empty { padding: 10px 0; }
        #placeholder_certificate .certificate-file-size,
        #placeholder_certificate_pending .certificate-file-size { display: inline-block; margin-left: 8px; }
        #placeholder_certificate .certificate-saved-file-delete,
        #placeholder_certificate_pending .certificate-file-delete { display: inline-block; margin-left: 10px; color: #dc3545; text-decoration: none; }
        #placeholder_certificate .certificate-saved-file-delete i,
        #placeholder_certificate_pending .certificate-file-delete i { color: inherit; }
        #placeholder_certificate .certificate-saved-file-delete:hover,
        #placeholder_certificate .certificate-saved-file-delete:focus,
        #placeholder_certificate_pending .certificate-file-delete:hover,
        #placeholder_certificate_pending .certificate-file-delete:focus { color: #b02a37; }
        @media (max-width: 760px) {
            .education-popup-page #layout_header { padding: 12px 14px; }
            .education-popup-page #layout_content { padding: 14px; }
            .education-popup-layout { flex-direction: column; gap: 16px; }
            .education-popup-page #layout_content .education-popup-files { flex: 0 0 auto; padding: 0; border-left: 0; border-top: 1px solid rgba(0,0,0,0.1); }
        }
    </style>
</head>
<body class="education-popup-page">
    <form id="form1" runat="server">
        <div id="layout_body">
            <div class="layout_full">
                <div id="layout_header">
                    <h1>보기</h1>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnComplete" runat="server" BeginEndTag="li" Text="완료" Class="btn btn-secondary" Visible="false" />
                        <cc3:ClientSmartButton ID="cbtnCancelComplete" runat="server" BeginEndTag="li" Text="완료 취소" Class="btn btn-danger" Visible="false" />
                        <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" Text="닫기" />
                    </ul>
                </div>
                <div id="layout_content">
                    <asp:PlaceHolder ID="phEducation" runat="server" Visible="false">
                        <div class="education-popup-layout">
                            <div class="education-popup-main">
                                <div id="toPrint">
                                    <!-- 기본정보 -->
                                    <table class="table_typeB">
                                        <caption>정보</caption>
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
                                                <th>완료여부</th>
                                                <td><asp:Literal ID="ltlComplete" runat="server"></asp:Literal></td>
                                                <th>진행상태</th>
                                                <td><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
                                            </tr>                                            
                                            <tr>
                                                <th>완료자</th>
                                                <td><asp:Literal ID="ltlCompleteUserName" runat="server"></asp:Literal></td>
                                                <th>완료일</th>
                                                <td><asp:Literal ID="ltlCompleteDate" runat="server" Mode="Encode"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <th>과정명</th>
                                                <td colspan="3"><asp:Label ID="lbTitle" runat="server"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <!-- 본문정보 -->
                                    <div class="content_box"><asp:Label ID="lbContent" runat="server"></asp:Label></div>
                                </div>
                            </div>
                            <div class="education-popup-files content-area-body-right">
                                <ul>
                                    <li class="title">공지첨부</li>
                                    <li class="education-attachment-list"><div id="placeholder_file" style="clear: both;"></div></li>
                                </ul>
                                <ul>
                                    <li class="title">첨부파일
                                        <asp:PlaceHolder ID="phCertificateUpload" runat="server" Visible="false">
                                            <a href="#" id="cbtnSwfFileAdd" title="파일추가" aria-label="첨부파일 추가"><i class="fas fa-plus-circle text-secondary"></i></a>
                                        </asp:PlaceHolder>
                                    </li>
                                    <li class="education-attachment-list">
                                        <div id="placeholder_certificate" style="clear: both;"><asp:Literal ID="ltlCertificateFile" runat="server"></asp:Literal></div>
                                        <div id="placeholder_certificate_pending" style="clear: both;"></div>
                                        <asp:Label ID="lblCompleteMessage" runat="server" ForeColor="Red" role="alert" Visible="false"></asp:Label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phNotFound" runat="server" Visible="true">
                        <div class="education-popup-empty">조회할 수 없는 내역입니다.</div>
                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
        <div style="display:none;">
            <asp:LinkButton ID="lbtnComplete" runat="server" CausesValidation="false" OnClick="lbtnComplete_Click">완료</asp:LinkButton>
            <asp:LinkButton ID="lbtnSaveAttachments" runat="server" CausesValidation="false" OnClick="lbtnSaveAttachments_Click">첨부파일 등록</asp:LinkButton>
            <asp:HiddenField ID="hfDeleteAttachmentGuid" runat="server" />
            <asp:LinkButton ID="lbtnDeleteAttachment" runat="server" CausesValidation="false" OnClick="lbtnDeleteAttachment_Click">첨부파일 삭제</asp:LinkButton>
        </div>
    </form>
    <!-- 사용자 정보 모달 -->
    <div id="zeoModalContact">
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank" title="팝업"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var HasEducation = <%=HasEducation ? "true" : "false" %>;
        var IsComplete = <%=Status == "3" ? "true" : "false" %>;
        var IsAdminConfirmed = <%=IsAdminConfirmed ? "true" : "false" %>;
        var IsWaitingForCompletion = <%=IsWaitingForCompletion ? "true" : "false" %>;
        var CertificateRequired = <%=CertificateRequired ? "true" : "false" %>;
        var SavedCertificateCount = <%=SavedCertificateCount %>;
        var SwfUploadGUID = "<%=HttpUtility.JavaScriptStringEncode(SwfUploadGUID) %>";
        var CompletePostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnComplete.UniqueID) %>";
        var SaveAttachmentsPostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnSaveAttachments.UniqueID) %>";
        var DeleteAttachmentFieldID = "<%=HttpUtility.JavaScriptStringEncode(hfDeleteAttachmentGuid.ClientID) %>";
        var DeleteAttachmentPostBackTarget = "<%=HttpUtility.JavaScriptStringEncode(lbtnDeleteAttachment.UniqueID) %>";
        var ListID = "<%=HttpUtility.JavaScriptStringEncode(ListID.ToString(System.Globalization.CultureInfo.InvariantCulture))%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/ViewPopupTotal.js?v=3"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/InlineCompletion.js?v=5"></script>
</body>
</html>
