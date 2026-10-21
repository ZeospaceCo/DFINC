InGoogleStep2 = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#GetAll').click(function (e) {
            e.preventDefault();
            InGoogleStep2.GetAll();
        });

        $('#GetOverlap').click(function (e) {
            e.preventDefault();
            InGoogleStep2.GetOverlap();
        });

        $('#GetExcept').click(function (e) {
            e.preventDefault();
            InGoogleStep2.GetExcept();
        });

        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            InGoogleStep2.Register();
        });

        InGoogleStep2.GetAll();

    });
});

/**************************************************
작성자   : 심재운
작성목적 : 전체 가져오기
***************************************************/
InGoogleStep2.GetAll = function () {
    
    var url = "/DeskPlusEIP/Address/Ws/Outlook/InGoogleStep2.asmx/GetCSVPageList";
    var types = "ALL";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InGoogleStep2.SuccessGetAll");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 전체 가져오기
**********************************************************************************************/
InGoogleStep2.SuccessGetAll = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InGoogleStep2.Resource();
}

/**************************************************
작성자   : 심재운
작성목적 : 중복 가져오기
***************************************************/
InGoogleStep2.GetOverlap = function () {

    var url = "/DeskPlusEIP/Address/Ws/Outlook/InGoogleStep2.asmx/GetCSVPageList";
    var types = "OVERLAP";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InGoogleStep2.SuccessGetOverlap");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 중복 가져오기
**********************************************************************************************/
InGoogleStep2.SuccessGetOverlap = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InGoogleStep2.Resource();
}

/**************************************************
작성자   : 심재운
작성목적 : 중복 가져오기
***************************************************/
InGoogleStep2.GetExcept = function () {
    var url = "/DeskPlusEIP/Address/Ws/Outlook/InGoogleStep2.asmx/GetCSVPageList";
    var types = "EXCEPT";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InGoogleStep2.SuccessGetExcept");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 중복 가져오기
**********************************************************************************************/
InGoogleStep2.SuccessGetExcept = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InGoogleStep2.Resource();
}





/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
InGoogleStep2.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();

    $("#tbxCSVID").val(allVals);
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 저장하기
**********************************************************************************************/
InGoogleStep2.Register = function () {
    InGoogleStep2.CheckedValue();
    var csvVal = $("#tbxCSVID").val();

    if (csvVal == "") {
        alert(R_SelectSaveAddress);
        return;
    }

    __doPostBack('lbtnRegister', '');
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 다국어
**********************************************************************************************/
InGoogleStep2.Resource = function () {
    $("#r_name")[0].innerHTML = R_FirstName;
    $("#r_mail")[0].innerHTML = R_Email;
    $("#r_mobile")[0].innerHTML = R_CellPhone;
    $("#r_homePhone")[0].innerHTML = R_Phone;
    $("#r_workPhone")[0].innerHTML = R_CompanyTel;
    $("#r_compnayName")[0].innerHTML = R_CompanyName;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}



/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 전체 체크박스
***************************************************/
function AllChecked() {

    if ($("#cbxAll").is(":checked")) {
        $(".cbxID").attr("checked", true);
    }
    else {
        $(".cbxID").attr("checked", false);
    }
}

