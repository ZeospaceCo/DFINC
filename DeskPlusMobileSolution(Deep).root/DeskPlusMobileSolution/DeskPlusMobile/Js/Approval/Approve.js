Approve = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-02
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    switch (ApprovalType) {
        case "S":
            $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ApprovalS);
            jQuery.each($("#btnApproveS"), function () { this.style.display = ""; });
            break;
        case "A":
            $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ApprovalA);
            jQuery.each($("#btnApproveA"), function () { this.style.display = ""; });
            break;
        case "H":
            $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ApprovalH);
            jQuery.each($("#btnApproveH"), function () { this.style.display = ""; });
            break;
        case "HR":
            $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ApprovalHR);
            jQuery.each($("#btnApproveHR"), function () { this.style.display = ""; });
            break;
    }

    //결재전용 비밀번호 사용
    jQuery.each($("#divpwd"), function () { this.style.display = (IsApprovePwd == "N") ? "none" : ""; });

    Approve.getContent();

    $('#btCancel').click(function (e) {
        Approve.viewList();
    });

    $('#btnApproveA').click(function (e) {
        Approve.approve('A');
    });

    $('#btnApproveS').click(function (e) {
        Approve.approve('S');
    });

    $('#btnApproveH').click(function (e) {
        Approve.approve('H');
    });

    $('#btnApproveHR').click(function (e) {
        Approve.approve('HR');
    });

    $('#before').click(function (e) {
        e.preventDefault();
        Approve.goBefore();
    });

    $('#listTitle').click(function (e) {
        Approve.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기
    Approve.MaxLang();
});

/**************************************************
작성자   : famo23
작성일자 : 2012-07-12
작성목적 : 글자 제한
***************************************************/
Approve.MaxLang = function () {

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
작성일자 : 2011-07-04
작성목적 : 문서 내용 가져오기 (웹서비스 호출)
***************************************************/
Approve.getContent = function () {
    LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetContentHTML";

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': CDID
    };

    var obj = eval("Approve.succeededContent");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-04
작성목적 : 문서 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Approve.succeededContent = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsCont.length < 1) {
        Approve.goError();
        return;
    }

    var cnt = conObj.dsCont[0].ContentHTML;

    ifrmForm.document.open();
    ifrmForm.document.write(cnt);
    ifrmForm.document.close();

    setTimeout("Approve.GetLegacy();", 100);

    LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-08-31
