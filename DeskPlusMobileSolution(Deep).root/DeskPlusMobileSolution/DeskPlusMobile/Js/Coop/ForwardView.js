ForwardView = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("ForwardView.readyLoad();", 400);

    $('#minus').click(function (e) {
        ForwardView.minus();
    });

    $('#plus').click(function (e) {
        ForwardView.plus();
    });

    $('#before').click(function (e) {
        ForwardView.goBefore();
    });

    $('#listTitle').click(function (e) {
        ForwardView.goBefore();
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 데이터 가져오기
***************************************************/
ForwardView.readyLoad = function () {

    ForwardView.getHearder();
    ForwardView.getTo();
    ForwardView.getCc();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 기본 정보값 가져오기 (웹서비스 호출)
***************************************************/
ForwardView.getHearder = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetForwardHearder";

    //파라미터값 배열에 담기
    ParamArray = {
        'ForwardListID': ForwardListID
    };

    var obj = eval("ForwardView.succeededHearder");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 기본 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
ForwardView.succeededHearder = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsInfo.length < 1) {
        ForwardView.goError();
        return;
    }


    $("#subject").html(conObj.dsInfo[0].Title);
    $("#etc").html(conObj.dsInfo[0].CoopNumber + "<font class='istf'> [" + ForwardView.getStatus(conObj.dsInfo[0].StatusLast) + "]</font>");
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
작성일자 : 2015-04-02
작성목적 : 수신자 정보값 가져오기 (웹서비스 호출)
***************************************************/
ForwardView.getTo = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetForwardTo";

    //파라미터값 배열에 담기
    ParamArray = {
        'ForwardListID': ForwardListID
    };

    var obj = eval("ForwardView.succeededTo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 수신자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
ForwardView.succeededTo = function (data, status) {

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
작성일자 : 2015-04-02
작성목적 : 참조자 정보값 가져오기 (웹서비스 호출)
***************************************************/
ForwardView.getCc = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetForwardCc";

    //파라미터값 배열에 담기
    ParamArray = {
        'ForwardListID': ForwardListID
    };

    var obj = eval("ForwardView.succeededCc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 참조자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
ForwardView.succeededCc = function (data, status) {

    var ccObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    $("#cc").empty();

    if (ccObj.dsCc.length > 0) {

        var tmpCc = "<table border='0' cellpadding='0' cellspacing='0'>";
        tmpCc += "<tr>";
        tmpCc += "<td width=37px><span class='istd'>" + R_Cc + "</span>&nbsp;</td>";
        tmpCc += "<td><span class='lidf'>";

        for (var j = 0; j < ccObj.dsCc.length; j++) {
            tmpCc += ccObj.dsCc[j].UserName + "(" + ccObj.dsCc[j].TitleName + "/" + ccObj.dsCc[j].DeptName + "),";
        }

        tmpCc += "</span></td>";
        tmpCc += "</tr>";
        tmpCc += "</table>";

        $("#cc").html(tmpCc);
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 상태값 가져오기 
***************************************************/
ForwardView.getStatus = function (val) {
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
작성일자 : 2015-04-02
작성목적 : 공개상태 가져오기 
***************************************************/
ForwardView.getIsClose = function (val) {
    switch (val) {
        case "1": return R_IsClose1;
        case "2": return R_IsClose2;
        default: return R_IsClose1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 이전 글 가기
***************************************************/
ForwardView.minus = function () {
    ForwardView.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 다음 글 가기
***************************************************/
ForwardView.plus = function () {
    ForwardView.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
ForwardView.getBeforeNext = function (gubun) {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetBeforeNextForward";

    //파라미터값 배열에 담기
    ParamArray = {
        'ForwardListID': ForwardListID,
        'Gubun': gubun,
        'Sv': SearchValue
    };

    var obj = eval("ForwardView.succeededBeforeNext");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 이전 , 다음 글 가기 (웹서비스 값 받기)
***************************************************/
ForwardView.succeededBeforeNext = function (data, status) {
    if (data.d != "0") {
        var tmpVal = data.d;
        if (tmpVal != "0") {
        
            ParamArray = {
                'BPage': BPage,
                'MoreSize': MoreSize,
                'PageNo': PageNo,
                'ForwardListID': tmpVal,
                'Sv': SearchValue
            };

            CallEncrypt(CallGetParam());
            document.location.href = "ForwardView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 목록 페이지 이동
***************************************************/
ForwardView.goBefore = function () {
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
작성일자 : 2015-04-02
작성목적 : 에러 발생시에 처리
***************************************************/
ForwardView.goError = function () {
    alert(R_DelCoop);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
ForwardView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': fileName.securityText()
    };

    var obj = eval("ForwardView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
ForwardView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        ForwardView.goError();
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

            location.href = "/DeskPlusMobile/Common/AttachForwardView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 전자결재근거문서 보기 화면으로 가기 
***************************************************/
ForwardView.appDocView = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID        
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/BaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 업무협업 근거문서 보기 화면으로 가기 
***************************************************/
ForwardView.CoopViewer = function (p_listID) {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': p_listID,
        'GroupID': p_listID,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseForwardView.aspx?p=" + GetEncryptValue;
}



