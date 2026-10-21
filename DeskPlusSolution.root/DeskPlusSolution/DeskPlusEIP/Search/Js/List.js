List = {}

if (typeof String.prototype.trim != 'function') {
    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/gi, "");
    };
}

if (typeof String.prototype.startsWith !== 'function') {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}

if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function (str) {
        return this.indexOf(str, this.length - str.length) !== -1;
    };
}

/*
if (!String.prototype.startsWith) {
Object.defineProperty(String.prototype, 'startsWith', {
enumerable: false,
configurable: false,
writable: false,
value: function (searchString, position) {
position = position || 0;
return this.indexOf(searchString, position) === position;
}
});
}

if (!String.prototype.endsWith) {
Object.defineProperty(String.prototype, 'endsWith', {
enumerable: false,
configurable: false,
writable: false,
value: function (searchString) {
return this.indexOf(searchString)+searchString.length === this.length;
}
});
}
*/

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

        //문서관리 바로가기
        $('#cbtnEDM').click(function (e) {
            e.preventDefault();
            List.SelMenu('EDM')
        });

        //주소록 바로가기
        $('#cbtnAddress').click(function (e) {
            e.preventDefault();
            List.SelMenu('ADDRESS')
        });

        //웹폴더 바로가기
        $('#cbtnWebHard').click(function (e) {
            e.preventDefault();
            List.SelMenu('WEBHARD')
        });

        //업무협업 바로가기
        $('#cbtnCoop').click(function (e) {
            e.preventDefault();
            List.SelMenu('COOP')
        });

        //업무협업 바로가기
        $('#cbtnTask').click(function (e) {
            e.preventDefault();
            List.SelMenu('TASK')
        });

        //일정관리 바로가기
        $('#cbtnPim').click(function (e) {
            e.preventDefault();
            List.SelMenu('PIM')
        });


        //자원관리 바로가기
        $('#cbtnRM').click(function (e) {
            e.preventDefault();
            List.SelMenu('RM')
        });

        //검색분야
        $('input[name=rblModule]:radio').change(function () {
            var selModule = $("input[name=rblModule]:checked").val();
            List.SelMenu(selModule);
        });

        //결과수
        $('input[name=rblListCount]:radio').change(function () {
            List.Search();
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            List.RegDate();
        });

        $("#startEndDate").hide(); //기간 이외

        $("#useApproval").hide();
        $("#useBoard").hide();
        $("#useEDM").hide();
        $("#useAddress").hide();
        $("#useWebHard").hide();
        $("#useCoop").hide();
        $("#useTask").hide();
        $("#usePim").hide();
        $("#useRM").hide();
        $("#Pager1").hide();

        $("input[name=rblModule]").filter("[value='']").prop("checked", true); //검색분야
        $("input[name=rblListCount]").filter("[value='20']").prop("checked", true); //결과수

        $(".clsDate").keydown(function (e) { return false; });

        List.Init();

        List.Search();
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2013-12-26
작성목적 : 파라미터 검색 키값으로 검색 초기값 설정하기(... 홈화면에서.. 날아옴 ㅡㅡ;)
***************************************************/
List.Init = function () {
    //$(".tabA a").removeAttr("class");      //Class 삭제
    $(".tab li").removeAttr("class");      //Class 삭제

    var selDom = "menu_whole";

    switch (ModuleName) {
        case "APPROVAL":
            selDom = "menu_approval";
            break;
        case "BOARD":
            selDom = "menu_board";
            break;
        case "EDM":
            selDom = "menu_edm";
            break;
        case "ADDRESS":
            selDom = "menu_address";
            break;
        case "WEBHARD":
            selDom = "menu_webhard";
            break;
        case "COOP":
            selDom = "menu_coop";
            break;
        case "TASK":
            selDom = "menu_task";
            break;
        case "PIM":
            selDom = "menu_pim";
            break;
        case "RM":
            selDom = "menu_rm";
            break;
    }

    //$("#" + selDom + " a").attr("class", "tab_on");
    $("#" + selDom).attr("class", "selected");
    $("input[name=rblModule]").filter("[value=" + ModuleName + "]").prop("checked", true);
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 검색
***************************************************/
List.Search = function () {
    List.SetLoading(true);
    $("#useApproval").hide();
    $("#useBoard").hide();
    $("#useEDM").hide();
    $("#useAddress").hide();
    $("#useWebHard").hide();
    $("#useCoop").hide();
    $("#useTask").hide();
    $("#usePim").hide();
    $("#useRM").hide();

    var response = List.SettingSearch();
    if (response == false) return;

    if (ModuleName != "") {
        List.LoadData();
        $("#Pager1").show();
    }
    else {
        $("#Pager1").show();

        setTimeout(function () {
            ModuleName = "APPROVAL";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "BOARD";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "EDM";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "ADDRESS";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "WEBHARD";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "COOP";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "TASK";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "PIM";
            List.LoadData();
        }, 1);

        setTimeout(function () {
            ModuleName = "RM";
            List.LoadData();
        }, 1);

        $("#Pager1").hide();
    }

    setTimeout(function () {
        List.Resource();
    }, 1);
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 검색 필드 설정하기
***************************************************/
List.SettingSearch = function () {
    SearchTerm = $("#ddlSearchTerm").val();
    SearchKey = $("#ddlSearchKey").val();
    SearchValue = $("#ctbSearch").val().securityText();

    if (!List.CheckValue()) return false;

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
작성자   : freeman
작성일자 : 2013-12-26
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
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadSearchData = function () {
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

    loadSearchData();
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-18
작성목적 : 웹서비스 카운트  호출
**********************************************************************************************/
List.GetCount = function () {
    var url = "/DeskPlusEIP/Search/Ws/List.asmx/GetPageCount";

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
작성자   : freeman
작성일자 : 2013-12-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
List.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-18
작성목적 : 검색어 조건 표시 오류 체크 (AND OR 조건문은 문자 사이에 와야 함)
**********************************************************************************************/
List.CheckValue = function () {
    SearchValue = SearchValue.trimText(); // 앞뒤 공백 삭제

    // AND OR 대문자는 검색 조건식으로 사용
    if (SearchValue == "AND" || SearchValue == "OR") {
        SearchValue = SearchValue.toLowercase(); // 소문자로 변경
        return true;
    }

    // AND OR 대문자는 단어 사이에 와야 함
    if (SearchValue.startsWith("AND ") || SearchValue.startsWith("OR ")) {
        alert(R_ConfirmCheckValue)
        return false;
    }

    // AND OR 대문자는 단어 사이에 와야 함
    if (SearchValue.endsWith(" AND") || SearchValue.endsWith(" OR")) {
        alert(R_ConfirmCheckValue)
        return false;
    }

    return true;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2013-12-18
작성목적 : 웹서비스 포틀릿 리스트 호출
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Search/Ws/List.asmx/GetPageList";

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
작성자   : freeman
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
            case "EDM":
                $("#useEDM").show();
                phDom = "phEDM";
                break;
            case "ADDRESS":
                $("#useAddress").show();
                phDom = "phAddress";
                break;
            case "WEBHARD":
                $("#useWebHard").show();
                phDom = "phWebHard";
                break;
            case "COOP":
                //$("#useCoop").show();
                //phDom = "phCoop";
                break;
            case "TASK":
                $("#useTask").show();
                phDom = "phTask";
                break;
            case "PIM":
                $("#usePim").show();
                phDom = "phPim";
                break;
            case "RM":
                $("#useRM").show();
                phDom = "phRM";
                break;
        }

        $('#' + phDom).setTemplateURL('/DeskPlusEIP/Search/Jt/JTBList.htm?seq=' + Guids(), null, { filter_data: false });
        $('#' + phDom).processTemplate(data.d);
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-02-14
작성목적 : 다국어
**********************************************************************************************/
List.Resource = function () {

    $(".r_number").text(R_Number);
    $(".r_modulename").text(R_Section);
    $(".r_title").text(R_SearchOutputTitle);
    $(".r_attach").text(R_AttachFile);
    $(".r_register").text(R_Registrant);
    $(".r_regdate").text(R_SearchOutputDate);

    if ($(".IDNameS") != undefined) { $(".IDNameS").text(R_Schedule); }
    if ($(".IDNameT") != undefined) { $(".IDNameT").text(R_ToDo); }

    $(".NoListData").text(R_NoContent);
    List.SetLoading(false);
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
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

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 게시판 새창으로 내용물 보기
***************************************************/
List.PopupView = function (p_moduleName, arg1, arg2, arg3) {
    switch (p_moduleName) {
        case "BOARD":
            List.BoardPopupView(arg1, arg2);
            break;
        case "APPROVAL":
            List.ApprovalPopupView(arg2);
            break;
        case "EDM":
            List.EDMPopupView(arg1, arg2, arg3);
            break;
        case "ADDRESS":
            List.AddressPopupView(arg1, arg2, arg3);
            break;
        case "WEBHARD":
            List.FileDownload(arg1, arg2, arg3);
            break;
        case "COOP":
            List.CoopPopupView(arg1, arg2);
            break;
        case "TASK":
            List.TaskPopupView(arg1, arg2);
            break;
        case "PIM":
            List.PimPopupView(arg1, arg2, arg3);
            break;
        case "RM":
            List.RMPopupView(arg1, arg2, arg3);
            break;
    }
}

/**************************************************
작성자   :
작성일자 :
작성목적 : 문서보기(결재진행중/완료)
***************************************************/
List.ApprovalPopupView = function (p_docID) { //alert(p_docID);
    ParamArray = { 'documentID': p_docID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "SHOWDOC", "1000", "700")
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
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
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage", "1000", "700")
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 문서관리 새창으로 내용물 보기
***************************************************/
List.EDMPopupView = function (categoryID, listID, grade) {
    ParamArray = {
        'docID': categoryID,
        'verID': listID,
        'aCLGrade': 7
    };

    CallEncrypt(CallGetParam());

    //var goPage = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    //WindowObjectReference = window.open(goPage, "ShowImage", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    var url = "/DeskPlusEDM/Document/ViewBySearch.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowEDM", "1000", "700")
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 업무협업 새창으로 내용물 보기
***************************************************/
List.CoopPopupView = function (listID, groupID) {
    ParamArray = {
        'listID': listID,
        'groupID': groupID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Coop/Content/Total/ViewPopup.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "TabCoopMain_PopupCoop", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    WindowObjectReference.focus();
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 업무협업 새창으로 내용물 보기
***************************************************/
List.TaskPopupView = function (listID, groupID) {
    ParamArray = {
        'listID': groupID,
        'groupID': groupID
    };
    
    CallEncrypt(CallGetParam());
    
    var url = "/DeskPlusEIP/Task/Common/ViewPopup.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "TabTaskMain_PopupCoop", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    WindowObjectReference.focus();
}

/**************************************************
작성자   : freeman
작성일자 : 2015-2-26
작성목적 : 주소록 새창으로 내용물 보기
***************************************************/
List.AddressPopupView = function (groupID, objectID, mail) {
    if (groupID == "G") { //회사 주소록이면 간략정보
        AccountInfo(objectID);
    } else { // 개인, 공용 주소록이면 메일쓰기
        if (mail == "") { alert(NoContent); return; }

        if (MailProduct.toUpperCase().indexOf("DEEP") < 0) {
            //zetta
            ParamArray = { 'toMail': mail };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Common/Mail/ZettaMail65/WriteMail.aspx?p=" + GetEncryptValue;
            window.open(url, "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        } else {
            //deep
            ParamArray = { 'to': mail };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Common/Mail/Deep/DeepWrite.aspx?p=" + GetEncryptValue;
            window.open(url, "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        }
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2015-03-16
작성목적 : 일정관리 새창으로 내용물 보기
***************************************************/
List.PimPopupView = function (idType, listID, displayID) {
    var url = "";
    if (idType == "S") {
        ParamArray = {
            'ScheduleID': listID,
            'DisplayID': displayID
        };

        CallEncrypt(CallGetParam());

        url = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx?p=" + GetEncryptValue;
    } else {
        ParamArray = {
            'TodoID': listID
        };

        CallEncrypt(CallGetParam());

        url = "/DeskPlusEIP/Pim/ToDo/ViewPopup.aspx?p=" + GetEncryptValue;
    }

    WindowObjectReference = window.open(url, "TabPimMain_PopupCoop", "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    WindowObjectReference.focus();
}

/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 자원관리 새창으로 내용물 보기
***************************************************/
List.RMPopupView = function (id, categoryID, reservationID) {
    ParamArray = {
        'categoryID': categoryID,
        'reservationID': reservationID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "TabRMMain_PopupCoop", "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
    WindowObjectReference.focus();
}


/**************************************************
작성자   : freeman
작성일자 : 2013-12-26
작성목적 : 탭 선택시
***************************************************/
List.SelMenu = function (p_module) {
    //$(".tabA a").removeAttr("class");      //Class 삭제
    $(".tab li").removeAttr("class");      //Class 삭제

    var selDom = "menu_whole";

    switch (p_module) {
        case "APPROVAL":
            selDom = "menu_approval";
            break;
        case "BOARD":
            selDom = "menu_board";
            break;
        case "EDM":
            selDom = "menu_edm";
            break;
        case "ADDRESS":
            selDom = "menu_address";
            break;
        case "WEBHARD":
            selDom = "menu_webhard";
            break;
        case "COOP":
            selDom = "menu_coop";
            break;
        case "TASK":
            selDom = "menu_task";
            break;
        case "PIM":
            selDom = "menu_pim";
            break;
        case "RM":
            selDom = "menu_rm";
            break;
    }

    //$("#" + selDom + " a").attr("class", "tab_on");
    $("#" + selDom).attr("class", "selected");
    $("input[name=rblModule]").filter("[value=" + p_module + "]").prop("checked", true);

    ModuleName = $("input[name=rblModule]:checked").val();

    $("#useApproval").hide();
    $("#useBoard").hide();
    $("#useEDM").hide();
    $("#useAddress").hide();
    $("#useWebHard").hide();
    $("#useCoop").hide();
    $("#useTask").hide();
    $("#usePim").hide();
    $("#useRM").hide();
    $("#Pager1").hide();

    List.Search();
}

/**********************************************************************************************
작성자   : 이현수
작성일자 : 2013-12-27
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
List.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성자   :
작성일자 :
작성목적 : [로딩페이지]
***************************************************/
List.SetLoading = function (_isComplete) {
    var arr = $("#divLoading");
    if (arr == null) return;

    $.each(arr, function () {
        if (_isComplete) {
            arr.fadeIn(10); //350
        } else {
            arr.fadeOut(100);
        }
    });
}