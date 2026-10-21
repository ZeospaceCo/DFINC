ListUser = {}



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
            ListUser.LoadData("");
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
            ListUser.SavePageSize();
            ListUser.Search();
        });

        //년도
        $('#ddlYearField').change(function () {
            ListUser.Search();
        });

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            ListUser.ExcelDown();
        });   

        ListUser.LoadData("");
    });
});


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListUser.SavePageSize = function () {

    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListUser.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Holiday",
        'pageSize': $("#ddlListCount").val(),
        'types': "User"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListUser.SuccessSavePageSize");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : 김형석
작성일자 : 2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
ListUser.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ListUser.Search = function () {

    ListUser.SettingSearch();

    CurrentPage = 1; //1페이지로 이동처리

    ListUser.LoadData("search");
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
ListUser.SettingSearch = function () {

    StartDate = $("#ddlYearField").val().split(",")[0];
    EndDate = $("#ddlYearField").val().split(",")[1];
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ListUser.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ListUser.GetPageList();
        ListUser.GetHoliCount();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        ListUser.GetCount(); //검색 총 카운트 가져오기
        ListUser.GetPageList(); //검색 리스트 가져오기
        ListUser.GetHoliCount();

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListUser.LoadData("search");
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
ListUser.GetCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListUser.asmx/GetListCount";
    
    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'endDate': EndDate
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListUser.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListUser.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
ListUser.GetPageList = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListUser.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'endDate': EndDate,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage
    };

    var obj = eval("ListUser.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListUser.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/ETC/Jt/Holiday/JTListUser.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ListUser.Resource();
}

/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ListUser.Resource = function () {
    $("#r_startdate")[0].innerHTML = R_StartDate;
    $("#r_starthalf")[0].innerHTML = R_StartHalf;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_endhalf")[0].innerHTML = R_EndHalf;
    $("#r_duration")[0].innerHTML = R_Duration;
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_docview")[0].innerHTML = R_DocView;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 사용자 연차 수 가져오기
**********************************************************************************************/
ListUser.GetHoliCount = function () {
    var url = "/DeskPlusEIP/ETC/Ws/Holiday/ListUser.asmx/GetHoliCount";

    //파라미터값 배열에 담기   
    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'endDate': EndDate
    };

    var obj = eval("ListUser.SuccessHoliCount");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 김형석
작성일자 : 2011-04-14
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ListUser.SuccessHoliCount = function (data, status) {

    var param = data.d[0].UserID + "," + data.d[0].WriteUserName + "," + data.d[0].All1 + "," + data.d[0].Jan1;
    $("#lblStatus02").html(R_HoliUserAll + " : <a onclick=\"ListUser.View('" + param + "');return false;\" style=\"cursor:pointer\">" + data.d[0].All1 + R_HoliUserCnt + "</a> &nbsp;&nbsp;&nbsp;&nbsp; " + R_HoliUserJan + " : " + data.d[0].Jan1 + R_HoliUserCnt);
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가 건 수
***************************************************/
ListUser.View = function (data) {

    var arr = [];

    var param = data.split(",");
    $.each(param, function (i, o) {
        arr[i] = o;
    });

    ParamArray = {
        'userID': arr[0],
        'name': arr[1],
        'all1': arr[2],
        'jan1': arr[3],
        'titleDate': $("#ddlYearField option:selected").text()
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/ETC/Holiday/Popup/ViewUser.aspx?p=" + GetEncryptValue;
    PopupWin(500, 202, url, "", "", true);
}


/**************************************************
작성자   : 김형석
작성일자 : 2011-04-11
작성목적 : 휴가원 문서보기
***************************************************/
ListUser.DocView = function (docID) {

    ParamArray = {       
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}


/**********************************************************************************************
작성자   : 김형석  
작성일자 : 2011-11-24
작성목적 : 엑셀
**********************************************************************************************/
ListUser.ExcelDown = function () {
    ParamArray = {
        'userID': UserID,
        'startDate': StartDate,
        'endDate': EndDate
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    $("#hdexcel").val(GetEncryptValue);

    __doPostBack('lbtnexcel', "");
}
