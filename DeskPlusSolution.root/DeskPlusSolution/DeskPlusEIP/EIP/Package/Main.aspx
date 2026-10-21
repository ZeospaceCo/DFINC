<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Package.Main" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"  TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%><%= System.DateTime.Now.ToLongTimeString() %></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Package.css"/>
    <!--[if IE 7]><link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package7.css" media="all" /><![endif]-->
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/js/Chart/chartJs/Chart.min.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들  
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var MailHttp = '<%=Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingType.MAIL, "MailServerIP", "") %>';
        var Mail = "<%=CurrentUserInfo.MailID%>@<%=CurrentUserInfo.MailDomain%>";
        var TabID = "<%=TabID %>";
        var TabLayout = "<%=TabLayout %>";
        var portletSuccess = true;
        var IsAdmin = "<%=IsAdmin %>";//관리자 탭여부 확인
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var PortletAuth = "<%=PortletAuth%>";
        var IsPortletMove = false;
        var MainPopupBoardList = "<%=MainPopupBoardList%>";
        var PopupWidth = "<%=PopupWidth%>";
        var PopupHeight = "<%=PopupHeight%>";
        var TodoPopup = "<%=TodoPopup%>";

        var R_SaveChangePortletOrder = "<%=GetResource("SaveChangePortletOrder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmApply = "<%=GetResource("ConfirmApply") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ErrorConnection = "<%=GetResource("ErrorConnection") %>";
        var R_FirstPage = "<%=GetResource("FirstPage") %>";
        var R_LastPage = "<%=GetResource("LastPage") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_SaveTabPortletOrder = "<%=GetResource("SaveTabPortletOrder")%>";
        var R_PortletMove = "<%=GetResource("PortletMove")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Package/Main.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/EIP/Js/Package/Main.js");
    </script>
    
</head>
<body>

<form name="frmMail" method="post" >					
   <input type="hidden" id="user_account" name="user_account" value="">					
   <input type="hidden" id="cmd" name="cmd" value="">					
   <input type="hidden" id="mail_sn" name="mail_sn" value="">					
   <input type="hidden" id="confirm_gubun" name="confirm_gubun" value="">					
   <input type="hidden" id="confirm_sn" name="confirm_sn" value="">					
   <input type="hidden" id="request_user_sn" name="request_user_sn" value="">					
   <input type="hidden" id="confirm_user_sn" name="confirm_user_sn" value="">					
</form>					



    <form id="form1" runat="server">
    <div style="display:none">
        <iframe src="about:blank" id="ifrmSLO" name="ifrmSLO" width="0" height="0"></iframe>
    </div>
<div id="container">     
    <!-- ############# start of contextMenu ############## -->
	<div id="MenuList"></div>
    <ul id="basicMenu" class="contextMenu">
        <li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>
        <li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>
        <li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>
        <li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>
    </ul>
    <!-- ############# end of contextMenu ############## -->

    <div id="maintabarea">
	    <ul class="maintab headtitle">
        <%=TabList %>
        </ul>
		<span><a href="#" class="mainbtn"><%=GetResource("MainPortletSave") %></a></span>            
    </div>

    <!-- 메인텝설정-->
    <ul class="mainset" style="display:none;">
        <% if (PortletAuth == true)  { %>
        <li><a id="TabPortletOrder"><span title="<%=GetResource("PortletMove")%>"><%=GetResource("PortletMove")%></span></a></li>                     
        <li><a id="AddTabPortlet"><span title="<%=GetResource("AddPortlet")%>"><%=GetResource("AddPortlet")%></span></a></li>                                        
        <% } %>
        <li><a id="ConfigTabMain"><span title="<%=GetResource("ConfigTabMain")%>"><%=GetResource("ConfigTabMain")%></span></a></li>
        <li><a id="ConfigTabUser"><span title="<%=GetResource("ConfigTabUser")%>"><%=GetResource("ConfigTabUser")%></span></a></li>
        <li><a id="ConfigPortletUser"><span title="<%=GetResource("ConfigPortletUser")%>"><%=GetResource("ConfigPortletUser")%></span></a></li>
        <% if (CurrentUserInfo.IsInRole("admin")) { %>
        <li><a id="ConfigTabAdmin"><span title="<%=GetResource("ConfigTabAdmin")%>"><%=GetResource("ConfigTabAdmin")%></span></a></li>
        <% } %>      
    </ul>

    <div id="maincull">
    <asp:PlaceHolder id="placeholderLayout" runat="server"></asp:PlaceHolder>
    </div>    

</div>
    <script type="text/javascript">
        <%
            foreach (System.Collections.Generic.KeyValuePair<string, string> content in PopupContent)
            {
                if (Request.Cookies[content.Key] == null)
                {
                    Response.Write(content.Value);
                }
            }
        %>
    </script>
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
</body>
</html>
