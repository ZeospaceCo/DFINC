<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorNonActiveX.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.EditorNonActiveX" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="divXFE" style="height:5px;"></div>
    <script type="text/javascript" src="/DeskPlusMobile/Editor/XFE2/js/xfe_main.js"></script>
    <script type="text/javascript">
        xfe = new XFE({
            basePath: "<%=httpType%>" + "<%=MobileDOMAIN%>" + "/DeskPlusMobile/Editor/XFE2",
            width: '100%',
            height: '470px'
        });
        xfe.render('divXFE');
    </script>

    <div style="display:none;"></div>

    </form>

    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript">

        var TemplateHeader = "<%=TemplateHeader %>";
        var TemplateFooter = "<%=TemplateFooter %>";

        var R_Msg_ErrCont = "<%=GetResource("Msg_ErrCont")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/EditorNonActiveX.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/EditorNonActiveX.js");
    </script>
</body>
</html>
