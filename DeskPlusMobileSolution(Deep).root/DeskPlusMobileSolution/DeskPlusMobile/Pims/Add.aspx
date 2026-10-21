<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Pims.Add" %>

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
    <div data-role="page" id="PimsAdd">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-calendar dropbtn "></i><%=GetResource("PimsAdd")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <ul class="mobilecontent di2" id="pims_calendar" style="display: none;">
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
                    <li class="contentblock" runat="server" id="dvAdd">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAdd">
                                    <span class="btnb"><%=GetResource("Save")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Gubun")%>
                                </div>
                                <div class="area">
                                    <span class="rdwrp fst">
                                        <input type="radio" id="rdpims_gb1" name="pims_gb" class="rd" value="tosc" checked /><label for=""><%=GetResource("Schedule")%></label></span> <span class="rdwrp">
                                                <input type="radio" id="rdpims_gb2" name="pims_gb" class="rd" value="todo" /><label for=""><%=GetResource("ToDo")%></label></span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Date")%>
                                </div>
                                <div class="area">
                                    <input type="text" id="tbStartDate" style="display:inline-block; width:50%;" class="form-control" runat="server" readonly />
                                    <span id="stime_set">&nbsp;<asp:DropDownList ID="ddlstartTime" runat="server" style="display:inline-block;width:auto;" CssClass="form-control"></asp:DropDownList> ~ </span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="data_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"></div>
                                <div class="area">
                                    <input type="text" id="tbEndDate" style="display:inline-block; width:50%;" class="form-control" runat="server" readonly />
                                    <span id="etime_set">&nbsp;<asp:DropDownList ID="ddlendTime" runat="server" style="display:inline-block;width:auto;" CssClass="form-control"></asp:DropDownList></span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Subject")%></div>
                                <div class="area">
                                    <input type="text" id="tbTitle" name="" maxlength="100" class="form-control" />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="openType_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Setting")%></div>
                                <div class="area">
                                    <span class="rdwrp fst">
                                        <input type="radio" id="rdopenType1" name="openType" value="1" class="rd" checked /><label for=""><%=GetResource("Share")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="rdopenType2" name="openType" value="0" class="rd" /><label for=""><%=GetResource("Private")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="rdopenType3" name="openType" value="3" class="rd" /><label for=""><%=GetResource("Dept")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="rdopenType4" name="openType" value="2" class="rd" /><label for=""><%=GetResource("All")%></label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="status_set" style="display:none;">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Status")%></div>
                                <div class="area">
                                    <span class="rdwrp fst">
                                        <input type="radio" id="rdstatus1" name="status" value="W" class="rd" checked /><label for=""><%=GetResource("Status1")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="rdstatus2" name="status" value="I" class="rd" /><label for=""><%=GetResource("Status2")%></label>
                                    </span>
                                    <span class="rdwrp">
                                        <input type="radio" id="rdstatus3" name="status" value="E" class="rd" /><label for=""><%=GetResource("Status3")%></label>
                                    </span>
                                    <span class="rdwrp">
                                        <input type="radio" id="rdstatus4" name="status" value="C" class="rd" /><label for=""><%=GetResource("Status4")%></label>
                                    </span>
                                    <span class="rdwrp">
                                        <input type="radio" id="rdstatus5" name="status" value="T" class="rd" /><label for=""><%=GetResource("Status5")%></label>
                                    </span>
                                    <span class="rdwrp">
                                        <input type="radio" id="rdstatus6" name="status" value="D" class="rd" /><label for=""><%=GetResource("Status6")%></label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="priority_set" style="display:none;">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Priority")%></div>
                                <div class="area">
                                    <span class="rdwrp fst">
                                        <input type="radio" id="priority1" name="priority" value="1" class="rd" /><label for=""><%=GetResource("Priority1")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="priority2" name="priority" value="2" class="rd" /><label for=""><%=GetResource("Priority2")%></label>
                                    </span> <span class="rdwrp">
                                        <input type="radio" id="priority3" name="priority" value="3" class="rd" checked /><label for=""><%=GetResource("Priority3")%></label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="statusID_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Category")%></div>
                                <div class="area">
                                    <asp:DropDownList ID="ddlStatus" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Content")%></div>
                                <div class="area1 full-width">
                                    <textarea id="tbcontent" class="form-control"></textarea>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("PimAlarm")%></div>
                                <div class="area">
                                    <asp:DropDownList ID="ddlnotikind" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                                        
                                    <asp:DropDownList ID="ddlAlarmTime" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"><asp:ListItem Text="Time" Value="0"></asp:ListItem></asp:DropDownList>
                                    <span id="divment"><%=GetResource("BeforeNoti")%></span>
                                    <span id="divEmail">
                                        <br /><input name="idEmail" type="text" class="form-control" value="" id="idEmail" runat="server" style="width:250px !important;" maxlength="200" />
                                        <%=GetResource("SimpleMail")%>
                                    </span>     
                                </div>
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
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var DeptID = "<%=DeptID%>";
        var TmpDate = "<%=tmpDate%>";
        var CalDate = "<%=tmpDate%>";
        var CalMonth = "";
        var BtnClickCheck = ""; 
        var CalCheck = "";
        var PreLang = "<%=PreLang %>";
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>"; 
        var R_InputSubject = "<%=GetResource("InputSubject") %>"; 
        var R_InputDate = "<%=GetResource("InputDate") %>"; 
        var R_InputCount = "<%=GetResource("InputCount") %>"; 
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>"; 
        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>"; 
        var R_Menu = "<%=GetResource("Menu")%>";      
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";   
        var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")"; 
        var R_AlarmAlert = "<%=GetResource("AlarmAlert") %>";   
        var R_CheckMail = "<%=GetResource("CheckMail") %>";   
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Pims/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Pims/Add.js");
    </script>
</body>
</html>
