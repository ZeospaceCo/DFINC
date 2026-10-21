<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attendance.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Attendance" %>
<script type="text/javascript">
    var attendance_tabPortletID = "<%=TabPortletID%>";
    var attendance_portletID = "<%=PortletID%>";
    var attendance_categoryID = "<%=CategoryID%>";
    var attendance_dataURL = "<%=DataURL%>";
    var attendance_dataType = "<%=DataType%>";
    var attendance_IsAuth = "<%=IsAuth%>";
    var attendance_IsMini = "<%=IsMinimize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var NowDate = "<%=NowDate%>";
    var NowHour = "<%=NowHour%>";    
    var SelDate = "<%=SelDate%>";
    var bWS = "<%=bWS%>";
    var WSTime = "<%=WSTime%>";
    var bWC = "<%=bWC%>";
    var WCTime = "<%=WCTime%>";

    var Attendance_OffWork = "<%=GetResource("Attendance_OffWork") %>";
    var Attendance_Early = "<%=GetResource("Attendance_Early") %>";
    var Attendance_Reason = "<%=GetResource("Attendance_Reason") %>";
    var Attendance_NotGotoWork = "<%=GetResource("Attendance_NotGotoWork") %>";
    var Attendance_OffWorkConfirm = "<%=GetResource("Attendance_OffWorkConfirm") %>";
    var Attendance_OffWorkRe = "<%=GetResource("Attendance_OffWorkRe") %>";    
                            
    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var portletDom = attendance_tabPortletID + "_" + attendance_portletID;

         if (attendance_IsAuth != "True") {
            $("#basicMenu_" + portletDom + " .del").remove();

        }

        //최소화 여부
        if (attendance_IsMini == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");   
        }

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            $("#ptListConfig_" + portletDom).slideToggle(600);      
        });

        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });
       

        $("#ptPreview_" + portletDom).on("click", function (e) {     
            e.preventDefault();
            Main.Popup();
        });

        //현재시간
        $('#clock_NowDateTime').jClocksGMT({ offset: '+9', hour24: false });


        $('#userSearch').click(function (e) {
            e.preventDefault();
            GoTree();
        });

        $('#GotoWork').click(function (e) {
            e.preventDefault();
            GotoWork();
        });

        $('#OffWork').click(function (e) {
            e.preventDefault();
            OffWork();
        });

        AttendanceInit();

    });

   function Attendance_ContextMenuWork(id, action, el, pos) {
    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                if (PortletAuth == "True") {
                    $("#portlet_" + id).fadeOut("slow");
                }
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(attendance_tabPortletID, attendance_portletID, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(attendance_tabPortletID, attendance_portletID, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {
                alert(R_NoSettingInfo);
                break;
            }
        }
    }

    function MOver_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").show();
        //관리자가 아니면 - 관리자 경우 활성화
        if (Banner_IsAuth != "True") {
            $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
        }
    }

    function MOut_<%=TabPortletID%>_<%=PortletID%>()
    {
        $("#div_<%=TabPortletID%>_<%=PortletID%>").hide();
    }

    function AttendanceInit()
    {      
        if(NowHour < 18)
        {
            document.getElementById("OffWork").innerText = Attendance_Early;
        }

        if(bWS == "1")
        {
            document.getElementById("attendanceGotoWork").innerText = WSTime;
            document.getElementById("GotoWork").className = "btn btn-atprimaryoff";

        }

        if(bWC == "1")
        {
            document.getElementById("attendanceOffWork").innerText = WCTime;
        }        
    }

    function AttendanceLoad()
    {   
        var url = "/DeskPlusEIP/Portlet/Ws/Package/Attendance.asmx/GetAttendanceData";
        ParamArray = { 'selDate':SelDate }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Attendance_SuccessList");

        AjaxService(url, data, obj);
    }

    function Attendance_SuccessList(data, status)
    {       
        var rtn = data.d;
        
        var array_rtn = rtn.split("|"); 

        bWS = array_rtn[0]; 
        bWC = array_rtn[2];

        if(array_rtn[0] == "1")
        {
            document.getElementById("attendanceGotoWork").innerText = array_rtn[1];
            document.getElementById("GotoWork").className = "btn btn-atprimaryoff";

        }

        if(array_rtn[2] == "1")
        {
            document.getElementById("attendanceOffWork").innerText = array_rtn[3];
        }        
    }

    function GotoWork()
    {  
        var reason = document.getElementById("txtReason").value;
        var hr = document.getElementById("hr").innerText;
        var minute = document.getElementById("minute").innerText;
        var period = document.getElementById("period").innerText;

        if(bWS == "1") return;

        if((parseInt(hr) > 8 && parseInt(minute) > 0 && period == "AM") || period == "PM" )
        {
            if(reason == "")
            {
                alert(Attendance_Reason);
                return;
            }
            else
            {
                var url = "/DeskPlusEIP/Portlet/Ws/Package/Attendance.asmx/SetGotoWork";
                ParamArray = { 'reason':reason }

                CallEncrypt(CallGetParam());
                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("GotoWork_SuccessList");

                AjaxService(url, data, obj);
            }
        }
        else
        {
            var url = "/DeskPlusEIP/Portlet/Ws/Package/Attendance.asmx/SetGotoWork";
            ParamArray = { 'reason':"" }

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("GotoWork_SuccessList");

            AjaxService(url, data, obj);
        }
    }

    function GotoWork_SuccessList(data, status)
    {       
        document.getElementById("txtReason").value = "";
        AttendanceLoad();
    }

    function OffWork()
    {              
        if(bWS != "1") 
        {
            alert(Attendance_NotGotoWork);
            return;
        }
        else
        {
            var reason = document.getElementById("txtReason").value;
            var hr = document.getElementById("hr").innerText;
            var period = document.getElementById("period").innerText;

            if(bWC != "1") 
            {
                if(!confirm(Attendance_OffWorkConfirm))
                {
                    return;
                }
            }
            else
            {
                if(!confirm(Attendance_OffWorkRe))
                {
                    return;
                }
            }

            if(period == "AM" || (period == "PM" && parseInt(hr) < 6) || (period == "PM" && parseInt(hr) == 12))
            {
                if(reason == "")
                {                     
                    alert(Attendance_Reason);
                    return;
                }
                else
                {
                    var url = "/DeskPlusEIP/Portlet/Ws/Package/Attendance.asmx/SetOffWork";
                    ParamArray = { 'reason':reason }

                    CallEncrypt(CallGetParam());
                    var data = "{p:'" + GetEncryptValue + "'}";
                    var obj = eval("OffWork_SuccessList");

                    AjaxService(url, data, obj);
                }
            }
            else
            {
                var url = "/DeskPlusEIP/Portlet/Ws/Package/Attendance.asmx/SetOffWork";
                ParamArray = { 'reason':"" }

                CallEncrypt(CallGetParam());
                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("OffWork_SuccessList");

                AjaxService(url, data, obj);
            }

        }
    }

    function OffWork_SuccessList(data, status)
    {       
        document.getElementById("txtReason").value = "";
        AttendanceLoad();      
    }
