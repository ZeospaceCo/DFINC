AddIP = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddIP.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#tbLicenseKey').focus();
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 
작성목적 : ip 저장하기
***************************************************/
AddIP.Confirm = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddIP.asmx/AddPortletIP";

    var validator = new Validator($("#form1"));
    validator.checkRequired("start1", R_InputIP, true);
    validator.checkRequired("start2", R_InputIP, true);
    validator.checkRequired("start3", R_InputIP, true);
    validator.checkRequired("start4", R_InputIP, true);

    validator.checkRequired("end1", R_InputIP, true);
    validator.checkRequired("end2", R_InputIP, true);
    validator.checkRequired("end3", R_InputIP, true);
    validator.checkRequired("end4", R_InputIP, true);

    validator.checkOnlyNumber("start1", R_InputIPNumber, true);
    validator.checkOnlyNumber("start2", R_InputIPNumber, true);
    validator.checkOnlyNumber("start3", R_InputIPNumber, true);
    validator.checkOnlyNumber("start4", R_InputIPNumber, true);

    validator.checkOnlyNumber("end1", R_InputIPNumber, true);
    validator.checkOnlyNumber("end2", R_InputIPNumber, true);
    validator.checkOnlyNumber("end3", R_InputIPNumber, true);
    validator.checkOnlyNumber("end4", R_InputIPNumber, true);

    if (!validator.validate()) return;    

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID,
        'startIP1': $("#start1").val(),
        'startIP2': $("#start2").val(),
        'startIP3': $("#start3").val(),
        'startIP4': $("#start4").val(),
        'endIP1': $("#end1").val(),
        'endIP2': $("#end2").val(),
        'endIP3': $("#end3").val(),
        'endIP4': $("#end4").val(),
        'ipAccess': $("#ddlIPAccess").val()
    };

    var obj = eval("AddIP.SuccessAddIP");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 
작성목적 : 처리 후, ip 저장하기  (-2 : 오류발생; -1:접근 문제; 0:숫자비교 문제; 1:추가)
**********************************************************************************************/
AddIP.SuccessAddIP = function (data, status) {
    if (data.d == "1") {
        if (parent.ModTab != undefined) {
            parent.ModTab.PortletIPList();
            parent.ModTab.Resource();
        }
        else if (parent.Mod != undefined) {
            parent.Mod.PortletIPList();
            parent.Mod.Resource();
        }

        if (parent.disablePopup) {
            parent.disablePopup();
        } else { self.close(); }
    }
    else if (data.d == "0") {
        alert(R_WrongIPRange);
    }
    else if (data.d == "-1") {
        alert(R_ErrorAccessAuth);
    }
    else if (data.d == "-2") {
        alert(R_ProcessingError);
    }
}

