ConfirmReject = {}
var objPopup;

$(function () {

    $(document).ready(function () {

        //[닫기]
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            var target = (window.dialogArguments) ? window.dialogArguments : parent;

            if (target.Add.AfterLine) target.Add.AfterLine();

            if (parent.disablePopup) {
                parent.disablePopup();
            }
            else {
                if (objPopup != null) {
                    objPopup.close();
                }
                self.close();
            } 
        });

        //[확인]
        $("#cbtnConfirm").click(function (e) { e.preventDefault(); ConfirmReject.Confirm(); });

        //엔터 이벤트 기술
        $("#ctbPwd").keydown(function (e) { //alert(e.keyCode)
            if (e.keyCode == 13) { e.preventDefault(); ConfirmReject.Confirm(); }
        });

        //[추가 알람대상자]
        $("#addAlarm").click(function (e) { ConfirmReject.ShowAlarmUser(); });

        $("#addAlarmDiv").hide();

        //패스워드사용유무 정보
        if (IsApprovePwd == "N") {
            $("#trpwd").hide();
        } else {
            $("#trpwd").show();
        }

        $("#ctbPwd").focus();
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 추가 알람대상자
***************************************************/
ConfirmReject.ShowAlarmUser = function () {

    if ($("#addAlarm").is(":checked")) {
        $("#addAlarmDiv").show();
        ConfirmReject.AddAlarm();
    }
    else {
        $("#addAlarmDiv").hide();
    }
}

ConfirmReject.AddAlarm = function () {

    var url = "/DeskPlusEIP/Site/Auth/LogInOutSide.aspx?DOMAINVALUE=0&USERID=" + LoginID + "&USERPASSWORD=" + Password + "&targetURL=/DeskPlusEIP/Approval/Process/Basic/AlarmList.aspx";
    objPopup = window.open(url, "AddAlarm", "width=800, height=467, menubar=no, location=no, resizable=yes, scrollbars=yes, status=no");
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
            ConfirmReject.CompareArrayVal2(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7]);
        }
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 넘어온 권한값과 권한 배열값과 비교하여 중복 체크
비교 대상자가 없으면 -1, 중복되면 1 , 중복되지 않으면 0
***************************************************/
ConfirmReject.CompareArrayVal2 = function (compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName) {

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
            ConfirmReject.AddAuthRow(compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName);
        }
    }
    else {
        response = 0; //다른 값일 경우
        ConfirmReject.AddAuthRow(compID, compName, compParentID, compTitleName, compMail, compPhone, compDutyName, compDeptName);
    }

    return response;
}

/**************************************************
작성자   : shimpark
작성목적 : 추가 table row 생성하고 권한값 AuthArray[] 배열에 할당하기
***************************************************/
ConfirmReject.AddAuthRow = function (keyID, name, parentID, titleName, mail, phone, dutyName, deptName) {
    var rows = $("#contentTable tr");
    RowMax = RowMax + 1;

    var selStr = keyID.split('$');

    $("#contentTable").append(
		"<tr id='boardAuth" + RowMax + "'>"
            + "<td class='center'><input type='hidden' id='selUserList" + RowMax + "' class='selUserList' value='" + selStr[1] + "," + unescape(name) + "," + unescape(parentID) + "," + unescape(titleName) + "," + unescape(mail) + "," + unescape(phone) + "," + unescape(dutyName) + "," + unescape(deptName) + "' /> "
            + unescape(deptName)
			+ "</td>"
            + "<td class='center'>" + unescape(titleName) + "/" + unescape(dutyName) + "</td>"
			+ "<td class='center'>" + unescape(name) + "</td>"
			+ "<td class='center'><a onclick=ConfirmReject.DelAuthRow(" + RowMax + ",'" + keyID + "','" + name + "','" + parentID + "','" + titleName + "','" + mail + "','" + phone + "','" + dutyName + "','" + deptName + "')><img src='" + ImagePath + "/Lang/" + PreLang + "/folderset_del.gif' /></a></td>"
		+ "</tr>"
	);

    $("#tvAuth").val($("#tvAuth").val() + keyID + "," + name + "," + parentID + "," + titleName + "," + mail + "," + phone + "," + dutyName + "," + deptName + "^^");
}

/**************************************************
작성자   : shimpark
작성목적 : 권한값 table 특정 row 삭제하기
***************************************************/
ConfirmReject.DelAuthRow = function (index, keyID, keyName, keyParentID, keyTitleName, keyMail, keyPhone, keyDutyName, keyDeptName) {
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
ConfirmReject.MaxLang = function () {

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
ConfirmReject.Confirm = function () {
    if (IsApprovePwd == "N") {
        ConfirmReject.CallFunc();
    }
    else {
        var pwd = $("#ctbPwd").val();
        if (pwd == "") { alert(R_Password2); $("#ctbPwd").focus(); return; }

        ParamArray = { 'pwd': encodeURIComponent(pwd) };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ConfirmReject.SuccessConfirm");
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/ConfirmPassword.asmx/GetIsPwd";

        AjaxService(url, data, obj);
    }
}
ConfirmReject.SuccessConfirm = function (data, status) {

    if (data.d == "Y") {
        ConfirmReject.CallFunc();
    } else {
        alert(R_Password3); $("#ctbPwd").focus(); return;
    }
}

ConfirmReject.CallFunc = function () {
    var targetInfo = $("input[name='rblReject']:checked").val();
    var targetDoc = (window.dialogArguments) ? window.dialogArguments.document : parent.document;
    $("#tbTargetInfo", targetDoc).val(targetInfo);            //반려대상

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
        window.returnValue = targetInfo;
        window.close();
    } else {
        if (parent.ConfirmRejectFunc) parent.ConfirmRejectFunc(targetInfo);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}
