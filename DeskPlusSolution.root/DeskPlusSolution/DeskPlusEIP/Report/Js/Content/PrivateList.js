PrivateList = {}

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
                'targetUserID': TargetUserID,
                'targetDeptID ': TargetDeptID,
                'weeks': Weeks,
                'flag': Flag,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'listType': ListType,
                'order': Order
            };

            CallEncrypt(CallGetParam());

            location.href = "Register.aspx?p=" + GetEncryptValue;
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            PrivateList.Search();
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                PrivateList.Search();
            }
        });

        //년도검색
        $('#ddlYear').change(function (e) {
            e.preventDefault();

            var selYearTxt = $("#ddlYear option:selected").text();

            if (selYearTxt == "▲" || selYearTxt == "▼") {
                PrivateList.LoadYear($('#ddlYear').val());
            }
        });

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        PrivateList.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
PrivateList.Init = function () {
    PrivateList.LoadYear(Years);

    PrivateList.LoadData();
    __addEventListener(document, "click", PrivateList.click);

    $("#zeoModalContact").draggable({ disabled: false });
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 년도 위아래 체크하기
***************************************************/
PrivateList.LoadYear = function (p_year) {
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

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
PrivateList.LoadData = function () {
    //첫리스트(뒷단에서처리) 및 정렬시에는 리스트만 가져오면 됨
    var loadBoardData = function () {
        PrivateList.GetPageList();
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
PrivateList.Search = function () {
    Years = $("#ddlYear").val();
    Months = $("#ddlMonth").val();
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    PrivateList.LoadData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
PrivateList.GetPageList = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/PrivateList.asmx/GetPageList";
    //alert(TotalItemCount);
    //파라미터값 배열에 담기

    ParamArray = {
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    var obj = eval("PrivateList.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
PrivateList.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTPrivateList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");
    PrivateList.Resource();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
PrivateList.Resource = function () {
    $("#r_term")[0].innerHTML = R_Report;
    $("#r_confirm")[0].innerHTML = R_Confirm;
    $("#r_attach")[0].innerHTML = R_Attach;
    $("#r_short")[0].innerHTML = R_Short;
    //$("#r_linked")[0].innerHTML = R_Linked;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".Write") != undefined) { $(".Write").text(R_Write); }
    if ($(".Save") != undefined) { $(".Save").text(R_Save); }
    if ($(".Request") != undefined) { $(".Request").text(R_Request); }
    if ($(".ConfirmCheck") != undefined) { $(".ConfirmCheck").text(R_ConfirmCheck); }

    if ($(".moveuser") != undefined) { $(".moveuser").text(R_MoveUser); }
    if ($(".copyuser") != undefined) { $(".copyuser").text(R_CopyUser); }
    if ($(".movedept") != undefined) { $(".movedept").text(R_MoveDept); }
    if ($(".copydept") != undefined) { $(".copydept").text(R_CopyDept); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".IconPopUp") != undefined) { $(".IconPopUp").attr('src', ImagePath + '/Icon/ico_popup_silver.gif'); }
    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 업무보고 보기, 작성하기
**********************************************************************************************/
PrivateList.View = function (p_ListID, p_StartDateYMD, p_EndDateYMD, p_Flag) {
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
    if (p_Flag == "0") {
        //작성페이지 이동
        location = "Register.aspx?p=" + GetEncryptValue;
    }
    else {
        location = "View.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 의견보기
**********************************************************************************************/
PrivateList.ShortReplyList = function (p_listID) {
    ParamArray = {
        'listID': p_listID
    };

    CallEncrypt(CallGetParam())

   // var url = "ShortReplyList.aspx?p=" + GetEncryptValue;

    //PopupWin(600, 480, url, "", "", true);
    var url = "/DeskPlusEIP/Report/Content/ShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ShortReplyList", "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
PrivateList.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var listID = el.getAttribute("ListID");

    //PrivateList.GetAttachFileListMenu(projectID, menuID, boardID);
    PrivateList.GetAttachFileListMenu(listID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, PrivateList.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, PrivateList.DownLoad, "EXT");
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
PrivateList.DownLoad = function (_e) {
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
PrivateList.TotalDownLoad = function (_e) {
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
PrivateList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": PrivateList.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

PrivateList.GetAttachFileListMenu = function (p_listID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/PrivateList.asmx/GetAttachFileListMenu";
    //var data = "{communityID :" + categoryID + ",listID:" + listID + "}";
    var data = "listID=" + p_listID;
    var obj = eval("PrivateList.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
PrivateList.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}