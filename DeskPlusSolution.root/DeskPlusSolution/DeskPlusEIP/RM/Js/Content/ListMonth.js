ListMonth = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-02
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

        // 처리내역(미승인/승인/거부)
        $('#cbxUsable').click(function (e) {

            var tmpUsable = [];
            $("[id*=cbxUsable] input:checked").each(function () {
                tmpUsable.push($(this).val());
            });

            SearchUsable = tmpUsable;
            ListMonth.AddModUsable(tmpUsable);

        });

        var tmpTime = GetTime($("#timeList").text());
        $("#timeList").html(tmpTime);

        //데이터 로딩
        ListMonth.GetList(TmpDate);
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
작성일자 : 2012-03-02
작성목적 : 포틀릿 Context Menu
***************************************************/
ListMonth.ContextMenuWork = function (id, action, el, pos) {
    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                ListMonth.ModUsable("1", tmpID[1]);
                break;
            }
        case "unaccept": //거부 처리
            {
                TmpReID = tmpID[1];
                ListMonth.Reject();
                //ListMonth.ModUsable("2", tmpID[1]);
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
작성일자 : 2012-03-02
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListMonth.GetList = function (_date) {
    var arrDate = _date.split('-');

    var url = "/DeskPlusEIP/RM/Ws/Content/ListMonth.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'year': arrDate[0],
        'month': arrDate[1],
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchUsable': SearchUsable
    };

    var obj = eval("ListMonth.SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-02
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListMonth.SuccessList = function (data, status) {
    $('#phListMonth').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTListMonth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListMonth').processTemplate(data.d);

    setTimeout(function () {
        ListMonth.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-02
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
    $(".callContextAMenu").contextMenu({ menu: 'aMenu', leftButton: true }, function (action, el, pos) {
        ListMonth.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextBMenu").contextMenu({ menu: 'bMenu', leftButton: true }, function (action, el, pos) {
        ListMonth.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextCMenu").contextMenu({ menu: 'cMenu', leftButton: true }, function (action, el, pos) {
        ListMonth.ContextMenuWork(el[0].id, action, el, pos);
    });
}

function addMonth(tmpDate, v_month) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = 1;
    var addDate = new Date(yyyy, eval(mm + '+' + v_month), dd);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);

    $("#timeList").empty();
    //$("#timeList").html(yyyy + "." + mm);
    var tmpTime = GetTime(yyyy + "-" + mm);
    $("#timeList").html(tmpTime);

    return yyyy + "-" + mm;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-02
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListMonth.PvMonth = function () {
    TmpDate = addMonth(TmpDate, -1);
    ListMonth.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-02
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListMonth.NxMonth = function () {
    TmpDate = addMonth(TmpDate, 1);
    ListMonth.GetList(TmpDate);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 예약 보기
**********************************************************************************************/
ListMonth.View = function (_reservationID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'month',
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
ListMonth.ModUsable = function (_arg, _reservationID) {
    ParamArray = {
        'usable': _arg,
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessModUsable");
    var url = "/DeskPlusEIP/RM/Ws/Content/ListMonth.asmx/ModUsable";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListMonth.SuccessModUsable = function (data, status) {

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

        ListMonth.GetList(TmpDate);
    }
}

/******************************************
작성자: famo23
작성일자: 2012-03-05
작성목적: 자원 예약
****************************************/
ListMonth.Add = function (_tmpDate) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': _tmpDate,
        //'searchField': SearchField,
        //'searchValue': SearchValue,
        'tagGubun': 'month',
        'groupGubun': '1'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2017-02-28
작성목적 : 거부 사유 입력창 오픈
***************************************************/
ListMonth.Reject = function () {

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
작성일자 : 2017-02-28
작성목적 : 거부 사유 입력후 
***************************************************/
function AddReject() {

    ListMonth.ModUsable("2", TmpReID);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-09
작성목적 : 범례 상태값 설정하기
**********************************************************************************************/
ListMonth.AddModUsable = function (_usable) {

    var url = "/DeskPlusEIP/RM/Ws/Content/ListMonth.asmx/AddModUsable";

    ParamArray = {
        'usable': _usable
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessAddModUsable");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-09
작성목적 : 범례 상태값 설정하기 (웹서비스 값 받기)
**********************************************************************************************/
ListMonth.SuccessAddModUsable = function (data, status) {
    if (data.d == true) {
        ListMonth.GetList(TmpDate);
    }
}

