Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // [미리보기]
        $('#cbtnPreview').click(function (e) { e.preventDefault(); Add.Preview(); });
        // [개인결재망]
        $('#cbtnPrivateNet').click(function (e) { e.preventDefault(); Add.PrivateNet(); });
        // [결재망저장]
        $('#cbtnSaveNet').click(function (e) { e.preventDefault(); Add.SaveNet(); });
        // [임시저장]
        $('#cbtnSaveDraft').click(function (e) { e.preventDefault(); Add.SaveDraft(); });
        // [결재순서변경]
        $('#cbtnModSeq').click(function (e) { e.preventDefault(); Common.SeqOrder(true, false); });
        // [문서상신]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); Add.Submit(); });
        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); Common.AttachSwfFile(); });
        // [근거문서-추가]
        $('#cbtnDocAdd').click(function (e) { e.preventDefault(); Common.AttachBaseDoc(); });

        // [결재선추가]
        $('#cbtnFlowAdd').click(function (e) { e.preventDefault(); Add.FlowAdd(); });
        // [결재선삭제]
        $('#cbtnFlowDel').click(function (e) { e.preventDefault(); Add.FlowDel(); });

        //개발용(근거문서 추가시 문서번호 보이기)
        $('#tbAddDocID').val(BaseDocIDs);

        // [EDM설정]
        $('#cbtnEDM').click(function (e) { e.preventDefault(); Add.GetEDM(); });

        Add.Init();
        Common.GetSpoolFileList();
    });
});

/**********************************************************************************************
작성자   : Freeman
작성일자 : 2012.06.27
작성목적 : 인장 생성후 페이지 리로드

function PopupWinReturnFunc() {
location.reload();
}
**********************************************************************************************/

Add.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {           //인장체크 (없으면 서명등록)
        $("#ulSubmit").css("display", "none");

        alert(R_MSG_ERRSEAL);

        var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
        PopupWin(300, 300, url, "-500", "", true);
        return;
    }

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

        if (NickName == "") {
            parent.frmLeft.Common.SetMenu(1); //트리메뉴제어(조직도)
        }

        // 양식로드
        ifrmForm.document.open();
        ifrmForm.document.write($("#htaFormHTML").val());
        ifrmForm.document.close();

        ifrmObj = ifrmForm.document;
        setTimeout("Add.InitForm();", 100);

        // 이벤트 지정 (For.연동파일)
        $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
        $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
        $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
        $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

        // 자주지정하는 결재자 정보
        Common.GetFavoriteApp();

        // 양식결재라인 정보
        Common.GetFormSeq(FormID);


        //지정된 결재자중 변경된 결재자 리스트
        Add.GetNetModfySeq(FormID);

        // 문서번호 정보
    } else {
        // 자주사용하는양식
        ifrmForm.document.location.href = "/DeskPlusEIP/Approval/Preference/FavoriteForm/Add.aspx";
    }
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
            $("#" + o.id, ifrmObj).removeAttr("isapp")      //승인시, isapp 어트리뷰트값 setting
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);
    $.each(arrInfo, function (i, o) {
        if (o.tagName.toUpperCase() == "INPUT") { o.value = ""; o.setAttribute("readonly", "true"); }
        if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }

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

    // 유동결재 초기화
    Common.SetDefault();

    if ($("#FLOWZD", ifrmObj).length > 0) {
        Common.SetFlowInfo("FLOWZD:1");
    }

    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
    Add.GetNetSeq(FormID);

    //수신자 지정
    if (appSeqDicZQ.size() < 1) {
        Add.GetNetZQSeq(FormID);
    }

    //참조자지정
    if (appSeqDicZC.size() < 1) {
        Add.GetNetZCSeq(FormID);
    }


    // 커스텀연동 호출
    //CustomFilePath = CustomFilePath + "?legacykey=" + LegacyKey;
    Common.GetLegacy(CustomFilePath);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재선추가]
