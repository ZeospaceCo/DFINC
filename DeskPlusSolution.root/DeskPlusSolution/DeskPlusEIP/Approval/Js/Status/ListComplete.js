Complete = {}



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
            //Complete.LoadData("");
            Complete.GoPageLoad();
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
            Complete.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Complete.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            Complete.Search();
        });

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            Complete.Admin();
        });

        //초기 메뉴로 설정
        $('#cbtnDefault').click(function (e) {
            e.preventDefault();
            Complete.SetDefault();
        });

        //전달
        $('#cbtnForward').click(function (e) {
            e.preventDefault();
            Complete.Forward();
        });

        //이관
        $('#cbtnTransfer').click(function (e) {
            e.preventDefault();
            Complete.Transfer();
        });

        //완결문서 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Complete.Del();
        });

        //완결문서 일괄 삭제
        $('#cbtnDelBatch').click(function (e) {
            e.preventDefault();
            Complete.DelAll();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Complete.SavePageSize();
            Complete.Search();
        });

        Complete.Init();

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
Complete.Init = function () {

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

    Complete.LoadData("");
    __addEventListener(document, "click", Complete.click);

    $("#zeoModalContact").draggable({ disabled: false });

    //$(document).click(function (e) { e.preventDefault(); Complete.subject_click(e); });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
Complete.RegDate = function () {
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
Complete.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        Complete.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        Complete.GetCount(); //검색 총 카운트 가져오기
        Complete.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                Complete.LoadData("search");
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
Complete.SettingSearch = function () {

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
Complete.Search = function () {

    var response = Complete.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //Complete.LoadData("search");
    Complete.GoPageLoad();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
Complete.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListComplete.asmx/GetPageList";

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

    var obj = eval("Complete.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
Complete.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/JTListComplete.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    Complete.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Complete.Resource = function () {

    $("#r_formName")[0].innerHTML = R_FormName;
    $("#r_approvalState")[0].innerHTML = R_Status;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attach")[0].innerHTML = R_AttachName;
    $("#r_user")[0].innerHTML = R_Name;
    $("#r_writeTime")[0].innerHTML = R_WriteTime;
    $("#r_completeDate")[0].innerHTML = R_CompleteDate;

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
Complete.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListComplete.asmx/GetListCount";
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
    var obj = eval("Complete.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Complete.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 보기
***************************************************/
Complete.View = function (_documentID, groupID) {

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
        'targetPage': TargetPage
    };

    CallEncrypt(CallGetParam());

    //location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;

    if (groupID < 800000) {
        url = "/DeskPlusEIP/Approval/Common/Popup33/ViewTot.aspx?p=" + GetEncryptValue;
        if ($.browser.msie) {
            PopupWinOpen(url, "", "OpenUrl", "1000", "745", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
        else {
            PopupWinOpen(url, "", "OpenUrl", "1000", "700", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
    }
    else {
        location.href = "/DeskPlusEIP/Approval/Folder/Common/View.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
Complete.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    Complete.LoadData("search");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-31
작성목적 : 결재상황 보기
***************************************************/
Complete.State = function (_documentID, _accountTag) {

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
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
Complete.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Complete.ShowAttachContextMenu(e); break; //첨부파일 클릭
        //        case "subject": Complete.ShowSubjectContextMenu(e); break; // 제목 클릭 
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 컨텍스트 메뉴 생성
***************************************************/
Complete.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");
    Complete.GetAttachFileListMenu(childDocID);

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
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Complete.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            var oItem = oContextMenuCtrl.addItem(attachFileName, Complete.DownLoad, "EXT");
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
Complete.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListComplete.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Complete.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Complete.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
Complete.DownLoad = function (_e) {
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
Complete.TotalDownLoad = function (_e) {
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
Complete.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출)
***************************************************/
Complete.SetDefault = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefault.asmx/SetDefaultMenu";

    ParamArray = {
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Complete.SuccessSetDefault");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-07-02
작성목적 : 초기 메뉴로 설정(웹서비스 호출 후)
***************************************************/
Complete.SuccessSetDefault = function (data, status) {
    alert(R_MSG_SUCCESS);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 전달하기
**********************************************************************************************/
Complete.Forward = function () {

    //키|구분자,키|구분자
    var selValue = Complete.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'docID': selValue
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Dept/ForwardMemo.aspx?p=" + GetEncryptValue;
        PopupWin(800, 500, url, "-500", "", true);
    }
    else {
        alert(R_MSG_SelPassDoc); //전달할 결재 문서를 선택해 주세요
    }

}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 이관하기
**********************************************************************************************/
Complete.Transfer = function () {

    //키|구분자,키|구분자
    var selValue = Complete.CheckedValue();

    if (selValue != "") {

        ParamArray = {
            'gubun': '2',
            'callback': 'GetTransferUser'
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
        PopupWin(400, 507, url, "-500", "", true);
    }
    else {
        alert(R_MSG_SelTransferDoc); //이관할 문서를 선택해 주세요
    }

}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 이관 처리하기
**********************************************************************************************/
function GetTransferUser(response) {

    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");

        var userList = responseStr[0];
        var docList = Complete.CheckedValue();

        var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/List.asmx/AddPassed";

        ParamArray = {
            'docID': docList,
            'recvUser': userList
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Complete.SuccessGetTransferUser");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이관 처리하기
**********************************************************************************************/
Complete.SuccessGetTransferUser = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_TransferComplete); //문서 복사본 이관 완료
    }
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 완결문서 삭제(웹서비스 호출)
***************************************************/
Complete.Del = function () {

    if (Complete.CheckedValue() == "") {
        alert(R_MSG_SelDelDoc); //삭제할 문서를 선택해 주세요
        return;
    }

    if (confirm(R_MSG_DelDoc)) {    //선택한 문서를 목록에서 삭제 하시겠습니까?
        var url = "/DeskPlusEIP/Approval/Ws/Status/ListComplete.asmx/Del";

        ParamArray = {
            'documentID': Complete.CheckedValue()
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Complete.SuccessDel");

        AjaxService(url, data, obj);
    }
    else {
        return;
    }

}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 완결문서 삭제(삭제후)
**********************************************************************************************/
Complete.SuccessDel = function (data, status) {

    CurrentPage = 1;
    //Complete.LoadData("search");   
    Complete.GoPageLoad();
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 완결문서 일괄 삭제(웹서비스 호출)
***************************************************/
Complete.DelAll = function () {

    if (confirm(R_MSG_DelAll)) {    // 완결문서를 목록에서 모두 삭제 하시겠습니까?
        var url = "/DeskPlusEIP/Approval/Ws/Status/ListComplete.asmx/DelAll";

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Complete.SuccessDelAll");

        AjaxService(url, data, obj);
    }
    else {
        return;
    }

}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-12
작성목적 : 완결문서 일괄 삭제(삭제후)
**********************************************************************************************/
Complete.SuccessDelAll = function (data, status) {

    //Complete.LoadData("search");
    CurrentPage = 1;
    Complete.GoPageLoad();
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
Complete.Checked = function () {

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
Complete.CheckedValue = function () {
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
Complete.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': FolderStatusID,
        'pageSize': $("#ddlListCount").val(),
        'types': 'Process'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Complete.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 결재상황조회 리스트 페이지 사이즈 저장
***************************************************/
Complete.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}



/**********************************************************************************************
중요표시 추가 및 삭제하기
**********************************************************************************************/
Complete.ModImpUser = function (_id, _arg) {
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
    var obj = eval("Complete.SuccessModImpUser");

    AjaxService(url, data, obj);
}
Complete.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
Complete.GoPageLoad = function () {

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
    location = "ListComplete.aspx?p=" + GetEncryptValue;
}
