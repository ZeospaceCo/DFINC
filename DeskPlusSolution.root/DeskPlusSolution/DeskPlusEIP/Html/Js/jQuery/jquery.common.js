/// <reference path="jquery-1.4.2.min.js" />

/*
* zeospace.inc
* http://www.zeospace.com
* Copyright (c) 1999 zeospace
* Dual licensed under the MIT and GPL licenses
*/

var GetEncryptValue = ""; //암호화 파라미터 값
var GetDecryptValue = ""; //복고화 파라미터 값
var GetUCRenderObj = ""; //user control 호출해서 랜더링할 object 명
var ParamArray = []; //파라미터 담는 전역 변수
//var LimitSpeChar = "' \"\ / \\ ; : - + < > ( ) # &"; //특수문자 차단용
var LimitSpeChar = ""; //특수문자 차단용

/**********************************************************************************************
작성목적 : 초기화 하기
**********************************************************************************************/
$(function () {
    $(document).ready(function () {
        BrowserSecurity();
        if (navigator.userAgent.indexOf("Firefox") > -1) { SetInnerTextProperty(); }

        $("#fnm").toggle(function(){
			$('#fnm > i').toggleClass('fa-chevron-left');
            parent.document.getElementsByTagName('frameset')[0].setAttribute('cols','0,*');
        }, function(){
			$('#fnm > i').toggleClass('fa-chevron-left');
			parent.document.getElementsByTagName('frameset')[0].setAttribute('cols','240,*');
        });

    });
});

/**********************************************************************************************
작성목적 : BackSpace 차단하기
**********************************************************************************************/
$(document).keydown(function (e) {
    if (e.keyCode === 8) {
        var element = e.target.nodeName.toLowerCase();
        if ((element != 'input' && element != 'textarea') || $(e.target).attr("readonly"))
        { return false; }
    }
});

/*****************************
* METHOD : 특수문자 존재여부
*****************************/
ExistSpecialChar = function (p_name) {
    //re = /[<>()#{}']/gi;
    //return true;

    var re = /[<>()#{}']/gi;
    if (re.test(p_name)) {
        return false;
    }

    return true;
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

/**************************************************
작성목적 : FireFox 에서 innerText를 사용할수 있게 처리 하는 함수 ByFreeman 20120724
***************************************************/
SetInnerTextProperty = function () {
    if (typeof HTMLElement != "undefined" && typeof HTMLElement.prototype.__defineGetter__ != "undefined") {
        HTMLElement.prototype.__defineGetter__("innerText", function () {
            if (this.textContent) {
                return (this.textContent)
            }
            else {
                var r = this.ownerDocument.createRange();
                r.selectNodeContents(this);
                return r.toString();
            }
        });

        HTMLElement.prototype.__defineSetter__("innerText", function (sText) {
            this.innerHTML = sText
        });
    }
}

/**********************************************************************************************
작성목적 : 드레그 및 우클릭, CTRL + A 차단하기.
**********************************************************************************************/
BrowserSecurity = function () {
    return;

    var omitformtags = ["input", "textarea", "select"]
    omitformtags = omitformtags.join("|")

    function disableselect(e) {
        if (omitformtags.indexOf(e.target.tagName.toLowerCase()) == -1)
            return false
    }

    function reEnable() {
        return true
    }

    if (typeof document.onselectstart != "undefined") {
        document.onselectstart = new Function("return false")
    }
    else {
        document.onmousedown = disableselect
        document.onmouseup = reEnable
    }

    document.ondragstart = new Function('return false');     // 드래그 방지
    document.oncontextmenu = new Function('return false');     // 우클릭 방지
}

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
* 작성자 : shimpark
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
* METHOD : CALL BACK ascx WITH AJAX : 동기형태
* 작성자 : shimpark
*****************************/
AjaxUCService = function (p_ucPath, p_renderObj) {
    GetUCRenderObj = p_renderObj;

    var url = "/DeskPlusEIP/Common/Ws/GetCommon.asmx/AjaxGetUC";

    ParamArray = {
        'ucPath': p_ucPath
    };

    CallEncrypt(CallGetParam());
    var dataJson = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: url,
        async: false,
        data: dataJson,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: eval(SuccessAjaxUCService),
        error: AjaxError
    });
}

/*****************************
* METHOD : 암호화 값 가져오기
* 작성자 : shimpark
*****************************/
SuccessAjaxUCService = function (data, status) {
    $('#' + GetUCRenderObj).html(data.d);
}

