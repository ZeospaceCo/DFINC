UserStatusD = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            UserStatusD.LoadData("");
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
            UserStatusD.SavePageSize();
            UserStatusD.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                UserStatusD.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            UserStatusD.Search();
        });

        //년변경
        $('#ddlYear').change(function (e) {
            e.preventDefault();
            //변경
            UserStatusD.Search();
        });

        //월변경
        $('#ddlMonth').change(function (e) {
            e.preventDefault();
            //변경
            UserStatusD.Search();
        }); ;

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            UserStatusD.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            UserStatusD.ExcelDown();
        });

        UserStatusD.Init();
        //데이터 로드
        UserStatusD.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
UserStatusD.Init = function () {

    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    $("#startDate").val(StartDate);
    $("#endDate").val(EndDate);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserStatusD.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Attendance",
        'pageSize': $("#ddlListCount").val(),
        'types': "UserStatusD"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserStatusD.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
UserStatusD.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
UserStatusD.Search = function () {

    UserStatusD.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    UserStatusD.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 검색 필드 설정하기
***************************************************/
UserStatusD.SettingSearch = function () {
    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();
    WorkType = $("#ddlWorkType").val();
    AttendanceType = $("#ddlAttendanceType").val();

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
UserStatusD.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        UserStatusD.GetCount(); //검색 총 카운트 가져오기
        UserStatusD.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        UserStatusD.GetCount(); //검색 총 카운트 가져오기
        UserStatusD.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                UserStatusD.LoadData("search");
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
UserStatusD.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetStatusDListCount";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'startDate': StartDate,
        'endDate': EndDate,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,
        'workType': WorkType,
        'attendanceType': AttendanceType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserStatusD.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
UserStatusD.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
UserStatusD.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/UserStatus.asmx/GetStatusDList";

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
        'workType': WorkType,
        'attendanceType': AttendanceType
    };

    var obj = eval("UserStatusD.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
UserStatusD.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTUserStatusD.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    UserStatusD.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
UserStatusD.Resource = function () {

    $("#r_attendance_Userinformation")[0].innerHTML = R_Attendance_Userinformation;     //사용자정보        
    $("#r_attendance_Department")[0].innerHTML = R_Attendance_Department;               //부서     
    $("#r_attendance_user")[0].innerHTML = R_Attendance_user;                           //사용자
    $("#r_attendance_Date")[0].innerHTML = R_Attendance_Date;                           //일자    
    $("#r_attendance_Start")[0].innerHTML = R_Attendance_Start + " / " + R_Attendance_Tardy;                         //출근    
    $("#r_time1")[0].innerHTML = R_Time;                                                //기간        
    //$("#r_attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;                       //지각    
    $("#r_attendance_End")[0].innerHTML = R_Attendance_End + " / " + R_Attendance_Early;                             //퇴근    
    //$("#r_attendance_Early")[0].innerHTML = R_Attendance_Early;                       //조퇴
    $("#r_time2")[0].innerHTML = R_Time;                                                //시간
    $("#r_attendance_vacation")[0].innerHTML = R_Attendance_vacation;                   //휴가    
    $("#r_attendance_Reason")[0].innerHTML = R_Attendance_Reason;                       //사유

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : 이지원  
작성일자 : 2017-11-24
작성목적 : 엑셀
**********************************************************************************************/
UserStatusD.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'startDate': StartDate,
        'endDate': EndDate,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': "99999",
        'pageSizePageNo': "1",
        'workType': WorkType,
        'attendanceType': AttendanceType
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
UserStatusD.TabPageMove = function (arg) {

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
작성일자 : 2018-04-17
작성목적 : 휴가원 문서보기
***************************************************/
UserStatusD.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
UserStatusD.Admin = function (_arg) {
    var url = "AdminDefault.aspx";
    location.href = url;
}