</script>
<link rel="stylesheet" href="/DeskPlusEIP/Portlet/Package/Clock/css/jClocksGMT.css">
<script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jClocksGMT.js"></script>
<script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jquery.rotate.js"></script>
<style>
    .btn-c {
      display: inline-block;
      font-weight: 400;
      text-align: center;
      white-space: nowrap;
      vertical-align: middle;
      -webkit-user-select: none;
      -moz-user-select: none;
      -ms-user-select: none;
      user-select: none;
      border: 1px solid transparent;
      padding: 0.2rem 0.5rem;
      font-size: 0.75rem;
      line-height: 1.5;
      border-radius: 0.25rem;
      transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
      width: 80px;
    }

    .btn-atprimary {
        color: #fff;
        background-color: #007bff;
        border-color: #007bff;
        width: 80px;
    }

    .btn-atprimaryoff {
        color: #000;
        background-color: #f3f3f3;
        border-color: #c2c2c2;
        width: 80px;
    }
    
    .btn-atdanger {
        color: #fff;
        background-color: #dc3545;
        border-color: #dc3545;
        width: 80px;
    }      
    
   
</style>
<!-- 포틀릿(배너) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice"> 

        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>" class="text-primary"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>
    
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">
                <div class="nav-tabs">     
                </div>
                <div class="content-tabs">
                    <!-- 내용 //-->

                    <table cellpadding="0" cellspacing="0" border="0" style="width:99%;">
                    <tbody>
                    <tr>
                        <td style="text-align:center;">
                            <div id='clock_NowDateTime' >
                                <div>                            
                                </div>
                                <div class='digital' style='padding-top:5px;'>
                                <%=NowDate%>
                                <br />
	                            <span class='hr' id="hr"></span><span class='minute' id="minute"></span> <span class='period' id="period"></span>
                                </div>
                            </div>
                        </td>     
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" style="width:99%;">
                                <tr>
                                    <td style="text-align:center;">
                                        <button type="button" class="btn btn-atprimary" style="cursor:pointer" id="GotoWork"><%=GetResource("Attendance_GotoWork")%></button>
                                    </td>
                                </tr>
                                <tr style="height:5px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;">
                                        <button type="button" class="btn btn-atdanger" style="cursor:pointer" id="OffWork"><%=GetResource("Attendance_OffWork")%></button>
                                    </td>
                                </tr>
                            </table>
                        </td>           
                    </tr>
                    <tr style="height:10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <textarea id="txtReason" style="width:95%; text-align:left" rows="3" cols="3"></textarea>
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td>
                        </td>
                    </tr>
                    <tr style="height:10px;">
                        <td style="font-weight:bold;" colspan="2"><span style="color:#9c9c9c;">▣<%=GetResource("Attendance_GotoWork")%>:</span><span id="attendanceGotoWork" style="padding-left:5px;"></span></td>                               
                    </tr>
                    <tr style="height:10px;">
                        <td style="font-weight:bold;" colspan="2"><span style="color:#9c9c9c;">▣<%=GetResource("Attendance_OffWork")%>:</span><span id="attendanceOffWork" style="padding-left:5px;"></span></td>
                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    
        <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>

<!-- 포틀릿(배너) End //-->