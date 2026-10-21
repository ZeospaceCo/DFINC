Attach = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-27
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        setTimeout("Attach.Attach();", 300);

    });
});


/**************************************************
작성자 : famo23
작성일자 : 2012-01-27
작성목적 : 업로드 (웹서비스 호출)
***************************************************/
Attach.Attach = function () {

    ParamArray = {
        'folderID': FolderID,
        'tmpValue': TmpValue,
        'uID': UID,
        'guid': GUID
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Attach.SuccessAttach");
    var url = "/DeskPlusEIP/WebHard/Ws/Upload/Attach.asmx/AttachFile";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-27
작성목적 : 업로드 (웹서비스 값 받기)
**********************************************************************************************/
Attach.SuccessAttach = function (data, status) {
    if (data.d) {
        if (parent.disablePopup) {
            var obj = eval("parent." + callBackEvent);
            obj();
            parent.disablePopup();
        } else {
            var obj = eval("opener." + callBackEvent);
            obj();
            self.close();
        }
    }
    else {
        alert(R_ProcessingError);
    }    
}




