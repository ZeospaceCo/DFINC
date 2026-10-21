ListMonth = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListMonth.LoadData("");
        }
    }

    $(document).ready(function () {

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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListMonth.Search();
        });
        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListMonth.Search();
            }
        });
        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListMonth.Search();
        });
        //년도
        $('#ddlYear').change(function () {
            ListMonth.Search();
        });
        //엑셀
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListMonth.Excel();
        });
        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListMonth.LoadYear($('#ddlYear').val());
            }
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        ListMonth.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ListMonth.Init = function () {

    ListMonth.LoadYear(SelYears);
    ListMonth.LoadData("");
}

ListMonth.LoadYear = function (p_year) {

    $('#ddlYear').empty();
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListMonth.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListMonth.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListMonth.GetCount(); //검색 총 카운트 가져오기
        ListMonth.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListMonth.LoadData("search");
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
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListMonth.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ListMonth.Search = function () {

    var response = ListMonth.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    ListMonth.LoadData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ListMonth.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/Statistics/ListMonth.asmx/";

    switch (TabMenu.toUpperCase()) {
        case "FORM":
            url += "GetFormList";
            break;
        case "DEPT":
            url += "GetDeptList";
            break;
        case "USER":
            url += "GetUserList";
            break;
        default:
            url += "GetFormList";
            break;
    }
    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount
        , 'pageSize': RecordPerPage
        , 'pageSizePageNo': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'years': $("#ddlYear").val()
    };

    var obj = eval("ListMonth.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ListMonth.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/Statistics/ListMonth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListMonth.Resource();    //다국어처리
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ListMonth.Resource = function () {

    if ($(".spStatMonth0") != undefined) { $(".spStatMonth0").text(R_StatMonth0); }
    if ($(".spStatMonth1") != undefined) { $(".spStatMonth1").text(R_StatMonth1); }
    if ($(".spStatMonth2") != undefined) { $(".spStatMonth2").text(R_StatMonth2); }
    if ($(".spStatMonth3") != undefined) { $(".spStatMonth3").text(R_StatMonth3); }
    if ($(".spStatMonth4") != undefined) { $(".spStatMonth4").text(R_StatMonth4); }
    if ($(".spStatMonth5") != undefined) { $(".spStatMonth5").text(R_StatMonth5); }
    if ($(".spStatMonth6") != undefined) { $(".spStatMonth6").text(R_StatMonth6); }
    if ($(".spStatMonth7") != undefined) { $(".spStatMonth7").text(R_StatMonth7); }
    if ($(".spStatMonth8") != undefined) { $(".spStatMonth8").text(R_StatMonth8); }
    if ($(".spStatMonth9") != undefined) { $(".spStatMonth9").text(R_StatMonth9); }
    if ($(".spStatMonth10") != undefined) { $(".spStatMonth10").text(R_StatMonth10); }
    if ($(".spStatMonth11") != undefined) { $(".spStatMonth11").text(R_StatMonth11); }
    if ($(".spStatMonth12") != undefined) { $(".spStatMonth12").text(R_StatMonth12); }
    if ($(".spStatTot") != undefined) { $(".spStatTot").text(R_StatTot); }
    if ($(".spStatUnit") != undefined) { $(".spStatUnit").text(R_StatUnit); }
    if ($(".spStatC") != undefined) { $(".spStatC").text(R_StatC); }
    if ($(".spStatW") != undefined) { $(".spStatW").text(R_StatW); }
    if ($(".spStatR") != undefined) { $(".spStatR").text(R_StatR); }
    if ($(".spStatD") != undefined) { $(".spStatD").text(R_StatD); }
    if ($(".spStatSum") != undefined) { $(".spStatSum").text(R_StatSum); }
    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ListMonth.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/Statistics/ListMonth.asmx/";

    switch (TabMenu.toUpperCase()) {
        case "FORM":
            url += "GetFormListCnt";
            break;
        case "DEPT":
            url += "GetDeptListCnt";
            break;
        case "USER":
            url += "GetUserListCnt";
            break;
        default:
            url += "GetFormListCnt";
            break;
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'searchField': SearchField
        , 'searchValue': SearchValue
        , 'years': $("#ddlYear").val()
    };

    var obj = eval("ListMonth.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListMonth.SuccessPageCnt = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

ListMonth.SetTab = function (_id) {
    if (_id == "") return;

    switch (_id.toUpperCase()) {
        case "FORM":
            $("#tabForm").attr("class", "tab_on");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        case "DEPT":
            $("#tabForm").attr("class", "");
            $("#tabDept").attr("class", "tab_on");
            $("#tabUser").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 1;
            break;
        case "USER":
            $("#tabForm").attr("class", "");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "tab_on");
            $("#ddlSearchField")[0].selectedIndex = 2;
            break;
        default:
            $("#tabForm").attr("class", "tab_on");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
    }

    TabMenu = _id;
    $("#ctbSearch").val("");
    CurrentPage = 1;
    ListMonth.LoadData("search");
}
//엑셀다운로드(보기)
ListMonth.Excel = function () {

    //파라미터값 배열에 담기
    ParamArray = {
        'tabMenu' : TabMenu
        , 'years' : $("#ddlYear").val()
        , 'searchFiled': $("#ddlSearchField").val()
        , 'searchValue': $("#ctbSearch").val().securityText()
        , 'totCnt' : TotalItemCount
    };
    CallEncrypt(CallGetParam());
    location.href = "ListMonthExcel.aspx?p=" + GetEncryptValue;
}