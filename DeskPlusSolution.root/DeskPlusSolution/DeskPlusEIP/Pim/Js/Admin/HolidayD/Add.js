Add = {}

$(function () {
    $(document).ready(function () {

        //저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //반복구분 변경시
        $("#rblHoliDayKind").change(function (e) {
            Add.HolidayKind();
        });

        //달력
        $("#ctbHoliDayN").datepick($.datepicker.regional[PreLang]);
        $("#ctbHoliDayN").keydown(function (e) { return false; });

        //달력
        $("#ctbHoliDayY").datepick({ dateFormat: "mm-dd" });
        $("#ctbHoliDayY").keydown(function (e) { return false; });

        $("#ctbHoliDayN").hide();

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiName').attr("disabled", true);
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-01-31
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMultiName").val(multiNames);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-01-13
작성목적 : 반복구분 변경시
**********************************************************************************************/
Add.HolidayKind = function () {

    var sField = $("input[name='rblHoliDayKind']:checked").val();

    if (sField == "0") {
        $("#ctbHoliDayN").hide();
        $("#ctbHoliDayY").show();
    }
    else {
        $("#ctbHoliDayN").show();
        $("#ctbHoliDayY").hide();
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-01-20
작성목적 : 저장체크
***************************************************/
Add.Save = function () {

    //휴일명
    if ($("#ctbTitle").val() == "") {
        alert(R_Holiday_Error1);
        return;
    }

    var sField = $("input[name='rblHoliDayKind']:checked").val();

    if (sField == "0") {
        //일자
        if ($("#ctbHoliDayY").val() == "") {
            alert(R_Holiday_Error2);
            return;
        }
    }
    else {
        //일자
        if ($("#ctbHoliDayN").val() == "") {
            alert(R_Holiday_Error2);
            return;
        }
    }

    var SolarLunar = $("input[name='rblSolarLunar']:checked").val();
    var HoliDay = $("#ctbHoliDay").val();

    var url = "/DeskPlusEIP/Pim/Ws/Admin/HolidayD/Add.asmx/GetCheckCount";

    ParamArray = {
        'solarLunar': SolarLunar,
        'holiDay': HoliDay
    };

    var obj = eval("Add.SuccessSave");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원  
작성일자 : 2017-01-20
작성목적 : 성공시
***************************************************/
Add.SuccessSave = function (data, status) {

    if (data.d != "0") {
        alert(R_Holiday_Error4);
        return;
    }
    else {
        //저장
        $('#ctbMultiName').attr("disabled", false);

        __doPostBack('lbtnAdd', 'Save');
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-01-23
작성목적 : 취소 
***************************************************/
Add.Cancel = function () {

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}