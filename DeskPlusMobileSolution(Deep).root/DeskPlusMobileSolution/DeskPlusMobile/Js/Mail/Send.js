Send = {}

/**************************************************

작성일자 : 2011-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    jQuery.each($("#btPCAddFile"), function () { this.style.display = ""; });

    $("#btPCAddFile").click(function () {
        $("#upload").click();
    });

    $('#form1').ajaxForm({
        dataType: 'text',
        beforeSerialize: function () {
            //$('#data').attr('value', '야호');
        },
        beforeSubmit: function () {
            $('#fileList').html('uploading...');
        },

        success: function (data) {
            Send.getSpoolList();
        },
        error: function (data) {
            NoticeShow("!", R_ProcessingError);
            LoadingShow(false);
        }

    });


    if (MailID != "") {
        LoadingShow(true);
        setTimeout("Send.getDraft();", 400);
    }
    else {
        if (Action == "1") { //답장
            LoadingShow(true);
            setTimeout("Send.getData();", 400);
        }
        else if (Action == "2") { //전달
            LoadingShow(true);
            setTimeout("Send.getData();", 400);
        }
        else if (Action == "3") { //전체답장
            LoadingShow(true);
            setTimeout("Send.getData();", 400);
        }
        else {
            setTimeout("Send.setData();", 400);
        }
    }

    if (FileCheck == "Y") {
        Send.getSpoolList();
    }

    LoadingShow(false);

    $('#cbViewCC').click(function (e) {
        //e.preventDefault();
        Send.viewCC();
    });

    $('#btCancel').click(function (e) {
        Send.cancel();
    });

    $('#btSend').click(function (e) {
        //메일 발송은 사용하는 지 아닌 지 체크
        if (UseMailApprove == "Y") {
            Send.getRequest();
        }
        else {
            Send.send();
        }
    });

    $('#cbToMe').click(function (e) {
        Send.toMe();
    });

    $('#btTo').click(function (e) {
        Send.address('1');
    });

    $('#btCc').click(function (e) {
        Send.address('2');
    });

    $('#btBcc').click(function (e) {
        Send.address('3');
    });

    $('#listTitle').click(function (e) {
        Send.cancel();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    $('#tbTo').autogrow(); //받는 사람 자동 높이 조절

    $("#tbTo").autocomplete({
        source: function (request, response) {

            //파라미터값 배열에 담기
            ParamArray = {
                'txt': $("#tbTo").val()
            };

            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";


            $.ajax({
                url: "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetAutoCompleteData",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{p:'" + GetEncryptValue + "'}",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.MailValue
                        }
                    }))
                    //debugger;
                },
                error: function (result) {
                    //alert("Error");
                }
            });
        },
        open: function (event, ui) {
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
            }
        },
        select: function (event, ui) {
            $('#tbSubject').focus();
            $('#tbTo').focus();
        }
    });

    $("#tbCc").autocomplete({
        source: function (request, response) {

            //파라미터값 배열에 담기
            ParamArray = {
                'txt': $("#tbCc").val()
            };

            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";


            $.ajax({
                url: "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetAutoCompleteData",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{p:'" + GetEncryptValue + "'}",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.MailValue
                        }
                    }))
                    //debugger;
                },
                error: function (result) {
                    action_popup.alert("Error");
                }
            });
        },
        open: function (event, ui) {
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
            }
        },
        select: function (event, ui) {
            $('#tbSubject').focus();
            $('#tbCc').focus();
        }
    });

    $("#tbBcc").autocomplete({
        source: function (request, response) {

            //파라미터값 배열에 담기
            ParamArray = {
                'txt': $("#tbBcc").val()
            };

            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";


            $.ajax({
                url: "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetAutoCompleteData",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{p:'" + GetEncryptValue + "'}",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.MailValue
                        }
                    }))
                    //debugger;
                },
                error: function (result) {
                    action_popup.alert("Error");
                }
            });
        },
        open: function (event, ui) {
            if (navigator.userAgent.match(/(iPod|iPhone|iPad)/)) {
                $('.ui-autocomplete').off('menufocus hover mouseover mouseenter');
            }
        },
        select: function (event, ui) {
            $('#tbSubject').focus();
            $('#tbBcc').focus();
        }
    });

    /*
    if (FileCheck == "Y") {

    if (DraftListID > 0) {

    if (OptionTop == "1") {
    $("#topDay").show();
    }

    if (PopUp == "1") {
    $("#date_start").show();
    $("#date_end").show();
    }
    }

    Write.getSpoolList();
    }
    */
});

