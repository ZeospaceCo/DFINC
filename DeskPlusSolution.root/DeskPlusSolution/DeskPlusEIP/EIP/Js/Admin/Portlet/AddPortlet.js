AddPortlet = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //포틀릿 추가
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            if (PortletTabID != "0") {
                AddPortlet.Mod();
            }
            else {
                AddPortlet.Add();
            }
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $("#ctbTabName").focus();
    });
});


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 포틀릿 탭 추가히기
***************************************************/
AddPortlet.Add = function () {

    if (TargetPortletID != "" && portletType == "P") {


        var validator = new Validator($("#form1"));
        validator.checkRequired("ctbTabName", "탭이름을 기재해 주세요.", true);
        if (!validator.validate()) return;  

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddPortlet.asmx/AddPortletTab";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletID': PortletID,
            'targetPortletID': TargetPortletID,
            'name': $("#ctbTabName").val()
        };

        var obj = eval("AddPortlet.SuccessAdd");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
    else {
        alert(R_CheckPortlet);
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 포틀릿 탭 수정하기
***************************************************/
AddPortlet.Mod = function () {

    if (TargetPortletID != "" && portletType == "P" && PortletTabID != "0") {

        var url = "/DeskPlusEIP/EIP/Ws/Admin/Portlet/AddPortlet.asmx/ModPortletTab";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'portletID': PortletID,
            'portletTabID': PortletTabID,
            'targetPortletID': TargetPortletID,
            'name': $("#ctbTabName").val()
        };

        var obj = eval("AddPortlet.SuccessAdd");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
    else {
        alert(R_CheckPortlet);
    }
}


/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시, 탭추가하기
**********************************************************************************************/
AddPortlet.SuccessAdd = function (data, status) {

    if (data.d == true) {
        parent.ModTab.TabList();
        parent.ModTab.Resource();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 메뉴 관리 선택
***************************************************/
function PortletNodeSelect_onNodeSelect(node) {

    $("#ctbTargetPortlet").val("");

    TargetPortletID = node.GetProperty("KeyID");
    portletType = node.GetProperty("PortletType");

    var categoryOrder = node.GetProperty("CategoryOrder");
    var levels = node.GetProperty("Levels");      

    if (TargetPortletID != "" && portletType == "P") {
        $("#ctbTargetPortlet").val(node.Text);
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}