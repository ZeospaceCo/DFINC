ListTop = {}

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
            ListTop.LoadData("");
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
            ListTop.Search();
        });
        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListTop.Search();
        });
        //년도
        $('#ddlYear').change(function () {
            ListTop.Search();
        });
        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListTop.LoadYear($('#ddlYear').val());
            }
        });
        //검색조건
        $("#ddlSearchField").change(function (e) { e.preventDefault(); ListTop.ddlSearchChange(); });

        ListTop.Init();
    });
});

/**************************************************
작성자   : 
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ListTop.Init = function () {

    $(".searchterm").hide();
    if (SearchField.toUpperCase() == "TERM") {
        $(".searchterm").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        $("#startDate").val(sDate);
        $("#endDate").val(eDate);
        // 비활성화ddlYear
        $("#ddlYear").attr("disabled", "true");
    }

    ListTop.LoadYear(SelYears);
    ListTop.LoadData("");
}

ListTop.LoadYear = function (p_year) {

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
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 검색-DDL처리
***************************************************/
ListTop.ddlSearchChange = function () {

    if ($("#ddlSearchField").val().toUpperCase() == "TERM") {
        $(".searchterm").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        // 비활성화ddlYear
        $("#ddlYear").attr("disabled", "true");

    } else {
        $(".searchterm").hide(); //기간   
        $("#startDate").val(""); $("#endDate").val("");

        // 활성화ddlYear
        $("#ddlYear")[0].removeAttribute("disabled");
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListTop.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListTop.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListTop.GetCount(); //검색 총 카운트 가져오기
        ListTop.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListTop.LoadData("search");
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
작성목적 : 검색
***************************************************/
ListTop.Search = function () {

    CurrentPage = 1; //1페이지로 이동처리  
    ListTop.LoadData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ListTop.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/Statistics/ListTop.asmx/";

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
    //alert(TotalItemCount + "," + RecordPerPage + "," + CurrentPage);

    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount
        , 'pageSize': RecordPerPage
        , 'pageSizePageNo': CurrentPage
        , 'sDate': $.trim($("#startDate").val())
        , 'eDate': $.trim($("#endDate").val())
        , 'years': $("#ddlYear").val()
    };

    var obj = eval("ListTop.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ListTop.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/Statistics/ListTop.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListTop.Resource();    //다국어처리
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ListTop.Resource = function () {

//    if ($(".spStatMonth0") != undefined) { $(".spStatMonth0").text(R_StatMonth0); }
//    if ($(".spStatMonth1") != undefined) { $(".spStatMonth1").text(R_StatMonth1); }
//    if ($(".spStatMonth2") != undefined) { $(".spStatMonth2").text(R_StatMonth2); }
//    if ($(".spStatMonth3") != undefined) { $(".spStatMonth3").text(R_StatMonth3); }
//    if ($(".spStatMonth4") != undefined) { $(".spStatMonth4").text(R_StatMonth4); }
//    if ($(".spStatMonth5") != undefined) { $(".spStatMonth5").text(R_StatMonth5); }
//    if ($(".spStatMonth6") != undefined) { $(".spStatMonth6").text(R_StatMonth6); }
//    if ($(".spStatMonth7") != undefined) { $(".spStatMonth7").text(R_StatMonth7); }
//    if ($(".spStatMonth8") != undefined) { $(".spStatMonth8").text(R_StatMonth8); }
//    if ($(".spStatMonth9") != undefined) { $(".spStatMonth9").text(R_StatMonth9); }
//    if ($(".spStatMonth10") != undefined) { $(".spStatMonth10").text(R_StatMonth10); }
//    if ($(".spStatMonth11") != undefined) { $(".spStatMonth11").text(R_StatMonth11); }
//    if ($(".spStatMonth12") != undefined) { $(".spStatMonth12").text(R_StatMonth12); }
//    if ($(".spStatTot") != undefined) { $(".spStatTot").text(R_StatTot); }
//    if ($(".spStatUnit") != undefined) { $(".spStatUnit").text(R_StatUnit); }
//    if ($(".spStatC") != undefined) { $(".spStatC").text(R_StatC); }
//    if ($(".spStatW") != undefined) { $(".spStatW").text(R_StatW); }
//    if ($(".spStatR") != undefined) { $(".spStatR").text(R_StatR); }
//    if ($(".spStatD") != undefined) { $(".spStatD").text(R_StatD); }
//    if ($(".spStatSum") != undefined) { $(".spStatSum").text(R_StatSum); }
    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ListTop.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/Statistics/ListTop.asmx/";

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
          'sDate': $.trim($("#startDate").val())
        , 'eDate': $.trim($("#endDate").val())
        , 'years': $("#ddlYear").val()
    };

    var obj = eval("ListTop.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListTop.SuccessPageCnt = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

ListTop.SetTab = function (_id) {
    if (_id == "") return;

    switch (_id.toUpperCase()) {
        case "FORM":
            $("#tabForm").attr("class", "tab_on");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "");
            break;
        case "DEPT":
            $("#tabForm").attr("class", "");
            $("#tabDept").attr("class", "tab_on");
            $("#tabUser").attr("class", "");
            break;
        case "USER":
            $("#tabForm").attr("class", "");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "tab_on");
            break;
        default:
            $("#tabForm").attr("class", "tab_on");
            $("#tabDept").attr("class", "");
            $("#tabUser").attr("class", "");
            break;
    }

    TabMenu = _id;
    $("#ctbSearch").val("");
    CurrentPage = 1;
    ListTop.LoadData("search");
}
