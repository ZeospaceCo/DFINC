Menu = {}

$(function () {
    $(document).ready(function () {

        //이벤트 
        $('#CbtnMonth').click(function (e) {
            e.preventDefault();
            Menu.SetAction("MONTH", "");

        });

        $('#CbtnWeek').click(function (e) {
            e.preventDefault();
            Menu.SetAction("WEEK", "");
        });

        $('#CbtnDay').click(function (e) {
            e.preventDefault();
            Menu.SetAction("DAY", "");
        });

        $('#CbtnGetUser').click(function (e) {
            e.preventDefault();
            Menu.GetUser();
        });

        $('#CbtnShareTotal').click(function (e) {
            e.preventDefault();
            Menu.ShareTotal();
        });

        //공유선택        
        $('#DDLShareUser').change(function (e) {
            e.preventDefault();
            Menu.ShareCombo();
        });

        $('#CbtnTarget').click(function (e) {
            e.preventDefault();
            $("select[name='DDLShareUser']").find('option').each(function () {
                $(this).remove();
            });
            //$("#DDLShareUser").append("ccc");
            $("#DDLShareUser").get(0).options[0] = new Option(MySchedule, "A");
            $("#DDLShareUser").get(0).options[1] = new Option(AllSchedule, "M");

            Menu.Anniversary();

        });

        Menu.GetAnniday();
        Menu.GetShareList();
    });
});



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 달력 선택
***************************************************/
Menu.OnCalendarChange = function (cal) {
    var targetURL = "";  //특정 URL로 바로갈경우 사용
    var targetFrame = ""; // 대상 프레임

    var selectedYear = cal.FormatDate(CalendarSingle.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(CalendarSingle.GetSelectedDate(), "M");
    var selectedDay = cal.FormatDate(CalendarSingle.GetSelectedDate(), "d");

    //    SetAction(currentView, '');
    var targetURL = "";
    var dateinfo = selectedYear + '-' + selectedMonth + '-' + selectedDay;

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


    ParamArray = {
        'Date': dateinfo,
        'Scope': Scope,
        'UserID': arg
    };
    CallEncrypt(CallGetParam());


    if (parent.frmContent.location.href.indexOf("ListDay.aspx") > -1) {
        targetURL = "../Schedule/ListDay.aspx?p=" + GetEncryptValue; //+ param;
    }
    if (parent.frmContent.location.href.indexOf("ListWeek.aspx") > -1) {
        targetURL = "../Schedule/ListWeek.aspx?p=" + GetEncryptValue; //+ param;
    }
    if (parent.frmContent.location.href.indexOf("ListMonth.aspx") > -1) {
        
        targetURL = "../Schedule/ListMonth.aspx?p=" + GetEncryptValue; //+ param;
    }
    if (targetURL == "") {
        targetURL = "../Schedule/ListDay.aspx?p=" + GetEncryptValue; //+ param;
    }

    if (targetFrame == "SELF") {
        document.location.href = targetURL;
    } else {
        parent.frmContent.location.href = targetURL;
    }

    Menu.GetAnniday();
    Menu.GetShareList();
}



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 일/주/월간 페이지 이동
***************************************************/
Menu.SetAction = function (_action, _target) {

    var targetURL = "";  //특정 URL로 바로갈경우 사용
    var targetFrame = ""; // 대상 프레임

    //    if ($("openTypeList").value != "") {
    //        selType = $("openTypeList").value;
    //    }

    //    var param = "?yy=" + selectedYear + "&mm=" + selectedMonth + "&dd=" + selectedDay + "&uid=" + $("ddlShares").value + "&unm=" + escape($("ddlShares").options[$("ddlShares").selectedIndex].text) + "&selType=" + selType;

    //    if (_action == "") {
    //        if (currentView != "") {
    //            _action = currentView;
    //        } else {
    //            _action = "DAY";
    //        }
    //    }

    switch (_action) {
        //일별 보기      
        case "DAY": targetURL = "../Schedule/ListDay.aspx"; //+ param;
            //            currentView = "DAY";
            break;
        //주별 보기      
        case "WEEK": targetURL = "../Schedule/ListWeek.aspx"; //+ param;
            //            currentView = "WEEK";
            break;
        //달별 보기      
        case "MONTH": targetURL = "../Schedule/ListMonth.aspx"; //+ param;
            //            currentView = "MONTH";
            break;
        //전체 일정보기      
        case "TOTAL": targetURL = "../Schedule/ListTotal.aspx"; //+ param;
            //            currentView = "TOTAL";
            break;
         
        default:
            targetURL = _target;
            break;
    }

    if (targetFrame == "SELF") {
        document.location.href = targetURL;
    } else {
        parent.frmContent.location.href = targetURL;
    }

    Menu.GetAnniday();
    Menu.GetShareList();
}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 전체 공유일정 보기
***************************************************/
Menu.ShareTotal = function () {
    var url = "../Schedule/ListTotal.aspx";
    parent.frmContent.location.href = url;
}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 일정 공개대상
***************************************************/
Menu.GetUser = function () {

    var url = "/DeskPlusEIP/Pim/Common/GetUser.aspx";


    parent.frmContent.ShareListOpen(url);
    //Menu.GetShareList();

}



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 목표관리
***************************************************/
Menu.Target = function () {
    var url = "../Target/List.aspx";
    parent.frmContent.location.href = url;

}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 기념일 관리
***************************************************/
Menu.AddAnniv = function () {
    var url = "../Anniversary/List.aspx";
    parent.frmContent.location.href = url;

    Menu.GetAnniday();
    Menu.GetShareList();
}

/*다국어*/
Menu.Resource = function () {
    //내일정보기
    //전체일정
}

/*웹서비스*/
Menu.GetShareList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Menu/Menu.asmx/GetOpenUserComboList";
    ParamArray = {
        'companyID': CompanyID,
        'UserId': UserID
    };
    
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessShareList");

    AjaxService(url, data, obj);
}

