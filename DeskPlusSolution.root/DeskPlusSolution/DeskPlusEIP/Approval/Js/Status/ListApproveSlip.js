Approve = {}

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
            //Approve.LoadData("");
            Approve.GoPageLoad();
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
            Approve.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Approve.Search();
            }
        });

        //대결의뢰자 선택
        $('#ddlProxyList').change(function () {
            ProxyUserID = $('#ddlProxyList').val();

            // 검색값 초기화
            $("#ddlSearchTerm").val("");
            $("#startEndDate").hide(); //기간
            $("#ddlSearchField").val("Subject");
            SearchValue = $("#ctbSearch").val("");
            Approve.Search();

            setTimeout(function () {
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
            }, 100);
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            Approve.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Approve.SavePageSize();
            Approve.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            Approve.Admin();
        });

        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) {
            e.preventDefault();
            Approve.SetDefault();
        });

        //일괄결재
        $("#cbtnApprove").click(function (e) {
            e.preventDefault();
            Approve.Submit();
        });

        Approve.Init();

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
Approve.Init = function () {

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

    Approve.LoadData("");
    __addEventListener(document, "click", Approve.click);

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
Approve.RegDate = function () {
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
Approve.LoadData = function (gubun) {

    $('#ddlProxyList').val(ProxyUserID);

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Approve.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Approve.GetCount(); //검색 총 카운트 가져오기
        Approve.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Approve.LoadData("search");
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

        // 일괄결재 결재대상문서
        var totCnt = parseInt(TotalItemCount);
        var node = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById(FolderStatusID);

        if (node) {
            var nodeText = (totCnt > 0) ? node.Value + "(" + totCnt + ")" : node.Value;
            node.SetProperty('Text', nodeText);
        }

        // 결재대상문서 카운트
        var approveTotCnt = parseInt(TotalApproveCount);
        var approveNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Approve");

        if (approveNode) {
            var nodeText = (approveTotCnt > 0) ? approveNode.Value + "(" + approveTotCnt + ")" : approveNode.Value;
            approveNode.SetProperty('Text', nodeText);
        }

        // 진행중인 문서 카운트
        var processTotCnt = parseInt(TotalProcessCount);
        var processNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Process");

        if (processNode) {
            var nodeText = (processTotCnt > 0) ? processNode.Value + "(" + processTotCnt + ")" : processNode.Value;
            processNode.SetProperty('Text', nodeText);
        }

        // 완결 문서 카운트
        var completeTotCnt = parseInt(TotalCompleteCount);
        var completeNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Complete");

        if (completeNode) {
            var nodeText = (completeTotCnt > 0) ? completeNode.Value + "(" + completeTotCnt + ")" : completeNode.Value;
            completeNode.SetProperty('Text', nodeText);
        }

        // 관계있는 문서 카운트
        var relatedTotCnt = parseInt(TotalRelatedCount);
        var relatedNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Related");

        if (relatedNode) {
            var nodeText = (relatedTotCnt > 0) ? relatedNode.Value + "(" + relatedTotCnt + ")" : relatedNode.Value;
            relatedNode.SetProperty('Text', nodeText);
        }

        // 보류 문서 카운트
        var delayTotCnt = parseInt(TotalDelayCount);
        var delayNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Delay");

        if (delayNode) {
            var nodeText = (delayTotCnt > 0) ? delayNode.Value + "(" + delayTotCnt + ")" : delayNode.Value;
            delayNode.SetProperty('Text', nodeText);
        }

        // 진행중인 문서 카운트
        var processSlipTotCnt = parseInt(TotalProcessSlipCount);
        var processSlipNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("ProcessSlip");

        if (processSlipNode) {
            var nodeText = (processSlipTotCnt > 0) ? processSlipNode.Value + "(" + processSlipTotCnt + ")" : processSlipNode.Value;
            processSlipNode.SetProperty('Text', nodeText);
        }

        // 완결 문서 카운트
        var completeSlipTotCnt = parseInt(TotalCompleteSlipCount);
        var completeSlipNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("CompleteSlip");

        if (completeSlipNode) {
            var nodeText = (completeSlipTotCnt > 0) ? completeSlipNode.Value + "(" + completeSlipTotCnt + ")" : completeSlipNode.Value;
            completeSlipNode.SetProperty('Text', nodeText);
        }

        // 관계있는 문서 카운트
        var relatedSlipTotCnt = parseInt(TotalRelatedSlipCount);
        var relatedSlipNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("RelatedSlip");

        if (relatedSlipNode) {
            var nodeText = (relatedSlipTotCnt > 0) ? relatedSlipNode.Value + "(" + relatedSlipTotCnt + ")" : relatedSlipNode.Value;
            relatedSlipNode.SetProperty('Text', nodeText);
        }

        // 보류 문서 카운트
        var delaySlipTotCnt = parseInt(TotalDelaySlipCount);
        var delaySlipNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("DelaySlip");

        if (delaySlipNode) {
            var nodeText = (delaySlipTotCnt > 0) ? delaySlipNode.Value + "(" + delaySlipTotCnt + ")" : delaySlipNode.Value;
            delaySlipNode.SetProperty('Text', nodeText);
        }

        // 수신 문서 카운트
        var receiveTotCnt = parseInt(TotalReceiveCount);
        var receiveNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Receive");

        if (receiveNode) {
            var nodeText = (receiveTotCnt > 0) ? receiveNode.Value + "(" + receiveTotCnt + ")" : receiveNode.Value;
            receiveNode.SetProperty('Text', nodeText);
        }

        // 부서 수신 문서 카운트
        var receiveDeptTotCnt = parseInt(TotalReceiveDeptCount);
        var receiveDeptNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("ReceiveDept");

        if (receiveDeptNode) {
            var nodeText = (receiveDeptTotCnt > 0) ? receiveDeptNode.Value + "(" + receiveDeptTotCnt + ")" : receiveDeptNode.Value;
            receiveDeptNode.SetProperty('Text', nodeText);
        }

        // 수신반려 문서 카운트
        var receiveRejectTotCnt = parseInt(TotalReceiveRejectCount);
        var receiveRejectNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("ReceiveReject");

        if (receiveRejectNode) {
            var nodeText = (receiveRejectTotCnt > 0) ? receiveRejectNode.Value + "(" + receiveRejectTotCnt + ")" : receiveRejectNode.Value;
            receiveRejectNode.SetProperty('Text', nodeText);
        }

        // 후열 문서 카운트
        var afterwardTotCnt = parseInt(TotalAfterwardCount);
        var afterwardNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Afterward");

        if (afterwardNode) {
            var nodeText = (afterwardTotCnt > 0) ? afterwardNode.Value + "(" + afterwardTotCnt + ")" : afterwardNode.Value;
            afterwardNode.SetProperty('Text', nodeText);
        }

        parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.Render();

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
Approve.SettingSearch = function () {

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
Approve.Search = function () {

    var response = Approve.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //Approve.LoadData("search");
    Approve.GoPageLoad();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 문서 리스트 호출
**********************************************************************************************/
Approve.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/GetPageListSlip";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'proxyUserID': ProxyUserID
    };

    var obj = eval("Approve.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 문서 리스트 호출
**********************************************************************************************/
Approve.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListApproveSlip.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Approve.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Approve.Resource = function () {

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
작성목적 : 웹서비스 문서 카운트  호출
**********************************************************************************************/
Approve.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/GetListCountSlip";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Approve.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
Approve.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    Approve.LoadData("search");
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재페이지 이동
***************************************************/
var rDocID, rSeq, rTag, rUserID, rDeptID;
Approve.Approval = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {

    if (_documentID == "0" || _sequence == "0" || _approvalTag == "" || _userID == "0" || _deptID == "0") {
        alert(R_MSG_ListApprove1); return; //"전달 파라미터 확인이 필요합니다\n관리자에게 문의하세요"
    }

    rDocID = _documentID;
    rSeq = _sequence;
    rTag = _approvalTag;
    rUserID = _userID;
    rDeptID = _deptID;

    Approve.IsAccess(_documentID, _sequence, _approvalTag, _userID, _deptID);
}
// viewing 체크
Approve.IsAccess = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/GetViewing";

    ParamArray = {
        'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
        , 'targetUserID': _userID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessIsAccess");

    AjaxService(url, data, obj);
}
Approve.SuccessIsAccess = function (data, status) {
    if (data.d.toUpperCase() == "OK") {
        Approve.GoPage(rDocID, rSeq, rTag, rUserID, rDeptID);
    }
    else {
        alert("[" + data.d + "] " + R_MSG_Processing); return; //~님이 수정 또는 결재중입니다.
    }
}
// 페이지 이동
Approve.GoPage = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {
    if (_documentID == "0" || _sequence == "0" || _approvalTag == "" || _userID == "0" || _deptID == "0") {
        alert(R_MSG_ListApprove1); return; //"전달 파라미터 확인이 필요합니다\n관리자에게 문의하세요"
    }
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'listID': 'ListApprove'
        , 'folderStatusID': FolderStatusID
        , 'targetPage': TargetPage
        , 'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
        , 'targetUserID': _userID
        , 'targetDeptID': _deptID
        , 'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/Approval/Process/Basic/Approve.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황 보기
***************************************************/
Approve.State = function (_documentID, _accountTag) {

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
Approve.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Approve.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;

Approve.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Approve.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Approve.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Approve.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");
    Approve.GetAttachFileListMenu(childDocID);

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
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Approve.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, Approve.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
Approve.DownLoad = function (_e) {
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
Approve.TotalDownLoad = function (_e) {
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
Approve.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출)
***************************************************/
Approve.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefault.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessSetDefault");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출 후)
***************************************************/
Approve.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
Approve.Checked = function () {

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
Approve.CheckedValue = function () {
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
Approve.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Process'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Approve.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : Freeman
작성일자 : 2012.06.27
작성목적 : 인장 생성후 페이지 리로드
**********************************************************************************************/
var isRtnFunc = false;
function PopupWinReturnFunc() {
    if (isRtnFunc) {

        ParamArray = { 'p': 'p' };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Approve.SuccessGetSealPath");
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetSealPath";
        AjaxService(url, data, obj);
    }
}
Approve.SuccessGetSealPath = function (data, status) {
    SealPath = data.d;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2015-12-17
작성목적 : 일괄결재승인
***************************************************/
Approve.Submit = function () {

    if (SealPath == "") {
        alert(R_MSG_ERRSEAL);

        isRtnFunc = true;
        var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
        PopupWin(300, 300, url, "-500", "", true);
        return;
    }

    // 이중클릭 확인
    if (ApprovalCheck != "") { alert(R_MsgCheckClick); return; }

    //키|구분자,키|구분자
    selValue = Approve.CheckedValue();

    if (selValue != "") {
        if ($.browser.msie) {
            if (!Approve.ConfirmPassword("S")) { return; }
            Approve.Approve();
        } else {
            Approve.ConfirmPassword("S");
        }
    }
    else {
        alert(R_MSG_SelBundle);
    }
}

Approve.ConfirmPassword = function (_type) {

    if ($.browser.msie) {
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPasswordBundle.aspx";
        //WindowObjectReference = PopupModalWinOpen(url, "400", "350")
        WindowObjectReference = PopupModalWinOpen(url, "500", "350")
        return WindowObjectReference;
    }
    else { //모바일,태블리에서  window.showModalDialog 이 동작하지 않음

        ParamArray = { 'type': _type };
        CallEncrypt(CallGetParam());

        isRtnFunc = true;
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPasswordBundle.aspx?p=" + GetEncryptValue;
        PopupWin(500, 350, url, "", "", true);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : showModalDialog 를 지원하지 않는 브라우져에서 리턴 승인 처리
***************************************************/
function ConfirmPasswordFunc(_type, _opinion) {
    switch (_type.toUpperCase()) {
        case "S":   // 승인
            Approve.Approve(_opinion);
            break;
    }
}

Approve.Approve = function (_opinion) {

    ApprovalCheck = "Y";

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    $('#loading').html(loadingPath);

    $("#zeoModalBG").css({ "width": $(document).width(), "height": $(document).height() });
    $("#zeoModalBG").css({ "opacity": "0.6" });
    $("#zeoModalBG").show();

    var docVal = "";
    $.each(selValue, function (i, o) {
        var tmpArr = o.split('^');

        docVal += tmpArr[0] + "," + tmpArr[2] + "|"; // DocumentID, ApprovalTag
    });

    var opinion = $("#tbOpinion").val();

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/SaveBundle";

    ParamArray = {
        'docVal': docVal.substring(0, docVal.length - 1)
        , 'opinion': opinion
    };

    var obj = eval("Approve.SuccessApprove");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

Approve.SuccessApprove = function (data, status) {
    ApprovalCheck = "";
    alert(R_MSG_SUCCESS);
    jQuery("#zeoModalBG").fadeOut("slow");
    
    Approve.Search();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
Approve.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID,
        'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/Approval/Status/ListApproveSlip.aspx?p=" + GetEncryptValue;
}