/*****************************
* METHOD : error WITH AJAX
* 작성자 : shimpark
*****************************/
var errCnt = 0;
AjaxError = function (request, status, error) {
    if (request.statusText == "Unauthorized") {
        if (errCnt == 0) {
            //alert("로그인 후 일정시간동안 이용하지 않아 자동으로 로그아웃되었습니다.");
            alert("로그인 후 일정시간동안 이용하지 않아 자동으로 로그아웃되었습니다.\n(You have been logged out for a period of time after signing in, not using it.)");
            ++errCnt;
            top.location.href = "/DeskPlusEIP/Default.aspx";
        }
    } else {
        alert(request.statusText);
    }
}

/*****************************
* METHOD : 암호화 메소드 호출
* 작성자 : shimpark
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
* 작성자 : shimpark
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
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 난수 생성
**********************************************************************************************/
Guids = function () {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : 듀얼 모니터일때 위치 적용
**********************************************************************************************/
function FindLeftWindowBoundry() {
    // In Internet Explorer window.screenLeft is the window's left boundry
    if (window.screenLeft) {
        return window.screenLeft;
    }

    // In Firefox window.screenX is the window's left boundry
    if (window.screenX)
        return window.screenX;

    return 0;
}
window.leftWindowBoundry = FindLeftWindowBoundry;

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : 듀얼 모니터일때 위치 적용
**********************************************************************************************/
function FindLeftScreenBoundry() {
    // Check if the window is off the primary monitor in a positive axis
    // X,Y                  X,Y                    S = Screen, W = Window
    // 0,0  ----------   1280,0  ----------
    //     |          |         |  ---     |
    //     |          |         | | W |    |
    //     |        S |         |  ---   S |
    //      ----------           ----------
    if (window.leftWindowBoundry() > window.screen.width) {
        return window.leftWindowBoundry() - (window.leftWindowBoundry() - window.screen.width);
    }

    // Check if the window is off the primary monitor in a negative axis
    // X,Y                  X,Y                    S = Screen, W = Window
    // 0,0  ----------  -1280,0  ----------
    //     |          |         |  ---     |
    //     |          |         | | W |    |
    //     |        S |         |  ---   S |
    //      ----------           ----------
    // This only works in Firefox at the moment due to a bug in Internet Explorer opening new windows into a negative axis
    // However, you can move opened windows into a negative axis as a workaround
    if (window.leftWindowBoundry() < 0 && window.leftWindowBoundry() > (window.screen.width * -1)) {
        return (window.screen.width * -1);
    }

    // If neither of the above, the monitor is on the primary monitor whose's screen X should be 0
    return 0;
}

window.leftScreenBoundry = FindLeftScreenBoundry;

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-24
작성목적 : 듀얼팝업창을 중간에 보이게 띄우기
**********************************************************************************************/
PopupWinOpen = function (url, parm, winName, width, height) {
    var sw = screen.availWidth;
    var sh = screen.availHeight;

    if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
        height = parseInt(height) + parseInt(50);
    }

    px = (sw - width) / 2;
    py = (sh - height) / 2;

    //    var opt = 'top=' + py + ',left=' + px;
    //    opt += ',width=' + width + ',height=' + height + ',menubar=no,location=no,resizable=yes,scrollbars=yes,status=no';

    var opt = "";
    opt = 'top=' + py + ',left=' + px;
    opt += ',width=' + width + ',height=' + height + ',menubar=no,location=yes,resizable=no,scrollbars=yes,status=no';

    var objPopup = null;
    if (parm != "") {
        objPopup = window.open(url + '?' + parm, winName, opt);
        objPopup.focus();
    } else {
        objPopup = window.open(url, winName, opt);
        objPopup.focus();
    }

    if (objPopup == null) {
        // safari 는 window.open 기본적으로 차단되어 있음 (written by shimpark)
        alert("Please allow me to access window open");
        return;
    }

    return objPopup;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-07-04
작성목적 : 듀얼팝업창을 중간에 보이게 띄우기 (속성 제어)
**********************************************************************************************/
PopupWinOpen = function (url, parm, winName, width, height, pro) {
    var sw = screen.availWidth;
    var sh = screen.availHeight;

    if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
        height = parseInt(height) + parseInt(50);
    }

    px = (sw - width) / 2;
    py = (sh - height) / 2;

    //    var opt = 'top=' + py + ',left=' + px;
    //    opt += ',width=' + width + ',height=' + height + ',menubar=no,location=no,resizable=yes,scrollbars=yes,status=no';

    var opt = "";
    opt = 'top=' + py + ',left=' + px;
    //opt += ',width=' + width + ',height=' + height + ',menubar=no,location=yes,resizable=no,scrollbars=yes,status=no';
    opt += ',width=' + width + ',height=' + height + ',' + pro;

    var objPopup = null;

    if (parm != "") {
        objPopup = window.open(url + '?' + parm, winName, opt);
        objPopup.focus();
    } else {
        objPopup = window.open(url, winName, opt);
        objPopup.focus();
    }

    if (objPopup == null) {
        // safari 는 window.open 기본적으로 차단되어 있음 (written by shimpark)
        alert("Please allow me to access window open");
        return;
    }

    return objPopup;
}

