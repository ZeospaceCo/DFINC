ListPrivate = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListPrivate.LoadData("");
            ListPrivate.GoPageLoad();
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
            ListPrivate.SavePageSize();
            ListPrivate.Search();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListPrivate.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListPrivate.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListPrivate.ExcelDown();
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

            ListPrivate.SearchType();
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListPrivate.LoadYear($('#ddlYear').val());
            }

            ListPrivate.SearchType();
        });

        //분기 구분 선택
        $('#ddlQuarter').change(function () {
            ListPrivate.SearchType();
        });

        //월 구분 선택
        $('#ddlMonth').change(function () {
            ListPrivate.SearchType();
        });

        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListPrivate.ParsingDate('left');
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListPrivate.ParsingDate('right');
        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = $(document).height();
            $("html, body").animate({ scrollTop: pos }, 100);

        });

        ListPrivate.Init();

        //데이터 로드
        ListPrivate.LoadData("");
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 검색
***************************************************/
ListPrivate.SearchType = function () {
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
        ListPrivate.Month();
    }
    if ($('#ddlGubun').val() == "Quarter") {
        $('#liYear').show();
        $('#liQuarter').show();
        $('#liMonth').hide();

        SearchType = "Quarter";
        ListPrivate.Quarter();
    }
    if ($('#ddlGubun').val() == "Year") {
        $('#liYear').show();
        $('#liQuarter').hide();
        $('#liMonth').hide();

        SearchType = "Year";
        ListPrivate.Year();
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListPrivate.Month = function (month) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetMonthDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListPrivate.SuccessMonth = function (data, status) {
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
ListPrivate.Quarter = function (quarter) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetQuarterDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 분기 검색
***************************************************/
ListPrivate.SuccessQuarter = function (data, status) {
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
ListPrivate.Year = function (year) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetYearDate";

    ParamArray = {
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 년간 검색
***************************************************/
ListPrivate.SuccessYear = function (data, status) {
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
ListPrivate.Init = function () {
    ListPrivate.LoadYear(Year);

    ListPrivate.SearchType();

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
ListPrivate.LoadYear = function (p_year) {
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
ListPrivate.ParsingDate = function (type) {
    if ($('#ddlGubun').val() == "Week") {
        ListPrivate.ParsingWeek(type);
    }
    if ($('#ddlGubun').val() == "Month") {
        ListPrivate.ParsingMonth(type);
    }
    if ($('#ddlGubun').val() == "Quarter") {
        ListPrivate.ParsingQuarter(type);
    }
    if ($('#ddlGubun').val() == "Year") {
        ListPrivate.ParsingYear(type);
    }

    ListPrivate.Search();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListPrivate.ParsingWeek = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingWeek";

    ParamArray = {
        'startdate': $("#startDate").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessParsingWeek");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListPrivate.SuccessParsingWeek = function (data, status) {
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
ListPrivate.ParsingMonth = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingMonth";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessParsingMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 월 단위로 날짜 변환
***************************************************/
ListPrivate.SuccessParsingMonth = function (data, status) {
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
ListPrivate.ParsingQuarter = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingQuarter";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessParsingQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 분기 단위로 날짜 변환
***************************************************/
ListPrivate.SuccessParsingQuarter = function (data, status) {
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
ListPrivate.ParsingYear = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingYear";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessParsingYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 년 단위로 날짜 변환
***************************************************/
ListPrivate.SuccessParsingYear = function (data, status) {
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
ListPrivate.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivate.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListPrivate.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
ListPrivate.Search = function () {

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


    ListPrivate.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    //ListPrivate.LoadData("search");
    ListPrivate.GoPageLoad();
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
ListPrivate.SettingSearch = function () {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    AttendanceType = $("#ddlAttendanceType").val();

    //SearchField = $("#ddlSearchField").val();
    //SearchValue = $("#ctbSearch").val().securityText();

    //$("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ListPrivate.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        //ListPrivate.GetCount(); //검색 총 카운트 가져오기
        ListPrivate.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        //ListPrivate.GetCount(); //검색 총 카운트 가져오기
        ListPrivate.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListPrivate.LoadData("search");
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
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListPrivate.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ListPrivate.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetPrivateList";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'attendanceType': AttendanceType,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchType': SearchType
    };

    var obj = eval("ListPrivate.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListPrivate.SuccessPageList = function (data, status) {
    if (data.d != null) {
        if (data.d.length > 0) {
            $("#startDate").val(data.d[0].StartDate);
            $("#endDate").val(data.d[0].EndDate);
            $("#Register").html(" / <abbr title='" + data.d[data.d.length - 1].RegisterStr + "'><span style='font-weight:bold'>" + R_Register + "</span> " + data.d[data.d.length - 1].Register + "%</abbr>");
        }
    }

    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/JTListPrivate.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListPrivate.Resource();

    $("#HRContent").height($(document).height() - 420);

    if ($("#HRTable").height() < $(document).height() - 420) {
        $("#HRContent").height($("#HRTable").height() + 1);
    }

    ListPrivate.Tooltip();
}

ListPrivate.Tooltip = function () {
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
ListPrivate.Resource = function () {
    
    $("#r_calcdate")[0].innerHTML = R_CalcDate;     //일자
    $("#r_normal_work")[0].innerHTML = R_Normal_Work;     //기본근무
    $("#r_over_work")[0].innerHTML = R_Over_Work;     //연장근무
    $("#r_holi_hr")[0].innerHTML = R_Holi_HR;     //근태정보
    $("#r_reason")[0].innerHTML = R_Reason;     //비고(사유)

    $("#r_WS_Reg")[0].innerHTML = R_WS_Reg;     //출근등록
    $("#r_WSLocation")[0].innerHTML = R_Location;     //위치
    $("#r_WC_Reg")[0].innerHTML = R_WC_Reg;     //퇴근등록
    $("#r_WCLocation")[0].innerHTML = R_Location;     //위치
    $("#r_StandardReg")[0].innerHTML = R_StandardReg; //등록기준
    $("#r_WorkTime")[0].innerHTML = R_WorkTime;     //근무시간
    $("#r_OverTime")[0].innerHTML = R_WorkTime;     //근무시간
    $("#r_PRDocumentID")[0].innerHTML = R_DocumentID;     //결재승인

    $("#r_Attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;     //지각
    $("#r_Attendance_Early")[0].innerHTML = R_Attendance_Early;     //조퇴
    $("#r_Holi")[0].innerHTML = R_Holi;     //휴가
    $("#r_Etc")[0].innerHTML = R_Etc;     //기타
    $("#r_AllWorkTime")[0].innerHTML = R_AllWorkTime;     //총근무시간

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : kkzz52  
작성일자 : 2017-11-24
작성목적 : 엑셀
**********************************************************************************************/
ListPrivate.ExcelDown = function () {

    $("#hdStartDate").val($("#startDate").val());
    $("#hdEndDate").val($("#endDate").val());
    $("#hdSearchType").val(SearchType);
    $("#hdJan").val(Jan);

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': $("#startDate").val(),
        'endDate': $("#endDate").val(),
        'attendanceType': AttendanceType,
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
ListPrivate.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 사유 상세 보기
***************************************************/
ListPrivate.EtcView = function (_calcDate, _dayWeek, _HUDocumentID, _PRDocumentID
    , _userName, _deptName, _titleName
    , _bHoliAll, _bLate, _bEarly, _bOver
    , _HoliGubun, _bLateGubun, _bEarlyGubun, _bOverGubun
    , _HoliGubun2, _LateRemark, _EarlyRemark, _OverRemark) {

    ParamArray = {
        'calcDate': _calcDate,
        'dayWeek': _dayWeek,
        'HUDocumentID': _HUDocumentID,
        'PRDocumentID': _PRDocumentID,
        'userName': _userName,
        'deptName': _deptName,
        'titleName': _titleName,        
        'bHoliAll': _bHoliAll,
        'bLate': _bLate,
        'bEarly': _bEarly,
        'bOver': _bOver,
        'HoliGubun': _HoliGubun,
        'bLateGubun': _bLateGubun,
        'bEarlyGubun': _bEarlyGubun,
        'bOverGubun': _bOverGubun,
        'HoliGubun2': _HoliGubun2,
        'LateRemark': _LateRemark,
        'EarlyRemark': _EarlyRemark,
        'OverRemark': _OverRemark       
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/HR/Content/Popup/ViewPrivateEtc.aspx?p=" + GetEncryptValue;

    PopupWin(600, 300, url, "", "", true);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
ListPrivate.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListPrivate.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'attendanceType': AttendanceType,
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
        'userID': UserID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'deptState': DeptState
    };

    CallEncrypt(CallGetParam());
    location = "ListPrivate.aspx?p=" + GetEncryptValue;
}