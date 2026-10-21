ConfirmPassword = {}
var objPopup;

$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); if (objPopup != null) { objPopup.close(); } window.close(); if (parent.disablePopup) { parent.disablePopup(); } else { if (objPopup != null) { objPopup.close(); } self.close(); } });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ConfirmPassword.Confirm(); });

        //[서명]
        $("#cbtnSign").click(function (e) { e.preventDefault(); ConfirmPassword.Sign(); });

        //엔터 이벤트 기술
        $("#ctbPwd").keydown(function (e) { //alert(e.keyCode)
            if (e.keyCode == 13) { e.preventDefault(); ConfirmPassword.Confirm(); }
        });

        //입력값 체크하기
        if ($.browser.msie) {
            $('.charsRemaining').text($('#tbOpinion').attr('maxlength'));
            $('#tbOpinion').bind('keyup', function () {
                $(this).css('border', '1px solid #d2d2d2');
                var $this = $(this);
                var len = $this.val().length;
                var maxlength = $this.attr('maxlength')

                $('.charsRemaining').text(maxlength - len);
                if (maxlength && len > maxlength) {
                    $(this).css('border', '1px solid #f00');
                    alert(R_Msg_ErrOpinion);
                    $this.val($this.val().slice(0, maxlength));
                    $('.charsRemaining').text("0");
                    return;
                }
            });
        }
        else {
            ConfirmPassword.MaxLang();
        }

        //[추가 알람대상자]
        $("#addAlarm").click(function (e) { ConfirmPassword.ShowAlarmUser(); });
        $("#addAlarmDiv").hide();

        //패스워드사용유무 정보
        if (IsApprovePwd == "N") {
            $("#trpwd").hide();
        } else {
            $("#trpwd").show();
        }
        
        $("#ctbPwd").focus();

        //인장유무 
        if (IsSign == "N") {
            $("#Sign").show();
        } else {
            $("#Sign").hide();
        }        
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 추가 알람대상자
***************************************************/
ConfirmPassword.ShowAlarmUser = function () {

    if ($("#addAlarm").is(":checked")) {
        $("#addAlarmDiv").show();
        ConfirmPassword.AddAlarm();
    }
    else {
        $("#addAlarmDiv").hide();
    }
}

ConfirmPassword.AddAlarm = function () {

    var url = "/DeskPlusEIP/Site/Auth/LogInOutSide.aspx?DOMAINVALUE=0&USERID=" + LoginID + "&USERPASSWORD=" + Password + "&targetURL=/DeskPlusEIP/Approval/Process/Basic/AlarmList.aspx";
    objPopup = window.open(url, "AddAlarm", "width=800, height=474, menubar=no, location=no, resizable=yes, scrollbars=yes, status=no");
    objPopup.focus();
}

