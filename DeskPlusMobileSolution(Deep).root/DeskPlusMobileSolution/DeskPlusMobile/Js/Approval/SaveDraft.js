SaveDraft = {}

var targetObj;
var beforeSubject = "";

/**************************************************
작성자   : famo23
작성일자 : 2019-07-25
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btSave').click(function (e) {
        e.preventDefault();
        SaveDraft.Save();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        SaveDraft.Close();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    SaveDraft.init();

});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-25
작성목적 : 초기화 
**********************************************************************************************/
SaveDraft.init = function () {

    beforeSubject = (parent.Subject != undefined) ? parent.Subject : "";
    $("#tbDraftName").val(beforeSubject);

    // 이전임시저장ID
    var draftID = (parent.DraftID != undefined) ? parent.DraftID : "0";
    draftID = (draftID == "") ? "0" : draftID;
    $("#tbDraftID").val(draftID);

    // 이전임시저장일
    var writeTimeYMD = (parent.WriteTimeYMD != undefined) ? parent.WriteTimeYMD : "";
    $("#tbWriteTimeYMD").val(writeTimeYMD);
}
/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-25
작성목적 : [닫기]
**********************************************************************************************/
SaveDraft.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-25
작성목적 : 저장
***************************************************/
SaveDraft.Save = function () {

    // Validation
    if ($("#tbDraftName").val() == "") {
        parent.action_popup.alert(R_Msg_InputDraftName); $("#tbDraftName").focus(); return;
    }

    // 양식ID/파일경로/에디터사용유무/이미지업로드경로
    $("#tbFormID").val(parent.FormID);
    $("#tbSwfUploadGUID").val(parent.SwfUploadGUID);
    $("#tbUploadPath").val(parent.UploadPath);

    // 문서종류
    $("#tbIsGuard").val($("#ddlGuard", parent.document).val());

    // 열람권한 
    $("#tbAccessTitleID").val($("#ddlAccessTitle", parent.document).val());

    // EDM설정정보
    $("#tbIsLinkedEtc").val($("#tbIsLinkedEtc", parent.document).val());

    // 근거문서
    $("#tbBaseDocIDs").val($("#tbBaseDocIDs", parent.document).val());

    // 유동결재망 처리
    $("#tbIsUseFlow").val($("#tbIsUseFlow", parent.document).val());

    // 참조열람
    $("#tbCarbonView").val($("#ddlCarbonView", parent.document).val());

    // 결재순서정렬
    $("#htaXmlSeq").val($("#htaXmlSeq", parent.document).val());
    $("#htaXmlSeqZQ").val($("#htaXmlSeqZQ", parent.document).val());
    $("#htaXmlSeqZC").val($("#htaXmlSeqZC", parent.document).val());

    var content = RemoveScriptTag($("#htaFormHTML", parent.document).val());

    // 문서내용(Html)처리
    $("#htaFormHTML").val(content);
    $("#htaFormTEXT").val($("#htaFormTEXT", parent.document).val() + GetInputVal(parent.document));
    $("#htaEditorHTML").val($("#htaEditorHTML", parent.document).val());
    $("#htaEditorMime").val($("#htaEditorMime", parent.document).val());

    SaveDraft.SetEditorTags();

    // 덮어쓸지 확인(Update/Insert)
    var isaction = "ADD";
    if ($("#tbDraftName").val() == beforeSubject && $("#tbDraftID").val() != "0") {

        parent.action_popup.confirm(R_Msg_ConfirmUpdate, function (res) {
            if (res) {
                isaction = "MOD";
                __doPostBack('lbtnSubmit', isaction);
            }

        })
    }
    else {
        __doPostBack('lbtnSubmit', isaction);
    }
}

SaveDraft.SetEditorTags = function (_nonActiveX) {

    if ($("#EDITOR", parent.ifrmForm.document).length < 1) return;

    // 문서내용 (에디터+이미지)
    var objEditor = parent.ifrmForm.document.getElementById("ifrmTFEditor").contentWindow.xfe;
    $("#htaEditorHTML").val(objEditor.getHtmlValue());


    var frmUrl = $("#ifrmTFEditor", parent.ifrmForm.document)[0].outerHTML;
    var cont = $("#htaFormHTML").val();
    $("#htaFormHTML").val(cont.replace(frmUrl, "<ZEOEDITOR></ZEOEDITOR>"));
}

/**************************************************
작성목적 : [확인] - 서버호출(처리후)
***************************************************/
SaveDraft.SubmitClose = function (_msg) {
    parent.action_popup.alert(_msg);
    SaveDraft.Close();
}

SaveDraft.SubmitCloseReload = function (_msg) {
    parent.action_popup.alert(_msg);

    var url = "/DeskPlusMobile/Approval/DraftList.aspx";
    parent.document.location.href = "/DeskPlusMobile/Approval/DraftList.aspx";

    SaveDraft.Close();
}
