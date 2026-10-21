
View = {}

$(function () {
    $(document).ready(function () {

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Closed();
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        View.GetShortReplyList();
        View.Resource();

        $("#zeoModalContact").draggable({ disabled: false });

    });
});

/**********************************************************************************************
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Mod); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}


/**************************************************
작성목적 : 닫기
***************************************************/
View.Closed = function () {
    if (parent.disablePopup) { window.open("about:blank", "_self").close(); return; }
    else if (parent.parent.disablePopup) { parent.PopupClose(); return; }
    else { parent.close(); }
}

/**************************************************
작성목적 : 댓글
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
작성목적 : 댓글  리스트 호출
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
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Apps/Board/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

}

/**********************************************************************************************
작성목적 : 댓글 수정화면 보여주기
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
작성목적 : 댓글 삭제
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
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
}

