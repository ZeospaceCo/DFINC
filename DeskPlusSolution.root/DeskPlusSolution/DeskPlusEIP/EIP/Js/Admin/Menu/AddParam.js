AddParam = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddParam.Confirm();
        });

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //암호화
        $('#ddlEncript').change(function () {
            AddParam.SetEncrpt();
        });

        //ldap
        $('#ddlLDAP').change(function () {
            AddParam.SetLDAP();
        });

        $('#ctbParamName').focus();
        $('#ctbParamValue').show();
        $('#ctbEncETC').hide();
        
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 암호화 설정
***************************************************/
AddParam.SetEncrpt = function () {
    $('#ctbEncETC').hide();
    var selValue = $('#ddlEncript').val();

    //기타입력
    if (selValue == "3") {
        $('#ctbEncETC').show();
    }
}


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 직접입력시 활성화
***************************************************/
AddParam.SetLDAP = function () {
    $('#ctbParamValue').hide();
    var selValue = $('#ddlLDAP').val();

    if (selValue == "") {
        $('#ctbParamValue').show();
    }
}



/**************************************************
작성자   : 
작성일자 : 
작성목적 : 저장
***************************************************/
AddParam.Confirm = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/AddParam.asmx/AddMenuDataLink";

    var encEtc = $('#ddlEncript').val();
    if (encEtc == "3") {
        encEtc = $('#ctbEncETC').val();
        if (encEtc == "") {
            alert(R_InputEncode);
            $('#ctbEncETC').focus();
            return;
        }
    }

    var linkValue = $('#ddlLDAP').val();
    if (linkValue == "") {
        linkValue = $('#ctbParamValue').val();
        if (linkValue == "") {
            alert(R_InputContent);
            $('#ctbParamValue').focus();
            return;
        }
    }

    var linkType = "OL";
    if ($("#ddlLDAP").val() == "") {
        linkType = "OU";
    }
    
    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID,
        'encriptType': $('#ddlEncript').val(),
        'encriptValue': encEtc,
        'linkName': $('#ctbParamName').val(),
        'linkValue': linkValue,
        'linkType': linkType,
        'description': $('#ctbDescription').val()
    };

    var obj = eval("AddParam.SuccessAddParam");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 저장 결과값 반환
**********************************************************************************************/
AddParam.SuccessAddParam = function (data, status) {
    if (data.d == true) {
        parent.Mod.MenuDataLinkList();
        parent.Mod.Resource();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}

