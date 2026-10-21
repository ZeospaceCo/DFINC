<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.RM.List" %>

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
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="RMList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                        </span>
                        <div class="float-right date text-dark">
                            <a href="#" id="add">
                                +<%=GetResource("ResourcesAdd")%>
                            </a>
                        </div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <ul class="mobilecontent di2">
                    <li>
                        <div class="chh">
                            <h1>
                                <a href="#" class="bt5 bt5pvm" title="이전달" id="pvMonth">
                                    <div>
                                        <span class="hc2"><%=GetResource("BeforeMonth")%></span>
                                    </div>
                                </a>
                                <a href="#" class="bt5 bt5pv" title="이전일" id="pvDay">
                                    <div>
                                        <span class="hc2"><%=GetResource("BeforeDay")%></span>
                                    </div>
                                </a> <span id="timeList"></span> <a href="#" class="bt5 bt5nx" title="다음일" id="nxDay">
                                    <div>
                                        <span class="hc2"><%=GetResource("NextDay")%></span>
                                    </div>
                                </a>
                                <a href="#" class="bt5 bt5nxm" title="다음달" id="nxMonth">
                                    <div>
                                        <span class="hc2"><%=GetResource("NextMonth")%></span>
                                    </div>
                                </a>
                            </h1>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent di2" id="rm_calendar" name="rm_calendar" style="display: none;">
                    <li>
                        <div class="mct" style="background: #C4C6E6;">
                            <div id="phCal">
                            </div>
                            <!-- 6주가 안되는 달인 경우 빈줄을 추가한다. -->
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent" id="phRM" style="clear: both;">
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
        var NowYear = "<%=System.DateTime.Now.Year.ToString()%>";
        var NowMonth = "<%=System.DateTime.Now.Month.ToString()%>";
        var NowDay = "<%=System.DateTime.Now.Day.ToString()%>";
        var NowDate = "";
        var TmpDate = "<%=tmpDate%>";
        var UserID = "<%=UserID%>";        
        var IsOpen = "";
        var TmpClassName = "";
        var TmpChoice = "";
        var BPage = "<%=bPage%>";        
        var PreLang = "<%=PreLang %>";
        var R_Calendar = "<%=GetResource("Calendar") %>";     
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";     
        var R_NoContent = "<%=GetResource("NoContent") %>";             
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_MyReserve = "<%=GetResource("MyReserve")%>";
        var R_ResourcesReserve = "<%=GetResource("ResourcesReserve")%>";
        var cal = "<%=GetResource("Calendar")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/RM/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/RM/List.js");
    </script>
</body>
</html>
