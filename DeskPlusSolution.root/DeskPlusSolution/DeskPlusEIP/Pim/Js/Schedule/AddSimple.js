AddSimple = {}

$(function () {
    $(document).ready(function () {
        

//        //확인
//        $('#CbtnConfirm').click(function (e) {
//            e.preventDefault();
//            //AddSimple.Confirm();
//            AddSimple.ValidateSchedule();
//        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            $("#divQuickSch").hide();
            
            
        });
        //달력
        $('#tbstartDate').datepick($.datepicker.regional[PreLang]);
        $('#tbendDate').datepick($.datepicker.regional[PreLang]);
    });
});

/*************************
작성자   : daniel
작성일자 : 2011-10-19 
작성목적 : 일정내용 세팅
*************************/
AddSimple.ValidateSchedule = function () {

    Title = jQuery.trim($('#tbTitle').val());

    Type = "I";

    if (Title.length == 0) {
        alert(Add_InputTitle);
        return;
    }
    OpenType = $('#ddlShare').val();

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

    if (sInt >= eInt) {
        alert(TimeOver);
        return;
    }

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

    AddSimple.ScheduleQuickADD();
}




/*************************
작성자   : daniel
작성일자 : 2011-10-19 
작성목적 : 일정추가
*************************/
AddSimple.ScheduleQuickADD = function () {

   Title =  replaceAll(Title, "+", "＋");
    
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/AddSimple.asmx/AddSchedule";
    ParamArray = {
        'Type': Type,
        'Title': Title.replace(/&/g, '＆'),
        'Content': '',
        'StartTime': StartTime,
        'EndTime': EndTime,
        'OpenType': OpenType,
        'Lunar': 'N',
        'StatusID': $("#ddlStatus").val(),
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddlAlarmTime").val(),
        'AlarmMail': $("#idEmail").val()
    };

    var obj = eval("AddSimple.SuccessQuickAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

function replaceAll(str, searchStr, replaceStr) {
  return str.split(searchStr).join(replaceStr);
}


/*************************
작성자   : daniel
작성일자 : 2011-10-19 
작성목적 : 일정추가성공
*************************/
AddSimple.SuccessQuickAdd = function (data, status) {
    //$('#txtPimcontent').val('');



    ParentGetEvent();
    //if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    $("#divQuickSch").hide();
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
