Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // [미리보기]
        $('#cbtnPreview').click(function (e) { e.preventDefault(); Add.Preview(); });
        // [문서상신]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); Add.Submit(); });
        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); Common.AttachSwfFile(); });
        // [근거문서-추가]
        $('#cbtnDocAdd').click(function (e) { e.preventDefault(); Common.AttachBaseDoc(); });
        // [임시저장]
        $('#cbtnSaveDraft').click(function (e) { e.preventDefault(); Add.SaveDraft(); });

        // [결재선추가]
        $('#cbtnFlowAdd').click(function (e) { e.preventDefault(); Add.FlowAdd(); });
        // [결재선삭제]
        $('#cbtnFlowDel').click(function (e) { e.preventDefault(); Add.FlowDel(); });

        //개발용(근거문서 추가시 문서번호 보이기)
        $('#tbAddDocID').val(BaseDocIDs);

        $('#cbtnEDM').click(function (e) { e.preventDefault(); Add.GetEDM(); });

        Add.Init();

        Common.GetSpoolFileList();
        Add.GetBaseDocListEdit(DocumentID);      // 근거문서 리스트
    });
});

Add.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {           //인장체크
        $("#ulSubmit").css("display", "none");
        alert(R_MSG_ERRSEAL); return;
    }

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

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("Add.CheckOld();", 100);     //수신부서 반려체크 후, 문서초기화

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
    Add.GetModfySeq(DocumentID);

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
}

/**************************************************
작성목적 : 수신부서 반려체크
***************************************************/
// 변경대상 초기화 (span & div & input) : 결재태그
Add.UpdIds = function (o, _oldtag) {

    var tmptag = "";
    var oldTag = _oldtag;
    $.each(o, function (i, o) {

        //var aaa = o.id;
        if ($("#" + o.id, ifrmObj).attr("btag")) {

            tmptag = $("#" + o.id, ifrmObj).attr("btag");       //ZQ01
            o.id = (o.id.indexOf("ZD0") > -1) ? o.id.replace(oldTag + "ZD0", tmptag) : o.id.replace(oldTag, tmptag + "_");
            //o.className = o.className.replace(oldTag, "");
            o.className = tmptag.substr(0, 2);

        } else {
            o.id = o.id.replace(oldTag, "");
            o.className = o.className.replace(oldTag, "");
        }
        //alert(aaa + "   :   " + o.id)

        nowEl = o;
        Common.DelSeq();
    });
}

