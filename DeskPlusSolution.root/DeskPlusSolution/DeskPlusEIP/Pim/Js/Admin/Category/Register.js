/**************************** Basic *********************************/
Register = {}

$(function () {
    $(document).ready(function () {

        // 저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Register.Confirm();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Register.Del();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            history.back();
        });


        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMuliLang').attr("disabled", true);

    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-8-23
작성목적 : 저장
***************************************************/
Register.Confirm = function (arg) {


    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_MsgCategoryName, true);
    if (!validator.validate()) return;

    $('#ctbMuliLang').attr("disabled", false);

    var url = "/DeskPlusEIP/Pim/Ws/Admin/Category/Register.asmx/Add";
    if (StatusID > 0) {
        url = "/DeskPlusEIP/Pim/Ws/Admin/Category/Register.asmx/Mod";
    }

    ParamArray = {
        'statusID': StatusID,
        'statusCode': $("#ctbStatusCode").val(),
        'name': $("#ctbName").val(),
        'multiName': $("#ctbMuliLang").val(),
        'colors': $("#ddlColor").val(),
        'useActive': $("#ddlActive").val()
    };

    var obj = eval("Register.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 처리 완료
**********************************************************************************************/
Register.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        location = "List.aspx";
    }
    else {
        alert(R_MsgError);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 삭제하기
**********************************************************************************************/
Register.Del = function () {

    if (window.confirm(R_ReallyApplyDelete)) {
        var url = "/DeskPlusEIP/Pim/Ws/Admin/Category/Register.asmx/Del";

        ParamArray = {
            'statusID': StatusID
        };

        var obj = eval("Register.SuccessConfirm");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

 



/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMuliLang").val(multiNames);
}