<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachDSView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Common.AttachDSView" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; minimum-scale=0.5; user-scalable=yes" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_main_<%=SkinType%>.css" />
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
            var url = "http://sp.deskplus.co.kr:9979/docviewer.melita";

            var form = document.createElement("form");
            var parm = new Array();
            var input = new Array();

            form.action = url;
            //    form.target = "_blank";
            form.method = "post";

            var fileID = <%=fileID %>;

            parm.push(['FILE_ID', fileID]);
            parm.push(['client', 'mobile_web']);

            for (var i = 0; i < parm.length; i++) {
                input[i] = document.createElement("input");
                input[i].setAttribute("type", "hidden");
                input[i].setAttribute('name', parm[i][0]);
                input[i].setAttribute("value", parm[i][1]);
                form.appendChild(input[i]);
            }

            document.body.appendChild(form);
            form.submit();
        })
    </script>
</head>
<body>
<div id="content" style="width:600px; height:700px;"></div>
    
</body>
</html>
