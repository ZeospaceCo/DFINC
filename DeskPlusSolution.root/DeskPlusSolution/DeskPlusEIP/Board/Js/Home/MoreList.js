MoreList = {}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
            CurrentPage = selectedPage;
            MoreList.LoadData("");
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
            MoreList.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                MoreList.Search();
            }
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            MoreList.List();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            MoreList.Search();
        });
       

        //리스트형 저장
        $('#listType1').click(function () {
            MoreList.ModCategoryUser("1");
            MoreList.GoPageListType("1");
        });

        //블로그형 저장
        $('#listType2').click(function () {
            MoreList.ModCategoryUser("2");
            MoreList.GoPageListType("2");
        });

        //갤러리형 저장
        $('#listType3').click(function () {
            MoreList.ModCategoryUser("3");
            MoreList.GoPageListType("3");
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            MoreList.ModCategoryUser("1");
            MoreList.GoPageListType("1");
        });

        MoreList.Init();

        //드롭다운박스 초기화
        MoreList.SearchField();

        //검색필드
        $('#ddlSearchField').change(function () {
            MoreList.SearchField();
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
MoreList.Init = function () {
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

    //MoreList.ShowListType();
    MoreList.LoadData("");
    __addEventListener(document, "click", MoreList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
MoreList.LoadData = function (gubun) {
    
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        MoreList.GetPageList();
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        MoreList.GetCount(); //검색 총 카운트 가져오기
        MoreList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                MoreList.LoadData("search");
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


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
MoreList.GetPageList = function () {
    //alert(1);
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': HomeCategory,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'optionUseDept': OptionUseDept,
        'optionUseView': OptionUseView,
        'auth': "0"
    };

    var obj = eval("MoreList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
MoreList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Home/JTMoreList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(JSON.parse(data.d)); //string 을 JSON.parse 통해json 형태로 object  화 함. (shimpark)
    $('#loading').html("");

    MoreList.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

MoreList.Resource = function () {
    //    $(".table_60")[0].innerHTML = R_Number;
    //    $(".table_100")[0].innerHTML = R_CategoryName;
    //    $(".table_titleA")[0].innerHTML = R_Title;
    //    $(".table_attach")[0].innerHTML = R_AttachName;
    //    $(".table_viewA")[0].innerHTML = R_ViewCount;
    //    $(".table_writer")[0].innerHTML = R_Register;
    //    $(".table_120A")[0].innerHTML = R_RegDate;

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_categoryName")[0].innerHTML = R_CategoryName;
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_attachName")[0].innerHTML = R_AttachName;
    $("#r_viewCount")[0].innerHTML = R_ViewCount;
    $("#r_register")[0].innerHTML = R_Register;
    $("#r_regDate")[0].innerHTML = R_RegDate;

    if ($(".s_notice") != undefined) { $(".s_notice").text(R_Notice); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".IconPopUp") != undefined) { $(".IconPopUp").attr('src', ImagePath + '/Icon/ico_popup_silver.gif'); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

    var tmpOrder = Order.replace(" asc", "").replace(" desc", "");
    var colValue = tmpOrder + " asc";

    if (Order == colValue) {
        $("#a_" + tmpOrder).addClass("asc");
    }
    else {
        $("#a_" + tmpOrder).addClass("desc");
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
MoreList.GetCount = function () {
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetListCount";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'homeCategory': HomeCategory,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'optionUseDept': OptionUseDept,
        'optionUseView': OptionUseView,
        'auth': "0"
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MoreList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
MoreList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
MoreList.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchGroup = $("#ddlSearchGroup").val();
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
MoreList.RegDate = function () {
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
MoreList.Search = function () {
    var response = MoreList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    MoreList.LoadData("search");
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 새창으로 게시물 보기
***************************************************/
MoreList.PopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': CatID,
        'categoryID': p_categoryID,
        'listID': p_listID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}


/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
MoreList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": MoreList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

MoreList.GetAttachFileListMenu = function (p_categoryID, p_listID) {
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "categoryID=" + p_categoryID + "&listID=" + p_listID;
    var obj = eval("MoreList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
MoreList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
MoreList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var categoryID = el.getAttribute("CategoryID");
    var listID = el.getAttribute("ListID");

    MoreList.GetAttachFileListMenu(categoryID, listID);

    if (AttachFileInfoList == "0") {
        alert(R_NoFileData);
        return;
    }
    else if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile);
        return;
    }
    else {
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "";
        var attachFileName = "";
        var attachFilePath = "";

        /*
        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, MoreList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }
        */

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            oItem = oContextMenuCtrl.addItem(attachFileName, MoreList.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 열기
***************************************************/
MoreList.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        //$("#ifrmAttachFile")[0].src = arg;
        window.open(arg);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 전체 다운로드
***************************************************/
MoreList.TotalDownLoad = function (_e) {
    var evt = new __event(_e);

    if (evt.target) {
        var arg = evt.target.getAttribute("arg");

        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        //$("#frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";

        PopupWinOpen("", "", "openWin", "600", "400")
        //window.open("", "openWin", "width=506px,height=388px,scrollbars=no,status=no,toolbar=no,resizable=0,location=no,menu=no");
        //$("#frmTotalDownLoad").submit();
        document.getElementById("frmTotalDownLoad").submit();
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
MoreList.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 중요표시 추가 및 삭제하기
**********************************************************************************************/
MoreList.ModImpUser = function (_listID, _arg) {
    if (_listID == "") return;

    //카테고리별 사용자 정보 저장하기
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/AddImpUser";

    if ($(_arg).attr("checked") == "checked") {
        $("#" + _listID).attr("checked", true);
        $(_arg).next().addClass("on");
    }
    else {
        $("#" + _listID).attr("checked", false);
        $(_arg).next().removeClass("on");
        url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/DelImpUser";
    }

    //1|GM
    ParamArray = {
        'companyID': CompanyID,
        'listID': _listID.split("_")[1]
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MoreList.SuccessModImpUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 중요표시 삭제 및 추가하기
**********************************************************************************************/
MoreList.SuccessModImpUser = function (data, status) {
    if (data.d == true) {
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 목록으로 가기
***************************************************/
MoreList.List = function () {
    location.href = "Content.aspx";
}


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 카테고리별 사용자 설정 정보 저장하기
***************************************************/
MoreList.ModCategoryUser = function (gubun) {
    
    var url = "/DeskPlusEIP/Board/Ws/Home/MoreList.asmx/ModCategoryUser";
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'catID': 0,
        'listType': gubun,
        'pageSize': $("#ddlListCount").val(),
        'types': 'BL'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("MoreList.SuccessModCategoryUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 카테고리별 사용자 정보 저장하기
**********************************************************************************************/
MoreList.SuccessModCategoryUser = function (data, status) {
    if (data.d == true) {
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리스트 타입선택시 이동하기
**********************************************************************************************/
MoreList.GoPageListType = function (gubun) {
    //alert(HomeCategory);
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    var iQuery = iPageUrl.attr('query');

    switch (gubun) {
        case "1":
            iPath = iPath.replace("/BlogList.aspx", "/MoreList.aspx");
            iPath = iPath.replace("/GalleryList.aspx", "/MoreList.aspx");
            break;
        case "2":
            iPath = iPath.replace("/List.aspx", "/BlogList.aspx");
            iPath = iPath.replace("/GalleryList.aspx", "/BlogList.aspx");
            break;
        case "3":
            iPath = iPath.replace("/List.aspx", "/GalleryList.aspx");
            iPath = iPath.replace("/BlogList.aspx", "/GalleryList.aspx");
            break;
        default:
            iPath = iPath.replace("/BlogList.aspx", "/MoreList.aspx");
            iPath = iPath.replace("/GalleryList.aspx", "/MoreList.aspx");
            break;
    }

    var response = MoreList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리

    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'homeCategory': HomeCategory
    };

    CallEncrypt(CallGetParam());

    //이동하기
    location = iPath + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-03-13
작성목적 : 드롭다운박스 초기화
***************************************************/
MoreList.SearchField = function () {
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
작성목적 : 새창으로 댓글리스트 보기
***************************************************/
MoreList.PopupShort = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Board/Content/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성목적 : 조회현황
***************************************************/
MoreList.ReadList = function (p_categoryID, p_listID) {
    ParamArray = {
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}
