/**************************** Basic *********************************/
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

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //파일추가
        $('#cbtnSwfFileAdd').click(function (e) {

            e.preventDefault();
            Add.AttachSwfFile();
        });



        $('#startDate').datepick($.datepicker.regional[PreLang]);

        $('#startDate').focus(function (e) {
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
});
/**************************** /Basic *********************************/

/**************************** Event *********************************/
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

    var validator = new Validator($("#form1"));

    validator.checkRequired("txttitle", Add_InputTitle, true);
    if (!validator.validate()) return;

    //validator.checkRequired("tbHiddenText_contents", Add_InputContent, true);
    //if (!validator.validate()) return;

    validator.checkRequired("startDate", Add_InputDate, true);
    if (!validator.validate()) return;

    var sdate = $("#startDate").val();
    var edate = $("#startDate").val();

    sdate = sdate.replace(/-/gi, '');
    edate = edate.replace(/-/gi, '');

    var stime = $("#ddlstartTime").val();
    var etime = $("#ddlendTime").val();

    stime = stime.replace(/:/gi, '');
    etime = etime.replace(/:/gi, '');

    sdate = sdate + stime;
    edate = edate + etime;

    var sInt = parseInt(sdate, 10);
    var eInt = parseInt(edate, 10);

    if (sInt >= eInt) {
        alert(TimeAlert); //TimeAlert
        return;
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

    ParamArray = {
        'SearchField': SearchField,
        'Searchkey': Searchkey
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
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

/**************************** /utill *********************************/

/**************************** etc *********************************/
/**************************** /etc *********************************/


/**************************************************
작성자   : 김광일
작성일자 : 2011-9-8
작성목적 : 시간선택
***************************************************/
Add.SettingTime = function () {
   

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

