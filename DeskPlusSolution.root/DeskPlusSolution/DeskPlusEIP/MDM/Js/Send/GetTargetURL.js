GetTargetURL = {}

var oContextMenuCtrl = null;

/**************************************************
작성자 : famo23
작성일자 : 2020-06-11
작성목적 : 초기화 
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            GetTargetURL.LoadData("");
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


        //바로가기 URL 등록
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetTargetURL.SetURL();            
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GetTargetURL.LoadData("");

    });
});

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
작성자 : famo23
작성일자 : 2020-06-11
작성목적 : 초기화
***************************************************/
GetTargetURL.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoarddData = function () {
        GetTargetURL.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        GetTargetURL.GetCount(); //검색 총 카운트 가져오기
        GetTargetURL.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                GetTargetURL.LoadData("search");
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
        }
        else {
            loadBoarddData();
        }
    }, 100);

}

/**************************************************
작성자 : famo23
작성일자 : 2020-06-11
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    CatID = node.ID;
    CurrentPage = 1; //1페이지로 이동처리     

    GetTargetURL.LoadData("search");

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-11
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetTargetURL.GetPageList = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Send/GetTargetURL.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'categoryID': CatID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': "",
        'searchTermValue': "",
        'searchGroup': "1",
        'searchField': "",
        'searchValue': "",
        'order': "OptionTop desc, GroupID DESC, ListOrder ASC, WriteDate desc"
    };

    var obj = eval("GetTargetURL.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-11
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
GetTargetURL.SuccessPageList = function (data, status) {

    $('#phBoard').setTemplateURL('/DeskPlusEIP/MDM/Jt/Send/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phBoard').processTemplate(data.d);

    GetTargetURL.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-12
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
GetTargetURL.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_title")[0].innerHTML = R_Subject;
    $("#r_view")[0].innerHTML = R_DocView;
    $("#r_register")[0].innerHTML = R_Register;
    $("#r_regDate")[0].innerHTML = R_RegDate;

    if ($(".s_notice") != undefined) { $(".s_notice").text(R_Notice); }
    if ($("#NoListData") != undefined) { $("#NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-12
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
GetTargetURL.GetCount = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Send/GetTargetURL.asmx/GetListCount";
    ParamArray = {
        'categoryID': CatID,
        'searchTerm': "",
        'searchTermValue': "",
        'searchGroup': "1",
        'searchField': "",
        'searchValue': ""
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetTargetURL.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-12
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
GetTargetURL.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-15
작성목적 : 게시판 제목 선택
**********************************************************************************************/
GetTargetURL.SetTitle = function (_categoryID, _listID, _title) {

    var tmpTilte = _title.replace(/zeo1/gi, '"');
    tmpTilte = tmpTilte.replace(/zeo2/gi, "'");
    $("#tbTitle").val(tmpTilte);
    $("#hdnCategoryID").val(_categoryID);
    $("#hdnListID").val(_listID);
    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-06-15
작성목적 : 확인
**********************************************************************************************/
GetTargetURL.SetURL = function () {

    var tmpCategoryID = $("#hdnCategoryID").val();
    var tmpListID = $("#hdnListID").val();
    var tmpTitle = $("#tbTitle").val();

    if (tmpListID == "" || tmpListID == "0") {
        alert(R_SelectSubject);
        return;
    }

    opener.SetTarget(tmpCategoryID, tmpListID, escape(tmpTitle));

    self.close();
}

/**************************************************
작성자   : famo23
작성일자 : 2020-06-16
작성목적 : 새창으로 게시판 글 보기
***************************************************/
GetTargetURL.PopupShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "PopupShortView", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

