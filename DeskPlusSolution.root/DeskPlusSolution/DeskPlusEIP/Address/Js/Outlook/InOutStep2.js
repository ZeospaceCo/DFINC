InOutStep2 = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#GetAll').click(function (e) {
            e.preventDefault();
            InOutStep2.GetAll();
        });

        $('#GetOverlap').click(function (e) {
            e.preventDefault();
            InOutStep2.GetOverlap();            
        });

        $('#GetExcept').click(function (e) {
            e.preventDefault();
            InOutStep2.GetExcept();
        });

        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            InOutStep2.Register();
        });

        //csv주소록
        $('#csvContact').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/InOutStep1.aspx";
        });

        //google주소록
        $('#googleContact').click(function (e) {
            e.preventDefault();
            InOutStep2.GoogleContact();
        });

        //주소록 내보내기
        $('#exportCsv').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/ExportCSV.aspx";
        });

        InOutStep2.GetAll();
    });
});

/**************************************************
작성자   : 심재운
작성목적 : 전체 가져오기
***************************************************/
InOutStep2.GetAll = function () {

    var url = "/DeskPlusEIP/Address/Ws/Outlook/InOutStep2.asmx/GetCSVPageList";
    var types = "ALL";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InOutStep2.SuccessGetAll");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 전체 가져오기
**********************************************************************************************/
InOutStep2.SuccessGetAll = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InOutStep2.Resource();
}

/**************************************************
작성자   : 심재운
작성목적 : 중복 가져오기
***************************************************/
InOutStep2.GetOverlap = function () {

    var url = "/DeskPlusEIP/Address/Ws/Outlook/InOutStep2.asmx/GetCSVPageList";
    var types = "OVERLAP";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InOutStep2.SuccessGetOverlap");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 중복 가져오기
**********************************************************************************************/
InOutStep2.SuccessGetOverlap = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InOutStep2.Resource();
}

/**************************************************
작성자   : 심재운
작성목적 : 중복 가져오기
***************************************************/
InOutStep2.GetExcept = function () {
    var url = "/DeskPlusEIP/Address/Ws/Outlook/InOutStep2.asmx/GetCSVPageList";
    var types = "EXCEPT";
    //파라미터값 배열에 담기
    ParamArray = {
        'types': types
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InOutStep2.SuccessGetExcept");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 주소록 중복 가져오기
**********************************************************************************************/
InOutStep2.SuccessGetExcept = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Address/Jt/Outlook/JTOutStep.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    InOutStep2.Resource();
}






/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 선택된 값 가져오기
***************************************************/
InOutStep2.CheckedValue = function () {
    var allVals = $('.cbxID:checked').map(function () {
        return $(this).val();
    }).get();
    
    $("#tbxCSVID").val(allVals);
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 저장하기
**********************************************************************************************/
InOutStep2.Register = function () {
    InOutStep2.CheckedValue();
    var csvVal = $("#tbxCSVID").val();    

    if (csvVal == "") {
        alert(R_SelectSaveAddress);
        return;
    }    

    __doPostBack('lbtnRegister', '');
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 구글 주소록 페이지 새창띄우기
***************************************************/
InOutStep2.GoogleContact = function () {
//    var urlpath = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
//    WindowObjectReference = window.open(urlpath, "GoogleContact", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");

    var url = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "GoogleContact", "800", "600")
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 다국어
**********************************************************************************************/
InOutStep2.Resource = function () {
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