Mobile = {}

$(function () {
    $(document).ready(function () {        

        //기본정보 이동
        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
        });

        //수정
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            Mobile.Confirm();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Mobile.Del();
        });

        //닫기
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //고유기값 정보 
        $('#ddlUUID').change(function () {
            Mobile.GetListInfo();
        });

        // 사용중인 기기 말고는 승인일, 로그인 요청정보, 알람설정이 안 보이게 처리
        if (ActiveMobile != "1") {
            $("#Approve").attr("style", "display:none");
            $("#Alarm").attr("style", "display:none");
        }

    });
});

/**************************************************
작성자 : famo23
작성일자 : 2015-10-07
작성목적 : UUID에 맞는 정보 가져오기 (웹서비스 호출)
***************************************************/
Mobile.GetListInfo = function () {

    var tmpListID = $("#ddlUUID").val();

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'listID': tmpListID
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mobile.SuccessList");
    var url = "/DeskPlusEIP/MDM/Ws/Device/DeviceInfo.asmx/GetListInfo";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-07
작성목적 : UUID에 맞는 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Mobile.SuccessList = function (data, status) {
    if (data.d) {

        $("#CTBDeviceName").val(data.d[0].DeviceName);
        $("#CTBMobilePhone").val(data.d[0].MobileNumber);
        $("#CTBMobileType").text(data.d[0].MobileType);
        $("#CTBRequestDate").text(data.d[0].RequestDateStr);               

        ActiveMobile = data.d[0].ActiveMobile;

        if (ActiveMobile == "0") {
            $("#CTBActiveMobile").text(R_ActiveMobile0);
            $("#Approve").attr("style", "display:none");
            $("#Alarm").attr("style", "display:none");
        }
        else if (ActiveMobile == "1") {
            $("#CTBActiveMobile").text(R_ActiveMobile1);
            $("#Approve").attr("style", "display:");
            $("#Alarm").attr("style", "display:");

            $("#CTBApproveDate").text(data.d[0].ApproveDateStr); 
            $("#ddlLoginType").val(data.d[0].LoginType);
            $("#ddlMailAlarm").val(data.d[0].MailAlarm);

            var etcAlarm = data.d[0].EtcAlarm;

            if (etcAlarm.indexOf("Approval") > -1) {
                $("#ddlApprovalAlarm").val("Y");
            }
            else {
                $("#ddlApprovalAlarm").val("N");
            }

            if (etcAlarm.indexOf("Board") > -1) {
                $("#ddlBoardAlarm").val("Y");
            }
            else {
                $("#ddlBoardAlarm").val("N");
            }

            if (etcAlarm.indexOf("Pim") > -1) {
                $("#ddlPimsAlarm").val("Y");
            }
            else {
                $("#ddlPimsAlarm").val("N");
            }

            if (etcAlarm.indexOf("TASK") > -1) {
                $("#ddlTaskAlarm").val("Y");
            }
            else {
                $("#ddlTaskAlarm").val("N");
            }

            if (etcAlarm.indexOf("Report") > -1) {
                $("#ddlWorkAlarm").val("Y");
            }
            else {
                $("#ddlWorkAlarm").val("N");
            }

            if (etcAlarm.indexOf("Community") > -1) {
                $("#ddlCommunityAlarm").val("Y");
            }
            else {
                $("#ddlCommunityAlarm").val("N");
            }

            if (etcAlarm.indexOf("ResourceManager") > -1) {
                $("#ddlResourceAlarm").val("Y");
            }
            else {
                $("#ddlResourceAlarm").val("N");
            }

            if (etcAlarm.indexOf("SNS") > -1) {
                $("#ddlSNSAlarm").val("Y");
            }
            else {
                $("#ddlSNSAlarm").val("N");
            }

            if (etcAlarm.indexOf("Notice") > -1) {
                $("#ddlNoticeAlarm").val("Y");
            }
            else {
                $("#ddlNoticeAlarm").val("N");
            }

        }
        else if (ActiveMobile == "2") {
            $("#CTBActiveMobile").text(R_ActiveMobile2);
            $("#Approve").attr("style", "display:none");
            $("#Alarm").attr("style", "display:none");
        }
        else {
            $("#CTBActiveMobile").text(R_ActiveMobile3);
            $("#Approve").attr("style", "display:none");
            $("#Alarm").attr("style", "display:none");
        }

        $("#ctbDesc").val(data.d[0].Memo);       

    }
    else {
        alert(R_Private_MyInfo_MSG_ModNo);
    }
}



/**************************************************
작성자 : famo23
작성일자 : 2015-10-08
작성목적 : 수정 (웹서비스 호출)
***************************************************/
Mobile.Confirm = function () {

    if (ActiveMobile != "1") {
        alert(R_ModMsg);
        return;
    }

    var tmpListID = $("#ddlUUID").val();

    var tmpLoginType = $("#ddlLoginType").val();
    var tmpMemo = $("#ctbDesc").val();

    var tmpMailAlarm = $("#ddlMailAlarm").val();
    var tmpEtcAlarm = "";

    if ($("#ddlApprovalAlarm").val() == "Y") {
        tmpEtcAlarm += "Approval,";
    }

    if ($("#ddlBoardAlarm").val() == "Y") {
        tmpEtcAlarm += "Board,";
    }

    if ($("#ddlPimsAlarm").val() == "Y") {
        tmpEtcAlarm += "Pim,";
    }

    if ($("#ddlTaskAlarm").val() == "Y") {
        tmpEtcAlarm += "TASK,";
    }

    if ($("#ddlWorkAlarm").val() == "Y") {
        tmpEtcAlarm += "Report,";
    }

    if ($("#ddlCommunityAlarm").val() == "Y") {
        tmpEtcAlarm += "Community,";
    }

    if ($("#ddlResourceAlarm").val() == "Y") {
        tmpEtcAlarm += "ResourceManager,";
    }

    if ($("#ddlSNSAlarm").val() == "Y") {
        tmpEtcAlarm += "SNS,";
    }

    if ($("#ddlNoticeAlarm").val() == "Y") {
        tmpEtcAlarm += "Notice,";
    }

    ParamArray = {
        'userID': UserID,
        'listID': tmpListID,
        'mailAlarm': tmpMailAlarm,
        'etcAlarm': tmpEtcAlarm,
        'loginType': tmpLoginType,
        'memo': tmpMemo
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mobile.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Device/DeviceInfo.asmx/Mod";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-08
작성목적 : 수정 (웹서비스 값 받기)
**********************************************************************************************/
Mobile.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_Private_MyInfo_MSG_ModOK);
    }
    else {
        alert(R_Private_MyInfo_MSG_ModNo);
    }
}


/**************************************************
작성자 : famo23
작성일자 : 2015-10-08
작성목적 : 삭제 (웹서비스 호출)
***************************************************/
Mobile.Del = function () {

    var tmpListID = $("#ddlUUID").val();
    var tmpUUID = $("#ddlUUID option:selected").text();

    if (tmpListID != null) {

        if (window.confirm(tmpUUID + " " + R_Mobile + "\n" + R_Admin_DeptManager_MSG_DelChk)) {
            ParamArray = {
                'userID': UserID,
                'listID': tmpListID
            };

            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Mobile.SuccessDel");
            var url = "/DeskPlusEIP/MDM/Ws/Device/DeviceInfo.asmx/Del";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_DelNoMobile);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-08
작성목적 : 삭제 (웹서비스 값 받기)
**********************************************************************************************/
Mobile.SuccessDel = function (data, status) {
    if (data.d) {
        alert(R_Private_MyInfo_MSG_ModOK);
        document.location.reload();
    }
    else {
        alert(R_Private_MyInfo_MSG_ModNo);
    }
}
