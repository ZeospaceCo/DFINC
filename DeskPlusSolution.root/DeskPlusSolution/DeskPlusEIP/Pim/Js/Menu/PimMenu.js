PimMenu = {}

$(function () {
    $(document).ready(function () {


        //기념일
        PimMenu.GetAnniday();
        PimMenu.GetScheduleStatic();

    });
});



/**************************************************
작성자   : 김광일
작성일자 : 2011-10-26
작성목적 : 기념일 가져오기 
***************************************************/
PimMenu.GetAnniday = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Menu/PimMenu.asmx/GetScheduleListMenu";

    ParamArray = {
        'companyID': CompanyID,
        'StartDate': NowDate
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PimMenu.SuccessGetAnniday");

    AjaxService(url, data, obj);
}

PimMenu.SuccessGetAnniday = function (data, status) {

    var strHtml = "";

    $.each(data.d, function (index, list) {

        strHtml = strHtml + "<li class='list_item'><h2><a href='#' onclick = \"PimMenu.ViewAnniDay('" + list.ScheduleID + "','" + list.Type + "')\">" + list.Title + "</a></h2></li>"
    });

    $("#divAni").html(strHtml);

    if (strHtml == "") {
        $("#AniUl").css("display", "none");
    }
    else {
        $("#AniUl").css("display", "");
    }
}



