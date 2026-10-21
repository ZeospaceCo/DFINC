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

        $("#txtCont").focus();
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 답글추가 
***************************************************/
ShortReply.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("txtCont", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    //Sript체크

    ParamArray = { 'quesID': QuesID, 'replyID': ReplyID, 'cont': $("#txtCont").val() };
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ShortReply.ScsAddReply");
    var url = "/DeskPlusEIP/Opinion/Ws/Folder/ShortReply.asmx/AddReply";
    AjaxService(url, data, obj);
}
ShortReply.ScsAddReply = function (data, status) {
    if (parent.disablePopup) { parent.GetReplyList(); parent.disablePopup(); }
    else { opener.GetReplyList(); self.close(); }   
}
