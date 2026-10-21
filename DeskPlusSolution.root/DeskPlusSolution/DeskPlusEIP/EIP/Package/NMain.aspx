<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NMain.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Package.NMain" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"  TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="csrf-token" content="<%=csrf_token%>">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%><%= System.DateTime.Now.ToLongTimeString() %></title>


    <!-- New layout wpresso -->
	<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package.css"/>
	<link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_home.css"/>
    <!-- New layout wpresso -->



    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PackageMenu.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PackageMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" href="/DeskPlusEIP/Portlet/Package/Clock/css/jClocksGMT.css">
    


    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/js/Chart/chartJs/Chart.min.js"></script>
        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jClocksGMTMain.js"></script>
    <script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jquery.rotate.js"></script>

    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들  
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var MailHttp = '<%=Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingType.MAIL, "MailServerIP", "") %>';
        var Mail = "<%=CurrentUserInfo.MailID%>@<%=CurrentUserInfo.MailDomain%>";
        var TabID = "<%=TabID %>";
        var TabLayout = "<%=TabLayout %>";
        var portletSuccess = true;
        var IsAdmin = "<%=IsAdmin %>";//관리자 탭여부 확인
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var PortletAuth = "<%=PortletAuth%>";
        var IsPortletMove = false;
        var MainPopupBoardList = "<%=MainPopupBoardList%>";
        var PopupWidth = "<%=PopupWidth%>";
        var PopupHeight = "<%=PopupHeight%>";
        var TodoPopup = "<%=TodoPopup%>";
        var ImageYN = "<%=ImageYN%>";
        var TodoMenu = "<%=TodoMenu %>";
        var IsMgr = "<%=IsMgr%>";
        //var TodoMenu = "APP-S,APP-W,TAS-S,PIM-S";

        var NowDate = "<%=NowDate%>";
        var NowHour = "<%=NowHour%>";    
        var SelDate = "<%=SelDate%>";
        
    var Attendance_OffWork = "";
    var Attendance_Early = "";
    var Attendance_Reason = "";
    var Attendance_NotGotoWork = "";
    var Attendance_OffWorkConfirm = "";
    var Attendance_OffWorkRe = "";    

    var bWS = "";
    var WSTime = "";
    var bWC = "";
    var WCTime = "";

        var R_SaveChangePortletOrder = "<%=GetResource("SaveChangePortletOrder") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmApply = "<%=GetResource("ConfirmApply") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ErrorConnection = "<%=GetResource("ErrorConnection") %>";
        var R_FirstPage = "<%=GetResource("FirstPage") %>";
        var R_LastPage = "<%=GetResource("LastPage") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_SaveTabPortletOrder = "<%=GetResource("SaveTabPortletOrder")%>";
        var R_PortletMove = "<%=GetResource("PortletMove")%>";
        var R_NoAuth  = "<%=GetResource("NoAuth")%>";
        var MsgLimitedItem = "<%=GetResource("TodoAlert01")%>";
		var MsgSelectedItem = "<%=GetResource("CheckPortlet")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Package/NMain.js?v=2"></script>
    <script type="text/javascript">      
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/EIP/Js/Package/NMain.js");
    </script>
    
</head>
<body class="home <%=ImageClass1%>" >
<!--
<form name="frmMail" method="post" >					
   <input type="hidden" id="user_account" name="user_account" value="">					
   <input type="hidden" id="cmd" name="cmd" value="">					
   <input type="hidden" id="mail_sn" name="mail_sn" value="">					
   <input type="hidden" id="confirm_gubun" name="confirm_gubun" value="">					
   <input type="hidden" id="confirm_sn" name="confirm_sn" value="">					
   <input type="hidden" id="request_user_sn" name="request_user_sn" value="">					
   <input type="hidden" id="confirm_user_sn" name="confirm_user_sn" value="">					
