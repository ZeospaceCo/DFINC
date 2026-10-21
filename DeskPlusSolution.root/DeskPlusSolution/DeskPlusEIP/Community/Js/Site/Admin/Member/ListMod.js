var ListMod = {};


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListMod.LoadData("");
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


        // 강제탈퇴
        $('#cbtnBreak').click(function (e) {
            e.preventDefault();
            ListMod.Drop();
        });

        // 가입복귀
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            ListMod.Active();
        });

        // 등급변경
        $('#cbtnGrade').click(function (e) {
            e.preventDefault();
            ListMod.GradeChange();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListMod.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListMod.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListMod.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListMod.ModSetting();
            ListMod.GoPageListType();
        });

        ListMod.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : shine
작성일자 :2012-05-22
작성목적 : 초기화 하기
***************************************************/
ListMod.Init = function () {

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

    ListMod.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 등록일자 활성화 여부
***************************************************/
ListMod.RegDate = function () {
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
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 검색
***************************************************/
ListMod.Search = function () {
    var response = ListMod.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListMod.LoadData("search");
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 검색 필드 설정하기
***************************************************/
ListMod.SettingSearch = function () {

    SearchGroup = $("#ddlSearchGroup").val();
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
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 리스트 초기화
***************************************************/
ListMod.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadUserInfoData = function () {
        ListMod.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadUserInfoSearchData = function () {
        ListMod.GetCount(); //검색 총 카운트 가져오기
        ListMod.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListMod.LoadData("search");
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
            loadUserInfoSearchData();
        }
        else {
            loadUserInfoData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListMod.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'order': Order
    };

    var obj = eval("ListMod.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListMod.SuccessPageList = function (data, status) {
    $('#phListMod').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Admin/Member/JTListMod.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListMod').processTemplate(data.d);
    $('#loading').html("");

    ListMod.Resource();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 전체 체크박스
**********************************************************************************************/
ListMod.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 선택된 값 가져오기
***************************************************/
ListMod.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 등급변경
***************************************************/
ListMod.GradeChange = function () {

    var selValue = ListMod.CheckedValue();

    if (selValue != "") {

        var index = $("#ddlGrade").get(0).options.selectedIndex;
        var tmpMsg = $("#ddlGrade").get(0).options[index].text;

        if (window.confirm(tmpMsg + R_ConfirmGrade)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': CommunityID,
                'userID': selValue,
                'grade': $("#ddlGrade").val()
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/ModGrade";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListMod.SuccessModGrade");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckGrade);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 등급변경 성공 웹서비스 호출
**********************************************************************************************/
ListMod.SuccessModGrade = function (data, status) {

    if (data.d == true) {
        ListMod.LoadData("search");
        alert(R_ApplyGrade);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListMod.GoPageListType = function () {
    var response = ListMod.SettingSearch();

    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    ListMod.Search();
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListMod.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_userName")[0].innerHTML = R_UserName;
    $("#r_memberType")[0].innerHTML = R_MemeberGrade;
    $("#r_joinDate")[0].innerHTML = R_JoinDate;
    $("#r_visitCount")[0].innerHTML = R_VisitCount;
    $("#r_status")[0].innerHTML = R_Status;
    //$("#r_boardCount")[0].innerHTML = R_BoardCount;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-22 
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListMod.GetCount = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/GetListCount";
    ParamArray = {
        'communityID': CommunityID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMod.SuccessCount");

    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-22 
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListMod.SuccessCount = function (data, status) {
    
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 강제 탈퇴 (웹서비스 호출)
***************************************************/
ListMod.Drop = function () {

    var selValue = ListMod.CheckedValue();

    if (selValue != "") {

        if (window.confirm(R_AdminDrop)) {
            ParamArray = {
                'communityID': CommunityID,
                'userID': selValue,
                'status': '0'
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListMod.SuccessDrop");
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/UserStatus";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckDroplList);
    }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 강제 탈퇴 (웹서비스 값 받기)
**********************************************************************************************/
ListMod.SuccessDrop = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        ListMod.LoadData("search");
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-15
작성목적 : 가입 복귀 (웹서비스 호출)
***************************************************/
ListMod.Active = function () {

    var selValue = ListMod.CheckedValue();

    if (selValue != "") {


        ParamArray = {
            'communityID': CommunityID,
            'userID': selValue,
            'status': '1'
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListMod.SuccessActive");
        var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/UserStatus";

        AjaxService(url, data, obj);

    }
    else {
        alert(R_CheckActivelList);
    }
    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-15
작성목적 : 가입 복귀 (웹서비스 값 받기)
**********************************************************************************************/
ListMod.SuccessActive = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        ListMod.LoadData("search");
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListMod.ModSetting = function (gubun) {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListMod.asmx/ModSetting";
    ParamArray = {
        'communityID': CommunityID,
        'menuID': '0',
        'pageSize': $("#ddlListCount").val(),
        'types': 'SL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMod.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListMod.SuccessSetting = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
ListMod.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ListMod.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;