Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // [인쇄]
        $('#cbtnPreview').click(function (e) { e.preventDefault(); Add.Preview(); });
        // [문서상신]
        $('#cbtnSubmit').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Add.GetIsMyTurn();
            if (!IsMyTurnState) {
                alert(R_MyTurnSusin);
                Add.List();
                return;
            }

            Add.Submit();
        });
        // [반려]
        $('#cbtnReject').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Add.GetIsMyTurn();
            if (!IsMyTurnState) {
                alert(R_MyTurnSusin);
                Add.List();
                return;
            }

            Add.Reject();
        });
        // [담당자변경]
        $('#cbtnModify').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Add.GetIsMyTurn();
            if (!IsMyTurnState) {
                alert(R_MyTurnSusin);
                Add.List();
                return;
            }

            Add.Modify();
        });
        // [완료]
        $('#cbtnComplete').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Add.GetIsMyTurn();
            if (!IsMyTurnState) {
                alert(R_MyTurnSusin);
                Add.List();
                return;
            }

            Add.Complete();
        });
        // [결재순서변경]
        $('#cbtnModSeq').click(function (e) { e.preventDefault(); Common.SeqOrder(true, true); });
        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); Common.AttachSwfFile(); });
        // [근거문서-추가]
        $('#cbtnDocAdd').click(function (e) { e.preventDefault(); Common.AttachBaseDoc(); });
        // [결재자지정]
        $('#cbtnSetLine').click(function (e) { e.preventDefault(); Add.SetLine(); });
        // [문서수정]
        $('#cbtnEdit').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Add.GetIsMyTurn();
            if (!IsMyTurnState) {
                alert(R_MyTurnSusin);
                Add.List();
                return;
            }

            Add.Edit();
        });

        // [결재선추가]
        $('#cbtnFlowAdd').click(function (e) { e.preventDefault(); Add.FlowAdd(); });
        // [결재선삭제]
        $('#cbtnFlowDel').click(function (e) { e.preventDefault(); Add.FlowDel(); });

        //개발용(근거문서 추가시 문서번호 보이기)
        $('#tbAddDocID').val(BaseDocIDs);

        $('#cbtnEDM').click(function (e) { e.preventDefault(); Add.GetEDM(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); Add.OpinionInfo(); });

        // [개인결재망]
        $('#cbtnPrivateNet').click(function (e) { e.preventDefault(); Add.PrivateNet(); });

        Add.Init();
        Common.GetSpoolFileList();
        Common.GetAttachFileListEdit(ParentDocumentID);   // 첨부파일 리스트
        Common.GetBaseDocListEdit(ParentDocumentID);      // 근거문서 리스트

        Add.OpinionCount(); //결재의견 존재여부
    });
});

