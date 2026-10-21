
GetTabUser = {}

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

            GetTabUser.OrderTab();
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
            GetTabUser.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


        //권한 추가
        $('#cbtnADDAuth').click(function (e) {
            e.preventDefault();
            GetTabUser.AddAuth();
        });

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiNamePopup($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        if (ExistTab == "0") {
            location = "/DeskPlusEIP/EIP/Portal/AddTab.aspx";
        }

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
GetTabUser.OrderTab = function () {
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
GetTabUser.DelTabRole = function (p_id) {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/GetTabUser.asmx/DelTabRole";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID,
        'tabRoleID': p_id
    };

    var obj = eval("GetTabUser.SuccessDelTabRole");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시, 텝삭제
**********************************************************************************************/
GetTabUser.SuccessDelTabRole = function (data, status) {

    if (data.d == true) {
        GetTabUser.TabRoleList();
        GetTabUser.Resource
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
GetTabUser.Confirm = function () {

    $('#ctbMultiName').attr("disabled", false);

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbBaseName", R_InputTabName, true);
    if (!validator.validate()) return;

    var layout = $("input[name=rbLayout]:checked").val();
    if (layout < 1) {
        alert(R_SettingLayout);
        return;
    }

    var url = "/DeskPlusEIP/EIP/Ws/Portal/GetTabUser.asmx/ModTab";

    var name = $('#ctbBaseName').val();
    var multiName = $('#ctbMultiName').val();
    var layout = $("input[name=rbLayout]:checked").val();
    var activeTab = $('#ddlActive').val();

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'tabID': TabID,
        'name': name,
        'multiName': multiName,
        'layout': layout,
        'activeTab': activeTab
    };

    var obj = eval("GetTabUser.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 성공시, 탭설정 수정
**********************************************************************************************/
GetTabUser.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_ConfirmMod);

        ParamArray = {
            'tabID': TabID
        };

        CallEncrypt(CallGetParam());

        //이동하기    
        location = "GetTabUser.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자:
작성일:2011-11-14
작성목적 : 리소스
***************************************************/
GetTabUser.Resource = function () {

}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-04-14
작성목적 : 텝이동하기
**********************************************************************************************/
GetTabUser.GoTab = function (p_id) {
    ParamArray = {
        'tabID': p_id
    };

    CallEncrypt(CallGetParam());

    //이동하기    
    location = "GetTabUser.aspx?p=" + GetEncryptValue;
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