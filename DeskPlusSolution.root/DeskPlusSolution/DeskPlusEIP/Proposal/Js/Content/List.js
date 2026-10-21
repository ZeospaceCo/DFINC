List = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });


        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //검색
        $('#cbtnSearching').click(function (e) {
            e.preventDefault();
            List.Search();
        })

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });


        //시작년도검색
        $('#ddlStartYear').change(function (e) {
            e.preventDefault();

            var selStartYearTxt = $("#ddlStartYear option:selected").text();

            if (selStartYearTxt == "▲" || selStartYearTxt == "▼") {
                List.LoadStartYear($('#ddlStartYear').val());
            }
        });

        //종료년도검색
        $('#ddlEndYear').change(function (e) {
            e.preventDefault();

            var selEndYearTxt = $("#ddlEndYear option:selected").text();

            if (selEndYearTxt == "▲" || selEndYearTxt == "▼") {
                List.LoadEndYear($('#ddlEndYear').val());
            }
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            List.Search();
        });

        //통계보기
        $('#cbtnStats').click(function (e) {
            e.preventDefault();
            List.Stats();
        });

        ListCount = $("#ddlListCount").val();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Init();

    });
});

/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

    List.LoadStartYear(StartYears);
    List.LoadEndYear(EndYears);

    List.LoadData();
    __addEventListener(document, "click", List.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 시작년도 위아래 체크하기
***************************************************/
List.LoadStartYear = function (p_year) {

    $('#ddlStartYear').empty();
    $('#ddlStartYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlStartYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlStartYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlStartYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlStartYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 종료년도 위아래 체크하기
***************************************************/
List.LoadEndYear = function (p_year) {

    $('#ddlEndYear').empty();
    $('#ddlEndYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlEndYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlEndYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlEndYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlEndYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리         
    //List.LoadData("search");
    List.GoPageLoad();
}

/**************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    JudgeGubun = $("#ddlJudge").val();

    StartYears = $("#ddlStartYear").val();
    StartMonths = $("#ddlStartMonth").val();
    EndYears = $("#ddlEndYear").val();
    EndMonths = $("#ddlEndMonth").val();

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadProposalData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadProposalSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                //List.LoadData("search");
                List.GoPageLoad();
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
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
            loadProposalSearchData();
        }
        else {
            loadProposalData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Proposal/Ws/Content/List.asmx/GetPageCount";
    
    ParamArray = {
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'gubun': ''
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    $("#cbtnStats").show();

    if (pageCount == 0)
        $("#cbtnStats").hide();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    
    var url = "/DeskPlusEIP/Proposal/Ws/Content/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'gubun': ''
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 통계보기
**********************************************************************************************/
List.Stats = function () {

    List.SettingSearch();

    ParamArray = {
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'gubun': ''
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Proposal/Content/PopUpStats.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowStatistics", "700", "700")
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 제안보기
**********************************************************************************************/
List.View = function (p_proposalNo) {
    var listPage = "../Content/View.aspx";

    ParamArray = {
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'currentPage': CurrentPage,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listCount': ListCount,
        'proposalNo': p_proposalNo,
        'menuID': 'List'
    };

    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2008-08-29
작성목적 : 심사조건 검색
***************************************************/
List.Status = function () {
    List.Search();
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'currentPage': CurrentPage,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listCount': ListCount,
        'menuID': 'List'
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


