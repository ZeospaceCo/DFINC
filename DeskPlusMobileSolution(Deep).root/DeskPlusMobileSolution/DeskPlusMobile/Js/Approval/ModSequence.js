ModSequence = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-29
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        ModSequence.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        ModSequence.Close();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    // 문서 리스트 가져오기
    ModSequence.init();

});


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-29
작성목적 : [닫기]
**********************************************************************************************/
ModSequence.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

var tmpAppSeqKey; var tmpAppSeqDic;
var ZAfirst = null; var ZAchild = null; var ZAcount = 0;    // 병렬합의 대표obj, 그외obj

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-29
작성목적 : init
**********************************************************************************************/
ModSequence.init = function () {

    tmpAppSeqKey = parent.appSeqKey;
    tmpAppSeqDic = parent.appSeqDic;

    ZAfirst = null;
    ZAchild = new Array()
    k = 0;

    var tmpHtml = "", objTitle = "";
    var tmpHtmlZD0 = "";

    $.each(tmpAppSeqKey, function (i, o) {

        var obj = tmpAppSeqDic.get(o);                  //원래의 id값

        if (obj.approvalTag.substr(0, 2) == "ZA") {     // 병렬합의자의 경우, 예외처리  
            tmpHtml += ModSequence.GetZAHtml(o);

        } else {
            objTitle = (parent.tagDic.get(obj.approvalTag) != null) ? parent.tagDic.get(obj.approvalTag) : parent.tagDic.get(obj.approvalTag.substr(0, 2));

            if (obj.approvalTag == "ZD0") {
                tmpHtmlZD0 += "<div id='" + o + "' class='listZD'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                tmpHtmlZD0 += "<span class='bg-secondary text-white'>" + objTitle + "</span>";
                tmpHtmlZD0 += "<span>" + obj.deptName + "&nbsp;" + obj.titleName + "&nbsp;" + obj.name + "</span></div>";

            } else {
                tmpHtml += "<div id='" + o + "' class='listDiv'><span><input type='radio' name='rdSeq' id='R_" + o + "' value='" + o + "'  /></span>";
                tmpHtml += "<span class='bg-warning text-black'>" + objTitle + "</span>";
                tmpHtml += "<span>" + obj.deptName + "&nbsp;" + obj.titleName + "&nbsp;" + obj.name + "</span></div>";
            }
        }
    });

    $("#ulList").html(tmpHtml);
    $("#ulZD0").html(tmpHtmlZD0);

    // 병렬합의자의 경우, 예외처리 (그외 XX명)
    if (ZAchild != null) {
        if (ZAchild.length > 0) {
            var v = $("#" + ZAfirst).html() + "(" + R_ETC3 + " " + ZAcount + ")";
            $("#" + ZAfirst).html(v);
        }
    }

    // 수신결재라인 처리
    var rtnHtml = "";
    tmpappSeqDicZQ = parent.appSeqDicZQ;
    if (tmpappSeqDicZQ.size() > 0) {

        var arrTagKey = parent.Common.GetAppSeqQueue(tmpappSeqDicZQ);
        $.each(arrTagKey, function (i, o) {
            var obj = tmpappSeqDicZQ.get(o);
            if (obj.approvalTag.indexOf("ZQ") == 0) {

                rtnHtml += "<div id='" + o + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                rtnHtml += "<span class='bg-secondary text-white'>" + R_ZQ + "</span>";
                rtnHtml += "<span>" + obj.deptName + "&nbsp;" + obj.titleName + "&nbsp;" + obj.name + "</span></div>";

            }
        });

        $("#ulZQ").html(rtnHtml);

        if (rtnHtml == "") {
            $("#liZQ").css("display", "none");
        } else {
            $("#liZQ").css("display", "");
        }
    }
    else {
        $("#liZQ").css("display", "none");
    }
}

