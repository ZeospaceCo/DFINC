UserList = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            UserList.LoadData("");
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
            UserList.SavePageSize();
            UserList.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                UserList.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            UserList.Search();
        });

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#startDate").val(StartDate);

        $("#endDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").val(EndDate);

        UserList.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 검색
***************************************************/
UserList.Search = function () {

    UserList.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    UserList.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 검색 필드 설정하기
***************************************************/
UserList.SettingSearch = function () {
    WorkType = $("#ddlWorkType").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 리스트 초기화
***************************************************/
UserList.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        UserList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        UserList.GetCount(); //검색 총 카운트 가져오기
        UserList.GetPageList(); //검색 리스트 가져오기  
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                UserList.LoadData("search");
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
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserList.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Attendance",
        'pageSize': $("#ddlListCount").val(),
        'types': "UserList"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserList.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserList.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 웹서비스 카운트
**********************************************************************************************/
UserList.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetUserListCount";
    ParamArray = {
        'deptID': DeptID,
        'accessAuth': AccessAuth,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'workType': WorkType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
UserList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 :  리스트 호출
**********************************************************************************************/
UserList.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetUserList";

    ParamArray = {
        'deptID': DeptID,
        'accessAuth': AccessAuth,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'workType': WorkType
    };

    var obj = eval("UserList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
UserList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTUserList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    UserList.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
UserList.Resource = function () {
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_dutyname")[0].innerHTML = R_DutyName;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-12
작성목적 : 데이터 상세보기
**********************************************************************************************/
UserList.View = function (_userID, _nameBase) {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': _userID,
        'userName': _nameBase,
        'deptID': DeptID,
        'accessAuth': AccessAuth,
        'callPage': 'Admin',
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'workType': WorkType
    };

    CallEncrypt(CallGetParam());
    location = "UserListView.aspx?p=" + GetEncryptValue;
}