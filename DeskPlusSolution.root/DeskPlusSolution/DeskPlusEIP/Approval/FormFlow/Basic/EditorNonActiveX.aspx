<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorNonActiveX.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.EditorNonActiveX" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="divXFE" style="height:5px;"></div>
<script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>
<script type="text/javascript">
    xfe = new XFE({
        basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
        width: '100%',
        height: '470px',
        ignoreMinHeight: true
    });
    xfe.render('divXFE');
</script>

    <div style="display:none;"></div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script language="javascript">
        var TemplateHeader = "<%=TemplateHeader %>";
        var TemplateFooter = "<%=TemplateFooter %>";

        var R_Msg_ErrCont = "<%=GetResource("Msg_ErrCont") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Basic/EditorNonActiveX.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Basic/EditorNonActiveX.js");
    </script>

</body>
</html>