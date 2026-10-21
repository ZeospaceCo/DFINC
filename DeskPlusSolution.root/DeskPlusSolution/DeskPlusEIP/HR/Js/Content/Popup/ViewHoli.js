ViewHoli = {}

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //ViewHoli.LoadData("");
            ViewHoli.GoPageLoad();
        }
    }

    $(document).ready(function () {
        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

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

        $('#ddlUser').change(function () {
            UserID = $('#ddlUser').val();
            ViewHoli.GoPageLoad();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            ViewHoli.SavePageSize();
            CurrentPage = 1; //1페이지로 이동처리
            ViewHoli.GoPageLoad();
        });

        ViewHoli.Init();
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
ViewHoli.Init = function () {
    ViewHoli.LoadData("");
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ViewHoli.SavePageSize = function () {

    var url = "/DeskPlusEIP/HR/Ws/Content/ListPrivateMod.asmx/SavePageSize";

    ParamArray = {
        'folderID': "HRHoli",
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewHoli.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ViewHoli.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ViewHoli.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ViewHoli.GetCount(); //검색 총 카운트 가져오기
        ViewHoli.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ViewHoli.GetCount(); //검색 총 카운트 가져오기
        ViewHoli.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ViewHoli.LoadData("search");
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
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 카운트 호출
**********************************************************************************************/
ViewHoli.GetCount = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/ViewHoli.asmx/GetHoliListCount";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'minClosingID': MinClosingID,
        'maxClosingID': MaxClosingID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewHoli.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ViewHoli.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ViewHoli.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/ViewHoli.asmx/GetHoliList";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'minClosingID': MinClosingID,
        'maxClosingID': MaxClosingID,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("ViewHoli.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ViewHoli.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/Popup/JTListViewHoli.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ViewHoli.Resource();
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ViewHoli.Resource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_deptname")[0].innerHTML = R_DeptName;
    $("#r_username")[0].innerHTML = R_UserName;
    $("#r_titlename")[0].innerHTML = R_TitleName;
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_starthalf")[0].innerHTML = R_StartHalf;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_endhalf")[0].innerHTML = R_EndHalf;
    $("#r_duration")[0].innerHTML = R_Duration;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_docview")[0].innerHTML = R_DocView;

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-17
작성목적 : 휴가원 문서보기
***************************************************/
ViewHoli.DocView = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
ViewHoli.GoPageLoad = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'userID': UserID,
        'userName': UserName,
        'deptID': DeptID,
        'pathID': PathID,
        'deptBase': DeptBase,
        'startDate': StartDate,
        'endDate': EndDate,
        'minClosingID': MinClosingID,
        'maxClosingID': MaxClosingID
    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/HR/Content/Popup/ViewHoli.aspx?p=" + GetEncryptValue;
}