List = {}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            //List.LoadData("");
            List.GoPageLoad();
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

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        if (SearchValue != "") {
            $("#ctbSearch").val(SearchValue.securityText());
        }

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

        //대결자 추가 
        $('#Add').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //대결자 삭제 
        $('#Del').click(function (e) {
            e.preventDefault();
            List.Del();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val()
            List.SavePageSize();
            List.Search();

        });

        List.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

    });
});


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

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

    List.LoadData("");
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    //List.LoadData("search");
    List.GoPageLoad();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
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
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadProxyData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadProxySearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
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
        // 삭제시에 페이지 총카운트 갯수 변화때문에 구분
        if (gubun == "search") {
            loadProxySearchData();

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
        else {
            loadProxyData();

            var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
            $("#pageInfo").text(CurrentPage + " / " + pageCount);
        }
    }, 100);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/Proxy/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': "0",
        'deptID': "0",        
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'totalItemCount': TotalItemCount,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Admin/Proxy/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Admin/Proxy/List.asmx/GetPageListCnt";

    ParamArray = {
        'companyID': CompanyID,
        'userID': "0",
        'deptID': "0",
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("List.SuccessGetPageListCnt");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 사용자 지정 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessGetPageListCnt = function (data, status) {
    var response = data.d;
    TotalItemCount = response;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_name0")[0].innerHTML = R_UserName;
    $("#r_dept0")[0].innerHTML = R_DeptName;
    //$("#r_photo")[0].innerHTML = R_Photo;
    $("#r_name")[0].innerHTML = R_TargetUser + " " + R_UserName;
    $("#r_dept")[0].innerHTML = R_TargetUser + " " + R_DeptName;
    $("#r_sta")[0].innerHTML = R_StateInfo;
    $("#r_proxydate")[0].innerHTML = R_ProxyDate;
    $("#r_proxytype")[0].innerHTML = R_ProxyType;
    $("#r_isModfy")[0].innerHTML = R_IsModfy;

    if ($(".spIng") != undefined) { $(".spIng").text(R_Msg_Resource06); } //진행
    if ($(".spCall") != undefined) { $(".spCall").text(R_Msg_Resource08); } //대기
    if ($(".spCallEnd") != undefined) { $(".spCallEnd").text(R_Msg_Resource09); } //종료

    if ($("#r_noData")[0]) {
        $("#r_noData")[0].innerHTML = R_NoData;
    }

    if ($(".BtnCheck") != undefined) { $(".BtnCheck").attr('src', ImagePath + '/Icon/CheckY.png'); }
    if ($(".BtnUncheck") != undefined) { $(".BtnUncheck").attr('src', ImagePath + '/Icon/CheckG.png'); }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 대결자 추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;

}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 대결자 삭제
**********************************************************************************************/
List.Del = function () {

    var proxyID = List.CheckedValue();

    if (proxyID != "") {
        if (window.confirm(R_ConfirmDeleteProxyPerson)) {

            ParamArray = {
                'proxyID': proxyID
            };

            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Ws/Admin/Proxy/List.asmx/DelProxyUser";
            var obj = eval("List.SuccessDelProxyUser");
            CallEncrypt(CallGetParam());
            data = "{p:'" + GetEncryptValue + "'}";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_CheckDeleteProxyPerson);
    }
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 대결자 삭제 성공시 페이지 표시
**********************************************************************************************/
List.SuccessDelProxyUser = function (data, status) {
    if (data.d == true) {

        CurrentPage = 1; //1페이지로 이동처리  
        //List.LoadData("search");
        List.GoPageLoad();
    }
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 전체 체크박스
***************************************************/
List.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-10
작성목적 : 체크박스 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "Proxy",
        'pageSize': $("#ddlListCount").val(),
        'types': 'Admin'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 문서함 리스트 페이지 사이즈 저장
***************************************************/
List.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 대결자 정보 보기
**********************************************************************************************/
List.View = function (_proxyID) {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'targetProxyID': _proxyID
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'userID': "0",
        'deptID': "0",
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