Add.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {           //인장체크 (없으면 서명등록)
        $("#ulSubmit").css("display", "none");
        alert(R_MSG_ERRSEAL);

        var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
        PopupWin(300, 300, url, "-500", "", true);
        return;
    }

    // 버튼처리
    if (AccountTag.indexOf("PS") < 0) {
        // 수신(ZQ/ZP)
        $("#ulCmn").css("display", "");

        if (AccountTag.indexOf("ZQ") < 0) {
            $("#ulZP").css("display", "");
            $("#ulZQ").css("display", "none");
        } else {
            $("#ulZP").css("display", "none");
            $("#ulZQ").css("display", "");
        }
    } else {
        // 이관(PS)
        $("#ulCmn").css("display", "none");
        $("#ulZQ").css("display", "none");
        $("#ulZP").css("display", "none");
    }

    // 기타정보설정

    // 양식로딩
    if (FormID > 0) {

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

        parent.frmLeft.Common.SetMenu(1); //트리메뉴제어(조직도)

        // 결재망
        ifrmForm.document.open();
        ifrmForm.document.write($("#htaFormNet").val().replace(/NBSP/gi, 'nbsp'));
        ifrmForm.document.close();
        // 이전문서
        ifrmParent.document.open();
        ifrmParent.document.write($("#htaFormHTML").val());
        ifrmParent.document.close();

        ifrmObj = ifrmForm.document;
        ifrmParentObj = ifrmParent.document;

        setTimeout("Add.SetOld(false, '');", 100);           // _OLD 처리

        // 자주지정하는 결재자 정보
        Common.GetFavoriteApp();

        // 양식결재라인 정보
        Common.GetFormSeq(FormID);

        // 이벤트 지정 (For.연동파일)
        $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
        $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
        $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
        $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    } else {

        ifrmForm.document.open();
        ifrmForm.document.write($("#htaFormHTML").val());
        ifrmForm.document.close();

        ifrmObj = ifrmForm.document;

        // 고정수신결재망 관련 버튼처리
        setTimeout(function () {
            if ($("#ZQ", ifrmObj).length == 0 && $("#ZP", ifrmObj).length == 0 && $("#" + AccountTag, ifrmObj).length > 0) {
                $("#ulSet").css("display", "");
            }
        }, 100);
    }
    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());
    Common.SetInActive($('#ifrmParent').get());

    // 문서번호 정보

    // 기타정보설정-문서종류
    $("#ddlGuard", parent.frmLeft.document).val($("#tbIsGuard").val());

    // 기타정보설정-열람권한
    var obj = $("#ddlAccessTitle option[value='" + $("#tbAccessTitleID").val() + "']", parent.frmLeft.document);
    if (obj.length > 0) {
        $("#ddlAccessTitle", parent.frmLeft.document).val($("#tbAccessTitleID").val());
    }
    parent.frmLeft.Common.SetGradeInfo($("#ddlAccessTitle", parent.frmLeft.document).val());

    // 기타정보설정-참조열람
    $("#ddlCarbonView", parent.frmLeft.document).val($("#tbCarbonView").val());

    // 커스텀연동 호출
    setTimeout(function () { Common.GetLegacy(CustomFilePath) }, 100);
}
// 원본문서 id & class 변경
Add.SetOld = function (_isFix, _class) {    //alert(_isFix + ":" + _class);

    var oldTag = Guids();

    // 변경대상 초기화 (span & div & input) : 결재태그
    var arrTag = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA, .formfieldinput, .formFieldInput", ifrmParentObj);
    $.each(arrTag, function (i, o) {

        if (AccountTag.indexOf("ZQ") < 0) {
            if (o.id.toUpperCase() != "OPINION" && o.id.toUpperCase() != "OPINIONVIEW" && o.id.toUpperCase() != "PRINT") {
                o.id = oldTag + "_" + o.id;         // o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
                o.className = oldTag + "_" + o.className;
            }
        } else {
            if (o.id.toUpperCase() != "NO" && o.id.toUpperCase() != "OPINION" && o.id.toUpperCase() != "OPINIONVIEW" && o.id.toUpperCase() != "PRINT") {       // 문서번호('NO'),첨언('OPINION') 예외처리 필요
                o.id = oldTag + "_" + o.id;         // o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
                o.className = oldTag + "_" + o.className;
            }
        }
    });

    // 원본문서 가져오기(붙이기)
    var targetID = "#PARENTDOCUMENT_" + AccountTag;
    if ($(targetID, ifrmObj).length > 0) {
        $(targetID, ifrmObj).html($("body", ifrmParentObj).html());
    }
    // $("#txtDebug").val($("body", ifrmParentObj).html());

    // 수신고정결재망의 경우, tag replace 처리 : ZQ01_ZQ = ZD0.xxx & 결재라인정보 가져오기
    if (_isFix) {
        arrTag = $(_class, ifrmObj);    //alert("tag replace2:" + arrTag.length);
        $.each(arrTag, function (i, o) {
            if (o.id.indexOf(AccountTag) == 0) {

                // tag backup (attr이용) - 최초상신자 반려 후, 재상신 시, 초기화할때 이용. (2013.11.20)
                $("#" + o.id, ifrmObj).attr("btag", AccountTag)

                // tag replace 처리
                o.id = (o.id.indexOf('_') < 0) ? "ZD0" : o.id.replace(AccountTag + "_", "");
                o.id = (o.id.indexOf('INFO') == 0) ? "ZD0_" + o.id : o.id;      //2013.11.30 고정수신망info누락 버그수정
                o.className = o.id.substring(0, 2);
                // 결재라인정보 설정
                if (o.id.indexOf("INFO") < 0) {
                    formSeqDic.add(o.id, parseInt(o.id.replace(o.className, "")) + 1);      // (ZD0,1)  (ZD1,2)  (ZC,3)  (ZP,4)
                }
            }
        });
    }
    // $("#txtDebug").val($("body", ifrmObj).html());

    // 결재망 초기화
    setTimeout("Add.InitForm();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 양식초기화
***************************************************/
Add.InitForm = function () {

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
        if ($("#" + o.id, ifrmObj).attr("isapp")) {
            $("#" + o.id, ifrmObj).removeAttr("isapp")
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.id.toUpperCase() != "NO") {
            if (o.tagName.toUpperCase() == "INPUT") { o.value = ""; o.setAttribute("readonly", "true"); }
            //if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }   //첨언태그
        }
        Common.ExceptObj(o);   // 특정필드 예외처리 (에디터 ..)
    });

    // 기본문서정보 설정 (양식관련 기본정보 처리 / 문서번호,보존년한,제목)
    Common.SetDefaultInfo();

    // 입력필드 활성화
    var actObj1 = $.grep(arrInfo, function (o, i) {
        return (o.id.indexOf("BODY") == 0);
    });
    var actObj2 = $("#SUBJ,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each($.merge(actObj1, actObj2), function (i, o) {
        $("#" + o.id, ifrmObj).removeAttr("unselectable");
        $("#" + o.id, ifrmObj).removeAttr("readonly");
        $("#" + o.id, ifrmObj).attr("contenteditable", "true");
    });

    // 결재망 추가 후 수신에서 내용 수정 가능하게 처리
    var actObj = $("div", ifrmObj);
    $.each(actObj, function (i, o) {
        if (o.id.indexOf("BODY_") > -1) {
            if ($(o).attr("unselectable")) { $(o).removeAttr("unselectable"); }
            if ($(o).attr("contenteditable")) { $(o).attr("contenteditable", "true"); }

            if ($(o).find('div').attr("unselectable")) { $(o).find('div').removeAttr("unselectable"); }
            if ($(o).find('div').attr("contenteditable")) { $(o).find('div').attr("contenteditable", "true"); }
        }
    });

    // 유동결재 초기화
    Common.SetDefault();
    if ($("#FLOWZD", ifrmObj).length > 0) {
        Common.SetFlowInfo("FLOWZD:1");
    }

    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재선추가]
