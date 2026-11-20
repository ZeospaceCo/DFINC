<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUp.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.PopUp" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["SolutionTitle"])%></title>
    <style type="text/css">
        html, body { width: 100%; height: 100%; margin: 0; padding: 0; }
        #form1 { height: 100%; display: flex; flex-direction: column; }
        .education-popup-header { flex: 0 0 auto; padding: 8px 12px; background: #999999; text-align: right; }
        .education-popup-header button { margin-left: 16px; padding: 0; border: 0; background: transparent; color: #ffffff; font-size: small; cursor: pointer; }
        .education-popup-header button:first-child { margin-left: 0; }
        #educationPopupFrame { display: block; width: 100%; flex: 1 1 auto; min-height: 0; border: 0; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="education-popup-header">
            <asp:PlaceHolder ID="phPopupActions" runat="server" Visible="false">
                <button type="button" id="educationPopupWeek">일주일간 띄우지 않기</button>
                <button type="button" id="educationPopupToday">오늘 하루 띄우지 않기</button>
                <button type="button" id="educationPopupNever">다시 띄우지 않기</button>
            </asp:PlaceHolder>
            <button type="button" id="educationPopupClose">닫기</button>
        </div>
        <iframe id="educationPopupFrame" src="<%=HttpUtility.HtmlAttributeEncode(ContentUrl)%>" title="내용" scrolling="auto" frameborder="0"></iframe>
    </form>
    <script type="text/javascript">
        var EducationPopUpCookieName = '<%=HttpUtility.JavaScriptStringEncode(CookieName)%>';
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/PopUp.js?v=2"></script>
</body>
</html>
