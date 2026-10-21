
List = {}

/**************************************************
초기화
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


        //폴더 추가
        $('#cbtnCatAdd').click(function (e) {
            e.preventDefault();
            List.CatAdd();
        });

        //코드 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });


        //순서변경
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            List.MoveUpdown();

        });

        //활성화
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            List.Active();
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

        List.LoadData("");
    });
});


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
        'sysCodeID': SYSCodeID,
        'parentCodeID': ParentCodeID,
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
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/List.asmx/GetSysCodePageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'parentCodeID': ParentCodeID,
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
    
    $('#placeholder').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Code/JTList.htm?seq=' + Guids(), null, { filter_data: false });
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
    //List.LoadData("search");
    List.GoPageLoad();
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/List.asmx/GetSysCodeCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'parentCodeID': ParentCodeID,
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
    $("#r_groupkey")[0].innerHTML = R_GroupKey;
    $("#r_code")[0].innerHTML = R_CodeKey;
    $("#r_codename")[0].innerHTML = R_CodeName;
    $("#r_interfacecode")[0].innerHTML = R_InterfaceCode;
    $("#r_active")[0].innerHTML = R_Active;

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
        'companyID': CompanyID,
        'sysCodeID': SYSCodeID,
        'parentCodeID': ParentCodeID
    };    

    CallEncrypt(CallGetParam());
    location = "Add.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 폴더 추가
**********************************************************************************************/
List.CatAdd = function () {

    ParamArray = {
        'companyID': CompanyID,
        'sysCodeID': SYSCodeID,
        'codeID': ParentCodeID
    };

    CallEncrypt(CallGetParam());
    location = "AddCategory.aspx?p=" + GetEncryptValue;
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
List.Del = function () {

    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(R_CheckDeleteList);
        return;
    }

    if (window.confirm(R_ConfirmDelete)) {
        
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/List.asmx/DelSysCode";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'sysCodeList': selValue
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
    if (data.d == true) {
        List.Search();
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
    
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Code/List.asmx/ActiveSysCode";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'sysCodeList': selValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessDel");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
List.MoveUpdown = function () {

    ParamArray = {
        'parentCodeID': ParentCodeID
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 382, url, "", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 보기
***************************************************/
List.View = function (p_SysCodeID, p_CodeID) {
    ParamArray = {
        'codeID': p_CodeID,
        'sysCodeID': p_SysCodeID,
        'parentCodeID': ParentCodeID,
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "Add.aspx?p=" + GetEncryptValue;
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

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'parentCodeID': ParentCodeID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


