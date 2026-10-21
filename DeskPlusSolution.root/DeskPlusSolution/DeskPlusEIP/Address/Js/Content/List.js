/// <reference path="../../../Html/Js/jQuery/jquery.js" />

List = {}

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

        $('input').placeholder(); //watertextbox

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

        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            var url = "../Admin/Default.aspx";
            parent.location.href = url;
        });

        //그룹관리
        $('#cbtnFolderManage').click(function (e) {
            e.preventDefault();
            List.FolderManage();
        });

        //그룹즐겨찾기
        $('#cbtnFavorite').click(function (e) {
            e.preventDefault();
            List.AddFavorite();
        });

        //주소추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        //주소이동
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            List.Move();
        });

        //복사
        $('#cbtnCopy').click(function (e) {
            e.preventDefault();
            List.Copy();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //주소전달
        $('#cbtnSend').click(function (e) {
            e.preventDefault();
            List.Send();
        });

        //편지쓰기
        $('#cbtnMail').click(function (e) {
            e.preventDefault();
            List.SendMail();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            List.Print();
        });

        //외부주소
        $('#cbtnOut').click(function (e) {
            e.preventDefault();
            List.InOut();
        });

        //간편주소추가
        $('#cbtnSimpleAdd').click(function (e) {
            e.preventDefault();
            List.AddSimple();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            List.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                List.Search();
            }
        });

        //리스트형 저장
        $('#listType1').click(function () {
            List.GoListType("1");
        });

        //블로그형 저장
        $('#listType2').click(function () {
            List.GoListType("2");
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            List.GoPageSize("1");
        });

        //개인주소록 추가버튼
        $('#cbtnPrivate').click(function () {
            List.AddPrivate();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Init();
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 초기화하기
***************************************************/
List.Init = function () {
    // 한국어가 아닐 경우면 외부연동, 초성검색 금지
    if (PreLang != "ko") {
        $(".roundtable_l_b").hide();
        $("#cbtnOut").hide();
    }

    //간편 주소록 기술
    $("#simpleAdd").hide();

    if (Types == "PG" && CategoryID != "0") {
        $("#simpleAdd").show();
    }

    //즐겨찾기 일 경우
    if (Types == "FG") {
        $(".board").hide();
        $(".blog").hide();
        $('#ddlListCount').hide();
    }

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    List.FilterStyle(SearchFilter); //필터 스킨 입히기
    List.LoadData("");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 형식으로 이동하기
***************************************************/
List.GoListType = function (_listType) {
    List.CategoryUser(_listType);

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    //var iQuery = iPageUrl.attr('query');

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'catID': CategoryID,
        'types': Types,
        'rTypes': RTypes,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    switch (_listType) {
        case "1":
            iPath = iPath.replace("/ListBlog.aspx", "/List.aspx");
            break;
        case "2":
            iPath = iPath.replace("/List.aspx", "/ListBlog.aspx");
            break;
    }

    CallEncrypt(CallGetParam());
    location.href = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 페이지 카운트 이동하기
***************************************************/
List.GoPageSize = function (_listType) {
    List.CategoryUser(_listType);

    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    //var iQuery = iPageUrl.attr('query');

    //이동하기
    //location = iPath + "?" + iQuery;

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'catID': CategoryID,
        'types': Types,
        'rTypes': RTypes,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 카테고리별 사용자 정보 저장하기
***************************************************/
List.CategoryUser = function (_listType) {
    //카테고리별 사용자 정보 저장하기
    var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/ModCategoryUser";
    //listType : 1 리스트; 2: 블로그
    ParamArray = {
        'categoryID': CategoryID,
        'types': Types,
        'listType': _listType,
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
List.SuccessCategoryUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 중요표시 추가 및 삭제하기
**********************************************************************************************/
List.ModImpUser = function (_listID, _arg) {
    //카테고리별 사용자 정보 저장하기
    var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/AddImpUser";

    if ($(_arg).attr("checked") == "checked") {
        $("#" + _listID).attr("checked", true);
        $(_arg).next().addClass("on");
    }
    else {
        $("#" + _listID).attr("checked", false);
        $(_arg).next().removeClass("on");
        url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/DelImpUser";
    }

    //1|GM
    ParamArray = {
        'ownerID': _listID.split("_")[2] + "|" + _listID.split("_")[3]
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModImpUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 중요표시 삭제 및 추가하기
**********************************************************************************************/
List.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
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
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        if (gubun == "search") {
            loadSearchData();
        }
        else {
            loadData();
        }
    }, 100);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : ㄱㄴㄷㄹ 검색
***************************************************/
List.SearchFilter = function (str) {
    //$("#ctbSearch").val("");
    //SearchValue = "";
    SearchFilter = str;
    List.FilterStyle(SearchFilter); //필터 스킨 입히기

    List.SearchData();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 검색 이벤트
***************************************************/
List.Search = function () {
    //SearchFilter = "";
    List.SearchData();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 검색데이터
***************************************************/
List.SearchData = function () {
    SearchOption = $("#ddlSearchOption").val();
    SearchKey = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    CurrentPage = 1; //1페이지로 이동처리

    List.GoPageLoad();

    /*
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

    List.LoadData("search");
    */
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'catID': CategoryID,
        'types': Types,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Content/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 주소록 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/GetListCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': CategoryID,
        'types': Types,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 카운트 이벤트
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
    $('.r_kyumgik').text(R_Both);
    $('.r_gubun_gm').text(R_CompanyAddress);
    $('.r_gubun_cm').text(R_CommonAddress);
    $('.r_gubun_pm').text(R_PrivateAddress);
    $('.r_gubun_rm').text(R_ForwardAddress);
    $('.r_gubun_tm').text(R_TitleAddress);
    $('.r_gubun_dm').text(R_DutyAddress);

    $("#r_name")[0].innerHTML = R_FirstName;
    $("#r_company")[0].innerHTML = R_Company;
    $("#r_mail")[0].innerHTML = R_Mail;
    $("#r_mobilePhone")[0].innerHTML = R_CellPhone;
    $("#r_officePhone")[0].innerHTML = R_CompanyTel;
    $("#r_deptName")[0].innerHTML = R_Dept;
    $("#r_titleName")[0].innerHTML = R_Title;
    $("#r_typeName")[0].innerHTML = R_Gubun;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

//개인주소저장 페이지 호출
List.AddPrivate = function () {
    var url = "/DeskPlusEIP/Address/Content/AddPrivate.aspx";
    PopupWin(900, 595, url, "-200", "", true);
}

//왼쪽 프레임 ReLoad
List.TreeReload = function () {
    ParamArray = {
        'categoryID': "0",
        'types': "PG",
        'gubun': ''
    };

    parent.frmLeft.location = "../folder/Tree.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 간단 주소 추가
***************************************************/
List.AddSimple = function () {
    var validator = new Validator($("#form1"));

    var name = $("#ctbName").val();
    var mail = $("#ctbMail").val();
    var phone = $("#ctbMobilePhone").val();

    if (name == "") {
        alert(R_InputFitstName);
        $("#ctbName").focus();
        return;
    }

    if (mail != "") {
        validator.checkEmail("ctbMail", R_NotCorrectEmailFormat, true);
    }

    if (phone != "") {
        //숫자,+,- 기호만 처리 한다.
        var pattern = new RegExp(/^[0-9-+]+$/);
        var response = pattern.test(phone);

        if (response == false) {
            alert(R_CellPhoneFormat);
            return;
        }

        //+ 을 plus 로 대체한다.(파라미터 발송시 날라감)
        phone = phone.replace(/\+/g, "plus")
    }

    if (!validator.validate()) return;

    ParamArray = {
        'categoryID': CategoryID,
        'types': Types,
        'name': name,
        'mail': mail,
        'mobile': phone
    };

    var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/AddPrivate";
    var obj = eval("List.SuccessAddPrivate");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 간편주소록 성공시 페이지 표시
**********************************************************************************************/
List.SuccessAddPrivate = function (data, status) {
    $("#ctbName").val("");
    $("#ctbMail").val("");
    $("#ctbMobilePhone").val("");

    alert(R_Complete);
    List.Search();
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 주소 추가
***************************************************/
List.Add = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'catID': CategoryID,
        'types': Types,
        'rTypes': RTypes,
        'currentPage': CurrentPage,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Address/Content/Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 주소 수정
***************************************************/
List.ModPrivate = function (type, categoryID, keyID) {
    ParamArray = {
        'categoryID': categoryID,
        'catID': CategoryID,
        'types': type,
        'rTypes': RTypes,
        'keyID': keyID,
        'currentPage': CurrentPage,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var goUrl = "";
    if (type == "PG") {
        goUrl = "/DeskPlusEIP/Address/Content/Add.aspx";
    }
    else if (type == "CG") {
        goUrl = "/DeskPlusEIP/Address/Content/ViewCommon.aspx";
    }

    location.href = goUrl + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 외부주소
***************************************************/
List.InOut = function () {
    location.href = "/DeskPlusEIP/Address/Outlook/InOutStep1.aspx";
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 주소전달
***************************************************/
List.Send = function () {
    var selValue = List.CheckedValue();
    
    if (selValue != "") {
        ParamArray = {
            'listID': selValue
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Address/Content/Send.aspx?p=" + GetEncryptValue;
        PopupWin(800, 497, url, "-500", "", true);
    }
    else {
        alert(R_SelectSendAddress);
    }
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 주소록 인쇄
***************************************************/
List.Print = function () {
    var selVal = List.CheckedValue()
    ParamArray = {
        'keyID': selVal,
        'types': Types
    };
    //    alert(selVal);
    if (selVal == "") {
        alert(R_SelectPintUser);
    }
    else {
        CallEncrypt(CallGetParam());
        //parent.location.href = "../Content/Print.aspx?p=" + GetEncryptValue;
        location.href = "../Content/Print.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 그룹즐겨찾기 추가
***************************************************/
List.AddFavorite = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Content/FavoriteFolder.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 그룹관리
***************************************************/
List.FolderManage = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Content/UserFolder.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 복사
***************************************************/
List.Copy = function () {
    var selValue = List.CheckedValue();

    if (selValue != "") {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': selValue,
            'types': Types,
            'gubun': "COPY"
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Address/Content/MoveCopy.aspx?p=" + GetEncryptValue;
        PopupWin(400, 400, url, "-200", "", true);
    }
    else {
        alert(R_SelectCopyAddress);
    }
}

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 이동하기
***************************************************/
List.Move = function () {
    var selValue = List.CheckedValue();

    if (selValue != "") {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': selValue,
            'types': Types,
            'gubun': "MOVE"
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Address/Content/MoveCopy.aspx?p=" + GetEncryptValue;
        PopupWin(400, 400, url, "-200", "", true);
    } else {
        alert(R_SelectMoveAddress);
    }
}

/**************************************************
삭제
***************************************************/
List.Del = function () {
    var selValue = List.CheckedValue();

    if (selValue != "") {
        //파라미터값 배열에 담기
        ParamArray = {
            'categoryID': CategoryID,
            'listID': selValue
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Address/Ws/Content/List.asmx/DelPrivate";
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("List.SuccessDel");

        AjaxService(url, data, obj);
    } else {
        alert(R_SelectDeleteAddress);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteDeleteAddress);

        CurrentPage = 1; //1페이지로 이동처리
        List.GoPageLoad();

        /*
        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData("search");
            }
        }

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

        List.LoadData("search");
        */
    }
    else {
        alert(R_AfterProcessingError);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 주소록 내보내기
***************************************************/
List.Excel = function () {
    //파라미터값 배열에 담기
    ParamArray = {
    };

    CallEncrypt(CallGetParam());

    location.href = "MakeCSV.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 필터 스타일 적용하기
***************************************************/
List.FilterStyle = function (idx) {
    $('span').removeClass("txt_point");

    switch (idx) {
        case "ㄱ":
            $('#index1').addClass("txt_point");
            break;
        case "ㄴ":
            $('#index2').addClass("txt_point");
            break;
        case "ㄷ":
            $('#index3').addClass("txt_point");
            break;
        case "ㄹ":
            $('#index4').addClass("txt_point");
            break;
        case "ㅁ":
            $('#index5').addClass("txt_point");
            break;
        case "ㅂ":
            $('#index6').addClass("txt_point");
            break;
        case "ㅅ":
            $('#index7').addClass("txt_point");
            break;
        case "ㅇ":
            $('#index8').addClass("txt_point");
            break;
        case "ㅈ":
            $('#index9').addClass("txt_point");
            break;
        case "ㅊ":
            $('#index10').addClass("txt_point");
            break;
        case "ㅋ":
            $('#index11').addClass("txt_point");
            break;
        case "ㅌ":
            $('#index12').addClass("txt_point");
            break;
        case "ㅍ":
            $('#index13').addClass("txt_point");
            break;
        case "ㅎ":
            $('#index14').addClass("txt_point");
            break;
        case "A":
            $('#index15').addClass("txt_point");
            break;
        case "1":
            $('#index16').addClass("txt_point");
            break;
        default:
            $('#index0').addClass("txt_point");
            break;
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 편지쓰기
***************************************************/
List.SendMail = function () {
    var toMail = "", toDeep = "";
    var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    var allVals = $('.cbxID:checked').map(function () {
        if (email_regex.test($(this).attr('email'))) {
            toMail += $(this).attr('email') + ";";
            toDeep += $(this).attr('email') + ",";
        }
    }).get();

    if (toMail == "") { alert(R_NoMailFormat); return; }

    if (MailProduct.toUpperCase().indexOf("DEEP") < 0) {
        //zetta
        ParamArray = { 'toMail': toMail };
        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Common/Mail/ZettaMail65/WriteMail.aspx?p=" + GetEncryptValue;
        window.open(url, "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    } else {
        //deep
        ParamArray = { 'to': toDeep };
        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Common/Mail/Deep/DeepWrite.aspx?p=" + GetEncryptValue;
        window.open(url, "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-02-19
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'catID': CategoryID,
        'types': Types,
        'rTypes': RTypes,
        'currentPage': CurrentPage,
        'searchOption': SearchOption,
        'searchKey': SearchKey,
        'searchValue': SearchValue,
        'searchFilter': SearchFilter,
        'order': Order
    };


    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}


