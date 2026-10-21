<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleMain.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.ScheduleMain" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<!DOCTYPE html>
<html>

<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
   	<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />   
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Core_Popup.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Core_Popup.css" />       
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/iframeResizer.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>        
    <!--script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.iframe-auto-height.js"></script-->
    <script type="text/javascript">
        var CompanyID = '<%=CompanyID %>';
        var UserID = '<%=UserID %>';
        var myperson = '<%= GetResource("myviewschedule") %>';
        var AllSchedule = '<%= GetResource("pim4") %>';                
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var InputSearchDay = "<%=GetResource("InputSearchDay") %>";                
        var MoveDatePage = '<%=pageinfo%>';
        var PreLang = "<%=PreLang%>"; 
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var PageScrollTop = 0;

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";

        function FrameHeightControl(arg) {
            
            if (arg == "M")
            {
                //document.getElementById("frmContent1").scrolling = "no";
            }
            if (arg == "W") {
                document.getElementById("frmContent1").scrolling = "auto";
                //document.getElementById("frmContent1").height = "1500px";
                //alert(document.getElementById("frmContent1").height);
                //$("#frmContent1").height(1500);
                //alert($("#frmContent1").height());
            }                
        }

        //체크박스 상태
        var ScheduleCheck = '<%=Opentype%>';

        function IframeCall() {            
            document.getElementById("frmContent1").contentWindow.PrintAction();
        }

        function ScheduleParentMove(arg, type) {
            
            if (type == "G" || type == "M" || type == "K" || type == "N" || type == "A") {
                location.href = "/Deskpluseip/Pim/Admin/Holiday/View.aspx?p=" + arg;
            }
            else {
                location.href = "/DeskPlusEIP/Pim/Schedule/View.aspx?p=" + arg;
            }
        }

        function TodoParentMove(arg) {

            location.href = arg;
        }

        function ParentCallInterFace() {
            
            if (parent.frmLeft.MenuReload != null)
                parent.frmLeft.MenuReload();
            
        }


    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/ScheduleMain.js" ></script>
    <script type="text/javascript">
         //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
         //JsCall("/DeskPlusEIP/Pim/Js/Schedule/ScheduleMain.js");             
    </script>
</head>
<body style="overflow:hidden;margin:0;padding:0;">
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header" style="min-width:650px;">
            <h1><%= GetResource("Schedule")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnAdmin" runat="server"    BeginEndTag="li" ></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnAnniv" runat="server"  BeginEndTag="li" ></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnShareObj" runat="server" BeginEndTag="li" ></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnAllShare" runat="server" BeginEndTag="li"  Visible="false"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnUserSche" runat="server"  BeginEndTag="li"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnLinkPim" runat="server"  BeginEndTag="li"  Visible="false"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnPrint" runat="server"    BeginEndTag="li" Visible="false"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="CbtnDefault" runat="server"  BeginEndTag="li" ></cc3:ClientSmartButton>

                <cc3:ClientSmartButton ID="cbtnToDoAdd" runat="server"  BeginEndTag="li" Class="btn btn-secondary"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server"  BeginEndTag="li" Class="btn btn-secondary"></cc3:ClientSmartButton>
            </ul>
        </div>

        <!-- 검색 //-->
        <div id="layout_srh" class="layout_srh clearfix">
            <div class="left">
                <ul class="tab">
                    <li><a href="#" id="aDay" class="tab_on" onclick="ScheduleMain.TabPageMove('day')" ><%= GetResource("DayView")%></a></li>
                    <li><a href="#" id="aWeek" onclick="ScheduleMain.TabPageMove('week')" ><%= GetResource("WeekView") %></a></li>
                    <li class="last">
                    <a href="#" id="aMonth" onclick="ScheduleMain.TabPageMove('month')" ><%= GetResource("MonthView") %></a></li>
            
                </ul>
            </div>

            <div class="right">
                <ul class="btn_searchbox">             
                  <li>
                    <asp:DropDownList ID="ddlSearchKind" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    <span id="spanDate" style="display:none;"><input type="text" id="startDate" size="12" class="clsDate searchtext form-control" readonly="readonly" />-<input type="text" id="endDate" size="12" class="clsDate searchtext form-control" readonly="readonly" /></span>
                  </li>
                  <li>
                    <asp:DropDownList ID="ddlPerson" runat="server" CssClass="searchselect form-control" Visible="false"></asp:DropDownList>
                  </li>
                  <li>
                    <asp:DropDownList ID="ddlValue" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                  </li>
                  <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <input name="ctbSearch" type="text" id="ctbSearch" class="searchtext form-control" placeholder="검색어 입력" Required="False" style="width:200px;" /></li>
                  <li>                
                  <a href="#" class="searchbtn btn-secondary" onclick="ScheduleMain.SearchList()"  title=""><%= GetResource("SEARCHTEXT") %></a></li>
                
                </ul>
            </div>
        </div>

        <div id="layout_content">
            <div class="row">
            	<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <iframe id="frmContent1" name="frmContent1" class="pane ui-layout-center" width="100%" frameborder="0" scrolling="no"></iframe>    
                </div>
            </div>            
        </div>        

     </div>
</div>   
</form>


<!-- ############# start of modal ############## -->
    <div id="zeoModalContact" style="position:absolute;z-index:100" >
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>        
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
<!-- ############# end of modal ############## -->
    <script type="text/javascript">
        $(function () {
            $("iframe").iFrameResize({
                log: false,                  // Enable console logging
                enablePublicMethods: true,                  // Enable methods within iframe hosted page
                resizedCallback: function (messageData) { // Callback fn when resize is received
                },
                messageCallback: function (messageData) { // Callback fn when message is received
                },
                closedCallback: function (id) { // Callback fn when iFrame is closed
                }
            });
        })
    </script>
</body>
</html>
