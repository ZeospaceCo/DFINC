<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Folder.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.Folder" %>

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
                    <ul class="mobliecontent ">
                        <li class="title text-primary">
                            <i class="fas fa-file-signature dropbtn "></i>
                            <%=GetResource("Approval")%>
                            <div class="float-right date text-dark">
                                <button type="button" id="aWrite" class="btn btn-lg btn-primary" runat="server">
                                    <span class="btnb"><%=GetResource("WriteDoc")%></span>
                                </button>
                            </div>
                        </li>
                        <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                        </li>
                        <li class="contentblock linkDiv" id="aRep">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-file-invoice"></i><font><%=GetResource("Report")%></font><font class="lif"><span id="rep"></span></font>
                                </a>
                                <div><span class="bg-secondary text-white"><%=GetResource("ReportExt")%></span></div>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aRel">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-file-contract"></i><font><%=GetResource("Related")%></font><font class="lif"><span id="rel"></span></font>
                                </a>
                                <div><span class="bg-secondary text-white"><%=GetResource("RelatedExt")%></span></div>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aApp">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-file-signature"></i><font><%=GetResource("Approved")%></font><font class="lif"><span id="app"></span></font>
                                </a>
                                <div><span class="bg-secondary text-white"><%=GetResource("ApprovedExt")%></span></div>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aPro">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-file-signature"></i><font><%=GetResource("ProxyApproved")%></font><font class="lif"><span id="pro"></span></font>
                                </a>
                                <div><span class="bg-secondary text-white"><%=GetResource("ProxyApprovedExt")%></span></div>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aPublic">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("PublicFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aPrivate">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("PrivateFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aApproval">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("ApprovalFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aReject">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("RejectFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aArbitrary">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("ArbitraryFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aCarboncopy">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("CarboncopyFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aForward">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("ForwardFolder")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aCirRecv">
                            <div class="title">
                                <a href="#" class="mct">
                                    <i class="fas fa-folder-open"></i><font><%=GetResource("CirRecv")%></font>
                                </a>
                            </div>
                        </li>
                        <li class="contentblock linkDiv" id="aCirSend">
                            <div class="title">
                                <a href="#" class="mct">
                                   <i class="fas fa-folder-open"></i><font><%=GetResource("CirSend")%></font>
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
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var R_Menu = "<%=GetResource("Menu")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/Folder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/Folder.js");
    </script>
</body>
</html>
