List = {}

$(function () {
    $(document).ready(function () {

       
        // 지정자 추가
        $('#cbtnAddUser').click(function (e) {
            e.preventDefault();
            List.UserAdd(ItemID);
        });

        // 수신자 순서
        $('#cbtnToOrder').click(function (e) {
            e.preventDefault();
            List.ItemToUserUpdown(ItemID);
        });

        // 참조자 순서

        $('#cbtnCcOrder').click(function (e) {
            e.preventDefault();
            List.ItemCcUserUpdown(ItemID);
        });


        //소분류 지정자추가
        $('#cbtnSubUserAdd').click(function (e) {
            e.preventDefault();
            List.SubUserAdd();
        });

        //소분류 수신자 순서
        $('#cbtnItemSubToUserUpdown').click(function (e) {
            e.preventDefault();
            List.ItemSubToUserUpdown();
        });

        //소분류 참조자 순서
        $('#cbtnItemSubCcUserUpdown').click(function (e) {
            e.preventDefault();
            List.ItemSubCcUserUpdown();
        });


        // 대분류 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            List.Add();
        });

        // 대분류 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            List.Mod();
        });

        // 대분류 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            List.Del();
        });

        //대분류 다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMenuName").val(), "GetMultiName", MultiLang);
        });


        //순서변경 클릭
        // 대분류 순서 변경
        $('.r_specifierUserOrder').click(function (e) {
            e.preventDefault();
            ItemUser.ItemUserUpdown();
        });

        // 소분류 추가
        $('#cbtnAddSub').click(function (e) {
            e.preventDefault();
            List.AddSub();
        });

        // 소분류 수정
        $('#cbtnModSub').click(function (e) {
            e.preventDefault();
            List.ModSub();
        });

        // 소분류 삭제
        $('#cbtnDelSub').click(function (e) {
            e.preventDefault();
            List.DelSub();
        });

        // 대분류 순서 변경
        $('#CbtnMove').click(function (e) {
            e.preventDefault();
            List.MoveUpdown();
        });

        // 소분류 순서 변경
        $('#CbtnSubMove').click(function (e) {
            e.preventDefault();
            List.MoveSubUpdown();
        });

        // 소분류 다국이름
        $('#MultiNameSub').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbSubMenuName").val(), "GetMultiNameSub", MultiLang);
        });

        $('#ctbMenuName').attr("disabled", true);
        $('#ctbSubMenuName').attr("disabled", true);

        $("#ctbParentName").focus();

        List.LoadData();

    });

});

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-14
작성목적 : 대분류 추가
**********************************************************************************************/
List.Add = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_BaseName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/AddItem";

    var active = $("#chkActive").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'active': active
    };

    var obj = eval("List.AddSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-14
작성목적 : 대분류 추가 성공시, 완료
**********************************************************************************************/
List.AddSuccess = function (data, status) {
    if (data.d == true) {
        List.LoadData();
        List.ItemInit();

        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-14
작성목적 : 대분류 수정
**********************************************************************************************/
List.Mod = function () {
    if (ItemID == "") {
        alert(R_CheckModList);
        return;
    }
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_BaseName, true);
    if (!validator.validate()) return;

    $('#ctbMenuName').attr("disabled", false);

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/ModItem";

    var active = $("#chkActive").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'active': active,
        'itemID': ItemID
    };

    $('#ctbMenuName').attr("disabled", true);

    var obj = eval("List.ModSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-14
작성목적 : 대분류 수정 성공시, 완료
**********************************************************************************************/
List.ModSuccess = function (data, status) {
    if (data.d == true) {
        List.Item(ItemID);
        List.ItemInit();
        alert(R_CompleteConfirm);

        List.GetPageList();
        List.Resource();
    }
}


//대분류 삭제
List.Del = function () {

    if (ItemID == "") {
        alert(R_SelectFirst);
        return;
    }

    //하위 존재하면 1; 없으면 "" 빈값
    if (ItemSub == "1") {
        alert(R_DelSubCategory);
        return;
    }

    if (window.confirm(R_DelGubun)) {
        var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/DelItem";

        ParamArray = {
            'itemID': ItemID
        };

        var obj = eval("List.DelSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }

}

/**********************************************************************************************
작성자: maniaysw
작성일자: 2015 - 04 - 14
작성목적: 대분류 삭제
/**********************************************************************************************/
List.DelSuccess = function (data, status) {
    if (data.d == true) {
        List.LoadData();
        List.ItemInit();
        alert(R_CompleteConfirm);
    }
}

/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-20
작성목적 : 대분류 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
   
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbMenuName").val(multiNames);
}

/**************************************************
작성자   : maniaysw        
작성일자 : 2015-04-20
작성목적 : 소분류 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiNameSub(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#ctbSubMenuName").val(multiNames);
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 대분류 상단 초기화
**********************************************************************************************/
List.ItemInit = function () {
    $("#ctbParentName").val("");
    $("#ctbMenuName").val("");
    $("#chkAcitve").attr("checked", true);

    ParamArray = {
        accActive: 2,
        'reLoad': 1
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Task/Menu/Tree.aspx?p=" + GetEncryptValue;

    parent.frmLeft.location = url;

}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 리스트에서 대분류의 분류명 선택 시 상단 바인딩
**********************************************************************************************/
List.Item = function (p_ItemID) {
    ItemSubID = ""   
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");

    ItemID = p_ItemID;
    List.GetItem(p_ItemID);
    List.GetPageSubList(p_ItemID);
    List.Resource();

    $("#ctbParentName").focus();
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 대분류의 상세내역 가져오기
**********************************************************************************************/
List.GetItem = function (p_ItemID) {

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/GetItem";

    ParamArray = {
        'itemID': p_ItemID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessItem");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 리스트에서 대분류의 분류명 선택 시 상단 바인딩 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessItem = function (data, status) {
    $("#ctbParentName").val("");
    $("#ctbMenuName").val("");

    $("#ctbParentName").val(data.d[0].NameBase);

    GetMultiName(data.d[0].NameBase,
                 data.d[0].NameKo,
                 data.d[0].NameEn,
                 data.d[0].NameJa,
                 data.d[0].NameFr,
                 data.d[0].NameZhcn,
                 data.d[0].NameZhtw);

    if (data.d[0].IsActive == "1")
        $("#chkActive").attr("checked", true);
    else
        $("#chkActive").attr("checked", false);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 리스트 초기화
**********************************************************************************************/
List.LoadData = function () {    
    // 대분류 리스트 가져오기
    List.GetPageList();
    List.GetPageSubList("0");
    List.Resource();
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 대분류 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/GetPageList";

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-20
작성목적 : 대분류 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    
    $('#phItemList').setTemplateURL('/DeskPlusEIP/Task/Jt/Admin/Menu/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phItemList').processTemplate(data.d);
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-20
작성목적 : 대분류 선택시 하위 아이템 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.GetPageSubList = function (p_ItemID) {
    //alert(p_ItemID);
    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/GetItemSubList";

    ParamArray = {
        'itemID': p_ItemID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageSubList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-20
작성목적 : 선택한 대분류에 해당하는 소분류 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageSubList = function (data, status) {

    if (data.d != "") { ItemSub = "1" } else { ItemSub = "" };
    $('#phItemSubList').setTemplateURL('/DeskPlusEIP/Task/Jt/Admin/Menu/JTListSub.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phItemSubList').processTemplate(data.d);

}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-20
작성목적 : 선택한 소분류에 해당하는 상단 정보 바인딩
**********************************************************************************************/
List.ItemSub = function (p_ItemID, p_ItemSubID) {
    ItemSubID = p_ItemSubID;
    //alert(ItemSubID);
    //ItemSub = p_ItemSubID;

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/GetItemSub";

    ParamArray = {
        'itemID': p_ItemID,
        'itemSubID': p_ItemSubID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessSubItem");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2014-04-20
작성목적 : 선택 소분류 바인딩성공
**********************************************************************************************/
List.SuccessSubItem = function (data, status) {
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");
    $("#ctbSubName").val(data.d[0].NameBase);

    $("#ctbSubName").focus();

    GetMultiNameSub(data.d[0].NameBase,
                 data.d[0].NameKo,
                 data.d[0].NameEn,
                 data.d[0].NameJa,
                 data.d[0].NameFr,
                 data.d[0].NameZhcn,
                 data.d[0].NameZhtw);

    if (data.d[0].IsActive == "1")
        $("#chkSubAcitve").attr("checked", true);
    else
        $("#chkSubAcitve").attr("checked", false);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    
    if ($(".Active_1") != undefined) { $(".Active_1").text(R_Active); }
    if ($(".Active_0") != undefined) { $(".Active_0").text(R_NonActive); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }
    if ($(".r_group") != undefined) { $(".r_group").text(R_GroupName); }
    if ($(".r_active") != undefined) { $(".r_active").text(R_UsedYN); }
    if ($(".r_template") != undefined) { $(".r_template").text(R_Template); }
        
    if ($(".r_favadd") != undefined) { $(".r_favadd").text(R_SpecifierAdd); }
    if ($(".r_favToOrder") != undefined) { $(".r_favToOrder").text(R_Receiver + R_SpecifierUserOrder); }
    if ($(".r_favCcOrder") != undefined) { $(".r_favCcOrder").text(R_Referencer + R_SpecifierUserOrder); }
        
    if ($(".r_specifierUserAdd") != undefined) { $(".r_specifierUserAdd").text(R_SpecifierUserAdd); }
    if ($(".r_specifierUserOrder") != undefined) { $(".r_specifierUserOrder").text(R_SpecifierUserOrder); }    

    if ($(".r_add") != undefined) { $(".r_add").text(R_Add); }
    if ($(".r_mod") != undefined) { $(".r_mod").text(R_Mod); }
}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 대분류 지정자 추가
***************************************************/
List.UserAdd = function (_itemID) {
    if (_itemID == "") {
        alert(R_SelectFirst);
        return;
    }

    ItemID = _itemID;

    ParamArray = {
        'itemID': ItemID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemUser.aspx?p=" + GetEncryptValue;
    PopupWin(800, 520, url, "-500", "", true);
}



/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-23
작성목적 : 소분류 지정자 추가
***************************************************/
List.SubUserAdd = function () {   
    if (ItemSubID == "") {
        alert(R_SelectSecond);
        return;
    }
    ParamArray = {
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemSubUser.aspx?p=" + GetEncryptValue;
    PopupWin(800, 520, url, "-500", "", true);

}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 아이템 수신자 순서 변경 클릭
***************************************************/
List.ItemToUserUpdown = function (_itemID) {
    if (_itemID == "") {
        alert(R_SelectFirst);
        return;
    }

    ItemID = _itemID;
    ParamArray = {
        'itemID': ItemID,
        'isType': 'TO'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemUserUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 485, url, "-500", "", true);

}

/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 아이템 참조자 순서 변경 클릭
***************************************************/
List.ItemCcUserUpdown = function (_itemID) {
    if (_itemID == "") {
        alert(R_SelectFirst);
        return;
    }
    ItemID = _itemID;
    ParamArray = {
        'itemID': ItemID,
        'isType': 'CC'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemUserUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 485, url, "-500", "", true);

}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 소분류 추가
***************************************************/
List.AddSub = function (_itemID) {
    // 대분류 선택 여부 확인
    if (ItemID == "") {
        alert(R_CheckCategory);
        return;
    }
   
    // 소분류명 존재 여부 확인
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubName", R_SubBaseName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/AddItemSub";

    // 활성화 값
    var active = $("#chkSubAcitve").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbSubName").val(),
        'multiName': $("#ctbSubMenuName").val(),
        'active': active,
        'itemID': ItemID
    };

    var obj = eval("List.AddSubSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**************************************************
작성자   : maniaysw
작성일자 : 2015-04-21
작성목적 : 소분류 추가완료
***************************************************/
List.AddSubSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageSubList(ItemID);
        List.Resource();
        List.ItemSubInit();
        alert(R_CompleteConfirm);
    }
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 
작성목적 : 소분류 상단 초기화
**********************************************************************************************/
List.ItemSubInit = function () {
    ItemSubID = "";
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");
    $("#chkSubAcitve").attr("checked", true);
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-23
작성목적 : 소분류 상단 수정
**********************************************************************************************/
List.ModSub = function () {
    //ItemSub    

    if (ItemSubID == "") {
        alert(R_SelectSecond);
        return;
    }
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_SubBaseName, true);
    if (!validator.validate()) return;

    $('#ctbSubMenuName').attr("disabled", false);

    var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/ModItemSub";

    var active = $("#chkSubAcitve").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbSubName").val(),
        'multiName': $("#ctbSubMenuName").val(),
        'active': active,
        'itemSubID': ItemSubID
    };

    //alert(ItemSub);
    $('#ctbSubMenuName').attr("disabled", true);

    var obj = eval("List.ModSubSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-23
작성목적 : 소분류 수정 성공
**********************************************************************************************/
List.ModSubSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageSubList(ItemID);
        List.Resource();
        List.ItemSubInit();
        alert(R_CompleteConfirm);
    }
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-23
작성목적 : 템플릿 정보 추가,수정
**********************************************************************************************/
List.TempAdd = function (p_itemSubID, p_itemID, p_tempCnt) {

    ParamArray = {
        'itemID': p_itemID,
        'itemSubID': p_itemSubID
    };

    CallEncrypt(CallGetParam());
    var url = "TemplateN.aspx";

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Template", "1000", "620")
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-23
작성목적 : 대분류 순서 수정
**********************************************************************************************/
List.MoveUpdown = function () {
    ParamArray = {
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/MoveUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-29
작성목적 : 소분류 수신자 순서 수정
**********************************************************************************************/
List.ItemSubToUserUpdown = function () {
    if (ItemSubID == "") {
        alert(R_SelectSecond);
        return;
    }
    
    ParamArray = {
        'itemSubID': ItemSubID,
        'isType': 'TO'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemSubUserUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 485, url, "-500", "", true);
}



/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-29
작성목적 : 소분류 참조자 순서 수정
**********************************************************************************************/
List.ItemSubCcUserUpdown = function () {
    if (ItemSubID == "") {
        alert(R_SelectSecond);
        return;
    }

    ParamArray = {
        'itemSubID': ItemSubID,
        'isType': 'CC'
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/ItemSubUserUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(500, 485, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-29
작성목적 : 소분류 삭제
**********************************************************************************************/
List.DelSub = function () {
    if (ItemSubID == "") {
        alert(R_SelectSecond);
        return;
    }

    if (window.confirm(R_DelGubun)) {
        var url = "/DeskPlusEIP/Task/Ws/Admin/Menu/List.asmx/DelSubItem";

        ParamArray = {
            'itemSubID': ItemSubID
        };

        var obj = eval("List.DelSubSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }

}


/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-29
작성목적 : 소분류 삭제
**********************************************************************************************/
List.DelSubSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageSubList(ItemID);
        List.Resource();
        List.ItemSubInit();
        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : maniaysw
작성일자 : 2015-04-30
작성목적 : 소분류 순서변경
**********************************************************************************************/
List.MoveSubUpdown = function () {

    if (ItemID == "") {
        alert(R_SelectFirst);
        return;
    }

    ParamArray = {
        'itemID': ItemID
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Task/Admin/Menu/MoveSubUpdown.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "-500", "", true);

}