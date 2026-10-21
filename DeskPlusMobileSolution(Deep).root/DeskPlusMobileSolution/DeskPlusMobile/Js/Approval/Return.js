Return = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-05
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    //결재전용 비밀번호 사용
    jQuery.each($("#divpwd"), function () { this.style.display = (IsApprovePwd == "N") ? "none" : ""; });

    setTimeout("Return.getData();", 400);
    Return.getContent();

    $('#btCancel').click(function (e) {
        Return.viewList();
    });

    $('#btnReturn').click(function (e) {
        Return.approve();
    });

    $('#before').click(function (e) {
        Return.goBefore();
    });

    $('#listTitle').click(function (e) {
        Return.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    Return.MaxLang();

});

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 글자 제한
***************************************************/
Return.MaxLang = function () {

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
작성일자 : 2011-07-05
작성목적 : 반려자 정보 가져오기 (웹서비스 호출)
***************************************************/
Return.getData = function () {
    LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetReject";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'GroupID': GroupID
    };

    var obj = eval("Return.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-09-26
작성목적 : 반려자 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Return.succeeded = function (data, status) {

    var reObj = data.d;

    if (reObj.length < 1) {
        Return.goError();
        return;
    }

    var tmpRe = "<table border='0' cellpadding='0' cellspacing='0'>";

    for (var i = 0; i < reObj.length; i++) {

        if (i == 0) {
            tmpRe += "<tr><td><input type='radio' id='rd" + i + "' name='rdReturn' class='rd' checked value='" + reObj[i].DocumentID + "-" + reObj[i].Sequence + "-" + reObj[i].ApprovalTag + "-" + reObj[i].UserID + "' /><label>";
            tmpRe += Return.getTagName(reObj[i].ApprovalTag) + reObj[i].DeptName + "/" + reObj[i].TitleName + "/" + reObj[i].UserName + "</label></td></tr>";

        }
        else {
            if (reObj[i].ApprovalTag.indexOf('ZA') == 0) {

                tmpRe += "<tr><td><input type='radio' id='rd" + i + "' name='rdReturn' class='rd' disabled value='" + reObj[i].DocumentID + "-" + reObj[i].Sequence + "-" + reObj[i].ApprovalTag + "-" + reObj[i].UserID + "'  /><label>";
                tmpRe += Return.getTagName(reObj[i].ApprovalTag) + reObj[i].DeptName + "/" + reObj[i].TitleName + "/" + reObj[i].UserName + "</label></td></tr>";
            }
            else {
                tmpRe += "<tr><td><input type='radio' id='rd" + i + "' name='rdReturn' class='rd' value='" + reObj[i].DocumentID + "-" + reObj[i].Sequence + "-" + reObj[i].ApprovalTag + "-" + reObj[i].UserID + "'  /><label>";
                tmpRe += Return.getTagName(reObj[i].ApprovalTag) + reObj[i].DeptName + "/" + reObj[i].TitleName + "/" + reObj[i].UserName + "</label></td></tr>";
            }
        }

    }

    tmpRe += "</table>";

    $("#rej").empty();
    $("#rej").append(tmpRe);

    LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-09-26
작성목적 : 결재태그 이름
***************************************************/
Return.getTagName = function (val) {

    if (val == "ZD0") {
        return "[" + R_TagDic_ZD0 + "]";
    }
    else {
        switch (val.substring(0, 2)) {
            case "ZD": return "[" + R_ZD + "]";
            case "ZH": return "[" + R_ZH + "]";
            case "ZA": return "[" + R_ZA + "]";
            default: return "[" + R_ZD + "]";
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-05
작성목적 : 문서 내용 가져오기 (웹서비스 호출)
***************************************************/
Return.getContent = function () {
    LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentHTML";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("Return.succeededContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-05
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Return.succeededContent = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsCont.length < 1) {
        Approve.goError();
        return;
    }

    var cnt = conObj.dsCont[0].ContentHTML;

    ifrmForm.document.open();
    ifrmForm.document.write(cnt);
    ifrmForm.document.close();

    setTimeout("Return.GetLegacy();", 100);

    LoadingShow(false);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-09-26
작성목적 : 연동모듈체크
***************************************************/
Return.GetLegacy = function () {
    if (CustomFilePath == "") return;

    ifrmProcessing.document.location.href = CustomFilePath + "?seq=" + Guids();
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-05
작성목적 : 목록 페이지 이동
***************************************************/
Return.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'TargetUID': TargetUID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-05
작성목적 : 취소 버튼 클릭시
***************************************************/
Return.viewList = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'DID': DocumentID,
            'ATag': ApprovalTag,
            'Seq': Sequence,
            'TargetUID': TargetUID,
            'TargetDID': TargetDID
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'DID': DocumentID,
            'Stat': Stat,
            'ATag': ApprovalTag,
            'AccTag': AccTag,
            'Num2': number2,
            'Seq': Sequence,
            'TargetUID': TargetUID,
            'TargetDID': TargetDID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Approval/View.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-09-26
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Return.modViewing = function (arg) {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': ApprovalTag,
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
작성일자 : 2012-09-26
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Return.modDocViewing = function (arg) {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': ApprovalTag,
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
작성일자 : 2012-09-26
작성목적 : 로그처리
***************************************************/
Return.setLog = function (LogGubun) {
    var response = false;

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'ATag': ApprovalTag,
        'Gubun': LogGubun,
        'DecisionTag': ApprovalType
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
작성일자 : 2012-09-26
작성목적 : 결재 처리 
***************************************************/
var targetInfo = "";
var isTargetZD = false;
Return.approve = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait); return;
    }

    var tmpContent = $("#memo").val();

    if (jQuery.trim(tmpContent) == "") {
        NoticeShow("!", R_InputOpinion); return;
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

    targetInfo = $(":input:radio[name=rdReturn]:checked").val();
    if (targetInfo == "") {
        NoticeShow("!", R_SelectReturnUser); return;
    }

    //targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID-[4]ApprovalType
    //반려대상부서가 발신(최초)부서인지 확인.DocumentID / GroupID (For,CustomScript)
    if (AccountTag == "ZD") {
        isTargetZD = true;
    } else {
        isTargetZD = (targetInfo.indexOf(GroupID + "-") == 0) ? true : false;
    }
    //alert("isTargetZD:" + isTargetZD);return;

    LoadingShow(true);

    var rtnValMyTurn = Return.getMyturn();
    if ((rtnValMyTurn == "ERROR") || (rtnValMyTurn == "N")) {
        Return.goError();
        return;
    }

    var rtnVal = Return.modViewing("Y");
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

    Return.approveR();
}

SubmitCustomCall = function (_approvalType) {

    CallComplete = true;
    Return.approveR();
}

/**************************************************
작성자   : famo23
작성일자 : 2012-09-26
작성목적 : 반려 처리
***************************************************/
Return.approveR = function () {

    var tmpOpinion = $("#memo").val();
    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    var targetInfo = $(":input:radio[name=rdReturn]:checked").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'AccTag': AccTag,
        'ATag': ApprovalTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'TargetInfo': targetInfo,
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Num2': number2,
        'GroupID': GroupID
    };


    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetReturn",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (Return.setLog(1)) {
                    var rtnVal = Return.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Return.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (Return.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    Return.SetBadge();
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "FILENO") {
                if (Return.setLog(5)) {
                    var rtnVal = Return.modDocViewing("N");
                    NoticeShow("!", R_SignNo);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Return.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (Return.setLog(3)) {
                    var rtnVal = Return.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Return.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (Return.setLog(4)) {
                var rtnVal = Return.modDocViewing("N");
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
작성일자 : 2011-07-19
작성목적 : 결재 순서 확인
***************************************************/
Return.getMyturn = function () {
    var response = "";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'ATag': ApprovalTag

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

/**************************************************
작성자   : famo23
작성일자 : 2011-07-19
작성목적 : 에러 발생시에 처리
***************************************************/
Return.goError = function () {
    action_popup.alert(R_ReLoadDoc);

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        location.href = "/DeskPlusMobile/Approval/Folder.aspx";
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-10-28
작성목적 : submit 버튼 처리
***************************************************/
Return.submitProcess = function () {

    Return.approve();
    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-03-06
작성목적 : badge 카운트 가져오기 (웹서비스 호출)
***************************************************/
Return.SetBadge = function () {

    if ((UUID != "") && (MobileType == "ios")) {
        var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBadgeNum";

        //파라미터값 배열에 담기
        ParamArray = {
            'guid1': '1'
        };

        var obj = eval("Return.succeededSetBadge");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {
        if (Alarm == "Y") {
            ParamArray = {
                'PageNo': PageNo
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
        }
        else {
            ParamArray = {
                'BPage': 'APP',
                'TargetUID': TargetUID,
                'Sv': SearchValue.securityText()
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-03-06
작성목적 : badge 카운트 가져오기 (웹서비스 값 받기)
***************************************************/
Return.succeededSetBadge = function (data, status) {

    var BadgeCnt = data.d;
    
    try {
        //window.webkit.messageHandlers.set_badge.postMessage(BadgeCnt);
        setBadgeIOS(parseInt(BadgeCnt));
    } catch (err) {
        //alert(err);
    }    

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': 'APP',
            'TargetUID': TargetUID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Approval/List.aspx?p=" + GetEncryptValue;
    }
}