MultiDomain = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {


        //확인
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            MultiDomain.Confirm();
        });

        //메일 체크
        $('#cbtnMailCheck').click(function (e) {
            e.preventDefault();
            MultiDomain.MailCheck();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        MultiDomain.GetPageList();
    });
});



/**************************************************
작성자   : shimpark
작성일자 : 2011-01-26
작성목적 : 처리하기
***************************************************/
MultiDomain.Confirm = function () {

    var mailID = $("#ctbMailID").val();
    var mailDomain = $("#ddlMailDomain").val();
    var mailPwd = $("#ctbMailPwd").val();

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbMailID", R_InputBaseMail, true);                //"기본메일을 입력해 주세요"
    validator.checkRequired("ctbMailPwd", R_InputPassword, true);                //"기본메일을 입력해 주세요"
    if (!validator.validate()) return;

    if (mailDomain == "") {
        alert(R_InputBaseMail);
        return;
    }    

    if (MailCheck == false) {
        alert(R_CheckMailRepetition);
        return;
    }

    var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiDomain.asmx/AddMultiMail";

    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'mailID': mailID,
        'mailDomain': mailDomain,
        'mailPwd': mailPwd
    };

    var obj = eval("MultiDomain.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 추가
**********************************************************************************************/
MultiDomain.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        MultiDomain.GetPageList();
    }
    else {
        alert(R_ErrorProcessing);
    }
}



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 메일 중복 체크
***************************************************/
MultiDomain.MailCheck = function () {
    MailCheck = false;
    
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiDomain.asmx/GetMailCheck";
    var mailID = $("#ctbMailID").val();
    var mailDomain = $("#ddlMailDomain").val();

    if (mailID == "") {
        alert(R_InputBaseMail);
        $("#ctbMailID").focus();
        return;
    }

    ParamArray = {
        'companyID': CompanyID,
        'mailID': mailID,
        'mailDomain': mailDomain,
        'gubun': 'add'
    };

    var obj = eval("MultiDomain.SuccessMailCheck");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 메일 중복 체크
**********************************************************************************************/
MultiDomain.SuccessMailCheck = function (data, status) {
    if (data.d == false) {
        MailCheck = true;
        alert(R_OnlyOneMail);
    }
    else {
        alert(R_RepetitionMail);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리스트 호출
**********************************************************************************************/
MultiDomain.GetPageList = function () {
    var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiDomain.asmx/GetMailPageList";    

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'order': ''
    };

    var obj = eval("MultiDomain.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
MultiDomain.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Account/Jt/admin/User/JTMultiDomain.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    MultiDomain.Resource();
}


/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
MultiDomain.Resource = function () {
    $("#r_mailID")[0].innerHTML = R_MailID;
    $("#r_mailDomain")[0].innerHTML = R_MailDomain;
    $("#r_basemail")[0].innerHTML = R_BaseMail;
    $("#r_base")[0].innerHTML = "";
    $("#r_del")[0].innerHTML = R_Admin_UserManager_cbsUserDel;

    $(".r_mailcheck").text(R_CheckBaseMail);

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 메인 설정
**********************************************************************************************/
MultiDomain.ModBase = function (p_idx) {

    if (p_idx != "") {
        var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiDomain.asmx/ModMailBase";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'userID': UserID,
            'userMailDomainID': p_idx
        };

        var obj = eval("MultiDomain.Success");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 삭제
**********************************************************************************************/
MultiDomain.Del = function (p_idx, p_mailID, p_mailDomain) {

    if (window.confirm(R_DeleteInfo)) {

        var url = "/DeskPlusEIP/Account/Ws/Admin/User/MultiDomain.asmx/DelMailBase";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'userID': UserID,
            'userMailDomainID': p_idx,
            'mailID': p_mailID,
            'mailDomain': p_mailDomain
        };

        var obj = eval("MultiDomain.Success");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
        
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 리스트 표시
**********************************************************************************************/
MultiDomain.Success = function (data, status) {

    if (data.d == true) {
        MultiDomain.GetPageList();
    }
    else {
        alert(R_ErrorProcessing);
    }
}