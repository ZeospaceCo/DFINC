AdminList = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            //AdminList.LoadData("");
            AdminList.GoPageLoad();
        }
    }


    $(document).ready(function () {
        RecordPerPage = $("#ddlListCount").val();

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


        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminList.Search();
            }
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            AdminList.RegDate();
        });


        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AdminList.Search();
        });
        
        // 담당자원
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            AdminList.ManagerList();
        });

        //엑셀다운로드
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            AdminList.Excel();
        });

        // 승인 
        $('#cbtnApproved').click(function (e) {
            e.preventDefault();
            AdminList.Approved('1');
        });

        // 거부 
        $('#cbtnUnapproved').click(function (e) {
            e.preventDefault();
            AdminList.Reject();
            //AdminList.Approved('2');
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            AdminList.ModCategoryUser();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", AdminList.click);

        // 대기자원 카운트 메뉴트리에 보이게 처리
        if (TotalItemCount == 0) {
            parent.frmLeft.mCnt.innerText = "";
            parent.frmLeft.mSubCnt.innerText = "";
        }
        else {
            parent.frmLeft.mCnt.innerText = "(" + TotalItemCount + ")";
            parent.frmLeft.mSubCnt.innerText = "(" + TotalItemCount + ")";
        }

        AdminList.LoadData("");

    });
});


/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 검색
***************************************************/
AdminList.Search = function () {
    var response = AdminList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    //AdminList.LoadData("search");
    AdminList.GoPageLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 초기화
***************************************************/
AdminList.LoadData = function (gubun) {
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


    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadRMData = function () {
        AdminList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadRMSearchData = function () {
        AdminList.GetCount(); //검색 총 카운트 가져오기
        AdminList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                AdminList.LoadData("search");
            }
        }

        RecordPerPage = $("#ddlListCount").val();

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
            loadRMSearchData();
        }
        else {
            loadRMData();
        }
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
AdminList.GetPageList = function () {

    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'usable': '0',
        'order': Order,
        'adminYN': AdminYN
    };

    var obj = eval("AdminList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AdminList.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/Manager/JTUsableList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    AdminList.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AdminList.Resource = function () {

    $("#r_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#r_reserveDate")[0].innerHTML = R_ReserveDate;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_attach")[0].innerHTML = R_HomeAttach;
    $("#r_subscriber")[0].innerHTML = R_Subscriber;
    $("#r_modifyDay")[0].innerHTML = R_ModifyDate;

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
AdminList.GetCount = function () {
    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/GetListCount";
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'usable': '0',
        'adminYN': AdminYN
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
AdminList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    // 대기자원 카운트 메뉴트리에 보이게 처리
    if (TotalItemCount == 0) {
        parent.frmLeft.mCnt.innerText = "";
        parent.frmLeft.mSubCnt.innerText = "";
    }
    else {
        parent.frmLeft.mCnt.innerText = "(" + TotalItemCount + ")";
        parent.frmLeft.mSubCnt.innerText = "(" + TotalItemCount + ")";
    }

}

/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 전체 체크박스
***************************************************/
AdminList.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 선택된 값 가져오기
***************************************************/
AdminList.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 검색 필드 설정하기
***************************************************/
AdminList.SettingSearch = function () {

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


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
AdminList.ModCategoryUser = function (gubun) {
    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/ModCategoryUser";
    ParamArray = {
        'categoryID': 0,
        'pageSize': $("#ddlListCount").val(),
        'types': "A"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminList.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
AdminList.SuccessModCategoryUser = function (data, status) {
    if (data.d) {
        RecordPerPage = $("#ddlListCount").val();
        AdminList.Search();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 자원 보기
**********************************************************************************************/
AdminList.View = function (_reservationID) {
    ParamArray = {
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'usable0',
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/RM/Content/View.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 자원 승인 / 거부 (웹서비스 호출)
**********************************************************************************************/
AdminList.Approved = function (_gubun) {
    var selValue = AdminList.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'usable': _gubun,
            'tmpValue': tmpValue
        };

        CallEncrypt(CallGetParam());

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("AdminList.SuccessApproved");
        var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/ModUsable";

        AjaxService(url, data, obj);

    } else {
        alert(R_SelectResources);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 자원 승인 / 거부 (웹서비스 값 받기)
**********************************************************************************************/
AdminList.SuccessApproved = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
        AdminList.Search();
    }
    else {
        alert(R_ProcessingError);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 자원 승인, 거부에 따른 페이지 리로드
***************************************************/
AdminList.ListLoad = function () {
    document.all("ddlSearchField").selectedIndex = 0;

    $("#ctbSearch").val("");

    CurrentPage = 1; //1페이지로 이동처리

    AdminList.LoadData("search");

    alert(R_CompleteConfirm);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
AdminList.GoPageLoad = function () {

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/RM/Content/Manager/Usable0List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 클릭 이벤트 분기
***************************************************/
AdminList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "attach": AdminList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 정보가져오기
***************************************************/
AdminList.GetAttachFileListMenu = function (p_reservationID) {
    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/GetAttachFileListMenu";
    var data = "reservationID=" + p_reservationID;
    var obj = eval("AdminList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
AdminList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
AdminList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    AdminList.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, AdminList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, AdminList.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 열기
***************************************************/
AdminList.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 전체 다운로드
***************************************************/
AdminList.TotalDownLoad = function (_e) {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
AdminList.RegDate = function () {
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



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2017-01-24
작성목적 : 엑셀다운로드
**********************************************************************************************/
AdminList.Excel = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    //파라미터값 배열에 담기
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'usable': '0',
        'order': Order,
        'adminYN': 'N'
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/RM/Content/ExcelDown.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2017-03-02
작성목적 : 거부 사유 입력창 오픈
***************************************************/
AdminList.Reject = function () {
    var selValue = AdminList.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {

        for (var i = 0; i < selValue.length; i++) {
            tmpValue += selValue[i] + ",";
        }

        ParamArray = {
            'callBackEvent': "AddReject",
            'reservationID': tmpValue
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/RM/Content/Reject/AddReject.aspx?p=" + GetEncryptValue;
        PopupWin(500, 200, url, "-500", "-220", false);
    }
    else {
        alert(R_SelectResources);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-02
작성목적 : 거부 사유 입력후 
***************************************************/
function AddReject() {

    var selValue = AdminList.CheckedValue();
    var tmpValue = "";

    for (var i = 0; i < selValue.length; i++) {
        tmpValue += selValue[i] + ",";
    }

    ParamArray = {
        'usable': "2",
        'tmpValue': tmpValue
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminList.SuccessApproved");
    var url = "/DeskPlusEIP/RM/Ws/Content/Manager/Usable0List.asmx/ModUsable";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2017-03-08
작성목적 : 담당자원내역
***************************************************/
AdminList.ManagerList = function () {

    var url = "/DeskPlusEIP/RM/Content/Manager/List.aspx"
    PopupWin(700, 390, url, "", "", true);
}
