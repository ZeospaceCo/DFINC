var Content = {};


/**********************************************************************************************
작성자   : famo23 
작성일자 : 2012-06-13
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        Content.GetIssueList();
        Content.GetReplyList();
        Content.GetViewList();

        __addEventListener(document, "click", Content.click);

        $("#zeoModalContact").draggable({ disabled: false });

    });
});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 이슈 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetIssueList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Content.asmx/GetIssueList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'grade': Grade             
    };

    var obj = eval("Content.SuccessIssue");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 이슈 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessIssue = function (data, status) {
    $('#phIssueList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Main/JTIssueList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phIssueList').processTemplate(data.d);

    Content.IssueResource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
Content.IssueResource = function () {

    $("#r_number")[0].innerHTML = R_Number;
    $("#r_menu")[0].innerHTML = R_MenuName;
    $("#r_title")[0].innerHTML = R_Title;        
    $("#r_register")[0].innerHTML = R_Registrant;
    $("#r_regDate")[0].innerHTML = R_RegDate;
    $("#r_del")[0].innerHTML = R_Del;
    $(".btn_del").text(R_Del);

    if ($(".NoIssueListData") != undefined) { $(".NoIssueListData").text(R_NoContent); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 이슈 삭제하기 (웹서비스 호출)
**********************************************************************************************/
Content.DelIssue = function (communityID, menuID, boardID) {

    if (window.confirm(R_DelIssue)) {
        var url = "/DeskPlusEIP/Community/Ws/Site/Main/Content.asmx/DelIssue";
        ParamArray = {
            'communityID': communityID,
            'menuID': menuID,
            'boardID': boardID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Content.SuccessDelIssue");

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-13
작성목적 : 이슈 삭제하기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessDelIssue = function (data, status) {
    if (data.d) {
        Content.GetIssueList();
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;        
    }
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14
작성목적 : 댓글 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetReplyList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Content.asmx/GetReplyList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'grade': Grade             
    };

    var obj = eval("Content.SuccessReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**************************************************
작성자   : shine
작성일자 : 2012-06-28
작성목적 : 새창으로 짧은답글리스트 보기
***************************************************/
Content.PopupShort = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Community/Site/Apps/Board/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 조회현황
***************************************************/
Content.ReadList = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID
    };

    CallEncrypt(CallGetParam())

    var url = "../Apps/Board/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14
작성목적 : 댓글 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessReply = function (data, status) {

    $('#phReplyList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Main/JTReplyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phReplyList').processTemplate(data.d);

    Content.ReplyResource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14
작성목적 : 댓글 리스트 메뉴 다국어 처리
**********************************************************************************************/
Content.ReplyResource = function () {

    $("#r_relplyMenu")[0].innerHTML = R_MenuName;
    $("#r_relplyTitle")[0].innerHTML = R_Title;
    $("#r_relplyCnt")[0].innerHTML = R_ViewCount;

    if ($(".NoReplyListData") != undefined) { $(".NoReplyListData").text(R_NoContent); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14 
작성목적 : 조회수 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Content.GetViewList = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Content.asmx/GetViewList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID,
        'grade': Grade             
    };

    var obj = eval("Content.SuccessView");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14
작성목적 : 조회수 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Content.SuccessView = function (data, status) {

    $('#phViewList').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Main/JTViewList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phViewList').processTemplate(data.d);

    Content.ViewResource();
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-14
작성목적 : 조회수 리스트 메뉴 다국어 처리
**********************************************************************************************/
Content.ViewResource = function () {
    $("#r_viewMenu")[0].innerHTML = R_MenuName;
    $("#r_viewTitle")[0].innerHTML = R_Title;
    $("#r_viewCnt")[0].innerHTML = R_ViewCount;

    if ($(".NoViewListData") != undefined) { $(".NoViewListData").text(R_NoContent); }

    //if ($(".IconShort") != undefined) { $(".IconShort").attr('src', ImagePath + '/Icon/icon_comment.gif'); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}
/**********************************************************************************************
작성자   : shine
작성일자 :2012-06-14
작성목적 : 게시물보기
**********************************************************************************************/
Content.View = function (p_communityID, p_menuID, p_boardID) {

    ParamArray = {
        'communityID': p_communityID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'currentPage': '1',
        'searchTerm': '',
        'searchTermValue': '',
        'searchField': '',
        'searchValue': '',
        'order': '',
        'grade': Grade
    };

    CallEncrypt(CallGetParam());

    location.href = "../Apps/Board/View.aspx?p=" + GetEncryptValue;
    
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 클릭 이벤트 분기
***************************************************/
Content.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "attach": Content.ShowAttachContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}


var oContextMenuCtrl = null;

Content.GetAttachFileListMenu = function (p_communityID, p_menuID, p_boardID) {
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/List.asmx/GetAttachFileListMenu";
    //var data = "{communityID :" + categoryID + ",listID:" + listID + "}";
    var data = "communityID=" + p_communityID + "&menuID=" + p_menuID + "&boardID=" + p_boardID;
    var obj = eval("Content.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-22
작성목적 : 첨부파일 리스트 가져오기
**********************************************************************************************/
Content.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Content.ShowAttachContextMenu = function (e) {

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var communityID = el.getAttribute("CommunityID");
    var menuID = el.getAttribute("MenuID");
    var boardID = el.getAttribute("BoardID");

    Content.GetAttachFileListMenu(communityID, menuID, boardID);

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
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Content.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            oItem = oContextMenuCtrl.addItem(attachFileName, Content.DownLoad, "EXT");
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
Content.DownLoad = function (_e) {
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
Content.TotalDownLoad = function (_e) {
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

