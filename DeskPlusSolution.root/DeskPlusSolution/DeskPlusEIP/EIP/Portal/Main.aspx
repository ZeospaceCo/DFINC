<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="Main.aspx.cs"
    Inherits="Zeospace.DeskPlus.EIP.Web.Portal.Main" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Portal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Portal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<!--script type="text/javascript">

    drawMessageBox();
    $("#splashScreen").show();

    function drawMessageBox()
    {
        var box = "<div id='splashScreen'  style='position:absolute;z-index:5;top:10%;left:35%;'>"
        +"<table cellpadding='0' cellspacing='0' style='width:300px; height:200px;'>"
        +"<tr><td style='width:100%; height:100%;font-family:Tahoma;' align='center' valign='middle'>"
        +"<br/><br />"
        + "<IMG src='/DeskPlusEIP/Design/C/Img/Main/loading.gif' BORDER='0' Name='Progress'/>"
        +"   Please wait..."
        +"</td><td></td></tr>"
        +"</table>"
        +"</div>";

        document.write(box);
    }
</script-->
<body>

<%// Response.Flush();%>

    <form id="form1" runat="server">
    <!-- ############# start of contextMenu ############## -->
	<div id="MenuList"></div>
    <ul id="basicMenu" class="contextMenu">
        <li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>
        <li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>
        <li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>
        <li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>
    </ul>
    <!-- ############# end of contextMenu ############## -->
    <div class="ptl_layout_body">
        <div class="ptl_layout_top" style="text-align: center; display: none;">
            <img src="<%=BaseImgPath%>/Main/main_news.gif" alt="" />
        </div>
        <div class="ptl_layout_mid">
            
            <div class="left" style="display:none;">
                <ul class="ptl_tab">
                    <%=TabList %>
                </ul>
                <ul class="ptl_admin_skin">                    
                    <% if (PortletAuth == true)  { %>
                    <li><a id="TabPortletOrder"><span title="<%=GetResource("PortletMove")%>">* <%=GetResource("PortletMove")%></span></a></li>                     
                    <li><a id="AddTabPortlet"><span title="<%=GetResource("AddPortlet")%>">* <%=GetResource("AddPortlet")%></span></a></li>                                        
                    <% } %>
                    <li><a id="ConfigTabMain"><span title="<%=GetResource("ConfigTabMain")%>">* <%=GetResource("ConfigTabMain")%></span></a></li>
                    <li><a id="ConfigTabUser"><span title="<%=GetResource("ConfigTabUser")%>">* <%=GetResource("ConfigTabUser")%></span></a></li>
                    <li><a id="ConfigPortletUser"><span title="<%=GetResource("ConfigPortletUser")%>">* <%=GetResource("ConfigPortletUser")%></span></a></li>
                    <li><a id="ConfigTabSkin"><span title="<%=GetResource("ConfigTabSkin")%>">* <%=GetResource("ConfigTabSkin")%></span></a></li>
                    <% if (CurrentUserInfo.IsInRole("admin")) { %>
                    <li><hr /></li>
                    <li><a id="ConfigTabAdmin"><span title="<%=GetResource("ConfigTabAdmin")%>">* <%=GetResource("ConfigTabAdmin")%></span></a></li>
                    <% } %>                    
                </ul>
            </div>
            <div class="cont" style="clear:both;">
                <asp:PlaceHolder id="placeholderLayout" runat="server"></asp:PlaceHolder>
            </div>
            <div id="tabPin" style="position:absolute; top:12px;"><img src="<%=BaseImgPath%>/Main/ptl_slide.gif" /></div>
        </div>
        <!-- div class="ptl_layout_bott">
        배너위치
    </div//-->
    </div>
    </form>

<%// Response.Flush();%>

<!--script type="text/javascript">    
    setTimeout(function () {
        $("#splashScreen").hide();
    }, 1000);
</script-->

    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들  
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var TabID = "<%=TabID %>";
        var TabLayout = "<%=TabLayout %>";
        var portletSuccess = true;
        var IsAdmin = "<%=IsAdmin %>";//관리자 탭여부 확인
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var PortletAuth = "<%=PortletAuth%>";
        var IsPortletMove = false;
        var R_SaveChangePortletOrder = "<%=GetResource("SaveChangePortletOrder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmApply = "<%=GetResource("ConfirmApply") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ErrorConnection = "<%=GetResource("ErrorConnection") %>";
        var R_FirstPage = "<%=GetResource("FirstPage") %>";
        var R_LastPage = "<%=GetResource("LastPage") %>";
        var R_SaveTabPortletOrder = "<%=GetResource("SaveTabPortletOrder")%>";
        var R_PortletMove = "<%=GetResource("PortletMove")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/Main.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/Main.js");
    </script>
</body>
</html>
