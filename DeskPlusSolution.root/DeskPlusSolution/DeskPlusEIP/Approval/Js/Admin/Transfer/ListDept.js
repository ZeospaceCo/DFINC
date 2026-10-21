ListDept = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListDept.LoadData("", "send");
        }
    }
    var Pager2 = {
        pageIndexChanged: function (selectedPage) {
            tCurrentPage = selectedPage;
            ListDept.LoadData("", "target");
        }
    }

    $(document).ready(function () {

        // 이관할부서 - 페이저 초기화
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text",              //text, image 중 선택
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

        // 이관받을부서 - 페이저 초기화
        $("#pager2").pager2({
            pager: "#pager2",
            pagerMode: "text",              //text, image 중 선택
            totalItemCount: tTotalItemCount,
            recordPerPage: tRecordPerPage,
            currentPage: tCurrentPage,
            pageIndexChanged: Pager2.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });


        // 이관할부서 - 문서함카테고리
        $("#ddlFolder").change(function (e) {
            e.preventDefault();
            ListDept.Search();
            $("#ddlFolderTarget").val($("#ddlFolder").val());
            ListDept.LoadData("search", "target");
        });

        // 이관받을부서 - 문서함카테고리
        $("#ddlFolderTarget").change(function (e) { e.preventDefault(); ListDept.SearchTarget(); });

        //페이지 카운트
        $('#ddlListCount').change(function () { RecordPerPage = $('#ddlListCount').val(); ListDept.Search(); });
        $('#ddlListCountTarget').change(function () { tRecordPerPage = $('#ddlListCountTarget').val(); ListDept.SearchTarget(); });

        // 이관할부서 - 검색
        $("#ctbSearch").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); ListDept.Search(); } });
        $('#cbtnSearch').click(function (e) { e.preventDefault(); ListDept.Search(); });

        // 이관받을부서 - 검색
        $("#ctbSearchTarget").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); ListDept.SearchTarget(); } });
        $('#cbtnSearchTarget').click(function (e) { e.preventDefault(); ListDept.SearchTarget(); });


        // 이관할부서 - 부서선택
        $('#btnsend').click(function (e) { e.preventDefault(); ListDept.GetDept("send"); });
        // 이관받을부서 - 부서선택
        $('#btntarget').click(function (e) { e.preventDefault(); ListDept.GetDept("target"); });


        //[선택이관]
        $('#cbtnTransfer').click(function (e) { e.preventDefault(); ListDept.SetTransfer(false, false); });

        //[전체이관]
        $('#cbtnTransferAll').click(function (e) { e.preventDefault(); ListDept.SetTransfer(true, false); });

        //[선택복사]
        $('#cbtnCopy').click(function (e) { e.preventDefault(); ListDept.SetTransfer(false, true); });

        //[전체복사]
        $('#cbtnCopyAll').click(function (e) { e.preventDefault(); ListDept.SetTransfer(true, true); });

        ListDept.Init();

        //검색어입력
        $("#ctbSearch, #ctbSearchTarget").attr("placeholder", R_PlaceholderText);
    });
});

/**************************************************
초기화 하기
***************************************************/
ListDept.Init = function () {

    // 이관할부서
    TotalItemCount = 0, ReqID = 0, ReqName = "";        // 데이타초기화
    $("#sendinfo").html(R_TransferMsgDept);
    ListDept.LoadData("search", "send");    

    // 이관받을부서
    tTotalItemCount = 0, TargetID = 0, TargetName = ""; // 데이타초기화
    $("#targetinfo").html(R_TransferMsgDept);
    ListDept.LoadData("search", "target");
}

