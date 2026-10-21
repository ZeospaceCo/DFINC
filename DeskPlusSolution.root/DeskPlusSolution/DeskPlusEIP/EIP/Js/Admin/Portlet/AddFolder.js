AddFolder = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //추가,수정
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddFolder.Confirm();
        });

        //순서
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            AddFolder.ModOrder();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            AddFolder.Del();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiLang').attr("disabled", true);


        $('#ctbName').focus();
    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21 2011-11-21
작성목적 : 추가 및 수정
**********************************************************************************************/
AddFolder.Confirm = function () {
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputGroupName, true);
    if (!validator.validate()) return;    

    var url = "";
    
    if (Gubun == "add") {
        url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddFolder.asmx/AddPortletCategory";
    }
    else if (Gubun == "mod") {
        url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddFolder.asmx/ModPortletCategory";
    }

    if (url != "") {
        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'categoryID': CategoryID,
            'name': $("#ctbName").val(),
            'multiName': $("#ctbMultiLang").val(),
            'description': $("#ctbDesc").val(),
            'activeCategory': $("#ddlActiveCategory").val()
        };

        var obj = eval("AddFolder.SuccessAdd");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-11-21 
작성목적 : 처리 후, 저장
**********************************************************************************************/
AddFolder.SuccessAdd = function (data, status) {
    var response = false;
    
    if (Gubun == "add") {
        if (data.d > 0) {
            response = true;
            alert(R_ConfirmAdd);
            AddFolder.GoTree(data.d);
        }
    }
    else if (Gubun == "mod") {
        if (data.d == true) {
            response = true;
            alert(R_ConfirmMod);
            AddFolder.GoTree(CategoryID);
        }
    }

    if (response == false) {
        alert(R_ProcessingError);
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 리스트 이동
**********************************************************************************************/
AddFolder.GoTree = function(p_categoryID) {

    //파라미터값 배열에 담기
    ParamArray = {
        'categoryID': p_categoryID
    };
    CallEncrypt(CallGetParam());

    parent.frmLeft.location.href = "Tree.aspx?p=" + GetEncryptValue;
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 순서변경
**********************************************************************************************/
AddFolder.ModOrder = function () {

    ParamArray = {
        'categoryID': CategoryID
    };

    CallEncrypt(CallGetParam());
    var url = "Updown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "", "", true);
}



/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 삭제
**********************************************************************************************/
AddFolder.Del = function () {

    if (CategoryID > 0) {

        if (window.confirm(R_ConfirmDelete)) {

            var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddFolder.asmx/DelPortletCategory";

            //파라미터값 배열에 담기
            ParamArray = {
                'companyID': CompanyID,
                'categoryID': CategoryID
            };

            var obj = eval("AddFolder.SuccessDel");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxService(url, data, obj);
        }
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-11-21 
작성목적 : 처리 후, 삭제
**********************************************************************************************/
AddFolder.SuccessDel = function (data, status) {
    if (data.d == true) {
        parent.frmLeft.location.href = "Tree.aspx";

        ParamArray = {
            'gubun': 'add'
        };
        CallEncrypt(CallGetParam());

        parent.frmRight.location.href = "AddFolder.aspx?p=" + GetEncryptValue;    
    }
    else {
        alert(R_ProcessingError);
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

    $("#ctbMultiLang").val(multiNames);
}
