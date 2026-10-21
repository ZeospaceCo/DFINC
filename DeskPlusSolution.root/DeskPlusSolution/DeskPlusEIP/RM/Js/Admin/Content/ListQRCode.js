ListQRCode = {}

var oContextMenuCtrl = null;
/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            ListQRCode.LoadData("");
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

        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListQRCode.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListQRCode.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListQRCode.Search();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            ListQRCode.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListQRCode.ModCategoryUser();
        });

        ListQRCode.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 초기화 하기
***************************************************/
ListQRCode.Init = function () {
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

    ListQRCode.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 달력 보이기
***************************************************/
ListQRCode.RegDate = function () {
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
작성일자 : 2012-03-08
작성목적 : 검색
***************************************************/
ListQRCode.Search = function () {
    var response = ListQRCode.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    ListQRCode.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 초기화
***************************************************/
ListQRCode.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadRMData = function () {
        ListQRCode.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadRMSearchData = function () {
        ListQRCode.GetCount(); //검색 총 카운트 가져오기
        ListQRCode.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListQRCode.LoadData("search");
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
            loadRMSearchData();
        }
        else {
            loadRMData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListQRCode.GetPageList = function () {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/ListQRCode.asmx/GetPageList";

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

    var obj = eval("ListQRCode.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListQRCode.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTQRCode.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    ListQRCode.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListQRCode.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListQRCode.GetCount = function () {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/ListQRCode.asmx/GetListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListQRCode.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListQRCode.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 검색 필드 설정하기
***************************************************/
ListQRCode.SettingSearch = function () {
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
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
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

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListQRCode.ModCategoryUser = function (gubun) {
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/ListQRCode.asmx/ModCategoryUser";
    ParamArray = {
        'categoryID': 0,
        'pageSize': $("#ddlListCount").val(),
        'types': "Q"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListQRCode.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListQRCode.SuccessModCategoryUser = function (data, status) {
    if (data.d) {
        RecordPerPage = $("#ddlListCount").val();
        ListQRCode.Search();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-08
작성목적 : QRCode 다운로드
**********************************************************************************************/
ListQRCode.FileDownload = function (filePath) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성자   : famo23
작성일자 : 2012-03-16
작성목적 : 인쇄
***************************************************/
ListQRCode.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}