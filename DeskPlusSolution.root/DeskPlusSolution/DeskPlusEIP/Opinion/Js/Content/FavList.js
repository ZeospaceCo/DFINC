FavList = {}

$(function () {

    $(document).ready(function () {

        $("#divPeriod").hide();

        //리스트형 저장
        $('#listType1').click(function () { FavList.AddSettingUser("1"); FavList.GoPageListType("1"); });

        //블로그형 저장
        $('#listType2').click(function () { FavList.AddSettingUser("2"); FavList.GoPageListType("2"); });

        // [검색]버튼
        $('#cbtnSrh').click(function (e) { e.preventDefault(); FavList.Srh(); });

        //검색란KeyEvt
        $("#ctbSrh").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); FavList.Srh(); } });

        //달력입력란KeyEvt
        $(".clsDate").keydown(function (e) { return false; });

        //검색조건
        $("#ddlSrh").change(function (e) { e.preventDefault(); FavList.ddlSrhChange(); });

        //검색조건(기간)
        $("#ddlSchTerm").change(function (e) { e.preventDefault(); FavList.ddlSchTermChange(); });


        //페이지 카운트
        $("#ddlListCount").change(function (e) { e.preventDefault(); FavList.AddSettingUser(ListType); FavList.Srh(); });

        //PageNo = 1; PageSize = $("#ddlListCount").val();

        //검색어입력
        $("#ctbSrh").attr("placeholder", R_PlaceholderText);

        FavList.ddlSrhChange();    //검색조건초기화
        FavList.LoadData('');     //페이지가져오기
    });

});


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색-DDL처리
***************************************************/
FavList.ddlSrhChange = function () {
    $("#sDate").val(""); $("#eDate").val(""); $("ctbSrh").val("");

    //날짜검색여부
    if (/DATE/.test($("#ddlSrh").val().toUpperCase())) {
        $("#divDate").show(); $("#ctbSrh").hide();
        $("#sDate").datepick($.datepicker.regional[PreLang]); $("#eDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#divDate").hide(); $("#ctbSrh").show();
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색(기간검색)-DDL
***************************************************/
FavList.ddlSchTermChange = function () {
    $("#sPerDate").val(""); $("#ePerDate").val("");

    //날짜검색여부
    if ($("#ddlSchTerm").val().toUpperCase() == "PERIOD") {
        $("#divPeriod").show();
        $("#sPerDate").datepick($.datepicker.regional[PreLang]); $("#ePerDate").datepick($.datepicker.regional[PreLang]);
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
FavList.GetPageList = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Content/FavList.asmx/GetFavList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };

    var obj = eval("FavList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
FavList.SuccessPageList = function (data, status) {
    var jtUrl = "";
    switch (ListType.toUpperCase()) {
        case "1":
            jtUrl = '/DeskPlusEIP/Opinion/Jt/Content/JTFavList.htm?seq=' + Guids();
            break;
        case "2":
            jtUrl = '/DeskPlusEIP/Opinion/Jt/Content/JTFavListBlog.htm?seq=' + Guids();
            break;
        default:
            jtUrl = '/DeskPlusEIP/Opinion/Jt/Content/JTFavList.htm?seq=' + Guids();
            break;
    }

    $('#placeholder').setTemplateURL(jtUrl, null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    FavList.Resource();    //다국어처리
}
FavList.Resource = function () {

    if (ListType.toUpperCase() == "2") {
        //Blog
        $(".jtFLB_F1").text(R_TitF4); $(".jtFLB_F2").text(R_TitF7); $(".jtFLB_F3").text(R_TitF8);
        $(".jtFLB_F4").text(R_TitF6); $(".jtFLB_F5").text(R_TitF9); $(".jtFLB_F6").text(R_TitF2);
        $(".jtFLB_F7").text(R_TitF10); $(".jtFLB_F8").text(R_Secret);
    } else {
        //List & Etc
        $("#jtFL_F1").text(R_TitF1); $("#jtFL_F2").text(R_TitF2); $("#jtFL_F3").text(R_TitF3);
        $("#jtFL_F4").text(R_TitF4); $("#jtFL_F5").text(R_TitF5); $("#jtFL_F6").text(R_TitF6); $("#jtFL_F7").text(R_Secret);
    }

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
작성목적 : 리스트 총개수 가져오기(WS)
***************************************************/
FavList.GetPageCnt = function () {
    var url = "/DeskPlusEIP/Opinion/Ws/Content/FavList.asmx/GetFavListCnt";

    ParamArray = {      //파라미터값 배열에 담기
        'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
    };

    var obj = eval("FavList.SuccessPageCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
FavList.SuccessPageCnt = function (data, status) {
    TotCnt = data.d;
    var pageCount = $("#pager").pager.refresh(TotCnt, PageSize);
    //$("#pageInfo").text(PageNo + " / " + pageCount);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 가져오기
***************************************************/
FavList.LoadData = function (_gubun) {
    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif'/>";
    $('#loading').html(loadingPath);

    if (ListType == "1") {
        $("#board").addClass("on");
        $("#bolg").removeClass("on");
    }
    else if (ListType == "2") {
        $("#board").removeClass("on");
        $("#bolg").addClass("on");
    }

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (sPage) {
            //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            PageNo = sPage;
            //FavList.LoadData('');
            FavList.GoPageLoad();
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
            FavList.GetPageCnt();
            FavList.GetPageList();
        } else {
            FavList.GetPageList();
        }
    }, 100);
}


/***리스트 처리 및 페이징 끝***/

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 검색
***************************************************/
FavList.Srh = function () {

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
    $("#ctbSrh").val(SVal);

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
    //FavList.LoadData('srh');
    FavList.GoPageLoad();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 정렬
***************************************************/
var nowID = null;
FavList.Sort = function (col, aid) {
    nowID = aid;

    var colValue = col + " asc";
    if (Order == colValue) { colValue = col + " desc"; }    //메뉴를 동일한 필드에서 두번이상 선택했을 경우

    Order = colValue;
    PageNo = 1;
    //FavList.LoadData('');
    FavList.GoPageLoad();
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 중요표시 추가 및 삭제하기
**********************************************************************************************/
FavList.ModImpUser = function (_id, _arg) {

    if (_id == "") return;

    var url = "/DeskPlusEIP/Opinion/Ws/Content/List.asmx/AddFavUser";   //중요표시 추가
    if ($(_arg).attr("checked") == "checked") {
        $("#" + _id).attr("checked", true);
        $(_arg).next().addClass("on");
    }
    else {
        $("#" + _id).attr("checked", false);
        $(_arg).next().removeClass("on");
        url = "/DeskPlusEIP/Opinion/Ws/Content/List.asmx/DelFavUser";   //중요표시 삭제
    }

    ParamArray = { 'id': _id.split("_")[1] };   //quesID
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("FavList.SuccessModImpUser");
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 성공시, 중요표시 삭제 및 추가하기
**********************************************************************************************/
FavList.SuccessModImpUser = function (data, status) {
    if (data.d == true) { }
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
FavList.GoPageListType = function (_listType) {
    ListType = _listType;
    ParamArray = {      //파라미터값 배열에 담기
        'menuID': MenuID
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'listType': ListType
    };
    CallEncrypt(CallGetParam());

    //이동하기    
    location = "/DeskPlusEIP/Opinion/Content/FavList.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 투표시->투표결과페이지 / 미 투표시->투표페이지 이동
**********************************************************************************************/
FavList.View = function (_quesID) {
    ParamArray = {
        'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'listType': ListType
        , 'quesID': _quesID
    };
    CallEncrypt(CallGetParam());
    var url = "Result.aspx";
    location.href = url + "?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 사용자별 환경설정 처리
**********************************************************************************************/
FavList.AddSettingUser = function (_listType) {
    var url = "/DeskPlusEIP/Opinion/Ws/Content/List.asmx/AddSettingUser";
    ParamArray = { 'menuID': MenuID.toUpperCase(), 'listType': _listType, 'pageSize': $("#ddlListCount").val() };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("FavList.ScsAddSettingUser");
    AjaxService(url, data, obj);
}
FavList.ScsAddSettingUser = function (data, status) {
    if (data.d == true) { }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
FavList.GoPageLoad = function () {

    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'listtype': ListType
    };

    CallEncrypt(CallGetParam());
    location = "FavList.aspx?p=" + GetEncryptValue;
}


