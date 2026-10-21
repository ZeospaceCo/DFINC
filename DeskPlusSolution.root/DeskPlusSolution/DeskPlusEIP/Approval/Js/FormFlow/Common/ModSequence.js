ModSequence = {}

$(function () {
    $(document).ready(function () {

        $("#ulWidgets").sortable();

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ModSequence.Confirm(); });

        ModSequence.init();
    });
});

var parentObj = null;
var tmpAppSeqKey; var tmpAppSeqDic;
var ZAfirst = null; var ZAchild = null; var ZAcount = 0;    // 병렬합의 대표obj, 그외obj

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : init
**********************************************************************************************/
ModSequence.init = function () {

    parentObj = (window.dialogArguments) ? window.dialogArguments : parent;

    tmpAppSeqKey = parentObj.appSeqKey;
    tmpAppSeqDic = parentObj.appSeqDic;

    ZAfirst = null;
    ZAchild = new Array()
    k = 0;

    var tmpHtml = "", objTitle = "";
    var tmpHtmlZD0 = "";

    $.each(tmpAppSeqKey, function (i, o) {

        var obj = tmpAppSeqDic.get(o);                  //원래의 id값

        if (obj.approvalTag.substr(0, 2) == "ZA") {     // 병렬합의자의 경우, 예외처리  
            tmpHtml += ModSequence.GetZAHtml(o);

        } else {            //alert(obj.key + ":" + obj.approvalTag)
            objTitle = (parentObj.tagDic.get(obj.approvalTag) != null) ? parentObj.tagDic.get(obj.approvalTag) : parentObj.tagDic.get(obj.approvalTag.substr(0, 2));
            LineSeq += 1;

            //SiteKoramco
            if (obj.approvalTag == "ZD0") {
                tmpHtmlZD0 += "<li id='" + o + "' class='" + obj.approvalTag.substr(0, 2) + "' >";       //ZD0, ZD4, ZH01
                tmpHtmlZD0 += LineSeq + ". [" + objTitle + "] ";
                //tmpHtmlZD0 += " [" + objTitle + "] ";
                tmpHtmlZD0 += obj.deptName + "&nbsp;";
                tmpHtmlZD0 += obj.titleName + "&nbsp;";
                tmpHtmlZD0 += obj.name + "&nbsp;";         //tmpHtml += "("+o+")" (debug)
                tmpHtmlZD0 += "</li>";
            } else {
                tmpHtml += "<li id='" + o + "' class='" + obj.approvalTag.substr(0, 2) + "'>";       //ZD0, ZD4, ZH01
                tmpHtml += LineSeq + ". [" + objTitle + "] ";
                //tmpHtml += " [" + objTitle + "] ";
                tmpHtml += obj.deptName + "&nbsp;";
                tmpHtml += obj.titleName + "&nbsp;";
                tmpHtml += obj.name + "&nbsp;";         //tmpHtml += "("+o+")" (debug)
                tmpHtml += "</li>";
            }
        }
    });

    $("#ulWidgets").html(tmpHtml)
    $("#ulWidgetsZD0").html(tmpHtmlZD0)

    // 병렬합의자의 경우, 예외처리 (그외 XX명)
    if (ZAchild != null) {
        if (ZAchild.length > 0) {
            var v = $("#" + ZAfirst).html() + "(" + R_ETC3 + " " + ZAcount + R_Person + ")";
            $("#" + ZAfirst).html(v);
        }
    }

    // 수신결재라인 처리
    var tmpHtml = "", rtnHtml = "";
    tmpappSeqDicZQ = parentObj.appSeqDicZQ;
    if (tmpappSeqDicZQ.size() > 0) {

        var arrTagKey = parentObj.Common.GetAppSeqQueue(tmpappSeqDicZQ);
        $.each(arrTagKey, function (i, o) {
            var obj = tmpappSeqDicZQ.get(o);
            if (obj.approvalTag.indexOf("ZQ") == 0) {
                rtnHtml += (rtnHtml == "") ? "" : " -> ";
                rtnHtml += obj.name + "(" + obj.deptName + ")";
            }
        });

        tmpHtml = "<strong>" + R_ZQ + " : </strong>";
        tmpHtml += rtnHtml;
        $("#divSeqZQLine").html(tmpHtml);

        if (rtnHtml == "") {
            $("#divSeqZQLine").css("display", "none");
        } else {
            $("#divSeqZQLine").css("display", "");
        }
    } else {
        $("#divSeqZQLine").css("display", "none");
    }

    // 열람권한정보 처리
    var idx = $("#ddlAccessTitle", parentObj.parent.frmLeft.document)[0].selectedIndex;
    tmpHtml = "<strong>" + R_AccessTitle + " : </strong>";
    tmpHtml += $("#ddlAccessTitle", parentObj.parent.frmLeft.document)[0].options[idx].text;
    $("#AccessTitleName").html(tmpHtml);
}

