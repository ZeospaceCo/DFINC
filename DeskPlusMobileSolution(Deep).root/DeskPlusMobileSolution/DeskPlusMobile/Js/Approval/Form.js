Form = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-10
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 양식가져오기
    setTimeout("Form.getList();", 400);

    $('#listTitle').click(function (e) {
        e.preventDefault();
        location.href = "WFolder.aspx";
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 양식 리스트 가져오기 (웹서비스 호출)
***************************************************/
Form.getList = function () {
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetFormList";

    //파라미터값 배열에 담기
    ParamArray = {
        'FolderID': FolderID,
        'TypeInfo': TypeInfo
    };

    var obj = eval("Form.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 양식 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Form.succeeded = function (data, status) {

    $('#phForm').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTForm.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phForm').processTemplate(data.d);

    setTimeout(function () {
        Form.Resource();
    }, 100);

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-09
작성목적 : 다국어 처리
**********************************************************************************************/
Form.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-10
작성목적 : 상신 or 양식리스트 화면으로 가기 
***************************************************/
Form.GoList = function (folderID, formID, typeInfo) {

    if (formID == "0") {
        ParamArray = {
            'FolderID': folderID,
            'TypeInfo': typeInfo
        };

        CallEncrypt(CallGetParam());
        document.location.href = "Form.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'FolderID': folderID,
            'FormID': formID
        };

        CallEncrypt(CallGetParam());
        document.location.href = "Add.aspx?p=" + GetEncryptValue;
    }
}
