<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPopup.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.ViewPopup" EnableSessionState="false" EnableViewState="false" EnableViewStateMac="true" EnableEventValidation="true" ValidateRequest="true" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="referrer" content="no-referrer" />
    <meta name="robots" content="noindex, nofollow, noarchive" />
    <title><%=System.Web.HttpUtility.HtmlEncode(System.Configuration.ConfigurationManager.AppSettings["SolutionTitle"])%></title>
    <link rel="stylesheet" type="text/css" href="<%=System.Web.HttpUtility.HtmlAttributeEncode(BaseCssPath)%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=System.Web.HttpUtility.HtmlAttributeEncode(CssPath)%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=System.Web.HttpUtility.HtmlAttributeEncode(BaseCssPath)%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=System.Web.HttpUtility.HtmlAttributeEncode(CssPath)%>/Func_Board.css" />
    <style type="text/css">
        html, body { margin: 0; padding: 0; min-width: 0; }
        .education-popup-page #layout_body { box-sizing: border-box; margin: 0; padding: 0; width: 100%; height: auto; min-width: 0; }
        .education-popup-page .layout_full { box-sizing: border-box; margin: 0; padding: 0; width: 100%; height: auto; min-width: 0; }
        .education-popup-page #layout_header { box-sizing: border-box; display: flex; flex-wrap: wrap; align-items: center; justify-content: space-between; gap: 10px; height: auto; padding: 14px 20px; }
        .education-popup-page #layout_header h1 { float: none; margin: 0; }
        .education-popup-page #layout_header .btn_right { position: static; float: none; display: flex; flex-wrap: wrap; gap: 4px; margin: 0; padding: 0; }
        .education-popup-page #layout_header .btn_right li { float: none; margin: 0; }
        .education-popup-page #layout_content { box-sizing: border-box; height: auto; padding: 16px 20px 20px; }
        .education-popup-page #layout_caption { box-sizing: border-box; width: 100%; min-width: 0; height: auto; float: none; margin: 0 0 14px; padding: 10px 12px; border: 1px solid #e5e8eb; background: #f5f6f8; color: #626971; }
        .education-popup-page #layout_caption .app_caption { display: flex; flex-wrap: wrap; align-items: baseline; gap: 4px 8px; float: none; margin: 0; font-size: 12px; font-weight: 600; line-height: 1.7; }
        .education-attachment-counts { min-width: 0; white-space: normal; word-break: keep-all; overflow-wrap: break-word; }
        .education-attachment-notice { min-width: 0; word-break: keep-all; overflow-wrap: break-word; word-wrap: break-word; }
        .education-popup-main { box-sizing: border-box; width: 100%; min-width: 0; float: none; margin: 0; padding: 0; }
        .education-popup-main td { overflow-wrap: break-word; word-wrap: break-word; }
        .education-popup-page .table_typeB { width: 100%; table-layout: fixed; }
        .education-popup-page .content_box { margin-top: 10px; min-height: 260px; overflow: auto; }
        .education-content-frame { display: block; box-sizing: border-box; width: 100%; min-height: 260px; border: 0; }
        .education-popup-empty { padding: 10px 0; }
        @media (max-width: 760px) {
            .education-popup-page #layout_header { padding: 12px 14px; }
            .education-popup-page #layout_content { padding: 14px; }
        }
    </style>
</head>
<body class="education-popup-page">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1>보기</h1>
                <ul class="btn_right">
                    <li><button type="button" id="cbtnClose" class="btn">닫기</button></li>
                </ul>
            </div>
            <div id="layout_content">
                <asp:PlaceHolder ID="phEducation" runat="server" Visible="false">
                    <div id="layout_caption">
                        <span class="app_caption">
                            <span class="education-attachment-counts">* 공지첨부(<strong><asp:Literal ID="ltlNoticeFileCount" runat="server" Mode="Encode" />개</strong>) / 첨부파일(<strong><asp:Literal ID="ltlCertificateFileCount" runat="server" Mode="Encode" />개</strong>)</span>
                            <span id="educationAttachmentNotice" class="education-attachment-notice">보안상, 첨부파일은 포탈에 로그인한 후 교육메뉴에서 확인할 수 있습니다.</span>
                        </span>
                    </div>
                    <div class="education-popup-main">
                        <div id="toPrint">
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
                                        <td colspan="3"><asp:Literal ID="ltlItem" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>기간</th>
                                        <td><asp:Literal ID="ltlDate" runat="server" /></td>
                                        <th>첨부파일필수</th>
                                        <td><asp:Literal ID="ltlFileYN" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>완료여부</th>
                                        <td><asp:Literal ID="ltlComplete" runat="server" /></td>
                                        <th>진행상태</th>
                                        <td><asp:Literal ID="ltlStatus" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <th>완료자</th>
                                        <td><asp:Literal ID="ltlCompleteUserName" runat="server" /></td>
                                        <th>완료일</th>
                                        <td><asp:Literal ID="ltlCompleteDate" runat="server" Mode="Encode" /></td>
                                    </tr>
                                    <tr>
                                        <th>과정명</th>
                                        <td colspan="3"><asp:Label ID="lbTitle" runat="server" /></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="content_box">
                                <iframe id="educationContentFrame" class="education-content-frame" sandbox="" referrerpolicy="no-referrer" title="내용" srcdoc="<%=ContentDocumentAttribute%>"></iframe>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phNotFound" runat="server" Visible="true">
                    <div class="education-popup-empty" role="status">조회할 수 없는 내역입니다.</div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <script type="text/javascript" nonce="<%=System.Web.HttpUtility.HtmlAttributeEncode(CspNonce)%>">
        (function () {
            var closeButton = document.getElementById('cbtnClose');
            if (closeButton) closeButton.addEventListener('click', function () { window.close(); });
        }());
    </script>
</body>
</html>
