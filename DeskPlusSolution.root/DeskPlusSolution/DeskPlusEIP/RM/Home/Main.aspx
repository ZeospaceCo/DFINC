<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Home.Main" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
   <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />   
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Res.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Res.css" /> 
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        var MainPageNo = "<%=MainPageNo%>";
        var DeptPageNo = "<%=DeptPageNo%>";
        var CalendarDate = "<%=CalendarDate %>";        
        var ListGubun = "<%=ListGubun %>";
        var CntYN = "<%=CntYN%>";
        var RmCnt = "<%=RmCnt%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";   
        var R_ReserveDate = "<%=GetResource("ReserveDate") %>";  
        var R_Subject = "<%=GetResource("Subject") %>";         
        var R_UsableStr = "<%=GetResource("UsableStr") %>";           
        var R_ModifyDate = "<%=GetResource("ModifyDate") %>";
        var R_Subscriber = "<%=GetResource("Subscriber") %>";
        var R_FirstList   = "<%=GetResource("FirstList")%>";
        var R_LastList   = "<%=GetResource("LastList")%>";
        var R_UsableResource = "<%=GetResource("UsableResource")%>";
        var R_ConfirmAdd = "<%=GetResource("ConfirmAdd")%>";
        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>";
        var R_HomeAttach  = "<%=GetResource("HomeAttach") %>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_HomeNoReserve = "<%=GetResource("HomeNoReserve") %>";
        var AttachFileInfoList = null;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja          
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Home/Main.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/RM/Js/Home/Main.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body" class="write" style="padding-right:30px">
 <div class="layout_full">   
	<div id="layout_content"><!-- 컨텐츠 하단영역 -->
		<div class="row">
			<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                
    
                <div class="src_body">

                   <div class="src_C">

                       <table class="r_tb" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                               <div class="RH_t_a">
                                  <div class="l_tit"><span><%=GetResource("BeforeReserve") %></span></div>
                                    <div class="fixed_img_col">
                                        <ul>
                                        <%=WeContent%>
                                        </ul>
                                     </div>
                               </div>
                            </td>
                            <td class="r_tb_td" align="right">
                                  <div class="reSeD">
                                      <ul class="cal_inbox">
                                        <li class="cal_gray_inm">
                                          <COMPONENTART:CALENDAR 
                                                id="CalendarSingle" 
                                                MonthSpacing="2" 
                                                DayNameFormat="Short" 
                                                CalendarTitleType="VisibleDateText"
                                                CalendarTitleDateFormat="yyyy MMMM"
                                                NextImageUrl="btn_right.gif" 
                                                PrevImageUrl="btn_left.gif" 
                                                TodayDayCssClass="today"					    
                                                MonthCssClass="month"
                                                NextPrevCssClass="nextprev" 
                                                CalendarCssClass="calendar" 
                                                SelectMonthCssClass="monthselector" 
                                                SelectWeekCssClass="weekselector"
                                                SelectedDayCssClass="selectedday" 
                                                WeekendDayCssClass="weekend" 
                                                OtherMonthDayCssClass="othermonthday" 
                                                DayHoverCssClass="dayhover"
                                                DayCssClass="day" 
                                                DayHeaderCssClass="dayheader" 
                                                CalendarTitleCssClass="title" 
                                                AllowDaySelection="true" 
                                                MonthColumns="1"
                                                MonthRows="1" 
                                                ControlType="Calendar" 
                            
                                                ClientSideOnSelectionChanged="Main.OnCalendarChange" 

                                                runat="server" Font-Bold="True"  width="190px" >
                                            </COMPONENTART:CALENDAR>
                                        </li>
                                      </ul>
                                  </div>

                                  <div class="reSeE">
                                    <div><p style="text-align:center;"><span id="homedate"></span></p></div>
                                    <asp:DropDownList ID="ddlList" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                    <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="seltime form-control input-sm" ></asp:DropDownList>&nbsp;~
                                    <asp:DropDownList ID="ddlendTime" runat="server" CssClass="seltime form-control input-sm" ></asp:DropDownList>
                                    <input id="btnAdd"  type="submit" class="form-control btn btn-primary mt-2" value="<%=GetResource("ConfirmReserve")%>">
                                  </div>
                            </td>                       
                        </tr>
                        </table>

                   </div>

                   <div class="src_C">
       
                      <!-- list -->
                      <div class="c_t_Fa">
                          <div class="l_list">
                            <ul class="tab_menu clearfix">
                               <li class="sel_regb"><a href="javascript:Main.GoTab('LISTALL');"><span id="sp_LISTALL" class="sel_list"><%=GetResource("HomeAll")%> <%=GetResource("ReserveList")%></span><span class=""> </span></a></li>
                               <li class="sel_regb"><a href="javascript:Main.GoTab('LISTMY');"><span id="sp_LISTMY" class="sel_list"><%=GetResource("HomeMy")%></span><span class="badge mygb"> </span></a></li>
                               <li class="sel_regb"><a href="javascript:Main.GoTab('LISTSHARE');"><span id="sp_LISTSHARE" class="sel_list"><%=GetResource("HomeShare")%></span><span class="badge srgb"> </span></a></li>
                               <li class="sel_regb"><a href="javascript:Main.GoTab('LISTSECURITY');"><span id="sp_LISTSECURITY" class="sel_list"><%=GetResource("HomeSecurity")%></span><span class="badge scgb"> </span></a></li>
                            </ul>
                            <span class="float-right">
                               <a id="myL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                               <a id="myR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                            </span>                                           
                          </div>
                          <!-- 테이블 시작 //-->
                          <div id="phList" style="clear: both;"></div>
                          <!-- 테이블 끝 //-->
                      </div>
                      <!-- //List -->
                   </div>

                   <div class="src_C">
                      <!-- list -->
                      <div class="c_t_Fa">
                      <div class="l_list">
                           <span class="s_tit"><%=GetResource("DeptReserve")%></span>
                           <span class="float-right">
                               <a id="deptL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                               <a id="deptR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                           </span>      
                      </div>
                      <!-- 테이블 시작 //-->
                      <div id="phDeptList" style="clear: both;"></div>
                      <!-- 테이블 끝 //-->
                      </div>
                      <!-- //List -->
                   </div>

                </div>                

              </div><!-- end col content-area-body-right -->
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->    
    
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
</body>
</html>
