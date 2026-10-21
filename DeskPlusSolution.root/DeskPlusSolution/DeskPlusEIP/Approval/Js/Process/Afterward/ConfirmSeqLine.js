ConfirmSeqLine = {}


$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ConfirmSeqLine.Confirm(); });
        
    });
});

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
**********************************************************************************************/
ConfirmSeqLine.Confirm = function () {
    var targetInfo = $("input[name='rblSeqList']:checked").val();

    if (window.dialogArguments) {
        window.returnValue = targetInfo;
        self.close();
    } else {
        if (parent.ConfirmSeqLineFunc) parent.ConfirmSeqLineFunc(targetInfo);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

