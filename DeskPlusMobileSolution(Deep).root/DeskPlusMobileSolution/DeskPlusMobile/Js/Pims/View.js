View = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-17
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    //LoadingShow(true);

    if (ScheduleID != 0) {
        setTimeout("View.getSchedule();", 400);
        //setTimeout("View.getAttach();", 400);
        setTimeout("View.getDS();", 400);
        setTimeout("View.getScheduleReply();", 400);
    }
    else {
        setTimeout("View.getToDo();", 400);
        setTimeout("View.getToDoReply();", 400);
    }

    if (OpenID != 0) {
        jQuery.each($("#btDel"), function () { this.style.display = "none"; });
        jQuery.each($("#btMod"), function () { this.style.display = "none"; });
    }
    else {
        if (RealUserID != UserID) {
            jQuery.each($("#btDel"), function () { this.style.display = "none"; });
            jQuery.each($("#btMod"), function () { this.style.display = "none"; });
        }
        else {
            jQuery.each($("#btDel"), function () { this.style.display = ""; });
            jQuery.each($("#btMod"), function () { this.style.display = ""; });
        }
    }

    //LoadingShow(false);


    $('#btDel').click(function (e) {
        View.del();
    });

    $('#btMod').click(function (e) {
        View.mod();
    });

    $('#btAdd').click(function (e) {
        View.add();
    })

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
작성일자 : 2011-06-17
작성목적 : 일정 가져오기 (웹서비스 호출)
***************************************************/
View.getSchedule = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetScheduleContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID,
        'DisplayID': DisplayID
    };

    var obj = eval("View.succeededSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-17
작성목적 : 일정 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededSchedule = function (data, status) {
    var schCont = eval('(' + data.d + ')');

    var tmpOpenType = R_Share + "일정";
    if (schCont.GetContent[0].OpenType == "0") {
        //개인
        tmpOpenType = R_Private + "일정";
    }
    else if (schCont.GetContent[0].OpenType == "1") {
        //공유
        tmpOpenType = R_Share + "일정";
    }
    else if (schCont.GetContent[0].OpenType == "2") {
        //전체
        tmpOpenType = R_All + "일정";
    }
    else if (schCont.GetContent[0].OpenType == "3") {
        //부서
        tmpOpenType = R_Dept + "일정";
    }

    $("#title").append(schCont.GetContent[0].Title);
    $("#date").append(schCont.GetContent[0].StartShortTime + " ~ " + schCont.GetContent[0].EndShortTime);
    $("#typeEtc").append(tmpOpenType + "&nbsp;&nbsp;" + schCont.GetContent[0].Status);
    $("#content").append(schCont.GetContent[0].bodyNoStyle);

    if (schCont.GetContent[0].Repeat == "Y") {
        jQuery.each($("#btMod"), function () { this.style.display = "none"; });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 호출)
***************************************************/
View.getToDo = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetToDoContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'ToDoID': ToDoID
    };

    var obj = eval("View.succeededToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededToDo = function (data, status) {
    var toDoCont = eval('(' + data.d + ')');

    var tmpStatus = R_Status1;
    if (toDoCont.GetToDoContent[0].Status == "W") {
        tmpStatus = R_Status1;
    }
    else if (toDoCont.GetToDoContent[0].Status == "I") {
        tmpStatus = R_Status2;
    }
    else if (toDoCont.GetToDoContent[0].Status == "E") {
        tmpStatus = R_Status3;
    }
    else if (toDoCont.GetToDoContent[0].Status == "C") {
        tmpStatus = R_Status4;
    }
    else if (toDoCont.GetToDoContent[0].Status == "T") {
        tmpStatus = R_Status5;
    }
    else if (toDoCont.GetToDoContent[0].Status == "D") {
        tmpStatus = R_Status6;
    }

    var tmpPriority = R_Priority1;
    if (toDoCont.GetToDoContent[0].Priority == "1") {
        tmpPriority = R_Priority1;
    }
    else if (toDoCont.GetToDoContent[0].Priority == "2") {
        tmpPriority = R_Priority2;
    }
    else if (toDoCont.GetToDoContent[0].Priority == "3") {
        tmpPriority = R_Priority3;
    }

    var tmpOpenType = R_Share + "할일";
    if (toDoCont.GetToDoContent[0].OpenType == "0") {
        //개인
        tmpOpenType = R_Private + "할일";
    }
    else if (toDoCont.GetToDoContent[0].OpenType == "1") {
        //공유
        tmpOpenType = R_Share + "할일";
    }
    else if (toDoCont.GetToDoContent[0].OpenType == "2") {
        //전체
        tmpOpenType = R_All + "할일";
    }
    else if (toDoCont.GetToDoContent[0].OpenType == "3") {
        //부서
        tmpOpenType = R_Dept + "할일";
    }

    $("#date").append(R_Day + " : " + toDoCont.GetToDoContent[0].StartShortTime);
    $("#title").append(toDoCont.GetToDoContent[0].Title);
    $("#typeEtc").append(tmpOpenType + "&nbsp;&nbsp;" + "<span class='bg-success text-white'>&nbsp;" + tmpStatus + "&nbsp;</span>&nbsp;&nbsp;" + tmpPriority);
    $("#content").append(toDoCont.GetToDoContent[0].bodyNoStyle);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 일정, 할일삭제하기
***************************************************/
View.del = function () {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {
            if (ScheduleID != 0) {
                View.delschedule();
            }
            else {
                View.delToDo();
            }
        }
    })
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 일정 삭제하기 (웹서비스 호출)
***************************************************/
View.delschedule = function () {
    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/DelSchedule";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID,
        'DisplayID': DisplayID
    };

    var obj = eval("View.succeededDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 삭제하기 (웹서비스 호출)
***************************************************/
View.delToDo = function () {
    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/DelToDo";

    //파라미터값 배열에 담기
    ParamArray = {
        'ToDoID': ToDoID
    };

    var obj = eval("View.succeededDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 일정 삭제하기 (웹서비스 값 받기)
***************************************************/
View.succeededDel = function (data, status) {
    View.goBefore();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 이전 페이지 이동
***************************************************/
View.goBefore = function () {
    ParamArray = {
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': OpenDate
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-28
작성목적 : 수정
***************************************************/
View.mod = function () {
    ParamArray = {
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': OpenDate,
        'ScheduleID': ScheduleID,
        'ToDoID': ToDoID,
        'DisplayID': DisplayID
    };

    CallEncrypt(CallGetParam());

    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-20
작성목적 : 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
View.getAttach = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetAttachFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID
    };

    var obj = eval("View.succeededAttach");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-04-20
작성목적 : 첨부파일 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededAttach = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = "";

    if (attObj.dsAtt.length > 0) {
        jQuery.each($("#att_line"), function () { this.style.display = ""; });
    }

    for (var i = 0; i < attObj.dsAtt.length; i++) {
        filepath = attObj.dsAtt[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

        tmpAttachTag += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";

        var tmpFileName = attObj.dsAtt[i].FileName;
        if (tmpFileName.indexOf("'") > -1) {
            tmpFileName = tmpFileName.split("'").join("");
        }

        tmpAttachTag += "<tr><td class='dva'><a onclick=\"View.goAttach('" + filepath + attObj.dsAtt[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsAtt[i].FileName + "</a>";
        tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsAtt[i].FileLength) + "byte</td></tr>";
        tmpAttachTag += "</table>";
    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-20
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
작성일자 : 2012-04-20
작성목적 : 첨부파일 읽기 (웹서비스 값 받기)
***************************************************/
View.succeededGoAttach = function (data, status) {
    if (data.d == "!NULL") {
        NoticeShow("!", R_ProcessingError);
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
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 닥스트리밍 첨부파일 가져오기 (웹서비스 호출)
***************************************************/
View.getDS = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetDSFile"

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID
        , 'TYPE': "PIM_ATTACH"
    };
    var obj = eval("View.succeededDS");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2018-04-13
작성목적 : 문서 스트리밍 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededDS = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경

    var filepath = "";
    var tmpAttachTag = R_Attach;

    for (var i = 0; i < attObj.dsDS.length; i++) {
        if (attObj.dsDS[i].FileID.length > 0) {
            tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"View.goDocStream('" + attObj.dsDS[i].FileID + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        } else {
            filepath = attObj.dsDS[i].FilePath.replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/")

            var tmpFileName = attObj.dsDS[i].FileName;
            if (tmpFileName.indexOf("'") > -1) {
                //tmpFileName = attObj.dsAtt[i].FileGUID;
                tmpFileName = tmpFileName.split("'").join("");
            }

            tmpAttachTag += "<div style='word-wrap:break-word;'><a class='linkDiv' onclick=\"View.goAttach('" + filepath + attObj.dsDS[i].FileGUID + "','" + tmpFileName + "')\">" + attObj.dsDS[i].FileName + "</a>";
            tmpAttachTag += "<span class='iste'>&nbsp;" + numberWithCommas(attObj.dsDS[i].FileLength) + "byte</span></div>";
        }

    }

    $("#attach").empty();
    $("#attach").append(tmpAttachTag);

    if (attObj.dsDS.length == 0) {
        View.getAttach();
    }

}


/**************************************************
작성자   : 이중호
작성일자 : 2018-06-21
작성목적 : 문서 스트리밍 Post
***************************************************/
View.goDocStream = function (fileID) {

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

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 댓글 가져오기 (웹서비스 호출)
***************************************************/
View.getScheduleReply = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetScheduleReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID
    };

    var obj = eval("View.succeededScheduleReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 댓글  가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededScheduleReply = function (data, status) {
    var srObj = data.d;

    var tmpSR = "";

    if (srObj.length > 0) {
        jQuery.each($("#reply"), function () { this.style.display = ""; });
    }

    for (var i = 0; i < srObj.length; i++) {

        if (srObj[i].View == "Y") {
            tmpSR += "<div class='title'><div>";
            tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpSR += srObj[i].UserName + "&nbsp;| " + srObj[i].InputDate;
            tmpSR += "<br />";
            tmpSR += srObj[i].Content + " " + srObj[i].ShowNew;
            tmpSR += "</td>";
            tmpSR += "<td>&nbsp;</td></tr><tr>"
            tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpSR += "</table>";
            tmpSR += "</div></div>";
        }
        else {
            tmpSR += "<div class='title'><div>";
            tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpSR += R_SecurityMsg;
            tmpSR += "</td>";
            tmpSR += "<td>&nbsp;</td></tr><tr>"
            tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpSR += "</table>";
            tmpSR += "</div></div>";
        }
    }

    $("#reply").empty();
    $("#reply").append(tmpSR);

}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 할일 댓글 가져오기 (웹서비스 호출)
***************************************************/
View.getToDoReply = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetToDoReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ToDoID': ToDoID
    };

    var obj = eval("View.succeededToDoReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 할일 댓글  가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededToDoReply = function (data, status) {
    var srObj = data.d;

    var tmpTR = "";

    if (srObj.length > 0) {
        jQuery.each($("#reply"), function () { this.style.display = ""; });
    }

    for (var i = 0; i < srObj.length; i++) {
        if (srObj[i].View == "Y") {
            tmpTR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpTR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpTR += srObj[i].UserName + "&nbsp;| " + srObj[i].InputDate;
            tmpTR += "<br />";
            tmpTR += srObj[i].Content + " " + srObj[i].ShowNew;
            tmpTR += "</td>";
            tmpTR += "<td>&nbsp;</td></tr><tr>"
            tmpTR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpTR += "</table>";
        }
        else {
            tmpTR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
            tmpTR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
            tmpTR += R_SecurityMsg;
            tmpTR += "</td>";
            tmpTR += "<td>&nbsp;</td></tr><tr>"
            tmpTR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
            tmpTR += "</table>";
        }
    }

    $("#reply").empty();
    $("#reply").append(tmpTR);

}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 댓글 등록 화면으로 가기 
***************************************************/
View.add = function () {

    ParamArray = {
        'ToDoID': ToDoID,
        'ScheduleID': ScheduleID,
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': OpenDate,
        'RealUserID': RealUserID,
        'DisplayID': DisplayID
    };

    CallEncrypt(CallGetParam());

    location.href = "ReplyAdd.aspx?p=" + GetEncryptValue;
}

