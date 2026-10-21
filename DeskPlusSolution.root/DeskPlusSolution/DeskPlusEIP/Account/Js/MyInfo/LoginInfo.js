LoginInfo = {}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            LoginInfo.GoPageLoad();
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

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
        });

        //닫기
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            LoginInfo.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                LoginInfo.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            LoginInfo.Search();
        });

        LoginInfo.Init();

        //드롭다운박스 초기화
        //LoginInfo.SearchField();

        //검색필드
        $('#ddlSearchField').change(function () {
            LoginInfo.SearchField();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            CurrentPage = 1;
            RecordPerPage = $('#ddlListCount').val();
            LoginInfo.GoPageLoad()
        });

    });
});



/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
LoginInfo.Init = function () {
    $("#startEndDate").hide(); //기간

    if (SearchTerm == "PE") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    LoginInfo.LoadData("");

}



/**************************************************
작성자   : maniaysw
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
LoginInfo.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadInfoData = function () {
        LoginInfo.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadInfoSearchData = function () {
        LoginInfo.GetCount(); //검색 총 카운트 가져오기
        LoginInfo.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                LoginInfo.LoadData("search");
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
            loadInfoSearchData();
        }
        else {
            loadInfoData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
LoginInfo.GetPageList = function () {
    
    var url = "/DeskPlusEIP/Account/Ws/MyInfo/LoginInfo.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue

    };

    var obj = eval("LoginInfo.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
LoginInfo.SuccessPageList = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/MyInfo/JTLoginInfo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d); //string 을 JSON.parse 통해json 형태로 object  화 함. (maniaysw)
    $('#loading').html("");

    LoginInfo.Resource();
}

LoginInfo.Resource = function (data, status) {    
    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_startTime")[0].innerHTML = R_LoginTime;
    $("#r_ipAddress")[0].innerHTML = R_IPAddress;    
}

LoginInfo.GoPageLoad = function () {
    
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue

    };

    CallEncrypt(CallGetParam());
    location = "LoginInfo.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : maniaysw
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
LoginInfo.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "PE") {
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
작성자   : maniaysw
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
LoginInfo.Search = function () {

    var response = LoginInfo.SettingSearch();
    
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    //LoginInfo.LoadData("search");
    LoginInfo.GoPageLoad();
}

/**************************************************
작성자   : maniaysw
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
LoginInfo.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();

    if (SearchTerm == "PE") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            SearchTerm = '6M';
            return false;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                SearchTerm = '6M';
                return false;
            }
        }

        SearchTermValue = getStartDate + "," + getEndDate;
    }

    return true;
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
LoginInfo.GetCount = function () {
    
    var url = "/DeskPlusEIP/Account/Ws/MyInfo/LoginInfo.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("LoginInfo.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
LoginInfo.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

