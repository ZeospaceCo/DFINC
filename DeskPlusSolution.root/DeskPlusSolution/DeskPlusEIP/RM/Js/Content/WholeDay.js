WholeDay = {}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-20
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {
        //이전날짜
        $('#pvDay').click(function (e) {
            e.preventDefault();
            WholeDay.PvDay();
        });

        //이후날짜
        $('#nxDay').click(function (e) {
            e.preventDefault();
            WholeDay.NxDay();
        });

        // 처리내역(미승인/승인/거부)
        $('#cbxUsable').click(function (e) {

            var tmpUsable = [];
            $("[id*=cbxUsable] input:checked").each(function () {
                tmpUsable.push($(this).val());
            });

            SearchUsable = tmpUsable;
            WholeDay.AddModUsable(tmpUsable);

        });

        $("#timeList").empty();
        GetTime(TmpDate);

        //같은 자원에 한에서 rowspan
        //RowSpan();
        WholeDay.Resource();
    });
});

/**************************************************
작성자   : shine
작성일:2012-05-15
작성목적 : 자원명이 같을 경우 rowspan
***************************************************/
function RowSpan() {
    var currName;
    var preName;
    var rowspan = 1;
    var firstTd;

    $(".tdRowspan").each(function (i) {
        currName = $(this).html();
        if (preName == null || preName != currName) {
            if ($(firstTd) != null) {
                $(firstTd).attr("rowspan", rowspan);    //첫번째 td rowspan값 저장
            }
            preName = $(this).html();   //이전 TD값
            firstTd = $(this);  //첫번째 TD 저장
            rowspan = 1;    //rowspan 초기화
        }
        else {
            if (currName == preName) {
                //같은 TD가 있으면
                rowspan = rowspan + 1;  //rowspan값 증가
                $(this).remove();   //해당 TD삭제
                $(firstTd).attr("rowspan", rowspan);
            }
        }
    });
}

/**************************************************
작성자   : shine
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
작성자   : shine
작성일:2012-02-23
작성목적 : 포틀릿 Context Menu
***************************************************/
WholeDay.ContextMenuWork = function (id, action, el, pos) {
    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                WholeDay.ModUsable("1", tmpID[1]);
                break;
            }
        case "unaccept": //거부 처리
            {
                TmpReID = tmpID[1];
                WholeDay.Reject();
                //WholeDay.ModUsable("2", tmpID[1]);
                break;
            }
        case "view": // 보기
            {
                WholeDay.View(tmpID[1]);
                break;
            }
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-20
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
WholeDay.Resource = function () {

    //Context Menu 설정 JTemplate 을 사용할 경우 loading 후 연결 ByFreeman 20120223
    $(".callContextAMenu").contextMenu({ menu: 'aMenu', leftButton: true }, function (action, el, pos) {
        WholeDay.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextBMenu").contextMenu({ menu: 'bMenu', leftButton: true }, function (action, el, pos) {
        WholeDay.ContextMenuWork(el[0].id, action, el, pos);
    });

    $(".callContextCMenu").contextMenu({ menu: 'cMenu', leftButton: true }, function (action, el, pos) {
        WholeDay.ContextMenuWork(el[0].id, action, el, pos);
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
작성자   : shine
작성일자 : 2012-02-21
작성목적 : 이전 날짜 가기
**********************************************************************************************/
WholeDay.PvDay = function () {
    TmpDate = addDay(TmpDate, -1);

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location.href = "WholeDay.aspx?p=" + GetEncryptValue;
    //__doPostBack('lbtnPre', '');
    //WholeDay.GetList(TmpDate);
    //RowSpan();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-21
작성목적 : 이후 날짜 가기
**********************************************************************************************/
WholeDay.NxDay = function () {
    TmpDate = addDay(TmpDate, 1);

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location.href = "WholeDay.aspx?p=" + GetEncryptValue;

    //__doPostBack('lbtnNext', '');
    //WholeDay.GetList(TmpDate);
    //RowSpan();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-22
작성목적 : 예약 보기
**********************************************************************************************/
WholeDay.View = function (_reservationID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'day',
        'reservationID': _reservationID,
        'groupGubun': '2'
    };

    CallEncrypt(CallGetParam());
    parent.location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 호출)
**********************************************************************************************/
WholeDay.ModUsable = function (_arg, _reservationID) {
    ParamArray = {
        'usable': _arg,
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WholeDay.SuccessModUsable");
    var url = "/DeskPlusEIP/RM/Ws/Content/WholeDay.asmx/ModUsable";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-24
작성목적 : 승인 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
WholeDay.SuccessModUsable = function (data, status) {

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

        ParamArray = {
            'categoryID': CategoryID,
            'tmpDate': TmpDate,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());
        location.href = "/DeskPlusEIP/RM/Content/WholeDay.aspx?p=" + GetEncryptValue;
    }    
}

/******************************************
작성자   : shine
작성일자: 2012-03-06
작성목적: 자원 예약
****************************************/
WholeDay.Add = function (_time, _listID) {
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'tmpTime': _time,
        'listID': _listID,
        //'searchField': SearchField,
        //'searchValue': SearchValue,
        'tagGubun': 'day',
        'groupGubun': '2'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "AddN.aspx?p=" + GetEncryptValue;
}

//자원상세정보
ViewListInfo = function (_listid) {
    ParamArray = { 'listid': _listid };
    CallEncrypt(CallGetParam());

    var url = "ResourceInfo.aspx?p=" + GetEncryptValue;
    window.open(url, "RM", "width=460px,height=470px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-02
작성목적 : 거부 사유 입력창 오픈
***************************************************/
WholeDay.Reject = function () {

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

    WholeDay.ModUsable("2", TmpReID);
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-10
작성목적 : 범례 상태값 설정하기
**********************************************************************************************/
WholeDay.AddModUsable = function (_usable) {

    var url = "/DeskPlusEIP/RM/Ws/Content/WholeDay.asmx/AddModUsable";

    ParamArray = {
        'usable': _usable
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WholeDay.SuccessAddModUsable");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-10
작성목적 : 범례 상태값 설정하기 (웹서비스 값 받기)
**********************************************************************************************/
WholeDay.SuccessAddModUsable = function (data, status) {
    if (data.d == true) {

        //파라미터값 배열에 담기
        ParamArray = {
            'categoryID': CategoryID,
            'tmpDate': TmpDate,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam());
        location.href = "WholeDay.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 호출)
**********************************************************************************************/
WholeDay.Send = function (_listID) {
    if (window.confirm(R_ConfirmSend)) {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': _listID,
            'tmpDate': TmpDate,
            'searchField': SearchField,
            'searchValue': SearchValue
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("WholeDay.SuccessSend");
        var url = "/DeskPlusEIP/RM/Ws/Content/WholeDay.asmx/SendRequest";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-03-14
작성목적 : 승인요청 버튼 선택시 (웹서비스 값 받기)
**********************************************************************************************/
WholeDay.SuccessSend = function (data, status) {

    if (data.d) {
        alert(R_SendAlarm);
    }
    else {
        alert(R_ProcessingError);     //에러
    }
}