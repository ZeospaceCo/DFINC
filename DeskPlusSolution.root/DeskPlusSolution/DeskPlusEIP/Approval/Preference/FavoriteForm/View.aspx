<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.FavoriteForm.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
<form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">

            <div id="popup_header">
                <h1><%=GetResource("ViewForm")%></h1>
                <ul class="btn_right">
                    <li><a href="#" id="Close" class="btn"><span><%=GetResource("Close")%></span></a></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            
            <!-- 양식로드 //-->
            <iframe id="ifrmForm" name="ifrmForm" src="about:blank" frameborder="0" runat="server" class="app_iframe"></iframe>

        </div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    </div>
    <!-- ==================================================================================== -->

</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var FormID = "<%=FormID%>";
        var ifrmObj = null;                     // ifrmForm object
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/FavoriteForm/View.js"></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/FavoriteForm/View.js");
    </script>

</body>
</html>
