AddReject = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-11
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            AddReject.Confirm();
        });

        //[닫기]
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2015-05-12
작성목적 : 확인
***************************************************/
AddReject.Confirm = function () {

    if (window.confirm(R_ShallWeReject)) {

        if (parent.disablePopup) {
            parent.$("#tbOpinion").val($("#tbOpinion").val().replace(/\n/gi, "<br>"));
            eval("parent." + CallBackEvent)();
            parent.disablePopup();
        } else {
            opener.$("#tbOpinion").val($("#tbOpinion").val().replace(/\n/gi, "<br>"));
            eval("opener." + CallBackEvent)();
            self.close();
        }
    }
}
