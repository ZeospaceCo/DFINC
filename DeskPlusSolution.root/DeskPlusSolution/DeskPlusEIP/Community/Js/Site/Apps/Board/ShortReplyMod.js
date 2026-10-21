ShortReplyMod = {}

$(function () {

    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            ShortReplyMod.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


    });
});

/**************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 짧은 답글 저장
***************************************************/
ShortReplyMod.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋');

    ParamArray = {
        'shortReplyID': ShortReplyID,
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'contents': escape(val)
    };

    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReplyMod.asmx/ModShortReple";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyMod.SuccessModReple");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-29
작성목적 : 댓글 수정 성공시 페이지 표시
**********************************************************************************************/
ShortReplyMod.SuccessModReple = function (data, status) {

    if (parent.disablePopup) { parent.View.GetShortReplyList(); parent.View.Resource(); parent.disablePopup(); }
    else { opener.View.GetShortReplyList(); opener.View.Resource(); self.close(); }
}