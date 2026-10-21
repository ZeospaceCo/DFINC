<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorActiveX.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.EditorActiveX" %>

<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>

    <div style="display:none;"></div>

    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/activexDPUploadEditor.js"></script>

    <script language="javascript">
        var UploadPath = "<%=UploadPath %>";
        var UploadHost = "<%=BaseHttpVal %>" + "<%=BaseHost %>";
        var UploadPort = "<%=BaseHttpPort %>";

        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var TemplateHeader = "<%=TemplateHeader %>";
        var TemplateFooter = "<%=TemplateFooter %>";

        var R_Msg_ErrCont = "<%=GetResource("Msg_ErrCont") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Basic/EditorActiveX.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Basic/EditorActiveX.js");
    </script>
    <script language="javascript" for="twe" event="OnControlInit()">
        EditorActiveX.ControlInit();
    </script>

</body>
</html>
