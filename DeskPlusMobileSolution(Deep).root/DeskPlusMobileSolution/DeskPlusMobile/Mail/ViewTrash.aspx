<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTrash.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Mail.ViewTrash" %>

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
    <div data-role="page" id="MailView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-envelope dropbtn "></i><%=GetResource("Trash")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-primary" id="btRestore">
                                    <span class="btnb"><%=GetResource("Restore")%></span>
                                </button>
                                <div class="p2">
                                    <span id="RestoreMessage" class="my">
                                        <%=GetResource("RestoreMessage")%></span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title">
                            <span id="subject"></span>
                            <div>
                                <div class="divHeader"><%=GetResource("Sender")%></div><div id="fromList" class="divList fontW400"></div>
                            </div>
                            <div>
                                <div class="divHeader"><%=GetResource("SendDate")%></div><div id="dateList" class="divList fontW400"></div>
                            </div>
                            <div>
                                <div id="btToCc" style="cursor: pointer">
                                    (<%=GetResource("ViewToCc")%> ▼)
                                </div>
                            </div>
                            <div id="ToCc" style="display: none">
                                <div><div class="divHeader"><%=GetResource("Receiver")%></div><div id="toList" class="divList fontW400"></div></div>
                                <div id="dtcc" style="display: none;"><div class="divHeader"><%=GetResource("Refer")%></div><div id="ccList" class="divList fontW400"></div></div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock di2" id="att_line" style="display: none">
                        <div class="title">
                            <span class="istM">
                                <%=GetResource("Attach")%></span>
                            <br />
                            <div id="attachList" style="width: 100%">
                            </div>
                        </div>
                    </li>
                    <li class="contentblock2 bg-white">
                        <div class="title">
                            <div class="rc" id="contentList">
                            </div>
                        </div>
                    </li>
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
        var Mail_sn = "<%=mail_sn%>";
        var Read_flag = "<%=read_flag%>";
        var Code = "<%=code%>", PageNo = "<%=pageNo%>";
        var MailUserID = "<%=mailUserID%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var ShowToCc = "N";
        var MobileType = "<%=MobileType %>";
        var NextUid;
        var PrevUid;
        var R_Error = "<%=GetResource("Error") %>"; 
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_DelList = "<%=GetResource("DelList") %>";
        var R_NoList = "<%=GetResource("NoList") %>";
        var R_ViewToCc = "<%=GetResource("ViewToCc")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Mail/ViewTrash.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Mail/ViewTrash.js");
    </script>
</body>
</html>