Add.CheckOld = function () {

    // 수신
    if ($("#PARENTDOCUMENT_ZQ01", ifrmObj).length > 0) {
        ifrmObj.body.innerHTML = $("#PARENTDOCUMENT_ZQ01", ifrmObj).html();

        setTimeout(function () {
            var strOld = "";
            var zTags = $("span", ifrmObj);
            $.each(zTags, function (i, o) {
                if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                    strOld += o.id.split("_")[0] + ",";
                }
            });

            // 고정수신 (최근발신태그 선처리)
            var objs = $(".ZD,.formfieldinput", ifrmObj);
            Add.UpdIds(objs, "");

            // 고정수신 (이전수발신태그 처리 - 승인)
            var oldTag = "", ids = "";
            $.each(strOld.trimEnds(',').split(','), function (i, o) {

                oldTag = o + "_";
                ids = "." + oldTag + "ZD," + "." + oldTag + "ZP," + "." + oldTag + "ZQ," + "." + oldTag + "ZH," + "." + oldTag + "ZC," + "." + oldTag + "ZR," + "." + oldTag + "ZA," + "." + oldTag + "formfieldinput";
                objs = $(ids, ifrmObj);

                Add.UpdIds(objs, oldTag);
            });

            // 고정수신 (이전수발신태그 강제초기화 처리)
            $.each($(".ZQ01,.ZQ02,.ZQ03,.ZQ04,.ZQ05", ifrmObj), function (i, o) {
                $(o).attr("class", o.id.substr(0, 2));
            });


            var actObj = $("input:checkbox,select", ifrmObj);
            $.each(actObj, function (i, o) {
                if ($(o, ifrmObj).attr("disabled")) {
                    $(o, ifrmObj).removeAttr("disabled");
                }
                if ($(o, ifrmObj).attr("readonly")) {
                    $(o, ifrmObj).removeAttr("readonly");
                }
            });
        }, 100);
    }

    setTimeout("Add.InitForm();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 양식초기화
***************************************************/
Add.InitForm = function () {

    // 에디터 처리
    if ($("#EDITOR", ifrmObj).length > 0) {
        $("#htaEditorHTML").val($("#EDITOR", ifrmObj).html());
    }

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        if (o.id.indexOf("FLOW") < 0) {

            if ($("#" + o.id, ifrmObj).attr("idx")) {
                $("#" + o.id, ifrmObj).remove();
            }
            else {
                o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
                if ($("#" + o.id, ifrmObj).attr("isapp")) {
                    $("#" + o.id, ifrmObj).removeAttr("isapp")
                }
                if ($("#" + o.id, ifrmObj).attr("IsAsign")) {
                    $("#" + o.id, ifrmObj).removeAttr("IsAsign");   //20130807 부서변경된 경우, 예외처리
                }
            }
        }
    });

    // 문서정보 태그 초기화 (input/div)
    var arrInfo = $(".formfieldinput", ifrmObj);            //var arrInfoExcept = $("#SUBJ,#LIFE,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each(arrInfo, function (i, o) {
        //if (o.id == "SUBJ" || o.id == "LIFE") return;       //초기화 예외필드처리
        //if (o.tagName.toUpperCase() == "INPUT") { o.value = ""; }
        //if (o.tagName.toUpperCase() == "DIV") { o.innerHTML = ""; }

        if (o.id == "OPINION") { o.innerHTML = ""; }    // 첨언필드 처리 (이전 완료문서에 있는 첨언내용삭제)

        Common.ExceptObj(o);   // 특정필드 예외처리 (에디터 ..)
    });

    // 기본문서정보 설정 (양식관련 기본정보 처리 / 문서번호,보존년한,제목)
    Common.SetDefaultInfo();

    //DATE앞에 old제거
    var strOld = "";
    var zTags = $("input", ifrmForm.document);
    $.each(zTags, function (i, o) {
        if (o.id.indexOf("DATE") > 0) {
            strOld = o.id.split("_")[0];
            strOld = strOld + "_";
            o.id = o.id.replace(strOld, "");
            o.className = o.className.replace(strOld, "");
        }
    });    

    // 입력필드 활성화
    var actObj1 = $.grep(arrInfo, function (o, i) {
        return (o.id.indexOf("BODY") == 0);
    });
    var actObj2 = $("#SUBJ,#ENFORCEDEPTNAME,#ENFORCENAME,#ENFORCERECEIVECARBON", ifrmObj);
    $.each($.merge(actObj1, actObj2), function (i, o) {
        $(o).removeAttr("unselectable");
        $(o).removeAttr("readonly");
        $(o).attr("contenteditable", "true");
    });
    var actObj = $("input:checkbox,input:radio,select,textarea", ifrmObj);
    $.each(actObj, function (i, o) {
        if ($(o).attr("unselectable")) { $(o).removeAttr("unselectable"); }
        if ($(o).attr("readonly")) { $(o).removeAttr("readonly"); }
        if ($(o).attr("disabled")) { $(o).removeAttr("disabled"); }
        if ($(o).attr("contenteditable")) { $(o).attr("contenteditable", "true"); }
    });

    // 유동결재 초기화
    Common.SetDefault();
    Common.SetFlowInfoModCotent($("#tbIsUseFlow").val());

    // 이전 결재라인정보 (상신자 정보도, 해당 내용에 포함됨)
    Add.GetSeq(GroupID);

    if ($("#ZD0", ifrmObj).attr("IsAsign") == undefined) {
        // 상신자지정.//20130807 부서변경된 경우, 예외처리
        Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", true);
    }

    // 커스텀연동 호출
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
작성목적 : 부서정보가 변경된 이전결재라인 가져오기
***************************************************/
Add.GetModfySeq = function (_docID) {    //alert(_docID)
    if (_docID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Submit/Add.asmx/GetModfySeq";
    ParamArray = { 'docID': _docID };
    var obj = eval("Add.SuccessGetModfySeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Add.SuccessGetModfySeq = function (data, status) {

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
작성자   : 
작성일자 : 
작성목적 : 이전결재라인 가져오기
***************************************************/
Add.GetSeq = function (_docID) {    //alert(_docID)
    if (_docID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Submit/Add.asmx/GetSeq";
    ParamArray = { 'docID': _docID };
    var obj = eval("Add.SuccessGetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Add.SuccessGetSeq = function (data, status) {

    var tmpDeptID = "";
    var tmpUser = "N";

    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");

    if (data != null) {
        $.each(data.d, function (i, o) {
            if (data.d[i].ApprovalTag == "ZD0") {
                tmpDeptID = data.d[i].DeptID;
                tmpUser = "Y";
                // 상신자재지정.
                //Common.SetApprovalZD(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, "");
            }
            else {
                Common.SetApproval(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, "");
            }
        });

        if (tmpUser == "Y") {
            parent.frmLeft.Common.SetDeptInfo(tmpDeptID);

            Common.ModDeptInfo();
        }
    }
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

    //필수입력 - 보존년한
    var life = "";
    if ($("#LIFE", ifrmObj).length > 0) {
        life = $.trim($("#LIFE", ifrmObj).val());
        if (life == "") { alert(R_Msg_ErrLife); $("#LIFE", ifrmObj).focus(); return; }
    }
    life = (life == "") ? R_LIFE : life;  // 보존년한필드가 없으면 기본값으로 대체
    $("#tbLifeTime").val(life);

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
           // alert(R_Msg_ErrSetZH); return;
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
Add.SubmitAfterCall = function (_customPath, _docID, _msg) {

    if (_customPath == "") { Add.SubmitClose(_msg); }

    documentID = _docID;
    msg = _msg;

    // 커스텀연동 호출
    Common.GetLegacy(_customPath);
}
SubmitAfterCall = function () {
    Add.SubmitClose(msg);
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
작성목적 : [미리보기]
***************************************************/
Add.Preview = function () {

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
Add.List = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "/DeskPlusEIP/Approval/FormFlow/Submit/List.aspx?p=" + GetEncryptValue;

    parent.frmLeft.Common.SetMenu(4); //트리메뉴제어(조직도)
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 근거문서가져오기
***************************************************/
Add.GetBaseDocListEdit = function (_docID) {
    ParamArray = { 'documentID': _docID };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessBaseDocListEdit");
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/Common.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
Add.SuccessBaseDocListEdit = function (data, status) {

    if (data.d == null) return;

    var beforeIDs = "";
    $.each(data.d, function (i, o) {
        beforeIDs += "|" + data.d[i].ParentDocumentID + "^" + data.d[i].Name + ";";
        /*
        var contentHtml = "<tr id='" + data.d[i].ParentDocumentID + "' class='rows' name='" + data.d[i].Name + "'>"
        + "<td>&nbsp;</td>"
        + "<td>" + data.d[i].DocNumber + "</td>"
        + "<td>" + data.d[i].FormBase + "</td>"
        + "<td><a onclick=\"Common.View('" + data.d[i].ParentDocumentID + "')\" style='cursor:pointer' >" + data.d[i].Subject + "</a></td>"
        + "<td>" + data.d[i].TitleBase + "&nbsp;" + data.d[i].NameBase + "</td>"
        + "<td class='center'><a onclick=\"Common.View('" + data.d[i].ParentDocumentID + "')\">&nbsp;&nbsp;<img class='BtnView' alt=''></a></td>"
        + "<td class='center'><a onclick=\"Common.Del('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\">&nbsp;&nbsp;<img class='BtnDel' alt=''></a></td>"
        + "<td>&nbsp;&nbsp;<a onclick=\"Common.Up('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\"><img class='BtnUp' alt=''></a>&nbsp;&nbsp; &nbsp;&nbsp;"
        + "<a onclick=\"Common.Down('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\"><img class='BtnDown' alt=''></a></td>"
        + "</tr>";
        */
        var contentHtml = "<li id='" + data.d[i].ParentDocumentID + "' class='rows'>"
            + "<a title=" + DocNumber + " onclick=\"Common.View('" + data.d[i].ParentDocumentID + "')\">" + data.d[i].Subject + "</a>&nbsp;<a onclick=\"Common.Del('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\"><i class='fas fa-times-circle' title=" + R_FileDel + "></i></a>&nbsp;"
            + "<a onclick=\"Common.Up('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\"><i class='fas fa-caret-up' title='up'></i></a><a onclick=\"Common.Down('" + data.d[i].ParentDocumentID + "', '" + data.d[i].DocNumber + "')\"><i class='fas fa-caret-down' title='down'></i></a>"
            + "<p>"
            + "<span title=" + DocNumber + ">" + data.d[i].DocNumber + "</span><span title=" + R_FormName + ">" + data.d[i].FormBase + "</span><span title=" + R_ReportUser + ">" + data.d[i].TitleBase + "&nbsp;" + data.d[i].NameBase + "</span>"
            + "</p>"
        + "</li>";

        $("#noContent").remove();
        $("#contentTable").append(contentHtml);
    });
    BaseDocIDs = beforeIDs;
    BaseDocCnt = beforeIDs.split(";").length - 1;

    //문서보기, 삭제, 순서변경 아이콘 처리
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnDel") != undefined) { $(".BtnDel").attr('src', ImagePath + '/Icon/ico_tree_listDel.gif'); }
    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }
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
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : [임시저장]
***************************************************/
Add.SaveDraft = function () {

    if (FormID < 1) { alert(Msg_ErrSelForm); return; }

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