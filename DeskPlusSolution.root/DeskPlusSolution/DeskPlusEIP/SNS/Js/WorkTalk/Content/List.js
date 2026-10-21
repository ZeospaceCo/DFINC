List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {
        RecordPerPage = $("#ddlListCount").val();

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

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModFolderUser();
            List.Search();
        });

        List.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    $("#startEndDate").hide(); //기간
    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    List.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 날짜 세팅
***************************************************/
List.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    RecordPerPage = $("#ddlListCount").val();
    CurrentPage = 1; //1페이지로 이동처리

    List.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var start_dates = getStartDate.dateCharRemove();
            var end_dates = getEndDate.dateCharRemove();

            if ((end_dates - start_dates) < 0) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadSNSData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSNSSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

        RecordPerPage = $("#ddlListCount").val();

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
            loadSNSSearchData();
        }
        else {
            loadSNSData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/SNS/Ws/WorkTalk/Content/List.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'menuID': MenuID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    if (MenuID == "2") {
        $('#phList').setTemplateURL('/DeskPlusEIP/SNS/Jt/WorkTalk/Content/JTShortRecvList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phList').processTemplate(data.d);
        $('#loading').html("");
    }
    else if (MenuID == "3") {
        $('#phList').setTemplateURL('/DeskPlusEIP/SNS/Jt/WorkTalk/Content/JTShortSendList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phList').processTemplate(data.d);
        $('#loading').html("");
    }
    else if (MenuID == "4") {
        $('#phList').setTemplateURL('/DeskPlusEIP/SNS/Jt/WorkTalk/Content/JTShortAppList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phList').processTemplate(data.d);
        $('#loading').html("");
    }
    else {
        $('#phList').setTemplateURL('/DeskPlusEIP/SNS/Jt/WorkTalk/Content/JTShortRecvList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#phList').processTemplate(data.d);
        $('#loading').html("");
    }

    List.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $("#r_categoryName")[0].innerHTML = R_CategoryName;
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_register")[0].innerHTML = R_Register;
    $("#r_regDate")[0].innerHTML = R_RegDate;

    if ($("#r_state")[0] != undefined) { $("#r_state")[0].innerHTML = R_State; }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/SNS/Ws/WorkTalk/Content/List.asmx/GetListCount";
    ParamArray = {
        'menuID': MenuID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 새창으로 게시물 보기
***************************************************/
List.PopupBoardViewList = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID,
        'currentPage': 1,
        'searchGroup': '',
        'searchField': '',
        'searchValue': '',
        'order': ''
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, 'PopupBoardViewList', '1200', '800', 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 새창으로 일정보기
***************************************************/
List.PopupScheduleViewList = function (p_companyID, p_categoryID, p_listID) {
    ParamArray = {
        'CompanyID': p_companyID,
        'ScheduleID': p_listID,
        'DisplayID': p_categoryID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "PopupScheduleViewList", "1200", "800")
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 새창으로 할일보기
***************************************************/
List.PopupToDoViewList = function (p_listID) {
    ParamArray = {
        'TodoID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/ToDo/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "PopupToDoViewList", "1200", "800")
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 문서보기(결재진행중/완료)
***************************************************/
List.PopupAppViewList = function (p_docID) {
    ParamArray = {
        'documentID': p_docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1200", "800")
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 모달창으로 게시판 댓글리스트 보기
***************************************************/
List.PopupBoardViewShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "../../Common/BoardTalk.aspx?p=" + GetEncryptValue;
    PopupWin(700, 520, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 모달창 일정댓글 보기
***************************************************/
List.PopupScheduleShort = function (p_scheduleID, p_displayID) {
    ParamArray = {
        'scheduleID': p_scheduleID,
        'displayID': p_displayID
    };

    CallEncrypt(CallGetParam());

    var url = "../../Common/ScheduleTalk.aspx?p=" + GetEncryptValue;
    var param = "p=" + GetEncryptValue;
    PopupWin(700, 520, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 모달창 할일댓글 보기
***************************************************/
List.PopupToDoShort = function (p_toDoID) {
    ParamArray = {
        'toDoID': p_toDoID
    };

    CallEncrypt(CallGetParam());

    var url = "../../Common/TodoTalk.aspx?p=" + GetEncryptValue;
    var param = "p=" + GetEncryptValue;
    PopupWin(700, 520, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-05-13
작성목적 : 문서보기(결재진행중/완료)
***************************************************/
List.PopupAppShort = function (p_docID, p_isComp, p_groupID, p_accountTag) {
    ParamArray = {
        'docID': p_docID,
        'groupID': p_groupID,
        'accountTag': p_accountTag,
        'isComp': p_isComp
    };
    CallEncrypt(CallGetParam());

    //var url = "/DeskPlusEIP/Approval/Common/Popup/ShortReplyList.aspx?p=" + GetEncryptValue;
    var url = "../../Common/ApprovalTalk.aspx?p=" + GetEncryptValue;
    PopupWin(700, 520, url, "", "", true);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModFolderUser = function () {
    var tmpTypes = "R";

    if (MenuID == "2") {
        tmpTypes = "R";
    }
    else if (MenuID == "3") {
        tmpTypes = "S";
    }
    else if (MenuID == "4") {
        tmpTypes = "A";
    }

    var url = "/DeskPlusEIP/SNS/Ws/WorkTalk/Content/List.asmx/ModFolderUser";
    ParamArray = {
        'pageSize': $("#ddlListCount").val(),
        'types': tmpTypes
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-05-14
작성목적 : 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}