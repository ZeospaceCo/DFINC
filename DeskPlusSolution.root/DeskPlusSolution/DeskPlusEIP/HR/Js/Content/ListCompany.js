ListCompany = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListCompany.LoadData("");
            ListCompany.GoPageLoad();
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

        $(function () {
            $('.chosen').chosen({ disable_search_threshold: 10 });
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListCompany.SavePageSize();
            ListCompany.Search();
        });

        $('#ddlDeptList').change(function () {
            DeptID = $('#ddlDeptList').val();
            ListCompany.Search("");
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListCompany.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListCompany.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListCompany.ExcelDown();
        });

        //검색 구분 선택
        $('#ddlGubun').change(function () {
            if ($('#ddlGubun').val() == "Month") {
                $("#ddlYear").val(NowYear);
                $("#ddlMonth").val(NowMonth);

                $("#startDate").attr("disabled", "true");
                $("#endDate").attr("disabled", "true");
            }
            else if ($('#ddlGubun').val() == "Quarter") {
                $("#ddlYear").val(NowYear);
                $("#ddlQuarter").val(NowQuarter);

                $("#startDate").attr("disabled", "true");
                $("#endDate").attr("disabled", "true");
            }
            else if ($('#ddlGubun').val() == "Year") {
                $("#ddlYear").val(NowYear);

                $("#startDate").attr("disabled", "true");
                $("#endDate").attr("disabled", "true");
            }
            else {
                if ($("#startDate").attr("disabled")) { $("#startDate").removeAttr("disabled"); }
                if ($("#endDate").attr("disabled")) { $("#endDate").removeAttr("disabled"); }
            }

            ListCompany.SearchType();
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListCompany.LoadYear($('#ddlYear').val());
            }

            ListCompany.SearchType();
        });

        //분기 구분 선택
        $('#ddlQuarter').change(function () {
            ListCompany.SearchType();
        });

        //월 구분 선택
        $('#ddlMonth').change(function () {
            ListCompany.SearchType();
        });

        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListCompany.ParsingDate('left');
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListCompany.ParsingDate('right');
        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = $(document).height();
            $("html, body").animate({ scrollTop: pos }, 100);

        });        

        ListCompany.Init();

        //데이터 로드
        ListCompany.LoadData("");
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 검색
***************************************************/
ListCompany.SearchType = function () {
    if ($('#ddlGubun').val() == "Week") {
        $('#liYear').hide();
        $('#liMonth').hide();
        $('#liQuarter').hide();

        SearchType = "Week";
        $("#startDate").val(WeekStartDate);
        $("#endDate").val(WeekEndDate);
    }
    if ($('#ddlGubun').val() == "Month") {
        $('#liYear').show();
        $('#liMonth').show();
        $('#liQuarter').hide();

        SearchType = "Month";
        ListCompany.Month();
    }
    if ($('#ddlGubun').val() == "Quarter") {
        $('#liYear').show();
        $('#liQuarter').show();
        $('#liMonth').hide();

        SearchType = "Quarter";
        ListCompany.Quarter();
    }
    if ($('#ddlGubun').val() == "Year") {
        $('#liYear').show();
        $('#liQuarter').hide();
        $('#liMonth').hide();

        SearchType = "Year";
        ListCompany.Year();
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListCompany.Month = function (month) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetMonthDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListCompany.SuccessMonth = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("|")[0]);
        $("#endDate").val(data.d.split("|")[1]);

        $("#startDate").attr("disabled", "true");
        $("#endDate").attr("disabled", "true");
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 분기 검색
***************************************************/
ListCompany.Quarter = function (quarter) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetQuarterDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 분기 검색
***************************************************/
ListCompany.SuccessQuarter = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("|")[0]);
        $("#endDate").val(data.d.split("|")[1]);

        $("#startDate").attr("disabled", "true");
        $("#endDate").attr("disabled", "true");
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 년간 검색
***************************************************/
ListCompany.Year = function (year) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetYearDate";

    ParamArray = {
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 년간 검색
***************************************************/
ListCompany.SuccessYear = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("|")[0]);
        $("#endDate").val(data.d.split("|")[1]);

        $("#startDate").attr("disabled", "true");
        $("#endDate").attr("disabled", "true");
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
ListCompany.Init = function () {
    ListCompany.LoadYear(Year);

    ListCompany.SearchType();

    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    $("#startDate").val(StartDate);
    $("#endDate").val(EndDate);

    $('#titleDate').text(StartDate + "~" + EndDate);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 년도 위아래 체크하기
***************************************************/
ListCompany.LoadYear = function (p_year) {
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
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 선택한 단위로 날짜 이동
***************************************************/
ListCompany.ParsingDate = function (type) {
    if ($('#ddlGubun').val() == "Week") {
        ListCompany.ParsingWeek(type);
    }
    if ($('#ddlGubun').val() == "Month") {
        ListCompany.ParsingMonth(type);
    }
    if ($('#ddlGubun').val() == "Quarter") {
        ListCompany.ParsingQuarter(type);
    }
    if ($('#ddlGubun').val() == "Year") {
        ListCompany.ParsingYear(type);
    }

    ListCompany.Search();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListCompany.ParsingWeek = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingWeek";

    ParamArray = {
        'startdate': $("#startDate").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessParsingWeek");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListCompany.SuccessParsingWeek = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("~")[0]);
        $("#endDate").val(data.d.split("~")[1]);

        $('#titleDate').text(data.d);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 월 단위로 날짜 변환
***************************************************/
ListCompany.ParsingMonth = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingMonth";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessParsingMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 월 단위로 날짜 변환
***************************************************/
ListCompany.SuccessParsingMonth = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("~")[0]);
        $("#endDate").val(data.d.split("~")[1]);

        $("#ddlYear").val(data.d.split("~")[2]);
        $("#ddlMonth").val(data.d.split("~")[3]);

        $('#titleDate').text($("#startDate").val() + "~" + $("#endDate").val());
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 분기 단위로 날짜 변환
***************************************************/
ListCompany.ParsingQuarter = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingQuarter";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessParsingQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 분기 단위로 날짜 변환
***************************************************/
ListCompany.SuccessParsingQuarter = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("~")[0]);
        $("#endDate").val(data.d.split("~")[1]);

        $("#ddlYear").val(data.d.split("~")[2]);
        $("#ddlQuarter").val(data.d.split("~")[3]);

        $('#titleDate').text($("#startDate").val() + "~" + $("#endDate").val());
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 년 단위로 날짜 변환
***************************************************/
ListCompany.ParsingYear = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingYear";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessParsingYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 년 단위로 날짜 변환
***************************************************/
ListCompany.SuccessParsingYear = function (data, status) {
    if (data.d != null) {
        $("#startDate").val(data.d.split("~")[0]);
        $("#endDate").val(data.d.split("~")[1]);

        $("#ddlYear").val(data.d.split("~")[2]);

        $('#titleDate').text($("#startDate").val() + "~" + $("#endDate").val());
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListCompany.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListCompany.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListCompany.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
ListCompany.Search = function () {

    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();

    var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

    if (dtResponse == false) {
        alert(R_StartDateEndDate);
        return;
    }

    var start = getStartDate.replace(/-/gi, '');
    var end = getEndDate.replace(/-/gi, '');
    var gapM = getDiffDays(start, end);
    
    if (gapM > 60) {
        alert(R_SearchLimit);
        return;
    }

    ListCompany.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    //ListCompany.LoadData("search");
    ListCompany.GoPageLoad();
}

function getDiffDays(pStartDate, pEndDate, pType) {
    //param : pStartDate - 시작일
    //param : pEndDate  - 마지막일
    //param : pType       - 'D':일수, 'M':개월수
    var strSDT = new Date(pStartDate.substring(0, 4), pStartDate.substring(4, 6) - 1, pStartDate.substring(6, 8));
    var strEDT = new Date(pEndDate.substring(0, 4), pEndDate.substring(4, 6) - 1, pEndDate.substring(6, 8));
    var strGapDT = 0;

    if (pType == 'D') {  //일수 차이
        strGapDT = (strEDT.getTime() - strSDT.getTime()) / (1000 * 60 * 60 * 24);
    } else {            //개월수 차이
        //년도가 같으면 단순히 월을 마이너스 한다.
        if (pEndDate.substring(0, 4) == pStartDate.substring(0, 4)) {
            strGapDT = pEndDate.substring(4, 6) * 1 - pStartDate.substring(4, 6) * 1;
        } else {
            strGapDT = ((strEDT.getTime() - strSDT.getTime()) / (1000 * 60 * 60 * 24 * 365.25 / 12)).toFixed(2);
        }
    }
    return strGapDT;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 검색 필드 설정하기
***************************************************/
ListCompany.SettingSearch = function () {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    //SearchField = $("#ddlSearchField").val();
    //SearchValue = $("#ctbSearch").val().securityText();

    //$("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ListCompany.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        //ListCompany.GetCount(); //검색 총 카운트 가져오기
        ListCompany.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        //ListCompany.GetCount(); //검색 총 카운트 가져오기
        ListCompany.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListCompany.LoadData("search");
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
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 카운트 호출
**********************************************************************************************/
ListCompany.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListCompany.asmx/GetCompanyListCount";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'startDate': StartDate,
        'endDate': EndDate,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,
        'searchType': SearchType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListCompany.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListCompany.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ListCompany.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListCompany.asmx/GetCompanyList";

    var isIN = 0;
    if ($('#chkIsIN').is(':checked') == true) {
        isIN = 1;
    }
    else {
        isIN = 0;
    }

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'startDate': StartDate,
        'endDate': EndDate,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchType': SearchType,
        'isIN': isIN
    };

    var obj = eval("ListCompany.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListCompany.SuccessPageList = function (data, status) {
    if (data.d != null) {
        if (data.d.length > 0) {
            $("#startDate").val(data.d[0].StartDate);
            $("#endDate").val(data.d[0].EndDate);
        }
    }

    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/JTListCompany.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListCompany.Resource();

    $("#HRContent").height($(document).height() - 298);

    if ($("#HRTable").height() < $(document).height() - 298) {
        $("#HRContent").height($("#HRTable").height() + 1);
    }

    ListCompany.Tooltip();
}

ListCompany.Tooltip = function () {
    /**
    * store the value of and then remove the title attributes from the
    * abbreviations (thus removing the default tooltip functionality of
    * the abbreviations)
    */
    $('abbr').each(function () {
        $(this).data('title', $(this).attr('title'));
        $(this).removeAttr('title');
    });

    $('abbr').mouseover(function () {
        // first remove all existing abbreviation tooltips
        $('abbr').next('.tooltip').remove();

        // create the tooltip
        $(this).after('<span class="tooltip">' + $(this).data('title') + '</span>');
       
        if ($(window).width() - $(this).position().left < 200) {
            var left = $(this).position().left - $(this).width() - 105;
            var top = $(this).position().top + 20;
            $(this).next().css('left', left);
            $(this).next().css('top', top);
        }
        else {
            // position the tooltip 4 pixels above and 4 pixels to the left of the abbreviation
            var left = $(this).position().left + $(this).width() + 4;
            var top = $(this).position().top - 4;
            $(this).next().css('left', left);
            $(this).next().css('top', top);
        }

    });

    $('abbr').click(function () {
        $(this).mouseover();

        // after a slight 2 second fade, fade out the tooltip for 1 second
        $(this).next().animate({ opacity: 0.9 }, { duration: 2000, complete: function () {
            $(this).fadeOut(1000);
        }
        });

    });

    $('abbr').mouseout(function () {
        $(this).next('.tooltip').remove();
    });
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListCompany.Resource = function () {

    $("#r_DeptName")[0].innerHTML = R_DeptName;     //부서명
    $("#r_PersonCount")[0].innerHTML = R_PersonCount;     //인원
    $("#r_WorkTime")[0].innerHTML = R_WorkTime;     //근무시간
    $("#r_52Time")[0].innerHTML = R_52Time;     //주 52시간 기준
    $("#r_Holi_HR")[0].innerHTML = R_Holi_HR;     //휴가/근태
    $("#r_HoliUserInfo")[0].innerHTML = R_HoliUserInfo;     //연차 사용자 정보 (휴가/반차/기타)

    $("#r_PersonCount2")[0].innerHTML = R_PersonCount2;     //(명)
    $("#r_BaseWork")[0].innerHTML = R_BaseWork;     //기본근무
    $("#r_Over_Work")[0].innerHTML = R_Over_Work;     //연장근무
    $("#r_StandardTime")[0].innerHTML = R_StandardTime;     //기준시간
    $("#r_WorkSum")[0].innerHTML = R_WorkSum;     //근무합산
    $("#r_StandardTimePer")[0].innerHTML = R_StandardTimePer;   //요율
    $("#r_Attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;   //지각
    $("#r_Attendance_Early")[0].innerHTML = R_Attendance_Early;   //조퇴
    $("#r_HoliType")[0].innerHTML = R_HoliType;   //연차
    $("#r_Etc")[0].innerHTML = R_Etc;   //기타

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : kkzz52  
작성일자 : 2017-11-24
작성목적 : 엑셀
**********************************************************************************************/
ListCompany.ExcelDown = function () {

    $("#hdStartDate").val($("#startDate").val());
    $("#hdEndDate").val($("#endDate").val());
    $("#hdSearchType").val(SearchType);

    var isIN = 0;
    if ($('#chkIsIN').is(':checked') == true) {
        isIN = 1;
    }
    else {
        isIN = 0;
    }

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'startDate': $("#startDate").val(),
        'endDate': $("#endDate").val(),
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': "99999",
        'pageSizePageNo': "1",
        'searchType': SearchType,
        'isIN': isIN
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-17
작성목적 : 휴가원 문서보기
***************************************************/
ListCompany.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
ListCompany.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 지각/조퇴 상세 보기
***************************************************/
ListCompany.LateView = function (_userID, _deptID, _pathID, _userName, _deptBase, _startDate, _endDate, _minClosingID, _maxClosingID, _gubun) {
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID.split("^")[0],
        'pathID': _deptID.split("^")[1],
        'userName': _deptID.split("^")[2],
        'deptBase': _deptID.split("^")[2],
        'startDate': _startDate,
        'endDate': _endDate,
        'minClosingID': _minClosingID,
        'maxClosingID': _maxClosingID,
        'gubun': _gubun
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/HR/Content/Popup/ViewLate.aspx?p=" + GetEncryptValue;

    PopupWin(900, 350, url, "", "", true);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 연차 상세 보기
***************************************************/
ListCompany.EtcView = function (_userID, _deptID, _pathID, _userName, _deptBase, _startDate, _endDate, _minClosingID, _maxClosingID) {
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID.split("^")[0],
        'pathID': _deptID.split("^")[1],
        'userName': _deptID.split("^")[2],
        'deptBase': _deptID.split("^")[2],
        'startDate': _startDate,
        'endDate': _endDate,
        'minClosingID': _minClosingID,
        'maxClosingID': _maxClosingID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/HR/Content/Popup/ViewHoli.aspx?p=" + GetEncryptValue;

    PopupWin(1000, 400, url, "", "", true);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-17
작성목적 : 개인근태현황표로 이동
***************************************************/
ListCompany.GoList = function (_deptID, _deptState, _startDate, _endDate) {

    ParamArray = {
        'folderStatusID': "Dept",
        'deptID': _deptID,
        'deptState': _deptState,
        'startDate': _startDate,
        'endDate': _endDate,
        'searchType': SearchType,
        'gubun': $('#ddlGubun').val(),
        'year': $('#ddlYear').val(),
        'quarter': $('#ddlQuarter').val(),
        'month': $('#ddlMonth').val()
    };

    CallEncrypt(CallGetParam());

    location.href = "ListDept.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListCompany.GoPageLoad = function () {

    var isIN = 0;
    if ($('#chkIsIN').is(':checked') == true) {
        isIN = 1;
    }
    else {
        isIN = 0;
    }

    ParamArray = {
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'folderStatusID': FolderStatusID,
        'startDate': StartDate,
        'endDate': EndDate,
        'searchType': SearchType,
        'gubun': $('#ddlGubun').val(),
        'year': $('#ddlYear').val(),
        'quarter': $('#ddlQuarter').val(),
        'month': $('#ddlMonth').val(),
        'deptID': DeptID,
        'isIN': isIN
    };

    CallEncrypt(CallGetParam());
    location = "ListCompany.aspx?p=" + GetEncryptValue;
}