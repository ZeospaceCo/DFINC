List = {}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 초기화
**********************************************************************************************/
$(function () {
    $(document).ready(function () {
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
작성자   : shine
작성일자 :
작성목적 : 대분류 추가
**********************************************************************************************/
List.Add = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_BaseName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/AddItem";

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
작성자   : shine
작성일자 : 2011-04-14
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
작성자   : shine
작성일자 :
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

    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/ModItem";

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
작성자   : shine
작성일자 : 2011-04-14
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

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 대분류 삭제
**********************************************************************************************/
List.Del = function () {
    if (ItemID == "") {
        alert(R_CheckDelList);
        return;
    }

    //하위 존재하면 1; 없으면 "" 빈값
    if (ItemSub == "1") {
        alert(R_DelSubCategory);
        return;
    }

    if (window.confirm(R_DelGubun)) {
        var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/DelItem";

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
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 대분류 삭제 성공시, 완료
**********************************************************************************************/
List.DelSuccess = function (data, status) {
    if (data.d == true) {
        List.LoadData();
        List.ItemInit();
        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 추가
**********************************************************************************************/
List.AddSub = function () {
    // 대분류 선택 여부 확인
    if (ItemID == "") {
        alert(R_CheckCategory);
        return;
    }

    // 소분류명 존재 여부 확인
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubName", R_SubBaseName, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/AddItemSub";

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

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 추가 성공시, 완료
**********************************************************************************************/
List.AddSubSuccess = function (data, status) {
    if (data.d == true) {
        List.GetPageSubList(ItemID);
        List.Resource();
        List.ItemSubInit();
        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 수정
**********************************************************************************************/
List.ModSub = function () {
    if (ItemID == "") {
        alert(R_CheckCategory);
        return;
    }

    // 소분류명 존재 여부 확인
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubName", R_SubBaseName, true);
    if (!validator.validate()) return;

    $('#ctbSubMenuName').attr("disabled", false);

    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/ModItemSub";

    var active = $("#chkSubAcitve").is(':checked') == true ? "1" : "0";

    ParamArray = {
        'baseName': $("#ctbSubName").val(),
        'multiName': $("#ctbSubMenuName").val(),
        'active': active,
        'itemID': ItemID,
        'itemSubID': ItemSubID
    };

    $('#ctbSubMenuName').attr("disabled", true);

    var obj = eval("List.ModSubSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 수정 성공시, 완료
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
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 삭제
**********************************************************************************************/
List.DelSub = function () {
    // 대분류 존재 여부 체크
    if (ItemID == "") {
        alert(R_CheckDelList);
        return;
    }
    // 소분류 선택 여부 체크
    if (ItemSubID == "") {
        alert(R_CheckDelList);
        return;
    }

    if (window.confirm(R_DelGubun)) {
        var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/DelItemSub";

        ParamArray = {
            'itemID': ItemID,
            'itemSubID': ItemSubID
        };

        var obj = eval("List.DelSubSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 삭제 성공시, 완료
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
작성자   : shine
작성일자 :
작성목적 : 대분류 상단 초기화
**********************************************************************************************/
List.ItemInit = function () {
    $("#ctbParentName").val("");
    $("#ctbMenuName").val("");
    $("#chkAcitve").attr("checked", true);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 소분류 상단 초기화
**********************************************************************************************/
List.ItemSubInit = function () {
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");
    $("#chkSubAcitve").attr("checked", true);
}
/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 리스트 초기화
**********************************************************************************************/
List.LoadData = function () {
    // 대분류 리스트 가져오기
    List.GetPageList();
    List.GetPageSubList("0");
    List.Resource();
}
/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($(".Active_1") != undefined) { $(".Active_1").text(R_Active); }
    if ($(".Active_0") != undefined) { $(".Active_0").text(R_NonActive); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoListData); }
    if ($(".r_group") != undefined) { $(".r_group").text(R_GroupName); }
    if ($(".r_active") != undefined) { $(".r_active").text(R_UsedYN); }
    if ($(".r_template") != undefined) { $(".r_template").text(R_Template); }

    if ($(".r_favlist") != undefined) { $(".r_favlist").text("지정자명단"); }
    if ($(".r_favadd") != undefined) { $(".r_favadd").text("지정자추가"); }
    if ($(".r_favorder") != undefined) { $(".r_favorder").text("지정자순서"); }

    if ($(".r_add") != undefined) { $(".r_add").text(R_Add); }
    if ($(".r_mod") != undefined) { $(".r_mod").text(R_Mod); }
}
/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 대분류 리스트 가져오기 (웹서비스 호출)
**********************************************************************************************/
List.GetPageList = function () {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/GetPageList";

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 대분류 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageList = function (data, status) {
    $('#phItemList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Admin/Category/JTDefault.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phItemList').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 리스트에서 대분류의 분류명 선택 시 상단 바인딩
**********************************************************************************************/
List.Item = function (p_ItemID) {
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");

    ItemID = p_ItemID;
    List.GetItem(p_ItemID);
    List.GetPageSubList(p_ItemID);
    List.Resource();

    $("#ctbParentName").focus();
}
/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 대분류의 상세내역 가져오기
**********************************************************************************************/
List.GetItem = function (p_ItemID) {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/GetItem";

    ParamArray = {
        'itemID': p_ItemID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessItem");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
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

    if (data.d[0].ActiveGroup == "1")
        $("#chkActive").attr("checked", true);
    else
        $("#chkActive").attr("checked", false);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 선택한 대분류의 소분류 리스트 가져오기
**********************************************************************************************/
List.GetPageSubList = function (p_ItemID) {
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/GetPageSubList";

    ParamArray = {
        'itemID': p_ItemID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("List.SuccessPageSubList");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 선택한 대분류에 해당하는 소분류 리스트 가져오기 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessPageSubList = function (data, status) {
    ItemSub = "";

    if (data.d != "") ItemSub = "1";
    $('#phItemSubList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Admin/Category/JTDefaultSub.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phItemSubList').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shine
작성일자 :
작성목적 : 선택한 소분류의 상세내역 가져오기
**********************************************************************************************/
List.SubItem = function (p_ItemID, p_ItemSubID) {
    ItemSubID = p_ItemSubID;
    var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/Default.asmx/GetSubItem";

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
작성자   : shine
작성일자 :
작성목적 : 리스트에서 소분류의 분류명 선택 시 상단 바인딩 (웹서비스 값 받기)
**********************************************************************************************/
List.SuccessSubItem = function (data, status) {
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");
    $("#ctbSubName").val(data.d[0].ItemSubName);

    $("#ctbSubName").focus();

    GetMultiNameSub(data.d[0].SubNameBase,
                 data.d[0].SubNameKo,
                 data.d[0].SubNameEn,
                 data.d[0].SubNameJa,
                 data.d[0].SubNameFr,
                 data.d[0].SubNameZhcn,
                 data.d[0].SubNameZhtw);

    if (data.d[0].SubActiveGroup == "1")
        $("#chkSubAcitve").attr("checked", true);
    else
        $("#chkSubAcitve").attr("checked", false);
}

/**************************************************
작성자   : shine
작성일자 : 2011-10-18
작성목적 : 소분류 템플릿 추가
***************************************************/
List.TempAdd = function (p_ItemSubID, p_ItemID, p_TempCnt) {
    ParamArray = {
        'itemSubID': p_ItemSubID,
        'itemID': p_ItemID
    };

    CallEncrypt(CallGetParam());

    var url = "";

    if ($.browser.msie) {
        url = "Add.aspx";
    } else {
        url = "AddN.aspx";
    }

    if (p_TempCnt != "0") {
        if ($.browser.msie) {
            url = "Mod.aspx";
        } else {
            url = "ModN.aspx";
        }
    }

    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "Template", "1000", "740")
}

/**************************************************
작성자   : shine
작성일자 : 2011-10-18
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
작성자   : shine
작성일자 : 2011-10-18
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

/**************************************************
작성자   : shimpark
작성목적 : 대분류 순서 변경하기
***************************************************/
List.MoveUpdown = function () {
    ParamArray = {
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Coop/Admin/Category/MoveUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 소분류 순서 변경하기
***************************************************/
List.MoveSubUpdown = function () {
    ParamArray = {
        'itemID': ItemID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Coop/Admin/Category/MoveSubUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 자주지정자 순서변경
***************************************************/
List.MoveFavUpdown = function (_itemSubID) {
    ParamArray = {
        'itemSubID': _itemSubID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Coop/Admin/Category/FavorUserUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "-500", "", true);
}

/**************************************************
작성자   : shimpark
작성목적 : 자주지정자 추가
***************************************************/
List.AddFav = function (_itemSubID) {
    ItemSubID = _itemSubID;

    ParamArray = {
        'gubun': "2"
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Coop/Common/GetUserList.aspx?p=" + GetEncryptValue;
    PopupWin(800, 485, url, "-500", "", true);
}

/**************************************************
작성자   :
작성일자 : 심재운
작성목적 : 자주 지정 사용자 저장하기
***************************************************/
function GetDept(rtnVal) {
    if (rtnVal != "undefined" && unescape(rtnVal) != "" && rtnVal != null) {
        var sList = unescape(rtnVal);

        var tmpArr = sList.split("‡");
        var idArr = tmpArr[0].split(","); //userid

        var userList = "";

        for (var i = 0; i < idArr.length; i++) {
            var uid = idArr[i];
            if (uid != "") {
                userList += uid + ",";
            }
        }

        //추가하기
        if (userList != "") {
            var url = "/DeskPlusEIP/Coop/Ws/Admin/Category/FavorUserUpdown.asmx/Add";
            ParamArray = {
                'itemSubID': ItemSubID,
                'userList': userList
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("List.SuccessFavUser");

            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 자주 지정자 처리시 , 완료
**********************************************************************************************/
List.SuccessFavUser = function (data, status) {
    if (data.d == true) {
        alert(R_CompleteConfirm);
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 자주 지정자 명단 보기
***************************************************/
List.ListFav = function (_itemSubID) {
    ItemSubID = _itemSubID;

    ParamArray = {
        'itemSubID': ItemSubID
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Coop/Admin/Category/FavorUserList.aspx?p=" + GetEncryptValue;

    PopupWin(700, 500, url, "-500", "", true);
}