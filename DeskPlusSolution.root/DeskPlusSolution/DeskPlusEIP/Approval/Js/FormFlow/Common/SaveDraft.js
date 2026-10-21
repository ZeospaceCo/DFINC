SaveDraft = {}

var targetObj;
var beforeSubject = "";

$(function () {
    $(document).ready(function () {

        //[닫기]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); SaveDraft.Close(); });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); SaveDraft.Confirm(); });

        //엔터 이벤트 기술
        $("#tbDraftName").keydown(function (e) { //alert(e.keyCode)
            if (e.keyCode == 13) { e.preventDefault(); SaveDraft.Confirm(); }
        });

        SaveDraft.init();
    });
});

SaveDraft.init = function () {

    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;

    beforeSubject = (targetObj.Subject != undefined) ? targetObj.Subject : "";
    $("#tbDraftName").val(beforeSubject);

    // 이전임시저장ID
    var draftID = (targetObj.DraftID != undefined) ? targetObj.DraftID : "0";
    draftID = (draftID == "") ? "0" : draftID;
    $("#tbDraftID").val(draftID);

    // 이전임시저장일
    var writeTimeYMD = (targetObj.WriteTimeYMD != undefined) ? targetObj.WriteTimeYMD : "";
    $("#tbWriteTimeYMD").val(writeTimeYMD);

    $("#tbDraftName").focus();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [확인]
***************************************************/

SaveDraft.Confirm = function () {

    // Validation
    if ($("#tbDraftName").val() == "") {
        alert(R_Msg_InputDraftName); $("#tbDraftName").focus(); return;
    }

    // 덮어쓸지 확인(Update/Insert)
    var isaction = "ADD";
    if ($("#tbDraftName").val() == beforeSubject && $("#tbDraftID").val() != "0") {
        if (window.confirm(R_Msg_ConfirmUpdate)) {
            isaction = "MOD";
        }
    }

    // 양식ID/파일경로/에디터사용유무/이미지업로드경로
    $("#tbFormID").val(targetObj.FormID);
    $("#tbSwfUploadGUID").val(targetObj.SwfUploadGUID);
    $("#tbNonActiveX").val(targetObj.NonActiveX);
    $("#tbUploadPath").val(targetObj.UploadPath);

    // 문서종류
    $("#tbIsGuard").val($("#tbIsGuard", targetObj.document).val());

    // 열람권한 
    $("#tbAccessTitleID").val($("#tbAccessTitleID", targetObj.document).val());

    // EDM설정정보
    $("#tbIsLinkedEtc").val($("#tbIsLinkedEtc", targetObj.document).val());

    // 근거문서
    $("#tbBaseDocIDs").val($("#tbBaseDocIDs", targetObj.document).val());

    // 유동결재망 처리
    $("#tbIsUseFlow").val($("#tbIsUseFlow", targetObj.document).val());

    // 참조열람
    $("#tbCarbonView").val($("#tbCarbonView", targetObj.document).val());

    // 결재순서정렬
    $("#htaXmlSeq").val($("#htaXmlSeq", targetObj.document).val());
    $("#htaXmlSeqZQ").val($("#htaXmlSeqZQ", targetObj.document).val());
    $("#htaXmlSeqZC").val($("#htaXmlSeqZC", targetObj.document).val());

    var content = RemoveScriptTag($("#htaFormHTML", targetObj.document).val());

    // 문서내용(Html)처리
    $("#htaFormHTML").val(content);
    //$("#htaFormHTML").val($("#htaFormHTML", targetObj.document).val());
    $("#htaFormTEXT").val($("#htaFormTEXT", targetObj.document).val() + GetInputVal(targetObj.document));
    $("#htaEditorHTML").val($("#htaEditorHTML", targetObj.document).val());
    $("#htaEditorMime").val($("#htaEditorMime", targetObj.document).val());

    SaveDraft.SetEditorTags(targetObj.NonActiveX);

    __doPostBack('lbtnSubmit', isaction);
}

SaveDraft.SetEditorTags = function (_nonActiveX) {

    if ($("#EDITOR", targetObj.ifrmForm.document).length < 1) return;

    // 문서내용 (에디터+이미지)
    if (_nonActiveX == "T") {

        var objEditor = targetObj.ifrmForm.document.getElementById("ifrmTFEditor").contentWindow.xfe;
        //alert(objEditor.getHtmlValue());
        $("#htaEditorHTML").val(objEditor.getHtmlValue());
    }
    else {
        var tmptwe = $("#twe", targetObj.ifrmForm.document.ifrmTFEditor.document);
        if (tmptwe == null) return;

        if (!EditorSecurity(tmptwe[0])) return;

        var strHtml = tmptwe[0].HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            tmptwe[0].HtmlValue = strHtml;
        }

        $("#htaEditorMime").val(tmptwe[0].MimeEnValue);
    }

    var frmUrl = $("#ifrmTFEditor", targetObj.ifrmForm.document)[0].outerHTML;
    var cont = $("#htaFormHTML").val();
    $("#htaFormHTML").val(cont.replace(frmUrl, "<ZEOEDITOR></ZEOEDITOR>"));
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [확인] - 서버호출(처리후)
***************************************************/
SaveDraft.SubmitClose = function (_msg) {
    //alert(_msg);
    //SaveDraft.Close();

    SaveDraft.SubmitCloseReload(_msg);
}
SaveDraft.SubmitCloseReload = function (_msg) {
    alert(_msg);

    ParamArray = { 'menuID': 'Temporary' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/FormFlow/Temporary/List.aspx?p=" + GetEncryptValue;
    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;
    targetObj.document.location.href = url;

    SaveDraft.Close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [닫기]
***************************************************/
SaveDraft.Close = function () {
    if (window.dialogArguments) {
        window.returnValue = false; self.close();
    } else {
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}
