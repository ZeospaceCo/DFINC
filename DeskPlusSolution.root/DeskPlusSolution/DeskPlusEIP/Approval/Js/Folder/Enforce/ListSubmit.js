ListSubmit = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ListSubmit.LoadData("");
            ListSubmit.GoPageLoad();
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

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListSubmit.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListSubmit.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListSubmit.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListSubmit.SavePageSize();
            ListSubmit.Search();
        });

        ListSubmit.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ListSubmit.Init = function () {

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

    ListSubmit.LoadData("");

    __addEventListener(document, "click", ListSubmit.click);
    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListSubmit.RegDate = function () {
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListSubmit.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListSubmit.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListSubmit.GetCount(); //검색 총 카운트 가져오기
        ListSubmit.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListSubmit.LoadData("search");
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
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}


/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListSubmit.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_MSG_NeedAddDate);
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ListSubmit.Search = function () {

    var response = ListSubmit.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //ListSubmit.LoadData("search");
    ListSubmit.GoPageLoad();
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 리스트 호출
**********************************************************************************************/
ListSubmit.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/ListSubmit.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ListSubmit.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
ListSubmit.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Enforce/JTListSubmit.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListSubmit.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ListSubmit.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_reqNumber")[0].innerHTML = R_EnforceReqNumber;
    $("#r_docNumber")[0].innerHTML = R_DocNumber;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_writetime")[0].innerHTML = R_Writetime;
    $("#r_receive")[0].innerHTML = R_ReceiveDept;
    $("#r_state")[0].innerHTML = R_EnforceStateInfo;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 웹서비스 결재상황조회 상신한 문서 카운트  호출
**********************************************************************************************/
ListSubmit.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/ListSubmit.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListSubmit.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListSubmit.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 정렬
**********************************************************************************************/
ListSubmit.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    ListSubmit.LoadData("search");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 클릭 이벤트 분기
***************************************************/
ListSubmit.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        //case "attach": Process.ShowAttachContextMenu(e); break; //첨부파일 클릭 
        case "subject": ListSubmit.ShowSubjectContextMenu(e); break; // 제목 클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 제목 클릭시 컨텍스트 메뉴
***************************************************/
ListSubmit.ShowSubjectContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var enforceID = el.getAttribute("enforceID");
    var docID = el.getAttribute("docID");
    var state = el.getAttribute("state");
    
    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(R_ViewDoc, ListSubmit.View_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("docID", docID, 0); // 문서 보기

    oItem = oContextMenuCtrl.addItem(R_EnforceView, ListSubmit.EnforceView_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("enforceID", enforceID, 0); // 신청내용보기

    switch (state) {
        case "W": //승인대기
            oItem = oContextMenuCtrl.addItem(R_EnforceEdit, ListSubmit.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
            oItem.txt.setAttribute("enforceID", enforceID, 0); // 신청내용수정
            oItem.txt.setAttribute("state", state, 0);          // "W"
            break;
        case "R":  //반려
            oItem = oContextMenuCtrl.addItem(R_EnforceReAdd, ListSubmit.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
            oItem.txt.setAttribute("enforceID", enforceID, 0);  // 수정후재신청
            oItem.txt.setAttribute("state", state, 0);          // "R"
            break;
    }

    oItem = oContextMenuCtrl.addItem(R_EnforceDel, ListSubmit.Del_onClick, ImagePath + "/Icon/ico_tree_listDel.gif");
    oItem.txt.setAttribute("enforceID", enforceID, 0); // 신청취소

    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(문서보기)
***************************************************/
ListSubmit.View_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var docID = evt.target.getAttribute("docID");
        ListSubmit.View(docID);
    }
}
ListSubmit.View = function (_documentID) {

    ParamArray = {
        'documentID': _documentID,
        'currentPage': CurrentPage,
        'recordPerPage' : RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'targetPage': TargetPage
    };
    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(신청내용수정/수정후재신청)
***************************************************/
ListSubmit.Edit_onClick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var enforceID = evt.target.getAttribute("enforceID");
        var state = evt.target.getAttribute("state");
        ListSubmit.Edit(enforceID, state);
    }
}
ListSubmit.Edit = function (_enforceID, _state) {

    ParamArray = {
        'enforceID': _enforceID,
        'state' : _state,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'targetPage': TargetPage,
        'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(신청내용보기)
***************************************************/
ListSubmit.EnforceView_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var enforceID = evt.target.getAttribute("enforceID");
        ListSubmit.EnforceView(enforceID);
    }
}
ListSubmit.EnforceView = function (_enforceID) {
    ParamArray = {
        'enforceID': _enforceID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'targetPage': TargetPage,
        'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(신청취소)
***************************************************/
ListSubmit.Del_onClick = function (e) {
    if (confirm(R_Msg_Del)) {
        var evt = new __event(e);
        if (evt.target) {
            var enforceID = evt.target.getAttribute("enforceID");
            ListSubmit.Del(enforceID);
        }
    }
}
ListSubmit.Del = function (_enforceID) {

    if (_enforceID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/ListSubmit.asmx/Del";
    ParamArray = {
        'enforceID': _enforceID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListSubmit.SuccessDel");

    AjaxService(url, data, obj);
}
ListSubmit.SuccessDel = function (data, status) {

    if (data.d == true) {
        CurrentPage = 1;
        ListSubmit.LoadData("search");
    }
}


/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
ListSubmit.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "EnforceSubmit",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Complete'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListSubmit.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
ListSubmit.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-18
작성목적 : 페이지이동 처리
**********************************************************************************************/
ListSubmit.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'cabinetID': CabinetID
    };

    CallEncrypt(CallGetParam());
    location = "ListSubmit.aspx?p=" + GetEncryptValue;
}
