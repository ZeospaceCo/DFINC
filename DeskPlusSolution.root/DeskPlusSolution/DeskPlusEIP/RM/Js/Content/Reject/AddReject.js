AddReject = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            AddReject.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //입력값 체크하기
        if (navigator.appName == 'Microsoft Internet Explorer') {
            $('.charsRemaining').text($('#textarea').attr('maxlength'));
            $('#textarea').bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('.charsRemaining').text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(maxlength + " limited");
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            AddReject.MaxLang();
        }
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2017-02-28
작성목적 : 글자 제한
***************************************************/
AddReject.MaxLang = function () {
    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2017-02-28
작성목적 : 거부사유 저장
***************************************************/
AddReject.Confirm = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_Add_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋')
        .replace(/[§]/gi, '※').replace(/[=]/gi, '＝');

    ParamArray = {
        'reservationID': ReservationID,
        'contents': escape(val)
    };

    var url = "/DeskPlusEIP/RM/Ws/Content/Reject/AddReject.asmx/AddContent";
    var obj = eval("AddReject.SuccessAddReject");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2017-02-28
작성목적 : 거부사유 저장후 호출 페이지 이동
**********************************************************************************************/
AddReject.SuccessAddReject = function (data, status) {

    if (data.d) {
        if (parent.disablePopup) {
            eval("parent." + CallBackEvent)();
            parent.disablePopup();
        } else {
            eval("opener." + CallBackEvent)();
            self.close();
        }
    }
    else {
        alert(R_ProcessingError);
    }
}