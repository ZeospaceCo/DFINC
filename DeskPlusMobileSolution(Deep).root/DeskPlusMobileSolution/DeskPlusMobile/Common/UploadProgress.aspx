<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadProgress.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Common.UploadProgress" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
<meta charset="uft8" />
<meta name="viewport" content="initial-scale=1.0; maximum-scale=1.0; minimum-scale=1.0; user-scalable=no;" />
<meta http-equiv="Expires" content="-1"/> 
<meta http-equiv="Pragma" content="no-cache"/> 
<meta http-equiv="Cache-Control" content="no-cache"/> 
<link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/mobile_login.css"/>
</head>
<body onunload="">
    <form id="form1" runat="server">
    <div class="tm_wrap"></div>
    <!-- header -->
    <div id="hd"></div>
    <!-- //header -->
    <hr />
    <!-- content -->
    <div id="ct">
    <h1></h1>
    <div class="sc1">	
    <fieldset>
    <legend class="hc"></legend>
        <div class="ia" style="text-align:center;">
        <ul class="ot"><li id="msg"></li></ul>
        </div>
         <div class="ot"  style="text-align:center;">
         <p class="sv">File Uploading...</p>
         </div>
    </fieldset>
    </div>
    </div>
        <hr />
        <div id="txtMessage"></div>    
        <br /><br /><br /><br />
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type = "text/javascript" >
        function goBeforePath(arg) {
            location.href = arg;
        }
    </script>
</body>
</html>

