<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Board.AlarmView" %>

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
    <div data-role="page" id="BoardView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-align-left  dropbtn "></i><%=GetResource("Board")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" runat="server" id="dvAdd">
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
                            <div><%=WriteDate%> | <%=GetResource("Viewing")%> : <%=ReadCount%></div>
                        </div>
                    </li>
                    <%=AttachList %>
                    <li class="contentblock bg-white">
                        <div class="title full-width">
                            <div class="rc">
                                <%=Content%>
                                <%=ImgUrl%>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="TitleShortReply" style="display: none">
                        <div class="title">
                            <span id="AddShortReply"><%=GetResource("AddShortReply")%></span>&nbsp;<span class="ReplyRowCnt"></span>
                        </div>
                        <div class="full-width" id="phShortList" style="clear: both;">
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
    <input type="hidden" id="hdCompanyID" />
    <input type="hidden" id="hdShortReplyID" />
    <input type="hidden" id="hdGood" />
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CID = "<%=categoryID %>";
        var ID = "<%=listID %>";
        var PageNo = "<%=pageNo %>";     
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_NoList = "<%=GetResource("NoList") %>";
        var OptionDownload = "<%=optionDownload %>";

        var OptionUseShortReply = "<%=optionUseShortReply %>";
        var OptionShortReplyGood = "<%=optionShortReplyGood %>";
        var OptionShortReplyTop = "<%=optionShortReplyTop %>";

        var R_AddShortReply = "<%=GetResource("AddShortReply") %>";
        var R_DelReply = "<%=GetResource("DelReply") %>";
        var R_Save = "<%=GetResource("Save") %>";
        var R_InputComment = "<%=GetResource("InputComment") %>";
        var R_DelList = "<%=GetResource("DelList") %>";
        var R_MyComment = "<%=GetResource("MyComment") %>";
        var R_AlreadyComment = "<%=GetResource("AlreadyComment") %>";
        var R_Recommend = "<%=GetResource("Recommend") %>";
        var R_Decommendation = "<%=GetResource("Decommendation") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Board/AlarmView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Board/AlarmView.js");
    </script>
</body>
</html>
