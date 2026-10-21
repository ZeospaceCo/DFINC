var List = {};


/**************************************************
작성목적 : 초기화
***************************************************/
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

        // 대분류 리스트 클릭
        $('#lbxParentCategory').click(function (e) {
            e.preventDefault();
            List.SetCategoryInfo();
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

        // 소분류 리스트 클릭
        $('#lbxSubCategory').click(function (e) {
            e.preventDefault();
            List.SetSubCategoryInfo();
        });

        //대분류 다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMenuName").val(), "GetMultiName", MultiLang);
        });

        // 소분류 다국이름
        $('#MultiNameSub').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbSubMenuName").val(), "GetMultiNameSub", MultiLang);
        });

        $('#ctbMenuName').attr("disabled", true);
        $('#ctbSubMenuName').attr("disabled", true);
       
    });
});

/**************************************************
작성자 : shine 
작성일자 : 2012-05-07
작성목적 : 대분류 추가 
***************************************************/
List.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_BaseName, true);
    if (!validator.validate()) return;

    ParamArray = {
        'categoryName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'parentCategoryID' : 0
    };


    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Add";

    var obj = eval("List.Success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시, 완료
**********************************************************************************************/
List.Success = function (data, status) {
    if (data.d == true) {
        List.GetList();
        alert(R_CompleteConfirm);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 대분류 카테고리 리스트 가져오기
**********************************************************************************************/
List.GetList = function () {

    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/GetList";

    var obj = eval("List.ListSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 리스트 가져오기 성공시, 완료
**********************************************************************************************/
List.ListSuccess = function (data, status) {
    // 기존 리스트 삭제
    $("select[id='lbxParentCategory']").find('option').each(function () {
        $(this).remove();

    });

    // 재바인딩
    $.each(data.d, function (index, list) {

        $("#lbxParentCategory").get(0).options[index] = new Option(list.GetName, list.CategoryID + "┐" + list.NameBase + "┐" + list.MultiName);
    });

    // 대분류, 다국명 텍스트박스 초기화
    if (DelCheck == "1") {
        $("#ctbParentName").val("");
        $("#ctbMenuName").val("");
        $("#ctbSubName").val("");
        $("#ctbSubMenuName").val("");

        $("select[id='lbxSubCategory']").find('option').each(function () {
            $(this).remove();

        });
        DelCheck = "0";
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 대분류 카테고리 선택 시 텍스트 박스 바인딩
**********************************************************************************************/
List.SetCategoryInfo = function () {
    if ($("#lbxParentCategory").val() != null) {
        $("#ctbMenuName").val($("#lbxParentCategory").val().split("┐")[2]);
        $("#ctbParentName").val($("#lbxParentCategory").val().split("┐")[1]);
        CategoryID = $("#lbxParentCategory").val().split("┐")[0];

        List.GetSubCategoryInfo(CategoryID);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 바인딩
**********************************************************************************************/
List.GetSubCategoryInfo = function (categoryID) {

    ParamArray = {
        'parentCategoryID': categoryID
    };

    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/SubList";

    var obj = eval("List.SubListSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 선택한 대분류에 대한 소분류 리스트 가져오기 성공시, 완료
**********************************************************************************************/
List.SubListSuccess = function (data, status) {
    // 기존 리스트 삭제
    $("select[id='lbxSubCategory']").find('option').each(function () {
        $(this).remove();

    });

    // 재바인딩
    $.each(data.d, function (index, list) {

        $("#lbxSubCategory").get(0).options[index] = new Option(list.GetName, list.CategoryID + "┐" + list.NameBase + "┐" + list.MultiName);
    });

    // 소분류, 다국명 텍스트박스 초기화
    $("#ctbSubName").val("");
    $("#ctbSubMenuName").val("");
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 텍스트 박스 바인딩
**********************************************************************************************/
List.SetSubCategoryInfo = function () {
    if ($("#lbxSubCategory").val() != null) {
        $("#ctbSubName").val($("#lbxSubCategory").val().split("┐")[1]);
        $("#ctbSubMenuName").val($("#lbxSubCategory").val().split("┐")[2]);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2011-04-14
작성목적 : 대분류 카테고리 수정 
***************************************************/
List.Mod = function () {

    if ($("#lbxParentCategory").val() == null) {
        alert(R_CheckModList);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbParentName", R_BaseName, true);
    if (!validator.validate()) return;


    $('#ctbMenuName').attr("disabled", false);

    CategoryID = $("#lbxParentCategory").val().split("┐")[0];

    ParamArray = {
        'categoryName': $("#ctbParentName").val(),
        'multiName': $("#ctbMenuName").val(),
        'categoryID': CategoryID
    };

    $('#ctbMenuName').attr("disabled", true);

    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Mod";

    var obj = eval("List.Success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-07
작성목적 : 대분류 삭제 
***************************************************/
List.Del = function () {

    if ($("#lbxParentCategory").val() == null) {
        alert(R_CheckDelList);
        return;
    }

    if (window.confirm(R_DelGubun)) {
        CategoryID = $("#lbxParentCategory").val().split("┐")[0];

        ParamArray = {
            'categoryID': CategoryID
        };

        DelCheck = "1";
        var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Del";

        var obj = eval("List.Success");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-07
작성목적 : 소분류 삭제 
***************************************************/
List.DelSub = function () {
    if ($("#ctbMenuName").val() == "") {
        alert(R_CheckCategory);
        return;
    }

    if ($("#ctbSubName").val() == "") {
        alert(R_CheckDelList);
        return;
    }

    if (window.confirm(R_DelGubun)) {

        SubCategoryID = $("#lbxSubCategory").val().split("┐")[0];

        ParamArray = {
            'categoryID': SubCategoryID
        };

        var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Del";

        var obj = eval("List.SubSuccess");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-07
작성목적 : 소분류 추가 
***************************************************/
List.AddSub = function () {

    if ($("#ctbMenuName").val() == "") {
        alert(R_CheckCategory);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubName", R_SubBaseName, true);
    if (!validator.validate()) return;

    CategoryID = $("#lbxParentCategory").val().split("┐")[0];

    ParamArray = {
        'parentCategoryID': CategoryID,
        'categoryName': $("#ctbSubName").val(),
        'multiName': $("#ctbSubMenuName").val()
    };

    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Add";

    var obj = eval("List.SubSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 소분류 리스트 가져오기 성공시, 완료
**********************************************************************************************/
List.SubSuccess = function (data, status) {
    if (data.d == true) {

        List.GetSubCategoryInfo(CategoryID);
        alert(R_CompleteConfirm);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-07
작성목적 : 소분류 수정 
***************************************************/
List.ModSub = function () {
    
    if ($("#ctbMenuName").val() == "") {
        alert(R_CheckCategory);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubName", R_SubBaseName, true);
    if (!validator.validate()) return;

    $('#ctbSubMenuName').attr("disabled", false);

    SubCategoryID = $("#lbxSubCategory").val().split("┐")[0];

    ParamArray = {
        'categoryName': $("#ctbSubName").val(),
        'multiName': $("#ctbSubMenuName").val(),
        'categoryID': SubCategoryID
    };

    $('#ctbSubMenuName').attr("disabled", true);

    var url = "/DeskPlusEIP/Community/Ws/Admin/Category/List.asmx/Mod";

    var obj = eval("List.SubSuccess");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
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
작성자   : famo23
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