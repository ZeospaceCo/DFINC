// ------------------------------------------------------------------------------------------------
// - 폼체크 기능 (from ZEOSPACE) 
// ------------------------------------------------------------------------------------------------
/**
* 숫자에 콤마금액 표기
* 
* @param {form object} checkForm 체크할 폼
* @param {function object} alertFunc 경고창을 표시할 함수
*/

(function ($) {
    $.fn.toPrice = function (cipher) {
        var strb, len, revslice;

        strb = $(this).val().toString();
        strb = strb.replace(/,/g, '');
        strb = $(this).getOnlyNumeric();
        strb = parseInt(strb, 10);
        if (isNaN(strb))
            return $(this).val('');

        strb = strb.toString();
        len = strb.length;

        if (len < 4)
            return $(this).val(strb);

        if (cipher == undefined || !isNumeric(cipher))
            cipher = 3;

        count = len / cipher;
        slice = new Array();

        for (var i = 0; i < count; ++i) {
            if (i * cipher >= len)
                break;
            slice[i] = strb.slice((i + 1) * -cipher, len - (i * cipher));
        }

        revslice = slice.reverse();
        return $(this).val(revslice.join(','));
    }

    $.fn.getOnlyNumeric = function (data) {
        var chrTmp, strTmp;
        var len, str;

        if (data == undefined) {
            str = $(this).val();
        }
        else {
            str = data;
        }

        len = str.length;
        strTmp = '';

        for (var i = 0; i < len; ++i) {
            chrTmp = str.charCodeAt(i);
            if ((chrTmp > 47 || chrTmp <= 31) && chrTmp < 58) {
                strTmp = strTmp + String.fromCharCode(chrTmp);
            }
        }

        if (data == undefined)
            return strTmp;
        else
            return $(this).val(strTmp);
    }

    var isNumeric = function (data) {
        var len, chrTmp;

        len = data.length;
        for (var i = 0; i < len; ++i) {
            chrTmp = str.charCodeAt(i);
            if ((chrTmp <= 47 && chrTmp > 31) || chrTmp >= 58) {
                return false;
            }
        }

        return true;
    }

    //cut font string
    //$('.fontCut').cutString({ length: 5 });
    $.fn.cutString = function (options) {
        var defaults = {
            length: 20,
            ellipsisText: "..."
        };

        var options = $.extend(defaults, options);
        return this.each(function () {
            obj = $(this);
            var str = $.trim(obj.text()),
               strLen = str.length,
               optLen = options.length,
               len = 0;
            function cut() {
                for (var i = 0; i < strLen; i++) {
                    len += (str.charCodeAt(i) > 128) ? 2 : 1;
                    if (len > optLen) return str.substring(0, i) + options.ellipsisText;
                }
                return str;
            }
            obj.text(cut());
        });
    };




})(jQuery);


/**
* 숫자만 추출하기
* var strText = getOnlyNumeric($(this).val());
* @param {str} 
*/

var getOnlyNumeric = function (str) {
    var chrTmp, strTmp;
    var len;

    len = str.length;
    strTmp = '';

    for (var i = 0; i < len; ++i) {
        chrTmp = str.charCodeAt(i);
        if ((chrTmp > 47 || chrTmp <= 31) && chrTmp < 58) {
            strTmp = strTmp + String.fromCharCode(chrTmp);
        }
    }

    return strTmp;
}