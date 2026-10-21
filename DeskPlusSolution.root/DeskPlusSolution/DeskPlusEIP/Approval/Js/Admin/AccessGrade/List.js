List = {}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 초기화
***************************************************/
$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
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

        // [추가]
        $('#cbtnAdd').click(function (e) { e.preventDefault(); List.Add(); });

        // [삭제]버튼
        $('#cbtnDel').click(function (e) { e.preventDefault(); List.Del(); });

        // [활성/비활성] 버튼
        $('#cbtnActive').click(function (e) { e.preventDefault(); List.ActiveProc(); });

        // [순서] 버튼
        $('#cbtnSeq').click(function (e) { e.preventDefault(); List.SetSeq(); });

        // [검색]버튼
        $('#cbtnSearch').click(function (e) { e.preventDefault(); List.Search(); });

        //검색란KeyEvt
        $("#ctbSearch").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); List.Search(); } });

        //검색조건
        $("#ddlSearchField").change(function (e) { e.preventDefault(); List.ddlSearchChange(); });

        //페이지 카운트
        $("#ddlListCount").change(function (e) { e.preventDefault(); RecordPerPage = $('#ddlListCount').val(); List.SavePageSize(); List.Search(); });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        //[기본값설정]
        $('#cbtnBase').click(function (e) { e.preventDefault(); List.SetBase(); });

        List.Init();

    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 검색필드 초기
***************************************************/
List.Init = function () {
    List.LoadData("");
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-07-06
작성목적 : 리스트 가져오기(WS)
***************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 리스트 가져오기(WS)
***************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/AccessGrade/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();    //다국어처리
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 다국어처리
***************************************************/
List.Resource = function () {

    $("#r_accessgradeName")[0].innerHTML = R_AccessGradeName;
    $("#r_accessgradeDept")[0].innerHTML = R_AccessGradeDept;
    $("#r_isactive")[0].innerHTML = R_IsActiveTit;
    $("#r_Isbase")[0].innerHTML = R_IsBase;

    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_NoContent); }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 리스트 총개수 가져오기(WS)
***************************************************/
List.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/GetListCnt";

    ParamArray = {      //파라미터값 배열에 담기
          'searchField': SearchField
        , 'searchValue': SearchValue
    };

    var obj = eval("List.SuccessPageCnt");
    CallEncrypt(CallGetParam()); 
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 리스트 총개수 가져오기
***************************************************/
List.SuccessPageCnt = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 리스트 가져오기
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadBoardData = function () {
        List.GetPageList();
    }

    var loadBoardSearchData = function () {
        List.GetPageCnt();  //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

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
작성일자 : 2017-06-07
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}
/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [검색]
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    List.GoPageLoad();
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
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
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 체크박스(선택된 값 가져오기)
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [삭제]
***************************************************/
List.Del = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_MSG_SelItem); return; }

    var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/GetChkList";
    ParamArray = { 'id' : selvals };

    var obj = eval("List.DelSuccess");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-27
작성목적 : [삭제체크]
***************************************************/
List.DelSuccess = function (data, status) {

    var CheckCount = data.d;

    var CheckCountInt = parseInt(CheckCount);

    if (CheckCountInt > 0) {
        alert(R_DelAccessGradeCheck);
    }
    else {
        var selvals = List.CheckedValue();

        if (confirm(R_Msg_Del)) {

            var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/DelList";
            ParamArray = { 'id': selvals };

            var obj = eval("List.Search");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [활성/비활성화]
***************************************************/
List.ActiveProc = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_MSG_SelItem); return; }

    if (confirm(R_MSG_DoActive)) {
        var rtnObj = "";
        for (i = 0; i < selvals.length; i++) {
            rtnObj += selvals[i] + "-";
            rtnObj += ($("#tbIsAccess_" + selvals[i]).val().toUpperCase() == "YES") ? "0" : "1";
            rtnObj += ",";
        }

        var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/UpdActive";
        ParamArray = { 'id': rtnObj };

        var obj = eval("List.Search");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}


/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [순서변경]
***************************************************/
List.SetSeq = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_MSG_SelItem); return; }

    var rtnObj = "";
    for (i = 0; i < selvals.length; i++) {
        rtnObj += selvals[i] + "^";
        rtnObj += $("#tbAccessGrade_" + selvals[i]).val();
        rtnObj += "$";
    }

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'id': rtnObj
    };
    CallEncrypt(CallGetParam());
    var url = "ModSequence.aspx?p=" + GetEncryptValue;
    PopupWin("400", "400", url, "", "", true);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [추가]
***************************************************/
List.Add = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
    };

    location.href = "Add.aspx?p=" + GetEncryptValue;
    CallEncrypt(CallGetParam());
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : [보기]
***************************************************/
List.Mod = function (_accessID) {

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'accessID': _accessID
    };

    CallEncrypt(CallGetParam());
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Admin_Access",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-06-07
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
          'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
[기본값설정]
***************************************************/
List.SetBase = function () {
    var selvals = List.CheckedValue();
    if (selvals == "") { alert(R_MSG_SelItem); return; }

    if (selvals.length > 1) { alert(R_MsgAccessGrade1); return; }

    if ($("#tbIsAccess_" + selvals).val() == "N") { alert(R_MsgAccessGrade2); return; }

    if (confirm(R_MsgAccessGrade3)) {
        var url = "/DeskPlusEIP/Approval/Ws/Admin/AccessGrade/List.asmx/SetBase";
        ParamArray = { 'aid': selvals };

        var obj = eval("List.Search");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}