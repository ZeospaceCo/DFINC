ListDay = {}
var date;
$(function () {

    // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
    var Pager = {
        pageIndexChanged: function (selectedPage) {
            CurrentPage = selectedPage;
            ListDay.LoadData();
        }
    }

    $(document).ready(function () {

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });

        //페이지 카운트
        $('#ddlListCount').change(function () {
            ListDay.PageSize();
        });

        date = new Date(Year, Month - 1, Day);
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        NowDate = date;
        //퀵일정저장
        $("#txtPimcontent").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                EndTime = '';
                ListDay.ValidationAdd();
                parent.ParentCallInterFace();
            }
        });

        ///엔터키 일정저장
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

                    ListDay.SimpleEventModify();
                }
                parent.ParentCallInterFace();

            }
        });


        $('#CbtnClosetodo').click(function (e) {
            e.preventDefault();
            $("#divtodo").hide("scale");
        });

        $('#CbtnCloseView').click(function (e) {
            e.preventDefault();
            $("#divQuickSchView").hide();
        });

        // 현재일
        $('#lblDate').click(function (e) {
            e.preventDefault();
            ListDay.Calendar_Control('T');
        });

        // 이전
        $('#imgLeft').click(function (e) {
            e.preventDefault();
            ListDay.Calendar_Control('<');
        });

        // 다음
        $('#imgRight').click(function (e) {
            e.preventDefault();
            ListDay.Calendar_Control('>');
        });

        // 상세
        $('#cbtnDetail').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'ScheduleID': '',
                'Page': 'ListDay.aspx',
                'Date': ''
            };

            CallEncrypt(CallGetParam());
            location.href = "AddN.aspx?p=" + GetEncryptValue;

        });

        // 일정저장
        $('#cbtnQuickadd').click(function (e) {
            e.preventDefault();
            EndTime = '';
            ListDay.ValidationAdd();
            parent.ParentCallInterFace();
        });

        // 일정삭제
        $('#CbtnScheDel').click(function (e) {
            e.preventDefault();
            ListDay.DelScheduleButton();
            parent.ParentCallInterFace();
        });

        // 상세
        $('#cbtntodoDetail').click(function (e) {
            e.preventDefault();
            location.href = "../ToDo/AddN.aspx";
        });


        $('#imghalf').hide();
        $('#imgall').show();

        // 반반
        $('#imghalf').click(function (e) {
            e.preventDefault();
            $('#imghalf').hide();
            $('#imgall').show();

            $("#divhalf1").show();
            $("#divhalf1").addClass("col-lg-6");
            $("#divhalf2").addClass("col-lg-6")
            /*
            var wid2 = $("#ifm_layout_body").width() / 2;
            wid2 = wid2 - 10;
            $("#divhalf2").width('49%');
            */
        });

        // 모두
        $('#imgall').click(function (e) {
            e.preventDefault();
            $('#imghalf').show();
            $('#imgall').hide();

            $("#divhalf1").hide();
            $("#divhalf1").removeClass("col-lg-6");
            $("#divhalf2").removeClass("col-lg-6")
            //$("#divhalf2").width('100%');
        });

        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            ListDay.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            ListDay.SettingTime();
        });


        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            var arg = $("#hdChoiceSche").val();
            if (arg.length == 0) {
                AddSimple.ValidateSchedule();
            }
            else {

                Title = jQuery.trim($('#tbTitle').val());
                Title = replaceAll(Title, "+", "＋");
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

                ListDay.SimpleEventModify();
            }
            parent.ParentCallInterFace();
        });

        //할일저장
        $('#cbtntodoadd').click(function (e) {
            e.preventDefault();
            ListDay.TodoAdd();
        });

        //퀵할일저장
        $("#txttodo").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListDay.TodoAdd();
            }
        });


        //스케줄상세페이지 이동
        $('#CbtnScheDetail').click(function (e) {
            e.preventDefault();
            ListDay.DetailPageMove();
        });

        $('#CbtnScheDetailView').click(function (e) {
            e.preventDefault();
            ListDay.DetailPageMove();
        });

        //카렌다 로딩
        $('#calendar').fullCalendar({
            header: {
                left: '',
                center: 'title',
                right: ''
            },
            editable: true, eventClick: function (event) {
                ShowView(event.id);
            },
            eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
                ListDay.EventModify(event);
            },
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
                ListDay.EventModify(event);
            },
            eventMouseover: function (calEvent, domEvent) {

            },
            eventMouseout: function (calEvent, domEvent) {

            },
            dayClick: function (date, allDay, jsEvent, view) {
                SimpleAddSChedule(date);
            }
        });



        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            ListDay.AddShortReply();
        });


        //카렌다 초기 날짜 이동
        $('#calendar').fullCalendar('gotoDate', y, m, d);

        ParentGetCheck();

        //날짜 제목 세팅
        ListDay.TitleSetting();
        //이벤트 세팅
        ListDay.SettingEvent();
        //할일 세팅 
        ListDay.GetTodoList();
        // 댓글 리스트
        //ListDay.LoadData();
        //ListDay.Resource();

        //일간 보기 설정 
        $('#calendar').fullCalendar('changeView', 'agendaDay');

        //아이프레임 부모페이지 높이 조절
        if (parent.FrameHeightControl != null) {
            var yHeight = 1500;
            parent.FrameHeightControl('D');

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
            ListDay.AlarmSetting();
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


//부모 페이지에서 자식 페이지 호출
function ParentGetEvent() {

    ListDay.SettingEvent();
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 할일 수정
***************************************************/
ListDay.AddT = function (arg) {

    if (CurrentUserID != 0) {
        return;
    }

    ClearPopup();

    ParamArray = {
        'TodoID': arg
    }
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Pim/ToDo/AddSimple.aspx?p=" + GetEncryptValue;

    //var po = GetPostion();
    //FixModalDesign(520, 245, url, '', po.y);

    var tmpWidowHeight = $(window).height();
    var tmpScrollTop = parent.PageScrollTop;
    var tmpHeight = parent.PageScrollTop + 50;
    var tmpGap1 = parseInt(tmpWidowHeight) - parseInt(tmpScrollTop);
    var tmpGap2 = 300 - parseInt(tmpGap1) + 20;

    if (parseInt(tmpGap1) < 300) {
        tmpHeight = parent.PageScrollTop - parseInt(tmpGap2);
    }

    FixModalDesign(520, 250, url, '', tmpHeight);
}


/**************************************************
작성자   : freeman
작성일자 : 2011-04-11
작성목적 : 관리자 도구
***************************************************/
ListDay.Admin = function (_arg) {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}


/*
용도: 달력 컨트롤
작성자: 김광일
작성일: 2011-7-12
*/
ListDay.Calendar_Control = function (_arg) {

    if (_arg == "<") {
        $('#calendar').fullCalendar('prev');
        ListDay.SettingEvent();
        ListDay.GetTodoList();
    }
    else if (_arg == ">") {
        $('#calendar').fullCalendar('next');
        ListDay.SettingEvent();
        ListDay.GetTodoList();
    }
    else if (_arg == "T") {
        $('#calendar').fullCalendar('gotoDate', NowYear, NowMonth - 1, NowDay);
        ListDay.SettingEvent();
        ListDay.GetTodoList();


    }
    ListDay.TitleSetting();
}


/*
용도: 날짜 관리
작성자: 김광일
작성일: 2011-7-12
*/
ListDay.DateControl = function () {
    ListDay.SettingEvent();
}

/*
용도: 타이틀 세팅
작성자: 김광일
작성일: 2011-7-12
*/
ListDay.TitleSetting = function () {
    var arWeek = WeekSort.split(',');
    var sdate ='';
    
    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            sdate=NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            sdate=NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
           sdate =NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
           sdate =NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    
    $('#lblDate').html(DateFormat(sdate)+" (" + arWeek[NowDate.getDay()] + ")");
}

//다국어 날짜 세팅
function DateFormat(_date) {
    var  arrDate = _date.split('-');    
    var  ParsingDate ='';
    switch(PreLang)
    {
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
ListDay.GetHoliday = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetDayInfoDB";
    ParamArray = {
        'year': NowDate.getFullYear()
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessGetHoliday");

    AjaxService(url, data, obj);
}


/*
용도 :  공휴일 가져오기 성공
작성자: 김광일
작성일 : 2011-11-22 
*/
ListDay.SuccessGetHoliday = function (data, status) {

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

    $.each(data.d, function (index, list) {
        if (d1 == list.DateInfo) {
            $("#thday0").css("color", "red");
        }
    });
}


/*
용도 :  이벤트 가져오기
작성자: 김광일
작성일 : 2011-8-02 
*/
ListDay.GetEvent = function () {
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
        'DayKind': 'D',
        'CurrentUserID': uid,
        'scope': scope

    }
    
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessGetEvent");

    AjaxService(url, data, obj);
}

/*
용도 :  이벤트 가져오기 성공
작성자: 김광일
작성일 : 2011-7-20 
*/
ListDay.SuccessGetEvent = function (data, status) {
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
            ShowView(event.id);
        },
        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {
            ListDay.EventModify(event);
        }, 
        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            ListDay.EventModify(event);
        },
        eventMouseover: function (calEvent, domEvent) {

        },
        eventMouseout: function (calEvent, domEvent) {
            $("#divShareInfo").hide();
        },
        dayClick: function (date, allDay, jsEvent, view) {
            $("#divShareInfo").hide();
            SimpleAddSChedule(date);
        }
    });

    var d = MoveTime.getDate();
    var m = MoveTime.getMonth();
    var y = MoveTime.getFullYear();
    $('#calendar').fullCalendar('gotoDate', y, m, d);

    $('#calendar').fullCalendar('option', 'contentHeight', 600);
 
}

