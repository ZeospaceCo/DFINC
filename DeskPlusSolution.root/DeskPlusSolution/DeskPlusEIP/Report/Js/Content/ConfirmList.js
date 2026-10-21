ConfirmList = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //작성하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'years': Years,
                'months': Months,
                'days': Days
            };

            CallEncrypt(CallGetParam());

            location.href = "Register.aspx?p=" + GetEncryptValue;
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ConfirmList.Search()
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ConfirmList.Search();
            }
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                ConfirmList.LoadYear($('#ddlYear').val());
            }
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        ConfirmList.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
ConfirmList.Init = function () {
    ConfirmList.LoadYear(Years);
    ConfirmList.LoadData();

    __addEventListener(document, "click", ConfirmList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 년도 위아래 체크하기
***************************************************/
ConfirmList.LoadYear = function (p_year) {
    $('#ddlYear').empty();
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) - 6) + "'>▲</option>");

    for (var i = parseInt(p_year) - 5; i <= parseInt(p_year) - 1; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }

    $('#ddlYear').append("<option value='" + p_year + "' selected='true'>" + p_year + " </option>");

    for (var i = parseInt(p_year) + 1; i <= parseInt(p_year) + 5; i++) {
        $('#ddlYear').append("<option value='" + i + "'>" + i + " </option>");
    }
    $('#ddlYear').append("<option value='" + parseInt(parseInt(p_year) + 6) + "'>▼</option>");
}

/************************************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 비공개 일지의 관리자 여부 체크(비공개 일지의 첨부파일, 의견 열람여부 확인)
*************************************************************************/
ConfirmList.AuthCheck = function (p_userID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/ConfirmList.asmx/AuthCheck";

    ParamArray = {
        'userID': p_userID,
        'useComplete': "1"
    };

    var obj = eval("ConfirmList.SuccessAuthCheck");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/************************************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 비공개 일지의 관리자 여부 체크(비공개 일지의 첨부파일, 의견 열람여부 확인)
*************************************************************************/
ConfirmList.SuccessAuthCheck = function (data, status) {
    Check = data.d;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
ConfirmList.LoadData = function () {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        ConfirmList.GetPageList();
    }

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        loadBoardData();
    }, 100);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 검색
***************************************************/
ConfirmList.Search = function () {
    Years = $("#ddlYear").val();
    Months = $("#ddlMonth").val();
    Weeks = $("#ddlWeek").val();
    Flag = $("#ddlConfirm").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    ConfirmList.LoadData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
ConfirmList.GetPageList = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/ConfirmList.asmx/GetPageList";
    //alert(TotalItemCount);
    //파라미터값 배열에 담기

    ParamArray = {
        'order': Order,
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'targetDeptID': TargetDeptID,
        'week': Weeks,
        'flag': Flag,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    var obj = eval("ConfirmList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ConfirmList.SuccessPageList = function (data, status) {
    $('#placeholderConfirmList').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTConfirmList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderConfirmList').processTemplate(data.d);
    $('#loading').html("");
    ConfirmList.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ConfirmList.Resource = function () {
    $("#r_term")[0].innerHTML = R_Report;
    $("#r_write")[0].innerHTML = R_Writer;
    $("#r_confirm")[0].innerHTML = R_Confirm;
    $("#r_close")[0].innerHTML = R_CloseYN;
    $("#r_attach")[0].innerHTML = R_Attach;
    $("#r_short")[0].innerHTML = R_Short;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".Write") != undefined) { $(".Write").text(R_Writer); }
    if ($(".Save") != undefined) { $(".Save").text(R_Save); }
    if ($(".Request") != undefined) { $(".Request").text(R_Request); }
    if ($(".ConfirmCheck") != undefined) { $(".ConfirmCheck").text(R_ConfirmCheck); }
    if ($(".IsClose") != undefined) { $(".IsClose").text(R_IsClose); }
    if ($(".IsOpen") != undefined) { $(".IsOpen").text(R_IsOpen); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".IconPopUp") != undefined) { $(".IconPopUp").attr('src', ImagePath + '/Icon/ico_popup_silver.gif'); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 게시물 보기
***************************************************/
ConfirmList.View = function (p_projectID, p_menuID, p_boardID) {
    ParamArray = {
        'projectID': p_projectID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
ConfirmList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");
    var userID = el.getAttribute("WriteUserID");
    var close = el.getAttribute("IsClose");

    // 비공개일 경우 관리자 여부 확인
    if (close == "2") {
        ConfirmList.AuthCheck(userID);

        if (Check == 0) {
            alert(R_CheckAdmin);
            return;
        }
    }

    ConfirmList.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, ConfirmList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, ConfirmList.DownLoad, "EXT");
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
ConfirmList.DownLoad = function (_e) {
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
ConfirmList.TotalDownLoad = function (_e) {
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
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
ConfirmList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": ConfirmList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

ConfirmList.GetAttachFileListMenu = function (p_listID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/ConfirmList.asmx/GetAttachFileListMenu";
    //var data = "{communityID :" + categoryID + ",listID:" + listID + "}";
    var data = "listID=" + p_listID;
    var obj = eval("ConfirmList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
ConfirmList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 업무보고 보기, 작성하기
**********************************************************************************************/
ConfirmList.View = function (p_ListID, p_StartDateYMD, p_EndDateYMD, p_Flag, p_IsClose, p_userID) {
    if (p_IsClose == "2") {
        // 비공개 시 관리자 여부 확인
        ConfirmList.AuthCheck(p_userID);

        if (Check == 0) {
            alert(R_CheckAdmin);
            return;
        }
    }

    ParamArray = {
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'targetDeptID ': TargetDeptID,
        'weeks': Weeks,
        'flag': Flag,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'listType': ListType,
        'order': Order,
        'startDate': p_StartDateYMD,
        'endDate': p_EndDateYMD,
        'listID': p_ListID
    };

    CallEncrypt(CallGetParam())

    //미작성글 (0:미작성;1:저장;2:요청;3:완료)
    if (p_Flag == "0" && UserID == p_userID) {
        //작성페이지 이동
        location = "Register.aspx?p=" + GetEncryptValue;
    }
    else if (p_Flag != "0") {
        location = "View.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 확인여부 업데이트(요청->확인)
**********************************************************************************************/
ConfirmList.Confirm = function (p_ListID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/ConfirmList.asmx/Confirm";

    ParamArray = {
        'listID': p_ListID
    };

    var obj = eval("ConfirmList.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
ConfirmList.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        ConfirmList.Search();
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 의견보기
**********************************************************************************************/
ConfirmList.ShortReplyList = function (p_listID, p_userID, p_close) {
    if (p_close == "2") {
        // 비공개 시 관리자 여부 확인
        ConfirmList.AuthCheck(p_userID);

        if (Check == 0) {
            alert(R_CheckAdmin);
            return;
        }
    }

    ParamArray = {
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

    //var url = "ShortReplyList.aspx?p=" + GetEncryptValue;

    //PopupWin(600, 480, url, "", "", true);
    var url = "/DeskPlusEIP/Report/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ShortReplyList", "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}