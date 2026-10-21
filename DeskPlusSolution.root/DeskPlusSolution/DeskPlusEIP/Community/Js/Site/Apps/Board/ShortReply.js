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
                    //alert(maxlength + " limited");
                    alert(R_LimitShortReply);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ShortReply.MaxLang();
        }
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-06-22
작성목적 : 글자 제한
***************************************************/
ShortReply.MaxLang = function () {

    var onEditCallback = function (remaining) {
        //$(this).siblings('.charsRemaining').text("Characters remaining: " + remaining);
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
작성자   : shine
작성일자 : 2012-05-25
작성목적 : 짧은 답글 저장
***************************************************/
ShortReply.Confirm = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("textarea", R_ReplyInputContent, true);
    if (!validator.validate()) return;

    var val = $("#textarea").val()
        .replace(/[&]/gi, '＆').replace(/["]/gi, '＂')
        .replace(/[']/gi, '＇').replace(/[(]/gi, '（')
        .replace(/[)]/gi, '）').replace(/[+]/gi, '＋');

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'boardID': BoardID,
        'grade': Grade,
        'listType': ListType,
        'contents': escape(val)
    };

    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Board/ShortReply.asmx/AddShortReply";
    var obj = eval("ShortReply.SuccessAddShortReply");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-05-25
작성목적 : 댓글 add 성공시 페이지 표시
**********************************************************************************************/
ShortReply.SuccessAddShortReply = function (data, status) {
    if (parent.disablePopup) { parent.View.GetShortReplyList(); parent.View.Resource(); parent.disablePopup(); }
    else { opener.View.GetShortReplyList(); opener.View.Resource(); self.close(); }
}