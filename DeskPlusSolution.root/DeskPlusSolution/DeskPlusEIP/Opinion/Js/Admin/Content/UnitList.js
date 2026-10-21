UnitList = {}

$(function () {

    $(document).ready(function () {

        $("#divPeriod").hide();

        //이벤트할당

        // [추가] 버튼
        $('#cbtnAdd').click(function (e) { e.preventDefault(); UnitList.Add(); });

        // [삭제] 버튼
        $('#cbtnDel').click(function (e) { e.preventDefault(); UnitList.Del(); });

        // [활성/비활성] 버튼
        $('#cbtnActive').click(function (e) { e.preventDefault(); UnitList.ActiveProc(); });

        //[검색]버튼
        $('#cbtnSrh').click(function (e) { e.preventDefault(); UnitList.Srh(); });

        //검색란KeyEvt
        $("#ctbSrh").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); UnitList.Srh(); } });

        //달력입력란KeyEvt
        $(".clsDate").keydown(function (e) { return false; });

        //검색조건
        $("#ddlSrh").change(function (e) { e.preventDefault(); UnitList.ddlSrhChange(); });

        //검색조건(기간)
        $("#ddlSchTerm").change(function (e) { e.preventDefault(); UnitList.ddlSchTermChange(); });

        //페이지 카운트
        $("#ddlListCount").change(function (e) { e.preventDefault(); UnitList.Srh(); });

        //PageSize = $("#ddlListCount").val();

        //검색어입력
        $("#ctbSrh").attr("placeholder", R_PlaceholderText);

        UnitList.ddlSrhChange();    //검색조건초기화
        UnitList.LoadData('');     //페이지가져오기
    });

});

/*UnitList.Init = function () {
    //? 날짜검색 시, 해당 값 설정.
}*/

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색-DDL처리
***************************************************/
UnitList.ddlSrhChange = function () {
    $("#sDate").val(""); $("#eDate").val(""); $("ctbSrh").val("");
    
    //날짜검색여부
    if (/DATE/.test($("#ddlSrh").val().toUpperCase())) {   
        $("#divDate").show(); $("#ctbSrh").hide();
        $("#sDate").datepick($.datepicker.regional[PreLang]); 
        $("#eDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#divDate").hide(); $("#ctbSrh").show();
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색(기간검색)-DDL
***************************************************/
UnitList.ddlSchTermChange = function () {
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
작성목적 : 리스트내용가져오기(WS)
***************************************************/
UnitList.GetPageList = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitList.asmx/GetUnitList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totCnt': TotCnt
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'section': Section
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };
    var obj = eval("UnitList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
UnitList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JTUnitList.htm?seq=' + Guids(), null, { filter_data: false });

    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    UnitList.Resource();    //다국어처리
}

//다국어처리
UnitList.Resource = function () {
    $("#jtUL_F1").text(R_JTF1);
    $("#jtUL_F2").text(R_JTF8);
    $("#jtUL_F3").text(R_JTF9);
    $("#jtUL_F4").text(R_JTF22);
    $("#jtUL_F5").text(R_JTF5);
    $("#jtUL_F6").text(R_JTF15);
    $(".jtNoData").text(R_JTNoData);

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
작성목적 : 리스트총개수가져오기(WS)
***************************************************/
UnitList.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitList.asmx/GetUnitListCnt";

    ParamArray = {      //파라미터값 배열에 담기
        'section': Section
        ,'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
    };

    var obj = eval("UnitList.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
UnitList.SuccessPageCnt = function (data, status) {
    TotCnt = data.d;
    var pageCount = $("#pager").pager.refresh(TotCnt, PageSize);
    //$("#pageInfo").text(PageNo + " / " + pageCount);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트가져오기
***************************************************/
UnitList.LoadData = function (_gubun) {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    $('#loading').html(loadingPath);

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (sPage) {
            PageNo = sPage;
            //UnitList.LoadData('');
            UnitList.GoPageLoad();
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
            UnitList.GetPageCnt();
            UnitList.GetPageList();
        } else {
            UnitList.GetPageList();
        }
    }, 100);

    // 이미지처리
}

/***리스트 처리 및 페이징 끝***/


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [검색]
***************************************************/
UnitList.Srh = function () {

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
            //$("#sDate").val(""); $("#eDate").val("");
        }
    }

    PageNo = 1; PageSize = $("#ddlListCount").val();
    //UnitList.LoadData('srh');
    UnitList.GoPageLoad();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 정렬
***************************************************/
var nowID = null;
UnitList.Sort = function (col, aid) {
    nowID = aid;

    var colValue = col + " asc";
    if (Order == colValue) { colValue = col + " desc"; }    //메뉴를 동일한 필드에서 두번이상 선택했을 경우

    Order = colValue;
    PageNo = 1;
    //UnitList.LoadData('');
    UnitList.GoPageLoad();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 전체 체크박스
***************************************************/
UnitList.Checked = function () {
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
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
UnitList.CheckedValue = function () {
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
UnitList.Add = function () {

    ParamArray = {
        'menuID': MenuID
        , 'section': Section
        , 'totCnt': TotCnt
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'unitID': 0
    };
    
    CallEncrypt(CallGetParam());
    location.href = "UnitAdd.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [수정]
***************************************************/
UnitList.View = function (_unitID) {

    ParamArray = {
        'menuID': MenuID
        , 'section': Section
        , 'totCnt': TotCnt
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'unitID': _unitID
    };

    CallEncrypt(CallGetParam());
    location.href = "UnitAdd.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [삭제]
***************************************************/
UnitList.Del = function () {
    var selvals = UnitList.cvVal();
    if (selvals == "") { alert(R_Msg_ErrDelItem); return; }

    if (confirm(R_Msg_Del)) {
        ParamArray = { 'id': selvals };

        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitList.asmx/DelUnitList";
        var obj = eval("UnitList.LoadData('srh')");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
UnitList.cvVal = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val() + ':' + $('#wDate_' + $(this).val()).text();    //unitID:2010-01-01
    }).get();
    return allVals;
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [활성/비활성화]
***************************************************/
UnitList.ActiveProc = function () {
    var selvals = UnitList.CheckedValue();
    if (selvals == "") { alert(R_Msg_ErrActive); return; }

    if (confirm(R_Msg_Active)) {
        var rtnObj = "";
        for (i = 0; i < selvals.length; i++) {
            rtnObj += selvals[i] + "-";
            rtnObj += ($("#tbIsAct_" + selvals[i]).val().toUpperCase() == "1") ? "0" : "1";
            rtnObj += ",";      // unitID-Y,unitID-Y,unitID-N,
        }

        ParamArray = { 'id': rtnObj };
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/UnitList.asmx/UpdActiveUnit";
        var obj = eval("UnitList.LoadData('srh')");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
UnitList.GoPageLoad = function () {

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
    location = "UnitList.aspx?p=" + GetEncryptValue;
}


