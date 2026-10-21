$(document).ready(function () {

    //962_2725
    var portletDom = PimL_tabPortletID + "_" + PimL_portletID;

    //관리자 여부
    if (PimL_IsAuth != "True") {
        $("#ptConfig_" + portletDom).remove();
    }

    //최소화 여부
    if (PimL_IsMini == "1") {
        $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
        $("#ptContent_" + portletDom).fadeOut("slow");
    }

    //포틀릿 최소화/최대화
    $("#ptSlide_" + portletDom).on("click", function (e) {
        if ($("#hdnTabSlide_" + portletDom).val() == "1") {
            //최대화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
            $("#ptContent_" + portletDom).fadeIn("slow");
            $("#hdnTabSlide_" + portletDom).val("0");
        }
        else {
            //최소화
            Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
            $("#ptContent_" + portletDom).fadeOut("slow");
            $("#hdnTabSlide_" + portletDom).val("1");
        }
    });

    // 포틀릿 설정
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

    // 더보기
    $("#ptPreview_" + portletDom).on("click", function (e) {
        e.preventDefault();

        parent.frmTop.Menu.GoParam("webpims");
        document.location.href = "/DeskPlusEIP/PIM/Default.aspx";
    });

    PimL_LoadData();

});

/**************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 달력 날짜 선택
***************************************************/
function OnCalendarChange(cal) {

    //선택된 날짜 
    var obj = eval(PimL_CalendarSingleObj);
    var selectedYear = cal.FormatDate(obj.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(obj.GetSelectedDate(), "MM");
    var selectedDay = cal.FormatDate(obj.GetSelectedDate(), "dd");
    var selectedWeek = obj.GetSelectedDate().getDay();

    PimL_TmpDate = selectedYear + "-" + selectedMonth + "-" + selectedDay;
    $("#sDate1").text(selectedMonth + "." + selectedDay + " " + getWeek(selectedWeek));

    //선택된 날짜의 다음날
    var PimL_next = new Date(selectedYear, eval(selectedMonth + "-1"), eval(selectedDay + "+1"));
    var nYear = PimL_next.getFullYear();
    var nMonth = (PimL_next.getMonth() + 1) < 10 ? "0" + (PimL_next.getMonth() + 1) : (PimL_next.getMonth() + 1);
    var nDay = PimL_next.getDate() < 10 ? "0" + PimL_next.getDate() : PimL_next.getDate();
    var nWeek = PimL_next.getDay();

    PimL_NextDate = nYear + "-" + nMonth + "-" + nDay;
    $("#sDate2").text(nMonth + "." + nDay + " " + getWeek(nWeek));

    // 일정, 할일 가져오기
    PimL_GetPimList();
    PimL_GetNPimList();
    PimL_GetTodoList();
    PimL_GetNTodoList();
}

/**************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 요일
***************************************************/
function getWeek(val) {

    switch (val) {
        case 0: return R_Sun;
        case 1: return R_Mon;
        case 2: return R_Tue;
        case 3: return R_Wed;
        case 4: return R_Thu;
        case 5: return R_Fri;
        case 6: return R_Sat;
        default: return "";
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2017-02-09
작성목적 : 리스트 가져오기
***************************************************/
function PimL_LoadData() {

    //선택된 날짜
    var ArrPimLDate = PimL_TmpDate.split("-");
    var PimL_date = new Date(PimL_TmpDate);
    var dWeek = PimL_date.getDay();
    $("#sDate1").text(ArrPimLDate[1] + "." + ArrPimLDate[2] + " " + getWeek(dWeek));

    var ArrPimLNDate = PimL_NextDate.split("-");
    var PimL_next = new Date(PimL_NextDate);
    var nWeek = PimL_next.getDay();
    $("#sDate2").text(ArrPimLNDate[1] + "." + ArrPimLNDate[2] + " " + getWeek(nWeek));

    // 일정, 할일 가져오기
    PimL_GetPimList();
    PimL_GetNPimList();
    PimL_GetTodoList();
    PimL_GetNTodoList();
}


/**************************************************
작성자   : famo23
작성일자 : 2017-02-10
작성목적 : 웹서비스 리스트 호출
***************************************************/
function PimL_GetPimList() {

    var url = "/DeskPlusEIP/Portlet/Ws/Package/PimList.asmx/GetScheduleList";

    ParamArray = {
        'tmpDate': PimL_TmpDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PimL_SuccessList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-10
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function PimL_SuccessList(data, status) {

    var strHtml = "";

    $.each(data.d, function (index, list) {

        if (PimL_NowDate == PimL_TmpDate) {

            if (list.PGubun == "N") {
                strHtml = strHtml + "<li><a onclick = 'TabPim_PopupSchView(" + list.ScheduleID + "," + list.DisplayID + ")'>" + list.StartTime + " ~ " + list.EndTime + "&nbsp;" + list.Title + "</a></li>";
            }
            else {
                strHtml = strHtml + "<li><a onclick = 'TabPim_PopupSchView(" + list.ScheduleID + "," + list.DisplayID + ")'><b>" + list.StartTime + " ~ " + list.EndTime + "&nbsp;" + list.Title + "</b></a></li>";
            }
        }
        else {
            strHtml = strHtml + "<li><a onclick = 'TabPim_PopupSchView(" + list.ScheduleID + "," + list.DisplayID + ")'>" + list.StartTime + " ~ " + list.EndTime + "&nbsp;" + list.Title + "</a></li>";
        }
    });

    $("#divsche1").html(strHtml);
}


/**************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 웹서비스 리스트 호출
***************************************************/
function PimL_GetNPimList() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/PimList.asmx/GetScheduleList";

    ParamArray = {
        'tmpDate': PimL_NextDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PimL_SuccessNList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function PimL_SuccessNList(data, status) {

    var NstrHtml = "";

    $.each(data.d, function (Nindex, Nlist) {

        NstrHtml = NstrHtml + "<li><a onclick = 'TabPim_PopupSchView(" + Nlist.ScheduleID + "," + Nlist.DisplayID + ")'>" + Nlist.StartTime + " ~ " + Nlist.EndTime + "&nbsp;" + Nlist.Title + "</a></li>";
    });

    $("#divsche2").html(NstrHtml);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-10
작성목적 : 일정보기
**********************************************************************************************/
function TabPim_PopupSchView(arg, dis) {

    var url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
    ParamArray = {
        'ScheduleID': arg,
        'DisplayID': dis
    };
    CallEncrypt(CallGetParam());
    WindowObjectReference = PopupWinOpen(url, "p=" + GetEncryptValue, "pimpop", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");    
}


/**************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 웹서비스 리스트 호출
***************************************************/
function PimL_GetTodoList() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/PimList.asmx/GetTodoList";

    ParamArray = {
        'tmpDate': PimL_TmpDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Todo_SuccessList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function Todo_SuccessList(data, status) {

    var strHtml = "";

    $.each(data.d, function (index, list) {
        strHtml = strHtml + "<li><a onclick = 'TabPim_PopupToDoView(" + list.ScheduleID + ")'>" + list.Priority + "&nbsp;" + list.Title + "</a></li>";
    });

    $("#divtodo1").html(strHtml);

    $("#hrtodo1").css("display", "none");

    if (strHtml != "") {
        if ($("#divsche1").html() != "") {
            $("#hrtodo1").css("display", "");
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 웹서비스 리스트 호출
***************************************************/
function PimL_GetNTodoList() {
    var url = "/DeskPlusEIP/Portlet/Ws/Package/PimList.asmx/GetTodoList";

    ParamArray = {
        'tmpDate': PimL_NextDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Todo_SuccessNList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-13
작성목적 : 성공시 , 리스트 표시
**********************************************************************************************/
function Todo_SuccessNList(data, status) {

    var NstrHtml = "";
    $("#hrtodo2").css("display", "none"); 

    $.each(data.d, function (Nindex, Nlist) {

        $("#hrtodo2").css("display", ""); 
        NstrHtml = NstrHtml + "<li><a onclick = 'TabPim_PopupToDoView(" + Nlist.ScheduleID + ")'>" + Nlist.Priority + "&nbsp;" + Nlist.Title + "</a></li>";

    });

    $("#divtodo2").html(NstrHtml);

    $("#hrtodo2").css("display", "none");

    if (NstrHtml != "") {
        if ($("#divsche2").html() != "") {
            $("#hrtodo2").css("display", "");
        }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-10
작성목적 : 일정보기
**********************************************************************************************/
function TabPim_PopupToDoView(arg) {

    var url = "/DeskPlusEIP/Pim/ToDo/ViewPopup.aspx";
    ParamArray = {
        'TodoID': arg
    };
    CallEncrypt(CallGetParam());
    WindowObjectReference = PopupWinOpen(url, "p=" + GetEncryptValue, "PopupToDoViewList", "1200", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-10
작성목적 : 일정이동
**********************************************************************************************/
function goPimUrl(p_gubun) {

    if (p_gubun == "1") {
        ParamArray = {
            'Date': PimL_TmpDate
        };
    }
    else {
        ParamArray = {
            'Date': PimL_NextDate
        };
    }

    CallEncrypt(CallGetParam());

    parent.frmTop.Menu.GoParam("webpims");
    document.location.href = "/DeskPlusEIP/PIM/Default.aspx?p=" + GetEncryptValue;
    
}
