
Portal = {}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-30
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    NowDate = NowYear + "-" + NowMonth + "-" + NowDay;

    //LoadingShow(true);

    // Badge 수정
    if ((UUID != "") && (approvalCheck == "Y") && (MobileType == "ios")) {
        Portal.SetBadge();
    }

    setTimeout("Portal.Load();", 300);

    //LoadingShow(false);

    $('#aMail').click(function (e) {
        Portal.goList('0');
    });

    $('#mailMore').click(function (e) {
        Portal.goList('1');
    });

    $('#aPim').click(function (e) {
        Portal.goList('2');
    });

    $('#pimMore').click(function (e) {
        Portal.goList('2');
    });

    $('#aBoard').click(function (e) {
        Portal.goList('3');
    });

    $('#boardMore').click(function (e) {
        Portal.goList('4');
    });

    $('#aApproval').click(function (e) {
        Portal.goList('5');
    });

    $('#approvalMore').click(function (e) {
        Portal.goList('6');
    });

    $('#AprREP').click(function (e) {
        Portal.goAppList('REP', 'List');
    });

    $('#AprAPP').click(function (e) {
        Portal.goAppList('APP', 'List');
    });

    $('#AppREL').click(function (e) {
        Portal.goAppList('REL', 'List');
    });

    $('#AppPRO').click(function (e) {
        location.href = "/DeskPlusMobile/Approval/Proxy.aspx";
    });

    $('#TaskRE').click(function (e) {
        Portal.goTaskList('RE');
    });

    $('#TaskTO').click(function (e) {
        Portal.goTaskList('TO');
    });

    $('#TaskDE').click(function (e) {
        location.href = "/DeskPlusMobile/Task/DelayList.aspx";
    });

    $('#TaskRC').click(function (e) {
        Portal.goTaskList('RC');
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2012-03-16
작성목적 : 처음 호출
***************************************************/
Portal.Load = function () {

    if (mailCheck == "Y") {
        Portal.getMailList(); //메일 리스트   
    }

    if (pimsCheck == "Y") {
        //Portal.getScheduleList(); //일정 리스트   
        //Portal.getToDoList(); //할일 리스트  
        Portal.getScheduleToDoList();
    }

    if (noticeCheck == "Y") {
        Portal.getBoardList();
    }

    if (approvalCheck == "Y") {
        Portal.getApprovalList();
    }

    if (attendCheck == "Y") {
        Portal.AttendanceInit(); //근태 초기화
    }

    $("p.hd_select").click();
}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-30
작성목적 : 메일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Portal.getMailList = function () {
    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetMailList";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Portal.succeededMailList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-30
작성목적 : 메일 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededMailList = function (data, status) {
    $('#phMail').setTemplateURL('/DeskPlusMobile/Portal/Jt/JTMail.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMail').processTemplate(data.d);

    setTimeout(function () {
        Portal.MailResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Portal.MailResource = function () {

    if ($("#NoMailListData")[0] != undefined) {
        $("#NoMailListData")[0].innerHTML = R_NoContent;
    }

    $(".receive").each(function () {
        $(this).text(R_Receive);
    });
    $(".refer").each(function () {
        $(this).text(R_Refer);
    });
    $(".refer2").each(function () {
        $(this).text(R_Refer2);
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 일정 가져오기 (웹서비스 호출)
***************************************************/
Portal.getScheduleList = function () {

    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetScheduleList";

    var tmpDate = NowYear + "-" + NowMonth + "-" + NowDay;

    //파라미터값 배열에 담기
    ParamArray = {
        'TmpDate': tmpDate,
        'UserID': UserID,
        'IsOpen': ''
    };

    var obj = eval("Portal.succeededSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 일정 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededSchedule = function (data, status) {
    $('#phSchedule').setTemplateURL('/DeskPlusMobile/Portal/Jt/JTSchedule.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSchedule').processTemplate(data.d);

    setTimeout(function () {
        Portal.ScheduleResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Portal.ScheduleResource = function () {

    if ($("#NoScheduleListData")[0] != undefined) {
        $("#NoScheduleListData")[0].innerHTML = R_NoTodaySchedule;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 호출)
***************************************************/
Portal.getToDoList = function () {

    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetToDoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': NowYear,
        'Month': NowMonth,
        'Day': NowDay,
        'UserID': UserID,
        'IsOpen': ''
    };

    var obj = eval("Portal.succeededToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededToDo = function (data, status) {
    $('#phToDo').setTemplateURL('/DeskPlusMobile/Portal/Jt/JTToDo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phToDo').processTemplate(data.d);

    setTimeout(function () {
        Portal.ToDoResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Portal.ToDoResource = function () {

    if ($("#NoToDoListData")[0] != undefined) {
        $("#NoToDoListData")[0].innerHTML = R_NoTodayToDo;
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-04-30
작성목적 : 일정 할일 가져오기 (웹서비스 호출)
***************************************************/
Portal.getScheduleToDoList = function () {
    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetScheduleToDoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'UserID': UserID,
        'IsOpen': '',
        'Year': NowYear,
        'Month': NowMonth,
        'Day': NowDay
    };

    var obj = eval("Portal.succeededScheduleToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-04-30
작성목적 : 일정 할일 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededScheduleToDo = function (data, status) {
    var rtnArray = JSON.parse(data.d);

    $("#phScheduleToDo").html("");
    var ScheduleYN = false;
    var ToDoYN = false;

    var rtnCnt = rtnArray["rtnList"].length > 10 ? 10 : rtnArray["rtnList"].length
    for (var i = 0; i < rtnCnt; i++) {
        if (rtnArray["rtnList"][i]["ScheduleID"] > 0) {
            ScheduleYN = true;
            var strHtml = "";
            strHtml += "<li class='contentblock " + (rtnArray["rtnList"][i]["UserID"] == UserID ? "mylist" : "") + "' >";
            strHtml += "<div class='Mtitle linkDiv'";
            strHtml += " onclick=\"javascript:Portal.viewSchedule('" + rtnArray["rtnList"][i]["ScheduleID"] + "','" + rtnArray["rtnList"][i]["UserID"] + "','" + rtnArray["rtnList"][i]["DisplayID"] + "')\">";
            switch (rtnArray["rtnList"][i]["OpenType"]) {
                case "0":
                    strHtml += "<a><div class='pim_pr'>●</div>";
                    break;
                case "1":
                    strHtml += "<a><div class='pim_dept'>●</div>";
                    break;
                case "2":
                    strHtml += "<a><div class='pim_share'>●</div>";
                    break;
                case "3":
                    strHtml += "<a><div class='pim_tot'>●</div>";
                    break;
            }
            strHtml += "<div class='conBox'><span class='LNoready'>" + rtnArray["rtnList"][i]["Title"] + " " + rtnArray["rtnList"][i]["ReplyCnt"] + " </span> <br />";
            strHtml += "<span class='sch_user'>";
            if (rtnArray["rtnList"][i]["UserInfo"] != "") {
                strHtml += rtnArray["rtnList"][i]["UserInfo"] + "<br />";
            }
            strHtml += rtnArray["rtnList"][i]["OpenTypeStr"] + "&nbsp;&nbsp;" + rtnArray["rtnList"][i]["Status"] + "<span>";
            strHtml += "&nbsp;" + rtnArray["rtnList"][i]["StartTime"] + " ~ " + rtnArray["rtnList"][i]["EndTime"] + "</span></div></a></div></li>";

            $("#phScheduleToDo").append(strHtml);
        } else if (rtnArray["rtnList"][i]["ToDoID"] > 0) {
            ToDoYN = true;
            var strHtml = "";
            strHtml += "<li class='contentblock " + (rtnArray["rtnList"][i]["UserID"] == UserID ? "mylist" : "") + "' >";
            strHtml += "<div class='Mtitle linkDiv'";
            strHtml += " onclick=\"javascript:Portal.viewToDo('" + rtnArray["rtnList"][i]["ToDoID"] + "','" + rtnArray["rtnList"][i]["UserID"] + "')\">";
            switch (rtnArray["rtnList"][i]["OpenType"]) {
                case "0":
                    strHtml += "<a><div class='pim_pr'>●</div>";
                    break;
                case "1":
                    strHtml += "<a><div class='pim_dept'>●</div>";
                    break;
                case "2":
                    strHtml += "<a><div class='pim_share'>●</div>";
                    break;
                case "3":
                    strHtml += "<a><div class='pim_tot'>●</div>";
                    break;
            }
            strHtml += "<div class='conBox'><span class='LNoready'>" + rtnArray["rtnList"][i]["ScheduleYN"] + rtnArray["rtnList"][i]["Title"] + " " + rtnArray["rtnList"][i]["ReplyCnt"] + " </span> <br />";
            strHtml += "<span class='sch_user'>";
            if (rtnArray["rtnList"][i]["UserInfo"] != "") {
                strHtml += rtnArray["rtnList"][i]["UserInfo"] + "<br />";
            }
            strHtml += rtnArray["rtnList"][i]["OpenTypeStr"] + "&nbsp;&nbsp;<span class='bg-success text-white'>&nbsp;" + rtnArray["rtnList"][i]["Status"] + "&nbsp;</span>";
            strHtml += "&nbsp;&nbsp;" + rtnArray["rtnList"][i]["Priority"] + "</span></div></a></div></li>";

            $("#phScheduleToDo").append(strHtml);
        }
    }

    if (!ScheduleYN) {
        var strHtml = "";
        strHtml += "<li class='contentblock'><div class='title'><a href='#' class='mct'><span id='NoScheduleListData' />" + R_NoTodaySchedule + "</a></div></li>"
        $("#phScheduleToDo").prepend(strHtml);
    }

    if (!ToDoYN) {
        var strHtml = "";
        strHtml += "<li class='contentblock'><div class='title'><a href='#' class='mct'><span id='NoToDoListData' />" + R_NoTodayToDo + "</a></div></li>"
        $("#phScheduleToDo").append(strHtml);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 일정 보기
***************************************************/
Portal.viewSchedule = function (ScheduleID, RealUserID, DisplayID) {

    ParamArray = {
        'ScheduleID': ScheduleID,
        'OpenID': '0',
        'OpenName': '',
        'OpenDate': NowDate,
        'RealUserID': RealUserID,
        'DisplayID': DisplayID
    };

    CallEncrypt(CallGetParam());

    location.href = "../Pims/View.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 할일 보기
***************************************************/
Portal.viewToDo = function (ToDoID, RealUserID) {

    ParamArray = {
        'ToDoID': ToDoID,
        'OpenID': '0',
        'OpenName': '',
        'OpenDate': NowDate,
        'RealUserID': RealUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "../Pims/View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 더보기
***************************************************/
Portal.goList = function (arg) {

    if (arg == "0") {
        // 편지함 가기
        location.href = "../Mail/Folder.aspx";
    }
    else if (arg == "1") {
        // 받은 편지함 가기 
        ParamArray = {
            'Code': '1'
        };

        CallEncrypt(CallGetParam());

        location.href = "../Mail/List.aspx?p=" + GetEncryptValue;
    }
    else if (arg == "2") {
        // 일정관리 가기 
        location.href = "../Pims/List.aspx";
    }
    else if (arg == "3") {
        // 게시판 가기
        location.href = "../Board/Folder.aspx";
    }
    else if (arg == "4") {
        if (MainCID != "") {
            // 게시판 가기 
            ParamArray = {
                'MainCID': MainCID,
                'PCatID': PCatID
            };

            CallEncrypt(CallGetParam());

            location.href = "../Board/List.aspx?p=" + GetEncryptValue;
        }
        else {
            location.href = "../Board/Folder.aspx";
        }
    }
    else if (arg == "5") {
        // 결재상황 조회 가기
        location.href = "../Approval/Folder.aspx";
    }
    else if (arg == "6") {
        // 결재대상문서 가기
        ParamArray = {
            'BPage': 'APP'
        };

        CallEncrypt(CallGetParam());

        location.href = "../Approval/List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 메일 보기 화면으로 가기 
***************************************************/
Portal.viewMail = function (mailsn, read_flag) {

    ParamArray = {
        'Mail_sn': mailsn,
        'Code': '1',
        'Read_flag': read_flag
    };


    CallEncrypt(CallGetParam());

    location.href = "../Mail/View.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 게시판 리스트 가져오기 (웹서비스 호출)
***************************************************/
Portal.getBoardList = function () {

    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetBoardList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': '1',
        'PageSize': '5',
        'SearchValue': '',
        'PCatID': PCatID
    };

    var obj = eval("Portal.succeededBoardList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 게시판 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededBoardList = function (data, status) {
    $('#phBoard').setTemplateURL('/DeskPlusMobile/Portal/Jt/JTBoard.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phBoard').processTemplate(data.d);

    setTimeout(function () {
        Portal.BoardResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Portal.BoardResource = function () {

    if ($("#NoBoardListData")[0] != undefined) {
        $("#NoBoardListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
Portal.viewBoard = function (categoryID, listID) {

    if (listID == "0") {

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': categoryID,
            'Sv': ''
        };

        CallEncrypt(CallGetParam());

        location.href = "../Board/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'BPageNo': '0',
            'CID': categoryID,
            'ID': listID,
            'Sv': ''
        };

        CallEncrypt(CallGetParam());

        location.href = "../Board/View.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 전자결재 리스트 가져오기 (웹서비스 호출)
***************************************************/
Portal.getApprovalList = function () {

    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetApprovalList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': '1',
        'PageSize': '5',
        'TargetUID': UserID
    };

    var obj = eval("Portal.succeededApprovalList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 전자결재 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Portal.succeededApprovalList = function (data, status) {
    $('#phApproval').setTemplateURL('/DeskPlusMobile/Portal/Jt/JTApproval.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        Portal.ApprovalResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-29
작성목적 : 다국어 처리
**********************************************************************************************/
Portal.ApprovalResource = function () {

    if ($("#NoApprovalListData")[0] != undefined) {
        $("#NoApprovalListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-14
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
Portal.viewApproval = function (dID, stat, aTag, accTag, num2, seq, aType) {

    ParamArray = {
        'BPage': 'APP',
        'MoreSize': '10',
        'PageNo': '1',
        'DID': dID,
        'Stat': stat,
        'ATag': aTag,
        'AccTag': accTag,
        'Num2': num2,
        'Seq': seq,
        'AType': aType,
        'TargetUID': UserID
    };

    CallEncrypt(CallGetParam());

    location.href = "../Approval/View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : freeman
작성일자 : 2015-08-26
작성목적 : 모바일 겸직 변경
***************************************************/
Portal.SetBaseDept = function () {
    var userInfo = $("#ddlUserInfo").val();
    var userInfoArr = userInfo.split('~');
    var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/SetBaseDept";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': userInfoArr[0],
        'UserID': userInfoArr[1],
        'DeptID': userInfoArr[2],
        'CodeTitleID': userInfoArr[3],
        'CodeDutyID': userInfoArr[4]
    };

    var obj = eval("Portal.SLO");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : freeman
작성일자 : 2015-08-26
작성목적 : 모바일 겸직 SLO
***************************************************/
Portal.SLO = function (data, status) {

    if (data.d == "OK") {
        var userInfo = $("#ddlUserInfo").val();
        var userInfoArr = userInfo.split('~');

        ParamArray = {
            'uID': userInfoArr[5],
            'uPW': userInfoArr[6],
            'uCID': userInfoArr[0]
        };

        //var url = "/DeskPlusMobile/Portal/Ws/GetPortal.asmx/GetLoginOutSide";
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

        location.href = "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetLoginOutSideENC?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2018-12-20
작성목적 : 결재 리스트 페이지 이동
***************************************************/
Portal.goAppList = function (arg, url) {

    ParamArray = {
        'BPage': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Approval/" + url + ".aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2018-12-20
작성목적 : 업무협업 리스트 페이지 이동
***************************************************/
Portal.goTaskList = function (arg) {

    ParamArray = {
        'BPage': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusMobile/Task/List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-20
작성목적 : 근태 초기화
***************************************************/
Portal.AttendanceInit = function () {

    var h = NowHour;
    var m = parseInt(NowMinute);
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
    var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

    var realWCTime = parseInt(WCTimeHour + WCTimeMinute); //실제 퇴근시간 시분 1800

    // 현재시각이 출근시간 이상이고 정규 퇴근시간 이전일때 조퇴로 표시
    if (tNowTime >= tWSTime && tNowTime < tWCTime) {
        document.getElementById("HROffWork").innerText = Attendance_Early; //조퇴
    }

    // 출근시간이 등록되어 있을 경우
    if (bWS == "1") {
        document.getElementById("attendanceGotoWork").innerText = WSTime; //출근시간
        //document.getElementById("GotoWork").className = "btn-lg btn-outline-light br-white";
    }
    else {
        //document.getElementById("HROffWork").className = "btn-lg btn-outline-light br-white";
    }

    // 퇴근시간이 등록되어 있을 경우
    if (bWC == "1") {
        // 퇴근시간이 정규 퇴근시간 이전이면 조퇴이고 조퇴라인 표시해줌
        if (realWCTime < tWCTime) {
            document.getElementById("trEarlyTime").style.display = "";
            document.getElementById("spEarlyTime").innerText = WCTime;
        }
        else {
            document.getElementById("spOffWork").style.display = "";
            document.getElementById("attendanceOffWork").innerText = WCTime;
        }
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 출근
***************************************************/
Portal.GotoWork = function () {
    //var h = NowHour;
    //var m = parseInt(NowMinute);
    var d = new Date();
    var h = d.getHours();
    var m = parseInt(d.getMinutes());
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 915
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900

    if (isNaN(tNowTime)) return;

    if (tWSTime == "0") return;

    if (tNowTime >= tWSTime) {
        if (bWS == "1") return;

        if (tNowTime > tWSTime) {

            ParamArray = {
                'late': '1'
            };

            CallEncrypt(CallGetParam());

            location.href = "../HR/Attendance.aspx?p=" + GetEncryptValue;
            return;
        }
    }
    else {
        if (bWS == "1") {
            // 출근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
            action_popup.confirm(Attendance_GotoWorkRe, function (res) {
                if (res) {
                    if (BtnClickCheck != "") {
                        //NoticeShow("!", R_ProcessingWait);
                        return;
                    }

                    LoadingShow(true);

                    BtnClickCheck = "Y";

                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetGotoWork";
                    ParamArray = { 'isHoli': isHoli }

                    var obj = eval("Portal.GotoWorkSucceeded");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
        else {
            if (BtnClickCheck != "") {
                //NoticeShow("!", R_ProcessingWait);
                return;
            }

            LoadingShow(true);

            BtnClickCheck = "Y";

            var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetGotoWork";
            ParamArray = { 'isHoli': isHoli }

            var obj = eval("Portal.GotoWorkSucceeded");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 출근 성공시
***************************************************/
Portal.GotoWorkSucceeded = function (data, status) {

    if (data.d != null && data.d.length > 0) {
        LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        //alert(R_GotoSucceed + "\n" + R_GotoTime + ": " + time + "\n" + R_Location + ": " + location); //"출근 처리되었습니다.\n출근시간: " + time + "\n위치: " + location

        document.location.href = "Portal.aspx";
    }
    else {
        //NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 퇴근
***************************************************/
Portal.OffWork = function () {
    //var h = NowHour;
    //var m = parseInt(NowMinute);
    var d = new Date();
    var h = d.getHours();
    var m = parseInt(d.getMinutes());
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
    var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

    // 현재시각이 출근시간 전이면 이전일로 처리한다. (00:00AM ~ 출근시간 사이)
    if (tNowTime < tWSTime) {
        Portal.PreAttendanceLoad();
    }
    else {
        if (bWS != "1") {
            // 출근 상태가 아닙니다.
            ParamArray = {
                'early': '1'
            };

            CallEncrypt(CallGetParam());

            location.href = "../HR/Attendance.aspx?p=" + GetEncryptValue;
            return;
        }

        if (tNowTime < tWCTime) { //조퇴
            if (bWC == "1") { //한번 조퇴했으면 조퇴취소 해야함.
                return;
            }

            //조퇴 처리
            ParamArray = {
                'early': '1'
            };

            CallEncrypt(CallGetParam());

            location.href = "../HR/Attendance.aspx?p=" + GetEncryptValue;
            return;
        }
        else {
            if (bWC != "1") {
                action_popup.confirm(Attendance_OffWorkConfirm, function (res) { // 퇴근 처리 하시겠습니까?
                    if (res) {
                        Portal.OffWorkSet();
                    }
                })
            }
            else {
                action_popup.confirm(Attendance_OffWorkRe, function (res) { // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
                    if (res) {
                        Portal.OffWorkSet();
                    }
                })
            }
        }
    }
}

/**************************************************
작성목적 : 퇴근 호출 
***************************************************/
Portal.OffWorkSet = function () {

    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetOffWork";
    ParamArray = { 'isHoli': isHoli }

    var obj = eval("Portal.OffWorkSucceeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 퇴근 성공시
***************************************************/
Portal.OffWorkSucceeded = function (data, status) {

    if (data.d != null && data.d.length > 0) {
        LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var early = data.d.split('^')[2];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        /*
        if (early == "1") {
        alert(R_EarlySucceed + "\n" + R_EarlyTime + ": " + time + "\n" + R_Location + ": " + location); //"조퇴 처리되었습니다.\n조퇴시간: " + time + "\n위치: " + location
        }
        else {
        alert(R_OffWorkSucceed + "\n" + R_OffWorkTime + ": " + time + "\n" + R_Location + ": " + location); //"퇴근 처리되었습니다.\n퇴근시간: " + time + "\n위치: " + location
        }
        */

        document.location.href = "Portal.aspx";
    }
    else {
        //NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 이전일 퇴근시간 등록
***************************************************/
Portal.PreAttendanceLoad = function () {

    if (BtnClickCheck != "") {
        //NoticeShow("!", R_ProcessingWait);
        return;
    }

    //LoadingShow(true);

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/HR/Ws/GetAttendance.asmx/GetPreAttendanceData";
    ParamArray = {}

    var obj = eval("Portal.PreAttendance_SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

Portal.PreAttendance_SuccessList = function (data, status) {

    if (data.d) {
        //LoadingShow(false);
        BtnClickCheck = "";
    }
    else {
        //NoticeShow("!", R_ProcessingError);
        //LoadingShow(false);
        return;
    }

    var rtn = data.d;

    var array_rtn = rtn.split("|");

    var Pre_bWS = array_rtn[0]; // 전날 출근여부
    var Pre_bWC = array_rtn[2]; // 전날 퇴근여부
    var Pre_isHoli = array_rtn[4]; // 전날 휴일여부
    var Pre_Date = array_rtn[5]; //전날 날짜

    if (Pre_bWS != "1") // 전날 출근여부
    {
        //조퇴 처리
        ParamArray = {
            'early': '1'
        };

        CallEncrypt(CallGetParam());

        location.href = "../HR/Attendance.aspx?p=" + GetEncryptValue;
        return;
    }
    else {
        if (Pre_bWC != "1") // 전날 퇴근여부
        {
            action_popup.confirm(Pre_Date + " " + Attendance_OffWorkConfirm, function (res) { // 퇴근 처리 하시겠습니까?
                if (res) {
                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetPreOffWork";
                    ParamArray = { 'reason': "", 'isHoli': Pre_isHoli }

                    var obj = eval("PreOffWork_SuccessList");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
        else {
            action_popup.confirm(Pre_Date + " " + Attendance_OffWorkRe, function (res) { // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
                if (res) {
                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetPreOffWork";
                    ParamArray = { 'reason': "", 'isHoli': Pre_isHoli }

                    var obj = eval("PreOffWork_SuccessList");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
    }
}

function PreOffWork_SuccessList(data, status) {
    if (data.d != null && data.d.length > 0) {
        //LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        //alert(R_OffWorkSucceed + "\n" + R_OffWorkTime + ": " + time + "\n" + R_Location + ": " + location); //"퇴근 처리되었습니다.\n퇴근시간: " + time + "\n위치: " + location

        document.location.href = "Portal.aspx";
    }
    else {
        //NoticeShow("!", R_ProcessingError);
        //LoadingShow(false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-03-06
작성목적 : badge 수정 
***************************************************/
Portal.SetBadge = function () {

    try {
        //window.webkit.messageHandlers.set_badge.postMessage(BadgeCnt);
        setBadgeIOS(parseInt(BadgeCnt));
    } catch (err) {
        //alert(err);
    }    
}

