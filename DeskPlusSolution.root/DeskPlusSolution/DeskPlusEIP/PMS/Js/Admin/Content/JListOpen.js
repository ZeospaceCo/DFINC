ListOpen = {}

/**************************************************
초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListOpen.LoadData();
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
            ListOpen.Search();
        });

        //승인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ListOpen.Approval();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListOpen.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListOpen.Search();
            }
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListOpen.RegDate();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        ListOpen.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ListOpen.Init = function () {
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

    ListOpen.LoadData();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListOpen.RegDate = function () {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListOpen.LoadData = function () {
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListOpen.GetCount(); //검색 총 카운트 가져오기
        ListOpen.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListOpen.LoadData();
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
        loadBoardSearchData();
    }, 100);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListOpen.SettingSearch = function () {
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
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ListOpen.Search = function () {
    var response = ListOpen.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    ListOpen.LoadData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ListOpen.GetPageList = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Admin/Content/ListOpen.asmx/GetPageList";

    //파라미터값 배열에 담기

    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderID': FolderID,
        'status': Status
    };

    var obj = eval("ListOpen.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ListOpen.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/PMS/Jt/Admin/Content/JTListOpen.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListOpen.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListOpen.Resource = function () {
    $("#r_projectname")[0].innerHTML = R_projectname;
    $("#r_deptname")[0].innerHTML = R_deptname;
    $("#r_maker")[0].innerHTML = R_username;
    $("#r_startdate")[0].innerHTML = R_planstartdate;
    $("#r_enddate")[0].innerHTML = R_planenddate;
    $("#r_status")[0].innerHTML = R_status;
    $("#r_process")[0].innerHTML = R_process;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ListOpen.GetCount = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Admin/Content/ListOpen.asmx/GetListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderID': FolderID,
        'status': Status
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListOpen.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListOpen.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-09
작성목적 : 전체 체크박스
**********************************************************************************************/
ListOpen.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-23
작성목적 : 선택된 값 가져오기
***************************************************/
ListOpen.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-23
작성목적 : 승인 처리  (웹서비스 호출)
**********************************************************************************************/
ListOpen.Approval = function () {
    var selValue = ListOpen.CheckedValue();

    if (selValue != "") {
        if (window.confirm(Admin_ListOpen_IsReceipt)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'projectID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/PMS/Ws/Admin/Content/ListOpen.asmx/Mod";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListOpen.SuccessApproval");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_ConfirmProject);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-23
작성목적 : 승인 처리  (웹서비스 값 받기)
***************************************************/
ListOpen.SuccessApproval = function (data, status) {
    if (data.d == true) {
        ListOpen.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}