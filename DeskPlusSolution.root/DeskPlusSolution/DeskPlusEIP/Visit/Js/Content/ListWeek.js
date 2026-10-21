ListWeek = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 초기화
**********************************************************************************************/

$(function () {    

    $(document).ready(function () {
          
        //이전날짜
        $('#pvWeek').click(function (e) {
            e.preventDefault();
            ListWeek.PvDay();
        });

        //이후날짜
        $('#nxWeek').click(function (e) {
            e.preventDefault();
            ListWeek.NxDay();
        });       

        var tmpTime = GetTime($("#timeList").text());
        $("#timeList").html(tmpTime);
        var tmpETime = GetTime($("#EtimeList").text());
        $("#EtimeList").html(tmpETime);

        //데이터 로딩        
        ListWeek.LoadData(TmpDate);

    });
});

/**************************************************
작성자: famo23
작성일:2014-02-06
작성목적 : 날짜 바인딩
***************************************************/
function GetTime(_date) {
    var arrDate = _date.split('-');
    var rtnVal = "";

    switch (PreLang) {
        case "ko":
            //"yy-mm-dd";
            rtnVal = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            break;
        case "en":
            //"mm/dd/yy";
            rtnVal = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"dd/mm/yy";
            rtnVal = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm/dd";
            rtnVal = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "fr":
            //"dd/mm/yy";
            rtnVal = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm/dd";
            rtnVal = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "zh-tw":
            //"yy-mm-dd";
            rtnVal = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            break;
    }
    return rtnVal;
}

/**************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 포틀릿 Context Menu
***************************************************/
ListWeek.ContextMenuWork = function (id, action, el, pos) {

    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                ListWeek.ModVisitType("1", tmpID[1], tmpID[2]);
                break;
            }
        case "unaccept": //취소 처리
            {
                ListWeek.ModVisitType("2", tmpID[1], tmpID[2]);
                break;
            }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 초기화
***************************************************/
ListWeek.LoadData = function (_date) {

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        ListWeek.GetPageList(_date);
    }, 100);
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListWeek.GetPageList = function (_date) {

    var url = "/DeskPlusEIP/Visit/Ws/Content/ListWeek.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'tmpdate': _date,    
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ListWeek.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListWeek.SuccessPageList = function (data, status) {

    $('#phListWeek').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTListWeek.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListWeek').processTemplate(data.d);

    setTimeout(function () {
        ListWeek.Resource();
    }, 100);
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
ListWeek.Resource = function () {

    $("#r_weekName")[0].innerHTML = R_WeekName;
    $("#r_visitType")[0].innerHTML = R_VisitType;
    $("#r_startDate")[0].innerHTML = R_StartDate;
    $("#r_endDate")[0].innerHTML = R_EndDate;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_writeUserName")[0].innerHTML = R_Subscriber;
    $("#r_writeDate")[0].innerHTML = R_WriteDate;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    //Context Menu 설정 JTemplate 을 사용할 경우 loading 후 연결 ByFreeman 20120223
    $(".callContextMenu").contextMenu({ menu: 'basicMenu', leftButton: true }, function (action, el, pos) {
        ListWeek.ContextMenuWork(el[0].id, action, el, pos);
    });
}


function addDay(tmpDate, v_day) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = arrDate[2];
    var addDate = new Date(yyyy, mm, eval(dd + '+' + v_day));
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    dd = addDate.getDate() < 10 ? "0" + addDate.getDate() : addDate.getDate();

    var tmp = yyyy + "-" + mm + "-" + dd;

    $("#timeList").empty();
    var tmpTime = GetTime(tmp);
    $("#timeList").html(tmpTime);

    addnxDay(tmp, 6);

    return yyyy + "-" + mm + "-" + dd;
}

function addnxDay(tmp, v_day) {

    var arrDate = tmp.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = arrDate[2];
    var addDate = new Date(yyyy, mm, eval(dd + '+' + v_day));
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    dd = addDate.getDate() < 10 ? "0" + addDate.getDate() : addDate.getDate();

    $("#EtimeList").empty();
    var tmpTime = GetTime(yyyy + "-" + mm + "-" + dd);
    $("#EtimeList").html(tmpTime);

    return yyyy + "-" + mm + "-" + dd;
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListWeek.PvDay = function () {
    TmpDate = addDay(TmpDate, -7);

    //ListWeek.GetPageList(TmpDate);
    ListWeek.GoPageLoad();
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListWeek.NxDay = function () {
    TmpDate = addDay(TmpDate, 7);

    //ListWeek.GetPageList(TmpDate);
    ListWeek.GoPageLoad();
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-05
작성목적 : 예약 보기
**********************************************************************************************/
ListWeek.View = function (_reserveID) {

    ParamArray = {
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'week',
        'reserveID': _reserveID
    };

    CallEncrypt(CallGetParam());
    parent.location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 방문 / 거부 처리 (웹서비스 호출)
**********************************************************************************************/
ListWeek.ModVisitType = function (_arg, _reserveID, _visitType) {
    if (_arg != _visitType) {

        ParamArray = {
            'visitType': _arg,
            'reserveID': _reserveID,
            'preLang': PreLang
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListWeek.SuccessModVisitType");
        var url = "/DeskPlusEIP/Visit/Ws/Content/ListWeek.asmx/ModVisitType";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 방문 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListWeek.SuccessModVisitType = function (data, status) {

    if (data.d) {
        ListWeek.GetPageList(TmpDate);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListWeek.GoPageLoad = function () {

    ParamArray = {
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'week'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "ListMain.aspx?p=" + GetEncryptValue;
}


