Afterward = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    //결재전용 비밀번호 사용
    jQuery.each($("#divpwd"), function () { this.style.display = (IsApprovePwd == "N") ? "none" : ""; });

    if (NextTag.indexOf("ZA") > -1) {
        jQuery.each($("#Atarget"), function () { this.style.display = ""; });
        Afterward.getData();
    }

    Afterward.getContent();

    $('#btCancel').click(function (e) {
        Afterward.viewList();
    });

    $('#btnLaterOn').click(function (e) {
        Afterward.approve();
    });

    $('#before').click(function (e) {
        Afterward.goBefore();
    });

    $('#listTitle').click(function (e) {
        Afterward.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    Afterward.MaxLang();

});

/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 글자 제한
***************************************************/
Afterward.MaxLang = function () {

    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 후열대상자 정보 가져오기 (웹서비스 호출)
***************************************************/
Afterward.getData = function () {
    LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetAtferTarget";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'NextTag': NextTag,
        'NextSeq': NextSeq
    };

    var obj = eval("Afterward.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 후열대상자 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Afterward.succeeded = function (data, status) {

    var reObj = data.d;

    if (reObj.length < 1) {
        Afterward.goError();
        return;
    }

    var tmpRe = "<table border='0' cellpadding='0' cellspacing='0'>";

    for (var i = 0; i < reObj.length; i++) {

        if (i == 0) {
            tmpRe += "<tr><td><input type='radio' id='rd" + i + "' name='rdAfter' class='rd' checked value='" + reObj[i].ApprovalTag + "' /><label>";
            tmpRe += "[" + R_ZA + "]" + reObj[i].DeptName + "/" + reObj[i].TitleName + "/" + reObj[i].UserName + "</label></td></tr>";
        }
        else {
            tmpRe += "<tr><td><input type='radio' id='rd" + i + "' name='rdAfter' class='rd' value='" + reObj[i].ApprovalTag + "'  /><label>";
            tmpRe += "[" + R_ZA + "]" + reObj[i].DeptName + "/" + reObj[i].TitleName + "/" + reObj[i].UserName + "</label></td></tr>";
        }
    }

    tmpRe += "</table>";

    $("#aft").empty();
    $("#aft").append(tmpRe);

    LoadingShow(false);
}



/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 문서 내용 가져오기 (웹서비스 호출)
***************************************************/
Afterward.getContent = function () {
    LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentHTML";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID
    };

    var obj = eval("Afterward.succeededContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Afterward.succeededContent = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsCont.length < 1) {
        Afterward.goError();
        return;
    }

    var cnt = conObj.dsCont[0].ContentHTML;

    ifrmForm.document.open();
    ifrmForm.document.write(cnt);
    ifrmForm.document.close();

    setTimeout("Afterward.GetLegacy();", 100);

    LoadingShow(false);
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 연동모듈체크
***************************************************/
Afterward.GetLegacy = function () {
    if (CustomFilePath == "") return;

    ifrmProcessing.document.location.href = CustomFilePath + "?seq=" + Guids();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 목록 페이지 이동
***************************************************/
Afterward.goBefore = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'TargetUID': TargetUID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 취소 버튼 클릭시
***************************************************/
Afterward.viewList = function () {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': DocumentID,
        'Stat': Stat,
        'ATag': ATag,
        'AccTag': AccTag,
        'Num2': Num2,
        'Seq': Sequence,
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/View.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Afterward.modViewing = function (arg) {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': ATag,
        'Viewing': arg,
        'TargetUID': TargetUID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModViewing",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = msg.d;
        },
        error: function (msg) {
            response = "ERROR";
        }
    });

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Afterward.modDocViewing = function (arg) {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': ATag,
        'Viewing': arg
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/ModDocViewing",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = msg.d;
        },
        error: function (msg) {
            response = "ERROR";
        }
    });

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 로그처리
***************************************************/
Afterward.setLog = function (LogGubun) {
    var response = false;

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'ATag': ATag,
        'Gubun': LogGubun,
        'DecisionTag': 'Afterward'
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetLog",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = true;
        },
        error: function (msg) {
            response = false;
        }
    });

    return response;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 후열 처리 
