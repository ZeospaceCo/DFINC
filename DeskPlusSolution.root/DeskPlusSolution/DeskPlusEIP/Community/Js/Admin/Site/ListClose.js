ListClose = {}
ListClose.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 초기화 
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListClose.LoadData("");
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


        // 비활성화
        $('#cbtnNonActive').click(function (e) {
            e.preventDefault();
            ListClose.NonActive();
        });

        // 활성화
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            ListClose.Active();
        });

        // 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            ListClose.Del();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListClose.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListClose.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListClose.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListClose.ModSetting();
            ListClose.GoPageListType();
        });

        ListClose.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });

    });
});

/**************************************************
작성자   : shine
작성일자 :2012-05-11
작성목적 : 초기화 하기
***************************************************/
ListClose.Init = function () {

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

    ListClose.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListClose.RegDate = function () {
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
작성일자 : 2012-05-11
작성목적 : 검색
***************************************************/
ListClose.Search = function () {
    var response = ListClose.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListClose.LoadData("search");
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListClose.SettingSearch = function () {

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
작성일자 : 2012-05-11 
작성목적 : 리스트 초기화
***************************************************/
ListClose.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadCommunityData = function () {
        ListClose.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadCommunitySearchData = function () {
        ListClose.GetCount(); //검색 총 카운트 가져오기
        ListClose.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListClose.LoadData("search");
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
            loadCommunitySearchData();
        }
        else {
            loadCommunityData();
        }
    }, 100);
}
/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-11
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListClose.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/GetPageList"
    
    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("ListClose.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessPageList = function (data, status) {

    $('#phListClose').setTemplateURL('/DeskPlusEIP/Community/Jt/Admin/Site/JTListClose.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListClose').processTemplate(data.d);
    $('#loading').html("");

    ListClose.Resource();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 전체 체크박스
**********************************************************************************************/
ListClose.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListClose.Resource = function () {
    
    $("#r_number")[0].innerHTML = R_Number;
    $("#r_status")[0].innerHTML = R_Status;
    $("#r_communityName")[0].innerHTML = R_CommunityName;
    $("#r_writeUserName")[0].innerHTML = R_ComWriteName;
    $("#r_description")[0].innerHTML = R_Description;
    $("#r_type")[0].innerHTML = R_CommunityType;
    $("#r_date")[0].innerHTML = R_OpenDate;


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
작성일자 : 20112-05-09
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListClose.GetCount = function () {

    var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/GetListCount";
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListClose.SuccessCount");

    AjaxService(url, data, obj);

}
/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessCount = function (data, status) {
    //alert(data.d);
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-09
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListClose.GoPageListType = function () {
    var response = ListClose.SettingSearch();

    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    ListClose.Search();

}

/**************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 선택된 값 가져오기
***************************************************/
ListClose.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 커뮤니티 비활성화
**********************************************************************************************/
ListClose.NonActive = function () {
    var selValue = ListClose.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmNonActive)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': selValue,
                'status': '0'
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/ModStatus";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListClose.SuccessNonActive");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckNonActive);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-15
작성목적 : 커뮤니티 비활성화 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessNonActive = function (data, status) {
    if (data.d == true) {
        ListClose.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-12
작성목적 : 커뮤니티 삭제 (웹서비스 호출)
**********************************************************************************************/
ListClose.Del = function () {
    var selValue = ListClose.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_DelAdminCommunity)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': selValue,
                'status': '2'
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/ModStatus";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListClose.SuccessDel");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckDelList);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-12
작성목적 : 커뮤니티 삭제 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessDel = function (data, status) {
    if (data.d == true) {
        ListClose.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-15
작성목적 : 커뮤니티 활성화
**********************************************************************************************/
ListClose.Active = function () {
    var selValue = ListClose.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmActive)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'communityID': selValue,
                'status': '1'
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/ModStatus";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListClose.SuccessActive");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckActive);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListClose.ModSetting = function (gubun) {

    var url = "/DeskPlusEIP/Community/Ws/Admin/Site/ListClose.asmx/ModSetting";
    ParamArray = {
        'communityID': '0',
        'menuID': '0',
        'pageSize': $("#ddlListCount").val(),
        'types': 'DL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListClose.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessSetting = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-15
작성목적 : 커뮤니티 활성화 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListClose.SuccessActive = function (data, status) {
    if (data.d == true) {
        ListClose.Search();
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}

ListClose.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ListClose.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;
