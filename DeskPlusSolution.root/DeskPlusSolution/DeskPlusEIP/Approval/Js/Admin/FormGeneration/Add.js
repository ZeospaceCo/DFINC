Add = {}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide'
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        $(".appTag").contextMenu({ menu: 'appTagPopup', leftButton: true }, function (action, el, pos) { //alert(pos.x + "," + pos.y + "," + pos.docX + "," + pos.docY);
            var objid = el[0].id + "_" + action;    // ex) ZD4_INFO09
            Add.CtxMenuAppTag(objid, action, el, pos);
        }, 247);
        //삭제 (항목삭제)
        $(".tagdel").click(function (e) {
            Add.DelTag();
        });
        //적용
        $(".tagset").click(function (e) {
            Add.SetTag();
        });

        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        /*if (NonActiveX == "T") { $('#ActiveX').click(function (e) { e.preventDefault(); if (confirm(R_Msg_ConfirmEditorTab)) { location.href = "Add.aspx?" + ClientQueryString; } }); 
        } else { $('#NonActiveX').click(function (e) { e.preventDefault(); if (confirm(R_Msg_ConfirmEditorTab)) { location.href = "AddN.aspx?" + ClientQueryString; } }); }*/

        // [인쇄]
        $('#cbtnPrint').click(function (e) { e.preventDefault(); Add.Print(); });
        // [삭제]
        $('#cbtnDel').click(function (e) { e.preventDefault(); Add.Del(); });
        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); Add.Add(); });
        // [닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); Add.Close(); });

        // 서식
        $("#selAttObj").change(function (e) { e.preventDefault(); Add.selAttObjChange(); });

        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenHTML_contents").val());

            editorEl = xfe.getDom();
        }

    });
});

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [등록]
***************************************************/
Add.Add = function () {

    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;
            alert(R_Msg_ErrCont);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
    }

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbFormName", R_Msg_ErrInputName, true);
    validator.checkRequired("ctbNameCode", R_Msg_ErrInputNameCode, true);

    if (!validator.validate()) return;

    //특수문자(^) 체크
    if (!Add.CheckChar()) { alert(R_MSG_SPECIALCHAR); return; }

    //결재라인정보 설정
    Add.SetFormSeq();

    __doPostBack('lbtnSubmit', "ADD");
}
// 특수문자 체크
Add.CheckChar = function () {
    var rtnval = true;
    var chkObj = $("#ctbFormName, #ctbNameCode");
    $.each(chkObj, function (i, o) {
        if ($("#" + o.id).val().indexOf("^") > -1) {
            $("#" + o.id).focus(); rtnval = false;
        }
    });
    return rtnval;
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 결재태그, 결재라인정보 가져오기
***************************************************/
Add.SetFormSeq = function () {

    var arrSeq = new jQuery.Hashtable();
    var arrKey = "";

    // 결재태그정리 (고정)
    var arrObj1 = $('span', editorEl).filter(function () {
        if (this.id) {
            if (/Z{1}[D|H|P|Q|C|R]$/.test($("#" + this.id, editorEl).attr('class')))
                return this;
        }
    });
    // 결재태그정리 (유동)
    var arrObj2 = $('div', editorEl).filter(function () {
        if (this.id) {
            if (/Z{1}[D|H|A|R]$/.test($("#" + this.id, editorEl).attr('class')))
                return this;
        }
    });

    var arrObj = $.merge(arrObj1, arrObj2);         //alert(arrObj.length); return;

    // 결재태그만 가져옴(for.정렬) : 배열필터링
    $.grep(arrObj, function (o, i) {
        if (o.id.indexOf('_INFO') < 0) {
            arrSeq.add(o.id, "");
            arrKey += o.id + ';';
        }
    });
    // 결재태그의 info 정보 처리.
    var tmpinfo = "";
    $.grep(arrObj, function (o, i) {
        if (o.id.toUpperCase().indexOf('_INFO') > 0) {
            if (arrSeq.contains(o.id.substr(0, 3))) {
                tmpinfo = arrSeq.get(o.id.substr(0, 3)) + o.id + ';';
                arrSeq.add(o.id.substr(0, 3), tmpinfo);
            }
        }
    });

    var rtnval = "";
    if (arrKey != "") {
        $.each(arrKey.trimEnds(';').split(';'), function (i, v) {
            // ZD0-ZDO_INFO0;ZDO_INFO0;ZDO_INFO0^ZH-^
            rtnval += v + '-' + arrSeq.get(v).trimEnds(';') + '^';
        });
    }

    $("#tbHiddenSeq").val(rtnval.trimEnds('^'));
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [인쇄]
***************************************************/
Add.Print = function () {
    //    var divToPrint = document.getElementById('ifrmForm');

    //    //스타일정보+내용
    //    var html = "<HTML><LINK rel=stylesheet type=text/css href='/DeskPlusEIP/Approval/Css/Func_ApprovalView.css'>"
    //    html += divToPrint.contentWindow.document.body.outerHTML;
    //    html += "</html>";

    //    newWin = window.open();
    //    newWin.document.write(html);
    //    newWin.location.reload();
    //    newWin.focus();
    //    newWin.print();
    //    newWin.close();
}
/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [삭제]
***************************************************/
Add.Del = function () {
    if (confirm(R_Msg_Del)) {
        __doPostBack('lbtnSubmit', "DEL");
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 중복 태그 검사
***************************************************/
Add.isExistTag = function (_id) {
    if (_id == "") return false;

    if ($("#" + _id, editorEl).length > 0) {
        var tmpobj = $("#" + _id, editorEl);
        if (tmpobj == null) return false;
        //return jQuery.contains(editorEl.documentElement, tmpobj);
        return true;
    }
    return false;
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 정렬
***************************************************/
Add.approvalCellWHFix = function () {
    if (nowEl.tagName == "TD") {
        nowEl.style.width = nowEl.offsetWidth;
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 태그생성-결재태그의 인장태그 지정 시 (span)
***************************************************/
Add.setAppTag = function (_id, _class, _innerText) {                            //alert(_id + "/" + _class + "/" + _innerText);
    var objhtml = "<span id='" + _id + "' class='" + _class + "' contenteditable='false' unselectable='on'>" + _innerText + "</span>";
    Add.approvalCellWHFix();
    if (nowEl) {
        nowEl.insertAdjacentHTML("beforeEnd", objhtml);
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : Tag대체
***************************************************/
Add.ReplaceTag = function () {
    if (tagDic.containsKey(nowEl.id)) {
        if (confirm(R_Msg_Replace)) {
            if ($.browser.msie) {
                nowEl.removeNode(true);
            }
            else {
                nowEl.remove();
            }
            return true;
        }
        else { return false; }
    }
    else { return true; }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 결재태그선택
***************************************************/
Add.CtxMenuAppTag = function (_id, _action, _el, _pos) {
    //alert(_id + "/" + _action);
    var objid = (_action.toUpperCase() == "STMPP") ? _id.replace("_STMPP", "") : _id;
    var dicid = (_action.toUpperCase() == "STMPP") ? _id.replace("_STMPP", "") : _action;
    
    if (nowEl != null) {
        if (!Add.isExistTag(objid)) {
            if (Add.ReplaceTag()) {
                Add.setAppTag(objid, objid.substr(0, 2), tagDic.get(dicid));              // ZD0, ZD, 담당
            }
        }
    } else {
        alert(R_Msg_ErrSelTarget); return;
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 결재태그선택-다중태그
***************************************************/
Add.MultiTag = function (_id) {
    if (nowEl != null) {
        if (!Add.isExistTag(_id)) {
            if (Add.ReplaceTag()) {
                Add.setAppTag(_id, _id.substr(0, 2), tagDic.get(_id));
            }
        }
    } else {
        alert(R_Msg_ErrSelTarget); return;
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 태그생성-단행서식,다행서식 지정 시 (input/div)
***************************************************/
Add.setFieldTag = function (_id, _innerText, _isDiv, _cssClass, _w, _h) {
    if (_id.toUpperCase() == "BODY_ETC")
        _id += "_" + Guids();

    var objh = (_id.toUpperCase() == "EDITOR" && _h == "0") ? "550px" : _h;
    objh = (objh == "0") ? "100%" : _h;
    var objw = (_w == "0") ? "100%" : _w;
    var obja = "left";
    var objv = "top";

    _cssClass = (_cssClass == "") ? "formfieldinput" : _cssClass;
    var inputobj = "<input id='" + _id + "' type='text' class='" + _cssClass + "' style='width:" + objw + ";text-align:" + obja + "' contenteditable='false' unselectable='on' value='" + _innerText + "' />";
    var divobj = "<div id=" + _id + " class='" + _cssClass + "' style='width:" + objw + ";height:" + objh + ";text-align:" + obja + ";vertical-align:" + objv + ";' contenteditable='false' unselectable='on'>" + _innerText + "</div>";

    var objhtml = (_isDiv) ? divobj : inputobj;             //alert(objhtml)
    if (nowEl) {
        nowEl.insertAdjacentHTML("beforeEnd", objhtml);
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 유동결재망,양식정보 태그클릭
***************************************************/
Add.tag_click = function (_obj) {
    if (nowEl != null) {
        if (!Add.isExistTag(_obj.id)) {
            if (Add.ReplaceTag()) {
                if ($("#" + _obj.id).attr("isdiv") == null) return;
                var isDiv = ($("#" + _obj.id).attr("isdiv").toUpperCase() == "TRUE") ? true : false;
                var cssClass = ($("#" + _obj.id).attr("class") == null) ? "" : $("#" + _obj.id).attr("class");
                Add.setFieldTag(_obj.id, tagDic.get(_obj.id), isDiv, cssClass, "100%", "100%");
            }
        }
    } else {
        alert(R_Msg_ErrSelTarget); return;
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 에디터클릭
***************************************************/
Add.form_click = function (_event) {    //alert(event);
    //nowEl = _event.srcElement;          //alert(nowEl.id + "---" + nowEl.tagName + "---" + nowEl.outerHTML);
    nowEl = _event.srcElement ? _event.srcElement : _event.target;  //ByFreeman FireFox 체크 20120713
    Add.displayInfo();                  //편집항목Display
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 편집항목Display
***************************************************/
Add.displayInfo = function () {
    if (nowEl == null) return;

    var tmphtml = "<b><span>TagName:</b>" + nowEl.tagName + "</span><br />";
    tmphtml += "<span>"
    tmphtml += (nowEl.id) ? "<b>ID:</b>" + nowEl.id : "";
    tmphtml += "</span><br />";

    if (nowEl.tagName.toUpperCase() != "BODY" && nowEl.tagName.toUpperCase() != "HTML") {
        tmphtml += "<span><b>Val:</b>";
        tmphtml += (nowEl.tagName.toUpperCase() == "INPUT") ? nowEl.value : nowEl.innerHTML;
        tmphtml += "</span>";
    }

    $.each($(".area"), function () {
        this.innerHTML = tmphtml;
    });

    Add.attributeInfo();                //항목속성Display
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.11
작성목적 : 항목속성Display
***************************************************/
Add.attributeInfo = function () {

    if (nowEl == null) return;
    if (nowEl.id == "" || nowEl.tagName.toUpperCase() == "BODY") return;
    if ($("#" + nowEl.id, editorEl).attr('class')) {
        if ($("#" + nowEl.id, editorEl).attr('class').toLowerCase() != "formfieldinput") return;
    }
    var tag = nowEl.tagName.toUpperCase();

    var selval = (tag == "INPUT") ? "single" : "multi";         //서식 지정.
    $("#selAttObj").val(selval);

    if ($("#" + nowEl.id, editorEl)[0]) {
        var objw = $("#" + nowEl.id, editorEl)[0].style.width;      //너비&높이 지정
        var objh = (tag == "INPUT") ? "" : $("#" + nowEl.id, editorEl)[0].style.height;

        $("#objw").val(objw); $("#objh").val(objh);
    }

    Add.selAttObjChange();
}
// 서식에 따른 입력제어
Add.selAttObjChange = function () {
    $("#objh").removeAttr("readonly");
    if ($("#selAttObj").val().toLowerCase() == "single") {
        $("#objh").attr("readonly", "true");
        $("#objh").val("");
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.11
작성목적 : [항목정렬]
***************************************************/
Add.align_click = function (_id) {          // alert(_id);

    if (nowEl == null) return;
    if (nowEl.id == "" || nowEl.tagName.toUpperCase() == "BODY") return;
    if ($("#" + nowEl.id, editorEl).attr('class').toLowerCase() != "formfieldinput") return;

    var align = "", vertical = "";
    switch (_id.toUpperCase()) {

        case "LT":
            align = "left"; vertical = "top";           //text-align:left; vertical-align: top;
            break;
        case "CT":
            align = "center"; vertical = "top";         //text-align:center; vertical-align: top;
            break;
        case "RT":
            align = "right"; vertical = "top";          //text-align:right; vertical-align: top;
            break;
        case "LM":
            align = "left"; vertical = "middle";        //text-align:left; vertical-align: middle;
            break;
        case "CM":
            align = "center"; vertical = "middle";      //text-align:center; vertical-align: middle;
            break;
        case "RM":
            align = "right"; vertical = "middle";       //text-align:right; vertical-align: middle;
            break;
        case "LB":
            align = "left"; vertical = "bottom";        //text-align:left; vertical-align: bottom;
            break;
        case "CB":
            align = "center"; vertical = "bottom";      //text-align:left; vertical-align: bottom;
            break;
        case "RB":
            align = "right"; vertical = "bottom";      //text-align:left; vertical-align: bottom;
            break;

    }

    $("#" + nowEl.id, editorEl)[0].style.textAlign = align;
    $("#" + nowEl.id, editorEl)[0].style.verticalAlign = vertical;
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [항목적용]
***************************************************/
Add.SetTag = function () {

    if (nowEl == null) return;
    if (nowEl.id == "" || nowEl.tagName.toUpperCase() == "BODY") return;
    if ($("#" + nowEl.id, editorEl).attr('class').toLowerCase() != "formfieldinput") return;


    var objid = nowEl.id;
    var objtext = (objid.indexOf("BODY_etc") == 0) ? tagDic.get("BODY_etc") : tagDic.get(objid);
    var isdiv = ($("#selAttObj").val().toLowerCase() == "single") ? false : true;
    var objclass = nowEl.getAttribute("class");
    var objw = $("#objw").val();
    var objh = $("#objh").val();                    //alert(objid + ":" + objtext + ":" + isdiv + ":" + objclass);

    var pobj = nowEl.parentNode;
    Add.DelTag();
    nowEl = pobj;

    Add.setFieldTag(objid, objtext, isdiv, objclass, objw, objh);
}

/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [태그삭제]
***************************************************/
Add.DelTag = function () {
    if (nowEl) {
        if (nowEl.tagName.toUpperCase() != "BODY") {
            if ($.browser.msie) {
                nowEl.removeNode(true);
            }
            else {
                nowEl.remove();
            }

            $.each($(".area"), function () {
                this.innerHTML = "";
            });
            nowEl = null;
        }
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : [닫기]
***************************************************/
Add.Close = function () {
    if (confirm(R_Msg_Close)) {
        //opener.location.reload();
        self.close();
    }
}
/**************************************************
작성자   : withsuki
작성일자 : 2012.05
작성목적 : 창닫기
***************************************************/
Add.SubmitClose = function () {

    ParamArray = {
        'formID': formID
        , 'currentPage': currentPage
        , 'recordPerPage': recordPerPage
        , 'searchField': searchField
        , 'searchValue': searchValue
        , 'searchTerm': searchTerm
        , 'searchTermValue': searchTermValue
        , 'order': order
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Admin/FormGeneration/List.aspx?p=" + GetEncryptValue;
    opener.document.location.href = url;
    self.close();
}