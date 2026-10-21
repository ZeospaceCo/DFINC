if (typeof DeskPlusEIP == "undefined") { var DeskPlusEIP = {}; };
//******************************************* Check browser *********************************************//
var __Op7Up, __Op6Dn, __IE4Up, __IE4, __IE5, __IE6, __IE7, __NN4, __UA = navigator.userAgent.toLowerCase();
if (window.opera) { var i = __UA.indexOf('opera'); if (i != -1) { var v = parseInt(__UA.charAt(i + 6)); __Op7Up = v >= 7; __Op6Dn = v < 7; } }
else if (navigator.vendor != 'KDE' && document.all && __UA.indexOf('msie') != -1) { __IE4Up = parseFloat(navigator.appVersion) >= 4; __IE4 = __UA.indexOf('msie 4') != -1; __IE5 = __UA.indexOf('msie 5') != -1; __IE6 = __UA.indexOf('msie 6') != -1; __IE7 = __UA.indexOf('msie 7') != -1; }
else if (document.layers) { __NN4 = true; }
var __Mac = __UA.indexOf('mac') != -1;
var __FF = __UA.indexOf('firefox') != -1;

//******************************************* Evnet Handelr *********************************************//
//function __addEventListener(e,eT,eL,cap){ if(!(e=__getElementById(e))) return; eT=eT.toLowerCase(); if((!__IE4Up && !__Op7Up) && e==window) { if(eT=='resize') { window.__PCW=__clientWidth(); window.__PCH=__clientHeight(); window.__REL=eL; __resizeEvent(); return; } if(eT=='scroll') { window.__PSL=__scrollLeft(); window.__PST=__scrollTop(); window.__SEL=eL; __scrollEvent(); return; }}var eh='e.on'+eT+'=eL';if(e.addEventListener) e.addEventListener(eT,eL,cap); else if(e.attachEvent) e.attachEvent('on'+eT,eL); else eval(eh);}
function __addEventListener(e, eT, eL, cap) { if (!(e = __getElementById(e))) return; eT = eT.toLowerCase(); if (e.addEventListener) e.addEventListener(eT, eL, cap || false); else if (e.attachEvent) e.attachEvent('on' + eT, eL); else { var o = e['on' + eT]; e['on' + eT] = typeof o == 'function' ? function (v) { o(v); eL(v); } : eL; } }

//function __removeEventListener(e,eT,eL,cap){if(!(e=__getElementById(e))) return;eT=eT.toLowerCase();if((!__IE4Up && !__Op7Up) && e==window) {if(eT=='resize') { window.__REL=null; return; }if(eT=='scroll') { window.__SEL=null; return; }}var eh='e.on'+eT+'=null';if(e.removeEventListener) e.removeEventListener(eT,eL,cap);else if(e.detachEvent) e.detachEvent('on'+eT,eL);else eval(eh);}
function __removeEventListener(e, eT, eL, cap) { if (!(e = __getElementById(e))) return; eT = eT.toLowerCase(); if (e.removeEventListener) e.removeEventListener(eT, eL, cap || false); else if (e.detachEvent) e.detachEvent('on' + eT, eL); else e['on' + eT] = null; }

function __event(evt) {
    var e = evt || window.event; if (!e) return;
    // type
    if (e.type) this.type = e.type;

    // target(srcElement)
    if (e.target) this.target = e.target;
    else if (e.srcElement) this.target = e.srcElement;

    // relatedTarget
    if (e.relatedTarget) this.relatedTarget = e.relatedTarget; else if (e.type == 'mouseover' && e.fromElement) this.relatedTarget = e.fromElement; else if (e.type == 'mouseout') this.relatedTarget = e.toElement;

    // pageX, pageY
    if (__Op6Dn) { this.pageX = e.clientX; this.pageY = e.clientY; } else if (__def(e.pageX, e.pageY)) { this.pageX = e.pageX; this.pageY = e.pageY; } else if (__def(e.clientX, e.clientY)) { this.pageX = e.clientX + __scrollLeft(); this.pageY = e.clientY + __scrollTop(); }

    // offsetX, offsetY
    if (__def(e.offsetX, e.offsetY)) { this.offsetX = e.offsetX; this.offsetY = e.offsetY; } else if (__def(e.layerX, e.layerY)) { this.offsetX = e.layerX; this.offsetY = e.layerY; } else { this.offsetX = this.pageX - __pageX(this.target); this.offsetY = this.pageY - __pageY(this.target); }

    // keyCode
    if (e.keyCode) { this.keyCode = e.keyCode; } else if (__def(e.which) && e.type.indexOf('key') != -1) { this.keyCode = e.which; }

    // shiftKey, ctrlKey, altKey
    this.shiftKey = e.shiftKey; this.ctrlKey = e.ctrlKey; this.altKey = e.altKey;
}
function __resizeEvent() { if (window.__REL) setTimeout('__resizeEvent()', 250); var cw = __clientWidth(), ch = __clientHeight(); if (window.__PCW != cw || window.__PCH != ch) { window.__PCW = cw; window.__PCH = ch; if (window.__REL) window.__REL(); } }
function __scrollEvent() { if (window.__SEL) setTimeout('__scrollEvent()', 250); var sl = __scrollLeft(), st = __scrollTop(); if (window.__PSL != sl || window.__PST != st) { window.__PSL = sl; window.__PST = st; if (window.__SEL) window.__SEL(); } }

//******************************************* Evnet Handelr *********************************************//


//******************************************* Common Property *******************************************//
function __clientWidth() { var w = 0; if (__Op6Dn) w = window.innerWidth; else if (document.compatMode == 'CSS1Compat' && !window.opera && document.documentElement && document.documentElement.clientWidth) w = document.documentElement.clientWidth; else if (document.body && document.body.clientWidth) w = document.body.clientWidth; else if (__def(window.innerWidth, window.innerHeight, document.height)) { w = window.innerWidth; if (document.height > window.innerHeight) w -= 16; } return w; }
function __clientHeight() { var h = 0; if (__Op6Dn) h = window.innerHeight; else if (document.compatMode == 'CSS1Compat' && !window.opera && document.documentElement && document.documentElement.clientHeight) h = document.documentElement.clientHeight; else if (document.body && document.body.clientHeight) h = document.body.clientHeight; else if (__def(window.innerWidth, window.innerHeight, document.width)) { h = window.innerHeight; if (document.width > window.innerWidth) h -= 16; } return h; }

