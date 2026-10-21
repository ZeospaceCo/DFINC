GetBaseDoc = {}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            GetBaseDoc.LoadData("");
        }
    }

    $(document).ready(function () {

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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            RecordPerPage = $('#ddlListCount').val();
            GetBaseDoc.SavePageSize();
            GetBaseDoc.Search();
        });

        //부서별문서함 탭
        $('#dept').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("dept");
        });

        //개인별문서함 탭
        $('#private').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("private");
        });

        //결재문서함 탭
        $('#approve').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("approve");
        });

        //전결문서함 탭
        $('#arbitrary').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("arbitrary");
        });

        //참조문서함 탭
        $('#carboncopy').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("carboncopy");
        });

        //전달문서함 탭
        $('#forward').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("forward");
        });

        //진행중인문서 탭
        $('#privateprocess').click(function (e) {
            e.preventDefault();
            GetBaseDoc.GoList("privateprocess");
        });

        //날짜검색
        $("#ddlSearchTerm").change(function (e) {
            e.preventDefault();
            GetBaseDoc.RegDate();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                GetBaseDoc.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            GetBaseDoc.Search();
        });

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            GetBaseDoc.Save();
        });

        GetBaseDoc.GetCount("dept");
        GetBaseDoc.Init();

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
GetBaseDoc.Init = function () {

    $("#startEndDate").hide(); //기간

    if (SearchTerm == "period") {
        $("#startEndDate").show(); //기간   
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchTermValue != "") {
            $("#startDate").val(SearchTermValue.split(",")[0]);
            $("#endDate").val(SearchTermValue.split(",")[1]);
        }
    }

    GetBaseDoc.LoadData("search");
    GetBaseDoc.Menu(FolderName);

    $("#zeoModalContact").draggable({ disabled: false });


}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
GetBaseDoc.RegDate = function () {
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
GetBaseDoc.LoadData = function (gubun) {

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        GetBaseDoc.GetPageList(FolderName);
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        GetBaseDoc.GetCount(FolderName); //검색 총 카운트 가져오기
        GetBaseDoc.GetPageList(FolderName); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                GetBaseDoc.LoadData("search");
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
GetBaseDoc.SettingSearch = function () {

    SearchTerm = $("#ddlSearchTerm").val();
    SearchField = $("#ddlSearchField").val();

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

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
GetBaseDoc.Search = function () {

    var response = GetBaseDoc.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리  
    GetBaseDoc.LoadData("search");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 문서함 탭 이동
***************************************************/
GetBaseDoc.GoList = function (gubun) {

    GetBaseDoc.Menu(gubun);
    FolderName = gubun;

    CurrentPage = 1;            //페이지 초기화
    $('#ctbSearch').val("");    //검색박스 초기화
    SearchValue = "";           //검색어 초기화   

    //문서함별 문서 카운트 가져오기
    //GetBaseDoc.GetCount(gubun);

    GetBaseDoc.LoadData("search");
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 문서함 탭 메뉴 변경
***************************************************/
GetBaseDoc.Menu = function (gubun) {

    $('#private').removeClass();
    $('#dept').removeClass();
    $('#carboncopy').removeClass();
    $('#arbitrary').removeClass();
    $('#approve').removeClass();
    $('#forward').removeClass();
    $('#privateprocess').removeClass();

    switch (gubun) {
        case "dept":
            $('#dept').addClass('tab_on');
            break;
        case "private":
            $('#private').addClass('tab_on');
            break;
        case "approve":
            $('#approve').addClass('tab_on');
            break;
        case "arbitrary":
            $('#arbitrary').addClass('tab_on');
            break;
        case "carboncopy":
            $('#carboncopy').addClass('tab_on');
            break;
        case "forward":
            $('#forward').addClass('tab_on');
            break;
        case "privateprocess":
            $('#privateprocess').addClass('tab_on');
            break;
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 문서함 리스트 호출
**********************************************************************************************/
GetBaseDoc.GetPageList = function (_folderName) {
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/GetBaseDoc.asmx/GetPageList";


    //파라미터값 배열에 담기
    ParamArray = {
        'totalItemCount': TotalItemCount,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderName': _folderName
    };

    var obj = eval("GetBaseDoc.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 문서함 리스트 표시
**********************************************************************************************/
GetBaseDoc.SuccessPageList = function (data, status) {
    $('#placeholder_baseDocList').setTemplateURL('/DeskPlusEIP/Approval/Jt/FormFlow/Basic/JTGetBaseDocList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_baseDocList').processTemplate(data.d);
    $('#loading').html("");

    GetBaseDoc.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

GetBaseDoc.Resource = function () {

    $("#r_docnumber")[0].innerHTML = R_DocNumber;
    $("#r_formname")[0].innerHTML = R_FormName;
    $("#r_subject")[0].innerHTML = R_Subject;
    $("#r_name")[0].innerHTML = R_Name;
    $("#r_writetime")[0].innerHTML = R_Writetime;
    $("#r_completetime")[0].innerHTML = R_Completetime;
    $("#r_add")[0].innerHTML = R_Add;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".BtnAdd") != undefined) { $(".BtnAdd").attr('src', ImagePath + '/Icon/CheckOut.gif'); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시물 카운트  호출
**********************************************************************************************/
GetBaseDoc.GetCount = function (_folderName) {
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/GetBaseDoc.asmx/GetListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderName': _folderName
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetBaseDoc.SuccessCount");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
GetBaseDoc.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 문서 보기
***************************************************/
GetBaseDoc.View = function (p_docID) {

    ParamArray = {
        'documentID': p_docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "_blank", "1200", "800");
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 문서 추가
***************************************************/
GetBaseDoc.Add = function (p_documentID, p_docNumber, p_formName, p_subject, p_user) {

    // 진행중인 문서 중 동일한 문서 체크
    if (window.opener) {
        if (opener.DocumentID != undefined) {
            if (opener.DocumentID == p_documentID) {
                alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }

        if (opener.ParentDocumentID != undefined) {
            if (opener.ParentDocumentID == p_documentID) {
                alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }

        if (opener.GroupID != undefined) {
            if (opener.GroupID == p_documentID) {
                alert(R_Msg_ErrBaseSameDoc); //동일한 문서는 추가할 수 없습니다.
                return;
            }
        }
    }

    // 부모창에 추가된 근거문서가 있으면 체크
    if (BaseDocIDs != "") {

        var trimDocumentIDsP = BaseDocIDs.split("|").join("");  // replaceAll
        trimDocumentIDsP = trimDocumentIDsP.trimEnds(';'); // 부모창
        var selStrParent = trimDocumentIDsP.split(';'); // 부모창

        // 부모창에 이미 추가된 문서인지 체크
        for (i = 0; i < selStrParent.length; i++) {
            if (p_documentID + "^" + p_docNumber == selStrParent[i]) {
                alert(R_Msg_ErrBaseDoc);
                return;
            }
        }
    }

    var trimDocumentIDs = BaseDocPopUpIDs.split("|").join("");  // replaceAll
    trimDocumentIDs = trimDocumentIDs.trimEnds(';'); // 팝업창
    var selStrPopUp = trimDocumentIDs.split(';'); //팝업창    

    // 팝업창에서 이미 선택된 문서인지 체크
    for (i = 0; i < selStrPopUp.length; i++) {
        if (p_documentID + "^" + p_docNumber == selStrPopUp[i]) {
            alert(R_Msg_ErrBaseDocList); return;
        }
    }

    //팝업용 파라미터에 추가한 문서번호 저장
    BaseDocPopUpIDs = BaseDocPopUpIDs + '|' + p_documentID + '^' + p_docNumber + ';'

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocPopUpIDs);

    GetBaseDoc.AddRow(p_documentID, p_docNumber, p_formName, p_subject, p_user);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 추가시 ROW 그리기
***************************************************/
GetBaseDoc.AddRow = function (p_documentID, p_docNumber, p_formName, p_subject, p_user) {

    $('#noContent').remove();

    var folderName = "";
    RowMax = RowMax + 1;

    switch (FolderName) {
        case "dept":
            folderName = R_DeptFolder;
            break;
        case "private":
            folderName = R_PrivateFolder;
            break;
        case "approve":
            folderName = R_ApprovalFolder;
            break;
        case "arbitrary":
            folderName = R_ArbitraryFolder;
            break;
        case "carboncopy":
            folderName = R_CarbonFolder;
            break;
        case "forward":
            folderName = R_TransFolderForward;
            break;
        case "privateprocess":
            folderName = R_DocProcess;
            break;
    }
    
    var contentHtml = "<tr id='" + p_documentID + "' class='rows' name='" + p_docNumber + "'>"
            + "<td class='center'>" + folderName + "</td>"
            + "<td>" + p_docNumber + "</td>"
            + "<td class='center'>" + p_formName + "</td>"
            + "<td><a onclick=\"GetBaseDoc.View('" + p_documentID + "')\" style='cursor:pointer' >" + TextReverseToXss(p_subject) + "</a></td>"
            + "<td class='center'>" + p_user + "</td>"
            + "<td class='center'><a onclick=\"GetBaseDoc.View('" + p_documentID + "')\">&nbsp;&nbsp;<img class='BtnView' alt=''></a></td>"
            + "<td class='center'><a onclick=\"GetBaseDoc.Del('" + p_documentID + "', '" + p_docNumber + "')\">&nbsp;&nbsp;<img class='BtnDel' alt=''></a></td>"
            + "<td class='center'><a onclick=\"GetBaseDoc.Up('" + p_documentID + "', '" + p_docNumber + "')\"><img class='BtnUp' alt=''></a>&nbsp;&nbsp;&nbsp;"
            + "<a onclick=\"GetBaseDoc.Down('" + p_documentID + "', '" + p_docNumber + "')\"><img class='BtnDown' alt=''></a></td>"
        + "</tr>";

    $("#contentTable").append(contentHtml);

    var contentHtml2 = "<li id='new_" + p_documentID + "' class='rows2' name='" + p_docNumber + "'>"
            + "<a title=" + R_Subject + " onclick=\"Common.View('" + p_documentID + "')\">" + TextReverseToXss(p_subject) + "</a>&nbsp;<a onclick=\"Common.Del('" + p_documentID + "', '" + p_docNumber + "')\"><i class='fas fa-times-circle' title=" + R_FileDel + "></i></a>&nbsp;"
            + "<a onclick=\"Common.Up('" + p_documentID + "', '" + p_docNumber + "')\"><i class='fas fa-caret-up' title='up'></i></a><a onclick=\"Common.Down('" + p_documentID + "', '" + p_docNumber + "')\"><i class='fas fa-caret-down' title='down'></i></a>"
            + "<p>"
            + "<span title=" + R_Sort + ">" + folderName + "</span><span title=" + R_DocNumber + ">" + p_docNumber + "</span><span title=" + R_FormName + ">" + p_formName + "</span><span title=" + R_ReportUser + ">" + p_user + "</span>"
            + "</p>"
        + "</li>";

    $("#contentTable2").append(contentHtml2);

    //문서보기, 삭제, 순서변경 아이콘 처리
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnDel") != undefined) { $(".BtnDel").attr('src', ImagePath + '/Icon/ico_tree_listDel.gif'); }
    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 문서 삭제
***************************************************/
GetBaseDoc.Del = function (p_documentID, p_docNumber) {

    BaseDocPopUpIDs = BaseDocPopUpIDs.split("|").join("");  // replaceAll
    BaseDocPopUpIDs = BaseDocPopUpIDs.trimEnds(';');
    var BaseDocIDsStr = BaseDocPopUpIDs.split(';');
    var newDocIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < BaseDocIDsStr.length; i++) {
        if (BaseDocIDsStr[i] != p_documentID + "^" + p_docNumber) {
            newDocIDs = newDocIDs + "|" + BaseDocIDsStr[i] + ";";
        }
    }

    BaseDocPopUpIDs = newDocIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddDocID').val(BaseDocPopUpIDs);

    GetBaseDoc.DelRow(p_documentID);
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 삭제시 ROW 제거
***************************************************/
GetBaseDoc.DelRow = function (p_documentID) {

    var row = $("#" + p_documentID);

    RowMax = RowMax - 1;

    row.remove();

    if (RowMax == 0) {
        $("#contentTable").append(
             "<tr id='noContent'>"
              + "<td colspan='8' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }


    var row = $("#new_" + p_documentID);

    row.remove();

    if (RowMax == 0) {
        $("#contentTable2").append(
             "<li id='noContent'>&nbsp;</li>"
    	);
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 저장
***************************************************/
GetBaseDoc.Save = function () {

    var rowCnt = $('#contentTable > tbody > tr').length;
    var BaseDocsPopUp = $("#tbAddDocID").val(); //팝업창 추가된 문서번호들 
    var BaseDocsPopUpIDs = BaseDocsPopUpIDs;
    var baseDocHTML = $(".rows2").parent().html();
    
    if (baseDocHTML != null) {
        baseDocHTML = baseDocHTML.replace(/GetBaseDoc./gi, 'Common.');
        baseDocHTML = baseDocHTML.replace(/new_/gi, '');

        // 부모창이 문서 작성 페이지에서 벗어났을경우 익스플로러 기본 오류메세지 숨긴 후 창닫기 
        try {
            if (window.opener && window.opener.open && !window.opener.closed) {

                //부모창 변수에 선택된 팝업창 변수값 저장
                BaseDocIDs = BaseDocIDs + BaseDocPopUpIDs;
                opener.BaseDocIDs = BaseDocIDs;

                //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
                var ParentRows = opener.BaseDocCnt + rowCnt;
                opener.BaseDocCnt = ParentRows;

                //부모창 히든박스에 문서번호 넣기 - 개발용
                $(opener.document).find('#tbAddDocID').val(BaseDocIDs);

                // ROW 그리기
                $(opener.document).find("#noContent").remove();
                $(opener.document).find("#contentTable").append(baseDocHTML);
            }
        }
        catch (e) { }

        self.close();
    }
    else {
        alert(R_MSG_NoSelect);
    }
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 순서변경(위로)
***************************************************/
GetBaseDoc.Up = function (_documentID, p_docNumber) {

    var selectRow = $('#' + _documentID);
    var upRow = selectRow.prev();

    selectRow.insertBefore(upRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 upRow에 effect를 넣어주면 해당 현상이 사라짐
    upRow.stop();

    if (upRow.html() != null) {

        var selectRowVal = '|' + _documentID + '^' + p_docNumber + ';';
        var upRowVal = '|' + upRow.attr("id") + '^' + upRow.attr("name") + ';';

        //파라미터에 저장된 문서번호 순서 변경
        BaseDocPopUpIDs = BaseDocPopUpIDs.replace(selectRowVal, '');
        BaseDocPopUpIDs = BaseDocPopUpIDs.replace(upRowVal, selectRowVal + upRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddDocID').val(BaseDocPopUpIDs);
    }


    var selectRow = $('#new_' + _documentID);
    var upRow = selectRow.prev();

    selectRow.insertBefore(upRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 upRow에 effect를 넣어주면 해당 현상이 사라짐
    upRow.stop();
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-06-20
작성목적 : 순서변경(아래로)
***************************************************/
GetBaseDoc.Down = function (_documentID, p_docNumber) {

    var selectRow = $('#' + _documentID);
    var downRow = selectRow.next();

    selectRow.insertAfter(downRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 downRow에 effect를 넣어주면 해당 현상이 사라짐
    downRow.stop();

    if (downRow.html() != null) {

        //파라미터에 저장된 문서번호 순서 변경
        var selectRowVal = '|' + _documentID + '^' + p_docNumber + ';';
        var downRowVal = '|' + downRow.attr("id") + '^' + downRow.attr("name") + ';';

        BaseDocPopUpIDs = BaseDocPopUpIDs.replace(selectRowVal, '');
        BaseDocPopUpIDs = BaseDocPopUpIDs.replace(downRowVal, downRowVal + selectRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddDocID').val(BaseDocPopUpIDs);
    }


    var selectRow = $('#new_' + _documentID);
    var downRow = selectRow.next();

    selectRow.insertAfter(downRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 downRow에 effect를 넣어주면 해당 현상이 사라짐
    downRow.stop();

}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
GetBaseDoc.SavePageSize = function () {

    var url = "/DeskPlusEIP/Approval/Ws/Status/ListProcess.asmx/SavePageSize";

    ParamArray = {
        'folderID': "BaseDoc",
        'pageSize': $("#ddlListCount").val(),
        'types': 'BaseDoc'
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetBaseDoc.SuccessSavePageSize");

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이문호
작성일자 :2012-07-27
작성목적 : 리스트 페이지 사이즈 저장
***************************************************/
GetBaseDoc.SuccessSavePageSize = function (data, status) {
    if (data.d == true) {
    }
}

function TextReverseToXss(arg) {

    arg = arg.replace(/&/gi, "&amp;");
    arg = arg.replace(/"/gi, "&quot;");
    arg = arg.replace(/#/gi, "&#35;");
    arg = arg.replace(/'/gi, "&#39;");
    arg = arg.replace("(", "&#40;");
    arg = arg.replace(")", "&#41;");
    arg = arg.replace(/</gi, "&lt;");
    arg = arg.replace(/>/gi, "&gt;");

    return arg;
}


