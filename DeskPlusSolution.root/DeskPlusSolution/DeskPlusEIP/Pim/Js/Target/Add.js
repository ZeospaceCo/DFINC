Add = {}


$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //취소
        $('#CbtnCancel').click(function (e) {
            e.preventDefault();
            Add.Cancel();
        });

        //공유대상찾기
        $('#CbtnGetUser').click(function (e) {
            e.preventDefault();
            Add.ShareUser();
        });

        //파일추가
        $('#cbtnSwfFileAdd').click(function (e) {

            e.preventDefault();
            Add.AttachSwfFile();
        });

        Add.GetSpoolFileList();
        $('#startDate').datepick($.datepicker.regional[PreLang]);
        $('#endDate').datepick($.datepicker.regional[PreLang]);

        if (parent.FrameHeightControl != null) {
            var yHeight = 900;
            parent.FrameHeightControl(yHeight);
        }
    });
});



/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 목표 추가 취소 
***************************************************/
Add.Cancel = function () {
    document.location.href = "../Target/List.aspx";
}


/**************************************************
작성자   : 김광일
작성일자 : 2011-8-23
작성목적 : 저장
***************************************************/
Add.Confirm = function (arg) {

    if ($("#startDate").val() == "") {
        alert(Add_InputDate);
        return;
    }

    if ($("#endDate").val() == "") {
        alert(Add_InputDate);
        return;
    }

    var sdate = $("#startDate").val();
    var edate = $("#endDate").val();


    var dtResponse = CompareDT(sdate, edate, "lessequal", PreLang);
    if (!dtResponse) {
        alert(TimeAlert); //TimeAlert
        return;
    }


    if ($("#txttitle").val() == "") {

        alert(Add_InputTitle);
        return;
    }
      
    __doPostBack('lbtnWrite', arg);
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
    var DelImg = "<img src='" + ImgPath + '/Lang/' + Lang + "/ico_folderset_del.gif' alt='del' />";

    var strHtml = "<table width='100%' cellspacing='0' class='table_typeT'>";
    strHtml += "<caption></caption>";
    strHtml += "<colgroup><col  /><col col width='90px' /></colgroup>";
    strHtml += "<tr><th id='r_name'>" + RNAME + "</th><th id='r_title'>" + RDEL + "</th></tr>";


    var chk = 'F';
    for (var i = 0; i < arrayid.length - 1; i++) {
        var arrayName = arrayid[i].split('$');

        if (arrayName[0].length > 0) {
            strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
            strHtml = strHtml + "<td class='center'><a onclick='javascript:Add.ShareUserClick(this)' value='" + arrayid[i] + "' >" + DelImg + "</a></td>"
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
