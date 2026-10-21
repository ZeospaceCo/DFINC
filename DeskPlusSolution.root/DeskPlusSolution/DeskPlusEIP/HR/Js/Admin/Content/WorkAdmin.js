WorkAdmin = {}

$(function () {
    $(document).ready(function () {

        //기본근무시간 설정 저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            WorkAdmin.Confirm();
        });

        //유연근무 대상 설정
        $('#cbtnUserAdd').click(function (e) {
            e.preventDefault();
            WorkAdmin.WorkEtcAdd();
        });

        //IP 추가
        $('#cbtnADDIP').click(function (e) {
            e.preventDefault();
            WorkAdmin.AddIP();
        });

        $('#ddlCecomYN').change(function () {
            if ($('#ddlCecomYN').val() == "Y") {
                $('#ddlCecomYN').val("N");
                alert(R_CecomReady);
            }
        });

        WorkAdmin.IPList();
        WorkAdmin.GetWorkEtcList(); //유연근무대상 리스트

    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : [확인]
***************************************************/
WorkAdmin.Confirm = function () {

    var sTime = $("#ddlStartTime option").index($("#ddlStartTime option:selected"));
    var eTime = $("#ddlEndTime option").index($("#ddlEndTime option:selected"));

    if (eTime <= sTime) {
        alert(R_StartDateEndDiffer);
        return;
    }

    if ((eTime - sTime) != 18) {
        alert(R_BaseWorkTimeError);
        return;
    }

    __doPostBack('lbtnSubmit', "MOD");
}


/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : 유연근무 대상 리스트
***************************************************/
WorkAdmin.GetWorkEtcList = function () {
    ParamArray = {};
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("WorkAdmin.ScsWorkEtcList");
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/WorkAdmin.asmx/GetWorkEtcList";
    AjaxService(url, data, obj);
}
WorkAdmin.ScsWorkEtcList = function (data, status) {
    $('#phWorkEtc').setTemplateURL('/DeskPlusEIP/HR/Jt/Admin/Content/JTWorkEtcList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phWorkEtc').processTemplate(data.d);
    setTimeout(function () { WorkAdmin.Resource(); }, 100);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : 유연근무 대상 추가
***************************************************/
WorkAdmin.WorkEtcAdd = function () {
    ParamArray = { 'func': 'WorkEtcAddWs', 'NoGroup': '' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/HR/Admin/Content/GetUserDept.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "WorkEtcAdd", "500", "603");
}
/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : 대상자추가(웹서비스호출)
***************************************************/
WorkEtcAddWs = function (_selv, _wsTime, _wctime, _searchWeek) {
    if (_selv != "") {
        var arrID = "";
        var selStr = _selv.split(';');
        $.each(selStr, function (i, v) {
            var arrVal = v.split('|');
            arrID += arrVal[0] + "$" + arrVal[1] + ";";        /* G$아이디;M$아이디; */
        });

        ParamArray = { 'arrID': arrID, 'wsTime': _wsTime, 'wctime': _wctime, 'searchWeek': _searchWeek };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("WorkAdmin.GetWorkEtcList");  //유연근무 대상 리스트
        var url = "/DeskPlusEIP/HR/Ws/Admin/Content/WorkAdmin.asmx/InsWorkEtc";
        AjaxService(url, data, obj);
    }
}
/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : 관리자삭제
***************************************************/
WorkAdmin.WorkEtcDel = function (_workEtcID) {
    if (window.confirm(R_Delete)) {
        ParamArray = { 'workEtcID': _workEtcID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("WorkAdmin.GetWorkEtcList");  //관리자리스트
        var url = "/DeskPlusEIP/HR/Ws/Admin/Content/WorkAdmin.asmx/DelWorkEtc";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018.11.29
작성목적 : 다국어처리
***************************************************/
WorkAdmin.Resource = function () {
    $("#r_DeptName").text(R_DeptName);
    $("#r_TitleName").text(R_TitleName);
    $("#r_Name").text(R_Name);
    $("#r_EtcWorkTime").text(R_EtcWorkTime);
    $("#r_Del").text(R_Del);
    $("#r_Gubun").text(R_Gubun);
    $("#r_ip")[0].innerHTML = R_IPBand;
    $("#r_delIP")[0].innerHTML = R_Del;
    $("#r_WeekDay")[0].innerHTML = R_WeekDay;
    $(".btn_del").text(R_Del);

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : IP 추가
**********************************************************************************************/
WorkAdmin.AddIP = function () {

    ParamArray = {

    };

    CallEncrypt(CallGetParam());
    var url = "AddIP.aspx?p=" + GetEncryptValue;

    PopupWin(500, 120, url, "", "", true);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2011-04-14
작성목적 : IP  리스트
**********************************************************************************************/
WorkAdmin.IPList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Admin/Content/WorkAdmin.asmx/GetIPList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID
    };

    var obj = eval("WorkAdmin.SuccessIPList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2011-04-14
작성목적 : 성공시, IP  리스트
**********************************************************************************************/
WorkAdmin.SuccessIPList = function (data, status) {
    $('#placeholder_IP').setTemplateURL('/DeskPlusEIP/HR/Jt/Admin/Content//JTIP.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_IP').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2011-04-14
작성목적 : IP 삭제
**********************************************************************************************/
WorkAdmin.DelIP = function (p_id) {

    if (window.confirm(R_Delete)) {
        var url = "/DeskPlusEIP/HR/Ws/Admin/Content/WorkAdmin.asmx/DelIP";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'IPID': p_id
        };

        var obj = eval("WorkAdmin.SuccessDelIP");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2011-04-14
작성목적 : 성공시, IP  삭제
**********************************************************************************************/
WorkAdmin.SuccessDelIP = function (data, status) {

    if (data.d == true) {
        WorkAdmin.IPList();
        WorkAdmin.Resource();
    }
}