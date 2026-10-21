Mod = {}

$(function () {
    $(document).ready(function () {

        //저장후 계속


        // 저장
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            if (BtnCheck == "Y") {
                alert(WaitMent);
                return;
            }

            Mod.Confirm("MOD");
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




        $('#startDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });


        //첨부파일 추가
        $('#CbtnFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachFile();
        });

        Mod.GetAttachFileList();
        Mod.GetSpoolFileList();

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

    var validator = new Validator($("#form1"));     

    validator.checkRequired("txttitle", Add_InputTitle, true);
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

    validator.checkRequired("startDate", Add_InputDate, true);
    if (!validator.validate()) return;

    BtnCheck = "Y";

    __doPostBack('lbtnWrite', arg);

    //제목    
    //날짜
    //내용
    //알람
    //파일


}


/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 일정 추가 취소 
***************************************************/
Mod.Cancel = function () {
    

    ParamArray = {
        'Searchkey': Searchkey,
        'SearchValue': SearchValue,
        'CurrentPage': CurrentPage,
        'Starttime': Starttime,
        'Endtime': Endtime,
        'order': Order
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
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
    
    var url = "/DeskPlusEIP/Pim/Ws/Anniversary/Mod.asmx/GetAttachFileList";

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
        
        var url = "/DeskPlusEIP/Pim/Ws/Anniversary/Mod.asmx/DelAttachFile";
        AjaxService(url, data, obj);

        
    }

}
function SuccessDel(data, status) {
    Mod.GetAttachFileList();
}