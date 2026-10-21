ListUse = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListUse.LoadData("");
            ListUse.GoPageLoad();
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

        // 승인불가
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            ListUse.Reject();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListUse.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListUse.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListUse.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $("#ddlListCount").val();
            ListUse.Search();
        });

        ListUse.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 초기화 하기
***************************************************/
ListUse.Init = function () {

    $("#startEndDate").hide(); //기간
    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    ListUse.LoadData("");
}

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 날짜 세팅
***************************************************/
ListUse.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 검색
***************************************************/
ListUse.Search = function () {
    var response = ListUse.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    //ListUse.LoadData("search");
    ListUse.GoPageLoad();
}

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 검색 필드 설정하기
***************************************************/
ListUse.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {

            var start_dates = getStartDate.dateCharRemove();
            var end_dates = getEndDate.dateCharRemove();

            if ((end_dates - start_dates) < 0) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 초기화
***************************************************/
ListUse.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadMDMData = function () {
        ListUse.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadMDMSearchData = function () {

        RecordPerPage = $("#ddlListCount").val();
        ListUse.GetCount(); //검색 총 카운트 가져오기
        ListUse.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListUse.LoadData("search");
            }
        }

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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadMDMSearchData();
        }
        else {
            loadMDMData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListUse.GetPageList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Auth/ListUse.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'activeMobie': '1',
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("ListUse.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListUse.SuccessPageList = function (data, status) {

    $('#phListUse').setTemplateURL('/DeskPlusEIP/MDM/Jt/Auth/JTListUse.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListUse').processTemplate(data.d);
    $('#loading').html("");

    ListUse.Resource();

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 전체 체크박스
**********************************************************************************************/
ListUse.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}
/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListUse.Resource = function () {

    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_reqName")[0].innerHTML = R_ReqName;
    $("#r_mobileType")[0].innerHTML = R_MobileType;
    $("#r_uuid")[0].innerHTML = R_UUID;
    $("#r_requestDate")[0].innerHTML = R_RequestDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListUse.GetCount = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Auth/ListUse.asmx/GetListCount";
    ParamArray = {
        'activeMobie': '1',
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListUse.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-16
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListUse.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shimpark
작성일자 : 2013-01-17
작성목적 : 선택된 값 가져오기
***************************************************/
ListUse.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2013-01-17
작성목적 : 승인 불가 처리  (웹서비스 호출)
**********************************************************************************************/
ListUse.Reject = function () {

    var selValue = ListUse.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmReject)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'listID': selValue,
                'activeMobile': '3'
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/MDM/Ws/Auth/ListUse.asmx/ListUpd";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListUse.SuccessReject");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2013-01-17
작성목적 : 승인 불가 처리  (웹서비스 값 받기)
***************************************************/
ListUse.SuccessReject = function (data, status) {
    if (data.d == true) {
        ListUse.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-06
작성목적 : 모바일정보 수정
***************************************************/
ListUse.DeviceInfo = function (p_userID, p_deptID, p_listID) {

    ParamArray = {
        'userID': p_userID,
        'deptID': p_deptID,
        'listID': p_listID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listPage': '/DeskPlusEIP/MDM/Auth/ListUse.aspx'
    };

    CallEncrypt(CallGetParam());

    location.href = "../Device/DeviceInfo.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListUse.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "ListUse.aspx?p=" + GetEncryptValue;
}


