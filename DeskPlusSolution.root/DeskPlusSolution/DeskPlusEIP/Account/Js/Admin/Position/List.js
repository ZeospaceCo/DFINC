
List = {}


/**************************************************
초기화
***************************************************/
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            List.LoadPageData("");
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
                List.SearchPage();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.SearchPage();
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.PageSize();
        });

        List.LoadPageData("");


        //######### 직급,직책 관련

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });

        //등록하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Confirm();
        });

        //삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //폴더 순서 변경
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            List.MoveUpdown();

        });

        if (Scope == "D") {
            $('#trAccess').hide();
        }
        else {
            $('#trAccess').show();
        }

        $('#ctbMuliLang').attr("disabled", true);

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이징 리스트 초기화
***************************************************/
List.LoadPageData = function (gubun) {

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadAccountData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadAccountSearchData = function () {
        List.GetPageCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 검색용 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadPageData("search");
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
    
    CurrentPage = 1; //1페이지로 이동처리  

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');

    ParamArray = {
        'codeID': CodeID,
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
List.SearchPage = function () {

    SearchGubun = $("#ddlSearchGubun").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    CurrentPage = 1; //1페이지로 이동처리  

    List.LoadPageData("search");
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 인사 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {


    var url = "/DeskPlusEIP/Account/Ws/Admin/Position/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'codeID': CodeID,
        'activeUser': '0',
        'totalCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
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
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/admin/Position/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 인사 카운트  호출
**********************************************************************************************/
List.GetPageCount = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/Position/List.asmx/GetListCount";

    ParamArray = {
        'companyID': CompanyID,
        'codeID': CodeID,
        'activeUser': '0',
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 사용자 카운트 이벤트
**********************************************************************************************/
List.SuccessPageCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
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

    List.SearchPage();
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
    $("#r_titlename")[0].innerHTML = R_Private_AccountInfo_ltTitle;     //"직급";
    $("#r_dutyname")[0].innerHTML = R_Private_AccountInfo_ltDuty;       //"직책";
    $("#r_active")[0].innerHTML = R_Admin_DeptManager_cbActive;         // 활성화

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 직급 추가
***************************************************/
List.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_RegisterTitleName, true);

    //validator.checkRequired("ctbFamilyName", R_InputFamilyName, true);
    //validator.checkRequired("ctbMail", R_InputEmail, true);
    //validator.checkEmail("ctbMail", R_NotEmailFormat, true);

    if (!validator.validate()) return;

    if (!ExistSpecialChar($("#ctbName").val())) {

        alert(R_SpecialChar + "( < > ( ) # & { } )");
        $("#ctbName").focus();
        return;
    }

    $('#ctbMuliLang').attr("disabled", false);

    if (Scope != "D") {
        var codeID = $("#ddlAccessTitle").val();

        if (codeID == "" || codeID == "0") { alert(R_AccessGrades + " " + R_MSG_SELITEM); return; }
    }

    __doPostBack('lbtnRegister', '');
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 직급 추가
***************************************************/
List.Del = function () {

    if (window.confirm(R_DeleteTitle)) {
        __doPostBack('lbtnRegister', 'del');
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMuliLang").val(multiNames);
}


/**************************************************
작성자   : shimpark
작성목적 : 순서 변경하기
***************************************************/
List.MoveUpdown = function () {

    ParamArray = {
        'codeID': CodeID
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}


/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
List.GoTree = function () {
    ParamArray = {
        'codeID': CodeID
    };

    CallEncrypt(CallGetParam());
    var url = "Tree.aspx?p=" + GetEncryptValue;

    parent.frmLeft.location.href = url;
}