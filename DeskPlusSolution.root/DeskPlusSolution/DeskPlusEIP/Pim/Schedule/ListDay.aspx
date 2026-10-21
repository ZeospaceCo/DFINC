<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDay.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.ListDay" EnableViewState="false" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />        
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PimFullCalendar.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PimFullCalendar.css"  />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PimFullCalendar.print.css" media='print' /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PimFullCalendar.print.css" media='print' />    
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
</head>
<body>
<form id="form1" runat="server">

<div id="ifm_layout_body">
    <div class="row">
          <div class="pim_date">
            <div class="left">
                <div class="viewSrh">             
                    <p class="pim_i"><input type="checkbox" id="chksh1" runat="server" onclick="ListDay.CheckSchedule(-1)" /><span class="blind"><%= GetResource("MYSCHEDULE") %></span></p>
                    <p class="pim_p"><input type="checkbox" id="chksh2" runat="server" onclick="ListDay.CheckSchedule(0)" /><span class="blind badge badge-info"><%= GetResource("schedule1")%></span></p>
                    <p class="pim_d"><input type="checkbox" id="chksh3" runat="server" onclick="ListDay.CheckSchedule(3)" /><span class="blind badge badge-success"><%= GetResource("pim2")%></span></p>
                    <p class="pim_s"><input type="checkbox" id="chksh4" runat="server" onclick="ListDay.CheckSchedule(1)" /><span class="blind badge badge-secondary badge-shar"><%= GetResource("pim3")%></span></p>
                    <p class="pim_t"><input type="checkbox" id="chksh5" runat="server" onclick="ListDay.CheckSchedule(2)" /><span class="blind badge badge-primary"><%= GetResource("pim4")%></span></p>
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

        <!-- 할일 시작 -->  
        <div id="divhalf1" class="col-12 col-lg-6 pt-0 content-area-body-left"><!-- width 100%(col-12) -->     
            <table class="pim_table_day">
            <caption><%=GetResource("TODAYTODO")%></caption>
            <colgroup>
                <col width="60px" />
                <col width="30px" />
                <col />
                <col width="60px" />
            </colgroup>
            <thead>
                <tr>
                    <th><%=GetResource("IMPORTANCE_MEASURE")%></th>
                    <th><%=GetResource("State")%></th>
                    <th><%=GetResource("TODAYTODO")%></th>                    
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><asp:DropDownList ID="ddltodo" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td><input type="text" id="txttodo" maxlength="100" class="pim_inputtxt" runat="server" /></td>
                    <td>
                        <ul>
                            <li><cc3:ClientSmartButton ID="cbtntodoadd" runat="server" class="btn btn-secondary" ></cc3:ClientSmartButton></li>
                            <li><cc3:ClientSmartButton ID="cbtntodoDetail" runat="server" class="btn btn-secondary" Visible="false"></cc3:ClientSmartButton></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding:0px;border:0px solid red;"><div id="placeholder"></div></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <ul class="pim_status">
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif"/><%= GetResource("priority1")%></li>
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_Going.gif"/><%= GetResource("priority2")%> </li>
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_Done.gif"/><%= GetResource("priority3")%> </li>
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_Cancel.gif"/><%= GetResource("priority4")%> </li>
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_Pass.gif"/><%= GetResource("priority5")%> </li>
                            <li><img alt="" src="<%=BaseImgPath%>/Pims/Status_Postpone.gif"/><%= GetResource("priority6")%> </li>
                        </ul>
                    </td>
                </tr>
            </tbody>
            </table>

            <div style="display:none">
            <div class="layout_subbox">
                <h2><%=GetResource("MyTalk")%></h2>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                </ul>
            </div>
            <div id="placeholder_shortReplyList" style="clear: both;"></div>
            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                    <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                    <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            </div>
        </div>        
        <!-- 할일 끝 -->  
        <!-- 일정 시작 -->  
        <div id="divhalf2" class="col-12 col-lg-6 content-area-body-right" style="min-height:100%;"><!-- width 100%(col-12) -->        
            <table class="pim_table_day" border="0">
            <caption><%= GetResource("TODAYSCHEDULE")%></caption>
            <colgroup>
                <col width="80px" />
                <col />
                <col width="60px" />
            </colgroup>
            <thead>
            <tr>
                <th><%= GetResource("TIMEINFO")%></th>
                <th><%= GetResource("TODAYSCHEDULE")%></th>
                <th style="text-align:right;padding:0px 10px;">
                    <div>
                    <img src="<%=BaseImgPath%>/Icon/ico_item_detail2.gif" id="imghalf" alt='<%= GetResource("Half")%>' title='<%= GetResource("Half")%>' style="cursor:pointer" />
                    <img src="<%=BaseImgPath%>/Icon/ico_item_detail.gif" id="imgall" alt='<%= GetResource("ALL")%>' title='<%= GetResource("ALL")%>' style="cursor:pointer" />            
                    </div>
                </th>
            </tr>
            </thead>
            <tbody>
            <tr style="border-bottom:0px solid #d2d2d2;">
                <td><asp:DropDownList ID="ddltime" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                <td><input type="text" class="pim_inputtxt" id="txtPimcontent" maxlength="100" runat="server"  /></td>
                <td>
                    <span><cc3:ClientSmartButton ID="cbtnQuickadd" runat="server" class="btn btn-secondary" ></cc3:ClientSmartButton></span>
                    <span style="display:none"><cc3:ClientSmartButton ID="cbtnDetail" runat="server" class="btn btn-secondary"  Visible="false"></cc3:ClientSmartButton></span>
                </td>
            </tr>
            </tbody>
            </table>    
            <div class="gap"></div>
            <div id="toPrint"> 
                <div id='calendar' style="float:left;padding:0px;border:0px solid red;"></div>
            </div>
        </div>
        <!-- 일정 끝 -->  
    </div> 
 
    <div style="display:none;">
        <input type="text" id="txtTodid" />
        <asp:TextBox ID="hdEvent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    </div>
    <!-- ############# divtodo  ############## -->
    <div id="divtodo" style="background:white;width:400px;height:350px;z-index:100;position: absolute;display:none;border:1px solid rgba(0,0,0,.1) !important;" class="layer_popup">
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
    <!-- ############# divtodo End ############## -->
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
    <!-- ############# quickAdd End ############## -->      
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
     
