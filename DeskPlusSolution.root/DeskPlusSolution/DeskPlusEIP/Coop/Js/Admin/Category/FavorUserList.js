List = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData();
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
            CurrentPage = "1"; //현재페이지
            TotalItemCount = "0"; //게시물수
            ListCount = $('#ddlListCount').val();
            List.LoadData();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        List.Init();
    });
});

/**************************************************
작성자   :
작성일자 :
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    $("#ddlListCount").val(ListCount);
    List.LoadData();
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    var loadPageData = function () {
        List.GetCount();
        List.GetPageList();
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadPageData();
    }, 100);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/FavorUserList.asmx/GetCount";

    ParamArray = {
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/FavorUserList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'itemSubID': ItemSubID,
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Admin/Category/JTFavorUserList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($("#j_DeptName") != undefined) { $("#j_DeptName").text("부서명"); }
    if ($("#j_TitleName") != undefined) { $("#j_TitleName").text("직급명"); }
    if ($("#j_Name") != undefined) { $("#j_Name").text("성명"); }
    if ($("#j_Del") != undefined) { $("#j_Del").text("삭제"); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 삭제
**********************************************************************************************/
List.Del = function (_itemFavUserID) {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/FavorUserList.asmx/Del";

    //파라미터값 배열에 담기
    ParamArray = {
        'itemFavUserID': _itemFavUserID
    };

    var obj = eval("List.SuccessDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 삭제, 성공시
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert("처리 되었습니다.");

        CurrentPage = "1"; //현재페이지
        TotalItemCount = "0"; //게시물수

        List.LoadData();
    }
}