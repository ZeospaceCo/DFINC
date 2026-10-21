<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListWeek.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.ListWeek" EnableViewState="false" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />  
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PimFullCalendar.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PimFullCalendar.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PimFullCalendar.print.css" media='print' /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PimFullCalendar.print.css" media='print' />    
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
   <form id="form1" runat="server">
   <img src="<%=BaseImgPath%>/pims/loading.gif" id="imgload" alt="" class="loadcls" style="display:none" /> 
   <!-- layout_body -->
   <div id="ifm_layout_body">
        <div class="pim_date">
            <div class="left">
                <div class="viewSrh">             
                    <p class="pim_i"><input type="checkbox" id="chksh1" runat="server" onclick="ListWeek.CheckSchedule(-1)" /><span class="blind"><%= GetResource("MYSCHEDULE") %></span></p>
                    <p class="pim_p"><input type="checkbox" id="chksh2" runat="server" onclick="ListWeek.CheckSchedule(0)" /><span class="blind badge badge-info"><%= GetResource("schedule1")%></span></p>
                    <p class="pim_d"><input type="checkbox" id="chksh3" runat="server" onclick="ListWeek.CheckSchedule(3)" /><span class="blind badge badge-success"><%= GetResource("pim2")%></span></p>
                    <p class="pim_s"><input type="checkbox" id="chksh4" runat="server" onclick="ListWeek.CheckSchedule(1)" /><span class="blind badge badge-secondary badge-shar"><%= GetResource("pim3")%></span></p>
                    <p class="pim_t"><input type="checkbox" id="chksh5" runat="server" onclick="ListWeek.CheckSchedule(2)" /><span class="blind badge badge-primary"><%= GetResource("pim4")%></span></p>
                </div>
            </div>
            <div class="mid">
                <i class="fas fa-angle-left"  id="imgLeft" alt="<%= GetResource("Previous") %>"></i>
                <asp:Label Style="cursor: pointer" ID="lblDate" runat="server" Text=""></asp:Label>
                <i class="fas fa-angle-right"  id="imgRight"   alt="<%= GetResource("Previous") %>"></i>
            </div>
            <div class="right">
                <span class="badge text-secondary" id="HelpCategory" style="cursor:pointer"><i class="fas fa-info-circle" style="font-size:20px" title="<%=GetResource("CategoryHelp")%>"></i></span>
            </div>
        </div>
        <div id="toPrint">
            <div id='calendar'></div>
        </div>
   </div>
   <!-- // layout_body -->
   <div style="display:none;">
        <asp:TextBox ID="hdEvent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
   </div>        
   <!-- ############# divtodo ############## -->
    <div id="divtodo" style="background:white;width:400px;height:350px;z-index:100;position:absolute;display:none;border:1px solid rgba(0,0,0,.1) !important;" class="layer_popup">
        <div class="modal-drag"><p></p></div>
        <div class="inside">
            <div class="popup_header">
                <h1 id="divPopTitle"></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClosetodo" runat="server" /></li>
                </ul>
            </div>
            <div class="popup_layout_style3" style="height:250px;">
                <div class="dotLine"></div>
                <ul><div id="divtodolist"></div></ul>
            </div>
        </div>
    </div>
    <!-- ############# divtodo ############## -->
    <!-- ############# quickAdd ############## -->
     <div id="divQuickSch" style="background:white;width:550px;height:300px;z-index:100;position:absolute;display:none;border:1px solid rgba(0,0,0,.1) !important;"  class="layer_popup">
            <div class="modal-drag"><p></p></div>
            <div class="inside">
                <div class="popup_header">
                    <h1 id="spTitle"></h1>
                    <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnScheDetail" runat="server"></cc3:ClientSmartButton></li>
                    <li><div id="divConfirm"><cc3:ClientSmartButton ID="CbtnConfirm"  runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton></div></li>
                    <li><cc3:ClientSmartButton ID="CbtnScheDel" runat="server" Class="btn btn-danger"  /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"  /></li>
                    </ul>
                </div>
                <div class="dotLine"></div>
                <div class="popup_layout_style3">
                    <table class="table_typeB">
			        <caption>
			            <%= GetResource("AddSchedule")%>
			        </caption>
			        <colgroup>
                        <col width="70px" />
                        <col width="220px" />
                        <col width="60px" />
                        <col />
			        </colgroup>
                    <tbody>
                    <tr>
                        <th><%= GetResource("Title")%></th>
                        <td colspan="3">
                            <input type="text" class="pims_text" runat="server" id="tbTitle" style="width:95%" maxlength="100"  />
                            <span id="Reshort"></span>
                            <input type="text" id="chkRepeat" size="0"  readonly="readonly" style="display:none;" value="N" /> 
                        </td>
                    </tr>
                    <tr>
                        <th><%= GetResource("ViewLimit")%></th>
                        <td><asp:DropDownList id="ddlShare"  runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th><%=GetResource("Category")%></th>
                        <td><asp:DropDownList id="ddlStatus"  runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>                        
                    </tr>                   
                    <tr>
                    <th><%= GetResource("Schedule")%></th>
                    <td colspan="3">
                        <div id="startEndDate">
                        <input type="text" id="tbstartDate" size="12"  readonly="readonly"  /> 
                        <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>~
                        <input type="text" id="tbendDate" size="12"  readonly="readonly" /> 
                        <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </div>
                    </td>
                    </tr>
                    <tr id="trAlarm">
                    <th><%= GetResource("PimAlarm")%></th>
                    <td colspan="3">
                       <asp:DropDownList ID="ddlnotikind" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                       <input name="idEmail" type="text" class="text required" value="" id="idEmail" runat="server" style="width:250px !important;" maxlength="200" />                         
                       <asp:DropDownList ID="ddlAlarmTime" runat="server" CssClass="searchselect form-control"><asp:ListItem Text="Time" Value=""></asp:ListItem></asp:DropDownList>
                       <span id="divment"><%=GetResource("BeforeNoti")%></span>
                       <ul class="btn_right"><li><cc3:ClientSmartButton ID="CbtnSetting" runat="server" class="btn btn-sm btn-secondary"></cc3:ClientSmartButton></li></ul>
                       <span id="msgMail"><Br /><%=GetResource("SimpleMail")%></span>
                    </td>
                    </tr>
                    <tr style="height:50px" id="trShare">
                        <th><%=GetResource("shareinfo")%></th>
                        <td colspan="3"><textarea id="tainfo"  readonly="readonly" style="width:95%;height:50px"></textarea></td>
                    </tr>
                    </tbody>
                    </table>
                </div>
            </div>
        <input type="hidden" id="hdChoiceSche" />
        <input type="hidden" id="hdType" />
    </div>
    <!-- ############# End ############## -->
    <!-- ############# quickView ############## -->
    <div id="divQuickSchView" style="background:white;width:550px;height:300px;z-index:100;position:absolute;display:none;border:1px solid rgba(0,0,0,.1) !important;"  class="layer_popup">
        <div class="modal-drag"><p></p></div>
            <div class="inside">
                <div class="popup_header">
                    <h1><%= GetResource("ViewSchedule")%></h1>
                    <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnScheDetailView" runat="server"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnCloseView" runat="server"  /></li>
                    </ul>
                </div>
                <div class="dotLine"></div>
                <div class="popup_layout_style3">
                    <table class="table_typeB">
			        <caption>
			            <%= GetResource("AddSchedule")%>
			        </caption>
			        <colgroup>
                        <col width="70px" />
                        <col width="220px" />
                        <col width="60px" />
                        <col />
			        </colgroup>
                    <tbody>
                    <tr style="height:40px">
                        <th><%= GetResource("Title")%></th>
                        <td colspan="3" id="tTitle" style="vertical-align:middle"></td>
                    </tr>
                    <tr style="height:40px" id="tRShareView">
                        <th><%= GetResource("ViewLimit")%></th>
                        <td id="tShare" style="vertical-align:middle"></td>
                        <th><%=GetResource("Category")%></th>
                        <td id="tStatus" style="vertical-align:middle"></td>                        
                    </tr>
                    <tr style="height:40px">
                        <th><%= GetResource("WRITER")%></th>
                        <td id="tWriter" style="vertical-align:middle"></td>
                        <th><%= GetResource("ShortReply")%></th>
                        <td id="tReshort" style="vertical-align:middle"></td>
                    </tr>
                    <tr style="height:40px">
                    <th><%= GetResource("Schedule")%></th>
                    <td colspan="3" style="vertical-align:middle">
                        <span id="sStartDate" ></span>
                        <span id="sStartTime"></span> ~
                        <span id="sendDate"></span>
                        <span id="sEndTime"></span>
                    </td>
                    </tr>                   
                    </tbody>
                    </table>
                </div>
            </div>
    </div>
    <!-- ############# quickAdd View ############## -->  
    </form>  
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p><p></p><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/iframeResizer.contentWindow.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">         
        //full calendar 기본 변수
        var scrollfocus=5; 
        var DefaultView='agendaWeek';
        var calendarheight = 0;
        var setfirstHour= 7;
        //요일
        var week = <%=Week %>;
        var weeklong = <%=WeekLong %>;
        //달
        var monthname = <%=monthname %>;
        var shortmonthname = <%=ShortMonthname %>;
        var Year = "<%=Year %>";
        var Month = "<%=Month %>";         
        var Day = "<%=Day %>";         
        //현재날짜
        var NowYear = "<%=NowYear %>";
        var NowMonth = "<%=NowMonth %>";         
        var NowDay = "<%=NowDay %>";
        //유저정보
        var UserId = "<%=UserId %>";
        var CompanyID = "<%=CompnayID%>";
        var CurrentUserID = "<%=CurrentUserID%>";  
        var CheckUserID= "";
        var ApplyDelete = '<%= GetResource("ApplyDelete") %>';
        var Type ="";
        var Title ="";
        var Content ="";
        var StartTime ="";
        var EndTime ="";
        var Repeat ="";
        var Alert ="";
        var AlertType ="";
        var AlertTargetAddress ="";
        //var IsOpen ="";
        //var DeptID
        var OpenType ="";
        var ReservationID="";
        var Lunar ="";
        var  DefaultSetting= "<%=DefaultSetting%>";
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';       
         
        var Notikind = "<%=Notikind%>";         
        var AlarmTime = "<%=AlarmTime%>";  
            
        //ment
        var TimeOver =       '<%= GetResource("TimeOver") %>';
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
        var todocadalog ='<%= GetResource("TODOCADALOG")%>';
        var schedulecadalog ='<%= GetResource("SCHEDULECADALOG")%>';
        var holidaycadalog ='<%= GetResource("HOLIDAYCADALOG")%>';
        var ViewScheduleText ='<%= GetResource("ViewSchedule")%>';
        var AddScheduleText ='<%= GetResource("AddSchedule")%>';
        var ModScheduleText ='<%= GetResource("ModSchedule")%>';
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var AddPimPlz= '<%=GetResource("AddPimPlz") %>';
        var  basicShareperson='<%= GetResource("basicShareperson")%>'; 
        var  basicsShareDept='<%= GetResource("basicsShareDept")%>'; 
        var  OneSharePerson='<%= GetResource("OneSharePerson")%>'; 
        var  oneShareDept='<%= GetResource("oneShareDept")%>'; 
        var PreLang = "<%=PreLang%>";
        var F_AM = "<%= GetResource("AM")%>";
        var F_PM = "<%= GetResource("PM")%>";
        var R_MsgRepeat= "<%=GetResource("MsgRepeat")%>";     //반복으로 지정된 일정입니다.\n[모든 일정] 에 반영됩니다.

        var AlarmAlert = '<%=GetResource("AlarmAlert") %>';
        var CheckMail = '<%=GetResource("CheckMail") %>';
        var ACCOUNT_SUCCESS = '<%=GetResource("ACCOUNT_SUCCESS") %>';
        var MsgError = '<%=GetResource("MsgError") %>';
        var SplitMail = '<%=GetResource("SplitMail") %>';
        var SimpleMail = '<%=GetResource("SimpleMail") %>';

          ///인쇄
          function PrintAction()
          {            
            var options={ leaveOpen: true, printMode: 'popup' };
            $('#toPrint').printElement(options);
          }

          function MoveTodoView(arg)
          {
               ListWeek.MoveTodoView(arg);
          }
          function MoveScheduleView(arg)
          {
               ListWeek.ShowView(arg);
          }

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/FullCalendar/fullcalendar.js"></script>          
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/FullCalendar/xdate.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/ListWeek.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/ListWeek.js");        
    </script>    
</body>
</html>
