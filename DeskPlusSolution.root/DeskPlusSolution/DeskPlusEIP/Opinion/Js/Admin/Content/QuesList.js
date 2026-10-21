QuesList = {}

$(function () {

    $(document).ready(function () {

        $("#divPeriod").hide();

        // 이벤트할당

        // [추가]버튼
        $('#cbtnAdd').click(function (e) { e.preventDefault(); QuesList.Add(); });

        // [삭제]버튼
        $('#cbtnDel').click(function (e) { e.preventDefault(); QuesList.Del(); });

        // [활성/비활성] 버튼
        $('#cbtnActive').click(function (e) { e.preventDefault(); QuesList.ActiveProc(); });

        // [검색]버튼
        $('#cbtnSrh').click(function (e) { e.preventDefault(); QuesList.Srh(); });

        //검색란KeyEvt
        $("#ctbSrh").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); QuesList.Srh(); } });

        //달력입력란KeyEvt
        $(".clsDate").keydown(function (e) { return false; });

        //검색조건
        $("#ddlSrh").change(function (e) { e.preventDefault(); QuesList.ddlSrhChange(); });

        //검색조건(기간)
        $("#ddlSchTerm").change(function (e) { e.preventDefault(); QuesList.ddlSchTermChange(); });

        //페이지 카운트
        $("#ddlListCount").change(function (e) { e.preventDefault(); QuesList.Srh(); });

        //PageNo = 1; PageSize = $("#ddlListCount").val();

        //검색어입력
        $("#ctbSrh").attr("placeholder", R_PlaceholderText);

        QuesList.ddlSrhChange();    //검색조건초기화
        QuesList.LoadData('');      //페이지가져오기
    });

});


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색-DDL처리
***************************************************/
QuesList.ddlSrhChange = function () {
    $("#sDate").val(""); $("#eDate").val(""); $("ctbSrh").val("");

    //날짜검색여부
    if (/DATE/.test($("#ddlSrh").val().toUpperCase())) {
        $("#divDate").show(); $("#ctbSrh").hide();
        $("#sPerDate").datepick($.datepicker.regional[PreLang]);
        $("#ePerDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#divDate").hide(); $("#ctbSrh").show();
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색(기간검색)-DDL
***************************************************/
QuesList.ddlSchTermChange = function () {
    $("#sPerDate").val(""); $("#ePerDate").val("");

    //날짜검색여부
    if ($("#ddlSchTerm").val().toUpperCase() == "PERIOD") {
        $("#divPeriod").show();

        $("#sPerDate").datepick($.datepicker.regional[PreLang]);
        $("#ePerDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#divPeriod").hide();
    }
}

/***리스트 처리 및 페이징 시작***/

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 가져오기(WS)
***************************************************/
QuesList.GetPageList = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesList.asmx/GetQuesList";

    //파라미터값 배열에 담기
    ParamArray = {
        'section': Section
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };

    var obj = eval("QuesList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
QuesList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JTQuesList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    QuesList.Resource();    //다국어처리
}
QuesList.Resource = function () {
    $("#jtQL_F1").text(R_JTF1);
    $("#jtQL_F2").text(R_JTF2);
    $("#jtQL_F3").text(R_JTF3);
    $("#jtQL_F4").text(R_JTF4);
    $("#jtQL_F5").text(R_JTF5);
    $("#jtQL_F6").text(R_JTF6);
    $(".jtNoData").text(R_JTNoData);
    $(".spResult").text(R_Result);

    // 정렬이미지 처리
    if (nowID != null) {
        if ($("#" + nowID).hasClass("asc")) { $("#" + nowID).removeClass("asc") }
        if ($("#" + nowID).hasClass("desc")) { $("#" + nowID).removeClass("desc") }
        var css = (Order.indexOf(" asc") > 0) ? "asc" : "desc";
        $("#" + nowID).addClass(css);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 총개수 가져오기(WS)
***************************************************/
QuesList.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesList.asmx/GetQuesListCnt";

    ParamArray = {      //파라미터값 배열에 담기
        'section': Section
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
    };

    var obj = eval("QuesList.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
QuesList.SuccessPageCnt = function (data, status) {
    TotCnt = data.d;
    var pageCount = $("#pager").pager.refresh(TotCnt, PageSize);
    //$("#pageInfo").text(PageNo + " / " + pageCount);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 가져오기
***************************************************/
QuesList.LoadData = function (_gubun) {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    $('#loading').html(loadingPath);

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (sPage) {
            //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            PageNo = sPage;
            //QuesList.LoadData('');
            QuesList.GoPageLoad();
        }
    }

    // 페이저 초기화 (실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.)
    $("#pager").pager({
        pager: "#pager",        //pageID
        pagerMode: "text",      //text or image
        totalItemCount: TotCnt, //총카운트
        recordPerPage: PageSize, //페이지 보여질 개수
        currentPage: PageNo,    //페이지번호(1부터시작)
        pageIndexChanged: Pager.pageIndexChanged,   //event
        prev10Text: "Before", //이전
        next10Text: "After",     //다음
        prev10ImgSrc: "images/Prev10.gif",
        next10ImgSrc: "images/next10.gif",  //"<img src='" + ImagePath + "/Icon/loading.gif'/>";
        selectedPageFontColor: "orange"
    });

    setTimeout(function () {
        if (_gubun.toUpperCase() == "SRH") {
            QuesList.GetPageCnt();
            QuesList.GetPageList();
        } else {
            QuesList.GetPageList();
        }
    }, 100);
}


/***리스트 처리 및 페이징 끝***/


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색
***************************************************/
QuesList.Srh = function () {

    // 기간검색
    SPeriod = $("#ddlSchTerm").val();
    SPeriodValue = "";
    if (SPeriod.toUpperCase() == "PERIOD") {
        if ($("#sPerDate").val() != "" && $("#ePerDate").val() != "") {
            var arr1 = $("#sPerDate").val().split("-");
            var arr2 = $("#ePerDate").val().split("-");
            var d1 = new Date(arr1[0], arr1[1], arr1[2]).valueOf();
            var d2 = new Date(arr2[0], arr2[1], arr2[2]).valueOf();
            if ((d2 - d1) < 0) { alert(R_Msg_ErrDate); return; }
            SVal = $("#sPerDate").val() + "~" + $("#ePerDate").val();
        } else {
            alert(R_Msg_ErrSEDate); $("#sPerDate").focus(); return;
        }
    }

    // 검색
    SKey = $("#ddlSrh").val();
    SVal = $("#ctbSrh").val().securityText();

    if (/DATE/.test(SKey.toUpperCase())) {
        if ($("#sDate").val() != "" && $("#eDate").val() != "") {
            var arr1 = $("#sDate").val().split("-");
            var arr2 = $("#eDate").val().split("-");
            var d1 = new Date(arr1[0], arr1[1], arr1[2]).valueOf();
            var d2 = new Date(arr2[0], arr2[1], arr2[2]).valueOf();
            if ((d2 - d1) < 0) { alert(R_Msg_ErrDate); return; }
            SVal = $("#sDate").val() + "~" + $("#eDate").val();
        } else {
            alert(R_Msg_ErrSEDate); return;
        }
    }

    PageNo = 1; PageSize = $("#ddlListCount").val();
    //QuesList.LoadData('srh');
    QuesList.GoPageLoad();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 정렬
***************************************************/
var nowID = null;
QuesList.Sort = function (col, aid) {
    nowID = aid;

    var colValue = col + " asc";
    if (Order == colValue) { colValue = col + " desc"; }    //메뉴를 동일한 필드에서 두번이상 선택했을 경우

    Order = colValue;
    PageNo = 1;
    //QuesList.LoadData('');
    QuesList.GoPageLoad();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 체크박스(전체)
***************************************************/
QuesList.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 체크박스(선택된 값 가져오기)
***************************************************/
QuesList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [추가]
***************************************************/
QuesList.Add = function () {

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "QuesAddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [삭제]
***************************************************/
QuesList.Del = function () {

    var selvals = QuesList.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrDelItem); return; }

    if (confirm(R_Msg_Del)) {
        ParamArray = { 'id': selvals };

        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesList.asmx/DelQuesList";
        var obj = eval("QuesList.LoadData('srh')");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [활성/비활성화]
***************************************************/
QuesList.ActiveProc = function () {
    var selvals = QuesList.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrActive); return; }

    if (confirm(R_Msg_Active)) {
        var rtnObj = "";
        for (i = 0; i < selvals.length; i++) {
            rtnObj += selvals[i] + "-";
            rtnObj += ($("#tbIsAct_" + selvals[i]).val().toUpperCase() == "1") ? "0" : "1";
            rtnObj += ",";      // quesID-Y,quesID-Y,quesID-N,
        }

        ParamArray = { 'id': rtnObj };
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesList.asmx/UpdActiveQues";
        var obj = eval("QuesList.LoadData('srh')");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [설문보기]
***************************************************/
QuesList.View = function (_quesID) {

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'quesID': _quesID
    };

    CallEncrypt(CallGetParam());
    location.href = "QuesView.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [설문결과]
***************************************************/
QuesList.Result = function (_quesID) {

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'quesID': _quesID
    };
    CallEncrypt(CallGetParam());
    location.href = "QuesResult.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
QuesList.GoPageLoad = function () {

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "QuesList.aspx?p=" + GetEncryptValue;
}


