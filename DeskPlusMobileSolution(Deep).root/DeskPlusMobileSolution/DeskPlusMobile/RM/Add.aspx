<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.RM.Add" %>

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
    <div data-role="page" id="PimsAdd">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-calendar-minus dropbtn "></i>
                            <%=GetResource("ResourcesAdd")%>
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
                                    <%=GetResource("ReserveDate")%>
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
                                    <input type="text" id="tbSubject" name="" maxlength="100" class="form-control" />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="list_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("SelectResource")%></div>
                                <div class="area">
                                    <asp:DropDownList ID="ddlList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="qty_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("RequestQty")%></div>
                                <div class="area">
                                    <asp:DropDownList ID="ddlQty" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="guard_set">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("SecuritySetting")%></div>
                                <div class="area">
                                    <asp:DropDownList ID="ddlGuard" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Content")%></div>
                                <div class="area1 full-width">
                                    <textarea id="tbContent" class="form-control"></textarea>
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
        var TmpDate = "<%=tmpDate%>";
        var BPage = "<%=bPage%>";
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
        var R_SelectResources = "<%=GetResource("SelectResources") %>";
        var R_QtyZero = "<%=GetResource("QtyZero") %>";
        var R_UsableResource = "<%=GetResource("UsableResource") %>";
        var R_Menu = "<%=GetResource("Menu")%>";      
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";   
        var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")"; 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/RM/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Pims/Add.js");
    </script>
</body>
</html>
