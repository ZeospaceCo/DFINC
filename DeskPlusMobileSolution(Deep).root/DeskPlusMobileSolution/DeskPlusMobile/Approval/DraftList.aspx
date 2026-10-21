<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DraftList.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.DraftList" %>
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
    <form id="form1" runat="server" autocomplete="off" onsubmit="return DraftList.SetSearch();">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="ApprovalList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("DraftFolder")%>
                        </span>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <button type="button" class="btn btn-lg btn-success" id="btAll" runat="server">
                                    <span class="btnb" id="allcheck"><%=GetResource("AllSelect")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-danger" id="btDel" runat="server">
                                    <span class="btnr"><%=GetResource("Del")%></span>
                                </button>                               
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup"><!-- 버튼 그룹 -->
                          <div class="float-left">
                                <div class="search-container float-right">
                                    <input type="text" id="tbSearch" class="keyword form-control" maxlength="100" runat="server" />
                                    <button type="button" class="btn btn-xs btn-primary" id="search" style="float:none;">
                                        <%=GetResource("Search")%>
                                    </button>
                                </div>
                          </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent" id="phApproval">
                </ul>
                <ul class="mobilecontent">
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="float-right">
                                <a href="#" class="ubtst">▲<%=GetResource("Top")%></a>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent">
                    <li class="contentblock">
                        <nav aria-label="Page navigation  row">
                            <ul class="pagination date left justify-content-left">
                                <li class="page-item">
                                    <a class="page-link" href="#" id="pvNum">
                                        <span><%=GetResource("BeforePage")%></span>
                                    </a>
                                </li>
                                <li class="page-item">
                                    <a class="page-link">
                                        <span class="bg-light text-black" id="PageNum" runat="server"></span> / <span class="bg-light text-black" id="TotalNum" runat="server"></span>
                                    </a>
                                </li>
                                <li class="page-item">
                                    <a class="page-link" href="#" id="nxNum">
                                        <span><%=GetResource("NextPage")%></span>
                                    </a>
                                </li>
                            </ul>
                        </nav>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <%--<uc2:Footer ID="Footer1" runat="server" />--%>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var PageSize = "10", PageNo = "<%=pageNo%>";
        var UserID = "<%=UserID%>";
        var SearchValue = "<%=searchValue%>";

        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_PrivateDraftName = "<%=GetResource("PrivateDraftName")%>";
        var R_SelAll = "<%=GetResource("SelAll") %>";
        var R_SelNoAll = "<%=GetResource("SelNoAll") %>"; 
        var R_MSG_SelDel  = "<%=GetResource("MSG_SelDel") %>"; 
        var R_DelList  = "<%=GetResource("DelList") %>"; 

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/DraftList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/DraftList.js");
    </script>
</body>
</html>
