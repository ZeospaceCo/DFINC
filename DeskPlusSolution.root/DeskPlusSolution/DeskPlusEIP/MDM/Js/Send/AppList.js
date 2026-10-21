AppList = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 데이터 초기화 
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            AppList.LoadData("");
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
            AppList.PageSize();
        });

        if (IsApp == "N") {
            $("#HeaderName").text(R_AppN);
        }

        AppList.LoadData("");
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
AppList.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'sendID': SendID,
        'isApp': IsApp,      
        'recordPerPage': $('#ddlListCount').val()        
    };

    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 초기화
***************************************************/
AppList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    setTimeout(function () {
        AppList.GetPageList();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
AppList.GetPageList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Send/AppList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'sendID': SendID,
        'isApp': IsApp,        
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("AppList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
AppList.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/MDM/Jt/Send/JTAppList.htm?seq=' + Guids());
    $('#placeholder').processTemplate(data.d);

    AppList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 다국어 처리
**********************************************************************************************/
AppList.Resource = function () {
    
    $("#j_DeptName")[0].innerHTML = R_DeptName;
    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_TitleName")[0].innerHTML = R_TitleName;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
AppList.GetCount = function () {
    var url = "/DeskPlusEIP/MDM/Ws/Send/AppList.asmx/GetListCount";

    ParamArray = {
        'sendID': SendID,
        'isApp': IsApp
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AppList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-03
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AppList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}