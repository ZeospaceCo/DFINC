List = {}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {

        //이벤트
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

        // [추가]
        $('#cbtnAdd').click(function (e) { e.preventDefault(); List.Add(); });

        // [삭제]버튼
        $('#cbtnDel').click(function (e) { e.preventDefault(); List.Del(); });

        // [순서] 버튼
        $('#cbtnFormSeq').click(function (e) { e.preventDefault(); List.SetFormSeq(); });

        // [검색]버튼
        $('#cbtnSearch').click(function (e) { e.preventDefault(); List.Search(); });

        //검색란KeyEvt
        $("#ctbSearch").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); List.Search(); } });

        //달력입력란KeyEvt
        $(".clsDate").keydown(function (e) { return false; });

        //검색조건
        $("#ddlSearchField").change(function (e) { e.preventDefault(); List.ddlSearchChange(); });

        //검색조건(기간)
        $("#ddlSearchTerm").change(function (e) { e.preventDefault(); List.ddlSearchTermChange(); });

        //페이지 카운트
        $("#ddlListCount").change(function (e) { e.preventDefault(); RecordPerPage = $('#ddlListCount').val(); List.SavePageSize(); List.Search(); });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 검색필드 초기
***************************************************/
List.Init = function () {
    $("#startEndDate").hide(); //기간

    if (SearchTerm.toUpperCase() == "PERIOD") {
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

/***리스트 처리 및 페이징 시작***/

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 가져오기(WS)
***************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Preference/FavoriteForm/List.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 가져오기(WS)
***************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/FavoriteForm/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();    //다국어처리
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 다국어처리
***************************************************/
List.Resource = function () {

    $("#r_formname")[0].innerHTML = R_FormName;
    $("#r_foldername")[0].innerHTML = R_FolderName;
    $("#r_pathname")[0].innerHTML = R_PathName;
    $("#r_writedate")[0].innerHTML = R_WriteDate;

    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 총개수 가져오기(WS)
***************************************************/
List.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Preference/FavoriteForm/List.asmx/GetListCnt";

    ParamArray = {      //파라미터값 배열에 담기
        'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
    };

    var obj = eval("List.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 총개수 가져오기
***************************************************/
List.SuccessPageCnt = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 가져오기
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
        List.GetPageCnt();  //검색 총 카운트 가져오기
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

/***리스트 처리 및 페이징 끝***/



/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm.toUpperCase() == "PERIOD") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate); return false;
        }

        var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);
        if (dtResponse == false) {
            alert(R_StartDateEndDate); return false;
        }
        SearchTermValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [검색]
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    List.LoadData("search");
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 체크박스(전체)
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
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 체크박스(선택된 값 가져오기)
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [삭제]
***************************************************/
List.Del = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrDelItem); return; }

    if (confirm(R_Msg_Del)) {
        ParamArray = { 'id': selvals };

        var url = "/DeskPlusEIP/Approval/Ws/Preference/FavoriteForm/List.asmx/DelList";
        var obj = eval("List.SetInit()");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 검색(기간검색)-DDL
***************************************************/
List.ddlSearchTermChange = function () {

    $("#startDate").val(""); $("#endDate").val("");

    var sField = $("#ddlSearchTerm").val();
    if (sField == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [추가]
***************************************************/
List.Add = function () {

    ParamArray = {
        'callback': 'List.AddForm'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Preference/FavoriteForm/GetForm.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}
List.AddForm = function (_id, _vals) {

    if (_id != "" && _vals != "") {

        var ids = unescape(_id);            // FormID|FormID|FormID|FormID|

        var url = "/DeskPlusEIP/Approval/Ws/Preference/FavoriteForm/List.asmx/AddList";
        ParamArray = { 'id': ids };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SetInit()");

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "FavForm",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Setting'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [순서변경]
***************************************************/
List.SetFormSeq = function () {

    ParamArray = {};
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Preference/FavoriteForm/ModSequence.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);

}

List.View = function (_formID) {

    if (_formID < 1) return;

    ParamArray = { 'formID': _formID };
    CallEncrypt(CallGetParam());
    var url = "View.aspx?p=" + GetEncryptValue;

    PopupWin("800", "600", url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-12-18
작성목적 : 추가, 삭제 이후
***************************************************/
List.SetInit = function () {
    $("#ddlSearchTerm").val("");
    $("#ctbSearch").val("");
    $("#startEndDate").hide(); //기간
    SearchValue = "";
    CurrentPage = 1; //1페이지로 이동처리  

    List.LoadData("search");

}