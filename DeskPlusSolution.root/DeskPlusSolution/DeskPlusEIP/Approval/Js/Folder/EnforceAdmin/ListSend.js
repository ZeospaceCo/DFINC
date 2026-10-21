ListSend = {}

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
            ListSend.LoadData("");
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
            ListSend.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListSend.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListSend.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListSend.Search();
        });

        ListSend.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ListSend.Init = function () {

    //아..안돼...withsuki
    if (parent.frmLeft != null) {
        var node = parent.frmLeft.UCAppFolderTreeScript4_tvFolder.FindNodeById("Enforce");
        if (node != null) {
            //node.SetProperty('CssClass', "TreeNode");
            //node.set_Selectable(false)
            //node.SetProperty('Text', "가나다라");
            //parent.frmLeft.UCAppFolderTreeScript4_tvFolder.Render();
            //node.ParentTreeView.UnCheckAll()
            //node.ParentTreeView.SelectedNode = null;
        }
    }

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

    ListSend.LoadData("");

    __addEventListener(document, "click", ListSend.click);
    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
ListSend.RegDate = function () {
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
ListSend.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListSend.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListSend.GetCount(); //검색 총 카운트 가져오기
        ListSend.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListSend.LoadData("search");
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
ListSend.SettingSearch = function () {

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
ListSend.Search = function () {

    var response = ListSend.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    ListSend.LoadData("search");
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 리스트 호출
**********************************************************************************************/
ListSend.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/EnforceAdmin/ListSend.asmx/GetPageList";

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
        'order': Order,
        'enforceStampIDArr': EnforceStampIDArr,
        'stampDeptIDArr': StampDeptIDArr
    };

    var obj = eval("ListSend.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
ListSend.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/EnforceAdmin/JTListSend.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListSend.Resource();
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

ListSend.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_reqNumber")[0].innerHTML = R_EnforceReqNumber;
    $("#r_confirmNumber")[0].innerHTML = R_EnforceCompleteNumber;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_issend")[0].innerHTML = R_EnforceIsSendInfo;
    $("#r_req")[0].innerHTML = R_EnforceReqInfo;
    $("#r_confirm")[0].innerHTML = R_EnforceConfirmInfo;
    $("#r_writetime")[0].innerHTML = R_Writetime;
    $("#r_confirmdate")[0].innerHTML = R_EnforceConfirmDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 웹서비스 결재상황조회 상신한 문서 카운트  호출
**********************************************************************************************/
ListSend.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/EnforceAdmin/ListSend.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'enforceStampIDArr': EnforceStampIDArr,
        'stampDeptIDArr': StampDeptIDArr
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListSend.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListSend.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 정렬
**********************************************************************************************/
ListSend.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    ListSend.LoadData("search");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 클릭 이벤트 분기
***************************************************/
ListSend.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "subject": ListSend.ShowSubjectContextMenu(e); break; // 제목 클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 제목 클릭시 컨텍스트 메뉴
***************************************************/
ListSend.ShowSubjectContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var enforceID = el.getAttribute("enforceID");

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    var oItem = oContextMenuCtrl.addItem(R_EnforceView, ListSend.EnforceView_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("enforceID", enforceID, 0); // 신청내용보기

    oItem = oContextMenuCtrl.addItem(R_EnforceSend, ListSend.Send_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("enforceID", enforceID, 0); // 공문변환

    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(신청내용보기)
***************************************************/
ListSend.EnforceView_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var enforceID = evt.target.getAttribute("enforceID");
        ListSend.EnforceView(enforceID);
    }
}
ListSend.EnforceView = function (_enforceID) {
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
    location.href = "/DeskPlusEIP/Approval/Folder/Enforce/View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(공문변환)
***************************************************/
ListSend.Send_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var enforceID = evt.target.getAttribute("enforceID");
        ListSend.Send(enforceID);
    }
}
ListSend.Send = function (_enforceID) {
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
    location.href = "/DeskPlusEIP/Approval/Folder/Enforce/Send.aspx?p=" + GetEncryptValue;
}