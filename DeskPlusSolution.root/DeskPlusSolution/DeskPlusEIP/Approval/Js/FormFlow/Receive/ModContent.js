ModContent = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // [미리보기]
        $('#cbtnPreview').click(function (e) { e.preventDefault(); ModContent.Preview(); });
        // [문서상신]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); ModContent.Submit(); });
        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); Common.AttachSwfFile(); });
        // [근거문서-추가]
        $('#cbtnDocAdd').click(function (e) { e.preventDefault(); Common.AttachBaseDoc(); });
        // 개발용(근거문서 추가시 문서번호 보이기)
        $('#tbAddDocID').val(BaseDocIDs);
        
        ModContent.Init();
        Common.GetSpoolFileList();
        Common.GetAttachFileListEdit(DocumentID);   // 첨부파일 리스트
        Common.GetBaseDocListEdit(DocumentID);      // 근거문서 리스트
    });
});

ModContent.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    $("body", document).click(function (e) {

        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        $("#zeoEditorDIV").hide();

        preEl = nowEl;

        // 이전 항목이 결재자였으면, 클래스명 변경.
        if (preEl) {
            if (preEl.id.indexOf("Z") == 0 || preEl.id.indexOf("FLOW") == 0) { preEl.className = preEl.className.replace("Sel", ""); }
        }
    });

    $("body", parent.frmLeft.document).click(function (e) {

        if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
        $("#zeoEditorDIV").hide();

        preEl = nowEl;

        // 이전 항목이 결재자였으면, 클래스명 변경.
        if (preEl) {
            if (preEl.id.indexOf("Z") == 0 || preEl.id.indexOf("FLOW") == 0) { preEl.className = preEl.className.replace("Sel", ""); }
        }
    });

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("ModContent.InitForm();", 100);

    // 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, true); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, true); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, true); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, true); });

    // 자주지정하는 결재자 정보
    Common.GetFavoriteApp();

    // 양식결재라인 정보
    Common.GetFormSeq(FormID);

    // 문서번호 정보
    // 기타정보설정(기존값셋팅)
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 양식초기화
***************************************************/
ModContent.InitForm = function () {

    // 에디터 처리
    if ($("#EDITOR", ifrmObj).length > 0) {
        $("#htaEditorHTML").val($("#EDITOR", ifrmObj).html());
    }

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZP, .ZQ, .ZC", ifrmObj);
    $.each(arrStamp, function (i, o) {
        if (o.id.toUpperCase().indexOf("_INFO") < 0) {      // _INFO는 승인시 처리되므로, 따로 예외처리하지 않음.

            if ($("#" + o.id, ifrmObj).children('img').length == 0 && o.id.indexOf("FLOW") < 0 && $("#" + o.id, ifrmObj).attr("isapp") == undefined) {    //children - img tag    //유동결재망 div는 삭제불가한 걸로.
                o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
            }
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);            //var arrInfoExcept = $("#SUBJ,#LIFE,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each(arrInfo, function (i, o) {
        //if (o.id == "SUBJ" || o.id == "LIFE") return;       //초기화 예외필드처리
        //if (o.tagName.toUpperCase() == "INPUT") { o.value = ""; }
        //if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }

        Common.ExceptObj(o);   // 특정필드 예외처리 (에디터 ..)
    });

    // 입력필드 활성화 (class명:formfieldinput)
    var actObj1 = $.grep(arrInfo, function (o, i) {
        return (o.id.indexOf("BODY") == 0);
    });
    var actObj2 = $("#SUBJ,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each($.merge(actObj1, actObj2), function (i, o) {
        $("#" + o.id, ifrmObj).removeAttr("unselectable");
        $("#" + o.id, ifrmObj).removeAttr("readonly");
        $("#" + o.id, ifrmObj).removeAttr("disabled");
        $("#" + o.id, ifrmObj).attr("contenteditable", "true");
    });
    /* class명이 'formfieldinput' 인 것만 처리.
    var actObj = $("input,textarea,div,select", ifrmForm.document);
    $.each(actObj, function (i, o) {
    if (o.id != "") {

    if ($("#" + o.id, ifrmObj).attr("unselectable")) {
    $("#" + o.id, ifrmObj).removeAttr("unselectable");
    }
    if ($("#" + o.id, ifrmObj).attr("readonly")) {
    $("#" + o.id, ifrmObj).removeAttr("readonly");
    }
    if ($("#" + o.id, ifrmObj).attr("disabled")) {
    $("#" + o.id, ifrmObj).removeAttr("disabled");
    }
    if ($("#" + o.id, ifrmObj).attr("contenteditable")) {
    $("#" + o.id, ifrmObj).attr("contenteditable", "true");
    }
    }
    });*/

    // 미결재라인정보
    Common.GetSeqNonApp(DocumentID);

    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);

    //$("#txtArea").val(ifrmForm.document.body.outerHTML)
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [문서상신]
***************************************************/
ModContent.Submit = function () {

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    if (appSeqDicZQ.size() == 0) { alert(R_CheckZQ); return; }

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }

    //필수입력 - 제목
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $.trim($("#SUBJ", ifrmObj).val());
        if (subj == "") { alert(R_Msg_ErrSubj); $("#SUBJ", ifrmObj).focus(); return; }
    }

    //필수입력 - 보존년한
    var life = "";
    if ($("#LIFE", ifrmObj).length > 0) {
        life = $.trim($("#LIFE", ifrmObj).val());
        if (life == "") { alert(R_Msg_ErrLife); $("#LIFE", ifrmObj).focus(); return; }
    }
    life = (life == "") ? R_LIFE : life;  // 보존년한필드가 없으면 기본값으로 대체
    $("#tbLifeTime").val(life);

    // 에디터 스타일 체크
    if (!Common.CheckStyle(NonActiveX)) {
        alert(R_MsgConfirmEditor); return;
    }

    //필수입력 - 첨언
    if (FormID != 7) {
        if ($("#tbOpinion", parent.frmLeft.document).val() == "") {
            alert(R_Msg_ErrOpinionNon);
            parent.frmLeft.Common.SetMenu(1); //트리메뉴제어(결재의견)
            $("#tbOpinion", parent.frmLeft.document).focus();
            return;
        }
    }

    // 결재의견 (For.첨언처리)
    var opinion = $("#tbOpinion", parent.frmLeft.document).val();
    if (opinion.length > 200) {
        alert(R_Msg_ErrOpinion);
        $("#tbOpinion", parent.frmLeft.document).focus();
        parent.frmLeft.Common.SetMenu(1); //트리메뉴제어(결재의견)
        return;
    }
    $("#tbOpinion").val(opinion.replace(/\n/gi, "<br>").replace(/ /g, "&nbsp;"));

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    if (window.confirm(R_MsgAlertEdit)) {

        // 유동결재망 처리
        $("#tbIsUseFlow").val(Common.GetFlowInfo());

        // 결재순서정렬
        appSeqKey = Common.GetAppSeqQueue(appSeqDic);
        // 결재정보처리 (순서에 맞게)
        $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));       // 수신결재
        $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));       // 참조결재

        // EDM처리 & 커스텀
        ApprovalType = "S";
        Common.SetLoading(true);   // 처리중

        if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
            ifrmProcessing.onComplete();

        } else {                                                        // 그외의 경우, submit호출
            AddSubmit();
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : Submit (연동문서에서 onComplete함수가 있다면, submit을 위해 이 함수를 호출)
***************************************************/
AddSubmit = function () {

    // 서버시간 체크
    Common.GetServerTime();

    // 제목 처리
    if ($("#SUBJ", ifrmObj).length > 0) {
        $("#tbSubject").val($("#SUBJ", ifrmObj).val());
    }

    // 문서내용(에디터) + 에디터이미지 처리(activeX인경우만, 이미지 업로드)
    Common.SetEditorTags(NonActiveX);

    // 빈 결재란 처리
    Common.SetDefaultTags();

    // 첨언처리 $("#tbOpinion").val()
    Common.SetOpinion(UserName, TitleName, DutyName, DeptName, $("#tbOpinion").val());

    // Input Tag 처리
    Common.SetReadOnly();

    // 특정태그처리 (for, chrome,ff,safari)
    Common.UpdateDOM();

    var content = RemoveScriptTag($("HTML", ifrmObj).html());

    // 문서내용(Html)처리
    //$("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

    __doPostBack('lbtnSubmit', "ADD");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 문서상신 후(성공/실패), 페이지 이동 (서버호출)
***************************************************/
ModContent.SubmitClose = function (_msg) {
    alert(_msg);

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID,
        'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());
    parent.document.location.href = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [미리보기]
***************************************************/
ModContent.Preview = function () {

    Common.UpdateDOM();

    ParamArray = { 'nonActiveX': NonActiveX };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Common/Popup/PreView.aspx?p=" + GetEncryptValue;

    if ($.browser.msie) {
        WindowObjectReference = PopupModalWinOpen(url, "800", "770")
    } else {
        PopupWin("800", "720", url, "", "", true);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 목록으로 가기 
***************************************************/
ModContent.List = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID,
        'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());
    parent.location.href = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
}/**************************************************
작성자   : 
작성일자 : 
작성목적 : 신메뉴 화면에서 상태조회 이동 시 신메뉴 유지
***************************************************/
function GetApprovalStatusDefaultPage(defaultPage) {
    try {
        var parentPath = parent.location.pathname.toLowerCase();
        if (parentPath.indexOf("/approval/formflow/defaultall.aspx") >= 0) {
            return defaultPage.replace("Default.aspx", "DefaultAll.aspx");
        }
        if (parentPath.indexOf("/approval/formflow/default.aspx") >= 0) {
            return defaultPage;
        }
    } catch (e) { }

    try {
        if (typeof ApprovalStatusMenuType != "undefined" && ApprovalStatusMenuType.toUpperCase() == "NEW") {
            return defaultPage.replace("Default.aspx", "DefaultAll.aspx");
        }
    } catch (e) { }
    return defaultPage;
}
