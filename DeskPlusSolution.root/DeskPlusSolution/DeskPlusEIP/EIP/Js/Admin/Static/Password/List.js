List = {}

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            List.LoadData("");
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

        // 기간검색
        $("#ddlSearchTerm").hide();
        $("#startEndDate").hide();

        // 검색조건
        $('#ddlSearchField').change(function (e) {
            e.preventDefault();
            List.VisibleSearch();
        });

        // 검색조건(기간검색)
        $('#ddlSearchTerm').change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //검색 
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();

            List.Search();
        });

        //엑셀다운로드
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            List.Excel();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.ModPageSize();
            
        });

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.LoadData("");
    });
});

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 기간검색 활성화 여부
***************************************************/
List.VisibleSearch = function () {

    var sField = $("#ddlSearchField").val();

    if (sField == "AccessTimeYMD") {
        $("#ddlSearchTerm").show();
    }
    else {
        $("#ddlSearchTerm").hide();     // 이름, IP 이외
        $("#startEndDate").hide();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
    var sTerm = $("#ddlSearchTerm").val();

    if (sTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    }
    else {
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
List.LoadData = function (gubun) {
    
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                //if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                List.LoadData("search");
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
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Static/Password/List.asmx/GetListCount";

    SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    
    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Static/Password/List.asmx/GetStaticPassword";

    SearchTerm = $("#ddlSearchTerm").val();
    SearchTermValue = $("#startDate").val() + "," + $("#endDate").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order' : Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#plShareUser').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/Static/Password/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plShareUser').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-07
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_dept")[0].innerHTML = R_DeptName;
    $("#r_title")[0].innerHTML = R_TitleName;
    $("#r_name")[0].innerHTML = R_UserName
    $("#r_ip")[0].innerHTML = R_IPAddress;
    $("#r_accessTime")[0].innerHTML = R_AccessTime;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 검색
**********************************************************************************************/
List.Search = function () {
    var response = List.SettingSearch();
    if (response == false) return;
    
    CurrentPage = 1; //1페이지로 이동처리  

    List.LoadData("search");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

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
작성자   : shine
작성일자 :2011-06-15
작성목적 : 페이지 사이즈 설정
***************************************************/
List.ModPageSize = function () {
    var response = List.SettingSearch();
    if (response == false) return;

    RecordPerPage = $('#ddlListCount').val();
    List.Search();

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-02-10
작성목적 : 엑셀다운로드
**********************************************************************************************/
List.Excel = function () {
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
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "ExcelDown.aspx?p=" + GetEncryptValue;
}