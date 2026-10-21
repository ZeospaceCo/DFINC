CalendarDual = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    NowDate = NowYear + "-" + NowMonth + "-" + NowDay;

    StarDate = NowDate;
    EndDate = NowDate;
    StartCalMonth = NowYear + "-" + NowMonth;
    EndCalMonth = NowYear + "-" + NowMonth;

    $("#selectStart").text(StarDate);
    $("#selectEnd").text(EndDate);

    CalendarDual.getCalendar(StartCalMonth);

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        CalendarDual.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        CalendarDual.Close();
    });

    $('#btStarDate').click(function (e) {
        e.preventDefault();
        CalendarDual.LoadDisplay("S");
    });

    $('#btEndDate').click(function (e) {
        e.preventDefault();
        CalendarDual.LoadDisplay("E");
    });

    $('#pvMonthS').click(function (e) {
        e.preventDefault();
        CalendarDual.pvMonthS();
    });

    $('#nxMonthS').click(function (e) {
        e.preventDefault();
        CalendarDual.nxMonthS();
    });

    $('#pvMonthE').click(function (e) {
        e.preventDefault();
        CalendarDual.pvMonthE();
    });

    $('#nxMonthE').click(function (e) {
        e.preventDefault();
        CalendarDual.nxMonthE();
    });

    $("#ddlStartYear").change(function (e) {
        e.preventDefault();
        CalendarDual.LoadStart();
    });

    $("#ddlStartMonth").change(function (e) {
        e.preventDefault();
        CalendarDual.LoadStart();
    });

    $("#ddlEndYear").change(function (e) {
        e.preventDefault();
        CalendarDual.LoadEnd();
    });

    $("#ddlEndMonth").change(function (e) {
        e.preventDefault();
        CalendarDual.LoadEnd();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
CalendarDual.getCalendar = function (_calMonth) {
    var arrDate = _calMonth.split('-');
    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/GetBasicCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1]
    };

    var obj = eval("CalendarDual.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
CalendarDual.succeededCalendar = function (data, status) {
    if (SelectGubun == "S") {
        $('#phCalStart').setTemplateURL('/DeskPlusMobile/Common/Jt/JTCalendarS.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phCalStart').processTemplate(data.d);
    }
    else if (SelectGubun == "E") {
        $('#phCalEnd').setTemplateURL('/DeskPlusMobile/Common/Jt/JTCalendarE.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phCalEnd').processTemplate(data.d);
    }
    else {
        $('#phCalStart').setTemplateURL('/DeskPlusMobile/Common/Jt/JTCalendarS.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phCalStart').processTemplate(data.d);

        $('#phCalEnd').setTemplateURL('/DeskPlusMobile/Common/Jt/JTCalendarE.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phCalEnd').processTemplate(data.d);

        SelectGubun = "S";
    }

    if ($('#S_' + StarDate).text() != "") {
        TmpS_ClassName = $('#S_' + StarDate).attr("class");
        TmpS_Choice = StarDate;

        if (TmpS_ClassName.indexOf("hol") > 0) {
            $('#S_' + StarDate).attr("class", "busy holchoice");
        }
        else {
            $('#S_' + StarDate).attr("class", "busy choice");
        }
    }
    else {
        TmpS_ClassName = "";
        TmpS_Choice = "";
    }

    if ($('#E_' + EndDate).text() != "") {
        TmpE_ClassName = $('#E_' + EndDate).attr("class");
        TmpE_Choice = EndDate;

        if (TmpE_ClassName.indexOf("hol") > 0) {
            $('#E_' + EndDate).attr("class", "busy holchoice");
        }
        else {
            $('#E_' + EndDate).attr("class", "busy choice");
        }
    }
    else {
        TmpE_ClassName = "";
        TmpE_Choice = "";
    }

    setTimeout(function () {
        CalendarDual.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 다국어 처리
**********************************************************************************************/
CalendarDual.CalendarResource = function () {

    $("#JSunS")[0].innerHTML = R_Sun;
    $("#JMonS")[0].innerHTML = R_Mon;
    $("#JTueS")[0].innerHTML = R_Tue;
    $("#JWedS")[0].innerHTML = R_Wed;
    $("#JThuS")[0].innerHTML = R_Thu;
    $("#JFriS")[0].innerHTML = R_Fri;
    $("#JSatS")[0].innerHTML = R_Sat;

    $("#JSunE")[0].innerHTML = R_Sun;
    $("#JMonE")[0].innerHTML = R_Mon;
    $("#JTueE")[0].innerHTML = R_Tue;
    $("#JWedE")[0].innerHTML = R_Wed;
    $("#JThuE")[0].innerHTML = R_Thu;
    $("#JFriE")[0].innerHTML = R_Fri;
    $("#JSatE")[0].innerHTML = R_Sat;
}
/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 날짜 이동
***************************************************/
CalendarDual.pvMonthS = function () {
    addMonth(StartCalMonth, -1, "S");
}

CalendarDual.nxMonthS = function () {
    addMonth(StartCalMonth, 1, "S");
}

CalendarDual.pvMonthE = function () {
    addMonth(EndCalMonth, -1, "E");
}

CalendarDual.nxMonthE = function () {
    addMonth(EndCalMonth, 1, "E");
}


function addMonth(calMonth, v_month, gubun) {
    var arrDate = calMonth.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");

    var addDate = new Date(yyyy, eval(mm + '+' + v_month), 1);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);

    if (gubun == "S") {
        StartCalMonth = yyyy + "-" + mm;
        $("#ddlStartYear").val(yyyy);
        $("#ddlStartMonth").val(parseInt(mm));
        CalendarDual.getCalendar(StartCalMonth);
    }
    else {

        EndCalMonth = yyyy + "-" + mm;
        $("#ddlEndYear").val(yyyy);
        $("#ddlEndMonth").val(parseInt(mm));
        CalendarDual.getCalendar(EndCalMonth);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 달력 이동이벤트
***************************************************/
CalendarDual.StartsetList = function (calDate) {
    StarDate = calDate;

    $("#selectStart").text(StarDate);

    if (TmpS_Choice != "") {
        $('#S_' + TmpS_Choice).attr("class", TmpS_ClassName);
    }

    TmpS_ClassName = $('#S_' + StarDate).attr("class");
    TmpS_Choice = StarDate;

    if (TmpS_ClassName.indexOf("hol") > 0) {
        $('#S_' + StarDate).attr("class", "busy holchoice");
    }
    else {
        $('#S_' + StarDate).attr("class", "busy choice");
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 달력 이동이벤트
***************************************************/
CalendarDual.EndsetList = function (calDate) {
    EndDate = calDate;

    $("#selectEnd").text(EndDate);

    if (TmpE_Choice != "") {
        $('#E_' + TmpE_Choice).attr("class", TmpE_ClassName);
    }

    TmpE_ClassName = $('#E_' + EndDate).attr("class");
    TmpE_Choice = EndDate;

    if (TmpE_ClassName.indexOf("hol") > 0) {
        $('#E_' + EndDate).attr("class", "busy holchoice");
    }
    else {
        $('#E_' + EndDate).attr("class", "busy choice");
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-20
작성목적 : 버튼 선택 
***************************************************/
CalendarDual.LoadDisplay = function (arg) {

    SelectGubun = arg;

    if (arg == "S") {
        //시작일 선택이 보이게 
        jQuery.each($("#btStarDate"), function () { this.style.display = "none"; });
        jQuery.each($("#btEndDate"), function () { this.style.display = ""; });
        jQuery.each($("#dStart"), function () { this.style.display = ""; });
        jQuery.each($("#dEnd"), function () { this.style.display = "none"; });
    }
    else {
        //종료일 선택이 보이게
        jQuery.each($("#btStarDate"), function () { this.style.display = ""; });
        jQuery.each($("#btEndDate"), function () { this.style.display = "none"; });
        jQuery.each($("#dStart"), function () { this.style.display = "none"; });
        jQuery.each($("#dEnd"), function () { this.style.display = ""; });
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : [닫기]
**********************************************************************************************/
CalendarDual.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 년/월 드롭다운
***************************************************/
CalendarDual.LoadStart = function (arg) {

    var tmpMonth = $("#ddlStartMonth").val();

    tmpMonth = (tmpMonth) < 10 ? "0" + tmpMonth : tmpMonth;

    StartCalMonth = $("#ddlStartYear").val() + "-" + tmpMonth;
    CalendarDual.getCalendar(StartCalMonth);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 년/월 드롭다운
***************************************************/
CalendarDual.LoadEnd = function (arg) {

    var tmpMonth = $("#ddlEndMonth").val();

    tmpMonth = (tmpMonth) < 10 ? "0" + tmpMonth : tmpMonth;

    EndCalMonth = $("#ddlEndYear").val() + "-" + tmpMonth;
    CalendarDual.getCalendar(EndCalMonth);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 확인
***************************************************/
CalendarDual.Confirm = function () {

    var arrStart = $("#selectStart").text().split('-');
    var arrEnd = $("#selectEnd").text().split('-');

    var hdYear1 = arrStart[0];
    var hdMonth1 = parseInt(arrStart[1]);
    var hdDay1 = parseInt(arrStart[2]);

    var hdYear2 = arrEnd[0];
    var hdMonth2 = parseInt(arrEnd[1]);
    var hdDay2 = parseInt(arrEnd[2]);

    var TstartDate = new Date(hdYear1, hdMonth1 - 1, hdDay1); //시작일
    var TendDate = new Date(hdYear2, hdMonth2 - 1, hdDay2); //종료일

    var holiYN = "N";   //휴일 플래그
    var holyCnt = 0;    //총 휴일 수            
    var yoil = "";      //요일

    //날짜 기간 구하기
    var interval = TendDate - TstartDate;
    var day = 1000 * 60 * 60 * 24;
    var oDuration = parseInt(interval / day) + 1;

    //날짜 기간 동안 휴일 수 체크         
    for (var i = 0; i < oDuration; i++) {
        //요일 구하기
        yoil = getDayOfWeek(TstartDate);
        //공휴일인지 체크
        holiYN = AjaxGetToDayByHolyS(CompanyID, TstartDate.getFullYear(), (TstartDate.getMonth() + 1), TstartDate.getDate());

        //주말이 아닌 공휴일일때 총 휴일 수 카운팅
        if (holiYN == "Y" && yoil != "토" && yoil != "일") {
            holyCnt += 1;
        }

        //주말일때 총 휴일 수 카운팅
        if (yoil == "토" || yoil == "일") {
            holyCnt += 1;
        }

        //1일 더하기
        TstartDate.setDate(TstartDate.getDate() + 1);
    }

    //시작일 원래대로
    TstartDate = new Date(hdYear1, hdMonth1 - 1, hdDay1);

    var startHalf = 0;
    var endHalf = 0;

    startHalf = $("#ddlStartHalf").val();
    endHalf = $("#ddlEndHalf").val();

    hdMonth1 = (hdMonth1) < 10 ? "0" + hdMonth1 : hdMonth1;
    hdDay1 = (hdDay1) < 10 ? "0" + hdDay1 : hdDay1;
    hdMonth2 = (hdMonth2) < 10 ? "0" + hdMonth2 : hdMonth2;
    hdDay2 = (hdDay2) < 10 ? "0" + hdDay2 : hdDay2;

    var selectDate1 = hdYear1 + "-" + hdMonth1 + "-" + hdDay1;
    var selectDate2 = hdYear2 + "-" + hdMonth2 + "-" + hdDay2;

    if (selectDate1 == selectDate2) {
        if (startHalf != "0" && endHalf != "0") {
            parent.action_popup.alert("휴가기간은 0일이 될 수 없습니다.");
            return;
        }
    }

    if (selectDate1 != selectDate2) {
        if (startHalf == "1" || endHalf == "2") {
            parent.action_popup.alert("휴가기간은 연속된 날짜만 등록 가능합니다.");
            return;
        }
    }

    var tmp_Date1 = selectDate1.replaceText("-", "");
    var tmp_Date2 = selectDate2.replaceText("-", "");

    if (tmp_Date1 > tmp_Date2) {
        parent.action_popup.alert("시작일자가 종료일자보다 작아야 합니다.");
        return;
    }

    parent.ifrmProcessing.GetDateUseDay(hdYear1, hdMonth1, hdDay1, hdYear2, hdMonth2, hdDay2, startHalf, endHalf, (oDuration - holyCnt));

    CalendarDual.Close();

}

//요일 구하기
function getDayOfWeek(now) {
    var day = now.getDay(); //일요일=0,월요일=1,...,토요일=6
    var week = new Array('일', '월', '화', '수', '목', '금', '토');
    return week[day];
}