***************************************************/
Add.FlowAdd = function () {
    if (FormID < 1) { alert(R_Msg_ErrSelNet); return; }
    nowEl = ifrmForm.document.all("FLOWZD");
    CommonFlow.Add(1, nowEl);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재선삭제]
***************************************************/
Add.FlowDel = function () {
    if (FormID < 1) { alert(R_Msg_ErrSelNet); return; }
    nowEl = ifrmForm.document.all("FLOWZD");
    CommonFlow.Remove(nowEl);
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : [문서상신]
***************************************************/
Add.Submit = function () {

    if (ChangeDeptID > 0) {
        if (!confirm(R_ModifyList + " : " + TargetDeptName + " -> " + DeptName + "\n" + DeptName + R_ConfirmAdd)) {
            return;
        }
    }

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    if (FormID < 1) { alert(R_Msg_ErrSelNet); return; }

    //필수입력 - 결재라인
    //if (appSeqDic.size() < 2) { alert(Msg_SetLine); return; }

    // 제목
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $.trim($("#SUBJ", ifrmObj).val());
        if (subj == "") { alert(R_Msg_ErrSubj); $("#SUBJ", ifrmObj).focus(); return; }
    }

    //보존년한 - 기본값:원본문서
    if ($("#LIFE", ifrmObj).length > 0) {
        var life = $.trim($("#LIFE", ifrmObj).val());
        if (life == "") { alert(R_Msg_ErrLife); $("#LIFE", ifrmObj).focus(); return; }
        $("#tbLifeTime").val(life);
    }

    if ($(".ZD", ifrmObj).text().indexOf("결재") > 0) {
        alert("결재자를 지정하지 않은 결재칸이 있습니다.\n결재칸을 삭제 또는 결재자를 지정해주시기 바랍니다.");
        return;
    }

    // 문서종류 - 기본값:원본문서
    var guardVal = $("#ddlGuard", parent.frmLeft.document).val();
    $("#tbIsGuard").val(guardVal);

    // 열람권한  - 기본값:원본문서
    var accessVal = $("#ddlAccessTitle", parent.frmLeft.document).val();
    $("#tbAccessTitleID").val(accessVal);

    // 참조열람
    var carbonViewVal = $("#ddlCarbonView", parent.frmLeft.document).val();
    $("#tbCarbonView").val(carbonViewVal);

    // 결재의견 (For.첨언처리)
    var opinion = $("#tbOpinion", parent.frmLeft.document).val();
    if (opinion.length > 500) {
        alert(R_Msg_ErrOpinion);
        $("#tbOpinion", parent.frmLeft.document).focus();
        parent.frmLeft.Common.SetMenu(3); //트리메뉴제어(결재의견)
        return;
    }
    $("#tbOpinion").val(opinion.replace(/\n/gi, "<br>").replace(/ /g, "&nbsp;"));

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    if ($.browser.msie) {
        var rtnVal = Common.ConfirmSequence()
        ConfirmModSequenceFunc(rtnVal);

    } else {
        appSeqKey = Common.GetAppSeqQueue(appSeqDic);
        // Common.GetAppSeqQueue(appSeqDic);
        //appSeqKey = appSeqKeyPush;  // 선택한 순서로 결재진행 표시 ByFreeman 20131023
        var rtnval = false;
        var url = "/DeskPlusEIP/Approval/FormFlow/Common/ModSequence.aspx";
        PopupWin(400, 465, url, "", "", true);
    }
}

