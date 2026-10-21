<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoardView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Community.BoardView" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=2.0, minimum-scale=0.5, user-scalable=yes, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/<%=SkinType%>/navimobile.css" />
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loadingH">
    </div>
    <div data-role="page" id="BoardView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent "> 
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class='fas fa-users dropbtn '></i>
                            <%=GetResource("BoardView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" id="btnAdd" class="btn btn-lg btn-primary">
                                    <span class="btnb"><%=GetResource("Comment")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title">
                            <span><%=Subject%></span>
                            <div><%=WriteUserInfo%></div>
                            <div><%=WriteDate%> | <%=GetResource("Viewing")%> : <%=ReadCount%> | <%=GetResource("RecomCount")%> : <%=RecomCount%></div>
                        </div>
                    </li>
                    <%=AttachList %>
                    <li class="contentblock bg-white">
                        <div class="title full-width">
                            <div class="dvb"><%=Content%></div>
                        </div>
                    </li>
                    <%=ShortList %>
                    <li class="contentblock">
                        <nav aria-label="Page navigation example row">
                            <ul class="pagination left ">
                                <li class="page-item">
                                    <a class="page-link" href="#" id="minus"><%=GetResource("BeforeDoc")%></a>
                                </li>
                                <li class="page-item">
                                    <a class="page-link" href="#" id="plus"><%=GetResource("NextDoc")%></a>
                                </li>
                            </ul>
                            <ul class="pagination right ">
                                <li class="page-item">
                                    <a class="page-link" href="#" id="before"><%=GetResource("List")%></a>
                                </li>
                            </ul>
                      </nav>
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
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var PageNo = "<%=pageNo%>";
        var MenuID = "<%=menuID %>";
        var CommunityID = "<%=communityID %>";
        var BoardID = "<%=boardID %>";
        var BeforeBoardID = "<%=BeforeBoardID %>";
        var NextBoardID = "<%=NextBoardID %>";
        var SearchValue = "<%=searchValue%>";
        var ReadAuth = "<%=readAuth %>";
        var WriteAuth = "<%=writeAuth %>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_NoList = "<%=GetResource("NoList") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Community/BoardView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Community/BoardView.js");
    </script>
</body>
</html>
