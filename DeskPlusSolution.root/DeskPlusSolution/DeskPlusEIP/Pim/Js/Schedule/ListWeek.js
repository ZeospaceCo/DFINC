ListWeek = {}

var date;

$(function () {

    $(document).ready(function () {
        // LoadingControl(1);

        date = new Date(Year, Month - 1, Day);
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        NowDate = date;

        $('#calendar').fullCalendar({
            header: {
                left: '',
                center: 'title',
                right: ''
            },
            editable: true, eventClick: function (event) {
                ListWeek.ShowView(event.id);
            },
            eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
                ListWeek.EventModify(event);
            },
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                ListWeek.EventModify(event);
            },
            eventMouseover: function (calEvent, domEvent) {

            },
            eventMouseout: function (calEvent, domEvent) {
              
            },
            dayClick: function (date, allDay, jsEvent, view) {
                ClearPopup();
                SimpleAddSChedule(date);
            }

        });


        $("#tbTitle").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();

                var arg = $("#hdChoiceSche").val();
                if (arg.length == 0) {
                    AddSimple.ValidateSchedule();
                }
                else {
                    Title = jQuery.trim($('#tbTitle').val());
                    if (Title.length == 0) {
                        alert(Add_InputTitle);
                        return;
                    }

                    StartTime = $('#tbstartDate').val() + " " + $('#ddlstartTime').val();
                    EndTime = $('#tbendDate').val() + " " + $('#ddlendTime').val();

                    var sdate = $("#tbstartDate").val();
                    var edate = $("#tbendDate").val();
                    var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);
                    if (!dtResponse) {
                        alert(TimeOver); //TimeAlert
                        return;
                    }
                    var stime = $("#ddlstartTime").val();
                    var etime = $("#ddlendTime").val();

                    stime = stime.replace(/:/gi, '');
                    etime = etime.replace(/:/gi, '');
                    var sInt = parseInt(stime, 10);
                    var eInt = parseInt(etime, 10);
                    if ((sInt >= eInt) && (sdate == edate)) {
                        alert(TimeOver); //TimeAlert
                        return;
                    }

                    ListWeek.SimpleEventModify();
                }

                parent.ParentCallInterFace();

            }
        });


        // 현재일
        $('#lblDate').click(function (e) {
            e.preventDefault();
            ListWeek.Calendar_Control('T')
        });


        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListWeek.Calendar_Control('<')
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListWeek.Calendar_Control('>')
        });

        $('#CbtnClosetodo').click(function (e) {
            e.preventDefault();
            $("#divtodo").hide("scale");
        });

        $('#CbtnCloseView').click(function (e) {
            e.preventDefault();
            $("#divQuickSchView").hide();
        });

        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            ListWeek.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            ListWeek.SettingTime();
        });

        //스케줄상세페이지 이동
        $('#CbtnScheDetail').click(function (e) {
            e.preventDefault();
            ListWeek.DetailPageMove();
        });

        $('#CbtnScheDetailView').click(function (e) {
            e.preventDefault();
            ListWeek.DetailPageMove();
        });


        // 일정삭제
        $('#CbtnScheDel').click(function (e) {
            e.preventDefault();
            ListWeek.DelScheduleButton();
            parent.ParentCallInterFace();
        });

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            //AddSimple.Confirm();
            var arg = $("#hdChoiceSche").val();
            if (arg.length == 0) {
                AddSimple.ValidateSchedule();
            }
            else {
                Title = jQuery.trim($('#tbTitle').val());
                if (Title.length == 0) {
                    alert(Add_InputTitle);
                    return;
                }

                StartTime = $('#tbstartDate').val() + " " + $('#ddlstartTime').val();
                EndTime = $('#tbendDate').val() + " " + $('#ddlendTime').val();

                var sdate = $("#tbstartDate").val();
                var edate = $("#tbendDate").val();
                var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);

                if (!dtResponse) {
                    alert(TimeOver); //TimeAlert
                    return;
                }
                var stime = $("#ddlstartTime").val();
                var etime = $("#ddlendTime").val();

                stime = stime.replace(/:/gi, '');
                etime = etime.replace(/:/gi, '');
                var sInt = parseInt(stime, 10);
                var eInt = parseInt(etime, 10);
                if ((sInt >= eInt) && (sdate == edate)) {
                    alert(TimeOver); //TimeAlert
                    return;
                }

                ListWeek.SimpleEventModify();
            }

            parent.ParentCallInterFace();
        });


        $('#calendar').fullCalendar('gotoDate', y, m, d);

        ParentGetCheck();
        ListWeek.TitleSetting();
        ListWeek.SettingEvent();
        $('#calendar').fullCalendar('changeView', 'agendaWeek');

        if (parent.FrameHeightControl != null) {
            var yHeight = 1200;
            parent.FrameHeightControl('W');
        }
        $("#ddlShare").val(DefaultSetting);

        // 도움말 오픈
        $('#HelpCategory').click(function (e) {
            e.preventDefault();
            var url = "../Help/GetCategoryInfo.aspx";
            var po = GetPostion();
            FixModalDesign(300, 350, url, '', po.y);
        });

        //기본알람으로 설정
        $('#CbtnSetting').click(function (e) {
            e.preventDefault();
            ListWeek.AlarmSetting();
        });

        //알람1
        $('#ddlnotikind').change(function (e) {
            e.preventDefault();

            if ($('#ddlnotikind').val() == "M") //메신저
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddlAlarmTime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "E") //EMAIL
            {
                $('#idEmail').show();
                $('#msgMail').show();
                $('#ddlAlarmTime').hide();
                $('#divment').hide();
            }
            if ($('#ddlnotikind').val() == "B") //Mobile
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddlAlarmTime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "") //무
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddlAlarmTime').hide();
                $('#divment').hide();
            }
        });

        $('#idEmail').hide();
        $('#msgMail').hide();
        $('#ddlAlarmTime').hide();
        $('#divment').hide();

        if ($('#ddlnotikind').val() == "M") {
            $('#ddlAlarmTime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "B") {
            $('#ddlAlarmTime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "E") {
            $('#idEmail').show();
            $('#msgMail').show();
        }

    });
});



