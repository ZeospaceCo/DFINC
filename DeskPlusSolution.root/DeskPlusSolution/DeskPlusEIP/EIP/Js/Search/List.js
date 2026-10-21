List = {}

$(function () {
    $(document).ready(function () {
        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                var selModule = $("input[name=rblModule]:checked").val();
                List.SelMenu(selModule);
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            var selModule = $("input[name=rblModule]:checked").val();
            List.SelMenu(selModule);
        });

        //전자결재 바로가기
        $('#cbtnApproval').click(function (e) {
            e.preventDefault();
            List.SelMenu('APPROVAL')
        });

        //게시판 바로가기
        $('#cbtnBoard').click(function (e) {
            e.preventDefault();
            List.SelMenu('BOARD')
        });

        //메일 바로가기
        $('#cbtnMail').click(function (e) {
            e.preventDefault();
            List.SelMenu('MAIL')
        });

        //자원관리 바로가기
        $('#cbtnRM').click(function (e) {
            e.preventDefault();
            List.SelMenu('RM')
        });

        //업무일지 바로가기
        $('#cbtnReport').click(function (e) {
            e.preventDefault();
            List.SelMenu('REPORT')
        });

        //검색분야
        $('input[name=rblModule]:radio').change(function () {
            var selModule = $("input[name=rblModule]:checked").val();
            List.SelMenu(selModule);
        });

        //결과수
        $('input[name=rblListCount]:radio').change(function () {
            var selModule = $("input[name=rblModule]:checked").val();
            List.SelMenu(selModule);
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        $("#startEndDate").hide(); //기간 이외
        $("input[name=rblModule]").filter("[value='']").prop("checked", true); //검색분야
        $("input[name=rblListCount]").filter("[value='20']").prop("checked", true); //결과수
        $(".clsDate").keydown(function (e) { return false; });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        List.Search();
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
List.Search = function () {
    $("#useApproval").hide();
    $("#useBoard").hide();
    $("#useMail").hide();
    $("#useRM").hide();
    $("#useReport").hide();

    $("#cbtnApproval").hide();
    $("#cbtnBoard").hide();
    $("#cbtnMail").hide();
    $("#cbtnRM").hide();
    $("#cbtnReport").hide();

    var response = List.SettingSearch();
    if (response == false) return;

    if (ModuleName != "") {
        List.LoadData();
        $("#Pager1").show();
    }
    else {
        $("#cbtnApproval").show();
        $("#cbtnBoard").show();
        $("#cbtnMail").show();
        $("#cbtnRM").show();
        $("#cbtnReport").show();

        setTimeout(function () {
            ModuleName = "APPROVAL";
            List.LoadData();
        }, 100);

        setTimeout(function () {
            ModuleName = "BOARD";
            List.LoadData();
        }, 100);

        setTimeout(function () {
            ModuleName = "MAIL";
            List.LoadData();
        }, 100);

        setTimeout(function () {
            ModuleName = "RM";
            List.LoadData();
        }, 100);

        setTimeout(function () {
            ModuleName = "REPORT";
            List.LoadData();
        }, 100);

        $("#Pager1").hide();
    }

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchKey = $("#ddlSearchKey").val();
    SearchValue = $("#ctbSearch").val();
    RecordPerPage = $("input[name=rblListCount]:checked").val();
    Order = ""; // $('#ddlSort').val();

    CurrentPage = 1; //1페이지로 이동처리

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
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        List.GetCount(); //검색 총 카운트 가져오기
        List.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                List.LoadData();
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

    loadBoardSearchData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Search/List.asmx/GetPageCount";

    ParamArray = {
        'moduleName': ModuleName,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchKey,
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
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 포틀릿 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Search/List.asmx/GetPageList";

    ParamArray = {
        'moduleName': ModuleName,
        'totalCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchKey,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    if (ModuleName != "") {
        var phDom = "";
        switch (ModuleName) {
            case "APPROVAL":
                $("#useApproval").show();
                phDom = "phApproval";
                break;
            case "BOARD":
                $("#useBoard").show();
                phDom = "phBoard";
                break;
            case "MAIL":
                //                $("#useMail").show();
                //                phDom = "phMail";
                break;
            case "RM":
                $("#useRM").show();
                phDom = "phRM";
                break;
            case "REPORT":
                $("#useReport").show();
                phDom = "phReport";
                break;
        }

        $('#' + phDom).setTemplateURL('/DeskPlusEIP/EIP/Jt/Search/JTList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#' + phDom).processTemplate(data.d);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-02-14
작성목적 : 다국어
**********************************************************************************************/
List.Resource = function () {
    if ($(".r_number") != undefined) { $(".r_number").text(R_Number) };
    if ($(".r_modulename") != undefined) { $(".r_modulename").text(R_Section) };
    if ($(".r_title") != undefined) { $(".r_title").text(R_SearchOutputTitle) };
    if ($(".r_attach") != undefined) { $(".r_attach").text(R_AttachFile) };
    if ($(".r_register") != undefined) { $(".r_register").text(R_Registrant) };
    if ($(".r_regdate") != undefined) { $(".r_regdate").text(R_SearchOutputDate) };
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent) };
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시물 보기
***************************************************/
List.View = function (p_categoryID, p_listID) {
    ParamArray = {
        'menuCatID': MenuCatID,
        'catID': CatID,
        'categoryID': p_categoryID,
        'listID': p_listID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchKey,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    //location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 포틀릿 추가하기
***************************************************/
List.Confirm = function (p_portletID) {
    var url = "/DeskPlusEIP/EIP/Ws/Portal/List.asmx/AddTabPortlet";

    //파라미터값 배열에 담기
    ParamArray = {
        'tabID': TabID,
        'columnID': FirstColumn,
        'portletID': p_portletID
    };

    var obj = eval("List.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 성공시, 포틀릿 추가하기
***************************************************/
List.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_AddPortletCheckedTab);
        List.LoadData();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 닫기
***************************************************/
List.Close = function () {
    ParamArray = {
        'tabID': TabID
    };
    CallEncrypt(CallGetParam());

    parent.location = "/DeskPlusEIP/EIP/Portal/Main.aspx?p=" + GetEncryptValue;

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시판 새창으로 내용물 보기
***************************************************/
List.PopupView = function (p_moduleName, p_categoryID, p_listID) {
    switch (p_moduleName) {
        case "BOARD":
            List.BoardPopupView(p_categoryID, p_listID);
            break;
        case "APPROVAL":
            List.AppPopupView(p_categoryID, p_listID);
            break;
        case "RM":
            List.RMPopupView(p_categoryID, p_listID);
            break;
        case "REPORT":
            List.ReportPopupView(p_categoryID, p_listID);
            break;
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시판 새창으로 내용물 보기
***************************************************/
List.BoardPopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'catID': p_categoryID,
        'categoryID': p_categoryID,
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    //var goPage = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    //WindowObjectReference = window.open(goPage, "ShowImage", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/Board/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "BoardPopupView", "1000", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 결재 새창으로 내용물 보기
***************************************************/
List.AppPopupView = function (p_categoryID, p_listID) {     //p_listID = documentID
    ParamArray = {
        'documentID': p_listID
        , 'srh': '1'
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "AppPopupView", "1000", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 자원관리 새창으로 내용물 보기
***************************************************/
List.RMPopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'reservationID': p_listID
    };

    CallEncrypt(CallGetParam());

    //var goPage = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    //WindowObjectReference = window.open(goPage, "ShowImage", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "RMPopupView", "1000", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 업무일지 새창으로 내용물 보기
***************************************************/
List.ReportPopupView = function (p_categoryID, p_listID) {
    ParamArray = {
        'listID': p_listID
    };

    CallEncrypt(CallGetParam());

    //var goPage = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    //WindowObjectReference = window.open(goPage, "ShowImage", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/Report/Content/ViewPopup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ReportPopupView", "1000", "700", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 탭 선택시
***************************************************/
List.SelMenu = function (p_module) {
    $(".tab a").removeAttr("class");      //Class 삭제

    var selDom = "menu_whole";

    switch (p_module) {
        case "APPROVAL":
            selDom = "menu_approval";
            break;
        case "BOARD":
            selDom = "menu_board";
            break;
        case "MAIL":
            selDom = "menu_mail";
            break;
        case "RM":
            selDom = "menu_rm";
            break;
        case "REPORT":
            selDom = "menu_report";
            break;
    }

    $("#" + selDom + " a").attr("class", "tab_on");
    $("input[name=rblModule]").filter("[value=" + p_module + "]").prop("checked", true);

    ModuleName = $("input[name=rblModule]:checked").val();

    List.Search();
}