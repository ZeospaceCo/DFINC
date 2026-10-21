Mod = {}

/**************************************************
작성목적 : 초기화 
***************************************************/
$(function () {
    $(document).ready(function () {

        // [인쇄]
        $('#cbtnPreview').click(function (e) { e.preventDefault(); Mod.Preview(); });
        // [결재순서변경]
        $('#cbtnModSeq').click(function (e) { e.preventDefault(); Common.SeqOrder(true, true); });
        // [문서상신]
        $('#cbtnSubmit').click(function (e) { e.preventDefault(); Mod.Submit(); });
        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); Common.AttachSwfFile(); });
        // [근거문서-추가]
        $('#cbtnDocAdd').click(function (e) { e.preventDefault(); Common.AttachBaseDoc(); });

        //개발용(근거문서 추가시 문서번호 보이기)
        $('#tbAddDocID').val(BaseDocIDs);

        $('#cbtnEDM').click(function (e) { e.preventDefault(); Mod.GetEDM(); });

        Mod.SetViewing("Y");    // 결재페이지 처리.

        Mod.Init();
        Common.GetSpoolFileList();
        Common.GetAttachFileListEdit(DocumentID);   // 첨부파일 리스트
        Common.GetBaseDocListEdit(DocumentID);      // 근거문서 리스트

        //크롬에서 unload Viewing 처리
        if (navigator.sendBeacon) {
            window.addEventListener("beforeunload", function (e) {
                
                function sleep(delay) {  
                    const start = new Date().getTime();    
                    while (new Date().getTime() < start + delay);
                }

                ParamArray = { 'documentID': DocumentID, 'sequence': '1', 'approvalTag': ApprovalTag, 'viewing': 'N' };
                CallEncrypt(CallGetParam())

                navigator.sendBeacon('/DeskPlusEIP/Approval/Process/Basic/SetViewing.aspx?p=' + GetEncryptValue, "");

                sleep(100);
            });
        }
    });
});
/*
// unload체크
$(window).unload(function () {
    Mod.SetViewing("N");
});
*/