/**************************************************
작성자   : DANIEL
작성일자 : 2011-04-11
작성목적 : 스케줄 상태값 가지고 다니기
***************************************************/
function ParentGetCheck() {
    if (parent.ScheduleCheck.length > 0) {

        var arChk = parent.ScheduleCheck.split(',');

        for (var i = 0; i < arChk.length; i++) {
            if (arChk[i].length > 0) {
                if (arChk[i] == 1) {
                    $("#chksh1").attr("checked", true);
                    $("#chksh2").attr("checked", false);
                    $("#chksh3").attr("checked", false);
                    $("#chksh4").attr("checked", false);
                    $("#chksh5").attr("checked", false);
                }
                if (arChk[i] == 2) {
                    $("#chksh2").attr("checked", true);
                    $("#chksh1").attr("checked", false);
                }
                if (arChk[i] == 3) {
                    $("#chksh3").attr("checked", true);
                    $("#chksh1").attr("checked", false);
                }
                if (arChk[i] == 4) {
                    $("#chksh4").attr("checked", true);
                    $("#chksh1").attr("checked", false);
                }
                if (arChk[i] == 5) {
                    $("#chksh5").attr("checked", true);
                    $("#chksh1").attr("checked", false);
                }
            }
        }

    }
    else {
        $("#chksh1").attr("checked", true);
    }
}

function ParentGetEvent() {
    ListWeek.SettingEvent();
}

/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
ListWeek.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/*
용도: 달력 컨트롤
작성자: 김광일
작성일: 2011-7-12
*/
ListWeek.Calendar_Control = function (_arg) {

    if (_arg == "<") {
        $('#calendar').fullCalendar('prev');
        ListWeek.SettingEvent();
    }
    else if (_arg == ">") {
        $('#calendar').fullCalendar('next');
        ListWeek.SettingEvent();
    }
    else if (_arg == "T") {
        $('#calendar').fullCalendar('gotoDate', NowYear, NowMonth - 1, NowDay);
        ListWeek.SettingEvent();
    }

    ListWeek.TitleSetting();

}


/*
용도: 날짜 관리
작성자: 김광일
작성일: 2011-7-12
*/
ListWeek.DateControl = function () {
    ListWeek.SettingEvent();
}

/*
용도: 타이틀 세팅
작성자: 김광일
작성일: 2011-7-12
*/
ListWeek.TitleSetting = function () {
    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            $('#lblDate').html(NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate()));
        else
            $('#lblDate').html(NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate()));
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            $('#lblDate').html(NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate()));
        else
            $('#lblDate').html(NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate()));

    }
    var dateinfo = $('#lblDate').html();
    ListWeek.PasingDate(dateinfo);
}


//다국어 날짜 세팅
function DateFormat(_date) {
    var arrDate = _date.split('-');
    var ParsingDate = '';
    switch (PreLang) {
        case "ko":
            //"yy-mm-dd";
            ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            break;
        case "en":
            //"mm/dd/yy";
            ParsingDate = arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"dd/mm/yy";
            ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm/dd";
            ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "fr":
            //"dd/mm/yy";
            ParsingDate = arrDate[2] + "/" + arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm/dd";
            ParsingDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2];
            break;
        case "zh-tw":            
            //"yy-mm-dd";
            ParsingDate = arrDate[0] + "-" + arrDate[1] + "-" + arrDate[2];
            break;
    }
    return ParsingDate;
}




/*
용도   : 공휴일 가져오기
작성자 : 김광일
작성일 : 2011-7-20 
수정자 : 이지원
수정일 : 2017-01-24
*/
ListWeek.GetHoliday = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetDayInfoDB";
    ParamArray = {
        'year': NowDate.getFullYear()
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessGetHoliday");

    AjaxService(url, data, obj);
}

//ByFreeman 20130108
// 날짜 추가 
function addDate(dateObject, numDays) {
    dateObject.setDate(dateObject.getDate() + numDays);

    var d1;

    if ((parseInt(dateObject.getMonth()) + 1) < 10) {
        if (parseInt(dateObject.getDate()) < 10)
            d1 = dateObject.getFullYear() + "-0" + (parseInt(dateObject.getMonth()) + 1) + '-0' + parseInt(dateObject.getDate());
        else
            d1 = dateObject.getFullYear() + "-0" + (parseInt(dateObject.getMonth()) + 1) + '-' + parseInt(dateObject.getDate());
    }
    else {

        if (parseInt(dateObject.getDate()) < 10)
            d1 = dateObject.getFullYear() + "-" + (parseInt(dateObject.getMonth()) + 1) + '-0' + parseInt(dateObject.getDate());
        else
            d1 = dateObject.getFullYear() + "-" + (parseInt(dateObject.getMonth()) + 1) + '-' + parseInt(dateObject.getDate());

    }

    return d1;
}

// 현재 Weekend 와 비교
function existWeek(arrWeek, dateObject) {
    for (i = 0; i <= 6; i++) {
        if (arrWeek[i] == dateObject) {
            return true;
        }
    }
    return false;
}

