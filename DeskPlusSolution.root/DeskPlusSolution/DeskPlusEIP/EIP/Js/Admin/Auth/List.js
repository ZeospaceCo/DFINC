
List = {}

List.DelTotal = 0;
List.DelDone = 0;
List.DelError = false;
/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadData("");
        }
    }

    $(document).ready(function () {

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
        

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.PageSize();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        if (window.sessionStorage && sessionStorage.getItem("EIP_AUTH_APPLY_MENU") == "Y") {
            sessionStorage.removeItem("EIP_AUTH_APPLY_MENU");
            List.ApplyMenuAuth();
        }

        List.LoadData("");
    });
});


List.ApplyMenuAuth = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Tree.asmx/ResetMenuList";

    ParamArray = {
        'companyID': CompanyID,
        'userID': "0"
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, function (data, status) {
        if (data.d != true) {
            alert(R_ErrorProcessing);
        }
    });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
    
    var getData = function () {
        List.GetPageList();
    }

    var getSearchData = function () {
        List.GetCount();
        List.GetPageList();

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

    setTimeout(function () {
        if (gubun == "search") {
            getSearchData();
            
        }
        else {
            getData();
        }
    }, 100);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
List.PageSize = function () {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'companyID': CompanyID,
        'roleID': RoleID,
        'gubun': Gubun,
        'recordPerPage': $('#ddlListCount').val(),        
        'order': Order,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = iPath + "?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 회사 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/List.asmx/GetRoleUserPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'roleID': RoleID,
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 회사 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Auth/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    List.Resource();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {

    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();

    CurrentPage = 1; //1페이지로 이동처리
    List.LoadData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/List.asmx/GetRoleUserPageCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'roleID': RoleID,
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
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {
    $("#r_rolecode")[0].innerHTML = R_AuthCode;
    $("#r_rolename")[0].innerHTML = R_AuthName;
    $("#r_name")[0].innerHTML = R_UserNameDeptName;
    $("#r_types")[0].innerHTML = R_Section;

    if ($(".r_user").text() != undefined) {
        $(".r_user").text(R_Users);
    }

    if ($(".r_dept").text() != undefined) {
        $(".r_dept").text(R_Dept);
    }

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 정렬
**********************************************************************************************/
List.Sort = function (col) {

    var colValue = col + " asc";

    //메뉴를 동일한 필드에서 두번이상 선택했을 경우
    if (Order == colValue) {
        colValue = col + " desc";
    }

    Order = colValue;
    CurrentPage = 1;


    List.LoadData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 추가
**********************************************************************************************/
List.Add = function () {

    ParamArray = {
        'roleID': RoleID,
        'gubun': Gubun,
        'currentPage': CurrentPage,
        'recordPerPage': $('#ddlListCount').val(),        
        'order': Order,
        'searchField': SearchField,
        'searchValue': SearchValue
    };    

    CallEncrypt(CallGetParam());
    location = "Mod.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
List.Del = function () {    

    var selValue = List.CheckedValue();    

    if (selValue.length == 0) {
        alert(R_CheckDeleteList);
        return;
    }

    if (window.confirm(R_ConfirmDelete)) {

        List.DelTotal = selValue.length;
        List.DelDone = 0;
        List.DelError = false;

        for(var i=0; i<selValue.length;i++)
        {
            List.DelProc(selValue[i].split("_")[0], selValue[i].split("_")[1], selValue[i].split("_")[2], selValue[i].split("_")[3]);
        }

        //List.Search();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
List.DelProc = function (p_companyID, p_roleID, p_userID, p_Types) {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Auth/List.asmx/DelRoleUser";
    

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': p_companyID,
        'roleID': p_roleID,
        'userID': p_userID,
        'types': p_Types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessDel");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 이벤트
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    List.DelDone += 1;

    if (data.d != true) {
        List.DelError = true;
        alert(R_ErrorProcessing);
    }

    if (List.DelDone >= List.DelTotal) {
        if (!List.DelError) {
            List.ApplyMenuAuth();
        }

        List.Search();
    }
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