Menu.SuccessShareList = function (data, status) {

    $("select[id='DDLShareUser']").find('option').each(function () {
        $(this).remove();

    });

    $("#DDLShareUser").get(0).options[0] = new Option(MySchedule, "A");
    $("#DDLShareUser").get(0).options[1] = new Option(AllSchedule, "M");

    $.each(data.d, function (index, list) {
        
        $("#DDLShareUser").get(0).options[index + 2] = new Option(list.Name, list.UserID);
    });
}


/*
작성자   : daniel
작성일자 : 2011-06-17
작성목적 : 공유대상 콤보
*/


/*
    기념일관리
*/
Menu.Anniversary = function () {
    var url = "../Anniversary/List.aspx";
    parent.frmContent.location.href = url;
}


/*****************************
 작성자: DANIEL
 작성일자: 2011-9-16
 용도: 기념일 가져오기
******************************/

Menu.GetAnniday = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Menu/Menu.asmx/GetScheduleListMenu";
    ParamArray = {
        'companyID': CompanyID,
        'StartDate': ''
    }
    //alert(NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1)+'-'+ (parseInt(NowDate.getDate()) ));

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessGetAnniday");

    AjaxService(url, data, obj);
}



Menu.SuccessGetAnniday = function (data, status) {

    var strHtml = "";

    $.each(data.d, function (index, list) {

        strHtml = strHtml + "<li><p  class='list_title'><a href='#' onclick = 'javascript:Menu.ViewAnniDay(" + list.ScheduleID + ")'>" + list.Title + "</a></p></li>"
    });
    

    $("#divAni").html(strHtml);
}

Menu.ViewAnniDay = function (arg) {
    
    ParamArray = {
        'ScheduleID': arg
    };

    CallEncrypt(CallGetParam());

    var url = "../Anniversary/View.aspx?p=" + GetEncryptValue;

    parent.frmContent.location.href = url;

    Menu.GetAnniday();
    Menu.GetShareList();
}



/*******************
 작성자: 김광일
 작성일: 2011-9-20
 용도 : 공유 선택 
********************/

Menu.ShareCombo = function () {

    var arg = $("#DDLShareUser").val();

    var targetURL = "";  //특정 URL로 바로갈경우 사용
    var targetFrame = ""; // 대상 프레임
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

    ParamArray = {
        'Scope': Scope,
        'UserID': arg
    };
    

    CallEncrypt(CallGetParam());

    if (parent.frmContent.location.href.indexOf("ListDay.aspx") > -1) {
        targetURL = "../Schedule/ListDay.aspx?p=" + GetEncryptValue; //+ param;
    }
    if (parent.frmContent.location.href.indexOf("ListWeek.aspx") > -1) {
        targetURL = "../Schedule/ListWeek.aspx?p=" + GetEncryptValue; //+ param;
    }
    if (parent.frmContent.location.href.indexOf("ListMonth.aspx") > -1) {
        targetURL = "../Schedule/ListMonth.aspx?p=" + GetEncryptValue; //+ param;
    }

    if (targetURL == "") {
        targetURL = "../Schedule/ListDay.aspx?p=" + GetEncryptValue; //+ param;
    }


    if (targetFrame == "SELF") {
        document.location.href = targetURL;
    } else {
        parent.frmContent.location.href = targetURL;
    }

    Menu.GetAnniday();
    //Menu.GetShareList();
}