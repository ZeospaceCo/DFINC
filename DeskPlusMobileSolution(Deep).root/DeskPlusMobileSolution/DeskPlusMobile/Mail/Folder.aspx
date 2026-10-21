<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Folder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Mail.Folder" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="MailFolder">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul>
                    <li class="title text-primary">
                        <i class="fas fa-envelope dropbtn "></i>
                        <%=GetResource("MailTitle")%>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock linkDiv" id="rec">
                        <div class="title">
                            <a href="#">
                                <i class="fas fa-envelope"></i>
                                <font><%=GetResource("ReceiveMail")%></font><font class="lif"><span id="Receive"></span></font>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock linkDiv" id="sen">
                        <div class="title">
                            <a href="#">
                                <i class="fas fa-envelope-open"></i>
                                <font><%=GetResource("SendMail")%></font><font class="lif"><span id="Send"></span></font>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock linkDiv" id="dra">
                        <div class="title">
                            <a href="#">
                                <i class="fas fa-envelope"></i>
                                <font><%=GetResource("DraftMail")%></font><font class="lif"><span id="Draft"></span></font>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock linkDiv" id="del">
                        <div class="title">
                            <a href="#">
                                <i class="fas fa-trash-alt"></i>
                                <font><%=GetResource("Trash")%></font><font class="lif"><span id="DelBox"></span></font>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock linkDiv" id="pri">
                        <div class="title">
                            <a href="#">
                                <i class="fas fa-envelope"></i>
                                <font><%=GetResource("PrivateMail")%></font><font class="lif"><span id="Private"></span></font>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock linkDiv" id="wri">
                        <div class="title">
                            <a href="#">
                            <i class="far fa-edit"style="color:#555;font-size:1.2em;"></i>
                            <font><%=GetResource("MailSend")%></font><font class="lif"><span id="Write"></span></font>
                            </a>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Mail/Folder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Mail/Folder.js");
    </script>
</body>
</html>
