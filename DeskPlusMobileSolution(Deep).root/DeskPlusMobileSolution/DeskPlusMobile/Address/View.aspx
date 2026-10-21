<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Address.View" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="format-detection" content="telephone=no">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="AddressView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-address-book dropbtn "></i><%=GetResource("ContactList")%>
                        </span>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li>
                        <div id="ct">
	                        <div class="mpw">
		                        <div class="mpifw">
			                        <div class="mppt">
                                        <img src="/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif" width="90" height="105" alt="사진"/>
                                    </div>
			                        <ul class="mpul">
                                        <li class="mpl border0">
                                            <div class="mpt"><span id="userInfo"></span></div>
                                            <div class="mps"><span id="deptName"></span></div>
                                            <div class="mpss"><span id="deptEtc"></span></div>
                                        </li>
                                        <li class="mpr border0" >
                                            <a href="#" class="/Content/ko/Base/Img/" id="mytalk"></a>   
                                        </li>
			                        </ul>
		                        </div>
                                <br />
                                <div class="mpifw">
                                    <ul class="mpuls">
                                        <li class="mpl border0">
                                            <div class="mpd"><%=GetResource("CellPhone")%></div>
                                            <div class="mps"><span id="scallMobile"></span></div>                        
                                        </li>
                                        <li class="mpr border0">
                                            <a href="#" class="btr3" id="callMobile"></a>
                                        </li>
                                    </ul>
                                    <ul class="mpulse">
                                        <li class="mpl border0">
                                            <div class="mpd"><%=GetResource("CellPhoneSms")%></div>
                                            <div class="mps"><span id="ssmsMobile"></span></div>
                                        </li>
                                        <li class="mprC border0">
                                            <a href="#" class="btr3" id="smsMobile"></a>
                                        </li>
                                    </ul>
                                    <ul class="mpulse">
                                        <li class="mpl border0">
                                            <div class="mpd"><%=GetResource("OfficePhone")%></div>
                                            <div class="mps"><span id="scallOffice"></span></div>
                                        </li>
                                        <li class="mprC border0">
                                            <a href="#" class="btr3" id="callOffice"></a>
                                        </li>
                                    </ul>
                                    <ul class="mpulse">
                                        <li class="mpl border0">
                                            <div class="mpd">E-mail</div>
                                            <div class="mps"><span id="smail"></span></div>
                                        </li>
                                        <li class="mprC border0">
                                            <a href="#" class="btr3" id="mail"></a>
                                        </li>
                                    </ul>
                                </div>
	                        </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup full-width">
                            <button type="button" class="btn btn-outline-light full-width" id="before"><%=GetResource("List")%></button>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var MobileType = "<%=MobileType %>";
        var Gubun = "<%=gubun%>";
        var OwnerID = "<%=ownerID%>";
        var SearchValue = "<%=searchValue%>";
        var PageNo = "<%=pageNo%>";
        var FastPath = "<%=fastPath%>";
        var FirstChar = "<%=firstChar%>";
        var MailCheck = "<%=mailCheck%>";
        var MyTalkCheck = "<%=myTalkCheck%>";
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Address/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Address/View.js");
    </script>
</body>
</html>
