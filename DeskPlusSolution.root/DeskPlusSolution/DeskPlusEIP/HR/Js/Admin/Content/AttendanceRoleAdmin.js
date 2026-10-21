AttendanceRoleAdmin = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AttendanceRoleAdmin.LoadData("");
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
            AttendanceRoleAdmin.SavePageSize();
            AttendanceRoleAdmin.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AttendanceRoleAdmin.Search();
            }
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AttendanceRoleAdmin.Add();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            AttendanceRoleAdmin.Del();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AttendanceRoleAdmin.Search();
        });

        AttendanceRoleAdmin.LoadData("");
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 검색
***************************************************/
AttendanceRoleAdmin.Search = function () {

    AttendanceRoleAdmin.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    AttendanceRoleAdmin.LoadData("search");
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 검색 필드 설정하기
***************************************************/
AttendanceRoleAdmin.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 리스트 초기화
***************************************************/
AttendanceRoleAdmin.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AttendanceRoleAdmin.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AttendanceRoleAdmin.GetCount(); //검색 총 카운트 가져오기
        AttendanceRoleAdmin.GetPageList(); //검색 리스트 가져오기  
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AttendanceRoleAdmin.LoadData("search");
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
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AttendanceRoleAdmin.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/AttendanceRoleAdmin.asmx/SavePageSize";

    ParamArray = {
        'folderID': menuID,
        'pageSize': $("#ddlListCount").val(),
        'types': "Admin"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AttendanceRoleAdmin.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AttendanceRoleAdmin.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 웹서비스 카운트
**********************************************************************************************/
AttendanceRoleAdmin.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/AttendanceRoleAdmin.asmx/GetListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AttendanceRoleAdmin.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AttendanceRoleAdmin.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 :  리스트 호출
**********************************************************************************************/
AttendanceRoleAdmin.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/AttendanceRoleAdmin.asmx/GetPageList";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("AttendanceRoleAdmin.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
AttendanceRoleAdmin.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Admin/Content/JTAttendanceRoleAdmin.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    AttendanceRoleAdmin.Resource();
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AttendanceRoleAdmin.Resource = function () {
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_dutyname")[0].innerHTML = R_DutyName;
    $("#r_Department")[0].innerHTML = R_View + R_DeptName;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2018-01-05
작성목적 : 공유자 추가 이동
**********************************************************************************************/
AttendanceRoleAdmin.SelectUserPop = function (arg) {
    SelectUser = arg;

    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "AttendanceRoleAdminAdd.aspx?p=" + GetEncryptValue;
    PopupWin(800, 560, url, "", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 추가
**********************************************************************************************/
AttendanceRoleAdmin.Add = function () {

    ParamArray = {
        'roleID': RoleID,
        'gubun': "HRAttendance",
        'currentPage': CurrentPage,
        'recordPerPage': $('#ddlListCount').val(),
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/EIP/Admin/Auth/Mod.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
AttendanceRoleAdmin.Del = function () {

    var selValue = AttendanceRoleAdmin.CheckedValue();

    if (selValue.length == 0) {
        alert(R_CheckDeleteList);
        return;
    }

    if (window.confirm(R_ConfirmDelete)) {

        for (var i = 0; i < selValue.length; i++) {
            AttendanceRoleAdmin.DelProc(selValue[i].split("_")[0], selValue[i].split("_")[1], selValue[i].split("_")[2], selValue[i].split("_")[3]);
        }

        AttendanceRoleAdmin.Search();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
AttendanceRoleAdmin.DelProc = function (p_companyID, p_roleID, p_userID, p_Types) {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/List.asmx/DelRoleUser";


    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': p_companyID,
        'roleID': p_roleID,
        'userID': p_userID,
        'types': p_Types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AttendanceRoleAdmin.SuccessDel");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이벤트
**********************************************************************************************/
AttendanceRoleAdmin.SuccessDel = function (data, status) {
    if (data.d == true) {

    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
AttendanceRoleAdmin.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
AttendanceRoleAdmin.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}