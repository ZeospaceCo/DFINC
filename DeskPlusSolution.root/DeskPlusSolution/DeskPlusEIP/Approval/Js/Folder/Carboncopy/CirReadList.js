CirReadList = {}



/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            CirReadList.LoadData("");
        }
    }

    $(document).ready(function () {

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

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
            CirReadList.PageSize();
        });

        CirReadList.LoadData("");
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
CirReadList.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'cirDocumentID': CirDocumentID,
        'totalItemCount': TotalItemCount,
        'recordPerPage': $('#ddlListCount').val(),
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
CirReadList.Resource = function () {
    $("#j_DeptName")[0].innerHTML = R_DeptName;
    $("#j_TitleName")[0].innerHTML = R_TitleName;
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_RegDate")[0].innerHTML = R_RegDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
CirReadList.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        CirReadList.GetCount();
        CirReadList.GetPageList();
    }, 100);
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 회람 리스트 호출
**********************************************************************************************/
CirReadList.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/CirReadList.asmx/GetPageListViewer";

    //파라미터값 배열에 담기
    ParamArray = {
        'cirDocumentID': CirDocumentID,
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("CirReadList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 성공시 회람 리스트 표시
**********************************************************************************************/
CirReadList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTCirReadList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    CirReadList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 웹서비스 회람 카운트  호출
**********************************************************************************************/
CirReadList.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/CirReadList.asmx/GetListViewerCount";

    ParamArray = {
        'cirDocumentID': CirDocumentID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("CirReadList.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
CirReadList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}