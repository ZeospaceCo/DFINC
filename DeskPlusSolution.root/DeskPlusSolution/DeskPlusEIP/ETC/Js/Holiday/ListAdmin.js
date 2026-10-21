ListAdmin = {}



/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListAdmin.LoadData("");
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

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListAdmin.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListAdmin.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ListAdmin.SavePageSize();
            ListAdmin.Search();
        });

        //년도
        $('#ddlYearField').change(function () {           
            ListAdmin.Search();
        });

        //휴가일 수 저장
        $('#cbtnPost').click(function (e) {
            ListAdmin.SetHoli();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListAdmin.ExcelDown();
        });

        //샘플 다운
        $('#cbtnSample').click(function (e) {
            e.preventDefault();
            ListAdmin.SampleDown();
        });

        //엑셀업로드
        $('#cbtnExcelUpload').click(function (e) {
            e.preventDefault();
            ListAdmin.ExcelUpload();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        ListAdmin.LoadData("");
    });
});


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListAdmin.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Holiday",
        'pageSize': $("#ddlListCount").val(),
        'types': "Admin"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdmin.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListAdmin.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ListAdmin.Search = function () {

    ListAdmin.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    ListAdmin.LoadData("search");
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListAdmin.SettingSearch = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    StartDate = $("#ddlYearField").val().split(",")[0];
    EndDate = $("#ddlYearField").val().split(",")[1];    
}


/**************************************************
작성자   : 김형석
작성일자 : 2013-05-01
작성목적 : 휴가일 수 저장하기
***************************************************/
ListAdmin.SetHoli = function () {

    var listUserID = "";    //사용자ID
    var listReceive1 = "";  //연차일수
    
    $.each($(".holiuserid"), function (i, o) {
        if (o.value.trimText() == "") {
            listUserID += "0" + ",";
        }
        else {
            listUserID += o.value + ",";
        }
    });

    $.each($(".holireceive1"), function (i, o) {
        if (o.value.trimText() == "") {
            listReceive1 += "0" + ",";
        }
        else {
            listReceive1 += o.value + ",";
        }
    });

    if (listUserID != "") {
        ListAdmin.SetHoliReceive(listUserID.trimEnds(","), listReceive1.trimEnds(","));
    }
}

/**************************************************
작성자   : 김형석
작성일자 :2011-06-15
작성목적 : 휴가일 수 저장하기
***************************************************/
ListAdmin.SetHoliReceive = function (userid, receive1) {

    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/SetHoliReceive";

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': userid,
        'holiReceive1': receive1      
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdmin.SuccessSetHoliReceive");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 휴가일 수 저장하기
**********************************************************************************************/
ListAdmin.SuccessSetHoliReceive = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        ListAdmin.Search();
    }
    else {
        alert(R_MSG_FAIL);
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListAdmin.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListAdmin.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListAdmin.GetCount(); //검색 총 카운트 가져오기
        ListAdmin.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListAdmin.LoadData("search");
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


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-18
작성목적 : 웹서비스  카운트  호출
**********************************************************************************************/
ListAdmin.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/GetListCount";
    
    ParamArray = {        
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdmin.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListAdmin.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
ListAdmin.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/GetPageList";
    
    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("ListAdmin.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListAdmin.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Holiday/JTListAdmin.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListAdmin.Resource();
}

/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListAdmin.Resource = function () {
    $("#r_number")[0].innerHTML = R_Number;
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_dutyname")[0].innerHTML = R_DutyName;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_year")[0].innerHTML = R_YearText;
    $("#r_history")[0].innerHTML = R_History;
    $("#r_holiused")[0].innerHTML = R_HoliUsed;
    $("#r_holireceive")[0].innerHTML = R_HoliReceive;

    if ($(".r_holitype1") != undefined) { $(".r_holitype1").text(R_HoliType1); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가 내역
***************************************************/
ListAdmin.View = function (userID, name) {

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': userID,
        'name': name,
        'titleDate': $("#ddlYearField option:selected").text(),
        'recordPerPage': "5"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/ETC/Holiday/Popup/ViewAdmin.aspx?p=" + GetEncryptValue;
    PopupWin(900, 400, url, "", "", true);
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가 내역
***************************************************/
ListAdmin.ViewHistory = function (userID, userName, type) {

    ParamArray = {
        'userID': userID,
        'name': userName,
        'startDate': StartDate,
        'titleDate': $("#ddlYearField option:selected").text(),
        'type': type
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/ETC/Holiday/Popup/ViewHistory.aspx?p=" + GetEncryptValue;
    PopupWin(650, 400, url, "", "", true);
}


/**********************************************************************************************
작성자   : 김형석  
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
ListAdmin.ExcelDown = function () {
    ParamArray = {
        'totalCount': TotalItemCount, 
        'startDate': StartDate,
        'endDate': EndDate,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/**********************************************************************************************
작성자   : ellepooky  
작성일자 :2018-11-22
작성목적 : Sample다운
**********************************************************************************************/
ListAdmin.SampleDown = function () {
    CallEncrypt("filePath=/DeskPlusEIP/FileData/1/Etc/Holiday/HoliAdd.xlsx&fileName=HoliAdd.xlsx");
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;

    window.open(attachFilePath);
}

/**********************************************************************************************
작성자   : ellepooky  
작성일자 :2018-11-22
작성목적 : 엑셀 업로드
**********************************************************************************************/
ListAdmin.ExcelUpload = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'Holiday',
        'callBackEvent': "ListAdmin.Upload"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510");
}

/**********************************************************************************************
작성자   : ellepooky  
작성일자 :2018-11-22
작성목적 : 엑셀 업로드
**********************************************************************************************/
ListAdmin.Upload = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListAdmin.SuccessUpload");
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/SetExcelUpload";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : ellepooky  
작성일자 :2018-11-22
작성목적 : 엑셀 업로드
**********************************************************************************************/
ListAdmin.SuccessUpload = function (data, status) {
    if (data.d == "1") {
        alert("적용되었습니다");
    }
    else if (data.d == "-1") {
        alert("에러가 발생하였습니다");
        return;
    }
    else {
        alert(data.d + " 파일은 등록 할 수 없습니다.");
    }
    
    location = "/DeskPlusEIP/ETC/Holiday/ListAdmin.aspx";
}