// 병렬합의 처리
ModSequence.GetZAHtml = function (_o) {
    var obj = tmpAppSeqDic.get(_o);

    var tmpHtml = "";
    if (ZAfirst == null) {
        ZAfirst = _o;

        tmpHtml += "<div id='" + _o + "' class='listDiv'><span><input type='radio' name='rdSeq' id='R_" + _o + "' value='" + _o + "'  /></span>";
        tmpHtml += "<span class='bg-warning text-black'>" + parent.tagDic.get(obj.approvalTag) + "</span>";
        tmpHtml += "<span>" + obj.deptName + "&nbsp;" + obj.titleName + "&nbsp;" + obj.name + "</span></div>";

    } else {
        ZAchild[ZAcount] = _o; ZAcount++;
    }

    return tmpHtml;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-31
작성목적 : 순서변경(위로)
***************************************************/
ModSequence.Up = function () {

    var TmpVal = $(":input:radio[name=rdSeq]:checked").val();

    var srcDiv = $("#" + TmpVal);
    var tgtDiv = $("#" + TmpVal).prev();

    if (tgtDiv[0]) {
        tgtDiv.before(srcDiv);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-31
작성목적 : 순서변경(아래로)
***************************************************/
ModSequence.Down = function () {

    var TmpVal = $(":input:radio[name=rdSeq]:checked").val();

    var srcDiv = $("#" + TmpVal);
    var tgtDiv = $("#" + TmpVal).next();

    if (tgtDiv[0]) {
        tgtDiv.after(srcDiv);
    }
}

function getOrder(selector, container) {
    var order = [];
    $(selector, container).each(function () {
        order.push(this.id);
    });
    return order;
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-31
작성목적 : 확인하고선 상신
**********************************************************************************************/
ModSequence.Confirm = function () {

    var items = getOrder(".listDiv", $("#ulList")[0]);
    var sortlist = "";

    var appSeqDicRe = new jQuery.Hashtable();       // For, 결재자지정정보
    var keyHistoryRe = new Array();                 // 결재순서변경 시, 변경 전후 Key.   변경전Key(Value):변경후Key
    var keyHistoryReCnt = 0;

    var k = ""; var v; var zaCnt = 0; var lastTag = "";
    var tagTmp = "";

    //ZD0 
    var tmpZDTag = $(".listZD").attr('id');
    k = "0_ZD0";
    v = tmpAppSeqDic.get(tmpZDTag);

    keyHistoryRe[tmpZDTag] = k;              // 변경전Key (Key) : 변경후Key (Value)
    keyHistoryReCnt++;
    appSeqDicRe.add(k, v);

    $("#ZD0", parent.ifrmObj).attr("key", tmpZDTag);    // 문서내의 tag에 대한 key값 변경

    for (var i = 0; i < items.length; i++) {

        tagTmp = items[i].split("_")[1];     // 1_ZD1                <-- items[i].id

        if (items[i].indexOf("ZA") > 0) { zaCnt = (i + 1).toString(); }

        k = (i + 1) + "_" + tagTmp;        // 1_ZD1
        v = tmpAppSeqDic.get(items[i]);

        keyHistoryRe[items[i]] = k;              // 변경전Key (Key) : 변경후Key (Value)
        keyHistoryReCnt++;
        appSeqDicRe.add(k, v);

        // 이전 key값 정보 변경
        $("#" + tagTmp, parent.ifrmObj).attr("key", items[i]);    // 문서내의 tag에 대한 key값 변경

        if (i == items.length - 1) {                // 최종결재태그 예외처리
            lastTag = v.approvalTag;
        }
    }

    // 병렬합의자의 경우, 예외처리
    if (ZAchild != null) {
        if (ZAchild.length > 0) {

            $.each(ZAchild, function (i, o) {

                k = zaCnt + "_" + o.split("_")[1];  // 1_ZA3
                v = tmpAppSeqDic.get(o);
                appSeqDicRe.add(k, v);
            });
        }
    }

    if (lastTag.indexOf("ZA") == 0) {
        parent.action_popup.alert(R_Msg_SeqErrZA); return;
    }
    if (lastTag.indexOf("ZH") == 0) {
        parent.action_popup.alert(R_Msg_SeqErrZH); return;
    }

    if (appSeqDicRe.size() > 0) {
        parent.appSeqDic.items = appSeqDicRe.items;      // 결재순서가 변경된 결재라인 정보
        parent.appSeqKeyHistory = keyHistoryRe;          // 지정취소를 위해 키 변경 전 후, 값을 가지고 있음
        parent.appSeqKeyHistoryCnt = keyHistoryReCnt;
    }

    if (parent.ConfirmModSequenceFunc) parent.ConfirmModSequenceFunc(true);
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}