ListPrivateMod = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListPrivateMod.LoadData("");
            ListPrivateMod.GoPageLoad();
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
            ListPrivateMod.SavePageSize();
            ListPrivateMod.Search();
        });

        $('#ddlDeptList').change(function () {
            DeptID = $('#ddlDeptList').val();
            ListPrivateMod.Search();
        });

        $('#ddlActiveDept').change(function () {
            DeptState = $('#ddlActiveDept').val();
            ListPrivateMod.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListPrivateMod.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListPrivateMod.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListPrivateMod.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListPrivateMod.ExcelDown();
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

            ListPrivateMod.SearchType();
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ListPrivateMod.LoadYear($('#ddlYear').val());
            }

            ListPrivateMod.SearchType();
        });

        //분기 구분 선택
        $('#ddlQuarter').change(function () {
            ListPrivateMod.SearchType();
        });

        //월 구분 선택
        $('#ddlMonth').change(function () {
            ListPrivateMod.SearchType();
        });

        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListPrivateMod.ParsingDate('left');
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListPrivateMod.ParsingDate('right');
        });

        //맨밑으로
        $('#cbtnDown').click(function (e) {
            var pos = $(document).height();
            $("html, body").animate({ scrollTop: pos }, 100);

        });

        ListPrivateMod.Init();

        //데이터 로드
        ListPrivateMod.LoadData("");

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 검색
***************************************************/
ListPrivateMod.SearchType = function () {
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
        ListPrivateMod.Month();
    }
    if ($('#ddlGubun').val() == "Quarter") {
        $('#liYear').show();
        $('#liQuarter').show();
        $('#liMonth').hide();

        SearchType = "Quarter";
        ListPrivateMod.Quarter();
    }
    if ($('#ddlGubun').val() == "Year") {
        $('#liYear').show();
        $('#liQuarter').hide();
        $('#liMonth').hide();

        SearchType = "Year";
        ListPrivateMod.Year();
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListPrivateMod.Month = function (month) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetMonthDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 월간 검색
***************************************************/
ListPrivateMod.SuccessMonth = function (data, status) {
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
ListPrivateMod.Quarter = function (quarter) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetQuarterDate";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 분기 검색
***************************************************/
ListPrivateMod.SuccessQuarter = function (data, status) {
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
ListPrivateMod.Year = function (year) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetYearDate";

    ParamArray = {
        'year': $("#ddlYear").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 년간 검색
***************************************************/
ListPrivateMod.SuccessYear = function (data, status) {
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
ListPrivateMod.Init = function () {
    ListPrivateMod.LoadYear(Year);

    ListPrivateMod.SearchType();

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
ListPrivateMod.LoadYear = function (p_year) {
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
ListPrivateMod.ParsingDate = function (type) {
    if ($('#ddlGubun').val() == "Week") {
        ListPrivateMod.ParsingWeek(type);
    }
    if ($('#ddlGubun').val() == "Month") {
        ListPrivateMod.ParsingMonth(type);
    }
    if ($('#ddlGubun').val() == "Quarter") {
        ListPrivateMod.ParsingQuarter(type);
    }
    if ($('#ddlGubun').val() == "Year") {
        ListPrivateMod.ParsingYear(type);
    }

    ListPrivateMod.Search();
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListPrivateMod.ParsingWeek = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingWeek";

    ParamArray = {
        'startdate': $("#startDate").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessParsingWeek");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 주간 단위로 날짜 변환
***************************************************/
ListPrivateMod.SuccessParsingWeek = function (data, status) {
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
ListPrivateMod.ParsingMonth = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingMonth";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'month': $("#ddlMonth").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessParsingMonth");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 월 단위로 날짜 변환
***************************************************/
ListPrivateMod.SuccessParsingMonth = function (data, status) {
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
ListPrivateMod.ParsingQuarter = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingQuarter";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'quarter': $("#ddlQuarter").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessParsingQuarter");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 분기 단위로 날짜 변환
***************************************************/
ListPrivateMod.SuccessParsingQuarter = function (data, status) {
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
ListPrivateMod.ParsingYear = function (type) {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivate.asmx/GetParsingYear";

    ParamArray = {
        'year': $("#ddlYear").val(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessParsingYear");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-01-30
작성목적 : 년 단위로 날짜 변환
***************************************************/
ListPrivateMod.SuccessParsingYear = function (data, status) {
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
ListPrivateMod.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivateMod.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListPrivateMod.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
ListPrivateMod.Search = function () {

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


    ListPrivateMod.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    //ListPrivateMod.LoadData("search");
    ListPrivateMod.GoPageLoad();
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
ListPrivateMod.SettingSearch = function () {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    AttendanceType = $("#ddlAttendanceType").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ListPrivateMod.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListPrivateMod.GetCount(); //검색 총 카운트 가져오기
        ListPrivateMod.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListPrivateMod.GetCount(); //검색 총 카운트 가져오기
        ListPrivateMod.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListPrivateMod.LoadData("search");
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
ListPrivateMod.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivateMod.asmx/GetPrivateModListCount";

    ParamArray = {
        'deptID': DeptID,
        'startDate': StartDate,
        'endDate': EndDate,
        'attendanceType': AttendanceType,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,
        'searchType': SearchType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListPrivateMod.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListPrivateMod.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ListPrivateMod.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivateMod.asmx/GetPrivateModList";

    ParamArray = {
        'deptID': DeptID,
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

    var obj = eval("ListPrivateMod.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListPrivateMod.SuccessPageList = function (data, status) {
    if (data.d != null) {
        if (data.d.length > 0) {
            $("#startDate").val(data.d[0].StartDate);
            $("#endDate").val(data.d[0].EndDate);
        }
    }

    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/JTListPrivateMod.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListPrivateMod.Resource();

    $("#HRContent").height($(document).height() - 420);
    if ($("#HRTable").height() < $(document).height() - 420) {
        $("#HRContent").height($("#HRTable").height() + 1);
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListPrivateMod.Resource = function () {

    $("#r_Name")[0].innerHTML = R_Name;     //이름
    $("#r_TitleName2")[0].innerHTML = R_TitleName2;     //직급
    $("#r_calcdate")[0].innerHTML = R_CalcDate;     //일자
    $("#r_normal_work")[0].innerHTML = R_Normal_Work;     //기본근무
    $("#r_over_work")[0].innerHTML = R_Over_Work;     //연장근무
    $("#r_holi_hr")[0].innerHTML = R_Holi_HR;     //근태정보
    $("#r_reason")[0].innerHTML = R_Reason;     //비고(사유)

    $("#r_WS_Reg")[0].innerHTML = R_WS_Reg;     //출근등록
    $("#r_WSLocation")[0].innerHTML = R_Location;     //위치
    $("#r_WC_Reg")[0].innerHTML = R_WC_Reg;     //퇴근등록
    $("#r_WCLocation")[0].innerHTML = R_Location;     //위치
    $("#r_History")[0].innerHTML = R_History;     //수정내역
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
ListPrivateMod.ExcelDown = function () {

    $("#hdStartDate").val($("#startDate").val());
    $("#hdEndDate").val($("#endDate").val());
    $("#hdSearchType").val(SearchType);

    ParamArray = {
        'deptID': DeptID,
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
ListPrivateMod.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2019-04-17
작성목적 : 출퇴근 수정 페이지 이동하기 전 중복 체크
**********************************************************************************************/
ListPrivateMod.CheckInfo = function (_attID, _userID, _deptID, _pathID, _dayWeek, _workDate, _bSysHoli, _userName, _deptName, _titleName) {

    if (_attID == "0") {
        var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivateMod.asmx/GetAttendanceByUserID";

        ParamArray = {
            'attID': _attID,
            'userID': _userID,
            'deptID': _deptID,
            'pathID': _pathID,
            'dayWeek': _dayWeek,     
            'workDate': _workDate,
            'bSysHoli': _bSysHoli,
            'userName': _userName,
            'deptName': _deptName,
            'titleName': _titleName
        };

        var obj = eval("ListPrivateMod.SuccessCheckInfo");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);    
    }
    else {
        ListPrivateMod.GoList(_attID, _userID, _deptID, _pathID, _dayWeek, _workDate, _bSysHoli, _userName, _deptName, _titleName);        
    }
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2019-04-17
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListPrivateMod.SuccessCheckInfo = function (data, status) {
    if (data.d != null) {
        if (data.d.length > 0) {
            var attCnt = data.d.split("^^")[0];
            var attID = data.d.split("^^")[1];
            var userID = data.d.split("^^")[2];
            var deptID = data.d.split("^^")[3];
            var pathID = data.d.split("^^")[4];
            var dayWeek = data.d.split("^^")[5];
            var workDate = data.d.split("^^")[6];
            var bSysHoli = data.d.split("^^")[7];
            var userName = data.d.split("^^")[8];
            var deptName = data.d.split("^^")[9];
            var titleName = data.d.split("^^")[10];

            if (attCnt == "0") {
                ListPrivateMod.GoList(attID, userID, deptID, pathID, dayWeek, workDate, bSysHoli, userName, deptName, titleName);
            }
            else {
                alert(R_Overlap); //타(겸직) 부서 정보로 등록되어 있습니다.
            }
        }
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-13
작성목적 : 출퇴근 수정 페이지로 이동
***************************************************/
ListPrivateMod.GoList = function (_attID, _userID, _deptID, _pathID, _dayWeek, _workDate, _bSysHoli, _userName, _deptName, _titleName) {

    ParamArray = {
        'attID': _attID,
        'userID': _userID,
        'deptID': _deptID,
        'pathID': _pathID,
        'dayWeek': _dayWeek,
        'workDate': _workDate,
        'isHoli': _bSysHoli,
        'userName': _userName,
        'deptName': _deptName,
        'titleName': _titleName
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/HR/Content/Popup/PrivateMod.aspx?p=" + GetEncryptValue;
    PopupWin(800, 300, url, "", "", true);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-13
작성목적 : 출퇴근 수정 히스토리 페이지로 이동
***************************************************/
ListPrivateMod.ViewHistory = function (_attID, _userID, _deptID, _dayWeek, _workDate) {

    ParamArray = {
        'attID': _attID,
        'userID': _userID,
        'deptID': _deptID,
        'dayWeek': _dayWeek,
        'workDate': _workDate
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/HR/Content/Popup/ViewHistory.aspx?p=" + GetEncryptValue;
    PopupWin(1000, 400, url, "", "", true);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-03-07
작성목적 : 사유 상세 보기
***************************************************/
ListPrivateMod.EtcView = function (_calcDate, _dayWeek, _HUDocumentID, _PRDocumentID
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
ListPrivateMod.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**************************************************
작성목적 : 사용자 정보
***************************************************/
ListPrivateMod.AccountInfo = function (_userID, _deptID, _pathID, _deptBase) {

    ParamArray = {
        'userID': _userID,
        'deptID': _deptID,
        'pathID': _pathID,
        'deptBase': _deptBase
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/HR/Content/Popup/AccountInfo.aspx?p=" + GetEncryptValue;

    PopupWin(500, 405, url, "", "", true);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListPrivateMod.GoPageLoad = function () {

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
        'deptState': DeptState
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/HR/Content/ListPrivateMod.aspx?p=" + GetEncryptValue;
}