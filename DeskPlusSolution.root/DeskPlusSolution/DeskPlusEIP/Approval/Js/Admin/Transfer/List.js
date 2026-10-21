List = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            List.SavePageSize();
            List.Search();
        });

        //문서함카테고리
        $("#ddlFolder").change(function (e) {
            e.preventDefault();
            CurrentPage = 1;
            List.SelFolder(false);
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) { e.preventDefault(); List.RegDate(); });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) { e.preventDefault(); List.Search(); }
        });

        //검색
        $('#cbtnSearch').click(function (e) { e.preventDefault(); List.Search(); });

        //변경전부서
        $('#cbtnBeforeDept').click(function (e) { e.preventDefault(); List.GetDept(true); });

        //변경후부서
        $('#cbtnAfterDept').click(function (e) { e.preventDefault(); List.GetDept(false); });

        //변경전삭제부서
        $('#cbtnBeforeDeptGarbage').click(function (e) { e.preventDefault(); List.GetGarbage(); });

        //변경전사용자
        $('#cbtnBeforeUser').click(function (e) { e.preventDefault(); List.GetUser(true); });

        //변경후사용자
        $('#cbtnAfterUser').click(function (e) { e.preventDefault(); List.GetUser(false); });

        //선택이관
        $('#cbtnTransfer').click(function (e) { e.preventDefault(); List.SetTransfer(false, false); });

        //전체이관
        $('#cbtnTransferAll').click(function (e) { e.preventDefault(); List.SetTransfer(true, false); });

        //선택복사
        $('#cbtnCopy').click(function (e) { e.preventDefault(); List.SetTransfer(false, true); });

        //전체복사
        $('#cbtnCopyAll').click(function (e) { e.preventDefault(); List.SetTransfer(true, true); });

        List.Init();

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 
작성일자 :
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

    List.SelFolder(true);  //버튼제어(조직,사용자)

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

    List.LoadData("");
    __addEventListener(document, "click", List.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : 
작성일자 :
작성목적 : 변경전/후,부서/사용자 정보가져오기
***************************************************/
var isBefore = null;        //sendInfo, receiveInfo

//  삭제된 부서 정보가져오기 (이관전 삭제된 부서정보)
List.GetGarbage = function () {
    isBefore = true;

    ParamArray = { 'callback': 'List.SetDept' };
    CallEncrypt(CallGetParam());

    var url = "GetGarbage.aspx?p=" + GetEncryptValue;
    PopupWin(500, 327, url, "-500", "", true);
}

//  변경전/후,부서 정보가져오기
List.GetDept = function (_isBefore) {

    if (_isBefore == null) return;
    isBefore = _isBefore;

    ParamArray = { 'callback': 'List.SetDept', 'isCheckBox': '0' };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDept.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}
List.SetDept = function (_ids, _values) { //alert(unescape(_ids) + "," + unescape(_values));

    var id = unescape(_ids);        //  deptID
    var val = unescape(_values).split("|")[1];    // deptID | deptName | parentDeptID | parentDeptName

    if (isBefore) {
        BeforeTargetID = id;
        $("#sendInfo").html(val)
    } else {
        AfterTargetID = id;
        $("#receiveInfo").html(val)
    }

    List.LoadData("search");    //해당정보의 리스트가져오기
}
//  변경전/후,사용자 정보가져오기
List.GetUser = function (_isBefore) {

    if (_isBefore == null) return;
    isBefore = _isBefore;

    ParamArray = { 'gubun': '2', 'callback': 'List.SetUser', 'isCheckBox': '0' };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 517, url, "-500", "", true);
}
List.SetUser = function (_val) {

    var val = unescape(_val).split("");    // userID + "" + userName + "" + deptID + "" + types;
    if (val[3].indexOf('M') < 0) {
        alert(R_Msg_ErrSelUser); return;
    }

    if (isBefore) {
        BeforeTargetID = val[0].replace(",", "");
        $("#sendInfo").html(val[1].replace(",", ""));
    } else {
        AfterTargetID = val[0].replace(",", "");
        $("#receiveInfo").html(val[1].replace(",", ""));
    }

    List.LoadData("search");    //해당정보의 리스트가져오기
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
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
작성일자 : 
작성목적 : 문서함선택
***************************************************/
List.SelFolder = function (_isinit) {

    var viewdept = "", viewuser = "";

    if ($("#ddlFolder").val().toUpperCase() == "DEPT") {
        viewdept = ""; viewuser = "none";   //부서별문서함
    } else {
        viewdept = "none"; viewuser = "";   //그외
    }

    $(".dept").css("display", viewdept);
    $(".user").css("display", viewuser);

    if (!_isinit) {
        //초기화
        TotalItemCount = 0, BeforeTargetID = 0, AfterTargetID = 0;

        $("#sendInfo").html("");
        $("#receiveInfo").html("");
    }

    List.LoadData("search");    //해당정보의 리스트가져오기
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

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
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
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
작성일자 : 
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "period") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
        
            alert(R_ApplyRegDate); return false;
        } else {

            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate); return false;
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
작성일자 : 
작성목적 : 검색
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    List.LoadData("search");
}

