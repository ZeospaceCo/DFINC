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

        //$('a[title]').qtip(); //tooltip shimpark 2015.10.19

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

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //리스트형 저장
        $('#listType1').click(function () {
            List.ModCategoryUser("1");
            List.GoPageListType("1");
        });

        //블로그형 저장
        $('#listType2').click(function () {
            List.ModCategoryUser("2");
            List.GoPageListType("2");
        });

        //갤러리형 저장
        $('#listType3').click(function () {
            List.ModCategoryUser("3");
            List.GoPageListType("3");
        });

        //갤러리형 카운트
        $('#ddlListCount').change(function () {
            List.ModCategoryUser("3");
            List.GoPageListType("3");
        });

        $("#zeoModalContact").draggable({ disabled: false });

        List.Init();

        //드롭다운박스 초기화
        List.SearchField();

        //검색필드
        $('#ddlSearchField').change(function () {
            List.SearchField();
        });

        //관심대상자추가
        $('#cbtnInterested').click(function () {
            List.AddnInterested();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 초기화 하기
***************************************************/
List.Init = function () {
    $("#startEndDate").hide(); //기간

    if (SearchTerm == "PE") {
        $("#startEndDate").show(); //기간
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    List.LoadData("");
    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 등록일자 활성화 여부
***************************************************/
List.RegDate = function () {
    //기간 선택
    var sField = $("#ddlSearchTerm").val();
    if (sField == "PE") {
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
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 카테고리별 사용자 설정 정보 저장하기
***************************************************/
List.ModCategoryUser = function (gubun) {
    var url = "/DeskPlusEIP/Board/Ws/Content/ListInterested.asmx/ModCategoryUser";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'catID': '0',
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': 'IP'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
List.SuccessModCategoryUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
List.GoPageListType = function (gubun) {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    var iQuery = iPageUrl.attr('query');

    switch (gubun) {
        case "1":
            iPath = iPath.replace("/BlogListInterested.aspx", "/ListInterested.aspx");
            iPath = iPath.replace("/GalleryListInterested.aspx", "/ListInterested.aspx");
            break;
        case "2":
            iPath = iPath.replace("/ListInterested.aspx", "/BlogListInterested.aspx");
            iPath = iPath.replace("/GalleryListInterested.aspx", "/BlogListInterested.aspx");
            break;
        case "3":
            iPath = iPath.replace("/ListInterested.aspx", "/GalleryListInterested.aspx");
            iPath = iPath.replace("/BlogListInterested.aspx", "/GalleryListInterested.aspx");
            break;
        default:
            iPath = iPath.replace("/BlogListInterested.aspx", "/ListInterested.aspx");
            iPath = iPath.replace("/GalleryListInterested.aspx", "/ListInterested.aspx");
            break;
    }

    var response = List.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'interestedUserID': InterestedUserID,
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

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        List.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
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
        if (gubun == "search") {
            loadBoardSearchData();
        }
        else {
            loadBoardData();
        }
    }, 100);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

    if (SearchTerm == "PE") {
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

    //두글자이상 검색
    if (SearchValue.length != 0 && SearchValue.length < 2) {
        alert(R_LengthOver);
        return false;
    } 

    return true;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
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
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Board/Ws/Content/ListInterested.asmx/GetInterestedPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'categoryID': CategoryID,
        'interestedUserID': InterestedUserID,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
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
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTGAlleryListInterested.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    $('#loading').html("");

    List.Resource();
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    $(".s_notice").text(R_Notice);
    $(".s_common").text(R_Common);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Content/GalleryListInterested.asmx/GetInterestedListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'categoryID': CategoryID,
        'interestedUserID': InterestedUserID,
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
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-27
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

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 게시물 보기
***************************************************/
List.View = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': CatID,
        'categoryID': p_categoryID,
        'interestedUserID': InterestedUserID,
        'listID': p_listID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "ViewInterested.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 새창으로 게시물 보기
***************************************************/
List.PopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': CatID,
        'categoryID': p_categoryID,
        'listID': p_listID,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'popup': '1'
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 이미지 보기
***************************************************/
List.ShowImage = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/GallerySubList.aspx";
    var parm = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, parm, "ShowImage", 800, 600);
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 새창으로 댓글리스트 보기
***************************************************/
List.PopupShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 조회현황
***************************************************/
List.ReadList = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}


/**********************************************************************************************
작성자   : 이지원
작성일자 : 2017-03-27
작성목적 : 페이지이동 처리
**********************************************************************************************/
List.GoPageLoad = function () {

    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'interestedUserID': InterestedUserID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "GalleryListInterested.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-13
작성목적 : 드롭다운박스 초기화
***************************************************/
List.SearchField = function () {
    SearchField = $("#ddlSearchField").val();

    if (SearchField == "TitleContents" || SearchField == "MobileContent") {

        SearchTerm = $("#ddlSearchTerm").val();

        var dropdownElement = $("#ddlSearchTerm");
        dropdownElement.find('option[value=AL]').remove();

        if (SearchTerm == "AL") {
            $("#ddlSearchTerm").val("1Y");
        }
    }
    else {
        SearchTerm = $("#ddlSearchTerm").val();

        var dropdownElement = $("#ddlSearchTerm");
        dropdownElement.find('option[value=AL]').remove();
        dropdownElement.find('option[value=PE]').remove();
        dropdownElement.append(
                    $('<option></option>').val("AL").html(R_SearchTermAL)
                );

        dropdownElement.append(
                    $('<option></option>').val("PE").html(R_SearchTermPE)
                );

        $("#ddlSearchTerm").val(SearchTerm);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-23
작성목적 : 관심대상자 추가
***************************************************/
List.AddnInterested = function () {
    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'gubun': "2",
        'accActive': 1
    };

    CallEncrypt(CallGetParam());

    var url = "";

    url = "/DeskPlusEIP/Board/Common/GetInterestedUser.aspx?p=" + GetEncryptValue;

    PopupWin(800, 500, url, "-500", "", true);
}