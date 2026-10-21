ListMonth = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {

        //이전달
        $('#pvMonth').click(function (e) {
            e.preventDefault();
            ListMonth.PvMonth();
        });

        //이후달
        $('#nxMonth').click(function (e) {
            e.preventDefault();
            ListMonth.NxMonth();
        });

        var tmpTime = GetTime($("#timeList").text());
        $("#timeList").html(tmpTime);

        //데이터 로딩
        ListMonth.GetList(TmpDate);

    });
});


/**************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 날짜 바인딩
***************************************************/
function GetTime(_date) {
    var arrDate = _date.split('-');
    var rtnVal = "";

    switch (PreLang) {
        case "ko":
            //"yy-mm";
            rtnVal = arrDate[0] + "-" + arrDate[1];
            break;
        case "en":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm";
            rtnVal = arrDate[0] + "/" + arrDate[1];
            break;
        case "fr":
            //"mm/yy";
            rtnVal = arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm";
            rtnVal = arrDate[0] + "/" + arrDate[1];
            break;
        case "zh-tw":
            //"yy-mm";
            rtnVal = arrDate[0] + "-" + arrDate[1];
            break;
    }
    return rtnVal;
}


/**************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 포틀릿 Context Menu
***************************************************/
ListMonth.ContextMenuWork = function (id, action, el, pos) {

    var tmpID = id.split("_");

    switch (action) {
        case "accept": // 승인 처리
            {
                ListMonth.ModVisitType("1", tmpID[1], tmpID[2]);
                break;
            }
        case "unaccept": //취소 처리
            {
                ListMonth.ModVisitType("2", tmpID[1], tmpID[2]);
                break;
            }
        case "view": // 보기
            {
                ListMonth.View(tmpID[1]);
                break;
            }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListMonth.GetList = function (_date) {
    var arrDate = _date.split('-');

    var url = "/DeskPlusEIP/Visit/Ws/Content/ListMonth.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {        
        'year': arrDate[0],
        'month': arrDate[1],
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("ListMonth.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListMonth.SuccessList = function (data, status) {

    $('#phListMonth').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTListMonth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListMonth').processTemplate(data.d);

    setTimeout(function () {
        ListMonth.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
ListMonth.Resource = function () {

    $("#j_sun")[0].innerHTML = R_Sunday;
    $("#j_mon")[0].innerHTML = R_Monday;
    $("#j_tue")[0].innerHTML = R_Tuesday;
    $("#j_wed")[0].innerHTML = R_Wednesday;
    $("#j_thu")[0].innerHTML = R_Thursday;
    $("#j_fri")[0].innerHTML = R_Friday;
    $("#j_sat")[0].innerHTML = R_Saturday;

    //Context Menu 설정 JTemplate 을 사용할 경우 loading 후 연결 ByFreeman 20120223
    $(".callContextMenu").contextMenu({ menu: 'basicMenu', leftButton: true }, function (action, el, pos) {
        ListMonth.ContextMenuWork(el[0].id, action, el, pos);
    });
}

function addMonth(tmpDate, v_month) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = arrDate[2];
    var addDate = new Date(yyyy, eval(mm + '+' + v_month), dd);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    dd = addDate.getDate() < 10 ? "0" + addDate.getDate() : addDate.getDate();

    $("#timeList").empty();
    //$("#timeList").html(yyyy + "." + mm);
    var tmpTime = GetTime(yyyy + "-" + mm);
    $("#timeList").html(tmpTime);

    return yyyy + "-" + mm + "-" + dd;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListMonth.PvMonth = function () {
    TmpDate = addMonth(TmpDate, -1);
    //ListMonth.GetList(TmpDate);
    ListMonth.GoPageLoad();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListMonth.NxMonth = function () {

    TmpDate = addMonth(TmpDate, 1);
    //ListMonth.GetList(TmpDate);
    ListMonth.GoPageLoad();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 예약 보기
**********************************************************************************************/
ListMonth.View = function (_reserveID) {

    ParamArray = {
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'month',
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
ListMonth.ModVisitType = function (_arg, _reserveID, _visitType) {
    if (_arg != _visitType) {

        ParamArray = {
            'visitType': _arg,
            'reserveID': _reserveID,
            'preLang': PreLang

        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListMonth.SuccessModVisitType");
        var url = "/DeskPlusEIP/Visit/Ws/Content/ListMonth.asmx/ModVisitType";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 방문 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListMonth.SuccessModVisitType = function (data, status) {

    if (data.d) {
        ListMonth.GetList(TmpDate);
    }
    else {
        alert(R_ProcessingError);
    }
}


/******************************************
작성자: famo23
작성일자: 2014-06-18
작성목적: 자원 예약
****************************************/
ListMonth.Add = function (_tmpDate) {

    ParamArray = {
        'tmpDate': _tmpDate,
        'tagGubun': 'month'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "AddN.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListMonth.GoPageLoad = function () {

    ParamArray = {
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'month'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "ListMain.aspx?p=" + GetEncryptValue;
}