</form>					
-->
<form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="forgeryToken" runat="server"/>

        <div style="display:none">
            <iframe src="about:blank" id="ifrmSLO" name="ifrmSLO" width="0" height="0"></iframe>
        </div>
       
        <div class="homeimg" style="background-image:url(<%=BGPortalImage%>);display:none" >
                <span class="slogan"><asp:Literal ID="lbslogan" runat="server"></asp:Literal></span>
        </div>
        <div class="portlet-roll-icon indeximagecontrol ">
            <div class="modenavi">
                <i class="fas fa-cog text-primary bg-white tab"></i>
            </div>
            <div class="modechange">
                <ul>
                <!-- li class="imgtoggle dissel <%=ImageClass2%>"><i class="fas fa-image text-primary bg-white tab"></i><%=GetResource("HiddenImage") %></li -->
                <li class="lebel text-primary">
                    <%=GetResource("MainPortletSave") %><em><%=GetResource("MsgTab") %></em>
                    <% if (PortletAuth == true)  { %>
                    <a id="TabPortletOrder"><span class="btn btn-sm btn-outline-light"><%=GetResource("PortletMove")%></span></a>                
                    <a id="AddTabPortlet"><span class="btn btn-sm btn-outline-light"><%=GetResource("AddPortlet")%></span></a>
                    <% } %>
                    <a id="ConfigTabMain"><span class="btn btn-sm btn-outline-light"><%=GetResource("ConfigTabMain")%></span></a>
                    <a id="ConfigTabUser"><span class="btn btn-sm btn-outline-light"><%=GetResource("ConfigTabUser")%></span></a>
                    <a id="ConfigPortletUser"><span class="btn btn-sm btn-outline-light"><%=GetResource("ConfigPortletUser")%></span></a>
                    <% if (CurrentUserInfo.IsInRole("admin")) { %>
                    <a id="ConfigTabAdmin"><span class="btn btn-sm btn-outline-light"><%=GetResource("ConfigTabAdmin")%></span></a>
                    <% } %>  
                </li>
                <%=TabList %>
                </ul>
            </div>
        </div>

    <!-- ############# start of contextMenu ############## -->
	<div id="MenuList"></div>
    <ul id="basicMenu" class="contextMenu">
        <li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>
        <li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>
        <li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>
        <li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>
    </ul>
    <!-- ############# end of contextMenu ############## -->



    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- ########################### >
    카테고리 badge는 a tag안에 <span class="badge text-white doc_ing">진행</span>
    <!-- ########################### -->
    <div id="container">

		<!-- start first section -->
        <style>
		body.home #container #leftntop{
			background:#c9c9e7;
			_background:url(<%=BaseImgPath%>/Home/v5_top_popupback.jpg); background-size:cover;
			background:url(<%=BaseImgPath%>/Home/20210714-2.png); background-size:cover;
			background-position:bottom center !important;
			
		}
		@media (min-width: 2416px) {
		body.home #container #leftntop{background:none}
		}
		</style>
        <div id="leftntop" >
            <div class="container-fluid ">



                <!-- start popup  -->
           
                <div class="layer_popup top_popup">
                    <div class="top_popup_container col-10">
			                <!-- start todo  -->
                            <div class="todo p-4">
								<div class="todaytime" id='clock_NowDateTime'>                               
                                	<span class="todaytime_title">Today</span>
                                	<span class="todaytime_date"><span><%=NowDate%></span></span>
                                <div class='digital' style='padding-top:5px;'>
	                            <span class='hr' id="hr"></span><span class='minute' id="minute"></span> <span class='period' id="period"></span>
                                </div>
                                </div>
                            </div>        
			                <!-- start todo  -->
        
			                <!-- start board list -->
                            <div class="inside col-12 p-5" style="padding: 0rem 3rem 3rem 3rem!important">
                                    <div class="portlet portlet-notice" style="box-shadow: rgba(0, 0, 0, 0) 0px 0px 0px; margin-bottom: 0px;">
                                    <div class="portlet-title portlet-header" style="border-bottom: 0px solid">
                                        <div class="actions">
                                            <a href="" id="ptConfig" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                                        </div>
                                    </div>
                                    </div>

                                    <!-- 메뉴설정 -->
                                    <div id="settarea" style="display: none; padding-bottom:20px; padding-top:0px; border-radius:21px; position:relative;" class="clearfix"> 
                                        <ul class="settarea clearfix">
                                            <li style="font-weight:bold;text-align:center;height:88px;" class="bg-secondary text-white col-2 p-2"><span><%=GetResource("TodoTitle")%></span></li>
                                            <li style="background:#f3f3f3;padding:10px;line-height:25px;display:flex;flex-flow:row wrap;align-items:center;height:88px;" title="<%=GetResource("TodoPopupMsg")%>" class="col-8 p-2">
                                                <span><input type="checkbox" class="settitem" id="APP-S" value="APP-S" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem01Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="APP-W" value="APP-W" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem02Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="TAS-S" value="TAS-S" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem03Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="TAS-W" value="TAS-W" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem04Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="PIM-T" value="PIM-T" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem05Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="PIM-S" value="PIM-S" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem06Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem" id="RM-C" value="RM-C" style="margin: 4px;" /><span class="gap10" style="padding-right:15px;"><%=GetResource("TodoItem07Sett")%></span></span>
                                                <span><input type="checkbox" class="settitem mgr" id="RM-W" value="RM-W" style="margin: 4px;" /><span class="mgr"><%=GetResource("TodoItem08Sett")%></span></span>                                                
                                            </li>
                                            <li style="text-align:center;height:88px;padding:0.3rem!important" class="col-2 p-2">
                                                <span class="listsetbtn"><a id="settsave" class="btn btn-secondary"><%=GetResource("Save")%></a><a id="settclose" class="btn btn-secondary"><%=GetResource("Closed")%></a></span>    
                                            </li>
                                        </ul>
                                    </div>
                            
                                    <div class="layout_scroll" style="min-height:260px;height:395px;display:block;width:100%; float:none">
                                        <!-- 테이블 시작 -->
                                        <table style="border:0; width:100%;">
                                        <caption></caption>
                                        <colgroup>
                                            <col width="49%">
                                            <col>
                                            <col width="49%">
                                        </colgroup>
                                        <tbody>
                    <tr>
                        <td><div class="pTWa" id="item_0"></div></td>
                        <td>&nbsp;</td>
                        <td><div class="pTWa" id="item_1"></div></td>
                    </tr>
                    <tr>
                        <td><div class="pTWa" id="item_2"></div></td>
                        <td>&nbsp;</td>
                        <td><div class="pTWa" id="item_3"></div></td>
                    </tr>
                                        </tbody>
                                        </table>
                                    </div>
							</div><!-- end board list // inside -->  



					</div><!-- top_popup_container -->  
                </div><!-- end layer_popup top_popup -->  
                <!-- end popup  -->



                <!-- start popup codding https://fonts.google.com/icons -->
                <div class="quickmenu top_quickmenu">
                <a href="/DeskPlusEIP/Task/Default.aspx" class="quick_color1 quickmenuicon"><span class="material-icons-outlined" >&#xe9fe;</span><span class="text"><%=GetResource("Task") %></span></a>
                <a href="/DeskPlusEIP/Approval/Home/Default.aspx" class="quick_color2 quickmenuicon"><span class="material-icons-outlined" >&#xe870;</span><span class="text"><%=GetResource("SearchFieldApproval")%></span></a>
                <a href="http://www.di.co.kr/" class="quick_color3 quickmenuicon" target="_blank"><span class="material-icons-outlined" >&#xe88a;</span><span class="text"><%=GetResource("Homepage")%></span></a>
				<a href="/DeskPlusEIP/Account/Search/Default.aspx" class="quick_color4 quickmenuicon"><span class="material-icons-outlined" >&#xf233;</span><span class="text"><%=GetResource("UserSearch")%></span></a>
                <a href="/DeskPlusEIP/Pim/Default.aspx" class="quick_color5 quickmenuicon"><span class="material-icons-outlined" >&#xe7fd;</span><span class="text"><%=GetResource("Pims")%></span></a>
				<a href="/DeskPlusEIP/RM/Default.aspx" class="quick_color6 quickmenuicon"><span class="material-icons-outlined" >&#xef63;</span><span class="text"><%=GetResource("ResourceReservation")%></span></a>
				<a href="/DeskPlusEIP/Report/Default.aspx" class="quick_color7 quickmenuicon"><span class="material-icons-outlined" >&#xe0c9;</span><span class="text"><%=GetResource("Businessreport")%></span></a>
                <a href="/DeskPlusEIP/Address/Default.aspx" class="quick_color8 quickmenuicon"><span class="material-icons-outlined" >&#xea67;</span><span class="text"><%=GetResource("AddressBook")%></span></a>
                <a href="http://mail.di.co.kr/mail/sso?cmd=login2&webmail_id=<%= CurrentUserInfo.BirthAddress1 %>" target="_blank" class="quick_color9 quickmenuicon"><span class="material-icons-outlined" >&#xe0be;</span><span class="text">E-Mail</span></a>
                <!--<a href="#" class="quick_color9 quickmenuicon" id="hmn_2"  onclick="javascript:top.frmTop.Menu.SettingFaMenu()"><span class="material-icons-outlined">&#xe429;</span><span class="text">설정</span></a-->



                </div><!-- end layer_popup top_popup -->  
                <!-- end popup codding -->



            </div>
        </div><!-- end first section -->

		<!-- start second section -->
        <div id="rightnbottom">
            <div class="container-fluid">
                <div class="row">
                     <asp:PlaceHolder id="placeholderLayout" runat="server"></asp:PlaceHolder>
                </div>
            </div>     
        </div><!-- end second section -->
    </div><!-- end container section -->

    <!-- ########################### -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->
    <!-- New layout wpresso -->




    <script type="text/javascript">
        <%
            foreach (System.Collections.Generic.KeyValuePair<string, string> content in PopupContent)
            {
                if (Request.Cookies[content.Key] == null)
                {
                    Response.Write(content.Value);
                }
            }
        %>
    </script>
</form>



    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- ########################### 기존 아래소스 주석처리함 -->
    <!--div id="modalhome"  class="modal fade" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
		  
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
          </button>

          <div class="modal-body">
			<form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
            <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
          </div>
          <!--div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary">Save changes</button>
          </div>
        </div>
      </div>
    </div-->
    <!-- ########################### -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->
    <!-- New Modal wpresso -->






    
    <!-- Modal -->
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
