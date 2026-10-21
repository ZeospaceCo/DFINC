Reject = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            //Reject.LoadData("");
            Reject.GoPageLoad();
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
        $("#ddlSearchTerm").change(function (e) { e.preventDefault(); Reject.RegDate(); });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Reject.Search(); } });

        //검색
        $('#cbtnSearch').click(function (e) { e.preventDefault(); Reject.Search(); });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) { e.preventDefault(); Reject.Admin(); });

        //완결문서 삭제
        $('#cbtnDel').click(function (e) { e.preventDefault(); Reject.Del(); });

        //완결문서 일괄 삭제
        $('#cbtnDelBatch').click(function (e) { e.preventDefault(); Reject.DelAll(); });


        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) { e.preventDefault(); Reject.SetDefault(); });

        //페이지 카운트
        $('#ddlListCount').change(function () { RecordPerPage = $('#ddlListCount').val(); Reject.SavePageSize(); Reject.Search(); });

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

        Reject.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
초기화 하기
***************************************************/
Reject.Init = function () {

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

    Reject.LoadData("");
    __addEventListener(document, "click", Reject.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
등록일자 활성화 여부
***************************************************/
Reject.RegDate = function () {
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
리스트 초기화
***************************************************/
Reject.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Reject.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Reject.GetCount(); //검색 총 카운트 가져오기
        Reject.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Reject.LoadData("search");
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

        // 좌측 트리 카운트  총 카운트 변경(삭제후 등)
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
}


/**************************************************
검색 필드 설정하기
***************************************************/
Reject.SettingSearch = function () {

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
검색
***************************************************/
Reject.Search = function () {

    var response = Reject.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //Reject.LoadData("search");
    Reject.GoPageLoad();
}

/**********************************************************************************************
결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
Reject.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReject.asmx/GetPageList";

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

    var obj = eval("Reject.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Reject.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListReject.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Reject.Resource();
}

/**********************************************************************************************
리스트 메뉴 다국어 처리
**********************************************************************************************/
Reject.Resource = function () {

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
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}

/**********************************************************************************************
웹서비스 결재상황조회 카운트  호출
**********************************************************************************************/
Reject.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReject.asmx/GetListCount";
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
    var obj = eval("Reject.SuccessCount");

    AjaxService(url, data, obj);
}
Reject.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
게시물 보기
***************************************************/
Reject.View = function (_documentID) {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'documentID': _documentID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID,
        'targetPage': TargetPage,
        'isreject': 'Y'
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
정렬
**********************************************************************************************/
Reject.Sort = function (col) {
    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) { colValue = col + " desc"; }

    Order = colValue;
    CurrentPage = 1;

    Reject.LoadData("search");
}

/**************************************************
결재상황 보기
***************************************************/
Reject.State = function (_documentID, _accountTag) {    //alert(_documentID + "," + _accountTag);

    ParamArray = {
        'isComplete': 'Y'
        , 'documentID': _documentID
        , 'accountTag': _accountTag
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Status/Common/GetDocFlow.aspx?p=" + GetEncryptValue;
    PopupWin(850, 400, url, "-500", "", true);
}

/**************************************************
클릭 이벤트 분기
***************************************************/
Reject.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Reject.ShowAttachContextMenu(e); break; //첨부파일 클릭
        case "subject": Reject.ShowSubjectContextMenu(e); break; // 제목 클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
/**************************************************
작성자   : 이문호
작성일자 : 2012-06-11
작성목적 : 제목 클릭시 컨텍스트 메뉴
***************************************************/
Reject.ShowSubjectContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var docID = el.getAttribute("docID");
    var accountTag = el.getAttribute("accountTag");
    var groupID = el.getAttribute("groupID");
    //alert(docID + "," + accountTag + "," + groupID)

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    // 문서 보기
    var oItem = oContextMenuCtrl.addItem(R_ViewDoc, Reject.View_onclick, ImagePath + "/Icon/icon_search.gif");
    oItem.txt.setAttribute("arg", docID, 0);

    //수정후재상신
    oItem = oContextMenuCtrl.addItem(R_EditNReport, Reject.Edit_onClick, ImagePath + "/Icon/ico_tree_listEdit.gif");
    oItem.txt.setAttribute("docID", docID, 0);
    oItem.txt.setAttribute("accountTag", accountTag, 0);
    oItem.txt.setAttribute("groupID", groupID, 0);

    oContextMenuCtrl.show(e);
}

/**************************************************
컨텍스트메뉴 클릭시(문서보기)
***************************************************/
Reject.View_onclick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        Reject.View(arg);
    }
}

