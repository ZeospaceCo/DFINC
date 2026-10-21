<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFolder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.WFolder" %>
<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
        <div id="divprg" align="center" class="loading">
        </div>
        <div data-role="page" id="ApprovalFolder">
            <uc1:Menu ID="Menu1" runat="server" />
            <div class="bodycontainer">
                <div class="introcontentblock bottomcontent">
                    <ul class="mobilecontent">
                        <li class="title text-primary">
                            <span id="listTitle">
                                <i class="fas fa-file-signature dropbtn "></i><%=GetResource("SelectForm")%>
                            </span>
                            <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                        </li>
                        <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                        </li>     
                    </ul>  
                    <ul class="mobilecontent" id="phFavoriteForm"></ul> 
                    <ul class="mobilecontent">               
                        <li class="contentblock linkDiv" id="aForm">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("FormFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aSubmit">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("SubmitFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aDraft">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("DraftFolder")%></font>
                                </a>
                            </div>
                        </li>                       
                    </ul>
                </div>
            </div>
        <%--<uc2:Footer ID="Footer1" runat="server" />--%>
        </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/WFolder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/WFolder.js");
    </script>
</body>
</html>
