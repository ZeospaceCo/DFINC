<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.MDM.Web.MenuAuth.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_MDM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_MDM.css" /> 
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=SiteMap%></h1> 
                <ul class="btn_right">                        
                    <cc3:ClientSmartButton ID="cbtnADDAuth" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>                        
                </ul>             
            </div>
            <div id="layout_content">   
                <div class="row">
				  <div class="col-12 content-area-body"><!-- width 100%(col-12) -->            
                    <!-- 권한/역활 접근제어 -->
                    <div id="placeholder_Role" style="clear: both;"></div>
                    <!-- 권한/역활 접근제어 -->     
                  </div>
                </div>                             
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
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var MenuID = "<%=MenuID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";  
        var R_AuthName = "<%=GetResource("AuthName") %>";  
        var R_Del = "<%=GetResource("Del") %>";  
        var R_ConfirmDel = "<%=GetResource("ConfirmDel") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/MDM/Js/MenuAuth/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/MDM/Js/MenuAuth/Add.js");
    </script>
</body>
</html>
