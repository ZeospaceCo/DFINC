var Add = {};

/**************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.MenuAdd();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Add.MenuMod();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.MenuDel();
        });

        // 메뉴구성 클릭
        $('#lbxMenu').click(function (e) {
            e.preventDefault();
            Add.SetInfo();
        });


        // 기본메뉴 변경시
        $("#ddlBasicMenu").change(function (e) {
            e.preventDefault();
            Add.ChangeBasicMenu();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            if ($("#ddlBasicMenu").val() != "5") {
                CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
            }
        });

        $('#CTBMultiName').attr("disabled", true);

        //상세정보 다국이름
        $('#MultiSub').click(function (e) {
            e.preventDefault();
            if ($("#tbType").val() != "5") {
                CallMultiName($("#CTBMultiSub").val(), "GetMultiSub", MultiLang);
            }
        });

        $('#CTBMultiSub').attr("disabled", true);

        $("html body").scrollTop($(document).height());
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiName").val(multiNames);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiSub(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiSub").val(multiNames);
}

/**************************************************
작성자 : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴추가 (웹서비스 값 호출) 
***************************************************/
Add.MenuAdd = function () {

    if (BtnCheck == "Y") return;

    if (($("#ddlBasicMenu").val() == "7") && (ScheduleYN == "Y")) {
        alert(R_ScheduleOne);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbNameBase", R_InputMenu, true);
    if (!validator.validate()) return;


    ParamArray = {
        'communityID': CommunityID,
        'type': $("#ddlBasicMenu").val(),
        'nameBase': $("#ctbNameBase").val(),
        'multiName': $("#CTBMultiName").val(),
        'writeAuth': $("#ddlBasicWriteAuth").val(),
        'readAuth': $("#ddlBasicReadAuth").val(),
        'weight': $("#ddlBasicWeight").val()
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessMenuAdd");
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/MenuAdd";

    BtnCheck = "Y";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴추가 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessMenuAdd = function (data, status) {
    BtnCheck = "N";

    if (data.d) {

        if ($("#ddlBasicMenu").val() == "7") {
            ScheduleYN = "Y";
        }

        Add.GetMenuList();
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
Add.GetMenuList = function () {

    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessGetMenuList");
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/GetMenuList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessGetMenuList = function (data, status) {
    // 기존 리스트 삭제
    $("select[id='lbxMenu']").find('option').each(function () {
        $(this).remove();

    });

    var i = 0;
    // 재바인딩
    $.each(data.d, function (index, list) {
        $("#lbxMenu").get(0).options[index] = new Option(list.NameBase, list.MenuID + "^" + list.Sequence);
        if (tmpMenuID == list.MenuID) {
            $("#lbxMenu").get(0).options[index].selected = true;
        }

        i++;
    });

    tmpSequence = i;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴 구성 선택시
**********************************************************************************************/
Add.SetInfo = function () {
    if ($("#lbxMenu").val() != null) {
        tmpMenuID = $("#lbxMenu").val().split('^')[0];

        if (tmpMenuID != null) {
            ParamArray = {
                'communityID': CommunityID,
                'menuID': tmpMenuID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Add.SuccessSetInfo");
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/GetMenuInfo";

            AjaxService(url, data, obj);
        }
    }    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴추가 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessSetInfo = function (data, status) {

    if (data.d != null) {

        $("#CTBMenuName").val($("#ddlBasicMenu").get(0).options[data.d[0].Type - 1].text);
        $("#ctbSubBase").val(data.d[0].NameBase);
        $("#CTBMultiSub").val(data.d[0].MultiName);
        $("#ddlWriteAuth").val(data.d[0].WriteAuth);
        $("#ddlReadAuth").val(data.d[0].ReadAuth);
        $("#ddlWeight").val(data.d[0].Weight);
        $("#tbType").val(data.d[0].Type);

        if ($("#tbType").val() == "5") {
            $("#ctbSubBase").val('-----');
            $("#CTBMultiSub").val('');
            $("#ctbSubBase").attr("disabled", true);
            $("#ddlWriteAuth").attr("disabled", true);
            $("#ddlReadAuth").attr("disabled", true);
            $("#ddlWeight").attr("disabled", true);
        }
        else if (data.d[0].Type == "6") {
            $("#ctbSubBase").attr("disabled", false);
            $("#ddlWriteAuth").attr("disabled", true);
            $("#ddlReadAuth").attr("disabled", true);
            $("#ddlWeight").attr("disabled", true);
        }
        else {
            $("#ctbSubBase").attr("disabled", false);
            $("#ddlWriteAuth").attr("disabled", false);
            $("#ddlReadAuth").attr("disabled", false);
            $("#ddlWeight").attr("disabled", false);
        }
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴수정 (웹서비스 값 호출) 
***************************************************/
Add.MenuMod = function () {

    if (tmpMenuID != "0") {

        var validator = new Validator($("#form1"));
        validator.checkRequired("ctbSubBase", R_InputMenu, true);
        if (!validator.validate()) return;

        ParamArray = {
            'communityID': CommunityID,
            'menuID': tmpMenuID,
            'nameBase': $("#ctbSubBase").val(),
            'multiName': $("#CTBMultiSub").val(),
            'weight': $("#ddlWeight").val(),
            'writeAuth': $("#ddlWriteAuth").val(),
            'readAuth': $("#ddlReadAuth").val()
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Add.SuccessMenuMod");
        var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/MenuMod";

        AjaxService(url, data, obj);
    }
    else {
        alert(R_SelMenu);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-24
작성목적 : 메뉴수정 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessMenuMod = function (data, status) {

    if (data.d) {
        Add.GetMenuList();
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}


/**********************************************************************************************
작성자 : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 삭제 (웹서비스 값 호출) 
**********************************************************************************************/
Add.MenuDel = function () {
    if (tmpMenuID != "0") {

        if (window.confirm(R_DelMenu)) {
            ParamArray = {
                'communityID': CommunityID,
                'menuID': tmpMenuID
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Add.SuccessMenuDel");
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/MenuDel";

            AjaxService(url, data, obj);
        }
    }
    else {
        alert(R_SelMenu);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 삭제 (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessMenuDel = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);

        tmpMenuID = 0;
        Add.GetMenuList();

        if ($("#tbType").val() == "7") {
            ScheduleYN = "N";
        }

        $("#CTBMenuName").val('');
        $("#ctbSubBase").val('');
        $("#CTBMultiSub").val('');
        $("#ddlWriteAuth").val(10);
        $("#ddlReadAuth").val(10);
        $("#ddlWeight").val(0);
        $("#tbType").val('');

        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자 : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 순서 Up (웹서비스 값 호출) 
**********************************************************************************************/
Add.MenuUp = function () {

    if ($("#lbxMenu").val() == null || tmpMenuID == 0) {
        alert(R_SelMenu);return;
    }

    if (tmpMenuID != "0") {

        var sequence = $("#lbxMenu").val().split('^')[1];

        if (sequence != "1") {

            ParamArray = {
                'communityID': CommunityID,
                'menuID': tmpMenuID,
                'sequence': sequence
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Add.SuccessMenuUp");
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/MenuUp";

            AjaxService(url, data, obj);
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 순서 Up (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessMenuUp = function (data, status) {

    if (data.d) {
        Add.GetMenuList();        
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자 : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 순서 Down (웹서비스 값 호출) 
**********************************************************************************************/
Add.MenuDown = function () {
    if ($("#lbxMenu").val() == null || tmpMenuID == 0) {
        alert(R_SelMenu); return;
    }

    if (tmpMenuID != "0") {

        var sequence = $("#lbxMenu").val().split('^')[1];

        if (tmpSequence != sequence) {

            ParamArray = {
                'communityID': CommunityID,
                'menuID': tmpMenuID,
                'sequence': sequence
            };

            CallEncrypt(CallGetParam())

            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("Add.SuccessMenuDown");
            var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Menu/Add.asmx/MenuDown";

            AjaxService(url, data, obj);
        }
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-25
작성목적 : 메뉴 순서 Down (웹서비스 값 받기)
**********************************************************************************************/
Add.SuccessMenuDown = function (data, status) {

    if (data.d) {
        Add.GetMenuList();
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-11
작성목적 : 메뉴 순서 Down (웹서비스 값 받기)
**********************************************************************************************/
Add.ChangeBasicMenu = function () {
    if ($("#ddlBasicMenu").val() == "5") {
        $("#ctbNameBase").val('-----');
        $("#CTBMultiName").val('');
        $("#ddlBasicWriteAuth").val("10");
        $("#ddlBasicReadAuth").val("10");
        $("#ddlBasicWeight").val("0");
        $("#ctbNameBase").attr("disabled", true);
        $("#ddlBasicWriteAuth").attr("disabled", true);
        $("#ddlBasicReadAuth").attr("disabled", true);
        $("#ddlBasicWeight").attr("disabled", true);
    }
    else if ($("#ddlBasicMenu").val() == "6") {
        $("#ddlBasicWriteAuth").val("10");
        $("#ddlBasicReadAuth").val("10");
        $("#ddlBasicWeight").val("0");
        $("#ctbNameBase").attr("disabled", false);
        $("#ddlBasicWriteAuth").attr("disabled", true);
        $("#ddlBasicReadAuth").attr("disabled", true);
        $("#ddlBasicWeight").attr("disabled", true);
    }
    else {
        $("#ctbNameBase").attr("disabled", false);
        $("#ddlBasicWriteAuth").attr("disabled", false);
        $("#ddlBasicReadAuth").attr("disabled", false);
        $("#ddlBasicWeight").attr("disabled", false);
    }
}


