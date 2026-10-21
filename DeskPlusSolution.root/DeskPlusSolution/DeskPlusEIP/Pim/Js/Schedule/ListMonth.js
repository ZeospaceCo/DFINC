ListMonth = {}

var date;
var ScheduleShow = 0;

$(function () {

    $(document).ready(function () {
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
                ListMonth.ShowView(event.id);
            },
            eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
                ListMonth.EventModify(event);
            },
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                ListMonth.EventModify(event);
            },
            eventMouseover: function (calEvent, domEvent) {
                var str = calEvent.id;
                if (str.length > 2) {
                    if (str.substr(0, 2) == "T_") {
                        return;
                    }
                }
            },
            eventMouseout: function (calEvent, domEvent) {

            },
            dayClick: function (date, allDay, jsEvent, view) {
                if (ScheduleShow == 1)
                    return;

                ClearPopup();
                SimpleAddSChedule(date);
            },
            loading: function (bool) {
                //if (bool) LoadingControl(1);
                //else LoadingControl(2);
            }

        });

        // 현재일
        $('#lblDate').click(function (e) {
            e.preventDefault();
            ListMonth.Calendar_Control('T')

        });

        $("#tbTitle").keydown(function (e) {
            if (e.keyCode == 13) {
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

                    ListMonth.SimpleEventModify();
                }
                parent.ParentCallInterFace();

            }
        });


        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListMonth.Calendar_Control('<')
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListMonth.Calendar_Control('>')
        });

        $('#CbtnClosetodo').click(function (e) {
            e.preventDefault();
            $("#divtodo").hide("scale");
        });

        $('#CbtnCloseView').click(function (e) {
            e.preventDefault();
            $("#divQuickSchView").hide();
        });

        //스케줄상세페이지 이동
        $('#CbtnScheDetail').click(function (e) {
            e.preventDefault();
            ListMonth.DetailPageMove();
        });

        $('#CbtnScheDetailView').click(function (e) {
            e.preventDefault();
            ListMonth.DetailPageMove();
        });

        // 일정삭제
        $('#CbtnScheDel').click(function (e) {
            e.preventDefault();
            ListMonth.DelScheduleButton();
            parent.ParentCallInterFace();
        });

        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            ListMonth.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            ListMonth.SettingTime();
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

                ListMonth.SimpleEventModify();
            }
            parent.ParentCallInterFace();

        });

        $('#calendar').fullCalendar('gotoDate', y, m, d);
        ParentGetCheck();
        ListMonth.SettingEvent();
        ListMonth.TitleSetting();

        $("#ddlShare").val(DefaultSetting);
        if (parent.FrameHeightControl != null) {

            parent.FrameHeightControl('M');
        }

        // 도움말 오픈
        $('#HelpCategory').click(function (e) {
            e.preventDefault();
            var url = "../Help/GetCategoryInfo.aspx";
            var po = GetPostion();
            FixModalDesign(300, 350, url, '', po.y);
            //PopupWin(300, 350, url, "-320", "0", false);
        });

        //기본알람으로 설정
        $('#CbtnSetting').click(function (e) {
            e.preventDefault();
            ListMonth.AlarmSetting();
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


function ParentGetEvent() {
    ListMonth.SettingEvent();
}


/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
ListMonth.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/*
용도: 달력 컨트롤
작성자: 김광일
작성일: 2011-7-12
*/
ListMonth.Calendar_Control = function (_arg) {

    if (_arg == "<") {
        $('#calendar').fullCalendar('prev');
        ListMonth.SettingEvent();
    }
    else if (_arg == ">") {
        $('#calendar').fullCalendar('next');
        ListMonth.SettingEvent();
    }
    else if (_arg == "T") {
        $('#calendar').fullCalendar('gotoDate', NowYear, NowMonth - 1, NowDay);
        ListMonth.SettingEvent();
    }
    ListMonth.TitleSetting();
}



/*
용도: 타이틀 세팅
작성자: 김광일
작성일: 2011-7-12
*/
ListMonth.TitleSetting = function () {
    //alert(NowDate.getMonth());
    var sdate = '';

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            sdate = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1);
        else
            sdate = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1);

    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            sdate = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1);
        else
            sdate = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1);

    }


    $('#lblDate').html(DateFormat(sdate));
}