GetPostion = function (e) {
    var posx = 0;
    var posy = 0;
    if (!e) var e = window.event;
    if (e.pageX || e.pageY) {
        posx = e.pageX;
        posy = e.pageY;
    }
    else if (e.clientX || e.clientY) {
        posx = e.clientX + document.body.scrollLeft
                                 + document.documentElement.scrollLeft;
        posy = e.clientY + document.body.scrollTop
                                  + document.documentElement.scrollTop;
    }

    return { 'x': posx, 'y': posy };
}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : (모달팝업)듀얼팝업창을 중간에 보이게 띄우기
**********************************************************************************************/
PopupModalWinOpen = function (url, width, height) {
    var sw = screen.availWidth;
    var sh = screen.availHeight;

    px = (sw - width) / 2;
    py = (sh - height) / 2;

    var opt = "";
    opt = 'dialogTop=' + py + 'px;dialogLeft=' + px;
    opt += 'px;dialogWidth:' + width + 'px; dialogHeight:' + height + 'px; center: yes; help: no; resizable: no; status: no; scroll:no';

    objPopup = window.showModalDialog(url, window, opt)
    return objPopup;
}

/*****************************
* METHOD : 동적 js 호출
*****************************/
JsCall = function (p_name) {
    var ran = Math.ceil(Math.random() * 500);
    p_name = p_name + "?v=" + ran;
    document.write("<scr" + "ipt type=\"text/javascript\" src=\"" + p_name + "\"></scr" + "ipt>");
}

/**************************************************
작성목적 : 다국어 파라미터 초기설정
***************************************************/
CallMultiParam = function (p_multiLang, p_callBackPage, p_useName) {
    var nameKo = "";
    var nameEn = "";
    var nameJa = "";
    var nameFr = "";
    var nameZhcn = "";
    var nameZhtw = "";
    var multiName = p_multiLang;

    if (multiName != "" && multiName.indexOf(",") != -1) {
        nameKo = (multiName.split(",")[0] != undefined) ? multiName.split(",")[0] : "";
        nameEn = (multiName.split(",")[1] != undefined) ? multiName.split(",")[1] : "";
        nameJa = (multiName.split(",")[2] != undefined) ? multiName.split(",")[2] : "";
        nameFr = (multiName.split(",")[3] != undefined) ? multiName.split(",")[3] : "";
        nameZhcn = (multiName.split(",")[4] != undefined) ? multiName.split(",")[4] : "";
        nameZhtw = (multiName.split(",")[5] != undefined) ? multiName.split(",")[5] : "";
    }

    if (p_callBackPage == "") p_callBackPage = "GetMultiName";
    //if (p_useName == "") p_useName = "ko,en,ja,fr,zhcn,zhtw";
    if (p_useName == "") p_useName = "ko,en";

    //파라미터값 배열에 담기
    //ko,en,ja,fr,zhcn,zhtw
    ParamArray = {
        'callBackPage': p_callBackPage,
        'useName': p_useName,
        'nameKo': nameKo,
        'nameEn': nameEn,
        'nameJa': nameJa,
        'nameFr': nameFr,
        'nameZhcn': nameZhcn,
        'nameZhtw': nameZhtw
    };

    CallEncrypt(CallGetParam());
}