/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
CompareArrayVal = function () {

    $.each(tvCompare.split("^^"), function (i, o) {
        if (o != "") {
            var arr = o.split(",");
            ConfirmPassword.CompareArrayVal2(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
        }
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
ConfirmPassword.CompareArrayVal2 = function (compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName) {

    var response = -1; //비교값이 없을 경우, 그냥 추가한다.

    var Auth = $("#tvAuth").val();

    if (Auth != "") {

        var AuthArray = Auth.split("^^");

        for (var i = 0; i < AuthArray.length; i++) {

            if (AuthArray[i] != "") {

                var value = AuthArray[i].split(",");

                if (value[0] == compID) {
                    response = 1; //동일한 값이 존재할 경우
                    break;
                }
                else {
                    response = 0; //다른 값일 경우                
                }
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            ConfirmPassword.AddAuthRow(compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ConfirmPassword.AddAuthRow(compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName);
    }

    return response;
}

/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
ConfirmPassword.AddAuthRow = function (keyID, name, parentID, titleName, mail, phone, dutyName, deptName) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
            + "<td class='center'><input type='hidden' id='selUserList" + RowMax + "' class='selUserList' value='" + selStr[1] + "," + unescape(name) + "," + unescape(parentID) + "," + unescape(titleName) + "," + unescape(mail) + "," + unescape(phone) + "," + unescape(dutyName) + "," + unescape(deptName) + "' /> "
            + unescape(deptName)
			+ "</td>"
            + "<td class='center'>" + unescape(titleName) + "</td>"
			+ "<td class='center'>" + unescape(name) + "</td>"
			+ "<td class='center'><a onclick=ConfirmPassword.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "','" + parentID + "','" + titleName + "','" + mail + "','" + phone + "','" + dutyName + "','" + deptName + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    $("#tvAuth").val($("#tvAuth").val() + keyID + "," + name + "," + parentID + "," + titleName + "," + mail + "," + phone + "," + dutyName + "," + deptName + "^^");
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
ConfirmPassword.DelAuthRow = function (index, keyID, keyName, keyParentID, keyTitleName, keyMail, keyPhone, keyDutyName, keyDeptName) {
    var row = $("#boardAuth" + index);

    row.remove();

    var Auth = $("#tvAuth").val();
    Auth = Auth.replace(keyID + "," + keyName + "," + keyParentID + "," + keyTitleName + "," + keyMail + "," + keyPhone + "," + keyDutyName + "," + keyDeptName + "^^", "");
    $("#tvAuth").val(Auth);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 글자 제한
***************************************************/
ConfirmPassword.MaxLang = function () {

    var onEditCallback = function (remaining) {
        $('.charsRemaining').text(remaining);
        if (remaining > 0) { $(this).css('border', '1px solid #d2d2d2'); }
    }
    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }
    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}

/**********************************************************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
**********************************************************************************************/
ConfirmPassword.Confirm = function () {

    if (IsApprovePwd == "N") {

        ConfirmPassword.CallFunc();

    } else {

        //필수입력
        var validator = new Validator($("#form1"));
        validator.checkRequired("ctbPwd", R_Password2, true);   //validator.checkMaxLength("tbOpinion", 200, R_Msg_ErrOpinion, true);
        if (!validator.validate()) return;

        var pwd = $("#ctbPwd").val();
        ParamArray = { 'pwd': encodeURIComponent(pwd) };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ConfirmPassword.SuccessConfirm");
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/ConfirmPassword.asmx/GetIsPwd";

        AjaxService(url, data, obj);
    }
}
ConfirmPassword.SuccessConfirm = function (data, status) {

    if (data.d == "Y") {
        ConfirmPassword.CallFunc();
    } else {
        alert(R_Password3); $("#ctbPwd").focus(); return;
    }
}
ConfirmPassword.CallFunc = function () {

    targetObj = (window.dialogArguments) ? window.dialogArguments : parent;

    var ChildDocID = targetObj.DocumentID;

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ConfirmPassword.SuccessCallFunc");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetDocCont";

    AjaxService(url, data, obj);
}
ConfirmPassword.SuccessCallFunc = function (data, status) {
    if (data.d != "") {
        $("#htaFormHTML", targetObj.document).val(data.d);
        targetObj.Approve.GetContentHtmlList();
    }
    setTimeout("ConfirmPassword.CallFuncAfter();", 600);
}

ConfirmPassword.CallFuncAfter = function () {

    var targetDoc = (window.dialogArguments) ? window.dialogArguments.document : parent.document;

    $("#tbOpinion", targetDoc).val($("#tbOpinion").val().replace(/\n/gi, "<br>").replace(/ /g, "&nbsp;"));    //첨언처리

    //알람대상자 모두에게
    if ($("#allAlarm").is(":checked")) {
        $("#tbAllAlarmInfo", targetDoc).val("1");
    }
    else {
        $("#tbAllAlarmInfo", targetDoc).val("0");
    }

    //추가 알람대상자
    if ($("#addAlarm").is(":checked")) {
        var AddAlarmInfo = "";

        $.each($(".selUserList"), function (i, o) {
            AddAlarmInfo += o.value + "^";
        });

        $("#tbAddAlarmInfo", targetDoc).val(AddAlarmInfo.trimEnds('^'));
    }
    else {
        $("#tbAddAlarmInfo", targetDoc).val("");
    }

    if (window.dialogArguments) {
        window.returnValue = true;
        window.close();
    } else {
        if (parent.ConfirmPasswordFunc) parent.ConfirmPasswordFunc(ReturnType);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-20
작성목적 : 사인하기
**********************************************************************************************/
ConfirmPassword.Sign = function () {
    var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
    PopupWin(300, 293, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-20
작성목적 : 사인 리로드
**********************************************************************************************/
function SignConfirm(path) {

    window.dialogArguments.SealPath = FullDomainPath + path;
}