//다국어 날짜 세팅
function DateFormat(_date) {

    var arrDate = _date.split('-');
    var ParsingDate = '';
    switch (PreLang) {
        case "ko":
            //"yy-mm";
            ParsingDate = arrDate[0] + "-" + arrDate[1];
            break;
        case "en":
            //"mm/yy";
            ParsingDate = arrDate[1] + "/" + arrDate[0];
            break;
        case "en-gb":
            //"mm/yy";
            ParsingDate = arrDate[1] + "/" + arrDate[0];
            break;
        case "ja":
            //"yy/mm";
            ParsingDate = arrDate[0] + "/" + arrDate[1];
            break;
        case "fr":
            //"mm/yy";
            ParsingDate = arrDate[1] + "/" + arrDate[0];
            break;
        case "zh-cn":
            //"yy/mm";
            ParsingDate = arrDate[0] + "/" + arrDate[1];
            break;
        case "zh-tw":
            //"yy-mm";
            ParsingDate = arrDate[0] + "-" + arrDate[1];
            break;
    }
    return ParsingDate;
}

//다국어 날짜 세팅
function DateFormatDate(_date) {

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
용도 :  공휴일 가져오기
작성자: 김광일
작성일 : 2011-7-20 
*/
ListMonth.GetHoliday = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetDayInfoDB";
    ParamArray = {
        'StartDate': NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + (parseInt(NowDate.getDate()))
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessGetHoliday");

    AjaxAsyncService(url, data, obj);
}

ListMonth.SuccessGetHoliday = function (data, status) {
    $.each(data.d, function (index, list) {
        if (list.Year == NowDate.getFullYear()) {
            var did = "#day_id" + list.Month + "_" + list.Day;
            $(did).html("<a href='#' onclick='CallMoveDay(" + list.Year + ", " + (parseInt(list.Month) - 1) + ", " + list.Day + ")'><font color='red'>" + list.Day + " " + list.Title + "</font></a>");
        }
    });

    $.each(data.d, function (index, list) {
        if (list.Year == NowDate.getFullYear()) {
            if (list.Kind == 1) {
                var did = "#day_id" + list.Month + "_" + list.Day;
                var datevalue = $(did).html();

                datevalue = "<font color='red'>" + datevalue + "</font>";
                $(did).html(datevalue);
            }

            if (list.Kind == 2) {
                var did = "#day_id" + list.Month + "_" + list.Day;
                var datevalue = $(did).html();
                datevalue = "<font color='red'>" + datevalue + "</font>";
                $(did).html(datevalue);
            }

            if (list.Kind == 3) {
                var did = "#day_id" + list.Month + "_" + list.Day;
                var datevalue = $(did).html();
                datevalue = "<font color='red'>" + datevalue + "</font>";
                $(did).html(datevalue);
            }
        }
    });

    $.each(data.d, function (index, list) {

        var did = "#day_id" + list.Month + "_" + list.Day;
        var datevalue = $(did).html();

        if (list.Kind == 0) {
            var dayvalue = "";
            var Gun = "";
            if (list.Count > 1)
                Gun = "(" + list.Count + "건)";
    
            dayvalue = dayvalue + "<a href='#'  onclick='DayUrlAction(\"" + list.DateInfo + "\")'>";
            dayvalue = dayvalue + "<font align='left' style='font-size:12px;font-weight:normal;z-index: 99;position:relative' color='" + list.ColorInfo + "'>" + list.Title + Gun + "</font>";
            dayvalue = dayvalue + "</a>";
            dayvalue = dayvalue + '&nbsp;' + datevalue;
            $(did).html(dayvalue);
        }
    });

    $('#calendar').fullCalendar('callWindowResize');
}


/* util  */
function DayUrlAction(_arg) {
    ScheduleShow = 1;
    ListMonth.GetHolidayList(_arg);
}



/*이벤트가져오기*/

/*
용도 :  이벤트 가져오기
작성자: 김광일
작성일 : 2011-8-02 
*/
ListMonth.GetEvent = function () {

    StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + "-" + NowDate.getDate()
    $("#txttemp").val(StartTime + "|" + Type + "|" + OpenType + "|" + uid);

    if (uid == null) {
        uid = 0;
    }

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
        'DayKind': 'M',
        'CurrentUserID': uid
    };



    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessGetEvent");

    AjaxAsyncService(url, data, obj);
}

