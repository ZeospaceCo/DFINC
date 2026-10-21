GetListBase = {}

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
            GetListBase.LoadData("");
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

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListCount = $('#ddlListCount').val();
            GetListBase.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                GetListBase.Search();
            }
        });

        //페이지 카운트
        $('#RblStatus').change(function () {
            SearchStatus = $('#RblStatus input:checked').val();
            GetListBase.Search();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            GetListBase.Search();
        });

        // 기간
        $("#ddlSearchField").change(function (e) {
            e.preventDefault();
            GetListBase.RegDate();
        });

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            GetListBase.Save();
        });

        GetListBase.Init();

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
GetListBase.Init = function () {
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

    GetListBase.LoadData("");
    __addEventListener(document, "click", GetListBase.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 등록일자 활성화 여부
***************************************************/
GetListBase.RegDate = function () {
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
작성자   :
작성일자 :
작성목적 : 리스트 초기화
***************************************************/
GetListBase.LoadData = function (gubun) {
    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadListBaseData = function () {
        GetListBase.GetCount(); //검색 총 카운트 가져오기
        GetListBase.GetPageList(); //검색 리스트 가져오기
    }

    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadListBaseSearchData = function () {
        GetListBase.GetCount(); //검색 총 카운트 가져오기
        GetListBase.GetPageList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
                CurrentPage = selectedPage;
                GetListBase.LoadData("search");
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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색 필드 설정하기
***************************************************/
GetListBase.SettingSearch = function () {
    SearchField = $("#ddlSearchField").val();

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
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
GetListBase.Search = function () {
    var response = GetListBase.SettingSearch();
    if (response == false) return;

    CurrentPage = 1; //1페이지로 이동처리
    GetListBase.LoadData("search");
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 문서함 리스트 호출
**********************************************************************************************/
GetListBase.GetPageList = function () {
    var url = "/DeskPlusEIP/Coop/Ws/Content/Total/List.asmx/GetPageList";

    //파라미터값 배열에 담기
    ParamArray = {
        'totalCount': TotalItemCount,
        'pageSize': ListCount,
        'pageSizePageNo': CurrentPage,
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'order': Order,
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    var obj = eval("GetListBase.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 문서함 리스트 표시
**********************************************************************************************/
GetListBase.SuccessPageList = function (data, status) {
    $('#placeholder_listBase').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Request/JTGetListBase.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_listBase').processTemplate(data.d);
    $('#loading').html("");

    GetListBase.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

GetListBase.Resource = function () {
    $("#r_num")[0].innerHTML = R_No;
    $("#r_title")[0].innerHTML = R_Title;
    $("#r_attach")[0].innerHTML = R_AttachName;
    $("#r_status")[0].innerHTML = R_State;
    $("#r_receiver")[0].innerHTML = R_Receiver;
    $("#r_requester")[0].innerHTML = R_Requester;
    $("#r_item")[0].innerHTML = R_First;
    $("#r_itemsub")[0].innerHTML = R_Second;
    $("#r_requestdate")[0].innerHTML = R_RequestDate;
    $("#r_enddate")[0].innerHTML = R_EndDate;
    $("#r_add")[0].innerHTML = "추가";

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".BtnAdd") != undefined) { $(".BtnAdd").attr('src', ImagePath + '/Icon/CheckOut.gif'); }
    if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시물 카운트  호출
**********************************************************************************************/
GetListBase.GetCount = function (_folderName) {
    var url = "/DeskPlusEIP/Coop/Ws/Content/Total/List.asmx/GetPageCount";

    ParamArray = {
        'searchStatus': SearchStatus,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchDateValue': SearchDateValue,
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetListBase.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
GetListBase.SuccessCount = function (data, status) {
    TotalItemCount = data.d;

    var pageCount = $("#pager").pager.refresh(TotalItemCount, ListCount);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 문서 추가
***************************************************/
GetListBase.Add = function (p_listID, p_groupID, p_title) {
    // 부모창에 추가된 근거문서가 있으면 체크
    if (CoopBaseIDs != "") {
        var trimListIDsP = CoopBaseIDs.split("|").join("");  // replaceAll
        trimListIDsP = trimListIDsP.trimEnds(';'); // 부모창
        var selStrParent = trimListIDsP.split(';'); // 부모창

        // 부모창에 이미 추가된 문서인지 체크
        for (i = 0; i < selStrParent.length; i++) {
            if (p_listID == selStrParent[i]) {
                alert(R_Msg_ErrBaseDocList);
                return;
            }
        }
    }

    var trimListIDs = CoopBasePopUpIDs.split("|").join("");  // replaceAll
    trimListIDs = trimListIDs.trimEnds(';'); // 팝업창
    var selStrPopUp = trimListIDs.split(';'); //팝업창

    // 팝업창에서 이미 선택된 문서인지 체크
    for (i = 0; i < selStrPopUp.length; i++) {
        if (p_listID == selStrPopUp[i]) {
            alert(R_Msg_ErrBaseDocList); return;
        }
    }

    //팝업용 파라미터에 추가한 문서번호 저장
    CoopBasePopUpIDs = CoopBasePopUpIDs + '|' + p_listID + ';'

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddCoopID').val(CoopBasePopUpIDs);

    GetListBase.AddRow(p_listID, p_groupID, p_title);
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 추가시 ROW 그리기
***************************************************/
GetListBase.AddRow = function (p_listID, p_groupID, p_title) {
    $('#noCoopContent').remove();

    CoopBaseCnt = CoopBaseCnt + 1;

    var contentHtml = "<tr id='" + p_listID + "' class='rows' name='" + p_listID + "'>"
            + "<td>" + p_title + "</td>"
            + "<td class='center'><a onclick=\"GetListBase.CoopViewer('" + p_listID + "','" + p_groupID + "')\">&nbsp;&nbsp;<img class='BtnView' alt=''></a></td>"
            + "<td class='center'><a onclick=\"GetListBase.CoopDel('" + p_listID + "')\">&nbsp;&nbsp;<img class='BtnDel' alt=''></a></td>"
            + "<td>&nbsp;&nbsp;<a onclick=\"GetListBase.CoopUp('" + p_listID + "')\"><img class='BtnUp' alt=''></a>&nbsp;&nbsp; &nbsp;&nbsp;"
            + "<a onclick=\"GetListBase.CoopDown('" + p_listID + "')\"><img class='BtnDown' alt=''></a></td>"
        + "</tr>";

    $("#contentCoopTable").append(contentHtml);

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
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서  문서 보기
***************************************************/
GetListBase.CoopViewer = function (p_listID, p_groupID) {
    ParamArray = {
        'listID': p_listID,
        'groupID': p_groupID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Coop/Content/Total/ViewPopup.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "CoopView_" + Guids(), "width=1000px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서 순서변경(위로)
***************************************************/
GetListBase.CoopUp = function (p_listID) {
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
        CoopBasePopUpIDs = CoopBasePopUpIDs.replace(selectRowVal, '');
        CoopBasePopUpIDs = CoopBasePopUpIDs.replace(upRowVal, selectRowVal + upRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddCoopID').val(CoopBasePopUpIDs);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서  순서변경(아래로)
***************************************************/
GetListBase.CoopDown = function (p_listID) {
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

        CoopBasePopUpIDs = CoopBasePopUpIDs.replace(selectRowVal, '');
        CoopBasePopUpIDs = CoopBasePopUpIDs.replace(downRowVal, downRowVal + selectRowVal);

        //히든박스에 문서번호 넣기 - 개발용
        $('#tbAddCoopID').val(CoopBasePopUpIDs);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 업무협업 근거문서 문서 삭제
***************************************************/
GetListBase.CoopDel = function (p_listID) {
    CoopBasePopUpIDs = CoopBasePopUpIDs.split("|").join("");  // replaceAll
    CoopBasePopUpIDs = CoopBasePopUpIDs.trimEnds(';');
    var coopBaseIDsStr = CoopBasePopUpIDs.split(';');
    var newListIDs = "";

    //팝업 파라미터에 삭제한 나머지 문서번호 저장
    for (var i = 0; i < coopBaseIDsStr.length; i++) {
        if (coopBaseIDsStr[i] != p_listID) {
            newListIDs = newListIDs + "|" + coopBaseIDsStr[i] + ";";
        }
    }

    CoopBasePopUpIDs = newListIDs;

    //히든박스에 문서번호 넣기 - 개발용
    $('#tbAddCoopID').val(CoopBasePopUpIDs);

    GetListBase.CoopDelRow(p_listID);
}

/**************************************************
작성자   : shimpark
작성일자 : 2012-06-20
작성목적 : 업무협업 근거문서  삭제시 ROW 제거
***************************************************/
GetListBase.CoopDelRow = function (p_listID) {
    var row = $("#" + p_listID);

    CoopBaseCnt = CoopBaseCnt - 1;

    row.remove();

    if (CoopBaseCnt == 0) {
        $("#contentCoopTable").append(
             "<tr id='noCoopContent'>"
              + "<td colspan='4' class='center'>" + R_NoContent + "</td>"
           + "</tr>"
    	);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2014-08-30
작성목적 : 저장
***************************************************/
GetListBase.Save = function () {
    var rowCnt = $('#contentCoopTable > tbody > tr').length;
    var listBaseHTML = $(".rows").parent().html();

    if (listBaseHTML != null) {
        listBaseHTML = listBaseHTML.replace(/GetListBase./gi, 'Common.');

        // 부모창이 문서 작성 페이지에서 벗어났을경우 익스플로러 기본 오류메세지 숨긴 후 창닫기
        try {
            if (window.opener && window.opener.open && !window.opener.closed) {
                //부모창 변수에 선택된 팝업창 변수값 저장
                CoopBaseIDs = CoopBaseIDs + CoopBasePopUpIDs;
                opener.CoopBaseIDs = CoopBaseIDs;

                //부모창 카운트 변수에 팝업창 카운트 변수값 더하기
                opener.CoopBaseCnt = opener.CoopBaseCnt + rowCnt;

                //부모창 히든박스에 문서번호 넣기 - 개발용
                $(opener.document).find('#tbAddCoopID').val(CoopBaseIDs);

                // ROW 그리기
                $(opener.document).find("#noCoopContent").remove();
                $(opener.document).find("#contentCoopTable").append(listBaseHTML);
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
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
GetListBase.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": GetListBase.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

GetListBase.GetAttachFileListMenu = function (p_listID) {
    var url = "/DeskPlusEIP/Coop/Ws/Content/Total/List.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "listID=" + p_listID;
    var obj = eval("GetListBase.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
GetListBase.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
GetListBase.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    GetListBase.GetAttachFileListMenu(listID);

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

        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, GetListBase.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }

        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];

            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            oItem = oContextMenuCtrl.addItem(attachFileName, GetListBase.DownLoad, "EXT");
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
GetListBase.DownLoad = function (_e) {
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
GetListBase.TotalDownLoad = function (_e) {
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