/**************************************************
컨텍스트메뉴 클릭시(수정후재상신)
***************************************************/
var eDocID, eState, eAccountTag, eGroupID;
Reject.Edit_onClick = function (e) {
    var evt = new __event(e);
    if (evt.target) {
        eDocID = evt.target.getAttribute("docID");
        eState = evt.target.getAttribute("state");
        eAccountTag = evt.target.getAttribute("accountTag");
        eGroupID = evt.target.getAttribute("groupID");

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
            , 'isreject': 'Y'         /*반려문서함 호출여부*/
        };

        CallEncrypt(CallGetParam());
        parent.location.href = "/DeskPlusEIP/Approval/FormFlow/Default.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
첨부파일 컨텍스트 메뉴 생성
***************************************************/
Reject.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");

    Reject.GetAttachFileListMenu(childDocID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Reject.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            var oItem = oContextMenuCtrl.addItem(attachFileName, Reject.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**********************************************************************************************
첨부파일 리스트 조회(웹서비스)
**********************************************************************************************/
Reject.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListReject.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Reject.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
첨부파일 리스트 가져오기
**********************************************************************************************/
Reject.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
첨부파일 열기
***************************************************/
Reject.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;     
        window.open(arg);
    }
}

/**************************************************
첨부파일 전체 다운로드
***************************************************/
Reject.TotalDownLoad = function (_e) {
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
관리자 도구
***************************************************/
Reject.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**************************************************
초기 메뉴로 설정(웹서비스 호출)
***************************************************/
Reject.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefault.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Reject.SuccessSetDefault");

    AjaxService(url, data, obj);
}
Reject.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}

/**************************************************
전체 체크박스
***************************************************/
Reject.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
체크박스 선택된 값 가져오기
***************************************************/
Reject.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Reject.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Process'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Reject.SuccessSavePageSize");

    AjaxService(url, data, obj);
}
Reject.SuccessSavePageSize = function (data, status) {
    if (data.d == true) { }
}


/**************************************************
완결문서 삭제(웹서비스 호출)
***************************************************/
Reject.Del = function () {

    if (Reject.CheckedValue() == "") { alert(R_MSG_SelDelDoc); return; }

    if (confirm(R_MSG_DelDoc)) {    //선택한 문서를 목록에서 삭제 하시겠습니까?
        var url = "/DeskPlusEIP/Approval/Ws/Status/ListReject.asmx/Del";

        ParamArray = { 'documentID': Reject.CheckedValue() };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Reject.SuccessDel");

        AjaxService(url, data, obj);
    }
}
Reject.SuccessDel = function (data, status) {
    CurrentPage = 1; //1페이지로 이동처리  
    Reject.LoadData("search");
}

/**************************************************
완결문서 일괄 삭제(웹서비스 호출)
***************************************************/
Reject.DelAll = function () {

    if (confirm(R_MSG_DelAll)) {    // 완결문서를 목록에서 모두 삭제 하시겠습니까?
        var url = "/DeskPlusEIP/Approval/Ws/Status/ListReject.asmx/DelAll";

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Reject.SuccessDel");

        AjaxService(url, data, obj);
    }
}


/**********************************************************************************************
중요표시 추가 및 삭제하기
**********************************************************************************************/
Reject.ModImpUser = function (_id, _arg) {
    if (_id == "") return;

    var url = "";

    if ($(_arg).attr("checked") == "checked") {
        $("." + _id).attr("checked", true);
        $("." + _id).next().addClass("on");
        //$(_arg).next().addClass("on");

        url = "/DeskPlusEIP/Approval/Ws/Folder/Impo/List.asmx/AddImpUser";
    }
    else {
        $("." + _id).attr("checked", false);
        $("." + _id).next().removeClass("on");
        //$(_arg).next().removeClass("on");

        url = "/DeskPlusEIP/Approval/Ws/Folder/Impo/List.asmx/DelImpUser";
    }

    ParamArray = { 'ids': _id.split("_")[1] };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Reject.SuccessModImpUser");

    AjaxService(url, data, obj);
}
Reject.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
Reject.GoPageLoad = function () {

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
    location = "ListReject.aspx?p=" + GetEncryptValue;
}
