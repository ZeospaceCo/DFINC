List = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
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

        // 메세지 보내기 
        $('#cbtnSend').click(function (e) {
            e.preventDefault();
            List.Send();

        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        // 기간
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            List.ModListUser();
            List.Search();
        });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

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

    List.LoadData("");
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 날짜 세팅
***************************************************/
List.RegDate = function () {
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
작성일자 : 2020-02-25
작성목적 : 검색
***************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    List.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

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
작성일자 : 2020-02-25
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadMDMData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadMDMSearchData = function () {

        RecordPerPage = $("#ddlListCount").val();
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
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
            loadMDMSearchData();
        }
        else {
            loadMDMData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Send/List.asmx/GetPageList"

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

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#phList').setTemplateURL('/DeskPlusEIP/MDM/Jt/Send/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 전체 체크박스
**********************************************************************************************/
List.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}
/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_writeDate")[0].innerHTML = R_SendTime;
    $("#r_name")[0].innerHTML = R_SendName;
    $("#r_receiveName")[0].innerHTML = R_ReceiveName;
    $("#r_content")[0].innerHTML = R_Content;
    $("#r_url")[0].innerHTML = R_TargetUrl;
    $("#r_app")[0].innerHTML = R_AppY;
    $("#r_noapp")[0].innerHTML = R_AppN;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Send/List.asmx/GetListCount";
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-25
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-26
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
List.ModListUser = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Send/List.asmx/ModListUser";
    ParamArray = {
        'isType': "SE",
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModListUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-26
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessModListUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-02-26
작성목적 : 알림(Push) 메세지 보내기 
**********************************************************************************************/
List.Send = function () {

    var url = "/DeskPlusEIP/MDM/Send/Add.aspx";
    PopupWin(500, 400, url, "", "", true);
    //WindowObjectReference = PopupWinOpen(url, "", "Send", "500", "400")
}

/**************************************************
작성자   : famo23
작성일자 : 2020-06-02
작성목적 : 리스트 초기화
***************************************************/
function GetList() {

    $("#ddlSearchTerm").val('');
    $("#ctbSearch").val('');

    SearchTerm = "";
    SearchField = $("#ddlSearchField").val();
    SearchTermValue = "";
    SearchValue = "";

    CurrentPage = 1; //1페이지로 이동처리     

    List.LoadData("search");
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-02
작성목적 : 알림 내용 상세
**********************************************************************************************/
List.OpenContent = function (_sendID) {

    ParamArray = {
        'sendID': _sendID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/MDM/Send/Content.aspx?p=" + GetEncryptValue;

    PopupWin(350, 410, url, "", "", true); 
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-02
작성목적 : 수신자 리스트 
**********************************************************************************************/
List.OpenReceive = function (_sendID) {

    ParamArray = {
        'sendID': _sendID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/MDM/Send/ReceiveList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : App 설치 리스트 
**********************************************************************************************/
List.OpenApp = function (_sendID, _isApp) {

    ParamArray = {
        'sendID': _sendID,
        'isApp': _isApp
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/MDM/Send/AppList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-16
작성목적 : 바로가기 새창 
**********************************************************************************************/
List.DocView = function (_targetURL) {

    WindowObjectReference = PopupWinOpen(_targetURL, "", "DocView", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}
