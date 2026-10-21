<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attendance.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.HR.Attendance" %>
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
    var NowMinute = "<%=NowMinute%>";
    var SelDate = "<%=SelDate%>";
    var bWS = "<%=bWS%>";
    var WSTime = "<%=WSTime%>";
    var bWC = "<%=bWC%>";
    var WCTime = "<%=WCTime%>";    
    var WCTimeHour = "<%=WCTimeHour%>"; // 퇴근시간 Hour
    var WCTimeMinute = "<%=WCTimeMinute%>"; // 퇴근시간 Minute

    var bWSTimeHour = "<%=bWSTimeHour%>"; // 정규 출근시간 Hour
    var bWCTimeHour = "<%=bWCTimeHour%>"; // 정규 퇴근시간 Hour
    var bWSTimeMinute = "<%=bWSTimeMinute%>"; // 정규 출근시간 Minute
    var bWCTimeMinute = "<%=bWCTimeMinute%>"; // 정규 퇴근시간 Minute

    var isHoli = "<%=isHoli%>"; //휴일 여부
    var OverTime = "<%=OverTime%>"; //주간 연장근무시간
    

    var Attendance_OffWork = "<%=GetResource("Attendance_OffWork") %>"; // 퇴근
    var Attendance_Early = "<%=GetResource("Attendance_Early") %>"; // 조퇴
    var Attendance_Reason = "<%=GetResource("Attendance_Reason") %>"; // 사유를 입력 하시기 바랍니다.
    var Attendance_NotGotoWork = "<%=GetResource("Attendance_NotGotoWork") %>"; // 출근 상태가 아닙니다.
    var Attendance_GotoWorkRe = "<%=GetResource("Attendance_GotoWorkRe") %>"; // 출근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
    var Attendance_OffWorkConfirm = "<%=GetResource("Attendance_OffWorkConfirm") %>"; // 퇴근 처리 하시겠습니까?
    var Attendance_OffWorkRe = "<%=GetResource("Attendance_OffWorkRe") %>"; // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
    var Attendance_EarlyConfirm = "<%=GetResource("Attendance_EarlyConfirm") %>"; // 조퇴 처리 하시겠습니까?
    var Attendance_EarlyRe = "<%=GetResource("Attendance_EarlyRe") %>"; // 조퇴 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
    var Attendance_EarlyTimeMsg = "<%=GetResource("Attendance_EarlyTimeMsg") %>"; // 조퇴 예정시간을 선택하세요.
    var Attendance_EarlyCancelConfirm = "<%=GetResource("Attendance_EarlyCancelConfirm") %>"; // 취소 처리 하시겠습니까?

    var R_GotoWork_Success = "<%=GetResource("GotoWork_Success") %>"; // 출근 처리되었습니다.
    var R_OffWorkEarly_Success = "<%=GetResource("OffWorkEarly_Success") %>"; // 조퇴 처리되었습니다.
    var R_OffWork_Success = "<%=GetResource("OffWork_Success") %>"; // 퇴근 처리되었습니다.
    var R_WSTime = "<%=GetResource("WSTime") %>"; // 출근시간
    var R_WCTime = "<%=GetResource("WCTime") %>"; // 퇴근시간
    var R_Early_Time = "<%=GetResource("Early_Time") %>"; // 조퇴시간
    var R_Location = "<%=GetResource("Location") %>"; // 위치

    var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>"; // 처리되었습니다.
                            
    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 받지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {
        var portletDom = attendance_tabPortletID + "_" + attendance_portletID;

        if (attendance_IsAuth != "True") {
           $("#ptConfig_" + portletDom).remove();  
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

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
                        
            ParamArray = {};

            CallEncrypt(CallGetParam());            
            location = "/DeskPlusEIP/HR/Default.aspx?p=" + GetEncryptValue;
        });

        //현재시간
        $('#clock_NowDateTime').jClocksGMT({ offset: '+9', hour24: false });
        
        // 출근
        $('#GotoWork').click(function (e) {
            e.preventDefault();
            //GotoWork();
            GetTime("GotoWork");
        });

        // 퇴근
        $('#HROffWork').click(function (e) {
            e.preventDefault();
            //OffWork();
            GetTime("OffWork");
        });

        // 조퇴취소
        $('#btnEarlyCancel').click(function (e) {
            e.preventDefault();
            EarlyCancel();
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
        var h = NowHour;
        var m = parseInt(NowMinute);
        var mm = m < 10 ? "0" + m : String(m);
        var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
        var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
        var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

        var realWCTime = parseInt(WCTimeHour + WCTimeMinute); //실제 퇴근시간 시분 1800

        // 현재시각이 출근시간 이상이고 정규 퇴근시간 이전일때 조퇴로 표시
        if(tNowTime >= tWSTime && tNowTime < tWCTime) 
        {
            document.getElementById("HROffWork").innerText = Attendance_Early; //조퇴
        }

        // 출근시간이 등록되어 있을 경우
        if(bWS == "1")
        {
            document.getElementById("attendanceGotoWork").innerText = WSTime; //출근시간
            document.getElementById("GotoWork").className = "btn btn-atprimaryoff";
        }

        // 퇴근시간이 등록되어 있을 경우
        if(bWC == "1")
        {
            // 퇴근시간이 정규 퇴근시간 이전이면 조퇴이고 조퇴라인 표시해줌
            if (realWCTime < tWCTime)
            {
                document.getElementById("trEarlyTime2").style.display = "";
                document.getElementById("spEarlyTime").innerText = WCTime;
            }
            else
            {            
                document.getElementById("spOffWork").style.display = "";
                document.getElementById("attendanceOffWork").innerHTML = WCTime + "&nbsp;&nbsp;&nbsp;&nbsp;";
            }
        }

        // 근무중 표시
        if (bWS == "1")
        {
            if (bWC == "0")
            {
                document.getElementById("onduty").style.display = "";                
            }
            if (bWC == "1")
            {
                // 현재시간이 퇴근(조퇴)시간 이전일때만 근무중 표시
                if (tNowTime < realWCTime)
                {
                    document.getElementById("onduty").style.display = "";
                }
            }
        }
        else
        {
            document.getElementById("beforework").style.display = "";
        }

        // 주간 연장근무시간
        document.getElementById("spOverTime").innerText = OverTime;
        
    }

    function AttendanceLoad()
    {   
        var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/GetAttendanceData";
        
        //ParamArray = { 'selDate':SelDate }
        ParamArray = { }
        
        
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Attendance_SuccessList");

        AjaxService(url, data, obj);
    }

    function Attendance_SuccessList(data, status)
    {       
        var rtn = data.d;
        
        var array_rtn = rtn.split("|"); 

        bWS = array_rtn[0]; //출근여부
        bWC = array_rtn[2]; //퇴근여부
        WCTimeHour = array_rtn[4]; //실제 퇴근시간 Hour
        WCTimeMinute = array_rtn[5]; //실제 퇴근시간 Minute
        isHoli = array_rtn[6]; //휴일여부
        bWSTimeHour = array_rtn[7]; //정규 출근시간 Hour
        bWSTimeMinute = array_rtn[8]; //정규 출근시간 Minute
        bWCTimeHour = array_rtn[9]; // 정규 퇴근시간 Hour
        bWCTimeMinute = array_rtn[10]; //정규 퇴근시간 Minute
        
        $('#nowdate').html(array_rtn[11]); //현재날짜로 표시
        OverTime = array_rtn[12]; //주간 연장근무시간

        var _nowHour = array_rtn[13]; //현재 시간
        var _nowMinute = array_rtn[14]; //현재 분
        
        //var h = $('#jDateHour').val();
        //var m = parseInt($('#jDateMinute').val());
        var h = _nowHour;
        var m = parseInt(_nowMinute);
        var mm = m < 10 ? "0" + m : String(m);
        var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
        var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
        var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

        var realWCTime = parseInt(WCTimeHour + WCTimeMinute); //실제 퇴근시간 시분 1800

        // 현재시각이 출근시간 이상이고 정규 퇴근시간 이전일때 조퇴로 표시
        if(tNowTime >= tWSTime && tNowTime < tWCTime) 
        {
            document.getElementById("HROffWork").innerText = Attendance_Early; //조퇴
        }
        else 
        {
            document.getElementById("HROffWork").innerText = Attendance_OffWork; //퇴근
        }
        
        // 출근시간이 등록되어 있을 경우
        if(array_rtn[0] == "1")
        {
            document.getElementById("attendanceGotoWork").innerText = array_rtn[1];
            document.getElementById("GotoWork").className = "btn btn-atprimaryoff";
        }
        else
        {
            document.getElementById("attendanceGotoWork").innerText = "";
            document.getElementById("GotoWork").className = "btn btn-atprimary";
        }

        // 퇴근시간이 등록되어 있을 경우
        if(array_rtn[2] == "1")
        {
            // 퇴근시간이 정규 퇴근시간 이전이면 조퇴이고 조퇴라인 표시해줌
            if (realWCTime < tWCTime)
            {
                document.getElementById("trEarlyTime2").style.display = "";
                document.getElementById("spEarlyTime").innerText = array_rtn[3];

                document.getElementById("spOffWork").style.display = "none";
                document.getElementById("attendanceOffWork").innerText = "";
            }
            else
            {            
                document.getElementById("trEarlyTime2").style.display = "none";
                document.getElementById("spEarlyTime").innerText = "";

                document.getElementById("spOffWork").style.display = "";
                document.getElementById("attendanceOffWork").innerHTML = array_rtn[3] + "&nbsp;&nbsp;&nbsp;&nbsp;";
            }
        }   
        else
        {
            document.getElementById("trEarlyTime2").style.display = "none";
            document.getElementById("spEarlyTime").innerText = "";

            document.getElementById("spOffWork").style.display = "none";
            document.getElementById("attendanceOffWork").innerText = "";
        }

        // 근무중 표시
        if (array_rtn[0] == "1")
        {
            document.getElementById("beforework").style.display = "none"; // 출 근 전

            if (array_rtn[2] == "0")
            {
                document.getElementById("onduty").style.display = "";
            }

            if (array_rtn[2] == "1")
            {
                // 현재시간이 퇴근(조퇴)시간 이전일때만 근무중 표시
                if (tNowTime < realWCTime)
                {
                    document.getElementById("onduty").style.display = "";
                }
                else
                {
                    document.getElementById("onduty").style.display = "none";
                }
            }
        }
        else
        {
            document.getElementById("beforework").style.display = ""; // 출 근 전
            document.getElementById("onduty").style.display = "none";
        }

        // 주간 연장근무시간
        document.getElementById("spOverTime").innerText = OverTime;
    }    

    // 조퇴 취소
    function EarlyCancel()
    {
        if(!confirm(Attendance_EarlyCancelConfirm)) // 취소 처리 하시겠습니까?
        {
            return;
        }

        var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/DelOffWork";
        ParamArray = { }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("EarlyCancel_SuccessList");

        AjaxService(url, data, obj);
    }

    function EarlyCancel_SuccessList(data, status)
    {       
        document.getElementById("txtReason").value = "";
        AttendanceLoad();
    }

    function GetTime(_func)
    {
        var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/GetTime";
        ParamArray = { 'func': _func }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("GetTime_SuccessList");

        AjaxService(url, data, obj);
    }

    function GetTime_SuccessList(data, status)
    {
        if (data.d != null && data.d.length > 0) {
            var h = data.d.split('^')[0];
            var m = data.d.split('^')[1];
            var func = data.d.split('^')[2];

            var obj = eval(func);
            if (obj) {
                obj(h, m);
            }
        }
    }

    function GotoWork(_nowHour, _nowMinute)
    {  
        var reason = document.getElementById("txtReason").value;
        
        //var h = $('#jDateHour').val();
        //var m = parseInt($('#jDateMinute').val());
        var h = _nowHour;
        var m = parseInt(_nowMinute);
        var mm = m < 10 ? "0" + m : String(m);
        var tNowTime = parseInt(h + mm); //현재 시분 915
        var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
        
        if (isNaN(tNowTime)) return;
        
        if (tWSTime == "0") return;

        if (tNowTime >= tWSTime)
        {
            if (bWS == "1") return;
        }
        else
        {
            if (bWS == "1") 
            {
                if (!confirm(Attendance_GotoWorkRe))
                {
                    return;
                }
            }
        }

        if (tNowTime > tWSTime)
        {
            if(jQuery.trim(reason) == "")
            {
                alert(Attendance_Reason);
                return;
            }
            else
            {
                var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/SetGotoWork";
                ParamArray = { 'reason':reason, 'isHoli':isHoli, 'bLate': '1' }

                CallEncrypt(CallGetParam());
                var data = "{p:'" + GetEncryptValue + "'}";
                var obj = eval("GotoWork_SuccessList");

                AjaxService(url, data, obj);
            }
        }
        else
        {
            var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/SetGotoWork";
            ParamArray = { 'reason':"", 'isHoli':isHoli }

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("GotoWork_SuccessList");

            AjaxService(url, data, obj);
        }
    }

    function GotoWork_SuccessList(data, status)
    {      
        if (data.d != null && data.d.length > 0) {
            var ip = data.d.split('^')[0];
            var time = data.d.split('^')[1];
            var location = "사외";

            if (ip == "IN") {
                location = "사내";
            }

            // 출근 처리되었습니다.
            // 출근시간:09:00
            // 위치:사내
            alert(R_GotoWork_Success + "\n" + R_WSTime + ": " + time + "\n" + R_Location + ": " + location);
         
            document.getElementById("txtReason").value = "";
            AttendanceLoad();
        }
    }

    function OffWork(_nowHour, _nowMinute)
    {
        //var h = $('#jDateHour').val();
        //var m = parseInt($('#jDateMinute').val());
        var h = _nowHour;
        var m = parseInt(_nowMinute);
        var mm = m < 10 ? "0" + m : String(m);
        var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
        var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900

        // 현재시각이 출근시간 전이면 이전일로 처리한다. (00:00AM ~ 출근시간 사이)
        if (tNowTime < tWSTime)
        {
            PreAttendanceLoad();
        }
        else
        {                      
            if(bWS != "1") 
            {
                alert(Attendance_NotGotoWork);
                return;
            }
            else
            {
                var reason = document.getElementById("txtReason").value;
            
                //var h = $('#jDateHour').val();
                //var m = parseInt($('#jDateMinute').val());
                var h = _nowHour;
                var m = parseInt(_nowMinute);
                var mm = m < 10 ? "0" + m : String(m);
                var tNowTime = parseInt(h + mm); //현재 시분 1805
                var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

                if (tNowTime < tWCTime) //조퇴
                {
                    if (bWC == "1") {
                        return;
                    }

                    //document.getElementById("trEarly1").style.display = "";
                    document.getElementById("trEarly2").style.display = "";
                    //document.getElementById("trEarlyTime1").style.display = "none";
                    document.getElementById("trEarlyTime2").style.display = "none";

                    if(jQuery.trim(reason) == "")
                    {                     
                        alert(Attendance_Reason);
                        return;
                    }
                    
                    if (document.getElementById("ddlEarly").value == "0")
                    {
                        alert(Attendance_EarlyTimeMsg); // 조퇴 예정시간을 선택하세요.
                        return;
                    }

                    if(!confirm(Attendance_EarlyConfirm)) // 조퇴 처리 하시겠습니까?
                    {
                        return;
                    }

                    var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/SetOffWork";
                    ParamArray = { 'reason':reason, 'isHoli':isHoli, 'ddlEarly':document.getElementById("ddlEarly").value, 'bEarly': '1' }

                    CallEncrypt(CallGetParam());
                    var data = "{p:'" + GetEncryptValue + "'}";
                    var obj = eval("OffWork_SuccessList");

                    AjaxService(url, data, obj);
                }
                else //퇴근
                {
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

                    var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/SetOffWork";
                    ParamArray = { 'reason':"", 'isHoli':isHoli }

                    CallEncrypt(CallGetParam());
                    var data = "{p:'" + GetEncryptValue + "'}";
                    var obj = eval("OffWork_SuccessList");

                    AjaxService(url, data, obj);
                }

            }
        }
    }

    function OffWork_SuccessList(data, status)
    {       
        if (data.d != null && data.d.length > 0) {
            var ip = data.d.split('^')[0];
            var time = data.d.split('^')[1];
            var early = data.d.split('^')[2];
            var location = "사외";

            if (ip == "IN") {
                location = "사내";
            }

            if (early == "1") {
                // 조퇴 처리되었습니다.
                // 조퇴시간: 05:00
                // 위치: 사내
                alert(R_OffWorkEarly_Success + "\n" + R_Early_Time + ": " + time + "\n" + R_Location + ": " + location);
            }
            else {
                // 퇴근 처리되었습니다.
                // 퇴근시간: 06:00
                // 위치: 사내
                alert(R_OffWork_Success + "\n" + R_WCTime + ": " + time + "\n" + R_Location + ": " + location);
            }

            document.getElementById("txtReason").value = "";
            //document.getElementById("trEarly1").style.display = "none";
            document.getElementById("trEarly2").style.display = "none";

            AttendanceLoad();
        }
    }

    // 이전일 퇴근시간 등록
    function PreAttendanceLoad()
    {   
        var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/GetPreAttendanceData";
        
        ParamArray = { }

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("PreAttendance_SuccessList");

        AjaxService(url, data, obj);
    }

    function PreAttendance_SuccessList(data, status)
    {       
        var rtn = data.d;
        
        var array_rtn = rtn.split("|"); 

        var Pre_bWS = array_rtn[0]; // 전날 출근여부
        var Pre_bWC = array_rtn[2]; // 전날 퇴근여부
        var Pre_isHoli = array_rtn[4]; // 전날 휴일여부
        var Pre_Date = array_rtn[5]; //전날 날짜

        if(Pre_bWS != "1") // 전날 출근여부
        {
            alert(Pre_Date + " " + Attendance_NotGotoWork);
            return;
        }
        else
        {
            if(Pre_bWC != "1") // 전날 퇴근여부
            {
                if(!confirm(Pre_Date + " " + Attendance_OffWorkConfirm))
                {
                    return;
                }
            }
            else
            {
                if(!confirm(Pre_Date + " " + Attendance_OffWorkRe))
                {
                    return;
                }
            }
            
            var url = "/DeskPlusEIP/Portlet/Ws/Package/HR/Attendance.asmx/SetPreOffWork";
            ParamArray = { 'reason':"", 'isHoli':Pre_isHoli }

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("PreOffWork_SuccessList");

            AjaxService(url, data, obj);

        }
    }

    function PreOffWork_SuccessList(data, status)
    {      
        if (data.d != null && data.d.length > 0) { 
            var ip = data.d.split('^')[0];
            var time = data.d.split('^')[1];
            var location = "사외";

            if (ip == "IN") {
                location = "사내";
            }

            // 퇴근 처리되었습니다.
            // 퇴근시간: 06:00
            // 위치: 사내
            alert(R_OffWork_Success + "\n" + R_WCTime + ": " + time + "\n" + R_Location + ": " + location);

            document.getElementById("txtReason").value = "";
            AttendanceLoad();
        } 
    }

    function GoHRList() {
        ParamArray = {};

        CallEncrypt(CallGetParam());            
        location = "/DeskPlusEIP/HR/Default.aspx?p=" + GetEncryptValue;
    }
</script>
<link rel="stylesheet" href="/DeskPlusEIP/Portlet/Package/Clock/css/jClocksGMT.css">
<script type="text/jscript" src="/DeskPlusEIP/Portlet/Package/Clock/js/jClocksGMT.js?v1=2"></script>
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
    
    .btn-atdangercancel {
        color: #fff;
        background-color: #dc3545;
        border-color: #dc3545;
    }  
    .totime{ vertical-align:middle}
   
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
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>
    
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">
                <div class="nav-tabs">     
                </div>
                <div class="content-tabs hr">
                    <!-- 내용 //-->

                    <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
                    <tbody>
                    <tr>
                        <td style="text-align:center;">
                            <div id='clock_NowDateTime' >
                                <div>                            
                                </div>
                                <div class='digital' style='padding-top:5px;'>
                                <span id="nowdate" style="pt-2"><%=NowDate%></span>
                                <br />
	                            <span class='hr' id="hr"></span><span class='minute' id="minute"></span> <span class='period' id="period"></span>
                                </div>
                            </div>
                        </td>     
                        <td class="pt-2">
                            <table cellpadding="0" cellspacing="0" border="0" style="width:99%;">
                                <tr>
                                    <td style="text-align:center;">
                                        <button type="button" class="btn btn-secondary" style="cursor:pointer; width:80px" id="GotoWork"><%=GetResource("Attendance_GotoWork")%></button>
                                    </td>
                                </tr>
                                <tr style="height:5px;">
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;">
                                        <button type="button" class="btn btn-danger" style="cursor:pointer; width:80px" id="HROffWork"><%=GetResource("Attendance_OffWork")%></button>
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
                            <textarea id="txtReason" style="width:100%; text-align:left" rows="3" cols="3"></textarea>
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td>
                        </td>
                    </tr>
                    <tr style="height:10px;">
                        <td style="font-weight:bold; padding-left:7px;" colspan="2">
                            <i class="lni lni-alarm-clock"></i>
                            <span id="beforework" style="display:none" class="number"><span style="margin-left:-3px"><%=GetResource("Beforework")%>&nbsp;&nbsp;</span></span>
                            <span id="onduty" style="display:none"  class="number"><span style="color: #dc3545;margin-left:-3px"><%=GetResource("OnDuty")%>&nbsp;&nbsp;</span></span>
                            <span id="spOffWork" style="color:#9c9c9c;margin-left:-3px; display:none" class="number"><%=GetResource("Attendance_OffWork")%>&nbsp;-</span><span id="attendanceOffWork" style="padding-left:5px;"></span>

	                        <span style="color:#9c9c9c;margin-left:-8px;" class="number"><%=GetResource("Attendance_GotoWork")%>&nbsp;-</span><span id="attendanceGotoWork" style="padding-left:5px;"></span>
                        </td>
                    </tr>
                    <tr id="trEarly2" style="height:10px; display: none">
                        <td style="font-weight:bold; padding-left:7px;"  colspan="2"><i class="lni lni-bi-cycle"></i><span style="color:#9c9c9c;"><%=GetResource("Attendance_Early")%>&nbsp;-</span>&nbsp;<asp:DropDownList ID="ddlEarly" runat="server" ClientIDMode="Static" CssClass="searchselect"></asp:DropDownList></td>
                    </tr>
                    <tr id="trEarlyTime2" style="height:10px; display: none">
                        <td style="font-weight:bold; padding-left:7px;"  colspan="2">
                            <i class="lni lni-bi-cycle"></i><span style="color:#9c9c9c;"><%=GetResource("Attendance_Early")%>&nbsp;-</span><span id="spEarlyTime" style="padding-left:5px;"></span>
                            &nbsp;&nbsp;<button type="button" class="btn btn-sm btn-atdangercancel" style="cursor:pointer" id="btnEarlyCancel"><%=GetResource("Cancel")%></button>
                        </td>
                    </tr>
                    <tr style="height:40px;display:none;">
                        <td style="font-weight:bold;  padding-left:7px;">
                            <span style="float:left;"><i class="lni lni-calendar "></i></span>
                            <span style="color:#9c9c9c; float:left;line-height:1.5em; padding-top:8px;" ><%=GetResource("WeekOverTime")%><br /><font style="font-weight:normal; font-size:12px; "><%=GetResource("WeekOverTimeMsg")%></font></span>
                        </td>
                        <td style="font-weight:bold; vertical-align:bottom">
                            <span style="" class="totime"><a style="font-size: 13pt"  class="number" onclick="GoHRList()"><font id="spOverTime" style="color:#dc3545; padding-left:5px;"></font>&nbsp;/&nbsp;12</a>&nbsp;<font style="color:#9c9c9c; "><%=GetResource("Hour")%></font></span>
                        </td>
                    </tr>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    
        <div style="display: none">
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
            <input type="hidden" id="jDateHour" />
            <input type="hidden" id="jDateMinute" />
        </div>
</div>

<!-- 포틀릿(배너) End //-->