<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.RM.View" %>

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
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loadingH">
    </div>
    <div data-role="page" id="MailView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-calendar-minus dropbtn "></i><%=GetResource("ReserveResourceView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-danger" id="btDel" style="display: none">
                                    <span class="btnr"><%=GetResource("Del")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-info" id="btMod" style="display: none">
                                    <span class="btnb"><%=GetResource("Mod")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btReply">
                                    <span class="btnb"><%=GetResource("Comment")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btApproved" style="display: none">
                                    <span class="btnb"><%=GetResource("Usable1")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btUnApproved" style="display: none">
                                    <span class="btnb"><%=GetResource("Usable2")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title">
                            <span id="title"></span>
                            <div id="date"></div>
                            <div id="etc"></div>
                        </div>
                    </li>
                    <li class="contentblock bg-white">
                        <div class="title">
                            <div id="content"></div>
                        </div>
                    </li>
                    <li class="contentblock bg-white" id="reply" style="display: none">
                    </li>
                    <li class="contentblock bg-white" id="reject" style="display: none">
                    </li>
                    <li class="contentblock" id="att_line" style="display: none">
                        <div class="title">
                            <span class="istM">
                                <%=GetResource("Attach")%></span>
                            <br />
                            <div id="attach" style="width: 100%">
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
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">        
        var ReservationID = "<%=reservationID%>";      
        var ParentReservationID = "";      
        var WriteYM = "";      
        var StartDate = "";
        var PageNo = "<%=pageNo%>";      
        var SearchValue = "<%=searchValue%>";
        var TmpDate = "<%=tmpDate%>";        
        var BPage = "<%=bPage%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";        
        var R_SecuritySetting = "<%=GetResource("SecuritySetting") %>"; 
        var R_ModifyDate = "<%=GetResource("ModifyDate") %>"; 
        var R_ReserveDate = "<%=GetResource("ReserveDate") %>"; 
        var R_Complete = "<%=GetResource("Complete") %>"; 
        var R_DelList = "<%=GetResource("DelList") %>"; 
        var R_RejectStr = "<%=GetResource("RejectStr") %>"; 
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";        
        var R_Menu = "<%=GetResource("Menu")%>";        
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/RM/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/RM/View.js");
    </script>
</body>
</html>
