
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

        //######### 부서 관련


        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();

            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });

        //우편번호
        $('#cbtnPost').click(function (e) {
            e.preventDefault();
            List.CallZipCode();
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

        //상위부서변경
        $('#cbtnChangeParentDept').click(function (e) {
            e.preventDefault();
            List.MoveParentDept();

        });

        //소속원 일괄관리
        $("#cbtnModUser").click(function (e) {
            e.preventDefault();
            List.ModUser();

        });

        $('#ctbMuliLang').attr("disabled", true);
        $('#ctbParentName').attr("disabled", true);

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
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/admin/Dept/JTList.htm?seq=' + Guids(), null, { filter_data: false });
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





/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 부서 변경
***************************************************/
List.MoveParentDept = function () {

    if (DeptID != "0") {
        //파라미터값 배열에 담기    
        ParamArray = {
            'deptID': DeptID
        };

        CallEncrypt(CallGetParam());
        var url = "Move.aspx?p=" + GetEncryptValue;

        PopupWin(400, 400, url, "-100", "", true);
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 부서 추가
***************************************************/
List.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbDeptName", R_RegisterDeptName, true);
    if (!validator.validate()) return;

    var re = /[<>#{}']/gi;
    if (re.test($("#ctbDeptName").val())) {
        alert(R_SpecialChar + "( < > # { } )");
        $("#ctbDeptName").focus();
        return;
    }

    $('#ctbMuliLang').attr("disabled", false);    
    __doPostBack('lbtnRegister', '');
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 부서 추가
***************************************************/
List.Del = function () {

    if (DeptID != "") {
        if (window.confirm(R_DeleteDept)) {
            __doPostBack('lbtnRegister', 'del');
        }
    }
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 페이지 호출
***************************************************/
List.CallZipCode = function () {

    new daum.Postcode({
        oncomplete: function (data) {
            // 팝업에서 검색결과 항목을 클릭했을때 실행할 코드를 작성하는 부분.

            // 도로명 주소의 노출 규칙에 따라 주소를 조합한다.
            // 내려오는 변수가 값이 없는 경우엔 공백('')값을 가지므로, 이를 참고하여 분기 한다.
            var fullRoadAddr = data.roadAddress; // 도로명 주소 변수
            var extraRoadAddr = ''; // 도로명 조합형 주소 변수

            // 법정동명이 있을 경우 추가한다.
            if (data.bname !== '') {
                extraRoadAddr += data.bname;
            }
            // 건물명이 있을 경우 추가한다.
            if (data.buildingName !== '') {
                extraRoadAddr += (extraRoadAddr !== '' ? ', ' + data.buildingName : data.buildingName);
            }
            // 도로명, 지번 조합형 주소가 있을 경우, 괄호까지 추가한 최종 문자열을 만든다.
            if (extraRoadAddr !== '') {
                extraRoadAddr = ' (' + extraRoadAddr + ')';
            }
            // 도로명, 지번 주소의 유무에 따라 해당 조합형 주소를 추가한다.
            if (fullRoadAddr !== '') {
                fullRoadAddr += extraRoadAddr;
            }

            //var zipcode = data.postcode1 + "" + data.postcode2;
            var zipcode = data.zonecode;

            $("#ctbZipCode").val(zipcode);
            $("#ctbAddress1").val(fullRoadAddr);
            $("#ctbAddress2").val("");
            $("#ctbAddress2").focus();
        }
    }).open();

    return;

    //파라미터값 배열에 담기    
    ParamArray = {
        'callBackPage': "GetZipCode"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Common/ZipCode/ZipCode.aspx?p=" + GetEncryptValue;
    PopupWin(600, 435, url, "-100", "", true);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 우편번호 설정하기
***************************************************/
function GetZipCode(zipcode, ido, si, dong) {
    ido = decodeURIComponent(ido);
    si = decodeURIComponent(si);
    dong = decodeURIComponent(dong);

    $("#ctbZipCode").val(zipcode);
    $("#ctbAddress1").val(ido + " " + si + " " + dong);
    $("#ctbAddress2").val();
    $("#ctbAddress2").focus();
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
작성자   : shimpark
작성목적 : 부서 순서 변경하기
***************************************************/
List.MoveUpdown = function () {

    ParamArray = {        
        'deptID': DeptID
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);
}

/**************************************************
작성자   : 
작성목적 : 부서원 일괄 관리
***************************************************/
List.ModUser = function () {

    ParamArray = {
        'deptID': DeptID
    };

    CallEncrypt(CallGetParam());
    var url = "ModUser.aspx?p=" + GetEncryptValue;
    PopupWin(700, 600, url, "-500", "", true);
}

/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
List.GoTree = function () {
    ParamArray = {        
        'deptID': DeptID
    };

    CallEncrypt(CallGetParam());
    var url = "Tree.aspx?p=" + GetEncryptValue;

    parent.frmFolderLeft.location.href = url;
}