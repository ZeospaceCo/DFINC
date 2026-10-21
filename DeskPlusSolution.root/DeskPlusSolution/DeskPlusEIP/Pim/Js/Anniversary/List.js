List = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;

            List.LoadData();
        }
    }

    $(document).ready(function () {
        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

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

        // 검색입력 초기화
        var sField = $("#ddlSearchType").val();
        if (sField == "WriteDate") {
            $("#startEndDate").show();
            $("#ctbSearch").hide();

            $("#startDate").datepick($.datepicker.regional[PreLang]);
            $("#endDate").datepick($.datepicker.regional[PreLang]);

            if (SearchValue != "") {
                var dateStr = SearchValue.split("~");

                $("#startDate").val(dateStr[0])
                $("#endDate").val(dateStr[1])
            }
        }
        else {
            $("#startEndDate").hide();
            $("#ctbSearch").show();

            if (SearchValue != "") {
                $("#ctbSearch").val(SearchValue);
            }
        }

        //검색조건 변화
        $("#ddlSearchType").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        $('#selSize').change(function (e) {
            e.preventDefault();
            List.Search();
        });

        //기념일 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //내기념일
        $('#cbtnMySchedule').click(function (e) {
            e.preventDefault();
            document.location.href = "/DeskplusEip/pim/Schedule/ScheduleMain.aspx";
        });

        //엑셀
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            List.ExcelDown();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.LoadData();

        //__addEventListener(document, "click", List.click);
    });
});

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
List.ExcelDown = function () {
    ParamArray = {
        'companyID': CompanyID,
        'type': type,
        'opentype': opentype,
        'SearchKey': Searchkey,
        'SearchValue': SearchValue,
        'PageSize': PageSize,
        'PageSizePageNo': CurrentPage,
        'Order': Order,
        'StartTime': Starttime,
        'EndTime': Endtime,
        'TotalCount': TotalItemCount
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 :
**********************************************************************************************/
List.LoadData = function () {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.SearchSetting();

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
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
        loadBoardSearchData();
    }, 100);
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 검색
***************************************************/
List.Search = function () {
    //

    SearchValue = $("#ctbSearch").val().securityText();
    CurrentPage = 1; //1페이지로 이동처리
    Searchkey = $("#ddlSearchType").val();

    if (Searchkey == "Title") {
        $("#startDate").val('');
        $("#endDate").val('')
    }

    List.LoadData("search");
}

/***********************
작성자   : DANIEL
작성일자 : 2011년 8월 19일
작성목적 : 검색조건 설정
**********************/
List.SearchSetting = function () {
    type = 'J';
    opentype = $("#ddlCategory").val();
    Searchkey = $("#ddlSearchType").val();
    SearchValue = $("#ctbSearch").val().securityText();
    Starttime = $("#startDate").val();
    Endtime = $("#endDate").val();
    PageSize = $("#selSize").val();
    RecordPerPage = $("#selSize").val();

    var sField = $("#ddlSearchType").val();
    if (sField == "WriteDate") {
        if (Starttime.length == 0) {
            alert(InputSearchDay); //InputSearchDay
            return;
        }
        if (Endtime.length == 0) {
            alert(InputSearchDay); //InputSearchDay
            return;
        }

        var dtResponse = CompareDT(Starttime, Endtime, "lessequal", PreLangReal);

        if (dtResponse == false) {
            alert(R_StartDateEndDate);
            return;
        }
    }
    List.GetPageListCnt(); //검색 총 카운트 가져오기
    List.GetPageList(); //검색 리스트 가져오기
}

/**********************************************************************************************
작성자   : DANIEL
작성일자 : 2011년 8월 16일
작성목적 : 리스트 가져오기
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Anniversary/List.asmx/GetAnniversaryInfoList";

    ParamArray = {
        'companyID': CompanyID,
        'type': type,
        'opentype': opentype,
        'SearchKey': Searchkey,
        'SearchValue': SearchValue,
        'PageSize': PageSize,
        'PageSizePageNo': CurrentPage,
        'Order': Order,
        'StartTime': Starttime,
        'EndTime': Endtime,
        'TotalCount': TotalItemCount
    };

    var obj = eval("List.SuccessAnniversaryList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

List.SuccessAnniversaryList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Anniversary/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");
    List.Resource();
}

/**********************************************************************************************
작성자   : DANIEL
작성일자 : 2011년 8월 16일
작성목적 : 갯수 가져오기
**********************************************************************************************/
List.GetPageListCnt = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Anniversary/List.asmx/GetAnniversaryInfoListCnt";

    ParamArray = {
        'companyID': CompanyID,
        'type': type,
        'opentype': opentype,
        'SearchKey': Searchkey,
        'SearchValue': SearchValue,
        'StartTime': Starttime,
        'EndTime': Endtime
    };

    var obj = eval("List.SuccessListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

List.SuccessListCnt = function (data, status) {
    //alert(RecordPerPage);
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);

    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   :
작성일자 :
작성목적 :
***************************************************/
List.Add = function () {
    location.href = "AddN.aspx";
}

/**************************************************
작성자   : daniel
작성일자 : 2011-08-16
작성목적 : 시작일자 활성화 여부
***************************************************/
List.RegDate = function () {
    var sField = $("#ddlSearchType").val();
    if (sField == "WriteDate") {
        $("#startEndDate").show();
        $("#ctbSearch").hide();
        $("#ctbSearch").val("");

        $("#startDate").val("");
        $("#endDate").val("");

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    }
    else {
        $("#startEndDate").hide();
        $("#ctbSearch").show();
        $("#ctbSearch").val("");
    }
}

List.Resource = function () {
    $("#r_kindDesk")[0].innerHTML = col1;
    $("#r_title")[0].innerHTML = col2;
    $("#r_startTime")[0].innerHTML = col3;
    $("#r_userName")[0].innerHTML = col4;
    $("#divt1").text(TextEmpty);
}

List.View = function (sid) {
    ParamArray = {
        'ScheduleID': sid,
        'Searchkey': Searchkey,
        'SearchValue': SearchValue,
        'CurrentPage': CurrentPage,
        'Starttime': Starttime,
        'Endtime': Endtime,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}