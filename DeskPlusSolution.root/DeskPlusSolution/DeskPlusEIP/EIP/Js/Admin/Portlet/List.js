
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


        //탭포틀릿 추가
        $('#cbtnAddTap').click(function (e) {
            e.preventDefault();
            List.AddTap();
        });

        //포틀릿 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //활성화
        $('#cbtnActive').click(function (e) {
            e.preventDefault();
            List.Active();
        });

        // 그룹변경
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            List.ModGroup();
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

        //타회사 포틀릿 추가하기
        $('#cbtnMultiCompany').click(function (e) {
            e.preventDefault();
            List.AddMultiCompany();
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
    RecordPerPage = $('#ddlListCount').val();
    List.Search();
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 :  리스트 호출
**********************************************************************************************/
List.GetPageList = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/List.asmx/GetPortletPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
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
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTList.htm?seq=' + Guids(), null, { filter_data: false });
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
작성목적 : 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/List.asmx/GetPortletPageCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
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
    $("#r_catname")[0].innerHTML = R_GroupName;
    $("#r_name")[0].innerHTML = R_PortletName;
    $("#r_uitype")[0].innerHTML = R_UIFormat;
    $("#r_active")[0].innerHTML = R_Active;
    $("#r_portlettype")[0].innerHTML = R_Section;

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
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'recordPerPage': $('#ddlListCount').val(),
        'order': Order,
        'searchField': SearchField,
        'searchValue': SearchValue
    };    

    CallEncrypt(CallGetParam());
    location = "Add.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭용추가 추가
**********************************************************************************************/
List.AddTap = function () {

    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'recordPerPage': $('#ddlListCount').val(),
        'order': Order,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    location = "AddTab.aspx?p=" + GetEncryptValue;
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
List.Del = function () {

    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(CheckDeleteList);
        return;
    }

    if (window.confirm(R_ConfirmDelete)) {
        
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/List.asmx/DelPortlet";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'categoryID': CategoryID,
            'portletList': selValue
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

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/List.asmx/ActivePortlet";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
        'portletList': selValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessDel");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 그룹변경
**********************************************************************************************/
List.ModGroup = function () {
    var selValue = List.CheckedValue();

    if (selValue == "") {
        alert(R_CheckList);
        return;
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'portletID': selValue
    };


    CallEncrypt(CallGetParam());
    var url = "../Portlet/Move.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-100", "", true);
 }


/**************************************************
작성자   : shimpark
작성목적 : 보기
***************************************************/
List.View = function (p_id, p_type) {

    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
        'portletID': p_id,
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    if (p_type == "P") {
        location = "Mod.aspx?p=" + GetEncryptValue;
    } else if (p_type == "T") {
        location = "ModTab.aspx?p=" + GetEncryptValue;
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

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 타회사 포틀릿 리스트 호출하기
***************************************************/
List.AddMultiCompany = function () {

    ParamArray = {
        'companyID': CompanyID,
        'fromCategoryID': CategoryID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/EIP/Admin/Portlet/CopyPortlet/Default.aspx?p=" + GetEncryptValue;

    window.open(url, "AddMultiCompany", "width=1000px,height=400px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-22
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


