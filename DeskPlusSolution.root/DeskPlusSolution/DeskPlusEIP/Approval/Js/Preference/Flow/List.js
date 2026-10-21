List = {}

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
            //List.LoadData("");
            List.GoPageLoad();
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

        //부서변경내역
        $('#ModList').click(function (e) {
            e.preventDefault();
            List.ModList();
        });

        //추가 
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제 
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.SavePageSize();
            List.LoadData();
        });

        List.Init();

    });
});

/**************************************************
작성자   : 
작성일자 :2012-05-09
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    
    List.LoadData("");
}

/**************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
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
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/List.asmx/GetFlowList";

    //파라미터값 배열에 담기
    ParamArray = {
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount,
        'order': Order
    };

    var obj = eval("List.SuccessFlowList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
List.SuccessFlowList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Preference/Flow/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    List.Resource();
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/List.asmx/GetFlowListCnt";

    //파라미터값 배열에 담기
    ParamArray = { 'p' : 'p'};

    var obj = eval("List.SuccessGetFlowListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessGetFlowListCnt = function (data, status) {
    var response = data.d;
    TotalItemCount = response;
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_memo")[0].innerHTML = R_Memo;
    $("#r_addDate")[0].innerHTML = R_AddDate;
    
    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-08
작성목적 :  추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'p': 'p'
    };
    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : 
작성일자 : 2012-05-10
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Flow",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Setting'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 부서변경내역
**********************************************************************************************/
List.ModList = function () {

    ParamArray = {};
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Preference/Flow/ListModify.aspx?p=" + GetEncryptValue;
    PopupWin(500, 350, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-10
작성목적 :  삭제
**********************************************************************************************/
List.Del = function () {

    var favSeqIDs = List.CheckedValue();
    if (favSeqIDs != "") {
        if (window.confirm(R_Msg_Del)) {

            ParamArray = { 'favSeqIDs': favSeqIDs };
            
            var url = "/DeskPlusEIP/Approval/Ws/Preference/Flow/List.asmx/Del";
            var obj = eval("List.SuccessDel");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_MSG_SelDel);
    }
}
List.SuccessDel = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리  
        List.LoadData("");
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 수정
**********************************************************************************************/
List.ModLine = function (_favSeqID) {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'favSeqID': _favSeqID
    };

    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Preference/Flow/Mod.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}
