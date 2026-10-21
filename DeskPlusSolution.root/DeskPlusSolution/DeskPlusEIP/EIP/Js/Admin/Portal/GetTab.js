
GetTab = {}

$(function () {
    $(document).ready(function () {

        //탭추가
        $('#cbtnAddTab').click(function (e) {
            e.preventDefault();

            location = "AddTab.aspx";
        });

        //탭순서
        $('#cbtnOrderTab').click(function (e) {
            e.preventDefault();

            GetTab.OrderTab();
        });

        //탭삭제
        $('#cbtnDelTab').click(function (e) {
            e.preventDefault();
            if (TabID > 0) {
                if (window.confirm(R_ConfirmDelete)) {
                    __doPostBack("lbtnSubmit", "del");
                }
            }
        });

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            GetTab.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        //권한 추가
        $('#cbtnADDAuth').click(function (e) {
            e.preventDefault();
            GetTab.AddAuth();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiNamePopup($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        if (ExistTab == "0") {
            location = "/DeskPlusEIP/EIP/Admin/Portal/AddTab.aspx";
        }

        //권한리스트
        GetTab.TabRoleList();
        

        //레이아웃 버튼 클릭 이벤트
        $("[name=rbLayout]").change(function () {
            for (var i = 1; i <= 7; i++) {
                $("#imglayout" + i).attr("src", ImagePath + "/Layout/" + i + ".gif");
            }

            var selID = $('input[name=rbLayout]:checked').val();
            $("#imglayout" + selID).attr("src", ImagePath + "/Layout/" + selID + "_on.gif");
        });

        $('#ctbMultiName').attr("disabled", true);
        $('#ctbBaseName').focus();

    });
});


/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 
***************************************************/
GetTab.OrderTab = function () {
    ParamArray = {
        'companyID': CompanyID,        
        'tabID': TabID
    };

    CallEncrypt(CallGetParam());

    var url = "ModTabOrder.aspx";
    var parm = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, parm, "ModTabOrder", "500", "400")
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 탭권한 삭제
***************************************************/
GetTab.DelTabRole = function (p_id) {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portal/GetTab.asmx/DelTabRole";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID,
        'tabRoleID': p_id
    };

    var obj = eval("GetTab.SuccessDelTabRole");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시, 텝삭제
**********************************************************************************************/
GetTab.SuccessDelTabRole = function (data, status) {

    if (data.d == true) {
        GetTab.TabRoleList();
        GetTab.Resource();
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 탭수정
***************************************************/
GetTab.Confirm = function () {

    $('#ctbMultiName').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbBaseName", R_InputTabName, true);
    if (!validator.validate()) return;

    var layout = $("input[name=rbLayout]:checked").val();
    if (layout < 1) {
        alert(R_SettingLayout);
        return;
    }

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portal/GetTab.asmx/ModTab";

    var name = $('#ctbBaseName').val();
    var multiName = $('#ctbMultiName').val();
    var isModify = $('#ddlIsModify').val();
    var layout = $("input[name=rbLayout]:checked").val();
    var activeTab = $('#ddlActive').val();

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID,
        'name': name,
        'multiName': multiName,
        'isModify': isModify,
        'layout': layout,
        'activeTab': activeTab
    };

    var obj = eval("GetTab.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시, 탭설정 수정
**********************************************************************************************/
GetTab.SuccessConfirm = function (data, status) {    
    if (data.d == true) {
        alert(R_ConfirmMod);

        ParamArray = {
            'tabID': TabID
        };

        CallEncrypt(CallGetParam());

        //이동하기    
        location = "GetTab.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 리소스
***************************************************/
GetTab.Resource = function () {

    $("#r_RoleName")[0].innerHTML = R_Auth;
    $("#r_delRole")[0].innerHTML = R_Del;

    if ($(".btn_del2")[0] != undefined) {
        $(".btn_del2")[0].innerHTML = R_Del;
    }


    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NotExistContent;
    }
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 권한 추가 
**********************************************************************************************/
GetTab.AddAuth = function () {
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID
    };

    CallEncrypt(CallGetParam());

    var url = "AddAuth.aspx";
    var parm = "p=" + GetEncryptValue;

    WindowObjectReference = PopupWinOpen(url, parm, "AddAuth", "800", "500")
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 권한접근제어 리스트
**********************************************************************************************/
GetTab.TabRoleList = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Admin/Portal/GetTab.asmx/GetTabRoleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID
    };

    var obj = eval("GetTab.SuccessTabRoleList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시, 권한접근제어 리스트
**********************************************************************************************/
GetTab.SuccessTabRoleList = function (data, status) {
    $('#plAuthList').setTemplateURL('/DeskPlusEIP/EIP/Jt/Admin/Portal/JTAuth.htm?seq=' + Guids(), null, { filter_data: false });
    $('#plAuthList').processTemplate(data.d);
    GetTab.Resource();
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 텝이동하기
**********************************************************************************************/
GetTab.GoTab = function (p_id) {
    ParamArray = {
        'tabID': p_id
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = "GetTab.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
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

    $("#ctbMultiName").val(multiNames);
}