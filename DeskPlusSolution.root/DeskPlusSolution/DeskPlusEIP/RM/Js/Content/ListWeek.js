ListWeek = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //이전날짜
        $('#pvWeek').click(function (e) {
            e.preventDefault();
            ListWeek.PvWeek();
        });

        //이후날짜
        $('#nxWeek').click(function (e) {
            e.preventDefault();
            ListWeek.NxWeek();
        });

        // 처리내역(미승인/승인/거부)
        $('#cbxUsable').click(function (e) {

            var tmpUsable = [];
            $("[id*=cbxUsable] input:checked").each(function () {
                tmpUsable.push($(this).val());
            });

            SearchUsable = tmpUsable;
            ListWeek.AddModUsable(tmpUsable);

        });

        var tmpTime = GetTime($("#timeList").text());
        $("#timeList").html(tmpTime);
        var tmpETime = GetTime($("#EtimeList").text());
        $("#EtimeList").html(tmpETime);

        //데이터 로딩
        ListWeek.GetList(TmpDate);
    });
});

/**************************************************
작성자: famo23
작성일:2012-05-15
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
작성일자 : 2012-02-24
작성목적 : 포틀릿 Context Menu
***************************************************/
ListWeek.ContextMenuWork = function (id, action, el, pos) {
    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                ListWeek.ModUsable("1", tmpID[1]);
                break;
            }
        case "unaccept": //거부 처리
            {
                TmpReID = tmpID[1];
                ListWeek.Reject();
                //ListWeek.ModUsable("2", tmpID[1]);
                break;
            }
        case "view": // 보기
            {
                ListWeek.View(tmpID[1]);
                break;
            }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListWeek.GetList = function (_date) {
    var url = "/DeskPlusEIP/RM/Ws/Content/ListWeek.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'tmpdate': _date,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchUsable': SearchUsable
    };

    var obj = eval("ListWeek.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListWeek.SuccessList = function (data, status) {
    $('#phListWeek').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTListWeek.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListWeek').processTemplate(data.d);

    setTimeout(function () {
        ListWeek.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
ListWeek.Resource = function () {
    $("#j_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#j_time")[0].innerHTML = R_Time;
    $("#j_title")[0].innerHTML = R_Subject;
    $("#j_subscriber")[0].innerHTML = R_Subscriber;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    //Context Menu 설정 JTemplate 을 사용할 경우 loading 후 연결 ByFreeman 20120223
    $(".callContextAMenu").contextMenu({ menu: 'aMenu', leftButton: true }, function (action, el, pos) {
        ListWeek.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextBMenu").contextMenu({ menu: 'bMenu', leftButton: true }, function (action, el, pos) {
        ListWeek.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextCMenu").contextMenu({ menu: 'cMenu', leftButton: true }, function (action, el, pos) {
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
    //$("#timeList").html(yyyy + "." + mm + "." + dd);
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
    //$("#EtimeList").html(yyyy + "." + mm + "." + dd);
    var tmpTime = GetTime(yyyy + "-" + mm + "-" + dd);
    $("#EtimeList").html(tmpTime);

    return yyyy + "-" + mm + "-" + dd;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListWeek.PvWeek = function () {
    TmpDate = addDay(TmpDate, -7);

    ListWeek.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListWeek.NxWeek = function () {
    TmpDate = addDay(TmpDate, 7);

    ListWeek.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 예약 보기
**********************************************************************************************/
ListWeek.View = function (_reservationID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'week',
        'reservationID': _reservationID,
        'groupGubun': '1'
    };

    CallEncrypt(CallGetParam());
    parent.location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 호출)
**********************************************************************************************/
ListWeek.ModUsable = function (_arg, _reservationID) {
    ParamArray = {
        'usable': _arg,
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessModUsable");
    var url = "/DeskPlusEIP/RM/Ws/Content/ListWeek.asmx/ModUsable";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListWeek.SuccessModUsable = function (data, status) {

    var rtnVal = data.d;

    if (rtnVal < 0) {
        alert(R_ProcessingError);     //에러
    }
    else {

        if (rtnVal == 0) {
            parent.parent.frmLeft.mCnt.innerText = "";
            parent.parent.frmLeft.mSubCnt.innerText = "";
        }
        else {
            parent.parent.frmLeft.mCnt.innerText = "(" + rtnVal + ")";
            parent.parent.frmLeft.mSubCnt.innerText = "(" + rtnVal + ")";            
        }

        ListWeek.GetList(TmpDate);
    }
}

/******************************************
작성자: famo23
작성일자: 2012-03-06
작성목적: 자원 예약
****************************************/
ListWeek.Add = function (_tmpDate, _time, _listID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': _tmpDate,
        'tmpTime': _time,
        'listID': _listID,
        //'searchField': SearchField,
        //'searchValue': SearchValue,
        'tagGubun': 'week',
        'groupGubun': '1'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-02
작성목적 : 거부 사유 입력창 오픈
***************************************************/
ListWeek.Reject = function () {

    ParamArray = {
        'callBackEvent': "AddReject",
        'reservationID': TmpReID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/RM/Content/Reject/AddReject.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}


/**************************************************
작성자   : famo23
작성일자 : 2017-03-02
작성목적 : 거부 사유 입력후 
***************************************************/
function AddReject() {

    ListWeek.ModUsable("2", TmpReID);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-09
작성목적 : 범례 상태값 설정하기
**********************************************************************************************/
ListWeek.AddModUsable = function (_usable) {

    var url = "/DeskPlusEIP/RM/Ws/Content/ListWeek.asmx/AddModUsable";

    ParamArray = {
        'usable': _usable
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessAddModUsable");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-09
작성목적 : 범례 상태값 설정하기 (웹서비스 값 받기)
**********************************************************************************************/
ListWeek.SuccessAddModUsable = function (data, status) {
    if (data.d == true) {
        ListWeek.GetList(TmpDate);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 호출)
**********************************************************************************************/
ListWeek.Send = function (_reservationID) {
    if (window.confirm(R_ConfirmSend)) {
        ParamArray = {
            'reservationID': _reservationID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListWeek.SuccessSend");
        var url = "/DeskPlusEIP/RM/Ws/Content/ListWeek.asmx/SendRequest";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 값 받기)
**********************************************************************************************/
ListWeek.SuccessSend = function (data, status) {

    if (data.d) {
        alert(R_SendAlarm);
    }
    else {
        alert(R_ProcessingError);     //에러
    }
}