function ConfirmModSequenceFunc(rtnVal) {
    if (rtnVal) {
        // 유동결재망 처리
        $("#tbIsUseFlow").val(Common.GetFlowInfo());

        // 결재순서정렬
        appSeqKey = Common.GetAppSeqQueue(appSeqDic);
        // 결재정보처리 (순서에 맞게)
        $("#htaXmlSeq").val(Common.GetSeqXML(appSeqKey, appSeqDic, appSeqDicZR));    // 발신결재
        $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));       // 수신결재
        $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));       // 참조결재

        // 병렬합의 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZA") > 0) {
            alert(R_Msg_ErrSetZA); return;
        }
        // 협조 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZH") > 0) {
            //alert(R_Msg_ErrSetZH); return;
        }

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
    if (ApprovalType == "S") {
        AddSubmitS();           //ApprovalType에 따른 분기
    } else {
        AddSubmitR();
    }

}

//상신
AddSubmitS = function () {

    // 제목 처리
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $("#SUBJ", ifrmObj).val();
        $("#tbSubject").val(subj);
    }

    // 문서내용(에디터) + 에디터이미지 처리(activeX인경우만, 이미지 업로드)
    Common.SetEditorTags(NonActiveX);

    //  빈 결재란 처리
    Common.SetDefaultTags();

    //  인장처리 (+결재정보+서버시간 체크)
    if (TargetUserID == UserID || ProxyUserID == "0") {
        Common.SetStamp("ZD0", SealPath, UserName, DeptName, TitleName, DutyName, "0");
    }
    else {
        Common.SetStamp("ZD0", SealPath, UserName, DeptName, TitleName, DutyName, UserID);
    }

    // 부서수신 결재 때문에 결재자 정보를 변경한다.
    if (TargetUserID != UserID && ProxyUserID == "0") {
        Add.ModifySusin("1");
    }

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
Add.SubmitClose = function (_msg) {
    alert(_msg);

    ParamArray = {};    //상신한문서로 카테고리아이디 맞추기.'p':'p'
    CallEncrypt(CallGetParam());
    var url = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
    parent.document.location.href = url;
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [인쇄]
***************************************************/
Add.Preview = function () {

    var divToPrint = document.getElementById('ifrmForm');

    //스타일정보+내용
    var html = divToPrint.contentWindow.document.documentElement.outerHTML;
    newWin = window.open();
    newWin.document.write(html);

    //alert(navigator.userAgent)
    if (navigator.userAgent.indexOf("Chrome") > 0 || navigator.userAgent.indexOf("Safari") > 0) {
        setTimeout("newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
    } else {
        newWin.location.reload();                               //for, exploror & firefox
        newWin.focus();
        newWin.print();
    }

    setTimeout("newWin.close();", 1000);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [반려]
***************************************************/
var targetInfo = "";
Add.Reject = function () {
    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    //if (FormID > 0) { alert(R_Msg_ErrNetNon); return; }

    //필수입력 - 첨언
    if ($("#tbOpinion", parent.frmLeft.document).val() == "") {
        alert(R_Msg_ErrOpinionNon);
        parent.frmLeft.Common.SetMenu(3); //트리메뉴제어(결재의견)
        $("#tbOpinion", parent.frmLeft.document).focus();
        return;
    }

    // 결재의견 (For.첨언처리)
    var opinion = $("#tbOpinion", parent.frmLeft.document).val();
    if (opinion.length > 500) {
        alert(R_Msg_ErrOpinion);
        $("#tbOpinion", parent.frmLeft.document).focus();
        parent.frmLeft.Common.SetMenu(3); //트리메뉴제어(결재의견)
        return;
    }

    $("#tbOpinion").val(opinion.replace(/\n/gi, "<br>").replace(/ /g, "&nbsp;"));

    Add.BeforeLine();

    // 서버시간 체크
    Common.GetServerTime();

    if ($.browser.msie) {  //크롬에서 window.showModalDialog 지원하지 않는 문제 
        // 반려대상
        targetInfo = Add.ConfirmReject();
        if (targetInfo == "!NULL" || targetInfo == "") { return; }

        ApprovalType = "R";
        Common.SetLoading(true);   // 처리중

        if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
            ifrmProcessing.onComplete();
        } else {                                                        // 그외의 경우, submit호출
            AddSubmitR();
        }
    } else {
        ParamArray = { 'documentID': ParentDocumentID, 'groupID': GroupID, 'isType': '2' };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/FormFlow/Receive/ConfirmReject.aspx?p=" + GetEncryptValue;
        PopupWin(600, 430, url, "", "", true);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 반려대상
***************************************************/
Add.ConfirmReject = function () {

    ParamArray = { 'documentID': ParentDocumentID, 'groupID': GroupID, 'isType': '2' };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/FormFlow/Receive/ConfirmReject.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupModalWinOpen(url, "600", "430");

    var rtnval = (WindowObjectReference == undefined || WindowObjectReference == "") ? "!NULL" : WindowObjectReference;
    //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID

    return rtnval;
}

function ConfirmRejectFunc(_targetInfo) {
    if (_targetInfo == "!NULL" || _targetInfo == "") { return; }

    targetInfo = _targetInfo;

    ApprovalType = "R";
    Common.SetLoading(true);   // 처리중

    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        ifrmProcessing.onComplete();

    } else {                                                        // 그외의 경우, submit호출
        AddSubmitR();
    }
}

//반려
AddSubmitR = function () {

    // 부서수신 결재 때문에 결재자 정보를 변경한다.
    if (TargetUserID != UserID && ProxyUserID == "0") {
        Add.ModifySusin("0");
    }

    // 첨언처리 $("#tbOpinion").val()
    Common.SetOpinion(UserName, TitleName, DutyName, DeptName, $("#tbOpinion").val());

    // Input Tag 처리
    Common.SetReadOnly();

    var content = RemoveScriptTag($("HTML", ifrmObj).html());

    // 문서내용(Html)처리
    //$("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

    //반려대상지정
    $("#tbTargetInfo").val(targetInfo); //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID
    __doPostBack('lbtnSubmit', "REJECT");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [담당자변경]
***************************************************/
Add.Modify = function () {

    Add.BeforeLine();

    if (FormID > 0) { alert(R_Msg_ErrNetNonReplace); return; }

    ParamArray = {
        'gubun': '2',
        'isCheckBox': '0',
        'callback': 'Add.ModifySuccess'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/GetDeptUser.aspx?p=" + GetEncryptValue;
    PopupWin(400, 507, url, "-500", "", true);
}
Add.ModifySuccess = function (response) {
    var rtnVal = response;

    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        rtnVal = unescape(rtnVal);
        var responseStr = rtnVal.split("");

        if (responseStr[3] == "G,") { alert(R_Msg_ErrSelUser); return; }

        if (confirm(R_Msg_Change)) {

            //tbTargetInfo : [0]UserID-[1]DeptID
            $("#tbTargetInfo").val(responseStr[0] + "-" + responseStr[2]);

            // 내용담당자 변경
            if ($("#" + AccountTag, ifrmObj).length > 0) {

                // type(M) | userID | userName | deptID | titleID | titleName | loginID | mail            | phone | officePhone | DutyName | DeptName | DeptID
                // M       | 45     | test     | 2      | 3       | 사원      | test    | test@kgcrane.com|       |             | 팀원     | 영업부   | 2
                var items = responseStr[4].split("|");

                $("#" + AccountTag, ifrmObj).attr("deptID", responseStr[2].replace(",", ""));
                $("#" + AccountTag, ifrmObj).attr("userID", responseStr[0].replace(",", ""));
                $("#" + AccountTag, ifrmObj).attr("deptName", items[11]);
                $("#" + AccountTag, ifrmObj).attr("titleID", items[4]);
                $("#" + AccountTag, ifrmObj).attr("titleName", items[5]);
                $("#" + AccountTag, ifrmObj).attr("dutyID", "0");
                $("#" + AccountTag, ifrmObj).attr("dutyName", items[10]);

                var itemName = items[2].split("/");
                $("#" + AccountTag, ifrmObj).html(items[11] + "/" + items[5] + "/" + itemName[0] + ",");

            }
            else {
                $.each($("span[idx]", ifrmObj), function (i, o) {
                    if (o.id.indexOf(AccountTag) > -1) {
                        var items = responseStr[4].split("|");
                        $(o).attr("deptID", responseStr[2].replace(",", ""));
                        $(o).attr("userID", responseStr[0].replace(",", ""));
                        $(o).attr("deptName", items[11]);
                        $(o).attr("titleID", items[4]);
                        $(o).attr("titleName", items[5]);
                        $(o).attr("dutyID", "0");
                        $(o).attr("dutyName", items[10]);

                        var itemName = items[2].split("/");
                        $(o).html(items[11] + "/" + items[5] + "/" + itemName + ",");

                    }
                });
            }

            var content = RemoveScriptTag($("HTML", ifrmObj).html());

            // 문서내용(Html)처리
            $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
            $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

            __doPostBack('lbtnSubmit', "MODIFY");
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [완료]
***************************************************/
Add.Complete = function () {

    Add.BeforeLine();

    if (FormID > 0) { alert(R_Msg_ErrNetNonCom); return; }

    if (confirm(R_Msg_ErrComplete)) {

        // 부서수신 결재 때문에 결재자 정보를 변경한다.
        if (TargetUserID != UserID && ProxyUserID == "0") {
            Add.ModifySusin("0");

            var content = RemoveScriptTag($("HTML", ifrmObj).html());

            // 문서내용(Html)처리
            $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
            $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));
        }

        __doPostBack('lbtnSubmit', "COMPLETE");
    }
    else {
        Add.AfterLine();
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 목록으로 가기 
***************************************************/
Add.List = function () {

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
    parent.location.href = Add.GetDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
}

Add.GetDefaultPage = function (defaultPage) {
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

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재자지정]
***************************************************/
Add.SetLine = function () {

    if (chkOneClick) { return; }    // 이중클릭 체크

    //1. 트리메뉴제어(조직도)
    parent.frmLeft.Common.SetMenu(1);

    //2. css replace : ZQ->ZQ01
    var arrCss = "";        //ex) .ZQ01.ZQ02
    var arrTag = $(".ZP, .ZQ, .ZQ02", ifrmObj);    //alert("tag replace1:" + arrTag.length);
    $.each(arrTag, function (i, o) {
        o.className = o.id.split("_")[0];
        arrCss = (arrCss.indexOf(o.className) < 0) ? arrCss + "." + o.className + "," : arrCss;
    });
    arrCss = arrCss.trimEnds(',');
    $("#htaFormHTML").val(ifrmObj.documentElement.outerHTML);

    //3. old 처리
    var netHTML = "<HTML><HEAD><META content='text/html; charset=utf-8' http-equiv='Content-Type'>";
    netHTML += "<LINK rel='stylesheet' type='text/css' href='" + FullUrl + "/DeskPlusEIP/Approval/Css/Func_ApprovalEdit.css'>";
    netHTML += "<STYLE>p {margin-top:2px;margin-bottom:2px;} table {font-family:gulim;font-size:9pt;}div {font-family:gulim;font-size:9pt;}</STYLE>";
    netHTML += "</HEAD><BODY>";
    netHTML += "<div id='PARENTDOCUMENT_" + AccountTag + "'></div>";
    netHTML += "</BODY></HTML>";

    // 결재망
    ifrmForm.document.open();
    ifrmForm.document.write(netHTML);
    ifrmForm.document.close();
    // 이전문서
    ifrmParent.document.open();
    ifrmParent.document.write($("#htaFormHTML").val());
    ifrmParent.document.close();

    ifrmObj = ifrmForm.document;
    ifrmParentObj = ifrmParent.document;

    setTimeout("Add.SetOld(true, '" + arrCss + "');", 100);           // _OLD 처리 & tag Replace & formSeq sett

    //4. 자주지정하는 결재자 정보
    Common.GetFavoriteApp();                    // 자주지정하는 결재자 정보

    //6. 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    FormID = 1000;  // validation check을 위해, 임시로 sett.
    $("#txtDebug").val($("body", ifrmObj).html());

    chkOneClick = true;
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : [문서수정]
**********************************************************************************************/
Add.Edit = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listID': "LISTRECEIVEMOD",
        'docID': ParentDocumentID,
        'groupID': GroupID,
        'sequence': Sequence,
        'approvalTag': ApprovalTag,
        'accountTag': AccountTag,
        'targetUserID': TargetUserID,
        'targetDeptID': TargetDeptID,
        'folderStatusID': FolderStatusID,
        'targetPage': TargetPage,
        'proxyUserID': ProxyUserID
    };

    CallEncrypt(CallGetParam());
    var url = Add.GetDefaultPage("/DeskPlusEIP/Approval/FormFlow/Default.aspx");
    parent.location.href = url + "?p=" + GetEncryptValue;
}


Add.GetEDM = function () {
    ParamArray = {
        'CompanyID': CompanyID,
        'USERID': LoginID,
        'USERPASSWORD': PWD,
        'targetURL': '/DeskplusEDM/Folder/Dialog/transEDM/GetFolder.aspx'
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEDM/Authentication/LoginOutSide.aspx?p=" + GetEncryptValue;


    if ($.browser.msie) {  //크롬에서 window.showModalDialog 지원하지 않는 문제 
        // 반려대상
        var rtnVal = window.showModalDialog(url, "", "dialogWidth:300px; dialogHeight:366px; center: Yes; help: No; resizable: No; status: No; scroll:No");
        //PopupWin(300, 366, url, "", "", true);
        ConfirmGetEDMFunc(rtnVal);

    } else {
        PopupWin(350, 400, url, "", "", true);
    }
}

function ConfirmGetEDMFunc(rtnVal) {
    //PopupWin(300, 366, url, "", "", true);
    //캐비넷값이 변경되었다면!!
    if (rtnVal != "" && rtnVal != undefined) {
        $("#tbIsLinkedEtc").val(rtnVal.split('^')[0]);

        // 양식에 이관 항목 표시 필드가 있다면 채워준다.
        if ($("#spEDMFolderNM", ifrmObj).length > 0) {
            // span 인경우
            $("#spEDMFolderNM", ifrmObj).text(rtnVal.split('^')[1]);
            // 그 외인 경우 -- 왠만하면 span 사용하지;;
            //$("#spEDMFolderNM", ifrmObj).val(rtnVal.split('^')[1]);
        }
    }
}

// 부서수신 결재 때문에 결재자 정보를 변경한다.
Add.ModifySusin = function (_old) {
    /*
    var tagID = "";
    if (_old == "1") {
    tagID = OldTagKey + "_" + AccountTag;
    }
    else {
    tagID = AccountTag;
    }

    $("#" + tagID, ifrmObj).attr("deptID", DeptID);
    $("#" + tagID, ifrmObj).attr("userID", UserID);
    $("#" + tagID, ifrmObj).attr("deptName", DeptName);
    $("#" + tagID, ifrmObj).attr("titleID", TitleID);
    $("#" + tagID, ifrmObj).attr("titleName", TitleName);
    $("#" + tagID, ifrmObj).attr("dutyID", DutyID);
    $("#" + tagID, ifrmObj).attr("dutyName", DutyName);

    $("#" + tagID, ifrmObj).html(DeptName + "/" + TitleName + "/" + UserName + ",");
    */
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부
**********************************************************************************************/
Add.OpinionCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/OpinionView.asmx/GetOpinionCount";

    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ParentDocumentID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessOpinionCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부 이벤트
**********************************************************************************************/
Add.SuccessOpinionCount = function (data, status) {

    var count = parseInt(data.d || 0, 10);

    if (count > 0) {
        if (FormID <= 0) {
            Add.OpinionInfo();
        }
    }

    var baseText = $("#cbtnOpinionInfo").text();

    $("#cbtnOpinionInfo").text(baseText + " (" + count + "건)");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 조회
**********************************************************************************************/
Add.OpinionInfo = function () {
    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ParentDocumentID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/OpinionView.aspx?p=" + GetEncryptValue;
    PopupWin(600, 540, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 현재 수신 결재순서 여부 확인
**********************************************************************************************/
Add.GetIsMyTurn = function () {

    if (AccountTag == "PS") {
        IsMyTurnState = true;
        return;
    }

    if (CompanyID == 0) return;

    ParamArray = {
        'documentID': GroupID,
        'sequence': Sequence,
        'accountTag': AccountTag,
        'targetUserID': TargetUserID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetIsMyTurn");
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Receive/Add.asmx/GetIsMyTurn";
    AjaxService(url, data, obj);
}
Add.SuccessGetIsMyTurn = function (data, state) {
    if (data.d.toUpperCase() == "Y") {
        IsMyTurnState = true;

    }
    else {
        IsMyTurnState = false;
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [개인결재망]
***************************************************/
Add.PrivateNet = function () {

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }
    // 팝업 clear처리
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV").hide();

    if ($.browser.msie) {
        var url = "/DeskPlusEIP/Approval/FormFlow/Common/GetFavList.aspx";
        WindowObjectReference = PopupModalWinOpen(url, "600", "300")
    } else {
        ParamArray = { 'p': 'p' };
        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Approval/FormFlow/Common/GetFavList.aspx?p=" + GetEncryptValue;
        PopupWin("600", "300", url, "", "", true);
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재망 추가 이전]
***************************************************/
Add.BeforeLine = function () {

    appSeqDic = new jQuery.Hashtable();
    appSeqKeyPush = new Array();

    FormID = "0";

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;

    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재망 추가 상태]
***************************************************/
Add.AfterLine = function () {

    //FormID = "281";
    FormID = "89";

    parent.frmLeft.Common.SetMenu(1); //트리메뉴제어(조직도)

    // 결재망
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormNet").val().replace(/NBSP/gi, 'nbsp'));
    ifrmForm.document.close();
    // 이전문서
    ifrmParent.document.open();
    ifrmParent.document.write($("#htaFormHTML").val());
    ifrmParent.document.close();

    ifrmObj = ifrmForm.document;
    ifrmParentObj = ifrmParent.document;

    setTimeout("Add.SetOld(false, '');", 100);           // _OLD 처리

    // 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());
    Common.SetInActive($('#ifrmParent').get());

    nowEl = null;
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
