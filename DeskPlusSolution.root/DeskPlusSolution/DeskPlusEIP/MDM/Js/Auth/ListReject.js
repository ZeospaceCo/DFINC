ListReject = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListReject.LoadData("");
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

        // 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            ListReject.Del();
        });

        // 승인처리
        $('#cbtnYes').click(function (e) {
            e.preventDefault();
            ListReject.Confirm();

        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListReject.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListReject.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListReject.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListReject.Search();
        });

        ListReject.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 초기화 하기
***************************************************/
ListReject.Init = function () {

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

    ListReject.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 날짜 세팅
***************************************************/
ListReject.RegDate = function () {
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
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 검색
***************************************************/
ListReject.Search = function () {
    var response = ListReject.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListReject.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 검색 필드 설정하기
***************************************************/
ListReject.SettingSearch = function () {

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
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 초기화
***************************************************/
ListReject.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadMDMData = function () {
        ListReject.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadMDMSearchData = function () {

        RecordPerPage = $("#ddlListCount").val();
        ListReject.GetCount(); //검색 총 카운트 가져오기
        ListReject.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListReject.LoadData("search");
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
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListReject.GetPageList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Auth/ListReject.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'activeMobie': '3',
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("ListReject.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListReject.SuccessPageList = function (data, status) {

    $('#phListReject').setTemplateURL('/DeskPlusEIP/MDM/Jt/Auth/JTListReject.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListReject').processTemplate(data.d);
    $('#loading').html("");

    ListReject.Resource();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 전체 체크박스
**********************************************************************************************/
ListReject.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}
/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListReject.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_reqName")[0].innerHTML = R_ReqName;
    $("#r_mobileType")[0].innerHTML = R_MobileType;
    $("#r_uuid")[0].innerHTML = R_UUID;
    $("#r_requestDate")[0].innerHTML = R_RequestDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListReject.GetCount = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Auth/ListReject.asmx/GetListCount";
    ParamArray = {
        'activeMobie': '3',
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListReject.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListReject.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 선택된 값 가져오기
***************************************************/
ListReject.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 승인 처리  (웹서비스 호출)
**********************************************************************************************/
ListReject.Confirm = function () {

    var selValue = ListReject.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmApprove)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'listID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/MDM/Ws/Auth/ListReject.asmx/ListApprove";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListReject.SuccessConfirm");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 승인 처리  (웹서비스 값 받기)
***************************************************/
ListReject.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        ListReject.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 삭제  (웹서비스 호출)
**********************************************************************************************/
ListReject.Del = function () {

    var selValue = ListReject.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmDel)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'listID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/MDM/Ws/Auth/ListReject.asmx/Del";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListReject.SuccessDel");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 삭제  (웹서비스 값 받기)
***************************************************/
ListReject.SuccessDel = function (data, status) {
    if (data.d == true) {
        ListReject.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}