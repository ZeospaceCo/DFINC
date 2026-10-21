Mod = {}

var ResourceArray = [];

/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인(수정)
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Confirm();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //반복설정
        $('#cbtnRepeat').click(function (e) {
            e.preventDefault();
            Startdate = $('#startDate').val();
            Mod.AddRepeat();
        });

        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            Mod.CancelRepeat();
        });
       

        //달력
        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").datepick($.datepicker.regional[PreLang]);


        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');

        });

        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $(".clsDate").keydown(function (e) { return false; });

        
        // 반복일정일때 endDate를 설정하는게 무의미 
        if ($("#txtrepeat").val() != "") {
            $("#endDate").hide();
        }

        //알람
        $('#ddlnotikind').change(function (e) {
            e.preventDefault();

            if ($('#ddlnotikind').val() == "Messenger") //메신저
            {
                $('#ddltime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "Mail") //EMAIL
            {
                $('#ddltime').hide();
                $('#divment').hide();
            }
            if ($('#ddlnotikind').val() == "SMS") //SMS
            {
                $('#ddltime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "") //무
            {
                $('#ddltime').hide();
                $('#divment').hide();
            }
        });

        $('#ddltime').hide();
        $('#divment').hide();

        xfe.setHtmlValue($("#hdContent").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#hdContent").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }            
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});


/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복설정
***************************************************/
Mod.AddRepeat = function () {
    var url = "AddRepeat.aspx";

    var getStartDate = $("#startDate").val();

    ParamArray = {
        'startDate': getStartDate
    };

    CallEncrypt(CallGetParam());

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "330")
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복설정 
***************************************************/
function AddRepeatInfo(_resultInfo) {
    $("#txtrepeat").val(_resultInfo);
    $("#endDate").hide();

    if ($("#hdRepeatChk").val() == "Y") {
        $("#hdRepeatChk").val('N');
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복 설정 취소 
***************************************************/
Mod.CancelRepeat = function () {
    $("#txtrepeat").val('');
    $("#endDate").show();

    $("#hdRepeatType").val('');
    $("#hdSelectType").val('');
    $("#hdIntervalCnt").val('');
    $("#hdOption1").val('');
    $("#hdOption2").val('');
    $("#hdOption3").val('');
    $("#hdOption4").val('');
    $("#hdOption5").val('');
    $("#hdOption6").val('');
    $("#hdOption7").val('');
    $("#hdMaxReptCnt").val('');
    $("#hdEndDate").val('');

    $("#tbRepeat").val('');
}


/**************************************************************
작성자 : famo23
작성일자 : 2012-06-01
작성목적 : 취소
**************************************************************/
Mod.Cancel = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'tmpdate': TmpDate,
        'scheduleID': ScheduleID,
        'grade': Grade
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;

}

/**************************************************************
작성자 : famo23
작성일자 : 2012-06-01
작성목적 : 스케줄 수정
**************************************************************/
Mod.Confirm = function () {

    if (BtnCheck == "Y") return;

    //에디터 처리
    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_EditorScript);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    //제목 입력 필수
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputTitle, true);
    if (!validator.validate()) return;


    //날짜 확인 
    var getStartDate = $("#startDate").val();
    var getEndDate = $("#endDate").val();
    var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
    var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

    if ($("#txtrepeat").val() != "") {
        if (getStartDate == "") {
            alert(R_ApplyStartDate);
            return false;
        }
        else {
            if (eTime <= sTime) {
                alert(R_StartDateEndDiffer);
                return;
            }
        }

        if ($("#hdRepeatChk").val() != "") {
            var url = "/DeskPlusEIP/Community/Site/Apps/Schedule/ConfirmRepeat.aspx";
            PopupWin(400, 270, url, "", "", true);
        }
        else {
            Mod.GetRepeat();
        }
    }
    else {
        if (getStartDate == "" || getEndDate == "") {
            alert(R_ApplyRegDate);
            return false;
        }
        else {
            var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

            if (dtResponse == false) {
                alert(R_StartDateEndDate);
                return false;
            }

            var dtResponse1 = CompareDT(getStartDate, getEndDate, "equal", PreLangReal);
            if (dtResponse1 == true) {
                if (eTime <= sTime) {
                    alert(R_StartDateEndDiffer);
                    return;
                }
            }

        }

        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 자원 반복 설정 
**********************************************************************************************/
function RepeatMod(_arg) {
    $("#hdRepeatSel").val(_arg);

    if (($("#hdRepeatChk").val() == "Y") && ($("#hdRepeatSel").val() == "1")) {
        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        Mod.GetRepeat();
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복 일자 가져오기 (웹서비스 호출)
************************************************************************/
Mod.GetRepeat = function () {

    ParamArray = {
        'repeatType': $("#hdRepeatType").val(),
        'selectType': $("#hdSelectType").val(),
        'intervalCnt': $("#hdIntervalCnt").val(),
        'option1': $("#hdOption1").val(),
        'option2': $("#hdOption2").val(),
        'option3': $("#hdOption3").val(),
        'option4': $("#hdOption4").val(),
        'option5': $("#hdOption5").val(),
        'option6': $("#hdOption6").val(),
        'option7': $("#hdOption7").val(),
        'maxReptCnt': $("#hdMaxReptCnt").val(),
        'endDate': $("#hdEndDate").val(),
        'startDate': $("#startDate").val(),
        'repeatChk': $("#hdRepeatChk").val(),
        'repeatSel': $("#hdRepeatSel").val(),
        'groupStartDate': GroupStartDate
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetRepeat");
    var url = "/DeskPlusEIP/Community/Ws/Site/Apps/Schedule/Mod.asmx/GetRepeat";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-01
작성목적 : 반복 일자 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Mod.SuccessGetRepeat = function (data, status) {

    if (data.d != "") {
        $("#tbRepeat").val(data.d);
        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        alert(R_NoRepeatDay);
        return;
    }
}