/**************************************************
작성목적 : 다국이름설정 (모달용)
***************************************************/
CallMultiName = function (p_multiLang, p_callBackPage, p_useName) {
    p_multiLang = p_multiLang.replace(/&/g, '＆');
    p_multiLang = p_multiLang.replace(/"/g, '＂');
    CallMultiParam(p_multiLang, p_callBackPage, p_useName);
    var url = "/DeskPlusEIP/Common/RegisterLanguage.aspx?p=" + GetEncryptValue;
    PopupWin(300, 293, url, "", "", false);
}

/**************************************************
작성목적 : 다국이름설정 (새창용)
***************************************************/
CallMultiNamePopup = function (p_multiLang, p_callBackPage, p_useName) {
    p_multiLang = p_multiLang.replace(/&/g, '＆');
    p_multiLang = p_multiLang.replace(/"/g, '＂');
    CallMultiParam(p_multiLang, p_callBackPage, p_useName);

    var url = "/DeskPlusEIP/Common/RegisterLangPopup.aspx";
    //window.open(url, "CallMultiNamePopup", "", "");
    PopupWinOpen(url, "p=" + GetEncryptValue, "CallMultiNamePopup", 300, 300);
}

/**************************************************
작성목적 : 사용자 정보
***************************************************/
AccountInfo = function (_arg) {
    
    ParamArray = {
        'userID': _arg
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Common/AccountInfo.aspx?p=" + GetEncryptValue;

    PopupWin(500, 340, url, "", "", true);
}

/**************************************************
작성목적 : 사용자 정보 새창 띄우기
***************************************************/
AccountInfoWin = function (_arg) {
    ParamArray = {
        'userID': _arg
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Account/Common/AccountInfo.aspx";

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "AccountInfo", "500", "340")
    //PopupWin(500, 340, url, "", "", true);
}

/**************************************************
작성목적 : 날짜 비교
* 작성자 : shimpark
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

/**************************************************
작성목적 : 날짜 다국어별 가져오기 (숫자 8자리, 다국어)
* 작성자 : shimpark
***************************************************/
GetMultiDate = function (p_year, p_month, p_day, p_preLangReal) {
    var p_month = (p_month < 10) ? '0' + p_month : p_month;
    var p_day = (p_day < 10) ? '0' + p_day : p_day;
    var getDate = p_year + "-" + p_month + "-" + p_day;

    switch (p_preLangReal) {
        case "ko":
            getDate = p_year + "-" + p_month + "-" + p_day;
            break;
        case "en":
            getDate = p_month + "/" + p_day + "/" + p_year;
            break;
        case "en-gb":
            getDate = p_day + "/" + p_month + "/" + p_year;
            break;
        case "ja":
            getDate = p_year + "/" + p_month + "/" + p_day;
            break;
        case "fr":
            getDate = p_day + "/" + p_month + "/" + p_year;
            break;
        case "zh-cn":
            getDate = p_year + "/" + p_month + "/" + p_day;
            break;
        case "zh-tw":
            getDate = p_year + "-" + p_month + "-" + p_day;
            break;
    }

    return getDate;
}

/**********************************************************************************************
작성목적 : 뒷자리 하나 없애기
* 작성자: shimpark
**********************************************************************************************/
String.prototype.trimEnds = function (c) {
    if (c) return this.replace(new RegExp(c.escapeRegExp() + "*$"), '');
    return this.replace(/\s+$/, '');
};

String.prototype.escapeRegExp = function () {
    return this.replace(/[.*+?^${}()|[\]\/\\]/g, "\\$0");
};

/**********************************************************************************************
작성목적 : 특수문자 제거(보안 이슈)
* 작성자: shimpark
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
작성목적 : 날짜 특수문자 제거
* 작성자: shimpark
**********************************************************************************************/
String.prototype.dateCharRemove = function () {
    var vals = this.replace(/[\-]/gi, "");
    vals = vals.replace(/[\/]/gi, "");
    vals = vals.replace(/[\.]/gi, "");

    return vals;
};

/**********************************************************************************************
작성목적 : 모든 문자열을 변경 :replaceText('the','a')
* 작성자: shimpark
**********************************************************************************************/
String.prototype.replaceText = function (find, replaceWith) {
    return this.split(find).join(replaceWith);
};

/**********************************************************************************************
작성목적 : 양쪽 공백 제거
* 작성자: shimpark
**********************************************************************************************/
String.prototype.trimText = function () {
    return jQuery.trim(this);
};

/**********************************************************************************************
작성목적 : 금액 표기
* 작성자: shimpark
**********************************************************************************************/
String.prototype.toMoney = function () {
    var num = this.replaceText();
    while ((/(-?[0-9]+)([0-9]{3})/).test(num)) {
        num = num.replace((/(-?[0-9]+)([0-9]{3})/), "$1,$2");
    }
    return num;
};

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-01-09
작성목적 : 에디터 보안 검사 (ActiveX 에디터 보안강화)
**********************************************************************************************/
EditorSecurity = function (obj) {
    var twe = obj;
    var dom = twe.getDom();

    //var oColls = dom.getElementsByTagName("FORM");
    var oColls = $("FORM", dom);
    if (oColls.length > 0) {
        twe.TagDiet("FORM", 0, 0, 0, 0, 0, 0); //form태그 제거
        alert("보안상 이유로 FORM태그는 자동삭제됩니다.");
    }

    //oColls = dom.getElementsByTagName("SCRIPT");
    oColls = $("SCRIPT", dom);

    if (oColls.length > 0) {
        twe.TagDiet("SCRIPT", 0, 0, 0, 0, 0, 0); //form태그 제거
        alert("보안상 이유로 SCRIPT태그는 자동삭제됩니다.");
    }

    var files = twe.GetLocalFiles();

    if (files != "") {
        var filesList = files.split(";");

        for (i = 0; i < filesList.length; i++) {
            var fileExt = filesList[i].substr(filesList[i].lastIndexOf("."));

            if (fileExt.indexOf("php") > -1 || fileExt.indexOf("asp") > -1 || fileExt.indexOf("aspx") > -1 || fileExt.indexOf("jsp") > -1 || fileExt.indexOf("exe") > -1 || fileExt.indexOf("bat") > -1 || fileExt.indexOf("js") > -1 || fileExt.indexOf("cs") > -1) {
                alert("보안상 이유로 " + fileExt + " 확장자를 포함한 파일은 허용되지 않습니다.");
                return false;
            }
        }
    }

    var links = twe.GetLocalLinks();

    if (links != "") {
        var filesList = files.split(";");
        
        for (i = 0; i < filesList.length; i++) {
            var fileExt = filesList[i].substr(filesList[i].lastIndexOf("."));

            if (fileExt.indexOf("php") > -1 || fileExt.indexOf("asp") > -1 || fileExt.indexOf("aspx") > -1 || fileExt.indexOf("jsp") > -1 || fileExt.indexOf("exe") > -1 || fileExt.indexOf("bat") > -1 || fileExt.indexOf("js") > -1 || fileExt.indexOf("cs") > -1) {
                alert("보안상 이유로 " + fileExt + " 확장자를 포함한 파일은 허용되지 않습니다.");
                return false;
            }
        }
    }

    return true;
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-01-09
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

/**************************************************
작성자   : freeman
작성일자 : 2013-07-25
작성목적 : 파일 첨부 페이지 리턴
***************************************************/
GetAttachPathURL = function () {
    /*
    var url;
    if ($.browser.msie) {
        url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx";
    } else {
        //url = "/DeskPlusEIP/Upload/SilverUpload/Main.aspx";
        url = "/DeskPlusEIP/Upload/JSUpload/Main.aspx";
    }
    */
    var url = "/DeskPlusEIP/Upload/JSUpload/Main.aspx";
    return url;
}

/**************************************************
작성자   : freeman
작성일자 : 2013-11-28
작성목적 : html에 있는 inpput value 값 가져오기 ContentTEXT에 저장할때 input 값이 빠져서 검색이 않됨
***************************************************/
GetInputVal = function (obj) {
    var rtnVal = " ";

    $(':input', obj).each(function (index) {
        rtnVal += this.value + " ";
    });

    return rtnVal;
}


/**************************************************
작성자   : layout_header fix on scroll down 
작성일자 : 2020-07-01
작성목적 : 본문 헤더 버튼영역 픽스
***************************************************/
function isIE() {
    var ua = window.navigator.userAgent; //Check the userAgent property of the window.navigator object
    var msie = ua.indexOf('MSIE '); // IE 10 or older
    var trident = ua.indexOf('Trident/'); //IE 11
    return (msie > 0 || trident > 0);
}

var lastScrollTop  = 0;
jQuery(window).scroll(function(event){
	if (! $('#layout_body').hasClass('write')){
		var scrollTop = $(this).scrollTop();
		if (lastScrollTop > scrollTop ) {
			if (!$('#layout_header').hasClass('scrollon') ){
			  $('#layout_body').css({'margin-top':'63px'})
			  $('#layout_header').css({'position':'fixed','top':'-63px', 'z-index':'10'}).animate({'top':'0' },100).addClass('scrollon');
			};
		}else if (lastScrollTop <= scrollTop && ! isIE() ) {
		  //}else {
		  //console.log('elseif');
		  $('#layout_header').removeAttr('style').removeClass('scrollon');
		  $('#layout_body').removeAttr('style');
		}
		lastScrollTop = scrollTop;
	};
});

