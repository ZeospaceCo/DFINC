Stats = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            Stats.LoadData("");
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
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
            ListCount = $('#ddlListCount').val();
            CurrentPage = 1; //1페이지로 이동처리  
            Stats.LoadData("search");
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            window.close();
        });

        Stats.LoadData();
    });
});

/**************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 초기화
***************************************************/
Stats.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadProposalData = function () {
        Stats.GetCount(); //검색 총 카운트 가져오기
        Stats.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadProposalSearchData = function () {
        Stats.GetCount(); //검색 총 카운트 가져오기
        Stats.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Stats.LoadData("search");
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
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

    //$('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadProposalSearchData();
        }
        else {
            loadProposalData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 통계 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Stats.GetCount = function () {

    var url = "/DeskPlusEIP/Proposal/Ws/Content/PopUpStats.asmx/GetPageCount";

    ParamArray = {
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Stats.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 통계 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Stats.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Stats.GetPageList = function () {

    var url = "/DeskPlusEIP/Proposal/Ws/Content/PopUpStats.asmx/GetPageList";

    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'judgeGubun': JudgeGubun,
        'startYears': StartYears,
        'startMonths': StartMonths,
        'endYears': EndYears,
        'endMonths': EndMonths,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("Stats.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2013-08-22
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Stats.SuccessPageList = function (data, status) {

    $('#phPopStats').setTemplateURL('/DeskPlusEIP/Proposal/Jt/Content/JTPopUpStats.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phPopStats').processTemplate(data.d);
    //$('#loading').html("");
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
Stats.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Stats.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;