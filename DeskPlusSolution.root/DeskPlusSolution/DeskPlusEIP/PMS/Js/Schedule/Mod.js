Mod = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            $("#txtrepeat").val('');
            $("#hdRepeat").val('');
        });

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();

            if (BtnCheck == "Y") {
                alert(WaitMent);
                return;
            }
            if ($("#hdRepeatChk").val() == "") {
                Mod.Confirm();
            }
            else {
                if ($("#hdRepeat").val() == "") {
                    Mod.Confirm();
                }
                else {
                    Mod.RepeatInfoMod();
                }
            }
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //시간선택
        $("#ddlStartTime").change(function (e) {
            e.preventDefault();

            Mod.SettingTime();
        });

        //시간선택
        $('#ddlEndTime').change(function (e) {
            e.preventDefault();

            Mod.SettingTime();
        });

        //시간선택
        $('#ddlStartTimeRepeat').change(function (e) {
            e.preventDefault();
            Mod.SettingTime2();
        });
        //시간선택
        $('#ddlEndTimeRepeat').change(function (e) {
            e.preventDefault();
            Mod.SettingTime2();
        });

        //반복설정
        $('#cbtnSetRepeat').click(function (e) {
            e.preventDefault();
            RepeatSchedule();
        });

        //달력
        $("#tbFullYMD1").datepick($.datepicker.regional[PreLang]);
        $("#tbFullYMD2").datepick($.datepicker.regional[PreLang]);

        $('#tbFullYMD1').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $('#tbFullYMD2').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $(".clsDate").keydown(function (e) { return false; });

        xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

/**********************************************************************************************
[시작일시의 달력 새창 띄우기]
**********************************************************************************************/
function SelectDateCode1() {
    var startDate = document.getElementById("tbFullYMD1").value;

    var url = "/DeskPlusEIP/Common/Calendar/CalendarDialog.aspx";
    var param = "func=GetDate1&startDate=" + startDate;
    WindowObjectReference = PopupWinOpen(url, param, "Calendar", "780", "380")
}

/**********************************************************************************************
[시작일시의 달력값 설정]
**********************************************************************************************/
function GetDate1(hdYear, hdMonth, hdDay) {
    document.getElementById("tbFullYMD1").value = hdYear + "-" + hdMonth + "-" + hdDay;
}

/**********************************************************************************************
[종료일시의 달력 새창 띄우기]
**********************************************************************************************/
function SelectDateCode2() {
    var startDate = document.getElementById("tbFullYMD2").value;

    var url = "/DeskPlusEIP/Common/Calendar/CalendarDialog.aspx";
    var param = "func=GetDate2&startDate=" + startDate;
    WindowObjectReference = PopupWinOpen(url, param, "Calendar", "780", "380")
}

/**********************************************************************************************
[종료일시의 달력값 설정]
**********************************************************************************************/
function GetDate2(hdYear, hdMonth, hdDay) {
    document.getElementById("tbFullYMD2").value = hdYear + "-" + hdMonth + "-" + hdDay;
}

/**********************************************************************************************
[ 저장하기]
**********************************************************************************************/
Mod.Confirm = function () {
    //일반 일정 저장하기
    cbtnScheduleSubmit();
}

/**********************************************************************************************
[반복일정을 내용과 제목만 변경하고자 할경우]
**********************************************************************************************/
function EditScheduleRepeatSubmit() {
    if (EditorName == "TCK") {
        __doPostBack('lbtnSubmit', '');
    }
    else {
        if (MimeUseChk == "Y")  //Mime을 사용하여 이미지 업로드
        {
            document.getElementById("tbContentHTML").value = document.getElementById("twe").HtmlValue;
            __doPostBack('lbtnSubmit', '');
        }
        else   //자체 업로드 기능을 이용하여 업로드
        {
            var files = form1.twe.GetLocalFiles();

            if (files != "")  // 본문에 이미지가 첨부되어 있을대
            {
                form1.DPUpload.SetPostFilePathInit(); //저장된 파일 관련 내용 초기화

                var filesList = files.split(";");
                for (i = 0; i < filesList.length; i++) {
                    if (filesList[i].substr(0, 2) != "\\\\") {
                        form1.DPUpload.SetPostFilePath(filesList[i]);
                    }
                }

                if ((form1.DPUpload.AppendFileCount() + form1.DPUpload.GetPostFilePathCount()) > 0) {
                    form1.DPUpload.SetFileNotUpload = 0;
                    form1.DPUpload.Host = uploadHost;
                    form1.DPUpload.Port = uploadPort;
                    form1.DPUpload.PostAcceptor = uploadURL + "?path=" + uploadPath + "&cID=" + DelFileCID + "&uID=" + DelFileUID;

                    form1.DPUpload.Post();

                    var rtnVal = form1.DPUpload.ServerRespones;

                    if (rtnVal == "") {
                        form1.twe.ChangeLocalFilesUrl(uploadHost + uploadPath + "/");
                        document.getElementById("tbContentHTML").value = document.getElementById("twe").HtmlValue;
                        __doPostBack('lbtnSubmit', '');
                    } else {
                        alert(rtnVal);
                    }
                }
            }
            else {
                document.getElementById("tbContentHTML").value = document.getElementById("twe").HtmlValue;
                BtnCheck = "Y";
                __doPostBack('lbtnSubmit', '');
            }
        }
    }
}

