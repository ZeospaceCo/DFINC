Search = {}

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            Search.LoadData();
        }
    }

    $(document).ready(function () {
        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#searchValue").val(SearchValue);
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


        //검색입력필드 엔터 이벤트 기술
        $("#cbtnSearch").click(function (e) { e.preventDefault(); Search.SearchDoc(); });
        //검색입력필드 엔터 이벤트 기술
        $("#searchValue").keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); Search.SearchDoc(); } });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            Search.SavePageSize();
            Search.SearchDoc();
        });

        Search.Init();
        Search.LoadData();
    });
});

Search.Init = function () {
    __addEventListener(document, "click", Search.click);

}

/*** 검색영역 start ***********************************************************************************************/


Search.SearchDoc = function () {

    var validator = new Validator($("#form1"));
    //validator.checkRequired("searchValue", "검색어를 입력해 주세요", true);
    validator.checkRequired("searchValue", R_InputSearch, true);
    if (!validator.validate()) return;

    SearchValue = $("#searchValue").val();

    $(".searchTotal").show();        //전자결재 검색영역 활성화
    $(".searchDetail").hide();       //이외 비활성화

    CurrentPage = 1;
    Search.LoadData();
}

Search.LoadData = function () {

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchDocData = function () {
        Search.GetCount(); //검색 총 카운트 가져오기
        Search.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                Search.LoadData();
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
    loadSearchDocData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 전자결재 검색 리스트 호출
**********************************************************************************************/
Search.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Home/Search.asmx/GetPageListDocAll";

    //파라미터값 배열에 담기    
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchValue': SearchValue
    };


    var obj = eval("Search.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 전자결재 검색 리스트 표시
**********************************************************************************************/
Search.SuccessPageList = function (data, status) {
    $('#phSearchDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Home/JTListDoc.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSearchDoc').processTemplate(data.d);
    //$('#loading').html("");        
    Search.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 전자결재 검색 리스트 메뉴 다국어 처리
**********************************************************************************************/

Search.Resource = function () {

    if ($("#spDocTh01") != undefined) { $("#spDocTh01").text(R_DocNumber); }
    if ($("#spDocTh02") != undefined) { $("#spDocTh02").text(R_Subject); }
    if ($("#spDocTh03") != undefined) { $("#spDocTh03").text(R_AttachName); }
    if ($("#spDocTh04") != undefined) { $("#spDocTh04").text(R_ReportUser); }
    if ($("#spDocTh05") != undefined) { $("#spDocTh05").text(R_WriteDate); }
    if ($("#spDocTh06") != undefined) { $("#spDocTh06").text(R_CompleteDate); }

    if ($("#spSearchDocTit") != undefined) { $("#spSearchDocTit").text(R_HomeSearchDocTit); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_HomeNoData); }
    if ($(".IconSecurity") != undefined) { $(".IconSecurity").attr('src', ImagePath + '/Icon/Security.gif'); }
    if ($(".IconEmergency") != undefined) { $(".IconEmergency").attr('src', ImagePath + '/Icon/Emergency.gif'); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 전자결재 검색 카운트  호출
**********************************************************************************************/
Search.GetCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Home/Search.asmx/GetPageListCountDocAll";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 전자결재 검색 카운트 이벤트
**********************************************************************************************/
Search.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
Search.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Search.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

Search.GetAttachFileListMenu = function (p_childDocID) {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Dept/List.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("Search.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Search.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Search.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var childDocID = el.getAttribute("childDocID");

    Search.GetIsAuthDoc(childDocID); //해당문서의 권한 가져오기
    if (!IsAuthDoc) {
        //권한이 없을경우
        alert(R_Msg_AttachDownAuth);
        return;
    }

    Search.GetAttachFileListMenu(childDocID);

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
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Search.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, Search.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 해당문서의 열람 권한체크
***************************************************/
Search.GetIsAuthDoc = function (p_childDocID) {

    var url = "/DeskPlusEIP/Approval/Ws/Home/Search.asmx/GetIsAuthDoc";

    //파라미터값 배열에 담기    
    ParamArray = {
        'documentID': p_childDocID
    };

    var obj = eval("Search.SuccessGetIsAuthDoc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시, 해당문서 권한
**********************************************************************************************/
Search.SuccessGetIsAuthDoc = function (data, status) {
    IsAuthDoc = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
Search.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 전체 다운로드
***************************************************/
Search.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        document.getElementById("frmTotalDownLoad").submit();

    }
}


/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 전자결재 검색 리스트 페이지 사이즈 저장
***************************************************/
Search.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Search_Doc",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Complete'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Search.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 전자결재 검색 리스트 페이지 사이즈 저장
***************************************************/
Search.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/*** 검색영역 end ***********************************************************************************************/



/*** 공통 start *********************************************************************************/

// 문서보기
Search.View = function (_companyID, _documentID, groupID) {

    if (_companyID == "" || _documentID == "" || _companyID == "0" || _documentID == "0") return;

    ParamArray = { 'companyID': _companyID, 'documentID': _documentID, 'srh': '1' };
    CallEncrypt(CallGetParam());

    //var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
    //window.open(url, "View" + _documentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    if (groupID < 800000) {
        url = "/DeskPlusEIP/Approval/Common/Popup33/ViewTot.aspx?p=" + GetEncryptValue;

        if ($.browser.msie) {
            PopupWinOpen(url, "", "View" + _documentID, "1000", "745", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
        else {
            PopupWinOpen(url, "", "View" + _documentID, "1000", "700", "menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
    }
    else {
        var url = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
        window.open(url, "View" + _documentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }
}

// 결재상태보기
Search.State = function (_isComplete, _documentID, _accountTag) {

    ParamArray = {
        'isComplete': _isComplete
        , 'documentID': _documentID
        , 'accountTag': _accountTag
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Status/Common/GetDocFlow.aspx?p=" + GetEncryptValue;
    PopupWin(850, 400, url, "-500", "", true);
}

// 결재하기
Search.Approve = function (_cID, _dID, _seq, _apptag, _tuserid, _tdeptid) {

    if (_cID == "" || _cID == "0" || _dID == "" || _dID == "0" || _seq == "" || _seq == "0" || _apptag == "" || _tuserid == "" || _tuserid == "0" || _tdeptid == "" || _tdeptid == "0") return;

    ParamArray = {
        'companyID': _cID
        , 'documentID': _dID
        , 'sequence': _seq
        , 'approvalTag': _apptag
        , 'targetUserID': _tuserid
        , 'targetDeptID': _tdeptid
        , 'func': 'Search.ApproveComplete()'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Process/Basic/ApprovePopup.aspx?p=" + GetEncryptValue;
    window.open(url, "Approve" + _dID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}
Search.ApproveComplete = function () {
    Search.SetTab(selmodule);
}
/*** 공통 start *********************************************************************************/



