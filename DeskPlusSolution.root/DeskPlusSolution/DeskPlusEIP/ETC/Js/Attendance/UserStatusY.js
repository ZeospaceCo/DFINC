UserStatusY = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            UserStatusY.LoadData("");
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
            UserStatusY.SavePageSize();
            UserStatusY.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                UserStatusY.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            UserStatusY.Search();
        });

        //년변경
        $('#ddlYear').change(function (e) {
            e.preventDefault();
            //변경
            UserStatusY.Search();
        });

        //월변경
        $('#ddlMonth').change(function (e) {
            e.preventDefault();
            //변경
            UserStatusY.Search();
        }); ;

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            UserStatusY.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            UserStatusY.ExcelDown();
        });

        //데이터 로드
        UserStatusY.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserStatusY.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Attendance",
        'pageSize': $("#ddlListCount").val(),
        'types': "UserStatusY"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserStatusY.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserStatusY.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
UserStatusY.Search = function () {

    UserStatusY.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    UserStatusY.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 검색 필드 설정하기
***************************************************/
UserStatusY.SettingSearch = function () {
    StatusYear = $('#ddlYear').val();                //년
    WorkType = $("#ddlWorkType").val();

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
UserStatusY.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        UserStatusY.GetCount(); //검색 총 카운트 가져오기
        UserStatusY.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        UserStatusY.GetCount(); //검색 총 카운트 가져오기
        UserStatusY.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                UserStatusY.LoadData("search");
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
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 카운트 호출
**********************************************************************************************/
UserStatusY.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetStatusYListCount";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,
        'workType': WorkType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserStatusY.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
UserStatusY.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
UserStatusY.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetStatusYList";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'workType': WorkType
    };

    var obj = eval("UserStatusY.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
UserStatusY.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTUserStatusY.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    UserStatusY.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
UserStatusY.Resource = function () {

    $("#r_attendance_Userinformation")[0].innerHTML = R_Attendance_Userinformation;  //사용자정보        
    $("#r_attendance_Department")[0].innerHTML = R_Attendance_Department;       //부서     
    $("#r_attendance_user")[0].innerHTML = R_Attendance_user;                   //사용자
    $("#r_gubun")[0].innerHTML = R_Gubun;                                       //구분
    $("#r_attendance_year")[0].innerHTML = StatusYear + R_Attendance_year;      //년
    $("#r_attendance_Today")[0].innerHTML = R_Attendance_Start;                 //금일출근
    $("#r_attendance_Start")[0].innerHTML = R_Attendance_normal + R_Attendance_Start;   //정상출근
    $("#r_attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;                 //지각
    $("#r_attendance_End")[0].innerHTML = R_Attendance_End;                     //퇴근

    $(".r_holi_Days").text(R_Holi_Days);                               //일수
    $(".r_attendance_percent").text(R_Attendance_percent);             //율
    $(".r_attendance_max").text(R_Attendance_max);                     //최대
    $(".r_attendance_min").text(R_Attendance_min);                     //최소
    $(".r_attendance_avg").text(R_Attendance_avg);                     //평균

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : 이지원  
작성일자 : 2017-11-24
작성목적 : 엑셀
**********************************************************************************************/
UserStatusY.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': "99999",
        'pageSizePageNo': "1",
        'workType': WorkType
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/******************************************
작성자   : 이지원
작성일자 : 2018-04-12
작성목적 : 탭 페이지 호출
****************************************/
UserStatusY.TabPageMove = function (arg) {

    switch (arg) {
        case "day":
            location.href = "UserStatusD.aspx"
            break;
        case "month":
            location.href = "UserStatusM.aspx"
            break;
        case "year":
            location.href = "UserStatusY.aspx"
            break;
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
UserStatusY.Admin = function (_arg) {
    var url = "AdminDefault.aspx";
    location.href = url;
}