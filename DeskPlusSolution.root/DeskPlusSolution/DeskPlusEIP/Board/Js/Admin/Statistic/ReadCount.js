ReadCount = {}

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
            ReadCount.LoadData("");
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
            ReadCount.KeyEvents();
        });

        $("#startDate").datepick();
        $("#endDate").datepick();

        $("#startEndDate").show();

        // 검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            ReadCount.Search();
        });

        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);

        $(".clsDate").keydown(function (e) { return false; });

        ReadCount.RegDate();

        ReadCount.LoadData("");
    });
});

ReadCount.LoadData = function (gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ReadCount.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ReadCount.GetCount(); //검색 총 카운트 가져오기
        ReadCount.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ReadCount.LoadData("search");
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
            //loadBoardData();
        }
    }, 100);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ReadCount.RegDate = function () {
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
ReadCount.Search = function () {
    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();

    if (getStartDate == "" || getEndDate == "") {
        alert(R_ApplyRegDate);
        return;
    }
    else {
        var start_dates = getStartDate.split("-");
        var end_dates = getEndDate.split("-");

        var start = new Date(start_dates[0], start_dates[1], start_dates[2]).valueOf();
        var end = new Date(end_dates[0], end_dates[1], end_dates[2]).valueOf();

        if ((end - start) < 0) {
            alert(R_StartDateEndDate);
            return;
        }
    }

    SearchValue = getStartDate + "~" + getEndDate;
    //////////

    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    if ($('input[id="rdoAsc"]:checked').val() == "0") {
        Order = " ReadCount ";
    }
    else {
        Order = " ReadCount desc";
    }

    RecordPerPage = $("#DDLSearchType").val()
    CurrentPage = 1; //1페이지로 이동처리

    //Searchkey = $("#DDLSearchType").val();

    ReadCount.LoadData("search");
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ReadCount.GetPageList = function () {
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/ReadCount.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'optionUseDept': 0,
        'optionUseView': 0
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ReadCount.SuccessPageList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ReadCount.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Admin/Statistic/JTReadCount.htm?seq=' + Guids());
    $('#placeholder').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    $('#loading').html("");

    ReadCount.Resource();
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ReadCount.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/ReadCount.asmx/GetListCount";

    ParamArray = {
        'categoryID': CategoryID,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'optionUseDept': 0,
        'optionUseView': 0
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ReadCount.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ReadCount.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
ReadCount.Sort = function (col) {
    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    ReadCount.LoadData("search");
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ReadCount.Resource = function () {
    $(".r_readCount_th1")[0].innerHTML = R_Rank;
    $(".r_readCount_th2")[0].innerHTML = R_ForderPath;
    $(".r_readCount_th3")[0].innerHTML = R_Title;
    $(".r_readCount_th4")[0].innerHTML = R_Write;
    $(".r_readCount_th5")[0].innerHTML = R_WriteDay;
    $(".r_readCount_th6")[0].innerHTML = R_ReadCnt;

    if ($(".center")[0] != undefined) {
        $(".center")[0].innerHTML = R_NoContent;
    }

    if ($(".s_notice") != undefined) { $(".s_notice").text(R_Notice); }
}

/**************************************************
작성자   : daniel
작성일자 : 2011-04-11
작성목적 : 키이벤트
***************************************************/
ReadCount.KeyEvents = function () {
    if (event.keyCode == 13) {
        ReadCount.Search();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 보기
***************************************************/
ReadCount.View = function (categoryID, listID, groupID, depth) {
    ParamArray = {
        'categoryID': categoryID,
        'listID': listID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}