***************************************************/
Add.FlowAdd = function () {
    nowEl = ifrmForm.document.all("FLOWZD");
    CommonFlow.Add(1, nowEl);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [결재선삭제]
***************************************************/
Add.FlowDel = function () {
    nowEl = ifrmForm.document.all("FLOWZD");
    CommonFlow.Remove(nowEl);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [문서상신]
***************************************************/
Add.Submit = function () {

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }

    //필수입력 - 결재라인
    //if (appSeqDic.size() < 2) { alert(Msg_SetLine); return; }

    //필수입력 - 제목
    if ($("#SUBJ", ifrmObj).length > 0) {
        var subj = $.trim($("#SUBJ", ifrmObj).val());
        if (subj == "") { alert(R_Msg_ErrSubj); $("#SUBJ", ifrmObj).focus(); return; }
    }
    
    if ($(".ZD", ifrmObj).text().indexOf("결재") > 0) {
        alert("결재자를 지정하지 않은 결재칸이 있습니다.\n결재칸을 삭제 또는 결재자를 지정해주시기 바랍니다.");
        return;
    }
    if ($(".ZH", ifrmObj).text().indexOf("결재") > 0 || $(".ZH", ifrmObj).text().indexOf("협조") > 0) {
        alert("협의자를 지정하지 않은 결재칸이 있습니다.\n결재칸을 삭제 또는 협의자를 지정해주시기 바랍니다.");
        return;
    }
    //조건별 필수 입력- EDM
    if (IsLinked == "1" && IsLinkedType == "2") {
        var edmVal = $("#tbIsLinkedEtc").val();
        if (edmVal == "") { alert(R_Msg_AlertEDMCheck); $("#cbtnEDM").click(); return; }
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

    // 부서정보 (상신부서)
    var deptVal = $("#ddlAdditionDept", parent.frmLeft.document).val();     //38|솔루션개발팀|15|과장|41|파트장
    $("#tbDeptID").val(deptVal.split("|")[0]);
    $("#tbDeptName").val(deptVal.split("|")[1]);

    // 문서종류
    var guardVal = $("#ddlGuard", parent.frmLeft.document).val();
    $("#tbIsGuard").val(guardVal);

    // 열람권한 
    var accessVal = $("#ddlAccessTitle", parent.frmLeft.document).val();
    $("#tbAccessTitleID").val(accessVal);

    // 참조열람
    var carbonViewVal = $("#ddlCarbonView", parent.frmLeft.document).val();
    $("#tbCarbonView").val(carbonViewVal);



    // 결재의견 (For.첨언처리)
    var opinion = $("#tbOpinion", parent.frmLeft.document).val();
    if (opinion.length > 200) {
        alert(R_Msg_ErrOpinion);
        $("#tbOpinion", parent.frmLeft.document).focus();
        parent.frmLeft.Common.SetMenu(3); //트리메뉴제어(결재의견)
        return;
    }
    $("#tbOpinion").val(opinion.replace(/\n/gi, "<br>").replace(/ /g, "&nbsp;"));

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    // 팝업 clear처리
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV").hide();

    if ($.browser.msie) {
        var rtnVal = Common.ConfirmSequence()
        ConfirmModSequenceFunc(rtnVal);
    } else {
        appSeqKey = Common.GetAppSeqQueue(appSeqDic);
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
        $("#htaXmlSeq").val(Common.GetSeqXML(appSeqKey, appSeqDic, appSeqDicZR));   // 발신결재
        $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));                   // 수신결재
        $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));                   // 참조결재

        // 병렬합의 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZA") > 0) {
            alert(R_Msg_ErrSetZA); return;
        }
        // 협조 최종결재지정여부 체크
        if (appSeqKey[appSeqKey.length - 1].indexOf("ZH") > 0) {
            //alert(R_Msg_ErrSetZH); return;
        }

        $("#tbUserID").val(UserID); // For.상신자 체크

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

    // 제목 처리
    if ($("#SUBJ", ifrmObj).length > 0) {
        $("#tbSubject").val($("#SUBJ", ifrmObj).val());
    }

    // 문서내용(에디터) + 에디터이미지 처리(activeX인경우만, 이미지 업로드)
    Common.SetEditorTags(NonActiveX);

    //  빈 결재란 처리
    Common.SetDefaultTags();

    //  인장처리 (+결재정보+서버시간 체크)
    Common.SetStamp("ZD0", SealPath, UserName, DeptName, TitleName, DutyName, "0");

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
작성목적 : 문서상신 후, 호출 (서버호출)
***************************************************/
var documentID = 0; var msg = "";
SubmitAfterCall = function (_customPath, _docID, _msg) {

    if (_customPath == "") { SubmitClose(_msg); }

    documentID = _docID;
    msg = _msg;

    // 커스텀연동 호출
    Common.GetLegacy(_customPath);

    /*
    setTimeout(function () {
    if (ifrmProcessing.onCompleteAfter) {
    ifrmProcessing.onCompleteAfter(_docID);
    } 
    }, 1000);
    */
}
SubmitAfterCallOK = function () {
    SubmitClose(msg);
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 문서상신 후(성공/실패), 페이지 이동 (서버호출)
***************************************************/
SubmitClose = function (_msg) {
    alert(_msg);

    ParamArray = {};    //상신한문서로 카테고리아이디 맞추기.'p':'p'
    CallEncrypt(CallGetParam());
    var url = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
    //parent.document.location.href = url;

    if (top.opener) {
        //top.opener.parent.document.location.href = url;
        top.close();
    }
    else {
        parent.document.location.href = url;
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 문서상신 후(성공/실패), 페이지 이동 (서버호출)
***************************************************/
SubmitCloseEtc = function () {
    alert("중복된 문서로 상신이 제한됩니다.");

    ParamArray = {};    //상신한문서로 카테고리아이디 맞추기.'p':'p'
    CallEncrypt(CallGetParam());
    var url = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
    //parent.document.location.href = url;

    if (top.opener) {
        //top.opener.parent.document.location.href = url;
        top.close();
    }
    else {
        parent.document.location.href = url;
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [미리보기]
***************************************************/
Add.Preview = function () {

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }
    // 팝업 clear처리
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV").hide();

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
작성목적 : [임시저장]
***************************************************/
Add.SaveDraft = function () {

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }
    // 팝업 clear처리
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();
    $("#zeoEditorDIV").hide();

    if (Add.SetDraftInfo()) {

        if ($.browser.msie) {
            var url = "/DeskPlusEIP/Approval/FormFlow/Common/SaveDraft.aspx";
            WindowObjectReference = PopupModalWinOpen(url, "400", "150")
        } else {
            ParamArray = { 'p': 'p' };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/FormFlow/Common/SaveDraft.aspx?p=" + GetEncryptValue;
            PopupWin("400", "150", url, "", "", true);
        }
    }
}
Add.SetDraftInfo = function () {

    // 문서종류
    var guardVal = $("#ddlGuard", parent.frmLeft.document).val();
    $("#tbIsGuard").val(guardVal);

    // 열람권한 
    var accessVal = $("#ddlAccessTitle", parent.frmLeft.document).val();
    $("#tbAccessTitleID").val(accessVal);

    // 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    // 유동결재망 처리
    $("#tbIsUseFlow").val(Common.GetFlowInfo());

    // 참조열람
    var carbonViewVal = $("#ddlCarbonView", parent.frmLeft.document).val();
    $("#tbCarbonView").val(carbonViewVal);

    // 결재순서정렬
    appSeqKey = Common.GetAppSeqQueue(appSeqDic);
    // 결재정보처리 (순서에 맞게)
    $("#htaXmlSeq").val(Common.GetSeqXML(appSeqKey, appSeqDic, appSeqDicZR));   // 발신결재
    $("#htaXmlSeqZQ").val(Common.GetSeqXMLZQZC(appSeqDicZQ));                   // 수신결재
    $("#htaXmlSeqZC").val(Common.GetSeqXMLZQZC(appSeqDicZC));                   // 참조결재

    // 특정태그처리 (for, chrome,ff,safari)
    Common.UpdateDOM();

    // 문서내용(Html)처리
    $("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));

    return true;
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
작성목적 : [결재망저장]
***************************************************/
Add.SaveNet = function () {

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }

    // 결재순서정렬
    appSeqKey = Common.GetAppSeqQueue(appSeqDic);               // 결재순서정렬

    // 결재정보처리 (순서에 맞게)
    var seqline = Add.GetSeqLine(appSeqKey, appSeqDic);   // 발신결재
    var seqlinezq = Add.GetSeqLineZQZC(appSeqDicZQ);        // 수신결재
    var seqlinezc = Add.GetSeqLineZQZC(appSeqDicZC);        // 참조결재
    var seqlinezr = Add.GetSeqLineZR(appSeqDicZR);        // 공람

    ParamArray = {
        'formid': FormID
        , 'vals': seqline + seqlinezq + seqlinezc + seqlinezr
    };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SaveNetSuccess");

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Add.asmx/SetNetLine";
    AjaxService(url, data, obj);
}
Add.SaveNetSuccess = function (data, status) {
    if (data != null) {
        if (data.d) { alert(R_MsgApprovalNetSaveOK); }
    }
}
/**************************************************
작성목적 : [결재라인-ZD,ZA,ZH 등]
***************************************************/
Add.GetSeqLine = function (_objArr, _objHash) { //alert(_objArr); alert(_objHash);

    var rtnvals = "";
    if (_objArr.length > 0 && _objHash.size() > 0) {
        var i = 1;
        $.each(_objArr, function (i, o) {

            //  apptag | seq | userid | deptid | isdept | issub $ 
            var obj = _objHash.get(o);
            if (obj.approvalTag != "ZD0") {
                rtnvals += obj.approvalTag + "|";
                rtnvals += i + "|";
                rtnvals += obj.userID + "|";
                rtnvals += obj.deptID + "|";
                rtnvals += obj.isGroup + "|";
                rtnvals += obj.subTree + "$";
                i++;
            }
        });
    }
    return rtnvals;
}
/**************************************************
작성목적 : [결재라인-ZP,ZQ,ZC 등]
***************************************************/
Add.GetSeqLineZQZC = function (_objHash) {

    var rtnvals = "";
    if (_objHash.size() > 0) {

        var arrTagKey = Common.GetAppSeqQueue(_objHash);

        $.each(arrTagKey, function (i, o) {
            var obj = _objHash.get(o);

            if (obj.approvalTag.indexOf("ZQ") > -1 && $("#ZQ01_ZD1", ifrmObj).length > 0) {
                rtnvals += obj.approvalTag + "|";
            }
            else {
                rtnvals += obj.approvalTag.substr(0, 2) + "|";
            }
            rtnvals += "0|";
            rtnvals += obj.userID + "|";
            rtnvals += obj.deptID + "|";
            rtnvals += obj.isGroup + "|";
            rtnvals += obj.subTree + "$";
        });
    }
    return rtnvals;
}
/**************************************************
작성목적 : [결재라인-ZP,ZQ,ZC 등]
***************************************************/
Add.GetSeqLineZR = function (_objHash) {

    var rtnvals = "";
    if (_objHash.size() > 0) {

        var arrTagKey = Common.GetAppSeqQueue(_objHash);

        $.each(arrTagKey, function (i, o) {
            var obj = _objHash.get(o);

            rtnvals += obj.approvalTag + "|";
            rtnvals += "0|";
            rtnvals += obj.userID + "|";
            rtnvals += obj.deptID + "|";
            rtnvals += obj.isGroup + "|";
            rtnvals += obj.subTree + "$";
        });
    }
    return rtnvals;
}

/**************************************************
작성자   : 방영환
작성일자 : 2017-03-23
작성목적 : 양식별 저장된 결재라인 중 부서 변경된 결재라인정보 가져오기
***************************************************/
Add.GetNetModfySeq = function (_formid) {
    if (_formid == 0) return;
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Add.asmx/GetNetModfyLine";
    ParamArray = { 'formid': _formid };
    var obj = eval("Add.SuccessGetNetModfySeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

Add.SuccessGetNetModfySeq = function (data, status) {
    var msgModfyLine = "";
    var tmpVal = "";

    if (data != null) {
        $.each(data.d, function (i, o) {
            tmpVal += "\n - " + data.d[i].DeptBase + " " + data.d[i].TitleBase + " " + data.d[i].NameBase;
        });
    }

    if (tmpVal != "") {
        msgModfyLine = "[" + R_ModfyAppLine + "]" + tmpVal + "\n" + R_Msg_ConfirmAppLine;
        alert(msgModfyLine);
        /*
        [결재정보변경]
        - 기술본부 저승이 사원
        - 기술본부 도깨비 회장
        결재라인을 확인하세요.
        */
    }
}

/**************************************************
작성목적 : (결재망저장) 양식별 저장된 기 결재라인 가져오기 
***************************************************/
Add.GetNetSeq = function (_formid) {
    if (_formid == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Add.asmx/GetNetLine";
    ParamArray = { 'formid': _formid };
    var obj = eval("Add.SuccessGetNetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Add.SuccessGetNetSeq = function (data, status) {

    if (data != null) {
        $.each(data.d, function (i, o) {

            // 유동결재망처리
            if (($("#FLOWZD", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZD") == 0)) {
                var tmpZD = data.d[i].ApprovalTag.replace("ZD", "");
                addZDcnt = tmpZD - $("#FLOWZD", ifrmObj).children().length + 1;
                if (addZDcnt > 0) { Common.SetFlowInfo("FLOWZD:" + addZDcnt); }
            }
            if (($("#FLOWZA", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZA") == 0)) {
                var tmpZA = data.d[i].ApprovalTag.replace("ZA", "");
                addZAcnt = tmpZA - $("#FLOWZA", ifrmObj).children().length;
                if (addZAcnt > 0) { Common.SetFlowInfo("FLOWZA:" + addZAcnt); }
            }
            if (($("#FLOWZH", ifrmObj).length > 0) && (data.d[i].ApprovalTag.indexOf("ZH") == 0)) {
                var tmpZH = data.d[i].ApprovalTag.replace("ZH", "");
                addZHcnt = tmpZH - $("#FLOWZH", ifrmObj).children().length;
                if (addZHcnt > 0) { Common.SetFlowInfo("FLOWZH:" + addZHcnt); }
            }

            Common.SetApproval(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, "");
        });
    }

}

/**************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 수신 정보 가져오기
***************************************************/
Add.GetNetZQSeq = function (_formid) {
    if (_formid == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Add.asmx/GetNetZQZCLine";

    ParamArray = {
        'formid': _formid,
        'appointerKind': "ZQ"
    };

    var obj = eval("Add.SuccessGetNetSeq");

    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2016-03-31
작성목적 : 참조 정보 가져오기
***************************************************/
Add.GetNetZCSeq = function (_formid) {
    if (_formid == 0) return;
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Add.asmx/GetNetZQZCLine";

    ParamArray = {
        'formid': _formid,
        'appointerKind': "ZC"
    };
    var obj = eval("Add.SuccessGetNetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 문서관리 정보 가져오기
***************************************************/
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
}/**************************************************
작성자   : 
작성일자 : 
작성목적 : 신메뉴 화면에서 상태조회 이동 시 신메뉴 유지
***************************************************/
function GetApprovalStatusDefaultPage(defaultPage) {
    try {
        if (typeof ApprovalStatusMenuType != "undefined" && ApprovalStatusMenuType.toUpperCase() == "NEW") {
            return defaultPage.replace("Default.aspx", "DefaultAll.aspx");
        }
    } catch (e) { }
    return defaultPage;
}