작성목적 : 연동모듈체크
***************************************************/
Approve.GetLegacy = function () {
    if (CustomFilePath == "") return;

    ifrmProcessing.document.location.href = CustomFilePath + "?seq=" + Guids();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-02
작성목적 : 목록 페이지 이동
***************************************************/
Approve.goBefore = function () {

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
작성일자 : 2011-07-02
작성목적 : 취소 버튼 클릭시
***************************************************/
Approve.viewList = function () {

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
작성일자 : 2011-07-04
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Approve.modViewing = function (arg) {
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
작성일자 : 2012-09-11
작성목적 : 결재 진행중 체크 (웹서비스 호출 받기)
***************************************************/
Approve.modDocViewing = function (arg) {
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
작성일자 : 2011-07-04
작성목적 : 로그처리
***************************************************/
Approve.setLog = function (LogGubun) {
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
작성일자 : 2012-08-31
작성목적 : 결재 처리 
***************************************************/
Approve.approve = function (_approvalType) {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait); return;
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

    var rtnValMyTurn = Approve.getMyturn();

    if ((rtnValMyTurn == "ERROR") || (rtnValMyTurn == "N")) {
        Approve.goError();
        return;
    }

    var rtnVal = Approve.modViewing("Y");

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

    switch (_approvalType) {
        case "S":
            Approve.approveS();         //승인
            break;
        case "A":
            Approve.approveA();         //전결
            break;
        case "H":
            Approve.approveH();         //합의
            break;
        case "HR":
            Approve.approveH();         //합의불가 
            break;
    }
}


SubmitCustomCall = function (_approvalType) {

    CallComplete = true;
    BtnClickCheck = "Y";

    switch (_approvalType.toUpperCase()) {
        case "S":
            Approve.approveS();         //승인
            break;
        case "A":
            Approve.approveA();         //전결
            break;
        case "H":
            Approve.approveH();         //합의
            break;
        case "HR":
            Approve.approveH();         //합의불가 
            break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-09-03
작성목적 : 승인 처리
***************************************************/
Approve.approveS = function () {
    var tmpOpinion = $("#memo").val();
    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'AccTag': AccTag,
        'ATag': ApprovalTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Num2': number2,
        'GroupID': GroupID
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetApproveS",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (Approve.setLog(1)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (Approve.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    Approve.SetBadge();
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "FILENO") {
                if (Approve.setLog(5)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_SignNo);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (Approve.setLog(3)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (Approve.setLog(4)) {
                var rtnVal = Approve.modDocViewing("N");
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
작성일자 : 2012-09-17
작성목적 : 전결 
***************************************************/
Approve.approveA = function () {
    var tmpOpinion = $("#memo").val();
    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'AccTag': AccTag,
        'ATag': ApprovalTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Num2': number2,
        'GroupID': GroupID
    };


    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetApproveA",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (Approve.setLog(1)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (Approve.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    Approve.SetBadge();
                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "FILENO") {
                if (Approve.setLog(5)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_SignNo);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (Approve.setLog(3)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (Approve.setLog(4)) {
                var rtnVal = Approve.modDocViewing("N");
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
작성일자 : 2012-09-20
작성목적 : 합의, 합의 불가 처리
***************************************************/
Approve.approveH = function () {
    var tmpOpinion = $("#memo").val();
    tmpOpinion = tmpOpinion.split("\n").join("<br>");

    //파라미터값 배열에 담기
    ParamArray = {
        'DID': DocumentID,
        'Seq': Sequence,
        'AccTag': AccTag,
        'ATag': ApprovalTag,
        'Opinion': encodeURIComponent(tmpOpinion),
        'TargetUID': TargetUID,
        'TargetDID': TargetDID,
        'Num2': number2,
        'GroupID': GroupID,
        'AppGun': ApprovalType
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Approval/Ws/SetApproval.asmx/SetApproveH",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            LoadingShow(false);
            if (msg.d == "!NULL") {
                if (Approve.setLog(1)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "OK") {
                if (Approve.setLog(2)) {
                    NoticeShow("*", R_Complete);

                    Approve.SetBadge();

                }
                else {
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else if (msg.d == "FILENO") {
                if (Approve.setLog(5)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_SignNo);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            } else {
                if (Approve.setLog(3)) {
                    var rtnVal = Approve.modDocViewing("N");
                    NoticeShow("!", R_ProcessingError);
                    BtnClickCheck = "";
                    return;
                }
                else {
                    var rtnVal = Approve.modDocViewing("N");
                    BtnClickCheck = "";
                    NoticeShow("!", R_LogError);
                    return;
                }
            }
        },
        error: function (msg) {
            if (Approve.setLog(4)) {
                var rtnVal = Approve.modDocViewing("N");
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
Approve.getMyturn = function () {
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
Approve.goError = function () {

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
Approve.submitProcess = function () {
    switch (ApprovalType) {
        case "S":
            //결재 승인
            Approve.approve('S');
            return false;
            break;
        case "A":
            //결재 전결
            Approve.approve('A');
            return false;
            break;
        case "H":
            //결재 합의
            Approve.approve('H');
            return false;
            break;
        case "HR":
            //결재 합의불가
            Approve.approve('HR');
            return false;
            break;
        case "O":
            //첨언 추가
            Approve.append();
            return false;
            break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-03-06
작성목적 : badge 카운트 가져오기 (웹서비스 호출)
***************************************************/
Approve.SetBadge = function () {

    if ((UUID != "") && (MobileType == "ios")) {
        var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetBadgeNum";

        //파라미터값 배열에 담기
        ParamArray = {
            'guid1': '1'
        };

        var obj = eval("Approve.succeededSetBadge");
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
Approve.succeededSetBadge = function (data, status) {

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