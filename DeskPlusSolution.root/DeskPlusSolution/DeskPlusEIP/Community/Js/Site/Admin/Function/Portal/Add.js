Add = {}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-05-12
작성목적 : 초기화 
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        // 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.MenuAdd();

        });

        // 제거
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Add.MenuDel();
        });

        Add.LoadData();
    });
});

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 추가하기
***************************************************/
Add.MenuAdd = function () {
    if ($("#lbxParentCategory").val() == null) {
        alert(R_CheckAddMenu);
        return;
    }

    var communityID = $("#lbxParentCategory").val().split("┐")[0];
    var menuID = $("#lbxParentCategory").val().split("┐")[1];

    if (communityID == 0 || menuID == 0) {
        alert(R_ProcessingError)
        return;
    }

    // 사용중인 메뉴 삭제
    $('#lbxParentCategory option:selected').each(function () {
        $(this).remove();
    });

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Portal/Add.asmx/MenuAdd";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': communityID,
        'menuID': menuID
    };

    var obj = eval("Add.SuccessMenuAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 사용중인 메뉴 추가(웹서비스 값 받기)
***************************************************/
Add.SuccessMenuAdd = function (data, status) {
    if (data.d == true) {
        Add.LoadData();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 삭제하기(웹서비스 호출)
***************************************************/
Add.MenuDel = function () {
    if ($("#lbxSubCategory").val() == null) {
        alert(R_CheckDelMenu);
        return;
    }

    var communityID = $("#lbxSubCategory").val().split("┐")[0];
    var menuID = $("#lbxSubCategory").val().split("┐")[1];

    if (communityID == 0 || menuID == 0) {
        alert(R_ProcessingError)
        return;
    }

    // 사용중인 메뉴 삭제
    $('#lbxSubCategory option:selected').each(function () {
        $(this).remove();
    });

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Portal/Add.asmx/MenuDel";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': communityID,
        'menuID': menuID
    };

    var obj = eval("Add.SuccessMenuDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 삭제하기 (웹서비스 값 받기)
***************************************************/
Add.SuccessMenuDel = function (data, status) {
    if (data.d == true) {
        Add.LoadData();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 데이터 가져오기
***************************************************/
Add.LoadData = function () {
    // 사용중인 메뉴
    Add.UsingMenu();
    // 시작화면에 보여줄 메뉴
    Add.ShowMenu();
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 사용중인 메뉴 가져오기 (웹서비스 호출)
***************************************************/
Add.UsingMenu = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Portal/Add.asmx/GetUsingList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID
    };

    var obj = eval("Add.SuccessUsingList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 사용중인 메뉴 가져오기(웹서비스 값 받기)
***************************************************/
Add.SuccessUsingList = function (data, status) {
    // 기존 리스트 삭제
    $("select[id='lbxParentCategory']").find('option').each(function () {
        $(this).remove();

    });

    // 재바인딩
    $.each(data.d, function (index, list) {

        $("#lbxParentCategory").get(0).options[index] = new Option(list.MenuName, list.CommunityID + "┐" + list.MenuID);
    });
}

/**************************************************
작성자   : shine
작성일자 : 2012-05-31
작성목적 : 시작화면에 보여줄 메뉴 가져오기
***************************************************/
Add.ShowMenu = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Portal/Add.asmx/GetShowList";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID
    };

    var obj = eval("Add.SuccessShowList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : shine
작성일자 : 2012-06-01
작성목적 : 시작화면에 보여줄 메뉴 가져오기(웹서비스 값 받기)
***************************************************/
Add.SuccessShowList = function (data, status) {
    var menuIDList = "";

    // 기존 리스트 삭제
    $("select[id='lbxSubCategory']").find('option').each(function () {
        $(this).remove();

    });

    // 재바인딩
    $.each(data.d, function (index, list) {

        $("#lbxSubCategory").get(0).options[index] = new Option(list.MenuName, list.CommunityID + "┐" + list.MenuID);
        menuIDList += list.MenuID + "┐";
    });
}