<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForwardView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Task.ForwardView" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html>
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
    <div data-role="page" id="ApprovalView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-puzzle-piece dropbtn "></i>
                            <%=GetResource("Detail")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" id="btnReply" class="btn btn-lg btn-primary">
                                    <span class="btnb"><%=GetResource("CReply")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title" style="padding-left:0.8em;">
                            <span id="subject"></span>
                        </div>
                        <div class="container-fluid fontW400">
                            <span class="dif" id="etc"></span>
                            <br />
                            <span class="dih"><%=GetResource("RequestDate")%>: </span>
                            <span id="requestDate"></span>
                            &nbsp;&nbsp;<span class="dih"><%=GetResource("CompleteRequestDate")%>: </span>
                            <span id="completeRequestDate"></span>
                            <br />
                            <span class="dih"><%=GetResource("StartDate")%>: </span>
                            <span id="startDate"></span>
                            &nbsp;&nbsp;<span class="dih"><%=GetResource("EndDate")%>: </span>
                            <span id="endDate"></span>
                            <br />
                            <span class="dih"><%=GetResource("ItemGubun")%>: </span>
                            <span id="item"></span> -> <span id="itemsub"></span>
                            <br />
                            <span class="dih"><%=GetResource("Gubun")%>: </span>
                            <span id="isGubun"></span>
                            &nbsp;&nbsp;<span class="dih"><%=GetResource("Source")%>: </span>
                            <span id="source"></span>
                            <br />
                            <span class="dih"><%=GetResource("IsClose")%>: </span>
                            <span id="isClose"></span>
                            &nbsp;&nbsp;<span class="dih"><%=GetResource("History")%>: </span>
                            <span id="history"></span>
                            <br />
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent ">
                    <li class="contentblock">
                        <div class="container-fluid fontW400">
                            <div class="mct">
                                <div id="requester" style="width: 100%">
                                </div>
                                <div id="to" style="width: 100%">
                                </div>
                                <div id="cc" style="width: 100%">
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent ">
                    <li class="contentblock">
                        <div class="container-fluid">
                            <asp:Panel ID="pnContent" runat="server"></asp:Panel>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent ">
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
        var ListID = "<%=listID%>", GroupID = "<%=groupID%>", ForwardID = "<%=forwardID %>";
        var PageNo = "<%=pageNo%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";    
        var SearchValue = "<%=searchValue%>";
        var Status = "<%=status%>";
        var PreLang = "<%=PreLang %>";
        var MobileType = "<%=MobileType %>";  
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_Requester = "<%=GetResource("Requester")%>";
        var R_To = "<%=GetResource("To")%>";
        var R_Cc = "<%=GetResource("Cc")%>";
        var R_CoopStatus1 = "<%=GetResource("CoopStatus1")%>";
        var R_CoopStatus2 = "<%=GetResource("CoopStatus2")%>";
        var R_CoopStatus3 = "<%=GetResource("CoopStatus3")%>";
        var R_CoopStatus4 = "<%=GetResource("CoopStatus4")%>";
        var R_TaskStatus5 = "<%=GetResource("TaskStatus5")%>";
        var R_TaskStatus6 = "<%=GetResource("TaskStatus6")%>";
        var R_IsClose1 = "<%=GetResource("IsClose1")%>";
        var R_IsClose2 = "<%=GetResource("IsClose2")%>";
        var R_DelCoop  = "<%=GetResource("DelCoop")%>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_Reject  = "<%=GetResource("Reject") %>";
        var R_TIsClose1 = "<%=GetResource("TIsClose1") %>";
        var R_TIsClose2 = "<%=GetResource("TIsClose2") %>";
        var R_IsGubun1 = "<%=GetResource("IsGubun1") %>";
        var R_IsGubun2 = "<%=GetResource("IsGubun2") %>";
        var R_ChangeTO = "<%=GetResource("ChangeTO") %>";
        var R_ChangeCC = "<%=GetResource("ChangeCC") %>";
        var R_ChangeDate = "<%=GetResource("ChangeDate") %>";
        var R_NoList = "<%=GetResource("NoList") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Task/ForwardView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Task/ForwardView.js");
    </script>
</body>
</html>
