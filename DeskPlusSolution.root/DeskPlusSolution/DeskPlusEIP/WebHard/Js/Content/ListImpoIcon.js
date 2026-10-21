ListImpoIcon = {}
ListImpoIcon.TreeFrame = parent.frmLeft;

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            ListImpoIcon.LoadData("");
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

        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            ListImpoIcon.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListImpoIcon.Search();
            }
        });

        //검색
        $('#CbtnSearch').click(function (e) {
            e.preventDefault();
            ListImpoIcon.Search();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListImpoIcon.ModFolderUser("2");
            ListImpoIcon.GoPageListType("2");
        });

        //해제
        $('#CbtnDel').click(function (e) {
            e.preventDefault();
            ListImpoIcon.Del();
        });

        //리스트형 저장
        $('#listType1').click(function () {
            ListImpoIcon.ModFolderUser("1");
            ListImpoIcon.GoPageListType("1");
        });

        //Icon 형 저장
        $('#listType2').click(function () {
            ListImpoIcon.ModFolderUser("2");
            ListImpoIcon.GoPageListType("2");
        });

        //Preview 형 저장
        $('#listType3').click(function () {
            ListImpoIcon.ModFolderUser("3");
            ListImpoIcon.GoPageListType("3");
        });

        $("#startEndDate").hide();

        $(".clsDate").keydown(function (e) { return false; });

        ListImpoIcon.LoadData("");

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", ListImpoIcon.click);

        $("#zeoModalContact").draggable({ disabled: false });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 달력 보이기 
***************************************************/
ListImpoIcon.RegDate = function () {
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
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 검색
***************************************************/
ListImpoIcon.Search = function () {
    var response = ListImpoIcon.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리     

    ListImpoIcon.LoadData("search");
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 초기화
***************************************************/
ListImpoIcon.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadWebHardData = function () {
        ListImpoIcon.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadWebHardSearchData = function () {
        ListImpoIcon.GetCount(); //검색 총 카운트 가져오기
        ListImpoIcon.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                ListImpoIcon.LoadData("search");
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
            loadWebHardSearchData();
        }
        else {
            loadWebHardData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListImpoIcon.GetPageList = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListImpoIcon.asmx/GetImpoPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    var obj = eval("ListImpoIcon.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListImpoIcon.SuccessPageList = function (data, status) {
    $('#phListImpoIcon').setTemplateURL('/DeskPlusEIP/WebHard/Jt/Content/JTListImpoIcon.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListImpoIcon').processTemplate(data.d);
    $('#loading').html("");

    ListImpoIcon.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListImpoIcon.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
ListImpoIcon.GetCount = function () {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListImpoIcon.asmx/GetImpoListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListImpoIcon.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
ListImpoIcon.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 정렬
**********************************************************************************************/
ListImpoIcon.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    ListImpoIcon.LoadData("search");
}


/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 클릭 이벤트 분기
***************************************************/
ListImpoIcon.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ListImpoIcon.DownLoad(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 첨부파일 열기
***************************************************/
ListImpoIcon.DownLoad = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    FileID = el.id;
    FilePath = el.getAttribute("FilePath");
    var attachFolderID = el.getAttribute("FolderID");

    ListImpoIcon.GetAuthority(attachFolderID);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 전체 체크박스
***************************************************/
ListImpoIcon.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 선택된 값 가져오기
***************************************************/
ListImpoIcon.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-12-30
작성목적 : 검색 필드 설정하기
***************************************************/
ListImpoIcon.SettingSearch = function () {

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
작성일자 : 2012-01-02
작성목적 : 다운로드 권한 체크 (웹서비스 호출)
**********************************************************************************************/
ListImpoIcon.GetAuthority = function (_attachFolderID) {
    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListImpoIcon.asmx/GetAuthority";
    ParamArray = {
        'folderID': _attachFolderID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListImpoIcon.SuccessAuthority");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-02
작성목적 : 다운로드 권한 체크 (웹서비스 값 받기)
**********************************************************************************************/
ListImpoIcon.SuccessAuthority = function (data, status) {
    var tmpVal = data.d;
    if (tmpVal == "N") {
        alert(R_DownloadAuthority);
    }
    else {

        var attachFileName = $("#fileName_" + FileID).text().trimText();

        CallEncrypt("filePath=" + FilePath + "&fileName=" + attachFileName);
        attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;

        window.open(attachFilePath);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-03
작성목적 : 중요표시 해제 (웹서비스 호출)
**********************************************************************************************/
ListImpoIcon.Del = function () {

    var selValue = ListImpoIcon.CheckedValue();
    var tmpValue = "";

    if (selValue != "") {
        if (confirm(R_DelImpoFile)) {

            for (var i = 0; i < selValue.length; i++) {
                tmpValue += selValue[i] + ",";
            }

            ParamArray = {
                'tmpValue': tmpValue
            };

            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListImpoIcon.SuccessDel");
            var url = "/DeskPlusEIP/WebHard/Ws/Content/ListImpoIcon.asmx/DelImpUser";

            AjaxService(url, data, obj);
        }

    } else {
        alert(R_SelectDelImpo);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-03
작성목적 : 중요표시 해제 (웹서비스 값 받기) 
**********************************************************************************************/
ListImpoIcon.SuccessDel = function (data, status) {

    if (data.d) {
        // 페이지 리스트 호출 
        alert(R_CompleteConfirm);
        ListImpoIcon.ListLoad();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-03
작성목적 : 리스트 해제에 따른 리스트 재로딩
***************************************************/
ListImpoIcon.ListLoad = function () {
    document.all("ddlSearchTerm").selectedIndex = 0;
    document.all("ddlSearchField").selectedIndex = 0;

    $("#ctbSearch").val("");
    $("#startDate").val("");
    $("#endDate").val("");
    $("#startEndDate").hide(); //기간 이외

    CurrentPage = 1; //1페이지로 이동처리     

    ListImpoIcon.LoadData("search");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-09
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
ListImpoIcon.GoPageListType = function (gubun) {

    var iPath = "ListImpo.apsx";

    switch (gubun) {
        case "1":
            iPath = "ListImpo.aspx";
            break;
        case "2":
            iPath = "ListImpoIcon.aspx";
            break;
        case "3":
            iPath = "ListImpoPreview.aspx";
            break;
        default:
            iPath = "ListImpo.aspx";
            break;
    }


    var response = ListImpoIcon.SettingSearch();
    if (response == false) return;


    ParamArray = {
        'currentPage': 1,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-09
작성목적 : 파일명 클릭시에 첨부파일 열기
***************************************************/
ListImpoIcon.TitleDownLoad = function (_fileID, _folderID, _filePath) {

    FileID = _fileID;
    FilePath = _filePath;
    ListImpoIcon.GetAuthority(_folderID);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-12
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
ListImpoIcon.ModFolderUser = function (gubun) {

    var url = "/DeskPlusEIP/WebHard/Ws/Content/ListImpoIcon.asmx/ModFolderUser";
    ParamArray = {
        'folderID': '0',
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': 'I'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListImpoIcon.SuccessModFolderUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-12
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
ListImpoIcon.SuccessModFolderUser = function (data, status) {
    if (data.d == true) {
    }
}