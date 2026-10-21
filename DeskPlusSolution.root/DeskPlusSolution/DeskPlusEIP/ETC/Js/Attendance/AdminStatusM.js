AdminStatusM = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AdminStatusM.LoadData("");
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
            AdminStatusM.SavePageSize();
            AdminStatusM.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminStatusM.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AdminStatusM.Search();
        });

        //년변경
        $('#ddlYear').change(function (e) {
            e.preventDefault();
            //변경
            AdminStatusM.Search();
        });

        //월변경
        $('#ddlMonth').change(function (e) {
            e.preventDefault();
            //변경
            AdminStatusM.Search();
        });;

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            AdminStatusM.Admin();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            AdminStatusM.ExcelDown();
        });
  
        //데이터 로드
        AdminStatusM.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminStatusM.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminStatus.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Attendance",
        'pageSize': $("#ddlListCount").val(),
        'types': "AdminStatusM"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminStatusM.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminStatusM.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-13
작성목적 : 검색
***************************************************/
AdminStatusM.Search = function () {

    AdminStatusM.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    AdminStatusM.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 검색 필드 설정하기
***************************************************/
AdminStatusM.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    StatusYear = $('#ddlYear').val();                //년
    StatusMonth = $('#ddlMonth').val();              //월
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
AdminStatusM.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AdminStatusM.GetCount(); //검색 총 카운트 가져오기
        AdminStatusM.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AdminStatusM.GetCount(); //검색 총 카운트 가져오기
        AdminStatusM.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AdminStatusM.LoadData("search");
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
AdminStatusM.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminStatus.asmx/GetStatusMListCount";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'statusMonth': StatusMonth,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue      //검색어
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminStatusM.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AdminStatusM.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
AdminStatusM.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Attendance/AdminStatus.asmx/GetStatusMList";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'statusMonth': StatusMonth,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("AdminStatusM.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
AdminStatusM.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Attendance/JTAdminStatusM.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    AdminStatusM.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AdminStatusM.Resource = function () {
 
    $("#r_attendance_Department")[0].innerHTML = R_Attendance_Department;       //부서        
    $("#r_gubun")[0].innerHTML = R_Gubun;                                       //구분
    $("#r_attendance_Today")[0].innerHTML = R_Attendance_Start;    //금일출근
    $("#r_attendance_Start")[0].innerHTML = R_Attendance_normal + R_Attendance_Start;   //정상출근
    $("#r_attendance_Tardy")[0].innerHTML = R_Attendance_Tardy;                 //지각
    $("#r_attendance_End")[0].innerHTML = R_Attendance_End;                     //퇴근

    $(".r_attendance_personnel").text(R_Attendance_personnel);         //인원
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
AdminStatusM.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'statusYear': StatusYear,
        'statusMonth': StatusMonth,
        'searchField': SearchField,     //검색필드
        'searchValue': SearchValue,      //검색어
        'totalCount': TotalItemCount,
        'pageSize': "99999",
        'pageSizePageNo': "1"
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
AdminStatusM.TabPageMove = function (arg) {

    switch (arg) {
        case "day":
            location.href = "AdminStatusD.aspx"
            break;
        case "month":
            location.href = "AdminStatusM.aspx"
            break;
        case "year":
            location.href = "AdminStatusY.aspx"
            break;
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
AdminStatusM.Admin = function (_arg) {
    var url = "AdminDefault.aspx";
    location.href = url;
}
