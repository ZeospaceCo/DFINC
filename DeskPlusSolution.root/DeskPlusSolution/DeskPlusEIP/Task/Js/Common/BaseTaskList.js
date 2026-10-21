BaseTaskList = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 데이터 초기화
**********************************************************************************************/

$(function () {
    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            BaseTaskList.LoadData("");
        }
    }

    $(document).ready(function () {
        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            BaseTaskList.Save();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                BaseTaskList.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            BaseTaskList.Search();
        });

        // 기간
        $("#ddlSearchField").change(function (e) {
            e.preventDefault();
            BaseTaskList.RegDate();
        });


        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            BaseTaskList.ModListUser();
            BaseTaskList.Search();
        });

        //대분류 선택
        $('#ddlItem').change(function () {
            BaseTaskList.GetItemSubList();
        });

        // 상태선택
        $('#cbxStatus').click(function (e) {

            var tmpStatus = [];
            $("[id*=cbxStatus] input:checked").each(function () {
                tmpStatus.push($(this).val());
            });

            SearchStatus = tmpStatus;
            BaseTaskList.ModStatus(tmpStatus);
            BaseTaskList.Search();

        });

        BaseTaskList.Init();

        $(".clsDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 초기화 하기
***************************************************/
BaseTaskList.Init = function () {
    $("#startEndDate").hide(); //기간
    $("#ctbSearch").show(); //검색어
    if (SearchField == "RequestDate" || SearchField == "StartDate" || SearchField == "EndDate") {
        $("#startEndDate").show(); //기간
        $("#ctbSearch").hide(); //검색어

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);

        if (SearchDateValue != "") {
            $("#startDate").val(SearchDateValue.split(",")[0]);
            $("#endDate").val(SearchDateValue.split(",")[1]);
        }
    }

    BaseTaskList.LoadData("");
    __addEventListener(document, "click", BaseTaskList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 등록일자 활성화 여부
***************************************************/
BaseTaskList.RegDate = function () {
    //기간 선택
    $("#ctbSearch").show(); //검색어
    var sField = $("#ddlSearchField").val();
    if (sField == "RequestDate" || sField == "StartDate" || sField == "EndDate") {
        $("#startEndDate").show(); //기간
        $("#ctbSearch").hide(); //검색어

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);
    } else {
        $("#startEndDate").hide(); //기간 이외
    }

    $("#startDate").val("");
    $("#endDate").val("");
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 리스트 초기화
***************************************************/
BaseTaskList.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadListBaseData = function () {
        BaseTaskList.GetCount(); //검색 총 카운트 가져오기
        BaseTaskList.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadListBaseSearchData = function () {
        BaseTaskList.GetCount(); //검색 총 카운트 가져오기
        BaseTaskList.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                BaseTaskList.LoadData("search");
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            ListCount: ListCount,
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
            loadListBaseSearchData();
        }
        else {
            loadListBaseData();
        }
    }, 100);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 검색 필드 설정하기
***************************************************/
BaseTaskList.SettingSearch = function () {
    SearchField = $("#ddlSearchField").val();    
    ItemID = $("#ddlItem").val(); // 대분류
    ItemSubID = $("#ddlItemSub").val(); // 소분류

    if (SearchField == "RequestDate" || SearchField == "StartDate" || SearchField == "EndDate") {
        var getStartDate = $("#startDate").val();
        var getEndDate = $("#endDate").val();

        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var start_dates = getStartDate.dateCharRemove();
            var end_dates = getEndDate.dateCharRemove();

            if ((end_dates - start_dates) < 0) {
                alert(R_StartDateEndDate);
                return false;
            }
        }

        SearchDateValue = getStartDate + "," + getEndDate;
    }

    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 검색
***************************************************/
BaseTaskList.Search = function () {
    var response = BaseTaskList.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    BaseTaskList.LoadData("search");
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 문서함 리스트 호출
**********************************************************************************************/
BaseTaskList.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Common/BaseTaskList.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,        
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    var obj = eval("BaseTaskList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 성공시 문서함 리스트 표시
**********************************************************************************************/
BaseTaskList.SuccessPageList = function (data, status) {
    $('#placeholder_listBase').setTemplateURL('/DeskPlusEIP/Task/Jt/Common/JTBaseTaskList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_listBase').processTemplate(data.d);
    $('#loading').html("");

    BaseTaskList.Resource();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
BaseTaskList.Resource = function () {
    $("#r_num")[0].innerHTML = R_IsGubun;
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_status")[0].innerHTML = R_State;
    $("#r_requester")[0].innerHTML = R_Requester;
    $("#r_requestdate")[0].innerHTML = R_RequestDate;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_add")[0].innerHTML = R_Add;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".BtnAdd") != undefined) { $(".BtnAdd").attr('src', ImagePath + '/Icon/CheckOut.gif'); }
    if ($(".IsGubun1") != undefined) { $(".IsGubun1").text(R_IsGubun1); }
    if ($(".IsGubun2") != undefined) { $(".IsGubun2").text(R_IsGubun2); }  
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 웹서비스 게시물 카운트  호출
**********************************************************************************************/
BaseTaskList.GetCount = function (_folderName) {
    var url = "/DeskPlusEIP/Task/Ws/Common/BaseTaskList.asmx/GetListCount";

    ParamArray = {
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID,
        'isGubun': IsGubun
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("BaseTaskList.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
BaseTaskList.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-17
작성목적 : 문서 추가
***************************************************/
BaseTaskList.Add = function (p_listID, p_groupID, p_taskNumber, p_title) {

    //해당 문서인지 확인
    if (GroupID > 0) {
        if (GroupID == p_groupID) {
            alert(R_ThisBase);
            return;
        }
    }

    // 부모창에 추가된 근거문서가 있으면 체크
    if (BaseTasks != "") {
        var trimListIDsP = BaseTasks.split("|").join("");  // replaceAll
        trimListIDsP = trimListIDsP.trimEnds(';'); // 부모창
        var selStrParent = trimListIDsP.split(';'); // 부모창

        // 부모창에 이미 추가된 문서인지 체크
        for (i = 0; i < selStrParent.length; i++) {
            if (p_listID == selStrParent[i]) {
                alert(R_ExistBase);
                return;
            }
        }
    }

    var trimListIDs = BaseTaskPopUpIDs.split("|").join("");  // replaceAll
    trimListIDs = trimListIDs.trimEnds(';'); // 팝업창
    var selStrPopUp = trimListIDs.split(';'); //팝업창

    // 팝업창에서 이미 선택된 문서인지 체크
    for (i = 0; i < selStrPopUp.length; i++) {
        if (p_listID == selStrPopUp[i]) {
            alert(R_ExistBase); return;
        }
    }

    //팝업용 파라미터에 추가한 문서번호 저장
    BaseTaskPopUpIDs = BaseTaskPopUpIDs + '|' + p_listID + ';'

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddTaskID').val(BaseTaskPopUpIDs);

    BaseTaskList.AddRow(p_listID, p_groupID, p_taskNumber, p_title);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 추가시 ROW 그리기
***************************************************/
BaseTaskList.AddRow = function (p_listID, p_groupID, p_taskNumber, p_title) {
    $('#noTaskContent').remove();

    BaseTaskCnt = BaseTaskCnt + 1;

    var contentHtml = "<tr id='" + p_listID + "' class='rows' name='" + p_listID + "'>"
            + "<td class='center'>" + p_taskNumber + "</td>"
            + "<td>" + p_title + "</td>"
            + "<td class='center'><a onclick=\"Common.TaskViewer('" + p_listID + "','" + p_groupID + "')\">&nbsp;&nbsp;<img class='BtnView' alt=''></a></td>"
            + "<td class='center'><a onclick=\"BaseTaskList.TaskDel('" + p_listID + "')\">&nbsp;&nbsp;<img class='BtnDel' alt=''></a></td>"
            + "<td>&nbsp;&nbsp;<a onclick=\"BaseTaskList.TaskUp('" + p_listID + "')\"><img class='BtnUp' alt=''></a>&nbsp;&nbsp; &nbsp;&nbsp;"
            + "<a onclick=\"BaseTaskList.TaskDown('" + p_listID + "')\"><img class='BtnDown' alt=''></a></td>"
        + "</tr>";

    $("#contentTaskTable").append(contentHtml);

    //문서보기, 삭제, 순서변경 아이콘 처리
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnDel") != undefined) { $(".BtnDel").attr('src', ImagePath + '/Icon/ico_tree_listDel.gif'); }
    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }
}

/******************************************************************************
############# 작성목적 : 업무협업 근거문서 순서변경(위로) ####################
*******************************************************************************/

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서 순서변경(위로)
***************************************************/
BaseTaskList.TaskUp = function (p_listID) {
    var selectRow = $('#' + p_listID);
    var upRow = selectRow.prev();

    selectRow.insertBefore(upRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 upRow에 effect를 넣어주면 해당 현상이 사라짐
    upRow.stop();

    if (upRow.html() != null) {
        var selectRowVal = '|' + p_listID + ';';
        var upRowVal = '|' + upRow.attr("id") + ';';

        //파라미터에 저장된 문서번호 순서 변경
        BaseTaskPopUpIDs = BaseTaskPopUpIDs.replace(selectRowVal, '');
        BaseTaskPopUpIDs = BaseTaskPopUpIDs.replace(upRowVal, selectRowVal + upRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddTaskID').val(BaseTaskPopUpIDs);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서  순서변경(아래로)
***************************************************/
BaseTaskList.TaskDown = function (p_listID) {
    var selectRow = $('#' + p_listID);
    var downRow = selectRow.next();

    selectRow.insertAfter(downRow);

    // IE의 테이블 하단  딜레이현상이 나타나 삽입
    // 이유는 모르겠으나 downRow에 effect를 넣어주면 해당 현상이 사라짐
    downRow.stop();

    if (downRow.html() != null) {
        //파라미터에 저장된 문서번호 순서 변경
        var selectRowVal = '|' + p_listID + ';';
        var downRowVal = '|' + downRow.attr("id") + ';';

        BaseTaskPopUpIDs = BaseTaskPopUpIDs.replace(selectRowVal, '');
        BaseTaskPopUpIDs = BaseTaskPopUpIDs.replace(downRowVal, downRowVal + selectRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddTaskID').val(BaseTaskPopUpIDs);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서 문서 삭제
***************************************************/
BaseTaskList.TaskDel = function (p_listID) {
    BaseTaskPopUpIDs = BaseTaskPopUpIDs.split("|").join("");  // replaceAll
    BaseTaskPopUpIDs = BaseTaskPopUpIDs.trimEnds(';');
    var BaseTasksStr = BaseTaskPopUpIDs.split(';');
    var newListIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < BaseTasksStr.length; i++) {
        if (BaseTasksStr[i] != p_listID) {
            newListIDs = newListIDs + "|" + BaseTasksStr[i] + ";";
        }
    }

    BaseTaskPopUpIDs = newListIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddTaskID').val(BaseTaskPopUpIDs);

    BaseTaskList.TaskDelRow(p_listID);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 업무협업 근거문서  삭제시 ROW 제거
***************************************************/
BaseTaskList.TaskDelRow = function (p_listID) {
    var row = $("#" + p_listID);

    BaseTaskCnt = BaseTaskCnt - 1;

    row.remove();

    if (BaseTaskCnt == 0) {
        $("#contentTaskTable").append(
             "<tr id='noTaskContent'>"
              + "<td colspan='4' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 저장
***************************************************/
BaseTaskList.Save = function () {
    var rowCnt = $('#contentTaskTable > tbody > tr').length;
    var listBaseHTML = $(".rows").parent().html();

    if (listBaseHTML != null) {
        listBaseHTML = listBaseHTML.replace(/BaseTaskList./gi, 'Common.');

        // 부모창이 문서 작성 페이지에서 벗어났을경우 익스플로러 기본 오류메세지 숨긴 후 창닫기
        try {
            if (window.opener && window.opener.open && !window.opener.closed) {
                //부모창 변수에 선택된 팝업창 변수값 저장
                BaseTasks = BaseTasks + BaseTaskPopUpIDs;
                opener.BaseTasks = BaseTasks;

                //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
                opener.BaseTaskCnt = opener.BaseTaskCnt + rowCnt;

                //부모창 히든박스에 문서번호 넣기 - 개발용
                $(opener.document).find('#tbAddTaskID').val(BaseTasks);

                // ROW 그리기
                $(opener.document).find("#noTaskContent").remove();
                $(opener.document).find("#contentTaskTable").append(listBaseHTML);
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
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 소분류 가져오기 (웹서비스 호출)
***************************************************/
BaseTaskList.GetItemSubList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Common/BaseTaskList.asmx/GetItemSub";

    var selVal = $('#ddlItem').val();

    ParamArray = {
        'itemID': selVal
    };

    var obj = eval("BaseTaskList.SuccessGetItemSubList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 소분류 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
BaseTaskList.SuccessGetItemSubList = function (data, status) {

    $("#ddlItemSub option").remove();
    $("#ddlItemSub").append("<option value='0'>" + R_SubCategory + "</option>");

    for (var obj in data.d) {
        var nameBase = data.d[obj].NameBase;
        var vals = data.d[obj].ItemSubID;

        $("#ddlItemSub").append("<option value='" + vals + "'>" + nameBase + "</option>");
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 상태값 사용자 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
BaseTaskList.ModStatus = function (_status) {

    var url = "/DeskPlusEIP/Task/Ws/Common/BaseTaskList.asmx/ModStatus";

    ParamArray = {
        'isType': IsType,
        'status': _status
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("BaseTaskList.SuccessModStatus");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 상태값 사용자 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
BaseTaskList.SuccessModStatus = function (data, status) {
    if (data.d == true) {
        
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 호출)
**********************************************************************************************/
BaseTaskList.ModListUser = function () {

    var url = "/DeskPlusEIP/Task/Ws/Common/BaseTaskList.asmx/ModListUser";
    ParamArray = {
        'isType': IsType,
        'pageSize': $("#ddlListCount").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("BaseTaskList.SuccessModListUser");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-15
작성목적 : 메뉴별 사용자 페이징 설정 정보 저장하기 (웹서비스 값 받기)
**********************************************************************************************/
BaseTaskList.SuccessModListUser = function (data, status) {
    if (data.d == true) {
    }
}

