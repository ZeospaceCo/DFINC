AdminRole = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AdminRole.LoadData("");
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
            AdminRole.SavePageSize();
            AdminRole.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminRole.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AdminRole.Search();
        });

        AdminRole.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 검색
***************************************************/
AdminRole.Search = function () {

    AdminRole.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    AdminRole.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 검색 필드 설정하기
***************************************************/
AdminRole.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 초기화
***************************************************/
AdminRole.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AdminRole.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AdminRole.GetCount(); //검색 총 카운트 가져오기
        AdminRole.GetPageList(); //검색 리스트 가져오기  
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AdminRole.LoadData("search");
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
작성일자 : 2018-01-05
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminRole.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Attendance",
        'pageSize': $("#ddlListCount").val(),
        'types': "AdminRole"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminRole.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminRole.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 웹서비스 카운트
**********************************************************************************************/
AdminRole.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/GetListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminRole.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AdminRole.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 :  리스트 호출
**********************************************************************************************/
AdminRole.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminRole.asmx/GetPageList";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("AdminRole.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
AdminRole.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTAdminRole.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    AdminRole.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AdminRole.Resource = function () {
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_dutyname")[0].innerHTML = R_DutyName;
    $("#r_Attendance_Department")[0].innerHTML = R_View + R_Attendance_Department;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 공유자 추가 이동
**********************************************************************************************/
AdminRole.SelectUserPop = function (arg) {
    SelectUser = arg;

    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "AdminRoleAdd.aspx?p=" + GetEncryptValue;
    PopupWin(700, 560, url, "", "", true);
}