Mod = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //폴더 이동
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            Mod.Move();
        });

        //포틀릿 수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            Mod.Modify();
        });

        //포틀릿 삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Mod.Del();
        });

        //목록으로
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //파라미터 추가
        $('#cbtnADDParam').click(function (e) {
            e.preventDefault();
            Mod.AddDataLink();
        });

        //권한 추가
        $('#cbtnADDAuth').click(function (e) {
            e.preventDefault();
            Mod.AddAuth();
        });

        //IP 추가
        $('#cbtnAddIP').click(function (e) {
            e.preventDefault();
            Mod.AddPortletIP();
        });

        //RSS2.0 Data 확인
        $('#cbtnDataCheck').click(function (e) {
            e.preventDefault();
            Mod.CheckData();
        });

        //링크 URL 확인
        $('#cbtnLinkURLCheck').click(function (e) {
            e.preventDefault();
            Mod.LinkURLCheck();
        });

        //ui형식
        $('#ddlUIType').change(function () {
            //탬플릿일 경우
            if ($('#ddlUIType').val() == "T") {
                $("#ddlUITemplate").show();
                $("#ctbUIURL").hide();
            }
            else {
                $("#ddlUITemplate").hide();
                $("#ctbUIURL").show();
            }
        });

        //link설정
        $('#ddlLinkType').change(function () {
            //하단프레임경우
            if ($('#ddlLinkType').val() == "C") {
                $("#ddlPopupWidth").hide();
                $("#ctbPopupWidth").hide();
                $("#splitChar").hide();
                $("#ddlPopupHeight").hide();
                $("#ctbPopupHeight").hide();
            }
            else {
                $("#ddlPopupWidth").show();
                $("#ctbPopupWidth").show();
                $("#splitChar").show();
                $("#ddlPopupHeight").show();
                $("#ctbPopupHeight").show();
            }
        });

        //가로
        $('#ddlPopupWidth').change(function () {
            if ($('#ddlPopupWidth').val() == "0") {
                $("#ctbPopupWidth").hide();
            }
            else {
                $("#ctbPopupWidth").show();
                $("#ctbPopupWidth").focus();
            }
        });

        //세로
        $('#ddlPopupHeight').change(function () {
            if ($('#ddlPopupHeight').val() == "0") {
                $("#ctbPopupHeight").hide();
            }
            else {
                $("#ctbPopupHeight").show();
                $("#ctbPopupHeight").focus();
            }
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
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

        Mod.LoadData();
        Mod.DataLinkList();
        Mod.RoleList();
        Mod.PortletIPList();
        Mod.Resource();
    });
});

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 초기화
**********************************************************************************************/
Mod.LoadData = function () {
    $('#ctbPortletName').focus();

    //탬플릿일 경우
    if ($('#ddlUIType').val() == "T") {
        $("#ddlUITemplate").show();
        $("#ctbUIURL").hide();
    }
    else {
        $("#ddlUITemplate").hide();
        $("#ctbUIURL").show();
    }

    //하단프레임경우
    if ($('#ddlLinkType').val() == "C") {
        $("#ddlPopupWidth").hide();
        $("#ctbPopupWidth").hide();
        $("#splitChar").hide();
        $("#ddlPopupHeight").hide();
        $("#ctbPopupHeight").hide();
    }
    else {
        $("#ddlPopupWidth").show();
        $("#ctbPopupWidth").show();
        $("#splitChar").show();
        $("#ddlPopupHeight").show();
        $("#ctbPopupHeight").show();

        if ($('#ddlPopupWidth').val() == "0") {
            $("#ctbPopupWidth").hide();
        }

        if ($('#ddlPopupHeight').val() == "0") {
            $("#ctbPopupHeight").hide();
        }
    }
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 폴더 이동하기
**********************************************************************************************/
Mod.Move = function () {
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
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 포틀릿 삭제
**********************************************************************************************/
Mod.Del = function () {
    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/DelPortlet";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'categoryID': CategoryID,
            'portletList': PortletID
        };

        var obj = eval("Mod.SuccessDel");
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
Mod.SuccessDel = function (data, status) {
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
작성목적 : 포틀릿 수정
**********************************************************************************************/
Mod.Modify = function () {
    $('#ctbMultiLang').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletName", R_InputPortletName, true);
    //validator.checkRequired("ctbPortletCode", "포틀릿코드를 입력해 주세요.", true);
    //validator.checkRequired("ctnDataURL", "Data 형식 값을 입력해 주세요.", true);
    //validator.checkRequired("ctbLinkURL", "link url 값을 입력해 주세요.", true);
    if ($('#ddlDataType').val() != "E") {
        validator.checkRequired("ctnDataURL", R_InputDataFormat, true);
    }

    if (!validator.validate()) return;

    //탬플릿이 아닐 경우
    if ($('#ddlUIType').val() != "T") {
        if ($("#ctbUIURL").val().trimText() == "") {
            alert(R_InputUIFormatURLValue);
            $("#ctbUIURL").focus();
            return;
        }
    }

    //link 설정
    /*
    if ($('#ddlLinkType').val() != "C") {
    if ($("#ddlPopupWidth").val() == "1") {
    if ($("#ctbPopupWidth").val().trimText() == "") {
    alert("가로 창 크기값을 입력해 주세요.");
    $("#ctbPopupWidth").focus();
    return;
    }
    }

    if ($("#ddlPopupHeight").val() == "1") {
    if ($("#ctbPopupHeight").val().trimText() == "") {
    alert("세로 창 크기값을 입력해 주세요.");
    $("#ctbPopupHeight").focus();
    return;
    }
    }
    }
    */

    //    Mod.GetCountCode();

    //    if (CountCode == 1) {
    //        alert("포틀릿코드가 중복되었습니다.");
    //        $("#ctbPortletCode").focus();
    //        return;
    //    }

    __doPostBack('lbtnRegister', '');
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 파라미터 추가 (메뉴, 포틀릿에서 사용 - 공통 폴더로 빼는 방안 검토)
**********************************************************************************************/
Mod.AddDataLink = function () {
    ParamArray = {
        'portletID': PortletID
    };

    CallEncrypt(CallGetParam());
    var url = "AddParam.aspx?p=" + GetEncryptValue;

    PopupWin(500, 335, url, "", "", true);
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 권한 추가  (메뉴, 포틀릿에서 사용 - 공통 폴더로 빼는 방안 검토)
**********************************************************************************************/
Mod.AddAuth = function () {
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
작성목적 : 데이터연동 리스트
**********************************************************************************************/
Mod.DataLinkList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/GetPortletDataLinkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("Mod.SuccessDataLinkList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 데이터연동 리스트
**********************************************************************************************/
Mod.SuccessDataLinkList = function (data, status) {
    $('#placeholder_DataLink').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTDataLink.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_DataLink').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터연동 삭제
**********************************************************************************************/
Mod.DelDataLink = function (p_id) {
    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/DelPortletDataLink";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletDataLinkID': p_id
        };

        var obj = eval("Mod.SuccessDelDataLink");
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
Mod.SuccessDelDataLink = function (data, status) {
    if (data.d == true) {
        Mod.DataLinkList();
        Mod.Resource();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 리스트
**********************************************************************************************/
Mod.RoleList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/GetPortletRoleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("Mod.SuccessRoleList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 권한접근제어 리스트
**********************************************************************************************/
Mod.SuccessRoleList = function (data, status) {
    $('#placeholder_Role').setTemplateURL('/DeskPlusEIP/EIP/Jt/admin/Portlet/JTRole.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_Role').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 권한접근제어 삭제
**********************************************************************************************/
Mod.DelRole = function (p_id) {
    if (window.confirm(R_ConfirmDelete)) {
        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/DelPortletRole";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletRoleID': p_id
        };

        var obj = eval("Mod.SuccessDelRole");
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
Mod.SuccessDelRole = function (data, status) {
    if (data.d == true) {
        Mod.RoleList();
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
    $("#r_targettype")[0].innerHTML = R_Target;
    $("#r_targetdatetype")[0].innerHTML = R_TargetData;
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
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 목록으로
**********************************************************************************************/
Mod.Cancel = function () {
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
작성일자 : 2011-12.26
작성목적 : RSS2.0 Data 확인
**********************************************************************************************/
Mod.CheckData = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctnDataURL", R_InputDataFormat, true);
    if (!validator.validate()) return;

    var url = $("#ctnDataURL").val();
    var WindowObjectReference = PopupWinOpen(url, "", "", "1024", "768")
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : link url 확인
**********************************************************************************************/
Mod.LinkURLCheck = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbLinkURL", R_InputLinkURLValue, true);
    if (!validator.validate()) return;

    var url = $("#ctbLinkURL").val();
    var WindowObjectReference = PopupWinOpen(url, "", "", "1024", "768")
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이미지 첨부파일 추가
***************************************************/
Mod.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'fileCount': '1',
        'moduleName': 'EIP',
        'callBackEvent': "Mod.GetAttachFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "520")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 사진 이미지 넣기
**********************************************************************************************/
Mod.GetAttachFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    for (var count in data.d) {
        var photoPath = data.d[count].FilePath;
        $("#photoImg")[0].innerHTML = "<img src='" + photoPath + "' width='120' height='60' />";
        $("#tbPhotoNEW").val(data.d[count].FileName);
    }
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : 권한 새창에서 권한리스트 갱신
**********************************************************************************************/
function LoadRoleList() {
    Mod.RoleList();
    Mod.Resource();
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
Mod.GetCountCode = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPortletCode", R_InputPortletCode, true);
    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/Mod.asmx/GetCountByPortletCode";

    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID,
        'portletCode': $("#ctbPortletCode").val()
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetCountPortletCode");

    AjaxService(url, data, obj)
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 성공시, PortletCode 로 카운트 가져오기
**********************************************************************************************/
Mod.SuccessGetCountPortletCode = function (data, status) {
    CountCode = data.d;
}

/**********************************************************************************************
작성자   :
작성일자 : 2011-11-21
작성목적 : IP 추가
**********************************************************************************************/
Mod.AddPortletIP = function () {
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
Mod.PortletIPList = function () {
    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/ModTab.asmx/GetPortletIPList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'portletID': PortletID
    };

    var obj = eval("Mod.SuccessPortletIPList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, ip 접근제어 리스트
**********************************************************************************************/
Mod.SuccessPortletIPList = function (data, status) {
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

        var obj = eval("Mod.SuccessDelPortletIP");
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
Mod.SuccessDelPortletIP = function (data, status) {
    if (data.d == true) {
        Mod.PortletIPList();
        Mod.Resource();
    }
}