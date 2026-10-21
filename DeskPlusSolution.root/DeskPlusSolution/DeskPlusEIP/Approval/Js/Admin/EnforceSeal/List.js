List = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
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

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.Search();
        });

        //추가 
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_MSG_NeedAddDate);
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //List.LoadData("search");
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/EnforceSeal/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {

    $("#r_num")[0].innerHTML = R_Number;
    $("#r_deptname")[0].innerHTML = R_Deptname;
    $("#r_exp")[0].innerHTML = R_Exp;
    $("#r_isstampstr")[0].innerHTML = R_Isstampstr;
    $("#r_isadminstr")[0].innerHTML = R_Isadminstr;
    $("#r_writedate")[0].innerHTML = R_Writedate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 웹서비스 결재상황조회 상신한 문서 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/List.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 정렬
**********************************************************************************************/
List.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    List.LoadData("search");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [추가]
***************************************************/
List.Add = function () {

    ParamArray = {
        'p': 'p'
    };
    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [수정]
***************************************************/
List.Mod = function (_stampID, _deptID) {
    ParamArray = {
        'stampID': _stampID
        , 'deptID': _deptID
    };
    CallEncrypt(CallGetParam());
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [삭제]
***************************************************/
List.Del = function () {

    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrDelItem); return; }

    if (confirm(R_Msg_Del)) {
        ParamArray = { 'id': selvals };

        var url = "/DeskPlusEIP/Approval/Ws/Admin/EnforceSeal/List.asmx/DelList";
        var obj = eval("List.Search()");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


