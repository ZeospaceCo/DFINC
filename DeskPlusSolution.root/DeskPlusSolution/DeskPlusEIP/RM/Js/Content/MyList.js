MyList = {}

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
            //MyList.LoadData("");
            MyList.GoPageLoad();
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
                MyList.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            MyList.Search();
        });

        //예약
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            MyList.Add();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            MyList.ModCategoryUser();
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            MyList.RegDate();
        });

        //엑셀다운로드
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            MyList.Excel();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        __addEventListener(document, "click", MyList.click);
        MyList.LoadData("");

    });
});


/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 검색
***************************************************/
MyList.Search = function () {
    var response = MyList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    //MyList.LoadData("search");
    MyList.GoPageLoad();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 초기화
***************************************************/
MyList.LoadData = function (gubun) {

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
        MyList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadRMSearchData = function () {
        MyList.GetCount(); //검색 총 카운트 가져오기
        MyList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                MyList.LoadData("search");
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
MyList.GetPageList = function () {
    var url = "/DeskPlusEIP/RM/Ws/Content/MyList.asmx/GetPageList";

    //파라미터값 배열에 담기
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

    var obj = eval("MyList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
MyList.SuccessPageList = function (data, status) {
    $('#phList').setTemplateURL('/DeskPlusEIP/RM/Jt/Content/JTMyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phList').processTemplate(data.d);
    $('#loading').html("");

    MyList.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
MyList.Resource = function () {

    $("#r_resourcesTitle")[0].innerHTML = R_ResourcesTitle;
    $("#r_reserveDate")[0].innerHTML = R_ReserveDate;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_usable")[0].innerHTML = R_UsableStr;
    $("#r_attach")[0].innerHTML = R_HomeAttach;
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
MyList.GetCount = function () {
    var url = "/DeskPlusEIP/RM/Ws/Content/MyList.asmx/GetListCount";
    ParamArray = {
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
MyList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 검색 필드 설정하기
***************************************************/
MyList.SettingSearch = function () {
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
작성목적 : 자원 예약
**********************************************************************************************/
MyList.Add = function () {
    ParamArray = {
        'tagGubun': 'my'
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
MyList.ModCategoryUser = function (gubun) {
    var url = "/DeskPlusEIP/RM/Ws/Content/MyList.asmx/ModCategoryUser";
    ParamArray = {
        'categoryID': 0,
        'pageSize': $("#ddlListCount").val(),
        'types': "M"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MyList.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 폴더별 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
MyList.SuccessModCategoryUser = function (data, status) {
    if (data.d) {
        RecordPerPage = $("#ddlListCount").val();
        MyList.Search();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-12-17
작성목적 : 자원 보기
**********************************************************************************************/
MyList.View = function (_reservationID) {
    ParamArray = {
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': 'my',
        'reservationID': _reservationID
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
MyList.GoPageLoad = function () {

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue

    };

    CallEncrypt(CallGetParam());
    location = "MyList.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 클릭 이벤트 분기
***************************************************/
MyList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");

    switch (action) {
        case "attach": MyList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 정보가져오기
***************************************************/
MyList.GetAttachFileListMenu = function (p_reservationID) {
    var url = "/DeskPlusEIP/RM/Ws/Content/MyList.asmx/GetAttachFileListMenu";
    var data = "reservationID=" + p_reservationID;
    var obj = eval("MyList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
MyList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : famo23
작성일자 : 2016-03-21
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
MyList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    MyList.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, MyList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, MyList.DownLoad, "EXT");
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
MyList.DownLoad = function (_e) {
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
MyList.TotalDownLoad = function (_e) {
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
MyList.RegDate = function () {
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
MyList.Excel = function () {

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
        'usable': '4',
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "ExcelDown.aspx?p=" + GetEncryptValue;
}