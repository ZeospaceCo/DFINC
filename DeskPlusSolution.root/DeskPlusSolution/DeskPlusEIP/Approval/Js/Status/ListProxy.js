Proxy = {}

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
            //Proxy.LoadData("");
            Proxy.GoPageLoad();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalProxyCount,
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
            Proxy.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Proxy.Search();
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
            Proxy.Search();

            setTimeout(function () {
                // 좌측 트리 카운트  총 카운트 변경
                try {
                    var totCnt = parseInt(TotalProxyCount);
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
            Proxy.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Proxy.SavePageSize();
            Proxy.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            Proxy.Admin();
        });

        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) {
            e.preventDefault();
            Proxy.SetDefault();
        });

        Proxy.Init();

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
Proxy.Init = function () {

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
   
    Proxy.LoadData("");
    __addEventListener(document, "click", Proxy.click);

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
Proxy.RegDate = function () {
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
Proxy.LoadData = function (gubun) {

    $('#ddlProxyList').val(ProxyUserID);

    var pageCount = $("#pager").pager.refresh(TotalProxyCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Proxy.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Proxy.GetCount(); //검색 총 카운트 가져오기
        Proxy.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Proxy.LoadData("search");
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalProxyCount,
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

        // 결재대상문서
        var totCnt = parseInt(TotalItemCount);
        var node = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById(FolderStatusID);

        if (node) {
            var nodeText = (totCnt > 0) ? node.Value + "(" + totCnt + ")" : node.Value;
            node.SetProperty('Text', nodeText);
        }

        // 수신반려 문서 카운트
        var proxyTotCnt = parseInt(TotalProxyCount);
        var proxyNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Proxy");

        if (proxyNode) {
            var nodeText = (proxyTotCnt > 0) ? proxyNode.Value + "(" + proxyTotCnt + ")" : proxyNode.Value;
            proxyNode.SetProperty('Text', nodeText);
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

        // 결재대상문서 카운트
        var approveTotCnt = parseInt(TotalApproveSlipCount);
        var approveNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("ApproveSlip");

        if (approveNode) {
            var nodeText = (approveTotCnt > 0) ? approveNode.Value + "(" + approveTotCnt + ")" : approveNode.Value;
            approveNode.SetProperty('Text', nodeText);
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
Proxy.SettingSearch = function () {

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
Proxy.Search = function () {

    var response = Proxy.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //Proxy.LoadData("search");
    Proxy.GoPageLoad();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 문서 리스트 호출
**********************************************************************************************/
Proxy.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProxy.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalProxyCount,
        'recordPerPage': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'proxyUserID': ProxyUserID
    };

    var obj = eval("Proxy.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 문서 리스트 호출
**********************************************************************************************/
Proxy.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListProxy.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Proxy.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Proxy.Resource = function () {

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
Proxy.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProxy.asmx/GetListCount";
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
    var obj = eval("Proxy.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Proxy.SuccessCount = function (data, status) {
    TotalProxyCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalProxyCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}




/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
Proxy.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    Proxy.LoadData("search");
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재페이지 이동
***************************************************/
var rDocID, rSeq, rTag, rUserID, rDeptID;
Proxy.Approval = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {

    if (_documentID == "0" || _sequence == "0" || _approvalTag == "" || _userID == "0" || _deptID == "0") {
        alert(R_MSG_ListApprove1); return; //"전달 파라미터 확인이 필요합니다\n관리자에게 문의하세요"
    }

    rDocID = _documentID;
    rSeq = _sequence;
    rTag = _approvalTag;
    rUserID = _userID;
    rDeptID = _deptID;

    Proxy.IsAccess(_documentID, _sequence, _approvalTag, _userID, _deptID);
}
// viewing 체크
Proxy.IsAccess = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProxy.asmx/GetViewing";

    ParamArray = {
          'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
        , 'targetUserID': _userID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Proxy.SuccessIsAccess");

    AjaxService(url, data, obj);
}
Proxy.SuccessIsAccess = function (data, status) {
    if (data.d.toUpperCase() == "OK") {
        Proxy.GoPage(rDocID, rSeq, rTag, rUserID, rDeptID);
    }
    else {
        alert("[" + data.d + "] " + R_MSG_Processing); return; //~님이 수정 또는 결재중입니다.
    }
}
// 페이지 이동
Proxy.GoPage = function (_documentID, _sequence, _approvalTag, _userID, _deptID) {
    if (_documentID == "0" || _sequence == "0" || _approvalTag == "" || _userID == "0" || _deptID == "0") {
        alert(R_MSG_ListApprove1); return; //"전달 파라미터 확인이 필요합니다\n관리자에게 문의하세요"
    }
    var isProxy = "N";
    if (ProxyUserID == "0") isProxy = "Y";
    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'listID': 'ListProxy'
        , 'folderStatusID': FolderStatusID
        , 'targetPage': TargetPage
        , 'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
        , 'targetUserID': _userID
        , 'targetDeptID': _deptID
        , 'proxyUserID': _userID
        , 'isProxy': isProxy
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/Approval/Process/Basic/Approve.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황 보기
***************************************************/
Proxy.State = function (_documentID, _accountTag) {

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
Proxy.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "attach": Proxy.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;

Proxy.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProxy.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Proxy.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Proxy.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Proxy.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");
    Proxy.GetAttachFileListMenu(childDocID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Proxy.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, Proxy.DownLoad, "EXT");
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
Proxy.DownLoad = function (_e) {
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
Proxy.TotalDownLoad = function (_e) {
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
Proxy.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출)
***************************************************/
Proxy.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefault.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Proxy.SuccessSetDefault");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출 후)
***************************************************/
Proxy.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
Proxy.Checked = function () {

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
Proxy.CheckedValue = function () {
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
Proxy.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Process'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Proxy.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Proxy.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
Proxy.GoPageLoad = function () {

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
    location = "ListProxy.aspx?p=" + GetEncryptValue;
}