</div>

    <!-- ############# start of contextMenu 상중하############## -->
    <ul id="divpri" class="contextMenu">
        <li><a href="#top"  style="cursor:pointer"><%= GetResource("TOP")%></a></li>
        <li><a href="#middel"  style="cursor:pointer"><%= GetResource("MIDDLE")%></a></li>
        <li><a href="#bottom" style="cursor:pointer"><%= GetResource("BOTTOM")%></a></li>            
    </ul>
    <!-- ############# end of contextMenu ############## -->
     

    <!-- ############# start of todo context ############## -->   
    <ul id="divTodoContext" class="contextMenu" style="width:200px">
        <li><a href="#1" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("priority1")%></a></li>
        <li><a href="#2" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_Going.gif" alt="" /><%= GetResource("priority2")%></a></li>
        <li><a href="#3" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_Done.gif" alt="" /><%= GetResource("priority3")%></a></li>
        <li><a href="#4" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_Cancel.gif" alt="" /><%= GetResource("priority4")%></a></li>
        <li><a href="#5" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_Pass.gif" alt="" /><%= GetResource("priority5")%></a></li>
        <li><a href="#6" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_Postpone.gif" alt="" /><%= GetResource("priority6")%></a></li>
        <li><a href="#7" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("todo1")%></a></li>
        <li><a href="#8" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("todo2")%></a></li>
        <li><a href="#9" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("todo3")%></a></li>
        <li><a href="#10" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("todo4")%></a> </li>
        <li><a href="#11" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("todo5")%></a></li>            
        <li><a href="#12" style="cursor:pointer"><img src="<%=BaseImgPath%>/Pims/Status_NotBegin.gif" alt="" /><%= GetResource("PimEbem")%></a></li>
    </ul>       
    <!-- ############# end of todo context ############## -->

