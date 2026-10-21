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
            //List.LoadData("");
            List.GoPageLoad();
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

        // [양식추가]
        $('#cbtnAdd').click(function (e) { e.preventDefault(); List.Add(''); });

        // [삭제]버튼
        $('#cbtnDel').click(function (e) { e.preventDefault(); List.Del(); });

        // [활성/비활성] 버튼
        $('#cbtnActive').click(function (e) { e.preventDefault(); List.ActiveProc(); });

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

        /*
        if ($.browser.msie) {
            $('#check_cbtnAdd').show();
        } else {
            $('#check_cbtnAdd').hide();
        }
        */

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Init();

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
    var url = "/DeskPlusEIP/Approval/Ws/Admin/FormGeneration/List.asmx/GetList";

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
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/FormGeneration/JTList.htm?seq=' + Guids(), null, { filter_data: false });
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
    $("#r_islegacy")[0].innerHTML = R_IsLegacy;
    $("#r_isedm")[0].innerHTML = R_IsEDM;
    $("#r_isactive")[0].innerHTML = R_IsActiveTit;
    $("#r_writedate")[0].innerHTML = R_WriteDate;
    $("#r_draft")[0].innerHTML = R_Draft;
    $("#r_admin")[0].innerHTML = R_Admin;
    //$("#r_EA")[0].innerHTML = R_EA;

    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 리스트 총개수 가져오기(WS)
***************************************************/
List.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/FormGeneration/List.asmx/GetListCnt";

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
    //List.LoadData("search");
    List.GoPageLoad();
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

        var url = "/DeskPlusEIP/Approval/Ws/Admin/FormGeneration/List.asmx/DelList";
        ParamArray = { 'id' : selvals };
        
        var obj = eval("List.Search");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [활성/비활성화]
***************************************************/
List.ActiveProc = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrActive); return; }

    if (confirm(R_Msg_Active)) {
        var rtnObj = "";
        for (i = 0; i < selvals.length; i++) {
            rtnObj += selvals[i] + "-";
            rtnObj += ($("#tbIsAct_" + selvals[i]).val().toUpperCase() == "Y") ? "0" : "1";
            rtnObj += ",";      // quesID-Y,quesID-Y,quesID-N,
        }

        var url = "/DeskPlusEIP/Approval/Ws/Admin/FormGeneration/List.asmx/UpdActive";
        ParamArray = { 'id': rtnObj };
        
        var obj = eval("List.Search");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 검색-DDL처리
***************************************************/
List.ddlSearchChange = function () {
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
List.Add = function (_fID) {

    var fID = (_fID == "") ? 0 : _fID;
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'formID': fID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Admin/FormGeneration/AddN.aspx";
    /*
    if ($.browser.msie) {
        url = "/DeskPlusEIP/Approval/Admin/FormGeneration/Add.aspx";
    } else {
        url = "/DeskPlusEIP/Approval/Admin/FormGeneration/AddN.aspx";
    }
    */

    var param = "p=" + GetEncryptValue;
    var prop = "menubar=no,location=yes,resizable=yes,scrollbars=yes,status=no";
    WindowObjectReference = PopupWinOpen(url, param, "FormGeneration", "1224", "800", prop)
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [보기]
***************************************************/
List.View = function (_formID) {

    ParamArray = {
          'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'formID': _formID
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05 
작성목적 : [보기]
***************************************************/
List.ModSetting = function (_formID) {

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'formID': _formID
    };

    CallEncrypt(CallGetParam());
    location.href = "ModSetting.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Admin_Form",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
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

List.IsCheckFolder = function (_selvals) {
    if (_selvals == "") return false;

    var rtnval = true;
    var bfolderID = "";

    for (i = 0; i < _selvals.length; i++) {

        bfolderID = (i == 0) ? $("#tbFolderID_" + _selvals[i]).val() : bfolderID;
        rtnval = ($("#tbFolderID_" + _selvals[i]).val() == bfolderID) ? true : false;
        if (!rtnval) { return false; }
    }

    return true;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [순서변경]
***************************************************/
List.SetFormSeq = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrFormSequence); return; }

    //폴더체크
    if (!List.IsCheckFolder(selvals)) {
        alert(R_Msg_ErrSelFolder); return;
    }


    var rtnObj = "";
    for (i = 0; i < selvals.length; i++) {
        rtnObj += selvals[i] + "^";
        rtnObj += $("#tbFormBase_" + selvals[i]).val();
        rtnObj += "$";
    }

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'id': rtnObj
    };
    CallEncrypt(CallGetParam());
    var url = "ModFormSequence.aspx?p=" + GetEncryptValue;
    PopupWin("400", "400", url, "", "", true);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
          'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}
