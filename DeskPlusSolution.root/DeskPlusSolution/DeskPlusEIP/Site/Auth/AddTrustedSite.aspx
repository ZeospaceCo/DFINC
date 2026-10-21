<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTrustedSite.aspx.cs" Inherits="Zeospace.DeskPlus.Base.Web.Site.Auth.AddTrustedSite" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
	<meta name="robots" content="noindex">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RegisterTrustSite")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Text="등록" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" Text="닫기" /></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
        <table class="table_typeB" border="0" style="width:98%;" >
        <tr>
        <td>
            <%=GetResource("IeSettingMsg1")%><br /><br />
            <%=GetResource("IeSettingMsg2")%>
        </td></tr>
        </table>
        </div>
    </div>
</div>
    <div id="divActiveX"><script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/activexDPTrustedSites.js"></script></div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>  
    <script type="text/javascript" >
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var domainName = "<%=Domain%>";  //도메인 정보를 config에서 가져올것
    </script>
    <script type="text/javascript" src=""../../Js/Site/Auth/AddTrustedSite.js" ></script>  
    <script type="text/javascript">
        //JsCall("../../Js/Site/Auth/AddTrustedSite.js");
    </script>
</body>
</html>