/**********************************************************************************************
[일정 저장하기]
**********************************************************************************************/
function cbtnScheduleSubmit() {
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

            alert(AlertScriptLong);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var sdate = $("#tbFullYMD1").val();
    var edate = $("#tbFullYMD2").val();

    if (sdate == "" || edate == "") {
        alert(R_ApplyRegDate);
        return false;
    }

    sdate = sdate.replace(/-/gi, '');
    edate = edate.replace(/-/gi, '');

    var stime = $("#ddlStartTime").val();
    var etime = $("#ddlEndTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');

    sdate = sdate + stime;
    edate = edate + etime;

    var sInt = parseInt(sdate);
    var eInt = parseInt(edate);

    if (sInt > eInt) {
        alert(R_StartDateEndDate); //TimeAlert
        return;
    }
    
    /*
    var rstime = $("#ddlStartTimeRepeat").val();
    var retime = $("#ddlEndTimeRepeat").val();

    rstime = rstime.replace(/:/gi, '');
    retime = retime.replace(/:/gi, '');
  

    if (parseInt(rstime) > parseInt(retime)) {
        alert(R_StartDateEndDate); //TimeAlert
        return;
    }
    */

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbTitleName", Add_InputTitle, true);
    if (!validator.validate()) return;

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(Add_InputContent);
        return;
    }

    $("#hdStart").val($("#tbFullYMD1").val());
    $("#hdEnd").val($("#tbFullYMD2").val());

    BtnCheck = "Y";
    __doPostBack('lbtnSubmit', '');
    //lbtnSubmit
}

/*--------------------------------------------------------------------------------------------------
// [반복일정 새창 띄우기]
--------------------------------------------------------------------------------------------------*/
function RepeatSchedule() {
    var url = "/DeskPlusEIP/Pim/Schedule/ScheduleRepeat.aspx";
    //PopupWin(400, 400, url, "-100", "-100", true);
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "314")
}

/*--------------------------------------------------------------------------------------------------
// [반복일정값 셋팅하기- 반복일정새창에서 값을 넘겨받음]
--------------------------------------------------------------------------------------------------*/
function GetRepeatInfo(_type, _selectType, _intervalCount, _option1, _option2, _option3, _option4, _option5, _option6, _option7, _endDate, _maxReptCount, _repeatResultInfo, _ddlResult) {
    document.getElementById("tbScheduleRepeat").value = _ddlResult;
    document.getElementById("tbScheduleRepeatInfo").value = _repeatResultInfo;
}

/*--------------------------------------------------------------------------------------------------
// [리스트 페이지 이동하기]
--------------------------------------------------------------------------------------------------*/
Mod.Cancel = function () {
    ParamArray = {
        'projectID': projectID,
        'menuID': menuID,
        'menuType': menuType,
        'rqProjectID': rqProjectID,
        'scheduleID': scheduleID,
        'status': status
    };

    CallEncrypt(CallGetParam());
    var url = "List.aspx?p=" + GetEncryptValue;
    self.location.href = url;
}

Mod.RepeatInfoMod = function () {
    var url = "/DeskPlusEIP/Pim/Schedule/ConfirmRepeat.aspx";

    PopupWin(500, 220, url, "-500", "-10", false);
}

Mod.SettingTime = function () {
    var sdate = $("#tbFullYMD1").val();
    var edate = $("#tbFullYMD2").val();

    sdate = sdate.replace(/-/gi, '');
    edate = edate.replace(/-/gi, '');

    var stime = $("#ddlStartTime").val();
    var etime = $("#ddlEndTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');

    sdate = sdate + stime;
    edate = edate + etime;

    var sInt = parseInt(sdate);
    var eInt = parseInt(edate);

    if (sInt >= eInt) {
        //stime +
        //var index1 = $("#ddlstartTime").index($("#ddlstartTime"));
        //alert(index1);
        var maxindex = $("#ddlStartTime").get(0).options.length;
        var sel = $("#ddlStartTime option").index($("#ddlStartTime option:selected"));

        if ((sel + 2) < maxindex) {
            $("#ddlEndTime option:eq(" + (sel + 1) + ")").attr("selected", "selected");
        }
        else {
            $("#ddlEndTime option:eq(47)").attr("selected", "selected");
        }
    }
}

Mod.SettingTime2 = function () {
    var sdate = $("#tbFullYMD1").val();
    var edate = $("#tbFullYMD2").val();

    sdate = sdate.replace(/-/gi, '');
    edate = edate.replace(/-/gi, '');

    var stime = $("#ddlStartTimeRepeat").val();
    var etime = $("#ddlEndTimeRepeat").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');

    sdate = sdate + stime;
    edate = edate + etime;

    var sInt = parseInt(sdate);
    var eInt = parseInt(edate);

    if (sInt >= eInt) {
        //stime +
        //var index1 = $("#ddlstartTime").index($("#ddlstartTime"));
        //alert(index1);
        var maxindex = $("#ddlStartTimeRepeat").get(0).options.length;
        var sel = $("#ddlStartTimeRepeat option").index($("#ddlStartTimeRepeat option:selected"));

        if ((sel + 2) < maxindex) {
            $("#ddlEndTimeRepeat option:eq(" + (sel + 1) + ")").attr("selected", "selected");
        }
        else {
            $("#ddlEndTimeRepeat option:eq(47)").attr("selected", "selected");
        }
    }
}