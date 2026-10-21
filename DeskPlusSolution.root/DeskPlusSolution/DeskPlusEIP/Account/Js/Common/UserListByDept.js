
UserListByDept = {}

/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            UserListByDept.LoadPageData("");
        }
    }

    $(document).ready(function () {
        //페이징 핸들러 정의
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

        if (SearchValue != "") {
            $("#ctbSearch").val(SearchValue.securityText());
        }

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                UserListByDept.SearchPage();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            UserListByDept.SearchPage();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            UserListByDept.PageSize();
        });

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            UserListByDept.Confirm();
        });

        // 닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        UserListByDept.LoadPageData("");

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});




/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이징 리스트 초기화
***************************************************/
UserListByDept.LoadPageData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadAccountData = function () {
        UserListByDept.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadAccountSearchData = function () {
        UserListByDept.GetPageCount(); //검색 총 카운트 가져오기
        UserListByDept.GetPageList(); //검색 리스트 가져오기

        // 검색용 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                UserListByDept.LoadPageData("search");
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
            loadAccountSearchData();
        }
        else {
            loadAccountData();
        }
    }, 100);

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
UserListByDept.PageSize = function () {

    CurrentPage = 1; //1페이지로 이동처리  

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'deptID': DeptID,
        'activeUser': '0',
        'totalCount': TotalItemCount,
        'recordPerPage': $('#ddlListCount').val(),
        'currentPage': CurrentPage,        
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
UserListByDept.SearchPage = function () {

    SearchGubun = $("#ddlSearchGubun").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    CurrentPage = 1; //1페이지로 이동처리  

    UserListByDept.LoadPageData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 인사 리스트 호출
**********************************************************************************************/
UserListByDept.GetPageList = function () {

    var url = "/DeskPlusEIP/Account/Ws/Admin/Dept/List.asmx/GetPageList";
    SearchGubun = $("#ddlSearchGubun").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,
        'activeUser': '0',
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("UserListByDept.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 인사 리스트 표시
**********************************************************************************************/
UserListByDept.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/Common/JTUserListByDept.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    UserListByDept.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 인사 카운트  호출
**********************************************************************************************/
UserListByDept.GetPageCount = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/Dept/List.asmx/GetListCount";
   
    ParamArray = {
        'companyID': CompanyID,
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,
        'activeUser': '0',
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("UserListByDept.SuccessPageCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 사용자 카운트 이벤트
**********************************************************************************************/
UserListByDept.SuccessPageCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
UserListByDept.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    UserListByDept.SearchPage();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
UserListByDept.Resource = function () {
    
    $("#r_name")[0].innerHTML = R_Admin_UserList_MSG_HTName;            //"이름";    
    $("#r_deptname")[0].innerHTML = R_Private_AccountInfo_ltUserDept;   // "부서";
    $("#r_titlename")[0].innerHTML = R_Private_AccountInfo_ltTitle;     //"직급";
    $("#r_dutyname")[0].innerHTML = R_Private_AccountInfo_ltDuty;       //"직책";
    $("#r_active")[0].innerHTML = R_Admin_DeptManager_cbActive;         // 활성화

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
UserListByDept.Checked = function () {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
UserListByDept.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-05-08
작성목적 : 확인
**********************************************************************************************/
UserListByDept.Confirm = function () {

    //키|구분자,키|구분자
    var selValue = UserListByDept.CheckedValue();

    if (selValue != "") {
        
        if (Callback != "") {
            var obj = eval("parent." + Callback);
            if (obj) {
                obj(escape(selValue));
            }
        }
        else {
            
            parent.GetUserDept(escape(selValue));
        }

        parent.disablePopup();
    }
}
