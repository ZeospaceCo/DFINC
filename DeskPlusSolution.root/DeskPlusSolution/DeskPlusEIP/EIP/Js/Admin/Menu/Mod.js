Mod = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //메뉴 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Mod.Add();
        });

        //메뉴 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Mod.Del();
        });


        //파라미터 추가
        $('#cbtnADDParam').click(function (e) {
            e.preventDefault();
            Mod.AddMenuDataLink();
        });

        //권한 추가
        $('#cbtnADDAuth').click(function (e) {
            e.preventDefault();
            Mod.AddAuth();
        });

        //IP 추가
        $('#cbtnADDIP').click(function (e) {
            e.preventDefault();
            Mod.AddIP();
        });

        //메뉴 순서 이동
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            Mod.ModOrder();
        });

        //상위메뉴이동
        $('#cbtnChagneParent').click(function (e) {
            e.preventDefault();
            Mod.ModParent();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        Mod.MenuDataLinkList();
        Mod.MenuRoleList();
        Mod.MenuIPList();
        Mod.Resource();

        $('#ctbMultiName').attr("disabled", true);
        $('#ctbMenuName').focus();
    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 메뉴추가
**********************************************************************************************/
Mod.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbMenuName", R_InputMenuName, true);                      //"이름을 입력해 주세요"
    //validator.checkRequired("ctbTargetURL", "대상 URL 을 입력해 주세요", true);                //"로그인 아이디를 입력해 주세요"    
    if (!validator.validate()) return;

    $('#ctbMultiName').attr("disabled", false);
    __doPostBack('lbtnWrite', '');
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

    $("#ctbMultiName").val(multiNames);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 메뉴삭제
**********************************************************************************************/
Mod.Del = function () {
    if (window.confirm(R_ConfirmDeleteMenu)) {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/DelMenu";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuID': MenuID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessDel");

        AjaxService(url, data, obj);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 삭제
**********************************************************************************************/
Mod.SuccessDel = function (data, status) {
    if (data.d == "1") {
        parent.frmLeft.location = "Tree.aspx";
    }
    else if (data.d == "0") {
        alert(R_ExistSubMenu);
    }
    else if (data.d == "-1") {
        alert(R_NotConfirmUserAccessAuth);
    }
    else {
        alert(R_ErrorProcessing);
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 파라미터 추가 (메뉴, 포틀릿에서 사용 - 공통 폴더로 빼는 방안 검토)
**********************************************************************************************/
Mod.AddMenuDataLink = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "AddParam.aspx?p=" + GetEncryptValue;

    PopupWin(500, 297, url, "", "", true);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터연동 리스트
**********************************************************************************************/
Mod.MenuDataLinkList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/GetMenuDataLinkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID
    };

    var obj = eval("Mod.SuccessMenuDataLinkList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 데이터연동 리스트
**********************************************************************************************/
Mod.SuccessMenuDataLinkList = function (data, status) {

    $('#placeholder_DataLink').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Menu/JTDataLink.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_DataLink').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터연동 삭제
**********************************************************************************************/
Mod.DelMenuDataLink = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/DelMenuDataLink";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuDataLinkID': p_id
        };

        var obj = eval("Mod.SuccessDelMenuDataLink");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 데이터연동 삭제
**********************************************************************************************/
Mod.SuccessDelMenuDataLink = function (data, status) {

    if (data.d == true) {
        Mod.MenuDataLinkList();
        Mod.Resource();
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리소스
**********************************************************************************************/
Mod.Resource = function () {
    $("#r_encripttype")[0].innerHTML = R_EncryptionGubun;
    $("#r_encriptvalue")[0].innerHTML = R_EncryptionValue;
    $("#r_linkname")[0].innerHTML = R_UserName;
    $("#r_linkevalue")[0].innerHTML = R_Values;
    $("#r_del")[0].innerHTML = R_Del;
    $("#r_RoleName")[0].innerHTML = R_UserName;
    $("#r_delRole")[0].innerHTML = R_Del;
    $("#r_ipaccess")[0].innerHTML = R_AccessCheck;
    $("#r_ip")[0].innerHTML = R_IPBand;
    $("#r_delIP")[0].innerHTML = R_Del;

    $(".btn_del").text(R_Del);

    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NotExistContent;
    }
    if ($(".NoListData")[1] != undefined) {
        $(".NoListData")[1].innerHTML = R_NotExistContent;
    }
    if ($(".NoListData")[2] != undefined) {
        $(".NoListData")[2].innerHTML = R_NotExistContent;
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 권한 추가  (메뉴, 포틀릿에서 사용 - 공통 폴더로 빼는 방안 검토)
**********************************************************************************************/
Mod.AddAuth = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "AddAuth.aspx?p=" + GetEncryptValue;

    //PopupWin(700, 485, url, "50", "", true);
    PopupWin(800, 500, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 리스트
**********************************************************************************************/
Mod.MenuRoleList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/GetMenuRoleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID
    };

    var obj = eval("Mod.SuccessMenuRoleList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 권한접근제어 리스트
**********************************************************************************************/
Mod.SuccessMenuRoleList = function (data, status) {

    $('#placeholder_Role').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Menu/JTRole.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_Role').processTemplate(data.d);

}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 삭제
**********************************************************************************************/
Mod.DelMenuRole = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/DelMenuRole";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuRoleID': p_id
        };

        var obj = eval("Mod.SuccessDelMenuRole");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 권한접근제어 삭제
**********************************************************************************************/
Mod.SuccessDelMenuRole = function (data, status) {

    if (data.d == true) {
        Mod.MenuRoleList();
        Mod.Resource();
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : IP 추가
**********************************************************************************************/
Mod.AddIP = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "AddIP.aspx?p=" + GetEncryptValue;

    PopupWin(550, 130, url, "", "", true);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : IP  리스트
**********************************************************************************************/
Mod.MenuIPList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/GetMenuIPList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID
    };

    var obj = eval("Mod.SuccessMenuIPList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, IP  리스트
**********************************************************************************************/
Mod.SuccessMenuIPList = function (data, status) {
    $('#placeholder_IP').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Menu/JTIP.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_IP').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : IP 삭제
**********************************************************************************************/
Mod.DelMenuIP = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Menu/Mod.asmx/DelMenuIP";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuIPID': p_id
        };

        var obj = eval("Mod.SuccessDelMenuIP");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, IP  삭제
**********************************************************************************************/
Mod.SuccessDelMenuIP = function (data, status) {

    if (data.d == true) {
        Mod.MenuIPList();
        Mod.Resource();
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 리스트 이동
**********************************************************************************************/
function GoTree() {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());

    parent.frmLeft.location = "Tree.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 순서변경
**********************************************************************************************/
Mod.ModOrder = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "", "", true);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 상위메뉴변경
**********************************************************************************************/
Mod.ModParent = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "Move.aspx?p=" + GetEncryptValue;

    PopupWin(400, 405, url, "", "", true);
}