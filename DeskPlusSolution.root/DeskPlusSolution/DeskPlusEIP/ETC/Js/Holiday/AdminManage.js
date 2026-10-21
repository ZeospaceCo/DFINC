AdminManage = {}


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
            AdminManage.LoadData("");
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

        //검색입력필드 엔터 이벤트 기술(이름,사번)
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminManage.Search();
            }
        });

        //검색입력필드 엔터 이벤트 기술(직급)
        $("#ctbTitleName").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminManage.Search();
            }
        });

        //검색입력필드 엔터 이벤트 기술(부서)
        $("#ctbDeptName").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                AdminManage.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            AdminManage.Search();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            AdminManage.SavePageSize();
            AdminManage.Search();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            AdminManage.ExcelDown();
        });

        //AdminManage.LoadData("");
        AdminManage.Init();
    });
});

/**************************************************
작성자   : shine
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
AdminManage.Init = function () {

    $("#startDate").datepick($.datepicker.regional[PreLang]);
    $("#endDate").datepick($.datepicker.regional[PreLang]);

    if (SearchTermValue != "") {
        $("#startDate").val(SearchTermValue.split(",")[0]);
        $("#endDate").val(SearchTermValue.split(",")[1]);
    }
    
    AdminManage.LoadData("");
    __addEventListener(document, "click", AdminManage.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminManage.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListAdmin.asmx/SavePageSize";

    ParamArray = {
        'folderID': "HolidayManage",
        'pageSize': $("#ddlListCount").val(),
        'types': "Admin"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminManage.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
AdminManage.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
AdminManage.Search = function () {

    AdminManage.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    AdminManage.LoadData("search");
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
AdminManage.SettingSearch = function () {

    // 재직, 퇴사 여부(1:재직, 0:퇴사)
    ActiveUsers = $("input[type=radio]:checked").val();
    //부서
    DeptName = $("#ctbDeptName").val();
    //직급
    TitleName = $("#ctbTitleName").val();
    // 검색필드
    SearchField = $("#ddlSearchField").val();
    // 구분
    Gubun = $("#ddlGubun").val();

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

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
AdminManage.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        AdminManage.GetCount(); //검색 총 카운트 가져오기
        AdminManage.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        AdminManage.GetCount(); //검색 총 카운트 가져오기
        AdminManage.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                AdminManage.LoadData("search");
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
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 웹서비스  카운트  호출
**********************************************************************************************/
AdminManage.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/AdminManage.asmx/GetPageCount";

    // 재직, 퇴사 여부(1:재직, 0:퇴사)
    ActiveUsers = $("input[type=radio]:checked").val();

    ParamArray = {
        'activeUser': ActiveUsers,
        'searchTermValue': SearchTermValue,
        'deptName': DeptName,
        'titleName': TitleName,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'gubun': Gubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AdminManage.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
AdminManage.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
AdminManage.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/AdminManage.asmx/GetPageList";

    ParamArray = {
        'activeUser': ActiveUsers,
        'searchTermValue': SearchTermValue,
        'deptName': DeptName,
        'titleName': TitleName,
        'gubun': Gubun,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("AdminManage.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
AdminManage.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Holiday/JTAdminManage.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    AdminManage.Resource();
}

/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
AdminManage.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_empNo")[0].innerHTML = R_EmpNo;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_starthalf")[0].innerHTML = R_StartHalf;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_endhalf")[0].innerHTML = R_EndHalf;
    $("#r_duration")[0].innerHTML = R_Duration;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_docview")[0].innerHTML = R_DocView;

    //if ($(".r_holitype1") != undefined) { $(".r_holitype1").text(R_HoliType1); }
    //if ($(".r_holitype2") != undefined) { $(".r_holitype2").text(R_HoliType2); }
    //if ($(".r_holitype3") != undefined) { $(".r_holitype3").text(R_HoliType3); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가 내역
***************************************************/
AdminManage.View = function (userID, name) {

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
AdminManage.ViewHistory = function (userID, userName, type) {

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
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
AdminManage.ExcelDown = function () {
    ParamArray = {
        'activeUser': ActiveUsers,
        'searchTermValue': SearchTermValue,
        'deptName': DeptName,
        'titleName': TitleName,
        'gubun': Gubun,
        'totalCount': TotalItemCount,
        'pageSize': TotalItemCount,
        'pageSizePageNo': 1,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 휴가원 문서보기
**********************************************************************************************/
AdminManage.DocView = function (p_docID) {

    ParamArray = {
        'documentID': p_docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + p_docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
AdminManage.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": AdminManage.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;