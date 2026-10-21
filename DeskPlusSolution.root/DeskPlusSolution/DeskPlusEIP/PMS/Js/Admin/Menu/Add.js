var Add = {}

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Add.Mod();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.Del();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBMultiName').attr("disabled", true);
    });
});

/**************************************************
작성자   : famo23
작성일자 : 2011-10-18
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

Add.Move = function (dir) {
    var menuID = $("#hdMenuID").val();
    if (menuID == "") {
        alert(MSG_NOSELECT_MENU);
        return;
    }

    var activeEl = document.getElementById("MenuItem_" + menuID);
    var targetEl = (dir == "up") ? activeEl.previousSibling : activeEl.nextSibling;
    if (targetEl) {
        var result = SetMenuOrder(CompanyID, ProjectID, menuID, dir);
        if (result.toLowerCase() == "true") {
            activeEl.swapNode(targetEl);
        }
    }
}

Add.SelectItem = function (_menuID) {
    // 메뉴 선택
    //var oldMenuID = $("#hdMenuID").val();
    //if (oldMenuID != "") $("#MenuItem_" + oldMenuID).attr("class", "");
    //$("#MenuItem_" + _menuID).attr("class", "selected");
    //if (oldMenuID != "") $("#MenuItem_" + oldMenuID).removeClass("selected");

    //    $(".PMSMenu li").removeAttr("class");
    //    $("#MenuItem_" + _menuID).attr("class", "selected");

    //    alert(oldMenuID + " : " + _menuID);

    // 메뉴 정보 가져오기 (Ajax)

    var s = AajxGetMenuData(CompanyID, ProjectID, _menuID, PreLang);
    var item;
    eval("item=" + s);

    $("#tbMenuName").val(decodeURIComponent(item.MenuBaseName));
    $("#CTBMultiName").val(item.MenuMultiLang);
    document.getElementById("cbIsUse").checked = (item.IsUse == "1") ? true : false;
    $("#tbMenuDesc").val(decodeURIComponent(item.MenuDesc));

    $("#hdMenuID").val(_menuID);
    $("#hdMenuType").val(item.MenuType);
}

Add.Del = function () {
    var menuID = $("#hdMenuID").val();
    var menuType = $("#hdMenuType").val();
    if (menuID == "") {
        alert(MSG_NOSELECT_MENU);
        return;
    }

    if (menuType != "4") {
        alert(MSG_NODELETE);
        return;
    }

    if (confirm(MSG_CONFIRM_DEL)) {
        __doPostBack("lbtnDel", "");
    }
}

Add.Add = function () {
    $('#CTBMultiName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("tbMenuName", Menu_Register_MSGName, true);
    if (!validator.validate()) {
        $("#tbMenuName").select();
        return;
    }

    __doPostBack("lbtnAdd", "");
}

Add.Mod = function () {
    $('#CTBMultiName').attr("disabled", false);
    var menuID = $("#hdMenuID").val();
    if (menuID == "") {
        alert(MSG_NOSELECT_MENU);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbMenuName", Menu_Register_MSGName, true);
    if (!validator.validate()) {
        $("#tbMenuName").select();
        return;
    }

    __doPostBack("lbtnMod", "")
}