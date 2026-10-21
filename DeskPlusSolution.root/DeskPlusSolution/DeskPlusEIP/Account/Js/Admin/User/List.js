List = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
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


        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });


        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            List.Modify();
        });


        //활성,비활성화
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            List.Active();
        });

        //부서 변경
        $('#cbtnModDept').click(function (e) {
            e.preventDefault();
            List.ModDept();
        });

        //사용자순서 변경
        $('#cbtnMoveUserDeptOrder').click(function (e) {
            e.preventDefault();
            List.MoveUserDeptOrder();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.PageSize();
        });


        //리스트형 저장
        $('#listType1').click(function () {
            List.GoListType("1");
        });

        //블로그형 저장
        $('#listType2').click(function () {
            List.GoListType("2");
        });

        List.Init("");

        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
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

    List.GetLicenseCount();
    List.LoadData("");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchGubun = $("#ddlSearchGubun").val();
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 형식으로 이동하기
***************************************************/
List.GoListType = function (_listType) {

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    var iQuery = iPageUrl.attr('query');

    switch (_listType) {
        case "1":
            iPath = iPath.replace("/ListBlog.aspx", "/List.aspx");
            break;
        case "2":
            iPath = iPath.replace("/List.aspx", "/ListBlog.aspx");
            break;
    }

    //이동하기    
    location = iPath + "?" + iQuery;

}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadAccountData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadAccountSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 검색용 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
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
List.PageSize = function () {

    var response = List.SettingSearch();
    if (response == false) return;
    CurrentPage = 1; //1페이지로 이동처리  

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'deptID': DeptID,
        'activeUser': '0',
        'totalCount': TotalItemCount,
        'recordPerPage': $('#ddlListCount').val(),
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}






/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 인사 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,
        'activeUser': '0',
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 인사 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/admin/User/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {
    $("#r_login")[0].innerHTML = R_LoginID;                             // "아이디";
    $("#r_name")[0].innerHTML = R_Admin_UserList_MSG_HTName;            //"이름";
    $("#r_mail")[0].innerHTML = R_MailAddress;                          //"메일주소";
    $("#r_phone")[0].innerHTML = R_Private_AccountInfo_ltMobile;        // "휴대폰";
    $("#r_deptname")[0].innerHTML = R_Private_AccountInfo_ltUserDept;   // "부서";
    $("#r_titlename")[0].innerHTML = R_Private_AccountInfo_ltTitle;      //"직급";
    $("#r_dutyname")[0].innerHTML = R_Private_AccountInfo_ltDuty;        //"직책";
    $("#r_active")[0].innerHTML = R_Admin_DeptManager_cbActive;
    $("#r_basedept")[0].innerHTML = R_Both;
    $("#r_UserDeptOrder")[0].innerHTML = R_UserDeptOrder;        //"순서";

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 인사 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/GetListCount";

    ParamArray = {
        'companyID': CompanyID,
        'deptID': (SearchGubun == "Whole") ? "0" : DeptID,
        'activeUser': '0',
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
작성목적 : 성공시 , 사용자 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
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

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
List.Sort = function (col) {

    var response = List.SettingSearch();
    if (response == false) return;

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;

    //List.LoadData("search");
    List.GoPageLoad();
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 사용자 추가
***************************************************/
List.Add = function () {

    ParamArray = {
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order,
        'listPage': 'List.aspx'
    };

    CallEncrypt(CallGetParam());
    location.href = "Add.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 사용자 수정
***************************************************/
List.Mod = function (p_userID) {

    ParamArray = {
        'userID': p_userID,
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order,
        'listPage': 'List.aspx'
    };

    CallEncrypt(CallGetParam());

    if (parseInt(UserCnt) > LicenseCnt) { //system user
        alert(R_LicenseError);
    } else {
        location.href = "Mod.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 사용자 삭제
***************************************************/
List.Del = function () {
    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(R_CheckDeleteList);
        return;
    }

    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/DelUser";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'userList': selValue
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessDel");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이벤트
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d == "TRUE") {
        List.Search();
    }
    else if (data.d == "-1") {
        //근태 메뉴 사용일때 퇴사일이 없으면 삭제 불가
        alert(R_Admin_User_Input_RetireDate);
    }
    else {
        alert(R_ErrorProcessing);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 활성화 여부
**********************************************************************************************/
List.Active = function () {
    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(R_CheckList);
        return;
    }

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/ModActive";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userList': selValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessDel");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
List.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   : 
작성일자 : 2012-02-02
작성목적 : 사용자의 기본부서변경
***************************************************/
List.ModDept = function () {

    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(R_CheckChangeDeptUserList);
        return;
    }

    ParamArray = {
        'userList': selValue
    };

    CallEncrypt(CallGetParam());
    var url = "../User/Move.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-100", "", true);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 사용자 수정
***************************************************/
List.Modify = function () {

    var userDeptOrderList = "";

    $('.cbxID:checked').map(function () {
        var obj = $("#UserDeptOrder_" + $(this).val());
        userDeptOrderList += obj.attr('userID') + "_" + obj.attr('deptID') + "_" + obj.val() + ",";
    });

    if (userDeptOrderList == "") {
        alert(R_CheckList);
        return;
    }

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/ModUserDeptOrder";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userDeptOrderList': userDeptOrderList
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModify");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 2012-02-02
작성목적 : 사용자순서변경
***************************************************/
List.MoveUserDeptOrder = function () {

    if (DeptID == "0") {
        alert(R_Admin_UserManager_MSG_NotSelectRootNode);
        return;
    }

    ParamArray = {
        'deptID': DeptID
    };

    CallEncrypt(CallGetParam());
    var url = "UserUpDown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 382, url, "", "", true);
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 사용자 수정
***************************************************/
List.Modify = function () {

    var userDeptOrderList = "";

    $('.cbxID:checked').map(function () {
        var obj = $("#UserDeptOrder_" + $(this).val());
        userDeptOrderList += obj.attr('userID') + "_" + obj.attr('deptID') + "_" + obj.val() + ",";
    });

    if (userDeptOrderList == "") {
        alert(R_CheckList);
        return;
    }

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/ModUserDeptOrder";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userDeptOrderList': userDeptOrderList
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModify");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 사용자 수정
**********************************************************************************************/
List.SuccessModify = function (data, status) {
    if (data.d == true) {
        alert(R_SUCCESS);
        List.Search();
    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : 라이센스 개수
**********************************************************************************************/
List.GetLicenseCount = function () {
    
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/List.asmx/GetCompanyLicenseCount";

    //파라미터값 배열에 담기
    ParamArray = {
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessGetLicenseCount");

    AjaxService(url, data, obj);
}



/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 성공시  라이센스 개수
**********************************************************************************************/
List.SuccessGetLicenseCount = function (data, status) {
    LicenseCnt = parseInt(data.d + 1);
    $("#licenseCnt").text(LicenseCnt);

    if (parseInt(UserCnt) > LicenseCnt) { //system user
        $("#spUserCnt").css({ "color": "red", "font-style": "bold" });
        $("#cbtnAdd").hide();
        $("#cbtnMod").hide();
        $("#cbtnModDept").hide();
        $("#cbtnMoveUserDeptOrder").hide();
    } else if (parseInt(UserCnt) == LicenseCnt) {        
        $("#cbtnAdd").hide();
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
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


