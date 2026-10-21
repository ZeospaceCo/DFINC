Add = {}

$(function () {
    $(document).ready(function () {

        //저장후 계속
        $('#CbtnContinue').click(function (e) {
            e.preventDefault();
            if (BtnCheck == "Y") {
                alert(WaitMent);
                return;
            }
            Add.Confirm("CONTINUE");
        });

        // 저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            if (BtnCheck == "Y") {
                alert(WaitMent);
                return;
            }
            Add.Confirm("SAVE");
        });

        //목록
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //반복설정취소
        $('#cbtnRepCancel').click(function (e) {
            e.preventDefault();
            $("#txtrepeat").val('');
            $("#hdRepeat").val('');
            $("#endDate").show();
            RepeatValue = "";
        });


        //파일추가
        $('#cbtnSwfFileAdd').click(function (e) {

            e.preventDefault();
            Add.AttachSwfFile();
        });

        //반복설정
        $('#cbtnRepeat').click(function (e) {
            e.preventDefault();
            Startdate = $('#startDate').val();
            Add.SettingRepeat();
        });

        //공유대상
        $('#CbtnGetUser').click(function (e) {
            e.preventDefault();
            Add.ShareUser();
        });

        //기본알람으로 설정
        $('#CbtnSetting').click(function (e) {
            e.preventDefault();
            Add.AlarmSetting();
        });

        //달력
        $('#startDate').datepick($.datepicker.regional[PreLang]);
        $('#endDate').datepick($.datepicker.regional[PreLang]);


        //태그프리 액티브 엑스
        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');

        });

        $('#endDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        Add.GetSpoolFileList();

        //시간선택
        $('#ddlstartTime').change(function (e) {
            e.preventDefault();
            Add.SettingTime();
        });
        //시간선택
        $('#ddlendTime').change(function (e) {
            e.preventDefault();
            Add.SettingTime();
        });


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

        if ($('#ddlnotikind').val() == "M") {
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

        ///아이프레임 부모페이지 높이 자동조절
        if (parent.FrameHeightControl != null) {
            var yHeight = 900;
            parent.FrameHeightControl(yHeight);
        }


        var sin = document.getElementById("ddlstartTime").selectedIndex;

        if (sin > 45) {
            document.getElementById("ddlendTime").selectedIndex = 47;
        }
        else {
            document.getElementById("ddlendTime").selectedIndex = sin + 2;
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
                    location.href = "Add.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "AddN.aspx?" + ClientQueryString;
                }
            });
        }
        */

    });
});

/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 시간선택
***************************************************/
Add.SettingTime = function () {
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


/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 반복설정
***************************************************/
Add.SettingRepeat = function () {
    var url = "/DeskPlusEIP/Pim/Schedule/ScheduleRepeat.aspx";
    //PopupWin(400, 400, url, "-100", "-100", true);
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "Repeat", "500", "330")
    
}


/**************************************************
작성자   : 김광일
작성일자 : 2011-8-23
작성목적 : 저장
***************************************************/
Add.Confirm = function (arg) {
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

    $("#hdRepeat").val(RepeatValue);

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


    var validator = new Validator($("#form1"));


    validator.checkRequired("txttitle", Add_InputTitle, true);
    if (!validator.validate()) return;

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
작성일자 : 2011-04-11
작성목적 : 일정 추가 취소 
***************************************************/
Add.Cancel = function () {
    
    if (PageScope == "ListDay.aspx" || PageScope == "ListMonth.aspx" || PageScope == "ListWeek.aspx" ) {
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
            , 'PageScope': PageScope
        };
    }
    else {
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
                'CurrentPage': s_CurrentPage
                , 'PageScope': PageScope
            };
        }
        else {
            PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
            ParamArray = {
                'Page': '',
                'Date': ''
                , 'PageScope': PageScope
            };
        }
    }
    CallEncrypt(CallGetParam());
    location.href = PageUrl + "?p=" + GetEncryptValue;
}



/**************************************************
작성자   : 김광일
작성일자 : 2011-08-22
작성목적 : 첨부파일 추가
***************************************************/
Add.AttachFile = function () {

}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'PIM',
        'callBackEvent': "Add.GetSpoolFileList"
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
Add.GetSpoolFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

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
        var obj = eval("Add.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

Add.ShareUser = function () {
    var url = "/DeskPlusEIP/Pim/Common/ShareUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "ShareUser", "800", "500")
}


/*********************
작성자:김광일
작성일:2011년 9월 19일
용도: 공유리스트 관리
**********************/
Add.ShareUserClick = function (obj) {

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
                + "<a onclick='Add.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + RDEL + "'><i class='fas fa-times-circle'></i></a>"
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
    strHtml += "<colgroup><col /><col col width='90px' /></colgroup>";
    strHtml += "<tr><th id='r_name'>" + RNAME + "</th><th id='r_title'>" + RDEL + "</th></tr>";

    var chk = 'F';
    for (var i = 0; i < arrayid.length - 1; i++) {
    var arrayName = arrayid[i].split('$');

    if (arrayName[0].length > 0) {
    strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
    strHtml = strHtml + "<td class='center'><a onclick='javascript:Add.ShareUserClick(this)' value='" + arrayid[i] + "'>" + DelImg + "</a></td>"
    strHtml = strHtml + "</tr>";
    chk = 'T';
    }
    }
    if (chk == 'F')
    strHtml += "<tr><td class='center'>" + TextEmpty + "</td><td>&nbsp;</td></tr>";

    strHtml += "</table>";

    $("#divShare").html('');
    $("#divShare").html(strHtml);
    */

}



function TextReverseToXss(arg) {
    
    arg = arg.replace(/&/gi, "&amp;");
    arg = arg.replace(/"/gi, "&quot;");
    arg = arg.replace(/#/gi, "&#35;");
    arg = arg.replace(/'/gi, "&#39;");
    arg = arg.replace("(", "&#40;");
    arg = arg.replace(")", "&#41;");
    arg = arg.replace(/</gi, "&lt;");
    arg = arg.replace(/>/gi, "&gt;");



    return arg;
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
Add.AlarmSetting = function () {

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
    var obj = eval("Add.SuccessSetting");
    var url = "/DeskPlusEIP/Pim/Ws/Schedule/Add.asmx/AlarmSetting";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2020-02-05
작성목적 : 기본 알람으로 설정 (웹서비스 값받기)
***************************************************/
Add.SuccessSetting = function (data, status) {

    if (data.d == true) {
        alert(ACCOUNT_SUCCESS);
    }
    else {
        alert(MsgError);
    }
}