/*
용도 :  날짜 타입 2011-12-10 바꾸기
작성자: 김광일
작성일 : 2011-11-22 
*/
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

/*
용도 :  일정 입력
작성자: 김광일
작성일 : 2011-11-22
*/
function AddEvent(sdate, edate, stitle, surl, suid, salldate, scolor, edit) {
    var all = false;
    if (suid.length > 2) {
        if (suid.substr(0, 2) == "T_") {
            all = false;
        }
        if (suid.substr(0, 2) == "S_") {
            all = false;
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

/*
용도 :  일정 입력 유효성검사
작성자: 김광일
작성일 : 2011-11-22
*/
ListDay.ValidationAdd = function () {

    Type = 'I,G';

    OpenType = DefaultSetting;

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            StartTime = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    StartTime += " " + $('#ddltime').val();

    var validator = new Validator($("#form1"));



    //제목    
    validator.checkRequired("txtPimcontent", AddPimPlz, true);
    if (!validator.validate()) return;   

    Title = $("#txtPimcontent").val();
    
    ListDay.ScheduleQuickADD();
}


/*
작성자: 김광일
용도 : 일정추가
작성일: 2011년8월30일
*/
ListDay.ScheduleQuickADD = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/AddSchedule";
    
    ParamArray = {
        'Type': Type,
        'Title': Title.replace(/&/g, '＆'),
        'Content': Content,
        'StartTime': StartTime,
        'EndTime': EndTime,
        'OpenType': OpenType,
        'Lunar': Lunar,
        'StatusID': $("#ddlStatus").val()
    };

    var obj = eval("ListDay.SuccessQuickAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListDay.SuccessQuickAdd = function (data, status) {
    $('#txtPimcontent').val('');
    ListDay.SettingEvent();
}


/*
작성자:김광일
용도: 체크 박스
날짜:2011/8/30
*/
ListDay.CheckSchedule = function (arg) {

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

    ListDay.ModCheckMenu(CheckState);
    ListDay.SettingEvent();
    ListDay.GetTodoList();
}

/******************************************
작성자: kkzz52
작성일: 2017-02-15
용도: 일정분류 체크박스 개인화 저장
****************************************/
ListDay.ModCheckMenu = function (_checkState) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListDay.asmx/ModCheckMenu";
    ParamArray = {
        'companyID': CompanyID,
        'userId': UserId,
        'checkState': _checkState
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessCheckMenu");

    AjaxService(url, data, obj);
}

ListDay.SuccessCheckMenu = function (data, status) {
    // alert(data.d); 성공시 true
}


/*
작성자: daniel
용도: 검색전 세팅

*/
ListDay.SettingEvent = function () {
    ClearPopup();

    $('#calendar').fullCalendar('removeEvents', null);
    OpenType = "";

    $('#hdEvent').val('');

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

    ListDay.GetEvent();
    ListDay.GetHoliday(); //공휴일표시
}

/*
작성자: daniel
용도: 보기페이지 이동

*/
ShowView = function (arg) {

    ClearPopup();

    var tmpWidowHeight = $(window).height();
    var tmpScrollTop = parent.PageScrollTop;
    var tmpHeight = parent.PageScrollTop + 50;
    var tmpGap1 = parseInt(tmpWidowHeight) - parseInt(tmpScrollTop);
    var tmpGap2 = 300 - parseInt(tmpGap1) + 20;

    if (parseInt(tmpGap1) < 300) {
        tmpHeight = parent.PageScrollTop - parseInt(tmpGap2);
    }

    var strAarg = arg.toString();

    if (strAarg.substr(0, 2) == "S_") {

        $("#divtodolist").html('');
        ListDay.GetEventDay(arg);
        $("#divPopTitle").html(schedulecadalog);

        $("#divtodo").css("top", tmpHeight);
        $("#divtodo").css("left", (($(window).width() / 2) - ($("#divtodo").width() / 2)));

        $("#divtodo").show();
        return;
    }

    ListDay.GetSingleSchedule(arg);
    //ListDay.GetShareInfo(arg);

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


/*
작성자: 김광일
용도: 할일 리스트
작성일 2011/9/1
*/
ListDay.GetTodoList = function () {

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            StartTime = NowDate.getFullYear() + "-0" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-0' + parseInt(NowDate.getDate());
        else
            StartTime = NowDate.getFullYear() + "-" + (parseInt(NowDate.getMonth()) + 1) + '-' + parseInt(NowDate.getDate());
    }

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
        'UserID': CurrentUserID,
        'companyID': CompanyID,
        'OpenType': OpenType,
        'Date': StartTime
    };

    var obj = eval("ListDay.SuccessTodoList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/*
작성자:김광일
용도:  할일 가져오기 성공
날짜: 2011-11-22

*/
ListDay.SuccessTodoList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Pim/Jt/Todo/JTTodoList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    $(".spdel").text(Del1);

    if ($(".IconNotBegin") != undefined) { $(".IconNotBegin").attr('src', ImagePath + '/Pims/Status_NotBegin.gif'); }
    if ($(".Status_Going") != undefined) { $(".Status_Going").attr('src', ImagePath + '/Pims/Status_Going.gif'); }
    if ($(".Status_Done") != undefined) { $(".Status_Done").attr('src', ImagePath + '/Pims/Status_Done.gif'); }
    if ($(".Status_Cancel") != undefined) { $(".Status_Cancel").attr('src', ImagePath + '/Pims/Status_Cancel.gif'); }
    if ($(".Status_Pass") != undefined) { $(".Status_Pass").attr('src', ImagePath + '/Pims/Status_Pass.gif'); }
    if ($(".Status_Postpone") != undefined) { $(".Status_Postpone").attr('src', ImagePath + '/Pims/Status_Postpone.gif'); }

    $.each(data.d, function (index, list) {
        var contodoid = "#aPri" + list.ToDoID;
        var contodoid2 = "#aTodoMenu" + list.ToDoID;

        //if(list.UserID
        if (UserId == list.UserID) {
            $(contodoid).contextMenu({
                menu: 'divpri',
                leftButton: true
            },
            function (action, el, pos) {
                ListDay.ContextMenu2(action, list.ToDoID);
            });

            $(contodoid2).contextMenu({
                menu: 'divTodoContext',
                leftButton: true
            },
            function (action, el, pos) {
                ListDay.ContextMenu(action, list.ToDoID);
            });
        }
    });



}

/*
작성자:김광일
용도:  할일 입력
날짜: 2011-11-22

*/
ListDay.TodoAdd = function () {

    var validator = new Validator($("#form1"));
    //제목    
    validator.checkRequired("txttodo", Add_InputTitle, true);
    if (!validator.validate()) return;


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

    t_type = 'I';
    t_status = 'W';
    t_title = $('#txttodo').val();
    t_title = replaceAll(t_title, "+", "＋");
    t_startdate = d1;
    t_Enddate = '';

    t_Priority = $('#ddltodo').val();
    t_ScheduleYN = 'N';
    t_ReservationID = '0';


    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ToDo.asmx/AddTodo";
    ParamArray = {
        'Type': t_type,
        'Status': t_status,
        'Title': t_title.replace(/&/g, '＆'),
        'Content': Content,
        'StartDate': t_startdate,
        'EndTime': '',
        'Priority': t_Priority,
        'OpenType': DefaultSetting
    };

    var obj = eval("ListDay.SuccessTodo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

ListDay.SuccessTodo = function (data, status) {

    $('#txttodo').val('');
    ListDay.GetTodoList();

}


/***********************************************************
작성자:김광일
작성일:2011/9/5
용도: 할일 삭제 
***********************************************************/
ListDay.TodoDel = function (todoid) {

    if (CurrentUserID != 0) {
        alert('본인 일정이 아닙니다.');
        return;
    }

    if (!confirm(ApplyDelete)) {
        return;
    }

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ToDo.asmx/DelTodo";
    ParamArray = {
        'CompnayID': CompanyID,
        'TodoID': todoid
    };

    var obj = eval("ListDay.TodoDelSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/***********************************************************
작성자:김광일
작성일:2011/9/5
용도: 할일 삭제성공
***********************************************************/
ListDay.TodoDelSuccess = function (data, status) {

    ListDay.GetTodoList();
}


/***********************************************************
작성자:김광일
작성일:2011/9/5
용도: 스케줄 수정
***********************************************************/
ListDay.MoveMod = function (arg) {
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
        'PageScope': 'ListDay.aspx',
        'Date': d1,
        'Opentype': OpenType
    };

    CallEncrypt(CallGetParam());

    parent.TodoParentMove("/DeskPlusEIP/Pim/ToDO/AddN.aspx?p=" + GetEncryptValue);
}

ListDay.MoveTodoView = function (arg) {
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
        'PageScope': 'ListDay.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());
    parent.TodoParentMove("/DeskPlusEIP/Pim/ToDO/View.aspx?p=" + GetEncryptValue);
}


/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정
***********************************************************/
ListDay.EventModify = function (event) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/ModDropSchedule";
    ParamArray = {
        'DisplayID': event.id,
        'StartTime': ConvertDateInfo(event.start),
        'EndTime': ConvertDateInfo(event.end),
        'StatusID': '-1'
    };

    var obj = eval("ListDay.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 시간수정성공
***********************************************************/
ListDay.SuccessEventModify = function (data, status) {

    if (!data.d)
        alert('수정실패');
    else
        delay(500);

    ListDay.SettingEvent();
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
용도:  일정 입력시 딜레이
날짜: 2011-11-22

*/
function delay(gap) {
    var then, now;
    then = new Date().getTime();
    now = then;
    while (now - then < gap) {
        now = new Date().getTime();
    }
}


/*
작성자:김광일
용도:  할일 삭제 미사용
날짜: 2011-11-22

*/
function ScheduleMenu(arg) {

    if (confirm(ApplyDelete)) {
        ListDay.DelSchedule(arg);
        return;
    }

}

/*
작성자:김광일
용도:  일정삭제
날짜: 2011-11-22

*/
ListDay.DelSchedule = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/DelScheduleDis";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListDay.SuccessDelSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*
작성자:김광일
용도:  일정삭제 성공
날짜: 2011-11-22

*/
ListDay.SuccessDelSchedule = function (data, status) {

    ListDay.SettingEvent();
}


/*
작성자:김광일
용도:  일정팝업에서 일정추가
날짜: 2011-11-22
*/
function SimpleAddSChedule(arg) {

    ClearPopup();

    $("#CbtnScheDetail").hide();
    $("#CbtnScheDel").hide();
    $("#spTitle").text(AddScheduleText);

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

    setTimeout('jQuery("#tbTitle").focus()', 500);
}


function GetEvent() {
    ListDay.SettingEvent();
}



/*
작성자:김광일
용도:  일정 종류 아이콘 설정
날짜: 2011-11-22

*/
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



/*
작성자:김광일
용도:  할일 팝업 뛰우기 상중하
날짜: 2011-11-22

*/
ListDay.ContextCall = function (arg) {

    ListDay.AdjustControl();
    $("#txtTodid").val(arg);
    $("#divCont").css("top", window.event.y + 50);
    $("#divCont").css("left", window.event.x);
    $("#divpri").hide();
    $("#divCont").show();

}

/*****************************************************/


/*
작성자:김광일
용도:  할일 팝업 닫기
날짜: 2011-11-22

*/
ListDay.ContextClose = function () {


    $("#divCont").hide();
    $("#divpri").hide();
    $("#txtTodid").val('');

}
/******************************/

/*
작성자:김광일
용도:  할일 팝업 초기화
날짜: 2011-11-22

*/
ListDay.AdjustControl = function () {
    $("#divCont").width(120);
    $("#divcon8").hide();
    $("#divcon9").hide();
    $("#divcon11").hide();
    $("#txttododate8").val('');
    $("#txttododate9").val('');
    $("#txttododate11").val('');

}

/*
작성자:김광일
용도:  할일 팝업 기능
날짜: 2011-11-22

*/
ListDay.ContextMenu = function (arg, todoarg) {

    ListDay.AdjustControl();
    t_Scope = "0";
    t_DateInfo = "";
    t_status = "";
    t_Priority = "";
    todoid = todoarg;


    if (arg == 1) { //시작안함
        t_status = "W";
        t_Scope = "0";
        ListDay.ModTodo();
    }

    if (arg == 2) { //진행중
        t_status = "I";
        t_Scope = "0";
        ListDay.ModTodo();
    }
    if (arg == 3) { //완료
        t_status = "E";
        t_Scope = "0";
        ListDay.ModTodo();
    }
    if (arg == 4) { //취소
        t_status = "C";
        t_Scope = "0";
        ListDay.ModTodo();
    }
    if (arg == 5) { //위임
        t_status = "T";
        t_Scope = "0";
        ListDay.ModTodo();
    }
    if (arg == 6) { //연기
        t_status = "D";
        t_Scope = "0";
        ListDay.ModTodo();
    }

    if (arg == 7) { // 내일할일
        t_Scope = "1";
        ListDay.ModTodo();
    }

    if (arg == 8) {  ///특정일까지할일
        t_Scope = "2";

        var url = "/DeskPlusEIP/Pim/Schedule/PopCalendar.aspx";
        //PopupWin(300, 250, url, "", "", false);
        var po = GetPostion();
        FixModalDesign(300, 250, url, '', po.y - 100);
        
    }

    if (arg == 9) {//특정일에 할일
        t_Scope = "3";
        var url = "/DeskPlusEIP/Pim/Schedule/PopCalendar.aspx";
        //PopupWin(300, 250, url, "", "", true);
        var po = GetPostion();
        FixModalDesign(300, 250, url, '', po.y - 150);     
    }

    if (arg == 10) {//내일로 연기
        t_Scope = "4";
        ListDay.ModTodo();
    }

    if (arg == 11) {//특정일 연기
        t_Scope = "5";
        var url = "/DeskPlusEIP/Pim/Schedule/PopCalendar.aspx";
        //PopupWin(300, 250, url, "-100", "-200", true);
        var po = GetPostion();
        FixModalDesign(300, 250, url, '', po.y - 180);
    }
    if (arg == 12) {//일정에        
        t_Scope = "7";
        var url = "/DeskPlusEIP/Pim/Schedule/PopPimAdd.aspx";
        //PopupWin(300, 250, url, "-100", "-200", true);
        var po = GetPostion();
        FixModalDesign(300, 250, url, '', po.y - 200);
    }
}

/*
작성자:김광일
용도:  할일 팝업 기능 2
날짜: 2011-11-22

*/
ListDay.ContextMenu2 = function (action, arg) {

    var sel = "";
    if (action == "top")
        sel = "1";
    if (action == "middel")
        sel = "2";
    if (action == "bottom")
        sel = "3";

    todoid = arg;
    t_Scope = "0";
    t_DateInfo = "";
    t_status = "";
    t_Priority = sel;
    ListDay.ModTodo();
}




/********************************
작성자:김광일
작성일: 2011-9-6
용도:할일수정
********************************/
ListDay.ModTodo = function () {
    //var todoid = $("#txtTodid").val();

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Todo.asmx/ModTodo";
    ParamArray = {
        'CompnayID': CompanyID,
        'TodoID': todoid,
        'Title': '',
        'SCOPE': t_Scope,
        'DateInfo': t_DateInfo,
        'Priority': t_Priority,
        'Status': t_status,
        'OpenType': '-1'
    };
    

    var obj = eval("ListDay.SuccessMod");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*
작성자:김광일
용도:  할일 수정 성공
날짜: 2011-11-22

*/
ListDay.SuccessMod = function (data, status) {
    ListDay.AdjustControl();
    ListDay.ContextClose();
    ListDay.GetTodoList();
    ListDay.SettingEvent();
}


ListDay.SetPara = function () {
    var chk = 0;
    if (t_Scope == 2) {  //특정일까지 할일
        chk = 1;
    }
    else if (t_Scope == 3) { //특정일에할일
        chk = 1;
    }
    else if (t_Scope == 4) {  //내일로연기     
        chk = 0;
    }
    else if (t_Scope == 5) {  //특정일로연기     
        chk = 1;
    }

    if (chk != 0) {
        if ($("#txtDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
    }


    ListDay.ModTodo();
}

/*
작성자:김광일
용도:  할일 수정
날짜: 2011-11-22

*/
ListDay.DateTodoMod = function (arg) {
    if (arg == "1") {//특정일까지할일
        t_Scope = "2";
        //t_DateInfo = $("#txttododate8").val();
    }
    if (arg == "2") {//특정일에할일
        t_Scope = "3";
        t_DateInfo = $("#txttododate9").val();
    }
    if (arg == "3") {//특정일연기
        t_Scope = "5";
        t_DateInfo = $("#txttododate11").val();
    }
    ListDay.ModTodo();
}


/*
작성자:김광일
용도:  일정가져오기
날짜: 2011-11-22

*/
ListDay.GetEventDay = function (arg) {

    OpenType = '';
    $("#divtodolist").html('')

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
        'DayKind': 'PDL',
        'CurrentUserID': uid
    }

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessGetEventDay");
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성일자 : 2011-04-11
작성목적 : 일정목록 리스트 출력
***************************************************/
ListDay.SuccessGetEventDay = function (data, status) {

    var strHtml = "";
    $.each(data.d, function (index, list) {
        var sColors = "<div style='float:left;background-color: " + list.StatusColors + ";width:5px;height:15px;' title='" + list.StatusColors + "'></div>";
        var sReplyCnt = (list.ReplyCnt > 0) ? "<a onclick='ListDay.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ")' style='cursor:hand' ><span style='color:red;'> (" + list.ReplyCnt + ")</span></a>" : "";
        strHtml = strHtml + "<li>" + sColors + "<div style='float:left;padding-left:5px;width:328px;'><a onclick='ShowView(" + list.DisplayID + ")' style='cursor:hand' >(" + list.KindDesk + ")&nbsp;" + list.Title + "</a>" + sReplyCnt + "</div><div class='dotLine' style='clear:both;'></div></li>";
    });

    $("#divtodolist").html(strHtml);
}

/*
작성자:김광일
용도:  팝업 모두 닫기
날짜: 2011-11-22

*/
function ClearPopup() {
    $("#trShare").hide();
    $("#divshare1").html('');
    $("#divshare2").html('');
    $("#divshare3").html('');
    $("#divshare4").html('');
    $("#divpri").hide();
    $("#divCont").hide();
    $("#divQuickSch").hide();
    $("#divQuickSchView").hide();
    $("#divtodo").hide();
    $("#divtodolist").html('');
    $("#tbTitle").val('');
    $("#tbstartDate").val('');
    $("#tbendDate").val('');
    $("#Reshort").text(''); 
    $("#chkRepeat").val('N');
    $("#hdChoiceSche").val('');
    $("#ddlnotikind").val(Notikind);
    $("#ddlAlarmTime").val(AlarmTime);
    $("#idEmail").val('');
    
    disablePopup();
    document.getElementById("ddlstartTime").selectedIndex = 0;
    document.getElementById("ddlendTime").selectedIndex = 0;
}


/*
작성자:김광일
용도:  일정 한개 가져오기
날짜: 2011-11-22

*/
ListDay.GetSingleSchedule = function (arg) {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetScheduleSoloInfo";
    ParamArray = {
        'CompanyID': CompanyID,
        'DisplayID': arg,
        'UserID': UserId
    }

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessGetSingleSchedule");

    AjaxService(url, data, obj);
}

/*
작성자:김광일
용도:  일정정보 가져오기 성공
날짜: 2011-11-22
*/
ListDay.SuccessGetSingleSchedule = function (data, status) {
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
            $("#Reshort")[0].innerHTML = "<a onclick='ListDay.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
            $("#tReshort")[0].innerHTML = "<a onclick='ListDay.PopupScheduleShort(" + list.ScheduleID + "," + list.DisplayID + ");' style='cursor:pointer'><span class='icon_Comment_view'>(" + list.ReplyCnt + ")</span></a>";
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

/*
작성자:김광일
용도:  상세 페이지 이동
날짜: 2011-11-22

*/
ListDay.DetailPageMove = function () {
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
        'PageScope': 'ListDay.aspx',
        'Date': d1,
        'Opentype': parent.ScheduleCheck
    };

    CallEncrypt(CallGetParam());

    parent.ScheduleParentMove(GetEncryptValue, pType);
}

/*
작성자:김광일
용도: 일정 삭제 버튼
날짜: 2011-11-22

*/
ListDay.DelScheduleButton = function () {
    var arg = $("#hdChoiceSche").val();
    if (confirm(ApplyDelete)) {
        ListDay.DelSchedule(arg);
        return;
    }

}


/***********************************************************
작성자:김광일
작성일:2011/9/7
용도: 이벤트 퀵 수정
***********************************************************/
ListDay.SimpleEventModify = function () {

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
    $("#tbTitle").val(replaceAll($("#tbTitle").val(), "+", "＋"));

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

    var obj = eval("ListDay.SuccessEventModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

function replaceAll(str, searchStr, replaceStr) {
    return str.split(searchStr).join(replaceStr);
}

/*
작성자:김광일
용도:  공유정보가져오기
날짜: 2011-11-22

*/
ListDay.GetShareInfo = function (arg) {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Schedule.asmx/GetPopShareInfo";
    ParamArray = {
        'DisplayID': arg,
        'CompanyID': CompanyID
    };
    var obj = eval("ListDay.SuccessGetShareInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*
작성자:김광일
용도:  일정공유정보 성공
날짜: 2011-11-22

*/
ListDay.SuccessGetShareInfo = function (data, status) {
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

ListDay.SettingTime = function () {

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


function ShowTime() {

    if ($("#spAcc").text() == "▲") {
        $("#spAcc").text('▼');


        for (var i = 0; i < 48; i++) {
            $("#trMin" + i).show();
            $("#divMin" + i).show();
        }
    }
    else {
        $("#spAcc").text('▲');

        for (var i = 0; i < 18; i++) {
            $("#trMin" + i).hide();
            $("#divMin" + i).hide();
        }
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 초기화
***************************************************/
ListDay.LoadData = function () {

    ListDay.GetCount(); //검색 총 카운트 가져오기    
    
    var pageCount = $("#pager").pager.refresh(TotalItemCount, RecordPerPage);
    $("#pageInfo").text(CurrentPage + " / " + pageCount);

    
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        //ListDay.GetShortReplyList(); //검색 리스트 가져오기

        // 페이저의 이벤트와 관련된 콜백 메서드들을 정의한다
        var Pager = {
            pageIndexChanged: function (selectedPage) {
                CurrentPage = selectedPage;
                ListDay.LoadData();
            }
        }

        // 페이저 초기화
        // 실제 데이터를 불러오기에 앞서 "반드시" 먼저 초기화를 해줘야 한다.
        $("#pager").pager({
            pager: "#pager",
            pagerMode: "text", //text, image 중 선택
            totalItemCount: TotalItemCount,
            recordPerPage: RecordPerPage,
            currentPage: CurrentPage,
            pageIndexChanged: Pager.pageIndexChanged,
            prev10Text: R_Previous,
            next10Text: R_Next,
            prev10ImgSrc: "images/Prev10.gif",
            next10ImgSrc: "images/next10.gif",
            selectedPageFontColor: "orange"
        });
    }

    setTimeout(function () {
        loadBoardSearchData();
    }, 1);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
ListDay.GetCount = function () {
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/ListDay.asmx/GetCount";

    // 월 2자리 만들기
    if(Month.length == 1)
    {
        Month = "0" + Month;
    }

    // 일 2자리 만들기
    if(Day.length == 1)
    {
        Day = "0" + Day;
    }
    
    ParamArray = {
        'nowDate': Year + Month + Day
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListDay.SuccessCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
ListDay.SuccessCount = function (data, status) {
    TotalItemCount = data.d;
}

/**************************************************
작성자   : shine
작성일자 : 2011-04-11
작성목적 : 페이지 사이즈에 이동하기
***************************************************/
ListDay.PageSize = function () {

    CurrentPage = 1; //1페이지로 이동처리  
    RecordPerPage = $('#ddlListCount').val();
    ListDay.LoadData();
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 다국어
**********************************************************************************************/
ListDay.Resource = function () {
    
    $("#j_Name").text(R_Name);
    $("#j_Content").text(R_Contents);

    if ($(".NoListData") != undefined) {
        $(".NoListData").text(R_TextEmpty);
    }

    if ($(".BtnDel") != undefined) {
        $(".BtnDel").text(R_Del);
    }
}

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 댓글 쓰기 팝업(본인이 본인에게만 쓴다)
**********************************************************************************************/
ListDay.AddShortReply = function () {

    if ((parseInt(NowDate.getMonth()) + 1) < 10) {
        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "0" + (parseInt(NowDate.getMonth()) + 1) + '0' + parseInt(NowDate.getDate());
        else
            iStartTime = NowDate.getFullYear() + "0" + (parseInt(NowDate.getMonth()) + 1) + '' + parseInt(NowDate.getDate());
    }
    else {

        if (parseInt(NowDate.getDate()) < 10)
            StartTime = NowDate.getFullYear() + "" + (parseInt(NowDate.getMonth()) + 1) + '0' + parseInt(NowDate.getDate());
        else
            StartTime = NowDate.getFullYear() + "" + (parseInt(NowDate.getMonth()) + 1) + '' + parseInt(NowDate.getDate());
    }
    
   
    ParamArray = {
        'nowDate': StartTime,
        'rUserID': RUserID      // 나에게 쓰기
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx?p=" + GetEncryptValue;
    PopupWin(600, 500, url, "", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
ListDay.DelReple = function (replyID, groupID) {

    if (confirm(R_ReallyApplyDelete)) {

        if (replyID == groupID) {

            ParamArray = {
                'groupID': groupID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListDay.SuccessDelShortReply");
            var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyList.asmx/DelGroupShortReply";
            AjaxService(url, data, obj);
        }
        else {
            ParamArray = {
                'replyID': replyID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("ListDay.SuccessDelShortReply");
            var url = "/DeskPlusEIP/Pim/Ws/ShortReply/ShortReplyList.asmx/DelShortReply";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
ListDay.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        alert(R_Processing);
        CurrentPage = 1; //1페이지로 이동처리  
        ListDay.LoadData();
    }
    else
        alert(R_MsgError);
}




/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-20
작성목적 : 댓글 그룹 보기 화면으로 이동
**********************************************************************************************/
ListDay.goGroup = function (_groupID) {

    ParamArray = {
        'rUserID': RUserID,
        'groupID': _groupID
    };


    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyGroup.aspx?p=" + GetEncryptValue;
    PopupWin(600, 500, url, "", "", true);
}


/**********************************************************************************************
작성자   : famo23  
작성일자 : 2012-12-12
작성목적 : 상대방 담벼락가기
**********************************************************************************************/
ListDay.goUserList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx?p=" + GetEncryptValue;
    PopupWin(600, 500, url, "", "", true);
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
ListDay.AlarmSetting = function () {

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
    var obj = eval("ListDay.SuccessSetting");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Add.asmx/AlarmSetting";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-06
작성목적 : 기본 알람으로 설정 (웹서비스 값받기)
***************************************************/
ListDay.SuccessSetting = function (data, status) {

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
작성일자 : 2020-11-30
작성목적 : 할일 댓글 리스트 팝업
**********************************************************************************************/
ListDay.PopupTodoShort = function (p_toDoID) {
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
ListDay.PopupScheduleShort = function (p_scheduleID, p_displayID) {
    ParamArray = {
        'scheduleID': p_scheduleID,
        'displayID': p_displayID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/Schedule/ViewShortReplyList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, "", "ViewShortReplyList" + Guids(), "600", "690", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');

}