***************************************************/
var msgReason = "";
Afterward.approve = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait); return;
    }

    // 후열사유 체크
    if ($("#ddlReason").val() == "") {
        NoticeShow("!", R_Msg_LaterOnSelItem); return;
    } else {
        msgReason = $("#ddlReason").val() + "<br>[" + R_ReadLaterOn + "]";
    }

    ///// 비밀번호 확인 시작 (N,Y)
    if (IsApprovePwd == "Y") {

        var tmpPass = $("#pass").val();
        var tmpUPwd = "";

        if (tmpPass == "") {
            NoticeShow("!", R_InputPwd); return;
        }

        ParamArray = {
            'Pwd': encodeURIComponent(tmpPass)
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";

        $.ajax({
            type: "POST",
            url: "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetPwd",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                tmpUPwd = msg.d;
            },
            error: function (msg) {
                tmpUPwd = msg.d;
            }
        });

        if (tmpUPwd != UPwd) {
            NoticeShow("!", R_NoPwd); return;
        }
    }
    ///// 비밀번호 확인 끝

    LoadingShow(true);

    var rtnValMyTurn = Afterward.getMyturn();

    if ((rtnValMyTurn == "ERROR") || (rtnValMyTurn == "N")) {
        Afterward.goError();
        return;
    }

    var rtnVal = Afterward.modViewing("Y");

    if (rtnVal == "ERROR") {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
        return;
    }
    else {
        if (rtnVal != "OK") {
            NoticeShow("!", rtnVal + R_ApprovalWait);
            LoadingShow(false);
            return;
        }
    }

    BtnClickCheck = "Y";

    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    Afterward.approveAfter();
}

SubmitCustomCall = function (_approvalType) {

    CallComplete = true;
    Afterward.approveAfter();
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 후열 처리 
***************************************************/
Afterward.approveAfter = function () {

    if (NextTag.indexOf("ZA") > -1) {
        NextTag = $(":input:radio[name=rdAfter]:checked").val();
    }

    var tmpOpinion = $("#memo").val();
    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'AccTag': AccTag,
        'ATag': ATag,
        'GroupID': GroupID,
        'NextSeq': NextSeq,
        'NextTag': NextTag,
        'OrginUserID': OrginUserID,
        'OrginDeptID': OrginDeptID,
        'MsgReason': msgReason,
        'Opinion': encodeURIComponent(tmpOpinion)
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetAfterward",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (Afterward.setLog(1)) {
                    var rtnVal = Afterward.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Afterward.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (Afterward.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    ParamArray = {
                        'BPage': 'REP',
                        'Sv': SearchValue.securityText()
                    };

                    CallEncrypt(CallGetParam());

                    location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (Afterward.setLog(3)) {
                    var rtnVal = Afterward.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Afterward.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (Afterward.setLog(4)) {
                var rtnVal = Afterward.modDocViewing("N");
                BtnClickCheck = "";
                NoticeShow("!", R_ProcessingError);
                LoadingShow(false);
                return;
            }
        }
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-07
작성목적 : 에러 발생시에 처리
***************************************************/
Afterward.goError = function () {
    action_popup.alert(R_ReLoadDoc);

    ParamArray = {
        'BPage': 'REP',
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-09
작성목적 : submit 버튼 처리
***************************************************/
Afterward.submitProcess = function () {

    Afterward.approve();
    return false;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-08-09
작성목적 : 결재 순서 확인
***************************************************/
Afterward.getMyturn = function () {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': NextTag

    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetMyturn",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            response = msg.d;
        },
        error: function (msg) {
            response = "ERROR";
        }
    });

    return response;
}