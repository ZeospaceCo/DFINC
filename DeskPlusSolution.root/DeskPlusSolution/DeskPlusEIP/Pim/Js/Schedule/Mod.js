Mod = {}

$(function () {
    $(document).ready(function () {

        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            $("#txtrepeat").val('');
            $("#hdRepeat").val('');
            $("#endDate").show();
        });
        //공유대상
        $('#CbtnGetUser').click(function (e) {
            e.preventDefault();
            Mod.ShareUser();
        });

        //기본알람으로 설정
        $('#CbtnSetting').click(function (e) {
            e.preventDefault();
            Mod.AlarmSetting();
        });

        //반복설정
        $('#cbtnRepeat').click(function (e) {

            e.preventDefault();
            Startdate = $('#startDate').val();
            Mod.SettingRepeat();
        });


        // 저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();

            if (BtnCheck == "Y") {
                alert(WaitMent);
                return;
            }

            if ($("#hdRepeatChk").val() == "") {
                Mod.Confirm("MOD");
            }
            else {
                if ($("#hdRepeat").val() == "") {
                    Mod.Confirm("MOD");
                }
                else {
                    Mod.RepeatInfoMod();
                }
            }

        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //파일추가
        $('#cbtnSwfFileAdd').click(function (e) {

            e.preventDefault();
            Mod.AttachSwfFile();
        });

        $('#startDate').datepick($.datepicker.regional[PreLang]);
        $('#endDate').datepick($.datepicker.regional[PreLang]);


        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            Mod.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            Mod.SettingTime();
        })

        //알람1
        $('#ddlnotikind').change(function (e) {
            e.preventDefault();

            if ($('#ddlnotikind').val() == "M") //메신저
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddltime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "E") //EMAIL
            {
                $('#idEmail').show();
                $('#msgMail').show();
                $('#ddltime').hide();
                $('#divment').hide();
            }
            if ($('#ddlnotikind').val() == "B") //Mobile
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddltime').show();
                $('#divment').show();
            }
            if ($('#ddlnotikind').val() == "") //무
            {
                $('#idEmail').hide();
                $('#msgMail').hide();
                $('#ddltime').hide();
                $('#divment').hide();
            }
        });


        $('#idEmail').hide();
        $('#msgMail').hide();
        $('#ddltime').hide();
        $('#divment').hide();

        if ($('#ddlnotikind').val() == "M") { //메신저 일정등록을 해 놓았다면 초기 로드시에 해당 정보 보여줘야 함 : 이대성 부장님 요청2016-03-30 by maniaysw
            $('#ddltime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "B") {
            $('#ddltime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "E") {
            $('#idEmail').show();
            $('#msgMail').show();
        }


        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');


        });
        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });


        //첨부파일 추가
        $('#CbtnFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachFile();
        });


        Mod.GetAttachFileList();  
        Mod.GetSpoolFileList();

        if (parent.FrameHeightControl != null) {
            var yHeight = 900;
            parent.FrameHeightControl(yHeight);
        }

        // 반복일정일때 endDate를 설정하는게 무의미 
        if ($("#txtrepeat").val() != "") {
            $("#endDate").hide();
        }

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