function __scrollLeft(e, bWin) { var offset = 0; if (!__def(e) || bWin || e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { var w = window; if (bWin && e) w = e; if (w.document.documentElement && w.document.documentElement.scrollLeft) offset = w.document.documentElement.scrollLeft; else if (w.document.body && __def(w.document.body.scrollLeft)) offset = w.document.body.scrollLeft; } else { e = __getElementById(e); if (e && __isNum(e.scrollLeft)) offset = e.scrollLeft; } return offset; }
function __scrollTop(e, bWin) { var offset = 0; if (!__def(e) || bWin || e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { var w = window; if (bWin && e) w = e; if (w.document.documentElement && w.document.documentElement.scrollTop) offset = w.document.documentElement.scrollTop; else if (w.document.body && __def(w.document.body.scrollTop)) offset = w.document.body.scrollTop; } else { e = __getElementById(e); if (e && __isNum(e.scrollTop)) offset = e.scrollTop; } return offset; }

function __offsetLeft(e) { if (!(e = __getElementById(e))) return 0; if (__def(e.offsetLeft)) return e.offsetLeft; else return 0; }
function __offsetTop(e) { if (!(e = __getElementById(e))) return 0; if (__def(e.offsetTop)) return e.offsetTop; else return 0; }

function __height(e, h) { if (!(e = __getElementById(e))) return 0; if (__isNum(h)) { if (h < 0) h = 0; else h = Math.round(h); } else h = -1; var css = __def(e.style); if (e == document || e.tagName.toLowerCase() == 'html' || e.tagName.toLowerCase() == 'body') { h = __clientHeight(); } else if (css && __def(e.offsetHeight) && __isStr(e.style.height)) { if (h >= 0) { var pt = 0, pb = 0, bt = 0, bb = 0; if (document.compatMode == 'CSS1Compat') { var gcs = __getComputedStyle; pt = gcs(e, 'padding-top', 1); if (pt !== null) { pb = gcs(e, 'padding-bottom', 1); bt = gcs(e, 'border-top-width', 1); bb = gcs(e, 'border-bottom-width', 1); } else if (__def(e.offsetHeight, e.style.height)) { e.style.height = h + 'px'; pt = e.offsetHeight - h; } } h -= (pt + pb + bt + bb); if (isNaN(h) || h < 0) return; else e.style.height = h + 'px'; } h = e.offsetHeight; } else if (css && __def(e.style.pixelHeight)) { if (h >= 0) e.style.pixelHeight = h; h = e.style.pixelHeight; } return h; }
function __left(e, iX) { if (!(e = __getElementById(e))) return 0; var css = __def(e.style); if (css && __isStr(e.style.left)) { if (__isNum(iX)) e.style.left = iX + 'px'; else { iX = parseInt(e.style.left); if (isNaN(iX)) iX = __getComputedStyle(e, 'left', 1); if (isNaN(iX)) iX = 0; } } else if (css && __def(e.style.pixelLeft)) { if (__isNum(iX)) e.style.pixelLeft = iX; else iX = e.style.pixelLeft; } return iX; }

function __left(e, iX) { if (!(e = __getElementById(e))) return 0; var css = __def(e.style); if (css && __isStr(e.style.left)) { if (__isNum(iX)) e.style.left = iX + 'px'; else { iX = parseInt(e.style.left); if (isNaN(iX)) iX = 0; } } else if (css && __def(e.style.pixelLeft)) { if (__isNum(iX)) e.style.pixelLeft = iX; else iX = e.style.pixelLeft; } return iX; }
function __top(e, iY) { if (!(e = __getElementById(e))) return 0; var css = __def(e.style); if (css && __isStr(e.style.top)) { if (__isNum(iY)) e.style.top = iY + 'px'; else { iY = parseInt(e.style.top); if (isNaN(iY)) iY = 0; } } else if (css && __isNum(e.style.pixelTop)) { if (__isNum(iY)) e.style.pixelTop = iY; else iY = e.style.pixelTop; } return iY; }

function __getBodyWidth() { var cw = __clientWidth(); var sw = window.document.body.scrollWidth; return cw > sw ? cw : sw; }
function __getBodyHeight() { var cw = __clientHeight(); var sw = window.document.body.scrollHeight; return cw > sw ? cw : sw; }

function __pageX(e) { if (!(e = __getElementById(e))) return 0; var x = 0; while (e) { if (__def(e.offsetLeft)) x += e.offsetLeft; e = __def(e.offsetParent) ? e.offsetParent : null; } return x; }
function __pageY(e) { if (!(e = __getElementById(e))) return 0; var y = 0; while (e) { if (__def(e.offsetTop)) y += e.offsetTop; e = __def(e.offsetParent) ? e.offsetParent : null; } return y; }

function __parent(e, bNode) { if (!(e = __getElementById(e))) return null; var p = null; if (!bNode && __def(e.offsetParent)) p = e.offsetParent; else if (__def(e.parentNode)) p = e.parentNode; else if (__def(e.parentElement)) p = e.parentElement; return p; };

function __createElement(type, parent) { var el = null; el = document.createElement(type); if (typeof parent != "undefined") { parent.appendChild(el); } return el; }
function __getComputedStyle(e, p, i) { if (!(e = __getElementById(e))) return null; var s, v = 'undefined', dv = document.defaultView; if (dv && dv.getComputedStyle) { s = dv.getComputedStyle(e, ''); if (s) v = s.getPropertyValue(p); } else if (e.currentStyle) { v = e.currentStyle[xCamelize(p)]; } else return null; return i ? (parseInt(v) || 0) : v; }

//******************************************* Common Property *******************************************//



//******************************************* Common Method *********************************************//
// getElementById
//function $() { var elements = new Array(); for (var i = 0; i < arguments.length; i++) { var element = arguments[i]; if (typeof element == "string") element = document.getElementById(element); if (arguments.length == 1) return element; elements.push(element); } return elements; };
// object 의 undefined 여부 체크
function __def() { for (var i = 0; i < arguments.length; ++i) { if (typeof (arguments[i]) == 'undefined') return false; } return true; }
// 숫자인지 체크
function __isNum() { for (var i = 0; i < arguments.length; ++i) { if (isNaN(arguments[i]) || typeof (arguments[i]) != 'number') return false; } return true; }
// 문자열인지 체크
function __isStr(s) { for (var i = 0; i < arguments.length; ++i) { if (typeof (arguments[i]) != 'string') return false; } return true; }

function __getElementById(e) { if (typeof (e) != 'string') return e; if (document.getElementById) e = document.getElementById(e); else if (document.all) e = document.all[e]; else e = null; return e; }
function __getElementsByTagName(t, _p) { var list = null; t = t || '*'; _p = _p || document; if (__IE4 || __IE5) { if (t == '*') list = _p.all; else list = _p.all.tags(t); } else if (_p.getElementsByTagName) list = _p.getElementsByTagName(t); return list || new Array(); }
function __getParentNode(e, bNode) { if (!(e = __getElementById(e))) return null; var p = null; if (!bNode && __def(e.offsetParent)) p = e.offsetParent; else if (__def(e.parentNode)) p = e.parentNode; else if (__def(e.parentElement)) p = e.parentElement; return p; }

// 쿠키값 세팅하기
function __setCookie(name, value, expire, path) { document.cookie = name + "=" + escape(value) + ((!expire) ? "" : ("; expires=" + expire.toGMTString())) + "; path=" + ((!path) ? "/" : path); }
// 쿠키값 가져오기
function __getCookie(name) { var value = null, search = name + "="; if (document.cookie.length > 0) { var offset = document.cookie.indexOf(search); if (offset != -1) { offset += search.length; var end = document.cookie.indexOf(";", offset); if (end == -1) end = document.cookie.length; value = unescape(document.cookie.substring(offset, end)); } } return value; }
// get방식의 key 값으로 value 값 얻기
function __getUrlParameter(name) { name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]"); var regexS = "[\\?&]" + name + "=([^&#]*)"; var regex = new RegExp(regexS); var results = regex.exec(window.location.href); if (results == null) { return ""; } else { return results[1]; } }
// display 값 바꾸기
function __toggle(o, oImg) { if (o.length) { for (var i = 0; i < o.length; i++) { o[i].style.display = r = (o[i].style.display == "none") ? "" : "none"; } } else { o.style.display = r = (o.style.display == "none") ? "" : "none"; }; if (oImg && oImg.tagName == "IMG") { oImg.src = (r == "none") ? oImg.src.replace("hidden", "visible") : oImg.src.replace("visible", "hidden") }; return r; }

// 클래스 추가하기
function __addClass(el, className) { __removeClass(el, className); el.className += " " + className; };
// 클래스 삭제하기
function __removeClass(el, className) { if (!(el && el.className)) { return; } var cls = el.className.split(" "); var ar = new Array(); for (var i = cls.length; i > 0; ) { if (cls[--i] != className) { ar[ar.length] = cls[i]; } } el.className = ar.join(" "); };

//******************************************* Common Method *********************************************//



DeskPlusEIP.Component = {
    createPopupArea: function (id) {
        sId = (typeof id == "undefined") ? "popup_menu_area" : id;
        if (document.getElementById(sId)) { document.getElementById(sId).style.display = ""; return document.getElementById(sId); }
        oEl = __createElement("DIV");
        if (oEl == null) return null;
        oEl.id = sId;
        if (!document.body) return null;
        return document.body.appendChild(oEl);
    },

    ContextMenuCtrl: function () { this.oEl = null; this.oTHead = null; this.x = 0; this.y = 0; },
    ContextMenuItem: new Array(),

    Calendar: function (dateStr, onSelected, onClose) {
        this.date = null;
        this.selectedDate = null;
        this.dateStr = dateStr;
        this.cal = null;

        this.onSelected = onSelected || null;
        this.onClose = onClose || null;

        this.selectedDate = null;
        this.oneSelectClose = true;

        this.firstDayOfWeek = 0;
        this.lan = "ko";
        this.minYear = 1900;
        this.maxYear = 2090;

        // HTML elements
        this.oEl = null;
        this.table = null;
        this.element = null;
        this.tbody = null;
        this.firstdayname = null;
        this.title = null;
        this.footer = null;
        this.showFooter = false;

        switch (this.lan) {
            case "ko":
                this._MN = Date.LanStr.ko._MN;
                this._SMN = Date.LanStr.ko._SMN;
                this._DN = Date.LanStr.ko._DN;
                this._SDN = Date.LanStr.ko._SDN;
                this._DEF_DATE_FORMAT = Date.LanStr.ko._DEF_DATE_FORMAT;
                this._TT_DATE_FORMAT = Date.LanStr.ko._TT_DATE_FORMAT;
                this._CAL_DATE_FORMAT = Date.LanStr.ko._CAL_DATE_FORMAT;
                break;
            case "en":
                this._MN = Date.LanStr.en._MN;
                this._SMN = Date.LanStr.en._SMN;
                this._DN = Date.LanStr.en._DN;
                this._SDN = Date.LanStr.en._SDN;
                this._DEF_DATE_FORMAT = Date.LanStr.en._DEF_DATE_FORMAT;
                this._TT_DATE_FORMAT = Date.LanStr.en._TT_DATE_FORMAT;
                this._CAL_DATE_FORMAT = Date.LanStr.en._CAL_DATE_FORMAT;
                break;

        }
    }

};

//**************************************** Context Menu *****************************************//
DeskPlusEIP.Component.ContextMenuCtrl.prototype.init = function () { if (this.oEl != null) this.oEl = null; this.oEl = __createElement("DIV"); this.oEl.id = "ContextDIV"; this.oEl.className = "popup"; var oTable = __createElement("TABLE"); var oTH = __createElement("THEAD"); oTable.setAttribute("cellspacing", "0", 0); oTable.setAttribute("cellpadding", "0", 0); oTable.appendChild(oTH); this.oTHead = oTH; this.oEl.appendChild(oTable); /*var ofrm = __createElement("IFRAME"); this.oEl.appendChild(ofrm);*/ };
//*****************  context 메뉴 추가(리스트의 첨부파일 다운로드 용) ************************//
DeskPlusEIP.Component.ContextMenuCtrl.prototype.addItem = function (sTxt, eventHandler, sImgUrl, subItem) {
    oTR = __createElement("TR");

    oTD1 = __createElement("TD"); oTD1.className = "context_item1";
    if (sImgUrl == "EXT") { var ext = sTxt.substring(sTxt.lastIndexOf(".") + 1, sTxt.length); var sUrl = ImagePath + "/Icon/Ext/" + ext + ".gif"; oTD1.innerHTML = "<img width=16 height=16 src=\"" + sUrl + "\" onerror=\"this.src='" + ImagePath + "/Icon/Ext/no.gif'\">"; }
    else { oTD1.innerHTML = (sImgUrl != "") ? "<img src=\"" + sImgUrl + "\">" : "&nbsp;"; }
    //__addEventListener(oTD1, "mouseover", this.over); __addEventListener(oTD1, "mouseout", this.out); 
    oTR.appendChild(oTD1);

    oTD2 = __createElement("TD"); oTD2.innerHTML = sTxt; oTD2.className = "context_item2"; if (__isStr(eventHandler)) oTD2.setAttribute("onclick", eventHandler, 0); else { __addEventListener(oTD2, "click", eventHandler); __addEventListener(oTD2, "click", this.hide); } __addEventListener(oTD2, "mouseover", this.over); __addEventListener(oTD2, "mouseout", this.out); oTR.appendChild(oTD2);
    oTD3 = __createElement("TD"); oTD3.className = "context_item3"; oTD3.innerHTML = __def(subItem) ? "<img src=\"" + ImagePath + " /ContextMenu/arrow.gif\" width=16 height=16>" : "&nbsp;"; __addEventListener(oTD3, "mouseover", this.over); __addEventListener(oTD3, "mouseout", this.out); oTR.appendChild(oTD3);

    if (__def(subItem)) DeskPlusEIP.Component.ContextMenuItem.push({ pObj: this, pNode: oTR, child: subItem, isShow: false }); else DeskPlusEIP.Component.ContextMenuItem.push({ pObj: this, pNode: oTR, child: null, isShow: false }); this.oTHead.appendChild(oTR); return { img: oTD1, txt: oTD2 };
};
//*****************  context 메뉴 추가(업무협업용) ************************//
DeskPlusEIP.Component.ContextMenuCtrl.prototype.addNameItem = function (sTxt, eventHandler, sImgUrl, subItem) {
    oTR = __createElement("TR");

    oTD1 = __createElement("TD"); oTD1.className = "context_item1";
    if (sImgUrl == "EXT") { var ext = sTxt.substring(sTxt.lastIndexOf(".") + 1, sTxt.length); var sUrl = ImagePath + "/Icon/Ext/" + ext + ".gif"; oTD1.innerHTML = "<img width=16 height=16 src=\"" + sUrl + "\" onerror=\"this.src='" + ImagePath + "/Icon/Ext/no.gif'\">"; }
    else { oTD1.innerHTML = (sImgUrl != "") ? "<img src=\"" + sImgUrl + "\">" : "&nbsp;"; }
    //좌측 아이콘 선택시, 이벤트설정하여 삭제기능 추가
    if (__isStr(eventHandler)) oTD1.setAttribute("onclick", eventHandler, 0); else { __addEventListener(oTD1, "click", eventHandler); __addEventListener(oTD1, "click", this.hide); } __addEventListener(oTD1, "mouseover", this.over); __addEventListener(oTD1, "mouseout", this.out);
    __addEventListener(oTD1, "mouseover", this.over); __addEventListener(oTD1, "mouseout", this.out); oTR.appendChild(oTD1);

    oTD2 = __createElement("TD"); oTD2.innerHTML = sTxt; oTD2.className = "context_item2"; if (__isStr(eventHandler)) oTD2.setAttribute("onclick", eventHandler, 0); else { __addEventListener(oTD2, "click", eventHandler); __addEventListener(oTD2, "click", this.hide); } __addEventListener(oTD2, "mouseover", this.over); __addEventListener(oTD2, "mouseout", this.out); oTR.appendChild(oTD2);
    oTD3 = __createElement("TD"); oTD3.className = "context_item3"; oTD3.innerHTML = __def(subItem) ? "<img src=\"" + ImagePath + " /ContextMenu/arrow.gif\" width=16 height=16>" : "&nbsp;"; __addEventListener(oTD3, "mouseover", this.over); __addEventListener(oTD3, "mouseout", this.out); oTR.appendChild(oTD3);

    if (__def(subItem)) DeskPlusEIP.Component.ContextMenuItem.push({ pObj: this, pNode: oTR, child: subItem, isShow: false }); else DeskPlusEIP.Component.ContextMenuItem.push({ pObj: this, pNode: oTR, child: null, isShow: false }); this.oTHead.appendChild(oTR); return { img: oTD1, txt: oTD2 };
};

DeskPlusEIP.Component.ContextMenuCtrl.prototype.over = function (e) {
    evt = new __event(e);
    el = evt.target;
    if (el.tagName.toUpperCase() == "IMG") el = el.parentNode;
    if (el.tagName.toUpperCase() == "TD") {
        oTR = el.parentNode;
        for (var i = 0; i < oTR.childNodes.length; i++) {
            if (i == 0) oTR.childNodes[i].className = "context_item1_over";
            else if (i == (oTR.childNodes.length - 1)) oTR.childNodes[i].className = "context_item3_over";
            else oTR.childNodes[i].className = "context_item2_over";
        }
        var contextMenuItems = DeskPlusEIP.Component.ContextMenuItem;
        for (var j = 0; j < contextMenuItems.length; j++) {
            if (contextMenuItems[j].pNode === oTR) {
                subCtrl = contextMenuItems[j].child;
                if (subCtrl != null) {
                    var div = oTR.parentNode.parentNode.parentNode;
                    var x = __left(div) + parseInt(__offsetLeft(oTR.lastChild)) + 18;
                    var y = __top(div) + parseInt(__offsetTop(oTR.lastChild));

                    subCtrl.show(null, null, x, y);
                    contextMenuItems[j].isShow = true;
                }
                contextMenuItems[j].pObj.oEl.style.display = "";
            }
        }
    }
}
DeskPlusEIP.Component.ContextMenuCtrl.prototype.out = function (e) {
    evt = new __event(e);
    el = evt.target;
    if (el.tagName.toUpperCase() == "IMG") el = el.parentNode;
    if (el.tagName.toUpperCase() == "TD") {
        oTR = el.parentNode;
        for (var i = 0; i < oTR.childNodes.length; i++) {
            if (i == 0) oTR.childNodes[i].className = "context_item1";
            else if (i == (oTR.childNodes.length - 1)) oTR.childNodes[i].className = "context_item3";
            else oTR.childNodes[i].className = "context_item2";
        }

        var contextMenuItems = DeskPlusEIP.Component.ContextMenuItem;
        for (var j = 0; j < contextMenuItems.length; j++) {
            if (contextMenuItems[j].pNode === oTR) {
                if (contextMenuItems[j].isShow) {
                    subCtrl.hide();
                    contextMenuItems[j].isShow = false;
                    break;
                }
            }
        }
    }
}
DeskPlusEIP.Component.ContextMenuCtrl.prototype.addBlank = function () { oTR = __createElement("TR"); oTD = __createElement("TD"); oTD.style.backgroundColor = "#E9E9E9"; oTR.appendChild(oTD); oTD = __createElement("TD"); oTD.setAttribute("colspan", "2", 0); oTD.innerHTML = "<div style='background-color:#CDCDCD;height:1px;'><img height=1 width='100%'></div>"; oTR.appendChild(oTD); this.oTHead.appendChild(oTR); };
DeskPlusEIP.Component.ContextMenuCtrl.prototype.clearItem = function () { menuAra = DeskPlusEIP.Component.createPopupArea(); menuAra.innerHTML = ""; };
DeskPlusEIP.Component.ContextMenuCtrl.prototype.hide = function () { if (this.oEl == undefined) { $("#ContextDIV").remove(); } else { this.oEl.style.display = "none"; } };
DeskPlusEIP.Component.ContextMenuCtrl.prototype.show = function (e, el, _x, _y) {
    var x, y;
    if (e) { evt = new __event(e); if (evt) { x = evt.pageX; y = evt.pageY; }; }
    else if ($(el)) { el = $(el); x = __pageX(el); y = __pageY(el) + el.offsetHeight; if (__IE7 || __IE6) x += 1; }

    this.x = x; this.y = y; this.oEl.style.display = "";
    if (document.ifrmForm) { y += 50; }
    if (_x) x = _x;
    if (_y) y = _y;
    this.oEl.style.position = "absolute"; this.oEl.style.top = y + "px"; this.oEl.style.left = x + "px";
    menuAra = DeskPlusEIP.Component.createPopupArea(); menuAra.appendChild(this.oEl);
};
//**************************************** Context Menu *****************************************//



/// **************************************** popup Context Menu *****************************************//
/// Developer : 
/// date      : 2007-05-16
/// desc      : util class
/// desc      : ie11 에서 사용 안되므로 절대 사용하지 마세요~(이유: window.createPopup();) shimpark

function ContextMenuCtrl(_clientX, _clientY) {
    var oContextDiv = null;
    var oTHead = null;
    var oPopup = null;

    this.Init = function () {

        this.oPopup = window.createPopup();

        if (document.all("ContextDIV")) {
            this.oContextDiv = document.all("ContextDIV")
            this.oTHead = this.oContextDiv.firstChild.firstChild;
            return;
        }

        this.oContextDiv = document.createElement("DIV");
        this.oContextDiv.id = "ContextDIV";
        this.oContextDiv.style.position = "absolute";
        this.oContextDiv.style.top = -1000;
        this.oContextDiv.style.left = -1000;
        this.oContextDiv.style.visibility = "hidden";

        oTable = document.createElement("TABLE");
        //oTable.style.behavior = "url(/DeskPlusEIP/Html/htc/popup.htc)";
        oTable.style.fontSize = "9pt";
        oTable.style.cursor = "hand";
        oTable.style.border = "solid 1px #929292";
        oTable.style.borderBottom = "solid 2px #929292";
        oTable.style.borderRight = "solid 2px #929292";
        oTable.setAttribute("cellspacing", "0", 0);
        oTable.setAttribute("cellpadding", "0", 0);
        oTable.setAttribute("border", "0", 0);
        oTable.setAttribute("onselectstart", "return false", 0);

        this.oTHead = document.createElement("THEAD");
        oTable.appendChild(this.oTHead);

        this.oContextDiv.appendChild(oTable);

        document.body.appendChild(this.oContextDiv);
    }

    //show
    this.show = function () {
        //팝업객체가 생성되지 않으면 실행시키지 않는다.
        w = this.oContextDiv.offsetWidth;
        h = this.oContextDiv.offsetHeight;

        //두개의 아규먼트 넘어오면 넘어온 값으로 좌표를 지정한다.
        if (arguments.length == 2) {
            x = arguments[0];
            y = arguments[1];
        }
        else {
            x = _clientX;
            y = _clientY;
        }

        var items = this.oContextDiv.firstChild.firstChild.children.length;
        if (items > 20) {
            this.oPopup.document.body.style.overflow = "auto";
            this.oPopup.document.body.style.border = "solid 1px #7D7D7D";
            h = 400;
            w += 15;
        }

        this.oPopup.document.body.innerHTML = this.oContextDiv.innerHTML;
        this.oPopup.show(x, y, w, h, document.body);
    }

    //clearItem
    this.clearItem = function () {
        if (this.oTHead) {
            oTRs = this.oTHead.children;
            for (i = oTRs.length - 1; i >= 0; i--) {
                oTRs[i].removeNode(true);
            }
        }
    }

    //AddItem
    this.AddItem = function (sTxt, eventHandler, sImgUrl) {
        oTR = this.oTHead.insertRow();
        oTD = document.createElement("TD");
        oTD.style.paddingLeft = "2px";
        oTD.style.paddingRight = "2px";
        oTD.style.height = "20px";
        oTD.style.width = "25px";
        oTD.style.border = "solid 1px #E9E9E9";
        oTD.style.textAlign = "center";

        if (sImgUrl != "")
            oTD.innerHTML = "<img src=\"" + sImgUrl + "\">";
        else
            oTD.innerHTML = "&nbsp;"

        oTD.setAttribute("onmouseover", "parent.oContextMenu.MouseOver(this)");
        oTD.setAttribute("onmouseout", "parent.oContextMenu.MouseOut(this)");
        oTD.setAttribute("onclick", "parent." + eventHandler + ";if(parent.oContextMenu.oPopup)parent.oContextMenu.oPopup.hide();");
        oTD.style.backgroundColor = "#E9E9E9";
        oTR.appendChild(oTD);

        oTD = document.createElement("TD");
        oTD.style.border = "solid 1px #FFFFFF";
        oTD.style.paddingLeft = "8px";
        oTD.style.paddingRight = "8px";
        oTD.style.paddingTop = "2px";
        oTD.style.height = "20px";
        oTD.nowrap = true;
        //oTD.innerText = sTxt;   // [20070612] By withsuki
        oTD.innerHTML = sTxt;
        oTD.setAttribute("onmouseover", "parent.oContextMenu.MouseOver(this);");
        oTD.setAttribute("onmouseout", "parent.oContextMenu.MouseOut(this)");
        oTD.setAttribute("onclick", "parent." + eventHandler + ";if(parent.oContextMenu.oPopup)parent.oContextMenu.oPopup.hide();");
        oTR.appendChild(oTD);
    }

    // AddBlank   // 구분선 삽입
    this.AddBlank = function () {
        oTR = this.oTHead.insertRow();
        oTD = document.createElement("TD");
        oTD.style.backgroundColor = "#E9E9E9";
        oTD.type = "blank2";
        oTR.appendChild(oTD);

        oTD = document.createElement("TD");
        oTD.type = "blank";
        oTD.innerHTML = "<div style='background-color:#CDCDCD;'><img height=1 width='100%'></div>"
        oTR.appendChild(oTD);
    }


    // Event Handler =================================================================
    this.MouseOver = function (oEl) {
        var objUnknown;
        objUnknown = oEl;
        var oTD = null;

        if (objUnknown.tagName.toUpperCase() == "TD")
            oTD = objUnknown;
        else if (objUnknown.parentElement.tagName.toUpperCase() == "TD")
            oTD = objUnknown.parentElement;

        if (oTD) {
            if (oTD.type != "blank" && oTD.type != "blank2") {
                this.setColor(oTD, 1);
            }
        }
    }

    this.MouseOut = function (oEl) {
        var objUnknown;
        objUnknown = oEl;
        var oTD = null;

        if (objUnknown.tagName.toUpperCase() == "TD")
            oTD = objUnknown;
        else if (objUnknown.parentElement.tagName.toUpperCase() == "TD")
            oTD = objUnknown.parentElement;

        if (oTD) {
            if (oTD.type != "blank" && oTD.type != "blank2") {
                this.setColor(oTD, 0);
            }
        }
    }

    this.setColor = function (oEl, mode) {
        if (mode == 1) {
            oTR = oEl.parentElement;
            oTR.firstChild.style.border = "solid 1px #91ACDB";
            oTR.firstChild.style.borderRight = "solid 1px #C7CEDB";
            oTR.firstChild.style.backgroundColor = "#C7CEDB";

            oTR.lastChild.style.border = "solid 1px #91ACDB";
            oTR.lastChild.style.borderLeft = "solid 1px #C7CEDB";
            oTR.lastChild.style.backgroundColor = "#C7CEDB";
        }
        else if (mode == 0) {
            oTR = oEl.parentElement;
            oTR.firstChild.style.border = "solid 1px #E9E9E9";
            oTR.firstChild.style.backgroundColor = "#E9E9E9";

            oTR.lastChild.style.border = "solid 1px #FFFFFF";
            oTR.lastChild.style.borderLeft = "solid 1px #FFFFFF";
            oTR.lastChild.style.backgroundColor = "#FFFFFF";
        }
    }
    // Event Handler =================================================================
}





//**************************************** Calendar *********************************************//
window._popupCalendar = null;
DeskPlusEIP.Component.Calendar.prototype.show = function (x, y) { if (this.oEl == null) { this.create(); } this.oEl.style.display = ""; this.oEl.style.postion = "absolute"; this.oEl.style.top = y; this.oEl.style.left = x; }
DeskPlusEIP.Component.Calendar.prototype.showAtEvent = function (e) { if (e) { evt = new __event(e); if (evt) { x = evt.pageX; y = evt.pageY; }; } this.show(x, y); };
DeskPlusEIP.Component.Calendar.prototype.showAtElement = function (el) { if ($(el)) { el = $(el); x = __pageX(el); y = __pageY(el) + el.offsetHeight; if (__IE7 || __IE6) x += 0; } this.show(x, y); };
DeskPlusEIP.Component.Calendar.prototype.create = function () {
    if (window._popupCalendar != null) window._popupCalendar.destroy();
    if (this.oEl != null) { this.oEl = null; } this.oEl = __createElement("DIV"); this.oEl.id = "CalendarDIV"; this.oEl.className = "popup";

    this.date = this.dateStr ? new Date(this.dateStr) : new Date();
    this.cal = this;

    var table = __createElement("table", this.oEl);
    table.cellSpacing = 0;
    table.cellPadding = 0;

    __addClass(table, "calendar_main");
    this.table = table;

    var thead = __createElement("thead", table);
    var cell = null;
    var row = null;

    row = __createElement("tr", thead);
    __addClass(row, "calendar_title_row");

    var hh = function (text, colspan, cls, navType, p) {
        cell = __createElement("td", row);
        cell.innerHTML = text;
        if (colspan > 1) cell.colSpan = colspan;
        if (__def(cls)) __addClass(cell, cls);
        if (__def(navType)) {
            cell.setAttribute("navType", navType, 0);
            __addEventListener(cell, "mouseover", p._over_btn);
            __addEventListener(cell, "mouseout", p._out_btn);
            __addEventListener(cell, "click", p._click_btn);
        }
        return cell;
    }

    hh("&#x00ab;", 1, "calendar_button", "prevYear", this);
    hh("&#x2039;", 1, "calendar_button", "prevMonth", this);

    this.title = hh("", 3, "calendar_title_cell");

    hh("&#x203a;", 1, "calendar_button", "nextMonth", this);
    hh("&#x00bb;", 1, "calendar_button", "nextYear", this);


    row = __createElement("tr", thead);
    for (var i = 0; i < 7; i++) {
        cell = __createElement("td", row);
        __addClass(cell, "calendar_day_header");
        if (i == 0) __addClass(cell, "calendar_day_header_holiday");
        if (i == 6) __addClass(cell, "calendar_day_header_weekend");
        cell.innerHTML = "<div unselectable='on'>" + this._SDN[i] + "</div>";
    }

    var tbody = __createElement("tbody", table);
    this.tbody = tbody;

    for (i = 6; i > 0; --i) {
        row = __createElement("tr", tbody);
        __addClass(row, "calendar_week");
        for (var j = 7; j > 0; --j) {
            cell = __createElement("td", row);
            cell.setAttribute("celltype", "day", 0);
            __addClass(cell, "calendar_day");
            __addEventListener(cell, "mouseover", this._over_day);
            __addEventListener(cell, "mouseout", this._out_day);
            __addEventListener(cell, "click", this._cell_click);

        }
    }

    if (this.showFooter) {
        row = __createElement("tr", tbody);
        cell = __createElement("td", row);
        cell.colSpan = "7";
        cell.innerHTML = "&nbsp;";
        __addClass(cell, "calendar_footer_cell");
        this.footer = cell;
    }

    this._init(this.date);
    menuAra = DeskPlusEIP.Component.createPopupArea(); menuAra.appendChild(this.oEl);
    window._popupCalendar = this;
}

DeskPlusEIP.Component.Calendar.prototype._init = function (date) {
    var today = new Date(),
	TY = today.getFullYear(),
	TM = today.getMonth(),
	TD = today.getDate();
    this.title.innerHTML = date.print(this._CAL_DATE_FORMAT);

    var year = date.getFullYear(); var month = date.getMonth(); var mday = date.getDate(); var no_days = date.getMonthDays();
    this.date = new Date(date);

    date.setDate(1);
    var day1 = (date.getDay() - this.firstDayOfWeek) % 7;
    if (day1 < 0) day1 += 7;
    date.setDate(-day1);
    date.setDate(date.getDate() + 1);

    var lunarDate = date.toLunar();

    var setClass = function (cell, _date, iday) {
        __removeClass(cell, "calendar_day_othermonth");
        __removeClass(cell, "calendar_day_today");
        __removeClass(cell, "calendar_day_holiday");
        __removeClass(cell, "calendar_day_weekend");

        thisMonth = (_date.getMonth() == month);
        if (!thisMonth) __addClass(cell, "calendar_day_othermonth");
        if (iday == mday && thisMonth) { __addClass(cell, "calendar_day_today"); };
        if (_date.getDay() == 6 && thisMonth) __addClass(cell, "calendar_day_weekend");
        if (_date.getDay() == 0 && thisMonth) __addClass(cell, "calendar_day_holiday");
        if (chkHoliDay(_date)) { __removeClass(cell, "calendar_day_weekend"); __addClass(cell, "calendar_day_holiday"); }
    }
    var chkHoliDay = function (_date) {
        _month = parseInt(_date.print("%m"), 10);
        _day = parseInt(_date.print("%d"), 10);
        for (var i = 0; i < Date.HoliDay.length; i++) {
            var t = Date.HoliDay[i].split(",");
            var t1 = t[0].split("-");
            if (t1[0] == _month && t1[1] == _day) return true;
        }
        return false;
    }

    var thisMonth = false;
    var row = this.tbody.firstChild;
    for (var i = 0; i < 6; ++i, row = row.nextSibling) {
        var cell = row.firstChild;
        for (var j = 0; j < 7; ++j, cell = cell.nextSibling, date.setDate(iday + 1)) {
            iday = date.getDate();
            cell.innerHTML = iday;
            cell.setAttribute("date", date.print("%y-%m-%d", this.lan), 0);
            setClass(cell, date, iday);
        }
    }
}

DeskPlusEIP.Component.Calendar.prototype.setDate = function (date) { if (!date.equalsTo(this.date)) { this._init(date); } };
DeskPlusEIP.Component.Calendar.prototype._click_btn = function (e) {
    var evt = new __event(e); var el = evt.target; var navType = el.getAttribute("navType");
    var cal = window._popupCalendar;
    var date = new Date(cal.date);
    var year = cal.date.getFullYear();
    var mon = cal.date.getMonth();
    var day = cal.date.getDate();

    function setMonth(m) { var day = date.getDate(); var max = date.getMonthDays(m); if (day > max) { date.setDate(max); } date.setMonth(m); };
    var newDate = null;
    switch (navType) {
        case "prevYear": date.setFullYear(year - 1); break;
        case "nextYear": date.setFullYear(year + 1); break;
        case "prevMonth": if (mon > 0) { setMonth(mon - 1); } else if (year-- > cal.minYear) { date.setFullYear(year); setMonth(11); } break;
        case "nextMonth": if (mon < 11) { setMonth(mon + 1); } else if (year < cal.maxYear) { date.setFullYear(year + 1); setMonth(0); } break;
    }
    cal._init(date);
}
DeskPlusEIP.Component.Calendar.prototype._over_btn = function (e) { var evt = new __event(e); var el = evt.target; var navType = el.getAttribute("navType"); __removeClass(el, "calendar_button"); __addClass(el, "calendar_button_over"); };
DeskPlusEIP.Component.Calendar.prototype._out_btn = function (e) { var evt = new __event(e); var el = evt.target; var navType = el.getAttribute("navType"); __removeClass(el, "calendar_button_over"); __addClass(el, "calendar_button"); };
DeskPlusEIP.Component.Calendar.prototype._cell_click = function (e) {
    var evt = new __event(e); var el = evt.target; var cal = window._popupCalendar; var sDate = el.getAttribute("date"); var date = sDate.toDate(); cal.selectedDate = date;
    if (cal.onSelected) cal.onSelected(cal, date.print(this._DEF_DATE_FORMAT));
    if (cal.oneSelectClose) cal.close();
}
DeskPlusEIP.Component.Calendar.prototype.close = function () { this.oEl.style.display = "none"; }
DeskPlusEIP.Component.Calendar.prototype.destroy = function () { var el = this.oEl.parentNode; el.removeChild(this.oEl); window._popupCalendar = null; };

DeskPlusEIP.Component.Calendar.prototype._over_day = function (e) { var evt = new __event(e); var el = evt.target; if (el.getAttribute("celltype") != "day") return; var row = el.parentNode; var cell = row.firstChild; for (var j = 0; j < 7; ++j, cell = cell.nextSibling) { __removeClass(cell, "calendar_day_out"); if (cell === el) __addClass(cell, "calendar_day_over"); else __addClass(cell, "calendar_day_over_r"); } }
DeskPlusEIP.Component.Calendar.prototype._out_day = function (e) { var evt = new __event(e); var el = evt.target; if (el.getAttribute("celltype") != "day") return; var row = el.parentNode; var cell = row.firstChild; for (var j = 0; j < 7; ++j, cell = cell.nextSibling) { __removeClass(cell, "calendar_day_over"); __removeClass(cell, "calendar_day_over_r"); } }
//**************************************** Calendar *********************************************//

//**************************************** TextBox 서버컨트롤 사용될 함수 (written by 심재운) *********************************************//
DeskPlusEIP.Component.TextBox = {

    f_setZero: function () {
        var obj = event.srcElement;
        if (obj.value == '') {
        }
    },

    f_key: function () {
        var e = event.keyCode;

        if (e == 13) {
            //vbscript function
            keyCheck();
            return;
        }
    },

    f_keyIsNum: function () {
        var e = event.keyCode;
        if (e == 13) {
            //vbscript function
            keyCheck();
            return;
        }

        if ((e >= 48 && e <= 57) || (e >= 96 && e <= 105) || e == 8 || e == 46 || e == 37 || e == 39 || e == 109 || e == 189 || e == 35 || e == 36 || e == 9)
            event.returnValue = true;
        else
            event.returnValue = false;
    },

    f_keyIsNumOnly: function () {
        var e = event.keyCode;

        if ((e >= 48 && e <= 57))
            event.returnValue = true;
        else
            event.returnValue = false;
    },

    f_onKeyManage: function () {
        var e = event.keyCode;
        var Obj = event.srcElement;
        if (e == 37 || e == 39) return;

        DeskPlusEIP.Component.TextBox.remove_minus(Obj);
        DeskPlusEIP.Component.TextBox.remove_zero(Obj);
        Obj.value = DeskPlusEIP.Component.TextBox.trim(Obj.value);
        Obj.value = DeskPlusEIP.Component.TextBox.add_comma(Obj.value);
    },


    f_onTrim: function () {
        var Obj = event.srcElement;
        Obj.value = DeskPlusEIP.Component.TextBox.trim(Obj.value);
    },

    f_onFocusManage: function () {
        var Obj = event.srcElement;
        if (Obj.value == 0) {
            Obj.select();
        }
    },

    trim: function (s) {
        s += ''; // 숫자라도 문자열로 변환
        return s.replace(/^\s*|\s*$/g, '');
    },



    remove_minus: function () {
        var Obj = event.srcElement;
        var oVal = Obj.value.replace(/,/g, '');
        var cut_idx = 0;

        if (oVal.charAt(0) == '-') {
            Obj.value = '-' + Obj.value.replace(/-/g, '');
        }
        else {
            Obj.value = Obj.value.replace(/-/g, '');
        }
    },

    remove_zero: function () {
        var Obj = event.srcElement;
        var oVal = Obj.value.replace(/,/g, '');
        var cut_idx = 0;
        var start_idx = 1;

        if (oVal.charAt(0) == '-') start_idx = 2;

        if (oVal.length > start_idx) {
            for (i = start_idx - 1; i < oVal.length - 1; i++) {
                if (oVal.charAt(i) == '0') cut_idx = i + 1;
                else break;
            }

            oVal = oVal.substring(cut_idx);
            Obj.value = oVal;
        }
    },

    add_comma: function (num) {
        var num_amount = num;
        var fmt_amount = '';
        var minus_flag = '';

        num_amount = num_amount.replace(/,/g, '');

        if (num_amount.charAt(0) == '-') {
            minus_flag = 'Y';
            num_amount = num_amount.substring(1);
        }


        if (num_amount.length > 3) {
            var str1 = num_amount.substring(0, num_amount.length % 3);
            var str2 = num_amount.substring(num_amount.length % 3, num_amount.length);

            if (str1.length != 0) str1 += ',';

            fmt_amount += str1;

            for (i = 0; i < str2.length; i++) {
                if (i % 3 == 0 && i != 0) fmt_amount += ',';
                fmt_amount += str2.charAt(i);
            }
        }
        else {
            fmt_amount = num_amount;
        }

        if (minus_flag == 'Y') fmt_amount = '-' + fmt_amount;

        return fmt_amount;
    }
};

//*************************************************************************************//