/***************************************************************************************************************************************************/
/**************************************************
리스트 초기화
***************************************************/
ListDept.LoadData = function (_gubun, _callid) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function (_callid) {
        ListDept.GetPageList(_callid);
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function (_callid) {
        ListDept.GetCount(_callid);     //검색 총 카운트 가져오기
        ListDept.GetPageList(_callid);  //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListDept.LoadData("search", _callid);
            }
        }
        var Pager2 = {
            pageIndexChanged: function (selectedPage) {
                tCurrentPage = selectedPage;
                ListDept.LoadData("search", _callid);
            }
        }

        // 이관할부서 - 페이저 초기화
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

        // 이관받을부서 - 페이저 초기화
        $("#pager2").pager2({
            pager: "#pager2",
            pagerMode: "text",              //text, image 중 선택
            totalItemCount: tTotalItemCount,
            recordPerPage: tRecordPerPage,
            currentPage: tCurrentPage,
            pageIndexChanged: Pager2.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
    var loadobj = (_callid == "send") ? $('#loading') : $('#loading2');
    $(loadobj).html(loadingPath);


    setTimeout(function () {
        if (_gubun == "search") {
            loadBoardSearchData(_callid);
        }
        else {
            loadBoardData(_callid);
        }
    }, 100);
}

/**************************************************
리스트 호출
***************************************************/
ListDept.GetPageList = function (_callid) {

    callid = _callid;       // send or target

    var totcnt      = (callid == "send") ? TotalItemCount : tTotalItemCount;
    var pagesize    = (callid == "send") ? RecordPerPage : tRecordPerPage;
    var pageno      = (callid == "send") ? CurrentPage : tCurrentPage;
    var srhfield    = (callid == "send") ? $("#ddlSearchField").val() : $("#ddlSearchFieldTarget").val();
    var srhval      = (callid == "send") ? $("#ctbSearch").val().securityText() : $("#ctbSearchTarget").val().securityText();
    var ord         = "";
    var srhid       = (callid == "send") ? ReqID : TargetID;

    var url = ListDept.GetUrl(false, true, callid);
    ParamArray = {
        'totalItemCount': totcnt,      //TotalItemCount
        'pageSize': pagesize,          //RecordPerPage
        'pageSizePageNo': pageno,      //CurrentPage
        'searchField': srhfield,       //SearchField
        'searchValue': srhval,         //SearchValue
        'order': ord,                  //Order
        'srhID': srhid                  //ReqID
    };
    var obj = eval("ListDept.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
ListDept.SuccessPageList = function (data, status) {

    var url = "";
    if (callid == "send") {

        //이관할부서 (send)
        if ($("#ddlFolder").val().toLowerCase().indexOf("cir") == 0) {
            url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTListCir.htm?seq=" + Guids();       //"CIRRECEIVE" / "CIRSEND"
        } else {
            url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTList.htm?seq=" + Guids();
        }

        $('#placeholder').setTemplateURL(url, null, { filter_data: false });
        $('#placeholder').processTemplate(data.d);

    } else {
        
        //이관받을부서 (target)
        if ($("#ddlFolderTarget").val().toLowerCase().indexOf("cir") == 0) {
            url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTListCir2.htm?seq=" + Guids();       //"CIRRECEIVE" / "CIRSEND"
        } else {
            url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTList2.htm?seq=" + Guids();
        }

        $('#tplaceholder').setTemplateURL(url, null, { filter_data: false });
        $('#tplaceholder').processTemplate(data.d);
    }

    $('#loading, #loading2').html("");

    ListDept.Resource();
}

/**************************************************
다국어 처리
**************************************************/
ListDept.Resource = function () {

    $(".r_subject").text(R_Subject);
    $(".r_name").text(R_Name);
    $(".r_completetime").text(R_CompleteDate);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
카운트  호출
**************************************************/
ListDept.GetCount = function (_callid) {

    callid = _callid;       // send or target

    var totcnt      = (callid == "send") ? TotalItemCount : tTotalItemCount;
    var srhfield    = (callid == "send") ? $("#ddlSearchField").val() : $("#ddlSearchFieldTarget").val();
    var srhval      = (callid == "send") ? $("#ctbSearch").val().securityText() : $("#ctbSearchTarget").val().securityText();
    var srhid       = (callid == "send") ? ReqID : TargetID;

    var url = ListDept.GetUrl(true, true, callid);
    ParamArray = {
        'totalItemCount': totcnt,
        'searchField': srhfield,
        'searchValue': srhval,
        'srhID': srhid
    };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDept.SuccessCount");

    AjaxService(url, data, obj);
}
ListDept.SuccessCount = function (data, status) {
    if (callid == "send") {
        //이관할부서
        TotalItemCount = data.d;
        var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
        $("#pageInfo").text(CurrentPage + " / " + pageCount);
    }
    else {
        //이관받을부서
        tTotalItemCount = data.d;
        var pageCount = $("#pager2").pager.refresh(tTotalItemCount, tRecordPerPage);
        $("#pageInfo2").text(tCurrentPage + " / " + pageCount);
    }    
}

/**************************************************
이관할부서 : 검색
***************************************************/
ListDept.Search = function () {

    // 부서선택확인
    if (ReqID == 0) { alert(R_TransferMsgDept); return; }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    CurrentPage = 1;
    ListDept.LoadData("search", "send");
}

/**************************************************
이관받을부서 : 검색
***************************************************/
ListDept.SearchTarget = function () {

    // 부서선택확인
    if (ReqID == 0) { alert(R_TransferMsgDept); return; }

    tSearchValue = $("#ctbSearchTarget").val().securityText();
    $("#ctbSearchTarget").val(tSearchValue);

    tCurrentPage = 1;
    ListDept.LoadData("search", "target");
}


/**************************************************
공통 : 웹서비스 URL정보
**************************************************/
ListDept.GetUrl = function (_isCnt, _isGet, _callid) {       //_isCnt(카운트호출), _isGet

    var url = "";
    if (_isGet) {
        //리스트 + 카운트가져오기
        url = (_isCnt) ? "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/GetListCount" : "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/GetPageList";
    } else {
        //이관처리
        url = "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/SetTransfer"
    }

    var ddlval = (_callid == "send") ? $("#ddlFolder").val().toUpperCase() : $("#ddlFolderTarget").val().toUpperCase();

    switch (ddlval) {
        case "DEPT":        //부서별문서함
            url += "Dept";
            break;
        case "CARBONDEPT":      //참조문서함(부서)
            url += "CarbonDept";
            break;
//        case "PRIVATE":     //개인문서함
//            url += "Private";
//            break;
//        case "APPROVE":     //결재문서함
//            url += "Approve";
//            break;
//        case "ARBITRARY":   //전결문서함
//            url += "Arbitrary";
//            break;
//        case "CARBON":      //참조문서함(개인)
//            url += "Carbon";
//            break;
//        case "FORWARD":     //전달문서함
//            url += "Forward";
//            break;
//        case "CIRRECEIVE":  //받은회람문서함
//            url += "CirReceive";
//            break;
//        case "CIRSEND":     //보낸회람문서함
//            url += "CirSend";
//            break;
    }
    return url;
}

/**************************************************
선택이관,전체이관,선택복사,전체복사
***************************************************/
ListDept.SetTransfer = function (_isAll, _isCopy) {

    if (_isAll == null || _isCopy == null) return;

    //이관할부서 및 이관받을부서 선택여부
    if (ReqID == 0 || TargetID == 0) {
        alert(R_TransferMsgTargetInfo); return;
    }

    //이관할부서 및 이관받을부서 선택여부
    if (ReqID == TargetID) {
        alert(R_TransferMsgChk); return;
    }

    //이관항목 선택여부
    var ids = CheckedValue();
    if (ids == "" && !_isAll) {
        alert(R_MSG_SelItem); return;
    }

    var msg = "";
    if (_isAll) {
        msg = (_isCopy) ? R_TransferMsgCopyAll : R_TransferMsgSubmitAll;
    } else {
        msg = (_isCopy) ? R_TransferMsgCopy : R_TransferMsgSubmit;
    }

    if (window.confirm(msg)) {

        var isAll = (_isAll) ? "1" : "0";
        var isCopy = (_isCopy) ? "1" : "0";
        var url = ListDept.GetUrl(false, false, "send");

        ParamArray = {
            'ids': ids,
            'isCopy': isCopy,
            'isAll': isAll,
            'sendID': ReqID,
            'receiveID': TargetID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListDept.SuccessSetTransfer");

        AjaxService(url, data, obj);
    }
}
ListDept.SuccessSetTransfer = function (data, status) {
    if (data.d) {
        alert(R_MSG_SUCCESS);
        ListDept.LoadData("search", "send");
        ListDept.LoadData("search", "target");
    }
}

/**************************************************
부서선택 & 사용자선택
***************************************************/
//  변경전/후,부서 정보가져오기
ListDept.GetDept = function (_callid) {

    if (_callid == null) return;
    callid = _callid;

    ParamArray = { 'callback': 'ListDept.SetDept', 'isCheckBox': '0', 'isactive' : '0', 'issub':'0' };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}
ListDept.SetDept = function (_ids, _values) { //alert(unescape(_ids) + "," + unescape(_values));

    var id = unescape(_ids);                    // deptID
    var val = unescape(_values).split("|")[1];  // deptID | deptName | parentDeptID | parentDeptName

    if (callid == "send") {
        ReqID = id;
        ReqName = val;
        $("#sendinfo").html(ReqName);
    } else {
        TargetID = id;
        TargetName = val;
        $("#targetinfo").html(TargetName);
    }
    ListDept.LoadData("search", callid);    //해당정보의 리스트가져오기
}



/**************************************************
전체 체크박스
***************************************************/
Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
선택된 값 가져오기
***************************************************/
CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
결재(회람) 보기
***************************************************/
ViewCir = function (_cirdocID) {

    ParamArray = { 'cirDocumentID': _cirdocID };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Common/Popup/ViewCir.aspx?p=" + GetEncryptValue;
    window.open(url, "View" + _cirdocID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
결재 보기
***************************************************/
View = function (_companyID, _docID) {

    if (_companyID == "" || _docID == "" || _companyID == "0" || _docID == "0") return;

    ParamArray = { 'companyID': _companyID, 'documentID': _docID };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
    window.open(url, "View" + _docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}