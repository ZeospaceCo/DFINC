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
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 짧은 답글 저장
***************************************************/
ShortReplyMod.Confirm = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': BoardID,
        'boardShortReplyID': BoardShortReplyID,
        'contents': $("#textarea").val().replace(/&/g, '＆')
    };
    // alert(ProjectID + "-" + MenuID + "-" + $("#textarea").val() + "-" + BoardID + "-" + BoardShortReplyID)
    var url = "/DeskPlusEIP/PMS/Ws/Board/ShortReplyMod.asmx/ModShortReple";
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReplyMod.SuccessModReple");
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 댓글 Mod 성공시 페이지 표시
**********************************************************************************************/
ShortReplyMod.SuccessModReple = function (data, status) {
    //    parent.View.GetShortReplyList();
    //    parent.View.Resource();
    //    parent.disablePopup();

    if (parent.disablePopup) { parent.View.GetShortReplyList(); parent.View.Resource(); parent.disablePopup(); }
    else { opener.View.GetShortReplyList(); opener.View.Resource(); self.close(); }
}