ListMonth.SuccessGetEvent = function (data, status) {
    var MoveTime = NowDate;
    $('#calendar').fullCalendar('destroy');

    $('#calendar').fullCalendar({
        loading: function (bool) {
        },
        events: data.d,
        header: {
            left: '',
            center: 'title',
            right: ''
        },
        editable: true, eventClick: function (event) {
            ListMonth.ShowView(event.id);
        },
        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
            ListMonth.EventModify(event);
        }, 
        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            ListMonth.EventModify(event);
        },
        eventMouseover: function (calEvent, domEvent) {
            var str = calEvent.id;
            if (str.length > 2) {
                if (str.substr(0, 2) == "T_") {
                    return;
                }
            }
        },
        eventMouseout: function (calEvent, domEvent) {
            //   $("#events-layer").remove();
        },
        dayClick: function (date, allDay, jsEvent, view) {
            if (ScheduleShow == 1)
                return;

            ClearPopup();
            SimpleAddSChedule(date);
        }

    });
    //LoadingControl(2);
    var d = MoveTime.getDate();
    var m = MoveTime.getMonth();
    var y = MoveTime.getFullYear();
    $('#calendar').fullCalendar('gotoDate', y, m, d);

    if (parent.FrameHeightControl != null) {
        var CalHei = $('#calendar').css("height");
        CalHei = CalHei.replace("px", "");
        var yHeight = parseInt(CalHei) + 100;
    }

    ListMonth.GetHoliday();
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
    var all = false;
    if (suid.length > 2) {
        if (suid.substr(0, 2) == "T_") {
            all = true;
        }
        if (suid.substr(0, 2) == "S_") {
            all = true;
        }
    }

    $('#calendar').fullCalendar('renderEvent', {
        start: ConvertDateType(sdate), end: ConvertDateType(edate),
        title: stitle,
        id: suid,
        allDay: all, editable: edit,
        backgroundColor: scolor
    }, false);
}

function AddTodo(sdate, edate, stitle, surl, suid, salldate, scolor, edit) {
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
ListMonth.ShowView = function (arg) {

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
            ListMonth.GetTodoList(arg);
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
            ListMonth.GetEventDay(arg);
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
            ListMonth.GetHolidayList(arg);
            $("#divPopTitle").html(holidaycadalog);
            $("#divtodo").css("top", tmpHeight);
            $("#divtodo").css("left", (($(window).width() / 2) - ($("#divtodo").width() / 2)));
            $("#divtodo").show('scale');
            //$("#divtodo").draggable({ disabled: false });
            return;
        }
    }

    ListMonth.GetSingleSchedule(arg);
    //ListMonth.GetShareInfo(arg);

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
ListMonth.MoveMod = function (arg) {
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
        'Page': 'ListMonth.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());

    location.href = "/DeskPlusEIP/Pim/ToDO/AddN.aspx?p=" + GetEncryptValue;
}




/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정
***********************************************************/
ListMonth.EventModify = function (event) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/ModDropSchedule";
    ParamArray = {
        'DisplayID': event.id,
        'StartTime': ConvertDateInfo(event.start),
        'EndTime': ConvertDateInfo(event.end),
        'StatusID': '-1'
    };

    var obj = eval("ListMonth.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정성공
***********************************************************/
ListMonth.SuccessEventModify = function (data, status) {
    if (!data.d)
        alert('수정실패');
    else
        delay(500);

    ListMonth.SettingEvent();
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
ListMonth.CheckSchedule = function (arg) {

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

    ListMonth.ModCheckMenu(CheckState);

    //IconSetting();

    ListMonth.SettingEvent();
}

/******************************************
작성자: kkzz52
작성일: 2017-02-15
용도: 일정분류 체크박스 개인화 저장
****************************************/
ListMonth.ModCheckMenu = function (_checkState) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListMonth.asmx/ModCheckMenu";
    ParamArray = {
        'companyID': CompanyID,
        'userId': UserId,
        'checkState': _checkState
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessCheckMenu");

    AjaxService(url, data, obj);
}

ListMonth.SuccessCheckMenu = function (data, status) {
    // alert(data.d); 성공시 true
}


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

/*
작성자: daniel
용도: 이벤트 가져오기전 세팅

*/
ListMonth.SettingEvent = function () {
    ClearPopup(); //팝업 닫기
    LoadingControl(1); //로딩 바 

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

    ListMonth.GetEvent();
}

///잠시 딜레이 주기
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
        ListMonth.DelSchedule(arg);
        return;
    }

}



