List = {}
var licenseKey = "";


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 데이터 초기화
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //라이센스 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });


        //회사관리 이동
        $('#cbtnComInfo').click(function (e) {
            e.preventDefault();
            List.CompanyInfo();
        });

        List.LoadData();
    });
});



/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 리스트 초기화
***************************************************/
List.LoadData = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/License/List.asmx/GetLicense";
    var obj = eval("List.SuccessLicense");
    AjaxService(url, "", obj);
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 성공시 회사 리스트 표시
**********************************************************************************************/
List.SuccessLicense = function (data, status) {
    $('#phLicenseList').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/License/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phLicenseList').processTemplate(data.d);

    List.Resource();
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

List.Resource = function () {
    $("#r_LicenseNumber")[0].innerHTML = R_LicenseKey;
    $("#r_Description")[0].innerHTML = R_Comments;
    $("#r_Count")[0].innerHTML = R_LicenseCount;
    $("#r_Status")[0].innerHTML = R_State;
    $("#r_Expire")[0].innerHTML = R_ExpirationDate;
    $("#r_ActActive")[0].innerHTML = R_ConfirmLicense;
    $("#r_Active")[0].innerHTML = R_ConfirmServer;
    $("#r_Del")[0].innerHTML = R_Del;

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NotExistList;
    }
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 추가
**********************************************************************************************/
List.Add = function () {
    var url = "Add.aspx";

    PopupWin(400, 120, url, "", "", true);
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 삭제
**********************************************************************************************/
List.Del = function (arg) {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/License/List.asmx/DelLicense";
    licenseKey = arg;

    //파라미터값 배열에 담기
    ParamArray = {
        'licenseKey': licenseKey
    };

    var obj = eval("List.SuccessDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 라이센스 삭제 성공
**********************************************************************************************/
List.SuccessDel = function (data, status) {
    if (data.d < 1) {
        alert(R_ProcessingErrorLicenseDelete);
    }

    List.LoadData();
}



/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : Act 서버 활성화
**********************************************************************************************/
List.ActServreActvie = function (arg) {

    licenseKey = arg.trimText();

    var url = "/DeskPlusEIP/EIP/Ws/Admin/License/List.asmx/ActActiveLicense";

    //파라미터값 배열에 담기
    ParamArray = {
        'licenseKey': licenseKey
    };

    var obj = eval("List.SuccessAct");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : Act 서버 활성화 성공
**********************************************************************************************/
List.SuccessAct = function (data, status) {
    if (data.d != "VALID_KEY") {  // 제대로 인증이 않되면 form 로 다시 한번 호출한다 
        $("#lickey").val(licenseKey);
        $("#ActLic").submit();
    } else {
        alert(data.d);
        List.LoadData();
    }
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 서버 Active
**********************************************************************************************/
List.ServerActvie = function (arg) {
    licenseKey = arg.trimText();

    var url = "/DeskPlusEIP/EIP/Ws/Admin/License/List.asmx/ActivelLicense";

    //파라미터값 배열에 담기
    ParamArray = {
        'licenseKey': licenseKey
    };

    var obj = eval("List.SuccessServerActvie");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 서버 활성화 성공
**********************************************************************************************/
List.SuccessServerActvie = function (data, status) {
    alert(data.d);

    List.LoadData();
}


/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-04-14
작성목적 : 회사관리 이동
**********************************************************************************************/
List.CompanyInfo = function () {
    var url = "../Company/List.aspx";
    document.location.href = url;
}



/**************************************************
작성자: freeman
작성일:2011-11-14
작성목적 : 모달 레이어 숨기기
***************************************************/
List.ModalHide = function () {
    disablePopup();
    //$("#modaldiv").hide();
}