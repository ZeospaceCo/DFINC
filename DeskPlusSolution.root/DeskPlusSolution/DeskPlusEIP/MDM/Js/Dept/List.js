
List = {}
/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 초기화 
***************************************************/
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
                List.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //접속불가
        $('#cbtnReject').click(function (e) {
            e.preventDefault();
            List.Reject();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.Search();
        });

        List.Init();

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {

    List.LoadData("");

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
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
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchGubun = $("#ddlSearchGubun").val();
    SearchField = $("#ddlSearchField").val();

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    RecordPerPage = $("#ddlListCount").val();

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadMDMData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadMDMSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
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
            loadMDMSearchData();
        }
        else {
            loadMDMData();
        }
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Dept/List.asmx/GetPageList"

    //파라미터값 배열에 담기
    ParamArray = {
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,        
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#phListDept').setTemplateURL('/DeskPlusEIP/MDM/Jt/Dept/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phListDept').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 전체 체크박스
**********************************************************************************************/
List.Checked = function () {
    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    $("#r_loginID")[0].innerHTML = R_LoginID;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_deptName")[0].innerHTML = R_DeptName;
    $("#r_titleName")[0].innerHTML = R_Title;
    $("#r_dutyName")[0].innerHTML = R_Duty;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 카운트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetCount = function () {

    var url = "/DeskPlusEIP/MDM/Ws/Dept/List.asmx/GetListCount";
    ParamArray = {  
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,          
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 리스트 카운트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}



/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-17
작성목적 : 모바일정보 수정
***************************************************/
List.DeviceInfo = function (p_userID) {

    ParamArray = {
        'userID': p_userID,
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listPage': '/DeskPlusEIP/MDM/Dept/List.aspx'
    };

    CallEncrypt(CallGetParam());

    location.href = "../Device/DeviceInfo.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-18
작성목적 : 접속 불가 처리  (웹서비스 호출)
**********************************************************************************************/
List.Reject = function () {

    var selValue = List.CheckedValue();

    if (selValue != "") {
        if (window.confirm(R_ConfirmAccessReject)) {
            //파라미터값 배열에 담기
            ParamArray = {
                'userID': selValue,
                'accessRight': '2'
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/MDM/Ws/Dept/List.asmx/ListUpd";
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessReject");

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelectList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-18
작성목적 : 접속 불가 처리  (웹서비스 값 받기)
***************************************************/
List.SuccessReject = function (data, status) {
    if (data.d == true) {
        List.Search();     
        alert(R_CompleteConfirm);           
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };


    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