// 병렬합의 처리
ModSequence.GetZAHtml = function (_o) {
    var obj = tmpAppSeqDic.get(_o);

    var tmpHtml = "";
    if (ZAfirst == null) {
        LineSeq += 1;
        ZAfirst = _o;

        tmpHtml = "<li id='" + _o + "' class='" + obj.approvalTag.substr(0, 2) + "'>";                   //ZA1
        tmpHtml += LineSeq + ". [" + parentObj.tagDic.get(obj.approvalTag) + "] ";
        tmpHtml += obj.deptName + "&nbsp;";
        tmpHtml += obj.titleName + "&nbsp;";
        tmpHtml += obj.name + "&nbsp;";
        tmpHtml += "</li>";

    } else {
        ZAchild[ZAcount] = _o; ZAcount++;
    }

    return tmpHtml;
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인] 항목순서변경
**********************************************************************************************/
ModSequence.Confirm = function () {

    var items = $("#ulWidgetsZD0 li, #ulWidgets li");
    var sortlist = "";

    var appSeqDicRe = new jQuery.Hashtable();       // For, 결재자지정정보
    var keyHistoryRe = new Array();                 // 결재순서변경 시, 변경 전후 Key.   변경전Key(Value):변경후Key
    var keyHistoryReCnt = 0;

    var k = ""; var v; var zaCnt = 0; var lastTag = "", firstTag = "";
    var tagTmp = "";
    for (var i = 0; i < items.length; i++) {

        tagTmp = items[i].id.split("_")[1];     // 1_ZD1                <-- items[i].id

        if (items[i].id.indexOf("ZA") > 0) { zaCnt = i.toString(); }

        k = i.toString() + "_" + tagTmp;        // 1_ZD1
        v = tmpAppSeqDic.get(items[i].id);

        keyHistoryRe[items[i].id] = k;              // 변경전Key (Key) : 변경후Key (Value)
        keyHistoryReCnt++;
        appSeqDicRe.add(k, v);

        // 이전 key값 정보 변경
        $("#" + tagTmp, parentObj.ifrmObj).attr("key", items[i].id);    // 문서내의 tag에 대한 key값 변경

        // 처음결재태그와 최종결재태그 체크.
        if (i == 0) {                               // 처음결재태그
            firstTag = v.approvalTag;
        }
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

    if (firstTag != parentObj.ApprovalTag) {
        alert(R_Msg_SeqErrZD); return;
    }

    if (lastTag.indexOf("ZA") == 0) {
        alert(R_Msg_SeqErrZA); return;
    }
    if (lastTag.indexOf("ZH") == 0) {
//        alert(R_Msg_SeqErrZH); return;
    }

    if (appSeqDicRe.size() > 0) {
        parentObj.appSeqDic.items = appSeqDicRe.items;      // 결재순서가 변경된 결재라인 정보
        parentObj.appSeqKeyHistory = keyHistoryRe;          // 지정취소를 위해 키 변경 전 후, 값을 가지고 있음
        parentObj.appSeqKeyHistoryCnt = keyHistoryReCnt;
        //parentObj.Common.ResetSequence(); alert(keyHistoryReCnt)
    }

    ModSequence.Close();
}

ModSequence.Close = function () {
    if (window.dialogArguments) {
        window.returnValue = true;
        self.close();
    } else {
        if (parent.ConfirmModSequenceFunc) parent.ConfirmModSequenceFunc(true);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}