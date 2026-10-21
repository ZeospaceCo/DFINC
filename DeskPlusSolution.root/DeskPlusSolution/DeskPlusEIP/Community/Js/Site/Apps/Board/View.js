View = {}

$(function () {

    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
        });

        //답변
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Reply();
        });

        //조회
        $('#cbtnListView').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });


        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.List();
        });

        //짧은답글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        //이미지 보기
        $('#cbtnShowImage').click(function (e) {
            e.preventDefault();
            View.ShowImage();
        });

        //이슈지정
        $('#cbtnIssue').click(function (e) {
            e.preventDefault();
            View.Issue();
        });

        // 추천하기
        $('#cbtnRecom').click(function (e) {
            e.preventDefault();
            View.Recommend();
        });

        View.GetShortReplyList();
        View.GetAttachFileList();
        View.Resource();

        $("#zeoModalContact").draggable({ disabled: false });

        //큰 이미지 줄이기
        $(".shrink-me").resize({ maxWidth: 800 });
    });
});

/**************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Mod); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }
    
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }


}

/**************************************************
작성자   : shine
작성일자 : 2012-05-25
작성목적 : 추가
***************************************************/
View.Add = function () {
    
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'listType': ListType,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 목록
***************************************************/
View.List = function () {

    //팝업창일 경우,
    if (Popup == "1") {
        self.close();
        return;
    }

    var url = "List.aspx";

    if (ListType == "Album") {
        url = "ListGallery.aspx";
    }

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listType': ListType,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location.href = url + "?p=" + GetEncryptValue;

}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
View.Del = function () {
    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("LBTNProcess", "DEL");
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 조회현황
***************************************************/
View.ReadList = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Community/Site/Apps/Board/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 짧은 답글
***************************************************/
View.AddShortReply = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'grade': Grade,
        'listType': ListType
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'screenShot': ''
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 짧은 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   :  shine
작성일자 : 2012-05-25
작성목적 : 짧은 답글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    
    $('#placeholder_shortReple').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_shortReple').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-25
작성목적 : 짧은 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (shortReplyID, communityID, menuID, boardID) {

    ParamArray = {
        'shortReplyID': shortReplyID,
        'communityID': communityID,
        'menuID': menuID,
        'boardID': boardID
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 짧은댓글 삭제
**********************************************************************************************/
View.DelReple = function (shortReplyID, userID) {

    if (confirm(R_ReallyApplyDelete)) {

        ParamArray = {
            'communityID': CommunityID,
            'menuID': MenuID,
            'boardID': BoardID,
            'shortReplyID': shortReplyID,
            'userID': userID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 짧은댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 이슈지정
**********************************************************************************************/
View.Issue = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAddIssue");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/View.asmx/AddIssue";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 이슈지정성공
**********************************************************************************************/
View.SuccessAddIssue = function (data, status) {
    if (data.d == true) {
        alert(R_ConfirmIssue);
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 추천하기
**********************************************************************************************/
View.Recommend = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'grade': Grade
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessRecommend");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/View.asmx/AddRecommend";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 추천하기 성공
**********************************************************************************************/
View.SuccessRecommend = function (data, status) {
    if (data.d == true) {
        alert(R_ConfirmRecommend);
    }
    else {
        alert(R_ProcessingError);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 게시물 수정
**********************************************************************************************/
View.Mod = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listType': ListType,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;

}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : 답변
**********************************************************************************************/
View.Reply = function () {
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());

    location.href = "ReplyN.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 이미지 보기
**********************************************************************************************/
View.ShowImage = function () {
    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Community/Site/Apps/Board/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}


