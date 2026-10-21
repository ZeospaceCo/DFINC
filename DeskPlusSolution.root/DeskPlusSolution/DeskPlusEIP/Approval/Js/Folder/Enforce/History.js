History = {}

$(function () {
    $(document).ready(function () {

        //취소
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        History.GetPageList("");
    });
});

/**********************************************************************************************
작성자   : 
작성일자 : 2012-05-31
작성목적 : 리스트 호출
**********************************************************************************************/
History.GetPageList = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Enforce/History.asmx/GetPageList";

    ParamArray = {
        'enforceID': EnforceID
    };

    var obj = eval("History.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 성공시 결재상황조회 상신한 문서 리스트 호출
**********************************************************************************************/
History.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Enforce/JTHistory.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    History.Resource();
}

History.Resource = function () {

    $("#r_user")[0].innerHTML = R_EnforceSendUser;
    $("#r_sendtype")[0].innerHTML = R_EnforceSendType;
    $("#r_writedate")[0].innerHTML = R_EnforceSendDate;
    
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}