<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComplete.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Task.AddComplete" %>

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
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="TaskAdd">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-puzzle-piece dropbtn "></i><%=GetResource("CoopComplete")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <ul class="mobilecontent di2" id="task_calendar" style="display: none;">
                    <li>
                        <div class="chh">
                            <h1>
                                <a href="#" class="bt5 bt5pvm" title="이전달" id="pvMonth">
                                    <div>
                                        <span class="hc2"><%=GetResource("BeforeMonth")%></span>
                                    </div>
                                </a> <span id="timeList"></span> <a href="#" class="bt5 bt5nxm" title="다음달" id="nxMonth">
                                    <div>
                                        <span class="hc2"><%=GetResource("NextMonth")%></span>
                                    </div>
                                </a>
                            </h1>
                        </div>
                    </li>
                    <li>
                        <div class="mct" style="background: #C4C6E6;">
                            <div id="phCal">
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobilecontent">
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" id="btCancel" class="btn btn-lg btn-danger">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" id="btAdd" class="btn btn-lg btn-primary">
                                    <span class="btnb"><%=GetResource("Save")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title width100" style="width:100px;">
                                    <%=GetResource("StartDate")%>
                                </div>
                                <div class="area2">
                                    <input type="text" id="tbStartDate" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title width100">
                                    <%=GetResource("EndDate")%>
                                </div>
                                <div class="area2">
                                    <input type="text" id="tbEndDate" class="form-control" runat="server" readonly />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <textarea id="memo" class="form-control ta3"></textarea>
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
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var BPage = "<%=bPage%>", ListID = "<%=listID%>", GroupID = "<%=groupID%>";
        var BType = "<%=bType%>";
        var PageNo = "<%=pageNo%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";    
        var SearchValue = "<%=searchValue%>";
        var Status = "<%=status%>";
        var StartCalMonth = "<%=startCalMonth%>";
        var EndCalMonth = "<%=endCalMonth%>";
        var BtnClickCheck = ""; 
        var CalCheck = "";
        var PreLang = "<%=PreLang %>";
        var Alarm  = "<%=alarm %>";

        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>";     
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_Menu = "<%=GetResource("Menu")%>";    
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>"; 
        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>"; 
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";   
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Task/AddComplete.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Task/AddComplete.js");
    </script>
</body>
</html>