ListWeek.SuccessGetHoliday = function (data, status) {
    //ByFreeman 20130108
    var weekdays = new Array();  //현재 weekend 값을 넣어 놓음
    var curIndex = NowDate.getDay();
    var i = 0; j = 0;

    for (i = curIndex; i >= 0; i--) {
        addDate(NowDate, -1);
    }

    for (j = 0; j <= 6; j++) { //한주를 배열에 추가함
        weekdays[j] = addDate(NowDate, 1);
    }

    $.each(data.d, function (index, list) {
        if (existWeek(weekdays, list.DateInfo)) {
            var tmpD = new Date(list.DateInfo);
            $("#thday" + tmpD.getDay()).css("color", "red");
        }
    });
}




/*이벤트가져오기*/

/*
용도 :  이벤트 가져오기
작성자: 김광일
작성일 : 2011-8-02 
*/
ListWeek.GetEvent = function () {
    StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + "-" + NowDate.getDate()
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleAllKind";

    var scope = 'N';
    Type = "I";
    var uid = CurrentUserID;
    if (CurrentUserID > 0) {
        uid = CurrentUserID;
        OpenType = "1";
        scope = "P";
    }


    ParamArray = {
        'StartDate': StartTime,
        'Type': Type,
        'OpenType': OpenType,
        'DayKind': 'W',
        'CurrentUserID': uid,
        'scope': scope
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessGetEvent");

    AjaxService(url, data, obj);
}

ListWeek.SuccessGetEvent = function (data, status) {

    var MoveTime = NowDate;
    $('#calendar').fullCalendar('destroy');

    $('#calendar').fullCalendar({
        header: {
            left: '',
            center: 'title',
            right: ''
        },
        loading: function (bool) {
            if (bool) LoadingControl(1);
            else LoadingControl(2);
        },
        events: data.d,
        editable: true, eventClick: function (event) {
            ListWeek.ShowView(event.id);
        }, 
        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
            ListWeek.EventModify(event);
        }, 
        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            ListWeek.EventModify(event);
        },
        eventMouseover: function (calEvent, domEvent) {

        },
        eventMouseout: function (calEvent, domEvent) {
            //  $("#events-layer").remove();
        },
        dayClick: function (date, allDay, jsEvent, view) {
            ClearPopup();
            SimpleAddSChedule(date);
        }

    });

    LoadingControl(2);

    var d = MoveTime.getDate();
    var m = MoveTime.getMonth();
    var y = MoveTime.getFullYear();
    $('#calendar').fullCalendar('gotoDate', y, m, d);

    $('#calendar').fullCalendar('option', 'contentHeight', 600);
}



function ConvertDateType(sDate) {
    var rDate;
    rDate = sDate.replace(/-/gi, '');

    var eDate = new Date(
                rDate.substring(0, 4), //년도
                (rDate.substring(4, 6)) - 1, //월
                rDate.substring(6, 8),  //날짜
                rDate.substring(9, 11), //시간
                rDate.substring(12, 14), 0, 0); //분

    return eDate;
}

function AddEvent(sdate, edate, stitle, surl, suid, salldate, scolor, edit) {

    $('#calendar').fullCalendar('renderEvent', {
        start: ConvertDateType(sdate), end: ConvertDateType(edate),
        title: stitle,
        id: suid,
        allDay: false, editable: edit,
        backgroundColor: scolor
    }, false);

}




/*
작성자: daniel
용도: 보기페이지 이동

*/
ListWeek.ShowView = function (arg) {

    ClearPopup();

    var tmpWidowHeight = $(window).height();
    var tmpScrollTop = parent.PageScrollTop;
    var tmpHeight = parent.PageScrollTop + 50;
    var tmpGap1 = parseInt(tmpWidowHeight) - parseInt(tmpScrollTop);
    var tmpGap2 = 300 - parseInt(tmpGap1) + 20;

    if (parseInt(tmpGap1) < 300) {
        tmpHeight = parent.PageScrollTop - parseInt(tmpGap2);
    }

    if (arg.length > 2) {

        //할일묶음
        if (arg.substr(0, 2) == "T_") {
            $("#divtodolist").html('');
            ListWeek.GetTodoList(arg);
            $("#divPopTitle").html(todocadalog);
            $("#divtodo").css("top", tmpHeight);
            $("#divtodo").css("left", (($(window).width() / 2) - ($("#divtodo").width() / 2)));
            $("#divtodo").show('scale');
            //$("#divtodo").draggable({ disabled: false });
            return;
        }
        //일정 묶음
        if (arg.substr(0, 2) == "S_") {
            $("#divtodolist").html('');
            ListWeek.GetEventDay(arg);
            $("#divPopTitle").html(schedulecadalog);
            $("#divtodo").css("top", tmpHeight);
            $("#divtodo").css("left", (($(window).width() / 2) - ($("#divtodo").width() / 2)));
            $("#divtodo").show('scale');
            //$("#divtodo").draggable({ disabled: false });
            return;
        }
        //기념일 묶음
        if (arg.substr(0, 2) == "H_") {
            $("#divtodolist").html('');
            ListWeek.GetHolidayList(arg);
            $("#divPopTitle").html(holidaycadalog);
            $("#divtodo").css("top", tmpHeight);
            $("#divtodo").css("left", (($(window).width() / 2) - ($("#divtodo").width() / 2)));
            $("#divtodo").show('scale');
            //$("#divtodo").draggable({ disabled: false });
            return;
        }   
    }
    ListWeek.GetSingleSchedule(arg);
    //ListWeek.GetShareInfo(arg);

    if (CheckUserID != UserId) {
        $("#divQuickSchView").css("top", tmpHeight);
        $("#divQuickSchView").css("left", (($(window).width() / 2) - ($("#divQuickSchView").width() / 2)));
        $("#divQuickSchView").show();
    }
    else {
        $("#divQuickSch").css("top", tmpHeight);
        $("#divQuickSch").css("left", (($(window).width() / 2) - ($("#divQuickSch").width() / 2)));
        $("#divQuickSch").show();
    }
}