// unload체크 
$(window).on('beforeunload', function () {
    ParamArray = { 'documentID': DocumentID, 'sequence': '1', 'approvalTag': ApprovalTag, 'viewing': 'N' };
    CallEncrypt(CallGetParam())

    var obj = eval("Mod.SuccessSetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Mod.asmx/SetViewing";

    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("get", url + "?p=" + GetEncryptValue, true);
    xmlHttp.send(null);

}); 
/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 병렬합의 업데이트
**********************************************************************************************/
Mod.SetViewing = function (_viewing) { //alert(_viewing)
    ParamArray = { 'documentID': DocumentID, 'sequence': '1', 'approvalTag': 'ZD0', 'viewing': _viewing };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Basic/Mod.asmx/SetViewing";
    AjaxService(url, data, obj);
}
Mod.SuccessSetViewing = function (data, state) {
}
Mod.Init = function () {

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

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("Mod.CheckOld();", 100);

    // 이벤트 지정 (For.연동파일)
    $("body", ifrmObj).click(function (e) { Common.ifrmForm_click(e, false); });
    $("body", ifrmObj).keydown(function (e) { Common.ifrmForm_keydown(e, false); });
    $("body", ifrmObj).keyup(function (e) { Common.ifrmForm_keyup(e, false); });
    $("body", ifrmObj).change(function (e) { Common.ifrmForm_change(e, false); });

    // 자주지정하는 결재자 정보
    Common.GetFavoriteApp();

    // 양식결재라인 정보
    Common.GetFormSeq(FormID);

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
작성자   : 
작성일자 : 
작성목적 : 수신부서 반려체크
***************************************************/
Mod.CheckOld = function () {

    // 수신
    if (AccountTag.indexOf("ZQ") > -1) {
        var acctNo = Number(AccountTag.replace("ZQ", ""));
        var topAcctNo = acctNo + 1;
        if (acctNo < 10) acctNo = "0" + acctNo;
        if (topAcctNo < 10) topAcctNo = "0" + topAcctNo;

        // 상위 결재망을 지우고, AccountTag에 해당하는 old만 제거한다.
        if ($("#PARENTDOCUMENT_ZQ" + topAcctNo, ifrmObj).length > 0) {
            ifrmObj.body.innerHTML = $("#PARENTDOCUMENT_ZQ" + topAcctNo, ifrmObj).html();

            setTimeout(function () {
                var strOldZQDic = new jQuery.Hashtable();
                var zTagsZQ = $("span", $("#PARENTDOCUMENT_ZQ" + acctNo, ifrmObj).html());
                $.each(zTagsZQ, function (i, o) {
                    if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                        strOldZQDic.add(o.id.split("_")[0], o.id.split("_")[0]);
                    }
                });

                // old제거
                var strOld = "";
                var zTags = $("span", ifrmObj);
                $.each(zTags, function (i, o) {
                    if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                        if (!strOldZQDic.contains(o.id.split("_")[0])) {
                            strOld += o.id.split("_")[0] + ",";
                        }
                    }
                });

                $.each(strOld.trimEnds(',').split(','), function (i, o) {
                    var oldTag = o + "_";

                    // 변경대상 초기화 (span & div & input) : 결재태그
                    var tobj = "." + oldTag + "ZD," + "." + oldTag + "ZP," + "." + oldTag + "ZQ," + "." + oldTag + "ZH," + "." + oldTag + "ZC," + "." + oldTag + "ZR," + "." + oldTag + "ZA," + "." + oldTag + "formfieldinput";
                    var arrTag = $(tobj, ifrmObj);
                    $.each(arrTag, function (i, o) {
                        o.id = o.id.replace(oldTag, "");
                        o.className = o.className.replace(oldTag, "");
                    });
                });
            }, 100);

        }
    }

    setTimeout("Mod.InitForm();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 양식초기화
***************************************************/
Mod.InitForm = function () {

    // 에디터 처리
    if ($("#EDITOR", ifrmObj).length > 0) {
        $("#htaEditorHTML").val($("#EDITOR", ifrmObj).html());
    }

    // 결재망 초기화 (span & div) = 고정(ZD1,ZA1,ZH1 ..) + 유동(FLOWZD,FLOWZH,FLOWZA)
    var arrStamp = $(".ZD, .ZP, .ZQ, .ZH, .ZC, .ZR, .ZA", ifrmObj);
    $.each(arrStamp, function (i, o) {
        o.innerHTML = (o.id.toUpperCase().indexOf("_INFO") > 0) ? "&nbsp;" : tagDic.get(o.id);
        if ($("#" + o.id, ifrmObj).attr("isapp")) {
            $("#" + o.id, ifrmObj).removeAttr("isapp")
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
    Common.SetFlowInfo($("#tbIsUseFlow").val());

    // 이전 결재라인정보 (상신자 정보도, 해당 내용에 포함됨)
    Mod.GetSeq(DocumentID);

    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [문서상신]
***************************************************/
Mod.Submit = function () {

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

    //보존년한 - 기본값:원본문서
    if ($("#LIFE", ifrmObj).length > 0) {
        var life = $.trim($("#LIFE", ifrmObj).val());
        if (life == "") { alert(R_Msg_ErrLife); $("#LIFE", ifrmObj).focus(); return; }
        $("#tbLifeTime").val(life);
    }

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
        parent.frmLeft.Common.SetMenu(2); //트리메뉴제어(결재의견)
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
작성목적 : 문서상신 후(성공/실패), 페이지 이동 (서버호출)
***************************************************/
Mod.SubmitClose = function (_msg) {
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
Mod.Preview = function () {

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
작성목적 : 목록으로 가기 
***************************************************/
Mod.List = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    parent.location.href = GetApprovalStatusDefaultPage("/DeskPlusEIP/Approval/Status/Default.aspx") + "?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 이전결재라인 가져오기
***************************************************/
Mod.GetSeq = function (_docID) {
    if (_docID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/Common.asmx/GetSeq";
    ParamArray = { 'docID': _docID };
    var obj = eval("Mod.SuccessGetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Mod.SuccessGetSeq = function (data, status) {
    // 상신자지정.
    Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");

    if (data != null) {
        $.each(data.d, function (i, o) {
            if (data.d[i].ApprovalTag == "ZD0") {
                // 상신자재지정.
                Common.SetApprovalZD(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, "");
            }
            else {
                Common.SetApproval(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, "");
            }
        });
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 문서관리 정보 가져오기
***************************************************/
Mod.GetEDM = function () {
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
