ListDay = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //이전날짜
        $('#pvDay').click(function (e) {
            e.preventDefault();
            ListDay.PvDay();
        });

        //이후날짜
        $('#nxDay').click(function (e) {
            e.preventDefault();
            ListDay.NxDay();
        });

        // 처리내역(미승인/승인/거부)
        $('#cbxUsable').click(function (e) {

            var tmpUsable = [];
            $("[id*=cbxUsable] input:checked").each(function () {
                tmpUsable.push($(this).val());
            });

            SearchUsable = tmpUsable;
            ListDay.AddModUsable(tmpUsable);

        });        

        $("#timeList").empty();
        GetTime(TmpDate);
        
        //데이터 로딩
        ListDay.GetList(TmpDate);
    });
});

/**************************************************
작성자: famo23
작성일:2012-05-15
작성목적 : 날짜 바인딩
***************************************************/
function GetTime(_date) {
    var arrDate = _date.split('-');

    switch (PreLang) {
        case "ko":
            //"yy-mm-dd";
            $("#timeList").html(arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2]);
            break;
        case "en":
            //"mm/dd/yy";
            $("#timeList").html(arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0]);
            break;
        case "en-gb":
            //"dd/mm/yy";
            $("#timeList").html(arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0]);
            break;
        case "ja":
            //"yy/mm/dd";
            $("#timeList").html(arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2]);
            break;
        case "fr":
            //"dd/mm/yy";
            $("#timeList").html(arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0]);
            break;
        case "zh-cn":
            //"yy/mm/dd";
            $("#timeList").html(arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2]);
            break;
        case "zh-tw":
            //"yy-mm-dd";
            $("#timeList").html(arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2]);
            break;
    }
}

/**************************************************
작성자: freeman
작성일:2012-02-23
작성목적 : 포틀릿 Context Menu
***************************************************/
ListDay.ContextMenuWork = function (id, action, el, pos) {
    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                ListDay.ModUsable("1", tmpID[1]);
                break;
            }
        case "unaccept": //거부 처리
            {
                TmpReID = tmpID[1];
                ListDay.Reject();
                //ListDay.ModUsable("2", tmpID[1]);
                break;
            }
        case "view": // 보기
            {
                ListDay.View(tmpID[1]);
                break;
            }
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListDay.GetList = function (_date) {
    //alert(_date);

    var url = "/DeskPlusEIP/RM/Ws/Content/ListDay.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'tmpdate': _date,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchUsable': SearchUsable
    };

    var obj = eval("ListDay.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListDay.SuccessList = function (data, status) {
    $('#phListDay').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTListDay.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListDay').processTemplate(data.d);

    setTimeout(function () {
        ListDay.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
ListDay.Resource = function () {
    $("#j_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#j_time")[0].innerHTML = R_Time;
    $("#j_title")[0].innerHTML = R_Subject;
    $("#j_subscriber")[0].innerHTML = R_Subscriber;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    //Context Menu 설정 JTemplate 을 사용할 경우 loading 후 연결 ByFreeman 20120223
    $(".callContextAMenu").contextMenu({ menu: 'aMenu', leftButton: true }, function (action, el, pos) {
        ListDay.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextBMenu").contextMenu({ menu: 'bMenu', leftButton: true }, function (action, el, pos) {
        ListDay.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextCMenu").contextMenu({ menu: 'cMenu', leftButton: true }, function (action, el, pos) {
        ListDay.ContextMenuWork(el[0].id, action, el, pos);
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

    $("#timeList").empty();
    //$("#timeList").html(yyyy + "." + mm + "." + dd);
    GetTime(yyyy + "-" + mm + "-" + dd);

    return yyyy + "-" + mm + "-" + dd;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-21
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListDay.PvDay = function () {
    TmpDate = addDay(TmpDate, -1);

    ListDay.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-21
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListDay.NxDay = function () {
    TmpDate = addDay(TmpDate, 1);

    ListDay.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 예약 보기
**********************************************************************************************/
ListDay.View = function (_reservationID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'day',
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
ListDay.ModUsable = function (_arg, _reservationID) {
    ParamArray = {
        'usable': _arg,
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessModUsable");
    var url = "/DeskPlusEIP/RM/Ws/Content/ListDay.asmx/ModUsable";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessModUsable = function (data, status) {

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

        ListDay.GetList(TmpDate);
    }
}

/******************************************
작성자: famo23
작성일자: 2012-03-06
작성목적: 자원 예약
****************************************/
ListDay.Add = function (_time, _listID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'tmpTime': _time,
        'listID': _listID,
        //'searchField': SearchField,
        //'searchValue': SearchValue,
        'tagGubun': 'day',
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
ListDay.Reject = function () {

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

    ListDay.ModUsable("2", TmpReID);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 범례 상태값 설정하기
**********************************************************************************************/
ListDay.AddModUsable = function (_usable) {

    var url = "/DeskPlusEIP/RM/Ws/Content/ListDay.asmx/AddModUsable";

    ParamArray = {        
        'usable': _usable
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessAddModUsable");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 범례 상태값 설정하기 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessAddModUsable = function (data, status) {
    if (data.d == true) {
        ListDay.GetList(TmpDate);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 호출)
**********************************************************************************************/
ListDay.Send = function (_reservationID) {
    if (window.confirm(R_ConfirmSend)) {
        ParamArray = {
            'reservationID': _reservationID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListDay.SuccessSend");
        var url = "/DeskPlusEIP/RM/Ws/Content/ListDay.asmx/SendRequest";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessSend = function (data, status) {

    if (data.d) {
        alert(R_SendAlarm);
    }
    else {
        alert(R_ProcessingError);     //에러
    }
}