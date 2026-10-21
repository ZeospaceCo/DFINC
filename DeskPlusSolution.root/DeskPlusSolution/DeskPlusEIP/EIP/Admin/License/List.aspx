<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.License.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_one">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("LicenseManage")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnComInfo" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary"  BeginEndTag="li" />
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
            <li>&nbsp;</li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

            <!-- 테이블 시작 //-->            
            <div id="phLicenseList" style="clear: both;"></div>            
            <!-- 테이블 끝 -->

            </div>
            </div>
        </div>
        <div style="display:">
        <iframe id="ifrm" name="ifrm" frameborder="0" scrolling="no" src="about:blank" ></iframe>
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
    
    <div style="display:none;">
    <form id="ActLic" action="<%=ActServerURL%>" target="ifrm" method="get">
    <input type="text" id="lickey" name="lickey" value="" />
    <input type="text" name="IpWeb" value="<%=WebServerIP%>" />
    <input type="text" name="IpMail" value="<%=MailServerIP%>" />
    <input type="text" name="Domain" value="<%=FullDomain%>" />
    </form>
    </div>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_LicenseKey = "<%=GetResource("LicenseKey") %>";
        var R_Comments = "<%=GetResource("Comments") %>";
        var R_LicenseCount = "<%=GetResource("LicenseCount") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_ExpirationDate = "<%=GetResource("ExpirationDate") %>";
        var R_ConfirmLicense = "<%=GetResource("ConfirmLicense") %>";
        var R_ConfirmServer = "<%=GetResource("ConfirmServer") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_NotExistList = "<%=GetResource("NotExistList") %>";
        var R_ProcessingErrorLicenseDelete = "<%=GetResource("ProcessingErrorLicenseDelete") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/License/List.js"></script>
</body>
</html>
