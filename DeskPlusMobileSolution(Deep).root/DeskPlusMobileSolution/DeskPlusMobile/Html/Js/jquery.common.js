/// <reference path="jquery-1.4.2.min.js" />

/*
* zeospace.inc
* http://www.zeospace.com
* Copyright (c) 1999 zeospace
* Dual licensed under the MIT and GPL licenses
*/


var GetEncryptValue = ""; //암호화 파라미터 값
var GetDecryptValue = ""; //복고화 파라미터 값
var ParamArray = []; //파라미터 담는 전역 변수
var LimitSpeChar = "' \"\ / \\ ; : - + < > ( ) # &"; //특수문자 차단용


/*****************************
* METHOD : CALL BACK WITH AJAX : 동기형태
*****************************/
AjaxService = function (p_url, p_data, p_success) {
    $.ajax({
        type: "POST",
        url: p_url,
        async: false,
        data: p_data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: p_success,
        error: AjaxError
    });
}


/*****************************
* METHOD : CALL BACK WITH AJAX : 비동기형태
*****************************/
AjaxAsyncService = function (p_url, p_data, p_success) {
    $.ajax({
        type: "POST",
        url: p_url,
        async: true,
        data: p_data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: p_success,
        error: AjaxError
    });
}

/*****************************
* METHOD : CALL BACK WITH AJAX : 비동기형태
*****************************/
AjaxAsyncServiceAll = function (p_url, p_data, p_success, p_error) {
    $.ajax({
        type: "POST",
        url: p_url,
        async: true,
        data: p_data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: p_success,
        error: p_error
    });
}


/*****************************
* METHOD : error WITH AJAX
*****************************/
var errCnt = 0;
AjaxError = function (request, status, error) {
    if (request.statusText == "Unauthorized") {
        if (errCnt == 0) {
            //alert("로그인 후 일정시간동안 이용하지 않아 자동으로 로그아웃되었습니다.");
            ++errCnt;
            top.location.href = "/DeskPlusMobile/Portal/LogOut.aspx";
        }
    }
    else if (request.statusText == "error") {
        if (errCnt == 0) {
            //alert("로그인 후 일정시간동안 이용하지 않아 자동으로 로그아웃되었습니다.");
            ++errCnt;
            top.location.href = "/DeskPlusMobile/Portal/LogOut.aspx";
        }
    }
    else {
        //alert(request.statusText);
    }
}



/*****************************
* METHOD : 암호화 메소드 호출
*****************************/
CallEncrypt = function (param) {

    //Base64Encoding 사용 ByFreeman 20120706
    GetEncryptValue = Base64.encode(param);
    GetEncryptValue = GetEncryptValue.replace(/[+]/gi, '~plus~');

    /* TripleDES 사용 (보안 강화)
    //param = encodeURIComponent(param);
    param = escape(param);
    var url = "/DeskplusEIP/Common/Ws/GetCommon.asmx/AjaxGetEncrypt";
    var data = "{data :'" + param + "'}";
    var obj = eval("SuccessCallEncrypt");
    AjaxService(url, data, obj);
    */
}

/*****************************
* METHOD : 암호화 값 가져오기
*****************************/
SuccessCallEncrypt = function (data, status) {
    GetEncryptValue = encodeURIComponent(data.d);
}


/*****************************
* METHOD : json 형태를 파라미터 문자열 반환하기
*****************************/
CallGetParam = function () {

    var response = "";

    $.each(ParamArray, function (key, value) {
        response += key + "=" + escape(value) + "&";
    });

    return response.trimEnds("&");

    /*
    var response = "";

    $.each(ParamArray, function (key, value) {
    response += key + "=" + value + "&";
    });   

    response += "guid=" + Guids();    

    return response;
    //return response.trimEnds("&");
    */
}