ListMonth.DelSchedule = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/DelScheduleDis";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListMonth.SuccessDelSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


ListMonth.SuccessDelSchedule = function (data, status) {
    ListMonth.SettingEvent();
}


function SimpleAddSChedule(arg) {
    var d3 = new Date();

    $("#CbtnScheDetail").hide();
    $("#CbtnScheDel").hide();
    $("#spTitle").text(AddScheduleText);
    $("#trAlarm").show();
    if (CurrentUserID != 0) {
        return;
    }

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

    $("#tbstartDate").val(DateFormatDate(day.substring(0, 10)));
    $("#tbendDate").val($("#tbstartDate").val());
    var start = "";
    if (arg.getMinutes() == "0") {
        start = d3.getHours() + ":0" + arg.getMinutes();
    }
    else {
        start = d3.getHours() + ":" + arg.getMinutes();
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

    return;

}

function GetEvent() {
    ListMonth.SettingEvent();
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
    if (arg == 1) {
        //뛰우기

        $("#divLoadback").show();
        var x = ((document.documentElement.clientWidth - 300) / 2) + document.documentElement.scrollLeft;

        var y = ((document.documentElement.clientHeight - 50) / 2) + document.documentElement.scrollTop;
    }
    else {
        //지우기
        $("#divLoadback").hide();
    }
}



// 할일보기 가져오기
ListMonth.GetTodoList = function (arg) {
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

    var obj = eval("ListMonth.SuccessTodoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

    /*
    if ($.browser.msie) {
        var obj = eval("ListMonth.SuccessTodoList");
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
ListMonth.SuccessTodoList = function (data, status) {

    var strHtml = "";
    $.each(data.d, function (index, list) {

        var sReplyCnt = (list.ReplyCnt > 0) ? "<a onclick='ListMonth.PopupTodoShort(" + list.ToDoID + ")' style='cursor:hand' ><span style='color:red;'> (" + list.ReplyCnt + ")</span></a>" : "";            
        strHtml = strHtml + "<li>(" + list.Type + ")&nbsp;<a href='#' onclick='ListMonth.MoveTodoView(" + list.ToDoID + ")'>" + list.Title + "</a>" + sReplyCnt + "<div class=dotLine></div></li>";
    });


    $("#divtodolist").html(strHtml);
}


ListMonth.MoveTodoView = function (arg) {
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
        'PageScope': 'ListMonth.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());
    parent.TodoParentMove("/DeskPlusEIP/Pim/ToDO/View.aspx?p=" + GetEncryptValue);

}

/*일정 날짜로 가져오기*/

ListMonth.GetEventDay = function (arg) {
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
    var obj = eval("ListMonth.SuccessGetEventDay");
    AjaxAsyncService(url, data, obj);

    /*
    if ($.browser.msie) {
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ListMonth.SuccessGetEventDay");
        AjaxAsyncService(url, data, obj);
    }
    else {
        url = "/DeskPlusEIP/Pim/Schedule/SortScheduleList.aspx?p=" + GetEncryptValue;
        PopupWin(400, 250, url, "0", "0", false);
        $("#zeoModalContact").css("top", (($(window).height() - 300) / 2) + $(window).scrollTop() + "px");
        $("#zeoModalContact").css("top", GetPostion().y + "px");
        $("#zeoModalContact").css("left", (($(window).width() - 400) / 2) + $(window).scrollLeft() + "px");
    }
    */

} /// <reference path="../../Schedule/ListDay.aspx" />


/**************************************************
작성일자 : 2011-04-11
작성목적 : 일정목록 리스트 출력
***************************************************/
/// <reference path="../ShortReply/" />

ListMonth.SuccessGetEventDay = function (data, status) {

    var strHtml = "";

    $.each(data.d, function (index, list) {
        var sColors = "<div style='float:left;background-color: " + list.StatusColors + ";width:5px;height:15px;' title='" + list.StatusColors + "'></div>";
        var sReplyCnt = (list.ReplyCnt > 0) ? "<a onclick='ListMonth.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ")' style='cursor:hand' ><span style='color:red;'> (" + list.ReplyCnt + ")</span></a>" : "";
        strHtml = strHtml + "<li>" + sColors + "<div style='float:left;padding-left:5px;width:328px;'><a onclick='ListMonth.ShowView(" + list.DisplayID + ")' style='cursor:hand' >(" + list.KindDesk + ")&nbsp;" + list.Title + "</a>" + sReplyCnt + "</div><div class='dotLine' style='clear:both;'></div></li>";
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
//////////////////////


ListMonth.GetSingleSchedule = function (arg) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleSoloInfo";
    ParamArray = {
        'CompanyID': CompanyID,
        'DisplayID': arg,
        'UserID': UserId
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMonth.SuccessGetSingleSchedule");

    AjaxService(url, data, obj);

}
ListMonth.SuccessGetSingleSchedule = function (data, status) {
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
            $("#Reshort")[0].innerHTML = "<a onclick='ListMonth.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
            $("#tReshort")[0].innerHTML = "<a onclick='ListMonth.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
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
            $("#trAlarm").show();

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

ListMonth.DetailPageMove = function () {
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
        'PageScope': 'ListMonth.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());
    parent.ScheduleParentMove(GetEncryptValue, pType);

}

ListMonth.DelScheduleButton = function () {
    var arg = $("#hdChoiceSche").val();
    if (confirm(ApplyDelete)) {
        ListMonth.DelSchedule(arg);
        return;
    }
}



/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 퀵 수정
***********************************************************/
ListMonth.SimpleEventModify = function () {

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

    var obj = eval("ListMonth.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListMonth.GetShareInfo = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetPopShareInfo";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListMonth.SuccessGetShareInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
ListMonth.SuccessGetShareInfo = function (data, status) {
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


ListMonth.SettingTime = function () {

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
ListMonth.GetHolidayList = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListMonth.asmx/GetHolidayList";
    ParamArray = {
        'StartDate': arg
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환  

    var obj = eval("ListMonth.SuccessHolidayList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

    /*
    if ($.browser.msie) {
        var obj = eval("ListMonth.SuccessHolidayList");
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
ListMonth.SuccessHolidayList = function (data, status) {
    //$("#divtodolist").html('');
    var strHtml = "";
    $.each(data.d, function (index, list) {
        strHtml = strHtml + "<li>&nbsp;<a href='#' onclick='ListMonth.MoveHolidayView(\"" + list.ScheduleID + "\",\"" + list.Type + "\")'>" + list.Title + "<div class=dotLine></div></a></li>";
    });

    $("#divtodolist").html(strHtml);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-20
작성목적 : 기념일 보기화면 이동
***************************************************/
ListMonth.MoveHolidayView = function (_scheduleID, _type) {

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
            'PageScope': 'ListMonth.aspx',
            'Date': d1,
            'Opentype': parent.ScheduleCheck
        };

        CallEncrypt(CallGetParam());
        parent.TodoParentMove("/DeskPlusEIP/Pim/Anniversary/View.aspx?p=" + GetEncryptValue);
    }
    else {
        ParamArray = {
            'ScheduleID': _scheduleID,
            'PageScope': 'ListMonth.aspx',
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
ListMonth.AlarmSetting = function () {

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
    var obj = eval("ListMonth.SuccessSetting");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Add.asmx/AlarmSetting";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-06
작성목적 : 기본 알람으로 설정 (웹서비스 값받기)
***************************************************/
ListMonth.SuccessSetting = function (data, status) {

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
ListMonth.PopupTodoShort = function (p_toDoID) {
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
ListMonth.PopupScheduleShort = function (p_scheduleID, p_displayID) {
    ParamArray = {
        'scheduleID': p_scheduleID,
        'displayID': p_displayID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Schedule/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}
