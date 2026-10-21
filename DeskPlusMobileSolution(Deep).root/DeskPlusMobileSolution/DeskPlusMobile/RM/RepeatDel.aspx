<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepeatDel.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.RM.RepeatDel" %>

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
                            <i class="fas fa-calendar-minus dropbtn "></i><%=GetResource("RepeatDel")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" runat="server" id="dvAdd">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btConfirm">
                                    <span class="btnb"><%=GetResource("Del")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div id="ct">
                                <ul class="mli">
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("RepeatGubun1")%></span>
                                            <br />
                                            <input type="radio" id="rdRepeat1" name="rdPims" class="rd" checked="checked" /><label for=""><%=GetResource("RepeatEx1")%></label>
                                            <br />
                                        </span>
                                    </li>
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("RepeatGubun2")%></span>
                                            <br />
                                            <input type="radio" id="rdRepeat2" name="rdPims" class="rd" /><label for=""><%=GetResource("RepeatEx2")%></label>
                                            <br />
                                        </span>
                                    </li>
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("RepeatGubun3")%></span>
                                            <br />
                                            <input type="radio" id="rdRepeat3" name="rdPims" class="rd" /><label for=""><%=GetResource("RepeatEx3")%></label>
                                            <br />
                                        </span>
                                    </li>
                                </ul>
                            </div>
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
        var ParentReservationID = "<%=parentReservationID%>";      
        var WriteYM = "<%=writeYM%>";      
        var RepeatSel = "";
        var StartDate = "<%=startDate%>";
        var PageNo = "<%=pageNo%>";      
        var SearchValue = "<%=searchValue%>";
        var TmpDate = "<%=tmpDate%>";        
        var BPage = "<%=bPage%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";        
        var R_DelList = "<%=GetResource("DelList") %>"; 
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";        
        var R_Menu = "<%=GetResource("Menu")%>";        
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/RM/RepeatDel.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/RM/RepeatDel.js");
    </script>
</body>
</html>