//웹서비스 URL정보
List.GetUrl = function (_isCnt, _isGet) {

    var url = "";
    if (_isGet) {
        url = (_isCnt) ? "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/GetListCount" : "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/GetPageList";
    } else {
        url = "/DeskPlusEIP/Approval/Ws/Admin/Transfer/List.asmx/SetTransfer"
    }

    switch ($("#ddlFolder").val().toUpperCase()) {
        case "DEPT":        //부서별문서함
            url += "Dept";
            break;
        case "PRIVATE":     //개인문서함
            url += "Private";
            break;
        case "APPROVE":     //결재문서함
            url += "Approve";
            break;
        case "ARBITRARY":   //전결문서함
            url += "Arbitrary";
            break;
        case "CARBON":      //참조문서함
            url += "Carbon";
            break;
        case "FORWARD":     //전달문서함
            url += "Forward";
            break;
        case "CIRRECEIVE":  //받은회람문서함
            url += "CirReceive";
            break;
        case "CIRSEND":     //보낸회람문서함
            url += "CirSend";
            break;
    }

    return url;
}

/**************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
***************************************************/
List.GetPageList = function () {

    var url = List.GetUrl(false, true);
    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'beforeTargetID': BeforeTargetID
    };
    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**************************************************/
List.SuccessPageList = function (data, status) {

    var url = "";
    if ($("#ddlFolder").val().toUpperCase() == "CIRRECEIVE" || $("#ddlFolder").val().toUpperCase() == "CIRSEND") {
        url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTListCir.htm?seq=" + Guids();
    } else {
        url = "/DeskPlusEIP/Approval/Jt/Admin/Transfer/JTList.htm?seq=" + Guids();
    }

    $('#placeholder').setTemplateURL(url, null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**************************************************/
List.Resource = function () {

    //$("#r_number")[0].innerHTML = R_Number;
    $("#r_docnumber")[0].innerHTML = R_DocNumber;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attachName")[0].innerHTML = R_AttachName;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_writetime")[0].innerHTML = R_WriteTime;
    $("#r_completetime")[0].innerHTML = R_CompleteDate;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 웹서비스 게시판 카운트  호출
**************************************************/
List.GetCount = function () {
    var url = List.GetUrl(true, true);
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'beforeTargetID': BeforeTargetID
    };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 , 게시물 카운트 이벤트
***************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 정렬
***************************************************/
List.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    List.LoadData("search");
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
List.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": List.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;
List.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/List.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("List.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 첨부파일 리스트 가져오기
***************************************************/
List.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
List.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");
    List.GetAttachFileListMenu(childDocID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, List.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, List.DownLoad, "EXT");
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
List.DownLoad = function (_e) {
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
List.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        document.getElementById("frmTotalDownLoad").submit();
    }
}

/**************************************************
작성자   : 
작성일자 :
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Admin_Transfer",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 :
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : 
작성일자 : 
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
작성일자 : 
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 
작성일자 :
작성목적 : 선택이관,전체이관,선택복사,전체복사
***************************************************/
List.SetTransfer = function (_isAll, _isCopy) {

    if (_isAll == null || _isCopy == null) return;

    if (BeforeTargetID == 0 || AfterTargetID == 0) {
        alert(R_TransferMsgTargetInfo); return;
    }

    var ids = List.CheckedValue();
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
        var url = List.GetUrl(false, false);

        ParamArray = {
            'ids': ids,
            'isCopy': isCopy,
            'isAll': isAll,
            'sendID': BeforeTargetID,
            'receiveID': AfterTargetID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessSetTransfer");

        AjaxService(url, data, obj);
    }
}
List.SuccessSetTransfer = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        List.LoadData("search");
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재(회람) 보기
***************************************************/
List.ViewCir = function (p_cirDocumentID) {

    
    ParamArray = { 'cirDocumentID': p_cirDocumentID};
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Common/Popup/ViewCir.aspx?p=" + GetEncryptValue;
    window.open(url, "View" + p_cirDocumentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    /*
    ParamArray = {
        'cirDocumentID': p_cirDocumentID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'targetPage': TargetPage,
        'beforeTargetID': BeforeTargetID,
        'beforeTargetName': $("#sendInfo").html(),
        'afterTargetID': AfterTargetID,
        'afterTargetIDName': $("#receiveInfo").html(),
        'selFolder': $("#ddlFolder").val()
    };
    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Admin/Transfer/ViewCir.aspx?p=" + GetEncryptValue;
    */
    
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 결재 보기
***************************************************/
List.View = function (p_companyID, p_docID) {

    if (p_companyID == "" || p_docID == "" || p_companyID == "0" || p_docID == "0") return;

    ParamArray = { 'companyID': p_companyID, 'documentID': p_docID };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
    window.open(url, "View" + p_docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    /*
    ParamArray = {
    'documentID': p_docID,
    'currentPage': CurrentPage,
    'searchTerm': SearchTerm,
    'searchTermValue': SearchTermValue,
    'searchField': SearchField,
    'searchValue': SearchValue,
    'order': Order,
    'targetPage': TargetPage,
    'beforeTargetID': BeforeTargetID,
    'beforeTargetName': $("#sendInfo").html(),
    'afterTargetID': AfterTargetID,
    'afterTargetIDName': $("#receiveInfo").html(),
    'selFolder': $("#ddlFolder").val()
    };
    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/Admin/Transfer/View.aspx?p=" + GetEncryptValue;
    */
}
