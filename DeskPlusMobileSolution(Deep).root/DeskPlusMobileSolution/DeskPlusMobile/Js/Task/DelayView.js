View = {}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
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

    $('#minus').click(function (e) {
        e.preventDefault();
        View.minus();
    });

    $('#plus').click(function (e) {
        e.preventDefault();
        View.plus();
    });

    $('#before').click(function (e) {
        View.goBefore();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }

});


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 버튼 제어 
***************************************************/
View.readyLoad = function () {

    View.getHearder();
    View.getTo();
    View.getCc();
    View.getHistory();
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 기본 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getHearder = function () {

    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetHearder";

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
작성일자 : 2018-12-19
작성목적 : 기본 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededHearder = function (data, status) {

    if (data.d == "") {
        View.goError();
        return;
    }

    var conObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    $("#subject").html(conObj.dsInfo[0].Title);
    $("#etc").html(conObj.dsInfo[0].TaskNumber + "<font class='istf'> [" + View.getStatus(conObj.dsInfo[0].StatusLast) + "]</font>");
    $("#item").html(conObj.dsInfo[0].Item);
    $("#itemsub").html(conObj.dsInfo[0].ItemSub);

    $("#requestDate").html(DateYMD(conObj.dsInfo[0].RequestDate, PreLang));
    $("#completeRequestDate").html(DateYMD(conObj.dsInfo[0].CompleteRequestDate, PreLang));
    $("#startDate").html(DateYMD(conObj.dsInfo[0].StartDate, PreLang));
    $("#endDate").html(DateYMD(conObj.dsInfo[0].EndDate, PreLang));

    $("#isClose").html(View.getIsClose(conObj.dsInfo[0].IsClose));
    $("#isGubun").html(View.getIsGubun(conObj.dsInfo[0].IsGubun));
    $("#source").html(conObj.dsInfo[0].SourceName);

    var tmpRe = "<table border='0' cellpadding='0' cellspacing='0'>";
    tmpRe += "<tr>";
    tmpRe += "<td width=45px><span class='istd'>" + R_Requester + "</span>&nbsp;</td>";
    tmpRe += "<td><span class='lidf'>" + conObj.dsInfo[0].DeptName + " " + conObj.dsInfo[0].TitleName + " " + conObj.dsInfo[0].UserName + "</span></td>";
    tmpRe += "</tr>";
    tmpRe += "</table>";

    $("#requester").html(tmpRe);

}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 수신자 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getTo = function () {

    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetTo";

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
작성일자 : 2018-12-19
작성목적 : 수신자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededTo = function (data, status) {

    var toObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var tmpTo = "<table border='0' cellpadding='0' cellspacing='0'>";
    tmpTo += "<tr>";
    tmpTo += "<td width=45px style='vertical-align:top;'><span class='istd'>" + R_To + "</span>&nbsp;</td>";
    tmpTo += "<td><span class='lidf'>";

    for (var i = 0; i < toObj.dsTo.length; i++) {
        tmpTo += toObj.dsTo[i].DeptName + " " + toObj.dsTo[i].TitleName + " " + toObj.dsTo[i].UserName + ",";
    }

    tmpTo = tmpTo.trimEnds(",");

    tmpTo += "</span></td>";
    tmpTo += "</tr>";
    tmpTo += "</table>";

    $("#to").html(tmpTo);

}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 참조자 정보값 가져오기 (웹서비스 호출)
***************************************************/
View.getCc = function () {

    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetCc";

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
작성일자 : 2018-12-19
작성목적 : 참조자 정보값 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededCc = function (data, status) {

    var ccObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    $("#cc").empty();

    if (ccObj.dsCc.length > 0) {

        var tmpCc = "<table border='0' cellpadding='0' cellspacing='0'>";
        tmpCc += "<tr>";
        tmpCc += "<td width=45px style='vertical-align:top;'><span class='istd'>" + R_Cc + "</span>&nbsp;</td>";
        tmpCc += "<td><span class='lidf'>";

        for (var j = 0; j < ccObj.dsCc.length; j++) {

            if (ccObj.dsCc[j].IsGroup == "M") {
                tmpCc += ccObj.dsCc[j].DeptName + " " + ccObj.dsCc[j].TitleName + " " + ccObj.dsCc[j].UserName + ",";
            }
            else {
                tmpCc += ccObj.dsCc[j].UserName + ",";
            }
        }

        tmpCc = tmpCc.trimEnds(",");

        tmpCc += "</span></td>";
        tmpCc += "</tr>";
        tmpCc += "</table>";

        $("#cc").html(tmpCc);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 변경이력 가져오기 (웹서비스 호출)
***************************************************/
View.getHistory = function () {

    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetHistoryCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'GroupID': GroupID
    };

    var obj = eval("View.succeededHistory");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 변경이력 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededHistory = function (data, status) {

    var tmpCount = data.d.split('^');

    var tmpStr = "";
    if (tmpCount[0] > 0) {
        tmpStr += R_ChangeTO + ",";
    }

    if (tmpCount[1] > 0) {
        tmpStr += R_ChangeCC + ",";
    }

    if (tmpCount[2] > 0) {
        tmpStr += R_ChangeDate;
    }

    tmpStr = tmpStr.trimEnds(",");

    $("#history").html(tmpStr);
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 상태값 가져오기 
***************************************************/
View.getStatus = function (val) {
    switch (val) {
        case "1": return R_CoopStatus1;
        case "2": return R_CoopStatus2;
        case "3": return R_CoopStatus3;
        case "4": return R_CoopStatus4;
        case "5": return R_TaskStatus5;
        case "6": return R_TaskStatus6;
        default: return R_CoopStatus1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 보안수준 가져오기 
***************************************************/
View.getIsClose = function (val) {
    switch (val) {
        case "1": return R_TIsClose1;
        case "2": return R_TIsClose2;
        default: return R_TIsClose1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 구분(협조업무/지시업무) 가져오기 
***************************************************/
View.getIsGubun = function (val) {
    switch (val) {
        case "1": return R_IsGubun1;
        case "2": return R_IsGubun2;
        default: return R_IsGubun1;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 이전 글 가기
***************************************************/
View.minus = function () {
    View.getBeforeNext('Top');
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 다음 글 가기
***************************************************/
View.plus = function () {
    View.getBeforeNext('Down');
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 이전 , 다음 글 가기 (웹서비스 호출)
***************************************************/
View.getBeforeNext = function (gubun) {

    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetBeforeNext";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': "DE",
        'ListID': ListID,
        'Gubun': gubun,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("View.succeededBeforeNext");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 이전 , 다음 글 가기 (웹서비스 값 받기)
***************************************************/
View.succeededBeforeNext = function (data, status) {

    var tmpVal = data.d;
    if (tmpVal != "0") {

        ParamArray = {
            'PageNo': PageNo,
            'ListID': tmpVal,
            'GroupID': tmpVal,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());
        location.href = "DelayView.aspx?p=" + GetEncryptValue;
    }
    else {
        action_popup.alert(R_NoList);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "DelayList.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 에러 발생시에 처리
***************************************************/
View.goError = function () {
    action_popup.alert(R_DelCoop);
    location.href = "Folder.aspx";
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goAttach = function (path, fileName) {

    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/CopyAttach";

    //파라미터값 배열에 담기
    ParamArray = {
        'Path': path,
        'FileName': encodeURIComponent(fileName)
    };

    var obj = eval("View.succeededGoAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
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
            if (MobileType == "ios") {

                ParamArray = {
                    'companyID': CompanyID,
                    'userID': UserID,
                    'attachName': encodeURIComponent(tmpAttachName)
                };

                CallEncrypt(CallGetParam());

                var url = HttpType + MobileDOMAIN + "/DeskPlusMobile/Common/AttachUrl.aspx?p=" + GetEncryptValue;
                //window.webkit.messageHandlers.open_safari.postMessage(url);
                openSafari(url);
            }
            else {
                location.href = "/DeskPlusMobile/FileData/" + CompanyID + "/users/" + UserID + "/Spool/Mobile/" + encodeURIComponent(tmpAttachName);
            }
        }
        else {
            ParamArray = {
                'AttachName': encodeURIComponent(tmpAttachName)
            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Common/AttachView.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 전자결재근거문서 보기 화면으로 가기 
***************************************************/
View.AppView = function (baseDID) {

    ParamArray = {
        'BaseDID': baseDID
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/BaseView.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 답변하기 등록 페이지 이동
***************************************************/
View.add = function () {

    ParamArray = {
        'BPage': "DE",
        'BType': BType,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "AddReply.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-19
작성목적 : 완료하기 등록 페이지 이동
***************************************************/
View.addComplete = function () {

    ParamArray = {
        'BPage': "DE",
        'BType': BType,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "AddComplete.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 첨부파일 읽기 (웹서비스 호출)
***************************************************/
View.goDS = function (fileID) {
    var url = "http://sp.deskplus.co.kr:9979/docviewer.melita";

    var form = document.createElement("form");
    var parm = new Array();
    var input = new Array();

    form.action = url;
    //    form.target = "_blank";
    form.method = "post";

    parm.push(['FILE_ID', fileID]);
    parm.push(['client', 'mobile_web']);

    for (var i = 0; i < parm.length; i++) {
        input[i] = document.createElement("input");
        input[i].setAttribute("type", "hidden");
        input[i].setAttribute('name', parm[i][0]);
        input[i].setAttribute("value", parm[i][1]);
        form.appendChild(input[i]);
    }

    document.body.appendChild(form);
    form.submit();
}