Mod.ShareUser = function () {
    var url = "/DeskPlusEIP/Pim/Common/ShareUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "ShareUser", "800", "500")
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-8-23
작성목적 : 저장
***************************************************/
Mod.Confirm = function (arg) {
    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
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

    var sdate = $("#startDate").val();
    var edate = $("#endDate").val();
    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');
    var sInt = parseInt(stime, 10);
    var eInt = parseInt(etime, 10);

    if ($("#txtrepeat").val() != "") {
        if ($("#startDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
        else {
            if (sInt >= eInt) {
                alert(TimeAlert); //TimeAlert
                return;
            }
        }
    }
    else {
        if (sdate == "" || edate == "") {
            alert(Add_InputDate);
            return;
        }
        else {
            var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);
            if (!dtResponse) {
                alert(TimeAlert); //TimeAlert
                return;
            }

            var dtResponse1 = CompareDT(sdate, edate, "equal", PreLang);
            if (dtResponse1 == true) {
                if (sInt >= eInt) {
                    alert(TimeAlert); //TimeAlert
                    return;
                }
            }
        }
    }

    if ($("#txttitle").val() == "") {

        alert(Add_InputTitle);
        return;
    }

    if ($("#startDate").val() == "") {
        alert(Add_InputDate);
        return;
    }

    //수정시 메신저 알람시간을 선택하지 않을시 본문내용 사라짐 현상 
    if ($('#ddlnotikind').val() == "M") {
        if ($('#ddltime').val() == "") {
            alert(AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "B") //Mobile
    {
        if ($("#ddltime").val() == "") {
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


    BtnCheck = "Y";
    __doPostBack('lbtnWrite', arg);


}


/**************************************************
작성자   : 이현수
작성일자 : 2013-03-26
작성목적 : 일정 추가 취소 
***************************************************/
Mod.Cancel = function () {
    history.back();
    /*
    if (PageScope == "ListDay.aspx" || PageScope == "ListMonth.aspx" || PageScope == "ListWeek.aspx") {
        ParamArray = {
            'PageScope': PageScope,
            'Date': paraDate,
            'Opentype': paraOpentype
        }
        CallEncrypt(CallGetParam());
        location.href = "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx?p=" + GetEncryptValue;
        return;
    }

    if (PageUrl == "") {
        PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
    }

    if (PageUrl == "ListSearch.aspx") {
        PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
        ParamArray = {
            'Distance': s_Distance,
            'inquryPerson': s_inquryPerson,
            'DatePeriod': s_DatePeriod,
            'SearchKind': s_SearchKind,
            'SerachValue': s_SerachValue,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'PageSize': s_PageSize,
            'CurrentPage': s_CurrentPage,
            'Page': PageUrl
        };
    }
    else if (PageUrl == "ListTotal.aspx") {
        PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
        ParamArray = {
            'Distance': s_Distance,
            'inquryPerson': s_inquryPerson,
            'DatePeriod': s_DatePeriod,
            'SearchKind': s_SearchKind,
            'SerachValue': s_SerachValue,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'PageSize': s_PageSize,
            'CurrentPage': s_CurrentPage
        };
    }
    else {
        PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
        ParamArray = {
            'ScheduleID': '',
            'Page': PageUrl,
            'Date': ''
        };
    }

    CallEncrypt(CallGetParam());
    location.href = PageUrl + "?p=" + GetEncryptValue;
    */
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-08-22
작성목적 : 첨부파일 추가
***************************************************/
Mod.AttachFile = function () {

}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Mod.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'PIM',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Mod.GetSpoolFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {

    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 임시파일 삭제 처리
**********************************************************************************************/
function DelSpoolPath(fileName) {

    if (window.confirm(R_ApplyDelete)) {

        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Mod.GetAttachFileList = function () {

    ParamArray = {
        'ScheduleID': ScheduleID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Pim/Ws/Common/AttachFile.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileListmod').setTemplateURL('/DeskPlusEIP/Pim/Jt/Anniversary/JTDelFile.htm?seq=' + Guids());
    $('#placeholder_fileListmod').processTemplate(data.d);

}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(attachFileID) {

    if (window.confirm(R_ApplyDelete)) {

        ParamArray = {
            'AttachID': attachFileID
        };

        CallEncrypt(CallGetParam())
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("SuccessDel");
        var url = "/DeskPlusEIP/Pim/Ws/Common/AttachFile.asmx/DelAttachFile";
        AjaxService(url, data, obj);


    }

}
function SuccessDel(data, status) {
    Mod.GetAttachFileList();
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 반복설정
***************************************************/
Mod.SettingRepeat = function () {
    var url = "/DeskPlusEIP/Pim/Schedule/ScheduleRepeat.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "330")
    
}



/*********************
작성자:김광일
작성일:2011년 9월 19일
용도: 공유리스트 관리
**********************/
Mod.ShareUserClick = function (obj) {

    $("#divShare").html('');

    var strtemp = $("#hdShareList").val().replace($(obj).attr('value'), '');
    strtemp = strtemp.replace($(obj).attr('value') + ",", '');

    $("#hdShareList").val(strtemp);
    var arrayid = strtemp.split(',');

    var chk = 'F';
    for (var i = 0; i < arrayid.length - 1; i++) {
        var arrayName = arrayid[i].split('$');

        if (arrayName[0].length > 0) {

            chk = 'T';
            $("#divShare").append(
		         "<li>"
			    + "<span>" + arrayName[2] + "</span>"
                + "<a onclick='Mod.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + R_FileDel + "'><i class='fas fa-times-circle'></i></a>"
                + "</li>"
	        );
        }
    }

    if (chk == 'F') {
        $("#divShare").html('<li></li>');
    }

    /*
    var DelImg = "<img src='" + BaseImgPath + '/Lang/' + Lang + "/ico_folderset_del.gif' alt='del' />";

    var strHtml = "<table width='100%' cellspacing='0' class='table_typeT'>";
    strHtml += "<caption></caption>";
    strHtml += "<colgroup><col  /><col col width='90px' /></colgroup>";
    strHtml += "<tr><th id='r_name'>" + RNAME + "</th><th id='r_title'>" + RDEL + "</th></tr>";

    var chk = 'F';
    for (var i = 0; i < arrayid.length - 1; i++) {
    var arrayName = arrayid[i].split('$');

    if (arrayName[0].length > 0) {
    strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
    strHtml = strHtml + "<td class='center'><a onclick='javascript:Mod.ShareUserClick(this)' value='" + arrayid[i] + "' >" + DelImg + "</a></td>"
    strHtml = strHtml + "</tr>";
    chk = 'T';
    }
    }

    if (chk == 'F')
    strHtml += "<tr><td colspan='2' class='center'>" + TextEmpty + "</td></tr>";

    strHtml += "</table>";

    $("#divShare").html('');
    $("#divShare").html(strHtml);
    */
}

/**************************************************
반복일정 수정
***************************************************/
Mod.RepeatInfoMod = function () {
    var url = "/DeskPlusEIP/Pim/Schedule/ConfirmRepeat.aspx";
    PopupWin(400, 270, url, "-500", "-10", false);
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 시간선택
***************************************************/
Mod.SettingTime = function () {

    var sdate = $("#startDate").val();
    var edate = $("#endDate").val();
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




function TextReverseToXss2(arg) {
    var rtStr = "";
    arg = arg.replace(/&amp;/gi, "&");
    arg = arg.replace(/&#35;/gi, "#");
    arg = arg.replace(/&#39;/gi, "'");
    arg = arg.replace(/&#40;/gi, "(");
    arg = arg.replace(/&#41;/gi, ")");
    arg = arg.replace(/&lt;/gi, "<");
    arg = arg.replace(/&gt;/gi, ">");
    arg = arg.replace("&#40;", "(");
    arg = arg.replace("&#41;", ")");

    return arg;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-10-17
작성목적 : 반복설정 
***************************************************/
function RepeatHide(_resultInfo) {
    $("#endDate").hide();
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-04
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
작성일자 : 2020-02-05
작성목적 : 기본 알람으로 설정 (웹서비스 호출)
***************************************************/
Mod.AlarmSetting = function () {

    if ($('#ddlnotikind').val() == "M") //메신저
    {
        if ($("#ddltime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "B") //Mobile
    {
        if ($("#ddltime").val() == "") {
            alert(AlarmAlert);
            return;
        }
    }

    ParamArray = {
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddltime").val()
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSetting");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Mod.asmx/AlarmSetting";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-05
작성목적 : 기본 알람으로 설정 (웹서비스 값받기)
***************************************************/
Mod.SuccessSetting = function (data, status) {

    if (data.d == true) {
        alert(ACCOUNT_SUCCESS);
    }
    else {
        alert(MsgError);
    }
}
