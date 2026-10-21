ListDay = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-05
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListDay.GetCount(TmpDate);
            //ListDay.GetPageList(TmpDate);
            ListDay.GoPageLoad();
        }
    }

    $(document).ready(function () {

        RecordPerPage = $("#ddlListCount").val();

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListDay.ModSetting();
        });

        $("#timeList").empty();
        GetTime(TmpDate);

        //데이터 로딩        
        ListDay.LoadData(TmpDate);

    });
});

/**************************************************
작성자: famo23
작성일자 : 2014-02-05
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
작성자: famo23
작성일자 : 2014-02-05
작성목적 : 포틀릿 Context Menu
***************************************************/
ListDay.ContextMenuWork = function (id, action, el, pos) {

    var tmpID = id.split("_");

    switch (action) {
        case "accept": //승인 처리
            {
                ListDay.ModVisitType("1", tmpID[1], tmpID[2]);
                break;
            }
        case "unaccept": //취소 처리
            {
                ListDay.ModVisitType("2", tmpID[1], tmpID[2]);
                break;
            }       
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 초기화
***************************************************/
ListDay.LoadData = function (_date) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        ListDay.GetPageList(_date);
    }, 100);
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListDay.GetPageList = function (_date) {

    var url = "/DeskPlusEIP/Visit/Ws/Content/ListDay.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {        
        'tmpdate': _date,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ListDay.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 리스트 가져오기 (웹서비스 값 호출)
**********************************************************************************************/
ListDay.SuccessPageList = function (data, status) {

    $('#phListDay').setTemplateURL('/DeskPlusEIP/Visit/Jt/Content/JTListDay.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListDay').processTemplate(data.d);

    setTimeout(function () {
        ListDay.Resource();
    }, 100);
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 메뉴 다국어 처리
**********************************************************************************************/
ListDay.Resource = function () {

    $("#r_row")[0].innerHTML = R_ROW;
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
        ListDay.ContextMenuWork(el[0].id, action, el, pos);
    });
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListDay.GetCount = function (_date) {
    var url = "/DeskPlusEIP/Visit/Ws/Content/ListDay.asmx/GetListCount";
    ParamArray = {
        'tmpdate': _date,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
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
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 이전 날짜 가기
**********************************************************************************************/
ListDay.PvDay = function () {
    TmpDate = addDay(TmpDate, -1);

    CurrentPage = 1; //1페이지로 이동처리     

    //ListDay.GetCount(TmpDate);
    //ListDay.GetPageList(TmpDate);
    ListDay.GoPageLoad();
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 이후 날짜 가기
**********************************************************************************************/
ListDay.NxDay = function () {
    TmpDate = addDay(TmpDate, 1);

    CurrentPage = 1; //1페이지로 이동처리     

    //ListDay.GetCount(TmpDate);
    //ListDay.GetPageList(TmpDate);
    ListDay.GoPageLoad();
}


/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-05
작성목적 : 예약 보기
**********************************************************************************************/
ListDay.View = function (_reserveID) {

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'day',
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
ListDay.ModVisitType = function (_arg, _reserveID, _visitType) {
    if (_arg != _visitType) {
    
            ParamArray = {
            'visitType': _arg,
            'reserveID': _reserveID,
            'preLang': PreLang
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListDay.SuccessModVisitType");
        var url = "/DeskPlusEIP/Visit/Ws/Content/ListDay.asmx/ModVisitType";

        AjaxService(url, data, obj);

    }
}

/**********************************************************************************************
작성자: famo23
작성일자 : 2014-02-06
작성목적 : 방문 / 거부 처리 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessModVisitType = function (data, status) {

    if (data.d) {
        //ListDay.GetCount(TmpDate);
        //ListDay.GetPageList(TmpDate);
        ListDay.GoPageLoad();
    }
    else {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListDay.ModSetting = function () {

    var url = "/DeskPlusEIP/Visit/Ws/Content/ListDay.asmx/ModSetting";
    ParamArray = {
        'pageSize': $("#ddlListCount").val(),        
        'types': 'D'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessModSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-06
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListDay.SuccessModSetting = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리     
        RecordPerPage = $("#ddlListCount").val();

        //ListDay.GetCount(TmpDate);
        //ListDay.GetPageList(TmpDate);
        ListDay.GoPageLoad();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListDay.GoPageLoad = function () {

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'day'
    };

    CallEncrypt(CallGetParam());

    parent.location.href = "ListMain.aspx?p=" + GetEncryptValue;  
}


