<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetLogOut.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.SetLogOut" %>
<!DOCTYPE html>
<html lang="ko">
<head>
<title></title>
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
         <p class="sv"><%=Today%></p>
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
        var UUID = "<%=TmpUUID%>";
        var MobileType = "<%=TmpMobileType%>";

        $(document).ready(function () {

            if ((UUID != "") && (MobileType == "Android")) {
                window.onpageshow = function (event) {

                    if (event.persisted || (window.performance && window.performance.navigation.type == 2)) {

                        if (window.confirm("앱을 종료하시겠습니까?")) {
                            closeApp();
                        }
                    }
                };
            }
        });

        function setUserInfo(key, lid, cid) {
            var keyData = key;
            var lidData = lid, cidData = cid;
            var message = {
                "command": "set_user_info",
                "data": {
                    "key": keyData,
                    "LID": lidData,
                    "CID": cidData
                }
            }
            sendZeospaceMessageAn(message);
        }

        function preventBack() {
            window.history.forward(0);

            if (UUID == "") {
                window.location.href = "Default.aspx";
            }
            else {
                if (MobileType == "Android") {
                    try {
                        setUserInfo("lcID", "", "");
                    } catch (err) {
                        //alert(err);
                    }
                    window.location.href = "AnLogin.aspx";
                }
                else if (MobileType == "ios") {
                    try {
                        //window.webkit.messageHandlers.set_badge.postMessage("0");
                        setBadgeIOS(0);
                    } catch (err) {
                        //alert(err);
                    }

                    window.location.href = "IosLogin.aspx";
                }
                else {
                    window.location.href = "Default.aspx";
                }
            }
        }

        window.onload = preventBack;
        window.onpageshow = function (evt) { if (evt.persisted) preventBack(); }
        window.onunload = function () { void (0) };    
    </script>
</body>
</html>
