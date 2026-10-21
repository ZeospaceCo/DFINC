<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Board.List" %>

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
    <form id="form1" runat="server" autocomplete="off" onsubmit="return List.SetSearch();">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="BoardList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-align-left  dropbtn "></i><%=Title%>
                        </span>
                        <div class="float-right date text-dark linkDiv" id="spAdd" runat="server">
                            <a href="#" id="add">+<%=GetResource("Write")%>&nbsp;&nbsp;</a>
                        </div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup"><!-- 버튼 그룹 -->
                            <div class="float-left">
                                <div class="search-container float-right">
                                    <input type="text" id="tbSearch" maxlength="100" class="keyword form-control" runat="server" />
                                    <button type="button" class="btn btn-xs btn-primary" id="search" style="float:none;">
                                        <span class="btnb"><%=GetResource("Search")%></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent padding0" id="phBoard" style="clear: both;">
                        
                </ul>
                <ul class="mobliecontent ">
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="float-right">
                                <a href="#" class="ubtst">▲<%=GetResource("Top")%></a>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent ">
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
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var PageSize = "10", PageNo = "<%=pageNo%>";
        var ItemToLoad = null;        
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var MainCID = "<%=mainCID %>";
        var SearchValue = "<%=searchValue%>";
        var PCatID = "<%=pCatID %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";    
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_InputBoa = "<%=GetResource("InputBoa")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Board/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Board/List.js");
    </script>
</body>
</html>
