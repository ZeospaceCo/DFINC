ConfirmSequence = {}

$(function () {
    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); self.close(); });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ConfirmSequence.Confirm(); });

        ConfirmSequence.init();
    });
});

var parentObj = null;
var tmpAppSeqKey; var tmpAppSeqDic;
var tmpappSeqDicKey;
/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : init
**********************************************************************************************/
ConfirmSequence.init = function () {

    parentObj = (window.dialogArguments) ? window.dialogArguments : parent;

    var rtnHtml = ""; var zaCnt = 0;

    // 발신결재라인
    tmpAppSeqKey = parentObj.appSeqKey;
    tmpAppSeqDic = parentObj.appSeqDic;

    if (tmpAppSeqKey.length > 0 && tmpAppSeqDic.size() > 0) {
        $.each(tmpAppSeqKey, function (i, o) {
            var obj = tmpAppSeqDic.get(o);

            if (obj.approvalTag.indexOf("ZA") < 0) {
                rtnHtml += (rtnHtml == "") ? "" : " -> ";
                rtnHtml += obj.name + "(" + obj.deptName + ")";
            } else {
                rtnHtml += (zaCnt == 0) ? " -> " : " / ";
                rtnHtml += obj.name + "(" + obj.deptName + ")";
                zaCnt++;
            }
        });
        $("#divSeqLine").html(rtnHtml);
    }

    // 수신결재라인
    tmpappSeqDicZQ = parentObj.appSeqDicZQ;
    if (tmpappSeqDicZQ.size() > 0) {

        var arrTagKey = parentObj.Common.GetAppSeqQueue(tmpappSeqDicZQ);

        rtnHtml = "";
        $.each(arrTagKey, function (i, o) {
            var obj = tmpappSeqDicZQ.get(o);
            if (obj.approvalTag.indexOf("ZQ") == 0) {
                rtnHtml += (rtnHtml == "") ? "" : " -> ";
                rtnHtml += obj.name + "(" + obj.deptName + ")";
            }
        });
        $("#divSeqZQLine").html(rtnHtml);

        if (rtnHtml == "") {
            $("#trZQ").css("display", "none");
        } else {
            $("#trZQ").css("display", "");
        }
    } else {
        $("#trZQ").css("display", "none");
    }

    // 열람권한정보 처리
    var idx = $("#ddlAccessTitle", parentObj.parent.frmLeft.document)[0].selectedIndex;
    $("#AccessTitleName").html($("#ddlAccessTitle", parentObj.parent.frmLeft.document)[0].options[idx].text);
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인] 문서상신
**********************************************************************************************/
ConfirmSequence.Confirm = function () {

    //parent.Add.AddSubmit();     //문서상신 호출.
    //ConfirmSequence.Close();

    window.returnValue = true;
    self.close();
}
