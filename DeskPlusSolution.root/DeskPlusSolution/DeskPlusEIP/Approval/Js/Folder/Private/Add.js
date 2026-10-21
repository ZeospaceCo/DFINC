
Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", "");
        });


        //추가 및 수정
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            if (Gubun == "Add") {
                Add.Register();
            } else if (Gubun == "Mod") {
                Add.Modify();
            }
        });

        //삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.Del();
        });


        //폴더 순서 변경
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            Add.MoveUpdown();

        });

        $('#ctbMuliLang').attr("disabled", true);
    });
});


/**************************************************
작성자   : 
작성목적 : 추가
***************************************************/
Add.Register = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_Msg_Folder, true);
    if (!validator.validate()) return;

    $('#ctbMuliLang').attr("disabled", false);

    ParamArray = {
        'folderID': FolderID,
        'active': $('#ddlActive').val(),
        'name': $('#ctbName').val(),
        'multiName': $('#ctbMuliLang').val()
    };

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Private/Add.asmx/Register";
    var obj = eval("Add.SuccessRegister");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 추가
**********************************************************************************************/
Add.SuccessRegister = function (data, status) {
    if (data.d > 0) {
        alert(R_MSG_SUCCESS);

        ParamArray = {
            'cabinetID': data.d,
            'accActive': "1"
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        parent.frmLeft.location = "/DeskplusEIP/Approval/Folder/Menu/Tree.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : 
작성목적 : 수정
***************************************************/
Add.Modify = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_Msg_Folder, true);
    if (!validator.validate()) return;

    $('#ctbMuliLang').attr("disabled", false);

    ParamArray = {
        'folderID': FolderID,
        'active': $('#ddlActive').val(),
        'name': $('#ctbName').val(),
        'multiName': $('#ctbMuliLang').val()
    };

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Private/Add.asmx/Modify";
    var obj = eval("Add.SuccessModify");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 수정
**********************************************************************************************/
Add.SuccessModify = function (data, status) {

    if (data.d == true) {
        alert(R_MSG_SUCCESS);

        ParamArray = {
            'cabinetID': CabinetID,
            'accActive': "1"
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        parent.frmLeft.location = "/DeskplusEIP/Approval/Folder/Menu/Tree.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 부서 추가
***************************************************/
Add.Del = function () {
    if (window.confirm(R_MSG_Del5)) {
        if (FolderID != "") {

            ParamArray = {
                'folderID': FolderID
            };

            var url = "/DeskPlusEIP/Approval/Ws/Folder/Private/Add.asmx/Del";
            var obj = eval("Add.SuccessDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 수정
**********************************************************************************************/
Add.SuccessDel = function (data, status) {

    if (data.d == true) {
        alert(R_MSG_SUCCESS);

        ParamArray = {
            'accActive': "1"
        };

        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        parent.frmLeft.location = "/DeskplusEIP/Approval/Folder/Menu/Tree.aspx?p=" + GetEncryptValue;
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



