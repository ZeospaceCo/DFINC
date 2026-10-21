List = {}

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
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

        // 기간검색
        $("#ddlSearchTerm").hide();
        $("#startEndDate").hide();

        // 검색조건
        $('#ddlSearchField').change(function (e) {
            e.preventDefault();
            List.VisibleSearch();
        });

        // 검색조건(기간검색)
        $('#ddlSearchTerm').change(function (e) {
            e.preventDefault();
            List.RegDate();
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

        //엑셀다운로드
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            List.Excel();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModPageSize();

        });

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.LoadData("");
    });
});



List.SetTab = function (_id) {
    if (_id == "") return;

    switch (_id.toUpperCase()) {
        case "LOGINS":
            $("#LoginS").attr("class", "tab_on");
            $("#LoginF").attr("class", "");
            $("#MLoginS").attr("class", "");
            $("#MLoginF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        case "LOGINF":
            $("#LoginS").attr("class", "");
            $("#LoginF").attr("class", "tab_on");
            $("#MLoginS").attr("class", "");
            $("#MLoginF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        case "MLOGINS":
            $("#LoginS").attr("class", "");
            $("#LoginF").attr("class", "");
            $("#MLoginS").attr("class", "tab_on");
            $("#MLoginF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        case "MLOGINF":
            $("#LoginS").attr("class", "");
            $("#LoginF").attr("class", "");
            $("#MLoginS").attr("class", "");
            $("#MLoginF").attr("class", "tab_on");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
        default:
            $("#LoginS").attr("class", "tab_on");
            $("#LoginF").attr("class", "");
            $("#MLoginS").attr("class", "");
            $("#MLoginF").attr("class", "");
            $("#ddlSearchField")[0].selectedIndex = 0;
            break;
    }

    TabMenu = _id;
    $("#ctbSearch").val("");
    CurrentPage = 1;
    List.LoadData("search");
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 기간검색 활성화 여부
***************************************************/
List.VisibleSearch = function () {

    var sField = $("#ddlSearchField").val();

    if (sField == "StartTimeYMD" || sField == "EndTimeYMD") {
        $("#ddlSearchTerm").show();
    }
    else {
        $("#ddlSearchTerm").hide();     // 이름, IP 이외
        $("#startEndDate").hide();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
    var sTerm = $("#ddlSearchTerm").val();

    if (sTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    }
    else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}



/**************************************************
작성자   : 
작성일자 :
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
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

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Statistics/Ws/Content/LogIn/List.asmx/"; //GetLSListCount";

    switch (TabMenu.toUpperCase()) {
        case "LOGINS":
            url += "GetLSListCount";
            break;
        case "LOGINF":
            url += "GetLSFListCount";
            break;
        case "MLOGINS":
            url += "GetLSMListCount";
            break;
        case "MLOGINF":
            url += "GetLSMFListCount";
            break;
        default:
            url += "GetLSListCount";
            break;
    }


    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val();
    TargetYear = $("#ddlSearchYear").val();

    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'targetYear': TargetYear
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Statistics/Ws/Content/LogIn/List.asmx/"; //GetPageList";
    var urlH = "/DeskPlusEIP/Statistics/Ws/Content/LogIn/List.asmx/"; //GetPageListLoginTotal";

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val();
    TargetYear = $("#ddlSearchYear").val();

    switch (TabMenu.toUpperCase()) {
        case "LOGINS":
            url += "GetPageList";
            urlH += "GetPageListLoginTotal";
            break;
        case "LOGINF":
            url += "GetPageFList";
            urlH += "GetPageFListLoginTotal";
            break;
        case "MLOGINS":
            url += "GetPageMList";
            urlH += "GetPageMListLoginTotal";
            break;
        case "MLOGINF":
            url += "GetPageMFList";
            urlH += "GetPageMFListLoginTotal";
            break;
        default:
            url += "GetPageList";
            urlH += "GetPageListLoginTotal";
            break;
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'targetYear': TargetYear,
        'order': Order
    };


    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

    // 상단 소계 바인딩
    var objH = eval("List.SuccessTotalDataBind");
    AjaxService(urlH, data, objH);

}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 상단 소계 카운트 바인딩
**********************************************************************************************/
List.SuccessTotalDataBind = function (data, status) {

    $("#r_TotalLogin")[0].innerHTML = R_TotalLoginCount;
    $("#d_1Month")[0].innerHTML = data.d[0].CntMonth1;
    $("#d_2Month")[0].innerHTML = data.d[0].CntMonth2;
    $("#d_3Month")[0].innerHTML = data.d[0].CntMonth3;
    $("#d_4Month")[0].innerHTML = data.d[0].CntMonth4;
    $("#d_5Month")[0].innerHTML = data.d[0].CntMonth5;
    $("#d_6Month")[0].innerHTML = data.d[0].CntMonth6;
    $("#d_7Month")[0].innerHTML = data.d[0].CntMonth7;
    $("#d_8Month")[0].innerHTML = data.d[0].CntMonth8;
    $("#d_9Month")[0].innerHTML = data.d[0].CntMonth9;
    $("#d_10Month")[0].innerHTML = data.d[0].CntMonth10;
    $("#d_11Month")[0].innerHTML = data.d[0].CntMonth11;
    $("#d_12Month")[0].innerHTML = data.d[0].CntMonth12;
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#plShareUser').setTemplateURL('/DeskPlusEIP/Statistics/Jt/Content/LogIn/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plShareUser').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_UserNM")[0].innerHTML = R_UserNM;
    $("#r_1Month")[0].innerHTML = R_1Month;
    $("#r_2Month")[0].innerHTML = R_2Month;
    $("#r_3Month")[0].innerHTML = R_3Month;
    $("#r_4Month")[0].innerHTML = R_4Month;
    $("#r_5Month")[0].innerHTML = R_5Month;
    $("#r_6Month")[0].innerHTML = R_6Month;
    $("#r_7Month")[0].innerHTML = R_7Month;
    $("#r_8Month")[0].innerHTML = R_8Month;
    $("#r_9Month")[0].innerHTML = R_9Month;
    $("#r_10Month")[0].innerHTML = R_10Month;
    $("#r_11Month")[0].innerHTML = R_11Month;
    $("#r_12Month")[0].innerHTML = R_12Month;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 검색
**********************************************************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  

    List.LoadData("search");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
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

            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
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
작성자   : shine
작성일자 :2011-06-15
작성목적 : 페이지 사이즈 설정
***************************************************/
List.ModPageSize = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    List.Search();

}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-10
작성목적 : 엑셀다운로드
**********************************************************************************************/
List.Excel = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val();
    TargetYear = $("#ddlSearchYear").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'tabMenu': TabMenu
        , 'targetYear': TargetYear
        , 'searchFiled': SearchField
        , 'searchValue': SearchValue
        , 'totCnt': TotalItemCount
    };


    CallEncrypt(CallGetParam());

    location.href = "ExcelDown.aspx?p=" + GetEncryptValue;
}