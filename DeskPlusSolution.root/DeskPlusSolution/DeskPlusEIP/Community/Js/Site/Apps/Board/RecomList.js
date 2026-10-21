RecomList = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            RecomList.LoadData("");
        }
    }

    $(document).ready(function () {

        RecordPerPage = $("#ddlListCount").val();

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
            RecomList.PageSize();
        });

        RecomList.LoadData("");
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
RecomList.PageSize = function () {
    CurrentPage = 1; //1페이지로 이동처리     
    RecordPerPage = $('#ddlListCount').val();
    RecomList.LoadData("search");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 다국어 처리
**********************************************************************************************/
RecomList.Resource = function () {

    $("#j_DeptName")[0].innerHTML = R_DeptName;
    $("#j_TitleName")[0].innerHTML = R_TitleName;
    $("#j_Name")[0].innerHTML = R_UserName;
    $("#j_RegDate")[0].innerHTML = R_RecomDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 초기화
***************************************************/
RecomList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadRecomListData = function () {
        RecomList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadRecomListSearchData = function () {
        RecomList.GetCount(); //검색 총 카운트 가져오기
        RecomList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                RecomList.LoadData("search");
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
            loadRecomListSearchData();
        }
        else {
            loadRecomListData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
RecomList.GetPageList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/RecomList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("RecomList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
RecomList.SuccessPageList = function (data, status) {
    $('#phRecomList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTRecomList.htm?seq=' + Guids());
    $('#phRecomList').processTemplate(data.d);
    $('#loading').html("");

    RecomList.Resource();

    $("#scrollDiv").css({ 'overflow-y': 'auto', 'min-height': '320px', 'height': '320px' });
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
RecomList.GetCount = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/RecomList.asmx/GetListCount";

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RecomList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-14
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
RecomList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

