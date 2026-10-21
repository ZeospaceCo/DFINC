ListAdd = {}
ListAdd.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-12
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListAdd.LoadData("");
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

        // 승인불가
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            ListAdd.Reject();

        });

        // 승인처리
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ListAdd.Confirm();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListAdd.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListAdd.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListAdd.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListAdd.ModSetting();
            ListAdd.GoPageListType();
        });

        ListAdd.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });

    });
});

/**************************************************
작성자   : shine
작성일자 :2012-05-16
작성목적 : 초기화 하기
***************************************************/
ListAdd.Init = function () {

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

    ListAdd.LoadData("");
    __addEventListener(document, "click", ListAdd.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 등록일자 활성화 여부
***************************************************/
ListAdd.RegDate = function () {
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
작성일자 : 2012-05-17
작성목적 : 가입 승인처리
***************************************************/
ListAdd.Confirm = function () {
    var selValue = ListAdd.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmComplete)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': CommunityID,
                'userID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListAdd.asmx/MemberAdd";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListAdd.SuccessMemberAdd");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckConfirm);
    }

}

/**************************************************
작성자   : shine
작성일자 : 2012-05-17
작성목적 : 가입 승인처리 성공 웹서비스 호출
***************************************************/
ListAdd.SuccessMemberAdd = function (data, status) {
    if (data.d == true) {
        ListAdd.LoadData("search");
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
작성자   : shine
작성일자 : 2012-05-17
작성목적 : 가입 승인불가
***************************************************/
ListAdd.Reject = function () {
    var selValue = ListAdd.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmReject)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': CommunityID,
                'userID': selValue
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListAdd.asmx/MemberReject";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListAdd.SuccessMemberReject");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckReject);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-17
작성목적 : 가입 승인불가 성공 웹서비스 호출
***************************************************/
ListAdd.SuccessMemberReject = function (data, status) {
    if (data.d == true) {
        ListAdd.LoadData("search");
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }

}

/**************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 검색
***************************************************/
ListAdd.Search = function () {
    var response = ListAdd.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListAdd.LoadData("search");
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 검색 필드 설정하기
***************************************************/
ListAdd.SettingSearch = function () {

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
작성일자 : 2012-05-16
작성목적 : 리스트 초기화
***************************************************/
ListAdd.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadMemberInfoData = function () {
        ListAdd.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadMemberInfoSearchData = function () {
        ListAdd.GetCount(); //검색 총 카운트 가져오기
        ListAdd.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListAdd.LoadData("search");
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
            loadMemberInfoSearchData();
        }
        else {
            loadMemberInfoData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-17
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListAdd.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListAdd.asmx/GetPageList";

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
        'order': Order
    };

    var obj = eval("ListAdd.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListAdd.SuccessPageList = function (data, status) {
    $('#phListAdd').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Admin/Member/JTListAdd.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListAdd').processTemplate(data.d);
    $('#loading').html("");
    
    ListAdd.Resource();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 전체 체크박스
**********************************************************************************************/
ListAdd.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 선택된 값 가져오기
***************************************************/
ListAdd.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-16
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListAdd.GoPageListType = function () {
    var response = ListAdd.SettingSearch();

    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    ListAdd.Search();
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListAdd.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_userName")[0].innerHTML = R_UserName;
    $("#r_decsription")[0].innerHTML = R_Description;
    $("#r_applyDate")[0].innerHTML = R_ApplyJoinDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    /*
    var tmpOrder = Order.replace(" asc", "").replace(" desc", "");
    var colValue = tmpOrder + " asc";

    if (Order == colValue) {
    $("#a_" + tmpOrder).addClass("asc");
    }
    else {
    $("#a_" + tmpOrder).addClass("desc");
    }

    if ($(".IcionOn") != undefined) { $(".IcionOn").attr('src', ImagePath + '/icon/fav_Icon_on.gif'); }
    if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    */
}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-16 
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListAdd.GetCount = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListAdd.asmx/GetListCount";
    ParamArray = {
        'communityID': CommunityID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdd.SuccessCount");

    AjaxService(url, data, obj);
    
}

/**********************************************************************************************
작성자   : shine
작성일자 :2012-05-16 
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListAdd.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListAdd.ModSetting = function (gubun) {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Member/ListAdd.asmx/ModSetting";
    ParamArray = {
        'communityID': CommunityID,
        'menuID': '0',
        'pageSize': $("#ddlListCount").val(),
        'types': 'JL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdd.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListAdd.SuccessSetting = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
ListAdd.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ListAdd.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

