View = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.Print({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });

        //짧은 답글
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.ShortReply();
        });

        View.Init();
    });
});

/**********************************************************************************************
초기화
**********************************************************************************************/
View.Init = function () {
    //짧은댓글 리스트
    View.GetShortReplyList();

    //if ($(".IconAttach") != undefined) { $(".IconAttach").attr('src', ImagePath + '/Icon/attach.gif'); }
    View.GetAttachFileList();

    __addEventListener(document, "click", View.click);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 짧은 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'projectID': ProjectID,
        'unitID': UnitID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/PMS/Ws/Unit/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 짧은 답글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/PMS/Jt/Unit/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 짧은 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (p_unitID, p_unitShortReplyID) {
    ParamArray = {
        'projectID': ProjectID,
        'unitID': p_unitID,
        'unitShortReplyID': p_unitShortReplyID
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    /*
    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_download")[0].innerHTML = R_FileDown;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_Content")[0].innerHTML = R_Contents;
    $("#j_RegDate")[0].innerHTML = R_RegDate;

    if ($(".r_mod") != undefined) {
        $(".r_mod").text(R_Modify);
    }

    if ($(".r_del") != undefined) {
        $(".r_del").text(R_Del);
    }
    */

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
[취소]
**********************************************************************************************/
View.Cancel = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'unitID': UnitID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'status': Status
    };

    CallEncrypt(CallGetParam());
    //var url = "Add.aspx?p=" + GetEncryptValue;

    location.href = "JList.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[CONTEXT MENU]
**********************************************************************************************/
View.ShowContextMenu = function () {
    if (AttachFilesInfo == "") {
        return;
    }

    var nMenuLeft, nMenuTop;
    nMenuLeft = event.clientX + PartalMenuGapL;
    nMenuTop = event.clientY + PartalMenuGapT;

    oContextMenu = new ContextMenuCtrl(nMenuLeft - 20, nMenuTop - 160);
    oContextMenu.Init("oContextMenu");
    oContextMenu.clearItem();

    var filesInfoStr = AttachFilesInfo.split(";");
    for (var i = 0; i < filesInfoStr.length; i++) {
        var fileInfoStr = filesInfoStr[i].split("$");

        oContextMenu.AddItem(fileInfoStr[1], "View.NavigateFileUrl('" + fileInfoStr[0] + "','" + fileInfoStr[2] + "')", "");
    }

    oContextMenu.show();
}

/**********************************************************************************************
[파일 다운로드]
**********************************************************************************************/
View.NavigateFileUrl = function (filePath, fileName) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
[작성하기]
**********************************************************************************************/
View.Add = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'currentPage': "1",
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'status': Status
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[인쇄 하기]
**********************************************************************************************/
View.Print = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
[짧은 답글]
**********************************************************************************************/
View.ShortReply = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'unitID': UnitID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'status': Status
    };

    CallEncrypt(CallGetParam());
    var url = "ShortReply.aspx?p=" + GetEncryptValue;

    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
[수정]
**********************************************************************************************/
View.Mod = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'unitID': UnitID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'status': Status
    };

    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[삭제]
**********************************************************************************************/
View.Del = function () {
    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("LBTNProcess", "DEL");
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 첨부파일 컨텍스트 메뉴 호출
***************************************************/

/*
View.AttachFile = function(_companyID, _documentID, _versionID) {
AttachFileInfoList = SelectAttachFiles(_companyID, _documentID, _versionID);

if (AttachFileInfoList != "iNULL") {
alert(event);
View.ShowContextMenu(event);
} else {
alert("첨부파일을 가져오는 동안 오류가 발생하였습니다");
}
}
*/

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
View.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": View.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

View.GetAttachFileListMenu = function (p_projectID, p_unitID) {
    var url = "/DeskPlusEIP/PMS/Ws/Unit/View.asmx/GetAttachFileListMenu";
    //var data = "{categoryID :" + categoryID + ",listID:" + listID + "}";
    var data = "projectID=" + p_projectID + "&unitID=" + p_unitID;

    var obj = eval("View.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
View.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
View.ShowAttachContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var iProjectID = el.getAttribute("ProjectID");
    var iUnitID = el.getAttribute("UnitID");

    View.GetAttachFileListMenu(iProjectID, iUnitID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, View.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, View.DownLoad, "EXT");
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
View.DownLoad = function (_e) {
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
View.TotalDownLoad = function (_e) {
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

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 짧은댓글 삭제
**********************************************************************************************/
View.DelReple = function (p_unitShortReplyID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'projectID': ProjectID,
            'unitID': UnitID,
            'unitShortReplyID': p_unitShortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/PMS/Ws/Unit/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 짧은댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'projectID': ProjectID,
        'unitID': UnitID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/PMS/Ws/Unit/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/PMS/Jt/Unit/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}