/***********************************************************
작성자:김광일
작성일:2011/9/5
용도: 스케줄 수정
***********************************************************/
ListWeek.MoveMod = function (arg) {
    var d1;

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());

    }


    ParamArray = {
        'TodoID': arg,
        'Page': 'ListWeek',
        'Date': d1
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/Pim/ToDO/AddN.aspx?p=" + GetEncryptValue;
}




/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정
***********************************************************/
ListWeek.EventModify = function (event) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/ModDropSchedule";
    ParamArray = {
        'DisplayID': event.id,
        'StartTime': ConvertDateInfo(event.start),
        'EndTime': ConvertDateInfo(event.end),
        'StatusID': '-1'
    };
    var obj = eval("ListWeek.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정성공
***********************************************************/
ListWeek.SuccessEventModify = function (data, status) {
    if (!data.d)
        alert('수정실패');
    else
        delay(500);

    ListWeek.SettingEvent();
    parent.ParentCallInterFace();
}


function ConvertDateInfo(date) {

    var d1;
    if ((parseInt(date.getMonth()) + 1) < 10) {
        if (parseInt(date.getDate()) < 10)
            d1 = date.getFullYear() + "-0" + (parseInt(date.getMonth()) + 1) + '-0' + parseInt(date.getDate());
        else
            d1 = date.getFullYear() + "-0" + (parseInt(date.getMonth()) + 1) + '-' + parseInt(date.getDate());
    }
    else {

        if (parseInt(date.getDate()) < 10)
            d1 = date.getFullYear() + "-" + (parseInt(date.getMonth()) + 1) + '-0' + parseInt(date.getDate());
        else
            d1 = date.getFullYear() + "-" + (parseInt(date.getMonth()) + 1) + '-' + parseInt(date.getDate());

    }

    d1 = d1 + ' ' + date.getHours() + ":" + date.getMinutes();

    return d1;
}




/*
작성자:김광일
용도: 체크 박스
날짜:2011/8/30
*/
ListWeek.CheckSchedule = function (arg) {

    if (arg == -1) {
        if ($("#chksh1").attr("checked")) {
            $("#chksh2").attr("checked", false);
            $("#chksh3").attr("checked", false);
            $("#chksh4").attr("checked", false);
            $("#chksh5").attr("checked", false);
        }
    }
    if (arg == 0) {   //개인일정
        if ($("#chksh2").attr("checked")) {
            $("#chksh1").attr("checked", false);
        }
    }
    if (arg == 3) {   //부서일정
        if ($("#chksh3").attr("checked")) {
            $("#chksh1").attr("checked", false);
        }
    }
    if (arg == 1) {   //공유일정
        if ($("#chksh4").attr("checked")) {
            $("#chksh1").attr("checked", false);
        }
    }
    if (arg == 2) {   //전체일정
        if ($("#chksh5").attr("checked")) {
            $("#chksh1").attr("checked", false);
        }
    }

    var CheckState = '';
    if ($("#chksh1").attr("checked"))
        CheckState = CheckState + ",1";
    if ($("#chksh2").attr("checked"))
        CheckState = CheckState + ",2";
    if ($("#chksh3").attr("checked"))
        CheckState = CheckState + ",3";
    if ($("#chksh4").attr("checked"))
        CheckState = CheckState + ",4";
    if ($("#chksh5").attr("checked"))
        CheckState = CheckState + ",5";

    if (CheckState == "") {
        $("#chksh1").attr("checked", true);
        CheckState = CheckState + ",1";
    }

    parent.ScheduleCheck = CheckState;

    ListWeek.ModCheckMenu(CheckState);

    ListWeek.SettingEvent();
}

/******************************************
작성자: kkzz52
작성일: 2017-02-15
용도: 일정분류 체크박스 개인화 저장
****************************************/
ListWeek.ModCheckMenu = function (_checkState) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListWeek.asmx/ModCheckMenu";
    ParamArray = {
        'companyID': CompanyID,
        'userId': UserId,
        'checkState': _checkState
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessCheckMenu");

    AjaxService(url, data, obj);
}

ListWeek.SuccessCheckMenu = function (data, status) {
    // alert(data.d); 성공시 true
}

/*
작성자: daniel
용도: 검색전 세팅

*/
ListWeek.SettingEvent = function () {
    ClearPopup();

    $('#calendar').fullCalendar('removeEvents', null);
    $('#hdEvent').val('');
    OpenType = '';

    if ($("#chksh1").attr("checked")) {
        OpenType = OpenType + ",-1";
    }
    if ($("#chksh2").attr("checked")) {
        OpenType = OpenType + ",0";
    }
    if ($("#chksh3").attr("checked")) {
        OpenType = OpenType + ",3";
    }
    if ($("#chksh4").attr("checked")) {
        OpenType = OpenType + ",1";
    }
    if ($("#chksh5").attr("checked")) {
        OpenType = OpenType + ",2";
    }

    ListWeek.GetEvent();
    ListWeek.GetHoliday();

}

function delay(gap) {
    var then, now;
    then = new Date().getTime();
    now = then;
    while (now - then < gap) {
        now = new Date().getTime();
    }
}


function ScheduleMenu(arg) {

    if (confirm(ApplyDelete)) {
        ListWeek.DelSchedule(arg);
        return;
    }

}



ListWeek.DelSchedule = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/DelScheduleDis";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListWeek.SuccessDelSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


ListWeek.SuccessDelSchedule = function (data, status) {

    ListWeek.SettingEvent();
}
function SimpleAddSChedule(arg) {

    $("#CbtnScheDetail").hide();
    $("#CbtnScheDel").hide();
    $("#spTitle").text(AddScheduleText);

    $('#msgMail').html("<br />" + SimpleMail);
    $('#idEmail').hide();
    $('#msgMail').hide();
    $('#ddlAlarmTime').hide();
    $('#divment').hide();

    if ($('#ddlnotikind').val() == "M") {
        $('#ddlAlarmTime').show();
        $('#divment').show();
    }
    if ($('#ddlnotikind').val() == "B") {
        $('#ddlAlarmTime').show();
        $('#divment').show();
    }
    if ($('#ddlnotikind').val() == "E") {
        $('#idEmail').show();
        $('#msgMail').show();
    }

    $("#Reshort").text(''); 
    $("#chkRepeat").val('N');
    $("#tbTitle").val('');
    $("#tbstartDate").val('');
    $("#tbendDate").val('');
    document.getElementById("ddlstartTime").selectedIndex = 0;
    document.getElementById("ddlendTime").selectedIndex = 0;

    var day = ConvertDateInfo(arg);

    $("#tbstartDate").val(DateFormat(day.substring(0, 10)));
    $("#tbendDate").val($("#tbstartDate").val());
    var start = "";
    if (arg.getMinutes() == "0") {
        start = arg.getHours() + ":0" + arg.getMinutes();
    }
    else {
        start = arg.getHours() + ":" + arg.getMinutes();
    }

    $("#ddlstartTime").val(start);

    var sin = document.getElementById("ddlstartTime").selectedIndex;

    if (sin > 45) {
        document.getElementById("ddlendTime").selectedIndex = 47;
    }
    else {
        document.getElementById("ddlendTime").selectedIndex = sin + 2;
    }

    var tmpWidowHeight = $(window).height();
    var tmpScrollTop = parent.PageScrollTop;
    var tmpHeight = parent.PageScrollTop + 50;
    var tmpGap1 = parseInt(tmpWidowHeight) - parseInt(tmpScrollTop);
    var tmpGap2 = 300 - parseInt(tmpGap1) + 20;

    if (parseInt(tmpGap1) < 300) {
        tmpHeight = parent.PageScrollTop - parseInt(tmpGap2);
    }

    $("#divQuickSch").css("top", tmpHeight);
    $("#divQuickSch").css("left", (($(window).width() / 2) - ($("#divQuickSch").width() / 2)));
    $("#divQuickSch").show('scale');
}

function GetEvent() {
    ListWeek.SettingEvent();
}



function IconSetting() {
    if ($("#chksh1").attr("checked"))
        $("#lb_i").addClass("on");
    else
        $("#lb_i").removeClass("on");

    if ($("#chksh2").attr("checked"))
        $("#lb_p").addClass("on");
    else
        $("#lb_p").removeClass("on");

    if ($("#chksh3").attr("checked"))
        $("#lb_d").addClass("on");
    else
        $("#lb_d").removeClass("on");

    if ($("#chksh4").attr("checked"))
        $("#lb_s").addClass("on");
    else
        $("#lb_s").removeClass("on");

    if ($("#chksh5").attr("checked"))
        $("#lb_t").addClass("on");
    else
        $("#lb_t").removeClass("on");
}




function LoadingControl(arg) {
    return;
    if (arg == 1) {
        //뛰우기
        $("#divLoadback").show();
        $("#imgload").show();
        var x = ((document.documentElement.clientWidth - 300) / 2) + document.documentElement.scrollLeft;

        var y = ((document.documentElement.clientHeight - 50) / 2) + document.documentElement.scrollTop;
    }
    else {
        //지우기
        $("#divLoadback").hide();
        $("#imgload").hide();
    }
}



ListWeek.PasingDate = function (arg) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/ParSingWeekString";
    ParamArray = {
        'DAYINFO': arg
    };

    var obj = eval("ListWeek.SuccessPasingDate");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListWeek.SuccessPasingDate = function (data, status) {
    $('#lblDate').html(data.d)
}


/*일정 날짜로 가져오기*/

ListWeek.GetEventDay = function (arg) {
    OpenType = '';

    if ($("#chksh1").attr("checked")) {
        OpenType = OpenType + ",-1";
    }
    if ($("#chksh2").attr("checked")) {
        OpenType = OpenType + ",0";
    }
    if ($("#chksh3").attr("checked")) {
        OpenType = OpenType + ",3";
    }
    if ($("#chksh4").attr("checked")) {
        OpenType = OpenType + ",1";
    }
    if ($("#chksh5").attr("checked")) {
        OpenType = OpenType + ",2";
    }

    var scope = 'N';
    Type = "I";
    var uid = CurrentUserID;
    if (CurrentUserID > 0) {
        uid = CurrentUserID;
        OpenType = "1";
        scope = "P";
    }

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleAllKind";
    ParamArray = {
        'StartDate': arg,
        'Type': Type,
        'OpenType': OpenType,
        'DayKind': 'PD',
        'CurrentUserID': uid
    }

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessGetEventDay");
    AjaxAsyncService(url, data, obj);

}


/**************************************************
작성일자 : 2011-04-11
작성목적 : 일정목록 리스트 출력
***************************************************/
ListWeek.SuccessGetEventDay = function (data, status) {

    var strHtml = "";
    $.each(data.d, function (index, list) {
        var sColors = "<div style='float:left;background-color: " + list.StatusColors + ";width:5px;height:15px;' title='" + list.StatusColors + "'></div>";
        var sReplyCnt = (list.ReplyCnt > 0) ? "<a onclick='ListWeek.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ")' style='cursor:hand' ><span style='color:red;'> (" + list.ReplyCnt + ")</span></a>" : "";
        strHtml = strHtml + "<li>" + sColors + "<div style='float:left;padding-left:5px;width:328px;'><a onclick='ListWeek.ShowView(" + list.DisplayID + ")' style='cursor:hand' >(" + list.KindDesk + ")&nbsp;" + list.Title + "</a>" + sReplyCnt + "</div><div class='dotLine' style='clear:both;'></div></li>";
    });

    $("#divtodolist").html(strHtml);
}