PimMenu.ViewAnniDay = function (_scheduleID, _type) {


    var tmpPageScope = parent.frmContent.PageScope;

    if (tmpPageScope == "ListDay.aspx" || tmpPageScope == "ListMonth.aspx" || tmpPageScope == "ListWeek.aspx") {
        PageScope = tmpPageScope;
    }
    else {
        if (PageScope == "") {
            PageScope = "ListDay.aspx";
        }
    }

    if (_type == 'J') {
        ParamArray = {
            'ScheduleID': _scheduleID,
            'PageScope': PageScope
        };

        CallEncrypt(CallGetParam());

        var url = "../Anniversary/View.aspx?p=" + GetEncryptValue;

        parent.frmContent.location.href = url;
    }
    else {
        ParamArray = {
            'ScheduleID': _scheduleID,
            'PageScope': PageScope,
            'Date': NowDate
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Pim/Admin/Holiday/View.aspx?p=" + GetEncryptValue;

        parent.frmContent.location.href = url;
    }
}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 달력 선택
***************************************************/
PimMenu.OnCalendarChange = function (cal) {

    var targetURL = "";  //특정 URL로 바로갈경우 사용
    var targetFrame = ""; // 대상 프레임

    var selectedYear = cal.FormatDate(CalendarSingle.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(CalendarSingle.GetSelectedDate(), "MM");
    var selectedDay = cal.FormatDate(CalendarSingle.GetSelectedDate(), "dd");

    //    SetAction(currentView, '');
    var targetURL = "";
    var dateinfo = selectedYear + '-' + selectedMonth + '-' + selectedDay;

    //alert(dateinfo);

    var arg = $("#DDLShareUser").val();
    var Scope = "";

    if (arg == "A") {  //내일정
        Scope = "A";
        arg = "0";
    }
    else if (arg == "M") { //전체일정
        arg = "0";
        Scope = "M";
        Menu.ShareTotal();
        return;
    }
    else {
        Scope = "U";
    }

    NowDate = dateinfo;

    var page = "ListDay.aspx";

    if (parent.frmContent.PageScope != null)
        page = parent.frmContent.PageScope;

    dateinfo = DateFormat(dateinfo);

    ParamArray = {
        'Date': dateinfo,
        'Scope': Scope,
        'UserID': arg,
        'page': parentpage,
        'PageScope': page
    };
    CallEncrypt(CallGetParam());

    var purl;
    //alert(document.location + " : " + parent.frames[1].location + " : " + parent.frames['frmContent'].location);
    try {
        purl = parent.frmContent.location.toString();

        if (purl.indexOf("AllSchedule.aspx") > -1) {
            targetURL = "/DeskplusEip/Pim/Schedule/AllSchedule.aspx?p=" + GetEncryptValue; //+ param;
        }
        else {
            targetURL = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx?p=" + GetEncryptValue; //+ param;
        }

        if (targetFrame == "SELF") {
            document.location.href = targetURL;
        } else {
            parent.frmContent.location.href = targetURL;
        }

        PimMenu.GetAnniday();
        PimMenu.GetScheduleStatic();
    } catch (e) {
        parent.frames[1].location.href = "/DeskplusEip/Pim/Schedule/AllSchedule.aspx?p=" + GetEncryptValue; //+ param;
        PimMenu.GetAnniday();
        PimMenu.GetScheduleStatic();
    }

}




/**************************************************
작성자   : 김광일
작성일자 : 2011-10-26
작성목적 : 일정정보 가져오기
***************************************************/
PimMenu.GetScheduleStatic = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Menu/PimMenu.asmx/GetScheduleStatic";

    ParamArray = {
        'UserID': UserID,
        'CompanyID': CompanyID,
        'StartDate': NowDate

    }
    //alert(NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1)+'-'+ (parseInt(NowDate.getDate()) ));

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PimMenu.SuccessScheduleStatic");

    AjaxService(url, data, obj);
}

PimMenu.SuccessScheduleStatic = function (data, status) {

    var strHtml = "";
    var chk1 = 1;
    var chk2 = 1;
    var chk3 = 1;
    var chk4 = 1;

    var rthtml1 = "<li class='pim_private'><span class='badge badge-info'>" + Schedule1 + " </span> <span class='badge number badge-light'>0</span>" + GUN + "</li>";
    var rthtml2 = "<li class='pim_dept'><span class='badge badge-success'>" + Schedule2 + " </span>  <span class='badge number badge-light'>0</span>" + GUN + "</li>";
    var rthtml3 = "<li class='pim_share'><span class='badge badge-secondary badge-shar'>" + Schedule3 + " </span>  <span class='badge number badge-light'>0</span>" + GUN + "</li>";
    var rthtml4 = "<li class='pim_tot'><span class='badge badge-primary'>" + Schedule4 + " </span>  <span class='badge number badge-light'>0</span>" + GUN + "</li>";

    $.each(data.d, function (index, list) {
        if (list.OpenType == "0") {
            rthtml1 = "<li class='pim_private'><span class='badge badge-info'>" + Schedule1 + " </span> <span class='badge number badge-info'>" + list.CompanyID + "</span>" + GUN + "</li>"
            chk1 = 2;
        }
        else if (list.OpenType == "3") {
            rthtml2 = "<li class='pim_dept'><span class='badge badge-success'>" + Schedule2 + " </span> <span class='badge number badge-light'>" + list.CompanyID + "</span>" + GUN + "</li>"
            chk2 = 2;
        }
        else if (list.OpenType == "1") {
            rthtml3 = "<li class='pim_share'><span class='badge badge-secondary badge-shar'>" + Schedule3 + " </span> <span class='badge number badge-light'>" + list.CompanyID + "</span>" + GUN + "</li>"
            chk3 = 2;
        }
        else if (list.OpenType == "2") {
            rthtml4 = "<li class='pim_tot'><span class='badge badge-primary'>" + Schedule4 + " </span> <span class='badge number badge-light'>" + list.CompanyID + "</span>" + GUN + "</li>"
            chk4 = 2;
        }

    });

    $("#divsche").html(rthtml1 + rthtml2 + rthtml3 + rthtml4);
}


//다국어 날짜 세팅
function DateFormat(_date) {
    var arrDate = _date.split('-');
    var ParsingDate = '';
    switch (PreLang) {
        case "ko":
            //"yy-mm-dd";
            ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            //$("#timeList").html(arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2]);
            break;
        case "en":
            //"mm/dd/yy";
            ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"dd/mm/yy";
            ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm/dd";
            ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "fr":
            //"dd/mm/yy";
            ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm/dd";
            ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "zh-tw":
            //"yy-mm-dd";
            ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            break;
    }
    return ParsingDate;
}