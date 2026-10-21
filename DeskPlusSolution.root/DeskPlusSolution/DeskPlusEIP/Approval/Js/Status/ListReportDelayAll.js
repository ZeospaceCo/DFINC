Process = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            //Process.LoadData("");
            Process.GoPageLoad();
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


        //alert(TotalItemCount + "-" + RecordPerPage + "-" + CurrentPage + "-" + Pager.pageIndexChanged);

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            Process.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Process.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            Process.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            Process.Admin();
        });

        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) {
            e.preventDefault();
            Process.SetDefault();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Process.SavePageSize();
            Process.Search();
        });

        Process.Init();

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
Process.Init = function () {

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

    Process.LoadData("");
    __addEventListener(document, "click", Process.click);

    $("#zeoModalContact").draggable({ disabled: false });

}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
Process.RegDate = function () {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
Process.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Process.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Process.GetCount(); //검색 총 카운트 가져오기
        Process.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Process.LoadData("search");
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

    // 좌측 트리 카운트  총 카운트 변경
    try {
        var totCnt = parseInt(TotalItemCount);
        var node = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById(FolderStatusID);

        if (node) {
            var nodeText = (totCnt > 0) ? node.Value + "(" + totCnt + ")" : node.Value;
            node.SetProperty('Text', nodeText);
            parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.Render();
        }
    } catch (e) { }

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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
Process.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
Process.Search = function () {

    var response = Process.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //Process.LoadData("search");
    Process.GoPageLoad();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
Process.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetPageList";

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

    var obj = eval("Process.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
Process.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListProcess.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Process.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Process.Resource = function () {

    $("#r_formName")[0].innerHTML = R_FormName;
    $("#r_approvalState")[0].innerHTML = R_Status;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attach")[0].innerHTML = R_AttachName;
    $("#r_user")[0].innerHTML = R_Name;
    $("#r_nextUser")[0].innerHTML = R_NextName;
    $("#r_writeTime")[0].innerHTML = R_WriteTime;

    $(".r_w").text(R_W);
    $(".r_i").text(R_I);
    $(".r_r").text(R_R);
    $(".r_d").text(R_D);
    $(".r_c").text(R_C);
    $(".r_h").text(R_H);
    $(".r_p").text(R_P);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 웹서비스 결재상황조회 상신한 문서 카운트  호출
**********************************************************************************************/
Process.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetListCount";
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
    var obj = eval("Process.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Process.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 보기
***************************************************/
Process.View = function (_documentID) {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'documentID': _documentID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listID': "ListProcess",
        'folderStatusID': FolderStatusID,
        'targetPage': TargetPage
    };

    CallEncrypt(CallGetParam());

    location.href = "Common/View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
Process.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    //Process.LoadData("search");
    Process.GoPageLoad();
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황 보기
***************************************************/
Process.State = function (_documentID, _accountTag) {    //alert(_documentID + "," + _accountTag);

    ParamArray = {
        'isComplete': 'N'
        , 'documentID': _documentID
        , 'accountTag': _accountTag
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Status/Common/GetDocFlow.aspx?p=" + GetEncryptValue;
    PopupWin(850, 400, url, "-500", "", true);
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
Process.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Process.ShowAttachContextMenu(e); break; //첨부파일 클릭
        case "subject": Process.ShowSubjectContextMenu(e); break; // 제목 클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자   : 이문호
작성일자 : 2012-06-11
작성목적 : 제목 클릭시 컨텍스트 메뉴
***************************************************/
Process.ShowSubjectContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var docID = el.getAttribute("docID");
    var state = el.getAttribute("state");
    var accountTag = el.getAttribute("accountTag");
    var nextSeq = el.getAttribute("nextSeq");
    var groupID = el.getAttribute("groupID");
    var myTurn = el.getAttribute("myTurn");
    var nextTag = el.getAttribute("approvalTag");
    var lastTag = el.getAttribute("lastTag");
    var islegacy = el.getAttribute("isLegacy");
    var fileName = el.getAttribute("fileName");

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();
    var oItem = oContextMenuCtrl.addItem(R_ViewDoc, Process.View_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("arg", docID, 0); // 문서 보기

    switch (state) {
        case "W": //결재대기
            oItem = oContextMenuCtrl.addItem(R_EditDoc, Process.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
            oItem.txt.setAttribute("docID", docID, 0);  //본문/결재선 수정
            oItem.txt.setAttribute("state", state, 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("groupID", groupID, 0);

            oItem = oContextMenuCtrl.addItem(R_DelayApp, Process.ApproveH_onClick, ImagePath + "/Icon/CheckOutStatus.gif");
            oItem.txt.setAttribute("docID", docID, 0); //상신 보류
            oItem.txt.setAttribute("state", "H", 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("groupID", groupID, 0);

            oItem = oContextMenuCtrl.addItem(R_CancelApp, Process.ApproveDel_onClick, ImagePath + "/Icon/ico_tree_listDel.gif");
            oItem.txt.setAttribute("groupID", groupID, 0); //상신 취소
            oItem.txt.setAttribute("docID", docID, 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("islegacy", islegacy, 0);
            oItem.txt.setAttribute("fileName", fileName, 0);


            if (nextTag != lastTag && nextTag != "") {
                oItem = oContextMenuCtrl.addItem(R_LaterOn, Process.ApproveLater_onClick, ImagePath + "/Icon/ico_lateron.gif");
                oItem.txt.setAttribute("docID", docID, 0);  //후열처리
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("nextTag", nextTag, 0);
                oItem.txt.setAttribute("nextSeq", nextSeq, 0);
                oItem.txt.setAttribute("state", state, 0);
            }

            break;
        case "R":  //반려
            if (myTurn == "Y") {
                oItem = oContextMenuCtrl.addItem(R_EditNReport, Process.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
                oItem.txt.setAttribute("docID", docID, 0); //수정후 재상신
                oItem.txt.setAttribute("state", state, 0);
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("groupID", groupID, 0);

                /* 발신부서 상신자 반려 시, 완료처리(DocTable)되기때문에 해당 리스트에서 조회불가
                if (docID == groupID) {
                // 발신부서 상신자에게 반려 시에만, 문서폐기 가능. 2016-01-28
                oItem = oContextMenuCtrl.addItem(R_DiscardDoc, Process.ApproveDel_onClick, ImagePath + "/Icon/ico_tree_listDel.gif");
                oItem.txt.setAttribute("groupID", groupID, 0); //문서 폐기
                oItem.txt.setAttribute("docID", docID, 0);
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("islegacy", islegacy, 0);
                oItem.txt.setAttribute("fileName", fileName, 0);
                }
                */
                // 발신부서 상신자에게 반려? 또는 그 이전 수신부서 에게 반려하는걸 넣어야할까?
            }

            break;
        case "H": // 상신 보류
            oItem = oContextMenuCtrl.addItem(R_EditDoc, Process.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
            oItem.txt.setAttribute("docID", docID, 0);  //본문/결재선 수정
            oItem.txt.setAttribute("state", state, 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("groupID", groupID, 0);

            oItem = oContextMenuCtrl.addItem(R_CancelDelayApp, Process.ApproveH_onClick, ImagePath + "/Icon/icon_rollback.gif");
            oItem.txt.setAttribute("docID", docID, 0);  //상신 보류 해제
            oItem.txt.setAttribute("state", "W", 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("groupID", groupID, 0);
/*
            oItem = oContextMenuCtrl.addItem(R_CancelApp, Process.ApproveDel_onClick, ImagePath + "/Icon/ico_tree_listDel.gif");
            oItem.txt.setAttribute("groupID", groupID, 0);  //상신 취소
            oItem.txt.setAttribute("docID", docID, 0);
            oItem.txt.setAttribute("accountTag", accountTag, 0);
            oItem.txt.setAttribute("islegacy", islegacy, 0);
            oItem.txt.setAttribute("fileName", fileName, 0);
*/
            break;
        case "I": //진행중
            if (accountTag != "ZD") {
                // ZP,ZQ의 경우, 수신문서 상신 시, 상태값이 'I'이므로, 다음결재자 순서체크 
                // ZD인경우는 다음결재자가 ZA인 경우, NextSeq가 2일수 있음. (= 수신결재망은 병렬합의결재망 불가)
                if (nextSeq == 2) {
                    oItem = oContextMenuCtrl.addItem(R_EditDoc, Process.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
                    oItem.txt.setAttribute("docID", docID, 0); // 본문/결재선 수정
                    oItem.txt.setAttribute("state", state, 0);
                    oItem.txt.setAttribute("accountTag", accountTag, 0);
                    oItem.txt.setAttribute("groupID", groupID, 0);

                    oItem = oContextMenuCtrl.addItem(R_CancelSusin, Process.ApproveCancel_onClick, ImagePath + "/Icon/ico_tree_listDel.gif");
                    oItem.txt.setAttribute("groupID", groupID, 0); //수신 회수
                    oItem.txt.setAttribute("docID", docID, 0);
                    oItem.txt.setAttribute("accountTag", accountTag, 0);
                }
            }

            if ((accountTag == "ZD") && (docID != groupID) && (nextSeq == 2)) {
                oItem = oContextMenuCtrl.addItem(R_EditDoc, Process.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
                oItem.txt.setAttribute("docID", docID, 0); // 본문/결재선 수정
                oItem.txt.setAttribute("state", state, 0);
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("groupID", groupID, 0);
            }

            if (nextTag != lastTag && nextTag != "") {
                oItem = oContextMenuCtrl.addItem(R_LaterOn, Process.ApproveLater_onClick, ImagePath + "/Icon/ico_lateron.gif");
                oItem.txt.setAttribute("docID", docID, 0);  //후열처리
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("nextTag", nextTag, 0);
                oItem.txt.setAttribute("nextSeq", nextSeq, 0);
                oItem.txt.setAttribute("state", state, 0);
            }

            if ((docID != groupID) && (nextSeq == 2)) {
                oItem = oContextMenuCtrl.addItem(R_DelayApp, Process.ApproveH_onClick, ImagePath + "/Icon/CheckOutStatus.gif");
                oItem.txt.setAttribute("docID", docID, 0); //상신 보류
                oItem.txt.setAttribute("state", "H", 0);
                oItem.txt.setAttribute("accountTag", accountTag, 0);
                oItem.txt.setAttribute("groupID", groupID, 0);
            }

            break;
        case "D": //보류
        case "P": //상신대기(수신)
            break;
    }

    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(문서보기)
***************************************************/
Process.View_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        Process.View(arg);
    }
    //oContextMenuCtrl.hide();
}

// viewing 체크
var FuncName;
Process.IsAccess = function (_documentID, _funcName) {    //alert(_documentID + "," + _funcName);

    if (_documentID == 0 || _funcName == "") return;

    FuncName = _funcName;
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetViewing";

    ParamArray = {
        'documentID': _documentID
        , 'sequence': '1'
        , 'approvalTag': 'ZD0'
        , 'targetUserID': UserID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Process.SuccessIsAccess");

    AjaxService(url, data, obj);
}
Process.SuccessIsAccess = function (data, status) {    //alert(FuncName)

    if (data.d.toUpperCase() == "OK") {
        if (FuncName != "") eval(FuncName);
    }
    else {
        alert("[" + data.d + "] " + R_MSG_Processing); return; //님이 수정 또는 결재중입니다.
    }
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(수정후재상신)
***************************************************/
var eDocID, eState, eAccountTag, eGroupID;
Process.Edit_onClick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        eDocID = evt.target.getAttribute("docID");
        eState = evt.target.getAttribute("state");
        eAccountTag = evt.target.getAttribute("accountTag");
        eGroupID = evt.target.getAttribute("groupID");

        Process.IsAccess(eDocID, "Process.Edit_GoPage()");
    }
}
Process.Edit_GoPage = function () {
    ParamArray = {
        'recordPerPage': RecordPerPage
            , 'currentPage': CurrentPage
            , 'searchTerm': SearchTerm
            , 'searchTermValue': SearchTermValue
            , 'searchField': SearchField
            , 'searchValue': SearchValue
            , 'order': Order
            , 'listID': 'ListProcess'
            , 'folderStatusID': FolderStatusID
            , 'targetPage': TargetPage

            , 'docID': eDocID
            , 'state': eState
            , 'accountTag': eAccountTag
            , 'groupID': eGroupID
    };

    CallEncrypt(CallGetParam());
    parent.location.href = "/DeskPlusEIP/Approval/FormFlow/DefaultAll.aspx?p=" + GetEncryptValue;
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(상신보류H/상신보류해제W)
***************************************************/
var hDocID, hState, hAccountTag, hGroupID;
Process.ApproveH_onClick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        hDocID = evt.target.getAttribute("docID");
        hState = evt.target.getAttribute("state");
        hAccountTag = evt.target.getAttribute("accountTag");
        hGroupID = evt.target.getAttribute("groupID");

        Process.IsAccess(hDocID, "Process.ApproveH_GoPage()");
    }
}
Process.ApproveH_GoPage = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/ApproveH";
    ParamArray = { 'docID': hDocID, 'accountTag': hAccountTag, 'state': hState, 'groupID': hGroupID };
    var obj = eval("Process.SuccessApproveH");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Process.SuccessApproveH = function (data, status) {
    if (data != null) {
        if (data.d == "TRUE") {
            alert(R_MSG_SUCCESS);
            location.reload();
        } else {
            alert(R_MSG_FAIL);
        }
    }
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(상신취소/문서폐기)
***************************************************/
var cDocID, cGroupID, cAccountTag, cIsLegacy, cFileName;        //연동모듈에서도 사용. 
Process.ApproveDel_onClick = function (e) {

    $("#ifrmProcessing").attr("src", "about:blank");

    if (confirm(R_Msg_Del)) {
        var evt = new __event(e);
        if (evt.target) {
            cDocID = evt.target.getAttribute("docID");
            cGroupID = evt.target.getAttribute("groupID");
            cAccountTag = evt.target.getAttribute("accountTag");
            cIsLegacy = evt.target.getAttribute("islegacy");
            cFileName = evt.target.getAttribute("fileName");

            if (cAccountTag.indexOf("ZP") == -1) {
                if ((cAccountTag.indexOf("ZQ") == -1) && (cDocID != cGroupID)) {
                    //이관받은 문서 
                    Process.IsAccess(cDocID, "Process.ApproveDel_GoPage()");
                }
                else {
                    Process.IsAccess(cGroupID, "Process.ApproveDel_GoPage()");
                }
            }
            else {  // 파란수신이면
                Process.IsAccess(cDocID, "Process.ApproveDel_GoPage()");
            }
        }
    }
}
Process.ApproveDel_GoPage = function () {

    // 연동체크
    if (cIsLegacy == "1" && cFileName != "") {

        var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetCustomFileName";
        ParamArray = { 'fileName': cFileName };

        var obj = eval("Process.SuccessCustomChk");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);

    } else {
        ApproveDel();
    }
}
Process.SuccessCustomChk = function (data, status) {
    if (data != null) {
        if (data.d == "") {
            ApproveDel();
        } else {
            ifrmProcessing.document.location.href = data.d + "?isaction=Y&seq=" + Guids();
            //ApproveDel() 는 커스텀에서 호출.
        }
    }
}

//문서삭제
ApproveDel = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/ApproveDelete";
    ParamArray = {
        'groupID': cGroupID,
        'documentID': cDocID,
        'accountTag': cAccountTag
    };

    var obj = eval("Process.SuccessApproveDel");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Process.SuccessApproveDel = function (data, status) {
    if (data != null) {
        if (data.d == "TRUE") {
            alert(R_MSG_SUCCESS);
            location.reload();
        } else {
            alert(R_MSG_FAIL);
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(수신회수)
***************************************************/
Process.ApproveCancel_onClick = function (e) {

    if (confirm(R_ConfirmCancelSusin)) { // 회수하시겠습니까?
        var evt = new __event(e);
        if (evt.target) {
            cDocID = evt.target.getAttribute("docID");
            cAccountTag = evt.target.getAttribute("accountTag");

            Process.IsAccess(cDocID, "Process.ApproveCancel_GoPage()");
        }
    }
}
Process.ApproveCancel_GoPage = function () {

    if (cAccountTag.indexOf("ZQ") > -1) {
        var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetContentHTML";
        ParamArray = {
            'documentID': cDocID
        };

        var obj = eval("Process.SuccessContentHTML");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
    else {
        Process.CancelSusin();
    }
}
Process.SuccessContentHTML = function (data, status) {
    if (data != null) {
        if (data.d != "!NULL") {
            ifrmForm.document.open();
            ifrmForm.document.write(data.d);
            ifrmForm.document.close();

            ifrmObj = ifrmForm.document;
            setTimeout("Process.CheckOld();", 100);
        } else {
            alert(R_MSG_FAIL);
        }
    }
}
Process.CancelSusin = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/CancelSusin";
    ParamArray = {
        'documentID': cDocID,
        'accountTag': cAccountTag,
        'contentHTML': encodeURIComponent($("#htaFormHTML").val())
    };

    var obj = eval("Process.SuccessCancelSusin");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Process.SuccessCancelSusin = function (data, status) {
    if (data != null) {
        if (data.d == "TRUE") {
            alert(R_MSG_SUCCESS);
            location.reload();
        } else {
            alert(R_MSG_FAIL);
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 수신회수용 본문내용 복원
***************************************************/
Process.CheckOld = function () {

    var acctNo = Number(cAccountTag.replace("ZQ", ""));
    var downAcctNo = acctNo - 1;
    if (acctNo < 10) acctNo = "0" + acctNo;
    if (downAcctNo < 10) downAcctNo = "0" + downAcctNo;

    // 상위 결재망을 지우고, AccountTag에 해당하는 old만 제거한다.
    if ($("#PARENTDOCUMENT_ZQ" + acctNo, ifrmObj).length > 0) {
        ifrmObj.body.innerHTML = $("#PARENTDOCUMENT_ZQ" + acctNo, ifrmObj).html();

        setTimeout(function () {
            var strOldZQDic = new jQuery.Hashtable();

            if ($("#PARENTDOCUMENT_ZQ" + downAcctNo, ifrmObj).length > 0) {
                var zTagsZQ = $("span", $("#PARENTDOCUMENT_ZQ" + downAcctNo, ifrmObj).html());
                $.each(zTagsZQ, function (i, o) {
                    if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                        strOldZQDic.add(o.id.split("_")[0], o.id.split("_")[0]);
                    }
                });
            }

            // old제거
            var strOld = "";
            var zTags = $("span", ifrmObj);
            $.each(zTags, function (i, o) {
                if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                    if (!strOldZQDic.contains(o.id.split("_")[0])) {
                        strOld += o.id.split("_")[0] + ",";
                    }
                }
            });

            $.each(strOld.trimEnds(',').split(','), function (i, o) {
                var oldTag = o + "_";

                // 변경대상 초기화 (span & div & input) : 결재태그
                var tobj = "." + oldTag + "ZD," + "." + oldTag + "ZP," + "." + oldTag + "ZQ," + "." + oldTag + "ZH," + "." + oldTag + "ZC," + "." + oldTag + "ZR," + "." + oldTag + "ZA," + "." + oldTag + "ZB," + "." + oldTag + "formfieldinput";
                var arrTag = $(tobj, ifrmObj);
                $.each(arrTag, function (i, o) {
                    o.id = o.id.replace(oldTag, "");
                    o.className = o.className.replace(oldTag, "");
                });
            });

            var content = RemoveScriptTag($("HTML", ifrmObj).html());
            $("#htaFormHTML").val("<HTML>" + content + "</HTML>");

        }, 100);

    }

    setTimeout("Process.CancelSusin();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 컨텍스트메뉴 클릭시(후열처리)
***************************************************/
var oDocID, oAccountTag, oNextTag, oNextSeq, oState;
Process.ApproveLater_onClick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        oDocID = evt.target.getAttribute("docID");
        oAccountTag = evt.target.getAttribute("accountTag");
        oNextTag = evt.target.getAttribute("nextTag");
        oNextSeq = evt.target.getAttribute("nextSeq");
        oState = evt.target.getAttribute("state");

        if (oNextTag.indexOf("ZA") < 0) {
            Process.IsAccess(oDocID, "Process.ApproveLater_GoPage()");
        } else {
            Process.GetZASeqLine(oDocID, oNextSeq, oNextTag);
        }
    }
}
Process.ApproveLater_GoPage = function () {
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'listID': 'ListProcess'
        , 'folderStatusID': FolderStatusID
        , 'targetPage': TargetPage
        , 'documentID': oDocID
        , 'accountTag': oAccountTag
        , 'approvalTag': oNextTag
        , 'sequence': oNextSeq
        , 'approvalState': oState
    };
    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Process/Afterward/Approve.aspx?p=" + GetEncryptValue;
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 후열처리 시, 미결재 합의병렬 결재자 리스트가져오기
***************************************************/
Process.GetZASeqLine = function (_documentID, _sequence, _approvalTag) {    //alert(_documentID + "," + _accountTag);

    ParamArray = {
        'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Process/Afterward/ConfirmSeqLine.aspx?p=" + GetEncryptValue;

    if ($.browser.msie) {
        WindowObjectReference = PopupModalWinOpen(url, "500", "246");

        var rtnval = (WindowObjectReference == undefined || WindowObjectReference == "") ? "!NULL" : WindowObjectReference;
        if (rtnval == "!NULL" || rtnval == "") { return; }

        var arrval = rtnval.split("-");
        oDocID = arrval[0];
        oNextSeq = arrval[1];
        oNextTag = arrval[2];

        Process.IsAccess(oDocID, "Process.ApproveLater_GoPage()");
    } else {
        PopupWin(500, 246, url, "", "", true);
    }
}

function ConfirmSeqLineFunc(rtnval) {
    if (rtnval == "!NULL" || rtnval == "") { return; }
    //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag

    var arrval = rtnval.split("-");
    oDocID = arrval[0];
    oNextSeq = arrval[1];
    oNextTag = arrval[2];

    Process.IsAccess(oDocID, "Process.ApproveLater_GoPage()");
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 컨텍스트 메뉴 생성
***************************************************/
Process.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");

    Process.GetAttachFileListMenu(childDocID);

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        /*
        if ($.browser.msie) {
        if (attachArr.length > 1) {
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Process.TotalDownLoad, ImagePath + "/Icon/folders.gif");
        oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
        oContextMenuCtrl.addBlank();
        }
        }
        */

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            var oItem = oContextMenuCtrl.addItem(attachFileName, Process.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 첨부파일 리스트 조회(웹서비스)
**********************************************************************************************/
Process.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Process.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Process.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
Process.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;     
        window.open(arg);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 전체 다운로드
***************************************************/
Process.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        //window.open("", "openWin", "width=506px,height=388px,scrollbars=no,status=no,toolbar=no,resizable=0,location=no,menu=no");
        //$("#frmTotalDownLoad").submit();
        document.getElementById("frmTotalDownLoad").submit();

    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
Process.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출)
***************************************************/
Process.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefaultAll.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Process.SuccessSetDefault");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출 후)
***************************************************/
Process.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
Process.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
Process.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Process.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReportDelayAll.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Process'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Process.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Process.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
Process.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    location = "ListReportDelayAll.aspx?p=" + GetEncryptValue;
}
