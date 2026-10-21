Add = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#tbLicenseKey').focus();
    });
});


/**************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 라이센스 추가
***************************************************/
Add.Confirm = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/License/Add.asmx/AddLicense";
    var validator = new Validator($("#form1"));

    validator.checkRequired("tbLicenseKey", "Please enter the license key", true);
    if (!validator.validate()) return;

    var licenseKey = $("#tbLicenseKey").val();  // - 값도 삭제하는 기능 제외 필요

    //파라미터값 배열에 담기
    ParamArray = {
        'licenseKey': licenseKey
    };

    var obj = eval("Add.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 라이센스 추가 성공
**********************************************************************************************/
Add.SuccessAdd = function (data, status) {
    if (data.d < 1) {
        alert(R_ProcessingErrorLicenseAdd);
    }

    parent.List.LoadData();
    parent.List.ModalHide();
}


