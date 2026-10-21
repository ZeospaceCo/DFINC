Confirm = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            Confirm.LoadData("");
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
            Confirm.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Confirm.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            Confirm.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            Confirm.Admin();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Confirm.SavePageSize();
            Confirm.Search();
        });

        // 좌측 트리 카운트  총 카운트 변경
        try {
            // 후열 문서 카운트
            var confirmTotCnt = parseInt(TotalConfirmCount);
            var afterwardNode = parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.FindNodeById("Confirm");

            if (afterwardNode) {//alert(afterwardTotCnt)
                var nodeText = (confirmTotCnt > 0) ? afterwardNode.Value + "(" + confirmTotCnt + ")" : afterwardNode.Value;
                afterwardNode.SetProperty('Text', nodeText);
            }

            parent.frmLeft.UCFolderStatusTreeScript1_tvFolderStatus.Render();

        } catch (e) { }

        Confirm.Init();

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
Confirm.Init = function () {

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

    Confirm.LoadData("");
    __addEventListener(document, "click", Confirm.click);

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
Confirm.RegDate = function () {
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
Confirm.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Confirm.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Confirm.GetCount(); //검색 총 카운트 가져오기
        Confirm.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Confirm.LoadData("search");
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
Confirm.SettingSearch = function () {

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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
Confirm.Search = function () {

    var response = Confirm.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    Confirm.LoadData("search");
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 후열 문서 리스트 호출
**********************************************************************************************/
Confirm.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListConfirm.asmx/GetPageList";

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

    var obj = eval("Confirm.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 수신반려 문서 리스트 호출
**********************************************************************************************/
Confirm.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListConfirm.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Confirm.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Confirm.Resource = function () {

    $("#r_formName")[0].innerHTML = R_FormName;
    $("#r_approvalState")[0].innerHTML = R_Status;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attach")[0].innerHTML = R_AttachName;
    $("#r_user")[0].innerHTML = R_Name;
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
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 웹서비스 수신 문서 카운트  호출
**********************************************************************************************/
Confirm.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListConfirm.asmx/GetListCount";
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
    var obj = eval("Confirm.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Confirm.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 정렬
**********************************************************************************************/
Confirm.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    Confirm.LoadData("search");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재페이지 이동
***************************************************/
var rDocID, rSeq, rTag, rUserID, rAccTag, rStat;
Confirm.Approval = function (_documentID, _sequence, _approvalTag, _userID, _accountTag, _approvalState) {

    //alert(_documentID + "." + _sequence + "." + _approvalTag + "." + _userID + "." + _accountTag + "." + _approvalState)

    if (_documentID == "0" || _approvalTag == "" || _userID == "0" || _accountTag == "" || _approvalState == "") {
        alert(R_MSG_ListApprove1); return; //"전달 파라미터 확인이 필요합니다\n관리자에게 문의하세요"
    }

    if (_approvalState == "C") {
        Confirm.GoPage(_documentID, _accountTag, _approvalTag, _sequence, _approvalState);
    } else {

        // 진행중인 문서는 열람여부 확인
        rDocID = _documentID;
        rSeq = _sequence;
        rTag = _approvalTag;
        rUserID = _userID;
        rAccTag = _accountTag;
        rStat = _approvalState;

        Confirm.IsAccess(_documentID, _sequence, _approvalTag, _userID);
    }
}
// viewing 체크
Confirm.IsAccess = function (_documentID, _sequence, _approvalTag, _userID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListApprove.asmx/GetViewing";

    ParamArray = {
        'documentID': _documentID
        , 'sequence': _sequence
        , 'approvalTag': _approvalTag
        , 'targetUserID': _userID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Confirm.SuccessIsAccess");

    AjaxService(url, data, obj);
}
Confirm.SuccessIsAccess = function (data, status) {
    if (data.d.toUpperCase() == "OK") {
        Confirm.GoPage(rDocID, rAccTag, rTag, rSeq, rStat);
    }
    else {
        alert("[" + data.d + "] " + R_MSG_Processing); return; //~님이 수정 또는 결재중입니다.
    }
}
/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : 후열-문서확인
***************************************************/
Confirm.GoPage = function (_documentID, _accountTag, _approvalTag, _sequence, _approvalState) { //alert(_documentID + "," + _accountTag + "," + _approvalTag + "," + _sequence + "," + _approvalState);

    ParamArray = {
        'recordPerPage': RecordPerPage
        , 'currentPage': CurrentPage
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'order': Order
        , 'listID': 'ListConfirm'
        , 'folderStatusID': FolderStatusID
        , 'targetPage': TargetPage
        , 'documentID': _documentID
        , 'accountTag': _accountTag
        , 'approvalTag': _approvalTag
        , 'sequence': _sequence
        , 'approvalState': _approvalState
    };
    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Process/Confirm/Approve.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재상황 보기
***************************************************/
Confirm.State = function (_documentID, _accountTag, _appStat) {

    var iscomplete = (_appStat == "C") ? "Y" : "N";
    ParamArray = {
        'isComplete': iscomplete
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
Confirm.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Confirm.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

Confirm.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListConfirm.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Confirm.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Confirm.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Confirm.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");
    Confirm.GetAttachFileListMenu(childDocID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Confirm.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, Confirm.DownLoad, "EXT");
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
Confirm.DownLoad = function (_e) {
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
Confirm.TotalDownLoad = function (_e) {
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
작성자   : 
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
Confirm.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/**************************************************
작성자   : 
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
Confirm.Checked = function () {

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
Confirm.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Confirm.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Confirm'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Confirm.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Confirm.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}