/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 난수 생성
**********************************************************************************************/
Guids = function () {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

String.prototype.trimEnds = function (c) {
    if (c) return this.replace(new RegExp(c.escapeRegExp() + "*$"), '');
    return this.replace(/\s+$/, '');
};

String.prototype.escapeRegExp = function () {
    return this.replace(/[.*+?^${}()|[\]\/\\]/g, "\\$0");
};


/*****************************
* METHOD : 동적 js 호출
*****************************/
JsCall = function (p_name) {
    var ran = Math.ceil(Math.random() * 500);
    p_name = p_name + "?v=" + ran;
    document.write("<scr" + "ipt type=\"text/javascript\" src=\"" + p_name + "\"></scr" + "ipt>");
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-04-14
작성목적 : 이벤트 할당 
**********************************************************************************************/

$(document).ready(function () {

    $("p.hd_select").toggle(function () {
        $("p.hd_select").html(R_Menu + " ▲");
        $("#m_menu").show();
    }, function () {
        $("p.hd_select").html(R_Menu + " ▼");
        $("#m_menu").hide();
    });


    if (window.addEventListener) {
        window.addEventListener('load', function () {
            setTimeout(scrollTo, 0, 0, 1);
        }, false);
    }

});


function hideURLbar() {
    window.scrollTo(0, 1);
}

function HideURLBar() {
    if (window.addEventListener) {
        window.addEventListener('load', function () {
            setTimeout(scrollTo, 0, 0, 1);
        }, false);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2010-06-23
작성목적 : 날짜형식 받아 오기  
***************************************************/
function DateDeserialize(dateStr) {
    return eval('new' + dateStr.replace(/\//g, ' '));
}


Format = function (dt, fmt) {
    return fmt.replace(/(Y|y|m|d|h|H|i|s|ap)/gi,
    function ($1) {
        switch ($1) {
            case 'Y': return dt.getFullYear();
            case 'y': return dt.getFullYear().toString().substr(2);
            case 'm':
                m = dt.getMonth() + 1;
                return (m < 10) ? "0" + m : m;
            case 'd':
                d = dt.getDate();
                return (d < 10) ? "0" + d : d;
            case 'h':
                if (dt.getHours() == "0") return "12";
                if (dt.getHours() == "12") return "12";
                (h = dt.getHours() % 12) ? h : 12;
                return (h < 10) ? "0" + h : h;
            case 'H':
                H = dt.getHours();
                return (H < 10) ? "0" + H : H;
            case 'i':
                i = dt.getMinutes();
                return (i < 10) ? "0" + i : i;
            case 's':
                s = dt.getSeconds();
                return (s < 10) ? "0" + s : s;
        }
    }
  );
}


function NoticeShow(arg1, arg2) {
    var notice = "";
    if (arg1 == "!") {
        notice = "<font style='color:#f00;font-weight:bold;'>! </font>";
        $("#msgNotice").css({ 'color': '#f00', 'border': '1px solid #f00', 'padding-left': '5px' });
        $("#msgNotice").html(notice + arg2);
        $("#msgNotice").show();

    }
    else {
        notice = "<font style='color:blue;font-weight:bold;'>* </font>";
        $("#msgNotice").css({ 'color': 'blue', 'border': '1px solid #0000ff', 'padding-left': '5px' });
        $("#msgNotice").html(notice + arg2);
        $("#msgNotice").show();
    }
}

function NoticeHide() {
    $("#msgNotice").hide();
}


/**************************************************
작성자   : withsuki
작성일자 : 2010-06-29
작성목적 : Loading중 처리(아래 객체필요)
<div id="divprg" align="center" style="position:absolute;left:0px;top:0px;z-index:999;width:100%;height:100%;display:none;background:#EFEFEF;opacity:0.5;">
Loading...
</div>
***************************************************/
function LoadingShow(_isComplete) {
    var arr = $("#divprg");
    if (arr == null) return;
    jQuery.each(arr, function () {
        if (_isComplete) {
            arr.fadeIn(350);
        } else {
            arr.fadeOut(350);
        }
    });
}


/**************************************************
작성자   : freeman
작성일자 : 2011-06-29
작성목적 : 로그아웃 처리
***************************************************/
function forcelogout() {
    window.location.replace("../Portal/LogOut.aspx");
    //window.open('about:blank', '_self').close(); //강제로 창 닫기
}


/******************************************************
작성자   : famo23
작성일자 : 2011-12-07
작성목적 : 현재 스크롤 위치 가져 오는 함수
******************************************************/
function getNowScroll() {
    var de = document.documentElement;
    var b = document.body;
    var now = {};

    now.X = document.all ? (!de.scrollLeft ? b.scrollLeft : de.scrollLeft) : (window.pageXOffset ? window.pageXOffset : window.scrollX);
    now.Y = document.all ? (!de.scrollTop ? b.scrollTop : de.scrollTop) : (window.pageYOffset ? window.pageYOffset : window.scrollY);
    return now;
}

/**********************************************************************************************
작성목적 : 날짜 특수문자 제거
**********************************************************************************************/
String.prototype.dateCharRemove = function () {

    var vals = this.replace(/[\-]/gi, "");
    vals = vals.replace(/[\/]/gi, "");
    vals = vals.replace(/[\.]/gi, "");

    return vals;
};

/*****************************
* METHOD : 특수문자 존재여부
*****************************/
ExistSpecialChar = function (p_name) {
    //re = /[<>()#{}']/gi;
    var re = /['"\/\\;:\-\<>+()#&]/;
    if (re.test(p_name)) {
        return false;
    }
    return true;
}



String.prototype.padLeft = function (totalWidth, paddingChar) {
    var paddedValue = new String(this);
    paddedValue = jQuery.trim(paddedValue);

    while (paddedValue.length < totalWidth) {
        paddedValue = paddingChar + paddedValue;
    }

    return paddedValue;
}

/**********************************************************************************************
작성목적 : 특수문자 제거(보안 이슈)
**********************************************************************************************/
String.prototype.securityText = function () {

    var vals = this.replace(/&/g, "＆");
    vals = vals.replace(/=/gi, "＝");

    return vals;

    //var re = /['"\/\\;:\-\<>+()#&]/g;    

    /*
    var re = /=&/g;
    return jQuery.trim(this.replace(re, ""));
    */
};



/**********************************************************************************************
작성목적 : 모든 문자열을 변경 :replaceText('the','a')
**********************************************************************************************/
String.prototype.replaceText = function (find, replaceWith) {
    return this.split(find).join(replaceWith);
};


/**************************************************
작성목적 : 날짜 언어별 변환
***************************************************/
DateYMD = function (p_Date, p_preLang) {

    p_Date = p_Date.replaceText("-", "");

    if (jQuery.trim(p_Date) == "") {
        return "";
    }

    switch (p_preLang) {
        case "ko":
            // "yy-mm-dd";              
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6) + "-" + p_Date.substring(6, 8);
            break;
        case "en":
            // "mm/dd/yy";
            p_Date = p_Date.substring(4, 6) + "/" + p_Date.substring(6, 8) + "/" + p_Date.substring(0, 4);
            break;
        case "en-gb":
            // "dd/mm/yy";
            p_Date = p_Date.substring(6, 8) + "/" + p_Date.substring(4, 6) + "/" + p_Date.substring(0, 4);
            break;
        case "ja":
            // "yy/mm/dd";
            p_Date = p_Date.substring(0, 4) + "/" + p_Date.substring(4, 6) + "/" + p_Date.substring(6, 8);
            break;
        case "fr":
            // "dd/mm/yy";
            p_Date = p_Date.substring(6, 8) + "/" + p_Date.substring(4, 6) + "/" + p_Date.substring(0, 4);
            break;
        case "zh-cn":
            // "yy/mm/dd";
            p_Date = p_Date.substring(0, 4) + "/" + p_Date.substring(4, 6) + "/" + p_Date.substring(6, 8);
            break;
        case "zh-tw":
            // "yy-mm-dd";
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6) + "-" + p_Date.substring(6, 8);
            break;
        default:
            // "yy-mm-dd";              
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6) + "-" + p_Date.substring(6, 8);
            break;
    }

    return p_Date;

}

/**************************************************
작성목적 : 날짜 언어별 변환
***************************************************/
DateYM = function (p_Date, p_preLang) {

    p_Date = p_Date.replaceText("-", "");

    switch (p_preLang) {
        case "ko":
            // "yy-mm";              
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6);
            break;
        case "en":
            // "mm/yy";
            p_Date = p_Date.substring(4, 6) + "/" + p_Date.substring(0, 4);
            break;
        case "en-gb":
            // "mm/yy";
            p_Date = p_Date.substring(4, 6) + "/" + p_Date.substring(0, 4);
            break;
        case "ja":
            // "yy/mm";
            p_Date = p_Date.substring(0, 4) + "/" + p_Date.substring(4, 6);
            break;
        case "fr":
            // "mm/yy";
            p_Date = p_Date.substring(4, 6) + "/" + p_Date.substring(0, 4);
            break;
        case "zh-cn":
            // "yy/mm";
            p_Date = p_Date.substring(0, 4) + "/" + p_Date.substring(4, 6);
            break;
        case "zh-tw":
            // "yy-mm";
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6);
            break;
        default:
            // "yy-mm";              
            p_Date = p_Date.substring(0, 4) + "-" + p_Date.substring(4, 6);
            break;
    }

    return p_Date;

}

/**************************************************
작성목적 : 사용자 정보
***************************************************/
CompareDT = function (p_sDate, p_eDate, p_compare, p_preLang) {
    var dateformat = "yy-mm-dd";

    switch (p_preLang) {
        case "ko":
            dateformat = "yy-mm-dd";
            p_sDate = p_sDate.replaceText("-", "");
            p_eDate = p_eDate.replaceText("-", "");
            p_sDate = Number(p_sDate.substring(0, 4) + p_sDate.substring(4, 6) + p_sDate.substring(6, 8));
            p_eDate = Number(p_eDate.substring(0, 4) + p_eDate.substring(4, 6) + p_eDate.substring(6, 8));
            break;
        case "en":
            dateformat = "mm/dd/yy";
            p_sDate = p_sDate.replaceText("/", "");
            p_eDate = p_eDate.replaceText("/", "");
            p_sDate = Number(p_sDate.substring(6, 8) + p_sDate.substring(0, 4) + p_sDate.substring(4, 6));
            p_eDate = Number(p_eDate.substring(6, 8) + p_eDate.substring(0, 4) + p_eDate.substring(4, 6));
            break;
        case "en-gb":
            dateformat = "dd/mm/yy";
            p_sDate = p_sDate.replaceText("/", "");
            p_eDate = p_eDate.replaceText("/", "");
            p_sDate = Number(p_sDate.substring(6, 8) + p_sDate.substring(4, 6) + p_sDate.substring(0, 4));
            p_eDate = Number(p_eDate.substring(6, 8) + p_eDate.substring(4, 6) + p_eDate.substring(0, 4));
            break;
        case "ja":
            dateformat = "yy/mm/dd";
            p_sDate = p_sDate.replaceText("/", "");
            p_eDate = p_eDate.replaceText("/", "");
            p_sDate = Number(p_sDate.substring(0, 4) + p_sDate.substring(4, 6) + p_sDate.substring(6, 8));
            p_eDate = Number(p_eDate.substring(0, 4) + p_eDate.substring(4, 6) + p_eDate.substring(6, 8));
            break;
        case "fr":
            dateformat = "dd/mm/yy";
            p_sDate = p_sDate.replaceText("/", "");
            p_eDate = p_eDate.replaceText("/", "");
            p_sDate = Number(p_sDate.substring(6, 8) + p_sDate.substring(4, 6) + p_sDate.substring(0, 4));
            p_eDate = Number(p_eDate.substring(6, 8) + p_eDate.substring(4, 6) + p_eDate.substring(0, 4));
            break;
        case "zh-cn":
            dateformat = "yy/mm/dd";
            p_sDate = p_sDate.replaceText("/", "");
            p_eDate = p_eDate.replaceText("/", "");
            p_sDate = Number(p_sDate.substring(0, 4) + p_sDate.substring(4, 6) + p_sDate.substring(6, 8));
            p_eDate = Number(p_eDate.substring(0, 4) + p_eDate.substring(4, 6) + p_eDate.substring(6, 8));
            break;
        case "zh-tw":
            dateformat = "yy-mm-dd";
            p_sDate = p_sDate.replaceText("-", "");
            p_eDate = p_eDate.replaceText("-", "");
            p_sDate = Number(p_sDate.substring(0, 4) + p_sDate.substring(4, 6) + p_sDate.substring(6, 8));
            p_eDate = Number(p_eDate.substring(0, 4) + p_eDate.substring(4, 6) + p_eDate.substring(6, 8));
            break;
        default:
            dateformat = "yy-mm-dd";
            p_sDate = p_sDate.replaceText("-", "");
            p_eDate = p_eDate.replaceText("-", "");
            p_sDate = Number(p_sDate.substring(0, 4) + p_sDate.substring(4, 6) + p_sDate.substring(6, 8));
            p_eDate = Number(p_eDate.substring(0, 4) + p_eDate.substring(4, 6) + p_eDate.substring(6, 8));
            break;
    }

    switch (p_compare) {
        case "equal":
            if (p_sDate == p_eDate) {
                return true;
            }
            break;
        case "not":
            if (p_sDate != p_eDate) {
                return true;
            }
            break;
        case "less":
            if (p_sDate < p_eDate) {
                return true;
            }
            break;
        case "greater":
            if (p_sDate > p_eDate) {
                return true;
            }
            break;
        case "lessequal":
            if (p_sDate <= p_eDate) {
                return true;
            }
            break;
        case "greaterequal":
            if (p_sDate >= p_eDate) {
                return true;
            }
            break;
        default:
            return false;
            break;
    }

    return false;

}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2012-06-25
작성목적 : 글자수 제한하기
**********************************************************************************************/
jQuery.fn.limitMaxlength = function (options) {

    var settings = jQuery.extend({
        attribute: "maxlength",
        onLimit: function () { },
        onEdit: function () { }
    }, options);

    // Event handler to limit the textarea
    var onEdit = function () {
        var textarea = jQuery(this);
        var maxlength = parseInt(textarea.attr(settings.attribute));

        if (textarea.val().length > maxlength) {
            textarea.val(textarea.val().substr(0, maxlength));

            // Call the onlimit handler within the scope of the textarea
            jQuery.proxy(settings.onLimit, this)();
        }

        // Call the onEdit handler within the scope of the textarea
        jQuery.proxy(settings.onEdit, this)(maxlength - textarea.val().length);
    }

    this.each(onEdit);

    return this.keyup(onEdit)
        .keydown(onEdit)
        .focus(onEdit);
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-01-09
작성목적 : 파라미터 암호화
Base64 encode / decode
http://www.webtoolkit.info/
**********************************************************************************************/


var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
					  this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
					  this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    },

    URLEncode: function (string) {
        return escape(this._utf8_encode(string));
    },

    // public method for url decoding
    URLDecode: function (string) {
        return this._utf8_decode(unescape(string));
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

/**********************************************************************************************
작성자   : 이중호
작성일자 : 2018-11-02
작성목적 : 터치 슬라이드 사용을 위한 $.browser 정의
**********************************************************************************************/
jQuery.browser = {}; (function () { jQuery.browser.msie = false; jQuery.browser.version = 0; if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) { jQuery.browser.msie = true; jQuery.browser.version = RegExp.$1; } })();

/**************************************************
작성목적 : html에 있는 inpput value 값 가져오기 ContentTEXT에 저장할때 input 값이 빠져서 검색이 않됨
***************************************************/
GetInputVal = function (obj) {
    var rtnVal = " ";

    $(':input', obj).each(function (index) {
        rtnVal += this.value + " ";
    });

    return rtnVal;
}

/*****************************
* METHOD : 알툴바 태그삭제용
* 사용법 RemoveScriptTag(content);
*****************************/
RemoveScriptTag = function (content) {
    if (content.indexOf("script") != -1) {		//
        content = content.replace(/<script>(.*?)<(\/?)script>/gi, ""); //script 태그삭제<script> ~ </script>
        content = content.replace(/<SCRIPT(.*?)<(\/?)SCRIPT>/gi, ""); //script 태그삭제<SCRPT ~ </SCRIPT>
    }
    return content;
}




function sendZeospaceMessage(message) {

    try {
        var ifMessage = JSON.stringify({ message })
        // console.log("[sendZeospaceMessage] message : ", message);
        // iOS
        if (window.webkit != undefined) {
            if (window.webkit.messageHandlers.zeospace != undefined) {
                window.webkit.messageHandlers.zeospace.postMessage(ifMessage);
            }
        }

    } catch (err) {
        //console.log(err);
    }
}

function openSafari(url) {
    var encoded = encodeURI(url);

    var message = {
        "command": "open_safari",
        "data": {
            "url": encoded
        }
    }
    sendZeospaceMessage(message);
}

function setBadgeIOS(count) {
    var message = {
        "command": "set_badge",
        "data": {
            "count": count
        }
    }
    sendZeospaceMessage(message);
}


function showToast(message) {

    try {
        //var ifMessage = JSON.stringify({ message })
        // console.log("[showToast] message : ", message);
        // iOS
        if (window.webkit != undefined) {
            if (window.webkit.messageHandlers.zeo_alert != undefined) {
                window.webkit.messageHandlers.zeo_alert.postMessage(message);
            }
        }
    } catch (err) {
        //console.log(err);
    }
}



/**
*  alert, confirm 대용 팝업 메소드 정의 <br/>
*  timer : 애니메이션 동작 속도 <br/>
*  alert : 경고창 <br/>
*  confirm : 확인창 <br/>
*  open : 팝업 열기 <br/>
*  close : 팝업 닫기 <br/>
*/
var action_popup = {
    timer: 500,
    confirm: function (txt, callback) {
        if (txt == null || txt.trim() == "") {
            console.warn("confirm message is empty.");
            return;
        } else if (callback == null || typeof callback != 'function') {
            console.warn("callback is null or not function.");
            return;
        } else {
            $(".type-confirm .btn_ok").on("click", function (e) {
                e.preventDefault();
                $(this).unbind("click");
                callback(true);
                action_popup.close(this);
            });
            this.open("type-confirm", txt);
        }
    },

    alert: function (txt) {
        if (txt == null || txt.trim() == "") {
            console.warn("confirm message is empty.");
            return;
        } else {
            this.open("type-alert", txt);
        }
    },

    open: function (type, txt) {
        var popup = $("." + type);
        popup.find(".menu_msg").text(txt);
        $("body").append("<div class='dimLayer'></div>");
        $(".dimLayer").css('height', $(document).height()).attr("target", type);
        popup.fadeIn(this.timer);
    },

    close: function (target) {
        var modal = $(target).closest(".modal-section");
        var dimLayer;
        if (modal.hasClass("type-confirm")) {
            dimLayer = $(".dimLayer[target=type-confirm]");
            $(".type-confirm .btn_ok").unbind("click");
        } else if (modal.hasClass("type-alert")) {
            dimLayer = $(".dimLayer[target=type-alert]")
        } else {
            console.warn("close unknown target.")
            return;
        }
        modal.fadeOut(this.timer);

        setTimeout(function () {
            dimLayer != null ? dimLayer.remove() : "";
        }, this.timer);
    }
}

function sendZeospaceMessageAn(message) {

    try {
        var ifMessage = JSON.stringify({ message })

        // Android
        if (zeospace != undefined) {
            zeospace.postMessage(ifMessage);
        }

    } catch (err) {
        // alert(err);
        console.log(err);
    }
}

function closeApp() {
    var message = {
        "command": "close_app"
    }
    sendZeospaceMessageAn(message);
}
