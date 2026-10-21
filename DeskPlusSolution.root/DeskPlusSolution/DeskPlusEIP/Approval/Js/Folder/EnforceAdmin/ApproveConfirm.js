ApproveConfirm = {}

$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); self.close(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ApproveConfirm.Confirm(); });

        //입력값 체크하기
        if ($.browser.msie) {
            $('.charsRemaining').text($('#tbOpinion').attr('maxlength'));
            $('#tbOpinion').bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('.charsRemaining').text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ApproveConfirm.MaxLang();
        }

        var msg = (parent.ApprovalType == "S") ? R_EnforceSendMsgApprove : R_EnforceSendMsgReject;
        $("#appmsg").html(msg);

        $("#tbOpinion").focus();
    });
});
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 글자 제한
***************************************************/
ApproveConfirm.MaxLang = function () {

    var onEditCallback = function (remaining) {
        $('.charsRemaining').text(remaining);
        if (remaining > 0) { $(this).css('border', '1px solid #d2d2d2'); }
    }
    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }
    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
**********************************************************************************************/
ApproveConfirm.Confirm = function () {

    $("#tbOpinion", parent.document).val($("#tbOpinion").val().replace(/\n/gi, "<br>"));

    if (parent.Approve.ConfirmFunc) parent.Approve.ConfirmFunc();
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}
