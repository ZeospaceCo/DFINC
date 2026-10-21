View = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("View.readyLoad();", 400);

    // 답변하기
    $('#btnReply').click(function (e) {
        View.add();
    });

    // 완료취소
    $('#btnCompCancel').click(function (e) {
        View.compCancel();
    });

    // 완료하기
    $('#btnComplete').click(function (e) {
        View.addComplete();
    });

    // 접수하기
    $('#btnAccept').click(function (e) {
        View.accept();
    });

    // 반려
    $('#btnReject').click(function (e) {
        View.reject();
    });

   
    // 삭제하기
    $('#btnDel').click(function (e) {
        View.del();
    });


    $('#minus').click(function (e) {
        View.minus();
    });

    $('#plus').click(function (e) {
        View.plus();
    });

    $('#before').click(function (e) {
        View.goBefore();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 버튼 제어 
***************************************************/
View.readyLoad = function () {

    if (BPage == "request") {
        if (Status == "4") {
            jQuery.each($("#btnCompCancel"), function () { this.style.display = ""; });
        }
    }
    else if (BPage == "receive") {
        if ((Status == "2") || (Status == "3")) {
            jQuery.each($("#btnComplete"), function () { this.style.display = ""; });
        }
        else if (Status == "1") {
            jQuery.each($("#btnAccept"), function () { this.style.display = ""; });
            jQuery.each($("#btnReject"), function () { this.style.display = ""; });    
        }
        else if (Status == "4") {
            jQuery.each($("#btnCompCancel"), function () { this.style.display = ""; });
        }
    }

    View.getHearder();
    View.getTo();
    View.getCc();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 기본 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getHearder = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetHearder";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("View.succeededHearder");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 기본 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededHearder = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsInfo.length < 1) {
        View.goError();
        return;
    }


    $("#subject").html(conObj.dsInfo[0].Title);
    $("#etc").html(conObj.dsInfo[0].CoopNumber + "<font class='istf'> [" + View.getStatus(conObj.dsInfo[0].StatusLast) + "]</font>");
    $("#item").html(conObj.dsInfo[0].Item);
    $("#itemsub").html(conObj.dsInfo[0].ItemSub);

    $("#requestDate").html(DateYMD(conObj.dsInfo[0].RequestDate, PreLang));
    $("#completeRequestDate").html(DateYMD(conObj.dsInfo[0].CompleteRequestDate, PreLang));
    $("#startDate").html(DateYMD(conObj.dsInfo[0].StartDate, PreLang));
    $("#endDate").html(DateYMD(conObj.dsInfo[0].EndDate, PreLang));

    var tmpRe = "<table border='0' cellpadding='0' cellspacing='0'>";
    tmpRe += "<tr>";
    tmpRe += "<td><span class='istd'>" + R_Requester + "</span>&nbsp;</td>";
    tmpRe += "<td><span class='lidf'>" + conObj.dsInfo[0].UserName + "(" + conObj.dsInfo[0].TitleName + "/" + conObj.dsInfo[0].DeptName + ")</span></td>";
    tmpRe += "</tr>";
    tmpRe += "</table>";

    $("#requester").html(tmpRe);

}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 수신자 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getTo = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetTo";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("View.succeededTo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 수신자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededTo = function (data, status) {

    var toObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpTo = "<table border='0' cellpadding='0' cellspacing='0'>";
    tmpTo += "<tr>";
    tmpTo += "<td width=37px><span class='istd'>" + R_To + "</span>&nbsp;</td>";
    tmpTo += "<td><span class='lidf'>";

    for (var i = 0; i < toObj.dsTo.length; i++) {
        tmpTo += toObj.dsTo[i].UserName + "(" + toObj.dsTo[i].TitleName + "/" + toObj.dsTo[i].DeptName + "),";        
    }

    tmpTo += "</span></td>";
    tmpTo += "</tr>";
    tmpTo += "</table>";

    $("#to").html(tmpTo);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 참조자 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getCc = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetCc";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("View.succeededCc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 참조자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededCc = function (data, status) {

    var ccObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    $("#cc").empty();

    if (ccObj.dsCc.length > 0) {

        var tmpCc = "<table border='0' cellpadding='0' cellspacing='0'>";
        tmpCc += "<tr>";
        tmpCc += "<td width=37px><span class='istd'>" + R_Cc + "</span>&nbsp;</td>";
        tmpCc += "<td><span class='lidf'>";

        for (var j = 0; j < ccObj.dsCc.length; j++) {
            tmpCc += ccObj.dsCc[j].UserName + ",";
            //tmpCc += ccObj.dsCc[j].UserName + "(" + ccObj.dsCc[j].TitleName + "/" + ccObj.dsCc[j].DeptName + "),";
        }

        tmpCc += "</span></td>";
        tmpCc += "</tr>";
        tmpCc += "</table>";

        $("#cc").html(tmpCc);
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 상태값 가져오기 
***************************************************/
View.getStatus = function (val) {
    switch (val) {
        case "1": return R_CoopStatus1;
        case "2": return R_CoopStatus2;
        case "3": return R_CoopStatus3;
        case "4": return R_CoopStatus4;
        case "5": return R_CoopStatus5;
        default: return R_CoopStatus1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 공개상태 가져오기 
***************************************************/
View.getIsClose = function (val) {
    switch (val) {
        case "1": return R_IsClose1;
        case "2": return R_IsClose2;
        default: return R_IsClose1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 이전 글 가기
***************************************************/
View.minus = function () {
    View.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 다음 글 가기
***************************************************/
View.plus = function () {
    View.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
View.getBeforeNext = function (gubun) {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetBeforeNext";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'ListID': ListID,
        'Gubun': gubun,
        'Sv': SearchValue
    };

    var obj = eval("View.succeededBeforeNext");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 이전 , 다음 글 가기 (웹서비스 값 받기)
***************************************************/
View.succeededBeforeNext = function (data, status) {
    if (data.d != "0") {
        var tmpVal = data.d;
        var arrVal = tmpVal.split("^");
        if (arrVal[0] != "0") {
        
            ParamArray = {
                'BPage': BPage,
                'MoreSize': MoreSize,
                'PageNo': PageNo,
                'ListID': arrVal[0],
                'GroupID': arrVal[1],
                'Status': arrVal[2],
                'Sv': SearchValue
            };

            CallEncrypt(CallGetParam());
            document.location.href = "View.aspx?p=" + GetEncryptValue;

            /*
            ListID = arrVal[0];
            GroupID = arrVal[1];
            Status = arrVal[2];
            View.readyLoad();
            */
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-03-31
작성목적 : 에러 발생시에 처리
***************************************************/
View.goError = function () {
    alert(R_DelCoop);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': fileName.securityText()
    };

    var obj = eval("View.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
View.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        View.goError();
        return;
    }
    else if (data.d == "FAIL") {
        NoticeShow("!", R_NoDownload);
        return;
    }
    else {
        var tmpAttachName = data.d;
        var lastidx = tmpAttachName.lastIndexOf(".");
        var tmpFiletype = tmpAttachName.substring(lastidx + 1, tmpAttachName.length);

        if (tmpFiletype.toLowerCase() != "mht") {
            location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + data.d;
        }
        else {
            ParamArray = {
                'AttachName': tmpAttachName
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 전자결재근거문서 보기 화면으로 가기 
***************************************************/
View.appDocView = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID        
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/BaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 업무협업 근거문서 보기 화면으로 가기 
***************************************************/
View.CoopViewer = function (p_listID) {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': p_listID,
        'GroupID': p_listID,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 답변하기 등록 페이지 이동
***************************************************/
View.add = function () {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 완료취소 (웹서비스 호출)
***************************************************/
View.compCancel = function () {

    if (window.confirm(R_MsgCompCancel)) {

        var url = "/DeskPlusMobile/Coop/Ws/SetCoop.asmx/CompCancel";

        ParamArray = {
            'ListID': ListID
        };

        var obj = eval("View.succeededCompCancel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 답변하기 등록 (웹서비스 값 받기)
***************************************************/
View.succeededCompCancel = function (data, status) {

    if (data.d) {
        ParamArray = {
            'BPage': BPage,
            'MoreSize': MoreSize,
            'PageNo': PageNo,
            'ListID': ListID,
            'GroupID': GroupID,
            'Status': "3",
            'Sv': SearchValue
        };

        CallEncrypt(CallGetParam());
        document.location.href = "View.aspx?p=" + GetEncryptValue;
    }
    else {
        NoticeShow("!", R_ProcessingError);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 완료하기 등록 페이지 이동
***************************************************/
View.addComplete = function () {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "AddComplete.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 접수하기 등록 페이지 이동
***************************************************/
View.accept = function () {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "AddAccept.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 반려하기 (웹서비스 호출)
***************************************************/
View.reject = function () {

    if (window.confirm(R_Reject)) {

        var url = "/DeskPlusMobile/Coop/Ws/SetCoop.asmx/Reject";

        ParamArray = {
            'ListID': ListID
        };

        var obj = eval("View.succeededReject");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 반려하기 등록 (웹서비스 값 받기)
***************************************************/
View.succeededReject = function (data, status) {

    if (data.d) {
        ParamArray = {
            'BPage': BPage,
            'MoreSize': MoreSize,
            'PageNo': PageNo,
            'ListID': ListID,
            'GroupID': GroupID,
            'Status': "5",
            'Sv': SearchValue
        };

        CallEncrypt(CallGetParam());
        document.location.href = "View.aspx?p=" + GetEncryptValue;
    }
    else {
        NoticeShow("!", R_ProcessingError);
    }
}

