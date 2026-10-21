DocUserSeq = {}

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

        DocUserSeq.LoadData();
    });
});

/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 리스트 초기화
***************************************************/
DocUserSeq.LoadData = function () {

    DocUserSeq.GetApprovalUserInfoList();
    DocUserSeq.GetApprovalUserInfoListCA();
    DocUserSeq.Resource();
}


/**************************************************
작성자   : 이문호
작성일자 : 2012-05-09
작성목적 : 다국어
***************************************************/
DocUserSeq.Resource = function () {

    $("#r_gubun")[0].innerHTML = R_Gubun;
    $("#r_tag")[0].innerHTML = R_Tag;
    $("#r_userInfo")[0].innerHTML = R_UserInfo;
    $("#r_date")[0].innerHTML = R_Date;

    $("#r_CarbonInfo")[0].innerHTML = R_CarbonInfo;
    $("#r_ViewDate")[0].innerHTML = R_ViewDate;

    if ($(".j_fileName") != undefined) { $(".j_fileName").text(R_FileName); }
    if ($(".j_fileType") != undefined) { $(".j_fileType").text(R_FileType); }
    if ($(".j_fileLen") != undefined) { $(".j_fileLen").text(R_FileLen); }
    if ($(".j_download") != undefined) { $(".j_download").text(R_FileDownload); }
    if ($(".j_docInfo") != undefined) { $(".j_docInfo").text(R_DocInfo); }
    if ($(".j_docView") != undefined) { $(".j_docView").text(R_DocView); }
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    if ($(".ZD0") != undefined) { $(".ZD0").text(R_ZD0); }
    if ($(".ZD") != undefined) { $(".ZD").text(R_ZD); }
    if ($(".ZH") != undefined) { $(".ZH").text(R_ZH); }
    if ($(".ZA") != undefined) { $(".ZA").text(R_ZA); }
    if ($(".ZR") != undefined) { $(".ZR").text(R_ZR); }
    if ($(".ApprovalTypeL") != undefined) { $(".ApprovalTypeL").text(R_ApprovalTypeL); }
    if ($(".ApprovalTypeP") != undefined) { $(".ApprovalTypeP").text(R_ApprovalTypeP); }

    if ($(".IconPlus") != undefined) { $(".IconPlus").attr('src', ImagePath + '/Icon/ico_item_plus.gif'); }
    if ($(".IconMinus") != undefined) { $(".IconMinus").attr('src', ImagePath + '/Icon/ico_item_minus.gif'); }
}


/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 결재자정보 리스트 호출
**********************************************************************************************/
DocUserSeq.GetApprovalUserInfoList = function () {
    ParamArray = {
        'childDocID': ChildDocID,
        'zd': R_Send, //발신
        'zpzq': R_Receive //수신
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("DocUserSeq.SuccessUserInfoList");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/DocUserSeq.asmx/GetApprovalUserInfoList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 결재자정보 리스트 보여주기
**********************************************************************************************/
DocUserSeq.SuccessUserInfoList = function (data, status) {

    $('#placeholder_Info').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTDocUserSeq.htm?seq=' + Guids());
    $('#placeholder_Info').processTemplate(data.d);

}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 참조자정보 리스트 호출
**********************************************************************************************/
DocUserSeq.GetApprovalUserInfoListCA = function () {
    ParamArray = {
        'documentID': DocumentID,
        'childDocID': ChildDocID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("DocUserSeq.SuccessUserInfoListCA");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/DocUserSeq.asmx/GetApprovalUserInfoListCA";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-06-08
작성목적 : 참조자정보 리스트 보여주기
**********************************************************************************************/
DocUserSeq.SuccessUserInfoListCA = function (data, status) {

    $('#placeholder_CA').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTDocUserSeqCA.htm?seq=' + Guids());
    $('#placeholder_CA').processTemplate(data.d);

}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 참조자 그룹 보여주기
***************************************************/
DocUserSeq.GroupView = function (_obj, _approvalTag) {

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

    DocUserSeq.Resource();
}