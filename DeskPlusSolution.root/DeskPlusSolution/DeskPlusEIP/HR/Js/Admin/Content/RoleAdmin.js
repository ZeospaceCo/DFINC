RoleAdmin = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            RoleAdmin.LoadData("");
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
            RoleAdmin.SavePageSize();
            RoleAdmin.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                RoleAdmin.Search();
            }
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            RoleAdmin.Add();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            RoleAdmin.Del();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            RoleAdmin.Search();
        });

        RoleAdmin.LoadData("");
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 검색
***************************************************/
RoleAdmin.Search = function () {

    RoleAdmin.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    RoleAdmin.LoadData("search");
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 검색 필드 설정하기
***************************************************/
RoleAdmin.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 초기화
***************************************************/
RoleAdmin.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        RoleAdmin.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        RoleAdmin.GetCount(); //검색 총 카운트 가져오기
        RoleAdmin.GetPageList(); //검색 리스트 가져오기  
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                RoleAdmin.LoadData("search");
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
RoleAdmin.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/SavePageSize";

    ParamArray = {
        'folderID': menuID,
        'pageSize': $("#ddlListCount").val(),
        'types': "Admin"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RoleAdmin.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
RoleAdmin.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 웹서비스 카운트
**********************************************************************************************/
RoleAdmin.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/GetListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RoleAdmin.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
RoleAdmin.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 :  리스트 호출
**********************************************************************************************/
RoleAdmin.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/RoleAdmin.asmx/GetPageList";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("RoleAdmin.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
RoleAdmin.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Admin/Content/JTRoleAdmin.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    RoleAdmin.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
RoleAdmin.Resource = function () {
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_dutyname")[0].innerHTML = R_DutyName;
    $("#r_Department")[0].innerHTML = R_View + R_DeptName;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 공유자 추가 이동
**********************************************************************************************/
RoleAdmin.SelectUserPop = function (arg) {
    SelectUser = arg;

    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "RoleAdminAdd.aspx?p=" + GetEncryptValue;
    PopupWin(800, 560, url, "", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 추가
**********************************************************************************************/
RoleAdmin.Add = function () {

    ParamArray = {
        'roleID': RoleID,
        'gubun': "HRDept",
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
RoleAdmin.Del = function () {

    var selValue = RoleAdmin.CheckedValue();

    if (selValue.length == 0) {
        alert(R_CheckDeleteList);
        return;
    }

    if (window.confirm(R_ConfirmDelete)) {

        for (var i = 0; i < selValue.length; i++) {
            RoleAdmin.DelProc(selValue[i].split("_")[0], selValue[i].split("_")[1], selValue[i].split("_")[2], selValue[i].split("_")[3]);
        }

        RoleAdmin.Search();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
RoleAdmin.DelProc = function (p_companyID, p_roleID, p_userID, p_Types) {

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
    var obj = eval("RoleAdmin.SuccessDel");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이벤트
**********************************************************************************************/
RoleAdmin.SuccessDel = function (data, status) {
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
RoleAdmin.Checked = function () {

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
RoleAdmin.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}