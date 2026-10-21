List = {}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    //SearchField = $("#ddlSearchField").val(); // 검색 기본 대상

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;

            List.LoadData("");
        }
    }

    $(document).ready(function () {
        RecordPerPage = $("#ddlListCount").val();
        //        alert(TotalItemCount + " : " + RecordPerPage + " : " + RecordPerPage + " : " + CurrentPage + " : ");
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

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $("#ddlListCount").val();
            List.Search();
        });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
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

    List.LoadData("");
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 날짜 세팅
***************************************************/
List.RegDate = function () {
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
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();

    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    List.LoadData("search");
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
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

    // ~ 는 FuzzyQuery 로 인식하여 삭제 함
    var re = /~/g;
    SearchValue = jQuery.trim(SearchValue.replace(re, ""));

    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    SearchField = $("#ddlSearchField").val(); // 검색 기본 대상

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

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
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadSearchData();
        }
        else {
            loadData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Search/Ws/Admin/Index/List.asmx/GetPageList"
    //alert(TotalItemCount + " : " + RecordPerPage + " : " + CurrentPage + " : " + SearchField + " : " + SearchValue + " : " + Order)
    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Search/Jt/Admin/Index/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    //$("#r_requestDate")[0].innerHTML = R_RequestDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Search/Ws/Admin/Index/List.asmx/GetPageCount";
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-11
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}