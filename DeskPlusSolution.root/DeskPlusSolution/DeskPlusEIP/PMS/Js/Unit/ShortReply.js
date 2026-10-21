ShortReply = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            ShortReply.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**********************************************************************************************
[저장하기]
**********************************************************************************************/
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
        'projectID': projectID,
        'menuID': menuID,
        'unitID': UnitID,
        'contents': val
    };

    var url = "/DeskPlusEIP/PMS/Ws/Unit/ShortReply.asmx/AddShortReply";
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
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'rqProjectID': rqProjectID,
        'unitID': UnitID,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'status': Status
    };

    CallEncrypt(CallGetParam())
    var url = "View.aspx?p=" + GetEncryptValue;

    parent.View.GetShortReplyList();
    parent.View.Resource();
    parent.disablePopup();
}