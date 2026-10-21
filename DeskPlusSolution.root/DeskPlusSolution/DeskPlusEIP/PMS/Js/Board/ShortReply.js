ShortReply = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            ShortReply.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**************************************************
작성자   : shmipark
작성일자 : 2011-04-11
작성목적 : 짧은 답글 저장
***************************************************/
ShortReply.Confirm = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': boardID,
        'contents': escape(val)
    };

    var url = "/DeskPlusEIP/PMS/Ws/Board/ShortReply.asmx/AddShortReply";
    var obj = eval("ShortReply.SuccessAddShortReply");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 add 성공시 페이지 표시
**********************************************************************************************/
ShortReply.SuccessAddShortReply = function (data, status) {
    //    parent.View.GetShortReplyList();
    //    parent.View.Resource();
    //    parent.disablePopup();

    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': boardID
    };

    CallEncrypt(CallGetParam())
    var url = "View.aspx?p=" + GetEncryptValue;

    parent.View.GetShortReplyList();
    parent.View.Resource();
    parent.disablePopup();
}