Send = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-01-20
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        setTimeout("Send.Send();", 300);    
         
    });
});


/**************************************************
작성자 : famo23
작성일자 : 2012-01-20
작성목적 : 전달
***************************************************/
Send.Send = function () {

    ParamArray = {
        'folderID': FolderID,
        'fileList': FileList,
        'uID': UID,      
        'order': Order
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Send.SuccessSend");
    var url = "/DeskPlusEIP/WebHard/Ws/Send/Send.asmx/SendFile";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-01-20
작성목적 : 파일 전달 (웹서비스 값 받기)
**********************************************************************************************/
Send.SuccessSend = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}




