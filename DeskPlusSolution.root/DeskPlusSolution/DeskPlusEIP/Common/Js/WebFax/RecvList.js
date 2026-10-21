RecvList = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            RecvList.LoadData();
        }
    }

    $(document).ready(function () {

        $('#ddlListCount').val(RecordPerPage);

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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            RecvList.Search();
        });        

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            RecvList.Search()
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            RecvList.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                RecvList.Search();
            }
        });

        //관리페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            parent.location = "/DeskPlusEIP/Common/WebFax/Admin/Default.aspx";
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        RecvList.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
RecvList.Init = function () {

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
    
    RecvList.LoadData();

    $("#zeoModalContact").draggable({ disabled: false });
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
RecvList.RegDate = function () {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
RecvList.LoadData = function () {

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        RecvList.GetCount(); //검색 총 카운트 가져오기
        RecvList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                RecvList.LoadData();
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
        loadBoardSearchData();
    }, 100);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
RecvList.GetCount = function () {

    var url = "/DeskPlusEIP/Common/Ws/WebFax/RecvList.asmx/GetListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'branchID': BranchID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RecvList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
RecvList.SuccessCount = function (data, status) {

    TotalItemCount = data.d;    

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
RecvList.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchKey = $("#ddlSearchKey").val();    

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

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
RecvList.Search = function () {

    var response = RecvList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    RecvList.LoadData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
RecvList.GetPageList = function () {
    
    var url = "/DeskPlusEIP/Common/Ws/WebFax/RecvList.asmx/GetPageList";

    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'order': Order,
        'faxID': FaxID
    };

    var obj = eval("RecvList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
RecvList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Common/Jt/WebFax/JTRecvList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");
    RecvList.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
RecvList.Resource = function () {

    $("#r_areacode")[0].innerHTML = R_FaxNationalCode;
    $("#r_faxnum")[0].innerHTML = R_FaxNumber;
    $("#r_pagecnt")[0].innerHTML = R_PageCnt;
    $("#r_recvtime")[0].innerHTML = R_RecvDate;
    $("#r_view")[0].innerHTML = R_View;
    $("#r_print")[0].innerHTML = R_Print;
    $("#r_download")[0].innerHTML = R_Download;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}



/**************************************************
작성자   : 
작성일자 : 2012-02-27
작성목적 : 수신명단
***************************************************/
RecvList.RecvList = function (p_GUID) {
    
    ParamArray = {
        'guid': p_GUID
    };

    CallEncrypt(CallGetParam());
        
    var url = "/DeskPlusEIP/Common/WebFax/SendRecvList.aspx?p=" + GetEncryptValue;
    PopupWin(600, 400, url, "", "", true);
}


/**************************************************
작성자   : 
작성일자 : 2012-02-27
작성목적 : 팩스 이미지 보기
***************************************************/
RecvList.ShowImage = function (p_filePath, p_pageCnt) {

    ParamArray = {
        'faxImage': p_filePath,
        'pageCnt': p_pageCnt
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    var url = "/DeskPlusEIP/Common/WebFax/ViewFaxImage.aspx";
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1024", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}


/**************************************************
작성자   : 
작성일자 : 2012-02-27
작성목적 : 인쇄 이미지 보기
***************************************************/
RecvList.PrintImage = function (p_filePath, p_pageCnt) {

    ParamArray = {
        'faxImage': p_filePath,
        'pageCnt': p_pageCnt
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    var url = "/DeskPlusEIP/Common/WebFax/ViewFaxPrint.aspx";
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1024", "800", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
RecvList.DownloadImage = function (p_fileFullPath) {

    if (p_fileFullPath != "") {
        var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + p_fileFullPath;
        window.open(attachFilePath);
    }
    else {
        alert(R_MsgNotFoundFile);
    }
}