function ClearPopup() {

    $("#trShare").hide();
    $("#divshare1").html('');
    $("#divshare2").html('');
    $("#divshare3").html('');
    $("#divshare4").html('');
    $("#divQuickSch").hide();
    $("#divQuickSchView").hide();
    $("#divtodo").hide();
    $("#divtodolist").html('');
    $("#tbTitle").val('');
    $("#tbstartDate").val('');
    $("#tbendDate").val('');
    document.getElementById("ddlstartTime").selectedIndex = 0;
    document.getElementById("ddlendTime").selectedIndex = 0;
    $("#hdChoiceSche").val('');
    $("#ddlnotikind").val(Notikind);
    $("#ddlAlarmTime").val(AlarmTime);
    $("#idEmail").val('');

    disablePopup();
}
///////////////////////////////////////////////////


ListWeek.GetSingleSchedule = function (arg) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleSoloInfo";
    ParamArray = {
        'CompanyID': CompanyID,
        'DisplayID': arg,
        'UserID': UserId

    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessGetSingleSchedule");

    AjaxService(url, data, obj);

}
ListWeek.SuccessGetSingleSchedule = function (data, status) {
    $("#CbtnScheDetail").show();
    $.each(data.d, function (index, list) {

        //Type
        $("#hdType").val(list.Type);

        $("#hdChoiceSche").val(list.DisplayID);
        //제목
        $("#tbTitle").val(list.Title);
        $("#tTitle").text(list.Title);
        //게시범위
        $("#ddlShare").val(list.OpenType);
        $("#tShare").text($("#ddlShare option:selected").text());
        //등록자
        $("#tWriter").text(list.UserName);
        //댓글
        if (list.ReplyCnt > 0) {
            $("#Reshort")[0].innerHTML = "<a onclick='ListWeek.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
            $("#tReshort")[0].innerHTML = "<a onclick='ListWeek.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
        }
        else {
            $("#Reshort")[0].innerHTML = "";
            $("#tReshort")[0].innerHTML = "";
        }
        //반복일정
        $("#chkRepeat").val(list.Repeat);
        //일정분류
        $("#ddlStatus").val(list.StatusID);
        if (list.StatusID == "0") {
            $("#tStatus").text("");
        }
        else {
            $("#tStatus").text($("#ddlStatus option:selected").text());
        }

        //일정시간
        var arStart = list.stringStartTime.split("@");
        var arEnd = list.stringENDTime.split("@");

        $("#tbstartDate").val(arStart[0]);
        $("#sStartDate").text(arStart[0]);

        $("#ddlstartTime").val(arStart[1]);
        $("#sStartTime").text($("#ddlstartTime").val());

        $("#tbendDate").val(arEnd[0]);
        $("#sendDate").text(arEnd[0]);

        $("#ddlendTime").val(arEnd[1]);
        $("#sEndTime").text($("#ddlendTime").val());

        CheckUserID = list.UserID;

        if (list.UserID == UserId) {
            $("#spTitle").text(ModScheduleText);
            $("#CbtnScheDel").show();          

            //알람
            if (list.Alert == "Y") {
                $("#ddlnotikind").val(list.AlertType);
                if (list.AlertType == "M") {
                    $("#ddlAlarmTime").val(list.AlertTime);
                }
                else if (list.AlertType == "B") {
                    $("#ddlAlarmTime").val(list.AlertTime);
                }
                else if (list.AlertType == "E") {
                    $("#idEmail").val(list.AlertTargetAddress);
                }
            }
            else {
                $("#ddlnotikind").val('');
                $("#ddlAlarmTime").val('');
            }

            $('#msgMail').html("<br />" + SplitMail);
            $('#idEmail').hide();
            $('#msgMail').hide();
            $('#ddlAlarmTime').hide();
            $('#divment').hide();

            if ($('#ddlnotikind').val() == "M") {
                $('#ddlAlarmTime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "B") {
                $('#ddlAlarmTime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "E") {
                $('#idEmail').show();
                $('#msgMail').show();
            }
        }

        $("#tRShareView").show();

        // 관리자가 공유한 일정은 관리자 화면에서만 삭제할 수 있도록..
        if (list.OpenType == "100") {
            CheckUserID = 0;
            $("#ddlShare").val("1");
            $("#tRShareView").hide();
        }

        //$("#trShare").show();
        $("#trShare").hide();        

    });
}

ListWeek.DetailPageMove = function () {
    var arg = $("#hdChoiceSche").val();
    var pType = $("#hdType").val();
    var d1;

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());

    }

    ParamArray = {
        'DisplayID': arg,
        'PageScope': 'ListWeek.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());
    parent.ScheduleParentMove(GetEncryptValue, pType);
}


ListWeek.DelScheduleButton = function () {
    var arg = $("#hdChoiceSche").val();
    if (confirm(ApplyDelete)) {
        ListWeek.DelSchedule(arg);
        return;
    }
}



/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 퀵 수정
***********************************************************/
ListWeek.SimpleEventModify = function () {

    if ($("#chkRepeat").val() == "Y") {
        if (!confirm(R_MsgRepeat)) { return; }
    }

    StartTime = $('#tbstartDate').val() + " " + $('#ddlstartTime').val();
    EndTime = $('#tbendDate').val() + " " + $('#ddlendTime').val();

    if ($('#ddlnotikind').val() == "M") //메신저
    {
        if ($("#ddlAlarmTime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "B") //Mobile
    {
        if ($("#ddlAlarmTime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "E") //메일
    {
        if ($.trim($('#idEmail').val()) != "") {
            if (!checkEmail($('#idEmail').val())) {
                alert(CheckMail);
                return;
            }
        }
    }

    var arg = $("#hdChoiceSche").val();
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/ModSimpleDisSchedule";
    ParamArray = {
        'DisplayID': arg,
        'StartTime': StartTime,
        'EndTime': EndTime,
        'OpenType': $("#ddlShare").val(),
        'Title': $("#tbTitle").val().replace(/&/g, '＆'),
        'StatusID': $("#ddlStatus").val(),
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddlAlarmTime").val(),
        'AlarmMail': $("#idEmail").val()
    };
    var obj = eval("ListWeek.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListWeek.GetShareInfo = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetPopShareInfo";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListWeek.SuccessGetShareInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListWeek.SuccessGetShareInfo = function (data, status) {
    var strHtml1 = "";
    var strHtml2 = "";
    var strHtml3 = "";
    var strHtml4 = "";

    $.each(data.d, function (index, list) {

        if (list.Kind == "P") {
            if (strHtml1.length > 0) strHtml1 += ", ";
            strHtml1 = strHtml1 + list.Name;
        } if (list.Kind == "D") {
            if (strHtml2.length > 0) strHtml2 += ", ";
            strHtml2 = strHtml2 + list.Name;
        } if (list.Kind == "UP") {
            if (strHtml3.length > 0) strHtml3 += ", ";
            strHtml3 = strHtml3 + list.Name;
        } if (list.Kind == "DP") {
            if (strHtml4.length > 0) strHtml4 += ", ";
            strHtml4 = strHtml4 + list.Name;
        }
    });

    if (strHtml1.length > 0) {
        strHtml1 = "* " + basicShareperson + " : " + strHtml1 + "\r\n";
    } else {
        strHtml1 = "";
    }

    if (strHtml2.length > 0) {
        strHtml2 = "* " + basicsShareDept + " : " + strHtml2 + "\r\n";
    } else {
        strHtml2 = "";
    }

    if (strHtml3.length > 0) {
        strHtml3 = "* " + OneSharePerson + " : " + strHtml3 + "\r\n";
    } else {
        strHtml3 = "";
    }

    if (strHtml4.length > 0) {
        strHtml4 = "* " + oneShareDept + " : " + strHtml4 + "\r\n";
    } else {
        strHtml4 = "";
    }

    $("#tainfo").val(strHtml1 + strHtml2 + strHtml3 + strHtml4);
}



// 할일보기 가져오기
ListWeek.GetTodoList = function (arg) {

    OpenType = '';

    if ($("#chksh1").attr("checked")) {

        OpenType = OpenType + ",-1";

    }
    if ($("#chksh2").attr("checked")) {

        OpenType = OpenType + ",0";

    }
    if ($("#chksh3").attr("checked")) {

        OpenType = OpenType + ",3";

    }
    if ($("#chksh4").attr("checked")) {

        OpenType = OpenType + ",1";

    }
    if ($("#chksh5").attr("checked")) {
        OpenType = OpenType + ",2";
    }

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ToDo.asmx/GetQuickTodoList";
    ParamArray = {
        'UserID': UserId,
        'companyID': CompanyID,
        'OpenType': OpenType,
        'Date': arg
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환  

    var obj = eval("ListWeek.SuccessTodoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
    
    /*
    if ($.browser.msie) {
        var obj = eval("ListWeek.SuccessTodoList");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {
        url = "/DeskPlusEIP/Pim/ToDo/SortTodoList.aspx?p=" + GetEncryptValue;
        PopupWin(400, 250, url, "0", "0", false);
        $("#zeoModalContact").css("top", (($(window).height() - 300) / 2) + $(window).scrollTop() + "px");
        $("#zeoModalContact").css("top", GetPostion().y + "px");
        $("#zeoModalContact").css("left", (($(window).width() - 400) / 2) + $(window).scrollLeft() + "px");
    }
    */
}
ListWeek.SuccessTodoList = function (data, status) {

    var strHtml = "";
    $.each(data.d, function (index, list) {

        var sReplyCnt = (list.ReplyCnt > 0) ? "<a onclick='ListWeek.PopupTodoShort(" + list.ToDoID + ")' style='cursor:hand' ><span style='color:red;'> (" + list.ReplyCnt + ")</span></a>" : "";            
        strHtml = strHtml + "<li>(" + list.Type + ")&nbsp;<a href='#' onclick='ListWeek.MoveTodoView(" + list.ToDoID + ")'>" + list.Title + "</a>" + sReplyCnt + "<div class=dotLine></div></li>";
    });


    $("#divtodolist").html(strHtml);

}

ListWeek.MoveTodoView = function (arg) {
    var d1;

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());

    }
    ParamArray = {
        'TodoID': arg,
        'PageScope': 'ListWeek.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());
    parent.TodoParentMove("/DeskPlusEIP/Pim/ToDO/View.aspx?p=" + GetEncryptValue);

}


ListWeek.SettingTime = function () {

    var sdate = $("#tbstartDate").val();
    var edate = $("#tbendDate").val();
    var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);

    if (!dtResponse)
        return;
    if (sdate != edate)
        return;

    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();
    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');
    var sInt = parseInt(stime, 10);
    var eInt = parseInt(etime, 10);

    if (sInt >= eInt) {
        var maxindex = $("#ddlstartTime").get(0).options.length;
        var sel = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));

        if ((sel + 2) < maxindex) {

            $("#ddlendTime option:eq(" + (sel + 2) + ")").attr("selected", "selected");
        }
        else {

            $("#ddlendTime option:eq(47)").attr("selected", "selected");
        }
    }
}





/**************************************************
작성자   : famo23
작성일자 : 2012-07-19
작성목적 : 기념일 보기 가져오기 (웹서비스 호출)
***************************************************/
ListWeek.GetHolidayList = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListMonth.asmx/GetHolidayList";
    ParamArray = {
        'StartDate': arg
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환  

    var obj = eval("ListWeek.SuccessHolidayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

    /*
    if ($.browser.msie) {
        var obj = eval("ListWeek.SuccessHolidayList");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);
    }
    else {
        url = "/DeskPlusEIP/Pim/Schedule/SortHoliDayList.aspx?p=" + GetEncryptValue;
        PopupWin(400, 250, url, "0", "0", false);
        $("#zeoModalContact").css("top", (($(window).height() - 300) / 2) + $(window).scrollTop() + "px");
        $("#zeoModalContact").css("top", GetPostion().y + "px");

        $("#zeoModalContact").css("left", (($(window).width() - 400) / 2) + $(window).scrollLeft() + "px");
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-19
작성목적 : 기념일 보기 가져오기 (웹서비스 값 받기)
***************************************************/
ListWeek.SuccessHolidayList = function (data, status) {
    var strHtml = "";
    $.each(data.d, function (index, list) {
        strHtml = strHtml + "<li>&nbsp;<a href='#' onclick='ListWeek.MoveHolidayView(\"" + list.ScheduleID + "\",\"" + list.Type + "\")'>" + list.Title + "<div class=dotLine></div></a></li>";
    });

    $("#divtodolist").html(strHtml);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-20
작성목적 : 기념일 보기화면 이동
***************************************************/
ListWeek.MoveHolidayView = function (_scheduleID, _type) {

    var d1;

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            d1 = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());

    }

    if (_type == 'J') {
        ParamArray = {
            'ScheduleID': _scheduleID,
            'PageScope': 'ListWeek.aspx',
            'Date': d1,
            'Opentype': parent.ScheduleCheck
        };

        CallEncrypt(CallGetParam());
        parent.TodoParentMove("/DeskPlusEIP/Pim/Anniversary/View.aspx?p=" + GetEncryptValue);
    }
    else {
        ParamArray = {
            'ScheduleID': _scheduleID,
            'PageScope': 'ListWeek.aspx',
            'Date': d1,
            'Opentype': parent.ScheduleCheck
        };

        CallEncrypt(CallGetParam());
        parent.TodoParentMove("/DeskPlusEIP/Pim/Admin/Holiday/View.aspx?p=" + GetEncryptValue);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-05
작성목적 : 메일 정규식 체크 
***************************************************/
function checkEmail(mailValue) {

    var arrMail = mailValue.split(';');

    for (var i = 0; i < arrMail.length; i++) {

        if ($.trim(arrMail[i]) != "") {
            var email = arrMail[i];
            var exptext = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*\.[a-zA-Z]{2,3}$/i;
            if (exptext.test(email) == false) {
                return false;
            };
        }
    }

    return true;
};


/**************************************************
작성자   : famo23
작성일자 : 2020-02-06
작성목적 : 기본 알람으로 설정 (웹서비스 호출)
***************************************************/
ListWeek.AlarmSetting = function () {

    if ($('#ddlnotikind').val() == "M") //메신저
    {
        if ($("#ddlAlarmTime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "B") //Mobile
    {
        if ($("#ddlAlarmTime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }

    ParamArray = {
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddlAlarmTime").val()
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListWeek.SuccessSetting");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Add.asmx/AlarmSetting";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-06
작성목적 : 기본 알람으로 설정 (웹서비스 값받기)
***************************************************/
ListWeek.SuccessSetting = function (data, status) {

    if (data.d == true) {
        Notikind = $('#ddlnotikind').val();
        AlarmTime = $("#ddlAlarmTime").val();
        alert(ACCOUNT_SUCCESS);
    }
    else {
        alert(MsgError);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-12-01
작성목적 : 할일 댓글 리스트 팝업
**********************************************************************************************/
ListWeek.PopupTodoShort = function (p_toDoID) {
    ParamArray = {
        'toDoID': p_toDoID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Todo/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2020-12-01
작성목적 : 일정 댓글 리스트 팝업
**********************************************************************************************/
ListWeek.PopupScheduleShort = function (p_scheduleID, p_displayID) {
    ParamArray = {
        'scheduleID': p_scheduleID,
        'displayID': p_displayID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Schedule/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}
