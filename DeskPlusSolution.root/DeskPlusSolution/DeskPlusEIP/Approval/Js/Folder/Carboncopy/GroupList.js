GroupList = {}

/**************************************************
초기화
***************************************************/
$(function () {

    $(document).ready(function () {

        // 닫기 
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        GroupList.LoadData();
    });
});

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
GroupList.LoadData = function () {

    GroupList.GetApprovalUserInfoListCA();
    GroupList.Resource();
}

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 다국어
***************************************************/
GroupList.Resource = function () {

    $("#r_CarbonInfo")[0].innerHTML = R_CarbonInfo;
    $("#r_ViewDate")[0].innerHTML = R_ViewDate;

    if ($(".IconPlus") != undefined) { $(".IconPlus").attr('src', ImagePath + '/Icon/ico_item_plus.gif'); }
    if ($(".IconMinus") != undefined) { $(".IconMinus").attr('src', ImagePath + '/Icon/ico_item_minus.gif'); }

}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 참조자정보 리스트 호출
**********************************************************************************************/
GroupList.GetApprovalUserInfoListCA = function () {
    ParamArray = {
        'documentID': DocumentID,
        'childDocID': ChildDocID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GroupList.SuccessUserInfoListCA");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/GroupList.asmx/GetApprovalUserInfoListCA";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 참조자정보 리스트 보여주기
**********************************************************************************************/
GroupList.SuccessUserInfoListCA = function (data, status) {

    $('#placeholder_CA').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTGroupList.htm?seq=' + Guids());
    $('#placeholder_CA').processTemplate(data.d);

}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 참조자 그룹 보여주기
***************************************************/
GroupList.GroupView = function (_obj, _approvalTag) {

    if ($('img', _obj).hasClass("IconPlus")) {
        $('img', _obj).removeClass("IconPlus");
        $('img', _obj).addClass("IconMinus");

        if ($(".trGroup" + _approvalTag) != undefined) { $(".trGroup" + _approvalTag).show(); }
    }
    else if ($('img', _obj).hasClass("IconMinus")) {
        $('img', _obj).removeClass("IconMinus");
        $('img', _obj).addClass("IconPlus");

        if ($(".trGroup" + _approvalTag) != undefined) { $(".trGroup" + _approvalTag).hide(); }
    }

    GroupList.Resource();
}