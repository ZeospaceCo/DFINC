ListModify = {}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-08
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListModify.LoadData("");
        }
    }

    $(document).ready(function () {

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

        //[삭제]
        $('#Del').click(function (e) {
            e.preventDefault();
            ListModify.Del();
        });

        //[닫기]
        $('#Close').click(function (e) { if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        ListModify.Init();
    });
});

/**************************************************
작성자   : 
작성일자 :2012-05-09
작성목적 : 초기화 하기
***************************************************/
ListModify.Init = function () {

    $("#DelAll")[0].style.display = (TotalItemCount == "0") ? "none" : "";  //건수가 있는경우, 전체삭제 활성화
    ListModify.LoadData("");
}

/**************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
ListModify.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListModify.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListModify.GetCount(); //검색 총 카운트 가져오기
        ListModify.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListModify.LoadData("search");
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


    setTimeout(function () {
        if (gubun == "search") {
            loadBoardSearchData();
            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);   
        }
        else {
            loadBoardData();
            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
    }, 100);

}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListModify.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/ListModify.asmx/GetFavoriteList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount,
        'order': Order
    };
    
    var obj = eval("ListModify.SuccessFavoriteList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListModify.SuccessFavoriteList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/Favorite/JTListModify.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
   
    ListModify.Resource();
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListModify.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/ListModify.asmx/GetFavoriteListCnt";

    //파라미터값 배열에 담기
    ParamArray = { };

    var obj = eval("ListModify.SuccessGetPageListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListModify.SuccessGetPageListCnt = function (data, status) {
    var response = data.d;
    TotalItemCount = response;
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListModify.Resource = function () {

    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_titleName")[0].innerHTML = R_TitleName;
    $("#r_dutyName")[0].innerHTML = R_DutyName;
    $("#r_userName")[0].innerHTML = R_UserName;
    $("#r_addDate")[0].innerHTML = R_AddDate;

    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 자주 지정하는 결재자 삭제
**********************************************************************************************/
ListModify.Del = function () {

    if (window.confirm(R_ConfirmDeleteFavoritePerson)) {

        ParamArray = {'p': 'p'};
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Ws/Preference/Favorite/ListModify.asmx/DelFavoriteUser";
        var obj = eval("ListModify.SuccessDelUser");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 자주지정하는 결재자 삭제 성공시 페이지 표시
**********************************************************************************************/
ListModify.SuccessDelUser = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리  
        ListModify.LoadData("search");
    }
}
