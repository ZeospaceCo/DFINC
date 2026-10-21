<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Write.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Board.Write" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <iframe name="hid_func" style="display: none"></iframe>
    <form id="form1" runat="server" name="form1" action="/DeskPlusMobile/Common/Upload/UploadCommon.ashx"
    method="POST" enctype="multipart/form-data" target="hid_func">
    <input type="text" name="companyID" value="<%=CompanyID%>" style="display: none" />
    <input type="text" name="userID" value="<%=UserID%>" style="display: none" />
    <input type="text" name="swfUploadGUID" value="<%=SwfUploadGUID%>" style="display: none" />
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="BoardView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-align-left  dropbtn "></i><%=GetResource("Write")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                </ul>
                <!--  달력 -->
                <ul class="mobilecontent di2" id="boa_calendar" style="display: none;">
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
                <!--  // 달력 -->
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
                                <button type="button" class="btn btn-lg btn-primary" id="btAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-success" id="btPCAddFile">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <input type="file" name="upload" id="upload" onchange="fileChange(this);" style="display:none;" />
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="divFile" style="display: none">
                        <div class="title">
                            <div id="fileList" class="to6 dva">
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Setting")%>
                                </div>
                                <div class="area1">
                                    <span>
                                        <asp:DropDownList ID="ddlCatHeader" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </span>
                                </div>
                                <div class="area" style="display:block;">
                                    <asp:DropDownList ID="ddlFontColor" runat="server" style="display:inline-block; width:auto;" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlFontSize" runat="server" style="display:inline-block; width:auto;" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="area">
                                    <input type="checkbox" id="cbxBold" class="rd" runat="server" /><%=GetResource("Bold") %>
                                    <input type="checkbox" id="cbOptionModify" class="rd" runat="server" /><%=GetResource("ModifyEveryone")%>
                                    <input type="checkbox" id="cbOptionAnonymity" class="rd" runat="server" /><span id="spAnonymity"
                                        runat="server"><%=GetResource("Anonymity")%></span><br />
                                    <input type="checkbox" id="cbOptionTop" class="rd" runat="server" /><%=GetResource("RegisterOptionTop")%>
                                    <span id="topDay" style="display: none">
                                        <asp:DropDownList ID="ddlDay" runat="server" style="display:inline-block; width:auto;" CssClass="form-control">
                                        </asp:DropDownList>
                                    </span>
                                    <input type="checkbox" id="cbPopUp" class="rd" runat="server" /><span id="spPopUp"
                                        runat="server"><%=GetResource("PopUpSetting")%></span>
                                    <br />
                                    <input type="checkbox" id="cbCheckDownLoad" class="rd" runat="server" /><%=GetResource("CheckDownLoad")%>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="date_start" style="display: none">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Date")%></div>
                                <div class="area">
                                    <input type="text" id="tbStartDate" class="form-control" style="display:inline-block; width:50%;" runat="server" readonly />~
                                </div>
                                <div class="area">
                                    <input type="text" id="tbEndDate" class="form-control" style="width:50%;" runat="server" readonly />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Subject")%></div>
                                <div class="area">
                                    <input type="text" id="tbTitle" class="form-control" runat="server" maxlength="100" />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Content")%></div>
                                <div class="area1 full-width">
                                    <textarea id="tbcontent" class="form-control" runat="server"></textarea>
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
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.form.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var PageNo = "<%=pageNo%>";
        var MainCID = "<%=mainCID %>";
        var SearchValue = "<%=searchValue%>";
        var PCatID = "<%=pCatID %>";
        var CalMonth = "";
        var CalDate = "";
        var CalCheck = "";
        var PreLang = "<%=PreLang %>";
        var NowYear = "<%=System.DateTime.Now.Year.ToString()%>";
        var NowMonth = "<%=System.DateTime.Now.Month.ToString()%>";
        var NowDay = "<%=System.DateTime.Now.Day.ToString()%>";
        var OptionAnonymity = "<%=OptionAnonymity%>";
        var OptionAlarm  = "<%=OptionAlarm%>";
        var CategoryName = "<%=CategoryName%>";
        var BoaComID = "<%=BoaComID%>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; 
        var MobileType = "<%=MobileType %>";
        var DraftListID =  "<%=draftListID %>";
        var FileCheck = "<%=FileCheck %>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var OptionTop = "<%=OptionTop %>";
        var PopUp = "<%=PopUp %>";    
        var BlockExt = "<%=blockExt %>"; 
        var UpFileSize = "<%=upFileSize %>"; 
           
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_InputCont  = "<%= GetResource("InputCont") %>";
        var R_InputSubject = "<%= GetResource("InputSubject") %>";
        var R_InputDate = "<%=GetResource("InputDate") %>";   
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";     
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_Sun = "<%=GetResource("Sun") %>";     
        var R_Mon = "<%=GetResource("Mon") %>";     
        var R_Tue = "<%=GetResource("Tue") %>";     
        var R_Wed = "<%=GetResource("Wed") %>";     
        var R_Thu = "<%=GetResource("Thu") %>";     
        var R_Fri = "<%=GetResource("Fri") %>";     
        var R_Sat = "<%=GetResource("Sat") %>";   
        var R_MaxFile = "<%=GetResource("MaxFile") %>";   
        var R_CheckFileType = "<%=GetResource("CheckFileType") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Board/Write.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Board/Write.js");
    </script>
</body>
</html>
