ModTab = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //폴더 이동
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            ModTab.Move();
        });

        //포틀릿 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            ModTab.Modify();
        });

        //포틀릿 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            ModTab.Del();
        });

        //목록으로
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            ModTab.Cancel();
        });

        //탭 추가
        $('#cbtnAddTab').click(function (e) {
            e.preventDefault();
            ModTab.AddTab();
        });

        //탭 순서
        $('#cbtnOrderTab').click(function (e) {
            e.preventDefault();
            ModTab.OrderTab();
        });

        //권한 추가
        $('#cbtnAddAuth').click(function (e) {
            e.preventDefault();
            ModTab.AddAuth();
        });

        //IP 추가
        $('#cbtnAddIP').click(function (e) {
            e.preventDefault();
            ModTab.AddPortletIP();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            ModTab.AttachSwfFile();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiLang').attr("disabled", true);

        if (PhotoName != "") {
            $("#photoImg")[0].innerHTML = "<img src='" + PhotoName + "' width='120' height='60' />";
        }

        $('#ctbPortletName').focus();

        ModTab.TabList();
        ModTab.RoleList();
        ModTab.PortletIPList();
        ModTab.Resource();

    });
});


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 포틀릿 삭제
**********************************************************************************************/
ModTab.Del = function () {

    if (window.confirm(R_ConfirmDelete)) {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/DelPortlet";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'categoryID': CategoryID,
            'portletList': PortletID
        };

        var obj = eval("ModTab.SuccessDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 포틀릿 삭제
**********************************************************************************************/
ModTab.SuccessDel = function (data, status) {

    if (data.d == true) {

        ParamArray = {
            'categoryID': CategoryID,
            'recordPerPage': RecordPerPage,
            'currentPage': CurrentPage,
            'searchField': SearchField,
            'searchValue': SearchValue,
            'order': Order
        };

        CallEncrypt(CallGetParam());
        location = "List.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 폴더 이동하기
**********************************************************************************************/
ModTab.Move = function () {

    ParamArray = {
        'companyID': CompanyID,
        'categoryID': CategoryID,
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "Move.aspx?p=" + GetEncryptValue;

    PopupWin(380, 390, url, "", "", true);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 포틀릿 수정
**********************************************************************************************/
ModTab.Modify = function () {
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletName", R_InputPortletName, true);
    //validator.checkRequired("ctbPortletCode", "포틀릿코드를 입력해 주세요.", true);
    //validator.checkRequired("ctbUIURL", "link 경로 값을 입력해 주세요.", true);
    if (!validator.validate()) return;

    //    ModTab.GetCountCode();

    //    if (CountCode == 1) {
    //        alert(R_DuplicatePortletCode);
    //        $("#ctbPortletCode").focus();
    //        return;
    //    }

    __doPostBack('lbtnRegister', '');
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 탭 추가
**********************************************************************************************/
ModTab.AddTab = function () {

    ParamArray = {
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "AddPortlet.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 탭 수정
**********************************************************************************************/
ModTab.ModTab = function (p_id) {

    ParamArray = {
        'portletID': PortletID,
        'portletTabID': p_id
    };

    CallEncrypt(CallGetParam());
    var url = "AddPortlet.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 탭 순서
**********************************************************************************************/
ModTab.OrderTab = function () {

    ParamArray = {
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "TabUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(350, 400, url, "", "", true);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 권한 추가
**********************************************************************************************/
ModTab.AddAuth = function () {

    ParamArray = {
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "AddAuth.aspx?p=" + GetEncryptValue;

    //PopupWin(700, 495, url, "50", "", true);
    PopupWin(800, 500, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭 리스트
**********************************************************************************************/
ModTab.TabList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/GetPortletTabList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("ModTab.SuccessTabList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 데이터연동 리스트
**********************************************************************************************/
ModTab.SuccessTabList = function (data, status) {

    $('#placeholder_Tab').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTTabContent.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_Tab').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 탭 삭제
**********************************************************************************************/
ModTab.DelTab = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/DelPortletTab";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletID': PortletID,
            'portletTabID': p_id
        };

        var obj = eval("ModTab.SuccessDelTab");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 탭 삭제
**********************************************************************************************/
ModTab.SuccessDelTab = function (data, status) {

    if (data.d == true) {
        ModTab.TabList();
        ModTab.Resource();
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 리스트
**********************************************************************************************/
ModTab.RoleList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/GetPortletRoleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("ModTab.SuccessRoleList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 권한접근제어 리스트
**********************************************************************************************/
ModTab.SuccessRoleList = function (data, status) {

    $('#placeholder_Role').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTTabRole.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_Role').processTemplate(data.d);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 삭제
**********************************************************************************************/
ModTab.DelRole = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/DelPortletRole";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletRoleID': p_id
        };

        var obj = eval("ModTab.SuccessDelRole");
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
ModTab.SuccessDelRole = function (data, status) {

    if (data.d == true) {
        ModTab.RoleList();
        ModTab.Resource();
    }
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 리소스
**********************************************************************************************/
ModTab.Resource = function () {
    $("#r_taborder")[0].innerHTML = R_TabOrder;
    $("#r_tabName")[0].innerHTML = R_TabName;
    $("#r_portletname")[0].innerHTML = R_PortletNames;
    $("#r_urlinfo")[0].innerHTML = R_URLInfo;
    $("#r_tabDel")[0].innerHTML = R_Del;
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

    if ($(".IconAdd") != undefined) { $(".IconAdd").attr('src', ImagePath + '/Icon/ico_add.gif'); }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 목록으로
**********************************************************************************************/
ModTab.Cancel = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location = "List.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : link url 확인
**********************************************************************************************/
ModTab.LinkURLCheck = function () {
    location.href = "List.aspx";
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 권한 새창에서 권한리스트 갱신
**********************************************************************************************/
function LoadRoleList() {
    ModTab.RoleList();
    ModTab.Resource();
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 권한 새창에서 권한리스트 갱신
**********************************************************************************************/
ModTab.ModTabName = function (p_id, p_multiName) {
    PortletTabID = p_id;
    CallMultiName(p_multiName, "GetMultiNames", MultiLang);
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiNames(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    if (PortletTabID != "") {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/ModPortletTabName";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletID': PortletID,
            'portletTabID': PortletTabID,
            'nameko': name_ko,
            'nameEn': name_en,
            'nameJa': name_ja,
            'nameFr': name_fr,
            'nameZhcn': name_zhcn,
            'nameZhtw': name_zhtw
        };

        var obj = eval("ModTab.SuccessModTabName");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 탭이름 변경
**********************************************************************************************/
ModTab.SuccessModTabName = function (data, status) {

    if (data.d == true) {
        alert(R_ConfirmChange);
        ModTab.TabList();
        ModTab.Resource();
    }
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이미지 첨부파일 추가
***************************************************/
ModTab.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'EIP',
        'callBackEvent': "ModTab.GetAttachFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 사진 이미지 넣기
**********************************************************************************************/
ModTab.GetAttachFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModTab.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
ModTab.SuccessAttachFile = function (data, status) {

    for (var count in data.d) {
        var photoPath = data.d[count].FilePath;
        $("#photoImg")[0].innerHTML = "<img src='" + photoPath + "' width='120' height='60' />";
        $("#tbPhotoNEW").val(data.d[count].FileName);
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

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : PortletCode 로 카운트 가져오기 (중복검열)
**********************************************************************************************/
ModTab.GetCountCode = function () {
    var validator = new Validator($("#form1"));
    //validator.checkRequired("ctbPortletCode", "포틀릿 코드를 입력해 주세요.", true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/GetCountByPortletCode";

    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID,
        'portletCode': $("#ctbPortletCode").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModTab.SuccessGetCountPortletCode");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시, PortletCode 로 카운트 가져오기
**********************************************************************************************/
ModTab.SuccessGetCountPortletCode = function (data, status) {
    CountCode = data.d;
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : IP 추가
**********************************************************************************************/
ModTab.AddPortletIP = function () {

    ParamArray = {
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "AddIP.aspx?p=" + GetEncryptValue;

    PopupWin(550, 130, url, "", "", true);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : ip 접근제어 리스트
**********************************************************************************************/
ModTab.PortletIPList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/GetPortletIPList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("ModTab.SuccessPortletIPList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, ip 접근제어 리스트
**********************************************************************************************/
ModTab.SuccessPortletIPList = function (data, status) {

    $('#placeholder_IP').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTIP.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_IP').processTemplate(data.d);
}



/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : IP 삭제
**********************************************************************************************/
DelPortletIP = function (p_id) {

    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/DelPortletIP";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletID': p_id
        };

        var obj = eval("ModTab.SuccessDelPortletIP");
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
ModTab.SuccessDelPortletIP = function (data, status) {

    if (data.d == true) {
        ModTab.PortletIPList();
        ModTab.Resource();
    }
}