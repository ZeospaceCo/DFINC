Add = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-05-11
작성목적 : 초기화
***************************************************/
$(function () {

    jQuery(document).ready(function () {
        jQuery("#tbCalendar").datepick($.datepicker.regional[PreLang]);
    });

    //저장후 계속
    $('#btnContinue').click(function (e) {
        e.preventDefault();

        if (BtnCheck == "Y") {
            alert(WaitMent);
            return;
        }
        Add.Confirm('CONTINUE');
    });

    //저장
    $('#btnConfirm').click(function (e) {
        e.preventDefault();

        if (BtnCheck == "Y") {
            alert(WaitMent);
            return;
        }


        Add.Confirm('SAVE');
    });

    //취소
    $('#btnCancel').click(function (e) {
        e.preventDefault();
        
        Add.Cancel();
    });

    $('#startDate').datepick($.datepicker.regional[PreLang]);
    $('#endDate').datepick($.datepicker.regional[PreLang]);



    $('#startDate').focus(function (e) {
        $('#tdDate').css('height', '220px');


    });
    $('#endDate').focus(function (e) {
        $('#tdDate').css('height', '220px');
    });

    if (parent.FrameHeightControl != null) {
        var yHeight = 800;
        parent.FrameHeightControl(yHeight);
    }

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


    Add.SettingTime();

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


/******************************
작성자:김광일
작성일: 2011-9-5
용도: 할일저장
******************************/
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



    var sdate = $("#startDate").val();
    var edate = $("#endDate").val();
    var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);
    if (!dtResponse) {
        alert(TimeAlert); //TimeAlert
        return;
    }
    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');
    var sInt = parseInt(stime, 10);
    var eInt = parseInt(etime, 10);
    if (sInt >= eInt) {
        alert(TimeAlert); //TimeAlert
        return;
    }

    if ($("#tbTitle").val() == "") {

        alert(Add_InputTitle);
        return;
    }

    if ($("#ddlSchedule").val() == "2") {
        if ($("#startDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
        if ($("#endDate").val() == "") {
            alert(Add_InputDate);
            return;
        }
    }


    BtnCheck = 'Y';

    __doPostBack('lbtnWrite', arg);

}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 일정 추가 취소 
***************************************************/
Add.Cancel = function () {

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
    else if (PageUrl == "ListSearch.aspx") {
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
    else if (PageUrl == "AllSchedule.aspx") {

        PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
        ParamArray = {
            'UserList': s_UserId,
            'StartDate': s_StartDate,
            'EndDate': s_EndDate,
            'title': s_SearchKind,
            'name': s_SerachValue
        };
    }
    else if (PageUrl == "ListDay.aspx") {

        PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
        ParamArray = {
            'Page': '',
            'Date': ''
                    , 'PageScope': 'ListDay'
        };

    }
    else if (PageUrl == "ListMonth.aspx") {

        PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
        ParamArray = {
            'Page': '',
            'Date': '',
            'PageScope': 'ListMonth'
        };


    }
    else {
        ParamArray = {
            'ScheduleID': '',
            'Page': '',
            'Date': ''
        };
    }
    CallEncrypt(CallGetParam());

    location.href = PageUrl + "?p=" + GetEncryptValue;


    return;
    if (PageUrl == "") {
        //PageUrl = "/DeskplusEip/Pim/Schedule/ListDay.aspx";

        PageUrl = "/DeskplusEip/Pim/Schedule/ScheduleMain.aspx";
        ParamArray = {
            'PageScope': PageScope,
            'Page': PageUrl,
            'Date': ''
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
                'CurrentPage': s_CurrentPage,
                'PageScope': PageScope
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
                'CurrentPage': s_CurrentPage,
                'PageScope': PageScope
            };
        }
        else {
            PageUrl = "/DeskplusEip/Pim/Schedule/" + PageUrl;
            ParamArray = {
                'PageScope': PageScope,
                'Page': PageUrl,
                'Date': ''
            };
        }
    }
    CallEncrypt(CallGetParam());
    location.href = PageUrl + "?p=" + GetEncryptValue;

}



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 일정 공개대상
***************************************************/
Add.GetUser = function () {

    var url = "../Common/GetUser.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "GetUser", "800", "500")
}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 첨부파일 추가
***************************************************/
Add.AddFile = function () {

}


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