/**************************************************

작성일자 : 2011-07-11
작성목적 : 취소 버튼 
***************************************************/
Send.cancel = function () {

    if (History == "1") {
        if (Code != "") {
            ParamArray = {
                'Code': Code,
                'PageNo': PageNo,
                'MSv': SearchValue.securityText()
            };

            CallEncrypt(CallGetParam());

            location.href = "List.aspx?p=" + GetEncryptValue;
        }
        else {
            if (OwnerID != "") {
                ParamArray = {
                    'gubun': Gubun,
                    'ownerID': OwnerID,
                    'sv': SearchValue.securityText()
                };

                CallEncrypt(CallGetParam());

                location.href = "/DeskPlusMobile/Address/View.aspx?p=" + GetEncryptValue;
            }
            else {
                location.href = "/DeskPlusMobile/Address/List.aspx";
            }
        }

    }
    else if (History == "2") {
        ParamArray = {
            'Mail_sn': Mail_sn,
            'Code': Code,
            'PageNo': PageNo,
            'MSv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
    else if (History == "3") {
        location.href = "Folder.aspx";
    }
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 답장, 전달 메일 내용 가져오기 (웹서비스 호출)
***************************************************/
Send.getData = function () {
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetTotalContent";

    //파라미터값 배열에 담기
    ParamArray = {
        'Code': Code,
        'Mail_sn': Mail_sn
    };

    var obj = eval("Send.succeededGet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 답장, 전달 메일 내용 가져오기 (웹서비스 값 받기)
***************************************************/
Send.succeededGet = function (data, status) {

    jQuery.each($("#contentList"), function () { this.style.display = ""; });

    var tmpContent = "------Original Message-----";
    tmpContent += "<br>From : " + data.d[0].from;
    tmpContent += "<br>To : " + data.d[0].to;

    if (jQuery.trim(data.d[0].cc) != "") {
        tmpContent += "<br>Cc : " + data.d[0].cc;
    }

    tmpContent += "<br>Date : " + data.d[0].sendDate;

    $("#contentList").text(tmpContent);

    var tmp = $("#contentList").html().split("&lt;br&gt;").join("<br>");

    $("#contentList").empty();
    $("#contentList").append(tmp);
    $("#contentList").append("<br>subject: " + data.d[0].subject);
    $("#contentList").append("<br>" + data.d[0].content);

    var tmpFrom = "";
    if (data.d[0].from.indexOf("<") != -1) {
        tmpFrom = data.d[0].from.split("<")[1].split(">")[0];
    }
    else {
        tmpFrom = data.d[0].from;
    }

    if (MailID == "") {

        //1:답장, 2:전달, 3:전체답장
        if (Action == "1") {

            $("#tbTo").val(tmpFrom + ",");
            $('#tbTo').autogrow();
            $("#tbSubject").val("Re : " + data.d[0].subject);
        }
        else if (Action == "2") {
            $("#tbSubject").val("Fw : " + data.d[0].subject);

            Forward = data.d[0].uid;

            //첨부파일 표시
            $("#attach").empty();

            if (data.d[0].attachLists.length > 0) {
                jQuery.each($("#att_line"), function () { this.style.display = ""; });
                var tmpTag = "";

                for (var i = 0; i < data.d[0].attachLists.length; i++) {
                    tmpTag += data.d[0].attachLists[i].name + "<br>";
                }
                $("#attachList").append(tmpTag);
            }
            else {
                jQuery.each($("#att_line"), function () { this.style.display = "none"; });
            }

        }
        else if (Action == "3") {
            var tmpToAddress = "";
            var arrTmpTo1 = "";
            var arrTmpTo2 = "";
            var arrToAddress = data.d[0].to.split(",");

            for (var i = 0; i < arrToAddress.length; i++) {
                arrTmpTo1 = arrToAddress[i].split("<");

                if (arrTmpTo1.length > 1) {
                    arrTmpTo2 = arrTmpTo1[1].split(">");
                    tmpToAddress += jQuery.trim(arrTmpTo2[0]) + ",";
                }
                else {
                    tmpToAddress += jQuery.trim(arrToAddress[i]) + ",";
                }
            }
            tmpToAddress = tmpToAddress.split(MailAddress + ",").join("");
            tmpToAddress = tmpToAddress.split(MailAddress).join("");


            if (tmpFrom != MailAddress) {
                tmpToAddress = jQuery.trim(tmpFrom) + "," + tmpToAddress;
            }

            if (jQuery.trim(tmpToAddress) == "") {
                tmpToAddress = MailAddress + ",";
            }


            $("#tbTo").val(tmpToAddress);
            $('#tbTo').autogrow();

            if (jQuery.trim(data.d[0].cc) != "") {
                var tmpCcAddress = "";
                var arrTmpCc1 = "";
                var arrTmpCc2 = "";
                var arrCcAddress = data.d[0].cc.split(",");
                for (var i = 0; i < arrCcAddress.length; i++) {
                    arrTmpCc1 = arrCcAddress[i].split("<");

                    if (arrTmpCc1.length > 1) {
                        arrTmpCc2 = arrTmpCc1[1].split(">");
                        tmpCcAddress += jQuery.trim(arrTmpCc2[0]) + ",";
                    }
                    else {
                        tmpCcAddress += jQuery.trim(arrCcAddress[i]) + ","
                    }
                }

                tmpCcAddress = tmpCcAddress.split(MailAddress + ",").join("");
                tmpCcAddress = tmpCcAddress.split(MailAddress).join("");

                if (tmpCcAddress != "") {
                    $("#cbViewCC").attr("checked", true);
                    jQuery.each($("#cc"), function () { this.style.display = ""; });
                    $("#tbCc").val(tmpCcAddress);
                    $('#tbCc').autogrow();
                    $('#tbBcc').autogrow();
                }
            }
            $("#tbSubject").val("Re : " + data.d[0].subject);
        }
    }
    else {
        if (Action == "2") {
            Forward = data.d[0].uid;

            //첨부파일 표시
            $("#attach").empty();

            if (data.d[0].attachLists.length > 0) {
                jQuery.each($("#att_line"), function () { this.style.display = ""; });
                var tmpTag = "";

                for (var i = 0; i < data.d[0].attachLists.length; i++) {
                    tmpTag += data.d[0].attachLists[i].name + "<br>";
                }
                $("#attachList").append(tmpTag);
            }
            else {
                jQuery.each($("#att_line"), function () { this.style.display = "none"; });
            }
        }
    }

    Send.setData();
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 초기 세팅
***************************************************/
Send.setData = function () {
    if (Mail != "") {
        if (Hearder == 1) {
            var arrMail = Mail.split(';');

            for (var i = 0; i < arrMail.length; i++) {
                if ($("#tbTo").val().indexOf(arrMail[i]) < 0) {
                    var tmpTo = ($("#tbTo").val() != "") ? ($("#tbTo").val() + "\n" + arrMail[i] + ",") : (arrMail[i] + ",");
                    $("#tbTo").val(tmpTo);
                    $('#tbTo').autogrow();
                }
            }

        }
        else if (Hearder == 2) {
            var arrMail = Mail.split(';');

            for (var i = 0; i < arrMail.length; i++) {
                if ($("#tbCc").val().indexOf(arrMail[i]) < 0) {
                    var tmpCc = ($("#tbCc").val() != "") ? ($("#tbCc").val() + "\n" + arrMail[i] + ",") : (arrMail[i] + ",");
                    $("#cbViewCC").attr("checked", true);
                    jQuery.each($("#cc"), function () { this.style.display = ""; });
                    $("#tbCc").val(tmpCc);
                    $('#tbCc').autogrow();
                    $('#tbBcc').autogrow();
                }
            }
        }
        else if (Hearder == 3) {
            var arrMail = Mail.split(';');

            for (var i = 0; i < arrMail.length; i++) {
                if ($("#tbBcc").val().indexOf(arrMail[i]) < 0) {
                    var tmpBcc = ($("#tbBcc").val() != "") ? ($("#tbBcc").val() + "\n" + arrMail[i] + ",") : (arrMail[i] + ",");
                    $("#cbViewCC").attr("checked", true);
                    jQuery.each($("#cc"), function () { this.style.display = ""; });
                    $("#tbBcc").val(tmpBcc);
                    $('#tbBcc').autogrow();
                    $('#tbCc').autogrow();
                }
            }
        }
        else {
            var arrMail = Mail.split(';');

            for (var i = 0; i < arrMail.length; i++) {
                if ($("#tbTo").val().indexOf(arrMail[i]) < 0) {
                    var tmpTo = ($("#tbTo").val() != "") ? ($("#tbTo").val() + "\n" + arrMail[i] + ",") : (arrMail[i] + ",");
                    $("#tbTo").val(tmpTo);
                    $('#tbTo').autogrow();
                }
            }
        }
    }
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 임시 저장한 메일 가져오기 (웹서비스 호출)
***************************************************/
Send.getDraft = function () {
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetDraft";

    //파라미터값 배열에 담기
    ParamArray = {
        'MailID': MailID
    };

    var obj = eval("Send.succeededGetDraft");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 임시 저장한 메일 가져오기 (웹서비스 값 받기)
***************************************************/
Send.succeededGetDraft = function (data, status) {
    var draObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경  

    if (draObj.GetDraft[0].To_Header != "") {

        $("#tbTo").val(draObj.GetDraft[0].To_Header);
        $('#tbTo').autogrow();
    }

    if (draObj.GetDraft[0].Cc_Header != "") {

        $("#cbViewCC").attr("checked", true);
        jQuery.each($("#cc"), function () { this.style.display = ""; });
        $("#tbCc").val(draObj.GetDraft[0].Cc_Header);
        $('#tbCc').autogrow();
        $('#tbBcc').autogrow();
    }

    if (draObj.GetDraft[0].Bcc_Header != "") {

        $("#cbViewCC").attr("checked", true);
        jQuery.each($("#cc"), function () { this.style.display = ""; });
        $("#tbBcc").val(draObj.GetDraft[0].Bcc_Header);
        $('#tbBcc').autogrow();
        $('#tbCc').autogrow();
    }

    var tmpSub = (draObj.GetDraft[0].Subject != "") ? draObj.GetDraft[0].Subject : "";
    $("#tbSubject").val(tmpSub);
    var tmpBody = (draObj.GetDraft[0].Body != "") ? draObj.GetDraft[0].Body : "";
    $("#tbContent").val(tmpBody);

    if (Action == "1") { //답장
        Send.getData();
    }
    else if (Action == "2") { //전달
        Send.getData();
        //Send.getAttachFile();
    }
    else if (Action == "3") { //전체답장
        Send.getData();
    }
    else {
        Send.setData();
    }
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 주소록 가기 
***************************************************/
Send.address = function (arg) {

    var tohearder = $("#tbTo").val();          //받는 사람
    var cchearder = $("#tbCc").val();          //참조
    var bcchearder = $("#tbBcc").val();        //숨은 참조
    var subject = $("#tbSubject").val();       //제목
    var content = $("#tbContent").val();       //내용

    if ((tohearder == "") && (cchearder == "") && (bcchearder == "") && (subject == "") && (content == "")) {
        ParamArray = {
            'Action': Action,
            'Code': Code,
            'Mail_sn': Mail_sn,
            'Hearder': arg,
            'History': History,
            'SwfUploadGUID': SwfUploadGUID,
            'FileCheck': FileCheck
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Address/List.aspx?p=" + GetEncryptValue;
        return;
    }

    var arryTo = tohearder.split(',');
    var tmpTo = "";
    for (var i = 0; i < arryTo.length; i++) {
        if (arryTo[i] != "") {
            tmpTo += arryTo[i] + ",";
        }
    }

    var arryCc = cchearder.split(',');
    var tmpCc = "";
    for (var i = 0; i < arryCc.length; i++) {
        if (arryCc[i] != "") {
            tmpCc += arryCc[i] + ",";
        }
    }

    var arryBcc = bcchearder.split(',');
    var tmpBcc = "";
    for (var i = 0; i < arryBcc.length; i++) {
        if (arryBcc[i] != "") {
            tmpBcc += arryBcc[i] + ",";
        }
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'MailID': MailID,
        'To': tmpTo,
        'Cc': tmpCc,
        'Bcc': tmpBcc,
        'Subject': encodeURIComponent(subject),
        'Body': encodeURIComponent(content)
    };

    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Mail/Ws/SetMail.asmx/SetDraft",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d > 0) {
                ParamArray = {
                    'Action': Action,
                    'MailID': msg.d,
                    'Code': Code,
                    'Mail_sn': Mail_sn,
                    'Hearder': arg,
                    'History': History,
                    'SwfUploadGUID': SwfUploadGUID,
                    'FileCheck': FileCheck
                };

                CallEncrypt(CallGetParam());

                location.href = "/DeskPlusMobile/Address/List.aspx?p=" + GetEncryptValue;

            }
            else {
                NoticeShow("!", R_ErrorDraftMail);
            }
        },
        error: function (msg) {
            NoticeShow("!", R_ProcessingError);
            return;
        }
    });
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 이메일 체크  
***************************************************/
function isEmail(dateStr) {
    return (/\w+([-+.]\w+)*@\w+([-.]\w+)*\.[a-zA-Z]{2,4}$/).test(jQuery.trim(dateStr)) ? true : false;
}


/**************************************************

작성일자 : 2011-07-11
작성목적 : 메일 발송하기 (웹서비스 호출)
***************************************************/
Send.send = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    var tohearder = $("#tbTo").val();          //받는 사람
    var cchearder = $("#tbCc").val();          //참조
    var bcchearder = $("#tbBcc").val();        //숨은 참조
    var subject = $("#tbSubject").val();       //제목
    var content = $("#tbContent").val();       //내용

    // 제목 필수 체크 
    if (subject == "") {
        NoticeShow("!", R_InputSubject);
        return;
    }

    LoadingShow(true);

    var arryTo = tohearder.split(',');
    var tmpTo = "";
    for (var i = 0; i < arryTo.length; i++) {
        if (arryTo[i] != "") {
            tmpTo += arryTo[i] + ",";
        }
    }

    if (tmpTo == "") {
        NoticeShow("!", R_InputReceiver);
        LoadingShow(false);
        return;
    }

    //Action 1 : 답장 / Action 2 : 전달 / Action 3 : 전체답장입니다.
    if ((Action == "1") || (Action == "2") || (Action == "3")) {
        content = content.split("\n").join("<br>");
        //html() 은 인코딩하므로, 변환하여  순수글자를 만들어 준다. modify shimpark 2014-12-09
        content = content + "<br><br>" + $("#contentList").html().replace(/&lt;/g, '<').replace(/&gt;/g, '>');

        // 내용 필수 체크 
        if (content == "") {
            NoticeShow("!", R_Content);
            LoadingShow(false);
            return;
        }
    }
    else {
        // 내용 필수 체크 
        if (content == "") {
            NoticeShow("!", R_Content);
            LoadingShow(false);
            return;
        }

        content = content.split("\n").join("<br>");
    }

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/SendMail";

    //파라미터값 배열에 담기
    ParamArray = {
        'To': tohearder,
        'Cc': cchearder,
        'Bcc': bcchearder,
        'Subject': encodeURIComponent(subject),
        'Body': encodeURIComponent(content),
        'AttachList': AttachList,
        'AttachNameList': AttachNameList,
        'AttachRealNameList': encodeURIComponent(AttachRealNameList),
        'Forward': Forward,
        'Code': Code,
        'SwfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Send.succeededSend");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-07-11
작성목적 : 메일 발송하기 (웹서비스 값 받기)
***************************************************/
Send.succeededSend = function (data, status) {
    LoadingShow(false);
    if (data.d == "0") {
        action_popup.alert(R_SaveMail);
        location.href = "Folder.aspx";
    }
    else if (data.d == "1") {
        NoticeShow("!", R_AskAdmin);
        BtnClickCheck = "";
        return;
    }
    else {
        var tmpRtn = data.d.split('^');

        if (tmpRtn[0] == "3") {
            NoticeShow("!", tmpRtn[1] + R_MailError1);
            BtnClickCheck = "";
            return;
        }
        else if (tmpRtn[0] == "4") {
            NoticeShow("!", tmpRtn[1] + R_MailError2);
            BtnClickCheck = "";
            return;
        }
        else if (tmpRtn[0] == "5") {
            NoticeShow("!", tmpRtn[1] + R_MailError3);
            BtnClickCheck = "";
            return;
        }
        else {
            NoticeShow("!", R_AskAdmin);
            BtnClickCheck = "";
            return;
        }

    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-11
작성목적 : 참조/숨은참조 보기
***************************************************/
Send.viewCC = function () {
    if ($("#cbViewCC").is(':checked')) {
        $("#cc").show();
        $('#tbCc').autogrow();  //참조 자동 높이 조절
        $('#tbBcc').autogrow();  //숨은 참조 자동 높이 조절
    } else {
        $("#cc").hide();
    }
}

/**************************************************

작성일자 : 2011-07-12
작성목적 : 나에게 체크시 
***************************************************/
Send.toMe = function () {
    var tmpMyAddress = MailAddress + ",";

    if ($("#cbToMe").is(':checked')) {

        if ($("#tbTo").val().indexOf(tmpMyAddress) < 0) {
            var tmpVal = jQuery.trim($("#tbTo").val());
            var tmpEnd = tmpVal.substring(tmpVal.length - 1, tmpVal.length);
            if (tmpEnd == ",") {
                tmpEnd = "";
            }
            else {
                tmpEnd = ",";
            }

            var tmpTo = ($("#tbTo").val() != "") ? ($("#tbTo").val() + tmpEnd + MailAddress + ",") : (MailAddress + ",");
            $("#tbTo").val(tmpTo);
            $('#tbTo').autogrow();
        }
    }
    else {
        var tmpTo = $("#tbTo").val().split(tmpMyAddress).join("");

        $("#tbTo").val(tmpTo);
        $('#tbTo').autogrow();

    }
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 메일 첨부파일 정보 가져오기 (웹서비스 호출)
***************************************************/
Send.getAttachFile = function () {
    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetAttachFile";

    //파라미터값 배열에 담기
    ParamArray = {
        'Mail_sn': Mail_sn
    };

    var obj = eval("Send.succeededGetAttachFile");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************

작성일자 : 2011-11-14
작성목적 : 메일 첨부파일 정보 가져오기 (웹서비스 값 받기)
***************************************************/
Send.succeededGetAttachFile = function (data, status) {
    var attObj = eval('(' + data.d + ')'); //Json String 을 Json Object로 변경  

    $("#attach").empty();

    if (attObj.GetAttachFile.length > 0) {
        jQuery.each($("#att_line"), function () { this.style.display = ""; });
        var tmpTag = "";

        for (var i = 0; i < attObj.GetAttachFile.length; i++) {

            tmpTag += attObj.GetAttachFile[i].filename + "<br>";

            var tmpfileSn = attObj.GetAttachFile[i].file_sn.toString(16).padLeft(12, "0");
            var tmpfilePath = tmpfileSn;
            tmpfilePath = "3s" + tmpfilePath.substring(0, 2) + "/2s" + tmpfilePath.substring(2, 4) + "/1s" + tmpfilePath.substring(4, 6) + "/0s" + tmpfilePath.substring(6, 8) + "/p" + tmpfilePath.substring(8, 10);

            AttachList += tmpfilePath + "/;";
            AttachNameList += attObj.GetAttachFile[i].file_sn + ";";
            AttachRealNameList += attObj.GetAttachFile[i].filename + ";";
        }

        $("#attach").append(tmpTag);
    }
    else {
        jQuery.each($("#att_line"), function () { this.style.display = "none"; });
    }

}


/**************************************************

작성일자 : 2012-11-30
작성목적 : 발송승인자 확인 (웹서비스 호출)
***************************************************/
Send.getRequest = function () {

    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetRequest";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Send.succeededRequest");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

/**************************************************

작성일자 : 2012-11-30
작성목적 : 발송승인자 확인 (웹서비스 값 받기)
***************************************************/
Send.succeededRequest = function (data, status) {

    var tmpCount = data.d;

    if (tmpCount > 0) {
        NoticeShow("!", R_MailSendUser);
        return;
    }
    else {
        Send.send();
    }

}

/**************************************************
작성자   : 방영환
작성일자 : 2015-06-29
작성목적 : 파일 선택 
***************************************************/
function fileChange(obj) {

    NoticeHide();

    if (jQuery.trim($('#upload').val()) != "") {
        var file = obj.files;
        var tmpRtn = "Y";

        if (file) {
            for (var i = 0; i < file.length; i++) {

                if (file[i].size > UpFileSize) {
                    tmpRtn = "N";
                    var tmpMsg = bytesToSize(UpFileSize);
                    NoticeShow("!", tmpMsg + R_MaxFile);
                    return;
                }

                var fileExt = file[i].name.substr(file[i].name.lastIndexOf("."));
                fileExt = fileExt.toLowerCase();

                $.each(BlockExt.split(','), function (idx) {
                    if (fileExt == ("." + BlockExt.split(',')[idx])) {
                        tmpRtn = "N";
                        NoticeShow("!", BlockExt + R_CheckFileType);
                        return;
                    }
                });

            }

            if (tmpRtn == "Y") {
                FileCheck = "Y";
                LoadingShow(true);
                $("#form1").submit();
            }
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
Send.getSpoolList = function () {

    var url = "/DeskPlusMobile/Mail/Ws/GetMail.asmx/GetSpoolList";

    //파라미터값 배열에 담기
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Send.succeededSpool");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Send.succeededSpool = function (data, status) {

    $('#fileList').empty();

    if (data != null) {

        $.each(data.d, function (i, o) {

            $('#fileList').append(data.d[i].FileName + "  <img src='/DeskPlusMobile/Content/ko/Base/Img/ico_tree_listDel.gif' class='icon_Talk' onclick=\"javascript:Send.DelFile('" + data.d[i].FileName + "')\" /> <br/>");
        });

        if (data.d.length == 0) {
            jQuery.each($("#divFile"), function () { this.style.display = "none"; });
        }
        else {
            jQuery.each($("#divFile"), function () { this.style.display = ""; });
        }
    }

    if (FileCheck == "N") {
        $('#upload').val("");
        $('#upload').replaceWith($('#upload').clone(true));
    }

    LoadingShow(false);
}


/**************************************************
작성자   : famo23
작성일자 : 2015-06-29
작성목적 : Spool 폴더 파일삭제
***************************************************/
Send.DelFile = function (fileName) {
    var url = "/DeskPlusMobile/Mail/Ws/SetMail.asmx/DelSpoolFileList";

    //파라미터값 배열에 담기
    ParamArray = {
        'fileName': fileName,
        'swfUploadGUID': SwfUploadGUID
    };

    var obj = eval("Send.getSpoolList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    if (i == 0) return bytes + ' ' + sizes[i];
    return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
}
