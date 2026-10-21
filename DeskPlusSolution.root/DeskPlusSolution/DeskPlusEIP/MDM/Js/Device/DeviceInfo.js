DeviceInfo = {}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-18
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            DeviceInfo.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            DeviceInfo.Confirm();
        });

        //목록 
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            DeviceInfo.Cancel();
        });

        //고유기값 정보 
        $('#ddlUUID').change(function () {
            DeviceInfo.GetListInfo();
        });

    });
});


/**************************************************************
작성자 : famo23
작성일자 : 2013-01-18
작성목적 : 목록
**************************************************************/
DeviceInfo.Cancel = function () {

    ParamArray = {                
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order        
    };

    CallEncrypt(CallGetParam());

    //location.href = "../Dept/List.aspx?p=" + GetEncryptValue; 
    location.href = ListPage + "?p=" + GetEncryptValue; 

}


/**************************************************
작성자 : famo23
작성일자 : 2013-01-18
작성목적 : 수정 (웹서비스 호출)
***************************************************/
DeviceInfo.Confirm = function () {

    var tmpListID = $("#ddlUUID").val();

    var tmpLoginType = $("#ddlLoginType").val();
    var tmpActiveMobile = $('input[name="rblActiveMobile"]:checked').val();
    var tmpMemo = $("#ctbDesc").val();

    var tmpAccessRight = $('input[name="rblAccessRight"]:checked').val();

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
        'accessRight': tmpAccessRight,
        'loginType': tmpLoginType,
        'activeMobile': tmpActiveMobile,
        'memo': tmpMemo
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("DeviceInfo.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Device/DeviceInfo.asmx/Confirm";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-18
작성목적 : 수정 (웹서비스 값 받기)
**********************************************************************************************/
DeviceInfo.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2013-01-18
작성목적 : 삭제 (웹서비스 호출)
***************************************************/
DeviceInfo.Del = function () {

    var tmpListID = $("#ddlUUID").val();
    var tmpUUID = $("#ddlUUID option:selected").text();

    if (tmpListID != null) {

        if (window.confirm(tmpUUID + " " + R_Mobile + "\n" + R_ConfirmDel)) {
            ParamArray = {
                'userID': UserID,
                'listID': tmpListID
            };

            CallEncrypt(CallGetParam());

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("DeviceInfo.SuccessDel");
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
작성일자 : 2013-01-18
작성목적 : 삭제 (웹서비스 값 받기)
**********************************************************************************************/
DeviceInfo.SuccessDel = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
        DeviceInfo.Cancel();
    }
    else {
        alert(R_ProcessingError);
    }
}


/**************************************************
작성자 : famo23
작성일자 : 2013-02-15
작성목적 : UUID에 맞는 정보 가져오기 (웹서비스 호출)
***************************************************/
DeviceInfo.GetListInfo = function () {

    var tmpListID = $("#ddlUUID").val();

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'listID': tmpListID        
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("DeviceInfo.SuccessList");
    var url = "/DeskPlusEIP/MDM/Ws/Device/DeviceInfo.asmx/GetListInfo";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-02-15
작성목적 : UUID에 맞는 정보 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
DeviceInfo.SuccessList = function (data, status) {
    if (data.d) {
        $("#CTBDeviceName").val(data.d[0].DeviceName);
        $("#CTBMobilePhone").val(data.d[0].MobileNumber);
        $("#CTBMobileType").text(data.d[0].MobileType);
        $("#CTBRequestDate").text(data.d[0].RequestDateStr);
        $("#CTBApproveDate").text(data.d[0].ApproveDateStr);
        $("#ddlLoginType").val(data.d[0].LoginType);

        if (data.d[0].ActiveMobile == "1") {
            $("#rblActiveMobile_0").attr("checked", "checked");
        }
        else {
            $("#rblActiveMobile_1").attr("checked", "checked");
        }

        $("#ctbDesc").val(data.d[0].Memo);

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
    else {
        alert(R_ProcessingError);
    }
}