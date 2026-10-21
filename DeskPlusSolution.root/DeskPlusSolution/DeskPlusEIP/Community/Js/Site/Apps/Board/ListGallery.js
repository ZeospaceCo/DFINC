ListGallery = {}
ListGallery.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListGallery.LoadData("");
            ListGallery.GoPageLoad();
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

        // 글올리기
        $('#cbtnWrite').click(function (e) {
            e.preventDefault();
            ListGallery.Add();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListGallery.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListGallery.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListGallery.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListGallery.ModSetting();
            //ListGallery.GoPageListType();
            ListGallery.Search();
        });

        ListGallery.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 초기화 하기
***************************************************/
ListGallery.Init = function () {

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

    ListGallery.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListGallery.RegDate = function () {
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
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 검색
***************************************************/
ListGallery.Search = function () {
    var response = ListGallery.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    //ListGallery.LoadData("search");
    ListGallery.GoPageLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListGallery.SettingSearch = function () {

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
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 초기화
***************************************************/
ListGallery.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardListData = function () {
        ListGallery.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardListSearchData = function () {
        ListGallery.GetCount(); //검색 총 카운트 가져오기
        ListGallery.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListGallery.LoadData("search");
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
            loadBoardListSearchData();
        }
        else {
            loadBoardListData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListGallery.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ListGallery.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("ListGallery.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListGallery.SuccessPageList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTListGallery.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    ListGallery.Resource();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListGallery.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListGallery.GetCount = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ListGallery.asmx/GetListCount";
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListGallery.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListGallery.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListGallery.GoPageListType = function () {
    var response = ListGallery.SettingSearch();
    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    ListGallery.Search();

}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 게시물 보기
***************************************************/
ListGallery.View = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listType': ListType,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 이미지 보기
***************************************************/
ListGallery.ShowImage = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Community/Site/Apps/Board/GallerySubList.aspx";
    var parm = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, parm, "ShowImage", 800, 600);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 글 작성하기
***************************************************/
ListGallery.Add = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listType': ListType,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 조회현황
***************************************************/
ListGallery.ReadList = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID
    };

    CallEncrypt(CallGetParam())

    var url = "ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 새창으로 짧은답글리스트 보기
***************************************************/
ListGallery.PopupShort = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'grade': Grade,
        'listType': ListType
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Community/Site/Apps/Board/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 추천 현황
***************************************************/
ListGallery.RecomList = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID
    };

    CallEncrypt(CallGetParam())

    var url = "RecomList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListGallery.ModSetting = function (gubun) {

    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ListGallery.asmx/ModSetting";
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'PL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListGallery.SuccessSetting");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-18
작성목적 : 리스트별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListGallery.SuccessSetting = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListGallery.GoPageLoad = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());
    location = "ListGallery.aspx?p=" + GetEncryptValue;
}


