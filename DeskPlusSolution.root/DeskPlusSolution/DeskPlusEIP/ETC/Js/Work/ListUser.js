ListUser = {}



/**********************************************************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListUser.LoadData("");
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
            ListUser.SavePageSize();
            ListUser.Search();
        });

        //년도
        $('#ddlYearField').change(function () {
            ListUser.Search();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListUser.ExcelDown();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListUser.Search();
        });


        //ListUser.LoadData("");
        ListUser.Init();
    });
});

/**************************************************
작성자   : ellepooky
작성일자 :2017-08-08
작성목적 : 초기화 하기
***************************************************/
ListUser.Init = function () {

    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    if (SearchTermValue != "") {
        $("#startDate").val(SearchTermValue.split(",")[0]);
        $("#endDate").val(SearchTermValue.split(",")[1]);
    }

    ListUser.LoadData("");
}

/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListUser.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Work/ListUser.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Pay",
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListUser.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListUser.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 검색
***************************************************/
ListUser.Search = function () {

    ListUser.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    ListUser.LoadData("search");
}


/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 검색 필드 설정하기
***************************************************/
ListUser.SettingSearch = function () {
    // 구분
    Gubun = $("#ddlGubun").val();

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

    return true;

}


/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 리스트 초기화
***************************************************/
ListUser.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListUser.GetCount();
        ListUser.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListUser.GetCount(); //검색 총 카운트 가져오기
        ListUser.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListUser.LoadData("search");
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
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 웹서비스  카운트  호출
**********************************************************************************************/
ListUser.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Work/ListUser.asmx/GetListCount";

    ParamArray = {
        'userID': UserID,
        'searchTermValue': SearchTermValue,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListUser.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListUser.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 :  리스트 호출
**********************************************************************************************/
ListUser.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Work/ListUser.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'userID': UserID,
        'searchTermValue': SearchTermValue,
        'gubun': Gubun,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("ListUser.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListUser.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Work/JTListUser.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListUser.Resource();
}

/**********************************************************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListUser.Resource = function () {
    $("#r_number")[0].innerHTML = R_Number;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_customername")[0].innerHTML = R_CustomerName;
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_duration")[0].innerHTML = R_Duration;
    $("#r_time")[0].innerHTML = R_Time;
    $("#r_worklist")[0].innerHTML = R_WorkList;
    $("#r_docview")[0].innerHTML = R_DocView;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 건 수
***************************************************/
ListUser.View = function (data) {

    var arr = [];

    var param = data.split(",");
    $.each(param, function (i, o) {
        arr[i] = o;
    });

    ParamArray = {
        'userID': arr[0],
        'name': arr[1],
        'all1': arr[2],
        'jan1': arr[3],
        'titleDate': $("#ddlYearField option:selected").text()
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/ETC/Work/Popup/ViewUser.aspx?p=" + GetEncryptValue;
    PopupWin(500, 202, url, "", "", true);
}


/**************************************************
작성자   : ellepooky
작성일자 : 2017-08-08
작성목적 : 문서보기
***************************************************/
ListUser.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}


/**********************************************************************************************
작성자   : ellepooky  
작성일자 : 2017-08-11
작성목적 : 엑셀
**********************************************************************************************/

ListUser.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'searchTermValue': SearchTermValue,
        'gubun': Gubun,
        'totalCount': TotalItemCount,
        'pageSize': TotalItemCount,
        'pageSizePageNo': 1,
        'order': Order
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}
