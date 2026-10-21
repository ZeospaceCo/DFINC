WriteCount = {}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            WriteCount.LoadData("");
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

        // 검색입력 초기화
        $("#startEndDate").show();
        $("#startDate").datepick();
        $("#endDate").datepick();

        //검색입력필드 엔터 이벤트 기술
        $("#CTBSearch").keydown(function (event) {
            WriteCount.KeyEvents();
        });

        $("#startDate").datepick();
        $("#endDate").datepick();

        $("#startEndDate").show();

        // 검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            WriteCount.Search();
        });

        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);

        WriteCount.RegDate();

        WriteCount.LoadData("");
    });
});

WriteCount.LoadData = function (gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        WriteCount.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        WriteCount.GetCount(); //검색 총 카운트 가져오기
        WriteCount.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                WriteCount.LoadData("search");
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
작성목적 : 등록일자 활성화 여부
***************************************************/
WriteCount.RegDate = function () {
    $("#startEndDate").show();
    $("#startDate").val("");
    $("#endDate").val("");
    $("#startDate").datepick();
    $("#endDate").datepick();
}

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
WriteCount.Search = function () {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    if ($('input[id="rdoAsc"]:checked').val() == "0") {
        Order = " COUNT(WriteUserID)  ASC";
    }
    else {
        Order = " COUNT(WriteUserID) DESC  ";
    }

    RecordPerPage = $("#DDLSearchType").val()
    CurrentPage = 1; //1페이지로 이동처리

    //Searchkey = $("#DDLSearchType").val();

    WriteCount.LoadData("search");
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
WriteCount.GetPageList = function () {
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/WriteCount.asmx/GetWriteCount";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'StartDate': StartDate,
        'EndDate': EndDate,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WriteCount.SuccessPageList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
WriteCount.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Admin/Statistic/JTWriteCount.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    WriteCount.Resource();
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
WriteCount.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/WriteCount.asmx/GetWriteCountCount";

    ParamArray = {
        'CategoryID': "",
        'StartDate': StartDate,
        'EndDate': EndDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WriteCount.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
WriteCount.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
WriteCount.Sort = function (col) {
    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    WriteCount.LoadData("search");
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

WriteCount.Resource = function () {
    $(".r_writeCount_th1")[0].innerHTML = R_Rank;
    $(".r_writeCount_th2")[0].innerHTML = R_Deft;
    $(".r_writeCount_th3")[0].innerHTML = R_User;
    $(".r_writeCount_th4")[0].innerHTML = R_WriteCnt;

    if ($(".center")[0] != undefined) {
        $(".center")[0].innerHTML = R_NotExistList;
    }
}

/**************************************************
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 키이벤트
***************************************************/
WriteCount.KeyEvents = function () {
    if (event.keyCode == 13) {
        WriteCount.Search();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 보기
***************************************************/
WriteCount.View = function (categoryID, listID, groupID, depth) {
    ParamArray = {
        'categoryID': categoryID,
        'listID': listID,
        'currentPage': CurrentPage,
        'StartDate': StartDate,
        'EndDate': EndDate,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}