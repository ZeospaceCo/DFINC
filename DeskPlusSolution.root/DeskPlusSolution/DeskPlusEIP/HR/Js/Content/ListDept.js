ListDept = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListDept.LoadData("");
            ListDept.GoPageLoad();
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

        $(function () {
            $('.chosen-select').chosen();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListDept.SavePageSize();
            ListDept.Search();
        });

        $('#ddlDeptList').change(function () {
            DeptID = $('#ddlDeptList').val();
            ListDept.Search();
        });

        $('#ddlActiveDept').change(function () {
            DeptState = $('#ddlActiveDept').val();
            ListDept.Search();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListDept.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListDept.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListDept.ExcelDown();
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

            ListDept.SearchType();
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListDept.LoadYear($('#ddlYear').val());
            }

            ListDept.SearchType();
        });

        //분기 구분 선택
        $('#ddlQuarter').change(function () {
            ListDept.SearchType();
        });

        //월 구분 선택
        $('#ddlMonth').change(function () {
            ListDept.SearchType();
        });

        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListDept.ParsingDate('left');
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListDept.ParsingDate('right');
        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = $(document).height();
            $("html, body").animate({ scrollTop: pos }, 100);

        });

        ListDept.Init();

        //데이터 로드
        ListDept.LoadData("");
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 검색
***************************************************/
ListDept.SearchType = function () {
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
        ListDept.Month();
    }
    if ($('#ddlGubun').val() == "Quarter") {
        $('#liYear').show();
        $('#liQuarter').show();
        $('#liMonth').hide();

        SearchType = "Quarter";
        ListDept.Quarter();
    }
    if ($('#ddlGubun').val() == "Year") {
        $('#liYear').show();
        $('#liQuarter').hide();
        $('#liMonth').hide();

        SearchType = "Year";
        ListDept.Year();
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListDept.Month = function (month) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetMonthDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListDept.SuccessMonth = function (data, status) {
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
ListDept.Quarter = function (quarter) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetQuarterDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 분기 검색
***************************************************/
ListDept.SuccessQuarter = function (data, status) {
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
ListDept.Year = function (year) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetYearDate";

    ParamArray = {
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 년간 검색
***************************************************/
ListDept.SuccessYear = function (data, status) {
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
ListDept.Init = function () {
    ListDept.LoadYear(Year);

    ListDept.SearchType();

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
ListDept.LoadYear = function (p_year) {
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
ListDept.ParsingDate = function (type) {
    if ($('#ddlGubun').val() == "Week") {
        ListDept.ParsingWeek(type);
    }
    if ($('#ddlGubun').val() == "Month") {
        ListDept.ParsingMonth(type);
    }
    if ($('#ddlGubun').val() == "Quarter") {
        ListDept.ParsingQuarter(type);
    }
    if ($('#ddlGubun').val() == "Year") {
        ListDept.ParsingYear(type);
    }

    ListDept.Search();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListDept.ParsingWeek = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingWeek";

    ParamArray = {
        'startdate': $("#startDate").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessParsingWeek");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListDept.SuccessParsingWeek = function (data, status) {
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
ListDept.ParsingMonth = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingMonth";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessParsingMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 월 단위로 날짜 변환
***************************************************/
ListDept.SuccessParsingMonth = function (data, status) {
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
ListDept.ParsingQuarter = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingQuarter";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessParsingQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 분기 단위로 날짜 변환
***************************************************/
ListDept.SuccessParsingQuarter = function (data, status) {
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
ListDept.ParsingYear = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingYear";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessParsingYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 년 단위로 날짜 변환
***************************************************/
ListDept.SuccessParsingYear = function (data, status) {
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
ListDept.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListDept.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListDept.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
ListDept.Search = function () {

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


    ListDept.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    //ListDept.LoadData("search");
    ListDept.GoPageLoad();
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
ListDept.SettingSearch = function () {
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
ListDept.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        //ListDept.GetCount(); //검색 총 카운트 가져오기
        ListDept.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        //ListDept.GetCount(); //검색 총 카운트 가져오기
        ListDept.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListDept.LoadData("search");
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
ListDept.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListDept.asmx/GetDeptListCount";

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
    var obj = eval("ListDept.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListDept.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ListDept.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListDept.asmx/GetDeptList";
    
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
        'searchType': SearchType
    };

    var obj = eval("ListDept.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListDept.SuccessPageList = function (data, status) {
    if (data.d != null) {
        if (data.d.length > 0) {
            $("#startDate").val(data.d[0].StartDate);
            $("#endDate").val(data.d[0].EndDate);
        }
    }

    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/JTListDept.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListDept.Resource();

    $("#HRContent").height($(document).height() - 420);

    if ($("#HRTable").height() < $(document).height() - 420) {
        $("#HRContent").height($("#HRTable").height() + 1);
    }

    ListDept.Tooltip();
}

ListDept.Tooltip = function () {
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

        // position the tooltip 4 pixels above and 4 pixels to the left of the abbreviation
        var left = $(this).position().left + $(this).width() + 4;
        var top = $(this).position().top - 4;
        $(this).next().css('left', left);
        $(this).next().css('top', top);

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
ListDept.Resource = function () {

    $("#r_Name")[0].innerHTML = R_Name;     //이름
    $("#r_TitleName2")[0].innerHTML = R_TitleName2;     //직급
    $("#r_WS_Reg")[0].innerHTML = R_WS_Reg;     //출근등록
    $("#r_WC_Reg")[0].innerHTML = R_WC_Reg;     //퇴근등록
    $("#r_Register")[0].innerHTML = R_Register;  //등록율
    $("#r_WorkTime")[0].innerHTML = R_WorkTime;  //근무시간
    $("#r_52Time")[0].innerHTML = R_52Time;  //52시간 기준
    $("#r_Holi_HR")[0].innerHTML = R_Holi_HR;  //근태정보
    $("#r_Note")[0].innerHTML = R_Note;     //비고

    $("#r_IN")[0].innerHTML = R_IN;     //사내
    $("#r_OUT")[0].innerHTML = R_OUT;     //사외
    $("#r_IN2")[0].innerHTML = R_IN;     //사내
    $("#r_OUT2")[0].innerHTML = R_OUT;     //사외
    $("#r_BaseWork")[0].innerHTML = R_BaseWork;     //기본근무
    $("#r_Over_Work")[0].innerHTML = R_Over_Work;     //연장근무
    $("#r_StandardTime")[0].innerHTML = R_StandardTime;     //기준시간
    $("#r_WorkSum")[0].innerHTML = R_WorkSum;     //근무합산
    $("#r_Attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;     //지각
    $("#r_Attendance_Early")[0].innerHTML = R_Attendance_Early;     //조퇴
    $("#r_HoliType")[0].innerHTML = R_HoliType;     //연차
    $("#r_Etc")[0].innerHTML = R_Etc;     //기타

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : kkzz52  
작성일자 : 2017-11-24
작성목적 : 엑셀
**********************************************************************************************/
ListDept.ExcelDown = function () {

    $("#hdStartDate").val($("#startDate").val());
    $("#hdEndDate").val($("#endDate").val());
    $("#hdSearchType").val(SearchType);

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
        'searchType': SearchType
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
ListDept.DocView = function (docID) {

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
ListDept.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 지각/조퇴 상세 보기
***************************************************/
ListDept.LateView = function (_userID, _deptID, _pathID, _userName, _deptBase, _startDate, _endDate, _minClosingID, _maxClosingID, _gubun) {
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID,
        'pathID': _pathID,
        'userName': _userName,
        'deptBase': _deptBase,
        'startDate': _startDate,
        'endDate': _endDate,
        'minClosingID': _minClosingID,
        'maxClosingID': _maxClosingID,
        'gubun': _gubun,
        'bPage': "ListDept"
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
ListDept.EtcView = function (_userID, _deptID, _pathID, _userName, _deptBase, _startDate, _endDate, _minClosingID, _maxClosingID) {
    ParamArray = {
        'userID': _userID,
        'deptID': _deptID,
        'pathID': _pathID,
        'userName': _userName,
        'deptBase': _deptBase,
        'startDate': _startDate,
        'endDate': _endDate,
        'minClosingID': _minClosingID,
        'maxClosingID': _maxClosingID,
        'bPage': "ListDept"
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
ListDept.GoList = function (_userID, _deptID, _pathID, _deptBase, _deptState, _startDate, _endDate, _minClosingID, _maxClosingID) {

    ParamArray = {
        'folderStatusID': "Private",
        'userID': _userID,
        'deptID': _deptID,
        'pathID': _pathID,
        'deptBase': _deptBase,
        'deptState': _deptState,
        'startDate': _startDate,
        'endDate': _endDate,
        'searchType': "",
        'gubun': $('#ddlGubun').val(),
        'year': $('#ddlYear').val(),
        'quarter': $('#ddlQuarter').val(),
        'month': $('#ddlMonth').val()
    };

    CallEncrypt(CallGetParam());

    location.href = "ListPrivate.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListDept.GoPageLoad = function () {    

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
        'deptState': DeptState
    };

    CallEncrypt(CallGetParam());
    location = "ListDept.aspx?p=" + GetEncryptValue;
}
