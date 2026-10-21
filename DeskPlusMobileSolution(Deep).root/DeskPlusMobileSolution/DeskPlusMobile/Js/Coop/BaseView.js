BaseView = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    setTimeout("BaseView.readyLoad();", 400);

    $('#listTitle').click(function (e) {
        BaseView.goBefore();
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 목록 페이지 이동
***************************************************/
BaseView.goBefore = function () {

    ParamArray = {
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 버튼 제어 
***************************************************/
BaseView.readyLoad = function () {

    BaseView.getHearder();
    BaseView.getTo();
    BaseView.getCc();
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 기본 정보값 가져오기 (웹서비스 호출)
***************************************************/
BaseView.getHearder = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetHearder";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("BaseView.succeededHearder");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 기본 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededHearder = function (data, status) {

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    if (conObj.dsInfo.length < 1) {
        BaseView.goError();
        return;
    }


    $("#subject").html(conObj.dsInfo[0].Title);
    $("#etc").html(conObj.dsInfo[0].CoopNumber + "<font class='istf'> [" + BaseView.getStatus(conObj.dsInfo[0].StatusLast) + "]</font>");
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
BaseView.getTo = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetTo";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("BaseView.succeededTo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 수신자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededTo = function (data, status) {

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
BaseView.getCc = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetCc";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID
    };

    var obj = eval("BaseView.succeededCc");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 참조자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededCc = function (data, status) {

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
작성일자 : 2015-03-31
작성목적 : 상태값 가져오기 
***************************************************/
BaseView.getStatus = function (val) {
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
BaseView.getIsClose = function (val) {
    switch (val) {
        case "1": return R_IsClose1;
        case "2": return R_IsClose2;
        default: return R_IsClose1;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 에러 발생시에 처리
***************************************************/
BaseView.goError = function () {
    alert(R_DelCoop);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
BaseView.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': fileName.securityText()
    };

    var obj = eval("BaseView.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
BaseView.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        BaseView.goError();
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

            location.href = "/DeskPlusMobile/Common/AttachBaseView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 전자결재근거문서 보기 화면으로 가기 
***************************************************/
BaseView.appDocView = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID        
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/BaseBaseView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-01
작성목적 : 업무협업 근거문서 보기 화면으로 가기 
***************************************************/
BaseView.CoopViewer = function (p_listID) {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': p_listID,
        'GroupID': p_listID,
        'Status': p_status,    
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "BaseView.aspx?p=" + GetEncryptValue;
}