</form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript"> 
        var ImagePath ="<%=BaseImgPath %>";         
        //full calendar 기본 변수
        var scrollfocus=375; 
        var DefaultView='agendaDay';
        var calendarheight = 500;
        var setfirstHour= <%=StartTime %>;        
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수 
        var RUserID = "<%=RUserID %>";
        var Order = "<%=Order %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja          
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
        var DefaultSetting= "<%=DefaultSetting%>";        
        var CheckUserID= "";
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var PreLang = "<%=PreLang%>";        
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
        //TODO
        var t_type ='';
        var t_status='';
        var t_title ='';
        var t_startdate='';
        var t_Enddate='';
        var t_ScheduleYN='N';
        var t_ReservationID='0';   
        var t_Priority='0';  
        var t_Scope="";
        var t_DateInfo="";         
        var todoid =0;        
         
          function  removeevent()
          { 
             $('#calendar').fullCalendar('removeEvents', null);           
          }

          ///인쇄
          function PrintAction()
          {            
            var options={ leaveOpen: true, printMode: 'popup' };
            $('#calendar').printElement(options);
          }   
          
          var Notikind = "<%=Notikind%>";         
          var AlarmTime = "<%=AlarmTime%>";  
                 
          //MENT
          var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
          var ApplyDelete = '<%= GetResource("ApplyDelete") %>';
          var TimeOver = '<%= GetResource("TimeOver") %>';          
          var todocadalog ='<%= GetResource("TODOCADALOG")%>';
          var schedulecadalog ='<%= GetResource("SCHEDULECADALOG")%>';
          var ViewScheduleText ='<%= GetResource("ViewSchedule")%>';
          var AddScheduleText ='<%= GetResource("AddSchedule")%>';
          var ModScheduleText ='<%= GetResource("ModSchedule")%>'; 
          var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
          var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
          var Del1 = '<%=GetResource("Del") %>';
          var AddPimPlz= '<%=GetResource("AddPimPlz") %>';
          var WeekSort= '<%=GetResource("WeekSort") %>';
          var basicShareperson='<%= GetResource("basicShareperson")%>'; 
          var basicsShareDept='<%= GetResource("basicsShareDept")%>'; 
          var OneSharePerson='<%= GetResource("OneSharePerson")%>'; 
          var oneShareDept='<%= GetResource("oneShareDept")%>'; 
          //
          var R_Name = "<%= GetResource("Name")%>";
          var R_Contents = "<%= GetResource("Contents")%>";
          var R_RegDate = "<%= GetResource("RegDate")%>";
          var R_TextEmpty = "<%= GetResource("TextEmpty")%>";
          var R_Previous = "<%= GetResource("Previous")%>";
          var R_Next = "<%= GetResource("Next")%>";
          var R_ApplyDelete = "<%= GetResource("ApplyDelete")%>";
          var R_ReallyApplyDelete = "<%= GetResource("ReallyApplyDelete")%>";
          var R_Del = "<%= GetResource("Del")%>";
          var R_Modify = "<%= GetResource("Edit")%>";
          var R_Processing = "<%= GetResource("ACCOUNT_SUCCESS")%>";
          var R_MsgError = "<%= GetResource("MsgError")%>";
          var F_AM = "<%= GetResource("AM")%>";
          var F_PM = "<%= GetResource("PM")%>";
          var R_MsgRepeat= "<%=GetResource("MsgRepeat")%>";     //반복으로 지정된 일정입니다.\n[모든 일정] 에 반영됩니다.

          var AlarmAlert = '<%=GetResource("AlarmAlert") %>';
          var CheckMail = '<%=GetResource("CheckMail") %>';
          var ACCOUNT_SUCCESS = '<%=GetResource("ACCOUNT_SUCCESS") %>';
          var MsgError = '<%=GetResource("MsgError") %>';
          var SplitMail = '<%=GetResource("SplitMail") %>';
          var SimpleMail = '<%=GetResource("SimpleMail") %>';

          function ToDoSelectCalendar(arg)
          {
            t_DateInfo =arg;
            ListDay.ModTodo();            
          }

          function CallPopUp(url,wid,hei,modal)
          {

            PopupWin(wid, hei, url, "400", "800", modal)
            //alert("");
          }
          
          function onreset()
          {
             alert('');
             //$('#calendar').fullCalendar().calendar.resetScroll();
             //$('#calendar').fullCalendar('resetScroll', '');
          }

          
          function MoveScheduleView(arg)
          {
               ShowView(arg);
          }

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/FullCalendar/fullcalendar.js"></script>      
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/FullCalendar/Qtip.js"></script>          
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/ListDay.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/ListDay.js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/AddSimple.js");        
    </script>
</body>
</html>
