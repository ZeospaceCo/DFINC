<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOut.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Portal.LogOut" %>
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
         쿠키정보가 만료되어서<br>
         다시 로그인창으로 이동 후<br>
         로그인처리됩니다.<br>
         (App에서 자동로그인설정시)</p>
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

        function preventBack() {
            window.history.forward(0);

            if (UUID == "") {
                var tmpYN = "N";

                var mobileKeyWords = new Array('iPhone', 'iPad');
                for (var word in mobileKeyWords) {
                    if (navigator.userAgent.match(mobileKeyWords[word]) != null) {
                        try {
                            //window.webkit.messageHandlers.get_value.postMessage('native_call');
                            tmpYN = "Y";
                            window.location.href = "IosLogin.aspx";
                        } catch (err) {
                            //alert(err);
                        }
                        break;
                    }
                }

                var mobileKeyWords = new Array('Android');
                for (var word in mobileKeyWords) {
                    if (navigator.userAgent.match(mobileKeyWords[word]) != null) {
                        try {
                            //Android.get_value();
                            tmpYN = "Y";
                            window.location.href = "AnLogin.aspx";
                        } catch (err) {
                            //alert(err);
                        }
                        break;
                    }
                }

                if (tmpYN == "N") {
                    window.location.href = "Default.aspx";
                }
            }
            else {
                if (MobileType == "Android") {
                    window.location.href = "AnLogin.aspx";
                }
                else if (MobileType == "ios") {
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
